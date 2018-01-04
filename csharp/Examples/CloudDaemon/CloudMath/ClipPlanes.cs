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
        /// Gets the clip planes from the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        public static void ClipPlanes(out Volume result, ref Matrix value)
        {
            result.ClipPlane1.Normal.X = value.M41 + value.M11;
            result.ClipPlane1.Normal.Y = value.M42 + value.M12;
            result.ClipPlane1.Normal.Z = value.M43 + value.M13;
            result.ClipPlane1.D = value.M44 + value.M14;

            result.ClipPlane2.Normal.X = value.M41 - value.M11;
            result.ClipPlane2.Normal.Y = value.M42 - value.M12;
            result.ClipPlane2.Normal.Z = value.M43 - value.M13;
            result.ClipPlane2.D = value.M44 - value.M14;

            result.ClipPlane3.Normal.X = value.M41 - value.M21;
            result.ClipPlane3.Normal.Y = value.M42 - value.M22;
            result.ClipPlane3.Normal.Z = value.M43 - value.M23;
            result.ClipPlane3.D = value.M44 - value.M24;

            result.ClipPlane4.Normal.X = value.M41 + value.M21;
            result.ClipPlane4.Normal.Y = value.M42 + value.M22;
            result.ClipPlane4.Normal.Z = value.M43 + value.M23;
            result.ClipPlane4.D = value.M44 + value.M24;

            result.ClipPlane5.Normal.X = value.M41 + value.M31;
            result.ClipPlane5.Normal.Y = value.M42 + value.M32;
            result.ClipPlane5.Normal.Z = value.M43 + value.M33;
            result.ClipPlane5.D = value.M44 + value.M34;

            result.ClipPlane6.Normal.X = value.M41 - value.M31;
            result.ClipPlane6.Normal.Y = value.M42 - value.M32;
            result.ClipPlane6.Normal.Z = value.M43 - value.M33;
            result.ClipPlane6.D = value.M44 - value.M34;
        }

        /// <summary>
        /// Gets the clip planes from the specified matrix.
        /// </summary>
        /// <param name="left">Output variable for the left clip plane.</param>
        /// <param name="right">Output variable for the right clip plane.</param>
        /// <param name="top">Output variable for the top clip plane.</param>
        /// <param name="bottom">Output variable for the bottom clip plane.</param>
        /// <param name="near">Output variable for the near clip plane.</param>
        /// <param name="far">Output variable for the far clip plane.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        public static void ClipPlanes(out Plane left, out Plane right, out Plane top, out Plane bottom, out Plane near, out Plane far, ref Matrix value)
        {
            left.Normal.X = value.M41 + value.M11;
            left.Normal.Y = value.M42 + value.M12;
            left.Normal.Z = value.M43 + value.M13;
            left.D = value.M44 + value.M14;

            right.Normal.X = value.M41 - value.M11;
            right.Normal.Y = value.M42 - value.M12;
            right.Normal.Z = value.M43 - value.M13;
            right.D = value.M44 - value.M14;

            top.Normal.X = value.M41 - value.M21;
            top.Normal.Y = value.M42 - value.M22;
            top.Normal.Z = value.M43 - value.M23;
            top.D = value.M44 - value.M24;

            bottom.Normal.X = value.M41 + value.M21;
            bottom.Normal.Y = value.M42 + value.M22;
            bottom.Normal.Z = value.M43 + value.M23;
            bottom.D = value.M44 + value.M24;

            near.Normal.X = value.M41 + value.M31;
            near.Normal.Y = value.M42 + value.M32;
            near.Normal.Z = value.M43 + value.M33;
            near.D = value.M44 + value.M34;

            far.Normal.X = value.M41 - value.M31;
            far.Normal.Y = value.M42 - value.M32;
            far.Normal.Z = value.M43 - value.M33;
            far.D = value.M44 - value.M34;
        }
    }
}
