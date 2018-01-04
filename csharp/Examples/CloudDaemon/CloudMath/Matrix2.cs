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
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace CloudMath
{
    /// <summary>
    /// Representation of a matrix with 2 rows and 2 columns.
    /// </summary>
    [Serializable]
    [XmlType("matrix2")]
    [DebuggerDisplay("M11 = {M11} M22 = {M22}")]
    public partial struct Matrix2 : IEquatable<Matrix2>, IFormattable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="m11">Element at the 1st row, 1st column.</param>
        /// <param name="m12">Element at the 1st row, 2nd column.</param>
        /// <param name="m21">Element at the 2nd row, 1st column.</param>
        /// <param name="m22">Element at the 2nd row, 2nd column.</param>
        public Matrix2(float m11, float m12, float m21, float m22)
        {
            this.M11 = m11; this.M12 = m12;
            this.M21 = m21; this.M22 = m22;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="unitX">Unit vector for X axis.</param>
        /// <param name="unitY">Unit vector for Y axis.</param>
        public Matrix2(ref Vector2 unitX, ref Vector2 unitY)
        {
            this.M11 = unitX.X; this.M12 = unitY.X;
            this.M21 = unitX.Y; this.M22 = unitY.Y;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Indicates whether the current object and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> and the matrix are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Matrix2)
            {
                return Equals((Matrix2)obj);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix2 other)
        {
            return
                this.M11 == other.M11 &&
                this.M12 == other.M12 &&
                this.M21 == other.M21 &&
                this.M22 == other.M22;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="delta">Maximum allowed error.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix2 other, float delta)
        {
            return
                System.Math.Abs(this.M11 - other.M11) <= delta &&
                System.Math.Abs(this.M12 - other.M12) <= delta &&
                System.Math.Abs(this.M21 - other.M21) <= delta &&
                System.Math.Abs(this.M22 - other.M22) <= delta;
        }

        /// <summary>
        /// Returns the hash code for the current object.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for the matrix.</returns>
        public override int GetHashCode()
        {
            return
                this.M11.GetHashCode() +
                this.M12.GetHashCode() +
                this.M21.GetHashCode() +
                this.M22.GetHashCode();
        }

        /// <summary>
        /// Gets the string representation of the current object.
        /// </summary>
        /// <returns>A <see cref="String"/> containing a human-readable representation of the object.</returns>
        public override string ToString()
        {
            return this.ToString("F", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats the value of the current object using the specified format.
        /// </summary>
        /// <param name="format">The <see cref="System.String"/> specifying the format to use. -or- null to use the default format defined for the type of the <see cref="System.IFormattable"/> implementation. </param>
        /// <param name="formatProvider">The <see cref="System.IFormatProvider"/> to use to format the value.-or- null to obtain the numeric format information from the current locale setting of the operating system. </param>
        /// <returns>A <see cref="String"/> containing a human-readable representation of the object.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append('[');
            sb.Append(this.M11.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M12.ToString(format, formatProvider));
            sb.AppendLine("]");

            sb.Append('[');
            sb.Append(this.M21.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M22.ToString(format, formatProvider));
            sb.AppendLine("]");

            return sb.ToString();
        }


        /// <summary>
        /// Gets an array containing the matrix elements.
        /// </summary>
        /// <returns>An array containing the matrix elements.</returns>
        public float[] ToArray()
        {
            return ToArray(MatrixElementOrder.ColumnMajor);
        }

        /// <summary>
        /// Gets an array containing the matrix elements.
        /// </summary>
        /// <param name="elementOrder">Matrix element order.</param>
        /// <returns>An array containing the matrix elements.</returns>
        public float[] ToArray(MatrixElementOrder elementOrder)
        {
            if (elementOrder == MatrixElementOrder.RowMajor)
            {
                return new float[] { 
                    this.M11, 
                    this.M12, 
                    this.M21, 
                    this.M22, 
                };
            }
            else
            {
                return new float[] { 
                    this.M11, 
                    this.M21, 
                    this.M12, 
                    this.M22, 
                };
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Determines whether two instances are equal.
        /// </summary>
        /// <param name="value1">A <see cref="Matrix2"/>.</param>
        /// <param name="value2">A <see cref="Matrix2"/>.</param>
        /// <returns>A boolean value indicating whether the instances are equal.</returns>
        public static bool operator ==(Matrix2 value1, Matrix2 value2)
        {
            return
                value1.M11 == value2.M11 &&
                value1.M12 == value2.M12 &&
                value1.M21 == value2.M21 &&
                value1.M22 == value2.M22;
        }

        /// <summary>
        /// Determines whether two instances are not equal.
        /// </summary>
        /// <param name="value1">A <see cref="Matrix2"/>.</param>
        /// <param name="value2">A <see cref="Matrix2"/>.</param>
        /// <returns>A boolean value indicating whether the instances are not equal.</returns>
        public static bool operator !=(Matrix2 value1, Matrix2 value2)
        {
            return
                value1.M11 != value2.M11 ||
                value1.M12 != value2.M12 ||
                value1.M21 != value2.M21 ||
                value1.M22 != value2.M22;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the empty matrix.
        /// </summary>
        /// <value>
        /// A <see cref="Matrix2"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Matrix2 Empty
        {
            get { return empty; }
        }

        /// <summary>
        /// Gets the identity matrix.
        /// </summary>
        /// <value>
        /// A <see cref="Matrix2"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Matrix2 Identity
        {
            get { return identity; }
        }

        /// <summary>
        /// Gets the trace of the matrix.
        /// </summary>
        /// <value>
        /// Trace of the matrix.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        [XmlIgnore]
        public float Trace
        {
            get { return this.M11 + this.M22; }
        }

        /// <summary>
        /// Gets the determinant of the matrix.
        /// </summary>
        /// <value>
        /// Determinant of the matrix.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        [XmlIgnore]
        public float Determinant
        {
            get { return this.M11 * this.M22 - this.M12 * this.M21; }
        }

        /// <summary>
        /// Gets or sets the unit vector pointing to the left.
        /// </summary>
        /// <value>
        /// A <see cref="Vector2"/>.
        /// </value>
        [XmlIgnore]
        public Vector2 Left
        {
            get
            {
                Vector2 value;
                value.X = -this.M11;
                value.Y = -this.M21;
                return value;
            }
            set
            {
                this.M11 = -value.X;
                this.M21 = -value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the unit vector pointing to the right.
        /// </summary>
        /// <value>
        /// A <see cref="Vector2"/>.
        /// </value>
        [XmlIgnore]
        public Vector2 Right
        {
            get
            {
                Vector2 value;
                value.X = this.M11;
                value.Y = this.M21;
                return value;
            }
            set
            {
                this.M11 = value.X;
                this.M21 = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the unit vector pointing up.
        /// </summary>
        /// <value>
        /// A <see cref="Vector2"/>.
        /// </value>
        [XmlIgnore]
        public Vector2 Up
        {
            get
            {
                Vector2 value;
                value.X = this.M12;
                value.Y = this.M22;
                return value;
            }
            set
            {
                this.M12 = value.X;
                this.M22 = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the unit vector pointing down.
        /// </summary>
        /// <value>
        /// A <see cref="Vector2"/>.
        /// </value>
        [XmlIgnore]
        public Vector2 Down
        {
            get
            {
                Vector2 value;
                value.X = -this.M12;
                value.Y = -this.M22;
                return value;
            }
            set
            {
                this.M12 = -value.X;
                this.M22 = -value.Y;
            }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Empty matrix.
        /// </summary>
        private static readonly Matrix2 empty = new Matrix2();

        /// <summary>
        /// Identity matrix.
        /// </summary>
        private static readonly Matrix2 identity = new Matrix2(1, 0, 0, 1);

        /// <summary>
        /// Element at the 1st row, 1st column.
        /// </summary>
        [XmlAttribute("m11")]
        public float M11;

        /// <summary>
        /// Element at the 2nd row, 1st column.
        /// </summary>
        [XmlAttribute("m21")]
        public float M21;

        /// <summary>
        /// Element at the 1st row, 2nd column.
        /// </summary>
        [XmlAttribute("m12")]
        public float M12;

        /// <summary>
        /// Element at the 2nd row, 2nd column.
        /// </summary>
        [XmlAttribute("m22")]
        public float M22;
        #endregion
    }
}
