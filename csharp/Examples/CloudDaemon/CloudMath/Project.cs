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
        /// Projects the specified vector on the specified line.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector2"/>.</param>
        /// <param name="normal">Projection line normal vector.</param>
        /// <param name="d">Projection line D component.</param>
        public static void Project(out Vector2 result, ref Vector2 value, ref Vector2 normal, float d)
        {
            float distance =
                normal.X * value.X +
                normal.Y * value.Y +
                d;

            result.X = value.X - normal.X * distance;
            result.Y = value.Y - normal.Y * distance;
        }

        /// <summary>
        /// Projects the specified vector on the specified plane.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector3"/>.</param>
        /// <param name="normal">Projection plane normal vector.</param>
        /// <param name="d">Projection plane D component.</param>
        public static void Project(out Vector3 result, ref Vector3 value, ref Vector3 normal, float d)
        {
            float distance =
                normal.X * value.X +
                normal.Y * value.Y +
                normal.Z * value.Z +
                d;

            result.X = value.X - normal.X * distance;
            result.Y = value.Y - normal.Y * distance;
            result.Z = value.Z - normal.Z * distance;
        }

        /// <summary>
        /// Projects the specified vector on the specified plane.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Vector4"/>.</param>
        /// <param name="normal">Projection plane normal vector.</param>
        /// <param name="d">Projection plane D component.</param>
        public static void Project(out Vector4 result, ref Vector4 value, ref Vector3 normal, float d)
        {
            float distance =
                normal.X * value.X +
                normal.Y * value.Y +
                normal.Z * value.Z +
                d;

            result.X = value.X - normal.X * distance;
            result.Y = value.Y - normal.Y * distance;
            result.Z = value.Z - normal.Z * distance;
            result.W = value.W;
        }

        /// <summary>
        /// Projects the specified vector on the specified plane.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Plane"/>.</param>
        public static void Project(out Vector3 result, ref Vector3 value1, ref Plane value2)
        {
            float distance =
                value2.Normal.X * value1.X +
                value2.Normal.Y * value1.Y +
                value2.Normal.Z * value1.Z +
                value2.D;

            result.X = value1.X - value2.Normal.X * distance;
            result.Y = value1.Y - value2.Normal.Y * distance;
            result.Z = value1.Z - value2.Normal.Z * distance;
        }

        /// <summary>
        /// Projects the specified vector on the specified plane.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Plane"/>.</param>
        public static void Project(out Vector4 result, ref Vector4 value1, ref Plane value2)
        {
            float distance =
                value2.Normal.X * value1.X +
                value2.Normal.Y * value1.Y +
                value2.Normal.Z * value1.Z +
                value2.D;

            result.X = value1.X - value2.Normal.X * distance;
            result.Y = value1.Y - value2.Normal.Y * distance;
            result.Z = value1.Z - value2.Normal.Z * distance;
            result.W = value1.W;
        }

        /// <summary>
        /// Projects the specified vector to the specified viewport using the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Project(out Vector2 result, ref Vector2 value1, ref Matrix value2, float viewportWidth, float viewportHeight)
        {
            float x = value1.X * value2.M11 + value1.Y * value2.M12 + value2.M14;
            float y = value1.X * value2.M21 + value1.Y * value2.M22 + value2.M24;
            float w = 1 / (value1.X * value2.M41 + value1.Y * value2.M42 + value2.M44);

            result.X = (x * w + 1) * 0.5f * viewportWidth;
            result.Y = (y * w + 1) * 0.5f * viewportHeight;
        }

        /// <summary>
        /// Projects the specified vector to the specified viewport using the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Project(out Vector3 result, ref Vector3 value1, ref Matrix value2, float viewportWidth, float viewportHeight)
        {
            float x = value1.X * value2.M11 + value1.Y * value2.M12 + value1.Z * value2.M13 + value2.M14;
            float y = value1.X * value2.M21 + value1.Y * value2.M22 + value1.Z * value2.M23 + value2.M24;
            float z = value1.X * value2.M31 + value1.Y * value2.M32 + value1.Z * value2.M33 + value2.M34;
            float w = 1 / (value1.X * value2.M41 + value1.Y * value2.M42 + value1.Z * value2.M43 + value2.M44);

            result.X = (x * w + 1) * 0.5f * viewportWidth;
            result.Y = (y * w + 1) * 0.5f * viewportHeight;
            result.Z = (z * w + 1) * 0.5f;
        }

        /// <summary>
        /// Projects the specified vector to the specified viewport using the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Project(out Vector4 result, ref Vector2 value1, ref Matrix value2, float viewportWidth, float viewportHeight)
        {
            float x = value1.X * value2.M11 + value1.Y * value2.M12 + value2.M14;
            float y = value1.X * value2.M21 + value1.Y * value2.M22 + value2.M24;
            float z = value1.X * value2.M31 + value1.Y * value2.M32 + value2.M34;
            float w = value1.X * value2.M41 + value1.Y * value2.M42 + value2.M44;

            result.X = (x * w + 1) * 0.5f * viewportWidth;
            result.Y = (y * w + 1) * 0.5f * viewportHeight;
            result.Z = (z + 1) * 0.5f;
            result.W = w;
        }

        /// <summary>
        /// Projects the specified vector to the specified viewport using the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Project(out Vector4 result, ref Vector3 value1, ref Matrix value2, float viewportWidth, float viewportHeight)
        {
            float x = value1.X * value2.M11 + value1.Y * value2.M12 + value1.Z * value2.M13 + value2.M14;
            float y = value1.X * value2.M21 + value1.Y * value2.M22 + value1.Z * value2.M23 + value2.M24;
            float z = value1.X * value2.M31 + value1.Y * value2.M32 + value1.Z * value2.M33 + value2.M34;
            float w = value1.X * value2.M41 + value1.Y * value2.M42 + value1.Z * value2.M43 + value2.M44;

            result.X = (x * w + 1) * 0.5f * viewportWidth;
            result.Y = (y * w + 1) * 0.5f * viewportHeight;
            result.Z = (z + 1) * 0.5f;
            result.W = w;
        }

        /// <summary>
        /// Projects the specified vector to the specified viewport using the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Project(out Vector4 result, ref Vector4 value1, ref Matrix value2, float viewportWidth, float viewportHeight)
        {
            float x = value1.X * value2.M11 + value1.Y * value2.M12 + value1.Z * value2.M13 + value1.W * value2.M14;
            float y = value1.X * value2.M21 + value1.Y * value2.M22 + value1.Z * value2.M23 + value1.W * value2.M24;
            float z = value1.X * value2.M31 + value1.Y * value2.M32 + value1.Z * value2.M33 + value1.W * value2.M34;
            float w = value1.X * value2.M41 + value1.Y * value2.M42 + value1.Z * value2.M43 + value1.W * value2.M44;

            result.X = (x * w + 1) * 0.5f * viewportWidth;
            result.Y = (y * w + 1) * 0.5f * viewportHeight;
            result.Z = (z + 1) * 0.5f;
            result.W = w;
        }
    }
}
