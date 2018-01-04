using System;
using System.Collections.Generic;

using System.Text;
using MXP.Messages;

namespace MXP
{

    /// <summary>
    /// MessageFactory is used to reserve and release messasages from recycling pool.
    /// Recycling helps to lower the carbage collection load on real time simulation applications.
    /// </summary>
    public class MessageFactory
    { 

		/// <value>
		/// The singleton MessageFactory instance. 
		/// </value>
        private static MessageFactory theCurret = null;
		/// <value>
		/// Pool of recycled messages. 
		/// </value>
        private IDictionary<byte, Queue<Message>> messages = new Dictionary<byte, Queue<Message>>();
        /// <summary>
        /// Contains the number of message types;
        /// </summary>
        private int messageTypeCount = 0;
		/// <value>
		/// Dictionary of in wire type code and C# message type. 
		/// </value>
        private IDictionary<byte, Type> codeTypeDictionary = new Dictionary<byte, Type>();
		/// <value>
		/// Dictionary of C# message type and in wire type code. 
		/// </value>
        private IDictionary<Type, byte> typeCodeDictionary = new Dictionary<Type, byte>();

		/// <value>
		/// Number of reserved messages per type. 
		/// </value>
        private IDictionary<Type, int> reservedCount = new Dictionary<Type, int>();
		/// <value>
		/// Number of released messages per type. 
		/// </value>
        private IDictionary<Type, int> releasedCount = new Dictionary<Type, int>();

        /// <summary>
        /// Returns number of different message types.
        /// </summary>
        public int MessageTypeCount
        {
            get
            {
                return messageTypeCount;
            }
        }

		/// <value>
		/// The singleton MessageFactory instance property. 
		/// </value>
        public static MessageFactory Current
        {
            get
            {
                lock (typeof(MessageFactory))
                {
                    if (theCurret == null)
                    {
                        theCurret = new MessageFactory();
                    }
                    return theCurret;
                }
            }
        }

		/// <summary>
		/// Default constructor which initializes the message factory by registering message types.
		/// </summary>
        public MessageFactory()
        {
            AddMessageType(new AcknowledgeMessage());
            AddMessageType(new KeepaliveMessage());
            AddMessageType(new ThrottleMessage());
            AddMessageType(new ChallengeRequestMessage());
            AddMessageType(new ChallengeResponseMessage());
            AddMessageType(new JoinRequestMessage());
            AddMessageType(new JoinResponseMessage());
            AddMessageType(new LeaveRequestMessage());
            AddMessageType(new LeaveResponseMessage());
            AddMessageType(new ListBubblesRequest());
            AddMessageType(new ListBubblesResponse());
            AddMessageType(new AttachRequestMessage());
            AddMessageType(new AttachResponseMessage());
            AddMessageType(new DetachRequestMessage());
            AddMessageType(new DetachResponseMessage());

            AddMessageType(new EjectRequestMessage());
            AddMessageType(new EjectResponseMessage());
            AddMessageType(new ExamineRequestMessage());
            AddMessageType(new ExamineResponseMessage());
            AddMessageType(new HandoverRequestMessage());
            AddMessageType(new HandoverResponseMessage());
            AddMessageType(new InjectRequestMessage());
            AddMessageType(new InjectResponseMessage());
            AddMessageType(new InteractRequestMessage());
            AddMessageType(new InteractResponseMessage());
            AddMessageType(new ModifyRequestMessage());
            AddMessageType(new ModifyResponseMessage());

            AddMessageType(new ActionEventMessage());
            AddMessageType(new DisappearanceEventMessage());
            AddMessageType(new HandoverEventMessage());
            AddMessageType(new MovementEventMessage());
            AddMessageType(new PerceptionEventMessage());

            AddMessageType(new SynchronizationBeginEventMessage());
            AddMessageType(new SynchronizationEndEventMessage());

            AddMessageType(new IdentifyRequestMessage());
            AddMessageType(new IdentifyResponseMessage());
        }

		/// <summary>
		/// Utility method for registering new message type. 
		/// </summary>
		/// <param name="prototype">
		/// A prototype instance of the message type to be registered.<see cref="Message"/>
		/// </param>
        private void AddMessageType(Message prototype)
        {
            codeTypeDictionary.Add(prototype.TypeCode, prototype.GetType());
            typeCodeDictionary.Add(prototype.GetType(), prototype.TypeCode);
            releasedCount[prototype.GetType()] = 0;
            reservedCount[prototype.GetType()] = 0;
            messages[prototype.TypeCode] = new Queue<Message>();
            messageTypeCount++;
        }

		/// <summary>
		/// Reserves message from message factory. 
		/// </summary>
		/// <param name="messageType">
		/// A message type of interest.<see cref="Type"/>
		/// </param>
		/// <returns>
		/// A reserved message.<see cref="Message"/>
		/// </returns>
        public Message ReserveMessage(Type messageType)
        {
            return ReserveMessage(typeCodeDictionary[messageType]);
        }

		/// <summary>
		/// Reserves message from message factory. If message of given type exists
		/// in recycle pool then returns that otherwise instantiates new message.
		/// </summary>
		/// <param name="typeCode">
		/// An in wire message type code.<see cref="System.Byte"/>
		/// </param>
		/// <returns>
		/// A reserved message.<see cref="Message"/>
		/// </returns>
        public Message ReserveMessage(byte typeCode)
        {
            lock (messages)
            {
                if (!codeTypeDictionary.ContainsKey(typeCode))
                {
                    throw new Exception("Unknown message type code: " + typeCode);
                }

                Type messageType=codeTypeDictionary[typeCode];

                reservedCount[messageType] = reservedCount[messageType]+1;

                if (messages[typeCode].Count == 0)
                {
                    Message newMessage = (Message)messageType.GetConstructor(new Type[0]).Invoke(new Object[0]);
                    newMessage.IsAutoRelease = true;
                    return newMessage;
                }

                Message message = messages[typeCode].Dequeue();
                return message;
            }
       }

		/// <summary>
		/// Clears and releases message to recycle pool. 
		/// </summary>
		/// <param name="message">
		/// A message to be released.<see cref="Message"/>
		/// </param>
        public void ReleaseMessage(Message message)
        {
            if (message.IsAutoRelease)
            {
                lock (messages)
                {
                    Type messageType = message.GetType();
                    releasedCount[messageType] = releasedCount[messageType] + 1;

                    if (messages[message.TypeCode].Contains(message))
                    {
                        throw new Exception("Message was already released: " + message.ToString());
                    }

                    message.Clear();
                    messages[message.TypeCode].Enqueue(message);
                }
            }
        }

		/// <summary>
		/// Writes message factory statistics to string. 
		/// </summary>
		/// <returns>
		/// A string representation of message factory statistics.<see cref="System.String"/>
		/// </returns>
        public override string ToString()
        {
            lock (messages)
            {
                String str = "MessageFactory {";
                
                foreach (Type type in typeCodeDictionary.Keys)
                {
                    if (messages[typeCodeDictionary[type]].Count != 0 || reservedCount[type] != 0 || releasedCount[type]!=0)
                    {
                        str += type.Name + "(" + messages[typeCodeDictionary[type]].Count + "|" + reservedCount[type] + "|" + releasedCount[type] + ") ";
                    }
                }

                str+="}";
                return str;
            }
        }

    }
}
