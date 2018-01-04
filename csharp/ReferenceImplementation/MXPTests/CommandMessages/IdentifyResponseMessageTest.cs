using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP.Messages;
using System.Diagnostics;

namespace MXPTests
{
    /// <summary>
    /// Summary description for IdentifyResponseMessageTest
    /// </summary>
    [TestFixture]
    public class IdentifyResponseMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void IdentifyResponseMessageEncoding()
        {
            IdentifyResponseMessage originalMessage = new IdentifyResponseMessage();

            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            IdentifyResponseMessage decodedMessage = new IdentifyResponseMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);
            
            decodedMessage.MessageId = originalMessage.MessageId;        

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void IdentifyResponseMessageClear()
        {
            IdentifyResponseMessage originalMessage = new IdentifyResponseMessage();

            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            originalMessage.Clear();
            IdentifyResponseMessage emptyMessage = new IdentifyResponseMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
