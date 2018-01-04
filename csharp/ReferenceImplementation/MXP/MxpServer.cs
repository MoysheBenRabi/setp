using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MXP.Messages;
using System.Diagnostics;
using MXP.Util;

namespace MXP
{

    /// <summary>
    /// MxpServer is application interface for participant to bubble server implementations. See MXTank.Tank class
    /// for example implementation.
    /// </summary>
    public class MxpServer
    {

        #region Fields

        private Transmitter transmitter;
        private IList<Session> sessions = new List<Session>();
        private IList<Session> sessionsToRemove = new List<Session>();

        private IDictionary<Guid,MxpBubble> Bubbles = new Dictionary<Guid,MxpBubble>();

        private String assetCacheUrl;
        private String programName;
        private byte programMajorVersion;
        private byte programMinorVersion;
        private int port;

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

        /// <summary>
        /// Constructs MxpServer with given cloud, connectivity and program parameters
        /// </summary>
        /// <param name="assetCacheUrl">Url to the cloud web server.</param>
        /// <param name="port">UDP port number the server listens for incoming participant connections.</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="programMajorVersion">Major version of the program</param>
        /// <param name="programMinorVersion">Minor version of the program</param>
        public MxpServer(
            String assetCacheUrl,
            int port,
            String programName, 
            byte programMajorVersion, 
            byte programMinorVersion
            )
        {

            this.assetCacheUrl = assetCacheUrl;
            this.programName = programName;
            this.programMajorVersion = programMajorVersion;
            this.programMinorVersion = programMinorVersion;
            this.port = port;
        }

        #endregion

        #region Startup and Shutdown

