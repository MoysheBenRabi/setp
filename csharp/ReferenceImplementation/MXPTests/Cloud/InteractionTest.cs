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
using MXP.Extentions.OpenMetaverseFragments.Proto;

namespace MXPTests.Cloud
{

    [TestFixture]
    public class InteractionTest
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

        private readonly float bubbleOneTwoDeltaX = 10;
        private readonly float bubbleOneTwoDeltaY = 11;
        private readonly float bubbleOneTwoDeltaZ = 13;

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

        private readonly string interationName = "TestAction";
        private readonly string interactionExtensionDialect = "TAED";
        private readonly byte interactionExtensionDialectMinorVersion = 1;
        private readonly byte interactionExtensionDialectMajorVersion = 2;
        private readonly byte[] interactionExtensionData = ASCIIEncoding.ASCII.GetBytes("TestActionExtensionData");

        private readonly uint interactRequestMessageId = 45632;
        private readonly byte interactFailureCode = 123;

        #endregion

        private CloudService serviceAlpha;
        private CloudBubble bubbleOne;
        private CloudService serviceBeta;
        private CloudBubble bubbleTwo;
        private CloudView viewOne;
        private CloudView viewTwo;

        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();

            serviceAlpha = new CloudService(serviceAlphaAssetCacheUrl, serviceAlphaAddress, serviceAlphaHubPort, serviceAlphaServerPort, serviceAlphaProgram, serviceAlphaProgramMajorVersion, serviceAlphaProgramMinorVersion);
            bubbleOne = new CloudBubble(bubbleOneId, bubbleOneName, bubbleOneRange, bubbleOnePerceptionRange);
            serviceAlpha.AddBubble(bubbleOne);
            bubbleOne.AddAllowedRemoteHubAddress(serviceBetaAddress);

            serviceAlpha.AddBubbleLink(bubbleOneId, bubbleTwoId, serviceBetaAddress, serviceBetaHubPort, bubbleOneTwoDeltaX, bubbleOneTwoDeltaY, bubbleOneTwoDeltaZ, true, true);

            serviceBeta = new CloudService(serviceBetaAssetCacheUrl, serviceBetaAddress, serviceBetaHubPort, serviceBetaServerPort, serviceBetaProgram, serviceBetaProgramMajorVersion, serviceBetaProgramMinorVersion);
            bubbleTwo = new CloudBubble(bubbleTwoId, bubbleTwoName, bubbleTwoRange, bubbleTwoPerceptionRange);
            bubbleTwo.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            serviceBeta.AddBubble(bubbleTwo);

            serviceAlpha.Startup(false);
            serviceBeta.Startup(false);

