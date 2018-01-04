using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MXP.Messages;
using System.Net;
using MXP.Util;
using MXP.Fragments;
using MXP.Common.Proto;
using MXP.Delegates.MXP.Delegates;

namespace MXP.Cloud
{
    /// <summary>
    /// CloudBubble is reference implementation of MxpBubble meant to be use in conjunction with MxpService.
    /// CloudBubble upkeeps list of linked bubbles, their connection state.
    /// CloudBubble upkeeps list of connected participants.
    /// CloudBubble upkeeps object cache of local and remote objects for timely responses to full synchronization requests.
    /// TODO Validate avatar ownership.
    /// </summary>
    public class CloudBubble : MxpBubble
    {
        #region Fields
        private List<string> allowedRemoteHubAddresses = new List<string>();
        private Dictionary<Guid, BubbleLink> bubbleLinks = new Dictionary<Guid, BubbleLink>();
        private Dictionary<Session, BubbleLink> sessionBubbleLinkDictionary = new Dictionary<Session, BubbleLink>();
        private Dictionary<BubbleLink, Session> bubbleLinkSessionDictionary = new Dictionary<BubbleLink, Session>();
        private Dictionary<Guid, ParticipantLink> participantLinks = new Dictionary<Guid, ParticipantLink>();
        private Dictionary<Session, ParticipantLink> sessionParticipantLinkDictionary = new Dictionary<Session, ParticipantLink>();
        private Dictionary<ParticipantLink, Session> participantSessionDictionary = new Dictionary<ParticipantLink, Session>();
        private CloudService service;
        private CloudCache cloudCache;
        private CloudBalancer balancer;
        private float maxParticipantObservationRange;
        #endregion

        #region Properties

        public CloudService Service
        {
            get
            {
                return service;
            }
            set
            {
                service = value;
            }
        }

        public CloudBalancer Balancer
        {
            get
            {
                return balancer;
            }
        }

        public CloudCache CloudCache
        {
            get
            {
                return cloudCache;
            }
        }

        #endregion

        #region Delegates

        public CloudParticipantDisconnected CloudParticipantDisconnected = delegate(CloudBubble bubble, Guid participantId) { };

        #endregion

        #region Constructors

        public CloudBubble(Guid bubbleId, string bubbleName, float bubbleRange, float bubblePerceptionRange)
            : base(bubbleId, bubbleName, bubbleRange, bubblePerceptionRange)
        {
            this.BubbleConnectAuthorize += OnBubbleConnectAuthorize;
            this.BubbleConnected += OnBubbleConnected;
            this.BubbleConnectFailure += OnBubbleConnectFailure;
            this.BubbleDisconnected += OnBubbleDisconnected;
            this.BubbleListRequested += OnBubbleListRequested;
            this.BubbleListReceived += OnBubbleListReceived;
            this.BubbleMessageReceived += OnBubbleMessageReceived;
            this.ParticipantConnectAuthorize += OnParticipantConnectAuthorize;
            this.ParticipantConnected += OnParticipantConnected;
            this.ParticipantDisconnected += OnParticipantDisconnected;
            this.ParticipantMessageReceived += OnParticipantMessageReceived;

            this.cloudCache = new CloudCache(bubbleId, bubblePerceptionRange, new TimeSpan(0, 0, 10, 0));
            this.cloudCache.CacheObjectPut += OnCacheObjectPut;
            this.cloudCache.CacheObjectRemoved += OnCacheObjectRemoved;

            this.balancer = new CloudBalancer(this);

            maxParticipantObservationRange = 250;
        }

        #endregion

        #region Startup and Shutdown

        public void Startup()
        {
            // Scheduling automatic delayed link connect.
            int i = 0;
            foreach (BubbleLink bubbleLink in bubbleLinks.Values)
            {
                if (!bubbleLink.IsConnected && bubbleLink.IsInitiator)
                {
                    bubbleLink.ScheduledConnect = DateTime.Now.Add(new TimeSpan(0, 0, i * MxpConstants.BubbleToBubbleConnectDelaySeconds));
                }
                i++;
            }
        }

        public void Shutdown()
        {
            // Transmitter is being shutdown and process is not being called so cleaning up manually sessions and link connect states.
            sessionBubbleLinkDictionary.Clear();
            foreach (BubbleLink bubbleLink in bubbleLinks.Values)
            {
                if (bubbleLink.IsConnected)
                {
                    bubbleLink.LastDisconnect = DateTime.Now;
                    bubbleLink.IsConnected = false;
                }
            }
        }

        #endregion

        #region Processing

