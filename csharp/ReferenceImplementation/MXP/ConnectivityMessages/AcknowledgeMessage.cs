using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;

namespace MXP.Messages
{
    public class AcknowledgeMessage : Message
    {

        public int maxPacketIdCount = MxpConstants.MaxFrameDataSize / 4;
        public uint[] packetIds = new uint[MxpConstants.MaxFrameDataSize / 4];
        public int packetIdCount = 0;

        public AcknowledgeMessage()
        {
            TypeCode = 1;
            FrameCount = 1;
            Quaranteed = false;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return (byte)(packetIdCount*4);
        }

        public void AddPacketId(uint packetId)
        {
            if (packetIdCount == maxPacketIdCount)
            {
                throw new Exception("Acknowledge message is already full.");
            }
            packetIds[packetIdCount] = packetId;
            packetIdCount++;
        }

        public uint GetPacketId(int index)
        {
            if (index >= packetIdCount)
            {
                throw new Exception("Out of packet id array bounds: "+index);
            }
            return packetIds[index];
        }

        public int PacketIdCount
        {
            get
            {
                return packetIdCount;
            }
        }

        public int MaxPacketIdCount
        {
            get
            {
                return maxPacketIdCount;
            }
        }

        #region IMessage Members

        public override void Clear()
        {
            for (int i = 0; i < packetIdCount; i++)
            {
                packetIds[i] = 0;
            }
            packetIdCount = 0;
            base.Clear();
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;
            for (int i = 0; i < packetIdCount; i++)
            {
                currentIndex = EncodeUtil.Encode(ref packetIds[i], packetBytes, currentIndex);
            }
            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            packetIdCount = length / 4;
            int currentIndex = startIndex;
            for (int i = 0; i < packetIdCount; i++)
            {
                currentIndex = EncodeUtil.Decode(ref packetIds[i], packetBytes, currentIndex);
            }
            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
