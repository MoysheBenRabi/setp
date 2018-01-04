using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP.Messages;
using System.Diagnostics;

namespace MXPTests
{
    /// <summary>
    /// Summary description for IdentifyRequestMessageTest
    /// </summary>
    [TestFixture]
    public class IdentifyRequestMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void IdentifyRequestMessageEncoding()
        {
            IdentifyRequestMessage originalMessage = new IdentifyRequestMessage();

            originalMessage.ParticipantId = Guid.NewGuid();
            originalMessage.ParticipantIdentityType = "open-id";
            originalMessage.ParticipantIdentity = "test-open-id";

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            IdentifyRequestMessage decodedMessage = new IdentifyRequestMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void EjectRequestMessageClear()
        {
            IdentifyRequestMessage originalMessage = new IdentifyRequestMessage();

            originalMessage.ParticipantId = Guid.NewGuid();
            originalMessage.ParticipantIdentityType = "open-id";
            originalMessage.ParticipantIdentity = "test-open-id";

            originalMessage.Clear();
            IdentifyRequestMessage emptyMessage = new IdentifyRequestMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
