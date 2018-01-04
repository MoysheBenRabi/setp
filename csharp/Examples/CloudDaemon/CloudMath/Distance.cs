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
        /// Calculates the distance between the specified vectors.
        /// </summary>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Vector2"/>.</param>
        /// <returns>Distance between the specified vectors.</returns>
        public static float Distance(ref Vector2 value1, ref Vector2 value2)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;

            return (float)System.Math.Sqrt(
                x * x +
                y * y);
        }

        /// <summary>
        /// Calculates the distance between the specified vectors.
        /// </summary>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <returns>Distance between the specified vectors.</returns>
        public static float Distance(ref Vector3 value1, ref Vector3 value2)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            float z = value1.Z - value2.Z;

            return (float)System.Math.Sqrt(
                x * x +
                y * y +
                z * z);
        }

        /// <summary>
        /// Calculates the distance between the specified vectors.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>Distance between the specified vectors.</returns>
        public static float Distance(ref Vector4 value1, ref Vector4 value2)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            float z = value1.Z - value2.Z;
            float w = value1.W - value2.W;

            return (float)System.Math.Sqrt(
                x * x +
                y * y +
                z * z +
                w * w);
        }

        /// <summary>
        /// Calculates the distance between the specified quaternions.
        /// </summary>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        /// <returns>Distance between the specified quaternions.</returns>
        public static float Distance(ref Quaternion value1, ref Quaternion value2)
        {
            float w = value1.W - value2.W;
            float i = value1.I - value2.I;
            float j = value1.J - value2.J;
            float k = value1.K - value2.K;

            return (float)System.Math.Sqrt(
                w * w +
                i * i +
                j * j +
                k * k);
        }

        /// <summary>
        /// Calculates the distance between the specified ray and the specified point.
        /// </summary>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <returns>Distance bethween the specified ray and the specified point.</returns>
        public static float Distance(ref Ray value1, ref Vector3 value2)
        {
            float x = value2.X - value1.Position.X;
            float y = value2.Y - value1.Position.Y;
            float z = value2.Z - value1.Position.Z;

            float dot =
                value1.Direction.X * x +
                value1.Direction.Y * y +
                value1.Direction.Z * z;

            if (dot > 0)
            {
                x -= value1.Direction.X * dot;
                y -= value1.Direction.Y * dot;
                z -= value1.Direction.Z * dot;
            }

            return (float)System.Math.Sqrt(
                x * x +
                y * y +
                z * z);
        }

        /// <summary>
        /// Calculates the distance between the specified plane and the specified point.
        /// </summary>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <returns>Distance between the plane and the point.</returns>
        public static float Distance(ref Plane value1, ref Vector3 value2)
        {
            return
                value1.Normal.X * value2.X +
                value1.Normal.Y * value2.Y +
                value1.Normal.Z * value2.Z +
                value1.D;
        }
    }
}
