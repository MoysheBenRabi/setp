using System;
using System.Collections.Generic;

using System.Text;
using MXP;
using MXP.Messages;
using System.Threading;
using System.Diagnostics;
using MXP.Fragments;
using MXP.Util;

namespace MXTank
{

    /// <summary>
    /// TestParticipant is example XMP client implementation.
    /// </summary>
    public class BoxKickerDaemonParticipant
    {
        public MxpClient client;
        private string serverAddress;
        private int serverPort;
        private Guid bubbleId;
        private string location;
        private string identityProviderUrl;
        private string participantName;
        private string participantPassphrase;

        private const int NumberOfObjects = 100;
        public Guid TypeId = Guid.NewGuid();
        private TestObject[] ObjectArray = new TestObject[NumberOfObjects];
        private Dictionary<Guid, TestObject> Objects = new Dictionary<Guid, TestObject>();

        public Thread thread;
        public bool isExitRequested = false;

        private DateTime lastInjectEjectTime = DateTime.MinValue;
        private DateTime lastUpdateTime = DateTime.Now;
        private bool isInjected = false;
        private Dictionary<uint, TankObject> perceivedObjects = new Dictionary<uint, TankObject>();

        public bool IsAlive
        {
            get
            {
                return thread != null && thread.IsAlive;
            }
        }

        public BoxKickerDaemonParticipant(
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
            String programName="Box Kicker Daemon Participant";
            byte programMajorVersion=0;
            byte programMinorVersion=1;
            client = new MxpClient(programName, programMajorVersion, programMinorVersion);
            client.ServerMessageReceived += OnMessage;

            Random random = new Random();
            for (int i = 0; i < NumberOfObjects; i++)
            {
                ObjectArray[i] = new TestObject();
                ObjectArray[i].ObjectId = Guid.NewGuid();
                ObjectArray[i].ObjectName = "Test Object " + i;
                ObjectArray[i].OrbitRadius = (float)(random.NextDouble() * 100)+5;
                ObjectArray[i].OrbitAngle = (float)(random.NextDouble() * 2*Math.PI);
                ObjectArray[i].OrbitAngularVelocity = (float)(random.NextDouble() * 2*Math.PI/8);

                Objects.Add(ObjectArray[i].ObjectId, ObjectArray[i]);
            }
        }

