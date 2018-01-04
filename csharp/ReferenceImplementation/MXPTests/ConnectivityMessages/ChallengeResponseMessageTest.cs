﻿using System;
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
    public class ChallengeResponseMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ChallengeResponseMessageEncoding()
        {
            ChallengeResponseMessage originalMessage = new ChallengeResponseMessage();
            
            for (int i = 0; i < originalMessage.ChallengeResponseBytes.Length; i++)
            {
                originalMessage.ChallengeResponseBytes[i] = (byte)i;
            }

            byte[] encodedBytes = new byte[256];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            ChallengeResponseMessage decodedMessage = new ChallengeResponseMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void ChallengeResponseMessageClear()
        {
            ChallengeResponseMessage originalMessage = new ChallengeResponseMessage();

            for (int i = 0; i < originalMessage.ChallengeResponseBytes.Length; i++)
            {
                originalMessage.ChallengeResponseBytes[i] = (byte)i;
            }

            originalMessage.Clear();
            ChallengeResponseMessage emptyMessage = new ChallengeResponseMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
