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
        /// Creates a planar shadow transformation.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="lightDirection">Light direction vector.</param>
        /// <param name="normal">Receiver plane normal vector.</param>
        /// <param name="d">Receiver plane D component.</param>
        public static void PlanarShadow(out Matrix result, ref Vector3 lightDirection, ref Vector3 normal, float d)
        {
            float dot = lightDirection.X * normal.X + lightDirection.Y * normal.Y + lightDirection.Z * normal.Z;
            float distance = dot + d;

            result.M11 = lightDirection.X * normal.X + distance;
            result.M12 = lightDirection.X * normal.Y;
            result.M13 = lightDirection.X * normal.Z;
            result.M14 = lightDirection.X * d;
            result.M21 = lightDirection.Y * normal.X;
            result.M22 = lightDirection.Y * normal.Y + distance;
            result.M23 = lightDirection.Y * normal.Z;
            result.M24 = lightDirection.Y * d;
            result.M31 = lightDirection.Z * normal.X;
            result.M32 = lightDirection.Z * normal.Y;
            result.M33 = lightDirection.Z * normal.Z + distance;
            result.M34 = lightDirection.Z * d;
            result.M41 = -normal.X;
            result.M42 = -normal.Y;
            result.M43 = -normal.Z;
            result.M44 = dot;
        }
    }
}
