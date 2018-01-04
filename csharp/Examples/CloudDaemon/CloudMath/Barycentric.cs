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
        /// Performs a linear interpolation between the specified colors using barycentric coordinates.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color3"/>.</param>
        /// <param name="value2">A <see cref="Color3"/>.</param>
        /// <param name="value3">A <see cref="Color3"/>.</param>
        /// <param name="u">Barycentric coordinate u, expressing weight towards <paramref name="value2"/>.</param>
        /// <param name="v">Barycentric coordinate v, expressing weight towards <paramref name="value3"/>.</param>
        public static void Barycentric(out Color3 result, ref Color3 value1, ref Color3 value2, ref Color3 value3, float u, float v)
        {
            result.R = value1.R + u * (value2.R - value1.R) + v * (value3.R - value1.R);
            result.G = value1.G + u * (value2.G - value1.G) + v * (value3.G - value1.G);
            result.B = value1.B + u * (value2.B - value1.B) + v * (value3.B - value1.B);
        }
        
        /// <summary>
        /// Performs a linear interpolation between the specified colors using barycentric coordinates.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color4"/>.</param>
        /// <param name="value2">A <see cref="Color4"/>.</param>
        /// <param name="value3">A <see cref="Color4"/>.</param>
        /// <param name="u">Barycentric coordinate u, expressing weight towards <paramref name="value2"/>.</param>
        /// <param name="v">Barycentric coordinate v, expressing weight towards <paramref name="value3"/>.</param>
        public static void Barycentric(out Color4 result, ref Color4 value1, ref Color4 value2, ref Color4 value3, float u, float v)
        {
            result.A = value1.A + u * (value2.A - value1.A) + v * (value3.A - value1.A);
            result.R = value1.R + u * (value2.R - value1.R) + v * (value3.R - value1.R);
            result.G = value1.G + u * (value2.G - value1.G) + v * (value3.G - value1.G);
            result.B = value1.B + u * (value2.B - value1.B) + v * (value3.B - value1.B);
        }

        /// <summary>
        /// Performs a linear interpolation between the specified values using barycentric coordinates.
        /// </summary>
        /// <param name="value1">A <see cref="Single"/>.</param>
        /// <param name="value2">A <see cref="Single"/>.</param>
        /// <param name="value3">A <see cref="Single"/>.</param>
        /// <param name="u">Barycentric coordinate u, expressing weight towards <paramref name="value2"/>.</param>
        /// <param name="v">Barycentric coordinate v, expressing weight towards <paramref name="value3"/>.</param>
        /// <returns>Interpolated value.</returns>
        public static float Barycentric(float value1, float value2, float value3, float u, float v)
        {
            return value1 + u * (value2 - value1) + v * (value3 - value1);
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors using barycentric coordinates.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Vector2"/>.</param>
        /// <param name="value3">A <see cref="Vector2"/>.</param>
        /// <param name="u">Barycentric coordinate u, expressing weight towards <paramref name="value2"/>.</param>
        /// <param name="v">Barycentric coordinate v, expressing weight towards <paramref name="value3"/>.</param>
        public static void Barycentric(out Vector2 result, ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float u, float v)
        {
            result.X = value1.X + u * (value2.X - value1.X) + v * (value3.X - value1.X);
            result.Y = value1.Y + u * (value2.Y - value1.Y) + v * (value3.Y - value1.Y);
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors using barycentric coordinates.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <param name="value3">A <see cref="Vector3"/>.</param>
        /// <param name="u">Barycentric coordinate u, expressing weight towards <paramref name="value2"/>.</param>
        /// <param name="v">Barycentric coordinate v, expressing weight towards <paramref name="value3"/>.</param>
        public static void Barycentric(out Vector3 result, ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float u, float v)
        {
            result.X = value1.X + u * (value2.X - value1.X) + v * (value3.X - value1.X);
            result.Y = value1.Y + u * (value2.Y - value1.Y) + v * (value3.Y - value1.Y);
            result.Z = value1.Z + u * (value2.Z - value1.Z) + v * (value3.Z - value1.Z);
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors using barycentric coordinates.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <param name="value3">A <see cref="Vector4"/>.</param>
        /// <param name="u">Barycentric coordinate u, expressing weight towards <paramref name="value2"/>.</param>
        /// <param name="v">Barycentric coordinate v, expressing weight towards <paramref name="value3"/>.</param>
        public static void Barycentric(out Vector4 result, ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, float u, float v)
        {
            result.X = value1.X + u * (value2.X - value1.X) + v * (value3.X - value1.X);
            result.Y = value1.Y + u * (value2.Y - value1.Y) + v * (value3.Y - value1.Y);
            result.Z = value1.Z + u * (value2.Z - value1.Z) + v * (value3.Z - value1.Z);
            result.W = value1.W + u * (value2.W - value1.W) + v * (value3.W - value1.W);
        }

        /// <summary>
        /// Performs a linear interpolation between the specified quaternions using barycentric coordinates.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        /// <param name="value3">A <see cref="Quaternion"/>.</param>
        /// <param name="u">Barycentric coordinate u, expressing weight towards <paramref name="value2"/>.</param>
        /// <param name="v">Barycentric coordinate v, expressing weight towards <paramref name="value3"/>.</param>
        public static void Barycentric(out Quaternion result, ref Quaternion value1, ref Quaternion value2, ref Quaternion value3, float u, float v)
        {
            result.W = value1.W + u * (value2.W - value1.W) + v * (value3.W - value1.W);
            result.I = value1.I + u * (value2.I - value1.I) + v * (value3.I - value1.I);
            result.J = value1.J + u * (value2.J - value1.J) + v * (value3.J - value1.J);
            result.K = value1.K + u * (value2.K - value1.K) + v * (value3.K - value1.K);
        }

        /// <summary>
        /// Performs a linear interpolation between the specified matrices using barycentric coordinates.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix2"/>.</param>
        /// <param name="value2">A <see cref="Matrix2"/>.</param>
        /// <param name="value3">A <see cref="Matrix2"/>.</param>
        /// <param name="u">Barycentric coordinate u, expressing weight towards <paramref name="value2"/>.</param>
        /// <param name="v">Barycentric coordinate v, expressing weight towards <paramref name="value3"/>.</param>
        public static void Barycentric(out Matrix2 result, ref Matrix2 value1, ref Matrix2 value2, ref Matrix2 value3, float u, float v)
        {
            result.M11 = value1.M11 + u * (value2.M11 - value1.M11) + v * (value3.M11 - value1.M11);
            result.M12 = value1.M11 + u * (value2.M12 - value1.M12) + v * (value3.M12 - value1.M12);
            result.M21 = value1.M21 + u * (value2.M21 - value1.M21) + v * (value3.M21 - value1.M21);
            result.M22 = value1.M21 + u * (value2.M22 - value1.M22) + v * (value3.M22 - value1.M22);
        }

        /// <summary>
        /// Performs a linear interpolation between the specified matrices using barycentric coordinates.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix3"/>.</param>
        /// <param name="value2">A <see cref="Matrix3"/>.</param>
        /// <param name="value3">A <see cref="Matrix3"/>.</param>
        /// <param name="u">Barycentric coordinate u, expressing weight towards <paramref name="value2"/>.</param>
        /// <param name="v">Barycentric coordinate v, expressing weight towards <paramref name="value3"/>.</param>
        public static void Barycentric(out Matrix3 result, ref Matrix3 value1, ref Matrix3 value2, ref Matrix3 value3, float u, float v)
        {
            result.M11 = value1.M11 + u * (value2.M11 - value1.M11) + v * (value3.M11 - value1.M11);
            result.M12 = value1.M11 + u * (value2.M12 - value1.M12) + v * (value3.M12 - value1.M12);
            result.M13 = value1.M11 + u * (value2.M13 - value1.M13) + v * (value3.M13 - value1.M13);
            result.M21 = value1.M21 + u * (value2.M21 - value1.M21) + v * (value3.M21 - value1.M21);
            result.M22 = value1.M21 + u * (value2.M22 - value1.M22) + v * (value3.M22 - value1.M22);
            result.M23 = value1.M21 + u * (value2.M23 - value1.M23) + v * (value3.M23 - value1.M23);
            result.M31 = value1.M31 + u * (value2.M31 - value1.M31) + v * (value3.M31 - value1.M31);
            result.M32 = value1.M31 + u * (value2.M32 - value1.M32) + v * (value3.M32 - value1.M32);
            result.M33 = value1.M31 + u * (value2.M33 - value1.M33) + v * (value3.M33 - value1.M33);
        }

        /// <summary>
        /// Performs a linear interpolation between the specified matrices using barycentric coordinates.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix4"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="value3">A <see cref="Matrix4"/>.</param>
        /// <param name="u">Barycentric coordinate u, expressing weight towards <paramref name="value2"/>.</param>
        /// <param name="v">Barycentric coordinate v, expressing weight towards <paramref name="value3"/>.</param>
        public static void Barycentric(out Matrix result, ref Matrix value1, ref Matrix value2, ref Matrix value3, float u, float v)
        {
            result.M11 = value1.M11 + u * (value2.M11 - value1.M11) + v * (value3.M11 - value1.M11);
            result.M12 = value1.M11 + u * (value2.M12 - value1.M12) + v * (value3.M12 - value1.M12);
            result.M13 = value1.M11 + u * (value2.M13 - value1.M13) + v * (value3.M13 - value1.M13);
            result.M14 = value1.M11 + u * (value2.M14 - value1.M14) + v * (value3.M14 - value1.M14);
            result.M21 = value1.M21 + u * (value2.M21 - value1.M21) + v * (value3.M21 - value1.M21);
            result.M22 = value1.M21 + u * (value2.M22 - value1.M22) + v * (value3.M22 - value1.M22);
            result.M23 = value1.M21 + u * (value2.M23 - value1.M23) + v * (value3.M23 - value1.M23);
            result.M24 = value1.M21 + u * (value2.M24 - value1.M24) + v * (value3.M24 - value1.M24);
            result.M31 = value1.M31 + u * (value2.M31 - value1.M31) + v * (value3.M31 - value1.M31);
            result.M32 = value1.M31 + u * (value2.M32 - value1.M32) + v * (value3.M32 - value1.M32);
            result.M33 = value1.M31 + u * (value2.M33 - value1.M33) + v * (value3.M33 - value1.M33);
            result.M34 = value1.M31 + u * (value2.M34 - value1.M34) + v * (value3.M34 - value1.M34);
            result.M41 = value1.M41 + u * (value2.M41 - value1.M41) + v * (value3.M41 - value1.M41);
            result.M42 = value1.M41 + u * (value2.M42 - value1.M42) + v * (value3.M42 - value1.M42);
            result.M43 = value1.M41 + u * (value2.M43 - value1.M43) + v * (value3.M43 - value1.M43);
            result.M44 = value1.M41 + u * (value2.M44 - value1.M44) + v * (value3.M44 - value1.M44);
        }
    }
}
