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
    public class AcknowledgeMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void AcknowledgeMessageEncoding()
        {

            AcknowledgeMessage originalMessage = new AcknowledgeMessage();
            originalMessage.AddPacketId(1);
            originalMessage.AddPacketId(2);
            originalMessage.AddPacketId(3);
            originalMessage.AddPacketId(4);
            originalMessage.AddPacketId(5);
            Assert.AreEqual(5, originalMessage.PacketIdCount);

            byte[] encodedBytes = new byte[256];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            AcknowledgeMessage decodedMessage = new AcknowledgeMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            decodedMessage.MessageId = originalMessage.MessageId;

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

            Assert.AreEqual((uint)1,decodedMessage.GetPacketId(0));
            Assert.AreEqual((uint)2, decodedMessage.GetPacketId(1));
            Assert.AreEqual((uint)3, decodedMessage.GetPacketId(2));
            Assert.AreEqual((uint)4, decodedMessage.GetPacketId(3));
            Assert.AreEqual((uint)5, decodedMessage.GetPacketId(4));
        }

        [Test]
        public void AcknowledgeMessageClear()
        {
            AcknowledgeMessage originalMessage = new AcknowledgeMessage();
            originalMessage.AddPacketId(1);
            originalMessage.AddPacketId(2);
            originalMessage.AddPacketId(3);
            originalMessage.AddPacketId(4);
            originalMessage.AddPacketId(5);

            originalMessage.Clear();
            AcknowledgeMessage emptyMessage = new AcknowledgeMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