        public void Startup(bool debugMessages)
        {

            if (transmitter != null)
            {
                throw new Exception("Already listening.");
            }
            transmitter = new Transmitter(port);
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
                    Message message = MessageFactory.Current.ReserveMessage(typeof(LeaveRequestMessage));
                    session.Send(message);
                }
            }
        }

        #endregion

        #region Session Management
        
        public void Disconnect(Session session)
        {
            if (session.IsConnected)
            {
                Message message = MessageFactory.Current.ReserveMessage(typeof(LeaveRequestMessage));
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
                    session.Bubble.ParticipantDisconnected(session);
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
                while(session.AvailableMessages > 0)
                {

                    Message message = session.Receive();

                    if (message.GetType() == typeof(JoinRequestMessage))
                    {

                        JoinRequestMessage joinRequestMessage = (JoinRequestMessage)message;

                        if (Bubbles.ContainsKey(joinRequestMessage.BubbleId))
                        {
                            session.Bubble = Bubbles[joinRequestMessage.BubbleId];
                        }

                        Guid participantId=Guid.Empty;
                        Guid avatarId = Guid.Empty;
                        bool success = session.Bubble!=null&&session.Bubble.ParticipantConnectAuthorize(
                            session, joinRequestMessage, out participantId, out avatarId);

                        if (success)
                        {
                            //LogUtil.Info("Participant connect success: " + joinRequestMessage.ParticipantIdentifier + " (" + session.IncomingSessionId + " " + (session.IsIncoming ? "from" : "to") + " " + session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + ")");

                            JoinResponseMessage joinResponseMessage = (JoinResponseMessage)MessageFactory.Current.ReserveMessage(
                                typeof(JoinResponseMessage));

                            joinResponseMessage.RequestMessageId = joinRequestMessage.MessageId;
                            joinResponseMessage.FailureCode = 0;

                            joinResponseMessage.ParticipantId = participantId;
                            joinResponseMessage.AvatarId = avatarId;
                            joinResponseMessage.BubbleAssetCacheUrl = assetCacheUrl;
                            joinResponseMessage.BubbleId = session.Bubble.BubbleId;
                            joinResponseMessage.BubbleName = session.Bubble.BubbleName;
                            joinResponseMessage.BubbleRealTime = 0; 
                            joinResponseMessage.ProgramName = programName;
                            joinResponseMessage.ProgramMajorVersion = programMajorVersion;
                            joinResponseMessage.ProgramMinorVersion = programMinorVersion;
                            joinResponseMessage.ProtocolMajorVersion = MxpConstants.ProtocolMajorVersion;
                            joinResponseMessage.ProtocolMinorVersion = MxpConstants.ProtocolMinorVersion;
                            joinResponseMessage.ProtocolSourceRevision = MxpConstants.ProtocolSourceRevision;

                            session.Send(joinResponseMessage);

                            session.SetStateConnected();

                            session.Bubble.ParticipantConnected(session, joinRequestMessage, participantId, avatarId);
                        }
                        else
                        {
                            //LogUtil.Info("Participant connect failure failure: " + joinRequestMessage.ParticipantIdentifier + " (" + session.IncomingSessionId + " " + (session.IsIncoming ? "from" : "to") + " " + session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + ")");

                            JoinResponseMessage joinResponseMessage = (JoinResponseMessage)MessageFactory.Current.ReserveMessage(typeof(JoinResponseMessage));
                            
                            joinResponseMessage.RequestMessageId = joinRequestMessage.MessageId;
                            joinResponseMessage.FailureCode = 1;

                            joinResponseMessage.BubbleAssetCacheUrl = assetCacheUrl;
                            
                            if (session.Bubble != null)
                            {
                                joinResponseMessage.BubbleName = session.Bubble.BubbleName;
                                joinResponseMessage.BubbleId = session.Bubble.BubbleId;
                            }
                            else
                            {
                                joinResponseMessage.BubbleName = "";
                                joinResponseMessage.BubbleId = Guid.Empty;
                            }

                            joinResponseMessage.BubbleRealTime = 0;
                            joinResponseMessage.ProgramName = programName;
                            joinResponseMessage.ProgramMajorVersion = programMajorVersion;
                            joinResponseMessage.ProgramMinorVersion = programMinorVersion;
                            joinResponseMessage.ProtocolMajorVersion = MxpConstants.ProtocolMajorVersion;
                            joinResponseMessage.ProtocolMinorVersion = MxpConstants.ProtocolMinorVersion;
                            joinResponseMessage.ProtocolSourceRevision = MxpConstants.ProtocolSourceRevision;

                            session.Send(joinResponseMessage);

                            session.SetStateDisconnected();

                            if (session.Bubble != null)
                            {
                                session.Bubble.ParticipantConnectFailure(session, message);
                            }
                        }
                        
                    }
                    if (message.GetType() == typeof(LeaveRequestMessage))
                    {

                        LeaveResponseMessage leaveResponseMessage = (LeaveResponseMessage)MessageFactory.Current.ReserveMessage(
                            typeof(LeaveResponseMessage));

                        //LogUtil.Info("Session leave request: " + session.IncomingSessionId + " (" + (session.IsIncoming ? "from" : "to") + " " + session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + ")");

                        leaveResponseMessage.RequestMessageId = message.MessageId;
                        leaveResponseMessage.FailureCode = 0;
                        session.Send(leaveResponseMessage);

                        /*if (session.SessionState != SessionState.Disconnected)
                        {
                            session.SetStateDisconnected();
                        }*/

                    }
                    if (message.GetType() == typeof(LeaveResponseMessage))
                    {

                        LeaveResponseMessage leaveResponseMessage = (LeaveResponseMessage)message;

                        //LogUtil.Info("Session leave response: " + session.IncomingSessionId + " (" + (session.IsIncoming ? "from" : "to") + " " + session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + ")");

                        if (leaveResponseMessage.FailureCode == 0)
                        {
                            session.SetStateDisconnected();
                        }

                    }
                    else
                    {
                        if (session.Bubble != null)
                        {
                            session.Bubble.ParticipantMessageReceived(session, message);
                        }
                    }

                    MessageFactory.Current.ReleaseMessage(message);
                    messagesProcessedCount++;
                    if (messagesProcessedCount > 1000)
                    {
                        break;
                    }
                }

            }

        }

        #endregion

    }
}
