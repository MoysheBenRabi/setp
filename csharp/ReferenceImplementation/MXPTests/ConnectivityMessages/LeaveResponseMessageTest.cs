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
    public class LeaveResponseMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void LeaveResponseMessageEncoding()
        {
            LeaveResponseMessage originalMessage = new LeaveResponseMessage();

            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            LeaveResponseMessage decodedMessage = new LeaveResponseMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void LeaveResponseMessageClear()
        {
            LeaveResponseMessage originalMessage = new LeaveResponseMessage();

            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            originalMessage.Clear();
            LeaveResponseMessage emptyMessage = new LeaveResponseMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
