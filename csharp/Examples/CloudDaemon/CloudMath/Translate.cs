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
        /// Creates a transformation matrix from the specified amounts.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="x">Translation along the X axis.</param>
        /// <param name="y">Translation along the Y axis.</param>
        /// <param name="z">Translation along the Z axis.</param>
        public static void Translate(out Matrix result, float x, float y, float z)
        {
            result.M11 = 1.0f;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = 1.0f;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = 1.0f;
            result.M34 = 0.0f;
            result.M41 = x;
            result.M42 = y;
            result.M43 = z;
            result.M44 = 1.0f;
            //result.M11 = 1;
            //result.M12 = 0;
            //result.M13 = 0;
            //result.M14 = x;
            //result.M21 = 0;
            //result.M22 = 1;
            //result.M23 = 0;
            //result.M24 = y;
            //result.M31 = 0;
            //result.M32 = 0;
            //result.M33 = 1;
            //result.M34 = z;
            //result.M41 = 0;
            //result.M42 = 0;
            //result.M43 = 0;
            //result.M44 = 1;
        }

        /// <summary>
        /// Translates the specified matrix with the specified amounts.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        /// <param name="x">Translation along the X axis.</param>
        /// <param name="y">Translation along the Y axis.</param>
        /// <param name="z">Translation along the Z axis.</param>
        public static void Translate(out Matrix result, ref Matrix value, float x, float y, float z)
        {
            result.M11 = value.M11 + x * value.M41;
            result.M12 = value.M12 + x * value.M42;
            result.M13 = value.M13 + x * value.M43;
            result.M14 = value.M14 + x * value.M44;
            result.M21 = value.M21 + y * value.M41;
            result.M22 = value.M22 + y * value.M42;
            result.M23 = value.M23 + y * value.M43;
            result.M24 = value.M24 + y * value.M44;
            result.M31 = value.M31 + z * value.M41;
            result.M32 = value.M32 + z * value.M42;
            result.M33 = value.M33 + z * value.M43;
            result.M34 = value.M34 + z * value.M44;
            result.M41 = value.M41;
            result.M42 = value.M42;
            result.M43 = value.M43;
            result.M44 = value.M44;
        }

        /// <summary>
        /// Creates a transformation matrix from the specified translation vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector3"/>.</param>
        public static void Translate(out Matrix result, ref Vector3 value)
        {
            result.M11 = 1;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = value.X;
            result.M21 = 0;
            result.M22 = 1;
            result.M23 = 0;
            result.M24 = value.Y;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = 1;
            result.M34 = value.Z;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Translates the specified matrix with the specified translation vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix4"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        public static void Translate(out Matrix result, ref Matrix value1, ref Vector3 value2)
        {
            result.M11 = value1.M11 + value2.X * value1.M41;
            result.M12 = value1.M12 + value2.X * value1.M42;
            result.M13 = value1.M13 + value2.X * value1.M43;
            result.M14 = value1.M14 + value2.X * value1.M44;
            result.M21 = value1.M21 + value2.Y * value1.M41;
            result.M22 = value1.M22 + value2.Y * value1.M42;
            result.M23 = value1.M23 + value2.Y * value1.M43;
            result.M24 = value1.M24 + value2.Y * value1.M44;
            result.M31 = value1.M31 + value2.Z * value1.M41;
            result.M32 = value1.M32 + value2.Z * value1.M42;
            result.M33 = value1.M33 + value2.Z * value1.M43;
            result.M34 = value1.M34 + value2.Z * value1.M44;
            result.M41 = value1.M41;
            result.M42 = value1.M42;
            result.M43 = value1.M43;
            result.M44 = value1.M44;
        }
    }
}
