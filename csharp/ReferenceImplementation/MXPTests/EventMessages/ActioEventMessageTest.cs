using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP.Messages;
using System.Diagnostics;
using MXP;
using MXP.Fragments;

namespace MXPTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestFixture]
    public class ActionEventTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ActionMessageOneFrameEncoding()
        {
            ActionEventMessage originalMessage = new ActionEventMessage();

            originalMessage.ActionFragment.ActionName = "TestInteractionName";
            originalMessage.ActionFragment.SourceObjectId = Guid.NewGuid();
            originalMessage.ActionFragment.ObservationRadius = 100;
            originalMessage.ActionFragment.ExtensionDialect = "TEST";
            originalMessage.ActionFragment.ExtensionDialectMajorVersion = 1;
            originalMessage.ActionFragment.ExtensionDialectMinorVersion = 2;
            originalMessage.SetPayloadData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "12345"));

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount, 1);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 255);

            originalMessage.EncodeFrameData(0, encodedBytes, 0);

            ActionEventMessage decodedMessage = new ActionEventMessage();

            decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));

            // Message id is not decoded by DecodeFrameData
            decodedMessage.MessageId = originalMessage.MessageId;

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

        }


        [Test]
        public void ActionMessageTwoFrameEncoding()
        {
            ActionEventMessage originalMessage = new ActionEventMessage();

            originalMessage.ActionFragment.ActionName = "TestInteractionName";
            originalMessage.ActionFragment.SourceObjectId = Guid.NewGuid();
            originalMessage.ActionFragment.ObservationRadius = 100;
            originalMessage.ActionFragment.ExtensionDialect = "TEST";
            originalMessage.ActionFragment.ExtensionDialectMajorVersion = 1;
            originalMessage.ActionFragment.ExtensionDialectMinorVersion = 2;
            originalMessage.SetPayloadData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount, 2);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(1), 195);

            int currentIndex = originalMessage.EncodeFrameData(0, encodedBytes, 0);
            currentIndex = originalMessage.EncodeFrameData(1, encodedBytes, currentIndex);

            ActionEventMessage decodedMessage = new ActionEventMessage();

            int currentDecodeIndex = decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));
            currentDecodeIndex = decodedMessage.DecodeFrameData(1, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(1));

            // Message id is not decoded by DecodeFrameData
            decodedMessage.MessageId = originalMessage.MessageId;

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

        }


        [Test]
        public void ActionMessageThreeFrameEncoding()
        {
            ActionEventMessage originalMessage = new ActionEventMessage();

            originalMessage.ActionFragment.ActionName = "TestInteractionName";
            originalMessage.ActionFragment.SourceObjectId = Guid.NewGuid();
            originalMessage.ActionFragment.ObservationRadius = 100;

            originalMessage.ActionFragment.ExtensionDialect = "TEST";
            originalMessage.SetPayloadData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"+                
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"+
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"+
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"+
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount,3);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(1), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(2), 40);

            int currentIndex = originalMessage.EncodeFrameData(0, encodedBytes, 0);
            currentIndex = originalMessage.EncodeFrameData(1, encodedBytes, currentIndex);
            currentIndex = originalMessage.EncodeFrameData(2, encodedBytes, currentIndex);

            ActionEventMessage decodedMessage = new ActionEventMessage();

            int currentDecodeIndex = decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));
            currentDecodeIndex = decodedMessage.DecodeFrameData(1, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(1));
            currentDecodeIndex = decodedMessage.DecodeFrameData(2, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(2));

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

        }

        [Test]
        public void ActionMessageClear()
        {
            ActionEventMessage originalMessage = new ActionEventMessage();

            originalMessage.ActionFragment.ActionName = "TestInteractionName";
            originalMessage.ActionFragment.SourceObjectId = Guid.NewGuid();
            originalMessage.ActionFragment.ObservationRadius = 100;
            originalMessage.ActionFragment.ExtensionDialect = "TEST";
            originalMessage.ActionFragment.ExtensionDialectMajorVersion = 1;
            originalMessage.ActionFragment.ExtensionDialectMinorVersion = 2;
            originalMessage.SetPayloadData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));

            originalMessage.Clear();
            ActionEventMessage emptyMessage = new ActionEventMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
