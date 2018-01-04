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
        /// Calculates the sum of the specified colors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color3"/>.</param>
        /// <param name="value2">A <see cref="Color3"/>.</param>
        public static void Add(out Color3 result, ref Color3 value1, ref Color3 value2)
        {
            result.R = value1.R + value2.R;
            result.G = value1.G + value2.G;
            result.B = value1.B + value2.B;
        }
        
        /// <summary>
        /// Calculates the sum of the specified colors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color4"/>.</param>
        /// <param name="value2">A <see cref="Color4"/>.</param>
        public static void Add(out Color4 result, ref Color4 value1, ref Color4 value2)
        {
            result.A = value1.A + value2.A;
            result.R = value1.R + value2.R;
            result.G = value1.G + value2.G;
            result.B = value1.B + value2.B;
        }

        /// <summary>
        /// Calculates the sum of the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Vector2"/>.</param>
        public static void Add(out Vector2 result, ref Vector2 value1, ref Vector2 value2)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        /// <summary>
        /// Calculates the sum of the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        public static void Add(out Vector3 result, ref Vector3 value1, ref Vector3 value2)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }

        /// <summary>
        /// Calculates the sum of the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        public static void Add(out Vector4 result, ref Vector4 value1, ref Vector4 value2)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            result.W = value1.W + value2.W;
        }

        /// <summary>
        /// Calculates the sum of the specified quaternions.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Add(out Quaternion result, ref Quaternion value1, ref Quaternion value2)
        {
            result.W = value1.W + value2.W;
            result.I = value1.I + value2.I;
            result.J = value1.J + value2.J;
            result.K = value1.K + value2.K;
        }

        /// <summary>
        /// Calculates the sum of the specified matrices.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix2"/>.</param>
        /// <param name="value2">A <see cref="Matrix2"/>.</param>
        public static void Add(out Matrix2 result, ref Matrix2 value1, ref Matrix2 value2)
        {
            result.M11 = value1.M11 + value2.M11;
            result.M12 = value1.M12 + value2.M12;
            result.M21 = value1.M21 + value2.M21;
            result.M22 = value1.M22 + value2.M22;
        }

        /// <summary>
        /// Calculates the sum of the specified matrices.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix3"/>.</param>
        /// <param name="value2">A <see cref="Matrix3"/>.</param>
        public static void Add(out Matrix3 result, ref Matrix3 value1, ref Matrix3 value2)
        {
            result.M11 = value1.M11 + value2.M11;
            result.M12 = value1.M12 + value2.M12;
            result.M13 = value1.M13 + value2.M13;
            result.M21 = value1.M21 + value2.M21;
            result.M22 = value1.M22 + value2.M22;
            result.M23 = value1.M23 + value2.M23;
            result.M31 = value1.M31 + value2.M31;
            result.M32 = value1.M32 + value2.M32;
            result.M33 = value1.M33 + value2.M33;
        }

        /// <summary>
        /// Calculates the sum of the specified matrices.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix4"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Add(out Matrix result, ref Matrix value1, ref Matrix value2)
        {
            result.M11 = value1.M11 + value2.M11;
            result.M12 = value1.M12 + value2.M12;
            result.M13 = value1.M13 + value2.M13;
            result.M14 = value1.M14 + value2.M14;
            result.M21 = value1.M21 + value2.M21;
            result.M22 = value1.M22 + value2.M22;
            result.M23 = value1.M23 + value2.M23;
            result.M24 = value1.M24 + value2.M24;
            result.M31 = value1.M31 + value2.M31;
            result.M32 = value1.M32 + value2.M32;
            result.M33 = value1.M33 + value2.M33;
            result.M34 = value1.M34 + value2.M34;
            result.M41 = value1.M41 + value2.M41;
            result.M42 = value1.M42 + value2.M42;
            result.M43 = value1.M43 + value2.M43;
            result.M44 = value1.M44 + value2.M44;
        }
    }
}