        public void Process()
        {

            // Process BubbleLinks
            foreach (BubbleLink bubbleLink in bubbleLinks.Values)
            {

                // Schedule and connect enabled, locally initiated connections if they are not connected.
                if ((!bubbleLink.IsConnected) && bubbleLink.IsEnabled && bubbleLink.IsInitiator)
                {
                    if (bubbleLink.ScheduledConnect == DateTime.MinValue)
                    {
                        bubbleLink.ScheduledConnect = DateTime.Now.Add(new TimeSpan(0, 0, MxpConstants.BubbleToBubbleConnectDelaySeconds));
                    }
                    if (bubbleLink.ScheduledConnect < DateTime.Now)
                    {
                        bubbleLink.ScheduledConnect = DateTime.MinValue; // Enable timed reconnect.
                        bubbleLink.ScheduledDisconnect = DateTime.Now; // Enable immediate disconnect on disable.
                        bubbleLink.IsConnecting = true;
                        service.Hub.Connect(BubbleId, bubbleLink.RemoteBubbleId, bubbleLink.RemoteHubAddress, bubbleLink.RemoteHubPort,
                            bubbleLink.RemoteBubbleCenter.X, bubbleLink.RemoteBubbleCenter.Y, bubbleLink.RemoteBubbleCenter.Z);
                        LogUtil.Info("Requesting bubble link connect - from " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteHubAddress + ":" + bubbleLink.RemoteHubPort + "/" + bubbleLink.RemoteBubbleId + ")");
                    }
                }

                // Disconnect session if it is disabled and connected.
                if ((!bubbleLink.IsEnabled) && bubbleLink.IsConnected)
                {
                    Session session = bubbleLinkSessionDictionary[bubbleLink];
                    if (bubbleLink.ScheduledDisconnect == DateTime.MinValue)
                    {
                        bubbleLink.ScheduledDisconnect =
                            DateTime.Now.Add(new TimeSpan(0, 0, MxpConstants.BubbleToBubbleDisconnectDelaySeconds));
                    }
                    if (bubbleLink.ScheduledDisconnect < DateTime.Now)
                    {
                        bubbleLink.ScheduledDisconnect = DateTime.MinValue; // Enable timed redisconnect.
                        bubbleLink.ScheduledConnect = DateTime.Now; // Enable immediate connect on enable.
                        service.Hub.Disconnect(session);

                        LogUtil.Info("Requesting bubble link disconnect - from " + BubbleName + " (" +
                                     service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " +
                                     bubbleLink.RemoteBubbleId + " (" + bubbleLink.RemoteHubAddress + ":" +
                                     bubbleLink.RemoteHubPort + ")");
                    }

                }

                // Process bubble link send list.
                if (bubbleLink.IsConnected && bubbleLink.ObjectSendList.Count > 0)
                {
                    Session session = bubbleLinkSessionDictionary[bubbleLink];
                    int n = 0;
                    while (bubbleLink.ObjectSendList.Count > 0)
                    {
                        Guid objectId = bubbleLink.ObjectSendList[bubbleLink.ObjectSendList.Count - 1];
                        CloudObject cloudObject = cloudCache.GetObject(objectId);
                        bubbleLink.ObjectSendList.RemoveAt(bubbleLink.ObjectSendList.Count - 1);

                        // Send only local objects to other bubbles.
                        if (cloudObject != null && cloudObject.BubbleId == BubbleId)
                        {
                            PerceptionEventMessage perception =
                                (PerceptionEventMessage)
                                MessageFactory.Current.ReserveMessage(typeof(PerceptionEventMessage));

                            cloudObject.ToObjectFragment(perception.ObjectFragment);

                            // Coordinate transformation
                            perception.ObjectFragment.Location.X = perception.ObjectFragment.Location.X - bubbleLink.RemoteBubbleCenter.X;
                            perception.ObjectFragment.Location.Y = perception.ObjectFragment.Location.Y - bubbleLink.RemoteBubbleCenter.Y;
                            perception.ObjectFragment.Location.Z = perception.ObjectFragment.Location.Z - bubbleLink.RemoteBubbleCenter.Z;

                            session.Send(perception);
                            bubbleLink.ObservedObjects[cloudObject.ObjectId] = DateTime.Now;
                            n++;
                        }
                        if (n > 10)
                        {
                            break;
                        }
                    }
                }

                // Update bubble link observed objects.
                if(bubbleLink.IsConnected)
                {
                    HashSet<Guid> objectsInRage=new HashSet<Guid>();
                    cloudCache.GetObjectIds(
                        bubbleLink.RemoteBubbleCenter.X - bubbleLink.RemoteBubblePerceptionRange,
                        bubbleLink.RemoteBubbleCenter.Y - bubbleLink.RemoteBubblePerceptionRange,
                        bubbleLink.RemoteBubbleCenter.Z - bubbleLink.RemoteBubblePerceptionRange,
                        bubbleLink.RemoteBubbleCenter.X + bubbleLink.RemoteBubblePerceptionRange,
                        bubbleLink.RemoteBubbleCenter.Y + bubbleLink.RemoteBubblePerceptionRange,
                        bubbleLink.RemoteBubbleCenter.Z + bubbleLink.RemoteBubblePerceptionRange,
                        objectsInRage);
                    UpdateObservedObjects(bubbleLinkSessionDictionary[bubbleLink], bubbleLink.ObservedObjects,
                                          objectsInRage, bubbleLink.RemoteBubbleCenter,true);
                }

            }

            // Process participants
            foreach (ParticipantLink participantLink in participantLinks.Values)
            {
                CloudObject avatar = cloudCache.GetObject(participantLink.AvatarId);

                // Adding object ids in perception range to send list if this has not been done before.
                if (avatar != null && participantLink.InitialObjectSendListFillDone == false)
                {
                    cloudCache.GetObjectIds(
                        avatar.Location.X - maxParticipantObservationRange,
                        avatar.Location.Y - maxParticipantObservationRange,
                        avatar.Location.Z - maxParticipantObservationRange,
                        avatar.Location.X + maxParticipantObservationRange,
                        avatar.Location.Y + maxParticipantObservationRange,
                        avatar.Location.Z + maxParticipantObservationRange,
                        participantLink.ObjectSendList);
                    participantLink.InitialObjectSendListFillDone = true;

                    SynchronizationBeginEventMessage synchronizationBeginEvent = (SynchronizationBeginEventMessage)
                        MessageFactory.Current.ReserveMessage(typeof (SynchronizationBeginEventMessage));
                    synchronizationBeginEvent.ObjectCount = (uint) participantLink.ObjectSendList.Count;
                    participantSessionDictionary[participantLink].Send(synchronizationBeginEvent);
                }

                // Process participant send list.
                if (participantLink.ObjectSendList.Count > 0)
                {
                    Session session = participantSessionDictionary[participantLink];
                    int n = 0;
                    while (participantLink.ObjectSendList.Count>0)
                    {
                        Guid objectId = participantLink.ObjectSendList[participantLink.ObjectSendList.Count-1];
                        CloudObject cloudObject = cloudCache.GetObject(objectId);
                        participantLink.ObjectSendList.RemoveAt(participantLink.ObjectSendList.Count - 1);

                        if (cloudObject != null)
                        {
                            PerceptionEventMessage perception =
                                (PerceptionEventMessage)
                                MessageFactory.Current.ReserveMessage(typeof(PerceptionEventMessage));

                            cloudObject.ToObjectFragment(perception.ObjectFragment);
                            session.Send(perception);
                            participantLink.ObservedObjects[cloudObject.ObjectId] = DateTime.Now;
                            n++;
                        }
                        if (n > 10)
                        {
                            break;
                        }
                    }

                    if(participantLink.ObjectSendList.Count==0)
                    {
                        SynchronizationEndEventMessage synchronizationEndEvent = (SynchronizationEndEventMessage)
                            MessageFactory.Current.ReserveMessage(typeof(SynchronizationEndEventMessage));
                        participantSessionDictionary[participantLink].Send(synchronizationEndEvent);
                    }
                }

                // Update participant observed objects
                if (avatar!=null)
                {
                    HashSet<Guid> objectsInRage = new HashSet<Guid>();
                    cloudCache.GetObjectIds(
                        avatar.Location.X - maxParticipantObservationRange,
                        avatar.Location.Y - maxParticipantObservationRange,
                        avatar.Location.Z - maxParticipantObservationRange,
                        avatar.Location.X + maxParticipantObservationRange,
                        avatar.Location.Y + maxParticipantObservationRange,
                        avatar.Location.Z + maxParticipantObservationRange,
                        objectsInRage);
                    UpdateObservedObjects(participantSessionDictionary[participantLink], participantLink.ObservedObjects,
                                          objectsInRage, new MsdVector3f(),false);
                }
            }

            balancer.Process();

        }

        #endregion

        #region Allowed Remote Hub Address Management

        public void AddAllowedRemoteHubAddress(string address)
        {
            allowedRemoteHubAddresses.Add(address);
        }

        public void RemoveAllowedRemoteHubAddress(string address)
        {
            allowedRemoteHubAddresses.Remove(address);
        }

        #endregion

        #region BubbleLink Management

        public Dictionary<Guid, BubbleLink> GetBubbleLinks()
        {
            return bubbleLinks;
        }

        public BubbleLink GetBubbleLink(Guid remoteBubbleId)
        {
            if (bubbleLinks.ContainsKey(remoteBubbleId))
            {
                return bubbleLinks[remoteBubbleId];
            }
            else
            {
                return null;
            }
        }

        public BubbleLink GetBubbleLink(Session session)
        {
            if (sessionBubbleLinkDictionary.ContainsKey(session))
            {
                return sessionBubbleLinkDictionary[session];
            }
            else
            {
                return null;
            }

        }

        public void AddBubbleLink(BubbleLink bubbleLink)
        {
            bubbleLinks.Add(bubbleLink.RemoteBubbleId, bubbleLink);
        }

        public void RemoveBubbleLink(BubbleLink bubbleLink)
        {
            bubbleLinks.Remove(bubbleLink.RemoteBubbleId);
        }

        public bool IsInPerceptionRange(BubbleLink bubbleLink, MsdVector3f location, float range)
        {
            return bubbleLink.IsConnected&&
                MathUtil.Distance(location, bubbleLink.RemoteBubbleCenter) <
                range + bubbleLink.RemoteBubblePerceptionRange;
        }

        public Session GetBubbleLinkSession(BubbleLink bubbleLink)
        {
            return bubbleLinkSessionDictionary[bubbleLink];
        }

        #endregion

        #region ParticipantLink Management

        public Dictionary<Guid, ParticipantLink> GetParticipants()
        {
            return participantLinks;
        }

        public ParticipantLink GetParticipant(Guid participantId)
        {
            if (participantLinks.ContainsKey(participantId))
            {
                return participantLinks[participantId];
            }
            else
            {
                return null;
            }
        }

        public Session GetParticipantSession(ParticipantLink participant)
        {
            if (participantSessionDictionary.ContainsKey(participant))
            {
                return participantSessionDictionary[participant];
            }
            else
            {
                return null;
            }
        }


        public ParticipantLink GetParticipant(Session session)
        {
            if (sessionParticipantLinkDictionary.ContainsKey(session))
            {
                return sessionParticipantLinkDictionary[session];
            }
            else
            {
                return null;
            }
        }

        public void DisconnectParticipant(Guid participantId)
        {
            if (participantLinks.ContainsKey(participantId))
            {
                ParticipantLink participantLink = participantLinks[participantId];
                foreach (Session session in sessionParticipantLinkDictionary.Keys)
                {
                    if (sessionParticipantLinkDictionary[session] == participantLink)
                    {
                        service.Server.Disconnect(session);
                    }
                }
            }
        }

        public bool IsInPerceptionRange(ParticipantLink participantLink, MsdVector3f location, float range)
        {
            CloudObject avatar = cloudCache.GetObject(participantLink.AvatarId);
            return avatar != null && MathUtil.Distance(location, avatar.Location) <
                       range + maxParticipantObservationRange;
        }

        #endregion

        #region Inter Bubble Message Handlers

