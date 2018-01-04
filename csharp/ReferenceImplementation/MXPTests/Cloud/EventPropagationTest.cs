using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP;
using System.Threading;
using MXP.Messages;
using MXP.Common.Proto;
using MXP.Util;
using MXP.Cloud;

namespace MXPTests.Cloud
{

    [TestFixture]
    public class EventPropagationTest
    {

        #region Constants
        private readonly string serviceAlphaProgram = "AlphaProgram";
        private readonly byte serviceAlphaProgramMajorVersion = 1;
        private readonly byte serviceAlphaProgramMinorVersion = 2;
        private readonly string serviceAlphaAddress = "127.0.0.1";
        private readonly int serviceAlphaHubPort = MxpConstants.DefaultHubPort;
        private readonly int serviceAlphaServerPort = MxpConstants.DefaultServerPort;
        private readonly string serviceAlphaAssetCacheUrl = "http://test.assetcache.one";

        private readonly string serviceBetaProgram = "BetaProgram";
        private readonly byte serviceBetaProgramMajorVersion = 3;
        private readonly byte serviceBetaProgramMinorVersion = 4;
        private readonly string serviceBetaAddress = "127.0.0.1";
        private readonly int serviceBetaHubPort = MxpConstants.DefaultHubPort + 10;
        private readonly int serviceBetaServerPort = MxpConstants.DefaultServerPort + 10;
        private readonly string serviceBetaAssetCacheUrl = "http://test.assetcache.two";

        private readonly Guid bubbleOneId = Guid.NewGuid();
        private readonly string bubbleOneName = "BubbleOne";
        private readonly float bubbleOneRange = 100;
        private readonly float bubbleOnePerceptionRange = 110;

        private readonly Guid bubbleTwoId = Guid.NewGuid();
        private readonly string bubbleTwoName = "BubbleTwo";
        private readonly float bubbleTwoRange = 100;
        private readonly float bubbleTwoPerceptionRange = 110;

        private readonly Guid bubbleThreeId = Guid.NewGuid();
        private readonly string bubbleThreeName = "BubbleThree";
        private readonly float bubbleThreeRange = 100;
        private readonly float bubbleThreePerceptionRange = 110;

        private readonly float bubbleOneTwoDeltaX = 10;
        private readonly float bubbleOneTwoDeltaY = 11;
        private readonly float bubbleOneTwoDeltaZ = 13;

        private readonly float bubbleTwoThreeDeltaX = 14;
        private readonly float bubbleTwoThreeDeltaY = 15;
        private readonly float bubbleTwoThreeDeltaZ = 16;

        private readonly string clientProgramName = "ClientProgram";
        private readonly byte clientProgramMajorVersion = 5;
        private readonly byte clientProgramMinorVersion = 6;
        private readonly string participantIdentityProviderUrl = "http://test.identityprovider";
        private readonly string participantNameOne = "TestParticipantNameOne";
        private readonly string participantPassphraseOne = "TestParticipantPassphraseOne";
        private readonly string participantNameTwo = "TestParticipantNameTwo";
        private readonly string participantPassphraseTwo = "TestParticipantPassphraseTwo";

        private readonly Guid objectId = Guid.NewGuid();
        private readonly uint objectIndex = 100;
        private readonly string objectName = "TestObjectName";
        private readonly Guid objectParentObjectId = Guid.NewGuid();
        private readonly Guid objectTypeId = Guid.NewGuid();
        private readonly string objectTypeName = "TestObjectType";
        private readonly float objectBoundingSphereRadius = 23;
        private readonly float objectMass = 24;
        private readonly float objectLocationX = 2;
        private readonly float objectLocationY = 3;
        private readonly float objectLocationZ = 4;
        private readonly float modifiedObjectLocationX = 12;
        private readonly float modifiedObjectLocationY = 13;
        private readonly float modifiedObjectLocationZ = 14;
        private readonly float objectVelocityX = 5;
        private readonly float objectVelocityY = 6;
        private readonly float objectVelocityZ = 7;
        private readonly float objectAccelerationX = 8;
        private readonly float objectAccelerationY = 9;
        private readonly float objectAccelerationZ = 10;
        private readonly float objectOrientationX = 11;
        private readonly float objectOrientationY = 12;
        private readonly float objectOrientationZ = 13;
        private readonly float objectOrientationW = 14;
        private readonly float objectAngularVelocityX = 15;
        private readonly float objectAngularVelocityY = 16;
        private readonly float objectAngularVelocityZ = 17;
        private readonly float objectAngularVelocityW = 18;
        private readonly float objectAngularAccelerationX = 19;
        private readonly float objectAngularAccelerationY = 20;
        private readonly float objectAngularAccelerationZ = 21;
        private readonly float objectAngularAccelerationW = 22;
        private readonly string objectExtensionDialect = "TOED";
        private readonly byte objectExtensionDialectMinorVersion = 23;
        private readonly byte objectExtensionDialectMajorVersion = 24;
        private readonly byte[] objectExtensionData = ASCIIEncoding.ASCII.GetBytes("TestObjectExtensionData");

