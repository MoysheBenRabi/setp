using System;
using System.Collections.Generic;
using System.Text;
using MXP.Common.Proto;

namespace MXP.Util
{
    // Simple utility functions for calculating with the msd vectors.
    public class MathUtil
    {
        public static float Length(MsdVector3f vector)
        {
            return (float) Math.Sqrt(vector.X*vector.X+vector.Y*vector.Y+vector.Z*vector.Z);
        }

        public static float Distance(MsdVector3f v1, MsdVector3f v2)
        {
            return (float)Math.Sqrt(
                (v2.X - v1.X) * (v2.X - v1.X) +
                (v2.Y - v1.Y) * (v2.Y - v1.Y) +
                (v2.Z - v1.Z) * (v2.Z - v1.Z));
        }
    }
}
