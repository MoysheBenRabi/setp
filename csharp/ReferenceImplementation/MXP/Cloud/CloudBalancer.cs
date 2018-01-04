using System;
using System.Collections.Generic;
using System.Text;
using MXP.Util;
using MXP.Common.Proto;
using MXP.Messages;
using MXP.Delegates;

namespace MXP.Cloud
{
    /// <summary>
    /// CloudBalancer is responsible of handovers between bubbles.
    /// </summary>
    public class CloudBalancer
    {
        private CloudBubble bubble;
        private List<Handover> handovers = new List<Handover>();

        public CloudBalancer(CloudBubble bubble)
        {
            this.bubble = bubble;
            bubble.BubbleMessageReceived += OnBubbleMessageReceived;
        }

        public CloudParticipantIdentify CloudParticipantIdentify = delegate(Guid participantId) { return participantId.ToString(); };
        public CloudParticipantIdentified CloudParticipantIdentified = delegate(CloudBubble bubble, IdentifyRequestMessage message) { return MxpResponseCodes.SUCCESS; };
        public CloudObjectHandover CloudObjectHandover = delegate(CloudBubble bubble, HandoverRequestMessage message) { return MxpResponseCodes.SUCCESS; };

        public void Process()
        {
            List<Handover> handoversToCarbageCollect=new List<Handover>();
            foreach (Handover handover in handovers)
            {
                // 5 seconds before handover can be retried or done again
                if (handover.Started.Add(new TimeSpan(0, 0, 5))<DateTime.Now)
                {
                    handoversToCarbageCollect.Add(handover);
                }
            }
            foreach (Handover handoverToCarbageCollect in handoversToCarbageCollect)
            {
                handovers.Remove(handoverToCarbageCollect);
            }
        }

        public void OnBubbleMessageReceived(Session session, Message message)
        {
            if (message.GetType() == typeof(IdentifyRequestMessage))
            {
                HandleIdentifyRequest(session, (IdentifyRequestMessage)message);
            }
            if (message.GetType() == typeof(IdentifyResponseMessage))
            {
                HandleIdentifyResponse(session, (IdentifyResponseMessage)message);
            }
            if (message.GetType() == typeof(HandoverRequestMessage))
            {
                HandleHandoverRequest(session, (HandoverRequestMessage)message);
            }
            if (message.GetType() == typeof(HandoverResponseMessage))
            {
                HandleHandoverResponse(session, (HandoverResponseMessage)message);
            }
        }

        public void EvaluateHandoverNeed(CloudObject cloudObject)
        {
            if (cloudObject.BubbleId != bubble.BubbleId)
            {
                throw new Exception("Remote objects can not need to be handed over.");
            }
                        
            MsdVector3f origo=new MsdVector3f();
            
            // Evaluating if object center is inside bubble range
            float distanceFromCenter=MathUtil.Distance(cloudObject.Location, origo);
            if (distanceFromCenter < bubble.BubbleRange)
            {
                return;
            }

            foreach (Handover handover in handovers)
            {
                if (handover.ObjectId == cloudObject.ObjectId)
                {
                    // Object is already in handover process.
                    return;
                }
            }

            // Object is not inside bubble radius 
            // Evaluate if there is another linked bubble where object center would be inside bubble range
            float shortestDistance = float.MaxValue;
            BubbleLink closestBubbleLink = null;
            foreach (BubbleLink bubbleLink in bubble.GetBubbleLinks().Values)
            {
                float distanceFromRemoteCenter = MathUtil.Distance(cloudObject.Location,bubbleLink.RemoteBubbleCenter);
                if (distanceFromRemoteCenter < bubble.BubbleRange && distanceFromCenter < shortestDistance)
                {
                    shortestDistance = distanceFromRemoteCenter;
                    closestBubbleLink = bubbleLink;
                }
            }

            if (closestBubbleLink != null)
            {
                RequestIdentify(closestBubbleLink, cloudObject);
            }
        }