            viewOne = new CloudView(1000, clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
            viewOne.Connect(serviceAlphaAddress, serviceAlphaServerPort, bubbleOneId, "", "", participantIdentityProviderUrl, participantNameOne, participantPassphraseOne, objectId, false);

            viewTwo = new CloudView(1000, clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
            viewTwo.Connect(serviceBetaAddress, serviceBetaServerPort, bubbleTwoId, "", "", participantIdentityProviderUrl, participantNameTwo, participantPassphraseTwo, objectId, false);

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

            CloudObject viewTwoCloudObject = viewTwo.CloudCache.GetObject(objectId);
            Assert.IsNotNull(viewTwoCloudObject);
        }

        [TearDown]
        public void TearDown()
        {

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
        public void DirectParticipantToParticipantInteractRequestBetweenBubbles()
        {

            InteractRequestMessage originalMessage = new InteractRequestMessage();
            originalMessage.InteractionFragment.InteractionName = interationName;
            originalMessage.InteractionFragment.SourceParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.TargetParticipantId = viewTwo.ParticipantId;
            originalMessage.InteractionFragment.ExtensionDialect = interactionExtensionDialect;
            originalMessage.InteractionFragment.ExtensionDialectMajorVersion = interactionExtensionDialectMajorVersion;
            originalMessage.InteractionFragment.ExtensionDialectMinorVersion = interactionExtensionDialectMinorVersion;
            originalMessage.InteractionFragment.SetExtensionData(interactionExtensionData);

            InteractRequestMessage receivedMessage = null;
            viewTwo.ServerInteractRequest += delegate(InteractRequestMessage message)
            {
                receivedMessage = message;
                receivedMessage.IsAutoRelease = false; // Avoid automatic pooling
            };

            viewOne.SendInteractRequest(originalMessage);

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

            Assert.IsNotNull(receivedMessage);
            Assert.AreEqual(interationName, receivedMessage.InteractionFragment.InteractionName);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.SourceParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.SourceObjectId);
            Assert.AreEqual(viewTwo.ParticipantId, receivedMessage.InteractionFragment.TargetParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.TargetObjectId);
            Assert.AreEqual(interactionExtensionDialect, receivedMessage.InteractionFragment.ExtensionDialect);
            Assert.AreEqual(interactionExtensionDialectMajorVersion, receivedMessage.InteractionFragment.ExtensionDialectMajorVersion);
            Assert.AreEqual(interactionExtensionDialectMinorVersion, receivedMessage.InteractionFragment.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(interactionExtensionData), ASCIIEncoding.ASCII.GetString(receivedMessage.InteractionFragment.GetExtensionData()));
        }

        [Test]
        public void DirectObjectToParticipantInteractRequestBetweenBubbles()
        {

            InteractRequestMessage originalMessage = new InteractRequestMessage();
            originalMessage.InteractionFragment.InteractionName = interationName;
            originalMessage.InteractionFragment.SourceParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.SourceObjectId = objectId;
            originalMessage.InteractionFragment.TargetParticipantId = viewTwo.ParticipantId;
            originalMessage.InteractionFragment.ExtensionDialect = interactionExtensionDialect;
            originalMessage.InteractionFragment.ExtensionDialectMajorVersion = interactionExtensionDialectMajorVersion;
            originalMessage.InteractionFragment.ExtensionDialectMinorVersion = interactionExtensionDialectMinorVersion;
            originalMessage.InteractionFragment.SetExtensionData(interactionExtensionData);

            InteractRequestMessage receivedMessage = null;
            viewTwo.ServerInteractRequest += delegate(InteractRequestMessage message)
            {
                receivedMessage = message;
                receivedMessage.IsAutoRelease = false; // Avoid automatic pooling
            };

            viewOne.SendInteractRequest(originalMessage);

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

            Assert.IsNotNull(receivedMessage);
            Assert.AreEqual(interationName, receivedMessage.InteractionFragment.InteractionName);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.SourceParticipantId);
            Assert.AreEqual(objectId, receivedMessage.InteractionFragment.SourceObjectId);
            Assert.AreEqual(viewTwo.ParticipantId, receivedMessage.InteractionFragment.TargetParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.TargetObjectId);
            Assert.AreEqual(interactionExtensionDialect, receivedMessage.InteractionFragment.ExtensionDialect);
            Assert.AreEqual(interactionExtensionDialectMajorVersion, receivedMessage.InteractionFragment.ExtensionDialectMajorVersion);
            Assert.AreEqual(interactionExtensionDialectMinorVersion, receivedMessage.InteractionFragment.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(interactionExtensionData), ASCIIEncoding.ASCII.GetString(receivedMessage.InteractionFragment.GetExtensionData()));
        }

