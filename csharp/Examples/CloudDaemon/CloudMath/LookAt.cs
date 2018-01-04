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
        /// Creates a "look at" view transformation from the specified target position, camera rotation and camera position offset.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="targetPosition">Target position.</param>
        /// <param name="cameraRotation">Camera rotation.</param>
        /// <param name="cameraPositionOffset">Camera position offset.</param>
        public static void LookAt(out Matrix result, ref Vector3 targetPosition, ref Quaternion cameraRotation, ref Vector3 cameraPositionOffset)
        {
            float ii = cameraRotation.I * cameraRotation.I;
            float ij = cameraRotation.I * cameraRotation.J;
            float ik = cameraRotation.I * cameraRotation.K;
            float iw = cameraRotation.I * cameraRotation.W;
            float jj = cameraRotation.J * cameraRotation.J;
            float jk = cameraRotation.J * cameraRotation.K;
            float jw = cameraRotation.J * cameraRotation.W;
            float kk = cameraRotation.K * cameraRotation.K;
            float kw = cameraRotation.K * cameraRotation.W;

            Vector3 x;
            x.X = 1 - 2 * (jj + kk);
            x.Y = 2 * (ij + kw);
            x.Z = 2 * (ik - jw);

            Vector3 y;
            y.X = 2 * (ij - kw);
            y.Y = 1 - 2 * (ii + kk);
            y.Z = 2 * (jk + iw);

            Vector3 z;
            z.X = 2 * (ik + jw);
            z.Y = 2 * (jk - iw);
            z.Z = 1 - 2 * (ii + jj);

            result.M11 = x.X;
            result.M12 = x.Y;
            result.M13 = x.Z;
            result.M14 = -x.X * targetPosition.X - x.Y * targetPosition.Y - x.Z * targetPosition.Z - cameraPositionOffset.X;
            result.M21 = y.X;
            result.M22 = y.Y;
            result.M23 = y.Z;
            result.M24 = -y.X * targetPosition.X - y.Y * targetPosition.Y - y.Z * targetPosition.Z - cameraPositionOffset.Y;
            result.M31 = z.X;
            result.M32 = z.Y;
            result.M33 = z.Z;
            result.M34 = -z.X * targetPosition.X - z.Y * targetPosition.Y - z.Z * targetPosition.Z - cameraPositionOffset.Z;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Creates a "look at" view transformation from the specified target position, camera up vector and camera position.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="targetPosition">Target position.</param>
        /// <param name="cameraUpVector">The up vector of the camera.</param>
        /// <param name="cameraPosition">Camera position.</param>
        public static void LookAt(out Matrix result, ref Vector3 targetPosition, ref Vector3 cameraUpVector, ref Vector3 cameraPosition)
        {
            Vector3 x, y, z;

            Subtract(out z, ref cameraPosition, ref targetPosition);
            Normalize(out z, ref z);

            Cross(out x, ref cameraUpVector, ref z);
            Normalize(out x, ref x);

            Cross(out y, ref z, ref x);
            Normalize(out y, ref y);

            result.M11 = x.X;
            result.M12 = x.Y;
            result.M13 = x.Z;
            result.M14 = -x.X * cameraPosition.X - x.Y * cameraPosition.Y - x.Z * cameraPosition.Z;
            result.M21 = y.X;
            result.M22 = y.Y;
            result.M23 = y.Z;
            result.M24 = -y.X * cameraPosition.X - y.Y * cameraPosition.Y - y.Z * cameraPosition.Z;
            result.M31 = z.X;
            result.M32 = z.Y;
            result.M33 = z.Z;
            result.M34 = -z.X * cameraPosition.X - z.Y * cameraPosition.Y - z.Z * cameraPosition.Z;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }
    }
}
