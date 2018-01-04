using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP.Messages;
using System.Diagnostics;
using MXP;
using MXP.Fragments;
using MXP.Extentions.OpenMetaverseFragments.Proto;

namespace OpenMetaverseExtensionTests
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
        public void ActionMessageEncoding()
        {
            ActionEventMessage originalMessage = new ActionEventMessage();

            originalMessage.ActionFragment.ActionName = "TestInteractionName";
            originalMessage.ActionFragment.SourceObjectId = Guid.NewGuid();
            originalMessage.ActionFragment.ObservationRadius = 100;

            OmChatExt originalChatFragment = new OmChatExt();
            originalChatFragment.Message = "TestChatMessage";

            originalMessage.SetExtension<OmChatExt>(originalChatFragment);

            byte[] encodedBytes = new byte[MxpConstants.MaxPacketSize];

            Assert.AreEqual(originalMessage.FrameCount, 1);
            Assert.AreEqual(originalMessage.FrameDataSize(0), 67);

            int currentIndex = originalMessage.EncodeFrameData(0, encodedBytes, 0);

            ActionEventMessage decodedMessage = new ActionEventMessage();
            decodedMessage.MessageId = originalMessage.MessageId;

            int currentDecodeIndex = decodedMessage.DecodeFrameData(0, encodedBytes, 0, originalMessage.FrameDataSize(0));

            OmChatExt decodedChatFragment = decodedMessage.GetExtension<OmChatExt>();

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

            Assert.AreEqual(originalChatFragment.Message, decodedChatFragment.Message);

        }

    }
}
