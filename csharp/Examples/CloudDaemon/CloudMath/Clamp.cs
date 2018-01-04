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
        /// Restricts the specified color withing the specified range.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Color3"/>.</param>
        /// <param name="min">A <see cref="Color3"/>.</param>
        /// <param name="max">A <see cref="Color3"/>.</param>
        public static void Clamp(out Color3 result, ref Color3 value, ref Color3 min, ref Color3 max)
        {
            float r = value.R;
            r = (r > max.R) ? max.R : r;
            r = (r < min.R) ? min.R : r;

            float g = value.G;
            g = (g > max.G) ? max.G : g;
            g = (g < min.G) ? min.G : g;

            float b = value.B;
            b = (b > max.B) ? max.B : b;
            b = (b < min.B) ? min.B : b;

            result.R = r;
            result.G = g;
            result.B = b;
        }
        
        /// <summary>
        /// Restricts the specified color withing the specified range.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Color4"/>.</param>
        /// <param name="min">A <see cref="Color4"/>.</param>
        /// <param name="max">A <see cref="Color4"/>.</param>
        public static void Clamp(out Color4 result, ref Color4 value, ref Color4 min, ref Color4 max)
        {
            float a = value.A;
            a = (a > max.A) ? max.A : a;
            a = (a < min.A) ? min.A : a;

            float r = value.R;
            r = (r > max.R) ? max.R : r;
            r = (r < min.R) ? min.R : r;

            float g = value.G;
            g = (g > max.G) ? max.G : g;
            g = (g < min.G) ? min.G : g;

            float b = value.B;
            b = (b > max.B) ? max.B : b;
            b = (b < min.B) ? min.B : b;

            result.A = a;
            result.R = r;
            result.G = g;
            result.B = b;
        }

        /// <summary>
        /// Restricts the specified vector withing the specified range.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector2"/>.</param>
        /// <param name="min">A <see cref="Vector2"/>.</param>
        /// <param name="max">A <see cref="Vector2"/>.</param>
        public static void Clamp(out Vector2 result, ref Vector2 value, ref Vector2 min, ref Vector2 max)
        {
            float x = value.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            float y = value.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Restricts the specified vector withing the specified range.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector3"/>.</param>
        /// <param name="min">A <see cref="Vector3"/>.</param>
        /// <param name="max">A <see cref="Vector3"/>.</param>
        public static void Clamp(out Vector3 result, ref Vector3 value, ref Vector3 min, ref Vector3 max)
        {
            float x = value.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            float y = value.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            float z = value.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Restricts the specified vector withing the specified range.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector4"/>.</param>
        /// <param name="min">A <see cref="Vector4"/>.</param>
        /// <param name="max">A <see cref="Vector4"/>.</param>
        public static void Clamp(out Vector4 result, ref Vector4 value, ref Vector4 min, ref Vector4 max)
        {
            float x = value.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            float y = value.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            float z = value.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            float w = value.W;
            w = (w > max.W) ? max.W : w;
            w = (w < min.W) ? min.W : w;

            result.X = x;
            result.Y = y;
            result.Z = z;
            result.W = w;
        }

        /// <summary>
        /// Restricts the specified vector withing the specified box.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        public static void Clamp(out Vector3 result, ref Vector3 value1, ref BoundingBox value2)
        {
            float x = value1.X;
            x = (x > value2.Maximum.X) ? value2.Maximum.X : x;
            x = (x < value2.Minimum.X) ? value2.Minimum.X : x;

            float y = value1.Y;
            y = (y > value2.Maximum.Y) ? value2.Maximum.Y : y;
            y = (y < value2.Minimum.Y) ? value2.Minimum.Y : y;

            float z = value1.Z;
            z = (z > value2.Maximum.Z) ? value2.Maximum.Z : z;
            z = (z < value2.Minimum.Z) ? value2.Minimum.Z : z;

            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Restricts the specified vector withing the specified box.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        public static void Clamp(out Vector4 result, ref Vector4 value1, ref BoundingBox value2)
        {
            float x = value1.X;
            x = (x > value2.Maximum.X) ? value2.Maximum.X : x;
            x = (x < value2.Minimum.X) ? value2.Minimum.X : x;

            float y = value1.Y;
            y = (y > value2.Maximum.Y) ? value2.Maximum.Y : y;
            y = (y < value2.Minimum.Y) ? value2.Minimum.Y : y;

            float z = value1.Z;
            z = (z > value2.Maximum.Z) ? value2.Maximum.Z : z;
            z = (z < value2.Minimum.Z) ? value2.Minimum.Z : z;

            result.X = x;
            result.Y = y;
            result.Z = z;
            result.W = value1.W;
        }
    }
}
