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
        /// Creates a rotation quaternion from a rotation around the X axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateX(out Quaternion result, float angleInRadians)
        {
            angleInRadians *= 0.5f;
            float sin = (float)System.Math.Sin(angleInRadians);
            float cos = (float)System.Math.Cos(angleInRadians);

            result.W = cos;
            result.I = sin;
            result.J = 0;
            result.K = 0;
        }

        /// <summary>
        /// Rotates the specified quaternion around the X axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateX(out Quaternion result, ref Quaternion value, float angleInRadians)
        {
            angleInRadians *= 0.5f;
            float sin = (float)System.Math.Sin(angleInRadians);
            float cos = (float)System.Math.Cos(angleInRadians);

            float w = cos * value.W - sin * value.I;
            float i = cos * value.I + sin * value.W;
            float j = cos * value.J - sin * value.K;
            float k = cos * value.K + sin * value.J;

            result.W = w;
            result.I = i;
            result.J = j;
            result.K = k;
        }

        /// <summary>
        /// Creates a transformation matrix from a rotation around the X axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateX(out Matrix3 result, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            result.M11 = 1;
            result.M12 = 0;
            result.M13 = 0;
            result.M21 = 0;
            result.M22 = cos;
            result.M23 = -sin;
            result.M31 = 0;
            result.M32 = sin;
            result.M33 = cos;
        }

        /// <summary>
        /// Rotates the specified matrix around the X axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix3"/>.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateX(out Matrix3 result, ref Matrix3 value, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            float m21 = cos * value.M21 - sin * value.M31;
            float m22 = cos * value.M22 - sin * value.M32;
            float m23 = cos * value.M23 - sin * value.M33;
            float m31 = sin * value.M21 + cos * value.M31;
            float m32 = sin * value.M22 + cos * value.M32;
            float m33 = sin * value.M23 + cos * value.M33;

            result.M11 = value.M11;
            result.M12 = value.M12;
            result.M13 = value.M13;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
        }

        /// <summary>
        /// Creates a rotation transformation from a rotation around the X axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateX(out Matrix result, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            result.M11 = 1.0f;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = cos;
            result.M23 = sin;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = -sin;
            result.M33 = cos;
            result.M34 = 0.0f;
            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
            result.M44 = 1.0f;
        }

        /// <summary>
        /// Rotates the specified matrix around the X axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateX(out Matrix result, ref Matrix value, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            float m21 = cos * value.M21 - sin * value.M31;
            float m22 = cos * value.M22 - sin * value.M32;
            float m23 = cos * value.M23 - sin * value.M33;
            float m24 = cos * value.M24 - sin * value.M34;
            float m31 = sin * value.M21 + cos * value.M31;
            float m32 = sin * value.M22 + cos * value.M32;
            float m33 = sin * value.M23 + cos * value.M33;
            float m34 = sin * value.M24 + cos * value.M34;

            result.M11 = value.M11;
            result.M12 = value.M12;
            result.M13 = value.M13;
            result.M14 = value.M14;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M24 = m24;
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
