using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP;
using MXP.Messages;
using System.Threading;

namespace MXPTests
{
    /// <summary>
    /// Summary description for TransmitterTest
    /// </summary>
    [TestFixture]
    public class TransmitterTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        
        [Test]
        public void TransmitterConnectivity()
        {
            Transmitter server = new Transmitter(MxpConstants.DefaultHubPort);

            Transmitter client = new Transmitter();

            JoinRequestMessage originalMessage = new JoinRequestMessage();

            originalMessage.BubbleId = Guid.NewGuid();
            originalMessage.LocationName = "TestLocation";
            originalMessage.IdentityProviderUrl = "IdentityProviderUrl";
            originalMessage.ParticipantIdentifier = "TestParticipantName";
            originalMessage.ParticipantSecret = "TestParticipantPassphrase";
            originalMessage.ParticipantRealTime = 10;

            originalMessage.ProgramName = "TestProgramName";
            originalMessage.ProgramMajorVersion = 1;
            originalMessage.ProgramMinorVersion = 2;
            originalMessage.ProtocolMajorVersion = 3;
            originalMessage.ProtocolMinorVersion = 4;

            Session clientSession = client.OpenSession("127.0.0.1", MxpConstants.DefaultHubPort,originalMessage);

            clientSession.Send(originalMessage);
            client.Send();

            Assert.AreEqual(1, clientSession.GetPacketWaitingAcknowledgeCount());

            Thread.Sleep(100);
            
            server.Receive();

            server.Send();

            Thread.Sleep(100);

            client.Receive();

            client.HandleControlMessages();

            Assert.AreEqual(0, clientSession.GetPacketWaitingAcknowledgeCount());

            Session serverSession = server.AcceptPendingSession();

            Message decodedMessage = serverSession.Receive();

            decodedMessage.IsAutoRelease = false;

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

            server.Shutdown();

            Thread.Sleep(100);
        }

        [Test]
        public void TransmitterMessageRetransmit()
        {
            Transmitter server = new Transmitter(MxpConstants.DefaultHubPort);

            Transmitter client = new Transmitter();

            JoinRequestMessage originalMessage = new JoinRequestMessage();

            originalMessage.BubbleId = Guid.NewGuid();
            originalMessage.LocationName = "TestLocation";
            originalMessage.IdentityProviderUrl = "IdentityProviderUrl";
            originalMessage.ParticipantIdentifier = "TestParticipantName";
            originalMessage.ParticipantSecret = "TestParticipantPassphrase";
            originalMessage.ParticipantRealTime = 10;

            originalMessage.ProgramName = "TestProgramName";
            originalMessage.ProgramMajorVersion = 1;
            originalMessage.ProgramMinorVersion = 2;
            originalMessage.ProtocolMajorVersion = 3;
            originalMessage.ProtocolMinorVersion = 4;

            Session clientSession = client.OpenSession("127.0.0.1", MxpConstants.DefaultHubPort, originalMessage);

            clientSession.Send(originalMessage);
            client.Send();

            Assert.AreEqual(1, clientSession.GetPacketWaitingAcknowledgeCount());

            Thread.Sleep(100);

            server.Receive();

            server.Send();

            Thread.Sleep(100);

            client.Receive();

            client.HandleControlMessages();

            Assert.AreEqual(0, clientSession.GetPacketWaitingAcknowledgeCount());

            Session serverSession = server.AcceptPendingSession();

            Message decodedMessage = serverSession.Receive();


            serverSession.Send(decodedMessage);

            server.Send();

            Thread.Sleep(100);

            client.Receive();

            Message decodedMessage2 = clientSession.Receive();

            decodedMessage.IsAutoRelease = false;
            decodedMessage2.IsAutoRelease = false;

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedMessage.ToString();
            String decodedMessage2String = decodedMessage2.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);
            Assert.AreEqual(originalMessageString, decodedMessage2String);

            server.Shutdown();

