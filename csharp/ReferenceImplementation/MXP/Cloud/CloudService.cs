using System;
using System.Collections.Generic;
using System.Text;
using MXP.Common.Proto;

namespace MXP.Cloud
{

    /// <summary>
    /// CloudService is reference implementation of the umbrella MXP service at server side.
    /// CloudService initializes both MxpServer and MxpHub components.
    /// </summary>
    public class CloudService
    {

        #region Fields
        
        private MxpServer server;
        private MxpHub hub;
        private Dictionary<Guid, CloudBubble> bubbles = new Dictionary<Guid, CloudBubble>();

        private bool isStarted = false;
        
        #endregion

        #region Properties

        public MxpServer Server
        {
            get
            {
                return server;
            }
        }

        public MxpHub Hub
        {
            get
            {
                return hub;
            }
        }

        public ICollection<CloudBubble> Bubbles
        {
            get
            {
                return bubbles.Values;
            }
        }

        #endregion

        #region Constructors

        public CloudService(
            String bubbleAssetCacheUrl,
            String serverAddress,
            int hubPort,
            int serverPort,
            String programName,
            byte programMajorVersion,
            byte programMinorVersion
            )
        {
            this.hub = new MxpHub(bubbleAssetCacheUrl, serverAddress, hubPort, programName, programMajorVersion, programMinorVersion);
            this.server = new MxpServer(bubbleAssetCacheUrl, serverPort, programName, programMajorVersion, programMinorVersion);
        }

        #endregion

        #region Startup and Shutdown

        public void Startup(bool debugMessages)
        {
            foreach (CloudBubble bubble in bubbles.Values)
            {
                bubble.Startup();
            }

            hub.Startup(debugMessages);
            server.Startup(debugMessages);

            isStarted = true;

        }

        public void Shutdown()
        {

            server.Shutdown();
            hub.Shutdown();

            foreach (CloudBubble bubble in bubbles.Values)
            {
                bubble.Shutdown();
            }
        }

        #endregion

        #region Processing

        public void Process()
        {
            foreach (CloudBubble bubble in bubbles.Values)
            {
                bubble.Process();
            }

            hub.Process();
            server.Process();
        }

        #endregion

        #region Bubble Management

        public void AddBubble(CloudBubble bubble)
        {
            bubble.Service = this;
            bubbles.Add(bubble.BubbleId,bubble);
            hub.AddBubble(bubble);
            server.AddBubble(bubble);

            // Starting manually if service has been started already.
            if (isStarted)
            {
                bubble.Startup();
            }
        }

        public void RemoveBubble(CloudBubble bubble)
        {
            bubble.Service = null;
            bubbles.Remove(bubble.BubbleId);
            hub.RemoveBubble(bubble);
            server.RemoveBubble(bubble);
        }

        #endregion

        #region Bubble Link Management

        public void AddBubbleLink(Guid localBubbleId,Guid remoteBubbleId,String remoteHubAddress, int remoteHubPort, float remoteBubbleCenterX, float remoteBubbleCenterY, float remoteBubbleCenterZ, bool isEnabled, bool isInitiator)
        {
            CloudBubble bubbleOne = bubbles[localBubbleId];

            BubbleLink bubbleLink = new BubbleLink();
            bubbleLink.RemoteHubAddress = remoteHubAddress;
            bubbleLink.RemoteHubPort = remoteHubPort;
            bubbleLink.RemoteBubbleId = remoteBubbleId;
            bubbleLink.RemoteBubbleCenter.X = remoteBubbleCenterX;
            bubbleLink.RemoteBubbleCenter.Y = remoteBubbleCenterY;
            bubbleLink.RemoteBubbleCenter.Z = remoteBubbleCenterZ;
            bubbleLink.IsEnabled = isEnabled;
            bubbleLink.IsInitiator = isInitiator;

            bubbleOne.AddBubbleLink(bubbleLink);
        }

        #endregion

    }
}
