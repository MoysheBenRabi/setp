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
    public class AttachRequestMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void AttachRequestMessageEncoding()
        {
            AttachRequestMessage originalMessage = new AttachRequestMessage();

            originalMessage.TargetBubbleId = Guid.NewGuid();

            originalMessage.SourceBubbleFragment.BubbleId = Guid.NewGuid();
            originalMessage.SourceBubbleFragment.BubbleName = "TestBubble1";
            originalMessage.SourceBubbleFragment.BubbleAssetCacheUrl = "TestCloudUrl";
            originalMessage.SourceBubbleFragment.BubbleAddress = "TestBubbleAddress";
            originalMessage.SourceBubbleFragment.BubblePort = 1;
            originalMessage.SourceBubbleFragment.BubbleCenter.X = 2;
            originalMessage.SourceBubbleFragment.BubbleCenter.Y = 3;
            originalMessage.SourceBubbleFragment.BubbleCenter.Z = 4;
            originalMessage.SourceBubbleFragment.BubbleRange = 5;
            originalMessage.SourceBubbleFragment.BubblePerceptionRange = 6;
            originalMessage.SourceBubbleFragment.BubbleRealTime = 7;

            originalMessage.ProgramName = "TestProgramName";
            originalMessage.ProgramMajorVersion = 1;
            originalMessage.ProgramMinorVersion = 2;
            originalMessage.ProtocolMajorVersion = 3;
            originalMessage.ProtocolMinorVersion = 4;
            originalMessage.ProtocolSourceRevision = 5;


            byte[] encodedBytes = new byte[originalMessage.FrameDataSize(0)];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            AttachRequestMessage decodedMessage = new AttachRequestMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
            Assert.AreEqual(originalMessage.SourceBubbleFragment.BubbleCenter.X, decodedMessage.SourceBubbleFragment.BubbleCenter.X);
            Assert.AreEqual(originalMessage.SourceBubbleFragment.BubbleCenter.Y, decodedMessage.SourceBubbleFragment.BubbleCenter.Y);
            Assert.AreEqual(originalMessage.SourceBubbleFragment.BubbleCenter.Z, decodedMessage.SourceBubbleFragment.BubbleCenter.Z);
        }

        [Test]
        public void AttachRequestMessageClear()
        {
            AttachRequestMessage originalMessage = new AttachRequestMessage();

            originalMessage.TargetBubbleId = Guid.NewGuid();

            originalMessage.SourceBubbleFragment.BubbleId = Guid.NewGuid();
            originalMessage.SourceBubbleFragment.BubbleName = "TestBubble1";
            originalMessage.SourceBubbleFragment.BubbleAssetCacheUrl = "TestCloudUrl";
            originalMessage.SourceBubbleFragment.BubbleAddress = "TestBubbleAddress";
            originalMessage.SourceBubbleFragment.BubblePort = 1;
            originalMessage.SourceBubbleFragment.BubbleCenter.X = 2;
            originalMessage.SourceBubbleFragment.BubbleCenter.Y = 3;
            originalMessage.SourceBubbleFragment.BubbleCenter.Z = 4;
            originalMessage.SourceBubbleFragment.BubbleRange = 5;
            originalMessage.SourceBubbleFragment.BubblePerceptionRange = 6;
            originalMessage.SourceBubbleFragment.BubbleRealTime = 7;

            originalMessage.ProgramName = "TestProgramName";
            originalMessage.ProgramMajorVersion = 1;
            originalMessage.ProgramMinorVersion = 2;
            originalMessage.ProtocolMajorVersion = 3;
            originalMessage.ProtocolMinorVersion = 4;
            originalMessage.ProtocolSourceRevision = 5;

            originalMessage.Clear();
            AttachRequestMessage emptyMessage = new AttachRequestMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
