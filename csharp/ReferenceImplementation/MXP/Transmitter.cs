using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;
using MXP.Messages;
using MXP.Util;

namespace MXP
{
    /// <summary>
    /// Transmitter encodes/decodes messages to packets and transmits them using UDP sockets.
    /// Transmitter controls the transport layer using transport messages.
    /// </summary>
    public class Transmitter
    {

        # region Fields

        private UdpClient udpClient;

        private Thread networkThread = null;
        private Thread receiveThread = null;

        private bool requestThreadExit = false;

        private Queue<Session> incomingPendingSessions = new Queue<Session>();
        // Contains all sessions identified with outbound session id.
        private Dictionary<uint, Session> outIdSessionDictionary = new Dictionary<uint, Session>();
        // Contains sessions which have inbound session id defined. Outbound connecting sessions do not have inbound session id.
        private Dictionary<SessionKey, Session> inKeySessionDictionary = new Dictionary<SessionKey, Session>();
        // Thread lock for locking sessiong management operations on session collections.
        private Object sessionManagementLock = new Object();

        private List<Session> sessionsToRemove = new List<Session>();
        private List<Packet> packetsToRemove = new List<Packet>();

        private uint sessionIdCounter = 0;
        private uint packetIdCounter = 0;

        public ulong PacketsSent = 0;
        public ulong PacketsReceived = 0;
        
        public ulong BytesSent = 0;
        public ulong BytesReceived = 0;
        private ulong lastBytesSent = 0;
        private ulong lastBytesReceived = 0;

        private DateTime SendRateUpdateTime = DateTime.Now;
        private DateTime lastReceiveRateUpdateTime = DateTime.Now;

        public double SendRate = 0;
        public double ReceiveRate = 0;
        public double RateTimeWindow = 1;
        private double MaximumSessionSendRate = 250000; // Maximum bytes per send. This should only be raised by the throttling message send by remote peer.

        public bool DebugMessages = false;

        #endregion

        #region Properties

        public bool IsAlive
        {
            get
            {
                return networkThread != null && networkThread.IsAlive && receiveThread != null && receiveThread.IsAlive;
            }
        }

        /// <summary>
        /// If set then packet send time to zero on outgoing packets is always fixed to this value.
        /// Used by serialization verification to create deterministic packet content.
        /// </summary>
        public DateTime DefaultPacketFirstSentTime
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public Transmitter(int port)
        {
            DefaultPacketFirstSentTime = DateTime.MinValue;
            udpClient = new UdpClient(port);
        }

        public Transmitter()
        {
            DefaultPacketFirstSentTime = DateTime.MinValue;
            udpClient = new UdpClient();
        }

        #endregion

        #region Startup and Shutdown

        public void Startup()
        {
            if (udpClient == null)
            {
                throw new Exception("Restart after stop is not supported.");
            }

            if (networkThread != null)
            {
                throw new Exception("Network thread already executing.");
            }

            if (receiveThread != null)
            {
                throw new Exception("Receiver thread already executing.");
            }

            requestThreadExit = false;

            networkThread = new Thread(new ThreadStart(Process));
            networkThread.Name = MxpOptions.ThreadNamePrefix + "-network";
            networkThread.Start();
            
            receiveThread = new Thread(new ThreadStart(ProcessReceive));
            receiveThread.Name = MxpOptions.ThreadNamePrefix + "-network-receive";
            receiveThread.Priority = ThreadPriority.AboveNormal;
            
            receiveThread.Start();
        }

        public void Shutdown()
        {
            requestThreadExit=true;
            if (networkThread != null)
            {
                while (networkThread.IsAlive)
                {
                    Thread.Sleep(10);
                }
            }
            udpClient.Close();
            if (receiveThread != null)
            {
                while (receiveThread.IsAlive)
                {
                    receiveThread.Abort();
                    Thread.Sleep(10);
                }
            }


            networkThread = null;
            receiveThread = null;
            udpClient = null;

        }
        #endregion

        #region Processing