        private readonly string actionName = "TestAction";
        private readonly float actionObservationRadius = 100;
        private readonly string actionExtensionDialect = "TAED";
        private readonly byte actionExtensionDialectMinorVersion = 1;
        private readonly byte actionExtensionDialectMajorVersion = 2;
        private readonly byte[] actionExtensionData = ASCIIEncoding.ASCII.GetBytes("TestActionExtensionData");
        
        #endregion


        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ActionEvent()
        {


            CloudService serviceAlpha = new CloudService(serviceAlphaAssetCacheUrl, serviceAlphaAddress, serviceAlphaHubPort, serviceAlphaServerPort, serviceAlphaProgram, serviceAlphaProgramMajorVersion, serviceAlphaProgramMinorVersion);
            CloudBubble bubbleOne = new CloudBubble(bubbleOneId, bubbleOneName, bubbleOneRange, bubbleOnePerceptionRange);
            serviceAlpha.AddBubble(bubbleOne);
            bubbleOne.AddAllowedRemoteHubAddress(serviceBetaAddress);

            serviceAlpha.AddBubbleLink(bubbleOneId, bubbleTwoId, serviceBetaAddress, serviceBetaHubPort, bubbleOneTwoDeltaX, bubbleOneTwoDeltaY, bubbleOneTwoDeltaZ, true, true);

            CloudService serviceBeta = new CloudService(serviceBetaAssetCacheUrl, serviceBetaAddress, serviceBetaHubPort, serviceBetaServerPort, serviceBetaProgram, serviceBetaProgramMajorVersion, serviceBetaProgramMinorVersion);
            CloudBubble bubbleTwo = new CloudBubble(bubbleTwoId, bubbleTwoName, bubbleTwoRange, bubbleTwoPerceptionRange);
            bubbleTwo.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            serviceBeta.AddBubble(bubbleTwo);

            CloudBubble bubbleThree = new CloudBubble(bubbleThreeId, bubbleThreeName, bubbleThreeRange, bubbleThreePerceptionRange);
            bubbleThree.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            bubbleThree.AddAllowedRemoteHubAddress(serviceBetaAddress);
            serviceBeta.AddBubble(bubbleThree);

            serviceBeta.AddBubbleLink(bubbleTwoId, bubbleThreeId, serviceBetaAddress, serviceBetaHubPort, bubbleTwoThreeDeltaX, bubbleTwoThreeDeltaY, bubbleTwoThreeDeltaZ, true, true);

            serviceAlpha.Startup(false);
            serviceBeta.Startup(false);

            CloudView viewOne = new CloudView(1000, clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
            viewOne.Connect(serviceAlphaAddress, serviceAlphaServerPort, bubbleOneId, "", "", participantIdentityProviderUrl, participantNameOne, participantPassphraseOne, objectId,false);

            CloudView viewTwo = new CloudView(1000, clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
            viewTwo.Connect(serviceBetaAddress, serviceBetaServerPort, bubbleThreeId, "", "", participantIdentityProviderUrl, participantNameTwo, participantPassphraseTwo, objectId, false);

            Thread.Sleep(10);
            serviceAlpha.Process();serviceBeta.Process();viewOne.Process();viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();

            Assert.IsTrue(viewOne.IsConnected);
            Assert.IsTrue(viewTwo.IsConnected);

            InjectRequestMessage injectRequestMessage=new InjectRequestMessage();
            injectRequestMessage.ObjectFragment.ObjectId = objectId;
            injectRequestMessage.ObjectFragment.ObjectIndex = objectIndex;
            injectRequestMessage.ObjectFragment.ObjectName = objectName;
            injectRequestMessage.ObjectFragment.TypeId = objectTypeId;
            injectRequestMessage.ObjectFragment.TypeName = objectTypeName;
            injectRequestMessage.ObjectFragment.OwnerId = viewOne.ParticipantId;
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
            viewOne.InjectObject(injectRequestMessage);

            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();


            CloudObject bubbleOneCloudObject=bubbleOne.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleOneCloudObject);

            CloudObject bubbleTwoCloudObject = bubbleTwo.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleTwoCloudObject);

            CloudObject bubbleThreeCloudObject = bubbleThree.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleThreeCloudObject);

