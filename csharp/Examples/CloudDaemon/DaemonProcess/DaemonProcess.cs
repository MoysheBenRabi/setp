using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.Objects;
using MXP.Cloud;
using MXP;
using MXP.Util;
using System.Threading;
using DaemonLogic;
using System.Configuration;

using MXP.Messages;
using CloudTank.Daemons;

namespace DaemonProcess
{
    public class DaemonProcess
    {
        // The LocalProcess entity id.
        public Guid LocalProcessId { get; set; }

        // Cloud service object.
        private CloudService service=null;


        // Entity context
        private DaemonEntities entityContext = null;

        // Persistent entities
        private LocalProcess localProcessEntity;
        private LocalProcessState localProcessStateEntity = null;
        private DaemonLogic.Participant participantEntity;
        private List<RemoteProcess> remoteProcessEntities;
        private List<Bubble> bubbleEntities;
        private IDictionary<Guid, DaemonParticipant> bubbleDaemons = new Dictionary<Guid, DaemonParticipant>();

        // Timing
        private DateTime lastTime = DateTime.MinValue;
        private TimeSpan lastProcessorTime = TimeSpan.Zero;
        private DateTime lastUpdate = DateTime.MinValue;

        private bool requestShutdown=false;

        // Field reflecting whether daemon process should keep running.
        public bool KeepRunning
        {
            get { return !requestShutdown; }
        }

        public bool IsRunning
        {
            get { return service != null; }
        }

        public void RequestShutdown()
        {
            requestShutdown = true;
        }

        public void Startup()
        {
            lock (this)
            {
                Thread.CurrentThread.Name = LocalProcessId.ToString()+"-main";
                MxpOptions.ThreadNamePrefix = LocalProcessId.ToString();

                LogUtil.Info("Daemon Process startup...");

                if (service != null)
                {
                    throw new Exception("DaemonProcess already started.");
                }

                // Preparing database entity context.
                entityContext = new DaemonEntities();

                // Loading configuration from database.
                localProcessEntity = QueryUtil.First<LocalProcess>(
                    from lp in entityContext.LocalProcess where lp.LocalProcessId == LocalProcessId select lp);
                participantEntity = QueryUtil.First<DaemonLogic.Participant>(
                    from lp in entityContext.LocalProcess where lp.LocalProcessId == LocalProcessId select lp.Participant);             
                remoteProcessEntities = (from rp in entityContext.RemoteProcess where rp.LocalProcess.LocalProcessId == LocalProcessId select rp).ToList<RemoteProcess>();
                bubbleEntities = (from b in entityContext.Bubble where b.LocalProcess.LocalProcessId == LocalProcessId select b).ToList<Bubble>();

                LogUtil.Info("Loaded local process configuration: " + localProcessEntity.Address + ":" + localProcessEntity.ServerPort + "/" + localProcessEntity.HubPort);

                // Creating service, bubbles and bubble links.
                service = new CloudService(ConfigurationManager.AppSettings["DaemonMemberWeb"], 
                    localProcessEntity.Address, 
                    localProcessEntity.HubPort, 
                    localProcessEntity.ServerPort,            
                    localProcessEntity.Name, 
                    DaemonProcessConstants.ProgramMajorVersion,
                    DaemonProcessConstants.ProgramMinorVersion);
               
                foreach (Bubble bubble in bubbleEntities)
                {
                    CloudBubble cloudBubble = new CloudBubble(bubble.BubbleId, bubble.Name, (float)bubble.Range, (float)bubble.PerceptionRange);
                    cloudBubble.ParticipantConnectAuthorize += OnParticipantConnectAuthorize;
                    cloudBubble.CloudParticipantDisconnected += OnCloudParticipantDisconnected;
                    service.AddBubble(cloudBubble);

                    foreach (RemoteProcess remoteProcess in remoteProcessEntities)
                    {
                        cloudBubble.AddAllowedRemoteHubAddress(remoteProcess.Address);

                    }

                    // Loading bubble link configuration from database.
                    List<DaemonLogic.BubbleLink> bubbleLinkConfigurations = (from b in entityContext.BubbleLink where b.Bubble.BubbleId == bubble.BubbleId select b).ToList<DaemonLogic.BubbleLink>();

                    foreach (DaemonLogic.BubbleLink bubbleLink in bubbleLinkConfigurations)
                    {
                        service.AddBubbleLink(bubble.BubbleId, bubbleLink.RemoteBubbleId, bubbleLink.Address, bubbleLink.Port, (float)-bubbleLink.X, (float)-bubbleLink.Y, (float)-bubbleLink.Z,
                                                      true, true);
                    }

                }                

                // Saving process state info to database.
                localProcessStateEntity = new LocalProcessState
                {
                    LocalProcessStateId = Guid.NewGuid(),
                    LocalProcess = localProcessEntity,
                    Participant = participantEntity,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Cpu = OnGetProcessingTime(),
                    Mem = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64) / 1024
                };
                entityContext.AddToLocalProcessState(localProcessStateEntity);
                entityContext.SaveChanges();

                service.Startup(false);

                LogUtil.Info("Daemon Process startup done.");

            }
        }

