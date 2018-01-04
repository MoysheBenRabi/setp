using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MXP;
using MXP.Cloud;
using MXP.Common.Proto;
using MXP.Extentions.OpenMetaverseFragments.Proto;
using MXP.Fragments;
using MXP.Messages;
using MXP.Util;
using CloudMath;
using CloudDeck.Model;

namespace CloudDeck
{
    /// <summary>
    /// DeckEngine coordinates working of DeckRenderer, DeckRudder, DeckScene and CloudView.
    /// </summary>
    public class DeckEngine
    {

        #region Fields

        public Guid ParticipantId = Guid.Empty;
        public String ParticipantNickName = "Anonymous";
        public Guid AvatarId = Guid.NewGuid();

        public int ExpectedObjectSynchronizationCount = 0;
        public int CurrentObjectSynchronizationCount = 0;

        private DateTime m_lastSteeringUpdate = DateTime.MinValue;
        private bool m_isHandoverConnect = false;

        private IDictionary<string, HashSet<Guid>> m_objectsWaitingAssets = new Dictionary<string, HashSet<Guid>>();
        private HashSet<Guid> m_objectUpdateQueue = new HashSet<Guid>();
       
        #endregion

        #region Properties
        
        public bool IsSynchronizing
        {
            get
            {
                return ExpectedObjectSynchronizationCount > CurrentObjectSynchronizationCount;
            }
        }

        public Guid DaemonId
        {
            get
            {
                // Default daemon id for bubble has same id as bubble id
                return DeckProgram.CloudView.BubbleId;
            }
        }

        #endregion

        #region Constructor

        public DeckEngine()
        {
            DeckProgram.CloudView.Client.ConnectionSuccess += OnConnected;
            DeckProgram.CloudView.Client.ServerDisconnected += OnDisconnected;
            DeckProgram.CloudView.LinkedBubbleListReceived += OnLinkedBubbleListReceived;
            DeckProgram.CloudView.ServerSynchronizationBegin += OnSynchronizationBegin;
            DeckProgram.CloudView.ServerSynchronizationEnd += OnSynchronizationEnd;
            DeckProgram.CloudView.ServerObjectHandover += OnServerObjectHandover;
            DeckProgram.CloudView.CloudCache.CacheObjectPut += OnObjectUpdate;
            DeckProgram.CloudView.CloudCache.CacheObjectRemoved += OnObjectRemove;
            DeckProgram.CloudView.ServerAction += OnObjectAction;
            DeckProgram.CloudView.ServerInteractResponse += OnInteractResponse;

            DeckProgram.AssetManager.AssetDownloadFailure += OnAssetDownloadFailure;
            DeckProgram.AssetManager.AssetDownloadSuccess += OnAssetDownloadSuccess;
        }

        #endregion

        #region Processing

