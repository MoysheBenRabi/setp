using System;
using System.Collections.Generic;

using System.Text;
using MXP.Messages;
using System.Net;
using System.Diagnostics;
using MXP.Util;

namespace MXP
{

	/// <summary>
	/// Value object class containing counter of frames encoded or decoded and reference to message being encoded or decoded.
	/// This value object is used to by encoding and decoding process. Allows same message be sent to multiple recipients
	/// as it is immutable in relation to encoding process. In other words does not change during the encoding process
	/// as the frame counter is stored outside the message object. 
	/// </summary>
    public class MessageEntry
    {
		/// <summary>
		/// Count of frames encoded or decoded. 
		/// </summary>
        public ushort FramesCompleted = 0;
		/// <summary>
		/// Message which is being encoded or decoded. 
		/// </summary>
        public Message Message;
    }

    /// <summary>
    /// Session stores the state information of protocol session between the two peers.
    /// Session contains: state (connecting, connected, disconnected), message queues, 
    /// quaranteed packets waiting acks, acks to be sent, partially sent and received messages.
    /// </summary>
    public class Session
    {
		#region Fields		
		/// <value>
		/// Id of this session in outbound packets. 
		/// </value>
        public uint OutgoingSessionId = 0;
		/// <value>
		/// Id of this session in inbound packets. 
		/// </value>
        public uint IncomingSessionId = 0;
		/// <value>
		/// Connection state of the sessions.
		/// </value>
        private SessionState sessionState = SessionState.Connecting;
		/// <value>
		/// Remote IP end point of this session. 
		/// </value>
        public IPEndPoint RemoteEndPoint;
		/// <value>
		/// Set of received packet ids which is used to invalidate duplicate packets.
		/// TODO Is it likely that id counter overflows. If that happens all received packets are ignored as duplicates.
		/// TODO This is also memory leak. Replace with container which has limited size and oldest entry is removed first when filled. (Cache)
		/// </value>
        public HashSet<uint> ReceivedPackets = new HashSet<uint>();

		/// <value>
		/// Used by sessions which are connected to bubble.
		/// TODO Refactor this out of session class.
		/// </value>
        public MxpBubble Bubble;

		/// <value>
		/// Used by outbound session to tie the session id of response ack to correct session. 
		/// </value>
        public uint FirstPacketId = 0;
		/// <value>
		/// Number of messages sent. 
		/// </value>
        public int MessagesSent = 0;
		/// <value>
		/// Number of messages received. 
		/// </value>
        public int MessagesReceived = 0;

		/// <value>
		/// Number of bytes sent- 
		/// </value>
        public double BytesSent = 0;
		/// <value>
		/// Send rate measured in bytes per second. 
		/// </value>
        public double SendRate = 0; 
		/// <value>
		/// Last send rate update time. 
		/// </value>
        public DateTime SendRateUpdateTime = DateTime.Now;
		/// <value>
		/// Time window which is used to measure the average send rate. 
		/// </value>
        public double SendRateTimeWindow = 0.2;

		/// <value>
		/// True if session is incoming. 
		/// </value>
        public bool IsIncoming = false;
		/// <value>
		/// If true then messages are writen debug log.
		/// </value>
        public bool DebugMessages = false;
		/// <value>
		/// True when first ack has been sent. This field is used to 
		/// avoid sending anything else to the upstream before connection opening message
		/// has been acked. 
		/// </value>
        public bool IsFirstAckSent = false;

		/// <value>
		/// Time of creation of the session. 
		/// </value>
        public DateTime CreationTime = DateTime.Now;
		/// <value>
		/// Time when connection was established.
		/// </value>
        public DateTime ConnectTime = DateTime.MinValue;
		/// <value>
		/// Time when session was disconnected. 
		/// </value>
        public DateTime DisconnectTime = DateTime.MinValue;
		/// <value>
		/// Time when last message frame was encoded to packet.
		/// </value>
        public DateTime LastSendTime = DateTime.Now;
		/// <value>
		/// Time when last frame was decoded to message.
		/// </value>
        public DateTime LastReceiveTime = DateTime.Now;

