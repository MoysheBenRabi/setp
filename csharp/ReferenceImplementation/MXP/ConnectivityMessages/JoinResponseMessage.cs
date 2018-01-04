using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;

namespace MXP.Messages
{
    public class JoinResponseMessage : Message
    {

        public JoinResponseMessage()
        {
            TypeCode = 11;
            FrameCount = 1;
            Quaranteed = true;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 192;
        }

        public uint RequestMessageId = 0; // 4
        public byte FailureCode; // 1

        public Guid BubbleId = Guid.Empty; // 16
        public Guid ParticipantId = Guid.Empty; // 16
        public Guid AvatarId = Guid.Empty; // 16
        public string BubbleName; // 40
        public string BubbleAssetCacheUrl; // 50
        public float BubbleRange; // 4
        public float BubblePerceptionRange; // 4
        public ulong BubbleRealTime; // 8

        public string ProgramName; // 25
        public byte ProgramMajorVersion; // 1
        public byte ProgramMinorVersion; // 1
        public byte ProtocolMajorVersion; // 1
        public byte ProtocolMinorVersion; // 1
        public uint ProtocolSourceRevision; // 4

        #region IMessage Members

        public override void Clear()
        {
            RequestMessageId = 0;
            FailureCode = 0;

            BubbleId = Guid.Empty;
            ParticipantId = Guid.Empty;
            AvatarId = Guid.Empty;
            BubbleName = null;
            BubbleAssetCacheUrl = null;
            BubbleRange = 0;
            BubblePerceptionRange = 0;
            BubbleRealTime = 0;

            ProgramName = null;
            ProgramMajorVersion = 0;
            ProgramMinorVersion = 0;
            ProtocolMajorVersion = 0;
            ProtocolMinorVersion = 0;
            ProtocolSourceRevision = 0;

            base.Clear();
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            // Response Fragment
            currentIndex = EncodeUtil.Encode(ref RequestMessageId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref FailureCode, packetBytes, currentIndex);

            currentIndex = EncodeUtil.Encode(ref BubbleId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref ParticipantId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref AvatarId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref BubbleName, packetBytes, currentIndex, 40);
            currentIndex = EncodeUtil.Encode(ref BubbleAssetCacheUrl, packetBytes, currentIndex, 50);
            currentIndex = EncodeUtil.Encode(ref BubbleRange, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref BubblePerceptionRange, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref BubbleRealTime, packetBytes, currentIndex);

            // Program Fragment
            currentIndex = EncodeUtil.Encode(ref ProgramName, packetBytes, currentIndex, 25);
            currentIndex = EncodeUtil.Encode(ref ProgramMajorVersion, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref ProgramMinorVersion, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref ProtocolMajorVersion, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref ProtocolMinorVersion, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref ProtocolSourceRevision, packetBytes, currentIndex);

            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;

            // Response Fragment
            currentIndex = EncodeUtil.Decode(ref RequestMessageId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref FailureCode, packetBytes, currentIndex);

            currentIndex = EncodeUtil.Decode(ref BubbleId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref ParticipantId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref AvatarId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref BubbleName, packetBytes, currentIndex, 40);
            currentIndex = EncodeUtil.Decode(ref BubbleAssetCacheUrl, packetBytes, currentIndex, 50);
            currentIndex = EncodeUtil.Decode(ref BubbleRange, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref BubblePerceptionRange, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref BubbleRealTime, packetBytes, currentIndex);

            // Program Fragment
            currentIndex = EncodeUtil.Decode(ref ProgramName, packetBytes, currentIndex, 25);
            currentIndex = EncodeUtil.Decode(ref ProgramMajorVersion, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref ProgramMinorVersion, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref ProtocolMajorVersion, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref ProtocolMinorVersion, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref ProtocolSourceRevision, packetBytes, currentIndex);

            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
