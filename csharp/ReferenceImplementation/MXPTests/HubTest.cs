using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP;
using System.Threading;
using MXP.Messages;

namespace MXPTests
{
    /// <summary>
    /// Summary description for ClientServerTest
    /// </summary>
    [TestFixture]
    public class HubTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void HubConnectionSuccess()
        {
            Guid sourceBubbleGuid = new Guid();
            Guid targetBubbleGuid = new Guid();

            MxpHub sourceHub = new MxpHub("http://test.cloud.url", "127.0.0.1", MxpConstants.DefaultHubPort, "TestServerProgram", 1, 2);

            MxpBubble sourceBubble = new MxpBubble(sourceBubbleGuid, "SourceBubbleName", 100, 1000);
            sourceHub.AddBubble(sourceBubble);


            Session sourceSession = null;

            sourceBubble.BubbleConnected += delegate(Session session, Message message)
            {
                sourceSession = session;
            };

            sourceHub.Startup(false);

            MxpHub targetHub = new MxpHub("http://test.cloud.url", "127.0.0.1", MxpConstants.DefaultHubPort + 1, "TestServerProgram", 1, 2);

            MxpBubble targetBubble = new MxpBubble(targetBubbleGuid, "TargetBubbleName", 100, 1000);
            targetHub.AddBubble(targetBubble);

            Session targetSession = null;

            targetBubble.BubbleConnectAuthorize += delegate(Session session, AttachRequestMessage message)
            {
                return true;
            };

            targetBubble.BubbleConnected += delegate(Session session, Message message)
            {
                targetSession = session;
            };

            targetHub.Startup(false);

            sourceHub.Connect(sourceBubbleGuid,targetBubbleGuid,"127.0.0.1", MxpConstants.DefaultHubPort+1, 100,100,100);

            Thread.Sleep(20);
            targetHub.Process();
            Thread.Sleep(20);
            sourceHub.Process();
            Thread.Sleep(20);
            targetHub.Process();
            Thread.Sleep(20);
            sourceHub.Process();
            Thread.Sleep(20);
            targetHub.Process();
            Thread.Sleep(20);
            sourceHub.Process();
            Thread.Sleep(20);
            targetHub.Process();
            Thread.Sleep(20);
            sourceHub.Process();

            Assert.IsNotNull(sourceSession);
            Assert.IsTrue(sourceSession.IsConnected);

            Assert.IsNotNull(targetSession);
            Assert.IsTrue(targetSession.IsConnected);

            Assert.AreEqual(1, sourceHub.SessionCount);
            Assert.AreEqual(1, targetHub.SessionCount);

            targetHub.Disconnect(targetSession);

            Thread.Sleep(20);
            targetHub.Process();
            Thread.Sleep(20);
            sourceHub.Process();
            Thread.Sleep(20);
            targetHub.Process();
            Thread.Sleep(20);
            sourceHub.Process();
            Thread.Sleep(20);
            targetHub.Process();
            Thread.Sleep(20);
            sourceHub.Process();
            Thread.Sleep(20);
            targetHub.Process();
            Thread.Sleep(20);
            sourceHub.Process();

            Assert.IsFalse(sourceSession.IsConnected);
            Assert.IsFalse(targetSession.IsConnected);

            Assert.AreEqual(0, sourceHub.SessionCount);
            Assert.AreEqual(0, targetHub.SessionCount);

            sourceHub.Shutdown();
            targetHub.Shutdown();
        }


        [Test]
        public void HubConnectionFailure()
        {

            Guid sourceBubbleGuid = new Guid();
            Guid targetBubbleGuid = new Guid();

            MxpHub sourceHub = new MxpHub("http://test.cloud.url", "127.0.0.1", MxpConstants.DefaultHubPort, "TestServerProgram", 1, 2);

            MxpBubble sourceBubble = new MxpBubble(sourceBubbleGuid, "SourceBubbleName", 100, 1000);
            sourceHub.AddBubble(sourceBubble);

            Session sourceSession = null;

            sourceBubble.BubbleConnectFailure += delegate(Session session, Message message)
            {
                sourceSession = session;
            };

            sourceHub.Startup(false);

            MxpHub targetHub = new MxpHub("http://test.cloud.url", "127.0.0.1", MxpConstants.DefaultHubPort + 1, "TestServerProgram", 1, 2);

            MxpBubble targetBubble = new MxpBubble(targetBubbleGuid, "TargetBubbleName", 100, 1000);
            targetHub.AddBubble(targetBubble);

            Session targetSession = null;
            
            targetBubble.BubbleConnectAuthorize += delegate(Session session, AttachRequestMessage message)
            {
                return false;
            };
            
            targetBubble.BubbleConnectFailure += delegate(Session session, Message message)
            {
                targetSession = session;
            };

            targetHub.Startup(false);

            sourceHub.Connect(sourceBubbleGuid, targetBubbleGuid, "127.0.0.1", MxpConstants.DefaultHubPort + 1, 100, 100, 100);

            Thread.Sleep(100);

            targetHub.Process();

            Thread.Sleep(100);

            sourceHub.Process();

            Assert.IsNotNull(sourceSession);
            Assert.IsFalse(sourceSession.IsConnected);

            Assert.IsNotNull(targetSession);
            Assert.IsFalse(targetSession.IsConnected);

            Assert.AreEqual(0, sourceHub.SessionCount);
            Assert.AreEqual(0, targetHub.SessionCount);

            sourceHub.Shutdown();
            targetHub.Shutdown();

        }
    }
}
