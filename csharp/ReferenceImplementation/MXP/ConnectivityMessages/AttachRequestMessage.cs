using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;
using MXP.Fragments;

namespace MXP.Messages
{
    public class AttachRequestMessage : Message
    {

        public Guid TargetBubbleId; // 16

        public BubbleFragment SourceBubbleFragment=new BubbleFragment(); // 195

        public string ProgramName; // 25
        public byte ProgramMajorVersion; // 1
        public byte ProgramMinorVersion; // 1
        public byte ProtocolMajorVersion; // 1
        public byte ProtocolMinorVersion; // 1
        public uint ProtocolSourceRevision; // 4

        public AttachRequestMessage()
        {
            TypeCode = 30;
            FrameCount = 1;
            Quaranteed = true;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 244;
        }

        #region IMessage Members

        public override void Clear()
        {
            TargetBubbleId = Guid.Empty;

            SourceBubbleFragment.Clear();

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

            currentIndex = EncodeUtil.Encode(ref TargetBubbleId, packetBytes, currentIndex);

            currentIndex = SourceBubbleFragment.EncodeFragmentData(packetBytes, currentIndex);

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

            currentIndex = EncodeUtil.Decode(ref TargetBubbleId, packetBytes, currentIndex);

            currentIndex = SourceBubbleFragment.DecodeFragmentData(packetBytes, currentIndex);

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
