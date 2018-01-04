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
    public class ChallengeRequestMessageTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ChallengeRequestMessageEncoding()
        {
            ChallengeRequestMessage originalMessage = new ChallengeRequestMessage();
            
            for (int i = 0; i < originalMessage.ChallengeRequestBytes.Length; i++)
            {
                originalMessage.ChallengeRequestBytes[i] = (byte)i;
            }

            byte[] encodedBytes = new byte[256];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            ChallengeRequestMessage decodedMessage = new ChallengeRequestMessage();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void ChallengeRequestMessageClear()
        {
            ChallengeRequestMessage originalMessage = new ChallengeRequestMessage();
            
            for (int i = 0; i < originalMessage.ChallengeRequestBytes.Length; i++)
            {
                originalMessage.ChallengeRequestBytes[i] = (byte)i;
            }

            originalMessage.Clear();
            ChallengeRequestMessage emptyMessage = new ChallengeRequestMessage();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
