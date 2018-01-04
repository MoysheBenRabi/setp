<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="true" CodeBehind="candidate_client_to_reference_server_docs.aspx.cs" Inherits="IOT.candidate_client_to_reference_server_docs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<h1>Help for Candidate Client to Reference Server Tests</h1>
<p>
</p>
<h2>Connection Details</h2>
<table class="grid">
<tr><td>Reference Server Address</td><td>iot.bubblecloud.org</td></tr>
<tr><td>Reference Server Port</td><td>1255</td></tr>
</table>
<h2>Example Message Log From Reference Server</h2>
<p>
The following example log can be acquired by first starting the iot suite test and then executing the iotester against the reference server with the following command:
</p>
<p>
iotester execute ctors iot.bubblecloud.org 1255
</p>
<div class="code">
<pre>
debug: Incoming session constructed: 1 (from 89.27.100.54:52378)
debug: Session 1 Received: JoinRequestMessage {BubbleId=539dfa16-5b52-4c9f-9a09-ad746520873e|AvatarId=123dfa16-5b52-4c9f-9a09-ad7465208321|BubbleName=|LocationName=|ParticipantIdentifier=TestParticipantName|ParticipantSecret=TestParticipantPassphrase|ParticipantRealTime=0|IdentityProviderUrl=http://test.identityprovider|ProgramName=ClientProgram|ProgramMajorVersion=5|ProgramMinorVersion=6|ProtocolMajorVersion=0|ProtocolMinorVersion=5|ProtocolSourceRevision=245|MessageId=35|TypeCode=10|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: AcknowledgeMessage {maxPacketIdCount=63|packetIds=1|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=112|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
info: Participant connected - TestParticipantName to IOT Bubble 1
debug: Session 1 Sent: JoinResponseMessage {RequestMessageId=35|FailureCode=0|BubbleId=539dfa16-5b52-4c9f-9a09-ad746520873e|ParticipantId=72cbf696-18cb-4691-960f-08175da6f492|AvatarId=123dfa16-5b52-4c9f-9a09-ad7465208321|BubbleName=IOT Bubble 1|BubbleAssetCacheUrl=http://test.assetcache.one|BubbleRange=0|BubblePerceptionRange=0|BubbleRealTime=0|ProgramName=Inter Operability Testing|ProgramMajorVersion=0|ProgramMinorVersion=1|ProtocolMajorVersion=0|ProtocolMinorVersion=5|ProtocolSourceRevision=245|MessageId=73|TypeCode=11|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: AcknowledgeMessage {maxPacketIdCount=63|packetIds=2|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=38|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: InjectRequestMessage {ObjectFragment=ObjectFragmet [ObjectId: 123dfa16-5b52-4c9f-9a09-ad7465208321,ParentObjectId: 04ffdc39-9e51-4a6d-8112-e6dd3498dbe9,ObjectIndex: 0,TypeId: 1316899a-b364-470f-bb19-b030a131df4f,ObjectName: TestObjectName,TypeName: TestObjectType,OwnerId: 72cbf696-18cb-4691-960f-08175da6f492,Location: 2,3,4,Velocity: 5,6,7,Acceleration: 8,9,10,Orientation: 11,12,13,14,AngularVelocity: 15,16,17,18,AngularAcceleration: 19,20,21,22,BoundingSphereRadius: 23,Mass: 24,ExtensionDialect: TEDI,ExtensionDialectMajorVersion: 24,ExtensionDialectMinorVersion: 23,ExtensionLength: 60,ExtensionData: 48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57]|MessageId=42|TypeCode=14|Quaranteed=True|FrameCount=2|IsAutoRelease=True}
debug: Session 1 Sent: AcknowledgeMessage {maxPacketIdCount=63|packetIds=3|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=119|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: InjectResponseMessage {RequestMessageId=42|FailureCode=0|MessageId=80|TypeCode=15|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: SynchronizationBeginEventMessage {ObjectCount=1|MessageId=84|TypeCode=70|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: PerceptionEventMessage {ObjectFragment=ObjectFragmet [ObjectId: 123dfa16-5b52-4c9f-9a09-ad7465208321,ParentObjectId: 04ffdc39-9e51-4a6d-8112-e6dd3498dbe9,ObjectIndex: 1,TypeId: 1316899a-b364-470f-bb19-b030a131df4f,ObjectName: TestObjectName,TypeName: TestObjectType,OwnerId: 72cbf696-18cb-4691-960f-08175da6f492,Location: 2,3,4,Velocity: 5,6,7,Acceleration: 8,9,10,Orientation: 11,12,13,14,AngularVelocity: 15,16,17,18,AngularAcceleration: 19,20,21,22,BoundingSphereRadius: 23,Mass: 24,ExtensionDialect: TEDI,ExtensionDialectMajorVersion: 24,ExtensionDialectMinorVersion: 23,ExtensionLength: 60,ExtensionData: 48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57]|MessageId=94|TypeCode=40|Quaranteed=True|FrameCount=2|IsAutoRelease=True}
debug: Session 1 Sent: SynchronizationEndEventMessage {MessageId=86|TypeCode=71|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: AcknowledgeMessage {maxPacketIdCount=63|packetIds=4|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=43|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: AcknowledgeMessage {maxPacketIdCount=63|packetIds=5|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=46|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: ModifyRequestMessage {ObjectFragment=ObjectFragmet [ObjectId: 123dfa16-5b52-4c9f-9a09-ad7465208321,ParentObjectId: 04ffdc39-9e51-4a6d-8112-e6dd3498dbe9,ObjectIndex: 1,TypeId: 1316899a-b364-470f-bb19-b030a131df4f,ObjectName: TestObjectName,TypeName: TestObjectType,OwnerId: 72cbf696-18cb-4691-960f-08175da6f492,Location: 12,13,14,Velocity: 5,6,7,Acceleration: 8,9,10,Orientation: 11,12,13,14,AngularVelocity: 15,16,17,18,AngularAcceleration: 19,20,21,22,BoundingSphereRadius: 23,Mass: 24,ExtensionDialect: TEDI,ExtensionDialectMajorVersion: 24,ExtensionDialectMinorVersion: 23,ExtensionLength: 60,ExtensionData: 48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57]|MessageId=54|TypeCode=16|Quaranteed=True|FrameCount=2|IsAutoRelease=True}
debug: Session 1 Sent: AcknowledgeMessage {maxPacketIdCount=63|packetIds=6|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=127|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: ModifyResponseMessage {RequestMessageId=54|FailureCode=0|MessageId=93|TypeCode=17|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: PerceptionEventMessage {ObjectFragment=ObjectFragmet [ObjectId: 123dfa16-5b52-4c9f-9a09-ad7465208321,ParentObjectId: 04ffdc39-9e51-4a6d-8112-e6dd3498dbe9,ObjectIndex: 1,TypeId: 1316899a-b364-470f-bb19-b030a131df4f,ObjectName: TestObjectName,TypeName: TestObjectType,OwnerId: 72cbf696-18cb-4691-960f-08175da6f492,Location: 12,13,14,Velocity: 5,6,7,Acceleration: 8,9,10,Orientation: 11,12,13,14,AngularVelocity: 15,16,17,18,AngularAcceleration: 19,20,21,22,BoundingSphereRadius: 23,Mass: 24,ExtensionDialect: TEDI,ExtensionDialectMajorVersion: 24,ExtensionDialectMinorVersion: 23,ExtensionLength: 60,ExtensionData: 48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57]|MessageId=124|TypeCode=40|Quaranteed=True|FrameCount=2|IsAutoRelease=True}
debug: Session 1 Received: AcknowledgeMessage {maxPacketIdCount=63|packetIds=7|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=55|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: AcknowledgeMessage {maxPacketIdCount=63|packetIds=8|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=57|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: ExamineRequestMessage {ObjectId=00000000-0000-0000-0000-000000000000|ObjectIndex=1|MessageId=61|TypeCode=22|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: AcknowledgeMessage {maxPacketIdCount=63|packetIds=9|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=133|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: InteractRequestMessage {InteractionFragment=ObjectFragmet [InteractionName: SourceParticipantId: 72cbf696-18cb-4691-960f-08175da6f492SourceObjectId: 00000000-0000-0000-0000-000000000000TargetParticipantId: 00000000-0000-0000-0000-000000000000TargetObjectId: 123dfa16-5b52-4c9f-9a09-ad7465208321,ExtensionDialect: TEDI,ExtensionDialectMajorVersion: 24,ExtensionDialectMinorVersion: 23,ExtensionLength: 60,ExtensionData: 48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57,48,49,50,51,52,53,54,55,56,57]|MessageId=63|TypeCode=20|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: AcknowledgeMessage {maxPacketIdCount=63|packetIds=10|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=135|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: EjectRequestMessage {ObjectId=123dfa16-5b52-4c9f-9a09-ad7465208321|MessageId=65|TypeCode=18|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: AcknowledgeMessage {maxPacketIdCount=63|packetIds=11|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=136|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: DisappearanceEventMessage {ObjectIndex=1|MessageId=108|TypeCode=45|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: EjectResponseMessage {RequestMessageId=65|FailureCode=0|MessageId=109|TypeCode=19|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: AcknowledgeMessage {maxPacketIdCount=63|packetIds=12|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=67|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Received: LeaveRequestMessage {MessageId=72|TypeCode=12|Quaranteed=True|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: AcknowledgeMessage {maxPacketIdCount=63|packetIds=13|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|packetIdCount=1|MessageId=142|TypeCode=1|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
debug: Session 1 Sent: LeaveResponseMessage {RequestMessageId=72|FailureCode=0|MessageId=115|TypeCode=13|Quaranteed=False|FrameCount=1|IsAutoRelease=True}
info: Session destructed: 1 (from 89.27.100.54:52378)
info: Participant disconnected - TestParticipantName to IOT Bubble 1
info: Network thread exited.
info: Network receive thread exited.
info: Network thread exited.
info: Network receive thread exited.
</pre>
</div>
</asp:Content>
