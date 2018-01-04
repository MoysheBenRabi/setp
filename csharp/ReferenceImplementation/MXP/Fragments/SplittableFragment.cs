using System;
using System.Collections.Generic;

using System.Text;

namespace MXP.Fragments
{
    public abstract class SplittableFragment
    {
        public abstract void Clear();

        public ushort FrameCount;

        public abstract byte FragmentDataSize(int frameIndex);

        public abstract int EncodeFragmentData(int frameIndex,byte[] packetBytes, int startIndex);

        public abstract int DecodeFragmentData(int frameIndex,byte[] packetBytes, int startIndex);
    }
}
