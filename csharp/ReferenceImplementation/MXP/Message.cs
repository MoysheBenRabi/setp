using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace MXP
{

    /// <summary>
    /// Message is abstract base class for MXP messages.
    /// </summary>
    public abstract class Message
    {

		/// <summary>
		/// Counter which is used to assign unique id to each message.
		/// TODO: Currently id is reserved to every new message from global counter. This counter could be session specific. 
		/// </summary>
        public static uint MessateIdCounter = 0;
        /// <summary>
        /// Message id. Unique in the session. 
        /// </summary>
        public uint MessageId;
		/// <summary>
		/// Message in wire type code. 
		/// </summary>
        public byte TypeCode;
		/// <summary>
		/// True if message is guaranteed and packets need to be resent if not acked in time. 
		/// </summary>
        public bool Quaranteed;
		/// <summary>
		/// Number of frames in this message. 
		/// </summary>
        public ushort FrameCount;

		/// <summary>
		/// True if network layer should release the message automaticly. Can not be used when same message is broadcasted to many observers. 
		/// </summary>
        public bool IsAutoRelease = false;

		/// <summary>
		/// Default constructor which assigns the message id to the message.
		/// TODO: Remove this constructor when message id counter is moved to session. 
		/// </summary>
        public Message()
        {
            lock (typeof(Message))
            {
                MessateIdCounter++;
                MessageId = MessateIdCounter;
            }
        }

		/// <summary>
		/// Generic code to write the content of a message to human readable string. 
		/// Some messages have specialized ToString methods.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder(GetType().Name + " {");

            FieldInfo[] fieldInfos = this.GetType().GetFields();

            for (int i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo fieldInfo = fieldInfos[i];
                if (fieldInfo.FieldType == typeof(byte[]))
                {
                    str .Append(fieldInfo.Name + "=");
                    byte[] array = (byte[])fieldInfo.GetValue(this);
                    if (array.Length < 512)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            str.Append( array[j]);
                            if (j < array.Length - 1)
                            {
                                str.Append( "|");
                            }
                        }
                    }
                    else
                    {
                        str.Append( "byte[" + array.Length + "]");
                    }
                }
                else if (fieldInfo.FieldType == typeof(uint[]))
                {
                    str.Append( fieldInfo.Name + "=");
                    uint[] array = (uint[])fieldInfo.GetValue(this);
                    for (int j = 0; j < array.Length; j++)
                    {
                        str.Append( array[j]);
                        if (j < array.Length - 1)
                        {
                            str.Append( "|");
                        }
                    }
                }
                else
                {
                    str.Append( fieldInfo.Name + "=" + fieldInfo.GetValue(this));
                }

                if (i < fieldInfos.Length - 1)
                {
                    str.Append( "|");
                }

            }

            str.Append("}");

            return str.ToString();
        }

		/// <summary>
		/// Clears the message so it can be reused. Inherited messages override this message
		/// to clear the message specific fields. They have to invoke this base method as well. 
		/// </summary>
        public virtual void Clear()
        {
            MessateIdCounter++;
            MessageId = MessateIdCounter;
        }

		/// <summary>
		/// Gets the size of given frame in bytes. 
		/// </summary>
		/// <param name="frameIndex">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Byte"/>
		/// </returns>
        public abstract byte FrameDataSize(int frameIndex);

        /// <summary>
        /// Prepares message to be encoded by calculating frame count.
        /// </summary>
        public virtual void PrepareEncoding()
        {
            return;
        }

		/// <summary>
		/// Encodes frame data to packet bytes. 
		/// </summary>
		/// <param name="frameIndex">
		/// Index of the frame in message. <see cref="System.Int32"/>
		/// </param>
		/// <param name="packetBytes">
		/// A byte array containing the packet bytes. <see cref="System.Byte"/>
		/// </param>
		/// <param name="startIndex">
		/// Start index of the frame data in packet bytes.<see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// Start index of next frame data in packet bytes. <see cref="System.Int32"/>
		/// </returns>
        public abstract int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex);

		/// <summary>
		/// Decodes frame data from packet bytes. 
		/// </summary>
		/// <param name="frameIndex">
		/// Index of the frame in message. <see cref="System.Int32"/>
		/// </param>
		/// <param name="packetBytes">
		/// A byte array containing the packet bytes. <see cref="System.Byte"/>
		/// </param>
		/// <param name="startIndex">
		/// Start index of the frame data in packet bytes.<see cref="System.Int32"/>
		/// </param>
		/// <param name="length">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// Start index of next frame data in packet bytes. <see cref="System.Int32"/>
		/// </returns>
        public abstract int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length);

    }
}
