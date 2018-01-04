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
        /// Calculates the middle point of the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Vector2"/>.</param>
        public static void MidPoint(out Vector2 result, ref Vector2 value1, ref Vector2 value2)
        {
            result.X = (value1.X + value2.X) * 0.5f;
            result.Y = (value1.Y + value2.Y) * 0.5f;
        }

        /// <summary>
        /// Calculates the middle point of the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        public static void MidPoint(out Vector3 result, ref Vector3 value1, ref Vector3 value2)
        {
            result.X = (value1.X + value2.X) * 0.5f;
            result.Y = (value1.Y + value2.Y) * 0.5f;
            result.Z = (value1.Z + value2.Z) * 0.5f;
        }

        /// <summary>
        /// Calculates the middle point of the specified vectors.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        public static void MidPoint(out Vector4 result, ref Vector4 value1, ref Vector4 value2)
        {
            result.X = (value1.X + value2.X) * 0.5f;
            result.Y = (value1.Y + value2.Y) * 0.5f;
            result.Z = (value1.Z + value2.Z) * 0.5f;
            result.W = (value1.W + value2.W) * 0.5f;
        }
    }
}
