using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MXP.Messages;

namespace MXP
{

    /// <summary>
    /// MxpHub is application interface for bubble to bubble network hub.
    /// </summary>
    public class MxpHub
    {

        #region Fields
        
        private Transmitter transmitter;
        private IList<Session> sessions = new List<Session>();
        private IList<Session> sessionsToRemove = new List<Session>();

        private IDictionary<Guid,MxpBubble> Bubbles = new Dictionary<Guid,MxpBubble>();

        public String HubAssetCacheUrl;
        public String ProgramName;
        public byte ProgramMajorVersion;
        public byte ProgramMinorVersion;
        public String HubAddress;
        public int HubPort;

        #endregion

        #region Properties

        /// <summary>
        /// Number of sessions pending. (Process() accepts pending sessions).
        /// </summary>
        public int PendingSessionCount
        {
            get
            {
                return transmitter.PendingSessionCount;
            }
        }
        /// <summary>
        /// Number of connected sessions.
        /// </summary>
        public int SessionCount
        {
            get
            {
                return sessions.Count;
            }
        }
        /// <summary>
        /// Property reflecting whether client transmitter threads are alive.
        /// </summary>
        public bool IsTransmitterAlive
        {
            get
            {
                return transmitter != null && transmitter.IsAlive;
            }
        }
        /// <summary>
        /// Number of packets sent.
        /// </summary>
        public ulong PacketsSent
        {
            get
            {
                return transmitter != null ? transmitter.PacketsSent : 0;
            }
        }
        /// <summary>
        /// Number of packets received.
        /// </summary>
        public ulong PacketsReceived
        {
            get
            {
                return transmitter != null ? transmitter.PacketsReceived : 0;
            }
        }
        /// <summary>
        /// Bytes client has received so far.
        /// </summary>
        public ulong BytesReceived
        {
            get
            {
                return transmitter != null ? transmitter.BytesReceived : 0;
            }
        }
        /// <summary>
        /// Bytes client has sent so far.
        /// </summary>
        public ulong BytesSent
        {
            get
            {
                return transmitter != null ? transmitter.BytesSent : 0;
            }
        }
        /// <summary>
        /// Number of bytes received (bytes per second) during past second.
        /// </summary>
        public double ReceiveRate
        {
            get
            {
                return transmitter != null ? transmitter.ReceiveRate : 0;
            }
        }
        /// <summary>
        /// Number of bytes sent (bytes per second) during past second.
        /// </summary>
        public double SendRate
        {
            get
            {
                return transmitter != null ? transmitter.SendRate : 0;
            }
        }

        #endregion

        #region Constructors

        public MxpHub(
            String bubbleAssetCacheUrl,  
            String serverAddress,
            int port,
            String programName, 
            byte programMajorVersion, 
            byte programMinorVersion
            )
        {

            this.HubAssetCacheUrl = bubbleAssetCacheUrl;
            this.ProgramName = programName;

            this.ProgramMajorVersion = programMajorVersion;
            this.ProgramMinorVersion = programMinorVersion;
            this.HubAddress = serverAddress;
            this.HubPort = port;

        }

        #endregion

        #region Startup and Shutdown

        public void Startup(bool debugMessages)
        {

            if (transmitter != null)
            {
                throw new Exception("Already listening.");
            }
            transmitter = new Transmitter((int)HubPort);
            transmitter.DebugMessages = debugMessages;
            transmitter.Startup();

        }

        public void Shutdown()
        {

            if (transmitter == null)
            {
                throw new Exception("Not started up yet.");
            }
            transmitter.Shutdown();
            transmitter = null;

        }

        #endregion

        #region Bubble Management

        public void AddBubble(MxpBubble bubble)
        {
            Bubbles.Add(bubble.BubbleId, bubble);
        }

        public void RemoveBubble(MxpBubble bubble)
        {
            Bubbles.Remove(bubble.BubbleId);

            foreach (Session session in sessions)
            {
                if (session.Bubble == bubble)
                {
                    Message message = (LeaveRequestMessage)MessageFactory.Current.ReserveMessage(typeof(LeaveRequestMessage));
                    session.Send(message);
                    //MessageFactory.Current.ReleaseMessage(message);
                }
            }
        }

