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
        /// Calculates the product of the specified quaternions.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Multiply(out Quaternion result, ref Quaternion value1, ref Quaternion value2)
        {
            float A = (value1.W + value1.I) * (value2.W + value2.I);
            float B = (value1.K - value1.J) * (value2.J - value2.K);
            float C = (value1.W - value1.I) * (value2.J + value2.K);
            float D = (value1.J + value1.K) * (value2.W - value2.I);
            float E = (value1.I + value1.K) * (value2.I + value2.J);
            float F = (value1.I - value1.K) * (value2.I - value2.J);
            float G = (value1.W + value1.J) * (value2.W - value2.K);
            float H = (value1.W - value1.J) * (value2.W + value2.K);

            result.W = B - (E + F - G - H) * 0.5f;
            result.I = A - (E + F + G + H) * 0.5f;
            result.J = C + (E - F + G - H) * 0.5f;
            result.K = D + (E - F - G + H) * 0.5f;
        }

        /// <summary>
        /// Calculates the product of the specified matrices.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix2"/>.</param>
        /// <param name="value2">A <see cref="Matrix2"/>.</param>
        public static void Multiply(out Matrix2 result, ref Matrix2 value1, ref Matrix2 value2)
        {
            float m11 = value1.M11 * value2.M11 + value1.M12 * value2.M21;
            float m12 = value1.M11 * value2.M12 + value1.M12 * value2.M22;
            float m21 = value1.M21 * value2.M11 + value1.M22 * value2.M21;
            float m22 = value1.M21 * value2.M12 + value1.M22 * value2.M22;

            result.M11 = m11;
            result.M12 = m12;
            result.M21 = m21;
            result.M22 = m22;
        }

        /// <summary>
        /// Calculates the product of the specified matrices.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix3"/>.</param>
        /// <param name="value2">A <see cref="Matrix3"/>.</param>
        public static void Multiply(out Matrix3 result, ref Matrix3 value1, ref Matrix3 value2)
        {
            float m11 = value2.M11 * value1.M11 + value2.M12 * value1.M21 + value2.M13 * value1.M31;
            float m12 = value2.M11 * value1.M12 + value2.M12 * value1.M22 + value2.M13 * value1.M32;
            float m13 = value2.M11 * value1.M13 + value2.M12 * value1.M23 + value2.M13 * value1.M33;
            float m21 = value2.M21 * value1.M11 + value2.M22 * value1.M21 + value2.M23 * value1.M31;
            float m22 = value2.M21 * value1.M12 + value2.M22 * value1.M22 + value2.M23 * value1.M32;
            float m23 = value2.M21 * value1.M13 + value2.M22 * value1.M23 + value2.M23 * value1.M33;
            float m31 = value2.M31 * value1.M11 + value2.M32 * value1.M21 + value2.M33 * value1.M31;
            float m32 = value2.M31 * value1.M12 + value2.M32 * value1.M22 + value2.M33 * value1.M32;
            float m33 = value2.M31 * value1.M13 + value2.M32 * value1.M23 + value2.M33 * value1.M33;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
        }

        /// <summary>
        /// Calculates the product of the specified matrices.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix4"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Multiply(out Matrix result, ref Matrix left, ref Matrix right)
        {
            result.M11 = (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41);
            result.M12 = (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42);
            result.M13 = (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43);
            result.M14 = (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44);
            result.M21 = (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41);
            result.M22 = (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42);
            result.M23 = (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43);
            result.M24 = (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44);
            result.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41);
            result.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42);
            result.M33 = (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43);
            result.M34 = (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44);
            result.M41 = (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41);
            result.M42 = (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42);
            result.M43 = (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43);
            result.M44 = (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44);

            //float m11 = value2.M11 * value1.M11 + value2.M12 * value1.M21 + value2.M13 * value1.M31 + value2.M14 * value1.M41;
            //float m12 = value2.M11 * value1.M12 + value2.M12 * value1.M22 + value2.M13 * value1.M32 + value2.M14 * value1.M42;
            //float m13 = value2.M11 * value1.M13 + value2.M12 * value1.M23 + value2.M13 * value1.M33 + value2.M14 * value1.M43;
            //float m14 = value2.M11 * value1.M14 + value2.M12 * value1.M24 + value2.M13 * value1.M34 + value2.M14 * value1.M44;
            //float m21 = value2.M21 * value1.M11 + value2.M22 * value1.M21 + value2.M23 * value1.M31 + value2.M24 * value1.M41;
            //float m22 = value2.M21 * value1.M12 + value2.M22 * value1.M22 + value2.M23 * value1.M32 + value2.M24 * value1.M42;
            //float m23 = value2.M21 * value1.M13 + value2.M22 * value1.M23 + value2.M23 * value1.M33 + value2.M24 * value1.M43;
            //float m24 = value2.M21 * value1.M14 + value2.M22 * value1.M24 + value2.M23 * value1.M34 + value2.M24 * value1.M44;
            //float m31 = value2.M31 * value1.M11 + value2.M32 * value1.M21 + value2.M33 * value1.M31 + value2.M34 * value1.M41;
            //float m32 = value2.M31 * value1.M12 + value2.M32 * value1.M22 + value2.M33 * value1.M32 + value2.M34 * value1.M42;
            //float m33 = value2.M31 * value1.M13 + value2.M32 * value1.M23 + value2.M33 * value1.M33 + value2.M34 * value1.M43;
            //float m34 = value2.M31 * value1.M14 + value2.M32 * value1.M24 + value2.M33 * value1.M34 + value2.M34 * value1.M44;
            //float m41 = value2.M41 * value1.M11 + value2.M42 * value1.M21 + value2.M43 * value1.M31 + value2.M44 * value1.M41;
            //float m42 = value2.M41 * value1.M12 + value2.M42 * value1.M22 + value2.M43 * value1.M32 + value2.M44 * value1.M42;
            //float m43 = value2.M41 * value1.M13 + value2.M42 * value1.M23 + value2.M43 * value1.M33 + value2.M44 * value1.M43;
            //float m44 = value2.M41 * value1.M14 + value2.M42 * value1.M24 + value2.M43 * value1.M34 + value2.M44 * value1.M44;

            //result.M11 = m11;
            //result.M12 = m12;
            //result.M13 = m13;
            //result.M14 = m14;
            //result.M21 = m21;
            //result.M22 = m22;
            //result.M23 = m23;
            //result.M24 = m24;
            //result.M31 = m31;
            //result.M32 = m32;
            //result.M33 = m33;
            //result.M34 = m34;
            //result.M41 = m41;
            //result.M42 = m42;
            //result.M43 = m43;
            //result.M44 = m44;
        }
    }
}