		/// <value>
		/// Queue of received messages to be processed. 
		/// </value>
        private Queue<Message> inboundMessages = new Queue<Message>();
		/// <value>
		/// Queue of messages to be sent. 
		/// </value>
        private Queue<Message> outboundMessages = new Queue<Message>();
		/// <value>
		/// Queue of received control messages to be processed. 
		/// </value>
        private Queue<Message> inboundControlMessages = new Queue<Message>();
		/// <value>
		/// Queue of acknowledge messages to be sent.
		/// TODO Could this be more generally outboundControlMessages? 
		/// </value>
        private List<AcknowledgeMessage> outboundAcknowledgeMessages = new List<AcknowledgeMessage>();
		/// <value>
		/// Packets which are waiting acknowledge. If these guaranteed packets are not acked in time
		/// they will be resent until resend count is full. 
		/// </value>
        private IDictionary<uint, Packet> packetsWaitingAcknowledge = new Dictionary<uint, Packet>();
		/// <value>
		/// Messaged which have been partially received.
		/// </value>
        private IDictionary<uint, MessageEntry> partialInboundMessages = new Dictionary<uint, MessageEntry>();
		/// <value>
		/// Messages which are currently being multiplexed to the outbound UDP packets.
		/// </value>
        private LinkedList<MessageEntry> partialOutboundMessages = new LinkedList<MessageEntry>();
		/// <value>
		/// Message which is next in turn to have a frame encoded to packet.
		/// </value>
        private LinkedListNode<MessageEntry> currentPartialOutboundMessage;

		#endregion

		#region Properties

		/// <value>
        /// Connection state of the sessions.
        /// </value>
		public SessionState SessionState
        {
            get
            {
                return sessionState;
            }
        }
		/// <value>
		/// True if session is connected.
		/// </value>
        public bool IsConnected
        {
            get
            {
                return sessionState == SessionState.Connected;
            }
        }		
		
		#endregion
		
        #region SessionState Methods
		/// <summary>
		/// Sets the session state to connected and updates connect time.
		/// TODO this can be done in SessionState property setter. 
		/// </summary>
        public void SetStateConnected()
        {
            if (sessionState == SessionState.Connecting)
            {
                sessionState = SessionState.Connected;
                ConnectTime = DateTime.Now;
            }
            else
            {
                throw new Exception("Session can only change state to connected from connecting: " + sessionState);
            }
        }
		/// <summary>
		/// Sets the session state to disconnected.
		/// TODO this can be done in SessionState property setter. 
		/// </summary>
        public void SetStateDisconnected()
        {
            if (sessionState == SessionState.Connecting||sessionState == SessionState.Connected)
            {
                sessionState = SessionState.Disconnected;
                DisconnectTime = DateTime.Now;
            }
            /*else
            {
                throw new Exception("Session can only change state to disconnected from connecting or connected.");
            }*/
        }

        #endregion

        #region Send Messages Methods

		/// <summary>
		/// Queues message for sending. 
		/// </summary>
		/// <param name="message">
		/// A message to be queued for sending.<see cref="Message"/>
		/// </param>
        public void Send(Message message)
        {
            // Prepares message to be encoded.
            message.PrepareEncoding();

            if (message.FrameCount == 0)
            {
				// TODO is this really valid?
                throw new Exception("Message frame count is zero possible reusing incoming message: "+message);
            }

            lock (outboundMessages)
            {
                outboundMessages.Enqueue(message);
            }
        }

		/// <summary>
		/// Gets the number of messages in the outbound queue. Does not count partially sent messages. 
		/// TODO: Change to property in accordance with the inbound message count property. 
		/// </summary>
		/// <returns>
		/// Number of the outbound messages.<see cref="System.Int32"/>
		/// </returns>
        public int GetOutboundMessageCount()
        {
            lock (outboundMessages)
            {
                return outboundMessages.Count;
            }
        }

