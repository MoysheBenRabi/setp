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
    public class MovementEventMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void MovementMessageEncoding()
        {
            MovementEventMessage originalMessage = new MovementEventMessage();

            originalMessage.ObjectIndex = 1;
            originalMessage.Location.X = 1; originalMessage.Location.Y = 2; originalMessage.Location.Z = 3;
            originalMessage.Orientation.X = 10; originalMessage.Orientation.Y = 11; originalMessage.Orientation.Z = 12; originalMessage.Orientation.W = 13;

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            MovementEventMessage decodedMessage = new MovementEventMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void MovementMessageClear()
        {
            MovementEventMessage originalMessage = new MovementEventMessage();

            originalMessage.ObjectIndex = 1;
            originalMessage.Location.X = 1; originalMessage.Location.Y = 2; originalMessage.Location.Z = 3;
            originalMessage.Orientation.X = 10; originalMessage.Orientation.Y = 11; originalMessage.Orientation.Z = 12; originalMessage.Orientation.W = 13;

            originalMessage.Clear();
            MovementEventMessage emptyMessage = new MovementEventMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
