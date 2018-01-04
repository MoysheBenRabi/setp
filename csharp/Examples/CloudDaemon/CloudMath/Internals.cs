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
using System.Runtime.InteropServices;

namespace CloudMath
{
    public static partial class Common
    {
        /// <summary>
        /// Smallest allowed difference in floating point values.
        /// </summary>
        private const float Epsilon = 0.00001f;

        /// <summary>
        /// One minus epsilon.
        /// </summary>
        private const float OneMinusEpsilon = 1 - Epsilon;

        /// <summary>
        /// Union of a 32-bit integer and a IEEE single-precision floating point number.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct Int32Single
        {
            /// <summary>
            /// Integer value.
            /// </summary>
            [FieldOffset(0)]
            internal Int32 i;

            /// <summary>
            /// Floating point value.
            /// </summary>
            [FieldOffset(0)]
            internal Single f;
        }
    }
}
