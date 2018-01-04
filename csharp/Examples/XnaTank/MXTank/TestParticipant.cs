using System;
using System.Collections.Generic;

using System.Text;
using MXP;
using MXP.Messages;
using System.Threading;
using System.Diagnostics;
using MXP.Util;

namespace MXTank
{
    public class TestObject
    {
        public Guid ObjectId;
        public string ObjectName;
        public float OrbitRadius;
        public float OrbitAngle;
        public float OrbitAngularVelocity;
        public float ObjectBoundingSphereRadius = 1;
        public float[] Location = new float[3];
        public float[] Velocity = new float[3];
    }

    /// <summary>
    /// TestParticipant is example XMP client implementation.
    /// </summary>
    public class TestParticipant
    {
        public MxpClient client;
        private string serverAddress;
        private int serverPort;
        private Guid bubbleId;
        private string location;
        private string identityProviderUrl;
        private string participantName;
        private string participantPassphrase;

        private const int NumberOfObjects = 500;
        public Guid TypeId = Guid.NewGuid();
        private TestObject[] Objects = new TestObject[NumberOfObjects];

        public Thread thread;
        public bool isExitRequested = false;

        public bool IsAlive
        {
            get
            {
                return thread != null && thread.IsAlive;
            }
        }

        public TestParticipant(
            string serverAddress,
            int serverPort,
            Guid bubbleId,
            string location,
            string identityProviderUrl,
            string participantName,
            string participantPassphrase
            )
        {
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
            this.bubbleId = bubbleId;
            this.location = location;
            this.identityProviderUrl = identityProviderUrl;
            this.participantName = participantName;
            this.participantPassphrase = participantPassphrase;
            String programName="Tank Test Participant";
            byte programMajorVersion=0;
            byte programMinorVersion=1;
            client = new MxpClient(programName, programMajorVersion, programMinorVersion);

            Random random = new Random();
            for (int i = 0; i < NumberOfObjects; i++)
            {
                Objects[i] = new TestObject();
                Objects[i].ObjectId = Guid.NewGuid();
                Objects[i].ObjectName = "Test Object " + i;
                Objects[i].OrbitRadius = (float)(random.NextDouble() * 200)+10;
                Objects[i].OrbitAngle = (float)(random.NextDouble() * 2*Math.PI);
                Objects[i].OrbitAngularVelocity = (float)(random.NextDouble() * 2*Math.PI/8);
            }
        }

        public void Startup()
        {
            if (!IsAlive)
            {
                client.Connect(serverAddress, serverPort, bubbleId, "", location, identityProviderUrl, participantName, participantPassphrase, Guid.Empty, false);
                thread = new Thread(new ThreadStart(Process));
                thread.Start();
            }
        }

        public void Shutdown()
        {
            if (IsAlive)
            {
                isExitRequested = true;
                client.Disconnect();
                while (IsAlive)
                {
                    Thread.Sleep(100);
                }
                isExitRequested = false;
                thread = null;
            }
        }

        public bool IsConnected
        {
            get
            {
                return client.IsConnected;
            }
        }

        public Guid ParticipantId
        {
            get
            {
                return client.ParticipantId;
            }
        }

        private DateTime lastInjectEjectTime = DateTime.MinValue;
        private DateTime lastUpdateTime = DateTime.Now;
        private bool isInjected = false;