        private bool OnBubbleConnectAuthorize(Session session, AttachRequestMessage message)
        {
            BubbleFragment remoteBubbleFragment = message.SourceBubbleFragment;

            LogUtil.Info("Incoming bubble link authorize. - RemoteHub: " + session.RemoteEndPoint.Address.ToString() + ":" + session.RemoteEndPoint.Port + " Remote Bubble: " + remoteBubbleFragment.BubbleName + " (" + remoteBubbleFragment.BubbleId + ")");

            // return false if link is explicitly disabled or already connected.
            if (bubbleLinks.ContainsKey(message.SourceBubbleFragment.BubbleId))
            {
                BubbleLink bubbleLink = bubbleLinks[message.SourceBubbleFragment.BubbleId];
                if (!bubbleLink.IsEnabled)
                {
                    LogUtil.Info("Incoming bubble link was rejected as link is disabled - " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteHubAddress + ":" + bubbleLink.RemoteHubPort + "/" + bubbleLink.RemoteBubbleId + ")");
                    return false;
                }
                if (bubbleLink.IsConnected)
                {
                    LogUtil.Info("Incoming bubble link was rejected as link is already connected - " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteHubAddress + ":" + bubbleLink.RemoteHubPort + "/" + bubbleLink.RemoteBubbleId + ")");
                    return false;
                }
                if (bubbleLink.IsConnecting&&BubbleId.ToString().CompareTo(bubbleLink.RemoteBubbleId.ToString())<0)
                {
                    LogUtil.Info("Incoming bubble link was rejected as link is already connecting and this bubbles id first in sort order. - " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteHubAddress + ":" + bubbleLink.RemoteHubPort + "/" + bubbleLink.RemoteBubbleId + ")");
                    return false;
                }
            }

            if (!allowedRemoteHubAddresses.Contains(session.RemoteEndPoint.Address.ToString()))
            {
                LogUtil.Info("Incoming bubble link was rejected as remote peer is not allowed remote hub address list - " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + message.SourceBubbleFragment.BubbleName + " (" + message.SourceBubbleFragment.BubbleAddress + ":" + message.SourceBubbleFragment.BubblePort + "/" + message.SourceBubbleFragment.BubbleId + ")");
                return false;
            }


            if (!((MathUtil.Length(message.SourceBubbleFragment.BubbleCenter) < BubblePerceptionRange + message.SourceBubbleFragment.BubbleRange ||
                MathUtil.Length(message.SourceBubbleFragment.BubbleCenter) < BubbleRange + message.SourceBubbleFragment.BubblePerceptionRange)))
            {
                LogUtil.Info("Incoming bubble link was rejected as neither of the linked bubbles can observe each others volume - " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + message.SourceBubbleFragment.BubbleName + " (" + message.SourceBubbleFragment.BubbleAddress + ":" + message.SourceBubbleFragment.BubblePort + "/" + message.SourceBubbleFragment.BubbleId + ")");
                return false;
            }

            LogUtil.Info("Incoming bubble link was accepted - " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + message.SourceBubbleFragment.BubbleName + " (" + message.SourceBubbleFragment.BubbleAddress + ":" + message.SourceBubbleFragment.BubblePort + "/" + message.SourceBubbleFragment.BubbleId + ")");
            return true;
        }

        private void OnBubbleConnected(Session session, Message message)
        {
            BubbleFragment remoteBubbleFragment;
            string remoteProgram;
            byte remoteProgramMajorVersion;
            byte remoteProgramMinorVersion;
            byte remoteProtocolMajorVersion;
            byte remoteProtocolMinorVersion;
            uint remoteProtocolSourceRevision;

            if (message.GetType() == typeof(AttachRequestMessage))
            {
                remoteBubbleFragment = ((AttachRequestMessage)message).SourceBubbleFragment;
                remoteProgram = ((AttachRequestMessage)message).ProgramName;
                remoteProgramMajorVersion = ((AttachRequestMessage)message).ProgramMajorVersion;
                remoteProgramMinorVersion = ((AttachRequestMessage)message).ProgramMinorVersion;
                remoteProtocolMajorVersion = ((AttachRequestMessage)message).ProtocolMajorVersion;
                remoteProtocolMinorVersion = ((AttachRequestMessage)message).ProtocolMinorVersion;
                remoteProtocolSourceRevision = ((AttachRequestMessage)message).ProtocolSourceRevision;
            }
            else
            {
                remoteBubbleFragment = ((AttachResponseMessage)message).TargetBubbleFragment;
                remoteProgram = ((AttachResponseMessage)message).ProgramName;
                remoteProgramMajorVersion = ((AttachResponseMessage)message).ProgramMajorVersion;
                remoteProgramMinorVersion = ((AttachResponseMessage)message).ProgramMinorVersion;
                remoteProtocolMajorVersion = ((AttachResponseMessage)message).ProtocolMajorVersion;
                remoteProtocolMinorVersion = ((AttachResponseMessage)message).ProtocolMinorVersion;
                remoteProtocolSourceRevision = ((AttachResponseMessage)message).ProtocolSourceRevision;
            }

            if (!bubbleLinks.ContainsKey(remoteBubbleFragment.BubbleId))
            {
                BubbleLink newBubbleLink = new BubbleLink();
                newBubbleLink.RemoteBubbleId = remoteBubbleFragment.BubbleId;
                newBubbleLink.IsEnabled = true;
                AddBubbleLink(newBubbleLink);
            }

            BubbleLink bubbleLink = bubbleLinks[remoteBubbleFragment.BubbleId];
            bubbleLink.RemoteBubbleOwnerId = remoteBubbleFragment.OwnerId;
            bubbleLink.RemoteBubbleName = remoteBubbleFragment.BubbleName;
            bubbleLink.RemoteBubbleCenter.X = remoteBubbleFragment.BubbleCenter.X;
            bubbleLink.RemoteBubbleCenter.Y = remoteBubbleFragment.BubbleCenter.Y;
            bubbleLink.RemoteBubbleCenter.Z = remoteBubbleFragment.BubbleCenter.Z;
            bubbleLink.RemoteBubbleRange = remoteBubbleFragment.BubbleRange;
            bubbleLink.RemoteBubblePerceptionRange = remoteBubbleFragment.BubblePerceptionRange;
            bubbleLink.RemoteBubbleAssetCacheUrl = remoteBubbleFragment.BubbleAssetCacheUrl;
            bubbleLink.RemoteHubAddress = session.RemoteEndPoint.Address.ToString();
            bubbleLink.RemoteHubPort = session.RemoteEndPoint.Port;
            bubbleLink.RemoteHubProgram = remoteProgram;
            bubbleLink.RemoteHubProgramMajorVersion = remoteProgramMajorVersion;
            bubbleLink.RemoteHubProgramMinorVersion = remoteProgramMinorVersion;
            bubbleLink.RemoteProtocolMajorVersion = remoteProtocolMajorVersion;
            bubbleLink.RemoteProtocolMinorVersion = remoteProtocolMinorVersion;
            bubbleLink.RemoteProtocolSourceRevision = remoteProtocolSourceRevision;

            bubbleLink.LastConnect = DateTime.Now;
            bubbleLink.IsConnected = true;
            bubbleLink.IsConnecting = false;

            sessionBubbleLinkDictionary.Add(session, bubbleLink);
            bubbleLinkSessionDictionary.Add(bubbleLink, session);

            LogUtil.Info("Bubble link connected - " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteHubAddress + ":" + bubbleLink.RemoteHubPort + "/" + bubbleLink.RemoteBubbleId + ")");

            // Requesting other linked bubbles of connected remote bubble..
            ListBubblesRequest listBubblesRequest = (ListBubblesRequest)MessageFactory.Current.ReserveMessage(typeof(ListBubblesRequest));
            listBubblesRequest.ListType = ListBubblesRequest.ListTypeLinked;
            session.Send(listBubblesRequest);
            //LogUtil.Info("Requesting other linked bubbles of connected remote bubble - RemoteHub: " + bubbleLink.RemoteHubAddress + ":" + bubbleLink.RemoteHubPort + " Remote Bubble: " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteBubbleId + ")");

            // Adding existing object ids in perception range to send list.
            cloudCache.GetObjectIds(
                bubbleLink.RemoteBubbleCenter.X - bubbleLink.RemoteBubblePerceptionRange,
                bubbleLink.RemoteBubbleCenter.Y - bubbleLink.RemoteBubblePerceptionRange,
                bubbleLink.RemoteBubbleCenter.Z - bubbleLink.RemoteBubblePerceptionRange,
                bubbleLink.RemoteBubbleCenter.X + bubbleLink.RemoteBubblePerceptionRange,
                bubbleLink.RemoteBubbleCenter.Y + bubbleLink.RemoteBubblePerceptionRange,
                bubbleLink.RemoteBubbleCenter.Z + bubbleLink.RemoteBubblePerceptionRange,
                bubbleLink.ObjectSendList);
        }

