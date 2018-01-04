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
    public class InteractResponseTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void InteractResponseMessageOneFrameEncoding()
        {
            InteractResponseMessage originalMessage = new InteractResponseMessage();
            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;
            originalMessage.InteractionFragment.InteractionName = "TestInteractionName";
            originalMessage.InteractionFragment.SourceParticipantId = Guid.NewGuid();
            originalMessage.InteractionFragment.SourceObjectId = Guid.NewGuid();
            originalMessage.InteractionFragment.TargetParticipantId = Guid.NewGuid();
            originalMessage.InteractionFragment.TargetObjectId = Guid.NewGuid();
            originalMessage.InteractionFragment.ExtensionDialect = "TEST";
            originalMessage.SetPayloadData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890123456"));

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount, 1);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 255);

            originalMessage.EncodeFrameData(0, encodedBytes, 0);

            InteractResponseMessage decodedMessage = new InteractResponseMessage();

            decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

        }


        [Test]
        public void InteractResponseMessageTwoFrameEncoding()
        {
            InteractResponseMessage originalMessage = new InteractResponseMessage();
            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            originalMessage.InteractionFragment.InteractionName = "TestInteractionName";
            originalMessage.InteractionFragment.SourceParticipantId = Guid.NewGuid();
            originalMessage.InteractionFragment.SourceObjectId = Guid.NewGuid();
            originalMessage.InteractionFragment.TargetParticipantId = Guid.NewGuid();
            originalMessage.InteractionFragment.TargetObjectId = Guid.NewGuid();
            originalMessage.InteractionFragment.ExtensionDialect = "TEST";
            originalMessage.SetPayloadData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount, 2);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(1), 244);

            int currentIndex = originalMessage.EncodeFrameData(0, encodedBytes, 0);
            currentIndex = originalMessage.EncodeFrameData(1, encodedBytes, currentIndex);

            InteractResponseMessage decodedMessage = new InteractResponseMessage();

            int currentDecodeIndex = decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));
            currentDecodeIndex = decodedMessage.DecodeFrameData(1, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(1));

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

        }


        [Test]
        public void InteractResponseMessageThreeFrameEncoding()
        {
            InteractResponseMessage originalMessage = new InteractResponseMessage();
            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            originalMessage.InteractionFragment.InteractionName = "TestInteractionName";
            originalMessage.InteractionFragment.SourceParticipantId = Guid.NewGuid();
            originalMessage.InteractionFragment.SourceObjectId = Guid.NewGuid();
            originalMessage.InteractionFragment.TargetParticipantId = Guid.NewGuid();
            originalMessage.InteractionFragment.TargetObjectId = Guid.NewGuid();
            originalMessage.InteractionFragment.ExtensionDialect = "TEST";
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
            Assert.AreEqual(originalMessage.FrameDataSize(2), 89);

            int currentIndex = originalMessage.EncodeFrameData(0, encodedBytes, 0);
            currentIndex = originalMessage.EncodeFrameData(1, encodedBytes, currentIndex);
            currentIndex = originalMessage.EncodeFrameData(2, encodedBytes, currentIndex);

            InteractResponseMessage decodedMessage = new InteractResponseMessage();

            int currentDecodeIndex = decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));
            currentDecodeIndex = decodedMessage.DecodeFrameData(1, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(1));
            currentDecodeIndex = decodedMessage.DecodeFrameData(2, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(2));

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

        }

        [Test]
        public void InteractResponseMessageClear()
        {
            InteractResponseMessage originalMessage = new InteractResponseMessage();
            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            originalMessage.InteractionFragment.InteractionName = "TestInteractionName";
            originalMessage.InteractionFragment.SourceParticipantId = Guid.NewGuid();
            originalMessage.InteractionFragment.SourceObjectId = Guid.NewGuid();
            originalMessage.InteractionFragment.TargetParticipantId = Guid.NewGuid();
            originalMessage.InteractionFragment.TargetObjectId = Guid.NewGuid();
            originalMessage.InteractionFragment.ExtensionDialect = "TEST";
            originalMessage.SetPayloadData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));

            originalMessage.Clear();
            InteractResponseMessage emptyMessage = new InteractResponseMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