        #endregion

        #region Session Management

        public void Connect(Guid sourceBubbleId,Guid targetBubbleId,String targetHubAddress, int targetHubPort, float targetBubbleX, float targetBubbleY, float targetBubbleZ)
        {

            AttachRequestMessage message = (AttachRequestMessage)MessageFactory.Current.ReserveMessage(typeof(AttachRequestMessage));

            Session session = transmitter.OpenSession(targetHubAddress, targetHubPort, message);

            message.TargetBubbleId = targetBubbleId;

            if (!Bubbles.ContainsKey(sourceBubbleId))
            {
                throw new Exception("No such local bubble: "+sourceBubbleId);
            }

            session.Bubble = Bubbles[sourceBubbleId];

            message.SourceBubbleFragment.BubbleId = session.Bubble.BubbleId;
            message.SourceBubbleFragment.BubbleName = session.Bubble.BubbleName;
            message.SourceBubbleFragment.BubbleAssetCacheUrl = HubAssetCacheUrl;
            message.SourceBubbleFragment.BubbleAddress = HubAddress;
            message.SourceBubbleFragment.BubblePort = (uint) HubPort;
            message.SourceBubbleFragment.BubbleCenter.X = -targetBubbleX;
            message.SourceBubbleFragment.BubbleCenter.Y = -targetBubbleY;
            message.SourceBubbleFragment.BubbleCenter.Z = -targetBubbleZ;
            message.SourceBubbleFragment.BubbleRange = session.Bubble.BubbleRange;
            message.SourceBubbleFragment.BubblePerceptionRange = session.Bubble.BubblePerceptionRange;
            message.SourceBubbleFragment.BubbleRealTime = 0;

            message.ProgramName = ProgramName;
            message.ProgramMajorVersion = ProgramMajorVersion;
            message.ProgramMinorVersion = ProgramMinorVersion;
            message.ProtocolMajorVersion = MxpConstants.ProtocolMajorVersion;
            message.ProtocolMinorVersion = MxpConstants.ProtocolMinorVersion;
            message.ProtocolSourceRevision = MxpConstants.ProtocolSourceRevision;

            session.Send(message);
            //MessageFactory.Current.ReleaseMessage(message);

            sessions.Add(session);

        }

        public void Disconnect(Session session)
        {
            if (session.IsConnected)
            {
                Message message = (DetachRequestMessage)MessageFactory.Current.ReserveMessage(typeof(DetachRequestMessage));
                session.Send(message);
                //MessageFactory.Current.ReleaseMessage(message);
            }
            else
            {
                throw new Exception("Not connected.");
            }
        }

        #endregion

        #region Processing
        
        public void Process()
        {
            ProcessMessages();
            Clean();
        }

        public void Clean()
        {

            for (int i = 0; i < sessions.Count; i++)
            {
                Session session = sessions.ElementAt(i);
                
                if (session.SessionState == SessionState.Disconnected)
                {
                    sessionsToRemove.Add(session);
                }
            }

            for (int i = 0; i < sessionsToRemove.Count; i++)
            {
                Session session = sessionsToRemove.ElementAt(i);
                sessions.Remove(session);
                if (session.Bubble != null)
                {
                    session.Bubble.BubbleDisconnected(session);
                }
            }

            sessionsToRemove.Clear();

        }