        private void OnBubbleConnectFailure(Session session, Message message)
        {
            BubbleFragment remoteBubbleFragment;
            if (message.GetType() == typeof(AttachRequestMessage))
            {
                remoteBubbleFragment = ((AttachRequestMessage)message).SourceBubbleFragment;
                LogUtil.Info("Incoming bubble link was rejected by local bubble - " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + remoteBubbleFragment.BubbleName + " (" + remoteBubbleFragment.BubbleAddress + ":" + remoteBubbleFragment.BubblePort + "/" + remoteBubbleFragment.BubbleId + ")");
            }
            else
            {
                remoteBubbleFragment = ((AttachResponseMessage)message).TargetBubbleFragment;
                LogUtil.Info("Outgoing bubble link rejected by remote bubble - " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + remoteBubbleFragment.BubbleName + " (" + remoteBubbleFragment.BubbleAddress + ":" + remoteBubbleFragment.BubblePort + "/" + remoteBubbleFragment.BubbleId + ")");
            }
            if(sessionBubbleLinkDictionary.ContainsKey(session))
            {
                sessionBubbleLinkDictionary[session].IsConnecting = false;
            }

        }

        private void OnBubbleDisconnected(Session session)
        {
            if (sessionBubbleLinkDictionary.ContainsKey(session))
            {
                BubbleLink bubbleLink = sessionBubbleLinkDictionary[session];
                bubbleLink.LastDisconnect = DateTime.Now;
                bubbleLink.IsConnected = false;

                sessionBubbleLinkDictionary.Remove(session);
                bubbleLinkSessionDictionary.Remove(bubbleLink);

                bubbleLink.ObjectSendList.Clear();
                bubbleLink.ObservedObjects.Clear();

                LogUtil.Info("Bubble link disconnected - " + BubbleName + " (" + service.Hub.HubAddress + ":" + service.Hub.HubPort + ") to " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteHubAddress + ":" + bubbleLink.RemoteHubPort + "/" + bubbleLink.RemoteBubbleId + ")");
            }
        }

        private void OnBubbleListRequested(Session session, ListBubblesRequest message)
        {
            BubbleLink bubbleLink = null;
            if(sessionBubbleLinkDictionary.ContainsKey(session))
            {
                bubbleLink = sessionBubbleLinkDictionary[session];
            }

            if (message.ListType == ListBubblesRequest.ListTypeLinked &&
                !sessionBubbleLinkDictionary.ContainsKey(session) &&
                !sessionParticipantLinkDictionary.ContainsKey(session))
            {
                // If link is not properly formed from bubble or participant then linked bubbles can not be listed.
                return;
            }

            //LogUtil.Info("Bubble list requested by remote bubble - RemoteHub: " + bubbleLink.RemoteHubAddress + ":" + bubbleLink.RemoteHubPort + " Remote Bubble: " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteBubbleId + ")");

            ListBubblesResponse listBubblesResponse = (ListBubblesResponse)MessageFactory.Current.ReserveMessage(typeof(ListBubblesResponse));
            if (message.ListType == ListBubblesRequest.ListTypeLinked)
            {
                foreach (BubbleLink currentBubbleLink in bubbleLinks.Values)
                {
                    if (currentBubbleLink.IsEnabled)
                    {
                        BubbleFragment bubbleFragment = new BubbleFragment();
                        bubbleFragment.BubbleId = currentBubbleLink.RemoteBubbleId;
                        bubbleFragment.BubbleName = currentBubbleLink.RemoteBubbleName;
                        bubbleFragment.BubbleAssetCacheUrl = currentBubbleLink.RemoteBubbleAssetCacheUrl;
                        bubbleFragment.BubbleRange = currentBubbleLink.RemoteBubbleRange;
                        bubbleFragment.BubblePerceptionRange = currentBubbleLink.RemoteBubblePerceptionRange;
                        bubbleFragment.BubbleAddress = currentBubbleLink.RemoteHubAddress;
                        bubbleFragment.BubblePort = (uint)currentBubbleLink.RemoteHubPort;
                        // If request was done by another bubble transforming correct relative coordinates.
                        if (bubbleLink != null)
                        {
                            bubbleFragment.BubbleCenter.X = currentBubbleLink.RemoteBubbleCenter.X -
                                                            bubbleLink.RemoteBubbleCenter.X;
                            bubbleFragment.BubbleCenter.Y = currentBubbleLink.RemoteBubbleCenter.Y -
                                                            bubbleLink.RemoteBubbleCenter.Y;
                            bubbleFragment.BubbleCenter.Z = currentBubbleLink.RemoteBubbleCenter.Z -
                                                            bubbleLink.RemoteBubbleCenter.Z;
                        }
                        else
                        {
                            bubbleFragment.BubbleCenter.X = currentBubbleLink.RemoteBubbleCenter.X;
                            bubbleFragment.BubbleCenter.Y = currentBubbleLink.RemoteBubbleCenter.Y;
                            bubbleFragment.BubbleCenter.Z = currentBubbleLink.RemoteBubbleCenter.Z;
                        }
                        bubbleFragment.OwnerId = currentBubbleLink.RemoteBubbleOwnerId;
                        listBubblesResponse.AddBubbleFragment(bubbleFragment);
                    }
                }
            }
            if (message.ListType == ListBubblesRequest.ListTypeHosted)
            {
                foreach (CloudBubble bubble in service.Bubbles)
                {
                    BubbleFragment bubbleFragment = new BubbleFragment();
                    bubbleFragment.BubbleId = bubble.BubbleId;
                    bubbleFragment.BubbleName = bubble.BubbleName;
                    bubbleFragment.BubbleAssetCacheUrl = service.Hub.HubAssetCacheUrl;
                    bubbleFragment.BubbleRange = bubble.BubbleRange;
                    bubbleFragment.BubblePerceptionRange = bubble.BubblePerceptionRange;
                    bubbleFragment.BubbleAddress = service.Hub.HubAddress;
                    bubbleFragment.BubblePort = (uint)service.Hub.HubPort;
                    // These are 0 as hosted bubbles or may not be linked and thus coordinates are not defined in this list.
                    bubbleFragment.BubbleCenter.X = 0;
                    bubbleFragment.BubbleCenter.Y = 0;
                    bubbleFragment.BubbleCenter.Z = 0;
                    bubbleFragment.OwnerId = bubble.OwnerId;
                    listBubblesResponse.AddBubbleFragment(bubbleFragment);
                }
            }
            session.Send(listBubblesResponse);
        }

        private void OnBubbleListReceived(Session session, ListBubblesResponse message)
        {
            //LogUtil.Info("Received bubble list - RemoteHub: " + bubbleLink.RemoteHubAddress + ":" + bubbleLink.RemoteHubPort + " Remote Bubble: " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteBubbleId + ")");

            foreach (BubbleFragment bubbleFragment in message.BubbleFragments)
            {
                if (!bubbleLinks.ContainsKey(bubbleFragment.BubbleId) && BubbleId != bubbleFragment.BubbleId &
                    ((MathUtil.Length(bubbleFragment.BubbleCenter) < BubblePerceptionRange + bubbleFragment.BubbleRange ||
                    MathUtil.Length(bubbleFragment.BubbleCenter) < BubbleRange + bubbleFragment.BubblePerceptionRange))
                    )
                {
                    BubbleLink newBubbleLink = new BubbleLink();
                    newBubbleLink.RemoteHubAddress = bubbleFragment.BubbleAddress;
                    newBubbleLink.RemoteHubPort = (int)bubbleFragment.BubblePort;
                    newBubbleLink.RemoteBubbleId = bubbleFragment.BubbleId;
                    newBubbleLink.RemoteBubbleName = bubbleFragment.BubbleName;
                    newBubbleLink.RemoteBubbleCenter.X = bubbleFragment.BubbleCenter.X;
                    newBubbleLink.RemoteBubbleCenter.Y = bubbleFragment.BubbleCenter.Y;
                    newBubbleLink.RemoteBubbleCenter.Z = bubbleFragment.BubbleCenter.Z;
                    newBubbleLink.IsEnabled = true;
                    newBubbleLink.IsInitiator = true;
                    newBubbleLink.ScheduledConnect = DateTime.Now;

                    AddBubbleLink(newBubbleLink);

                    //LogUtil.Info("Dynamic bubble link created - RemoteHub: " + newBubbleLink.RemoteHubAddress + ":" + newBubbleLink.RemoteHubPort + " Remote Bubble: " + newBubbleLink.RemoteBubbleName + " (" + newBubbleLink.RemoteBubbleId + ") for Local Bubble: "+BubbleName+" ("+BubbleId+")");                
                }
            }

        }

