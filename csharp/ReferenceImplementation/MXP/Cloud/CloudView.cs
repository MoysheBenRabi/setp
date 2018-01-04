using System;
using System.Collections.Generic;
using System.Text;
using MXP.Messages;
using MXP.Delegates;
using System.Threading;

namespace MXP.Cloud
{

    /// <summary>
    /// CloudView is reference implementation of the umbrella MXP view at client side.
    /// CloudView upkeeps object cache.
    /// </summary>
    public class CloudView
    {
        #region Fields
        private MxpClient client;
        private CloudCache cloudCache;
        private float viewRange;
        private string programName;
        private byte programMajorVersion;
        private byte programMinorVersion;
        private String hostname;
        private int port;
        private Guid bubbleId;
        private string bubbleName;
        private string location;
        private String identityProviderUrl;
        private string participantIdentifier;
        private string participantSecret;
        public ServerAction ServerAction=delegate (ActionEventMessage actionEvent) {};
        public ServerInteractRequest ServerInteractRequest = delegate(InteractRequestMessage interactRequest) { };
        public ServerInteractResponse ServerInteractResponse = delegate(InteractResponseMessage interactResponse) { };
        public ServerSynchronizationBegin ServerSynchronizationBegin = delegate(SynchronizationBeginEventMessage synchronizationBeginEvent) { };
        public ServerSynchronizationEnd ServerSynchronizationEnd = delegate(SynchronizationEndEventMessage synchronizationEndEvent) { };
        public ServerBubbleListReceived LinkedBubbleListReceived = delegate(ListBubblesResponse message) { };
        public ServerObjectHandover ServerObjectHandover = delegate(HandoverEventMessage message) { };
        #endregion

        #region Properties
        public MxpClient Client
        {
            get
            {
                return client;
            }
        }
        public CloudCache CloudCache
        {
            get
            {
                return cloudCache;
            }
        }
        public float ViewRange
        {
            get
            {
                return viewRange;
            }
        }
        public bool IsConnected
        {
            get
            {
                return client.IsConnected;
            }
        }
        public bool IsConnecting
        {
            get
            {
                return client.IsConnecting;
            }
        }
        public Guid ParticipantId
        {
            get
            {
                return client.ParticipantId;
            }
        }
        public Guid AvatarId
        {
            get
            {
                return client.AvatarId;
            }
        }
        public SessionState SessionState
        {
            get
            {
                return client.SessionState;
            }
        }

        public string ProgramName
        {
            get { return programName; }
        }

        public byte ProgramMajorVersion
        {
            get { return programMajorVersion; }
        }

        public byte ProgramMinorVersion
        {
            get { return programMinorVersion; }
        }

        public string Hostname
        {
            get { return hostname; }
        }

        public int Port
        {
            get { return port; }
        }

        public Guid BubbleId
        {
            get { return bubbleId; }
        }

        public string BubbleName
        {
            get { return bubbleName; }
        }

        public string Location
        {
            get { return location; }
        }

        public string IdentityProviderUrl
        {
            get { return identityProviderUrl; }
        }

        public string ParticipantIdentifier
        {
            get { return participantIdentifier; }
        }

        public string ParticipantSecret
        {
            get { return participantSecret; }
        }

        public bool IsTransmitterAlive
        {
            get { return Client.IsTransmitterAlive; }
        }

        #endregion

        #region Constructors
        public CloudView(float viewRange,string programName, byte programMajorVersion, byte programMinorVersion)
        {
            client=new MxpClient(programName,programMajorVersion,programMinorVersion);
            cloudCache = new CloudCache(Guid.Empty,viewRange,new TimeSpan(0,0,10,0));
            this.viewRange = viewRange;
            this.programName = programName;
            this.programMajorVersion = programMajorVersion;
            this.programMinorVersion = programMinorVersion;
            client.ServerMessageReceived=OnServerMessageReceived;
        }
        #endregion

        #region Connect and Disconnect
        public void Connect(String hostname, int port, Guid bubbleId, string bubbleName, string location, String identityProviderUrl, string participantIdentifier, string participantSecret, Guid avatarId, bool debugMessages)
        {
            if(client.IsConnected||client.IsConnecting)
            {
                throw new Exception("Client is already connecting or connected.");
            }
            this.hostname = hostname;
            this.port = port;
            this.bubbleId = bubbleId;
            this.bubbleName = bubbleName;
            this.location = location;
            this.identityProviderUrl = identityProviderUrl;
            this.participantIdentifier = participantIdentifier;
            this.participantSecret = participantSecret;
            client.Connect(hostname, port, bubbleId, bubbleName, location, identityProviderUrl, participantIdentifier, participantSecret, avatarId, debugMessages);    
        }

        public void Disconnect()
        {
            client.Disconnect();
            client.Process();
            Thread.Sleep(10);
            client.Process();
            Thread.Sleep(10);
            client.Process();
        }

        #endregion

        #region Processing
        public void Process()
        {
            cloudCache.Process();
            client.Process();
        }
        #endregion
    
        #region Server Message Handlers

