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
        /// Calculates the dot product of the specified vectors.
        /// </summary>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Vector2"/>.</param>
        /// <returns>Dot product of the specified vectors.</returns>
        public static float Dot(ref Vector2 value1, ref Vector2 value2)
        {
            return
                value1.X * value2.X +
                value1.Y * value2.Y;
        }

        /// <summary>
        /// Calculates the dot product of the specified vectors.
        /// </summary>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <returns>Dot product of the specified vectors.</returns>
        public static float Dot(ref Vector3 value1, ref Vector3 value2)
        {
            return
                value1.X * value2.X +
                value1.Y * value2.Y +
                value1.Z * value2.Z;
        }

        /// <summary>
        /// Calculates the dot product of the specified vectors.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>Dot product of the specified vectors.</returns>
        public static float Dot(ref Vector4 value1, ref Vector4 value2)
        {
            return
                value1.X * value2.X +
                value1.Y * value2.Y +
                value1.Z * value2.Z +
                value1.W * value2.W;
        }

        /// <summary>
        /// Calculates the dot product of the specified quaternions.
        /// </summary>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        /// <returns>Dot product of the specified quaternions.</returns>
        public static float Dot(ref Quaternion value1, ref Quaternion value2)
        {
            return
                value1.W * value2.W +
                value1.I * value2.I +
                value1.J * value2.J +
                value1.K * value2.K;
        }
    }
}