        private void Process()
        {
            while (!isExitRequested)
            {
                try
                {
                    client.Process();

                    if (client.IsConnected && DateTime.Now.Subtract(lastUpdateTime).TotalMilliseconds > 500 && isInjected)
                    {
                        for (int i = 0; i < NumberOfObjects; i++)
                        {
                            Objects[i].OrbitAngle += Objects[i].OrbitAngularVelocity * ((float)DateTime.Now.Subtract(lastUpdateTime).TotalMilliseconds) / 1000f;
                        }
                    }


                    if (((DateTime.Now.Subtract(lastInjectEjectTime).TotalSeconds > 5 && isInjected) || (DateTime.Now.Subtract(lastInjectEjectTime).TotalSeconds > 1 && !isInjected)) && client.IsConnected)
                    {
                        if (!isInjected)
                        {
                            for (int i = 0; i < NumberOfObjects; i++)
                            {
                                InjectRequestMessage injectRequestMessage = (InjectRequestMessage)MessageFactory.Current.ReserveMessage(typeof(InjectRequestMessage));
                                injectRequestMessage.ObjectFragment.OwnerId = ParticipantId;
                                injectRequestMessage.ObjectFragment.ObjectId = Objects[i].ObjectId;
                                injectRequestMessage.ObjectFragment.ObjectName = Objects[i].ObjectName;
                                injectRequestMessage.ObjectFragment.TypeId = TypeId;
                                injectRequestMessage.ObjectFragment.TypeName = "TestType";
                                injectRequestMessage.ObjectFragment.Location.X = (float)(Objects[i].OrbitRadius * Math.Cos(Objects[i].OrbitAngle));
                                injectRequestMessage.ObjectFragment.Location.Y = 0;
                                injectRequestMessage.ObjectFragment.Location.Z = (float)(Objects[i].OrbitRadius * Math.Sin(Objects[i].OrbitAngle));
                                injectRequestMessage.ObjectFragment.BoundingSphereRadius = 1;
                                client.Send(injectRequestMessage);

                                if (i % 10 == 9)
                                {
                                    client.Process();
                                    Thread.Sleep(1);
                                }

                            }
                            isInjected = true;
                        }
                        else
                        {
                            for (int i = 0; i < NumberOfObjects; i++)
                            {
                                EjectRequestMessage ejectRequestMessage = (EjectRequestMessage)MessageFactory.Current.ReserveMessage(typeof(EjectRequestMessage));
                                ejectRequestMessage.ObjectId = Objects[i].ObjectId;
                                client.Send(ejectRequestMessage);

                                if (i % 10 == 9)
                                {
                                    client.Process();
                                    Thread.Sleep(1);
                                }

                            }
                            isInjected = false;
                        }
                        lastInjectEjectTime = DateTime.Now;
                    }
                    else
                    {
                        if (!client.IsConnecting && !client.IsConnected && !isExitRequested)
                        {
                            client.Connect(serverAddress, serverPort, bubbleId, "", location, identityProviderUrl, participantName, participantPassphrase, Guid.Empty, false);
                        }
                    }

                    if (client.IsConnected && DateTime.Now.Subtract(lastUpdateTime).TotalMilliseconds > 500 && isInjected)
                    {
                        for (int i = 0; i < NumberOfObjects; i++)
                        {
                            InjectRequestMessage injectRequestMessage = (InjectRequestMessage)MessageFactory.Current.ReserveMessage(typeof(InjectRequestMessage));
                            injectRequestMessage.ObjectFragment.OwnerId = ParticipantId;
                            injectRequestMessage.ObjectFragment.ObjectId = Objects[i].ObjectId;
                            injectRequestMessage.ObjectFragment.ObjectName = Objects[i].ObjectName;
                            injectRequestMessage.ObjectFragment.TypeId = TypeId;
                            injectRequestMessage.ObjectFragment.TypeName = "TestType";
                            injectRequestMessage.ObjectFragment.Location.X = (float)(Objects[i].OrbitRadius * Math.Cos(Objects[i].OrbitAngle));
                            injectRequestMessage.ObjectFragment.Location.Y = 0;
                            injectRequestMessage.ObjectFragment.Location.Z = (float)(Objects[i].OrbitRadius * Math.Sin(Objects[i].OrbitAngle));
                            injectRequestMessage.ObjectFragment.BoundingSphereRadius = 1;
                            client.Send(injectRequestMessage);
                        }
                        lastUpdateTime = DateTime.Now;
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

    }
}
