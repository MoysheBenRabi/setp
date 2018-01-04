using System;
using System.Collections.Generic;
using System.Text;
using MXP.Cloud;
using MXP.Messages;
using System.Threading;

namespace InteroperabilityTester
{
    public class ExecuteClientToReferenceServerTestCaseCommand
    {
        public static void Execute(string ip, int port)
        {
            log4net.Config.XmlConfigurator.Configure();

            CloudView view=null;
            try
            {
                string clientProgramName = "ClientProgram";
                byte clientProgramMajorVersion = 5;
                byte clientProgramMinorVersion = 6;
                string participantIdentityProviderUrl = "http://test.identityprovider";
                string participantName = "TestParticipantName";
                string participantPassphrase = "TestParticipantPassphrase";
                Guid avatarId = new Guid("123DFA16-5B52-4c9f-9A09-AD7465208321");

                view = new CloudView(1000, clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
                view.Connect(ip, port, new Guid("539DFA16-5B52-4c9f-9A09-AD746520873E"), "", "", participantIdentityProviderUrl, participantName, participantPassphrase, avatarId,true);

                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(100);
                    view.Process();
                    if (view.IsConnected)
                    {
                        break;
                    }
                }

                if (!view.IsConnected)
                {
                    Console.WriteLine("Unable to connect server.");
                    return;
                }

                Guid objectId = avatarId;
                uint objectIndex = 100;
                string objectName = "TestObjectName";
                Guid objectParentObjectId = Guid.NewGuid();
                Guid objectTypeId = Guid.NewGuid();
                string objectTypeName = "TestObjectType";
                float objectBoundingSphereRadius = 23;
                float objectMass = 24;
                float objectLocationX = 2;
                float objectLocationY = 3;
                float objectLocationZ = 4;
                float modifiedObjectLocationX = 12;
                float modifiedObjectLocationY = 13;
                float modifiedObjectLocationZ = 14;
                float objectVelocityX = 5;
                float objectVelocityY = 6;
                float objectVelocityZ = 7;
                float objectAccelerationX = 8;
                float objectAccelerationY = 9;
                float objectAccelerationZ = 10;
                float objectOrientationX = 11;
                float objectOrientationY = 12;
                float objectOrientationZ = 13;
                float objectOrientationW = 14;
                float objectAngularVelocityX = 15;
                float objectAngularVelocityY = 16;
                float objectAngularVelocityZ = 17;
                float objectAngularVelocityW = 18;
                float objectAngularAccelerationX = 19;
                float objectAngularAccelerationY = 20;
                float objectAngularAccelerationZ = 21;
                float objectAngularAccelerationW = 22;
                string objectExtensionDialect = "TEDI";
                byte objectExtensionDialectMinorVersion = 23;
                byte objectExtensionDialectMajorVersion = 24;
                byte[] objectExtensionData = ASCIIEncoding.ASCII.GetBytes("012345678901234567890123456789012345678901234567890123456789");

                InjectRequestMessage injectRequestMessage = new InjectRequestMessage();
                injectRequestMessage.ObjectFragment.ObjectId = objectId;
                injectRequestMessage.ObjectFragment.ObjectIndex = objectIndex;
                injectRequestMessage.ObjectFragment.ObjectName = objectName;
                injectRequestMessage.ObjectFragment.TypeId = objectTypeId;
                injectRequestMessage.ObjectFragment.TypeName = objectTypeName;
                injectRequestMessage.ObjectFragment.OwnerId = view.ParticipantId;
                injectRequestMessage.ObjectFragment.ParentObjectId = objectParentObjectId;
                injectRequestMessage.ObjectFragment.Mass = objectMass;
                injectRequestMessage.ObjectFragment.BoundingSphereRadius = objectBoundingSphereRadius;
                injectRequestMessage.ObjectFragment.Location.X = objectLocationX;
                injectRequestMessage.ObjectFragment.Location.Y = objectLocationY;
                injectRequestMessage.ObjectFragment.Location.Z = objectLocationZ;
                injectRequestMessage.ObjectFragment.Velocity.X = objectVelocityX;
                injectRequestMessage.ObjectFragment.Velocity.Y = objectVelocityY;
                injectRequestMessage.ObjectFragment.Velocity.Z = objectVelocityZ;
                injectRequestMessage.ObjectFragment.Acceleration.X = objectAccelerationX;
                injectRequestMessage.ObjectFragment.Acceleration.Y = objectAccelerationY;
                injectRequestMessage.ObjectFragment.Acceleration.Z = objectAccelerationZ;
                injectRequestMessage.ObjectFragment.Orientation.X = objectOrientationX;
                injectRequestMessage.ObjectFragment.Orientation.Y = objectOrientationY;
                injectRequestMessage.ObjectFragment.Orientation.Z = objectOrientationZ;
                injectRequestMessage.ObjectFragment.Orientation.W = objectOrientationW;
                injectRequestMessage.ObjectFragment.AngularVelocity.X = objectAngularVelocityX;
                injectRequestMessage.ObjectFragment.AngularVelocity.Y = objectAngularVelocityY;
                injectRequestMessage.ObjectFragment.AngularVelocity.Z = objectAngularVelocityZ;
                injectRequestMessage.ObjectFragment.AngularVelocity.W = objectAngularVelocityW;
                injectRequestMessage.ObjectFragment.AngularAcceleration.X = objectAngularAccelerationX;
                injectRequestMessage.ObjectFragment.AngularAcceleration.Y = objectAngularAccelerationY;
                injectRequestMessage.ObjectFragment.AngularAcceleration.Z = objectAngularAccelerationZ;
                injectRequestMessage.ObjectFragment.AngularAcceleration.W = objectAngularAccelerationW;
                injectRequestMessage.ObjectFragment.ExtensionDialect = objectExtensionDialect;
                injectRequestMessage.ObjectFragment.ExtensionDialectMajorVersion = objectExtensionDialectMajorVersion;
                injectRequestMessage.ObjectFragment.ExtensionDialectMinorVersion = objectExtensionDialectMinorVersion;
                injectRequestMessage.ObjectFragment.SetExtensionData(objectExtensionData);
                view.InjectObject(injectRequestMessage);

                Thread.Sleep(20);
                view.Process();
                Thread.Sleep(20);
                view.Process();
                Thread.Sleep(20);
                view.Process();
                Thread.Sleep(20);
                view.Process();
                Thread.Sleep(20);
                view.Process();
                Thread.Sleep(20);
                view.Process();
                Thread.Sleep(20);
                view.Process();

                CloudObject cloudObject = view.CloudCache.GetObject(objectId);

                ModifyRequestMessage modifyRequestMessage = new ModifyRequestMessage();
                cloudObject.ToObjectFragment(modifyRequestMessage.ObjectFragment);
                modifyRequestMessage.ObjectFragment.Location.X = modifiedObjectLocationX;
                modifyRequestMessage.ObjectFragment.Location.Y = modifiedObjectLocationY;
                modifyRequestMessage.ObjectFragment.Location.Z = modifiedObjectLocationZ;
                view.ModifyObject(modifyRequestMessage);

                Thread.Sleep(100);
                view.Process();

                ExamineRequestMessage examineRequestMessage = new ExamineRequestMessage();
                examineRequestMessage.ObjectIndex = cloudObject.RemoteObjectIndex;
                view.ExamineObject(examineRequestMessage);

                Thread.Sleep(100);
                view.Process();

                InteractRequestMessage interactRequesMessage = new InteractRequestMessage();
                interactRequesMessage.InteractionFragment.SourceParticipantId = view.ParticipantId;
                interactRequesMessage.InteractionFragment.SourceObjectId = Guid.Empty;
                interactRequesMessage.InteractionFragment.TargetParticipantId = Guid.Empty;
                interactRequesMessage.InteractionFragment.TargetObjectId = objectId;
                interactRequesMessage.InteractionFragment.ExtensionDialect = objectExtensionDialect;
                interactRequesMessage.InteractionFragment.ExtensionDialectMajorVersion = objectExtensionDialectMajorVersion;
                interactRequesMessage.InteractionFragment.ExtensionDialectMinorVersion = objectExtensionDialectMinorVersion;
                interactRequesMessage.InteractionFragment.SetExtensionData(objectExtensionData);
                view.InteractWithObject(interactRequesMessage);

                Thread.Sleep(100);
                view.Process();

                EjectRequestMessage ejectRequestMessage = new EjectRequestMessage();
                ejectRequestMessage.ObjectId = objectId;
                view.EjectObject(ejectRequestMessage);

                Thread.Sleep(100);
                view.Process();

            }
            finally
            {
                if (view != null)
                {
                    view.Disconnect();

                    while (view.Client.IsTransmitterAlive)
                    {
                        Thread.Sleep(100);
                        view.Process();
                    }

                }
            }

        }
    }
}