        /// <summary>
        /// Network thread main loop.
        /// </summary>
        public void Process()
        {
            TimeSpan sleepSpan = new TimeSpan(10000);
            try
            {
                while (!requestThreadExit)
                {
                    try
                    {
                        HandleControlMessages();
                        Send();
                        RemoveDisconnectedSessions();
                        // TODO Clean away timed out partly received messages. (Non quaranteed multipart messages can cause this.)
                        Thread.Sleep(sleepSpan);
                    }
                    catch (ThreadAbortException)
                    {
                        requestThreadExit = true;
                        udpClient.Close();
                        break;
                    }
                    catch (Exception e)
                    {
                        LogUtil.Info(e.ToString());
                    }
                }
            }
            finally
            {
                LogUtil.Info("Network thread exited.");
            }

        }

        public void Send()
        {
            ICollection<Session> sessionList = outIdSessionDictionary.Values;

            for (int i = 0; i < sessionList.Count; i++)
            {
                Session session = sessionList.ElementAt(i);

                // Send acks to the received messages.
                session.SendAcknowledgeMessages();

                // Resend non acknowledged packets and remove packets waiting for acknowledge which has already been sent maximum number of times.
                DateTime now = DateTime.Now;
                ICollection<Packet> packets = session.GetPacketsWaitingAcknowledge();
                for (int p = 0; p < packets.Count; p++)
                {
                    Packet packet = packets.ElementAt(p);
                    if (packet.ResendCount == MxpConstants.MaxResendCount)
                    {
                        packetsToRemove.Add(packet);
                    }

                    if (now.Subtract(packet.LastSendTime).TotalMilliseconds > MxpConstants.MaxAcknowledgeWaitTimeMilliSeconds)
                    {
                        packet.LastSendTime = now;
                        packet.ResendCount++;
                        udpClient.Send(packet.PacketBytes, packet.PacketLength, session.RemoteEndPoint);
                        LogUtil.Warn("Resending packet: "+packet.PacketId+" for "+packet.ResendCount+" time.");
                    }
                }

                for (int p = 0; p < packetsToRemove.Count; p++)
                {
                    Packet packet = packetsToRemove.ElementAt(p);
                    session.RemovePacketWaitingAcknowledge(packet.PacketId);
                }

                packetsToRemove.Clear();

                // Refreshing session send rate.
                double timeSinceSessionByteSentUpdate = DateTime.Now.Subtract(session.SendRateUpdateTime).TotalSeconds;
                if (timeSinceSessionByteSentUpdate > session.SendRateTimeWindow)
                {
                    session.SendRate = session.BytesSent / timeSinceSessionByteSentUpdate;

                    //session.LastBytesSent = session.BytesSent;
                    //session.LastSendRateUpdateTime = session.SendRateUpdateTime;

                    session.BytesSent = 0;
                    session.SendRateUpdateTime = DateTime.Now;
                }


                while (session.GetOutboundMessageCount() > 0 || session.GetPartiallySentMessageCount() > 0)
                {

                    if (session.BytesSent > MaximumSessionSendRate * session.SendRateTimeWindow)
                    {
                        LogUtil.Warn("Exceeding transmission limit. Dropping unquaranteed messages.");
                        // Lets not send more than the bandwidth limit allows for and drop some unquaranteed packets.
                        // For example 32 byte movement events
                        session.DropUnquaranteedOutboundMessages(10);
                        break;
                    }

                    // Prepare packet
                    Packet packet = PacketFactory.Current.ReservePacket();
                    lock (typeof(Session))
                    {
                        packetIdCounter++;
                        packet.PacketId = packetIdCounter;
                    }
                    if (session.FirstPacketId == 0)
                    {
                        session.FirstPacketId = packet.PacketId;
                    }
                    packet.SessionId = session.OutgoingSessionId;
                    if (DefaultPacketFirstSentTime!=DateTime.MinValue)
                    {
                        packet.FirstSendTime = (ulong)DefaultPacketFirstSentTime.Subtract(new DateTime(2000, 1, 1, 0, 0, 0)).TotalMilliseconds;
                    }
                    else
                    {
                        packet.FirstSendTime = (ulong)DateTime.Now.Subtract(new DateTime(2000, 1, 1, 0, 0, 0)).TotalMilliseconds;
                    }
                    packet.ResendCount = 0;
                    packet.Quaranteed = false;

                    // Encode packet
                    PacketEncoder.EncodePacketHeader(packet);

                    // Will contain the information whether one of the encoded frames were quaranteed.
                    bool oneOrMoreFramesQuaranteed = false;

                    PacketEncoder.EncodePacketData(session, packet, ref oneOrMoreFramesQuaranteed);

                    if (oneOrMoreFramesQuaranteed)
                    {
                        // set the quaranteed field to true in the packet header
                        packet.Quaranteed = oneOrMoreFramesQuaranteed;
                        EncodeUtil.Encode(ref packet.Quaranteed, packet.PacketBytes, 16);
                    }

                    // Send packet
                    packet.LastSendTime = now;
                    udpClient.Send(packet.PacketBytes, packet.PacketLength, session.RemoteEndPoint);

                    // Incrementing session send byte
                    session.BytesSent += packet.PacketLength;

                    // Updating transmitter send statistics
                    PacketsSent++;
                    BytesSent += (ulong)packet.PacketLength;

                    double timeSinceLastUpdate = now.Subtract(SendRateUpdateTime).TotalSeconds;
                    if (timeSinceLastUpdate > RateTimeWindow)
                    {
                        SendRate = (BytesSent - lastBytesSent) / timeSinceLastUpdate;
                        SendRateUpdateTime = now;
                        lastBytesSent = BytesSent;
                    }

                    // Releasing non quaranteed packet or adding quaranteed packet to wait acknowledge queue
                    if (packet.Quaranteed)
                    {
                        session.AddPacketWaitingAcknowledge(packet);
                    }
                    else
                    {
                        PacketFactory.Current.ReleasePacket(packet);
                    }

                }

            }
        }

