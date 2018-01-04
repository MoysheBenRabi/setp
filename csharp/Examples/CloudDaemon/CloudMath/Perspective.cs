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
        /// Creates a perspective projection transformation.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="width">Near plane width.</param>
        /// <param name="height">Near plane height.</param>
        /// <param name="nearPlaneDistance">Near plane distance from camera.</param>
        /// <param name="farPlaneDistance">Far plane distance from camera.</param>
        public static void Perspective(out Matrix result, float width, float height, float nearPlaneDistance, float farPlaneDistance)
        {
            result.M11 = (2 * nearPlaneDistance) / width;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = (2 * nearPlaneDistance) / height;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = (farPlaneDistance + nearPlaneDistance) / (nearPlaneDistance - farPlaneDistance);
            result.M34 = (2 * farPlaneDistance * nearPlaneDistance) / (nearPlaneDistance - farPlaneDistance);
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = -1;
            result.M44 = 0;
        }

        /// <summary>
        /// Creates a perspective projection transformation with the specified on field of view.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="fieldOfViewInRadians">Field of view in radians.</param>
        /// <param name="width">Viewport width.</param>
        /// <param name="height">Viewport height.</param>
        /// <param name="nearPlaneDistance">Near plane distance from camera.</param>
        /// <param name="farPlaneDistance">Far plane distance from camera.</param>
        public static void Perspective(out Matrix result, float fieldOfViewInRadians, float width, float height, float nearPlaneDistance, float farPlaneDistance)
        {
            float tan = 1 / (float)System.Math.Tan(fieldOfViewInRadians * 0.5f);

            result.M11 = tan * (height / width);
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = tan;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = (farPlaneDistance + nearPlaneDistance) / (nearPlaneDistance - farPlaneDistance);
            result.M34 = (2 * farPlaneDistance * nearPlaneDistance) / (nearPlaneDistance - farPlaneDistance);
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = -1;
            result.M44 = 0;
        }
    }
}
