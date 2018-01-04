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
        /// Creates a projection transformation from view frustum parameters.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="left">X coordinate of the left frustum plane.</param>
        /// <param name="right">X coordinate of the right frustum plane.</param>
        /// <param name="bottom">Y coordinate of the bottom frustum plane.</param>
        /// <param name="top">Y coordinate of the top frustum plane.</param>
        /// <param name="near">Z coordinate of the near frustum plane.</param>
        /// <param name="far">Z coordinate of the far frustum plane.</param>
        public static void Frustum(out Matrix result, float left, float right, float bottom, float top, float near, float far)
        {
            float width = right - left;
            float height = top - bottom;
            float depth = far - near;

            result.M11 = 2 * near / width;
            result.M12 = 0;
            result.M13 = (right + left) / width;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = 2 * near / height;
            result.M23 = (top + bottom) / height;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = -(far + near) / depth;
            result.M34 = -(2 * far * near) / depth;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = -1;
            result.M44 = 0;
        }
    }
}