        public void Process()
        {

            if (m_objectUpdateQueue.Count > 0)
            {
                foreach (Guid objectId in m_objectUpdateQueue)
                {

                    CloudObject cloudObject = DeckProgram.CloudView.CloudCache.GetObject(objectId);
                    OmModelPrimitiveExt modelPrimitiveExt = cloudObject.GetExtension<OmModelPrimitiveExt>();

                    Vector3 location = new Vector3(cloudObject.Location.X, cloudObject.Location.Y, cloudObject.Location.Z);
                    Quaternion orientation = new Quaternion(cloudObject.Orientation.W, cloudObject.Orientation.X, cloudObject.Orientation.Y, cloudObject.Orientation.Z);
                    Vector3 scale = new Vector3(cloudObject.BoundingSphereRadius * modelPrimitiveExt.Scale, cloudObject.BoundingSphereRadius * modelPrimitiveExt.Scale, cloudObject.BoundingSphereRadius * modelPrimitiveExt.Scale);

                    bool isNewAvatar = objectId == AvatarId && !DeckProgram.DeckScene.ContainsObject(objectId);

                    if (!DeckProgram.DeckScene.ContainsObject(objectId))
                    {
                        if (IsSynchronizing)
                        {
                            CurrentObjectSynchronizationCount++;
                            if (ExpectedObjectSynchronizationCount <= CurrentObjectSynchronizationCount)
                            {
                                CurrentObjectSynchronizationCount = 0;
                                ExpectedObjectSynchronizationCount = 0;
                                Thread.Sleep(200);
                            }
                        }

                        DeckProgram.DeckScene.AddObject(new DeckObject(objectId,cloudObject.ObjectName,cloudObject.TypeName,modelPrimitiveExt.ModelUrl,modelPrimitiveExt.Scale,location,orientation,cloudObject.BoundingSphereRadius,scale));
                    }

                    DeckObject deckObject = DeckProgram.DeckScene.GetObject(cloudObject.ObjectId);

                    if (isNewAvatar)
                    {
                        Matrix orientationMatrix;
                        Common.Rotate(out orientationMatrix, ref orientation);

                        DeckProgram.DeckRudder.AvatarTargetLocation = location;
                        DeckProgram.DeckRudder.AvatarCurrentLocation = location;
                        DeckProgram.DeckRudder.AvatarTargetOrientationMatrix = orientationMatrix;
                        DeckProgram.DeckRudder.AvatarCurrentOrientationMatrix = orientationMatrix;
                        DeckProgram.DeckRudder.UpdateCameraTransformation();
                        DeckProgram.DeckRudder.SynchronizeCameraCurrentLocationWithTargetLocation();
                    }

                    deckObject.ObjectName = cloudObject.ObjectName;
                    deckObject.NetworkLocation = location;
                    deckObject.NetworkOrientation = orientation;
                    deckObject.Radius = cloudObject.BoundingSphereRadius;
                    deckObject.ModelScale = modelPrimitiveExt.Scale;
                    deckObject.NetworkScale = scale;

                    DeckProgram.DeckScene.UpdateObject(deckObject);

                    DeckProgram.DeckScene.ActivateObject(objectId);

                }
                m_objectUpdateQueue.Clear();

            }            

            if (DeckProgram.DeckRudder.IsSteered&&DateTime.Now.Subtract(m_lastSteeringUpdate).TotalMilliseconds>100)
            {
                m_lastSteeringUpdate = DateTime.Now;

                if (DeckProgram.CloudView.CloudCache.GetObject(AvatarId) != null)
                {
                    ModifyRequestMessage modifyRequestMessage = new ModifyRequestMessage();
                    modifyRequestMessage.ObjectFragment.ObjectId = AvatarId;
                    modifyRequestMessage.ObjectFragment.OwnerId = ParticipantId;
                    modifyRequestMessage.ObjectFragment.TypeId = Guid.Empty;
                    modifyRequestMessage.ObjectFragment.ObjectName = ParticipantNickName;
                    modifyRequestMessage.ObjectFragment.TypeName = "Avatar";
                    modifyRequestMessage.ObjectFragment.BoundingSphereRadius = 2f;
                    modifyRequestMessage.ObjectFragment.Mass = 2f;
                    modifyRequestMessage.ObjectFragment.Location.X = (float)DeckProgram.DeckRudder.AvatarTargetLocation.X;
                    modifyRequestMessage.ObjectFragment.Location.Y = (float)DeckProgram.DeckRudder.AvatarTargetLocation.Y;
                    modifyRequestMessage.ObjectFragment.Location.Z = (float)DeckProgram.DeckRudder.AvatarTargetLocation.Z;
                    Matrix rotationMatrix = DeckProgram.DeckRudder.AvatarTargetOrientationMatrix;
                    Quaternion quaternion;
                    Common.Rotate(out quaternion, ref rotationMatrix);
                    modifyRequestMessage.ObjectFragment.Orientation.X = (float)quaternion.I;
                    modifyRequestMessage.ObjectFragment.Orientation.Y = (float)quaternion.J;
                    modifyRequestMessage.ObjectFragment.Orientation.Z = (float)quaternion.K;
                    modifyRequestMessage.ObjectFragment.Orientation.W = (float)quaternion.W;

                    OmModelPrimitiveExt modelPrimitiveExt = new OmModelPrimitiveExt();
                    modelPrimitiveExt.ModelUrl = "http://assets.bubblecloud.org/Collada/Seymour/Seymour_triangulate.dae";
                    modelPrimitiveExt.Scale = (float)0.1f;
                    modifyRequestMessage.SetExtension(modelPrimitiveExt);

                    DeckProgram.CloudView.ModifyObject(modifyRequestMessage);
                    DeckProgram.DeckRudder.IsSteered = false;
                }
            }

        }

