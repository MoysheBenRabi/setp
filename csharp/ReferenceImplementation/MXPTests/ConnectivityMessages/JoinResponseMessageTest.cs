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
    public class JoinResponseMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void JoinResponseMessageEncoding()
        {
            JoinResponseMessage originalMessage = new JoinResponseMessage();


            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            originalMessage.BubbleId = Guid.Empty;
            originalMessage.ParticipantId = Guid.Empty;
            originalMessage.AvatarId = Guid.Empty;
            originalMessage.BubbleName = "TestBubbleName";
            originalMessage.BubbleAssetCacheUrl = "TestBubbleAssetCacheUrl";
            originalMessage.BubbleRange = 3;
            originalMessage.BubblePerceptionRange = 4;
            originalMessage.BubbleRealTime = 5;

            originalMessage.ProgramName = "TestProgramName";
            originalMessage.ProgramMajorVersion = 6;
            originalMessage.ProgramMinorVersion = 7;
            originalMessage.ProtocolMajorVersion = 8;
            originalMessage.ProtocolMinorVersion = 9;
            originalMessage.ProtocolSourceRevision = 10;

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            JoinResponseMessage decodedMessage = new JoinResponseMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void JoinResponseMessageClear()
        {
            JoinResponseMessage originalMessage = new JoinResponseMessage();

            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            originalMessage.BubbleId = Guid.Empty;
            originalMessage.ParticipantId = Guid.Empty;
            originalMessage.AvatarId = Guid.Empty;
            originalMessage.BubbleName = "TestBubbleName";
            originalMessage.BubbleAssetCacheUrl = "TestBubbleAssetCacheUrl";
            originalMessage.BubbleRange = 3;
            originalMessage.BubblePerceptionRange = 4;
            originalMessage.BubbleRealTime = 5;

            originalMessage.ProgramName = "TestProgramName";
            originalMessage.ProgramMajorVersion = 6;
            originalMessage.ProgramMinorVersion = 7;
            originalMessage.ProtocolMajorVersion = 8;
            originalMessage.ProtocolMinorVersion = 9;
            originalMessage.ProtocolSourceRevision = 10;

            originalMessage.Clear();
            JoinResponseMessage emptyMessage = new JoinResponseMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
