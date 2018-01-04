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
        /// Unprojects the specified vector from the specified viewport using the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Unproject(out Vector2 result, ref Vector2 value1, ref Matrix value2, float viewportWidth, float viewportHeight)
        {
            Matrix matrix;
            Invert(out matrix, ref value2);

            float x = (value1.X / viewportWidth) * 2 - 1;
            float y = (value1.Y / viewportHeight) * 2 - 1;

            float dx = x * matrix.M11 + y * matrix.M12 + matrix.M14;
            float dy = x * matrix.M21 + y * matrix.M22 + matrix.M24;
            float dw = 1 / (x * matrix.M41 + y * matrix.M42 + matrix.M44);

            result.X = dx * dw;
            result.Y = dy * dw;
        }

        /// <summary>
        /// Unprojects the specified vector from the specified viewport using the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Unproject(out Vector3 result, ref Vector3 value1, ref Matrix value2, float viewportWidth, float viewportHeight)
        {
            Matrix matrix;
            Invert(out matrix, ref value2);

            float x = (value1.X / viewportWidth) * 2 - 1;
            float y = (value1.Y / viewportHeight) * 2 - 1;
            float z = value1.Z * 2 - 1;

            float dx = (x * matrix.M11) + (y * matrix.M12) + (z * matrix.M13) + matrix.M14;
            float dy = (x * matrix.M21) + (y * matrix.M22) + (z * matrix.M23) + matrix.M24;
            float dz = (x * matrix.M31) + (y * matrix.M32) + (z * matrix.M33) + matrix.M34;
            float dw = 1 / ((x * matrix.M41) + (y * matrix.M42) + (z * matrix.M43) + matrix.M44);

            result.X = dx * dw;
            result.Y = dy * dw;
            result.Z = dz * dw;
        }

        /// <summary>
        /// Unprojects the specified vector from the specified viewport using the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Unproject(out Vector4 result, ref Vector2 value1, ref Matrix value2, float viewportWidth, float viewportHeight)
        {
            Matrix matrix;
            Invert(out matrix, ref value2);

            float x = (value1.X / viewportWidth) * 2 - 1;
            float y = (value1.Y / viewportHeight) * 2 - 1;

            float dx = x * matrix.M11 + y * matrix.M12 + matrix.M14;
            float dy = x * matrix.M21 + y * matrix.M22 + matrix.M24;
            float dz = x * matrix.M31 + y * matrix.M32 + matrix.M34;
            float dw = x * matrix.M41 + y * matrix.M42 + matrix.M44;

            result.X = dx;
            result.Y = dy;
            result.Z = dz;
            result.W = dw;
        }

        /// <summary>
        /// Unprojects the specified vector from the specified viewport using the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Unproject(out Vector4 result, ref Vector3 value1, ref Matrix value2, float viewportWidth, float viewportHeight)
        {
            Matrix matrix;
            Invert(out matrix, ref value2);

            float x = (value1.X / viewportWidth) * 2 - 1;
            float y = (value1.Y / viewportHeight) * 2 - 1;
            float z = value1.Z * 2 - 1;

            float dx = x * matrix.M11 + y * matrix.M12 + z * matrix.M13 + matrix.M14;
            float dy = x * matrix.M21 + y * matrix.M22 + z * matrix.M23 + matrix.M24;
            float dz = x * matrix.M31 + y * matrix.M32 + z * matrix.M33 + matrix.M34;
            float dw = x * matrix.M41 + y * matrix.M42 + z * matrix.M43 + matrix.M44;

            result.X = dx;
            result.Y = dy;
            result.Z = dz;
            result.W = dw;
        }

        /// <summary>
        /// Unprojects the specified vector from the specified viewport using the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Unproject(out Vector4 result, ref Vector4 value1, ref Matrix value2, float viewportWidth, float viewportHeight)
        {
            Matrix matrix;
            Invert(out matrix, ref value2);

            float x = (value1.X / viewportWidth) * 2 - 1;
            float y = (value1.Y / viewportHeight) * 2 - 1;
            float z = value1.Z * 2 - 1;
            float w = value1.W;

            float dx = x * matrix.M11 + y * matrix.M12 + z * matrix.M13 + w * matrix.M14;
            float dy = x * matrix.M21 + y * matrix.M22 + z * matrix.M23 + w * matrix.M24;
            float dz = x * matrix.M31 + y * matrix.M32 + z * matrix.M33 + w * matrix.M34;
            float dw = x * matrix.M41 + y * matrix.M42 + z * matrix.M43 + w * matrix.M44;

            result.X = dx;
            result.Y = dy;
            result.Z = dz;
            result.W = dw;
        }
    }
}
