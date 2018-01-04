using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP.Messages;
using System.Diagnostics;

namespace MXPTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestFixture]
    public class JoinRequestMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void JoinRequestMessageEncoding()
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
            originalMessage.ProtocolSourceRevision = 5;

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            JoinRequestMessage decodedMessage = new JoinRequestMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void JoinRequestMessageClear()
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

            originalMessage.Clear();
            JoinRequestMessage emptyMessage = new JoinRequestMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
