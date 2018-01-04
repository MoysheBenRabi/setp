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
    public class ListBubblesResponseTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ListBubblesResponseEncoding()
        {
            ListBubblesResponse originalMessage = new ListBubblesResponse();

            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble1";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange=5;
                bubbleEntry.BubblePerceptionRange=6;
                bubbleEntry.BubbleRealTime=7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble2";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble3";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble4";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount,4);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(1), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(2), 255);
            Assert.AreEqual(originalMessage.FrameDataSize(3), 255);

            int currentIndex = originalMessage.EncodeFrameData(0, encodedBytes, 0);
            currentIndex = originalMessage.EncodeFrameData(1, encodedBytes, currentIndex);
            currentIndex = originalMessage.EncodeFrameData(2, encodedBytes, currentIndex);
            currentIndex = originalMessage.EncodeFrameData(3, encodedBytes, currentIndex);

            ListBubblesResponse decodedMessage = new ListBubblesResponse();

            int currentDecodeIndex = decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));
            currentDecodeIndex = decodedMessage.DecodeFrameData(1, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(1));
            currentDecodeIndex = decodedMessage.DecodeFrameData(2, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(2));
            currentDecodeIndex = decodedMessage.DecodeFrameData(3, encodedBytes, currentDecodeIndex, originalMessage.FrameDataSize(3));
         
            decodedMessage.MessageId = originalMessage.MessageId;

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

            Assert.AreEqual(originalMessage.BubbleFragments.Count, decodedMessage.BubbleFragments.Count);

            for (int i = 0; i < originalMessage.BubbleFragments.Count; i++)
            {
                Assert.AreEqual(originalMessage.BubbleFragments[i].BubbleId, decodedMessage.BubbleFragments[i].BubbleId);
                Assert.AreEqual(originalMessage.BubbleFragments[i].BubbleName, decodedMessage.BubbleFragments[i].BubbleName);
            }

        }

        [Test]
        public void ListBubblesResponseClear()
        {
            ListBubblesResponse originalMessage = new ListBubblesResponse();

            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble1";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble2";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble3";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble4";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }
            {
                BubbleFragment bubbleEntry = new BubbleFragment();
                bubbleEntry.BubbleId = Guid.NewGuid();
                bubbleEntry.BubbleName = "TestBubble5";
                bubbleEntry.BubbleAssetCacheUrl = "TestCloudUrl";
                bubbleEntry.BubbleAddress = "TestBubbleAddress";
                bubbleEntry.BubblePort = 1;
                bubbleEntry.BubbleCenter.X = 2;
                bubbleEntry.BubbleCenter.Y = 3;
                bubbleEntry.BubbleCenter.Z = 4;
                bubbleEntry.BubbleRange = 5;
                bubbleEntry.BubblePerceptionRange = 6;
                bubbleEntry.BubbleRealTime = 7;
                originalMessage.AddBubbleFragment(bubbleEntry);
            }

            originalMessage.Clear();
            ListBubblesResponse emptyMessage = new ListBubblesResponse();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
