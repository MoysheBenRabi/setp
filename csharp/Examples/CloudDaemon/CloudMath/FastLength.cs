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
        /// Calculates the length of the specified vector using approximated square root function.
        /// </summary>
        /// <param name="value">A <see cref="Vector2"/>.</param>
        /// <returns>Approximated length of the vector.</returns>
        public static float FastLength(ref Vector2 value)
        {
            return FastSqrt(
                value.X * value.X +
                value.Y * value.Y);
        }

        /// <summary>
        /// Calculates the length of the specified vector using approximated square root function.
        /// </summary>
        /// <param name="value">A <see cref="Vector3"/>.</param>
        /// <returns>Approximated length of the vector.</returns>
        public static float FastLength(ref Vector3 value)
        {
            return FastSqrt(
                value.X * value.X +
                value.Y * value.Y +
                value.Z * value.Z);
        }

        /// <summary>
        /// Calculates the length of the specified vector using approximated square root function.
        /// </summary>
        /// <param name="value">A <see cref="Vector4"/>.</param>
        /// <returns>Approximated length of the vector.</returns>
        public static float FastLength(ref Vector4 value)
        {
            return FastSqrt(
                value.X * value.X +
                value.Y * value.Y +
                value.Z * value.Z +
                value.W * value.W);
        }

        /// <summary>
        /// Calculates the length of the specified quaternion using approximated square root function.
        /// </summary>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        /// <returns>Approximated length of the quaternion.</returns>
        public static float FastLength(ref Quaternion value)
        {
            return FastSqrt(
                value.W * value.W +
                value.I * value.I +
                value.J * value.J +
                value.K * value.K);
        }
    }
}