        #endregion

        #region Event Handlers

        public void OnConnected(JoinResponseMessage message)
        {
            DeckProgram.CloudView.RequestLinkedBubbleList();

            if (!m_isHandoverConnect)
            {
                InjectRequestMessage injectRequest = new InjectRequestMessage();
                injectRequest.ObjectFragment.ObjectId = AvatarId;
                injectRequest.ObjectFragment.ObjectName = ParticipantNickName;
                injectRequest.ObjectFragment.TypeName = "Avatar";
                injectRequest.ObjectFragment.BoundingSphereRadius = 2f;
                injectRequest.ObjectFragment.Mass = 2f;
                OmModelPrimitiveExt modelPrimitiveExt = new OmModelPrimitiveExt();
                modelPrimitiveExt.ModelUrl = "http://assets.bubblecloud.org/Collada/Seymour/Seymour_triangulate.dae";
                modelPrimitiveExt.Scale = 0.1f;
                injectRequest.SetExtension(modelPrimitiveExt);
                DeckProgram.CloudView.InjectObject(injectRequest);
            }
            m_isHandoverConnect = false;

            LogUtil.Info("DeckEngine connected.");

            ExpectedObjectSynchronizationCount = int.MaxValue; // This is overriden later by synchronization begin event.
            CurrentObjectSynchronizationCount = 0;

            ParticipantId = message.ParticipantId;
            DeckProgram.HudForm.SetBubbleInformation(
                message.ProgramName + " / " + message.BubbleName + " ( Hub Software Version " + message.ProgramMajorVersion + "." + message.ProgramMinorVersion +
                ", Hub Protocol Version "+message.ProtocolMajorVersion+"."+message.ProtocolMinorVersion+", Source Revision "+message.ProtocolSourceRevision+" )");
        }

        public void OnDisconnected(Message message)
        {
            DeckProgram.DeckScene.Clear();
            m_objectUpdateQueue.Clear();
            DeckProgram.CloudView.CloudCache.Clear();
        }

        public void OnServerObjectHandover(HandoverEventMessage handoverEvent)
        {
            if (DeckProgram.DeckScene.ContainsObject(handoverEvent.ObjectFragment.ObjectId))
            {
                DeckProgram.DeckScene.DeactivateObject(handoverEvent.ObjectFragment.ObjectId);
            }
            DeckBubble targetBubble = DeckProgram.DeckScene.GetBubble(handoverEvent.TargetBubbleId);
            string targetBubbleLoginUrl = targetBubble.WebUrl + "GetLoginSecret.aspx?goto=mxp://" + targetBubble.Address + ":" + targetBubble.Port + "/" + targetBubble.BubbleId;
            DeckProgram.LoginForm.SetBubbleLoginAddress(targetBubbleLoginUrl);
            DeckProgram.CloudView.Disconnect();
            m_isHandoverConnect = true;
        }

        public void OnLinkedBubbleListReceived(ListBubblesResponse listBubblesResponse)
        {
            foreach(BubbleFragment bubbleFragment in listBubblesResponse.BubbleFragments)
            {
                LogUtil.Info("Received linked bubble: " + bubbleFragment.BubbleName + " X: " + bubbleFragment.BubbleCenter.X+", Y: "+bubbleFragment.BubbleCenter.Y+", Z: "+bubbleFragment.BubbleCenter.Z+" Range: "+bubbleFragment.BubbleRange);

                DeckBubble deckBubble = new DeckBubble
                {
                    BubbleId=bubbleFragment.BubbleId,
                    Name=bubbleFragment.BubbleName,
                    Address=bubbleFragment.BubbleAddress,
                    Port=bubbleFragment.BubblePort-1,
                    WebUrl=bubbleFragment.BubbleAssetCacheUrl,
                    Range=bubbleFragment.BubbleRange,
                    Center=new MsdVector3f()
                };

                deckBubble.Center.X=bubbleFragment.BubbleCenter.X;
                deckBubble.Center.Y=bubbleFragment.BubbleCenter.Y;
                deckBubble.Center.Z=bubbleFragment.BubbleCenter.Z;

                DeckProgram.DeckScene.AddBubble(deckBubble);
            }
        }

