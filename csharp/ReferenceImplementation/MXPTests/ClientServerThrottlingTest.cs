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
    public class ClientServerThrottlingTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void ClientServerThrottling()
        {
            Guid bubbleGuid = Guid.NewGuid();

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

            Thread.Sleep(100);

            server.Process();

            Assert.IsNotNull(serverSession);
            Assert.IsTrue(serverSession.IsConnected);

            Thread.Sleep(100);

            client.Process();

            Thread.Sleep(100);

            Assert.IsTrue(client.IsConnected);

            int loopCount = 100;
            int messagesPerLoop=50;
            double[] transmitterSendRates = new double[2*loopCount];
            double[] transmitterReceiveRates = new double[2*loopCount];
            double[] sessionSendRates = new double[2*loopCount];

            int messagesReceived = 0;
            bubble.ParticipantMessageReceived+=delegate(Session session, Message message)
            {
                messagesReceived++;
            };

            for (int i = 0; i < loopCount; i++)
            {
                for (int j = 0; j < messagesPerLoop; j++)
                {
                    MovementEventMessage originalMessage = new MovementEventMessage();
                    originalMessage.ObjectIndex = 1;
                    originalMessage.Location.X = 1; originalMessage.Location.Y = 2; originalMessage.Location.Z = 3;
                    originalMessage.Orientation.X = 10; originalMessage.Orientation.Y = 11; originalMessage.Orientation.Z = 12; originalMessage.Orientation.W = 13;

                    client.Send(originalMessage);
                }

                client.Process();
                Thread.Sleep(8);
                server.Process();
                Thread.Sleep(8);
                transmitterSendRates[i] = client.SendRate;
                transmitterReceiveRates[i] = server.ReceiveRate;
                sessionSendRates[i] = client.SessionSendRate;
            }

            for (int i = 0; i < loopCount; i++)
            {
                client.Process();
                server.Process();
                Thread.Sleep(16);
                transmitterSendRates[loopCount+i] = client.SendRate;
                transmitterReceiveRates[loopCount+i] = server.ReceiveRate;
                sessionSendRates[loopCount+i] = client.SessionSendRate;
            }

            Assert.IsTrue(loopCount * messagesPerLoop>= messagesReceived);

            server.Shutdown();
        }

        
    }
}
