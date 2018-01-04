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
    public class SynchronizationBeginEventMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void SynchronizationBeginEventMessageEncoding()
        {
            SynchronizationBeginEventMessage originalMessage = new SynchronizationBeginEventMessage();

            originalMessage.ObjectCount = 1;

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            SynchronizationBeginEventMessage decodedMessage = new SynchronizationBeginEventMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void SynchronizationBeginEventMessageClear()
        {
            SynchronizationBeginEventMessage originalMessage = new SynchronizationBeginEventMessage();

            originalMessage.ObjectCount = 1;

            originalMessage.Clear();
            SynchronizationBeginEventMessage emptyMessage = new SynchronizationBeginEventMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
