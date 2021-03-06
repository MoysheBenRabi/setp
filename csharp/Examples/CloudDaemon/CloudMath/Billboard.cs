﻿// Copyright (c) 2008 Vesa Tuomiaro
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
        /// Creates a billboard transformation for the specified object position.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="objectPosition">Billboard object position.</param>
        /// <param name="cameraPosition">Camera position.</param>
        /// <param name="cameraUpVector">The up vector of the camera.</param>
        public static void Billboard(out Matrix result, ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 cameraUpVector)
        {
            Vector3 localX, localY, localZ;

            Subtract(out localZ, ref cameraPosition, ref objectPosition);
            Normalize(out localZ, ref localZ);

            Cross(out localX, ref cameraUpVector, ref localZ);
            Normalize(out localX, ref localX);

            Cross(out localY, ref localZ, ref localX);
            Normalize(out localY, ref localY);

            result.M11 = localX.X;
            result.M12 = localY.X;
            result.M13 = localZ.X;
            result.M14 = objectPosition.X;
            result.M21 = localX.Y;
            result.M22 = localY.Y;
            result.M23 = localZ.Y;
            result.M24 = objectPosition.Y;
            result.M31 = localX.Z;
            result.M32 = localY.Z;
            result.M33 = localZ.Z;
            result.M34 = objectPosition.Z;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }
    }
}
