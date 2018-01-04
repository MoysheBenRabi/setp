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
    public class ThrottleMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ThrottleMessageEncoding()
        {
            ThrottleMessage originalMessage = new ThrottleMessage();
            originalMessage.BytesPerSecond = 10000;

            byte[] encodedBytes = new byte[256];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            ThrottleMessage decodedMessage = new ThrottleMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

            Assert.AreEqual(originalMessage.BytesPerSecond, decodedMessage.BytesPerSecond);
        }

        [Test]
        public void ThrottleMessageClear()
        {
            ThrottleMessage originalMessage = new ThrottleMessage();
            originalMessage.BytesPerSecond = 10000;

            originalMessage.Clear();
            ThrottleMessage emptyMessage = new ThrottleMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