        /// <summary>
        /// Receive thread main loop
        /// </summary>
        public void ProcessReceive()
        {
            try
            {
                while (!requestThreadExit)
                {
                    try
                    {
                        Receive();
                    }
                    catch (ThreadAbortException)
                    {
                        break;
                    }
                    catch (Exception e)
                    {
                        LogUtil.Error(e.ToString());
                    }
                }
            }
            finally
            {
                LogUtil.Info("Network receive thread exited.");
            }
        }

        public void Receive()
        {

            IPEndPoint remoteEndPoint = null;
            byte[] receivedBytes;
            try
            {
                receivedBytes = udpClient.Receive(ref remoteEndPoint);
            }
            catch (ThreadAbortException e)
            {
                throw e;
            }
            catch (ThreadInterruptedException)
            {
                return;
            }
            catch (SocketException)
            {
                return;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(10);
                return;
            }

            DateTime now = DateTime.Now;

            // Update receive statistics
            PacketsReceived++;
            BytesReceived += (ulong)receivedBytes.Length;

            double timeSinceLastUpdate = now.Subtract(lastReceiveRateUpdateTime).TotalSeconds;

            if (timeSinceLastUpdate > RateTimeWindow)
            {
                ReceiveRate = (BytesReceived - lastBytesReceived) / timeSinceLastUpdate;
                lastReceiveRateUpdateTime = now;
                lastBytesReceived = BytesReceived;
            }

            // Receiving udp packet.
            Packet packet = PacketFactory.Current.ReservePacket();
            receivedBytes.CopyTo(packet.PacketBytes, 0);
            packet.PacketLength = receivedBytes.Length;

            // Decoding packet header.
            PacketEncoder.DecodePacketHeader(packet);

            // Resolving session.
            Session session = GetSession(remoteEndPoint, packet.SessionId,packet);

            if (session == null)
            {
                // Packet belongs to old session.
                PacketFactory.Current.ReleasePacket(packet);
                return;
            }

            if(session.ReceivedPackets.Contains(packet.PacketId))
            {
                LogUtil.Warn("Packet has been already received: "+packet.PacketId);
                // Packet has been already received.
                session.AddAcknowledge(packet.PacketId);
                PacketFactory.Current.ReleasePacket(packet);
                return;                
            }

            session.ReceivedPackets.Add(packet.PacketId);

            // Will contain the information whether one of the encoded frames were quaranteed.
            bool oneOrMoreFramesQuaranteed = false;

            // Decoding packet data.
            PacketEncoder.DecodePacketData(session, packet, ref oneOrMoreFramesQuaranteed);

            if (packet.Quaranteed != oneOrMoreFramesQuaranteed)
            {
                throw new Exception("Packet and frames quaranteed state mismatch. Packet: " + oneOrMoreFramesQuaranteed + " Frames: " + oneOrMoreFramesQuaranteed);
            }

            // Add acknowledge to be sent for quaranteed packet
            if (packet.Quaranteed)
            {
                session.AddAcknowledge(packet.PacketId);
            }

            // Release packet back to recycling pool in the packet factory
            PacketFactory.Current.ReleasePacket(packet);

        }

