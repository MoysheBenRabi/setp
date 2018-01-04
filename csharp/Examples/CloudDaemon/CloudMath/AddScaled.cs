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
        /// Calculates the sum of the specified colors where the second color is scaled with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color3"/>.</param>
        /// <param name="value2">A <see cref="Color3"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void AddScaled(out Color3 result, ref Color3 value1, ref Color3 value2, float amount)
        {
            result.R = value1.R + value2.R * amount;
            result.G = value1.G + value2.G * amount;
            result.B = value1.B + value2.B * amount;
        }
        
        /// <summary>
        /// Calculates the sum of the specified colors where the second color is scaled with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color4"/>.</param>
        /// <param name="value2">A <see cref="Color4"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void AddScaled(out Color4 result, ref Color4 value1, ref Color4 value2, float amount)
        {
            result.A = value1.A + value2.A * amount;
            result.R = value1.R + value2.R * amount;
            result.G = value1.G + value2.G * amount;
            result.B = value1.B + value2.B * amount;
        }

        /// <summary>
        /// Calculates the sum of the specified vectors where the second vector is scaled with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Vector2"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void AddScaled(out Vector2 result, ref Vector2 value1, ref Vector2 value2, float amount)
        {
            result.X = value1.X + value2.X * amount;
            result.Y = value1.Y + value2.Y * amount;
        }

        /// <summary>
        /// Calculates the sum of the specified vectors where the second vector is scaled with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void AddScaled(out Vector3 result, ref Vector3 value1, ref Vector3 value2, float amount)
        {
            result.X = value1.X + value2.X * amount;
            result.Y = value1.Y + value2.Y * amount;
            result.Z = value1.Z + value2.Z * amount;
        }

        /// <summary>
        /// Calculates the sum of the specified vectors where the second vector is scaled with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void AddScaled(out Vector4 result, ref Vector4 value1, ref Vector4 value2, float amount)
        {
            result.X = value1.X + value2.X * amount;
            result.Y = value1.Y + value2.Y * amount;
            result.Z = value1.Z + value2.Z * amount;
            result.W = value1.W + value2.W * amount;
        }

        /// <summary>
        /// Calculates the sum of the specified quaternions where the second quaternion is scaled with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void AddScaled(out Quaternion result, ref Quaternion value1, ref Quaternion value2, float amount)
        {
            result.W = value1.W + value2.W * amount;
            result.I = value1.I + value2.I * amount;
            result.J = value1.J + value2.J * amount;
            result.K = value1.K + value2.K * amount;
        }

        /// <summary>
        /// Calculates the sum of the specified matrices where the second matrix is scaled with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix2"/>.</param>
        /// <param name="value2">A <see cref="Matrix2"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void AddScaled(out Matrix2 result, ref Matrix2 value1, ref Matrix2 value2, float amount)
        {
            result.M11 = value1.M11 + value2.M11 * amount;
            result.M12 = value1.M12 + value2.M12 * amount;
            result.M21 = value1.M21 + value2.M21 * amount;
            result.M22 = value1.M22 + value2.M22 * amount;
        }

        /// <summary>
        /// Calculates the sum of the specified matrices where the second matrix is scaled with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix3"/>.</param>
        /// <param name="value2">A <see cref="Matrix3"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void AddScaled(out Matrix3 result, ref Matrix3 value1, ref Matrix3 value2, float amount)
        {
            result.M11 = value1.M11 + value2.M11 * amount;
            result.M12 = value1.M12 + value2.M12 * amount;
            result.M13 = value1.M13 + value2.M13 * amount;
            result.M21 = value1.M21 + value2.M21 * amount;
            result.M22 = value1.M22 + value2.M22 * amount;
            result.M23 = value1.M23 + value2.M23 * amount;
            result.M31 = value1.M31 + value2.M31 * amount;
            result.M32 = value1.M32 + value2.M32 * amount;
            result.M33 = value1.M33 + value2.M33 * amount;
        }

        /// <summary>
        /// Calculates the sum of the specified matrices where the second matrix is scaled with the specified amount.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix4"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="amount">Scaling factor.</param>
        public static void AddScaled(out Matrix result, ref Matrix value1, ref Matrix value2, float amount)
        {
            result.M11 = value1.M11 + value2.M11 * amount;
            result.M12 = value1.M12 + value2.M12 * amount;
            result.M13 = value1.M13 + value2.M13 * amount;
            result.M14 = value1.M14 + value2.M14 * amount;
            result.M21 = value1.M21 + value2.M21 * amount;
            result.M22 = value1.M22 + value2.M22 * amount;
            result.M23 = value1.M23 + value2.M23 * amount;
            result.M24 = value1.M24 + value2.M24 * amount;
            result.M31 = value1.M31 + value2.M31 * amount;
            result.M32 = value1.M32 + value2.M32 * amount;
            result.M33 = value1.M33 + value2.M33 * amount;
            result.M34 = value1.M34 + value2.M34 * amount;
            result.M41 = value1.M41 + value2.M41 * amount;
            result.M42 = value1.M42 + value2.M42 * amount;
            result.M43 = value1.M43 + value2.M43 * amount;
            result.M44 = value1.M44 + value2.M44 * amount;
        }
    }
}
