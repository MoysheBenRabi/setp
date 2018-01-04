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
        /// Gets the minor of a matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix3"/>.</param>
        /// <param name="row">Row number to exlude. In range [1, 3].</param>
        /// <param name="column">Column number to exlude. In range [1, 3].</param>
        public static void Minor(out Matrix2 result, ref Matrix value, int row, int column)
        {
            switch (row)
            {
                case 1:
                    switch (column)
                    {
                        case 1:
                            result.M11 = value.M22;
                            result.M12 = value.M23;
                            result.M21 = value.M32;
                            result.M22 = value.M33;
                            break;

                        case 2:
                            result.M11 = value.M21;
                            result.M12 = value.M23;
                            result.M21 = value.M31;
                            result.M22 = value.M33;
                            break;

                        case 3:
                            result.M11 = value.M21;
                            result.M12 = value.M22;
                            result.M21 = value.M31;
                            result.M22 = value.M32;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("column");
                    }
                    break;

                case 2:
                    switch (column)
                    {
                        case 1:
                            result.M11 = value.M12;
                            result.M12 = value.M13;
                            result.M21 = value.M32;
                            result.M22 = value.M33;
                            break;

                        case 2:
                            result.M11 = value.M11;
                            result.M12 = value.M13;
                            result.M21 = value.M31;
                            result.M22 = value.M33;
                            break;

                        case 3:
                            result.M11 = value.M11;
                            result.M12 = value.M12;
                            result.M21 = value.M31;
                            result.M22 = value.M32;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("column");
                    }
                    break;

                case 3:
                    switch (column)
                    {
                        case 1:
                            result.M11 = value.M12;
                            result.M12 = value.M13;
                            result.M21 = value.M22;
                            result.M22 = value.M23;
                            break;

                        case 2:
                            result.M11 = value.M11;
                            result.M12 = value.M13;
                            result.M21 = value.M21;
                            result.M22 = value.M23;
                            break;

                        case 3:
                            result.M11 = value.M11;
                            result.M12 = value.M12;
                            result.M21 = value.M21;
                            result.M22 = value.M22;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("column");
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException("row");
            }
        }

        /// <summary>
        /// Gets the minor of a matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        /// <param name="row">Row number to exlude. In range [1, 4].</param>
        /// <param name="column">Column number to exlude. In range [1, 4].</param>
        public static void Minor(out Matrix3 result, ref Matrix value, int row, int column)
        {
            switch (row)
            {
                case 1:
                    switch (column)
                    {
                        case 1:
                            result.M11 = value.M22;
                            result.M12 = value.M23;
                            result.M13 = value.M24;
                            result.M21 = value.M32;
                            result.M22 = value.M33;
                            result.M23 = value.M34;
                            result.M31 = value.M42;
                            result.M32 = value.M43;
                            result.M33 = value.M44;
                            break;

                        case 2:
                            result.M11 = value.M21;
                            result.M12 = value.M23;
                            result.M13 = value.M24;
                            result.M21 = value.M31;
                            result.M22 = value.M33;
                            result.M23 = value.M34;
                            result.M31 = value.M41;
                            result.M32 = value.M43;
                            result.M33 = value.M44;
                            break;

                        case 3:
                            result.M11 = value.M21;
                            result.M12 = value.M22;
                            result.M13 = value.M24;
                            result.M21 = value.M31;
                            result.M22 = value.M32;
                            result.M23 = value.M34;
                            result.M31 = value.M41;
                            result.M32 = value.M42;
                            result.M33 = value.M44;
                            break;

                        case 4:
                            result.M11 = value.M21;
                            result.M12 = value.M22;
                            result.M13 = value.M23;
                            result.M21 = value.M31;
                            result.M22 = value.M32;
                            result.M23 = value.M33;
                            result.M31 = value.M41;
                            result.M32 = value.M42;
                            result.M33 = value.M43;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("column");
                    }
                    break;

                case 2:
                    switch (column)
                    {
                        case 1:
                            result.M11 = value.M12;
                            result.M12 = value.M13;
                            result.M13 = value.M14;
                            result.M21 = value.M32;
                            result.M22 = value.M33;
                            result.M23 = value.M34;
                            result.M31 = value.M42;
                            result.M32 = value.M43;
                            result.M33 = value.M44;
                            break;

                        case 2:
                            result.M11 = value.M11;
                            result.M12 = value.M13;
                            result.M13 = value.M14;
                            result.M21 = value.M31;
                            result.M22 = value.M33;
                            result.M23 = value.M34;
                            result.M31 = value.M41;
                            result.M32 = value.M43;
                            result.M33 = value.M44;
                            break;

                        case 3:
                            result.M11 = value.M11;
                            result.M12 = value.M12;
                            result.M13 = value.M14;
                            result.M21 = value.M31;
                            result.M22 = value.M32;
                            result.M23 = value.M34;
                            result.M31 = value.M41;
                            result.M32 = value.M42;
                            result.M33 = value.M44;
                            break;

                        case 4:
                            result.M11 = value.M11;
                            result.M12 = value.M12;
                            result.M13 = value.M13;
                            result.M21 = value.M31;
                            result.M22 = value.M32;
                            result.M23 = value.M33;
                            result.M31 = value.M41;
                            result.M32 = value.M42;
                            result.M33 = value.M43;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("column");
                    }
                    break;

                case 3:
                    switch (column)
                    {
                        case 1:
                            result.M11 = value.M12;
                            result.M12 = value.M13;
                            result.M13 = value.M14;
                            result.M21 = value.M22;
                            result.M22 = value.M23;
                            result.M23 = value.M24;
                            result.M31 = value.M42;
                            result.M32 = value.M43;
                            result.M33 = value.M44;
                            break;

                        case 2:
                            result.M11 = value.M11;
                            result.M12 = value.M13;
                            result.M13 = value.M14;
                            result.M21 = value.M21;
                            result.M22 = value.M23;
                            result.M23 = value.M24;
                            result.M31 = value.M41;
                            result.M32 = value.M43;
                            result.M33 = value.M44;
                            break;

                        case 3:
                            result.M11 = value.M11;
                            result.M12 = value.M12;
                            result.M13 = value.M14;
                            result.M21 = value.M21;
                            result.M22 = value.M22;
                            result.M23 = value.M24;
                            result.M31 = value.M41;
                            result.M32 = value.M42;
                            result.M33 = value.M44;
                            break;

                        case 4:
                            result.M11 = value.M11;
                            result.M12 = value.M12;
                            result.M13 = value.M13;
                            result.M21 = value.M21;
                            result.M22 = value.M22;
                            result.M23 = value.M23;
                            result.M31 = value.M41;
                            result.M32 = value.M42;
                            result.M33 = value.M43;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("column");
                    }
                    break;

                case 4:
                    switch (column)
                    {
                        case 1:
                            result.M11 = value.M12;
                            result.M12 = value.M13;
                            result.M13 = value.M14;
                            result.M21 = value.M22;
                            result.M22 = value.M23;
                            result.M23 = value.M24;
                            result.M31 = value.M32;
                            result.M32 = value.M33;
                            result.M33 = value.M34;
                            break;

                        case 2:
                            result.M11 = value.M11;
                            result.M12 = value.M13;
                            result.M13 = value.M14;
                            result.M21 = value.M21;
                            result.M22 = value.M23;
                            result.M23 = value.M24;
                            result.M31 = value.M31;
                            result.M32 = value.M33;
                            result.M33 = value.M34;
                            break;

                        case 3:
                            result.M11 = value.M11;
                            result.M12 = value.M12;
                            result.M13 = value.M14;
                            result.M21 = value.M21;
                            result.M22 = value.M22;
                            result.M23 = value.M24;
                            result.M31 = value.M31;
                            result.M32 = value.M32;
                            result.M33 = value.M34;
                            break;

                        case 4:
                            result.M11 = value.M11;
                            result.M12 = value.M12;
                            result.M13 = value.M13;
                            result.M21 = value.M21;
                            result.M22 = value.M22;
                            result.M23 = value.M23;
                            result.M31 = value.M31;
                            result.M32 = value.M32;
                            result.M33 = value.M33;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("column");
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException("row");
            }
        }
    }
}