		/// <summary>
		/// Drops give number of unquaranteed messages. Used in congestion situation to remove unimportant messages from output message queue.
		/// </summary>
		/// <param name="messagesToDequeue">
		/// Number of messages to drop.<see cref="System.Int32"/>
		/// </param>
        public void DropUnquaranteedOutboundMessages(int messagesToDequeue)
        {
            // Dequeue given amount of messages.
            for(int i=0;i<messagesToDequeue;i++)
            {
                Message message = null;
                lock (outboundMessages)
                {
                    if (outboundMessages.Count == 0)
                    {
                        break;
                    }
                    message = outboundMessages.Dequeue();
                }

                // Add message to partialOutboundMessages if it is quaranteed and otherwise drop
                if (message.Quaranteed)
                {
                    lock (partialOutboundMessages)
                    {
                        MessageEntry entry = new MessageEntry();
                        entry.Message = message;
                        partialOutboundMessages.AddLast(entry);
                    }
                }
            }
        }

        #endregion

        #region Receive Messages Methods
		/// <value>
		/// Count of available messages for processing.
		/// </value>
        public int AvailableMessages
        {
            get
            {
                lock (inboundMessages)
                {
                    return (!IsIncoming)||IsFirstAckSent?inboundMessages.Count:0; // For incoming sessions pass messages out only after first ack has been sent.
                }
            }
        }

