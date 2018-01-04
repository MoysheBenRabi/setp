using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MXP;
using System.Threading;
using System.Diagnostics;
using MXP.Messages;
using MXP.Fragments;

namespace MXDeck.Engine
{
    public class DeckEngine
    {

        #region fields

        public MxpClient Client;
        public DeckBubble Bubble;
        public string serverAddress;
        public int serverPort;
        public string Location;

        private Guid bubbleId;
        private string identityProviderUrl;
        private string participantName;
        private string participantPassphrase;
        private Deck deck;
        private DateTime lastAvatarUpdateTime = DateTime.Now;

        public bool IsConnected
        {
            get
            {
                return Client.IsConnected;
            }
        }

        public Guid ParticipantId
        {
            get
            {
                return Client.ParticipantId;
            }
        }

        public SessionState SessionState
        {
            get
            {
                return Client.SessionState;
            }
        }

        #endregion

        #region constructors, startup and shutdown

        public DeckEngine(
            string serverAddress,
            int serverPort,
            Guid bubbleId,
            string location,
            string identityProviderUrl,
            string participantName,
            string participantPassphrase,
            Deck deck
            )
        {
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
            this.bubbleId = bubbleId;
            this.Location = location;
            this.identityProviderUrl = identityProviderUrl;
            this.participantName = participantName;
            this.participantPassphrase = participantPassphrase;
            String programName = "MX XNA Deck";
            byte programMajorVersion = 0;
            byte programMinorVersion = 1;
            Client = new MxpClient(programName, programMajorVersion, programMinorVersion);
            Client.ServerMessageReceived+=OnMessage;
            Client.ServerDisconnected += OnDisconnected;
            Bubble = new DeckBubble();
            Bubble.BubbleId = this.bubbleId;
            this.deck = deck;
        }

        public void Startup()
        {
            Client.Connect(serverAddress, serverPort, bubbleId, "", Location, identityProviderUrl, participantName, participantPassphrase,deck.AvatarId, true);
        }

        public void Shutdown()
        {
            Client.Disconnect();
        }

        #endregion

