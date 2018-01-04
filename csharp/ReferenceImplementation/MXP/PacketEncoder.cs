using System;
using System.Collections.Generic;

using System.Text;
using MXP.Util;

namespace MXP
{
    /// <summary>
    /// PacketEncoder encodes and decodes packet headers and packet data to and from UDP packet bytes.
    /// </summary>
    public class PacketEncoder
    {
        public static void EncodePacketHeader(Packet packet)
        {
            int currentIndex = 0;
            currentIndex = EncodeUtil.Encode(ref packet.SessionId, packet.PacketBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref packet.PacketId, packet.PacketBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref packet.FirstSendTime, packet.PacketBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref packet.Quaranteed, packet.PacketBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref packet.ResendCount, packet.PacketBytes, currentIndex);
            packet.DataStartIndex = currentIndex;
        }

        public static void EncodePacketData(Session session, Packet packet, ref bool packetQuaranteed)
        {
            int currentIndex = packet.DataStartIndex;
            int lastIndex=0;
           
            for(int i=0;i<150;i++) 
            {
                lastIndex=currentIndex;

                bool frameQuaranteed = false;
                currentIndex = FrameEncoder.EncodeFrame(session, packet.PacketBytes, currentIndex, ref frameQuaranteed);
                
                if(frameQuaranteed)
                {
                    packetQuaranteed = true;
                }

                if (currentIndex > MxpConstants.MaxPacketSize - MxpConstants.MaxFrameDataSize)
                {
                    break; // Packet full.
                }
                if (currentIndex == lastIndex)
                {
                    break; // Nothing queued to send.
                }
            }

            packet.PacketLength = currentIndex;
        }

        public static void DecodePacketHeader(Packet packet)
        {
            int currentIndex = 0;
            currentIndex = EncodeUtil.Decode(ref packet.SessionId, packet.PacketBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref packet.PacketId, packet.PacketBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref packet.FirstSendTime, packet.PacketBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref packet.Quaranteed, packet.PacketBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref packet.ResendCount, packet.PacketBytes, currentIndex);
            packet.DataStartIndex = currentIndex;
        }

        public static void DecodePacketData(Session session, Packet packet, ref bool packetQuaranteed)
        {
            int currentIndex = packet.DataStartIndex;

            for (int i = 0; i < 150; i++)
            {              
                bool frameQuaranteed = false;
                
                currentIndex = FrameEncoder.DecodeFrame(session, packet.PacketBytes, currentIndex, ref frameQuaranteed);

                if (frameQuaranteed)
                {
                    packetQuaranteed = true;
                }

                if (currentIndex == packet.PacketLength)
                {
                    break; // Packet full.
                }
            }

        }
    }
}
