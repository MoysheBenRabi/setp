using System;
using System.Collections.Generic;

using System.Text;
using MXP.Messages;
using System.Threading;
using MXP.Delegates;

namespace MXP
{

    /// <summary>
    /// MxpClient is application interface for participant to bubble client implementations. 
    /// See MXTank.TestParticipant for simple example client implementation.
    /// </summary>
    public class MxpClient
    {

        #region Events

        /// <summary>
        /// Connection to server succeeded.
        /// </summary>
        public ServerConnected ConnectionSuccess = delegate(JoinResponseMessage message) { };
        /// <summary>
        /// Connection to server failed.
        /// </summary>
        public ServerConnectFailure ConnectionFailure = delegate(JoinResponseMessage message) { };
        /// <summary>
        /// Message was received from server.
        /// </summary>
        public ServerMessageReceived ServerMessageReceived = delegate(Message message) { };
        /// <summary>
        /// Server was disconnected. This can be due to explicit disconnect request from either party
        /// or timeout of keepalive messages.
        /// </summary>
        public ServerDisconnected ServerDisconnected = delegate(Message message) { };
        
        #endregion

        #region Fields

        private Transmitter transmitter;
        private Session session;

        private String programName;
        private byte programMajorVersion;
        private byte programMinorVersion;

        private Guid participantId = Guid.Empty;
        private Guid avatarId = Guid.Empty;
        private Guid bubbleId = Guid.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Id of the participant is received from server when connection is established.
        /// Id should originate from the identity provider based on the participant name and passphrase.
        /// </summary>
        public Guid ParticipantId
        {
            get
            {
                return participantId;
            }
        }

        /// <summary>
        /// AvatarId is identifier of the avatar linked to participant.
        /// </summary>
        public Guid AvatarId
        {
            get
            {
                return avatarId;
            }
        }

        public Guid BubbleId
        {
            get
            {
                return bubbleId;
            }
        }

