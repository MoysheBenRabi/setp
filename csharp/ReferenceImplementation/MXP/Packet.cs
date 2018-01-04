using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace MXP
{

    /// <summary>
    /// Packet is a container which consists of one to many message frames. Packets are sent over UDP to the recipient.
    /// </summary>
    public class Packet
    {
        public uint SessionId;
        public uint PacketId;
        public ulong FirstSendTime;
        public bool Quaranteed;
        public byte ResendCount;
        public int DataStartIndex;
        public int PacketLength;
        public byte[] PacketBytes = new byte[MxpConstants.MaxPacketSize];
        public static byte[] ClearBytes = new byte[MxpConstants.MaxPacketSize]; 

        // Last send time is used to decude when to resend non acked quaranteed packets.
        public DateTime LastSendTime { get; set; }

        public Packet()
        {
            SessionId = 0;
            PacketId = 0;
            FirstSendTime = 0;
            Quaranteed = false;
            ResendCount = 0;
            DataStartIndex = 0;
            PacketLength = 0;
            LastSendTime = new DateTime();
        }

        public virtual void Clear()
        {
            SessionId = 0;
            PacketId = 0;
            FirstSendTime = 0;
            Quaranteed = false;
            ResendCount = 0;
            DataStartIndex = 0;
            PacketLength = 0;
            LastSendTime = new DateTime();
            Buffer.BlockCopy(ClearBytes, 0, PacketBytes, 0, MxpConstants.MaxPacketSize); 
            // Not clearing bytes as it is not necessary.
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            Packet message = (Packet)obj;

            return this.PacketId.Equals(message.PacketId);
        }

        public override int GetHashCode()
        {
            return this.PacketId.GetHashCode();
        }

        public override string ToString()
        {
            String str = GetType().Name + " {";

            FieldInfo[] fieldInfos = this.GetType().GetFields();

            for (int i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo fieldInfo = fieldInfos[i];
                if (!fieldInfo.IsPublic)
                {
                    continue;
                }
                if (fieldInfo.FieldType == typeof(byte[]))
                {
                    str += fieldInfo.Name + "=";
                    byte[] bytes = (byte[])fieldInfo.GetValue(this);
                    for (int j=0;j<bytes.Length;j++)
                    {                        
                        str += bytes[j];
                        if (j < bytes.Length - 1)
                        {
                            str += "|";
                        }
                    }
                }
                else
                {
                    str += fieldInfo.Name + "=" + fieldInfo.GetValue(this);
                }

                if (i < fieldInfos.Length - 1)
                {
                    str += "|";
                }

            }

            str += "}";

            return str;
        }

    }
}
