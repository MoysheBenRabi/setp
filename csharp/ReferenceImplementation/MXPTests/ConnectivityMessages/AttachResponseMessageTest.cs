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
    public class AttachResponseMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void AttachResponseMessageEncoding()
        {
            AttachResponseMessage originalMessage = new AttachResponseMessage();


            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            originalMessage.TargetBubbleFragment.BubbleId = Guid.NewGuid();
            originalMessage.TargetBubbleFragment.BubbleName = "TestBubble1";
            originalMessage.TargetBubbleFragment.BubbleAssetCacheUrl = "TestCloudUrl";
            originalMessage.TargetBubbleFragment.BubbleAddress = "TestBubbleAddress";
            originalMessage.TargetBubbleFragment.BubblePort = 1;
            originalMessage.TargetBubbleFragment.BubbleCenter.X = 2;
            originalMessage.TargetBubbleFragment.BubbleCenter.Y = 3;
            originalMessage.TargetBubbleFragment.BubbleCenter.Z = 4;
            originalMessage.TargetBubbleFragment.BubbleRange = 5;
            originalMessage.TargetBubbleFragment.BubblePerceptionRange = 6;
            originalMessage.TargetBubbleFragment.BubbleRealTime = 7;

            originalMessage.ProgramName = "TestProgramName";
            originalMessage.ProgramMajorVersion = 1;
            originalMessage.ProgramMinorVersion = 2;
            originalMessage.ProtocolMajorVersion = 3;
            originalMessage.ProtocolMinorVersion = 4;
            originalMessage.ProtocolSourceRevision = 5;

            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            AttachResponseMessage decodedMessage = new AttachResponseMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void AttachResponseMessageClear()
        {
            AttachResponseMessage originalMessage = new AttachResponseMessage();

            originalMessage.RequestMessageId = 1;
            originalMessage.FailureCode = 2;

            originalMessage.TargetBubbleFragment.BubbleId = Guid.NewGuid();
            originalMessage.TargetBubbleFragment.BubbleName = "TestBubble1";
            originalMessage.TargetBubbleFragment.BubbleAssetCacheUrl = "TestCloudUrl";
            originalMessage.TargetBubbleFragment.BubbleAddress = "TestBubbleAddress";
            originalMessage.TargetBubbleFragment.BubblePort = 1;
            originalMessage.TargetBubbleFragment.BubbleCenter.X = 2;
            originalMessage.TargetBubbleFragment.BubbleCenter.Y = 3;
            originalMessage.TargetBubbleFragment.BubbleCenter.Z = 4;
            originalMessage.TargetBubbleFragment.BubbleRange = 5;
            originalMessage.TargetBubbleFragment.BubblePerceptionRange = 6;
            originalMessage.TargetBubbleFragment.BubbleRealTime = 7;

            originalMessage.ProgramName = "TestProgramName";
            originalMessage.ProgramMajorVersion = 1;
            originalMessage.ProgramMinorVersion = 2;
            originalMessage.ProtocolMajorVersion = 3;
            originalMessage.ProtocolMinorVersion = 4;
            originalMessage.ProtocolSourceRevision = 5;

            originalMessage.Clear();

            AttachResponseMessage emptyMessage = new AttachResponseMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
