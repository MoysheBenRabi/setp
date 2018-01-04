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
        /// Creates a selection projection transformation.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="x">Selection rectangle offset on X axis from viewport origin.</param>
        /// <param name="y">Selection rectangle offset on Y axis from viewport origin.</param>
        /// <param name="width">Selection rectangle width.</param>
        /// <param name="height">Selection rectangle height.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Selection(out Matrix result, float x, float y, float width, float height, float viewportWidth, float viewportHeight)
        {
            result.M11 = viewportWidth / width;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = (viewportWidth - 2 * x) / width - 1;
            result.M21 = 0;
            result.M22 = viewportHeight / height;
            result.M23 = 0;
            result.M24 = (viewportHeight - 2 * y) / height - 1;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = 1;
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Applies a selection projection to the specified projection matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        /// <param name="x">Selection rectangle offset on X axis from viewport origin.</param>
        /// <param name="y">Selection rectangle offset on Y axis from viewport origin.</param>
        /// <param name="width">Selection rectangle width.</param>
        /// <param name="height">Selection rectangle height.</param>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        public static void Selection(out Matrix result, ref Matrix value, float x, float y, float width, float height, float viewportWidth, float viewportHeight)
        {
            float s11 = viewportWidth / width;
            float s22 = viewportHeight / height;
            float s14 = (viewportWidth - 2 * x) / width - 1;
            float s24 = (viewportHeight - 2 * y) / height - 1;

            result.M11 = s11 * value.M11 + s14 * value.M41;
            result.M12 = s11 * value.M12 + s14 * value.M42;
            result.M13 = s11 * value.M13 + s14 * value.M43;
            result.M14 = s11 * value.M14 + s14 * value.M44;
            result.M21 = s22 * value.M21 + s24 * value.M41;
            result.M22 = s22 * value.M22 + s24 * value.M42;
            result.M23 = s22 * value.M23 + s24 * value.M43;
            result.M24 = s22 * value.M24 + s24 * value.M44;
            result.M31 = value.M31;
            result.M32 = value.M32;
            result.M33 = value.M33;
            result.M34 = value.M34;
            result.M41 = value.M41;
            result.M42 = value.M42;
            result.M43 = value.M43;
            result.M44 = value.M44;
        }
    }
}
