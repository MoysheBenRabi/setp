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
    public class SynchronizationEndEventMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void SynchronizationEndEventMessageEncoding()
        {
            SynchronizationEndEventMessage originalMessage = new SynchronizationEndEventMessage();

            byte[] encodedBytes = new byte[256];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            SynchronizationEndEventMessage decodedMessage = new SynchronizationEndEventMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void SynchronizationEndEventMessageClear()
        {
            SynchronizationEndEventMessage originalMessage = new SynchronizationEndEventMessage();

            originalMessage.Clear();
            SynchronizationEndEventMessage emptyMessage = new SynchronizationEndEventMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