        private void RequestIdentify(BubbleLink bubbleLink, CloudObject cloudObject)
        {
            Session session = bubble.GetBubbleLinkSession(bubbleLink);
            Guid participantId = cloudObject.OwnerId;
            ParticipantLink participantLink = bubble.GetParticipant(cloudObject.OwnerId);

            // Delegate to access participant identity
            string participantIdentity = CloudParticipantIdentify(participantId);

            IdentifyRequestMessage identifyRequest = (IdentifyRequestMessage) MessageFactory.Current.ReserveMessage(typeof(IdentifyRequestMessage));
            identifyRequest.ParticipantId = participantId;
            identifyRequest.ParticipantIdentityType = IdentifyRequestMessage.OPEN_ID_IDENTITY;
            identifyRequest.ParticipantIdentity = participantIdentity;

            Handover handover = new Handover();
            handover.Started = DateTime.Now;
            handover.RemoteBubbleId = bubbleLink.RemoteBubbleId;
            handover.ParticipantId = cloudObject.OwnerId;
            handover.ObjectId = cloudObject.ObjectId;
            handover.IdentityRequestMessageId = identifyRequest.MessageId;
            handovers.Add(handover);

            LogUtil.Info("Sent identify request from " + bubble.BubbleName + " to " + bubbleLink.RemoteBubbleName + " for " + identifyRequest.ParticipantIdentity +" ("+identifyRequest.ParticipantIdentityType+") to be able to handover object "+cloudObject.ObjectId+".");

            session.Send(identifyRequest);
        }

        private void HandleIdentifyRequest(Session session, IdentifyRequestMessage identifyRequest)
        {
            BubbleLink bubbleLink = bubble.GetBubbleLink(session);
            LogUtil.Info("Received identify request from " + bubbleLink.RemoteBubbleName + " to " + bubble.BubbleName + " for " + identifyRequest.ParticipantIdentity + " (" + identifyRequest.ParticipantIdentityType + ").");

            IdentifyResponseMessage identifyResponse = (IdentifyResponseMessage) MessageFactory.Current.ReserveMessage(typeof(IdentifyResponseMessage));
            identifyResponse.RequestMessageId = identifyRequest.MessageId;

            // Call identify to allow application to reject identity.
            identifyResponse.FailureCode = CloudParticipantIdentified(bubble,identifyRequest);

            session.Send(identifyResponse);
        }

        private void HandleIdentifyResponse(Session session, IdentifyResponseMessage identifyResponse)
        {
            foreach (Handover handover in handovers)
            {
                if (identifyResponse.RequestMessageId == handover.IdentityRequestMessageId)
                {
                    CloudObject cloudObject = bubble.CloudCache.GetObject(handover.ObjectId);
                    if (cloudObject == null)
                    {
                        return;
                    }
                    if (cloudObject.BubbleId != bubble.BubbleId)
                    {
                        throw new Exception("Remote objects can not need to be handed over.");
                    }
                    ParticipantLink participantLink = bubble.GetParticipant(cloudObject.OwnerId);
                    BubbleLink bubbleLink = bubble.GetBubbleLink(handover.RemoteBubbleId);

                    LogUtil.Info("Received identify response (" + identifyResponse.FailureCode + ") from " + bubble.BubbleName + " to " + bubbleLink.RemoteBubbleName + " to be able to handover object " + handover.ObjectId + ".");

                    if (identifyResponse.FailureCode != MxpResponseCodes.SUCCESS)
                    {
                        return;
                    }

                    HandoverRequestMessage handoverRequest = (HandoverRequestMessage)MessageFactory.Current.ReserveMessage(typeof(HandoverRequestMessage));
                    handoverRequest.SourceBubbleId = bubble.BubbleId;
                    handoverRequest.TargetBubbleId = handover.RemoteBubbleId;
                    cloudObject.ToObjectFragment(handoverRequest.ObjectFragment);
                    handoverRequest.ObjectFragment.Location.X = cloudObject.Location.X - bubbleLink.RemoteBubbleCenter.X;
                    handoverRequest.ObjectFragment.Location.Y = cloudObject.Location.Y - bubbleLink.RemoteBubbleCenter.Y;
                    handoverRequest.ObjectFragment.Location.Z = cloudObject.Location.Z - bubbleLink.RemoteBubbleCenter.Z;
                    handover.HandoverRequestMessageId = handoverRequest.MessageId;
                    session.Send(handoverRequest);
                    
                    // Storing cloud object in case of rollback
                    handover.CloudObject = cloudObject;
                    // Removing cloud object from bubble.
                    bubble.CloudCache.RemoveObject(cloudObject.ObjectId);                    

                    return;
                }
            }
        }