		/// <summary>
		/// Pops a message from the received messages queue. 
		/// </summary>
		/// <returns>
		/// A received message.<see cref="Message"/>
		/// </returns>
        public Message Receive()
        {
            lock (inboundMessages)
            {
                if (AvailableMessages > 0)
                {
                    return inboundMessages.Dequeue();
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Control Messages Methods

		/// <value>
		/// Number of received control messages. 
		/// </value>
        public int AvailableControlMessages
        {
            get
            {
                lock (inboundControlMessages)
                {
                    return inboundControlMessages.Count;
                }
            }
        }

		/// <summary>
		/// Pops a control message from received control message queue.
		/// TODO: Align the method name with that of Receive or the other way around. 
		/// </summary>
		/// <returns>
		/// A control message which has been received.<see cref="Message"/>
		/// </returns>
        public Message PopControlMessage()
        {
            lock (inboundControlMessages)
            {
                if (inboundControlMessages.Count > 0)
                {
                    return inboundControlMessages.Dequeue();
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Partially Received Messages Methods

		/// <summary>
		/// Number of partially received messages.
		/// TODO: Change to property 
		/// </summary>
		/// <returns>
		/// Number of partially received messages.<see cref="System.Int32"/>
		/// </returns>
        public int GetPartiallyReceivedMessageCount()
        {
            lock (partialInboundMessages)
            {
                return partialInboundMessages.Count;
            }
        }

		/// <summary>
		/// Finds or constructs message for the given frame. 
		/// </summary>
		/// <param name="id">
		/// Id of the message.<see cref="System.UInt32"/>
		/// </param>
        /// <param name="typeCode">
		/// Type code of the message.<see cref="System.Byte"/>
		/// </param>
		/// <param name="frameCount">
		/// Frame count of the message. Used when message is constructed.<see cref="System.UInt16"/>
		/// </param>
		/// <param name="frameIndex">
		/// Index of the frame which has been received and will be decoded to message.<see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// MessageEntry containing the requested message. <see cref="MessageEntry"/>
		/// </returns>
        public MessageEntry GetPartialInboundMessage(uint id,byte typeCode, ushort frameCount,int frameIndex)
        {
            lock (partialInboundMessages)
            {
                LastReceiveTime = DateTime.Now;
                if (partialInboundMessages.ContainsKey(id))
                {
                    return partialInboundMessages[id];
                }
                else
                {
                    if (frameIndex > 0)
                    {
                        // TODO Figure out how to handle frames which arrive before message initialization frame.
                        // we need to receive the first frame first to properly initialize reading rest of the frames.
                        return null;
                    }

                    Message message = MessageFactory.Current.ReserveMessage(typeCode);
                    message.MessageId = id;
                    message.FrameCount = frameCount;
                    MessageEntry entry = new MessageEntry();
                    entry.Message = message;
                    partialInboundMessages.Add(message.MessageId, entry);
                    return entry;
                }
            }
        }

		/// <summary>
		/// Marks given message complete and removes it from the partially received messages. 
		/// </summary>
		/// <param name="message">
		/// A message to be completed.<see cref="Message"/>
		/// </param>
        public void CompleteInboundMessage(Message message)
        {
            MessagesReceived++;
            
            lock (partialInboundMessages)
            {
                partialInboundMessages.Remove(message.MessageId);
            }

            if (DebugMessages)
            {
                LogUtil.Debug("Session " + IncomingSessionId + " Received: " + message);
            }

            if (message.GetType() == typeof(ThrottleMessage) ||
                message.GetType() == typeof(AcknowledgeMessage) ||
                message.GetType() == typeof(KeepaliveMessage))
            {
                lock (inboundControlMessages)
                {
                    inboundControlMessages.Enqueue(message);
                }
            }
            else
            {
                lock (inboundMessages)
                {
                    inboundMessages.Enqueue(message);
                }
            }
        }

        #endregion

        #region Partially Sent Outbound Messages Methods

		/// <summary>
		/// Number of partially sent message count. 
		/// TODO: Change to property.
		/// </summary>
		/// <returns>
		/// Number of partially sent messages.<see cref="System.Int32"/>
		/// </returns>
        public int GetPartiallySentMessageCount()
        {
            lock (partialOutboundMessages)
            {
                return partialOutboundMessages.Count;
            }
        }

		/// <summary>
		/// Returns a message from the partial outbound message list. Fills 
		/// the list if outbound messages are availabe and list is not full.
		/// Partial outbound message list contains messages which are currently 
		/// being multiplexed to the outbound UDP packets.
		/// </summary>
		/// <returns>
		/// Current message from the partial outbound message list.
		/// </returns>
        public MessageEntry GetPartialOutboundMessage()
        {
			// Get the partial outbound message count to separate variable to avoid nested locks of two different lists.
            int partialOutboundMessageCount=0;
            lock (partialOutboundMessages)
            {
                partialOutboundMessageCount = partialOutboundMessages.Count;
            }
			
			// If there is space add a new message to the partial outbound message list.
            if (partialOutboundMessageCount < 10)
            {
                lock (outboundMessages)
                {
                    if (outboundMessages.Count > 0)
                    {
                        MessageEntry entry = new MessageEntry();
                        entry.Message = outboundMessages.Dequeue();
                        partialOutboundMessages.AddLast(entry);
                    }
                }
            }

			// Return current partial outbound message.
            lock (partialOutboundMessages)
            {
				// Return null if there is no outbound messages available.
                if (partialOutboundMessages.Count == 0)
                {
                    return null;
                }

				// If there is no current partial outbound message then choose first.
                if (currentPartialOutboundMessage == null)
                {
                    currentPartialOutboundMessage = partialOutboundMessages.First;
                }

                LastSendTime = DateTime.Now;

                MessageEntry message = currentPartialOutboundMessage.Value;

                currentPartialOutboundMessage = currentPartialOutboundMessage.Next;

                return message;
            }
        }

		/// <summary>
		/// Removes outbound message from partial outbound messages after all frames has been sent. 
		/// </summary>
		/// <param name="message">
		/// A message to be completed.<see cref="MessageEntry"/>
		/// </param>
        public void CompleteOutboundMessage(MessageEntry message)
        {
            if (DebugMessages)
            {
                LogUtil.Debug("Session " + IncomingSessionId + " Sent: " + message.Message);
            }
            lock (partialOutboundMessages)
            {
                MessagesSent++;
                partialOutboundMessages.Remove(message);
                if (currentPartialOutboundMessage != null && currentPartialOutboundMessage.Value == message)
                {
                    currentPartialOutboundMessage = null;
                }
                MessageFactory.Current.ReleaseMessage(message.Message);
            }
            // TODO should these be delegated to transmitter or server and hub completed outbound message event handlers?
            if (message.Message.GetType() == typeof(DetachResponseMessage) || 
                message.Message.GetType() == typeof(LeaveResponseMessage))
            {
                SetStateDisconnected();
            }
        }

        #endregion

        #region Acknowledge Messages Methods

		/// <summary>
		/// Adds given packet id to existing acknowledge message or creates new acknowledge message and adds to it. 
		/// </summary>
		/// <param name="packetId">
		/// A packet id to be acknowledged.<see cref="System.UInt32"/>
		/// </param>
        public void AddAcknowledge(uint packetId)
        {
            lock (outboundAcknowledgeMessages)
            {
                if (outboundAcknowledgeMessages.Count == 0)
                {
                    outboundAcknowledgeMessages.Add((AcknowledgeMessage)MessageFactory.Current.ReserveMessage(typeof(AcknowledgeMessage)));
                }
                AcknowledgeMessage acknowledgeMessage = outboundAcknowledgeMessages[outboundAcknowledgeMessages.Count - 1];
                if (acknowledgeMessage.PacketIdCount == acknowledgeMessage.MaxPacketIdCount)
                {
                    outboundAcknowledgeMessages.Add((AcknowledgeMessage)MessageFactory.Current.ReserveMessage(typeof(AcknowledgeMessage)));
                    acknowledgeMessage = outboundAcknowledgeMessages[outboundAcknowledgeMessages.Count - 1];
                }
                acknowledgeMessage.AddPacketId(packetId);
            }
        }

		/// <summary>
		/// Sends out all acknowledge messages. 
		/// </summary>
        public void SendAcknowledgeMessages()
        {
            lock (outboundAcknowledgeMessages)
            {
                for (int i = 0; i < outboundAcknowledgeMessages.Count; i++)
                {
                    Send(outboundAcknowledgeMessages[i]);
                    IsFirstAckSent = true;
                }
                outboundAcknowledgeMessages.Clear();
            }
        }

        #endregion

        #region Packets Waiting Acknowledge Methods

		/// <summary>
		/// Adds packet to list of packets waiting acknowledge. 
		/// These packets are resent if acknowledge is not received in time. 
		/// </summary>
		/// <param name="packet">
		/// A packet which needs to be acknowledged.<see cref="Packet"/>
		/// </param>
        public void AddPacketWaitingAcknowledge(Packet packet)
        {
            if (packetsWaitingAcknowledge.Count > MxpConstants.MaxPacketsWaitingAcknowledge)
            {
                packetsWaitingAcknowledge.Clear();
            }
            packetsWaitingAcknowledge.Add(packet.PacketId, packet);
        }

		/// <summary>
		/// Removes packets waiting acknowledge from the packets waiting acknowledge list. 
		/// </summary>
		/// <param name="packetId">
		/// A packet which has been acknowledged.<see cref="System.UInt32"/>
		/// </param>
        public void RemovePacketWaitingAcknowledge(uint packetId)
        {
            if (packetsWaitingAcknowledge.ContainsKey(packetId))
            {
                PacketFactory.Current.ReleasePacket(packetsWaitingAcknowledge[packetId]);
                packetsWaitingAcknowledge.Remove(packetId);
            }
        }

		/// <summary>
		/// Returns list of packets waiting for acknowledge. 
		/// </summary>
		/// <returns>
		/// Packets waiting acknowledge.<see cref="ICollection"/>
		/// </returns>
        public ICollection<Packet> GetPacketsWaitingAcknowledge()
        {
            return packetsWaitingAcknowledge.Values;
        }

		/// <summary>
		/// Gets number of packets waiting for acknowledge.
		/// TODO: Change to property. 
		/// </summary>
		/// <returns>
		/// Number of packets waiting acknowledge.<see cref="System.Int32"/>
		/// </returns>
        public int GetPacketWaitingAcknowledgeCount()
        {
            return packetsWaitingAcknowledge.Count;
        }

        #endregion
    
    }

}
