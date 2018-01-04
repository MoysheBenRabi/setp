using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP.Messages;
using MXP;
using MXP.Fragments;
using MXP.Util;

namespace MXPTests
{
    /// <summary>
    /// Summary description for PacketEncoderTest
    /// </summary>
    [TestFixture]
    public class PacketEncoderTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void PacketEncodingSingleFrame()
        {
            JoinRequestMessage originalMessage = new JoinRequestMessage();

            originalMessage.BubbleId = Guid.NewGuid();
            originalMessage.LocationName = "TestLocation";
            originalMessage.IdentityProviderUrl = "IdentityProviderUrl";
            originalMessage.ParticipantIdentifier = "TestParticipantName";
            originalMessage.ParticipantSecret = "TestParticipantPassphrase";
            originalMessage.ParticipantRealTime = 10;

            originalMessage.ProgramName = "TestProgramName";
            originalMessage.ProgramMajorVersion = 1;
            originalMessage.ProgramMinorVersion = 2;
            originalMessage.ProtocolMajorVersion = 3;
            originalMessage.ProtocolMinorVersion = 4;

            Packet originalPacket = new Packet();
            originalPacket.SessionId = 1;
            originalPacket.PacketId = 2;
            originalPacket.FirstSendTime = 3;
            originalPacket.ResendCount = 4;
            originalPacket.Quaranteed = true;            

            Session senderSession = new Session();
            senderSession.Send(originalMessage);

            PacketEncoder.EncodePacketHeader(originalPacket);
            bool packetQuaranteed = false;
            PacketEncoder.EncodePacketData(senderSession, originalPacket, ref packetQuaranteed);
            Assert.IsTrue(packetQuaranteed);

            Session receiverSession = new Session();

            Packet receivedPacket = new Packet();
            receivedPacket.PacketLength = originalPacket.PacketLength;
            receivedPacket.PacketBytes = originalPacket.PacketBytes;

            PacketEncoder.DecodePacketHeader(receivedPacket);

            packetQuaranteed = false;
            PacketEncoder.DecodePacketData(receiverSession, receivedPacket, ref packetQuaranteed);
            Assert.IsTrue(packetQuaranteed);

            Message decodedMessage = receiverSession.Receive();

            Assert.IsNotNull(decodedMessage);

            Assert.AreEqual(0, senderSession.GetOutboundMessageCount());
            Assert.AreEqual(0, senderSession.GetPartiallySentMessageCount());
            Assert.AreEqual(0, senderSession.AvailableMessages);
            Assert.AreEqual(0, senderSession.GetPartiallyReceivedMessageCount());
            Assert.AreEqual(0, receiverSession.GetOutboundMessageCount());
            Assert.AreEqual(0, receiverSession.GetPartiallySentMessageCount());
            Assert.AreEqual(0, receiverSession.AvailableMessages);
            Assert.AreEqual(0, receiverSession.GetPartiallyReceivedMessageCount());

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            
            decodedMessage.IsAutoRelease = false;

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

            String originalPacketString = originalPacket.ToString();
            String receivedPacketString = receivedPacket.ToString();
            Assert.AreEqual(originalPacketString, receivedPacketString);
            
        }