        private void OnBubbleMessageReceived(Session session, Message message)
        {
            if (!sessionBubbleLinkDictionary.ContainsKey(session))
            {
                LogUtil.Warn("Message received from not properly connected bubble - type code: " + message.TypeCode);
                return;
            }

            BubbleLink bubbleLink = sessionBubbleLinkDictionary[session];

            if (message.GetType() == typeof(PerceptionEventMessage))
            {
                PerceptionEventMessage perceptionEventMessage = (PerceptionEventMessage)message;
                if (cloudCache.GetObject(perceptionEventMessage.ObjectFragment.ObjectId) != null &&
                    cloudCache.GetObject(perceptionEventMessage.ObjectFragment.ObjectId).BubbleId != bubbleLink.RemoteBubbleId)
                {
                    LogUtil.Warn("Object perception received from wrong bubble - Object: " + perceptionEventMessage.ObjectFragment.ObjectId + " Bubble: " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteBubbleId + ")");
                    return;
                }
                else
                {
                    CloudObject cloudObject = cloudCache.GetObject(perceptionEventMessage.ObjectFragment.ObjectId);
                    if (cloudObject == null)
                    {
                        cloudObject = new CloudObject();
                    }
                    cloudObject.FromObjectFragment(bubbleLink.RemoteBubbleId, perceptionEventMessage.ObjectFragment);
                    cloudCache.PutObject(cloudObject, true);
                }
            }

            if (message.GetType() == typeof(DisappearanceEventMessage))
            {
                DisappearanceEventMessage disappearanceEventMessage = (DisappearanceEventMessage)message;
                CloudObject cloudObject = cloudCache.GetObject(bubbleLink.RemoteBubbleId, disappearanceEventMessage.ObjectIndex);
                if (cloudObject == null)
                {
                    LogUtil.Warn("Object disappearance for non existing object. - Object: " + cloudObject + " Bubble: " + bubbleLink.RemoteBubbleName + " (" + bubbleLink.RemoteBubbleId + ")");
                    return;
                }
                else
                {
                    cloudCache.RemoveObject(cloudObject.ObjectId);
                }
            }

            if (message.GetType() == typeof (ActionEventMessage))
            {
                OnActionEvent(bubbleLink.RemoteBubbleId, Guid.Empty, (ActionEventMessage) message);
            }

            if (message.GetType() == typeof(InteractRequestMessage))
            {
                OnInteractRequest(bubbleLink.RemoteBubbleId, Guid.Empty, (InteractRequestMessage)message);
            }

            if (message.GetType() == typeof(InteractResponseMessage))
            {
                OnInteractResponse(bubbleLink.RemoteBubbleId, Guid.Empty, (InteractResponseMessage)message);
            }

            if (message.GetType() == typeof(MovementEventMessage))
            {
                OnMovementEvent(bubbleLink.RemoteBubbleId, Guid.Empty, (MovementEventMessage)message);
            }

        }

        #endregion

        #region Participant Connectivity Message Handlers

        private bool OnParticipantConnectAuthorize(Session session, JoinRequestMessage message, out Guid participantId, out Guid avatarId)
        {
            foreach (ParticipantLink participant in participantLinks.Values)
            {
                if (participant.ParticipantIdentifier == message.ParticipantIdentifier)
                {
                    participantId = participant.ParticipantId;
                    avatarId = participant.AvatarId;
                    return false;
                }
            }
            participantId = Guid.NewGuid();
            if (message.AvatarId != Guid.Empty)
            {
                avatarId = message.AvatarId;
            }
            else
            {
                avatarId = Guid.NewGuid();
            }
            return true;
        }

        private void OnParticipantConnected(Session session, JoinRequestMessage message, Guid participantId, Guid avatarId)
        {
            ParticipantLink participantLink = new ParticipantLink();
            participantLink.AvatarId = avatarId;
            participantLink.ParticipantId = participantId;
            participantLink.ParticipantIdentifier = message.ParticipantIdentifier;
            participantLink.ParticipantRealTime = message.ParticipantRealTime;
            participantLink.ProgramName = message.ProgramName;
            participantLink.ProgramMajorVersion = message.ProgramMajorVersion;
            participantLink.ProgramMinorVersion = message.ProgramMinorVersion;
            participantLink.ProtocolMajorVersion = message.ProtocolMajorVersion;
            participantLink.ProtocolMinorVersion = message.ProtocolMinorVersion;
            participantLink.ProtocolSourceRevision = message.ProtocolSourceRevision;
            participantLinks.Add(participantLink.ParticipantId, participantLink);
            sessionParticipantLinkDictionary.Add(session, participantLink);
            participantSessionDictionary.Add(participantLink, session);

            LogUtil.Info("Participant connected - " + participantLink.ParticipantIdentifier + " to " + BubbleName);

        }

        private void OnParticipantDisconnected(Session session)
        {
            if (sessionParticipantLinkDictionary.ContainsKey(session))
            {
                ParticipantLink participantLink = sessionParticipantLinkDictionary[session];
                participantLinks.Remove(participantLink.ParticipantId);
                sessionParticipantLinkDictionary.Remove(session);
                participantSessionDictionary.Remove(participantLink);
                CloudParticipantDisconnected(this, participantLink.ParticipantId);
                LogUtil.Info("Participant disconnected - " + participantLink.ParticipantIdentifier + " to " + BubbleName);
            }
        }

        private void OnParticipantMessageReceived(Session session, Message message)
        {
            if (!sessionParticipantLinkDictionary.ContainsKey(session))
            {
                LogUtil.Warn("Message received from not properly connected participant - type code: " + message.TypeCode);
                return;
            }

            ParticipantLink participantLink = sessionParticipantLinkDictionary[session];

            if (message.GetType() == typeof (InjectRequestMessage))
            {
                InjectRequestMessage injectRequest = (InjectRequestMessage) message;
                InjectResponseMessage injectResponse =
                    (InjectResponseMessage) MessageFactory.Current.ReserveMessage(typeof (InjectResponseMessage));
                injectResponse.RequestMessageId = injectRequest.MessageId;

                // Mark the object to belong to the injecter
                injectRequest.ObjectFragment.OwnerId = participantLink.ParticipantId;

                if (cloudCache.GetObject(injectRequest.ObjectFragment.ObjectId) != null &&
                    cloudCache.GetObject(injectRequest.ObjectFragment.ObjectId).OwnerId != participantLink.ParticipantId)
                {
                    LogUtil.Warn("Object injection received from wrong participant - Object: " +
                                 injectRequest.ObjectFragment.ObjectId + " Participant: " + participantLink.ParticipantIdentifier +
                                 " (" + participantLink.ParticipantId + ")");
                    injectResponse.FailureCode = MxpResponseCodes.UNAUTHORIZED_OPERATION;
                }
                else
                {
                    CloudObject cloudObject = cloudCache.GetObject(injectRequest.ObjectFragment.ObjectId);
                    if (cloudObject == null)
                    {
                        cloudObject = new CloudObject();
                    }
                    cloudObject.FromObjectFragment(BubbleId, injectRequest.ObjectFragment);
                    cloudCache.PutObject(cloudObject, true);
                }
                session.Send(injectResponse);
            }

            if (message.GetType() == typeof (ModifyRequestMessage))
            {
                ModifyRequestMessage modifyRequest = (ModifyRequestMessage) message;
                ModifyResponseMessage modifyResponse =
                    (ModifyResponseMessage) MessageFactory.Current.ReserveMessage(typeof (ModifyResponseMessage));
                modifyResponse.RequestMessageId = modifyRequest.MessageId;

                // Mark the object to belong to the injecter
                modifyRequest.ObjectFragment.OwnerId = participantLink.ParticipantId;

                CloudObject cloudObject = cloudCache.GetObject(modifyRequest.ObjectFragment.ObjectId);
                if (cloudObject != null &&
                    (cloudObject.OwnerId != participantLink.ParticipantId||
                    cloudObject.BubbleId != BubbleId))
                {
                    LogUtil.Warn("Object modify received from wrong participant or through wrong bubble - Object: " +
                                 modifyRequest.ObjectFragment.ObjectId + " Participant: " + participantLink.ParticipantIdentifier +
                                 " (" + participantLink.ParticipantId + ")");
                    modifyResponse.FailureCode = MxpResponseCodes.UNAUTHORIZED_OPERATION;
                }
                else
                {
                    /*if (cloudObject == null)
                    {
                        cloudObject = new CloudObject();
                    }*/
                    if (cloudObject != null)
                    {
                        cloudObject.FromObjectFragment(BubbleId, modifyRequest.ObjectFragment);
                        cloudCache.PutObject(cloudObject, true);

                        // After modification from participant do evaluation whether object should be transfered to another bubble
                        balancer.EvaluateHandoverNeed(cloudObject);
                        modifyResponse.FailureCode = MxpResponseCodes.SUCCESS;
                    }
                    else
                    {
                        modifyResponse.FailureCode = MxpResponseCodes.UNKNOWN_ID;
                    }
                }
                session.Send(modifyResponse);
            }

            if (message.GetType() == typeof (EjectRequestMessage))
            {
                EjectRequestMessage ejectRequest = (EjectRequestMessage) message;
                EjectResponseMessage ejectResponse =
                    (EjectResponseMessage) MessageFactory.Current.ReserveMessage(typeof (EjectResponseMessage));

                ejectResponse.RequestMessageId = ejectRequest.MessageId;
                if (cloudCache.GetObject(ejectRequest.ObjectId) == null)
                {
                    LogUtil.Warn("Object eject received for non existing object. - Object: " + ejectRequest.ObjectId +
                                 " Participant: " + participantLink.ParticipantIdentifier + " (" + participantLink.ParticipantId + ")");
                }
                else if (cloudCache.GetObject(ejectRequest.ObjectId).OwnerId != participantLink.ParticipantId)
                {
                    LogUtil.Warn("Object eject received from wrong participant - Object: " + ejectRequest.ObjectId +
                                 " Participant: " + participantLink.ParticipantIdentifier + " (" + participantLink.ParticipantId + ")");
                    ejectResponse.FailureCode = MxpResponseCodes.UNAUTHORIZED_OPERATION;
                }
                else
                {
                    cloudCache.RemoveObject(ejectRequest.ObjectId);
                }
                session.Send(ejectResponse);
            }

            if (message.GetType() == typeof (ActionEventMessage))
            {
                OnActionEvent(Guid.Empty, participantLink.ParticipantId, (ActionEventMessage)message);
            }

            if (message.GetType() == typeof(InteractRequestMessage))
            {
                OnInteractRequest(Guid.Empty, participantLink.ParticipantId, (InteractRequestMessage)message);
            }

            if (message.GetType() == typeof(InteractResponseMessage))
            {
                OnInteractResponse(Guid.Empty, participantLink.ParticipantId, (InteractResponseMessage)message);
            }

            if (message.GetType() == typeof(MovementEventMessage))
            {
                OnMovementEvent(Guid.Empty, participantLink.ParticipantId, (MovementEventMessage)message);
            }

            if(message.GetType() == typeof(ListBubblesRequest))
            {
                OnBubbleListRequested(session, (ListBubblesRequest)message);
            }
        }

