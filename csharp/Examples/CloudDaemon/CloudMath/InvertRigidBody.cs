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
        /// Calculates the inverse of the specified matrix containing only rigid-body transformations.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix2"/>.</param>
        public static void InvertRigidBody(out Matrix2 result, ref Matrix2 value)
        {
            float m11 = value.M11;
            float m12 = value.M21;
            float m21 = value.M12;
            float m22 = value.M22;

            result.M11 = m11;
            result.M12 = m12;
            result.M21 = m21;
            result.M22 = m22;
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix containing only rigid-body transformations.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix3"/>.</param>
        public static void InvertRigidBody(out Matrix3 result, ref Matrix3 value)
        {
            float m11 = value.M11;
            float m12 = value.M21;
            float m13 = value.M31;
            float m21 = value.M12;
            float m22 = value.M22;
            float m23 = value.M32;
            float m31 = value.M13;
            float m32 = value.M23;
            float m33 = value.M33;

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
        /// Calculates the inverse of the specified matrix containing only rigid-body transformations.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        public static void InvertRigidBody(out Matrix result, ref Matrix value)
        {
            float m11 = value.M11;
            float m12 = value.M21;
            float m13 = value.M31;
            float m14 = -(value.M11 * value.M14 + value.M21 * value.M24 + value.M31 * value.M34);
            float m21 = value.M12;
            float m22 = value.M22;
            float m23 = value.M32;
            float m24 = -(value.M12 * value.M14 + value.M22 * value.M24 + value.M32 * value.M34);
            float m31 = value.M13;
            float m32 = value.M23;
            float m33 = value.M33;
            float m34 = -(value.M13 * value.M14 + value.M23 * value.M24 + value.M33 * value.M34);

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M14 = m14;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M24 = m24;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
            result.M34 = m34;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }
    }
}
