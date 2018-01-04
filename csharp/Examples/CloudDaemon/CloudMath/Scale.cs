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
        /// Scales the specified color.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Color3"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Color3 result, ref Color3 value, float amount)
        {
            result.R = value.R * amount;
            result.G = value.G * amount;
            result.B = value.B * amount;
        }

        /// <summary>
        /// Scales the specified color.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Color4"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Color4 result, ref Color4 value, float amount)
        {
            result.A = value.A * amount;
            result.R = value.R * amount;
            result.G = value.G * amount;
            result.B = value.B * amount;
        }

        /// <summary>
        /// Scales the specified vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector2"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Vector2 result, ref Vector2 value, float amount)
        {
            result.X = value.X * amount;
            result.Y = value.Y * amount;
        }

        /// <summary>
        /// Scales the specified vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector3"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Vector3 result, ref Vector3 value, float amount)
        {
            result.X = value.X * amount;
            result.Y = value.Y * amount;
            result.Z = value.Z * amount;
        }

        /// <summary>
        /// Scales the specified vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector4"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Vector4 result, ref Vector4 value, float amount)
        {
            result.X = value.X * amount;
            result.Y = value.Y * amount;
            result.Z = value.Z * amount;
            result.W = value.W * amount;
        }

        /// <summary>
        /// Scales the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Quaternion result, ref Quaternion value, float amount)
        {
            result.W = value.W * amount;
            result.I = value.I * amount;
            result.J = value.J * amount;
            result.K = value.K * amount;
        }

        /// <summary>
        /// Creates a transformation matrix from the specified scaling factor.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Matrix2 result, float amount)
        {
            result.M11 = amount;
            result.M12 = 0;
            result.M21 = 0;
            result.M22 = amount;
        }

        /// <summary>
        /// Scales the specified matrix with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix2"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Matrix2 result, ref Matrix2 value, float amount)
        {
            result.M11 = value.M11 * amount;
            result.M12 = value.M12 * amount;
            result.M21 = value.M21 * amount;
            result.M22 = value.M22 * amount;
        }

        /// <summary>
        /// Creates a transformation matrix from the the specified scaling vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector2"/>.</param>
        public static void Scale(out Matrix2 result, ref Vector2 value)
        {
            result.M11 = value.X;
            result.M12 = 0;
            result.M21 = 0;
            result.M22 = value.Y;
        }

        /// <summary>
        /// Scales the specified matrix with the specified vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix2"/>.</param>
        /// <param name="value2">A <see cref="Vector2"/>.</param>
        public static void Scale(out Matrix2 result, ref Matrix2 value1, ref Vector2 value2)
        {
            result.M11 = value1.M11 * value2.X;
            result.M12 = value1.M12 * value2.Y;
            result.M21 = value1.M21 * value2.X;
            result.M22 = value1.M22 * value2.Y;
        }

        /// <summary>
        /// Creates a transformation matrix from the specified scaling factor.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Matrix3 result, float amount)
        {
            result.M11 = amount;
            result.M12 = 0;
            result.M13 = 0;
            result.M21 = 0;
            result.M22 = amount;
            result.M23 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = amount;
        }

        /// <summary>
        /// Scales the specified matrix with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix3"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Matrix3 result, ref Matrix3 value, float amount)
        {
            result.M11 = value.M11 * amount;
            result.M12 = value.M12 * amount;
            result.M13 = value.M13 * amount;
            result.M21 = value.M21 * amount;
            result.M22 = value.M22 * amount;
            result.M23 = value.M23 * amount;
            result.M31 = value.M31 * amount;
            result.M32 = value.M32 * amount;
            result.M33 = value.M33 * amount;
        }

        /// <summary>
        /// Creates a transformation matrix from the the specified scaling vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector3"/>.</param>
        public static void Scale(out Matrix3 result, ref Vector3 value)
        {
            result.M11 = value.X;
            result.M12 = 0;
            result.M13 = 0;
            result.M21 = 0;
            result.M22 = value.Y;
            result.M23 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = value.Z;
        }

        /// <summary>
        /// Scales the specified matrix with the specified vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix3"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        public static void Scale(out Matrix3 result, ref Matrix3 value1, ref Vector3 value2)
        {
            result.M11 = value1.M11 * value2.X;
            result.M12 = value1.M12 * value2.Y;
            result.M13 = value1.M13 * value2.Z;
            result.M21 = value1.M21 * value2.X;
            result.M22 = value1.M22 * value2.Y;
            result.M23 = value1.M23 * value2.Z;
            result.M31 = value1.M31 * value2.X;
            result.M32 = value1.M32 * value2.Y;
            result.M33 = value1.M33 * value2.Z;
        }

        /// <summary>
        /// Creates a transformation matrix from the specified scaling factor.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Matrix result, float amount)
        {
            result.M11 = amount;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = amount;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = amount;
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Scales the specified matrix with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void Scale(out Matrix result, ref Matrix value, float amount)
        {
            result.M11 = value.M11 * amount;
            result.M12 = value.M12 * amount;
            result.M13 = value.M13 * amount;
            result.M14 = value.M14 * amount;
            result.M21 = value.M21 * amount;
            result.M22 = value.M22 * amount;
            result.M23 = value.M23 * amount;
            result.M24 = value.M24 * amount;
            result.M31 = value.M31 * amount;
            result.M32 = value.M32 * amount;
            result.M33 = value.M33 * amount;
            result.M34 = value.M34 * amount;
            result.M41 = value.M41 * amount;
            result.M42 = value.M42 * amount;
            result.M43 = value.M43 * amount;
            result.M44 = value.M44 * amount;
        }

        /// <summary>
        /// Creates a transformation matrix from the the specified scaling vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector3"/>.</param>
        public static void Scale(out Matrix result, ref Vector3 value)
        {
            result.M11 = value.X;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = value.Y;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = value.Z;
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Scales the specified matrix with the specified vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix4"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        public static void Scale(out Matrix result, ref Matrix value1, ref Vector3 value2)
        {
            result.M11 = value1.M11 * value2.X;
            result.M12 = value1.M12 * value2.Y;
            result.M13 = value1.M13 * value2.Z;
            result.M14 = value1.M14;
            result.M21 = value1.M21 * value2.X;
            result.M22 = value1.M22 * value2.Y;
            result.M23 = value1.M23 * value2.Z;
            result.M24 = value1.M24;
            result.M31 = value1.M31 * value2.X;
            result.M32 = value1.M32 * value2.Y;
            result.M33 = value1.M33 * value2.Z;
            result.M34 = value1.M34;
            result.M41 = value1.M41 * value2.X;
            result.M42 = value1.M42 * value2.Y;
            result.M43 = value1.M43 * value2.Z;
            result.M44 = value1.M44;
        }

        /// <summary>
        /// Creates a transformation matrix from the the specified scaling vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector4"/>.</param>
        public static void Scale(out Matrix result, ref Vector4 value)
        {
            result.M11 = value.X;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = value.Y;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = value.Z;
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = value.W;
        }

        /// <summary>
        /// Scales the specified matrix with the specified vector.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        public static void Scale(out Matrix result, ref Matrix value1, ref Vector4 value2)
        {
            result.M11 = value1.M11 * value2.X;
            result.M12 = value1.M12 * value2.Y;
            result.M13 = value1.M13 * value2.Z;
            result.M14 = value1.M14 * value2.W;
            result.M21 = value1.M21 * value2.X;
            result.M22 = value1.M22 * value2.Y;
            result.M23 = value1.M23 * value2.Z;
            result.M24 = value1.M24 * value2.W;
            result.M31 = value1.M31 * value2.X;
            result.M32 = value1.M32 * value2.Y;
            result.M33 = value1.M33 * value2.Z;
            result.M34 = value1.M34 * value2.W;
            result.M41 = value1.M41 * value2.X;
            result.M42 = value1.M42 * value2.Y;
            result.M43 = value1.M43 * value2.Z;
            result.M44 = value1.M44 * value2.W;
        }
    }
}
