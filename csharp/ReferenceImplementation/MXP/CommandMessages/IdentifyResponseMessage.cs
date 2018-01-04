using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;

namespace MXP.Messages
{
    public class IdentifyResponseMessage : Message
    {

        public IdentifyResponseMessage()
        {
            TypeCode = 81;
            FrameCount = 1;
            Quaranteed = true;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 5;
        }

        public uint RequestMessageId = 0; // 4
        public byte FailureCode; // 1

        #region IMessage Members

        public override void Clear()
        {
            RequestMessageId = 0;
            FailureCode = 0;

            base.Clear();
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            // Response Fragment
            currentIndex = EncodeUtil.Encode(ref RequestMessageId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref FailureCode, packetBytes, currentIndex);

            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;

            // Response Fragment
            currentIndex = EncodeUtil.Decode(ref RequestMessageId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref FailureCode, packetBytes, currentIndex);

            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