            CloudObject viewOneCloudObject = viewOne.CloudCache.GetObject(objectId);
            Assert.IsNotNull(viewOneCloudObject);

            CloudObject viewTwoCloudObject = viewTwo.CloudCache.GetObject(objectId);
            Assert.IsNotNull(viewTwoCloudObject);

            ActionEventMessage originalAction = new ActionEventMessage();
            originalAction.ActionFragment.ActionName = actionName;
            originalAction.ActionFragment.ObservationRadius = actionObservationRadius;
            originalAction.ActionFragment.SourceObjectId = objectId;
            originalAction.ActionFragment.ExtensionDialect = actionExtensionDialect;
            originalAction.ActionFragment.ExtensionDialectMajorVersion = actionExtensionDialectMajorVersion;
            originalAction.ActionFragment.ExtensionDialectMinorVersion = actionExtensionDialectMinorVersion;
            originalAction.ActionFragment.SetExtensionData(actionExtensionData);

            viewOne.ExecuteAction(originalAction);

            ActionEventMessage receivedAction = null;

            viewTwo.ServerAction += delegate(ActionEventMessage actionEvent)
            {
                receivedAction = actionEvent;
                Assert.AreEqual(actionName, receivedAction.ActionFragment.ActionName);
                Assert.AreEqual(objectId, receivedAction.ActionFragment.SourceObjectId);
                Assert.AreEqual(actionObservationRadius, receivedAction.ActionFragment.ObservationRadius);
                Assert.AreEqual(actionExtensionDialect, receivedAction.ActionFragment.ExtensionDialect);
                Assert.AreEqual(actionExtensionDialectMajorVersion, receivedAction.ActionFragment.ExtensionDialectMajorVersion);
                Assert.AreEqual(actionExtensionDialectMinorVersion, receivedAction.ActionFragment.ExtensionDialectMinorVersion);
                Assert.AreEqual(ASCIIEncoding.ASCII.GetString(actionExtensionData),
                    ASCIIEncoding.ASCII.GetString(receivedAction.ActionFragment.GetExtensionData()));
            };

            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();

            Assert.IsNotNull(receivedAction);

            EjectRequestMessage ejectRequestMessage = new EjectRequestMessage();
            ejectRequestMessage.ObjectId = objectId;
            viewOne.EjectObject(ejectRequestMessage);

            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            
            Assert.IsNull(bubbleOne.CloudCache.GetObject(objectId));
            Assert.IsNull(bubbleTwo.CloudCache.GetObject(objectId));
            Assert.IsNull(bubbleThree.CloudCache.GetObject(objectId));
            Assert.IsNull(viewOne.CloudCache.GetObject(objectId));
            Assert.IsNull(viewTwo.CloudCache.GetObject(objectId));

            viewOne.Disconnect();
            viewTwo.Disconnect();

            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();

            serviceAlpha.Shutdown();
            serviceBeta.Shutdown();

        }