        public void Process(float timeDelta,bool avatarControlledOrChanged)
        {
            try
            {
                Client.Process();
                Bubble.Process(timeDelta);
                if (!Client.IsConnected && !Client.IsConnecting)
                {
                    deck.ReceivedAvatarObject = null;
                    Client.Connect(serverAddress, serverPort, bubbleId, "", Location, identityProviderUrl, participantName, participantPassphrase,deck.AvatarId, true);
                }

                if (Client.IsConnected && DateTime.Now.Subtract(lastAvatarUpdateTime).TotalMilliseconds > 200)
                {
                    if (deck.ReceivedAvatarObject == null)
                    {
                        InjectRequestMessage injectRequestMessage = (InjectRequestMessage)MessageFactory.Current.ReserveMessage(typeof(InjectRequestMessage));
                        injectRequestMessage.ObjectFragment.OwnerId = ParticipantId;
                        injectRequestMessage.ObjectFragment.ObjectId = deck.AvatarId;
                        injectRequestMessage.ObjectFragment.ObjectName = deck.AvatarName;
                        injectRequestMessage.ObjectFragment.TypeId = deck.AvatarTypeId;
                        injectRequestMessage.ObjectFragment.TypeName = deck.AvatarTypeName;
                        injectRequestMessage.ObjectFragment.Location.X = deck.AvatarLocation.X / 10f;
                        injectRequestMessage.ObjectFragment.Location.Y = deck.AvatarLocation.Y / 10f;
                        injectRequestMessage.ObjectFragment.Location.Z = deck.AvatarLocation.Z / 10f;
                        injectRequestMessage.ObjectFragment.Orientation.X = deck.AvatarOrientation.X;
                        injectRequestMessage.ObjectFragment.Orientation.Y = deck.AvatarOrientation.Y;
                        injectRequestMessage.ObjectFragment.Orientation.Z = deck.AvatarOrientation.Z;
                        injectRequestMessage.ObjectFragment.Orientation.W = deck.AvatarOrientation.W;
                        injectRequestMessage.ObjectFragment.BoundingSphereRadius = deck.AvatarBoundingSphereRadius;
                        Client.Send(injectRequestMessage);
                    }
                    else
                    {
                        MovementEventMessage movementEvent = new MovementEventMessage();
                        movementEvent.ObjectIndex = deck.ReceivedAvatarObject.ObjectIndex;
                        movementEvent.Location.X = deck.AvatarLocation.X / 10f;
                        movementEvent.Location.Y = deck.AvatarLocation.Y / 10f;
                        movementEvent.Location.Z = deck.AvatarLocation.Z / 10f;
                        movementEvent.Orientation.X = deck.AvatarOrientation.X;
                        movementEvent.Orientation.Y = deck.AvatarOrientation.Y;
                        movementEvent.Orientation.Z = deck.AvatarOrientation.Z;
                        movementEvent.Orientation.W = deck.AvatarOrientation.W;
                        Client.Send(movementEvent);
                    }
                    lastAvatarUpdateTime = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Thread.Sleep(1000);

            }
        }

        public void OnMessage(Message message)
        {
            if (message.GetType() == typeof(JoinResponseMessage))
            {
                JoinResponseMessage joinResponse = (JoinResponseMessage)message;
                if (joinResponse.FailureCode == MxpResponseCodes.SUCCESS)
                {
                    Bubble.BubbleName = joinResponse.BubbleName;
                    Bubble.CloudUrl = joinResponse.BubbleAssetCacheUrl;
                    Bubble.ServerProgramName = joinResponse.ProgramName;
                    Bubble.ServerProgramMajorVersion = joinResponse.ProgramMajorVersion;
                    Bubble.ServerProgramMinorVersion = joinResponse.ProgramMinorVersion;
                    Bubble.ServerProtocolMajorVersion = joinResponse.ProtocolMajorVersion;
                    Bubble.ServerProtocolMinorVersion = joinResponse.ProtocolMinorVersion;
                    Bubble.ServerProtocolSourceRevision = (int) joinResponse.ProtocolSourceRevision;
                    Bubble.BubbleRange = 100;
                }
            }

            if (message.GetType() == typeof(PerceptionEventMessage))
            {
                PerceptionEventMessage perception = (PerceptionEventMessage)message;
                ObjectFragment fragment = perception.ObjectFragment;

                DeckObject obj = null;
                if (!Bubble.Objects.ContainsKey(fragment.ObjectIndex))
                {
                    obj = new DeckObject();
                    Bubble.Objects.Add(fragment.ObjectIndex, obj);
                    Bubble.IdObjectDictionary.Add(fragment.ObjectId,obj);
                }
                else
                {
                    obj = Bubble.Objects[fragment.ObjectIndex];
                }

                obj.SetValues(fragment);

                if (perception.ObjectFragment.ObjectId == deck.AvatarId)
                {
                    deck.ReceivedAvatarObject = obj;
                }
                
            }

            if (message.GetType() == typeof(MovementEventMessage))
            {
                MovementEventMessage movement = (MovementEventMessage)message;
                
                DeckObject obj = null;
                if (Bubble.Objects.ContainsKey(movement.ObjectIndex))
                {
                    obj = Bubble.Objects[movement.ObjectIndex];
                    obj.SetValues(movement);
                }

            }

            if (message.GetType() == typeof(DisappearanceEventMessage))
            {
                DisappearanceEventMessage disappearance = (DisappearanceEventMessage)message;
                if (Bubble.Objects.ContainsKey(disappearance.ObjectIndex))
                {
                    if(Bubble.Objects.ContainsKey(disappearance.ObjectIndex))
                    {
                        DeckObject obj= Bubble.Objects[disappearance.ObjectIndex];
                        Bubble.IdObjectDictionary.Remove(obj.ObjectId);
                        Bubble.Objects.Remove(disappearance.ObjectIndex);
                    }
                }
            }

            if (message.GetType() == typeof(ActionEventMessage))
            {
                ActionEventMessage actionEvent=(ActionEventMessage)message;
                String sourceObjectIdentifier=null;
                if(Bubble.IdObjectDictionary.ContainsKey(actionEvent.ActionFragment.SourceObjectId))
                {
                    DeckObject obj=Bubble.IdObjectDictionary[actionEvent.ActionFragment.SourceObjectId];
                    sourceObjectIdentifier=obj.ObjectName;
                }
                else
                {
                    sourceObjectIdentifier=actionEvent.ActionFragment.SourceObjectId.ToString();
                }
                deck.ConsoleLines.Add(sourceObjectIdentifier+" "+actionEvent.ActionFragment.ActionName+" ("+actionEvent.ActionFragment.ExtensionDialect+"): "+Encoding.UTF8.GetString(actionEvent.ActionFragment.GetExtensionData(),0,(int)actionEvent.ActionFragment.ExtensionLength));
            }

        }

        public void SendChatLine(String line)
        {
            ActionEventMessage chatActionEvent = new ActionEventMessage();

            chatActionEvent.ActionFragment.ActionName = "Chat";
            chatActionEvent.ActionFragment.SourceObjectId = deck.AvatarId;
            chatActionEvent.ActionFragment.ObservationRadius = 100;

            chatActionEvent.ActionFragment.ExtensionDialect = "TEXT";
            chatActionEvent.SetPayloadData(UTF8Encoding.UTF8.GetBytes(line));

            Client.Send(chatActionEvent);
        }

        public void OnDisconnected(Message message)
        {
            Bubble.Objects.Clear();
        }
    }
}
