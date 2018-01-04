using System;
using System.Text;
using System.Collections.Generic;

using MXP;
using System.Threading;
using MXP.Messages;
using NUnit.Framework;

namespace MXPTests
{
    /// <summary>
    /// Summary description for ClientServerTest
    /// </summary>
    [TestFixture]
    public class ClientServerTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ClientServerConnectionSuccess()
        {
            Guid bubbleGuid = new Guid();

            MxpServer server = new MxpServer("http://test.cloud.url", MxpConstants.DefaultHubPort, "TestServerProgram", 1, 2);

            MxpBubble bubble = new MxpBubble(bubbleGuid, "TestBubbleName", 100, 1000);
            server.AddBubble(bubble);

            Session serverSession = null;

            bubble.ParticipantConnectAuthorize += delegate(Session session, JoinRequestMessage message, out Guid participantId, out Guid avatarId
            )
            {
                participantId = Guid.Empty;
                avatarId = Guid.Empty;
                return true;
            };

            bubble.ParticipantConnected += delegate(Session session, JoinRequestMessage message, Guid participantId, Guid avatarId)
            {
                serverSession = session;
            };

            server.Startup(false);

            MxpClient client = new MxpClient("TestClientProgram",1,2);

            client.ServerMessageReceived += delegate(Message message)
            {
            };

            client.Connect("127.0.0.1", MxpConstants.DefaultHubPort, bubbleGuid, "", "TestLocation", "TestIdentityProviderUrl", "TestUserName", "TestUserPassword", Guid.Empty, false);

            Thread.Sleep(20);
            client.Process();
            Thread.Sleep(20);
            server.Process();
            Thread.Sleep(20);
            client.Process();
            Thread.Sleep(20);
            server.Process();
            Thread.Sleep(20);
            client.Process();
            Thread.Sleep(20);
            server.Process();
            Thread.Sleep(20);
            client.Process();
            Thread.Sleep(20);
            server.Process();

            Assert.IsNotNull(serverSession);
            Assert.IsTrue(serverSession.IsConnected);

            Thread.Sleep(20);
            client.Process();
            Thread.Sleep(20);
            server.Process();
            Thread.Sleep(20);
            client.Process();
            Thread.Sleep(20);
            server.Process();

            Assert.IsTrue(client.IsConnected);

            client.Disconnect();

            Thread.Sleep(20);
            server.Process();
            Thread.Sleep(20);
            client.Process();
            Thread.Sleep(20);
            server.Process();
            Thread.Sleep(20);
            client.Process();
            Thread.Sleep(20);
            server.Process();
            Thread.Sleep(20);
            client.Process();

            Assert.IsFalse(client.IsConnected);
            Assert.IsFalse(serverSession.IsConnected);

            server.Shutdown();
        }

        [Test]
        public void ClientServerConnectionFailure()
        {
            Guid bubbleGuid = new Guid();

            MxpServer server = new MxpServer("http://test.cloud.url", MxpConstants.DefaultHubPort, "TestServerProgram", 1, 2);

            MxpBubble bubble = new MxpBubble(Guid.NewGuid(), "TestBubbleName", 100, 1000);
            server.AddBubble(bubble);

            bubble.ParticipantConnectAuthorize += delegate(Session session, JoinRequestMessage message, out Guid participantId, out Guid avatarId)
            {
                participantId = Guid.Empty;
                avatarId = Guid.Empty;
                return false;
            };
            server.Startup(false);

            MxpClient client = new MxpClient("TestClientProgram", 1, 2);

            client.ServerMessageReceived += delegate(Message message)
            {
            };

            client.Connect("127.0.0.1", MxpConstants.DefaultHubPort, bubbleGuid, "", "TestLocation", "TestIdentityProviderUrl", "TestUserName", "TestUserPassword", Guid.Empty, false);

            Thread.Sleep(100);

            server.Process();

            Thread.Sleep(100);

            client.Process();

            Assert.IsFalse(client.IsConnected);

            server.Shutdown();
        }
    }
}
