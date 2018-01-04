using System;
using System.Collections.Generic;

using System.Text;
using MXP;
using MXP.Messages;
using System.Threading;
using System.Diagnostics;
using MXP.Fragments;
using MXP.Util;
using MXP.Cloud;
using MXP.Common.Proto;
using MXP.Extentions.OpenMetaverseFragments.Proto;
using DaemonLogic;

using System.Linq;
using System.Data.Objects;


namespace CloudTank.Daemons
{

    /// <summary>
    /// Test participant injects and updates objects stored to daemon database.
    /// </summary>
    public class DaemonParticipant
    {

        public CloudView client;
        private string serverAddress;
        private int serverPort;
        private Guid bubbleId;
        public string DaemonIdentifier;
        public string DaemonSecret;
        private DaemonEntities entityContext = new DaemonEntities();

        private Bubble bubble=null;
        private List<DaemonLogic.CloudObject> Objects = null;
        private HashSet<Guid> InjectedObjects = new HashSet<Guid>();
        private Dictionary<Guid, DaemonLogic.CloudObject> IdObjectDictionary = new Dictionary<Guid, DaemonLogic.CloudObject>();
        private DateTime lastRefreshTime;

        public Guid BubbleId
        {
            get
            {
                return bubbleId;
            }
        }

        public DaemonParticipant(
            string serverAddress,
            int serverPort,
            Guid bubbleId,
            string daemonIdentifier,
            string daemonSecret
            )
        {
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
            this.bubbleId = bubbleId;
            this.DaemonIdentifier = daemonIdentifier;
            this.DaemonSecret = daemonSecret;
            String programName = "Cloud Daemon Participant";
            byte programMajorVersion = 0;
            byte programMinorVersion = 1;

            bubble = QueryUtil.First<Bubble>((from b in entityContext.Bubble where b.BubbleId == bubbleId select b));

            Objects = (from o in entityContext.CloudObject where o.Bubble.BubbleId == bubble.BubbleId && o.Enabled==true select o).ToList<DaemonLogic.CloudObject>();

            lastRefreshTime = DateTime.Now;

            if (Objects.Count == 0)
            {
                return;
            }

            client = new CloudView(100, programName, programMajorVersion, programMinorVersion);

            client.ServerInteractRequest += OnInteractRequest;

            foreach (DaemonLogic.CloudObject cloudObject in Objects)
            {
                DaemonLogic.ObjectType objectType = QueryUtil.First<DaemonLogic.ObjectType>(from o in entityContext.CloudObject where o.CloudObjectId == cloudObject.CloudObjectId select o.ObjectType);
                DaemonLogic.Participant participant = QueryUtil.First<DaemonLogic.Participant>(from o in entityContext.CloudObject where o.CloudObjectId == cloudObject.CloudObjectId select o.Participant);
                IdObjectDictionary.Add(cloudObject.CloudObjectId, cloudObject);
            }
        }

        public void Startup()
        {
            if (client==null)
            {
                return;
            }
            client.Connect(serverAddress, serverPort, bubbleId, "", "", "", DaemonIdentifier, DaemonSecret, Objects[0].CloudObjectId, false);
        }

        public void Shutdown()
        {
            if (client == null)
            {
                return;
            }
            client.Disconnect();
        }

        public bool IsConnected
        {
            get
            {
                if (client == null)
                {
                    return false;
                }
                return client.IsConnected||client.IsConnecting;
            }
        }

        public Guid ParticipantId
        {
            get
            {
                if (client == null)
                {
                    return Guid.Empty;
                }
                return client.ParticipantId;
            }
        }

        public void Process()
        {
            if (client == null)
            {
                return;
            }

            client.Process();

            if (client.IsConnected&&DateTime.Now.Subtract(lastRefreshTime)>new TimeSpan(0,0,10))
            {
                int addCount = 0;

                foreach (DaemonLogic.CloudObject cloudObject in Objects)
                {
                    if (!InjectedObjects.Contains(cloudObject.CloudObjectId))
                    {
                        InjectOrUpdateObject(cloudObject);

                        addCount++;
                        if (addCount > 10)
                        {
                            break;
                        }
                    }
                }

                List<DaemonLogic.CloudObject> modifiedOjects = (from o in entityContext.CloudObject where o.Bubble.BubbleId == bubble.BubbleId && o.Modified>lastRefreshTime select o).ToList<DaemonLogic.CloudObject>();

                foreach (DaemonLogic.CloudObject cloudObject in modifiedOjects)
                {
                    entityContext.Refresh(System.Data.Objects.RefreshMode.StoreWins, cloudObject);

                    if (cloudObject.Enabled)
                    {

                        DaemonLogic.ObjectType objectType = QueryUtil.First<DaemonLogic.ObjectType>(from o in entityContext.CloudObject where o.CloudObjectId == cloudObject.CloudObjectId select o.ObjectType);
                        DaemonLogic.Participant participant = QueryUtil.First<DaemonLogic.Participant>(from o in entityContext.CloudObject where o.CloudObjectId == cloudObject.CloudObjectId select o.Participant);

                        InjectOrUpdateObject(cloudObject);

                    }
                    else
                    {
                        if (Objects.Contains(cloudObject))
                        {
                            EjectObject(cloudObject);
                        }
                    }
                }

                lastRefreshTime = DateTime.Now;
            }

        }

