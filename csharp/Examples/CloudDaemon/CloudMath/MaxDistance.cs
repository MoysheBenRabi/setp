// Copyright (c) 2008 Vesa Tuomiaro
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, 
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.

using System;

namespace CloudMath
{
    public static partial class Common
    {
        /// <summary>
        /// Calculates the maximum distance between the specified plane and the specified bounding box.
        /// </summary>
        /// <param name="value">A <see cref="Plane"/>.</param>
        /// <param name="min">Minimum coordinate of the bounding box.</param>
        /// <param name="max">Maximum coordinate of the bounding box.</param>
        /// <returns>Maximum distance between the plane and the bounding box.</returns>
        public static float MaxDistance(ref Plane value, ref Vector3 min, ref Vector3 max)
        {
            float x = (value.Normal.X < 0) ? min.X : max.X;
            float y = (value.Normal.Y < 0) ? min.Y : max.Y;
            float z = (value.Normal.Z < 0) ? min.Z : max.Z;

            return
                value.Normal.X * x +
                value.Normal.Y * y +
                value.Normal.Z * z +
                value.D;
        }

        /// <summary>
        /// Calculates the maximum distance between the specified plane and the specified box.
        /// </summary>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        /// <returns>Maximum distance between the plane and the box.</returns>
        public static float MaxDistance(ref Plane value1, ref BoundingBox value2)
        {
            float x = (value1.Normal.X < 0) ? value2.Minimum.X : value2.Maximum.X;
            float y = (value1.Normal.Y < 0) ? value2.Minimum.Y : value2.Maximum.Y;
            float z = (value1.Normal.Z < 0) ? value2.Minimum.Z : value2.Maximum.Z;

            return
                value1.Normal.X * x +
                value1.Normal.Y * y +
                value1.Normal.Z * z +
                value1.D;
        }
    }
}
