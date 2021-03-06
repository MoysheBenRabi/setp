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
        /// Calculates a fast approximation of the reciprocal of the square root of the specified number.
        /// </summary>
        /// <param name="value">A number.</param>
        /// <returns>Approximated reciprocal of the square root of the number.</returns>
        public static float FastInvSqrt(float value)
        {
            Int32Single u = new Int32Single();
            u.f = value;
            u.i = 0x5F3759DF - (u.i >> 1);
            return (1.5f - value * 0.5f * u.f * u.f) * u.f;
        }
    }
}