        public void Startup()
        {
            if (!IsAlive)
            {
                client.Connect(serverAddress, serverPort, bubbleId, "", location, identityProviderUrl, participantName, participantPassphrase,Guid.Empty, false);
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

        public void OnMessage(Message message)
        {
            if (message.GetType() == typeof(PerceptionEventMessage))
            {
                PerceptionEventMessage perception = (PerceptionEventMessage)message;
                TankObject tankObject = new TankObject();
                tankObject.SetValues(perception.ObjectFragment);
                perceivedObjects[perception.ObjectFragment.ObjectIndex] = tankObject;
            }

            if (message.GetType() == typeof(MovementEventMessage))
            {
                MovementEventMessage movement = (MovementEventMessage)message;
                
                if(perceivedObjects.ContainsKey(movement.ObjectIndex))
                {
                    TankObject perceivedObject = perceivedObjects[movement.ObjectIndex];
                    perceivedObject.Location[0] = movement.Location.X;
                    perceivedObject.Location[1] = movement.Location.Y;
                    perceivedObject.Location[2] = movement.Location.Z;
                    foreach (TestObject obj in Objects.Values)
                    {
                        if (obj.ObjectId != perceivedObject.ObjectId)
                        {
                            float dx=obj.Location[0]-perceivedObject.Location[0];
                            float dy=obj.Location[1]-perceivedObject.Location[1];
                            float dz=obj.Location[2]-perceivedObject.Location[2];
                            float distance = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
                            
                            if (distance < obj.ObjectBoundingSphereRadius + perceivedObject.BoundingSphereRadius)
                            {
                                float velocity = 8;
                                // collision detected
                                obj.Velocity[0] = velocity * dx / distance;
                                //obj.Velocity[1] = velocity * dy / distance;
                                obj.Velocity[2] = velocity * dz / distance;

                                if (Objects.ContainsKey(perceivedObject.ObjectId))
                                {
                                    // if perceived object was test object then make it react to collision as well
                                    TestObject testObject = Objects[perceivedObject.ObjectId];
                                    testObject.Velocity[0] = -velocity * dx / distance;
                                    //testObject.Velocity[1] = -velocity * dy / distance;
                                    testObject.Velocity[2] = -velocity * dz / distance;
                                }

                            }
                        }
                    }
                }

            }

        }
        private void Process()
        {
            while (!isExitRequested)
            {
                try
                {
                    client.Process();

                    if (((DateTime.Now.Subtract(lastInjectEjectTime).TotalSeconds > 300 && isInjected) || (DateTime.Now.Subtract(lastInjectEjectTime).TotalSeconds > 1 && !isInjected)) && client.IsConnected)
                    {
                        if (!isInjected)
                        {
                            for (int i = 0; i < NumberOfObjects; i++)
                            {
                                // Reset objects
                                InjectRequestMessage injectRequestMessage = (InjectRequestMessage)MessageFactory.Current.ReserveMessage(typeof(InjectRequestMessage));
                                injectRequestMessage.ObjectFragment.OwnerId = ParticipantId;
                                injectRequestMessage.ObjectFragment.ObjectId = ObjectArray[i].ObjectId;
                                injectRequestMessage.ObjectFragment.ObjectName = ObjectArray[i].ObjectName;
                                injectRequestMessage.ObjectFragment.TypeId = TypeId;
                                injectRequestMessage.ObjectFragment.TypeName = "TestType";

                                ObjectArray[i].Location[0] = (float)(ObjectArray[i].OrbitRadius * Math.Cos(ObjectArray[i].OrbitAngle));
                                ObjectArray[i].Location[1] = 0.5f;
                                ObjectArray[i].Location[2] = (float)(ObjectArray[i].OrbitRadius * Math.Sin(ObjectArray[i].OrbitAngle));

                                injectRequestMessage.ObjectFragment.Location.X = ObjectArray[i].Location[0];
                                injectRequestMessage.ObjectFragment.Location.Y = ObjectArray[i].Location[1];
                                injectRequestMessage.ObjectFragment.Location.Z = ObjectArray[i].Location[2];

                                injectRequestMessage.ObjectFragment.BoundingSphereRadius = ObjectArray[i].ObjectBoundingSphereRadius;
                                client.Send(injectRequestMessage);

                                if (i % 10 == 9)
                                {
                                    client.Process();
                                    Thread.Sleep(10);
                                }

                            }

                            isInjected = true;
                        }
                        else
                        {
                            for (int i = 0; i < NumberOfObjects; i++)
                            {
                                EjectRequestMessage ejectRequestMessage = (EjectRequestMessage)MessageFactory.Current.ReserveMessage(typeof(EjectRequestMessage));
                                ejectRequestMessage.ObjectId = ObjectArray[i].ObjectId;
                                client.Send(ejectRequestMessage);

                                if (i % 10 == 9)
                                {
                                    client.Process();
                                    Thread.Sleep(10);
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
                            // Mark for reinject on reconnect
                            isInjected = false;

                            client.Connect(serverAddress, serverPort, bubbleId, "", location, identityProviderUrl, participantName, participantPassphrase, Guid.Empty, false);
                        }
                    }

                    if (client.IsConnected && DateTime.Now.Subtract(lastUpdateTime).TotalMilliseconds > 500 && isInjected)
                    {
                        float timeDelta=(float)DateTime.Now.Subtract(lastUpdateTime).TotalSeconds;

                        for (int i = 0; i < NumberOfObjects; i++)
                        {

                            //if (ObjectArray[i].Velocity[0] >= 0.01f || ObjectArray[i].Velocity[1] >= 0.01f || ObjectArray[i].Velocity[2] >= 0.01f)
                            //{
                            ObjectArray[i].Location[0] += ObjectArray[i].Velocity[0] * timeDelta;
                            ObjectArray[i].Location[1] += ObjectArray[i].Velocity[1] * timeDelta;
                            ObjectArray[i].Location[2] += ObjectArray[i].Velocity[2] * timeDelta;

                            ObjectArray[i].Velocity[0] *= 0.9f;
                            ObjectArray[i].Velocity[1] *= 0.9f;
                            ObjectArray[i].Velocity[2] *= 0.9f;

                            //}

                            if (Math.Sqrt(ObjectArray[i].Velocity[0] * ObjectArray[i].Velocity[0] +
                                ObjectArray[i].Velocity[1] * ObjectArray[i].Velocity[1] +
                                 ObjectArray[i].Velocity[2] * ObjectArray[i].Velocity[2]) < 0.01f)
                            {
                                ObjectArray[i].Velocity[0] = 0;
                                ObjectArray[i].Velocity[1] = 0;
                                ObjectArray[i].Velocity[2] = 0;
                            }
                            else
                            {
                                InjectRequestMessage injectRequestMessage = (InjectRequestMessage)MessageFactory.Current.ReserveMessage(typeof(InjectRequestMessage));
                                injectRequestMessage.ObjectFragment.OwnerId = ParticipantId;
                                injectRequestMessage.ObjectFragment.ObjectId = ObjectArray[i].ObjectId;
                                injectRequestMessage.ObjectFragment.ObjectName = ObjectArray[i].ObjectName;
                                injectRequestMessage.ObjectFragment.TypeId = TypeId;
                                injectRequestMessage.ObjectFragment.TypeName = "TestType";
                                injectRequestMessage.ObjectFragment.Location.X = ObjectArray[i].Location[0];
                                injectRequestMessage.ObjectFragment.Location.Y = ObjectArray[i].Location[1];
                                injectRequestMessage.ObjectFragment.Location.Z = ObjectArray[i].Location[2];
                                injectRequestMessage.ObjectFragment.BoundingSphereRadius = ObjectArray[i].ObjectBoundingSphereRadius;
                                client.Send(injectRequestMessage);
                            }

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
