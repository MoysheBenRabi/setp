﻿using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP.Messages;
using System.Diagnostics;
using MXP;
using MXP.Fragments;

namespace MXPTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestFixture]
    public class InjectRequestTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void InjectRequestMessageOneFrameEncoding()
        {
            InjectRequestMessage originalMessage = new InjectRequestMessage();

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
            originalMessage.ObjectFragment.ExtensionDialect = "TEST";
            originalMessage.ObjectFragment.ExtensionDialectMajorVersion = 24;
            originalMessage.ObjectFragment.ExtensionDialectMinorVersion = 25;
            originalMessage.ObjectFragment.SetExtensionData(UTF8Encoding.UTF8.GetBytes(
                "123456789012345678901234567890123456789012345"));
            originalMessage.PrepareEncoding();

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount, 1);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 255);

            originalMessage.EncodeFrameData(0, encodedBytes, 0);

            InjectRequestMessage decodedMessage = new InjectRequestMessage();

            decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

        }


        [Test]
        public void InjectRequestMessageTwoFrameEncoding()
        {
            InjectRequestMessage originalMessage = new InjectRequestMessage();

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
            originalMessage.ObjectFragment.ExtensionDialect = "TEST";
            originalMessage.ObjectFragment.SetExtensionData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));
            originalMessage.PrepareEncoding();

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount, 2);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(1), 255);

            int currentIndex = originalMessage.EncodeFrameData(0, encodedBytes, 0);
            currentIndex = originalMessage.EncodeFrameData(1, encodedBytes, currentIndex);

            InjectRequestMessage decodedMessage = new InjectRequestMessage();

            int currentDecodeIndex = decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));
            currentDecodeIndex = decodedMessage.DecodeFrameData(1, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(1));

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

        }


        [Test]
        public void InjectRequestMessageThreeFrameEncoding()
        {
            InjectRequestMessage originalMessage = new InjectRequestMessage();

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
            originalMessage.ObjectFragment.ExtensionDialect = "TEST";
            originalMessage.ObjectFragment.SetExtensionData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"+                
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"+
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"+
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));
            originalMessage.PrepareEncoding();

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount,3);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(1), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(2), 100);

            int currentIndex = originalMessage.EncodeFrameData(0, encodedBytes, 0);
            currentIndex = originalMessage.EncodeFrameData(1, encodedBytes, currentIndex);
            currentIndex = originalMessage.EncodeFrameData(2, encodedBytes, currentIndex);

            InjectRequestMessage decodedMessage = new InjectRequestMessage();

            int currentDecodeIndex = decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));
            currentDecodeIndex = decodedMessage.DecodeFrameData(1, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(1));
            currentDecodeIndex = decodedMessage.DecodeFrameData(2, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(2));

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

        }

        [Test]
        public void InjectRequestMessageClear()
        {
            InjectRequestMessage originalMessage = new InjectRequestMessage();
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
            originalMessage.ObjectFragment.ExtensionDialect = "TEST";
            originalMessage.ObjectFragment.SetExtensionData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));
            originalMessage.PrepareEncoding();

            originalMessage.Clear();
            InjectRequestMessage emptyMessage = new InjectRequestMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