        private void InjectOrUpdateObject(DaemonLogic.CloudObject cloudObject)
        {
            if (!IdObjectDictionary.ContainsKey(cloudObject.CloudObjectId))
            {
                IdObjectDictionary.Add(cloudObject.CloudObjectId, cloudObject);
                Objects.Add(cloudObject);
            }

            if (!InjectedObjects.Contains(cloudObject.CloudObjectId))
            {
                InjectedObjects.Add(cloudObject.CloudObjectId);
                InjectRequestMessage injectRequestMessage = new InjectRequestMessage();
                injectRequestMessage.ObjectFragment.ObjectId = cloudObject.CloudObjectId;
                injectRequestMessage.ObjectFragment.OwnerId = cloudObject.Participant.ParticipantId;
                injectRequestMessage.ObjectFragment.TypeId = cloudObject.ObjectType.ObjectTypeId;
                injectRequestMessage.ObjectFragment.ObjectName = cloudObject.Name;
                injectRequestMessage.ObjectFragment.TypeName = cloudObject.ObjectType.Name;
                injectRequestMessage.ObjectFragment.BoundingSphereRadius = (float)cloudObject.Radius;
                injectRequestMessage.ObjectFragment.Mass = (float)cloudObject.Mass;
                injectRequestMessage.ObjectFragment.Location.X = (float)cloudObject.X;
                injectRequestMessage.ObjectFragment.Location.Y = (float)cloudObject.Y;
                injectRequestMessage.ObjectFragment.Location.Z = (float)cloudObject.Z;
                injectRequestMessage.ObjectFragment.Orientation.X = (float)cloudObject.OX;
                injectRequestMessage.ObjectFragment.Orientation.Y = (float)cloudObject.OY;
                injectRequestMessage.ObjectFragment.Orientation.Z = (float)cloudObject.OZ;
                injectRequestMessage.ObjectFragment.Orientation.W = (float)cloudObject.OW;

                OmModelPrimitiveExt modelPrimitiveExt = new OmModelPrimitiveExt();
                modelPrimitiveExt.ModelUrl = cloudObject.ModelUrl;
                modelPrimitiveExt.Scale = (float)cloudObject.ModelScale;
                injectRequestMessage.SetExtension(modelPrimitiveExt);

                client.InjectObject(injectRequestMessage);
            }
            else
            {

                ModifyRequestMessage modifyRequestMessage = new ModifyRequestMessage();
                modifyRequestMessage.ObjectFragment.ObjectId = cloudObject.CloudObjectId;
                modifyRequestMessage.ObjectFragment.OwnerId = cloudObject.Participant.ParticipantId;
                modifyRequestMessage.ObjectFragment.TypeId = cloudObject.ObjectType.ObjectTypeId;
                modifyRequestMessage.ObjectFragment.ObjectName = cloudObject.Name;
                modifyRequestMessage.ObjectFragment.TypeName = cloudObject.ObjectType.Name;
                modifyRequestMessage.ObjectFragment.BoundingSphereRadius = (float)cloudObject.Radius;
                modifyRequestMessage.ObjectFragment.Mass = (float)cloudObject.Mass;
                modifyRequestMessage.ObjectFragment.Location.X = (float)cloudObject.X;
                modifyRequestMessage.ObjectFragment.Location.Y = (float)cloudObject.Y;
                modifyRequestMessage.ObjectFragment.Location.Z = (float)cloudObject.Z;
                modifyRequestMessage.ObjectFragment.Orientation.X = (float)cloudObject.OX;
                modifyRequestMessage.ObjectFragment.Orientation.Y = (float)cloudObject.OY;
                modifyRequestMessage.ObjectFragment.Orientation.Z = (float)cloudObject.OZ;
                modifyRequestMessage.ObjectFragment.Orientation.W = (float)cloudObject.OW;

                OmModelPrimitiveExt modelPrimitiveExt = new OmModelPrimitiveExt();
                modelPrimitiveExt.ModelUrl = cloudObject.ModelUrl;
                modelPrimitiveExt.Scale = (float)cloudObject.ModelScale;
                modifyRequestMessage.SetExtension(modelPrimitiveExt);

                client.ModifyObject(modifyRequestMessage);
            }
        }

