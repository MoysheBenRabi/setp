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
        /// Creates a model transformation from the specified model rotation and position.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="modelRotation">Model rotation.</param>
        /// <param name="modelPosition">Model position.</param>
        public static void Model(out Matrix result, ref Quaternion modelRotation, ref Vector3 modelPosition)
        {
            float ii = modelRotation.I * modelRotation.I;
            float ij = modelRotation.I * modelRotation.J;
            float ik = modelRotation.I * modelRotation.K;
            float iw = modelRotation.I * modelRotation.W;
            float jj = modelRotation.J * modelRotation.J;
            float jk = modelRotation.J * modelRotation.K;
            float jw = modelRotation.J * modelRotation.W;
            float kk = modelRotation.K * modelRotation.K;
            float kw = modelRotation.K * modelRotation.W;

            result.M11 = 1 - 2 * (jj + kk);
            result.M12 = 2 * (ij - kw);
            result.M13 = 2 * (ik + jw);
            result.M14 = modelPosition.X;
            result.M21 = 2 * (ij + kw);
            result.M22 = 1 - 2 * (ii + kk);
            result.M23 = 2 * (jk - iw);
            result.M24 = modelPosition.Y;
            result.M31 = 2 * (ik - jw);
            result.M32 = 2 * (jk + iw);
            result.M33 = 1 - 2 * (ii + jj);
            result.M34 = modelPosition.Z;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Creates a model transformation from the specified object direction vectors and position.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="modelForwardVector">The forward vector of the model.</param>
        /// <param name="modelUpVector">The up vector of the model.</param>
        /// <param name="modelPosition">Model position.</param>
        public static void Model(out Matrix result, ref Vector3 modelForwardVector, ref Vector3 modelUpVector, ref Vector3 modelPosition)
        {
            Vector3 x, y, z;

            Negate(out z, ref modelForwardVector);
            Normalize(out z, ref z);

            Cross(out x, ref modelUpVector, ref z);
            Normalize(out x, ref x);

            Cross(out y, ref z, ref x);
            Normalize(out y, ref y);

            result.M11 = x.X;
            result.M12 = y.X;
            result.M13 = z.X;
            result.M14 = modelPosition.X;
            result.M21 = x.Y;
            result.M22 = y.Y;
            result.M23 = z.Y;
            result.M24 = modelPosition.Y;
            result.M31 = x.Z;
            result.M32 = y.Z;
            result.M33 = z.Z;
            result.M34 = modelPosition.Z;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }
    }
}
