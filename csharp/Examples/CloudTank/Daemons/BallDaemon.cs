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

namespace CloudTank.Daemons
{

    /// <summary>
    /// TestParticipant is example XMP client implementation.
    /// </summary>
    public class BallDaemon
    {
        public class BallObject
        {
            public Guid ObjectId;
            public string ObjectName;
            public float OrbitRadius;
            public float OrbitAngle;
            public float OrbitAngularVelocity;
            public float Mass = 10;
            public float ObjectBoundingSphereRadius = 1;
            public MsdVector3f Location = new MsdVector3f();
            public MsdVector3f Velocity = new MsdVector3f();
            public bool IsInjected = false;
        }

        public CloudView client;
        private string serverAddress;
        private int serverPort;
        private Guid bubbleId;
        private string daemonIdentifier;
        private string daemonSecret;

        private const int NumberOfObjects = 50;
        public Guid TypeId = Guid.NewGuid();
        private BallObject[] BallArray = new BallObject[NumberOfObjects];
        private Dictionary<Guid, BallObject> Balls = new Dictionary<Guid, BallObject>();

        private int updateIndex = 0;

        public BallDaemon(
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
            this.daemonIdentifier = daemonIdentifier;
            this.daemonSecret = daemonSecret;
            String programName = "Ball Daemon";
            byte programMajorVersion = 0;
            byte programMinorVersion = 1;
            client = new CloudView(100, programName, programMajorVersion, programMinorVersion);

            Random random = new Random();
            int i = 0;
            BallArray[i] = new BallObject();
            BallArray[i].ObjectId = Guid.NewGuid();
            BallArray[i].ObjectName = "Ball Daemon Avatar";
            BallArray[i].OrbitRadius = 0;
            BallArray[i].OrbitAngle = 0;
            BallArray[i].OrbitAngularVelocity = 0;
            BallArray[i].Location.X = 0;
            BallArray[i].Location.Y = 0;
            BallArray[i].Location.Z = 0;
            Balls.Add(BallArray[i].ObjectId, BallArray[i]);

            for (i = 1; i < NumberOfObjects; i++)
            {
                BallArray[i] = new BallObject();
                BallArray[i].ObjectId = Guid.NewGuid();
                BallArray[i].ObjectName = "Test Object " + i;
                BallArray[i].OrbitRadius = (float)(random.NextDouble() * 45)+5;
                BallArray[i].OrbitAngle = (float)(random.NextDouble() * 2 * Math.PI);
                BallArray[i].OrbitAngularVelocity = (float)(random.NextDouble() * 2 * Math.PI / 8);
                BallArray[i].Location.X = (float)(BallArray[i].OrbitRadius * Math.Cos(BallArray[i].OrbitAngle));
                BallArray[i].Location.Y = 0.5f;
                BallArray[i].Location.Z = (float)(BallArray[i].OrbitRadius * Math.Sin(BallArray[i].OrbitAngle));
                Balls.Add(BallArray[i].ObjectId, BallArray[i]);
            }
        }

        public void Startup()
        {
            client.Connect(serverAddress, serverPort, bubbleId, "", "", "", daemonIdentifier, daemonSecret, BallArray[0].ObjectId,false);
        }

        public void Shutdown()
        {
            client.Disconnect();
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

        public void Process()
        {
            client.Process();

            if (client.IsConnected)
            {
                int addCount = 0;
                foreach (BallObject ball in BallArray)
                {
                    if (!ball.IsInjected)
                    {
                        ball.IsInjected = true;
                        InjectRequestMessage injectRequestMessage = new InjectRequestMessage();
                        injectRequestMessage.ObjectFragment.ObjectId = ball.ObjectId;
                        injectRequestMessage.ObjectFragment.OwnerId = client.ParticipantId;
                        injectRequestMessage.ObjectFragment.ObjectName = ball.ObjectName;
                        injectRequestMessage.ObjectFragment.TypeName = "Ball";
                        injectRequestMessage.ObjectFragment.BoundingSphereRadius = ball.ObjectBoundingSphereRadius;
                        injectRequestMessage.ObjectFragment.Mass = ball.Mass;
                        injectRequestMessage.ObjectFragment.Location.X = ball.Location.X;
                        injectRequestMessage.ObjectFragment.Location.Y = ball.Location.Y;
                        injectRequestMessage.ObjectFragment.Location.Z = ball.Location.Z;

                        OmModelPrimitiveExt modelPrimitiveExt=new OmModelPrimitiveExt();
                        modelPrimitiveExt.ModelUrl = "http://assets.bubblecloud.org/Collada/Duck/duck_triangulate.dae";
                        modelPrimitiveExt.Scale = 0.01f;
                        injectRequestMessage.SetExtension(modelPrimitiveExt);
                        
                        //injectRequestMessage.ObjectFragment.SetExtensionData(new byte[50]);
                        client.InjectObject(injectRequestMessage);
                        addCount++;
                        if (addCount > 5)
                        {
                            break;
                        }
                    }
                }

                for (int i = 0; i < 20 ; i++)
                {
                    BallArray[updateIndex].OrbitAngle += 0.01f;
                    BallArray[updateIndex].Location.X =
                        (float) (BallArray[updateIndex].OrbitRadius*Math.Cos(BallArray[updateIndex].OrbitAngle));
                    BallArray[updateIndex].Location.Y = 0.5f;
                    BallArray[updateIndex].Location.Z =
                        (float) (BallArray[updateIndex].OrbitRadius*Math.Sin(BallArray[updateIndex].OrbitAngle));

                    if (client.CloudCache.GetObject(BallArray[updateIndex].ObjectId) != null)
                    {
                        MovementEventMessage movementEvent = new MovementEventMessage();
                        movementEvent.ObjectIndex =
                            client.CloudCache.GetObject(BallArray[updateIndex].ObjectId).RemoteObjectIndex;
                        movementEvent.Location.X = BallArray[updateIndex].Location.X;
                        movementEvent.Location.Y = BallArray[updateIndex].Location.Y;
                        movementEvent.Location.Z = BallArray[updateIndex].Location.Z;
                        client.MoveObject(movementEvent);
                    }

                    this.updateIndex++;
                    if (this.updateIndex >= NumberOfObjects)
                    {
                        this.updateIndex = 0;
                    }
                }
            }

        }

    }
}
