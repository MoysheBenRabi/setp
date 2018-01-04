using System;
using System.Collections.Generic;
using System.Text;
using MXP.Messages;
using MXP;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;
using MXP.Fragments;

namespace IOT.Encoding
{
    /// <summary>
    /// Implements message serialization for IOT suite.
    /// </summary>
    public class ReferenceMessageLoader
    {
        private static object ThreadLock = new object();

        private static ReferenceMessageLoader current=null;
        public static ReferenceMessageLoader Current
        {
            get
            {
                lock (ThreadLock)
                {
                    if (current == null)
                    {
                        current = new ReferenceMessageLoader();
                    }
                    return current;
                }
            }
        }

        private IDictionary<string, ReferenceMessage> referenceMessages = new Dictionary<string, ReferenceMessage>();
        public IDictionary<string, ReferenceMessage> ReferenceMessages
        {
            get
            {
                return referenceMessages;
            }
        }

        private const int UdpClientPort = 9263;
        private UdpClient udpClient;

        private ReferenceMessageLoader()
        {
            LoadReferenceMessages();
        }

        private void LoadReferenceMessages()
        {
            udpClient = new UdpClient(UdpClientPort);

            // Control messages
            {
                AcknowledgeMessage originalMessage = new AcknowledgeMessage();
                originalMessage.MessageId = 1;
                originalMessage.AddPacketId(1);
                originalMessage.AddPacketId(2);
                originalMessage.AddPacketId(3);
                originalMessage.AddPacketId(4);
                originalMessage.AddPacketId(5);
                SerializeReferenceMessage(originalMessage);
            }
            {
                AttachRequestMessage originalMessage = new AttachRequestMessage();
                originalMessage.MessageId = 1;
                originalMessage.TargetBubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.SourceBubbleFragment.BubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                SerializeReferenceMessage(originalMessage);
            }
            {
                AttachResponseMessage originalMessage = new AttachResponseMessage();
                originalMessage.MessageId = 1;
                originalMessage.RequestMessageId = 1;
                originalMessage.FailureCode = 2;
                originalMessage.TargetBubbleFragment.BubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.TargetBubbleFragment.BubbleName = "TestBubble1";
                originalMessage.TargetBubbleFragment.BubbleAssetCacheUrl = "TestCloudUrl";
                originalMessage.TargetBubbleFragment.BubbleAddress = "TestBubbleAddress";
                originalMessage.TargetBubbleFragment.BubblePort = 1;
                originalMessage.TargetBubbleFragment.BubbleCenter.X = 2;
                originalMessage.TargetBubbleFragment.BubbleCenter.Y = 3;
                originalMessage.TargetBubbleFragment.BubbleCenter.Z = 4;
                originalMessage.TargetBubbleFragment.BubbleRange = 5;
                originalMessage.TargetBubbleFragment.BubblePerceptionRange = 6;
                originalMessage.TargetBubbleFragment.BubbleRealTime = 7;
                originalMessage.ProgramName = "TestProgramName";
                originalMessage.ProgramMajorVersion = 1;
                originalMessage.ProgramMinorVersion = 2;
                originalMessage.ProtocolMajorVersion = 3;
                originalMessage.ProtocolMinorVersion = 4;
                originalMessage.ProtocolSourceRevision = 5;
                SerializeReferenceMessage(originalMessage);
            }
            {
                ChallengeRequestMessage originalMessage = new ChallengeRequestMessage();
                originalMessage.MessageId = 1;
                for (int i = 0; i < originalMessage.ChallengeRequestBytes.Length; i++)
                {
                    originalMessage.ChallengeRequestBytes[i] = (byte)i;
                }
                SerializeReferenceMessage(originalMessage);
            }
            {
                ChallengeResponseMessage originalMessage = new ChallengeResponseMessage();
                originalMessage.MessageId = 1;
                for (int i = 0; i < originalMessage.ChallengeResponseBytes.Length; i++)
                {
                    originalMessage.ChallengeResponseBytes[i] = (byte)i;
                }
                SerializeReferenceMessage(originalMessage);
            }
            {
                DetachRequestMessage originalMessage = new DetachRequestMessage();
                originalMessage.MessageId = 1;
                SerializeReferenceMessage(originalMessage);
            }
            {
                DetachResponseMessage originalMessage = new DetachResponseMessage();
                originalMessage.MessageId = 1;
                originalMessage.RequestMessageId = 1;
                originalMessage.FailureCode = 2;
                SerializeReferenceMessage(originalMessage);
            }
            {
                JoinRequestMessage originalMessage = new JoinRequestMessage();
                originalMessage.MessageId = 1;
                originalMessage.BubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.BubbleName = "TestBubbleName";
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
                originalMessage.ProtocolSourceRevision = 5;
                SerializeReferenceMessage(originalMessage);
            }
            {
                JoinResponseMessage originalMessage = new JoinResponseMessage();
                originalMessage.MessageId = 1;
                originalMessage.RequestMessageId = 1;
                originalMessage.FailureCode = 2;
                originalMessage.BubbleId = Guid.Empty;
                originalMessage.ParticipantId = Guid.Empty;
                originalMessage.AvatarId = Guid.Empty;
                originalMessage.BubbleName = "TestBubbleName";
                originalMessage.BubbleAssetCacheUrl = "TestBubbleAssetCacheUrl";
                originalMessage.BubbleRange = 3;
                originalMessage.BubblePerceptionRange = 4;
                originalMessage.BubbleRealTime = 5;
                originalMessage.ProgramName = "TestProgramName";
                originalMessage.ProgramMajorVersion = 6;
                originalMessage.ProgramMinorVersion = 7;
                originalMessage.ProtocolMajorVersion = 8;
                originalMessage.ProtocolMinorVersion = 9;
                originalMessage.ProtocolSourceRevision = 10;
                SerializeReferenceMessage(originalMessage);
            }
            {
                KeepaliveMessage originalMessage = new KeepaliveMessage();
                originalMessage.MessageId = 1;
                SerializeReferenceMessage(originalMessage);
            }
            {
                LeaveRequestMessage originalMessage = new LeaveRequestMessage();
                originalMessage.MessageId = 1;
                SerializeReferenceMessage(originalMessage);
            }
            {
                LeaveResponseMessage originalMessage = new LeaveResponseMessage();
                originalMessage.MessageId = 1;
                originalMessage.RequestMessageId = 1;
                originalMessage.FailureCode = 2;
                SerializeReferenceMessage(originalMessage);
            }
            {
                ListBubblesRequest originalMessage = new ListBubblesRequest();
                originalMessage.MessageId = 1;
                originalMessage.ListType = ListBubblesRequest.ListTypeHosted;
                SerializeReferenceMessage(originalMessage);
            }
            {
                ListBubblesResponse originalMessage = new ListBubblesResponse();
                originalMessage.MessageId = 1;
                {
                    BubbleFragment bubbleEntry = new BubbleFragment();
                    bubbleEntry.BubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                    bubbleEntry.BubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                    bubbleEntry.BubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                    bubbleEntry.BubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                    bubbleEntry.BubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                SerializeReferenceMessage(originalMessage);
            }
            {
                ThrottleMessage originalMessage = new ThrottleMessage();
                originalMessage.MessageId = 1;
                originalMessage.BytesPerSecond = 10000;
                SerializeReferenceMessage(originalMessage);
            }

            // Command messages
            {
                EjectRequestMessage originalMessage = new EjectRequestMessage();
                originalMessage.MessageId = 1;
                originalMessage.ObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                SerializeReferenceMessage(originalMessage);
            }
            {
                EjectResponseMessage originalMessage = new EjectResponseMessage();
                originalMessage.MessageId = 1;
                originalMessage.RequestMessageId = 1;
                originalMessage.FailureCode = 2;
                SerializeReferenceMessage(originalMessage);
            }
            {
                ExamineRequestMessage originalMessage = new ExamineRequestMessage();
                originalMessage.MessageId = 1;
                originalMessage.ObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectIndex = 1;
                SerializeReferenceMessage(originalMessage);
            }
            {
                ExamineResponseMessage originalMessage = new ExamineResponseMessage();
                originalMessage.MessageId = 1;
                originalMessage.RequestMessageId = 1;
                originalMessage.FailureCode = 2;
                originalMessage.ObjectFragment.ObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectIndex = 1;
                originalMessage.ObjectFragment.TypeId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectName = "TestObjectName";
                originalMessage.ObjectFragment.TypeName = "TestTypeName";
                originalMessage.ObjectFragment.OwnerId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                    "1234567890123456789012345678901234567890"));
                SerializeReferenceMessage(originalMessage);
            }
            {
                HandoverRequestMessage originalMessage = new HandoverRequestMessage();
                originalMessage.MessageId = 1;
                originalMessage.SourceBubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.TargetBubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectIndex = 1;
                originalMessage.ObjectFragment.TypeId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectName = "TestObjectName";
                originalMessage.ObjectFragment.TypeName = "TestTypeName";
                originalMessage.ObjectFragment.OwnerId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                    "1234567890123"));
                SerializeReferenceMessage(originalMessage);
            }
            {
                HandoverResponseMessage originalMessage = new HandoverResponseMessage();
                originalMessage.MessageId = 1;
                originalMessage.RequestMessageId = 1;
                originalMessage.FailureCode = 2;
                SerializeReferenceMessage(originalMessage);
            }
            {
                InjectRequestMessage originalMessage = new InjectRequestMessage();
                originalMessage.MessageId = 1;
                originalMessage.ObjectFragment.ObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectIndex = 1;
                originalMessage.ObjectFragment.TypeId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectName = "TestObjectName";
                originalMessage.ObjectFragment.TypeName = "TestTypeName";
                originalMessage.ObjectFragment.OwnerId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.Location.X = 1; originalMessage.ObjectFragment.Location.Y = 2; originalMessage.ObjectFragment.Location.Z = 3;
                originalMessage.ObjectFragment.Velocity.X = 4; originalMessage.ObjectFragment.Velocity.Y = 5; originalMessage.ObjectFragment.Velocity.Z = 6;
                originalMessage.ObjectFragment.Acceleration.X = 7; originalMessage.ObjectFragment.Acceleration.Y = 8; originalMessage.ObjectFragment.Acceleration.Z = 9;
                originalMessage.ObjectFragment.Orientation.X = 10; originalMessage.ObjectFragment.Orientation.Y = 11; originalMessage.ObjectFragment.Orientation.Z = 12; originalMessage.ObjectFragment.Orientation.W = 13;
                originalMessage.ObjectFragment.AngularVelocity.X = 14; originalMessage.ObjectFragment.AngularVelocity.Y = 15; originalMessage.ObjectFragment.AngularVelocity.Z = 16; originalMessage.ObjectFragment.AngularVelocity.W = 17;
                originalMessage.ObjectFragment.AngularAcceleration.X = 18; originalMessage.ObjectFragment.AngularAcceleration.Y = 19; originalMessage.ObjectFragment.AngularAcceleration.Z = 20; originalMessage.ObjectFragment.AngularAcceleration.W = 21;
                originalMessage.ObjectFragment.BoundingSphereRadius = 22;
                originalMessage.ObjectFragment.Mass = 23;
                originalMessage.ObjectFragment.ExtensionDialect = "TEST";
                originalMessage.ObjectFragment.ExtensionDialectMajorVersion = 24;
                originalMessage.ObjectFragment.ExtensionDialectMinorVersion = 25;
                originalMessage.ObjectFragment.SetExtensionData(UTF8Encoding.UTF8.GetBytes(
                    "123456789012345678901234567890123456789012345"));
                SerializeReferenceMessage(originalMessage);
            }
            {
                InjectResponseMessage originalMessage = new InjectResponseMessage();
                originalMessage.MessageId = 1;
                originalMessage.RequestMessageId = 1;
                originalMessage.FailureCode = 2;
                SerializeReferenceMessage(originalMessage);
            }
            {
                InteractRequestMessage originalMessage = new InteractRequestMessage();
                originalMessage.MessageId = 1;
                originalMessage.InteractionFragment.InteractionName = "TestInteractionName";
                originalMessage.InteractionFragment.SourceParticipantId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.InteractionFragment.SourceObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.InteractionFragment.TargetParticipantId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.InteractionFragment.TargetObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.InteractionFragment.ExtensionDialect = "TEST";
                originalMessage.SetInteractionPayloadData(UTF8Encoding.UTF8.GetBytes(
                    "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                    "1234567890123456789012345678901234567890123456789012345678901"));
                SerializeReferenceMessage(originalMessage);
            }
            {
                InteractResponseMessage originalMessage = new InteractResponseMessage();
                originalMessage.MessageId = 1;
                originalMessage.RequestMessageId = 1;
                originalMessage.FailureCode = 2;
                originalMessage.InteractionFragment.InteractionName = "TestInteractionName";
                originalMessage.InteractionFragment.SourceParticipantId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.InteractionFragment.SourceObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.InteractionFragment.TargetParticipantId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.InteractionFragment.TargetObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.InteractionFragment.ExtensionDialect = "TEST";
                originalMessage.SetInteractionPayloadData(UTF8Encoding.UTF8.GetBytes(
                    "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                    "12345678901234567890123456789012345678901234567890123456"));
                SerializeReferenceMessage(originalMessage);
            }
            {
                ModifyRequestMessage originalMessage = new ModifyRequestMessage();
                originalMessage.MessageId = 1;
                originalMessage.ObjectFragment.ObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectIndex = 1;
                originalMessage.ObjectFragment.TypeId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectName = "TestObjectName";
                originalMessage.ObjectFragment.TypeName = "TestTypeName";
                originalMessage.ObjectFragment.OwnerId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                    "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                    "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678"));
                SerializeReferenceMessage(originalMessage);
            }
            {
                ModifyResponseMessage originalMessage = new ModifyResponseMessage();
                originalMessage.MessageId = 1;
                originalMessage.RequestMessageId = 1;
                originalMessage.FailureCode = 2;
                SerializeReferenceMessage(originalMessage);
            }

            // Event messages
            {
                ActionEventMessage originalMessage = new ActionEventMessage();
                originalMessage.MessageId = 1;
                originalMessage.ActionFragment.ActionName = "TestInteractionName";
                originalMessage.ActionFragment.SourceObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ActionFragment.ObservationRadius = 100;
                originalMessage.ActionFragment.ExtensionDialect = "TEST";
                originalMessage.ActionFragment.ExtensionDialectMajorVersion = 1;
                originalMessage.ActionFragment.ExtensionDialectMinorVersion = 2;
                originalMessage.SetPayloadData(UTF8Encoding.UTF8.GetBytes(
                    "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                    "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                    "12345"));
                SerializeReferenceMessage(originalMessage);
            }
            {
                DisappearanceEventMessage originalMessage = new DisappearanceEventMessage();
                originalMessage.MessageId = 1;
                originalMessage.ObjectIndex = 1;
                SerializeReferenceMessage(originalMessage);
            }
            {
                HandoverEventMessage originalMessage = new HandoverEventMessage();
                originalMessage.MessageId = 1;
                originalMessage.SourceBubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.TargetBubbleId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectIndex = 1;
                originalMessage.ObjectFragment.TypeId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectName = "TestObjectName";
                originalMessage.ObjectFragment.TypeName = "TestTypeName";
                originalMessage.ObjectFragment.OwnerId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                    "1234567890123"));
                SerializeReferenceMessage(originalMessage);
            }
            {
                MovementEventMessage originalMessage = new MovementEventMessage();
                originalMessage.MessageId = 1;
                originalMessage.ObjectIndex = 1;
                originalMessage.Location.X = 1; originalMessage.Location.Y = 2; originalMessage.Location.Z = 3;
                originalMessage.Orientation.X = 10; originalMessage.Orientation.Y = 11; originalMessage.Orientation.Z = 12; originalMessage.Orientation.W = 13;

                SerializeReferenceMessage(originalMessage);
            }
            {
                PerceptionEventMessage originalMessage = new PerceptionEventMessage();
                originalMessage.MessageId = 1;
                originalMessage.ObjectFragment.ObjectId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectIndex = 1;
                originalMessage.ObjectFragment.TypeId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
                originalMessage.ObjectFragment.ObjectName = "TestObjectName";
                originalMessage.ObjectFragment.TypeName = "TestTypeName";
                originalMessage.ObjectFragment.OwnerId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
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
                    "123456789012345678901234567890123456789012345"));
                SerializeReferenceMessage(originalMessage);
            }
            {
                SynchronizationBeginEventMessage originalMessage = new SynchronizationBeginEventMessage();
                originalMessage.MessageId = 1;
                originalMessage.ObjectCount = 1;
                SerializeReferenceMessage(originalMessage);
            }
            {
                SynchronizationEndEventMessage originalMessage = new SynchronizationEndEventMessage();
                originalMessage.MessageId = 1;
                SerializeReferenceMessage(originalMessage);
            }

            udpClient.Close();
            udpClient = null;
        }

        private void SerializeReferenceMessage(Message message)
        {
            ReferenceMessage item = new ReferenceMessage();
            item.MessageValue = message;
            item.MessageName = message.GetType().Name;
            if (item.MessageName.EndsWith("Message"))
            {
                item.MessageName = item.MessageName.Substring(0, item.MessageName.Length - 7);
            }
            item.MessageFileName = item.MessageName.ToLower();
            item.MessageFileName += ".dat";
            item.StringValue = message.ToString();
            item.ByteValue = MessageToPacketBytes(message);
            referenceMessages.Add(item.MessageFileName, item);
        }

        private byte[] MessageToPacketBytes(Message message)
        {
            Transmitter transmitter = new Transmitter();
            transmitter.DefaultPacketFirstSentTime = new DateTime(2009, 11, 5, 15, 33, 25);
            Session session = transmitter.OpenSession("127.0.0.1", UdpClientPort, null);
            session.Send(message);
            transmitter.Send();

            Thread.Sleep(100);

            byte[] receivedBytes;
            IPEndPoint remoteEndPoint = null;
            MemoryStream stream = new MemoryStream(10);
            while (udpClient.Available > 0)
            {
                receivedBytes = udpClient.Receive(ref remoteEndPoint);
                stream.Write(receivedBytes,0,receivedBytes.Length);
            }
            byte[] sourceBytes = stream.GetBuffer();
            byte[] targetBytes = new byte[stream.Length];

            for (int i = 0; i < targetBytes.Length; i++)
            {
                targetBytes[i] = sourceBytes[i];
            }

            return targetBytes;
        }

    }
}
