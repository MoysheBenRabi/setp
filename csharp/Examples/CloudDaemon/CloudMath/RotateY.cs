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
        /// Creates a rotation quaternion from a rotation around the Y axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateY(out Quaternion result, float angleInRadians)
        {
            angleInRadians *= 0.5f;
            float sin = (float)System.Math.Sin(angleInRadians);
            float cos = (float)System.Math.Cos(angleInRadians);

            result.W = cos;
            result.I = 0;
            result.J = sin;
            result.K = 0;
        }

        /// <summary>
        /// Rotates the specified quaternion around the Y axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateY(out Quaternion result, ref Quaternion value, float angleInRadians)
        {
            angleInRadians *= 0.5f;
            float sin = (float)System.Math.Sin(angleInRadians);
            float cos = (float)System.Math.Cos(angleInRadians);

            float w = cos * value.W - sin * value.J;
            float i = cos * value.I + sin * value.K;
            float j = cos * value.J + sin * value.W;
            float k = cos * value.K - sin * value.I;

            result.W = w;
            result.I = i;
            result.J = j;
            result.K = k;
        }

        /// <summary>
        /// Creates a transformation matrix from a rotation around the Y axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateY(out Matrix3 result, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            result.M11 = cos;
            result.M12 = 0;
            result.M13 = sin;
            result.M21 = 0;
            result.M22 = 1;
            result.M23 = 0;
            result.M31 = -sin;
            result.M32 = 0;
            result.M33 = cos;
        }

        /// <summary>
        /// Rotates the specified matrix around the Y axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix3"/>.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateY(out Matrix3 result, ref Matrix3 value, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            float m11 = cos * value.M11 + sin * value.M31;
            float m12 = cos * value.M12 + sin * value.M32;
            float m13 = cos * value.M13 + sin * value.M33;
            float m31 = cos * value.M31 - sin * value.M11;
            float m32 = cos * value.M32 - sin * value.M12;
            float m33 = cos * value.M33 - sin * value.M13;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M21 = value.M21;
            result.M22 = value.M22;
            result.M23 = value.M23;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
        }

        /// <summary>
        /// Creates a rotation transformation from a rotation around the Y axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateY(out Matrix result, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            result.M11 = cos;
            result.M12 = 0.0f;
            result.M13 = -sin;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = 1.0f;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = sin;
            result.M32 = 0.0f;
            result.M33 = cos;
            result.M34 = 0.0f;
            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
            result.M44 = 1.0f;
        }

        /// <summary>
        /// Rotates the specified matrix around the Y axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateY(out Matrix result, ref Matrix value, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            float m11 = cos * value.M11 + sin * value.M31;
            float m12 = cos * value.M12 + sin * value.M32;
            float m13 = cos * value.M13 + sin * value.M33;
            float m14 = cos * value.M14 + sin * value.M34;
            float m31 = cos * value.M31 - sin * value.M11;
            float m32 = cos * value.M32 - sin * value.M12;
            float m33 = cos * value.M33 - sin * value.M13;
            float m34 = cos * value.M34 - sin * value.M14;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M14 = m14;
            result.M21 = value.M21;
            result.M22 = value.M22;
            result.M23 = value.M23;
            result.M24 = value.M24;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
            result.M34 = m34;
            result.M41 = value.M41;
            result.M42 = value.M42;
            result.M43 = value.M43;
            result.M44 = value.M44;
        }
    }
}
