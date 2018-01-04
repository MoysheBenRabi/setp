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
    public class KeepaliveMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void KeepaliveMessageEncoding()
        {
            KeepaliveMessage originalMessage = new KeepaliveMessage();

            byte[] encodedBytes = new byte[256];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            KeepaliveMessage decodedMessage = new KeepaliveMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void KeepaliveMessageClear()
        {
            KeepaliveMessage originalMessage = new KeepaliveMessage();

            originalMessage.Clear();
            KeepaliveMessage emptyMessage = new KeepaliveMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
