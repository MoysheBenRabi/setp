using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;
using MXP.Fragments;

namespace MXP.Messages
{
    public class IdentifyRequestMessage : Message
    {
        public const String OPEN_ID_IDENTITY = "OpenId";

        public Guid ParticipantId; // 16
        public string ParticipantIdentityType; // 20
        public string ParticipantIdentity; // 219

        public IdentifyRequestMessage()
        {
            TypeCode = 80;
            FrameCount = 1;
            Quaranteed = true;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 255;
        }

        #region IMessage Members

        public override void Clear()
        {
            ParticipantId = Guid.Empty;
            ParticipantIdentityType = null;
            ParticipantIdentity = null;
            base.Clear();
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            currentIndex = EncodeUtil.Encode(ref ParticipantId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref ParticipantIdentityType, packetBytes, currentIndex, 20);
            currentIndex = EncodeUtil.Encode(ref ParticipantIdentity, packetBytes, currentIndex, 219);

            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;

            currentIndex = EncodeUtil.Decode(ref ParticipantId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref ParticipantIdentityType, packetBytes, currentIndex, 20);
            currentIndex = EncodeUtil.Decode(ref ParticipantIdentity, packetBytes, currentIndex, 219);

            return currentIndex;
        }

        #endregion

    }
}
