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
        /// Performs a Catmull-Rom spline interpolation between the specified colors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color3"/>.</param>
        /// <param name="value2">A <see cref="Color3"/>.</param>
        /// <param name="value3">A <see cref="Color3"/>.</param>
        /// <param name="value4">A <see cref="Color3"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void CatmullRom(out Color3 result, ref Color3 value1, ref Color3 value2, ref Color3 value3, ref Color3 value4, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;

            result.R = 0.5f * (2 * value2.R + amount * (value3.R - value1.R) + amount2 * (2 * value1.R - 5 * value2.R + 4 * value3.R - value4.R) + amount3 * (value4.R - 3 * value3.R + 3 * value2.R - value1.R));
            result.G = 0.5f * (2 * value2.G + amount * (value3.G - value1.G) + amount2 * (2 * value1.G - 5 * value2.G + 4 * value3.G - value4.G) + amount3 * (value4.G - 3 * value3.G + 3 * value2.G - value1.G));
            result.B = 0.5f * (2 * value2.B + amount * (value3.B - value1.B) + amount2 * (2 * value1.B - 5 * value2.B + 4 * value3.B - value4.B) + amount3 * (value4.B - 3 * value3.B + 3 * value2.B - value1.B));
        }
        
        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified colors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color4"/>.</param>
        /// <param name="value2">A <see cref="Color4"/>.</param>
        /// <param name="value3">A <see cref="Color4"/>.</param>
        /// <param name="value4">A <see cref="Color4"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void CatmullRom(out Color4 result, ref Color4 value1, ref Color4 value2, ref Color4 value3, ref Color4 value4, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;

            result.A = 0.5f * (2 * value2.A + amount * (value3.A - value1.A) + amount2 * (2 * value1.A - 5 * value2.A + 4 * value3.A - value4.A) + amount3 * (value4.A - 3 * value3.A + 3 * value2.A - value1.A));
            result.R = 0.5f * (2 * value2.R + amount * (value3.R - value1.R) + amount2 * (2 * value1.R - 5 * value2.R + 4 * value3.R - value4.R) + amount3 * (value4.R - 3 * value3.R + 3 * value2.R - value1.R));
            result.G = 0.5f * (2 * value2.G + amount * (value3.G - value1.G) + amount2 * (2 * value1.G - 5 * value2.G + 4 * value3.G - value4.G) + amount3 * (value4.G - 3 * value3.G + 3 * value2.G - value1.G));
            result.B = 0.5f * (2 * value2.B + amount * (value3.B - value1.B) + amount2 * (2 * value1.B - 5 * value2.B + 4 * value3.B - value4.B) + amount3 * (value4.B - 3 * value3.B + 3 * value2.B - value1.B));
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified values.
        /// </summary>
        /// <param name="value1">A <see cref="Single"/>.</param>
        /// <param name="value2">A <see cref="Single"/>.</param>
        /// <param name="value3">A <see cref="Single"/>.</param>
        /// <param name="value4">A <see cref="Single"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        /// <returns>Interpolated value.</returns>
        public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;
            
            return 0.5f * (2 * value2 + amount * (value3 - value1) + amount2 * (2 * value1 - 5 * value2 + 4 * value3 - value4) + amount3 * (value4 - 3 * value3 + 3 * value2 - value1));
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Vector2"/>.</param>
        /// <param name="value3">A <see cref="Vector2"/>.</param>
        /// <param name="value4">A <see cref="Vector2"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void CatmullRom(out Vector2 result, ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;

            result.X = 0.5f * (2 * value2.X + amount * (value3.X - value1.X) + amount2 * (2 * value1.X - 5 * value2.X + 4 * value3.X - value4.X) + amount3 * (value4.X - 3 * value3.X + 3 * value2.X - value1.X));
            result.Y = 0.5f * (2 * value2.Y + amount * (value3.Y - value1.Y) + amount2 * (2 * value1.Y - 5 * value2.Y + 4 * value3.Y - value4.Y) + amount3 * (value4.Y - 3 * value3.Y + 3 * value2.Y - value1.Y));
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <param name="value3">A <see cref="Vector3"/>.</param>
        /// <param name="value4">A <see cref="Vector3"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void CatmullRom(out Vector3 result, ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;

            result.X = 0.5f * (2 * value2.X + amount * (value3.X - value1.X) + amount2 * (2 * value1.X - 5 * value2.X + 4 * value3.X - value4.X) + amount3 * (value4.X - 3 * value3.X + 3 * value2.X - value1.X));
            result.Y = 0.5f * (2 * value2.Y + amount * (value3.Y - value1.Y) + amount2 * (2 * value1.Y - 5 * value2.Y + 4 * value3.Y - value4.Y) + amount3 * (value4.Y - 3 * value3.Y + 3 * value2.Y - value1.Y));
            result.Z = 0.5f * (2 * value2.Z + amount * (value3.Z - value1.Z) + amount2 * (2 * value1.Z - 5 * value2.Z + 4 * value3.Z - value4.Z) + amount3 * (value4.Z - 3 * value3.Z + 3 * value2.Z - value1.Z));
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <param name="value3">A <see cref="Vector4"/>.</param>
        /// <param name="value4">A <see cref="Vector4"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void CatmullRom(out Vector4 result, ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, ref Vector4 value4, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;

            result.X = 0.5f * (2 * value2.X + amount * (value3.X - value1.X) + amount2 * (2 * value1.X - 5 * value2.X + 4 * value3.X - value4.X) + amount3 * (value4.X - 3 * value3.X + 3 * value2.X - value1.X));
            result.Y = 0.5f * (2 * value2.Y + amount * (value3.Y - value1.Y) + amount2 * (2 * value1.Y - 5 * value2.Y + 4 * value3.Y - value4.Y) + amount3 * (value4.Y - 3 * value3.Y + 3 * value2.Y - value1.Y));
            result.Z = 0.5f * (2 * value2.Z + amount * (value3.Z - value1.Z) + amount2 * (2 * value1.Z - 5 * value2.Z + 4 * value3.Z - value4.Z) + amount3 * (value4.Z - 3 * value3.Z + 3 * value2.Z - value1.Z));
            result.W = 0.5f * (2 * value2.W + amount * (value3.W - value1.W) + amount2 * (2 * value1.W - 5 * value2.W + 4 * value3.W - value4.W) + amount3 * (value4.W - 3 * value3.W + 3 * value2.W - value1.W));
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified quaternions.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        /// <param name="value3">A <see cref="Quaternion"/>.</param>
        /// <param name="value4">A <see cref="Quaternion"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void CatmullRom(out Quaternion result, ref Quaternion value1, ref Quaternion value2, ref Quaternion value3, ref Quaternion value4, float amount)
        {
            float amount2 = amount * amount;
            float amount3 = amount * amount2;

            result.W = 0.5f * (2 * value2.W + amount * (value3.W - value1.W) + amount2 * (2 * value1.W - 5 * value2.W + 4 * value3.W - value4.W) + amount3 * (value4.W - 3 * value3.W + 3 * value2.W - value1.W));
            result.I = 0.5f * (2 * value2.I + amount * (value3.I - value1.I) + amount2 * (2 * value1.I - 5 * value2.I + 4 * value3.I - value4.I) + amount3 * (value4.I - 3 * value3.I + 3 * value2.I - value1.I));
            result.J = 0.5f * (2 * value2.J + amount * (value3.J - value1.J) + amount2 * (2 * value1.J - 5 * value2.J + 4 * value3.J - value4.J) + amount3 * (value4.J - 3 * value3.J + 3 * value2.J - value1.J));
            result.K = 0.5f * (2 * value2.K + amount * (value3.K - value1.K) + amount2 * (2 * value1.K - 5 * value2.K + 4 * value3.K - value4.K) + amount3 * (value4.K - 3 * value3.K + 3 * value2.K - value1.K));
        }
    }
}
