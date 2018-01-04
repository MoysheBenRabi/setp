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
    public class DisappearanceEventMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void DisappearanceMessageEncoding()
        {
            DisappearanceEventMessage originalMessage = new DisappearanceEventMessage();

            originalMessage.ObjectIndex = 1;

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            DisappearanceEventMessage decodedMessage = new DisappearanceEventMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void DisappearanceMessageClear()
        {
            DisappearanceEventMessage originalMessage = new DisappearanceEventMessage();

            originalMessage.ObjectIndex = 1;

            originalMessage.Clear();
            DisappearanceEventMessage emptyMessage = new DisappearanceEventMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
