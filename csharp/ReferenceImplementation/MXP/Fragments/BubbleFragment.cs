using System;
using System.Collections.Generic;

using System.Text;
using MXP.Util;
using MXP.Common.Proto;

namespace MXP.Fragments
{
    public class BubbleFragment : Fragment
    {
        public Guid BubbleId; // 16
        public string BubbleName; // 40
        public string BubbleAssetCacheUrl; // 51
        public Guid OwnerId; // 16
        public string BubbleAddress; // 40
        public uint BubblePort; // 4
        public MsdVector3f BubbleCenter = new MsdVector3f(); // 12
        public float BubbleRange; // 4
        public float BubblePerceptionRange; // 4
        public ulong BubbleRealTime; // 8

        public override string ToString()
        {
            return "BubbleFragmet [" +
                "BubbleId:" + BubbleId +
                ",BubbleName:" + BubbleName +
                ",BubbleAssetCacheUrl:" + BubbleAssetCacheUrl +
                ",OwnerId:" + OwnerId +
                ",BubbleServerAddress:" + BubbleAddress +
                ",BubbleServerPort:" + BubblePort +
                ",BubbleCenter.X:" + BubbleCenter.X +
                ",BubbleCenter.Y:" + BubbleCenter.Y +
                ",BubbleCenter.Z:" + BubbleCenter.Z +
                ",BubbleRange:" + BubbleRange +
                ",BubblePerceptionRange:" + BubblePerceptionRange +
                ",BubbleRealTime:" + BubbleRealTime +
                "]";
        }

        public override void Clear()
        {
            BubbleId=Guid.Empty;
            BubbleName=null;
            BubbleAssetCacheUrl = null;
            OwnerId = Guid.Empty;
            BubbleAddress = null;
            BubblePort=0;
            BubbleCenter.X = 0;
            BubbleCenter.Y = 0;
            BubbleCenter.Z = 0;
            BubbleRange = 0;
            BubblePerceptionRange = 0;
            BubbleRealTime = 0;
        }

        public override int EncodeFragmentData(byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;
            
            currentIndex = EncodeUtil.Encode(ref BubbleId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref BubbleName, packetBytes, currentIndex, 40);
            currentIndex = EncodeUtil.Encode(ref BubbleAssetCacheUrl, packetBytes, currentIndex, 51);
            currentIndex = EncodeUtil.Encode(ref OwnerId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref BubbleAddress, packetBytes, currentIndex, 40);
            currentIndex = EncodeUtil.Encode(ref BubblePort, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(BubbleCenter.X, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(BubbleCenter.Y, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(BubbleCenter.Z, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref BubbleRange, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref BubblePerceptionRange, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref BubbleRealTime, packetBytes, currentIndex);

            return currentIndex;
        }

        public override int DecodeFragmentData(byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            float tempFloat = 0;
            currentIndex = EncodeUtil.Decode(ref BubbleId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref BubbleName, packetBytes, currentIndex, 40);
            currentIndex = EncodeUtil.Decode(ref BubbleAssetCacheUrl, packetBytes, currentIndex, 51);
            currentIndex = EncodeUtil.Decode(ref OwnerId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref BubbleAddress, packetBytes, currentIndex, 40);
            currentIndex = EncodeUtil.Decode(ref BubblePort, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
            BubbleCenter.X = tempFloat;
            currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
            BubbleCenter.Y = tempFloat;
            currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
            BubbleCenter.Z = tempFloat;
            currentIndex = EncodeUtil.Decode(ref BubbleRange, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref BubblePerceptionRange, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref BubbleRealTime, packetBytes, currentIndex);

            return currentIndex;
        }

    }
}
