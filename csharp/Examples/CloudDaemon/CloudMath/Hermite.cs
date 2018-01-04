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
        /// Performs a Hermite spline interpolation between the specified colors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color3"/>.</param>
        /// <param name="tangent1">A <see cref="Color3"/>.</param>
        /// <param name="value2">A <see cref="Color3"/>.</param>
        /// <param name="tangent2">A <see cref="Color3"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Hermite(out Color3 result, ref Color3 value1, ref Color3 tangent1, ref Color3 value2, ref Color3 tangent2, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;
            float h1 = (2 * amount3) - (3 * amount2) + 1;
            float h2 = (3 * amount2) - (2 * amount3);
            float h3 = amount3 - (2 * amount2) + amount;
            float h4 = amount3 - amount2;

            result.R = value1.R * h1 + value2.R * h2 + tangent1.R * h3 + tangent2.R * h4;
            result.G = value1.G * h1 + value2.G * h2 + tangent1.G * h3 + tangent2.G * h4;
            result.B = value1.B * h1 + value2.B * h2 + tangent1.B * h3 + tangent2.B * h4;
        }
        
        /// <summary>
        /// Performs a Hermite spline interpolation between the specified colors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color4"/>.</param>
        /// <param name="tangent1">A <see cref="Color4"/>.</param>
        /// <param name="value2">A <see cref="Color4"/>.</param>
        /// <param name="tangent2">A <see cref="Color4"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Hermite(out Color4 result, ref Color4 value1, ref Color4 tangent1, ref Color4 value2, ref Color4 tangent2, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;
            float h1 = (2 * amount3) - (3 * amount2) + 1;
            float h2 = (3 * amount2) - (2 * amount3);
            float h3 = amount3 - (2 * amount2) + amount;
            float h4 = amount3 - amount2;

            result.A = value1.A * h1 + value2.A * h2 + tangent1.A * h3 + tangent2.A * h4;
            result.R = value1.R * h1 + value2.R * h2 + tangent1.R * h3 + tangent2.R * h4;
            result.G = value1.G * h1 + value2.G * h2 + tangent1.G * h3 + tangent2.G * h4;
            result.B = value1.B * h1 + value2.B * h2 + tangent1.B * h3 + tangent2.B * h4;
        }

        /// <summary>
        /// Performs a Hermite spline interpolation between the specified values.
        /// </summary>
        /// <param name="value1">A <see cref="Single"/>.</param>
        /// <param name="tangent1">A <see cref="Single"/>.</param>
        /// <param name="value2">A <see cref="Single"/>.</param>
        /// <param name="tangent2">A <see cref="Single"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        /// <returns>Interpolated value.</returns>
        public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;
            float h1 = (2 * amount3) - (3 * amount2) + 1;
            float h2 = (3 * amount2) - (2 * amount3);
            float h3 = amount3 - (2 * amount2) + amount;
            float h4 = amount3 - amount2;

            return value1 * h1 + value2 * h2 + tangent1 * h3 + tangent2 * h4;
        }

        /// <summary>
        /// Performs a Hermite spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="tangent1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Vector2"/>.</param>
        /// <param name="tangent2">A <see cref="Vector2"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Hermite(out Vector2 result, ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;
            float h1 = (2 * amount3) - (3 * amount2) + 1;
            float h2 = (3 * amount2) - (2 * amount3);
            float h3 = amount3 - (2 * amount2) + amount;
            float h4 = amount3 - amount2;

            result.X = value1.X * h1 + value2.X * h2 + tangent1.X * h3 + tangent2.X * h4;
            result.Y = value1.Y * h1 + value2.Y * h2 + tangent1.Y * h3 + tangent2.Y * h4;
        }

        /// <summary>
        /// Performs a Hermite spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="tangent1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <param name="tangent2">A <see cref="Vector3"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Hermite(out Vector3 result, ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;
            float h1 = (2 * amount3) - (3 * amount2) + 1;
            float h2 = (3 * amount2) - (2 * amount3);
            float h3 = amount3 - (2 * amount2) + amount;
            float h4 = amount3 - amount2;

            result.X = value1.X * h1 + value2.X * h2 + tangent1.X * h3 + tangent2.X * h4;
            result.Y = value1.Y * h1 + value2.Y * h2 + tangent1.Y * h3 + tangent2.Y * h4;
            result.Z = value1.Z * h1 + value2.Z * h2 + tangent1.Z * h3 + tangent2.Z * h4;
        }

        /// <summary>
        /// Performs a Hermite spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="tangent1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <param name="tangent2">A <see cref="Vector4"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Hermite(out Vector4 result, ref Vector4 value1, ref Vector4 tangent1, ref Vector4 value2, ref Vector4 tangent2, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;
            float h1 = (2 * amount3) - (3 * amount2) + 1;
            float h2 = (3 * amount2) - (2 * amount3);
            float h3 = amount3 - (2 * amount2) + amount;
            float h4 = amount3 - amount2;

            result.X = value1.X * h1 + value2.X * h2 + tangent1.X * h3 + tangent2.X * h4;
            result.Y = value1.Y * h1 + value2.Y * h2 + tangent1.Y * h3 + tangent2.Y * h4;
            result.Z = value1.Z * h1 + value2.Z * h2 + tangent1.Z * h3 + tangent2.Z * h4;
            result.W = value1.W * h1 + value2.W * h2 + tangent1.W * h3 + tangent2.W * h4;
        }

        /// <summary>
        /// Performs a Hermite spline interpolation between the specified quaternions.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="tangent1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        /// <param name="tangent2">A <see cref="Quaternion"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Hermite(out Quaternion result, ref Quaternion value1, ref Quaternion tangent1, ref Quaternion value2, ref Quaternion tangent2, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;
            float h1 = (2 * amount3) - (3 * amount2) + 1;
            float h2 = (3 * amount2) - (2 * amount3);
            float h3 = amount3 - (2 * amount2) + amount;
            float h4 = amount3 - amount2;

            result.W = value1.W * h1 + value2.W * h2 + tangent1.W * h3 + tangent2.W * h4;
            result.I = value1.I * h1 + value2.I * h2 + tangent1.I * h3 + tangent2.I * h4;
            result.J = value1.J * h1 + value2.J * h2 + tangent1.J * h3 + tangent2.J * h4;
            result.K = value1.K * h1 + value2.K * h2 + tangent1.K * h3 + tangent2.K * h4;
        }
    }
}