        [Test]
        public void DirectParticipantToObjectInteractRequestBetweenBubbles()
        {

            InteractRequestMessage originalMessage = new InteractRequestMessage();
            originalMessage.InteractionFragment.InteractionName = interationName;
            originalMessage.InteractionFragment.SourceParticipantId = viewTwo.ParticipantId;
            originalMessage.InteractionFragment.TargetParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.TargetObjectId = objectId;
            originalMessage.InteractionFragment.ExtensionDialect = interactionExtensionDialect;
            originalMessage.InteractionFragment.ExtensionDialectMajorVersion = interactionExtensionDialectMajorVersion;
            originalMessage.InteractionFragment.ExtensionDialectMinorVersion = interactionExtensionDialectMinorVersion;
            originalMessage.InteractionFragment.SetExtensionData(interactionExtensionData);

            InteractRequestMessage receivedMessage = null;
            viewOne.ServerInteractRequest += delegate(InteractRequestMessage message)
            {
                receivedMessage = message;
                receivedMessage.IsAutoRelease = false; // Avoid automatic pooling
            };

            viewTwo.SendInteractRequest(originalMessage);

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

            Assert.IsNotNull(receivedMessage);
            Assert.AreEqual(interationName, receivedMessage.InteractionFragment.InteractionName);
            Assert.AreEqual(viewTwo.ParticipantId, receivedMessage.InteractionFragment.SourceParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.SourceObjectId);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.TargetParticipantId);
            Assert.AreEqual(objectId, receivedMessage.InteractionFragment.TargetObjectId);
            Assert.AreEqual(interactionExtensionDialect, receivedMessage.InteractionFragment.ExtensionDialect);
            Assert.AreEqual(interactionExtensionDialectMajorVersion, receivedMessage.InteractionFragment.ExtensionDialectMajorVersion);
            Assert.AreEqual(interactionExtensionDialectMinorVersion, receivedMessage.InteractionFragment.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(interactionExtensionData), ASCIIEncoding.ASCII.GetString(receivedMessage.InteractionFragment.GetExtensionData()));
        }

        [Test]
        public void DirectParticipantToSelfInteractRequest()
        {

            InteractRequestMessage originalMessage = new InteractRequestMessage();
            originalMessage.InteractionFragment.InteractionName = interationName;
            originalMessage.InteractionFragment.SourceParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.TargetParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.ExtensionDialect = interactionExtensionDialect;
            originalMessage.InteractionFragment.ExtensionDialectMajorVersion = interactionExtensionDialectMajorVersion;
            originalMessage.InteractionFragment.ExtensionDialectMinorVersion = interactionExtensionDialectMinorVersion;
            originalMessage.InteractionFragment.SetExtensionData(interactionExtensionData);

            InteractRequestMessage receivedMessage = null;
            viewOne.ServerInteractRequest += delegate(InteractRequestMessage message)
            {
                receivedMessage = message;
                receivedMessage.IsAutoRelease = false; // Avoid automatic pooling
            };

            viewOne.SendInteractRequest(originalMessage);

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

            Assert.IsNotNull(receivedMessage);
            Assert.AreEqual(interationName, receivedMessage.InteractionFragment.InteractionName);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.SourceParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.SourceObjectId);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.TargetParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.TargetObjectId);
            Assert.AreEqual(interactionExtensionDialect, receivedMessage.InteractionFragment.ExtensionDialect);
            Assert.AreEqual(interactionExtensionDialectMajorVersion, receivedMessage.InteractionFragment.ExtensionDialectMajorVersion);
            Assert.AreEqual(interactionExtensionDialectMinorVersion, receivedMessage.InteractionFragment.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(interactionExtensionData), ASCIIEncoding.ASCII.GetString(receivedMessage.InteractionFragment.GetExtensionData()));
        }

        private readonly string createTypeId = Guid.NewGuid().ToString();
        private readonly string createObjectId = Guid.NewGuid().ToString();

        [Test]
        public void OmInsertRequest()
        {

            InteractRequestMessage originalMessage = new InteractRequestMessage();
            originalMessage.InteractionFragment.InteractionName = interationName;
            originalMessage.InteractionFragment.SourceParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.TargetParticipantId = viewTwo.ParticipantId;

            OmInsertRequestExt originalExt = new OmInsertRequestExt();
            originalExt.TypeId = createTypeId;
            originalExt.Location = new MsdVector3f();
            originalExt.Orientation = new MsdQuaternion4f();

            originalMessage.SetExtension<OmInsertRequestExt>(originalExt);

            InteractRequestMessage receivedMessage = null;
            viewTwo.ServerInteractRequest += delegate(InteractRequestMessage message)
            {
                receivedMessage = message;
                receivedMessage.IsAutoRelease = false; // Avoid automatic pooling
            };

            viewOne.SendInteractRequest(originalMessage);

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

            Assert.IsNotNull(receivedMessage);

            OmInsertRequestExt receivedExt = receivedMessage.GetExtension<OmInsertRequestExt>();

            Assert.AreEqual(interationName, receivedMessage.InteractionFragment.InteractionName);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.SourceParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.SourceObjectId);
            Assert.AreEqual(viewTwo.ParticipantId, receivedMessage.InteractionFragment.TargetParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.TargetObjectId);

            Assert.AreEqual(createTypeId, receivedExt.TypeId);

        }


        [Test]
        public void DirectParticipantToParticipantInteractResponseBetweenBubbles()
        {

            InteractResponseMessage originalMessage = new InteractResponseMessage();
            originalMessage.RequestMessageId = interactRequestMessageId;
            originalMessage.FailureCode = interactFailureCode;
            originalMessage.InteractionFragment.InteractionName = interationName;
            originalMessage.InteractionFragment.SourceParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.TargetParticipantId = viewTwo.ParticipantId;
            originalMessage.InteractionFragment.ExtensionDialect = interactionExtensionDialect;
            originalMessage.InteractionFragment.ExtensionDialectMajorVersion = interactionExtensionDialectMajorVersion;
            originalMessage.InteractionFragment.ExtensionDialectMinorVersion = interactionExtensionDialectMinorVersion;
            originalMessage.InteractionFragment.SetExtensionData(interactionExtensionData);

            InteractResponseMessage receivedMessage = null;
            viewTwo.ServerInteractResponse += delegate(InteractResponseMessage message)
            {
                receivedMessage = message;
                receivedMessage.IsAutoRelease = false; // Avoid automatic pooling
            };

            viewOne.SendInteractResponse(originalMessage);

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

            Assert.IsNotNull(receivedMessage);
            Assert.AreEqual(interactRequestMessageId, receivedMessage.RequestMessageId);
            Assert.AreEqual(interactFailureCode, receivedMessage.FailureCode);
            Assert.AreEqual(interationName, receivedMessage.InteractionFragment.InteractionName);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.SourceParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.SourceObjectId);
            Assert.AreEqual(viewTwo.ParticipantId, receivedMessage.InteractionFragment.TargetParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.TargetObjectId);
            Assert.AreEqual(interactionExtensionDialect, receivedMessage.InteractionFragment.ExtensionDialect);
            Assert.AreEqual(interactionExtensionDialectMajorVersion, receivedMessage.InteractionFragment.ExtensionDialectMajorVersion);
            Assert.AreEqual(interactionExtensionDialectMinorVersion, receivedMessage.InteractionFragment.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(interactionExtensionData), ASCIIEncoding.ASCII.GetString(receivedMessage.InteractionFragment.GetExtensionData()));
        }

        [Test]
        public void DirectObjectToParticipantInteractResponseBetweenBubbles()
        {

            InteractResponseMessage originalMessage = new InteractResponseMessage();
            originalMessage.RequestMessageId = interactRequestMessageId;
            originalMessage.FailureCode = interactFailureCode;
            originalMessage.InteractionFragment.InteractionName = interationName;
            originalMessage.InteractionFragment.SourceParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.SourceObjectId = objectId;
            originalMessage.InteractionFragment.TargetParticipantId = viewTwo.ParticipantId;
            originalMessage.InteractionFragment.ExtensionDialect = interactionExtensionDialect;
            originalMessage.InteractionFragment.ExtensionDialectMajorVersion = interactionExtensionDialectMajorVersion;
            originalMessage.InteractionFragment.ExtensionDialectMinorVersion = interactionExtensionDialectMinorVersion;
            originalMessage.InteractionFragment.SetExtensionData(interactionExtensionData);

            InteractResponseMessage receivedMessage = null;
            viewTwo.ServerInteractResponse += delegate(InteractResponseMessage message)
            {
                receivedMessage = message;
                receivedMessage.IsAutoRelease = false; // Avoid automatic pooling
            };

            viewOne.SendInteractResponse(originalMessage);

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

            Assert.IsNotNull(receivedMessage);
            Assert.AreEqual(interactRequestMessageId, receivedMessage.RequestMessageId);
            Assert.AreEqual(interactFailureCode, receivedMessage.FailureCode);
            Assert.AreEqual(interationName, receivedMessage.InteractionFragment.InteractionName);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.SourceParticipantId);
            Assert.AreEqual(objectId, receivedMessage.InteractionFragment.SourceObjectId);
            Assert.AreEqual(viewTwo.ParticipantId, receivedMessage.InteractionFragment.TargetParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.TargetObjectId);
            Assert.AreEqual(interactionExtensionDialect, receivedMessage.InteractionFragment.ExtensionDialect);
            Assert.AreEqual(interactionExtensionDialectMajorVersion, receivedMessage.InteractionFragment.ExtensionDialectMajorVersion);
            Assert.AreEqual(interactionExtensionDialectMinorVersion, receivedMessage.InteractionFragment.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(interactionExtensionData), ASCIIEncoding.ASCII.GetString(receivedMessage.InteractionFragment.GetExtensionData()));
        }

        [Test]
        public void DirectParticipantToObjectInteractResponseBetweenBubbles()
        {

            InteractResponseMessage originalMessage = new InteractResponseMessage();
            originalMessage.RequestMessageId = interactRequestMessageId;
            originalMessage.FailureCode = interactFailureCode;
            originalMessage.InteractionFragment.InteractionName = interationName;
            originalMessage.InteractionFragment.SourceParticipantId = viewTwo.ParticipantId;
            originalMessage.InteractionFragment.TargetParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.TargetObjectId = objectId;
            originalMessage.InteractionFragment.ExtensionDialect = interactionExtensionDialect;
            originalMessage.InteractionFragment.ExtensionDialectMajorVersion = interactionExtensionDialectMajorVersion;
            originalMessage.InteractionFragment.ExtensionDialectMinorVersion = interactionExtensionDialectMinorVersion;
            originalMessage.InteractionFragment.SetExtensionData(interactionExtensionData);

            InteractResponseMessage receivedMessage = null;
            viewOne.ServerInteractResponse += delegate(InteractResponseMessage message)
            {
                receivedMessage = message;
                receivedMessage.IsAutoRelease = false; // Avoid automatic pooling
            };

            viewTwo.SendInteractResponse(originalMessage);

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

            Assert.IsNotNull(receivedMessage);
            Assert.AreEqual(interactRequestMessageId, receivedMessage.RequestMessageId);
            Assert.AreEqual(interactFailureCode, receivedMessage.FailureCode);
            Assert.AreEqual(interationName, receivedMessage.InteractionFragment.InteractionName);
            Assert.AreEqual(viewTwo.ParticipantId, receivedMessage.InteractionFragment.SourceParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.SourceObjectId);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.TargetParticipantId);
            Assert.AreEqual(objectId, receivedMessage.InteractionFragment.TargetObjectId);
            Assert.AreEqual(interactionExtensionDialect, receivedMessage.InteractionFragment.ExtensionDialect);
            Assert.AreEqual(interactionExtensionDialectMajorVersion, receivedMessage.InteractionFragment.ExtensionDialectMajorVersion);
            Assert.AreEqual(interactionExtensionDialectMinorVersion, receivedMessage.InteractionFragment.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(interactionExtensionData), ASCIIEncoding.ASCII.GetString(receivedMessage.InteractionFragment.GetExtensionData()));
        }

        [Test]
        public void DirectParticipantToSelfInteractResponse()
        {

            InteractResponseMessage originalMessage = new InteractResponseMessage();
            originalMessage.RequestMessageId = interactRequestMessageId;
            originalMessage.FailureCode = interactFailureCode;
            originalMessage.InteractionFragment.InteractionName = interationName;
            originalMessage.InteractionFragment.SourceParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.TargetParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.ExtensionDialect = interactionExtensionDialect;
            originalMessage.InteractionFragment.ExtensionDialectMajorVersion = interactionExtensionDialectMajorVersion;
            originalMessage.InteractionFragment.ExtensionDialectMinorVersion = interactionExtensionDialectMinorVersion;
            originalMessage.InteractionFragment.SetExtensionData(interactionExtensionData);

            InteractResponseMessage receivedMessage = null;
            viewOne.ServerInteractResponse += delegate(InteractResponseMessage message)
            {
                receivedMessage = message;
                receivedMessage.IsAutoRelease = false; // Avoid automatic pooling
            };

            viewOne.SendInteractResponse(originalMessage);

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

            Assert.IsNotNull(receivedMessage);
            Assert.AreEqual(interactRequestMessageId, receivedMessage.RequestMessageId);
            Assert.AreEqual(interactFailureCode, receivedMessage.FailureCode);
            Assert.AreEqual(interationName, receivedMessage.InteractionFragment.InteractionName);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.SourceParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.SourceObjectId);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.TargetParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.TargetObjectId);
            Assert.AreEqual(interactionExtensionDialect, receivedMessage.InteractionFragment.ExtensionDialect);
            Assert.AreEqual(interactionExtensionDialectMajorVersion, receivedMessage.InteractionFragment.ExtensionDialectMajorVersion);
            Assert.AreEqual(interactionExtensionDialectMinorVersion, receivedMessage.InteractionFragment.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(interactionExtensionData), ASCIIEncoding.ASCII.GetString(receivedMessage.InteractionFragment.GetExtensionData()));
        }

        [Test]
        public void OmInsertResponse()
        {

            InteractResponseMessage originalMessage = new InteractResponseMessage();
            originalMessage.InteractionFragment.InteractionName = interationName;
            originalMessage.InteractionFragment.SourceParticipantId = viewOne.ParticipantId;
            originalMessage.InteractionFragment.TargetParticipantId = viewTwo.ParticipantId;

            OmInsertResponseExt originalExt = new OmInsertResponseExt();
            originalExt.ObjectId = createObjectId;

            originalMessage.SetExtension<OmInsertResponseExt>(originalExt);

            InteractResponseMessage receivedMessage = null;
            viewTwo.ServerInteractResponse += delegate(InteractResponseMessage message)
            {
                receivedMessage = message;
                receivedMessage.IsAutoRelease = false; // Avoid automatic pooling
            };

            viewOne.SendInteractResponse(originalMessage);

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

            Assert.IsNotNull(receivedMessage);

            OmInsertResponseExt receivedExt = receivedMessage.GetExtension<OmInsertResponseExt>();

            Assert.AreEqual(interationName, receivedMessage.InteractionFragment.InteractionName);
            Assert.AreEqual(viewOne.ParticipantId, receivedMessage.InteractionFragment.SourceParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.SourceObjectId);
            Assert.AreEqual(viewTwo.ParticipantId, receivedMessage.InteractionFragment.TargetParticipantId);
            Assert.AreEqual(Guid.Empty, receivedMessage.InteractionFragment.TargetObjectId);

            Assert.AreEqual(createObjectId, receivedExt.ObjectId);

        }
    }
}
