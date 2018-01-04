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
        /// Reflects the specified vector off a line with the specified normal.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector2"/>.</param>
        /// <param name="normal">Line normal vector.</param>
        public static void Reflect(out Vector2 result, ref Vector2 value, ref Vector2 normal)
        {
            float dot = 2 * (
                value.X * normal.X +
                value.Y * normal.Y);

            result.X = value.X - (dot * normal.X);
            result.Y = value.Y - (dot * normal.Y);
        }

        /// <summary>
        /// Reflects the specified vector off a plane with the specified normal.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector3"/>.</param>
        /// <param name="normal">Plane normal vector.</param>
        public static void Reflect(out Vector3 result, ref Vector3 value, ref Vector3 normal)
        {
            float dot = 2 * (
                value.X * normal.X +
                value.Y * normal.Y +
                value.Z * normal.Z);

            result.X = value.X - (dot * normal.X);
            result.Y = value.Y - (dot * normal.Y);
            result.Z = value.Z - (dot * normal.Z);
        }

        /// <summary>
        /// Reflects the specified vector off a plane with the specified normal.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector4"/>.</param>
        /// <param name="normal">Plane normal vector.</param>
        public static void Reflect(out Vector4 result, ref Vector4 value, ref Vector3 normal)
        {
            float dot = 2 * (
                value.X * normal.X +
                value.Y * normal.Y +
                value.Z * normal.Z);

            result.X = value.X - (dot * normal.X);
            result.Y = value.Y - (dot * normal.Y);
            result.Z = value.Z - (dot * normal.Z);
            result.W = value.W;
        }

        /// <summary>
        /// Creates a transformation matrix from a reflection plane.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="normal">Reflection plane normal vector.</param>
        /// <param name="d">Reflection plane D component.</param>
        public static void Reflect(out Matrix result, ref Vector3 normal, float d)
        {
            float x = -2 * normal.X;
            float y = -2 * normal.Y;
            float z = -2 * normal.Z;

            result.M11 = x * normal.X + 1;
            result.M12 = x * normal.Y;
            result.M13 = x * normal.Z;
            result.M14 = x * d;
            result.M21 = y * normal.X;
            result.M22 = y * normal.Y + 1;
            result.M23 = y * normal.Z;
            result.M24 = y * d;
            result.M31 = z * normal.X;
            result.M32 = z * normal.Y;
            result.M33 = z * normal.Z + 1;
            result.M34 = z * d;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Reflects the specified matrix off a plane with the specified normal.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        /// <param name="normal">Reflection plane normal vector.</param>
        /// <param name="d">Reflection plane equation D component.</param>
        public static void Reflect(out Matrix result, ref Matrix value, ref Vector3 normal, float d)
        {
            float x = -2 * normal.X;
            float y = -2 * normal.Y;
            float z = -2 * normal.Z;

            float m11 = x * normal.X + 1;
            float m12 = x * normal.Y;
            float m13 = x * normal.Z;
            float m14 = x * d;
            float m21 = y * normal.X;
            float m22 = y * normal.Y + 1;
            float m23 = y * normal.Z;
            float m24 = y * d;
            float m31 = z * normal.X;
            float m32 = z * normal.Y;
            float m33 = z * normal.Z + 1;
            float m34 = z * d;

            result.M11 = m11 * value.M11 + m12 * value.M21 + m13 * value.M31 + m14 * value.M41;
            result.M12 = m11 * value.M12 + m12 * value.M22 + m13 * value.M32 + m14 * value.M42;
            result.M13 = m11 * value.M13 + m12 * value.M23 + m13 * value.M33 + m14 * value.M43;
            result.M14 = m11 * value.M14 + m12 * value.M24 + m13 * value.M34 + m14 * value.M44;
            result.M21 = m21 * value.M11 + m22 * value.M21 + m23 * value.M31 + m24 * value.M41;
            result.M22 = m21 * value.M12 + m22 * value.M22 + m23 * value.M32 + m24 * value.M42;
            result.M23 = m21 * value.M13 + m22 * value.M23 + m23 * value.M33 + m24 * value.M43;
            result.M24 = m21 * value.M14 + m22 * value.M24 + m23 * value.M34 + m24 * value.M44;
            result.M31 = m31 * value.M11 + m32 * value.M21 + m33 * value.M31 + m34 * value.M41;
            result.M32 = m31 * value.M12 + m32 * value.M22 + m33 * value.M32 + m34 * value.M42;
            result.M33 = m31 * value.M13 + m32 * value.M23 + m33 * value.M33 + m34 * value.M43;
            result.M34 = m31 * value.M14 + m32 * value.M24 + m33 * value.M34 + m34 * value.M44;
            result.M41 = value.M41;
            result.M42 = value.M42;
            result.M43 = value.M43;
            result.M44 = value.M44;
        }
    }
}