        /// <summary>
        /// Id of the session. 0 if client is not connected.
        /// </summary>
        public uint SessionId
        {
            get
            {
                return session != null ? session.IncomingSessionId : 0;
            }
        }
        /// <summary>
        /// Property reflecting whether client is connected to the server.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return session != null && session.SessionState == SessionState.Connected;
            }
        }
        /// <summary>
        /// Property reflecting whether client is currently trying to connect to server.
        /// </summary>
        public bool IsConnecting
        {
            get
            {
                return session != null && session.SessionState == SessionState.Connecting;
            }
        }
        /// <summary>
        /// State of the client session.
        /// </summary>
        public SessionState SessionState
        {
            get
            {
                return session!=null?session.SessionState:SessionState.Disconnected;
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
                return transmitter.BytesReceived;
            }
        }
        /// <summary>
        /// Bytes client has sent so far.
        /// </summary>
        public ulong BytesSent
        {
            get
            {
                return transmitter.BytesSent;
            }
        }
        /// <summary>
        /// Number of bytes received (bytes per second) during past second.
        /// </summary>
        public double ReceiveRate
        {
            get
            {
                return transmitter.ReceiveRate;
            }
        }
        /// <summary>
        /// Number of bytes sent (bytes per second) during past second.
        /// </summary>
        public double SendRate
        {
            get
            {
                return transmitter.SendRate;
            }
        }
        
        /// <summary>
        /// Send rate rate from client session with smaller time window than transmitter send rate.
        /// </summary>
        public double SessionSendRate
        {
            get
            {
                return session != null ? session.SendRate : 0;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs MxpClient with given program identification information.
        /// </summary>
        /// <param name="programName">Name of the program</param>
        /// <param name="programMajorVersion">Major version of the program</param>
        /// <param name="programMinorVersion">Minor version of the program</param>
        public MxpClient(String programName,byte programMajorVersion,byte programMinorVersion)
        {

            this.programName = programName;
            this.programMajorVersion = programMajorVersion;
            this.programMinorVersion = programMinorVersion;

        }
        
        #endregion
        
        #region Startup and Shutdown

        /// <summary>
        /// Starts up client
        /// </summary>
        private void Startup()
        {

            if (transmitter == null)
            {
                transmitter = new Transmitter();
                transmitter.Startup();
            }

        }

        /// <summary>
        /// Shuts down client
        /// </summary>
        private void Shutdown()
        {

            if (transmitter == null)
            {
                throw new Exception("Not started up yet.");
            }
            transmitter.Shutdown();
            transmitter = null;
            session = null;
            participantId = Guid.Empty;

        }

        #endregion

        #region Processing

        /// <summary>
        /// Executes client processing like message handling and session management
        /// </summary>
        public void Process()
        {

            int messagesProcessedCount = 0;
            while (session != null && session.AvailableMessages > 0)
            {
                Message message = session.Receive();

                if (message.GetType() == typeof(JoinResponseMessage))
                {
                    JoinResponseMessage joinResponseMessage = (JoinResponseMessage)message;
                    participantId = joinResponseMessage.ParticipantId;
                    avatarId = joinResponseMessage.AvatarId;
                    bubbleId = joinResponseMessage.BubbleId;
                    if (joinResponseMessage.FailureCode == 0)
                    {
                        session.SetStateConnected();
                        ConnectionSuccess(joinResponseMessage);
                    }
                    else
                    {
                        session.SetStateDisconnected();
                        ConnectionFailure(joinResponseMessage);
                    }
                }
                else if (message.GetType() == typeof(LeaveRequestMessage))
                {
                    LeaveResponseMessage leaveResponse = (LeaveResponseMessage)MessageFactory.Current.ReserveMessage(typeof(LeaveResponseMessage));
                    leaveResponse.RequestMessageId = leaveResponse.MessageId;
                    leaveResponse.FailureCode = MxpResponseCodes.SUCCESS;

                    session.Send(leaveResponse);

                    ServerDisconnected(message);

                    Shutdown();
                }
                else if (message.GetType() == typeof(LeaveResponseMessage))
                {
                    LeaveResponseMessage leaveResponseMessage = (LeaveResponseMessage)message;
                    if (leaveResponseMessage.FailureCode == 0)
                    {
                        ServerDisconnected(message);

                        Shutdown();
                    }
                }

                ServerMessageReceived(message);

                MessageFactory.Current.ReleaseMessage(message);

                messagesProcessedCount++;

                //if (messagesProcessedCount > 1000)
                if (messagesProcessedCount > 5 && session.AvailableMessages < 30)
                {
                    break;
                }
            }

            if (session != null && session.SessionState == SessionState.Disconnected)
            {
                ServerDisconnected(null);

                Shutdown();
            }

        }

        #endregion

        #region Connect and Disconnect

        /// <summary>
        /// Connects to a bubble with given address and identity information.
        /// </summary>
        /// <param name="hostname">Bubble server address</param>
        /// <param name="port">Bubble server port</param>
        /// <param name="bubbleId">Unique identifier of the bubble</param>
        /// <param name="bubbleName">Should be set to "" if bubbleId is not empty guid.</param>
        /// <param name="location">Name of the virtual location where participant is placed on connect</param>
        /// <param name="identityProviderUrl">Url of the identity provider. For example OpenId provider.</param>
        /// <param name="participantIdentifier">Participant name</param>
        /// <param name="participantSecret">Participant passphrase</param>
        public void Connect(String hostname, int port, Guid bubbleId, string bubbleName, string location, String identityProviderUrl, string participantIdentifier, string participantSecret, Guid avatarId, bool debugMessages)
        {
            Startup();

            JoinRequestMessage message = (JoinRequestMessage)MessageFactory.Current.ReserveMessage(typeof(JoinRequestMessage));
            session = transmitter.OpenSession(hostname, port, message);
            session.DebugMessages = debugMessages;

            message.BubbleId = bubbleId;
            message.BubbleName = bubbleName;
            message.LocationName = location;
            message.IdentityProviderUrl = identityProviderUrl;
            message.ParticipantIdentifier = participantIdentifier;
            message.ParticipantSecret = participantSecret;
            message.ParticipantRealTime = 0;
            message.AvatarId = avatarId;

            message.ProgramName = programName;
            message.ProgramMajorVersion = programMajorVersion;
            message.ProgramMinorVersion = programMinorVersion;
            message.ProtocolMajorVersion = MxpConstants.ProtocolMajorVersion;
            message.ProtocolMinorVersion = MxpConstants.ProtocolMinorVersion;
            message.ProtocolSourceRevision = MxpConstants.ProtocolSourceRevision;

            session.Send(message);

        }


        /// <summary>
        /// Disconnects client from server
        /// </summary>
        public void Disconnect()
        {

            if (IsConnected)
            {
                Message message = (LeaveRequestMessage)MessageFactory.Current.ReserveMessage(typeof(LeaveRequestMessage));
                session.Send(message);
            }

            Thread.Sleep(10);
            Process();

        }

        #endregion

        #region Message Send

        /// <summary>
        /// Sends a message to server.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public void Send(Message message)
        {

            if (IsConnected)
            {
                session.Send(message);
            }
            else
            {
                throw new Exception("Not connected.");
            }

        }

        #endregion

    }
}
