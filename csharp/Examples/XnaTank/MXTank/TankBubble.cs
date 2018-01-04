using System;
using System.Collections.Generic;

using System.Text;
using MXP;
using MXP.Messages;
using MXP.Fragments;

namespace MXTank
{

    /// <summary>
    /// TankBubble is a bubble hosted in Tank. TankBubble is hooked by event handlers to the MxpBubble and manages the actual distributed
    /// virtual environment information broking between participants and bubbles.
    /// </summary>
    public class TankBubble : Bubble
    {

        #region fields

        public MxpBubble MxpBubble;

        public List<TankObject> Objects = new List<TankObject>();
        public IDictionary<Guid, TankObject> IdObjectDictionary = new Dictionary<Guid, TankObject>();
        public IDictionary<Guid, List<TankObject>> ParticipantIdObjectDictionary = new Dictionary<Guid, List<TankObject>>();
        public IDictionary<uint, TankObject> IndexObjectDictionary = new Dictionary<uint, TankObject>();

        
        public List<RemoteParticipant> Participants = new List<RemoteParticipant>();
        public IDictionary<Session, RemoteParticipant> SessionParticipantDictionary = new Dictionary<Session, RemoteParticipant>();
        public IDictionary<Guid, RemoteParticipant> IdParticipantDictionary = new Dictionary<Guid, RemoteParticipant>();

        public List<RemoteBubble> Bubbles = new List<RemoteBubble>();
        public IDictionary<Session, RemoteBubble> SessionBubbleDictionary = new Dictionary<Session, RemoteBubble>();
        public IDictionary<Guid, RemoteBubble> IdBubbleDictionary = new Dictionary<Guid, RemoteBubble>();
        
        #endregion

        #region constructors

        public TankBubble(
            Guid bubbleId,
            string bubbleName,
            Guid ownerId,
            float bubbleRange,
            float bubblePerceptionRange
            ) : base(bubbleId,bubbleName,ownerId,bubbleRange,bubblePerceptionRange)

        { 
            
            this.MxpBubble = new MxpBubble(bubbleId,bubbleName,bubbleRange,bubblePerceptionRange);
            MxpBubble.ParticipantConnectAuthorize += OnParticipantConnectAuthorize;
            MxpBubble.ParticipantConnected += OnParticipantConnected;
            MxpBubble.ParticipantDisconnected += OnParticipantDisconnected;
            MxpBubble.ParticipantMessageReceived += OnParticipantMessageReceived;
            MxpBubble.BubbleConnected += OnBubbleConnected;
            MxpBubble.BubbleDisconnected += OnBubbleDisconnected;
            MxpBubble.BubbleMessageReceived += OnParticipantMessageReceived;
        }

        #endregion

        #region message handlers

        public bool OnParticipantConnectAuthorize(Session session, JoinRequestMessage message, out Guid participantId, out Guid avatarId)
        {
            participantId = Guid.NewGuid();
            avatarId = Guid.Empty;
            JoinRequestMessage joinRequestMessage = (JoinRequestMessage)message;
            RemoteParticipant participant = new RemoteParticipant(participantId, joinRequestMessage.ParticipantIdentifier, session);
            AddParticipant(participant);
            return true;
        }

        public void OnParticipantConnected(Session session, Message message, Guid participantId, Guid avatarId)
        {
        }

        public void OnParticipantDisconnected(Session session)
        {
            if (SessionParticipantDictionary.ContainsKey(session))
            {
                RemoteParticipant participant = SessionParticipantDictionary[session];
                RemoveParticipant(participant);
            }
        }

        public bool OnBubbleConnectAuthorize(Session session, AttachRequestMessage message)
        {
            return true;
        }

        public void OnBubbleConnected(Session session, Message message)
        {
            AttachRequestMessage attachRequestMessage = (AttachRequestMessage)message;
            RemoteBubble bubble = new RemoteBubble(
                attachRequestMessage.SourceBubbleFragment.BubbleId,
                attachRequestMessage.SourceBubbleFragment.BubbleName,
                attachRequestMessage.SourceBubbleFragment.BubbleAssetCacheUrl,
                attachRequestMessage.SourceBubbleFragment.OwnerId,
                attachRequestMessage.SourceBubbleFragment.BubbleAddress,
                attachRequestMessage.SourceBubbleFragment.BubblePort,
                attachRequestMessage.SourceBubbleFragment.BubbleCenter.X,
                attachRequestMessage.SourceBubbleFragment.BubbleCenter.Y,
                attachRequestMessage.SourceBubbleFragment.BubbleCenter.Z,
                attachRequestMessage.SourceBubbleFragment.BubbleRange,
                attachRequestMessage.SourceBubbleFragment.BubblePerceptionRange,
                attachRequestMessage.SourceBubbleFragment.BubbleRealTime,
                session);
            AddBubble(bubble);
        }

        public void OnBubbleDisconnected(Session session)
        {
            if(SessionBubbleDictionary.ContainsKey(session))
            {
                RemoteBubble bubble = SessionBubbleDictionary[session];
                RemoveBubble(bubble);
            }
        }