        private void HandleHandoverRequest(Session session, HandoverRequestMessage handoverRequest)
        {
            BubbleLink bubbleLink = bubble.GetBubbleLink(session);

            LogUtil.Info("Received handover request from " + bubbleLink.RemoteBubbleName + " to " + bubble.BubbleName + " for object " + handoverRequest.ObjectFragment.ObjectId + ").");

            HandoverResponseMessage handoverResponse = (HandoverResponseMessage)MessageFactory.Current.ReserveMessage(typeof(HandoverResponseMessage));
            handoverResponse.RequestMessageId = handoverRequest.MessageId;

            // Calling delegate to allow application to reject the handover.
            handoverResponse.FailureCode = CloudObjectHandover(bubble, handoverRequest);

            if (handoverResponse.FailureCode == MxpResponseCodes.SUCCESS)
            {
                CloudObject cloudObject = bubble.CloudCache.GetObject(handoverRequest.ObjectFragment.ObjectId);

                if (cloudObject == null)
                {
                    // Remote object does not exist in cache so creating new one.
                    cloudObject = new CloudObject();
                }
                else
                {
                    // Object should be located in bubble requesting handover
                    if (cloudObject.BubbleId != handoverRequest.SourceBubbleId)
                    {
                        handoverResponse.FailureCode = MxpResponseCodes.UNAUTHORIZED_OPERATION;
                        session.Send(handoverResponse);
                        return;
                    }

                    // Object should be owned by same participant if not then this is id collision.
                    if (cloudObject.OwnerId != handoverRequest.ObjectFragment.OwnerId)
                    {
                        handoverResponse.FailureCode = MxpResponseCodes.RESERVED_ID;
                        session.Send(handoverResponse);
                        return;
                    }
                }

                cloudObject.FromObjectFragment(bubble.BubbleId, handoverRequest.ObjectFragment);
                // Setting remove object index to 0 as it is local object now and cache will then set it equal to local index
                cloudObject.RemoteObjectIndex = 0;
                bubble.CloudCache.PutObject(cloudObject, true);
            }

            session.Send(handoverResponse);

            // send event
        }

        private void HandleHandoverResponse(Session session, HandoverResponseMessage handoverResponse)
        {
            BubbleLink bubbleLink = bubble.GetBubbleLink(session);

            foreach (Handover handover in handovers)
            {
                if (handoverResponse.RequestMessageId == handover.HandoverRequestMessageId)
                {
                    LogUtil.Info("Received handover response (" + handoverResponse.FailureCode + ") from " + bubbleLink.RemoteBubbleName + " to " + bubble.BubbleName + " for object " + handover.ObjectId + ").");

                    // If failure then rollback
                    if (handoverResponse.FailureCode != MxpResponseCodes.SUCCESS)
                    {
                        bubble.CloudCache.PutObject(handover.CloudObject, true);
                    }
                    else
                    {
                        ParticipantLink participant = bubble.GetParticipant(handover.ParticipantId);
                        Session participantSession = bubble.GetParticipantSession(participant);
                        HandoverEventMessage handoverEvent = new HandoverEventMessage();
                        handoverEvent.SourceBubbleId = bubble.BubbleId;
                        handoverEvent.TargetBubbleId = handover.RemoteBubbleId;
                        handover.CloudObject.ToObjectFragment(handoverEvent.ObjectFragment);
                        participantSession.Send(handoverEvent);
                    }
                }
            }

        }
    }
}