            Thread.Sleep(100);
        }

        /*
        [Test]
        public void TransmitterLongMessagetTransmit()
        {
            Transmitter server = new Transmitter(MxpConstants.DefaultHubPort);

            Transmitter client = new Transmitter();

            JoinRequestMessage joinRequestMessage = new JoinRequestMessage();

            joinRequestMessage.BubbleId = Guid.NewGuid();
            joinRequestMessage.LocationName = "TestLocation";
            joinRequestMessage.IdentityProviderUrl = "IdentityProviderUrl";
            joinRequestMessage.ParticipantIdentifier = "TestParticipantName";
            joinRequestMessage.ParticipantSecret = "TestParticipantPassphrase";
            joinRequestMessage.ParticipantRealTime = 10;

            joinRequestMessage.ProgramName = "TestProgramName";
            joinRequestMessage.ProgramMajorVersion = 1;
            joinRequestMessage.ProgramMinorVersion = 2;
            joinRequestMessage.ProtocolMajorVersion = 3;
            joinRequestMessage.ProtocolMinorVersion = 4;

            Session clientSession = client.OpenSession("127.0.0.1", MxpConstants.DefaultHubPort, joinRequestMessage);

            clientSession.Send(joinRequestMessage);
            client.Send();

            Assert.AreEqual(1, clientSession.GetPacketWaitingAcknowledgeCount());

            Thread.Sleep(100);

            server.Receive();

            server.Send();

            Thread.Sleep(100);

            client.Receive();

            client.HandleControlMessages();

            Assert.AreEqual(0, clientSession.GetPacketWaitingAcknowledgeCount());

            Session serverSession = server.AcceptPendingSession();

            Message joinMessage = serverSession.Receive();
            Assert.IsNotNull(joinMessage);

            InjectRequestMessage originalMessage = new InjectRequestMessage();

            originalMessage.ObjectFragment.ObjectId = Guid.NewGuid();
            originalMessage.ObjectFragment.ObjectIndex = 1;
            originalMessage.ObjectFragment.TypeId = Guid.NewGuid();
            originalMessage.ObjectFragment.ObjectName = "TestObjectName";
            originalMessage.ObjectFragment.TypeName = "TestTypeName";
            originalMessage.ObjectFragment.OwnerId = Guid.NewGuid();
            originalMessage.ObjectFragment.Location.X = 1; originalMessage.ObjectFragment.Location.Y = 2; originalMessage.ObjectFragment.Location.Z = 3;
            originalMessage.ObjectFragment.Velocity.X = 4; originalMessage.ObjectFragment.Velocity.Y = 5; originalMessage.ObjectFragment.Velocity.Z = 6;
            originalMessage.ObjectFragment.Acceleration.X = 7; originalMessage.ObjectFragment.Acceleration.Y = 8; originalMessage.ObjectFragment.Acceleration.Z = 9;
            originalMessage.ObjectFragment.Orientation.X = 10; originalMessage.ObjectFragment.Orientation.Y = 11; originalMessage.ObjectFragment.Orientation.Z = 12; originalMessage.ObjectFragment.Orientation.W = 13;
            originalMessage.ObjectFragment.AngularVelocity.X = 14; originalMessage.ObjectFragment.AngularVelocity.Y = 15; originalMessage.ObjectFragment.AngularVelocity.Z = 16; originalMessage.ObjectFragment.AngularVelocity.W = 17;
            originalMessage.ObjectFragment.AngularAcceleration.X = 18; originalMessage.ObjectFragment.AngularAcceleration.Y = 19; originalMessage.ObjectFragment.AngularAcceleration.Z = 20; originalMessage.ObjectFragment.AngularAcceleration.W = 21;
            originalMessage.ObjectFragment.BoundingSphereRadius = 22;
            originalMessage.ObjectFragment.Mass = 23;
            originalMessage.ObjectFragment.ExtensionDialect = "TEST";
            originalMessage.ObjectFragment.SetExtensionData(UTF8Encoding.UTF8.GetBytes(
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"));


            clientSession.Send(originalMessage);

            server.Process();
            client.Process();
            Thread.Sleep(100);


            Assert.AreEqual(0, clientSession.GetPacketWaitingAcknowledgeCount());

            Message decodedInjectMessage = serverSession.Receive();

            decodedInjectMessage.IsAutoRelease = false;

            String originalMessageString = originalMessage.ToString();
            String decodedMessageString = decodedInjectMessage.ToString();
            Assert.AreEqual(originalMessageString, decodedMessageString);

            client.Shutdown();
            server.Shutdown();


            Thread.Sleep(100);
        }*/

    }
}