        public void Shutdown()
        {
            if (service != null)
            {
                lock (this)
                {
                    LogUtil.Info("Daemon Process shutdown...");

                    CloudService cloudService = service;

                    try
                    {
                        foreach (DaemonParticipant daemonParticipant in bubbleDaemons.Values)
                        {
                            daemonParticipant.Shutdown();
                        }

                        cloudService.Shutdown();
                    }
                    finally
                    {
                        // Removing process state info from database.
                        if (localProcessStateEntity != null)
                        {
                            entityContext.DeleteObject(localProcessStateEntity);
                            entityContext.SaveChanges();
                        }
                    }

                    service = null;
                    LogUtil.Info("Daemon Process shutdown done.");
                }
            }
        }

        public void Process()
        {
            lock (this)
            {
                service.Process();
                
                foreach (DaemonParticipant daemonParticipant in bubbleDaemons.Values)
                {
                    if (daemonParticipant.IsConnected)
                    {
                        daemonParticipant.Process();
                    }
                }

                // Updating process state info to database.
                if (DateTime.Now.Subtract(lastUpdate) > new TimeSpan(0, 0, 10))
                {
                    localProcessStateEntity.Cpu = OnGetProcessingTime();
                    localProcessStateEntity.Mem = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64) / 1024;
                    localProcessStateEntity.Modified = DateTime.Now;
                    entityContext.SaveChanges();
                    lastUpdate = DateTime.Now;

                    // Check whether process has been disabled in database.
                    entityContext.Refresh(RefreshMode.StoreWins, localProcessEntity);
                    if (localProcessEntity.Enabled == false)
                    {
                        RequestShutdown();
                    }

                    // Adding missing daemons
                    foreach (Bubble bubble in bubbleEntities)
                    {
                        if (!bubbleDaemons.ContainsKey(bubble.BubbleId))
                        {
                            DaemonParticipant daemonParticipant = new DaemonParticipant(localProcessEntity.Address.Equals("0.0.0.0") ? "127.0.0.1" : localProcessEntity.Address, localProcessEntity.ServerPort, bubble.BubbleId, bubble.BubbleId.ToString("N"), Guid.NewGuid().ToString("N"));
                            bubbleDaemons.Add(bubble.BubbleId, daemonParticipant);
                            daemonParticipant.Startup();
                        }
                    }

                    // Listing disconnected daemons
                    List<DaemonParticipant> disconnectedDaemons = new List<DaemonParticipant>();
                    foreach (DaemonParticipant daemonParticipant in bubbleDaemons.Values)
                    {
                        if (!daemonParticipant.IsConnected)
                        {
                            disconnectedDaemons.Add(daemonParticipant);
                        }
                    }

                    // Discarding disconnected daemonst
                    foreach (DaemonParticipant daemonParticipant in disconnectedDaemons)
                    {
                        bubbleDaemons.Remove(daemonParticipant.BubbleId);
                    }

                }
            }
        }

        private bool OnParticipantConnectAuthorize(Session session, JoinRequestMessage message, out Guid participantId, out Guid avatarId)
        {
            try
            {
                string idString = message.ParticipantIdentifier;
                idString = idString.Insert(8, "-");
                idString = idString.Insert(13, "-");
                idString = idString.Insert(18, "-");
                idString = idString.Insert(23, "-");
                participantId = new Guid(idString);
                avatarId = message.AvatarId;
            }
            catch(Exception)
            {
                LogUtil.Warn("Participant login id was not guid (without dashes): " + message.ParticipantIdentifier);
                participantId = Guid.Empty;
                avatarId = Guid.Empty;
                return false;
            }

            foreach (CloudBubble bubble in service.Bubbles)
            {
                if (message.BubbleId == bubble.BubbleId&&bubble.GetParticipant(participantId)!=null)
                {
                    LogUtil.Warn("Participant already connected " + participantId + ".");
                    return false;
                }
            }

            foreach (DaemonParticipant daemonParticipant in bubbleDaemons.Values)
            {
                if (message.ParticipantIdentifier.Equals(daemonParticipant.DaemonIdentifier) &&
                    message.ParticipantSecret.Equals(daemonParticipant.DaemonSecret))
                {
                    LogUtil.Info("Login secret match for daemon participant " + participantId + ".");
                    return true;
                }
            }

            Participant participant = ParticipantLogic.GetParticipant(participantId);
            if (participant.LoginSecret == null)
            {
                LogUtil.Warn("No login secret defined for participant " + participantId + " in database.");
                return false;
            }
            if (DateTime.Now > participant.LoginSecretExpires)
            {
                LogUtil.Warn("Login secret expired for participant " + participantId + " in database.");
                return false;
            }
            if (participant.LoginSecret.Equals(message.ParticipantSecret))
            {
                LogUtil.Info("Login secret match for participant " + participantId + ".");
                ParticipantLogic.ClearLoginSecret(participant);
                return true;
            }
            else
            {
                LogUtil.Warn("Login secret mismatch for participant " + participantId + ".");
                return false;
            }

        }

        public byte OnCloudParticipantIdentified(CloudBubble bubble, IdentifyRequestMessage message)
        {
            Guid participantId = message.ParticipantId;
            String participantIdentifier = message.ParticipantIdentity;
            String participantIdentityType = message.ParticipantIdentityType;
            
            if (!participantIdentityType.Equals(IdentifyRequestMessage.OPEN_ID_IDENTITY))
            {
                return MxpResponseCodes.UNSUPPORTED_OPERATION;
            }

            Participant participant=QueryUtil.First<DaemonLogic.Participant>(
                    from p in entityContext.Participant where p.ParticipantId == participantId select p);
            
            if (participant!=null)
            {
                if (participant.OpenIdUrl != participantIdentifier)
                {
                    return MxpResponseCodes.RESERVED_ID;
                }
                else
                {
                    return MxpResponseCodes.SUCCESS;
                }
            }

            participant = QueryUtil.First<DaemonLogic.Participant>(
                    from p in entityContext.Participant where p.OpenIdUrl == participantIdentifier select p);
            if (participant != null)
            {
                return MxpResponseCodes.UNAUTHORIZED_OPERATION;
            }

            participant = new Participant
            {
                ParticipantId=participantId,
                OpenIdUrl=participantIdentifier,                
            };

            entityContext.AddToParticipant(participant);
            entityContext.SaveChanges();

            return MxpResponseCodes.SUCCESS;
        }

        public byte OnCloudObjectHandover(CloudBubble bubble, HandoverRequestMessage message)
        {
            return MxpResponseCodes.SUCCESS;
        }

        private void OnCloudParticipantDisconnected(CloudBubble bubble, Guid participantId)
        {
            HashSet<Guid> participantObjectIds = bubble.CloudCache.GetParticipantObjectIds(participantId);
            List<Guid> participantObjectsToRemove=participantObjectIds.ToList<Guid>();
            foreach (Guid objectId in participantObjectsToRemove)
            {
                MXP.Cloud.CloudObject cloudObject = bubble.CloudCache.GetObject(objectId);
                // Remove if local bubble is primary bubble.
                if (bubble.BubbleId == cloudObject.BubbleId)
                {
                    bubble.CloudCache.RemoveObject(objectId);
                }
            }
        }

        private double OnGetProcessingTime()
        {
            double processorUsage = 0;
            System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
            if (lastTime != DateTime.MinValue)
            {
                TimeSpan totalTimeSpan = DateTime.Now.Subtract(lastTime);
                TimeSpan currentProcessorTime = process.TotalProcessorTime;
                TimeSpan processorTimeSpan = currentProcessorTime.Subtract(lastProcessorTime);
                processorUsage = 100.0 * ((double)processorTimeSpan.TotalMilliseconds) / ((double)totalTimeSpan.TotalMilliseconds);
                processorUsage = Math.Round(processorUsage * 10) / 10 / Environment.ProcessorCount;
            }
            lastProcessorTime = process.TotalProcessorTime;
            lastTime = DateTime.Now;
            return processorUsage;
        }

    }
}
