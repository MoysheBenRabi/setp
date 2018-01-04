using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;

namespace MXP.Messages
{
    public class JoinRequestMessage : Message
    {

        public JoinRequestMessage()
        {
            TypeCode = 10;
            FrameCount = 1;
            Quaranteed = true;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 255;
        }

        public Guid BubbleId= Guid.Empty; // 16
        public Guid AvatarId = Guid.Empty; // 16
        public string BubbleName; // 40
        public string LocationName; // 28

        public string ParticipantIdentifier; // 32
        public string ParticipantSecret; // 32
        public ulong ParticipantRealTime; // 8
        public string IdentityProviderUrl; // 50

        public string ProgramName; // 25
        public byte ProgramMajorVersion; // 1
        public byte ProgramMinorVersion; // 1
        public byte ProtocolMajorVersion; // 1
        public byte ProtocolMinorVersion; // 1
        public uint ProtocolSourceRevision; // 4

        #region IMessage Members

        public override void Clear()
        {
            BubbleId = Guid.Empty;
            AvatarId = Guid.Empty;
            BubbleName = null;
            LocationName = null;
            ParticipantIdentifier=null;
            ParticipantSecret = null;
            ParticipantRealTime = 0;
            IdentityProviderUrl = null;

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

            currentIndex = EncodeUtil.Encode(ref BubbleId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref AvatarId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref BubbleName, packetBytes, currentIndex, 40);
            currentIndex = EncodeUtil.Encode(ref LocationName, packetBytes, currentIndex, 28);
            currentIndex = EncodeUtil.Encode(ref ParticipantIdentifier, packetBytes, currentIndex, 32);
            currentIndex = EncodeUtil.Encode(ref ParticipantSecret, packetBytes, currentIndex, 32);
            currentIndex = EncodeUtil.Encode(ref ParticipantRealTime, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref IdentityProviderUrl, packetBytes, currentIndex, 50);

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

            currentIndex = EncodeUtil.Decode(ref BubbleId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref AvatarId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref BubbleName, packetBytes, currentIndex, 40);
            currentIndex = EncodeUtil.Decode(ref LocationName, packetBytes, currentIndex, 28);
            currentIndex = EncodeUtil.Decode(ref ParticipantIdentifier, packetBytes, currentIndex, 32);
            currentIndex = EncodeUtil.Decode(ref ParticipantSecret, packetBytes, currentIndex, 32);
            currentIndex = EncodeUtil.Decode(ref ParticipantRealTime, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref IdentityProviderUrl, packetBytes, currentIndex, 50);

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