        #endregion

        #region Cache Event Handlers

        public void OnCacheObjectPut(CloudObject cloudObject)
        {

            /*
            if (cloudObject.BubbleId == BubbleId) // Broadcasting only local objects to other bubbles
            {
                foreach (BubbleLink bubbleLink in bubbleLinks.Values)
                {
                    UpdateBubbleLinkObservationList(bubbleLink, cloudObject, true);
                }
            }

            foreach (Participant participant in participants.Values)
            {
                UpdateParticiantObservationList(participant, cloudObject, true);
            }
            */
        }

        public void OnCacheObjectRemoved(CloudObject cloudObject)
        {
            // Have to clean up observed objects immediately as disappearance uses ObjectIndex which is not available later.

            if (cloudObject.BubbleId == BubbleId) // Broadcasting only local objects to other bubbles
            {
                foreach (BubbleLink bubbleLink in bubbleLinks.Values)
                {

                    if (!bubbleLink.IsConnected)
                    {
                        continue;
                    }
                    Session session = bubbleLinkSessionDictionary[bubbleLink];

                    if (bubbleLink.ObservedObjects.ContainsKey(cloudObject.ObjectId))
                    {
                        DisappearanceEventMessage disapperance =
                            (DisappearanceEventMessage)
                            MessageFactory.Current.ReserveMessage(typeof(DisappearanceEventMessage));
                        disapperance.ObjectIndex = cloudObject.LocalObjectIndex;
                        session.Send(disapperance);
                        bubbleLink.ObservedObjects.Remove(cloudObject.ObjectId);
                    }
                }
            }

            foreach (ParticipantLink participant in participantLinks.Values)
            {
                Session session = participantSessionDictionary[participant];
                if (participant.ObservedObjects.ContainsKey(cloudObject.ObjectId))
                {
                    DisappearanceEventMessage disapperance =
                        (DisappearanceEventMessage)
                        MessageFactory.Current.ReserveMessage(typeof(DisappearanceEventMessage));
                    disapperance.ObjectIndex = cloudObject.LocalObjectIndex;
                    session.Send(disapperance);
                    participant.ObservedObjects.Remove(cloudObject.ObjectId);
                }
            }

        }

        #endregion

        #region Object Observation List Management

        /// <summary>
        /// Updates observation list with objects in range. Disappearances are sent to removed objects.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="observationList"></param>
        /// <param name="objectsInRange"></param>
        public void UpdateObservedObjects(Session session, IDictionary<Guid, DateTime> observationList, HashSet<Guid> objectsInRange, MsdVector3f peerCenter, bool sendOnlyLocal)
        {
            IList<Guid> updateObjects = new List<Guid>();
            IList<Guid> removeObjects = new List<Guid>();

            foreach (Guid objectId in observationList.Keys)
            {
                if (objectsInRange.Contains(objectId))
                {
                    CloudObject cloudObject = cloudCache.GetObject(objectId);
                    // Send perception if object has been changed since last perception.
                    // TODO Send update also periodically as keepalive.
                    if (cloudObject.LastUpdated > observationList[objectId])
                    {
                        updateObjects.Add(objectId);
                    }
                    objectsInRange.Remove(objectId);
                }
                else
                {
                    removeObjects.Add(objectId);
                }
            }

            foreach (Guid objectId in updateObjects)
            {
                PerceptionEventMessage perception =
                    (PerceptionEventMessage)MessageFactory.Current.ReserveMessage(typeof(PerceptionEventMessage));
                cloudCache.GetObject(objectId).ToObjectFragment(perception.ObjectFragment);
                perception.ObjectFragment.Location.X = perception.ObjectFragment.Location.X - peerCenter.X;
                perception.ObjectFragment.Location.Y = perception.ObjectFragment.Location.Y - peerCenter.Y;
                perception.ObjectFragment.Location.Z = perception.ObjectFragment.Location.Z - peerCenter.Z;
                session.Send(perception);
                observationList[objectId] = DateTime.Now;
            }

            // Remove objects
            foreach (Guid objectId in removeObjects)
            {
                observationList.Remove(objectId);
                DisappearanceEventMessage disappearance =
                    (DisappearanceEventMessage)
                    MessageFactory.Current.ReserveMessage(typeof(DisappearanceEventMessage));
                disappearance.ObjectIndex = cloudCache.GetObject(objectId).LocalObjectIndex;
                session.Send(disappearance);
            }

            // Add objects
            foreach (Guid objectId in objectsInRange)
            {
                CloudObject cloudObject = cloudCache.GetObject(objectId);
                if (!sendOnlyLocal || cloudObject.BubbleId == BubbleId)
                {
                    observationList.Add(objectId, DateTime.Now);
                    PerceptionEventMessage perception =
                        (PerceptionEventMessage)MessageFactory.Current.ReserveMessage(typeof(PerceptionEventMessage));
                    cloudCache.GetObject(objectId).ToObjectFragment(perception.ObjectFragment);
                    perception.ObjectFragment.Location.X = perception.ObjectFragment.Location.X - peerCenter.X;
                    perception.ObjectFragment.Location.Y = perception.ObjectFragment.Location.Y - peerCenter.Y;
                    perception.ObjectFragment.Location.Z = perception.ObjectFragment.Location.Z - peerCenter.Z;
                    session.Send(perception);
                }
            }

        }

        #endregion

        #region Event Message Handlers