        [Test]
        public void PacketEncodingMultiFrame()
        {
            ListBubblesResponse originalMessage = new ListBubblesResponse();

            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble1";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble2";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble3";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble4";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble5";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }

            Packet originalPacket = new Packet();
            originalPacket.SessionId = 1;
            originalPacket.PacketId = 2;
            originalPacket.FirstSendTime = 3;
            originalPacket.ResendCount = 4;
            originalPacket.Quaranteed = true;

            Session senderSession = new Session();
            senderSession.Send(originalMessage);

            PacketEncoder.EncodePacketHeader(originalPacket);
            bool packetQuaranteed = false;
            PacketEncoder.EncodePacketData(senderSession, originalPacket, ref packetQuaranteed);
            Assert.IsTrue(packetQuaranteed);

            Session receiverSession = new Session();

            Packet receivedPacket = new Packet();
            receivedPacket.PacketLength = originalPacket.PacketLength;
            receivedPacket.PacketBytes = originalPacket.PacketBytes;

            PacketEncoder.DecodePacketHeader(receivedPacket);

            packetQuaranteed = false;
            PacketEncoder.DecodePacketData(receiverSession, receivedPacket, ref packetQuaranteed);
            Assert.IsTrue(packetQuaranteed);

            Message decodedMessage = receiverSession.Receive();

            Assert.IsNotNull(decodedMessage);

            Assert.AreEqual(0, senderSession.GetOutboundMessageCount());
            Assert.AreEqual(0, senderSession.GetPartiallySentMessageCount());
            Assert.AreEqual(0, senderSession.AvailableMessages);
            Assert.AreEqual(0, senderSession.GetPartiallyReceivedMessageCount());
            Assert.AreEqual(0, receiverSession.GetOutboundMessageCount());
            Assert.AreEqual(0, receiverSession.GetPartiallySentMessageCount());
            Assert.AreEqual(0, receiverSession.AvailableMessages);
            Assert.AreEqual(0, receiverSession.GetPartiallyReceivedMessageCount());

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

            String originalPacketString = originalPacket.ToString();
            String receivedPacketString = receivedPacket.ToString();
            Assert.AreEqual(originalPacketString, receivedPacketString);

        }


        [Test]
        public void PacketEncodingPerceptionMessageMultiFrame()
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
            originalMessage.ObjectFragment.ExtensionDialect = "TEST";
            originalMessage.ObjectFragment.SetExtensionData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));

            Packet originalPacket = new Packet();
            originalPacket.SessionId = 1;
            originalPacket.PacketId = 2;
            originalPacket.FirstSendTime = 3;
            originalPacket.ResendCount = 4;
            originalPacket.Quaranteed = true;

            Session senderSession = new Session();
            senderSession.Send(originalMessage);

            PacketEncoder.EncodePacketHeader(originalPacket);
            bool packetQuaranteed = false;
            PacketEncoder.EncodePacketData(senderSession, originalPacket, ref packetQuaranteed);
            Assert.IsTrue(packetQuaranteed);

            Session receiverSession = new Session();

            Packet receivedPacket = new Packet();
            receivedPacket.PacketLength = originalPacket.PacketLength;
            receivedPacket.PacketBytes = originalPacket.PacketBytes;

            List<int> highlightedIndexes = new List<int>();
            highlightedIndexes.Add(18+5);
            highlightedIndexes.Add(18+265+5);

            PacketEncoder.DecodePacketHeader(receivedPacket);

            Assert.AreEqual(2,receivedPacket.PacketBytes[18+5]);
            Assert.AreEqual(2,receivedPacket.PacketBytes[18+265+5]);

            packetQuaranteed = false;
            PacketEncoder.DecodePacketData(receiverSession, receivedPacket, ref packetQuaranteed);
            Assert.IsTrue(packetQuaranteed);

            Message decodedMessage = receiverSession.Receive();

            Assert.IsNotNull(decodedMessage);

            Assert.AreEqual(0, senderSession.GetOutboundMessageCount());
            Assert.AreEqual(0, senderSession.GetPartiallySentMessageCount());
            Assert.AreEqual(0, senderSession.AvailableMessages);
            Assert.AreEqual(0, senderSession.GetPartiallyReceivedMessageCount());
            Assert.AreEqual(0, receiverSession.GetOutboundMessageCount());
            Assert.AreEqual(0, receiverSession.GetPartiallySentMessageCount());
            Assert.AreEqual(0, receiverSession.AvailableMessages);
            Assert.AreEqual(0, receiverSession.GetPartiallyReceivedMessageCount());



            decodedMessage.MessageId = originalMessage.MessageId;
            decodedMessage.IsAutoRelease = false;

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

            String originalPacketString = originalPacket.ToString();
            String receivedPacketString = receivedPacket.ToString();
            Assert.AreEqual(originalPacketString, receivedPacketString);

        }


    }
}
