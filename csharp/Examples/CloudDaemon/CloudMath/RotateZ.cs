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
        /// Creates a rotation quaternion from a rotation around the Z axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateZ(out Quaternion result, float angleInRadians)
        {
            angleInRadians *= 0.5f;
            float sin = (float)System.Math.Sin(angleInRadians);
            float cos = (float)System.Math.Cos(angleInRadians);

            result.W = cos;
            result.I = 0;
            result.J = 0;
            result.K = sin;
        }

        /// <summary>
        /// Rotates the specified quaternion around the Z axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateZ(out Quaternion result, ref Quaternion value, float angleInRadians)
        {
            angleInRadians *= 0.5f;
            float sin = (float)System.Math.Sin(angleInRadians);
            float cos = (float)System.Math.Cos(angleInRadians);

            float w = cos * value.W - sin * value.K;
            float i = cos * value.I - sin * value.J;
            float j = cos * value.J + sin * value.I;
            float k = cos * value.K + sin * value.W;

            result.W = w;
            result.I = i;
            result.J = j;
            result.K = k;
        }

        /// <summary>
        /// Creates a transformation matrix from a rotation around the Z axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateZ(out Matrix2 result, float angleInRadians)
        {
            float sin = (float)System.Math.Sin(angleInRadians);
            float cos = (float)System.Math.Cos(angleInRadians);

            result.M11 = cos;
            result.M12 = -sin;
            result.M21 = sin;
            result.M22 = cos;
        }

        /// <summary>
        /// Rotates the specified matrix around the Z axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix2"/>.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateZ(out Matrix2 result, ref Matrix2 value, float angleInRadians)
        {
            float sin = (float)System.Math.Sin(angleInRadians);
            float cos = (float)System.Math.Cos(angleInRadians);

            float m11 = cos * value.M11 - sin * value.M21;
            float m12 = cos * value.M12 - sin * value.M22;
            float m21 = sin * value.M11 + cos * value.M21;
            float m22 = sin * value.M12 + cos * value.M22;

            result.M11 = m11;
            result.M12 = m12;
            result.M21 = m21;
            result.M22 = m22;
        }

        /// <summary>
        /// Creates a transformation matrix from a rotation around the Z axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateZ(out Matrix3 result, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            result.M11 = cos;
            result.M12 = -sin;
            result.M13 = 0;
            result.M21 = sin;
            result.M22 = cos;
            result.M23 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = 1;
        }

        /// <summary>
        /// Rotates the specified matrix around the Z axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix3"/>.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateZ(out Matrix3 result, ref Matrix3 value, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            float m11 = cos * value.M11 - sin * value.M21;
            float m12 = cos * value.M12 - sin * value.M22;
            float m13 = cos * value.M13 - sin * value.M23;
            float m21 = sin * value.M11 + cos * value.M21;
            float m22 = sin * value.M12 + cos * value.M22;
            float m23 = sin * value.M13 + cos * value.M23;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M31 = value.M31;
            result.M32 = value.M32;
            result.M33 = value.M33;
        }

        /// <summary>
        /// Creates a transformation matrix from a rotation around the Z axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateZ(out Matrix result, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            result.M11 = cos;
            result.M12 = sin;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = -sin;
            result.M22 = cos;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = 1.0f;
            result.M34 = 0.0f;
            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
            result.M44 = 1.0f;
        }

        /// <summary>
        /// Rotates the specified matrix around the Z axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void RotateZ(out Matrix result, ref Matrix value, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            float m11 = cos * value.M11 - sin * value.M21;
            float m12 = cos * value.M12 - sin * value.M22;
            float m13 = cos * value.M13 - sin * value.M23;
            float m14 = cos * value.M14 - sin * value.M24;
            float m21 = sin * value.M11 + cos * value.M21;
            float m22 = sin * value.M12 + cos * value.M22;
            float m23 = sin * value.M13 + cos * value.M23;
            float m24 = sin * value.M14 + cos * value.M24;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M14 = m14;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M24 = m24;
            result.M31 = value.M31;
            result.M32 = value.M32;
            result.M33 = value.M33;
            result.M34 = value.M34;
            result.M41 = value.M41;
            result.M42 = value.M42;
            result.M43 = value.M43;
            result.M44 = value.M44;
        }
    }
}