        private bool AuthorizeAction(Guid sourceBubbleId,Guid sourceParticipantId, Guid objectId, CloudObject cloudObject)
        {
            if (sourceBubbleId != Guid.Empty && cloudObject.BubbleId != sourceBubbleId)
            {
                LogUtil.Warn("Object action received from non primary bubble - Object: " +
                             objectId + " Bubble: " + sourceBubbleId + ")");
                return false;
            }

            if (sourceParticipantId != Guid.Empty && cloudObject.OwnerId != sourceParticipantId)
            {
                LogUtil.Warn("Object action received from non owner participant - Object: " +
                             objectId + " Participant: " + sourceParticipantId + ")");
                return false;
            }
            return true;
        }

        public void OnMovementEvent(Guid sourceBubbleId, Guid sourceParticipantId, MovementEventMessage movementEvent)
        {
            CloudObject cloudObject = cloudCache.GetObject(sourceBubbleId != Guid.Empty ? sourceBubbleId : BubbleId, movementEvent.ObjectIndex);
            if (cloudObject == null)
            {
                LogUtil.Warn("Object action received for non existing object - Object: " + movementEvent.ObjectIndex + " Source Bubble: " + sourceBubbleId + " Source Participant: " + sourceParticipantId + ")");
                return;
            }
            Guid objectId = cloudObject.ObjectId;

            if (!AuthorizeAction(sourceBubbleId, sourceParticipantId, objectId, cloudObject))
            {
                return;
            }

            cloudObject.Location.X = movementEvent.Location.X;
            cloudObject.Location.Y = movementEvent.Location.Y;
            cloudObject.Location.Z = movementEvent.Location.Z;
            cloudObject.Orientation.X = movementEvent.Orientation.X;
            cloudObject.Orientation.Y = movementEvent.Orientation.Y;
            cloudObject.Orientation.Z = movementEvent.Orientation.Z;
            cloudObject.Orientation.W = movementEvent.Orientation.W;
            cloudCache.PutObject(cloudObject, false);

            {
                foreach (ParticipantLink participant in participantLinks.Values)
                {
                    if (participant.ObservedObjects.Keys.Contains(cloudObject.ObjectId))
                    {
                        MovementEventMessage movementMessage =
                            (MovementEventMessage)MessageFactory.Current.ReserveMessage(typeof(MovementEventMessage));
                        movementMessage.ObjectIndex = cloudObject.LocalObjectIndex;
                        movementMessage.Location.X = movementEvent.Location.X;
                        movementMessage.Location.Y = movementEvent.Location.Y;
                        movementMessage.Location.Z = movementEvent.Location.Z;
                        movementMessage.Orientation.X = movementEvent.Orientation.X;
                        movementMessage.Orientation.Y = movementEvent.Orientation.Y;
                        movementMessage.Orientation.Z = movementEvent.Orientation.Z;
                        movementMessage.Orientation.W = movementEvent.Orientation.W;

                        participantSessionDictionary[participant].Send(movementMessage);
                    }
                }

            }

            if (cloudObject.BubbleId == BubbleId) // Broadcasting only local objects to other bubbles
            {
                foreach (BubbleLink bubbleLink in bubbleLinks.Values)
                {
                    if (bubbleLink.ObservedObjects.Keys.Contains(cloudObject.ObjectId))
                    {
                        MovementEventMessage movementMessage =
                            (MovementEventMessage)MessageFactory.Current.ReserveMessage(typeof(MovementEventMessage));
                        movementMessage.ObjectIndex = cloudObject.LocalObjectIndex;
                        movementMessage.Location.X = movementEvent.Location.X;
                        movementMessage.Location.Y = movementEvent.Location.Y;
                        movementMessage.Location.Z = movementEvent.Location.Z;
                        movementMessage.Orientation.X = movementEvent.Orientation.X;
                        movementMessage.Orientation.Y = movementEvent.Orientation.Y;
                        movementMessage.Orientation.Z = movementEvent.Orientation.Z;
                        movementMessage.Orientation.W = movementEvent.Orientation.W;

                        movementMessage.Location.X = movementEvent.Location.X - bubbleLink.RemoteBubbleCenter.X;
                        movementMessage.Location.Y = movementEvent.Location.Y - bubbleLink.RemoteBubbleCenter.Y;
                        movementMessage.Location.Z = movementEvent.Location.Z - bubbleLink.RemoteBubbleCenter.Z;
                        bubbleLinkSessionDictionary[bubbleLink].Send(movementMessage);
                    }
                }

                // After movement event from participant do evaluation whether object should be transfered to another bubble
                balancer.EvaluateHandoverNeed(cloudObject);
            }


        }

        public void OnActionEvent(Guid sourceBubbleId,Guid sourceParticipantId, ActionEventMessage actionEvent)
        {
            Guid objectId = actionEvent.ActionFragment.SourceObjectId;
            CloudObject cloudObject = cloudCache.GetObject(objectId);
            if (cloudObject == null)
            {
                LogUtil.Warn("Object action received for non existing object - Object: " + objectId + " Source Bubble: "+sourceBubbleId+" Source Participant: " + sourceParticipantId + ")");
                return;
            }

            if(!AuthorizeAction(sourceBubbleId,sourceParticipantId,objectId,cloudObject))
            {
                return;
            }

            if (cloudObject.BubbleId == BubbleId) // Broadcasting only local objects to other bubbles
            {
                foreach (BubbleLink bubbleLink in bubbleLinks.Values)
                {
                    if (IsInPerceptionRange(bubbleLink, cloudObject.Location,actionEvent.ActionFragment.ObservationRadius))
                    {
                        ActionEventMessage actionMessage = (ActionEventMessage)MessageFactory.Current.ReserveMessage(typeof(ActionEventMessage));
                        actionMessage.ActionFragment.ActionName = actionEvent.ActionFragment.ActionName;
                        actionMessage.ActionFragment.SourceObjectId = actionEvent.ActionFragment.SourceObjectId;
                        actionMessage.ActionFragment.ObservationRadius = actionEvent.ActionFragment.ObservationRadius;
                        actionMessage.ActionFragment.ExtensionDialect = actionEvent.ActionFragment.ExtensionDialect;
                        actionMessage.ActionFragment.ExtensionDialectMajorVersion = actionEvent.ActionFragment.ExtensionDialectMajorVersion;
                        actionMessage.ActionFragment.ExtensionDialectMinorVersion = actionEvent.ActionFragment.ExtensionDialectMinorVersion;

                        byte[] extensionDataBuffer = new byte[actionEvent.ActionFragment.ExtensionLength];
                        Array.Copy(actionEvent.ActionFragment.GetExtensionData(), extensionDataBuffer, actionEvent.ActionFragment.ExtensionLength);
                        actionMessage.SetPayloadData(extensionDataBuffer);

                        bubbleLinkSessionDictionary[bubbleLink].Send(actionMessage);
                    }
                }
            }

            foreach (ParticipantLink participant in participantLinks.Values)
            {
                if (IsInPerceptionRange(participant, cloudObject.Location, actionEvent.ActionFragment.ObservationRadius))
                {
                    ActionEventMessage actionMessage = (ActionEventMessage)MessageFactory.Current.ReserveMessage(typeof(ActionEventMessage));
                    actionMessage.ActionFragment.ActionName = actionEvent.ActionFragment.ActionName;
                    actionMessage.ActionFragment.SourceObjectId = actionEvent.ActionFragment.SourceObjectId;
                    actionMessage.ActionFragment.ObservationRadius = actionEvent.ActionFragment.ObservationRadius;
                    actionMessage.ActionFragment.ExtensionDialect = actionEvent.ActionFragment.ExtensionDialect;
                    actionMessage.ActionFragment.ExtensionDialectMajorVersion = actionEvent.ActionFragment.ExtensionDialectMajorVersion;
                    actionMessage.ActionFragment.ExtensionDialectMinorVersion = actionEvent.ActionFragment.ExtensionDialectMinorVersion;

                    byte[] extensionDataBuffer = new byte[actionEvent.ActionFragment.ExtensionLength];
                    Array.Copy(actionEvent.ActionFragment.GetExtensionData(), extensionDataBuffer, actionEvent.ActionFragment.ExtensionLength);
                    actionMessage.SetPayloadData(extensionDataBuffer);

                    participantSessionDictionary[participant].Send(actionMessage);
                }
            }

        }