        public void OnSynchronizationBegin(SynchronizationBeginEventMessage synchronizationBeginEvent)
        {
            ExpectedObjectSynchronizationCount = (int) synchronizationBeginEvent.ObjectCount;
            CurrentObjectSynchronizationCount = 0;
            LogUtil.Info("DeckEngine synchronization begin: " + ExpectedObjectSynchronizationCount);
        }

        public void OnSynchronizationEnd(SynchronizationEndEventMessage synchronizationEndEvent)
        {
            LogUtil.Info("DeckEngine synchronization done.");
            RequestObjectTypes();
        }

        public void OnObjectUpdate(CloudObject cloudObject)
        {
            if (cloudObject.HasExtension)
            {
                OmModelPrimitiveExt modelPrimitiveExt = cloudObject.GetExtension<OmModelPrimitiveExt>();
                string modelUrl = modelPrimitiveExt.ModelUrl;
                if (!DeckProgram.AssetManager.IsBlackListed(modelUrl))
                {
                    if (!DeckProgram.AssetManager.IsAssetDownloaded(modelUrl) &&
                        !DeckProgram.AssetManager.IsAssetDownloading(modelUrl))
                    {
                        LogUtil.Info("Loading referenced model: " + modelUrl);
                        DeckProgram.AssetManager.DownloadAsset(modelUrl);
                    }
                    if (DeckProgram.AssetManager.IsAssetDownloaded(modelUrl))
                    {
                        if (!m_objectUpdateQueue.Contains(cloudObject.ObjectId))
                        {
                            DeckProgram.AssetManager.EnsureModelIsLoadedToRenderer(modelUrl);
                            m_objectUpdateQueue.Add(cloudObject.ObjectId);
                        }
                    }
                    else
                    {
                        if (!m_objectsWaitingAssets.ContainsKey(modelUrl))
                        {
                            m_objectsWaitingAssets.Add(modelUrl, new HashSet<Guid>());
                        }
                        if (!m_objectsWaitingAssets[modelUrl].Contains(cloudObject.ObjectId))
                        {
                            m_objectsWaitingAssets[modelUrl].Add(cloudObject.ObjectId);
                        }
                    }
                }
            }
        }

        public void OnAssetDownloadSuccess(string assetUrl)
        {
            DeckProgram.AssetManager.EnsureModelIsLoadedToRenderer(assetUrl);
            if (m_objectsWaitingAssets.ContainsKey(assetUrl))
            {
                m_objectUpdateQueue.UnionWith(m_objectsWaitingAssets[assetUrl]);
                m_objectsWaitingAssets.Remove(assetUrl);
            }
        }

        public void OnAssetDownloadFailure(string assetUrl)
        {
        }

        public void OnObjectRemove(CloudObject cloudObject)
        {
            LogUtil.Info("DeckEngine object remove: " + cloudObject.TypeName);
            if(m_objectUpdateQueue.Contains(cloudObject.ObjectId))
            {
                m_objectUpdateQueue.Remove(cloudObject.ObjectId);
            }
            if (DeckProgram.DeckScene.ContainsObject(cloudObject.ObjectId))
            {
                DeckProgram.DeckScene.RemoveObject(cloudObject.ObjectId);
            }
        }

        public void OnObjectAction(ActionEventMessage actionEvent)
        {
            if ("Chat".Equals(actionEvent.ActionFragment.ActionName))
            {
                OmChatExt omChatExt=actionEvent.GetExtension<OmChatExt>();
                DeckProgram.DeckScene.AddChatMessage(actionEvent.ActionFragment.SourceObjectId, omChatExt.Message);
            }
        }

        public void OnInteractResponse(InteractResponseMessage responseMessage)
        {
            if ("TypeList".Equals(responseMessage.InteractionFragment.InteractionName))
            {
                OmTypeListResponseExt omTypeListResponse = responseMessage.GetExtension<OmTypeListResponseExt>();
                List<DeckObjectType> objectTypes = new List<DeckObjectType>();
                foreach (OmObjectType omObjectType in omTypeListResponse.ObjectType)
                {
                    DeckObjectType deckObjectType = new DeckObjectType
                    {
                        TypeId = new Guid(omObjectType.TypeId),
                        TypeName = omObjectType.TypeName
                    };
                    objectTypes.Add(deckObjectType);
                }
                DeckProgram.DeckDaemon.SetObjectTypes(objectTypes);
            }
        }