        public void OnBubbleMessageReceived(Session session, Message message)
        {
            if(SessionBubbleDictionary.ContainsKey(session))
            {
                RemoteBubble bubble = SessionBubbleDictionary[session];
            }
        }

        #endregion

        #region participant, bubble and object add/remove methods
        
        public void AddParticipant(RemoteParticipant participant)
        {
            SessionParticipantDictionary.Add(participant.Session, participant);
            IdParticipantDictionary.Add(participant.ParticipantId, participant);
            Participants.Add(participant);
            foreach (TankObject obj in Objects)
            {
                PerceptionEventMessage perceptionEvent = new PerceptionEventMessage();
                obj.GetValues(perceptionEvent.ObjectFragment);
                participant.Session.Send(perceptionEvent);
            }
        }

        public void RemoveParticipant(RemoteParticipant participant)
        {
            if(ParticipantIdObjectDictionary.ContainsKey(participant.ParticipantId))
            {
                List<TankObject> objects = new List<TankObject>(ParticipantIdObjectDictionary[participant.ParticipantId]);
                foreach (TankObject obj in objects)
                {
                    RemoveObject(obj);
                    ObjectEjected(obj);
                }
            }

            SessionParticipantDictionary.Remove(participant.Session);
            IdParticipantDictionary.Remove(participant.ParticipantId);
            Participants.Remove(participant);
        }

        public void AddBubble(RemoteBubble bubble)
        {
            SessionBubbleDictionary.Add(bubble.Session,bubble);
            IdBubbleDictionary.Add(bubble.BubbleId, bubble);
            Bubbles.Add(bubble);
        }

        public void RemoveBubble(RemoteBubble bubble)
        {
            SessionBubbleDictionary.Remove(bubble.Session);
            IdBubbleDictionary.Remove(bubble.BubbleId);
            Bubbles.Remove(bubble);
        }

        public void AddObject(TankObject obj)
        {
            obj.ObjectIndex = TankObject.GetNewIndex();
            if (!ParticipantIdObjectDictionary.ContainsKey(obj.OwnerId))
            {
                ParticipantIdObjectDictionary.Add(obj.OwnerId, new List<TankObject>());
            }
            ParticipantIdObjectDictionary[obj.OwnerId].Add(obj);
            IndexObjectDictionary.Add(obj.ObjectIndex, obj);
            IdObjectDictionary.Add(obj.ObjectId, obj);
            Objects.Add(obj);
        }

        public void RemoveObject(TankObject obj)
        {

            ParticipantIdObjectDictionary[obj.OwnerId].Remove(obj);
            if (ParticipantIdObjectDictionary[obj.OwnerId].Count == 0)
            {
                ParticipantIdObjectDictionary.Remove(obj.OwnerId);
            }
            IndexObjectDictionary.Remove(obj.ObjectIndex);
            IdObjectDictionary.Remove(obj.ObjectId);
            Objects.Remove(obj);
        }

        #endregion

        public void OnParticipantMessageReceived(Session session, Message message)
        {
            if (SessionParticipantDictionary.ContainsKey(session))
            {
                RemoteParticipant participant = SessionParticipantDictionary[session];

                if (message.GetType() == typeof(InjectRequestMessage))
                {
                    OnInjectRequest(participant, (InjectRequestMessage)message);
                }

                if (message.GetType() == typeof(EjectRequestMessage))
                {
                    OnEjectRequest(participant, (EjectRequestMessage)message);
                }

                if (message.GetType() == typeof(MovementEventMessage))
                {
                    OnMovementEvent(participant, (MovementEventMessage)message);
                }

                if (message.GetType() == typeof(ActionEventMessage))
                {
                    OnActionEvent(participant, (ActionEventMessage)message);
                }

            }
        }

        public void OnInjectRequest(RemoteParticipant participant, InjectRequestMessage injectRequest)
        {
            InjectResponseMessage injectResponse = new InjectResponseMessage();
            injectResponse.MessageId = injectRequest.MessageId;

            ObjectFragment fragment=injectRequest.ObjectFragment;

            if (IdObjectDictionary.ContainsKey(fragment.ObjectId))
            {
                // Object exists already so this is modify
                TankObject obj = IdObjectDictionary[fragment.ObjectId];
                if (obj.OwnerId == participant.ParticipantId)
                {
                    obj.SetValues(fragment);
                    ObjectMoved(obj);
                    injectResponse.FailureCode = MxpResponseCodes.SUCCESS;
                    participant.Session.Send(injectResponse);
                }
                else
                {
                    injectResponse.FailureCode = MxpResponseCodes.UNAUTHORIZED_OPERATION;
                    participant.Session.Send(injectResponse);
                }
            }
            else
            {
                // Object does not exist so this is inject
                TankObject obj = new TankObject();
                obj.OwnerId = participant.ParticipantId;
                obj.SetValues(fragment);
                AddObject(obj);
                ObjectInjected(obj);
                injectResponse.FailureCode = MxpResponseCodes.SUCCESS;
                participant.Session.Send(injectResponse);
            }


        }

