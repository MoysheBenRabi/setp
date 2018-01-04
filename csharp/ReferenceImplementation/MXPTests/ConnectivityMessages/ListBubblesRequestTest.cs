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
    public class ListBubblesRequestTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ListBubblesRequestEncoding()
        {
            ListBubblesRequest originalMessage = new ListBubblesRequest();
            originalMessage.ListType = ListBubblesRequest.ListTypeHosted;

            byte[] encodedBytes = new byte[256];

            int dataLength=originalMessage.EncodeFrameData(0, encodedBytes, 0);

            ListBubblesRequest decodedMessage = new ListBubblesRequest();
            decodedMessage.DecodeFrameData(0, encodedBytes, 0, dataLength);

            
            
            decodedMessage.MessageId = originalMessage.MessageId;
            

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
        }

        [Test]
        public void ListBubblesRequestClear()
        {
            ListBubblesRequest originalMessage = new ListBubblesRequest();
            originalMessage.ListType = ListBubblesRequest.ListTypeHosted;

            originalMessage.Clear();
            ListBubblesRequest emptyMessage = new ListBubblesRequest();
            emptyMessage.MessageId = originalMessage.MessageId;
            Assert.AreEqual(originalMessage.ToString(), emptyMessage.ToString());
        }
    }
}
