using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;
using MXP.Common.Proto;

namespace MXP.Messages
{
    public class MovementEventMessage : Message
    {

        public MovementEventMessage()
        {
            TypeCode = 41;
            FrameCount = 1;
            Quaranteed = false;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            return 32;
        }

        public uint ObjectIndex; // 4
        public MsdVector3f Location = new MsdVector3f(); // 12
        public MsdQuaternion4f Orientation = new MsdQuaternion4f(); // 16

        public override string ToString()
        {
            String str = "Movement [" +
            ",ObjectIndex: " + ObjectIndex +
            ",Location: " + Location.X + "," + Location.Y + "," + Location.Z + "," +
            ",Orientation: " + Orientation.X + "," + Orientation.Y + "," + Orientation.Z + "," + Orientation.W;
            str += "]";

            return str;
        }

        #region IMessage Members

        public override void Clear()
        {
            ObjectIndex = 0;
            Location.X = 0; Location.Y = 0; Location.Z = 0;
            Orientation.X = 0; Orientation.Y = 0; Orientation.Z = 0; Orientation.W = 0;
            base.Clear();
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            currentIndex = EncodeUtil.Encode(ref ObjectIndex, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(Location.X, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(Location.Y, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(Location.Z, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(Orientation.X, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(Orientation.Y, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(Orientation.Z, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(Orientation.W, packetBytes, currentIndex);

            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;
            float tempFloat = 0;
            currentIndex = EncodeUtil.Decode(ref ObjectIndex, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
            Location.X = tempFloat;
            currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
            Location.Y = tempFloat;
            currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
            Location.Z = tempFloat;
            currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
            Orientation.X = tempFloat;
            currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
            Orientation.Y = tempFloat;
            currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
            Orientation.Z = tempFloat;
            currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
            Orientation.W = tempFloat;

            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
