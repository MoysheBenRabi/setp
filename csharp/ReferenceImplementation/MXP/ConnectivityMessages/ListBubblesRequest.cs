using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;

namespace MXP.Messages
{
    public class ListBubblesRequest : Message
    {
        public const int ListTypeHosted = 0;
        public const int ListTypeLinked = 1;
        public const int ListTypeConnected = 2;

        public byte ListType;

        public ListBubblesRequest()
        {
            TypeCode = 25;
            FrameCount = 1;
            Quaranteed = true;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 1;
        }

        #region IMessage Members

        public override void Clear()
        {
            base.Clear();
            ListType = 0;
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;
            currentIndex = EncodeUtil.Encode(ref ListType, packetBytes, currentIndex);
            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;
            currentIndex = EncodeUtil.Decode(ref ListType, packetBytes, currentIndex);
            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
