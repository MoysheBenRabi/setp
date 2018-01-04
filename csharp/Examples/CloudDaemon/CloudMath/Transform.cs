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
        /// Transforms the specified vector with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Transform(out Vector2 result, ref Vector2 value1, ref Matrix value2)
        {
            float x = value1.X;
            float y = value1.Y;

            result.X = x * value2.M11 + y * value2.M12 + value2.M14;
            result.Y = x * value2.M21 + y * value2.M22 + value2.M24;
        }

        /// <summary>
        /// Transforms the specified vector with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Transform(out Vector3 result, ref Vector3 value1, ref Matrix value2)
        {
            float x = value1.X;
            float y = value1.Y;
            float z = value1.Z;

            result.X = x * value2.M11 + y * value2.M12 + z * value2.M13 + value2.M14;
            result.Y = x * value2.M21 + y * value2.M22 + z * value2.M23 + value2.M24;
            result.Z = x * value2.M31 + y * value2.M32 + z * value2.M33 + value2.M34;
        }

        /// <summary>
        /// Transforms the specified vector by the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Transform(out Vector4 result, ref Vector2 value1, ref Matrix value2)
        {
            float x = value1.X;
            float y = value1.Y;

            result.X = x * value2.M11 + y * value2.M12 + value2.M14;
            result.Y = x * value2.M21 + y * value2.M22 + value2.M24;
            result.Z = x * value2.M31 + y * value2.M32 + value2.M34;
            result.W = x * value2.M41 + y * value2.M42 + value2.M44;
        }

        /// <summary>
        /// Transforms the specified vector by the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Transform(out Vector4 result, ref Vector3 value1, ref Matrix value2)
        {
            float x = value1.X;
            float y = value1.Y;
            float z = value1.Z;

            result.X = x * value2.M11 + y * value2.M12 + z * value2.M13 + value2.M14;
            result.Y = x * value2.M21 + y * value2.M22 + z * value2.M23 + value2.M24;
            result.Z = x * value2.M31 + y * value2.M32 + z * value2.M33 + value2.M34;
            result.W = x * value2.M41 + y * value2.M42 + z * value2.M43 + value2.M44;
        }

        /// <summary>
        /// Transforms the specified vector by the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Transform(out Vector4 result, ref Vector4 value1, ref Matrix value2)
        {
            float x = value1.X;
            float y = value1.Y;
            float z = value1.Z;
            float w = value1.W;

            result.X = x * value2.M11 + y * value2.M12 + z * value2.M13 + w * value2.M14;
            result.Y = x * value2.M21 + y * value2.M22 + z * value2.M23 + w * value2.M24;
            result.Z = x * value2.M31 + y * value2.M32 + z * value2.M33 + w * value2.M34;
            result.W = x * value2.M41 + y * value2.M42 + z * value2.M43 + w * value2.M44;
        }

        /// <summary>
        /// Transforms the specified ray with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Transform(out Ray result, ref Ray value1, ref Matrix value2)
        {
            float px = value1.Position.X;
            float py = value1.Position.Y;
            float pz = value1.Position.Z;
            float dx = value1.Direction.X;
            float dy = value1.Direction.Y;
            float dz = value1.Direction.Z;

            result.Position.X = px * value2.M11 + py * value2.M12 + pz * value2.M13 + value2.M14;
            result.Position.Y = px * value2.M21 + py * value2.M22 + pz * value2.M23 + value2.M24;
            result.Position.Z = px * value2.M31 + py * value2.M32 + pz * value2.M33 + value2.M34;
            result.Direction.X = dx * value2.M11 + dy * value2.M12 + dz * value2.M13;
            result.Direction.Y = dx * value2.M21 + dy * value2.M22 + dz * value2.M23;
            result.Direction.Z = dx * value2.M31 + dy * value2.M32 + dz * value2.M33;
        }

        /// <summary>
        /// Transforms the specified plane with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Transform(out Plane result, ref Plane value1, ref Matrix value2)
        {
            Matrix matrix;
            Invert(out matrix, ref value2);

            float x = value1.Normal.X;
            float y = value1.Normal.Y;
            float z = value1.Normal.Z;
            float d = value1.D;

            result.Normal.X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + d * matrix.M41;
            result.Normal.Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + d * matrix.M42;
            result.Normal.Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + d * matrix.M43;
            result.D = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + d * matrix.M44;
        }

        /// <summary>
        /// Transforms the specified box with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Transform(out BoundingBox result, ref BoundingBox value1, ref Matrix value2)
        {
            float radiusX = (value1.Maximum.X - value1.Minimum.X) * 0.5f;
            float radiusY = (value1.Maximum.Y - value1.Minimum.Y) * 0.5f;
            float radiusZ = (value1.Maximum.Z - value1.Minimum.Z) * 0.5f;
            float centerX = (value1.Minimum.X + value1.Maximum.X) * 0.5f;
            float centerY = (value1.Minimum.Y + value1.Maximum.Y) * 0.5f;
            float centerZ = (value1.Minimum.Z + value1.Maximum.Z) * 0.5f;

            float x = centerX * value2.M11 + centerY * value2.M12 + centerZ * value2.M13 + value2.M14;
            float y = centerX * value2.M21 + centerY * value2.M22 + centerZ * value2.M23 + value2.M24;
            float z = centerX * value2.M31 + centerY * value2.M32 + centerZ * value2.M33 + value2.M34;

            result.Minimum.X = x - radiusX;
            result.Minimum.Y = y - radiusY;
            result.Minimum.Z = z - radiusZ;
            result.Maximum.X = x + radiusX;
            result.Maximum.Y = y + radiusY;
            result.Maximum.Z = z + radiusZ;
        }

        /// <summary>
        /// Transforms the specified sphere with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Transform(out Sphere result, ref Sphere value1, ref Matrix value2)
        {
            float x = value1.Position.X;
            float y = value1.Position.Y;
            float z = value1.Position.Z;

            result.Position.X = x * value2.M11 + y * value2.M12 + z * value2.M13 + value2.M14;
            result.Position.Y = x * value2.M21 + y * value2.M22 + z * value2.M23 + value2.M24;
            result.Position.Z = x * value2.M31 + y * value2.M32 + z * value2.M33 + value2.M34;
            result.Radius = value1.Radius;
        }

        /// <summary>
        /// Transforms the specified volume with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Volume"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Transform(out Volume result, ref Volume value1, ref Matrix value2)
        {
            Matrix matrix;
            Invert(out matrix, ref value2);

            float x, y, z, d;

            x = value1.ClipPlane1.Normal.X;
            y = value1.ClipPlane1.Normal.Y;
            z = value1.ClipPlane1.Normal.Z;
            d = value1.ClipPlane1.D;

            result.ClipPlane1.Normal.X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + d * matrix.M41;
            result.ClipPlane1.Normal.Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + d * matrix.M42;
            result.ClipPlane1.Normal.Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + d * matrix.M43;
            result.ClipPlane1.D = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + d * matrix.M44;

            x = value1.ClipPlane2.Normal.X;
            y = value1.ClipPlane2.Normal.Y;
            z = value1.ClipPlane2.Normal.Z;
            d = value1.ClipPlane2.D;

            result.ClipPlane2.Normal.X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + d * matrix.M41;
            result.ClipPlane2.Normal.Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + d * matrix.M42;
            result.ClipPlane2.Normal.Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + d * matrix.M43;
            result.ClipPlane2.D = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + d * matrix.M44;

            x = value1.ClipPlane3.Normal.X;
            y = value1.ClipPlane3.Normal.Y;
            z = value1.ClipPlane3.Normal.Z;
            d = value1.ClipPlane3.D;

            result.ClipPlane3.Normal.X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + d * matrix.M41;
            result.ClipPlane3.Normal.Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + d * matrix.M42;
            result.ClipPlane3.Normal.Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + d * matrix.M43;
            result.ClipPlane3.D = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + d * matrix.M44;

            x = value1.ClipPlane4.Normal.X;
            y = value1.ClipPlane4.Normal.Y;
            z = value1.ClipPlane4.Normal.Z;
            d = value1.ClipPlane4.D;

            result.ClipPlane4.Normal.X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + d * matrix.M41;
            result.ClipPlane4.Normal.Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + d * matrix.M42;
            result.ClipPlane4.Normal.Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + d * matrix.M43;
            result.ClipPlane4.D = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + d * matrix.M44;

            x = value1.ClipPlane5.Normal.X;
            y = value1.ClipPlane5.Normal.Y;
            z = value1.ClipPlane5.Normal.Z;
            d = value1.ClipPlane5.D;

            result.ClipPlane5.Normal.X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + d * matrix.M41;
            result.ClipPlane5.Normal.Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + d * matrix.M42;
            result.ClipPlane5.Normal.Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + d * matrix.M43;
            result.ClipPlane5.D = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + d * matrix.M44;

            x = value1.ClipPlane6.Normal.X;
            y = value1.ClipPlane6.Normal.Y;
            z = value1.ClipPlane6.Normal.Z;
            d = value1.ClipPlane6.D;

            result.ClipPlane6.Normal.X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + d * matrix.M41;
            result.ClipPlane6.Normal.Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + d * matrix.M42;
            result.ClipPlane6.Normal.Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + d * matrix.M43;
            result.ClipPlane6.D = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + d * matrix.M44;
        }
    }
}
