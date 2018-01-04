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
        /// Normalizes the specified vector using approximated square root function.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector2"/>.</param>
        public static void FastNormalize(out Vector2 result, ref Vector2 value)
        {
            float inv = FastInvSqrt(
                value.X * value.X +
                value.Y * value.Y);
            
            result.X = value.X * inv;
            result.Y = value.Y * inv;
        }

        /// <summary>
        /// Normalizes the specified vector using approximated square root function.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector3"/>.</param>
        public static void FastNormalize(out Vector3 result, ref Vector3 value)
        {
            float inv = FastInvSqrt(
                value.X * value.X +
                value.Y * value.Y +
                value.Z * value.Z);
            
            result.X = value.X * inv;
            result.Y = value.Y * inv;
            result.Z = value.Z * inv;
        }

        /// <summary>
        /// Normalizes the specified vector using approximated square root function.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector4"/>.</param>
        public static void FastNormalize(out Vector4 result, ref Vector4 value)
        {
            float inv = FastInvSqrt(
                value.X * value.X +
                value.Y * value.Y +
                value.Z * value.Z +
                value.W * value.W);
            
            result.X = value.X * inv;
            result.Y = value.Y * inv;
            result.Z = value.Z * inv;
            result.W = value.W * inv;
        }

        /// <summary>
        /// Normalizes the specified quaternion using approximated square root function.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        public static void FastNormalize(out Quaternion result, ref Quaternion value)
        {
            float inv = FastInvSqrt(
                value.W * value.W +
                value.I * value.I +
                value.J * value.J +
                value.K * value.K);
            
            result.W = value.W * inv;
            result.I = value.I * inv;
            result.J = value.J * inv;
            result.K = value.K * inv;
        }

        /// <summary>
        /// Normalizes the specified ray using approximated square root function.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Ray"/>.</param>
        public static void FastNormalize(out Ray result, ref Ray value)
        {
            float inv = FastInvSqrt(
                value.Direction.X * value.Direction.X +
                value.Direction.Y * value.Direction.Y +
                value.Direction.Z * value.Direction.Z);
            
            result.Position = value.Position;
            result.Direction.X = value.Direction.X * inv;
            result.Direction.Y = value.Direction.Y * inv;
            result.Direction.Z = value.Direction.Z * inv;
        }

        /// <summary>
        /// Normalizes the specified plane using approximated square root function.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Plane"/>.</param>
        public static void FastNormalize(out Plane result, ref Plane value)
        {
            float inv = FastInvSqrt(
                value.Normal.X * value.Normal.X +
                value.Normal.Y * value.Normal.Y +
                value.Normal.Z * value.Normal.Z);

            result.Normal.X = value.Normal.X * inv;
            result.Normal.Y = value.Normal.Y * inv;
            result.Normal.Z = value.Normal.Z * inv;
            result.D = value.D * inv;
        }

        /// <summary>
        /// Normalizes the specified volume using approximated square root function.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Volume"/>.</param>
        public static void FastNormalize(out Volume result, ref Volume value)
        {
            FastNormalize(out result.ClipPlane1, ref value.ClipPlane1);
            FastNormalize(out result.ClipPlane2, ref value.ClipPlane2);
            FastNormalize(out result.ClipPlane3, ref value.ClipPlane3);
            FastNormalize(out result.ClipPlane4, ref value.ClipPlane4);
            FastNormalize(out result.ClipPlane5, ref value.ClipPlane5);
            FastNormalize(out result.ClipPlane6, ref value.ClipPlane6);
        }
    }
}
