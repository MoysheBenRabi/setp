using System;
using System.Collections.Generic;

using System.Text;

namespace MXP.Fragments
{
    public abstract class Fragment
    {
        public abstract void Clear();

        public abstract int EncodeFragmentData(byte[] packetBytes, int startIndex);

        public abstract int DecodeFragmentData(byte[] packetBytes, int startIndex);
    }
}
