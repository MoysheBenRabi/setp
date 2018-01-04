using System;
using System.Collections.Generic;

using System.Text;
using MXP;
using System.Threading;
using System.Diagnostics;
using MXP.Util;

namespace MXTank
{
    /// <summary>
    /// Tank is example MXP bubble server implementation.
    /// </summary>
    public class Tank
    {

        #region Fields

        private List<TankBubble> Bubbles=new List<TankBubble>();
        private MxpHub hub;
        private MxpServer server;
        private Thread thread;
        private bool isShutdownRequested = false;
        
        #endregion

        #region Properties

        public ulong HubPacketsSent
        {
            get
            {
                return hub.PacketsSent;
            }
        }

        public ulong HubPacketsReceived
        {
            get
            {
                return hub.PacketsReceived;
            }
        }

        public ulong HubBytesSent
        {
            get
            {
                return hub.BytesSent;
            }
        }

        public ulong HubBytesReceived
        {
            get
            {
                return hub.BytesReceived;
            }
        }

        public ulong ServerPacketsSent
        {
            get
            {
                return server.PacketsSent;
            }
        }

        public ulong ServerPacketsReceived
        {
            get
            {
                return server.PacketsReceived;
            }
        }

        public ulong ServerBytesSent
        {
            get
            {
                return server.BytesSent;
            }
        }

        public ulong ServerBytesReceived
        {
            get
            {
                return server.BytesReceived;
            }
        }

        #endregion

        #region Constructors

        public Tank(String bubbleAssetCacheUrl,  
            String serverAddress,
            int serverPort,
            int hubPort)
        {
            String programName = "Metaverse Exchange Tank";
            byte programMajorVersion=0;
            byte programMinorVersion=4;
            server = new MxpServer(bubbleAssetCacheUrl, serverPort, programName, programMajorVersion, programMinorVersion);
            hub = new MxpHub(bubbleAssetCacheUrl, serverAddress, hubPort, programName, programMajorVersion, programMinorVersion);
        }

        #endregion

        #region Startup and Shutdown

        public void Startup()
        {
            if (!IsAlive)
            {
                hub.Startup();
                server.Startup();

                foreach (TankBubble bubble in Bubbles)
                {
                    bubble.Startup();
                }

                thread = new Thread(new ThreadStart(Process));
                thread.Start();

                LogUtil.Info("Tank started.");
            }
        }

        public void Shutdown()
        {
            if (IsAlive)
            {
                if (isShutdownRequested)
                {
                    throw new Exception("Shutdown already requested.");
                }

                foreach (TankBubble bubble in Bubbles)
                {
                    bubble.Shutdown();
                }

                isShutdownRequested = true;
                while (IsAlive)
                {
                    Thread.Sleep(100);
                }
                hub.Shutdown();
                server.Shutdown();
                isShutdownRequested = false;
                LogUtil.Info("Tank stopped.");
            }

        }

        public bool IsAlive
        {
            get
            {
                return thread != null && thread.IsAlive;
            }
        }

        #endregion

        #region Processing

        private void Process()
        {
            while (!isShutdownRequested)
            {

                try
                {

                    hub.Process();
                    server.Process();

                    for (int i = 0; i < Bubbles.Count; i++)
                    {
                        Bubbles[i].Process();
                    }

                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception e)
                {
                    LogUtil.Error(e.ToString());
                    Thread.Sleep(1000);
                }

                Thread.Sleep(50);
            }
        }

        #endregion

        #region Bubble Management

        public void AddBubble(TankBubble bubble)
        {
            server.AddBubble(bubble.MxpBubble);
            hub.AddBubble(bubble.MxpBubble);
            Bubbles.Add(bubble);
        }

        public void RemoveBubble(TankBubble bubble)
        {
            server.RemoveBubble(bubble.MxpBubble);
            hub.RemoveBubble(bubble.MxpBubble);
            Bubbles.Remove(bubble);
        }

        public List<TankBubble> GetBubbles()
        {
            return new List<TankBubble>(Bubbles);
        }

        #endregion

    }
}
