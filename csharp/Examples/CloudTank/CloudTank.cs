using System;
using System.Collections.Generic;
using System.Text;
using MXP.Cloud;
using Nini.Config;
using MXP;
using MXP.Util;
using CloudTank.Daemons;

namespace CloudTank
{
    public class CloudTank
    {
        private CloudService service=null;
        private List<BallDaemon> ballDaemons=new List<BallDaemon>();

        public bool IsStarted
        {
            get { return service != null; }
        }

        public void Startup()
        {
            lock (this)
            {
                LogUtil.Info("CloudTank startup...");

                if (service != null)
                {
                    throw new Exception("CloudTank already started.");
                }

                IniConfigSource configSource = new IniConfigSource("CloudTank.ini");

                {
                    IConfig cloudTankConfig = configSource.Configs["cloud-tank"];
                    LogUtil.LogDebug = cloudTankConfig.GetBoolean("log-debug");
                    string address = cloudTankConfig.GetString("address");
                    int serverPort = cloudTankConfig.GetInt("server-port");
                    int hubPort = cloudTankConfig.GetInt("hub-port");
                    service = new CloudService("http://example.asset.url", address, hubPort, serverPort,
                                               CloudTankConstants.ProgramName, CloudTankConstants.ProgramMajorVersion,
                                               CloudTankConstants.ProgramMinorVersion);
                }

                foreach (IConfig config in configSource.Configs)
                {

                    if (config.Name.StartsWith("bubble"))
                    {
                        string bubbleName = config.GetString("bubble-name");
                        Guid bubbleId = new Guid(config.GetString("bubble-id"));
                        float bubbleRange = config.GetFloat("bubble-range");
                        float bubblePerceptionRange = config.GetFloat("bubble-perception-range");

                        CloudBubble bubble = new CloudBubble(bubbleId, bubbleName, bubbleRange, bubblePerceptionRange);
                        service.AddBubble(bubble);



                        foreach (string propertyName in config.GetKeys())
                        {

                            if (propertyName.StartsWith("allowed-remote-hub-address"))
                            {
                                bubble.AddAllowedRemoteHubAddress(config.GetString(propertyName));
                            }

                            if (propertyName.StartsWith("bubble-link"))
                            {
                                Uri uri = new Uri(config.GetString(propertyName));
                                string remoteAddress = uri.Host;
                                int remoteHubPort = uri.IsDefaultPort ? MxpConstants.DefaultHubPort : uri.Port;
                                string remoteBubbleString = uri.AbsolutePath.Substring(1);
                                Guid remoteBubbleId = new Guid(remoteBubbleString);
                                string query = uri.Query;

                                float x = 0;
                                float y = 0;
                                float z = 0;
                                string[] parameters = query.Substring(1).Split('&');

                                foreach (string parameter in parameters)
                                {
                                    string[] parameterParts = parameter.Split('=');
                                    if (parameterParts[0].Equals("x"))
                                    {
                                        x = Convert.ToSingle(parameterParts[1]);
                                    }
                                    if (parameterParts[0].Equals("y"))
                                    {
                                        y = Convert.ToSingle(parameterParts[1]);
                                    }
                                    if (parameterParts[0].Equals("z"))
                                    {
                                        z = Convert.ToSingle(parameterParts[1]);
                                    }
                                }

                                service.AddBubbleLink(bubbleId, remoteBubbleId, remoteAddress, remoteHubPort, -x, -y, -z,
                                                      true, true);
                            }

                        }

                    }


                    if (config.Name.StartsWith("ball-daemon"))
                    {
                        string serverAddress = config.GetString("server-address");
                        int serverPort = config.GetInt("server-port");
                        Guid bubbleId = new Guid(config.GetString("bubble-id"));
                        string daemonIdentifier = config.GetString("daemon-identifier");
                        string daemonSecret = config.GetString("daemon-secret");
                        BallDaemon ballDaemon = new BallDaemon(serverAddress, serverPort, bubbleId, daemonIdentifier, daemonSecret);
                        ballDaemons.Add(ballDaemon);
                    }


                }

                service.Startup(false);

                foreach(BallDaemon ballDaemon in ballDaemons)
                {
                    ballDaemon.Startup();
                }

                LogUtil.Info("CloudTank startup done.");

            }
        }

        public void Shutdown()
        {
            lock (this)
            {
                LogUtil.Info("CloudTank shutdown...");

                foreach (BallDaemon ballDaemon in ballDaemons)
                {
                    ballDaemon.Shutdown();
                }

                if (service == null)
                {
                    throw new Exception("CloundTank is not started.");
                }
                CloudService cloudService = service;
                service = null;
                cloudService.Shutdown();
                LogUtil.Info("CloudTank shutdown done.");
            }
        }

        public void Process()
        {
            lock (this)
            {
                service.Process();

                foreach (BallDaemon ballDaemon in ballDaemons)
                {
                    ballDaemon.Process();
                }
            }
        }


    }
}
