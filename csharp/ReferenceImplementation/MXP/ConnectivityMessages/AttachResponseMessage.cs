using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;
using MXP.Fragments;

namespace MXP.Messages
{
    public class AttachResponseMessage : Message
    {

        public AttachResponseMessage()
        {
            TypeCode = 31;
            FrameCount = 1;
            Quaranteed = true;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 233;
        }

        public uint RequestMessageId = 0; // 4
        public byte FailureCode; // 1

        public BubbleFragment TargetBubbleFragment=new BubbleFragment(); // 195

        public string ProgramName; //  25
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

            TargetBubbleFragment.Clear();

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

            currentIndex = TargetBubbleFragment.EncodeFragmentData(packetBytes, currentIndex);

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

            currentIndex = TargetBubbleFragment.DecodeFragmentData(packetBytes, currentIndex);

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
