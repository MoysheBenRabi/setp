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
    /// TODO this messages frame count needs to be fixed and the unit test as well for testing it properly.
    /// </summary>
    [TestFixture]
    public class ModifyResponseMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ModifyResponseMessageEncoding()
        {
            ModifyResponseMessage originalMessage = new ModifyResponseMessage();

            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            ModifyResponseMessage decodedMessage = new ModifyResponseMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void ModifyResponseMessageClear()
        {
            ModifyResponseMessage originalMessage = new ModifyResponseMessage();

            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            originalMessage.Clear();
            ModifyResponseMessage emptyMessage = new ModifyResponseMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