        #endregion

        #region Session Management

        public Session OpenSession(String hostname, int port,Message connectRequestMessage)
        {
            return GetSession(new IPEndPoint(Dns.GetHostAddresses(hostname)[0], port), 0, null);
        }

        public int PendingSessionCount
        {
            get
            {
                return incomingPendingSessions.Count;
            }
        }

        public Session AcceptPendingSession()
        {
            lock (sessionManagementLock)
            {
                return incomingPendingSessions.Dequeue();
            }
        }

        /// <summary>
        /// Constructs new sessions or finds existing session.
        /// TODO: Simplify this method by dividing to two separate methods:
        /// One for construction of outbound session and another for constructing or finding session
        /// to match incoming packet.
        /// </summary>
        /// <param name="remoteEndPoint">Remote address and port where session is connected to.</param>
        /// <param name="incomingSessionId">Id of the incoming session.</param>
        /// <param name="incomingPacket">The incoming packet or null.</param>
        /// <returns>Session or null if error occurs.</returns>
        private Session GetSession(IPEndPoint remoteEndPoint, uint incomingSessionId, Packet incomingPacket)
        {
            SessionKey sessionKey = new SessionKey(remoteEndPoint, incomingSessionId);
            lock (sessionManagementLock)
            {
                if (!inKeySessionDictionary.ContainsKey(sessionKey))
                {
                    // Checking whether this incoming packet is connection request.
                    bool isConnectionRequest = false;
                    if (incomingPacket != null)
                    {
                        byte messageType = 0;
                        EncodeUtil.Decode(ref messageType, incomingPacket.PacketBytes, 18);
                        if (messageType == 10 || messageType == 30)
                        {
                            isConnectionRequest = true;
                        }
                    }

                    if (isConnectionRequest)
                    {
                        // Construct new incoming session
                        Session session = new Session();
                        session.IsIncoming = true;
                        session.RemoteEndPoint = remoteEndPoint;
                        session.IncomingSessionId = incomingSessionId;

                        sessionIdCounter++;
                        session.OutgoingSessionId = sessionIdCounter;

                        inKeySessionDictionary.Add(sessionKey, session);
                        outIdSessionDictionary.Add(session.OutgoingSessionId, session);
                        
                        incomingPendingSessions.Enqueue(session);

                        session.DebugMessages = DebugMessages;

                        LogUtil.Debug("Incoming session constructed: " + session.IncomingSessionId + " (" + (session.IsIncoming ? "from" : "to") + " " + session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + ")");

                        return session;
                    }
                    else
                    {
                        // This is either response to outgoing session request or new outgoing session request.
                        if (incomingPacket!=null)
                        {
                            // This is first response and needs to be bind to outgoing pending session.
                            
                            // Extracting manually message type which should be 1 i.e. ack.
                            byte messageType = 0;
                            EncodeUtil.Decode(ref messageType, incomingPacket.PacketBytes, 18);

                            if (messageType != 1)
                            {
                                LogUtil.Warn("Incoming packet could not be tied to session - incomingSessionId: " + incomingSessionId + " message type:" + messageType);
                                return null;
                            }

                            // Extracting manually connect request packet id from ack fragment.
                            uint ackedPacketId=0;
                            EncodeUtil.Decode(ref ackedPacketId,incomingPacket.PacketBytes,28);

                            Session session = null;
                            foreach (Session sessionCandidate in outIdSessionDictionary.Values)
                            {
                                if (sessionCandidate.FirstPacketId == ackedPacketId && sessionCandidate.SessionState == SessionState.Connecting)
                                {
                                    session = sessionCandidate;
                                }
                            }

                            if (session!=null)
                            {
                                session.IncomingSessionId = incomingSessionId;
                                inKeySessionDictionary.Add(sessionKey, session);
                                LogUtil.Debug("Session id received from peer: " + session.IncomingSessionId + " (" + (session.IsIncoming ? "from" : "to") + " " + session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + ")");
                                return session;
                            }
                            else
                            {
                                LogUtil.Warn("Incoming ack packet could not be tied to session. - incomingSessionId: " + incomingSessionId + " message type:" + messageType);
                                return null;
                            }
                        }
                        else
                        {
                            // Construct new outgoing session
                            Session session = new Session();
                            session.IsIncoming = false;
                            session.RemoteEndPoint = remoteEndPoint;
                            sessionIdCounter++;
                            session.OutgoingSessionId = sessionIdCounter;
                            outIdSessionDictionary.Add(session.OutgoingSessionId,session);

                            session.DebugMessages = DebugMessages;
                            LogUtil.Debug("Outgoing session constructed: " + session.IncomingSessionId + " (" + (session.IsIncoming ? "from" : "to") + " " + session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + ")");

                            return session;
                        }
                    }
                }
                else
                {
                    return inKeySessionDictionary[sessionKey];
                }
            }
        }

