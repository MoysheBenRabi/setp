using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;

namespace MXP.Messages
{
    public class ChallengeRequestMessage : Message
    {

        public byte[] ChallengeRequestBytes=new byte[64];

        public ChallengeRequestMessage()
        {
            TypeCode = 4;
            FrameCount = 1;
            Quaranteed = true;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 64;
        }

        #region IMessage Members

        public override void Clear()
        {
            for (int i = 0; i < ChallengeRequestBytes.Length; i++)
            {
                ChallengeRequestBytes[i] = 0;
            }
            base.Clear();
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;
            currentIndex = EncodeUtil.Encode(ref ChallengeRequestBytes, packetBytes, currentIndex,64);
            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;
            currentIndex = EncodeUtil.Decode(ref ChallengeRequestBytes, packetBytes, currentIndex, 64);
            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