        public void OnServerMessageReceived(Message message)
        {
            if(message.GetType()==typeof(PerceptionEventMessage))
            {
                PerceptionEventMessage perception = (PerceptionEventMessage) message;
                CloudObject cloudObject = cloudCache.GetObject(perception.ObjectFragment.ObjectId);
                if(cloudObject==null)
                {
                    cloudObject=new CloudObject();
                }
                cloudObject.FromObjectFragment(client.BubbleId,perception.ObjectFragment);
                cloudCache.PutObject(cloudObject,true);
            }
            if (message.GetType() == typeof(MovementEventMessage))
            {
                MovementEventMessage movement = (MovementEventMessage)message;
                CloudObject cloudObject = cloudCache.GetObject(client.BubbleId, movement.ObjectIndex);
                if (cloudObject != null)
                {
                    cloudObject.Location.X = movement.Location.X;
                    cloudObject.Location.Y = movement.Location.Y;
                    cloudObject.Location.Z = movement.Location.Z;
                    cloudObject.Orientation.X = movement.Orientation.X;
                    cloudObject.Orientation.Y = movement.Orientation.Y;
                    cloudObject.Orientation.Z = movement.Orientation.Z;
                    cloudObject.Orientation.W = movement.Orientation.W;
                    cloudCache.PutObject(cloudObject, true);
                }
            }
            if (message.GetType() == typeof(DisappearanceEventMessage))
            {
                DisappearanceEventMessage disappearance = (DisappearanceEventMessage) message;
                CloudObject cloudObject = cloudCache.GetObject(client.BubbleId, disappearance.ObjectIndex);
                if (cloudObject != null)
                {
                    cloudCache.RemoveObject(cloudObject.ObjectId);
                }
            }
            if(message.GetType() == typeof(ActionEventMessage))
            {
                ServerAction((ActionEventMessage) message);
            }
            if (message.GetType() == typeof(SynchronizationBeginEventMessage))
            {
                ServerSynchronizationBegin((SynchronizationBeginEventMessage)message);
            }
            if (message.GetType() == typeof(SynchronizationEndEventMessage))
            {
                ServerSynchronizationEnd((SynchronizationEndEventMessage)message);
            }
            if (message.GetType() == typeof(ListBubblesResponse))
            {
                LinkedBubbleListReceived((ListBubblesResponse)message);
            }
            if(message.GetType() == typeof(HandoverEventMessage))
            {
                ServerObjectHandover((HandoverEventMessage)message);
            }
            if (message.GetType() == typeof(InteractRequestMessage))
            {
                ServerInteractRequest((InteractRequestMessage)message);
            }
            if (message.GetType() == typeof(InteractResponseMessage))
            {
                ServerInteractResponse((InteractResponseMessage)message);
            }
        }

        #endregion

        #region Object Management

        /// <summary>
        /// Requests linked bubble list from connected bubble.
        /// </summary>
        public void RequestLinkedBubbleList()
        {
            ListBubblesRequest listBubblesRequest = new ListBubblesRequest();
            listBubblesRequest.ListType = ListBubblesRequest.ListTypeLinked;
            client.Send(listBubblesRequest);
        }

        /// <summary>
        /// Injecting messages instead of cache objects as cache objects are not meant to
        /// be modified locally but only as response to messages from cloud.
        /// </summary>
        /// <param name="injectRequest"></param>
        public void InjectObject(InjectRequestMessage injectRequest)
        {
            // need to insert this to reserve object index for mxp
            //CloudObject cloudObject=new CloudObject();
            //cloudObject.FromObjectFragment(client.BubbleId,injectRequest.ObjectFragment);
            //cloudCache.PutObject(cloudObject,false);
            // Ensure object index is 0 so bubble will assign its local object index to remote object index.
            injectRequest.ObjectFragment.ObjectIndex = 0;
            client.Send(injectRequest);
        }

        /// <summary>
        /// Modifying messages instead of cache objects as cache objects are not meant to
        /// be modified locally but only as response to messages from cloud.
        /// </summary>
        /// <param name="modifyRequest"></param>
        public void ModifyObject(ModifyRequestMessage modifyRequest)
        {
            CloudObject cloudObject = cloudCache.GetObject(modifyRequest.ObjectFragment.ObjectId);
            modifyRequest.ObjectFragment.ObjectIndex = cloudObject.RemoteObjectIndex;
            client.Send(modifyRequest);
        }

        public void EjectObject(EjectRequestMessage ejectRequest)
        {
            client.Send(ejectRequest);
        }

        public void ExamineObject(ExamineRequestMessage examineRequest)
        {
            client.Send(examineRequest);
        }

        public void InteractWithObject(InteractRequestMessage interactRequest)
        {
            client.Send(interactRequest);
        }

        public void ExecuteAction(ActionEventMessage action)
        {
            client.Send(action);
        }

        public void SendInteractRequest(InteractRequestMessage interactRequest)
        {
            client.Send(interactRequest);
        }

        public void SendInteractResponse(InteractResponseMessage interactResponse)
        {
            client.Send(interactResponse);
        }

        public void MoveObject(MovementEventMessage movement)
        {
            client.Send(movement);
        }

        #endregion    
    }

}