        public void RemoveDisconnectedSessions()
        {

            lock (sessionManagementLock)
            {
                ICollection<Session> sessionList = outIdSessionDictionary.Values;

                for (int i = 0; i < sessionList.Count; i++)
                {
                    Session session = sessionList.ElementAt(i);
                    if (session.SessionState == SessionState.Connecting &&
                        DateTime.Now.Subtract(session.CreationTime).TotalSeconds > MxpConstants.TimeOutSeconds)
                    {
                        session.SetStateDisconnected();
                        LogUtil.Info("Session timed out: " + session.IncomingSessionId + " (" + (session.IsIncoming ? "from" : "to") + " " + session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + ")");
                    }

                    if (session.SessionState == SessionState.Disconnected)
                    {
                        sessionsToRemove.Add(session);
                        LogUtil.Info("Session destructed: " + session.IncomingSessionId + " (" + (session.IsIncoming ? "from" : "to") + " " + session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + ")");
                    }
                }

                for (int i = 0; i < sessionsToRemove.Count; i++)
                {
                    Session session = sessionsToRemove.ElementAt(i);
                    inKeySessionDictionary.Remove(new SessionKey(session.RemoteEndPoint,session.IncomingSessionId));
                    outIdSessionDictionary.Remove(session.OutgoingSessionId);
                }

                sessionsToRemove.Clear();

            }
        }

        #endregion

        #region Control Message Handling

        public void HandleControlMessages()
        {
            lock (sessionManagementLock)
            {
                ICollection<Session> sessionList = outIdSessionDictionary.Values;

                for (int i = 0; i < sessionList.Count; i++)
                {
                    Session session = sessionList.ElementAt(i);

                    DateTime now = DateTime.Now;

                    if (session.IsConnected&&now.Subtract(session.LastSendTime).TotalSeconds > MxpConstants.MaxIdleTimeSecods)
                    {
                        KeepaliveMessage message = (KeepaliveMessage)MessageFactory.Current.ReserveMessage(typeof(KeepaliveMessage));
                        session.Send(message);
                        //MessageFactory.Current.ReleaseMessage(message);
                    }

                    if (now.Subtract(session.LastReceiveTime).TotalSeconds > (double)MxpConstants.TimeOutSeconds)
                    {
                        session.SetStateDisconnected();
                    }

                    if (session.AvailableControlMessages > 0)
                    {
                        Message message = session.PopControlMessage();
                        if (message.GetType() == typeof(AcknowledgeMessage))
                        {
                            AcknowledgeMessage acknowledgeMessage = (AcknowledgeMessage)message;
                            for (int p = 0; p < acknowledgeMessage.PacketIdCount; p++)
                            {
                                session.RemovePacketWaitingAcknowledge(acknowledgeMessage.GetPacketId(p));
                            }
                        }

                        MessageFactory.Current.ReleaseMessage(message);
                    }

                }
            }
        }

        #endregion
    
    }
}
