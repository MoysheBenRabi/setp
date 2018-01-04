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
        /// Calculates the inverse of the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        public static void Invert(out Quaternion result, ref Quaternion value)
        {
            result.W = value.W;
            result.I = -value.I;
            result.J = -value.J;
            result.K = -value.K;
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix2"/>.</param>
        public static void Invert(out Matrix2 result, ref Matrix2 value)
        {
            float m11 = value.M11;
            float m12 = value.M12;
            float m21 = value.M21;
            float m22 = value.M22;

            float inv_det = 1 / (m11 * m22 - m12 * m21);

            result.M11 = m22 * inv_det;
            result.M12 = -m12 * inv_det;
            result.M21 = -m21 * inv_det;
            result.M22 = m11 * inv_det;
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix3"/>.</param>
        public static void Invert(out Matrix3 result, ref Matrix3 value)
        {
            float m11 = value.M11;
            float m12 = value.M12;
            float m13 = value.M13;
            float m21 = value.M21;
            float m22 = value.M22;
            float m23 = value.M23;
            float m31 = value.M31;
            float m32 = value.M32;
            float m33 = value.M33;

            result.M11 = m22 * m33 - m23 * m32;
            result.M21 = m23 * m31 - m21 * m33;
            result.M31 = m21 * m32 - m22 * m31;

            float inv_det = 1 / (m11 * result.M11 + m12 * result.M21 + m13 * result.M31);

            result.M11 *= inv_det;
            result.M21 *= inv_det;
            result.M31 *= inv_det;

            result.M12 = (m32 * m13 - m12 * m33) * inv_det;
            result.M13 = (m12 * m23 - m22 * m13) * inv_det;
            result.M22 = (m11 * m33 - m31 * m13) * inv_det;
            result.M23 = (m21 * m13 - m11 * m23) * inv_det;
            result.M32 = (m31 * m12 - m11 * m32) * inv_det;
            result.M33 = (m11 * m22 - m12 * m21) * inv_det;
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        public static void Invert(out Matrix result, ref Matrix value)
        {
            float inv_det, d01, d02, d12, d13, d23, d30;

            float m11 = value.M11;
            float m12 = value.M12;
            float m13 = value.M13;
            float m14 = value.M14;
            float m21 = value.M21;
            float m22 = value.M22;
            float m23 = value.M23;
            float m24 = value.M24;
            float m31 = value.M31;
            float m32 = value.M32;
            float m33 = value.M33;
            float m34 = value.M34;
            float m41 = value.M41;
            float m42 = value.M42;
            float m43 = value.M43;
            float m44 = value.M44;

            d01 = m13 * m24 - m14 * m23;
            d02 = m13 * m34 - m14 * m33;
            d12 = m23 * m34 - m24 * m33;
            d13 = m23 * m44 - m24 * m43;
            d23 = m33 * m44 - m34 * m43;
            d30 = m43 * m14 - m44 * m13;

            result.M11 = (m22 * d23 - m32 * d13 + m42 * d12);
            result.M12 = -(m12 * d23 + m32 * d30 + m42 * d02);
            result.M13 = (m12 * d13 + m22 * d30 + m42 * d01);
            result.M14 = -(m12 * d12 - m22 * d02 + m32 * d01);

            inv_det = 1 / (m11 * result.M11 + m21 * result.M12 + m31 * result.M13 + m41 * result.M14);

            result.M11 *= inv_det;
            result.M12 *= inv_det;
            result.M13 *= inv_det;
            result.M14 *= inv_det;

            result.M21 = -(m21 * d23 - m31 * d13 + m41 * d12) * inv_det;
            result.M22 = (m11 * d23 + m31 * d30 + m41 * d02) * inv_det;
            result.M23 = -(m11 * d13 + m21 * d30 + m41 * d01) * inv_det;
            result.M24 = (m11 * d12 - m21 * d02 + m31 * d01) * inv_det;

            d01 = m11 * m22 - m12 * m21;
            d02 = m11 * m32 - m12 * m31;
            d12 = m21 * m32 - m22 * m31;
            d13 = m21 * m42 - m22 * m41;
            d23 = m31 * m42 - m32 * m41;
            d30 = m41 * m12 - m42 * m11;

            result.M31 = (m24 * d23 - m34 * d13 + m44 * d12) * inv_det;
            result.M32 = -(m14 * d23 + m34 * d30 + m44 * d02) * inv_det;
            result.M33 = (m14 * d13 + m24 * d30 + m44 * d01) * inv_det;
            result.M34 = -(m14 * d12 - m24 * d02 + m34 * d01) * inv_det;
            result.M41 = -(m23 * d23 - m33 * d13 + m43 * d12) * inv_det;
            result.M42 = (m13 * d23 + m33 * d30 + m43 * d02) * inv_det;
            result.M43 = -(m13 * d13 + m23 * d30 + m43 * d01) * inv_det;
            result.M44 = (m13 * d12 - m23 * d02 + m33 * d01) * inv_det;
        }
    }
}
