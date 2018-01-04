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
    public class CloudTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }


        [Test]
        public void IdentifyAndHandover()
        {
            string serviceAlphaProgram = "AlphaProgram";
            byte serviceAlphaProgramMajorVersion = 1;
            byte serviceAlphaProgramMinorVersion = 2;
            string serviceAlphaAddress = "127.0.0.1";
            int serviceAlphaHubPort = MxpConstants.DefaultHubPort;
            int serviceAlphaServerPort = MxpConstants.DefaultServerPort;
            string serviceAlphaAssetCacheUrl = "http://test.assetcache.one";

            string serviceBetaProgram = "BetaProgram";
            byte serviceBetaProgramMajorVersion = 3;
            byte serviceBetaProgramMinorVersion = 4;
            string serviceBetaAddress = "127.0.0.1";
            int serviceBetaHubPort = MxpConstants.DefaultHubPort + 10;
            int serviceBetaServerPort = MxpConstants.DefaultServerPort + 10;
            string serviceBetaAssetCacheUrl = "http://test.assetcache.two";

            string clientProgramName = "ClientProgram";
            byte clientProgramMajorVersion = 5;
            byte clientProgramMinorVersion = 6;
            string participantIdentityProviderUrl = "http://test.identityprovider";
            string participantNameOne = "TestParticipantNameOne";
            string participantPassphraseOne = "TestParticipantPassphraseOne";

            Guid bubbleOneId = Guid.NewGuid();
            string bubbleOneName = "BubbleOne";
            float bubbleOneRange = 60;
            float bubbleOnePerceptionRange = 80;

            Guid bubbleTwoId = Guid.NewGuid();
            string bubbleTwoName = "BubbleTwo";
            float bubbleTwoRange = 60;
            float bubbleTwoPerceptionRange = 80;

            float bubbleOneTwoDeltaX = 50;
            float bubbleOneTwoDeltaY = 0;
            float bubbleOneTwoDeltaZ = 0;

            Guid objectId = Guid.NewGuid();
            uint objectIndex = 100;
            string objectName = "TestObjectName";
            Guid objectParentObjectId = Guid.NewGuid();
            Guid objectTypeId = Guid.NewGuid();
            string objectTypeName = "TestObjectType";
            float objectBoundingSphereRadius = 23;
            float objectMass = 24;
            float objectLocationX = 65;
            float objectLocationY = 0;
            float objectLocationZ = 0;
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

            CloudService serviceAlpha = new CloudService(serviceAlphaAssetCacheUrl, serviceAlphaAddress, serviceAlphaHubPort, serviceAlphaServerPort, serviceAlphaProgram, serviceAlphaProgramMajorVersion, serviceAlphaProgramMinorVersion);
            CloudBubble bubbleOne = new CloudBubble(bubbleOneId, bubbleOneName, bubbleOneRange, bubbleOnePerceptionRange);
            serviceAlpha.AddBubble(bubbleOne);
            serviceAlpha.AddBubbleLink(bubbleOneId, bubbleTwoId, serviceBetaAddress, serviceBetaHubPort, bubbleOneTwoDeltaX, bubbleOneTwoDeltaY, bubbleOneTwoDeltaZ, true, true);

            CloudService serviceBeta = new CloudService(serviceBetaAssetCacheUrl, serviceBetaAddress, serviceBetaHubPort, serviceBetaServerPort, serviceBetaProgram, serviceBetaProgramMajorVersion, serviceBetaProgramMinorVersion);
            CloudBubble bubbleTwo = new CloudBubble(bubbleTwoId, bubbleTwoName, bubbleTwoRange, bubbleTwoPerceptionRange);
            bubbleTwo.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            serviceBeta.AddBubble(bubbleTwo);

            serviceAlpha.Startup(false);
            serviceBeta.Startup(false);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();

            CloudView viewOne = new CloudView(1000, clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
            viewOne.Connect(serviceAlphaAddress, serviceAlphaServerPort, bubbleOneId, "", "", participantIdentityProviderUrl, participantNameOne, participantPassphraseOne, objectId, false);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();

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
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();


            Assert.AreEqual(bubbleOneId, bubbleOne.CloudCache.GetObject(objectId).BubbleId);
            Assert.AreEqual(bubbleOneId, bubbleTwo.CloudCache.GetObject(objectId).BubbleId);
            Assert.AreEqual(bubbleOneId, viewOne.CloudCache.GetObject(objectId).BubbleId);

            CloudObject bubbleOneCloudObject = bubbleOne.CloudCache.GetObject(objectId);

            bubbleOne.Balancer.EvaluateHandoverNeed(bubbleOneCloudObject);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();


            Assert.AreEqual(bubbleTwoId, bubbleTwo.CloudCache.GetObject(objectId).BubbleId);
            Assert.AreEqual(bubbleTwoId, bubbleOne.CloudCache.GetObject(objectId).BubbleId);

            serviceAlpha.Shutdown();
            serviceBeta.Shutdown();
        }



        [Test]
        public void BubbleLinkAutomaticConnectivity()
        {
            string serviceAlphaProgram="AlphaProgram";
            byte serviceAlphaProgramMajorVersion=1;
            byte serviceAlphaProgramMinorVersion=2;
            string serviceAlphaAddress="127.0.0.1";
            int serviceAlphaHubPort=MxpConstants.DefaultHubPort;
            int serviceAlphaServerPort=MxpConstants.DefaultServerPort;
            string serviceAlphaAssetCacheUrl = "http://test.assetcache.one";

            string serviceBetaProgram="BetaProgram";
            byte serviceBetaProgramMajorVersion=3;
            byte serviceBetaProgramMinorVersion=4;
            string serviceBetaAddress="127.0.0.1";
            int serviceBetaHubPort=MxpConstants.DefaultHubPort+10;
            int serviceBetaServerPort=MxpConstants.DefaultServerPort+10;
            string serviceBetaAssetCacheUrl = "http://test.assetcache.two";
            
            Guid bubbleOneId = Guid.NewGuid();
            string bubbleOneName="BubbleOne";
            float bubbleOneRange=100;
            float bubbleOnePerceptionRange=110;

            Guid bubbleTwoId = Guid.NewGuid();
            string bubbleTwoName="BubbleTwo";
            float bubbleTwoRange = 100;
            float bubbleTwoPerceptionRange=110;

            float bubbleOneTwoDeltaX=10;
            float bubbleOneTwoDeltaY=11;
            float bubbleOneTwoDeltaZ=13;

            CloudService serviceAlpha = new CloudService(serviceAlphaAssetCacheUrl, serviceAlphaAddress, serviceAlphaHubPort, serviceAlphaServerPort, serviceAlphaProgram, serviceAlphaProgramMajorVersion, serviceAlphaProgramMinorVersion);
            CloudBubble bubbleOne = new CloudBubble(bubbleOneId, bubbleOneName, bubbleOneRange, bubbleOnePerceptionRange);
            serviceAlpha.AddBubble(bubbleOne);
            serviceAlpha.AddBubbleLink(bubbleOneId, bubbleTwoId, serviceBetaAddress, serviceBetaHubPort, bubbleOneTwoDeltaX, bubbleOneTwoDeltaY, bubbleOneTwoDeltaZ, true, true);

            CloudService serviceBeta = new CloudService(serviceBetaAssetCacheUrl, serviceBetaAddress, serviceBetaHubPort, serviceBetaServerPort, serviceBetaProgram, serviceBetaProgramMajorVersion, serviceBetaProgramMinorVersion);
            CloudBubble bubbleTwo = new CloudBubble(bubbleTwoId, bubbleTwoName, bubbleTwoRange, bubbleTwoPerceptionRange);
            bubbleTwo.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            serviceBeta.AddBubble(bubbleTwo);

            serviceAlpha.Startup(false);
            serviceBeta.Startup(false);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();    

            BubbleLink linkOneTwo=bubbleOne.GetBubbleLink(bubbleTwoId);
            Assert.IsNotNull(linkOneTwo);
            Assert.IsTrue(linkOneTwo.IsConnected);
            Assert.AreEqual(serviceBetaProgram, linkOneTwo.RemoteHubProgram);
            Assert.AreEqual(serviceBetaProgramMajorVersion, linkOneTwo.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceBetaProgramMinorVersion, linkOneTwo.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkOneTwo.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkOneTwo.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkOneTwo.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceBetaAddress, linkOneTwo.RemoteHubAddress);
            Assert.AreEqual(serviceBetaHubPort, linkOneTwo.RemoteHubPort);
            Assert.AreEqual(bubbleTwoId, linkOneTwo.RemoteBubbleId);
            Assert.AreEqual(bubbleTwoName, linkOneTwo.RemoteBubbleName);
            Assert.AreEqual(serviceBetaAssetCacheUrl, linkOneTwo.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleTwoRange, linkOneTwo.RemoteBubbleRange);
            Assert.AreEqual(bubbleTwoPerceptionRange, linkOneTwo.RemoteBubblePerceptionRange);
            Assert.AreEqual(bubbleOneTwoDeltaX, linkOneTwo.RemoteBubbleCenter.X);
            Assert.AreEqual(bubbleOneTwoDeltaY, linkOneTwo.RemoteBubbleCenter.Y);
            Assert.AreEqual(bubbleOneTwoDeltaZ, linkOneTwo.RemoteBubbleCenter.Z);

            BubbleLink linkTwoOne = bubbleTwo.GetBubbleLink(bubbleOneId);
            Assert.IsNotNull(linkTwoOne);
            Assert.IsTrue(linkTwoOne.IsConnected);
            Assert.AreEqual(serviceAlphaProgram, linkTwoOne.RemoteHubProgram);
            Assert.AreEqual(serviceAlphaProgramMajorVersion, linkTwoOne.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceAlphaProgramMinorVersion, linkTwoOne.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkTwoOne.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkTwoOne.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkTwoOne.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceAlphaAddress, linkTwoOne.RemoteHubAddress);
            Assert.AreEqual(serviceAlphaHubPort, linkTwoOne.RemoteHubPort);
            Assert.AreEqual(bubbleOneId, linkTwoOne.RemoteBubbleId);
            Assert.AreEqual(bubbleOneName, linkTwoOne.RemoteBubbleName);
            Assert.AreEqual(serviceAlphaAssetCacheUrl, linkTwoOne.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleOneRange, linkTwoOne.RemoteBubbleRange);
            Assert.AreEqual(bubbleOnePerceptionRange, linkTwoOne.RemoteBubblePerceptionRange);
            Assert.AreEqual(-bubbleOneTwoDeltaX, linkTwoOne.RemoteBubbleCenter.X);
            Assert.AreEqual(-bubbleOneTwoDeltaY, linkTwoOne.RemoteBubbleCenter.Y);
            Assert.AreEqual(-bubbleOneTwoDeltaZ, linkTwoOne.RemoteBubbleCenter.Z);

            linkOneTwo.IsEnabled = false;

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);

            Assert.IsFalse(linkOneTwo.IsConnected);
            Assert.IsFalse(linkTwoOne.IsConnected);

            serviceAlpha.Shutdown();
            serviceBeta.Shutdown();
        }

        [Test]
        public void BubbleLinkAutomaticLinking()
        {
            string serviceAlphaProgram = "AlphaProgram";
            byte serviceAlphaProgramMajorVersion = 1;
            byte serviceAlphaProgramMinorVersion = 2;
            string serviceAlphaAddress = "127.0.0.1";
            int serviceAlphaHubPort = MxpConstants.DefaultHubPort;
            int serviceAlphaServerPort = MxpConstants.DefaultServerPort;
            string serviceAlphaAssetCacheUrl = "http://test.assetcache.one";

            string serviceBetaProgram = "BetaProgram";
            byte serviceBetaProgramMajorVersion = 3;
            byte serviceBetaProgramMinorVersion = 4;
            string serviceBetaAddress = "127.0.0.1";
            int serviceBetaHubPort = MxpConstants.DefaultHubPort + 10;
            int serviceBetaServerPort = MxpConstants.DefaultServerPort + 10;
            string serviceBetaAssetCacheUrl = "http://test.assetcache.two";

            Guid bubbleOneId = Guid.NewGuid();
            string bubbleOneName = "BubbleOne";
            float bubbleOneRange = 100;
            float bubbleOnePerceptionRange = 110;

            Guid bubbleTwoId = Guid.NewGuid();
            string bubbleTwoName = "BubbleTwo";
            float bubbleTwoRange = 100;
            float bubbleTwoPerceptionRange = 110;

            Guid bubbleThreeId = Guid.NewGuid();
            string bubbleThreeName = "BubbleThree";
            float bubbleThreeRange = 100;
            float bubbleThreePerceptionRange = 110;

            float bubbleOneTwoDeltaX = 10;
            float bubbleOneTwoDeltaY = 11;
            float bubbleOneTwoDeltaZ = 13;

            float bubbleTwoThreeDeltaX = 14;
            float bubbleTwoThreeDeltaY = 15;
            float bubbleTwoThreeDeltaZ = 16;


            CloudService serviceAlpha = new CloudService(serviceAlphaAssetCacheUrl, serviceAlphaAddress, serviceAlphaHubPort, serviceAlphaServerPort, serviceAlphaProgram, serviceAlphaProgramMajorVersion, serviceAlphaProgramMinorVersion);
            CloudBubble bubbleOne = new CloudBubble(bubbleOneId, bubbleOneName, bubbleOneRange, bubbleOnePerceptionRange);
            serviceAlpha.AddBubble(bubbleOne);
            
            serviceAlpha.AddBubbleLink(bubbleOneId, bubbleTwoId, serviceBetaAddress, serviceBetaHubPort, bubbleOneTwoDeltaX, bubbleOneTwoDeltaY, bubbleOneTwoDeltaZ, true, true);

            CloudService serviceBeta = new CloudService(serviceBetaAssetCacheUrl, serviceBetaAddress, serviceBetaHubPort, serviceBetaServerPort, serviceBetaProgram, serviceBetaProgramMajorVersion, serviceBetaProgramMinorVersion);
            
            CloudBubble bubbleTwo = new CloudBubble(bubbleTwoId, bubbleTwoName, bubbleTwoRange, bubbleTwoPerceptionRange);
            bubbleTwo.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            serviceBeta.AddBubble(bubbleTwo);

            CloudBubble bubbleThree = new CloudBubble(bubbleThreeId, bubbleThreeName, bubbleThreeRange, bubbleThreePerceptionRange);
            bubbleThree.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            serviceBeta.AddBubble(bubbleThree);

            serviceBeta.AddBubbleLink(bubbleTwoId, bubbleThreeId, serviceBetaAddress, serviceBetaHubPort, bubbleTwoThreeDeltaX, bubbleTwoThreeDeltaY, bubbleTwoThreeDeltaZ, true, true);

            serviceAlpha.Startup(false);
            serviceBeta.Startup(false);

            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();
            Thread.Sleep(30);
            serviceAlpha.Process();
            Thread.Sleep(30);
            serviceBeta.Process();


            LogUtil.Debug("Automated links should be ready now.");

            BubbleLink linkOneTwo = bubbleOne.GetBubbleLink(bubbleTwoId);
            Assert.IsNotNull(linkOneTwo);
            Assert.IsTrue(linkOneTwo.IsConnected);
            Assert.AreEqual(serviceBetaProgram, linkOneTwo.RemoteHubProgram);
            Assert.AreEqual(serviceBetaProgramMajorVersion, linkOneTwo.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceBetaProgramMinorVersion, linkOneTwo.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkOneTwo.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkOneTwo.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkOneTwo.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceBetaAddress, linkOneTwo.RemoteHubAddress);
            Assert.AreEqual(serviceBetaHubPort, linkOneTwo.RemoteHubPort);
            Assert.AreEqual(bubbleTwoId, linkOneTwo.RemoteBubbleId);
            Assert.AreEqual(bubbleTwoName, linkOneTwo.RemoteBubbleName);
            Assert.AreEqual(serviceBetaAssetCacheUrl, linkOneTwo.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleTwoRange, linkOneTwo.RemoteBubbleRange);
            Assert.AreEqual(bubbleTwoPerceptionRange, linkOneTwo.RemoteBubblePerceptionRange);
            Assert.AreEqual(bubbleOneTwoDeltaX, linkOneTwo.RemoteBubbleCenter.X);
            Assert.AreEqual(bubbleOneTwoDeltaY, linkOneTwo.RemoteBubbleCenter.Y);
            Assert.AreEqual(bubbleOneTwoDeltaZ, linkOneTwo.RemoteBubbleCenter.Z);

            BubbleLink linkTwoOne = bubbleTwo.GetBubbleLink(bubbleOneId);
            Assert.IsNotNull(linkTwoOne);
            Assert.IsTrue(linkTwoOne.IsConnected);
            Assert.AreEqual(serviceAlphaProgram, linkTwoOne.RemoteHubProgram);
            Assert.AreEqual(serviceAlphaProgramMajorVersion, linkTwoOne.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceAlphaProgramMinorVersion, linkTwoOne.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkTwoOne.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkTwoOne.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkTwoOne.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceAlphaAddress, linkTwoOne.RemoteHubAddress);
            Assert.AreEqual(serviceAlphaHubPort, linkTwoOne.RemoteHubPort);
            Assert.AreEqual(bubbleOneId, linkTwoOne.RemoteBubbleId);
            Assert.AreEqual(bubbleOneName, linkTwoOne.RemoteBubbleName);
            Assert.AreEqual(serviceAlphaAssetCacheUrl, linkTwoOne.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleOneRange, linkTwoOne.RemoteBubbleRange);
            Assert.AreEqual(bubbleOnePerceptionRange, linkTwoOne.RemoteBubblePerceptionRange);
            Assert.AreEqual(-bubbleOneTwoDeltaX, linkTwoOne.RemoteBubbleCenter.X);
            Assert.AreEqual(-bubbleOneTwoDeltaY, linkTwoOne.RemoteBubbleCenter.Y);
            Assert.AreEqual(-bubbleOneTwoDeltaZ, linkTwoOne.RemoteBubbleCenter.Z);

            BubbleLink linkTwoThree = bubbleTwo.GetBubbleLink(bubbleThreeId);
            Assert.IsNotNull(linkTwoThree);
            Assert.IsTrue(linkTwoThree.IsConnected);
            Assert.AreEqual(serviceBetaProgram, linkTwoThree.RemoteHubProgram);
            Assert.AreEqual(serviceBetaProgramMajorVersion, linkTwoThree.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceBetaProgramMinorVersion, linkTwoThree.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkTwoThree.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkTwoThree.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkTwoThree.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceBetaAddress, linkTwoThree.RemoteHubAddress);
            Assert.AreEqual(serviceBetaHubPort, linkTwoThree.RemoteHubPort);
            Assert.AreEqual(bubbleThreeId, linkTwoThree.RemoteBubbleId);
            Assert.AreEqual(bubbleThreeName, linkTwoThree.RemoteBubbleName);
            Assert.AreEqual(serviceBetaAssetCacheUrl, linkTwoThree.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleThreeRange, linkTwoThree.RemoteBubbleRange);
            Assert.AreEqual(bubbleThreePerceptionRange, linkTwoThree.RemoteBubblePerceptionRange);
            Assert.AreEqual(bubbleTwoThreeDeltaX, linkTwoThree.RemoteBubbleCenter.X);
            Assert.AreEqual(bubbleTwoThreeDeltaY, linkTwoThree.RemoteBubbleCenter.Y);
            Assert.AreEqual(bubbleTwoThreeDeltaZ, linkTwoThree.RemoteBubbleCenter.Z);


            BubbleLink linkThreeTwo = bubbleThree.GetBubbleLink(bubbleTwoId);
            Assert.IsNotNull(linkThreeTwo);
            Assert.IsTrue(linkThreeTwo.IsConnected);
            Assert.AreEqual(serviceBetaProgram, linkThreeTwo.RemoteHubProgram);
            Assert.AreEqual(serviceBetaProgramMajorVersion, linkThreeTwo.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceBetaProgramMinorVersion, linkThreeTwo.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkThreeTwo.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkThreeTwo.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkThreeTwo.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceBetaAddress, linkThreeTwo.RemoteHubAddress);
            Assert.AreEqual(serviceBetaHubPort, linkThreeTwo.RemoteHubPort);
            Assert.AreEqual(bubbleTwoId, linkThreeTwo.RemoteBubbleId);
            Assert.AreEqual(bubbleTwoName, linkThreeTwo.RemoteBubbleName);
            Assert.AreEqual(serviceBetaAssetCacheUrl, linkThreeTwo.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleTwoRange, linkThreeTwo.RemoteBubbleRange);
            Assert.AreEqual(bubbleTwoPerceptionRange, linkThreeTwo.RemoteBubblePerceptionRange);
            Assert.AreEqual(-bubbleTwoThreeDeltaX, linkThreeTwo.RemoteBubbleCenter.X);
            Assert.AreEqual(-bubbleTwoThreeDeltaY, linkThreeTwo.RemoteBubbleCenter.Y);
            Assert.AreEqual(-bubbleTwoThreeDeltaZ, linkThreeTwo.RemoteBubbleCenter.Z);

            BubbleLink linkThreeOne = bubbleThree.GetBubbleLink(bubbleOneId);
            Assert.IsNotNull(linkThreeOne);
            Assert.IsTrue(linkThreeOne.IsConnected);
            Assert.AreEqual(serviceAlphaProgram, linkThreeOne.RemoteHubProgram);
            Assert.AreEqual(serviceAlphaProgramMajorVersion, linkThreeOne.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceAlphaProgramMinorVersion, linkThreeOne.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkThreeOne.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkThreeOne.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkThreeOne.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceAlphaAddress, linkThreeOne.RemoteHubAddress);
            Assert.AreEqual(serviceAlphaHubPort, linkThreeOne.RemoteHubPort);
            Assert.AreEqual(bubbleOneId, linkThreeOne.RemoteBubbleId);
            Assert.AreEqual(bubbleOneName, linkThreeOne.RemoteBubbleName);
            Assert.AreEqual(serviceAlphaAssetCacheUrl, linkThreeOne.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleOneRange, linkThreeOne.RemoteBubbleRange);
            Assert.AreEqual(bubbleOnePerceptionRange, linkThreeOne.RemoteBubblePerceptionRange);
            Assert.AreEqual(-(bubbleTwoThreeDeltaX + bubbleOneTwoDeltaX), linkThreeOne.RemoteBubbleCenter.X);
            Assert.AreEqual(-(bubbleTwoThreeDeltaY + bubbleOneTwoDeltaY), linkThreeOne.RemoteBubbleCenter.Y);
            Assert.AreEqual(-(bubbleTwoThreeDeltaZ + bubbleOneTwoDeltaZ), linkThreeOne.RemoteBubbleCenter.Z);

            BubbleLink linkOneThree = bubbleOne.GetBubbleLink(bubbleThreeId);
            Assert.IsNotNull(linkOneThree);
            Assert.IsTrue(linkOneThree.IsConnected);
            Assert.AreEqual(serviceBetaProgram, linkOneThree.RemoteHubProgram);
            Assert.AreEqual(serviceBetaProgramMajorVersion, linkOneThree.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceBetaProgramMinorVersion, linkOneThree.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkOneThree.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkOneThree.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkOneThree.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceBetaAddress, linkOneThree.RemoteHubAddress);
            Assert.AreEqual(serviceBetaHubPort, linkOneThree.RemoteHubPort);
            Assert.AreEqual(bubbleThreeId, linkOneThree.RemoteBubbleId);
            Assert.AreEqual(bubbleThreeName, linkOneThree.RemoteBubbleName);
            Assert.AreEqual(serviceBetaAssetCacheUrl, linkOneThree.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleThreeRange, linkOneThree.RemoteBubbleRange);
            Assert.AreEqual(bubbleThreePerceptionRange, linkOneThree.RemoteBubblePerceptionRange);
            Assert.AreEqual(bubbleTwoThreeDeltaX + bubbleOneTwoDeltaX, linkOneThree.RemoteBubbleCenter.X);
            Assert.AreEqual(bubbleTwoThreeDeltaY + bubbleOneTwoDeltaY, linkOneThree.RemoteBubbleCenter.Y);
            Assert.AreEqual(bubbleTwoThreeDeltaZ + bubbleOneTwoDeltaZ, linkOneThree.RemoteBubbleCenter.Z);

            linkOneTwo.IsEnabled = false;

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);


            Assert.IsFalse(linkOneTwo.IsConnected);
            Assert.IsFalse(linkTwoOne.IsConnected);
            Assert.IsTrue(linkTwoThree.IsConnected);
            Assert.IsTrue(linkThreeTwo.IsConnected);
            Assert.IsTrue(linkOneThree.IsConnected);
            Assert.IsTrue(linkThreeOne.IsConnected);

            serviceAlpha.Shutdown();
            serviceBeta.Shutdown();
        }

        [Test]
        public void BubbleLinkAutomaticLinkingFailure()
        {
            string serviceAlphaProgram = "AlphaProgram";
            byte serviceAlphaProgramMajorVersion = 1;
            byte serviceAlphaProgramMinorVersion = 2;
            string serviceAlphaAddress = "127.0.0.1";
            int serviceAlphaHubPort = MxpConstants.DefaultHubPort;
            int serviceAlphaServerPort = MxpConstants.DefaultServerPort;
            string serviceAlphaAssetCacheUrl = "http://test.assetcache.one";

            string serviceBetaProgram = "BetaProgram";
            byte serviceBetaProgramMajorVersion = 3;
            byte serviceBetaProgramMinorVersion = 4;
            string serviceBetaAddress = "127.0.0.1";
            int serviceBetaHubPort = MxpConstants.DefaultHubPort + 10;
            int serviceBetaServerPort = MxpConstants.DefaultServerPort + 10;
            string serviceBetaAssetCacheUrl = "http://test.assetcache.two";

            Guid bubbleOneId = Guid.NewGuid();
            string bubbleOneName = "BubbleOne";
            float bubbleOneRange = 100;
            float bubbleOnePerceptionRange = 110;

            Guid bubbleTwoId = Guid.NewGuid();
            string bubbleTwoName = "BubbleTwo";
            float bubbleTwoRange = 100;
            float bubbleTwoPerceptionRange = 110;

            Guid bubbleThreeId = Guid.NewGuid();
            string bubbleThreeName = "BubbleThree";
            float bubbleThreeRange = 100;
            float bubbleThreePerceptionRange = 110;

            float bubbleOneTwoDeltaX = 200;
            float bubbleOneTwoDeltaY = 0;
            float bubbleOneTwoDeltaZ = 0;

            float bubbleTwoThreeDeltaX = 200;
            float bubbleTwoThreeDeltaY = 0;
            float bubbleTwoThreeDeltaZ = 0;


            CloudService serviceAlpha = new CloudService(serviceAlphaAssetCacheUrl, serviceAlphaAddress, serviceAlphaHubPort, serviceAlphaServerPort, serviceAlphaProgram, serviceAlphaProgramMajorVersion, serviceAlphaProgramMinorVersion);
            CloudBubble bubbleOne = new CloudBubble(bubbleOneId, bubbleOneName, bubbleOneRange, bubbleOnePerceptionRange);
            serviceAlpha.AddBubble(bubbleOne);

            serviceAlpha.AddBubbleLink(bubbleOneId, bubbleTwoId, serviceBetaAddress, serviceBetaHubPort, bubbleOneTwoDeltaX, bubbleOneTwoDeltaY, bubbleOneTwoDeltaZ, true, true);

            CloudService serviceBeta = new CloudService(serviceBetaAssetCacheUrl, serviceBetaAddress, serviceBetaHubPort, serviceBetaServerPort, serviceBetaProgram, serviceBetaProgramMajorVersion, serviceBetaProgramMinorVersion);

            CloudBubble bubbleTwo = new CloudBubble(bubbleTwoId, bubbleTwoName, bubbleTwoRange, bubbleTwoPerceptionRange);
            bubbleTwo.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            serviceBeta.AddBubble(bubbleTwo);

            CloudBubble bubbleThree = new CloudBubble(bubbleThreeId, bubbleThreeName, bubbleThreeRange, bubbleThreePerceptionRange);
            bubbleThree.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            serviceBeta.AddBubble(bubbleThree);

            serviceBeta.AddBubbleLink(bubbleTwoId, bubbleThreeId, serviceBetaAddress, serviceBetaHubPort, bubbleTwoThreeDeltaX, bubbleTwoThreeDeltaY, bubbleTwoThreeDeltaZ, true, true);

            serviceAlpha.Startup(false);
            serviceBeta.Startup(false);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();

            LogUtil.Debug("Automated links should be ready now.");

            BubbleLink linkOneTwo = bubbleOne.GetBubbleLink(bubbleTwoId);
            Assert.IsNotNull(linkOneTwo);
            Assert.IsTrue(linkOneTwo.IsConnected);
            Assert.AreEqual(serviceBetaProgram, linkOneTwo.RemoteHubProgram);
            Assert.AreEqual(serviceBetaProgramMajorVersion, linkOneTwo.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceBetaProgramMinorVersion, linkOneTwo.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkOneTwo.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkOneTwo.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkOneTwo.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceBetaAddress, linkOneTwo.RemoteHubAddress);
            Assert.AreEqual(serviceBetaHubPort, linkOneTwo.RemoteHubPort);
            Assert.AreEqual(bubbleTwoId, linkOneTwo.RemoteBubbleId);
            Assert.AreEqual(bubbleTwoName, linkOneTwo.RemoteBubbleName);
            Assert.AreEqual(serviceBetaAssetCacheUrl, linkOneTwo.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleTwoRange, linkOneTwo.RemoteBubbleRange);
            Assert.AreEqual(bubbleTwoPerceptionRange, linkOneTwo.RemoteBubblePerceptionRange);
            Assert.AreEqual(bubbleOneTwoDeltaX, linkOneTwo.RemoteBubbleCenter.X);
            Assert.AreEqual(bubbleOneTwoDeltaY, linkOneTwo.RemoteBubbleCenter.Y);
            Assert.AreEqual(bubbleOneTwoDeltaZ, linkOneTwo.RemoteBubbleCenter.Z);

            BubbleLink linkTwoOne = bubbleTwo.GetBubbleLink(bubbleOneId);
            Assert.IsNotNull(linkTwoOne);
            Assert.IsTrue(linkTwoOne.IsConnected);
            Assert.AreEqual(serviceAlphaProgram, linkTwoOne.RemoteHubProgram);
            Assert.AreEqual(serviceAlphaProgramMajorVersion, linkTwoOne.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceAlphaProgramMinorVersion, linkTwoOne.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkTwoOne.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkTwoOne.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkTwoOne.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceAlphaAddress, linkTwoOne.RemoteHubAddress);
            Assert.AreEqual(serviceAlphaHubPort, linkTwoOne.RemoteHubPort);
            Assert.AreEqual(bubbleOneId, linkTwoOne.RemoteBubbleId);
            Assert.AreEqual(bubbleOneName, linkTwoOne.RemoteBubbleName);
            Assert.AreEqual(serviceAlphaAssetCacheUrl, linkTwoOne.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleOneRange, linkTwoOne.RemoteBubbleRange);
            Assert.AreEqual(bubbleOnePerceptionRange, linkTwoOne.RemoteBubblePerceptionRange);
            Assert.AreEqual(-bubbleOneTwoDeltaX, linkTwoOne.RemoteBubbleCenter.X);
            Assert.AreEqual(-bubbleOneTwoDeltaY, linkTwoOne.RemoteBubbleCenter.Y);
            Assert.AreEqual(-bubbleOneTwoDeltaZ, linkTwoOne.RemoteBubbleCenter.Z);

            BubbleLink linkTwoThree = bubbleTwo.GetBubbleLink(bubbleThreeId);
            Assert.IsNotNull(linkTwoThree);
            Assert.IsTrue(linkTwoThree.IsConnected);
            Assert.AreEqual(serviceBetaProgram, linkTwoThree.RemoteHubProgram);
            Assert.AreEqual(serviceBetaProgramMajorVersion, linkTwoThree.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceBetaProgramMinorVersion, linkTwoThree.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkTwoThree.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkTwoThree.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkTwoThree.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceBetaAddress, linkTwoThree.RemoteHubAddress);
            Assert.AreEqual(serviceBetaHubPort, linkTwoThree.RemoteHubPort);
            Assert.AreEqual(bubbleThreeId, linkTwoThree.RemoteBubbleId);
            Assert.AreEqual(bubbleThreeName, linkTwoThree.RemoteBubbleName);
            Assert.AreEqual(serviceBetaAssetCacheUrl, linkTwoThree.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleThreeRange, linkTwoThree.RemoteBubbleRange);
            Assert.AreEqual(bubbleThreePerceptionRange, linkTwoThree.RemoteBubblePerceptionRange);
            Assert.AreEqual(bubbleTwoThreeDeltaX, linkTwoThree.RemoteBubbleCenter.X);
            Assert.AreEqual(bubbleTwoThreeDeltaY, linkTwoThree.RemoteBubbleCenter.Y);
            Assert.AreEqual(bubbleTwoThreeDeltaZ, linkTwoThree.RemoteBubbleCenter.Z);


            BubbleLink linkThreeTwo = bubbleThree.GetBubbleLink(bubbleTwoId);
            Assert.IsNotNull(linkThreeTwo);
            Assert.IsTrue(linkThreeTwo.IsConnected);
            Assert.AreEqual(serviceBetaProgram, linkThreeTwo.RemoteHubProgram);
            Assert.AreEqual(serviceBetaProgramMajorVersion, linkThreeTwo.RemoteHubProgramMajorVersion);
            Assert.AreEqual(serviceBetaProgramMinorVersion, linkThreeTwo.RemoteHubProgramMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMajorVersion, linkThreeTwo.RemoteProtocolMajorVersion);
            Assert.AreEqual(MxpConstants.ProtocolMinorVersion, linkThreeTwo.RemoteProtocolMinorVersion);
            Assert.AreEqual(MxpConstants.ProtocolSourceRevision, linkThreeTwo.RemoteProtocolSourceRevision);
            Assert.AreEqual(serviceBetaAddress, linkThreeTwo.RemoteHubAddress);
            Assert.AreEqual(serviceBetaHubPort, linkThreeTwo.RemoteHubPort);
            Assert.AreEqual(bubbleTwoId, linkThreeTwo.RemoteBubbleId);
            Assert.AreEqual(bubbleTwoName, linkThreeTwo.RemoteBubbleName);
            Assert.AreEqual(serviceBetaAssetCacheUrl, linkThreeTwo.RemoteBubbleAssetCacheUrl);
            Assert.AreEqual(bubbleTwoRange, linkThreeTwo.RemoteBubbleRange);
            Assert.AreEqual(bubbleTwoPerceptionRange, linkThreeTwo.RemoteBubblePerceptionRange);
            Assert.AreEqual(-bubbleTwoThreeDeltaX, linkThreeTwo.RemoteBubbleCenter.X);
            Assert.AreEqual(-bubbleTwoThreeDeltaY, linkThreeTwo.RemoteBubbleCenter.Y);
            Assert.AreEqual(-bubbleTwoThreeDeltaZ, linkThreeTwo.RemoteBubbleCenter.Z);

            BubbleLink linkThreeOne = bubbleThree.GetBubbleLink(bubbleOneId);
            Assert.IsNull(linkThreeOne);

            BubbleLink linkOneThree = bubbleOne.GetBubbleLink(bubbleThreeId);
            Assert.IsNull(linkOneThree);

            linkOneTwo.IsEnabled = false;

            Thread.Sleep(10);

            serviceAlpha.Process();

            Thread.Sleep(10);

            serviceBeta.Process();

            Thread.Sleep(10);

            serviceBeta.Process();

            Thread.Sleep(10);

            serviceAlpha.Process();

            Thread.Sleep(10);

            serviceBeta.Process();

            Thread.Sleep(10);


            Assert.IsFalse(linkOneTwo.IsConnected);
            Assert.IsFalse(linkTwoOne.IsConnected);
            Assert.IsTrue(linkTwoThree.IsConnected);
            Assert.IsTrue(linkThreeTwo.IsConnected);

            serviceAlpha.Shutdown();
            serviceBeta.Shutdown();
        }

        [Test]
        public void ParticipantConnectivity()
        {
            string serviceAlphaProgram = "AlphaProgram";
            byte serviceAlphaProgramMajorVersion = 1;
            byte serviceAlphaProgramMinorVersion = 2;
            string serviceAlphaAddress = "127.0.0.1";
            int serviceAlphaHubPort = MxpConstants.DefaultHubPort;
            int serviceAlphaServerPort = MxpConstants.DefaultServerPort;
            string serviceAlphaAssetCacheUrl = "http://test.assetcache.one";

            string serviceBetaProgram = "BetaProgram";
            byte serviceBetaProgramMajorVersion = 3;
            byte serviceBetaProgramMinorVersion = 4;
            string serviceBetaAddress = "127.0.0.1";
            int serviceBetaHubPort = MxpConstants.DefaultHubPort + 10;
            int serviceBetaServerPort = MxpConstants.DefaultServerPort + 10;
            string serviceBetaAssetCacheUrl = "http://test.assetcache.two";

            Guid bubbleOneId = Guid.NewGuid();
            string bubbleOneName = "BubbleOne";
            float bubbleOneRange = 100;
            float bubbleOnePerceptionRange = 110;

            Guid bubbleTwoId = Guid.NewGuid();
            string bubbleTwoName = "BubbleTwo";
            float bubbleTwoRange = 100;
            float bubbleTwoPerceptionRange = 110;

            float bubbleOneTwoDeltaX = 10;
            float bubbleOneTwoDeltaY = 11;
            float bubbleOneTwoDeltaZ = 13;

            string clientProgramName = "ClientProgram";
            byte clientProgramMajorVersion = 5;
            byte clientProgramMinorVersion = 6;
            string participantIdentityProviderUrl = "http://test.identityprovider";
            string participantName = "TestParticipantName";
            string participantPassphrase = "TestParticipantPassphrase";
            Guid avatarId = Guid.NewGuid();

            CloudService serviceAlpha = new CloudService(serviceAlphaAssetCacheUrl, serviceAlphaAddress, serviceAlphaHubPort, serviceAlphaServerPort, serviceAlphaProgram, serviceAlphaProgramMajorVersion, serviceAlphaProgramMinorVersion);
            CloudBubble bubbleOne = new CloudBubble(bubbleOneId, bubbleOneName, bubbleOneRange, bubbleOnePerceptionRange);
            serviceAlpha.AddBubble(bubbleOne);
            serviceAlpha.AddBubbleLink(bubbleOneId, bubbleTwoId, serviceBetaAddress, serviceBetaHubPort, bubbleOneTwoDeltaX, bubbleOneTwoDeltaY, bubbleOneTwoDeltaZ, true, true);

            CloudService serviceBeta = new CloudService(serviceBetaAssetCacheUrl, serviceBetaAddress, serviceBetaHubPort, serviceBetaServerPort, serviceBetaProgram, serviceBetaProgramMajorVersion, serviceBetaProgramMinorVersion);
            CloudBubble bubbleTwo = new CloudBubble(bubbleTwoId, bubbleTwoName, bubbleTwoRange, bubbleTwoPerceptionRange);
            bubbleTwo.AddAllowedRemoteHubAddress(serviceAlphaAddress);
            serviceBeta.AddBubble(bubbleTwo);

            serviceAlpha.Startup(false);
            serviceBeta.Startup(false);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();

            CloudView view = new CloudView(1000,clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
            view.Connect(serviceAlphaAddress, serviceAlphaServerPort, bubbleOneId, "", "", participantIdentityProviderUrl, participantName, participantPassphrase, avatarId, false);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            view.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            view.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            view.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            view.Process();

            Assert.IsTrue(view.IsConnected);
            ParticipantLink participantLink = bubbleOne.GetParticipant(view.ParticipantId);
            Assert.IsNotNull(participantLink);
            Assert.AreEqual(participantLink.ParticipantId,view.ParticipantId);
            Assert.AreEqual(participantLink.ParticipantIdentifier, participantName );
            Assert.AreEqual(participantLink.ProgramName, clientProgramName );
            Assert.AreEqual(participantLink.ProgramMajorVersion, clientProgramMajorVersion);
            Assert.AreEqual(participantLink.ProgramMinorVersion, clientProgramMinorVersion);
            Assert.AreEqual(participantLink.ProtocolMajorVersion, MxpConstants.ProtocolMajorVersion);
            Assert.AreEqual(participantLink.ProtocolMinorVersion, MxpConstants.ProtocolMinorVersion);
            Assert.AreEqual(participantLink.ProtocolSourceRevision, MxpConstants.ProtocolSourceRevision);
            Assert.AreEqual(participantLink.AvatarId, avatarId);
            Assert.AreEqual(view.AvatarId, avatarId);

            
            bubbleOne.DisconnectParticipant(view.ParticipantId);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            view.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            view.Process();

            Assert.IsFalse(view.IsConnected);
            Assert.IsNull(bubbleOne.GetParticipant(view.ParticipantId));

            serviceAlpha.Shutdown();
            serviceBeta.Shutdown();
        }

        [Test]
        public void ObjectSynchronization()
        {
            string serviceAlphaProgram = "AlphaProgram";
            byte serviceAlphaProgramMajorVersion = 1;
            byte serviceAlphaProgramMinorVersion = 2;
            string serviceAlphaAddress = "127.0.0.1";
            int serviceAlphaHubPort = MxpConstants.DefaultHubPort;
            int serviceAlphaServerPort = MxpConstants.DefaultServerPort;
            string serviceAlphaAssetCacheUrl = "http://test.assetcache.one";

            string serviceBetaProgram = "BetaProgram";
            byte serviceBetaProgramMajorVersion = 3;
            byte serviceBetaProgramMinorVersion = 4;
            string serviceBetaAddress = "127.0.0.1";
            int serviceBetaHubPort = MxpConstants.DefaultHubPort + 10;
            int serviceBetaServerPort = MxpConstants.DefaultServerPort + 10;
            string serviceBetaAssetCacheUrl = "http://test.assetcache.two";

            Guid bubbleOneId = Guid.NewGuid();
            string bubbleOneName = "BubbleOne";
            float bubbleOneRange = 100;
            float bubbleOnePerceptionRange = 110;

            Guid bubbleTwoId = Guid.NewGuid();
            string bubbleTwoName = "BubbleTwo";
            float bubbleTwoRange = 100;
            float bubbleTwoPerceptionRange = 110;

            Guid bubbleThreeId = Guid.NewGuid();
            string bubbleThreeName = "BubbleThree";
            float bubbleThreeRange = 100;
            float bubbleThreePerceptionRange = 110;

            float bubbleOneTwoDeltaX = 10;
            float bubbleOneTwoDeltaY = 11;
            float bubbleOneTwoDeltaZ = 13;

            float bubbleTwoThreeDeltaX = 14;
            float bubbleTwoThreeDeltaY = 15;
            float bubbleTwoThreeDeltaZ = 16;

            string clientProgramName = "ClientProgram";
            byte clientProgramMajorVersion = 5;
            byte clientProgramMinorVersion = 6;
            string participantIdentityProviderUrl = "http://test.identityprovider";
            string participantNameOne = "TestParticipantNameOne";
            string participantPassphraseOne = "TestParticipantPassphraseOne";
            string participantNameTwo = "TestParticipantNameTwo";
            string participantPassphraseTwo = "TestParticipantPassphraseTwo";

            Guid objectId = Guid.NewGuid();
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

            serviceAlpha.Startup(false);
            serviceBeta.Startup(false);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();

            LogUtil.Info("Creating view one.");

            CloudView viewOne = new CloudView(1000,clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
            viewOne.Connect(serviceAlphaAddress, serviceAlphaServerPort, bubbleOneId, "", "", participantIdentityProviderUrl, participantNameOne, participantPassphraseOne, objectId, false);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            viewOne.Process();

            Assert.IsTrue(viewOne.IsConnected);
            ParticipantLink participantLink = bubbleOne.GetParticipant(viewOne.ParticipantId);
            Assert.IsNotNull(participantLink);

            LogUtil.Info("Starting inject.");

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
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();

            CloudObject bubbleOneCloudObject=bubbleOne.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleOneCloudObject);
            Assert.AreEqual(objectId,bubbleOneCloudObject.ObjectId);
            Assert.AreEqual(viewOne.CloudCache.GetObject(objectId).LocalObjectIndex,bubbleOneCloudObject.RemoteObjectIndex);
            Assert.AreEqual(1, bubbleOneCloudObject.LocalObjectIndex);
            Assert.AreEqual(objectName, bubbleOneCloudObject.ObjectName);
            Assert.AreEqual(objectTypeId, bubbleOneCloudObject.TypeId);
            Assert.AreEqual(objectTypeName, bubbleOneCloudObject.TypeName);
            Assert.AreEqual(viewOne.ParticipantId, bubbleOneCloudObject.OwnerId);
            Assert.AreEqual(objectParentObjectId,bubbleOneCloudObject.ParentObjectId);
            Assert.AreEqual(objectMass,bubbleOneCloudObject.Mass);
            Assert.AreEqual(objectBoundingSphereRadius,bubbleOneCloudObject.BoundingSphereRadius);
            Assert.AreEqual(objectLocationX,bubbleOneCloudObject.Location.X);
            Assert.AreEqual(objectLocationY,bubbleOneCloudObject.Location.Y);
            Assert.AreEqual(objectLocationZ,bubbleOneCloudObject.Location.Z);
            Assert.AreEqual(objectVelocityX,bubbleOneCloudObject.Velocity.X);
            Assert.AreEqual(objectVelocityY,bubbleOneCloudObject.Velocity.Y);
            Assert.AreEqual(objectVelocityZ,bubbleOneCloudObject.Velocity.Z);
            Assert.AreEqual(objectAccelerationX,bubbleOneCloudObject.Acceleration.X);
            Assert.AreEqual(objectAccelerationY,bubbleOneCloudObject.Acceleration.Y);
            Assert.AreEqual(objectAccelerationZ,bubbleOneCloudObject.Acceleration.Z);
            Assert.AreEqual(objectOrientationX,bubbleOneCloudObject.Orientation.X);
            Assert.AreEqual(objectOrientationY,bubbleOneCloudObject.Orientation.Y);
            Assert.AreEqual(objectOrientationZ,bubbleOneCloudObject.Orientation.Z);
            Assert.AreEqual(objectOrientationW,bubbleOneCloudObject.Orientation.W);
            Assert.AreEqual(objectAngularVelocityX,bubbleOneCloudObject.AngularVelocity.X);
            Assert.AreEqual(objectAngularVelocityY,bubbleOneCloudObject.AngularVelocity.Y);
            Assert.AreEqual(objectAngularVelocityZ,bubbleOneCloudObject.AngularVelocity.Z);
            Assert.AreEqual(objectAngularVelocityW,bubbleOneCloudObject.AngularVelocity.W);
            Assert.AreEqual(objectAngularAccelerationX,bubbleOneCloudObject.AngularAcceleration.X);
            Assert.AreEqual(objectAngularAccelerationY,bubbleOneCloudObject.AngularAcceleration.Y);
            Assert.AreEqual(objectAngularAccelerationZ,bubbleOneCloudObject.AngularAcceleration.Z);
            Assert.AreEqual(objectAngularAccelerationW,bubbleOneCloudObject.AngularAcceleration.W);
            Assert.AreEqual(objectExtensionDialect,bubbleOneCloudObject.ExtensionDialect);
            Assert.AreEqual(objectExtensionDialectMajorVersion,bubbleOneCloudObject.ExtensionDialectMajorVersion);
            Assert.AreEqual(objectExtensionDialectMinorVersion, bubbleOneCloudObject.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(objectExtensionData),ASCIIEncoding.ASCII.GetString(bubbleOneCloudObject.ExtensionData));

            CloudObject bubbleTwoCloudObject = bubbleTwo.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleTwoCloudObject);
            Assert.AreEqual(objectId, bubbleTwoCloudObject.ObjectId);
            Assert.AreEqual(1, bubbleTwoCloudObject.RemoteObjectIndex);
            Assert.AreEqual(1, bubbleTwoCloudObject.LocalObjectIndex);
            Assert.AreEqual(objectName, bubbleTwoCloudObject.ObjectName);
            Assert.AreEqual(objectTypeId, bubbleTwoCloudObject.TypeId);
            Assert.AreEqual(objectTypeName, bubbleTwoCloudObject.TypeName);
            Assert.AreEqual(viewOne.ParticipantId, bubbleTwoCloudObject.OwnerId);
            Assert.AreEqual(objectParentObjectId, bubbleTwoCloudObject.ParentObjectId);
            Assert.AreEqual(objectMass, bubbleTwoCloudObject.Mass);
            Assert.AreEqual(objectBoundingSphereRadius, bubbleTwoCloudObject.BoundingSphereRadius);
            Assert.AreEqual(objectLocationX - bubbleOneTwoDeltaX, bubbleTwoCloudObject.Location.X);
            Assert.AreEqual(objectLocationY - bubbleOneTwoDeltaY, bubbleTwoCloudObject.Location.Y);
            Assert.AreEqual(objectLocationZ - bubbleOneTwoDeltaZ, bubbleTwoCloudObject.Location.Z);
            Assert.AreEqual(objectVelocityX, bubbleTwoCloudObject.Velocity.X);
            Assert.AreEqual(objectVelocityY, bubbleTwoCloudObject.Velocity.Y);
            Assert.AreEqual(objectVelocityZ, bubbleTwoCloudObject.Velocity.Z);
            Assert.AreEqual(objectAccelerationX, bubbleTwoCloudObject.Acceleration.X);
            Assert.AreEqual(objectAccelerationY, bubbleTwoCloudObject.Acceleration.Y);
            Assert.AreEqual(objectAccelerationZ, bubbleTwoCloudObject.Acceleration.Z);
            Assert.AreEqual(objectOrientationX, bubbleTwoCloudObject.Orientation.X);
            Assert.AreEqual(objectOrientationY, bubbleTwoCloudObject.Orientation.Y);
            Assert.AreEqual(objectOrientationZ, bubbleTwoCloudObject.Orientation.Z);
            Assert.AreEqual(objectOrientationW, bubbleTwoCloudObject.Orientation.W);
            Assert.AreEqual(objectAngularVelocityX, bubbleTwoCloudObject.AngularVelocity.X);
            Assert.AreEqual(objectAngularVelocityY, bubbleTwoCloudObject.AngularVelocity.Y);
            Assert.AreEqual(objectAngularVelocityZ, bubbleTwoCloudObject.AngularVelocity.Z);
            Assert.AreEqual(objectAngularVelocityW, bubbleTwoCloudObject.AngularVelocity.W);
            Assert.AreEqual(objectAngularAccelerationX, bubbleTwoCloudObject.AngularAcceleration.X);
            Assert.AreEqual(objectAngularAccelerationY, bubbleTwoCloudObject.AngularAcceleration.Y);
            Assert.AreEqual(objectAngularAccelerationZ, bubbleTwoCloudObject.AngularAcceleration.Z);
            Assert.AreEqual(objectAngularAccelerationW, bubbleTwoCloudObject.AngularAcceleration.W);
            Assert.AreEqual(objectExtensionDialect, bubbleTwoCloudObject.ExtensionDialect);
            Assert.AreEqual(objectExtensionDialectMajorVersion, bubbleTwoCloudObject.ExtensionDialectMajorVersion);
            Assert.AreEqual(objectExtensionDialectMinorVersion, bubbleTwoCloudObject.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(objectExtensionData), ASCIIEncoding.ASCII.GetString(bubbleTwoCloudObject.ExtensionData));

            CloudObject viewOneCloudObject = viewOne.CloudCache.GetObject(objectId);
            Assert.IsNotNull(viewOneCloudObject);
            Assert.AreEqual(objectId, viewOneCloudObject.ObjectId);
            Assert.AreEqual(1, viewOneCloudObject.RemoteObjectIndex);
            Assert.AreEqual(1, viewOneCloudObject.LocalObjectIndex);
            Assert.AreEqual(objectName, viewOneCloudObject.ObjectName);
            Assert.AreEqual(objectTypeId, viewOneCloudObject.TypeId);
            Assert.AreEqual(objectTypeName, viewOneCloudObject.TypeName);
            Assert.AreEqual(viewOne.ParticipantId, viewOneCloudObject.OwnerId);
            Assert.AreEqual(objectParentObjectId, viewOneCloudObject.ParentObjectId);
            Assert.AreEqual(objectMass, viewOneCloudObject.Mass);
            Assert.AreEqual(objectBoundingSphereRadius, viewOneCloudObject.BoundingSphereRadius);
            Assert.AreEqual(objectLocationX, viewOneCloudObject.Location.X);
            Assert.AreEqual(objectLocationY, viewOneCloudObject.Location.Y);
            Assert.AreEqual(objectLocationZ, viewOneCloudObject.Location.Z);
            Assert.AreEqual(objectVelocityX, viewOneCloudObject.Velocity.X);
            Assert.AreEqual(objectVelocityY, viewOneCloudObject.Velocity.Y);
            Assert.AreEqual(objectVelocityZ, viewOneCloudObject.Velocity.Z);
            Assert.AreEqual(objectAccelerationX, viewOneCloudObject.Acceleration.X);
            Assert.AreEqual(objectAccelerationY, viewOneCloudObject.Acceleration.Y);
            Assert.AreEqual(objectAccelerationZ, viewOneCloudObject.Acceleration.Z);
            Assert.AreEqual(objectOrientationX, viewOneCloudObject.Orientation.X);
            Assert.AreEqual(objectOrientationY, viewOneCloudObject.Orientation.Y);
            Assert.AreEqual(objectOrientationZ, viewOneCloudObject.Orientation.Z);
            Assert.AreEqual(objectOrientationW, viewOneCloudObject.Orientation.W);
            Assert.AreEqual(objectAngularVelocityX, viewOneCloudObject.AngularVelocity.X);
            Assert.AreEqual(objectAngularVelocityY, viewOneCloudObject.AngularVelocity.Y);
            Assert.AreEqual(objectAngularVelocityZ, viewOneCloudObject.AngularVelocity.Z);
            Assert.AreEqual(objectAngularVelocityW, viewOneCloudObject.AngularVelocity.W);
            Assert.AreEqual(objectAngularAccelerationX, viewOneCloudObject.AngularAcceleration.X);
            Assert.AreEqual(objectAngularAccelerationY, viewOneCloudObject.AngularAcceleration.Y);
            Assert.AreEqual(objectAngularAccelerationZ, viewOneCloudObject.AngularAcceleration.Z);
            Assert.AreEqual(objectAngularAccelerationW, viewOneCloudObject.AngularAcceleration.W);
            Assert.AreEqual(objectExtensionDialect, viewOneCloudObject.ExtensionDialect);
            Assert.AreEqual(objectExtensionDialectMajorVersion, viewOneCloudObject.ExtensionDialectMajorVersion);
            Assert.AreEqual(objectExtensionDialectMinorVersion, viewOneCloudObject.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(objectExtensionData), ASCIIEncoding.ASCII.GetString(viewOneCloudObject.ExtensionData));

            LogUtil.Info("Starting modify.");

            ModifyRequestMessage modifyRequestMessage = new ModifyRequestMessage();
            viewOneCloudObject.ToObjectFragment(modifyRequestMessage.ObjectFragment);
            modifyRequestMessage.ObjectFragment.Location.X = modifiedObjectLocationX;
            modifyRequestMessage.ObjectFragment.Location.Y = modifiedObjectLocationY;
            modifyRequestMessage.ObjectFragment.Location.Z = modifiedObjectLocationZ;
            viewOne.ModifyObject(modifyRequestMessage);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();


            bubbleOneCloudObject = bubbleOne.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleOneCloudObject);
            Assert.AreEqual(objectId, bubbleOneCloudObject.ObjectId);
            Assert.AreEqual(viewOneCloudObject.LocalObjectIndex, bubbleOneCloudObject.RemoteObjectIndex);
            Assert.AreEqual(1, bubbleOneCloudObject.LocalObjectIndex);
            Assert.AreEqual(objectName, bubbleOneCloudObject.ObjectName);
            Assert.AreEqual(objectTypeId, bubbleOneCloudObject.TypeId);
            Assert.AreEqual(objectTypeName, bubbleOneCloudObject.TypeName);
            Assert.AreEqual(viewOne.ParticipantId, bubbleOneCloudObject.OwnerId);
            Assert.AreEqual(objectParentObjectId, bubbleOneCloudObject.ParentObjectId);
            Assert.AreEqual(objectMass, bubbleOneCloudObject.Mass);
            Assert.AreEqual(objectBoundingSphereRadius, bubbleOneCloudObject.BoundingSphereRadius);
            Assert.AreEqual(modifiedObjectLocationX, bubbleOneCloudObject.Location.X);
            Assert.AreEqual(modifiedObjectLocationY, bubbleOneCloudObject.Location.Y);
            Assert.AreEqual(modifiedObjectLocationZ, bubbleOneCloudObject.Location.Z);
            Assert.AreEqual(objectVelocityX, bubbleOneCloudObject.Velocity.X);
            Assert.AreEqual(objectVelocityY, bubbleOneCloudObject.Velocity.Y);
            Assert.AreEqual(objectVelocityZ, bubbleOneCloudObject.Velocity.Z);
            Assert.AreEqual(objectAccelerationX, bubbleOneCloudObject.Acceleration.X);
            Assert.AreEqual(objectAccelerationY, bubbleOneCloudObject.Acceleration.Y);
            Assert.AreEqual(objectAccelerationZ, bubbleOneCloudObject.Acceleration.Z);
            Assert.AreEqual(objectOrientationX, bubbleOneCloudObject.Orientation.X);
            Assert.AreEqual(objectOrientationY, bubbleOneCloudObject.Orientation.Y);
            Assert.AreEqual(objectOrientationZ, bubbleOneCloudObject.Orientation.Z);
            Assert.AreEqual(objectOrientationW, bubbleOneCloudObject.Orientation.W);
            Assert.AreEqual(objectAngularVelocityX, bubbleOneCloudObject.AngularVelocity.X);
            Assert.AreEqual(objectAngularVelocityY, bubbleOneCloudObject.AngularVelocity.Y);
            Assert.AreEqual(objectAngularVelocityZ, bubbleOneCloudObject.AngularVelocity.Z);
            Assert.AreEqual(objectAngularVelocityW, bubbleOneCloudObject.AngularVelocity.W);
            Assert.AreEqual(objectAngularAccelerationX, bubbleOneCloudObject.AngularAcceleration.X);
            Assert.AreEqual(objectAngularAccelerationY, bubbleOneCloudObject.AngularAcceleration.Y);
            Assert.AreEqual(objectAngularAccelerationZ, bubbleOneCloudObject.AngularAcceleration.Z);
            Assert.AreEqual(objectAngularAccelerationW, bubbleOneCloudObject.AngularAcceleration.W);
            Assert.AreEqual(objectExtensionDialect, bubbleOneCloudObject.ExtensionDialect);
            Assert.AreEqual(objectExtensionDialectMajorVersion, bubbleOneCloudObject.ExtensionDialectMajorVersion);
            Assert.AreEqual(objectExtensionDialectMinorVersion, bubbleOneCloudObject.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(objectExtensionData), ASCIIEncoding.ASCII.GetString(bubbleOneCloudObject.ExtensionData));

            bubbleTwoCloudObject = bubbleTwo.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleTwoCloudObject);
            Assert.AreEqual(objectId, bubbleTwoCloudObject.ObjectId);
            Assert.AreEqual(1, bubbleTwoCloudObject.RemoteObjectIndex);
            Assert.AreEqual(1, bubbleTwoCloudObject.LocalObjectIndex);
            Assert.AreEqual(objectName, bubbleTwoCloudObject.ObjectName);
            Assert.AreEqual(objectTypeId, bubbleTwoCloudObject.TypeId);
            Assert.AreEqual(objectTypeName, bubbleTwoCloudObject.TypeName);
            Assert.AreEqual(viewOne.ParticipantId, bubbleTwoCloudObject.OwnerId);
            Assert.AreEqual(objectParentObjectId, bubbleTwoCloudObject.ParentObjectId);
            Assert.AreEqual(objectMass, bubbleTwoCloudObject.Mass);
            Assert.AreEqual(objectBoundingSphereRadius, bubbleTwoCloudObject.BoundingSphereRadius);
            Assert.AreEqual(modifiedObjectLocationX - bubbleOneTwoDeltaX, bubbleTwoCloudObject.Location.X);
            Assert.AreEqual(modifiedObjectLocationY - bubbleOneTwoDeltaY, bubbleTwoCloudObject.Location.Y);
            Assert.AreEqual(modifiedObjectLocationZ - bubbleOneTwoDeltaZ, bubbleTwoCloudObject.Location.Z);
            Assert.AreEqual(objectVelocityX, bubbleTwoCloudObject.Velocity.X);
            Assert.AreEqual(objectVelocityY, bubbleTwoCloudObject.Velocity.Y);
            Assert.AreEqual(objectVelocityZ, bubbleTwoCloudObject.Velocity.Z);
            Assert.AreEqual(objectAccelerationX, bubbleTwoCloudObject.Acceleration.X);
            Assert.AreEqual(objectAccelerationY, bubbleTwoCloudObject.Acceleration.Y);
            Assert.AreEqual(objectAccelerationZ, bubbleTwoCloudObject.Acceleration.Z);
            Assert.AreEqual(objectOrientationX, bubbleTwoCloudObject.Orientation.X);
            Assert.AreEqual(objectOrientationY, bubbleTwoCloudObject.Orientation.Y);
            Assert.AreEqual(objectOrientationZ, bubbleTwoCloudObject.Orientation.Z);
            Assert.AreEqual(objectOrientationW, bubbleTwoCloudObject.Orientation.W);
            Assert.AreEqual(objectAngularVelocityX, bubbleTwoCloudObject.AngularVelocity.X);
            Assert.AreEqual(objectAngularVelocityY, bubbleTwoCloudObject.AngularVelocity.Y);
            Assert.AreEqual(objectAngularVelocityZ, bubbleTwoCloudObject.AngularVelocity.Z);
            Assert.AreEqual(objectAngularVelocityW, bubbleTwoCloudObject.AngularVelocity.W);
            Assert.AreEqual(objectAngularAccelerationX, bubbleTwoCloudObject.AngularAcceleration.X);
            Assert.AreEqual(objectAngularAccelerationY, bubbleTwoCloudObject.AngularAcceleration.Y);
            Assert.AreEqual(objectAngularAccelerationZ, bubbleTwoCloudObject.AngularAcceleration.Z);
            Assert.AreEqual(objectAngularAccelerationW, bubbleTwoCloudObject.AngularAcceleration.W);
            Assert.AreEqual(objectExtensionDialect, bubbleTwoCloudObject.ExtensionDialect);
            Assert.AreEqual(objectExtensionDialectMajorVersion, bubbleTwoCloudObject.ExtensionDialectMajorVersion);
            Assert.AreEqual(objectExtensionDialectMinorVersion, bubbleTwoCloudObject.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(objectExtensionData), ASCIIEncoding.ASCII.GetString(bubbleTwoCloudObject.ExtensionData));

            viewOneCloudObject = viewOne.CloudCache.GetObject(objectId);
            Assert.IsNotNull(viewOneCloudObject);
            Assert.AreEqual(objectId, viewOneCloudObject.ObjectId);
            Assert.AreEqual(1, viewOneCloudObject.RemoteObjectIndex);
            Assert.AreEqual(1, viewOneCloudObject.LocalObjectIndex);
            Assert.AreEqual(objectName, viewOneCloudObject.ObjectName);
            Assert.AreEqual(objectTypeId, viewOneCloudObject.TypeId);
            Assert.AreEqual(objectTypeName, viewOneCloudObject.TypeName);
            Assert.AreEqual(viewOne.ParticipantId, viewOneCloudObject.OwnerId);
            Assert.AreEqual(objectParentObjectId, viewOneCloudObject.ParentObjectId);
            Assert.AreEqual(objectMass, viewOneCloudObject.Mass);
            Assert.AreEqual(objectBoundingSphereRadius, viewOneCloudObject.BoundingSphereRadius);
            Assert.AreEqual(modifiedObjectLocationX, viewOneCloudObject.Location.X);
            Assert.AreEqual(modifiedObjectLocationY, viewOneCloudObject.Location.Y);
            Assert.AreEqual(modifiedObjectLocationZ, viewOneCloudObject.Location.Z);
            Assert.AreEqual(objectVelocityX, viewOneCloudObject.Velocity.X);
            Assert.AreEqual(objectVelocityY, viewOneCloudObject.Velocity.Y);
            Assert.AreEqual(objectVelocityZ, viewOneCloudObject.Velocity.Z);
            Assert.AreEqual(objectAccelerationX, viewOneCloudObject.Acceleration.X);
            Assert.AreEqual(objectAccelerationY, viewOneCloudObject.Acceleration.Y);
            Assert.AreEqual(objectAccelerationZ, viewOneCloudObject.Acceleration.Z);
            Assert.AreEqual(objectOrientationX, viewOneCloudObject.Orientation.X);
            Assert.AreEqual(objectOrientationY, viewOneCloudObject.Orientation.Y);
            Assert.AreEqual(objectOrientationZ, viewOneCloudObject.Orientation.Z);
            Assert.AreEqual(objectOrientationW, viewOneCloudObject.Orientation.W);
            Assert.AreEqual(objectAngularVelocityX, viewOneCloudObject.AngularVelocity.X);
            Assert.AreEqual(objectAngularVelocityY, viewOneCloudObject.AngularVelocity.Y);
            Assert.AreEqual(objectAngularVelocityZ, viewOneCloudObject.AngularVelocity.Z);
            Assert.AreEqual(objectAngularVelocityW, viewOneCloudObject.AngularVelocity.W);
            Assert.AreEqual(objectAngularAccelerationX, viewOneCloudObject.AngularAcceleration.X);
            Assert.AreEqual(objectAngularAccelerationY, viewOneCloudObject.AngularAcceleration.Y);
            Assert.AreEqual(objectAngularAccelerationZ, viewOneCloudObject.AngularAcceleration.Z);
            Assert.AreEqual(objectAngularAccelerationW, viewOneCloudObject.AngularAcceleration.W);
            Assert.AreEqual(objectExtensionDialect, viewOneCloudObject.ExtensionDialect);
            Assert.AreEqual(objectExtensionDialectMajorVersion, viewOneCloudObject.ExtensionDialectMajorVersion);
            Assert.AreEqual(objectExtensionDialectMinorVersion, viewOneCloudObject.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(objectExtensionData), ASCIIEncoding.ASCII.GetString(viewOneCloudObject.ExtensionData));

            LogUtil.Info("Adding last bubblelink.");

            serviceBeta.AddBubbleLink(bubbleTwoId, bubbleThreeId, serviceBetaAddress, serviceBetaHubPort, bubbleTwoThreeDeltaX, bubbleTwoThreeDeltaY, bubbleTwoThreeDeltaZ, true, true);
            bubbleTwo.GetBubbleLink(bubbleThreeId).ScheduledConnect = DateTime.Now;

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();

            CloudObject bubbleThreeCloudObject = bubbleThree.CloudCache.GetObject(objectId);
            Assert.IsNotNull(bubbleThreeCloudObject);
            Assert.AreEqual(objectId, bubbleThreeCloudObject.ObjectId);
            Assert.AreEqual(1, bubbleThreeCloudObject.RemoteObjectIndex);
            Assert.AreEqual(1, bubbleThreeCloudObject.LocalObjectIndex);
            Assert.AreEqual(objectName, bubbleThreeCloudObject.ObjectName);
            Assert.AreEqual(objectTypeId, bubbleThreeCloudObject.TypeId);
            Assert.AreEqual(objectTypeName, bubbleThreeCloudObject.TypeName);
            Assert.AreEqual(viewOne.ParticipantId, bubbleThreeCloudObject.OwnerId);
            Assert.AreEqual(objectParentObjectId, bubbleThreeCloudObject.ParentObjectId);
            Assert.AreEqual(objectMass, bubbleThreeCloudObject.Mass);
            Assert.AreEqual(objectBoundingSphereRadius, bubbleThreeCloudObject.BoundingSphereRadius);
            Assert.AreEqual(modifiedObjectLocationX - bubbleOneTwoDeltaX - bubbleTwoThreeDeltaX, bubbleThreeCloudObject.Location.X);
            Assert.AreEqual(modifiedObjectLocationY - bubbleOneTwoDeltaY - bubbleTwoThreeDeltaY, bubbleThreeCloudObject.Location.Y);
            Assert.AreEqual(modifiedObjectLocationZ - bubbleOneTwoDeltaZ - bubbleTwoThreeDeltaZ, bubbleThreeCloudObject.Location.Z);
            Assert.AreEqual(objectVelocityX, bubbleThreeCloudObject.Velocity.X);
            Assert.AreEqual(objectVelocityY, bubbleThreeCloudObject.Velocity.Y);
            Assert.AreEqual(objectVelocityZ, bubbleThreeCloudObject.Velocity.Z);
            Assert.AreEqual(objectAccelerationX, bubbleThreeCloudObject.Acceleration.X);
            Assert.AreEqual(objectAccelerationY, bubbleThreeCloudObject.Acceleration.Y);
            Assert.AreEqual(objectAccelerationZ, bubbleThreeCloudObject.Acceleration.Z);
            Assert.AreEqual(objectOrientationX, bubbleThreeCloudObject.Orientation.X);
            Assert.AreEqual(objectOrientationY, bubbleThreeCloudObject.Orientation.Y);
            Assert.AreEqual(objectOrientationZ, bubbleThreeCloudObject.Orientation.Z);
            Assert.AreEqual(objectOrientationW, bubbleThreeCloudObject.Orientation.W);
            Assert.AreEqual(objectAngularVelocityX, bubbleThreeCloudObject.AngularVelocity.X);
            Assert.AreEqual(objectAngularVelocityY, bubbleThreeCloudObject.AngularVelocity.Y);
            Assert.AreEqual(objectAngularVelocityZ, bubbleThreeCloudObject.AngularVelocity.Z);
            Assert.AreEqual(objectAngularVelocityW, bubbleThreeCloudObject.AngularVelocity.W);
            Assert.AreEqual(objectAngularAccelerationX, bubbleThreeCloudObject.AngularAcceleration.X);
            Assert.AreEqual(objectAngularAccelerationY, bubbleThreeCloudObject.AngularAcceleration.Y);
            Assert.AreEqual(objectAngularAccelerationZ, bubbleThreeCloudObject.AngularAcceleration.Z);
            Assert.AreEqual(objectAngularAccelerationW, bubbleThreeCloudObject.AngularAcceleration.W);
            Assert.AreEqual(objectExtensionDialect, bubbleThreeCloudObject.ExtensionDialect);
            Assert.AreEqual(objectExtensionDialectMajorVersion, bubbleThreeCloudObject.ExtensionDialectMajorVersion);
            Assert.AreEqual(objectExtensionDialectMinorVersion, bubbleThreeCloudObject.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(objectExtensionData), ASCIIEncoding.ASCII.GetString(bubbleTwoCloudObject.ExtensionData));

            LogUtil.Info("Connecting view two.");

            CloudView viewTwo = new CloudView(1000, clientProgramName, clientProgramMajorVersion, clientProgramMinorVersion);
            viewTwo.Connect(serviceBetaAddress, serviceBetaServerPort, bubbleThreeId, "", "", participantIdentityProviderUrl, participantNameTwo, participantPassphraseTwo, objectId, false);

            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewTwo.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewTwo.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewTwo.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewTwo.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewTwo.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewTwo.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewTwo.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewTwo.Process();

            Assert.IsTrue(viewTwo.IsConnected);
            ParticipantLink participantLinkTwo = bubbleThree.GetParticipant(viewTwo.ParticipantId);
            Assert.IsNotNull(participantLinkTwo);

            CloudObject viewTwoCloudObject = viewTwo.CloudCache.GetObject(objectId);
            Assert.IsNotNull(viewTwoCloudObject);
            Assert.AreEqual(objectId, viewTwoCloudObject.ObjectId);
            Assert.AreEqual(1, viewTwoCloudObject.RemoteObjectIndex);
            Assert.AreEqual(1, viewTwoCloudObject.LocalObjectIndex);
            Assert.AreEqual(objectName, viewTwoCloudObject.ObjectName);
            Assert.AreEqual(objectTypeId, viewTwoCloudObject.TypeId);
            Assert.AreEqual(objectTypeName, viewTwoCloudObject.TypeName);
            Assert.AreEqual(viewOne.ParticipantId, viewTwoCloudObject.OwnerId);
            Assert.AreEqual(objectParentObjectId, viewTwoCloudObject.ParentObjectId);
            Assert.AreEqual(objectMass, viewTwoCloudObject.Mass);
            Assert.AreEqual(objectBoundingSphereRadius, viewTwoCloudObject.BoundingSphereRadius);
            Assert.AreEqual(modifiedObjectLocationX - bubbleOneTwoDeltaX - bubbleTwoThreeDeltaX, viewTwoCloudObject.Location.X);
            Assert.AreEqual(modifiedObjectLocationY - bubbleOneTwoDeltaY - bubbleTwoThreeDeltaY, viewTwoCloudObject.Location.Y);
            Assert.AreEqual(modifiedObjectLocationZ - bubbleOneTwoDeltaZ - bubbleTwoThreeDeltaZ, viewTwoCloudObject.Location.Z);
            Assert.AreEqual(objectVelocityX, viewTwoCloudObject.Velocity.X);
            Assert.AreEqual(objectVelocityY, viewTwoCloudObject.Velocity.Y);
            Assert.AreEqual(objectVelocityZ, viewTwoCloudObject.Velocity.Z);
            Assert.AreEqual(objectAccelerationX, viewTwoCloudObject.Acceleration.X);
            Assert.AreEqual(objectAccelerationY, viewTwoCloudObject.Acceleration.Y);
            Assert.AreEqual(objectAccelerationZ, viewTwoCloudObject.Acceleration.Z);
            Assert.AreEqual(objectOrientationX, viewTwoCloudObject.Orientation.X);
            Assert.AreEqual(objectOrientationY, viewTwoCloudObject.Orientation.Y);
            Assert.AreEqual(objectOrientationZ, viewTwoCloudObject.Orientation.Z);
            Assert.AreEqual(objectOrientationW, viewTwoCloudObject.Orientation.W);
            Assert.AreEqual(objectAngularVelocityX, viewTwoCloudObject.AngularVelocity.X);
            Assert.AreEqual(objectAngularVelocityY, viewTwoCloudObject.AngularVelocity.Y);
            Assert.AreEqual(objectAngularVelocityZ, viewTwoCloudObject.AngularVelocity.Z);
            Assert.AreEqual(objectAngularVelocityW, viewTwoCloudObject.AngularVelocity.W);
            Assert.AreEqual(objectAngularAccelerationX, viewTwoCloudObject.AngularAcceleration.X);
            Assert.AreEqual(objectAngularAccelerationY, viewTwoCloudObject.AngularAcceleration.Y);
            Assert.AreEqual(objectAngularAccelerationZ, viewTwoCloudObject.AngularAcceleration.Z);
            Assert.AreEqual(objectAngularAccelerationW, viewTwoCloudObject.AngularAcceleration.W);
            Assert.AreEqual(objectExtensionDialect, viewTwoCloudObject.ExtensionDialect);
            Assert.AreEqual(objectExtensionDialectMajorVersion, viewTwoCloudObject.ExtensionDialectMajorVersion);
            Assert.AreEqual(objectExtensionDialectMinorVersion, viewTwoCloudObject.ExtensionDialectMinorVersion);
            Assert.AreEqual(ASCIIEncoding.ASCII.GetString(objectExtensionData), ASCIIEncoding.ASCII.GetString(viewTwoCloudObject.ExtensionData));
            
            EjectRequestMessage ejectRequestMessage = new EjectRequestMessage();
            ejectRequestMessage.ObjectId = objectId;
            viewOne.EjectObject(ejectRequestMessage);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            viewTwo.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            viewTwo.Process();
            
            Assert.IsNull(bubbleOne.CloudCache.GetObject(objectId));
            Assert.IsNull(bubbleTwo.CloudCache.GetObject(objectId));
            Assert.IsNull(bubbleThree.CloudCache.GetObject(objectId));
            Assert.IsNull(viewOne.CloudCache.GetObject(objectId));
            Assert.IsNull(viewTwo.CloudCache.GetObject(objectId));

            viewOne.Disconnect();

            Thread.Sleep(100);

            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();
            Thread.Sleep(10);
            serviceAlpha.Process();
            Thread.Sleep(10);
            serviceBeta.Process();
            Thread.Sleep(10);
            viewOne.Process();

            serviceAlpha.Shutdown();
            serviceBeta.Shutdown();

        }

    }
}
