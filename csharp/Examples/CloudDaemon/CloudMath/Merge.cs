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
        /// Merges the specified boxes.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        public static void Merge(out BoundingBox result, ref BoundingBox value1, ref BoundingBox value2)
        {
            result.Minimum.X = System.Math.Min(value1.Minimum.X, value2.Minimum.X);
            result.Minimum.Y = System.Math.Min(value1.Minimum.Y, value2.Minimum.Y);
            result.Minimum.Z = System.Math.Min(value1.Minimum.Z, value2.Minimum.Z);
            result.Maximum.X = System.Math.Max(value1.Maximum.X, value2.Maximum.X);
            result.Maximum.Y = System.Math.Max(value1.Maximum.Y, value2.Maximum.Y);
            result.Maximum.Z = System.Math.Max(value1.Maximum.Z, value2.Maximum.Z);
        }

        /// <summary>
        /// Merges the specified spheres.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        public static void Merge(out Sphere result, ref Sphere value1, ref Sphere value2)
        {
            float x = value2.Position.X - value1.Position.X;
            float y = value2.Position.Y - value1.Position.Y;
            float z = value2.Position.Z - value1.Position.Z;
            
            float distance = (float)System.Math.Sqrt(x * x + y * y + z * z);

            if (value1.Radius - value2.Radius >= distance)
            {
                result = value1;
            }
            else if (value2.Radius - value1.Radius >= distance)
            {
                result = value2;
            }
            else
            {
                float radius = (distance + value1.Radius + value2.Radius) * 0.5f;
                float d = (radius - value1.Radius) / distance;

                result.Position.X = value1.Position.X + x * d;
                result.Position.Y = value1.Position.Y + y * d;
                result.Position.Z = value1.Position.Z + z * d;
                result.Radius = radius;
            }
        }
    }
}
