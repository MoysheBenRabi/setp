using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;

namespace MXP.Messages
{
    public class ThrottleMessage : Message
    {

        public uint BytesPerSecond;

        public ThrottleMessage()
        {
            TypeCode = 3;
            FrameCount = 1;
            Quaranteed = true;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 4;
        }

        #region IMessage Members

        public override void Clear()
        {
            BytesPerSecond = 0;
            base.Clear();
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;
            currentIndex = EncodeUtil.Encode(ref BytesPerSecond, packetBytes, currentIndex);
            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;
            currentIndex = EncodeUtil.Decode(ref BytesPerSecond, packetBytes, currentIndex);
            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
