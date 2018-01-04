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
        /// Performs a spherical linear interpolation between the specified colors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color3"/>.</param>
        /// <param name="value2">A <see cref="Color3"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Slerp(out Color3 result, ref Color3 value1, ref Color3 value2, float amount)
        {
            float s0 = 1 - amount;
            float s1 = amount;

            float dot =
                value1.R * value2.R +
                value1.G * value2.G +
                value1.B * value2.B;

            float cosom = System.Math.Abs(dot);

            if (cosom < OneMinusEpsilon)
            {
                double omega = System.Math.Acos(cosom);
                double sinom = 1 / System.Math.Sin(omega);

                s0 = (float)(sinom * System.Math.Sin(omega * s0));
                s1 = (float)(sinom * System.Math.Sin(omega * s1));
            }

            if (dot < 0)
            {
                s1 = -s1;
            }

            result.R = s0 * value1.R + s1 * value2.R;
            result.G = s0 * value1.G + s1 * value2.G;
            result.B = s0 * value1.B + s1 * value2.B;
        }

        /// <summary>
        /// Performs a spherical linear interpolation between the specified colors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Color4"/>.</param>
        /// <param name="value2">A <see cref="Color4"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Slerp(out Color4 result, ref Color4 value1, ref Color4 value2, float amount)
        {
            float s0 = 1 - amount;
            float s1 = amount;

            float dot =
                value1.A * value2.A +
                value1.R * value2.R +
                value1.G * value2.G +
                value1.B * value2.B;

            float cosom = System.Math.Abs(dot);

            if (cosom < OneMinusEpsilon)
            {
                double omega = System.Math.Acos(cosom);
                double sinom = 1 / System.Math.Sin(omega);

                s0 = (float)(sinom * System.Math.Sin(omega * s0));
                s1 = (float)(sinom * System.Math.Sin(omega * s1));
            }

            if (dot < 0)
            {
                s1 = -s1;
            }

            result.A = s0 * value1.A + s1 * value2.A;
            result.R = s0 * value1.R + s1 * value2.R;
            result.G = s0 * value1.G + s1 * value2.G;
            result.B = s0 * value1.B + s1 * value2.B;
        }

        /// <summary>
        /// Performs a spherical linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Vector2"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Slerp(out Vector2 result, ref Vector2 value1, ref Vector2 value2, float amount)
        {
            float s0 = 1 - amount;
            float s1 = amount;

            float dot =
                value1.X * value2.X +
                value1.Y * value2.Y;

            float cosom = System.Math.Abs(dot);

            if (cosom < OneMinusEpsilon)
            {
                double omega = System.Math.Acos(cosom);
                double sinom = 1 / System.Math.Sin(omega);

                s0 = (float)(sinom * System.Math.Sin(omega * s0));
                s1 = (float)(sinom * System.Math.Sin(omega * s1));
            }

            if (dot < 0)
            {
                s1 = -s1;
            }

            result.X = s0 * value1.X + s1 * value2.X;
            result.Y = s0 * value1.Y + s1 * value2.Y;
        }

        /// <summary>
        /// Performs a spherical linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Slerp(out Vector3 result, ref Vector3 value1, ref Vector3 value2, float amount)
        {
            float s0 = 1 - amount;
            float s1 = amount;

            float dot =
                value1.X * value2.X +
                value1.Y * value2.Y +
                value1.Z * value2.Z;

            float cosom = System.Math.Abs(dot);

            if (cosom < OneMinusEpsilon)
            {
                double omega = System.Math.Acos(cosom);
                double sinom = 1 / System.Math.Sin(omega);

                s0 = (float)(sinom * System.Math.Sin(omega * s0));
                s1 = (float)(sinom * System.Math.Sin(omega * s1));
            }

            if (dot < 0)
            {
                s1 = -s1;
            }

            result.X = s0 * value1.X + s1 * value2.X;
            result.Y = s0 * value1.Y + s1 * value2.Y;
            result.Z = s0 * value1.Z + s1 * value2.Z;
        }

        /// <summary>
        /// Performs a spherical linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Slerp(out Vector4 result, ref Vector4 value1, ref Vector4 value2, float amount)
        {
            float s0 = 1 - amount;
            float s1 = amount;

            float dot =
                value1.X * value2.X +
                value1.Y * value2.Y +
                value1.Z * value2.Z +
                value1.W * value2.W;

            float cosom = System.Math.Abs(dot);

            if (cosom < OneMinusEpsilon)
            {
                double omega = System.Math.Acos(cosom);
                double sinom = 1 / System.Math.Sin(omega);

                s0 = (float)(sinom * System.Math.Sin(omega * s0));
                s1 = (float)(sinom * System.Math.Sin(omega * s1));
            }

            if (dot < 0)
            {
                s1 = -s1;
            }

            result.X = s0 * value1.X + s1 * value2.X;
            result.Y = s0 * value1.Y + s1 * value2.Y;
            result.Z = s0 * value1.Z + s1 * value2.Z;
            result.W = s0 * value1.W + s1 * value2.W;
        }

        /// <summary>
        /// Performs a spherical linear interpolation between the specified quaternions.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        /// <param name="amount">Interpolation value in range [0, 1].</param>
        public static void Slerp(out Quaternion result, ref Quaternion value1, ref Quaternion value2, float amount)
        {
            float s0 = 1 - amount;
            float s1 = amount;

            float dot =
                value1.W * value2.W +
                value1.I * value2.I +
                value1.J * value2.J +
                value1.K * value2.K;

            float cosom = System.Math.Abs(dot);

            if (cosom < OneMinusEpsilon)
            {
                double omega = System.Math.Acos(cosom);
                double sinom = 1 / System.Math.Sin(omega);

                s0 = (float)(sinom * System.Math.Sin(omega * s0));
                s1 = (float)(sinom * System.Math.Sin(omega * s1));
            }

            if (dot < 0)
            {
                s1 = -s1;
            }

            result.W = s0 * value1.W + s1 * value2.W;
            result.I = s0 * value1.I + s1 * value2.I;
            result.J = s0 * value1.J + s1 * value2.J;
            result.K = s0 * value1.K + s1 * value2.K;
        }
    }
}
