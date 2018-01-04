using System;
using System.Collections.Generic;

using System.Text;
using MXP.Messages;
using MXP.Delegates;

namespace MXP
{

    /// <summary>
    /// MxpBubble is the server side object which peers connect to and contains event handlers for incomig messages.
    /// This is skeletal implementation which is extended or aggregated by server implementations. See MXTank.Tank class
    /// for example implementation.
    /// </summary>
    public class MxpBubble
    { 

        public Guid BubbleId=Guid.Empty;
        public Guid OwnerId = Guid.Empty;
        public string BubbleName;
        public float BubbleRange;
        public float BubblePerceptionRange;

        public MxpBubble(Guid bubbleId, string bubbleName, float bubbleRange, float bubblePerceptionRange)
        {
            BubbleId = bubbleId;
            BubbleName = bubbleName;
            BubbleRange = bubbleRange;
            BubblePerceptionRange = bubblePerceptionRange;
        }

        public void Clear()
        {
            BubbleId = Guid.Empty;
            BubbleName = null;
            BubbleRange = 0;
            BubblePerceptionRange = 0;
        }

        public ParticipantConnectAuthorize ParticipantConnectAuthorize =
            delegate(Session session, JoinRequestMessage message, out Guid participantId, out Guid avatarId)
                {
                    participantId = Guid.Empty;
                    avatarId = Guid.Empty;
                    return true;
                };

        public ParticipantConnected ParticipantConnected = delegate(Session session, JoinRequestMessage message, Guid participantId, Guid avatarId) { };
        public ParticipantConnectFailure ParticipantConnectFailure = delegate(Session session, Message message) { };
        public ParticipantMessageReceived ParticipantMessageReceived = delegate(Session session, Message message) { };
        public ParticipantDisconnected ParticipantDisconnected = delegate(Session session) { };

        public BubbleConnectAuthorize BubbleConnectAuthorize = delegate(Session session, AttachRequestMessage message) { return true; };
        public BubbleConnected BubbleConnected = delegate(Session session, Message message) { };
        public BubbleConnectFailure BubbleConnectFailure = delegate(Session session, Message message) { };
        public BubbleMessageReceived BubbleMessageReceived = delegate(Session session, Message message) { };
        public BubbleDisconnected BubbleDisconnected = delegate(Session session) { };

        public BubbleListRequested BubbleListRequested = delegate(Session session, ListBubblesRequest message) { };
        public BubbleListReceived BubbleListReceived = delegate(Session session, ListBubblesResponse message) { };

    }

}
