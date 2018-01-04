using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP.Messages;
using System.Diagnostics;
using MXP;
using MXP.Fragments;
using MXP.Extentions.OpenMetaverseFragments.Proto;
using MXP.Common.Proto;

namespace OpenMetaverseExtensionTests
{
    /// <summary>
    /// Summary description for PerceptionOpenMetaverseTest
    /// </summary>
    [TestFixture]
    public class PerceptionEventMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void PerceptionMessageEncoding()
        {
            PerceptionEventMessage originalMessage = new PerceptionEventMessage();

            originalMessage.ObjectFragment.ObjectId = Guid.NewGuid();
            originalMessage.ObjectFragment.ObjectIndex = 1;
            originalMessage.ObjectFragment.TypeId = Guid.NewGuid();
            originalMessage.ObjectFragment.ObjectName = "TestObjectName";
            originalMessage.ObjectFragment.TypeName = "TestTypeName";
            originalMessage.ObjectFragment.OwnerId = Guid.NewGuid();
            originalMessage.ObjectFragment.Location.X = 1; originalMessage.ObjectFragment.Location.Y = 2; originalMessage.ObjectFragment.Location.Z = 3;
            originalMessage.ObjectFragment.Velocity.X = 4; originalMessage.ObjectFragment.Velocity.Y = 5; originalMessage.ObjectFragment.Velocity.Z = 6;
            originalMessage.ObjectFragment.Acceleration.X = 7; originalMessage.ObjectFragment.Acceleration.Y = 8; originalMessage.ObjectFragment.Acceleration.Z = 9;
            originalMessage.ObjectFragment.Orientation.X = 10; originalMessage.ObjectFragment.Orientation.Y = 11; originalMessage.ObjectFragment.Orientation.Z = 12; originalMessage.ObjectFragment.Orientation.W = 13;
            originalMessage.ObjectFragment.AngularVelocity.X = 14; originalMessage.ObjectFragment.AngularVelocity.Y = 15; originalMessage.ObjectFragment.AngularVelocity.Z = 16; originalMessage.ObjectFragment.AngularVelocity.W = 17;
            originalMessage.ObjectFragment.AngularAcceleration.X = 18; originalMessage.ObjectFragment.AngularAcceleration.Y = 19; originalMessage.ObjectFragment.AngularAcceleration.Z = 20; originalMessage.ObjectFragment.AngularAcceleration.W = 21;
            originalMessage.ObjectFragment.BoundingSphereRadius = 22;
            originalMessage.ObjectFragment.Mass = 23;

            OmSlPrimitiveExt originalExtensionFragment = new OmSlPrimitiveExt();
            originalExtensionFragment.State = 24;
            originalExtensionFragment.NameValue = "TestExtensionNameValue";
            MsdVector3f scale = new MsdVector3f();
            scale.X = 1;
            scale.Y = 2;
            scale.Z = 3;
            originalExtensionFragment.Scale = scale;

            originalMessage.SetExtension<OmSlPrimitiveExt>(originalExtensionFragment);
            originalMessage.PrepareEncoding();

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount, 2);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(1), 49);

            int currentIndex = originalMessage.EncodeFrameData(0, encodedBytes, 0);
            currentIndex = originalMessage.EncodeFrameData(1, encodedBytes, currentIndex);

            PerceptionEventMessage decodedMessage = new PerceptionEventMessage();

            int currentDecodeIndex = decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));
            currentDecodeIndex = decodedMessage.DecodeFrameData(1, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(1));

            decodedMessage.MessageId = originalMessage.MessageId;

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

            OmSlPrimitiveExt decodedExtensionFragmet = decodedMessage.GetExtension<OmSlPrimitiveExt>();

            Assert.AreEqual(originalExtensionFragment.State, decodedExtensionFragmet.State);
            Assert.AreEqual(originalExtensionFragment.NameValue, decodedExtensionFragmet.NameValue);

        }

    }
}