        public void OnEjectRequest(RemoteParticipant participant, EjectRequestMessage ejectRequest)
        {
            EjectResponseMessage ejectResponse = new EjectResponseMessage();
            ejectResponse.MessageId = ejectRequest.MessageId;

            if (IdObjectDictionary.ContainsKey(ejectRequest.ObjectId))
            {
                TankObject obj = IdObjectDictionary[ejectRequest.ObjectId];

                if (obj.OwnerId == participant.ParticipantId)
                {
                    RemoveObject(obj);
                    ObjectEjected(obj);

                    ejectResponse.FailureCode = MxpResponseCodes.SUCCESS;
                    participant.Session.Send(ejectResponse);
                }
                else
                {
                    ejectResponse.FailureCode = MxpResponseCodes.UNAUTHORIZED_OPERATION;
                    participant.Session.Send(ejectResponse);
                }
            }
            else
            {
                ejectResponse.FailureCode = MxpResponseCodes.UNKNOWN_OBJECT_ID;
                participant.Session.Send(ejectResponse);
            }

        }

        public void OnMovementEvent(RemoteParticipant participant, MovementEventMessage movementEvent)
        {            
            if(IndexObjectDictionary.ContainsKey(movementEvent.ObjectIndex))
            {
                TankObject obj=IndexObjectDictionary[movementEvent.ObjectIndex];
                obj.Location[0]=movementEvent.Location.X;
                obj.Location[1]=movementEvent.Location.Y;
                obj.Location[2]=movementEvent.Location.Z;
                obj.Orientation[0] = movementEvent.Orientation.X;
                obj.Orientation[1] = movementEvent.Orientation.Y;
                obj.Orientation[2] = movementEvent.Orientation.Z;
                obj.Orientation[3] = movementEvent.Orientation.W;
                ObjectMoved(obj);
            }
        }

        public void OnActionEvent(RemoteParticipant participant, ActionEventMessage actionEvent)
        {
            if (IdObjectDictionary.ContainsKey(actionEvent.ActionFragment.SourceObjectId))
            {
                // only forwarding actions of known objects.
                TankObject obj = IdObjectDictionary[actionEvent.ActionFragment.SourceObjectId];
                ObjectActed(actionEvent);
            }
        }

        public void ObjectInjected(TankObject obj)
        {
            PerceptionEventMessage perceptionEvent = new PerceptionEventMessage();
            obj.GetValues(perceptionEvent.ObjectFragment);

            for (int i = 0; i < Participants.Count; i++)
            {
                Participants[i].Session.Send(perceptionEvent);
            }
        }

        public void ObjectMoved(TankObject obj)
        {
            MovementEventMessage movementEvent = new MovementEventMessage();
            movementEvent.ObjectIndex = obj.ObjectIndex;
            movementEvent.Location.X = obj.Location[0];
            movementEvent.Location.Y = obj.Location[1];
            movementEvent.Location.Z = obj.Location[2];
            movementEvent.Orientation.X = obj.Orientation[0];
            movementEvent.Orientation.Y = obj.Orientation[1];
            movementEvent.Orientation.Z = obj.Orientation[2];
            movementEvent.Orientation.W = obj.Orientation[3];

            for (int i = 0; i < Participants.Count; i++)
            {
                Participants[i].Session.Send(movementEvent);
            }
        }

        public void ObjectEjected(TankObject obj)
        {
            DisappearanceEventMessage disappearanceEvent = new DisappearanceEventMessage();
            disappearanceEvent.ObjectIndex = obj.ObjectIndex;

            for (int i = 0; i < Participants.Count; i++)
            {
                Participants[i].Session.Send(disappearanceEvent);
            }
        }

        public void ObjectActed(ActionEventMessage actionEvent)
        {
            ActionEventMessage newActionEvent = new ActionEventMessage();
            newActionEvent.ActionFragment.ActionName = actionEvent.ActionFragment.ActionName;
            newActionEvent.ActionFragment.SourceObjectId = actionEvent.ActionFragment.SourceObjectId;
            newActionEvent.ActionFragment.ExtensionDialect = actionEvent.ActionFragment.ExtensionDialect;
            newActionEvent.ActionFragment.SetExtensionData(actionEvent.ActionFragment.GetExtensionData());

            for (int i = 0; i < Participants.Count; i++)
            {
                Participants[i].Session.Send(newActionEvent);
            }
        }

        public void Startup()
        {
        }

        public void Shutdown()
        {
            LeaveRequestMessage leaveRequest = new LeaveRequestMessage();
            foreach(RemoteParticipant participant in Participants)
            {
                participant.Session.Send(leaveRequest);
            }

            List<RemoteParticipant> participants = new List<RemoteParticipant>(Participants);
            foreach (RemoteParticipant participant in participants)
            {
                RemoveParticipant(participant);
            }
        }

        public void Process()
        {
        }

    }
}