        [Test]
        public void MovementEvent()
        {

            CloudService serviceAlpha = new CloudService(serviceAlphaAssetCacheUrl, serviceAlphaAddress, serviceAlphaHubPort, serviceAlphaServerPort, serviceAlphaProgram, serviceAlphaProgramMajorVersion, serviceAlphaProgramMinorVersion);
            CloudBubble bubbleOne = new CloudBubble(bubbleOneId, bubbleOneName, bubbleOneRange, bubbleOnePerceptionRange);
            serviceAlpha.AddBubble(bubbleOne);
            bubbleOne.AddAllowedRemoteHubAddress(serviceBetaAddress);

            serviceAlpha.AddBubbleLink(bubbleOneId, bubbleTwoId, serviceBetaAddress, serviceBetaHubPort, bubbleOneTwoDeltaX, bubbleOneTwoDeltaY, bubbleOneTwoDeltaZ, true, true);

            CloudService serviceBeta = new CloudService(serviceBetaAssetCacheUrl, serviceBetaAddress, serviceBetaHubPort, serviceBetaServerPort, serviceBetaProgram, serviceBetaProgramMajorVersion, serviceBetaProgramMinorVersion);
            CloudBubble bubbleTwo = new CloudBubble(bubbleTwoId, bubbleTwoName, bubbleTwoRange, bubbleTwoPerceptionRange);
            bubbleTwo.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            serviceBeta.AddBubble(bubbleTwo);

            CloudBubble bubbleThree = new CloudBubble(bubbleThreeId, bubbleThreeName, bubbleThreeRange, bubbleThreePerceptionRange);
            bubbleThree.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            bubbleThree.AddAllowedRemoteHubAddress(serviceBetaAddress);
            serviceBeta.AddBubble(bubbleThree);

            serviceBeta.AddBubbleLink(bubbleTwoId, bubbleThreeId, serviceBetaAddress, serviceBetaHubPort, bubbleTwoThreeDeltaX, bubbleTwoThreeDeltaY, bubbleTwoThreeDeltaZ, true, true);

            serviceAlpha.Startup(false);
            serviceBeta.Startup(false);

            CloudView viewOne = new CloudView(1000, clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
            viewOne.Connect(serviceAlphaAddress, serviceAlphaServerPort, bubbleOneId, "", "", participantIdentityProviderUrl, participantNameOne, participantPassphraseOne, objectId, false);

            CloudView viewTwo = new CloudView(1000, clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
            viewTwo.Connect(serviceBetaAddress, serviceBetaServerPort, bubbleThreeId, "", "", participantIdentityProviderUrl, participantNameTwo, participantPassphraseTwo, objectId, false);

            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();

            Assert.IsTrue(viewOne.IsConnected);
            Assert.IsTrue(viewTwo.IsConnected);

            InjectRequestMessage injectRequestMessage = new InjectRequestMessage();
            injectRequestMessage.ObjectFragment.ObjectId = objectId;
            injectRequestMessage.ObjectFragment.ObjectIndex = objectIndex;
            injectRequestMessage.ObjectFragment.ObjectName = objectName;
            injectRequestMessage.ObjectFragment.TypeId = objectTypeId;
            injectRequestMessage.ObjectFragment.TypeName = objectTypeName;
            injectRequestMessage.ObjectFragment.OwnerId = viewOne.ParticipantId;
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
            viewOne.InjectObject(injectRequestMessage);

            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();


            CloudObject bubbleOneCloudObject = bubbleOne.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleOneCloudObject);

            CloudObject bubbleTwoCloudObject = bubbleTwo.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleTwoCloudObject);

            CloudObject bubbleThreeCloudObject = bubbleThree.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleThreeCloudObject);

            CloudObject viewOneCloudObject = viewOne.CloudCache.GetObject(objectId);
            Assert.IsNotNull(viewOneCloudObject);

            CloudObject viewTwoCloudObject = viewTwo.CloudCache.GetObject(objectId);
            Assert.IsNotNull(viewTwoCloudObject);

            MovementEventMessage movement = new MovementEventMessage();
            movement.ObjectIndex = viewOneCloudObject.LocalObjectIndex;
            movement.Location.X = modifiedObjectLocationX;
            movement.Location.Y = modifiedObjectLocationY;
            movement.Location.Z = modifiedObjectLocationZ;
            movement.Orientation.X = objectOrientationX;
            movement.Orientation.Y = objectOrientationY;
            movement.Orientation.Z = objectOrientationZ;
            movement.Orientation.W = objectOrientationW;

            viewOne.MoveObject(movement);

            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();

            Assert.AreEqual(modifiedObjectLocationX - bubbleOneTwoDeltaX - bubbleTwoThreeDeltaX, bubbleThreeCloudObject.Location.X);
            Assert.AreEqual(modifiedObjectLocationY - bubbleOneTwoDeltaY - bubbleTwoThreeDeltaY, bubbleThreeCloudObject.Location.Y);
            Assert.AreEqual(modifiedObjectLocationZ - bubbleOneTwoDeltaZ - bubbleTwoThreeDeltaZ, bubbleThreeCloudObject.Location.Z);
            Assert.AreEqual(objectOrientationX, bubbleThreeCloudObject.Orientation.X);
            Assert.AreEqual(objectOrientationY, bubbleThreeCloudObject.Orientation.Y);
            Assert.AreEqual(objectOrientationZ, bubbleThreeCloudObject.Orientation.Z);
            Assert.AreEqual(objectOrientationW, bubbleThreeCloudObject.Orientation.W);

            EjectRequestMessage ejectRequestMessage = new EjectRequestMessage();
            ejectRequestMessage.ObjectId = objectId;
            viewOne.EjectObject(ejectRequestMessage);

            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();

            Assert.IsNull(bubbleOne.CloudCache.GetObject(objectId));
            Assert.IsNull(bubbleTwo.CloudCache.GetObject(objectId));
            Assert.IsNull(bubbleThree.CloudCache.GetObject(objectId));
            Assert.IsNull(viewOne.CloudCache.GetObject(objectId));
            Assert.IsNull(viewTwo.CloudCache.GetObject(objectId));

            viewOne.Disconnect();
            viewTwo.Disconnect();

            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process(); serviceBeta.Process(); viewOne.Process(); viewTwo.Process();

            serviceAlpha.Shutdown();
            serviceBeta.Shutdown();

        }

    }
}
