using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;

namespace MXP.Messages
{
    public class ExamineRequestMessage : Message
    {

        public ExamineRequestMessage()
        {
            TypeCode = 22;
            FrameCount = 1;
            Quaranteed = true;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 20;
        }

        public Guid ObjectId= Guid.Empty; // 16
        public uint ObjectIndex; // 4

        #region IMessage Members

        public override void Clear()
        {
            ObjectId = Guid.Empty;
            ObjectIndex = 0;

            base.Clear();
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            currentIndex = EncodeUtil.Encode(ref ObjectId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref ObjectIndex, packetBytes, currentIndex);

            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;

            currentIndex = EncodeUtil.Decode(ref ObjectId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref ObjectIndex, packetBytes, currentIndex);

            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