        public void OnInteractRequest(Guid sourceBubbleId, Guid sourceParticipantId, InteractRequestMessage originalRequest)
        {
            Guid objectId = originalRequest.InteractionFragment.SourceObjectId;
            
            if (objectId != Guid.Empty)
            {
                CloudObject cloudObject = cloudCache.GetObject(objectId);
                if (cloudObject == null)
                {
                    LogUtil.Warn("Object action received for non existing object - Object: " + objectId + " Source Bubble: " + sourceBubbleId + " Source Participant: " + sourceParticipantId + ")");
                    return;
                }

                if (!AuthorizeAction(sourceBubbleId, sourceParticipantId, objectId, cloudObject))
                {
                    return;
                }
            }

            if (participantLinks.ContainsKey(originalRequest.InteractionFragment.TargetParticipantId))
            {
                ParticipantLink participant = participantLinks[originalRequest.InteractionFragment.TargetParticipantId];

                InteractRequestMessage cloneMessage = (InteractRequestMessage)MessageFactory.Current.ReserveMessage(typeof(InteractRequestMessage));
                cloneMessage.InteractionFragment.InteractionName = originalRequest.InteractionFragment.InteractionName;
                cloneMessage.InteractionFragment.SourceParticipantId = originalRequest.InteractionFragment.SourceParticipantId;
                cloneMessage.InteractionFragment.SourceObjectId = originalRequest.InteractionFragment.SourceObjectId;
                cloneMessage.InteractionFragment.TargetParticipantId = originalRequest.InteractionFragment.TargetParticipantId;
                cloneMessage.InteractionFragment.TargetObjectId = originalRequest.InteractionFragment.TargetObjectId;
                cloneMessage.InteractionFragment.ExtensionDialect = originalRequest.InteractionFragment.ExtensionDialect;
                cloneMessage.InteractionFragment.ExtensionDialectMajorVersion = originalRequest.InteractionFragment.ExtensionDialectMajorVersion;
                cloneMessage.InteractionFragment.ExtensionDialectMinorVersion = originalRequest.InteractionFragment.ExtensionDialectMinorVersion;

                byte[] extensionDataBuffer = new byte[originalRequest.InteractionFragment.ExtensionLength];
                Array.Copy(originalRequest.InteractionFragment.GetExtensionData(), extensionDataBuffer, originalRequest.InteractionFragment.ExtensionLength);
                cloneMessage.SetPayloadData(extensionDataBuffer);

                participantSessionDictionary[participant].Send(cloneMessage);

                return;
            }

            if (sourceBubbleId == Guid.Empty) // Broadcasting only local objects to other bubbles
            {
                foreach (BubbleLink bubbleLink in bubbleLinks.Values)
                {
                    InteractRequestMessage cloneMessage = (InteractRequestMessage)MessageFactory.Current.ReserveMessage(typeof(InteractRequestMessage));
                    cloneMessage.InteractionFragment.InteractionName = originalRequest.InteractionFragment.InteractionName;
                    cloneMessage.InteractionFragment.SourceParticipantId = originalRequest.InteractionFragment.SourceParticipantId;
                    cloneMessage.InteractionFragment.SourceObjectId = originalRequest.InteractionFragment.SourceObjectId;
                    cloneMessage.InteractionFragment.TargetParticipantId = originalRequest.InteractionFragment.TargetParticipantId;
                    cloneMessage.InteractionFragment.TargetObjectId = originalRequest.InteractionFragment.TargetObjectId;
                    cloneMessage.InteractionFragment.ExtensionDialect = originalRequest.InteractionFragment.ExtensionDialect;
                    cloneMessage.InteractionFragment.ExtensionDialectMajorVersion = originalRequest.InteractionFragment.ExtensionDialectMajorVersion;
                    cloneMessage.InteractionFragment.ExtensionDialectMinorVersion = originalRequest.InteractionFragment.ExtensionDialectMinorVersion;

                    byte[] extensionDataBuffer = new byte[originalRequest.InteractionFragment.ExtensionLength];
                    Array.Copy(originalRequest.InteractionFragment.GetExtensionData(), extensionDataBuffer, originalRequest.InteractionFragment.ExtensionLength);
                    cloneMessage.SetPayloadData(extensionDataBuffer);

                    bubbleLinkSessionDictionary[bubbleLink].Send(cloneMessage);
                }
            }

        }

        public void OnInteractResponse(Guid sourceBubbleId, Guid sourceParticipantId, InteractResponseMessage originalResponse)
        {
            Guid objectId = originalResponse.InteractionFragment.SourceObjectId;

            if (objectId != Guid.Empty)
            {
                CloudObject cloudObject = cloudCache.GetObject(objectId);
                if (cloudObject == null)
                {
                    LogUtil.Warn("Object action received for non existing object - Object: " + objectId + " Source Bubble: " + sourceBubbleId + " Source Participant: " + sourceParticipantId + ")");
                    return;
                }

                if (!AuthorizeAction(sourceBubbleId, sourceParticipantId, objectId, cloudObject))
                {
                    return;
                }
            }

            if (participantLinks.ContainsKey(originalResponse.InteractionFragment.TargetParticipantId))
            {
                ParticipantLink participant = participantLinks[originalResponse.InteractionFragment.TargetParticipantId];

                InteractResponseMessage cloneMessage = (InteractResponseMessage)MessageFactory.Current.ReserveMessage(typeof(InteractResponseMessage));
                cloneMessage.RequestMessageId = originalResponse.RequestMessageId;
                cloneMessage.FailureCode = originalResponse.FailureCode;
                cloneMessage.InteractionFragment.InteractionName = originalResponse.InteractionFragment.InteractionName;
                cloneMessage.InteractionFragment.SourceParticipantId = originalResponse.InteractionFragment.SourceParticipantId;
                cloneMessage.InteractionFragment.SourceObjectId = originalResponse.InteractionFragment.SourceObjectId;
                cloneMessage.InteractionFragment.TargetParticipantId = originalResponse.InteractionFragment.TargetParticipantId;
                cloneMessage.InteractionFragment.TargetObjectId = originalResponse.InteractionFragment.TargetObjectId;
                cloneMessage.InteractionFragment.ExtensionDialect = originalResponse.InteractionFragment.ExtensionDialect;
                cloneMessage.InteractionFragment.ExtensionDialectMajorVersion = originalResponse.InteractionFragment.ExtensionDialectMajorVersion;
                cloneMessage.InteractionFragment.ExtensionDialectMinorVersion = originalResponse.InteractionFragment.ExtensionDialectMinorVersion;

                byte[] extensionDataBuffer = new byte[originalResponse.InteractionFragment.ExtensionLength];
                Array.Copy(originalResponse.InteractionFragment.GetExtensionData(), extensionDataBuffer, originalResponse.InteractionFragment.ExtensionLength);
                cloneMessage.SetPayloadData(extensionDataBuffer);

                participantSessionDictionary[participant].Send(cloneMessage);

                return;
            }

            if (sourceBubbleId == Guid.Empty) // Broadcasting only local objects to other bubbles
            {
                foreach (BubbleLink bubbleLink in bubbleLinks.Values)
                {
                    InteractResponseMessage cloneMessage = (InteractResponseMessage)MessageFactory.Current.ReserveMessage(typeof(InteractResponseMessage));
                    cloneMessage.RequestMessageId = originalResponse.RequestMessageId;
                    cloneMessage.FailureCode = originalResponse.FailureCode;
                    cloneMessage.InteractionFragment.InteractionName = originalResponse.InteractionFragment.InteractionName;
                    cloneMessage.InteractionFragment.SourceParticipantId = originalResponse.InteractionFragment.SourceParticipantId;
                    cloneMessage.InteractionFragment.SourceObjectId = originalResponse.InteractionFragment.SourceObjectId;
                    cloneMessage.InteractionFragment.TargetParticipantId = originalResponse.InteractionFragment.TargetParticipantId;
                    cloneMessage.InteractionFragment.TargetObjectId = originalResponse.InteractionFragment.TargetObjectId;
                    cloneMessage.InteractionFragment.ExtensionDialect = originalResponse.InteractionFragment.ExtensionDialect;
                    cloneMessage.InteractionFragment.ExtensionDialectMajorVersion = originalResponse.InteractionFragment.ExtensionDialectMajorVersion;
                    cloneMessage.InteractionFragment.ExtensionDialectMinorVersion = originalResponse.InteractionFragment.ExtensionDialectMinorVersion;

                    byte[] extensionDataBuffer = new byte[originalResponse.InteractionFragment.ExtensionLength];
                    Array.Copy(originalResponse.InteractionFragment.GetExtensionData(), extensionDataBuffer, originalResponse.InteractionFragment.ExtensionLength);
                    cloneMessage.SetPayloadData(extensionDataBuffer);

                    bubbleLinkSessionDictionary[bubbleLink].Send(cloneMessage);
                }
            }

        }


        #endregion

    }
}