        #endregion

        #region Public Interface

        public void SendChatMessage(string message)
        {
            ActionEventMessage actionEvent = new ActionEventMessage();
            actionEvent.ActionFragment.ObservationRadius = 1000;
            actionEvent.ActionFragment.ActionName = "Chat";
            actionEvent.ActionFragment.SourceObjectId = AvatarId; 

            OmChatExt omChatExt=new OmChatExt();
            omChatExt.Message=message;

            actionEvent.SetExtension<OmChatExt>(omChatExt);

            DeckProgram.CloudView.ExecuteAction(actionEvent);
        }

        public void RequestObjectTypes()
        {
            InteractRequestMessage interactRequest = new InteractRequestMessage();
            interactRequest.InteractionFragment.SourceParticipantId = this.ParticipantId;
            interactRequest.InteractionFragment.TargetParticipantId = this.DaemonId;
            interactRequest.InteractionFragment.InteractionName = "TypeList";
            DeckProgram.CloudView.SendInteractRequest(interactRequest);
        }

        public void RequestObjectInsert(Guid typeId, Vector3 location, Quaternion orientation)
        {
            InteractRequestMessage request = new InteractRequestMessage();
            request.InteractionFragment.SourceParticipantId = this.ParticipantId;
            request.InteractionFragment.TargetParticipantId = this.DaemonId;
            request.InteractionFragment.InteractionName = "ObjectInsert";
            OmInsertRequestExt requestExt = new OmInsertRequestExt();
            requestExt.TypeId = typeId.ToString();
            requestExt.Location = new MsdVector3f();
            requestExt.Location.X = location.X;
            requestExt.Location.Y = location.Y;
            requestExt.Location.Z = location.Z;
            requestExt.Orientation = new MsdQuaternion4f();
            requestExt.Orientation.X = orientation.I;
            requestExt.Orientation.Y = orientation.J;
            requestExt.Orientation.Z = orientation.K;
            requestExt.Orientation.W = orientation.W;
            request.SetExtension<OmInsertRequestExt>(requestExt);
            DeckProgram.CloudView.SendInteractRequest(request);           
        }

        public void RequestObjectDelete(Guid objectId)
        {
            InteractRequestMessage request = new InteractRequestMessage();
            request.InteractionFragment.SourceParticipantId = this.ParticipantId;
            request.InteractionFragment.TargetParticipantId = this.DaemonId;
            request.InteractionFragment.InteractionName = "ObjectDelete";
            OmDeleteRequestExt requestExt = new OmDeleteRequestExt();
            requestExt.ObjectId = objectId.ToString();
            request.SetExtension<OmDeleteRequestExt>(requestExt);
            DeckProgram.CloudView.SendInteractRequest(request);  
        }

        public void RequestObjectUpdate(Guid objectId, string name, Vector3 location, Quaternion orientation, float scale)
        {
            InteractRequestMessage request = new InteractRequestMessage();
            request.InteractionFragment.SourceParticipantId = this.ParticipantId;
            request.InteractionFragment.TargetParticipantId = this.DaemonId;
            request.InteractionFragment.InteractionName = "ObjectUpdate";
            OmUpdateRequestExt requestExt = new OmUpdateRequestExt();
            requestExt.ObjectId = objectId.ToString();
            requestExt.Name = name;
            requestExt.Location = new MsdVector3f();
            requestExt.Location.X = location.X;
            requestExt.Location.Y = location.Y;
            requestExt.Location.Z = location.Z;
            requestExt.Orientation = new MsdQuaternion4f();
            requestExt.Orientation.X = orientation.I;
            requestExt.Orientation.Y = orientation.J;
            requestExt.Orientation.Z = orientation.K;
            requestExt.Orientation.W = orientation.W;
            requestExt.Scale = scale;
            request.SetExtension<OmUpdateRequestExt>(requestExt);
            DeckProgram.CloudView.SendInteractRequest(request);   
        }

        #endregion
    }

}
