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
    public class ExamineRequestMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ExamineRequestMessageEncoding()
        {
            ExamineRequestMessage originalMessage = new ExamineRequestMessage();

            originalMessage.ObjectId = Guid.NewGuid();
            originalMessage.ObjectIndex = 1;

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            ExamineRequestMessage decodedMessage = new ExamineRequestMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void ExamineRequestMessageClear()
        {
            ExamineRequestMessage originalMessage = new ExamineRequestMessage();
            originalMessage.ObjectIndex = 1;

            originalMessage.ObjectId = Guid.NewGuid();

            originalMessage.Clear();
            ExamineRequestMessage emptyMessage = new ExamineRequestMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