        public void ProcessMessages()
        {

            if (transmitter.PendingSessionCount > 0)
            {
                sessions.Add(transmitter.AcceptPendingSession());
            }
            
            for (int i = 0; i < sessions.Count; i++)
            {
                int messagesProcessedCount = 0;
                Session session = sessions[i];
                while (session.AvailableMessages > 0)
                {

                    Message message = session.Receive();

                    if (message.GetType() == typeof(AttachRequestMessage))
                    {

                        AttachRequestMessage attachRequestMessage = (AttachRequestMessage)message;

                        if (Bubbles.ContainsKey(attachRequestMessage.TargetBubbleId))
                        {
                            session.Bubble = Bubbles[attachRequestMessage.TargetBubbleId];
                        }

                        bool success = session.Bubble!=null&&session.Bubble.BubbleConnectAuthorize(
                            session, attachRequestMessage);

                        if (success)
                        {
                            AttachResponseMessage attachResponseMessage = (AttachResponseMessage)MessageFactory.Current.ReserveMessage(
                                typeof(AttachResponseMessage));

                            attachResponseMessage.RequestMessageId = attachRequestMessage.MessageId;
                            attachResponseMessage.FailureCode = 0;

                            attachResponseMessage.TargetBubbleFragment.BubbleId = session.Bubble.BubbleId;
                            attachResponseMessage.TargetBubbleFragment.BubbleName = session.Bubble.BubbleName;
                            attachResponseMessage.TargetBubbleFragment.BubbleAssetCacheUrl = HubAssetCacheUrl;
                            attachResponseMessage.TargetBubbleFragment.BubbleAddress = HubAddress;
                            attachResponseMessage.TargetBubbleFragment.BubblePort = (uint)HubPort;
                            attachResponseMessage.TargetBubbleFragment.BubbleCenter.X = -attachRequestMessage.SourceBubbleFragment.BubbleCenter.X;
                            attachResponseMessage.TargetBubbleFragment.BubbleCenter.Y = -attachRequestMessage.SourceBubbleFragment.BubbleCenter.Y;
                            attachResponseMessage.TargetBubbleFragment.BubbleCenter.Z = -attachRequestMessage.SourceBubbleFragment.BubbleCenter.Z;
                            attachResponseMessage.TargetBubbleFragment.BubbleRange = session.Bubble.BubbleRange;
                            attachResponseMessage.TargetBubbleFragment.BubblePerceptionRange = session.Bubble.BubblePerceptionRange;
                            attachResponseMessage.TargetBubbleFragment.BubbleRealTime = 0; // Calculate real time here.

                            attachResponseMessage.ProgramName = ProgramName;
                            attachResponseMessage.ProgramMajorVersion = ProgramMajorVersion;
                            attachResponseMessage.ProgramMinorVersion = ProgramMinorVersion;
                            attachResponseMessage.ProtocolMajorVersion = MxpConstants.ProtocolMajorVersion;
                            attachResponseMessage.ProtocolMinorVersion = MxpConstants.ProtocolMinorVersion;
                            attachResponseMessage.ProtocolSourceRevision = MxpConstants.ProtocolSourceRevision;

                            session.Send(attachResponseMessage);
                            //MessageFactory.Current.ReleaseMessage(attachResponseMessage);

                            session.SetStateConnected();

                            session.Bubble.BubbleConnected(session, message);
                        }
                        else
                        {
                            AttachResponseMessage attachResponseMessage = (AttachResponseMessage)MessageFactory.Current.ReserveMessage(typeof(AttachResponseMessage));
                            
                            attachResponseMessage.RequestMessageId = attachRequestMessage.MessageId;
                            attachResponseMessage.FailureCode = 1;


                            if (session.Bubble != null)
                            {
                                attachResponseMessage.TargetBubbleFragment.BubbleId = session.Bubble.BubbleId;
                                attachResponseMessage.TargetBubbleFragment.BubbleName = session.Bubble.BubbleName;
                                attachResponseMessage.TargetBubbleFragment.BubbleAssetCacheUrl = HubAssetCacheUrl;
                                attachResponseMessage.TargetBubbleFragment.BubbleAddress = HubAddress;
                                attachResponseMessage.TargetBubbleFragment.BubblePort = (uint) HubPort;
                                attachResponseMessage.TargetBubbleFragment.BubbleCenter.X = -attachRequestMessage.SourceBubbleFragment.BubbleCenter.X;
                                attachResponseMessage.TargetBubbleFragment.BubbleCenter.Y = -attachRequestMessage.SourceBubbleFragment.BubbleCenter.Y;
                                attachResponseMessage.TargetBubbleFragment.BubbleCenter.Z = -attachRequestMessage.SourceBubbleFragment.BubbleCenter.Z;
                                attachResponseMessage.TargetBubbleFragment.BubbleRange = session.Bubble.BubbleRange;
                                attachResponseMessage.TargetBubbleFragment.BubblePerceptionRange = session.Bubble.BubblePerceptionRange;
                                attachResponseMessage.TargetBubbleFragment.BubbleRealTime = 0; // Calculate real time here.
                            }
                            else
                            {
                                attachResponseMessage.TargetBubbleFragment.BubbleId = Guid.Empty;
                                attachResponseMessage.TargetBubbleFragment.BubbleName = "";
                                attachResponseMessage.TargetBubbleFragment.BubbleAssetCacheUrl = "";
                                attachResponseMessage.TargetBubbleFragment.BubbleAddress = "";
                                attachResponseMessage.TargetBubbleFragment.BubblePort = 0;
                                attachResponseMessage.TargetBubbleFragment.BubbleCenter.X = 0;
                                attachResponseMessage.TargetBubbleFragment.BubbleCenter.Y = 0;
                                attachResponseMessage.TargetBubbleFragment.BubbleCenter.Z = 0;
                                attachResponseMessage.TargetBubbleFragment.BubbleRange = 0;
                                attachResponseMessage.TargetBubbleFragment.BubblePerceptionRange = 0;
                                attachResponseMessage.TargetBubbleFragment.BubbleRealTime = 0;
                            }

                            attachResponseMessage.ProgramName = ProgramName;
                            attachResponseMessage.ProgramMajorVersion = ProgramMajorVersion;
                            attachResponseMessage.ProgramMinorVersion = ProgramMinorVersion;
                            attachResponseMessage.ProtocolMajorVersion = MxpConstants.ProtocolMajorVersion;
                            attachResponseMessage.ProtocolMinorVersion = MxpConstants.ProtocolMinorVersion;
                            attachResponseMessage.ProtocolSourceRevision = MxpConstants.ProtocolSourceRevision;
                            session.Send(attachResponseMessage);
                            //MessageFactory.Current.ReleaseMessage(attachResponseMessage);

                            session.SetStateDisconnected();

                            if (session.Bubble != null)
                            {
                                session.Bubble.BubbleConnectFailure(session, message);
                            }
                        }
                        
                    }
                    if (message.GetType() == typeof(AttachResponseMessage))
                    {
                        AttachResponseMessage attachResponseMessage = (AttachResponseMessage)message;
                        if (session.Bubble != null)
                        {
                            if (attachResponseMessage.FailureCode == 0)
                            {
                                session.SetStateConnected();
                                session.Bubble.BubbleConnected(session, message);
                            }
                            else
                            {
                                session.SetStateDisconnected();
                                session.Bubble.BubbleConnectFailure(session, message);
                            }
                        }
                    } 
                    if (message.GetType() == typeof(DetachRequestMessage))
                    {

                        DetachResponseMessage detachResponseMessage = (DetachResponseMessage)MessageFactory.Current.ReserveMessage(
                            typeof(DetachResponseMessage));

                        detachResponseMessage.RequestMessageId = message.MessageId;
                        detachResponseMessage.FailureCode = 0;
                        session.Send(detachResponseMessage);
                        //MessageFactory.Current.ReleaseMessage(detachResponseMessage);

                        //session.SetStateDisconnected();

                    }
                    if (message.GetType() == typeof(DetachResponseMessage))
                    {

                        DetachResponseMessage leaveResponseMessage = (DetachResponseMessage)message;

                        if (leaveResponseMessage.FailureCode == 0)
                        {
                            if (session.SessionState == SessionState.Connected || session.SessionState == SessionState.Connecting)
                            {
                                session.SetStateDisconnected();
                            }
                        }

                    }
                    if (message.GetType() == typeof(ListBubblesRequest))
                    {
                        if (session.Bubble != null)
                        {
                            session.Bubble.BubbleListRequested(session, (ListBubblesRequest) message);
                        }
                    }
                    if (message.GetType() == typeof(ListBubblesResponse))
                    {
                        if (session.Bubble != null)
                        {
                            session.Bubble.BubbleListReceived(session, (ListBubblesResponse)message);
                        }
                    }
                    else
                    {
                        if (session.Bubble != null)
                        {
                            session.Bubble.BubbleMessageReceived(session, message);
                        }
                    }

                    MessageFactory.Current.ReleaseMessage(message);

                    messagesProcessedCount++;
                    if (messagesProcessedCount > 5000)
                    {
                        break;
                    }
                }

            }

        }

        #endregion

    }
}
