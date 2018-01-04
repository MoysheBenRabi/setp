using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP;
using MXP.Messages;

namespace MXPTests
{
    /// <summary>
    /// Summary description for FrameEncoderTest
    /// </summary>
    [TestFixture]
    public class FrameEncoderTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void FrameEncoding()
        {
            JoinRequestMessage originalMessage = new JoinRequestMessage();

            originalMessage.BubbleId = Guid.NewGuid();
            originalMessage.BubbleName = "TestBubbleName";
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

            Session senderSession = new Session();
            senderSession.Send(originalMessage);
            
            byte[] packetBytes=new byte[300];
            bool quaranteed = false;
            FrameEncoder.EncodeFrame(senderSession, packetBytes, 0, ref quaranteed);

            Assert.IsTrue(quaranteed);

            Session receiverSession = new Session();
            FrameEncoder.DecodeFrame(receiverSession, packetBytes, 0, ref quaranteed);

            Assert.IsTrue(quaranteed);

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
        }

    }
}