        private void EjectObject(DaemonLogic.CloudObject cloudObject)
        {
            if (Objects.Contains(cloudObject))
            {
                Objects.Remove(cloudObject);
                IdObjectDictionary.Remove(cloudObject.CloudObjectId);
                if (InjectedObjects.Contains(cloudObject.CloudObjectId))
                {
                    InjectedObjects.Remove(cloudObject.CloudObjectId);
                    EjectRequestMessage ejectRequestMessage = new EjectRequestMessage();
                    ejectRequestMessage.ObjectId = cloudObject.CloudObjectId;
                    client.EjectObject(ejectRequestMessage);
                }
            }
        }

        public void OnInteractRequest(InteractRequestMessage interactRequest)
        {

            if (interactRequest.InteractionFragment.InteractionName == "TypeList")
            {
                InteractResponseMessage interactionResponse = (InteractResponseMessage) MessageFactory.Current.ReserveMessage(typeof(InteractResponseMessage));
                interactionResponse.RequestMessageId = interactRequest.MessageId;
                interactionResponse.FailureCode = MxpResponseCodes.SUCCESS;

                interactionResponse.InteractionFragment.InteractionName = "TypeList";
                interactionResponse.InteractionFragment.TargetParticipantId = interactRequest.InteractionFragment.SourceParticipantId;
                interactionResponse.InteractionFragment.SourceParticipantId = interactRequest.InteractionFragment.TargetParticipantId;

                OmTypeListResponseExt responseExt = new OmTypeListResponseExt();

                List<DaemonLogic.ObjectType> objectTypes = (from t in entityContext.ObjectType orderby t.Name select t).ToList<DaemonLogic.ObjectType>();
                foreach (DaemonLogic.ObjectType objectType in objectTypes)
                {
                    OmObjectType omObjectType = new OmObjectType();
                    omObjectType.TypeId = objectType.ObjectTypeId.ToString();
                    omObjectType.TypeName = objectType.Name;
                    responseExt.ObjectType.Add(omObjectType);
                }

                interactionResponse.SetExtension<OmTypeListResponseExt>(responseExt);

                client.SendInteractResponse(interactionResponse);
            }

            if (interactRequest.InteractionFragment.InteractionName == "ObjectInsert")
            {
                OmInsertRequestExt requestExt = interactRequest.GetExtension<OmInsertRequestExt>();

                Guid typeId=new Guid(requestExt.TypeId);
                ObjectType objectType = QueryUtil.First<DaemonLogic.ObjectType>(from t in entityContext.ObjectType where t.ObjectTypeId == typeId select t);
                entityContext.Refresh(System.Data.Objects.RefreshMode.StoreWins, objectType);
                Participant participant = QueryUtil.First<DaemonLogic.Participant>(from p in entityContext.Participant where p.ParticipantId == interactRequest.InteractionFragment.SourceParticipantId select p);

                DaemonLogic.CloudObject cloudObject = new DaemonLogic.CloudObject
                {
                    CloudObjectId = Guid.NewGuid(),
                    Participant = participant,
                    ObjectType = objectType,
                    Bubble = bubble,
                    Name = "New " + objectType.Name,
                    Radius = objectType.Radius,
                    Mass = objectType.Mass,
                    ModelUrl = objectType.ModelUrl,
                    ModelScale = objectType.ModelScale,
                    X = requestExt.Location.X,
                    Y = requestExt.Location.Y,
                    Z = requestExt.Location.Z,
                    OX = requestExt.Orientation.X,
                    OY = requestExt.Orientation.Y,
                    OZ = requestExt.Orientation.Z,
                    OW = requestExt.Orientation.W,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Enabled = true
                };
                entityContext.AddToCloudObject(cloudObject);

                entityContext.SaveChanges();

                InjectOrUpdateObject(cloudObject);

                InteractResponseMessage interactionResponse = (InteractResponseMessage)MessageFactory.Current.ReserveMessage(typeof(InteractResponseMessage));
                interactionResponse.RequestMessageId = interactRequest.MessageId;
                interactionResponse.FailureCode = MxpResponseCodes.SUCCESS;
                interactionResponse.InteractionFragment.InteractionName = "ObjectInsert";
                interactionResponse.InteractionFragment.TargetParticipantId = interactRequest.InteractionFragment.SourceParticipantId;
                interactionResponse.InteractionFragment.SourceParticipantId = interactRequest.InteractionFragment.TargetParticipantId;

                OmInsertResponseExt responseExt = new OmInsertResponseExt();
                responseExt.ObjectId = cloudObject.CloudObjectId.ToString();
                
                interactionResponse.SetExtension<OmInsertResponseExt>(responseExt);

                client.SendInteractResponse(interactionResponse);

            }

            if (interactRequest.InteractionFragment.InteractionName == "ObjectUpdate")
            {
                OmUpdateRequestExt requestExt = interactRequest.GetExtension<OmUpdateRequestExt>();

                Guid objectId = new Guid(requestExt.ObjectId);

                byte failureCode = MxpResponseCodes.SUCCESS;
                if (IdObjectDictionary.ContainsKey(objectId))
                {

                    DaemonLogic.CloudObject cloudObject = IdObjectDictionary[objectId];

                    if (cloudObject.Participant.ParticipantId != interactRequest.InteractionFragment.SourceParticipantId &&
                        bubble.Participant.ParticipantId != interactRequest.InteractionFragment.SourceParticipantId)
                    {
                        failureCode = MxpResponseCodes.UNAUTHORIZED_OPERATION;
                    }
                    else
                    {
                        cloudObject.Name = requestExt.Name;
                        cloudObject.Radius = requestExt.Scale;
                        cloudObject.X = requestExt.Location.X;
                        cloudObject.Y = requestExt.Location.Y;
                        cloudObject.Z = requestExt.Location.Z;
                        cloudObject.OX = requestExt.Orientation.X;
                        cloudObject.OY = requestExt.Orientation.Y;
                        cloudObject.OZ = requestExt.Orientation.Z;
                        cloudObject.OW = requestExt.Orientation.W;
                        cloudObject.Modified = DateTime.Now;

                        entityContext.SaveChanges();
                        InjectOrUpdateObject(cloudObject);
                    }

                }
                else
                {
                    failureCode = MxpResponseCodes.UNKNOWN_ID;
                }

                InteractResponseMessage interactionResponse = (InteractResponseMessage)MessageFactory.Current.ReserveMessage(typeof(InteractResponseMessage));
                interactionResponse.RequestMessageId = interactRequest.MessageId;

                interactionResponse.FailureCode = failureCode;
                interactionResponse.InteractionFragment.InteractionName = "ObjectUpdate";
                interactionResponse.InteractionFragment.TargetParticipantId = interactRequest.InteractionFragment.SourceParticipantId;
                interactionResponse.InteractionFragment.SourceParticipantId = interactRequest.InteractionFragment.TargetParticipantId;

                client.SendInteractResponse(interactionResponse);

            }

            if (interactRequest.InteractionFragment.InteractionName == "ObjectDelete")
            {
                OmDeleteRequestExt requestExt = interactRequest.GetExtension<OmDeleteRequestExt>();
                Guid objectId=new Guid(requestExt.ObjectId);

                byte failureCode = MxpResponseCodes.SUCCESS;

                if (this.IdObjectDictionary.ContainsKey(objectId))
                {
                    DaemonLogic.CloudObject cloudObject= IdObjectDictionary[objectId];
                    if (cloudObject.Participant.ParticipantId != interactRequest.InteractionFragment.SourceParticipantId&&
                        bubble.Participant.ParticipantId != interactRequest.InteractionFragment.SourceParticipantId)
                    {
                        failureCode = MxpResponseCodes.UNAUTHORIZED_OPERATION;
                    }
                    else
                    {
                        entityContext.DeleteObject(cloudObject);
                        entityContext.SaveChanges();
                        EjectObject(cloudObject);
                    }
                }
                else
                {
                    failureCode = MxpResponseCodes.UNKNOWN_ID;
                }

                InteractResponseMessage interactionResponse = (InteractResponseMessage)MessageFactory.Current.ReserveMessage(typeof(InteractResponseMessage));
                interactionResponse.RequestMessageId = interactRequest.MessageId;

                interactionResponse.FailureCode = failureCode;
                interactionResponse.InteractionFragment.InteractionName = "ObjectDelete";
                interactionResponse.InteractionFragment.TargetParticipantId = interactRequest.InteractionFragment.SourceParticipantId;
                interactionResponse.InteractionFragment.SourceParticipantId = interactRequest.InteractionFragment.TargetParticipantId;

                client.SendInteractResponse(interactionResponse);
            }

        }

    }
}
