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
    /// Representation of a matrix with 4 rows and 4 columns.
    /// </summary>
    [Serializable]
    [XmlType("matrix4")]
    [DebuggerDisplay("M11 = {M11} M22 = {M22} M33 = {M33} M44 = {M44}")]
    public partial struct Matrix : IEquatable<Matrix>, IFormattable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="m11">Element at the 1st row, 1st column.</param>
        /// <param name="m12">Element at the 1st row, 2nd column.</param>
        /// <param name="m13">Element at the 1st row, 3rd column.</param>
        /// <param name="m14">Element at the 1st row, 4th column.</param>
        /// <param name="m21">Element at the 2nd row, 1st column.</param>
        /// <param name="m22">Element at the 2nd row, 2nd column.</param>
        /// <param name="m23">Element at the 2nd row, 3rd column.</param>
        /// <param name="m24">Element at the 2nd row, 4th column.</param>
        /// <param name="m31">Element at the 3rd row, 1st column.</param>
        /// <param name="m32">Element at the 3rd row, 2nd column.</param>
        /// <param name="m33">Element at the 3rd row, 3rd column.</param>
        /// <param name="m34">Element at the 3rd row, 4th column.</param>
        /// <param name="m41">Element at the 4th row, 1st column.</param>
        /// <param name="m42">Element at the 4th row, 2nd column.</param>
        /// <param name="m43">Element at the 4th row, 3rd column.</param>
        /// <param name="m44">Element at the 4th row, 4th column.</param>
        public Matrix(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            this.M11 = m11; this.M12 = m12; this.M13 = m13; this.M14 = m14;
            this.M21 = m21; this.M22 = m22; this.M23 = m23; this.M24 = m24;
            this.M31 = m31; this.M32 = m32; this.M33 = m33; this.M34 = m34;
            this.M41 = m41; this.M42 = m42; this.M43 = m43; this.M44 = m44;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="unitX">Unit vector for X axis.</param>
        /// <param name="unitY">Unit vector for Y axis.</param>
        /// <param name="unitZ">Unit vector for Z axis.</param>
        /// <param name="translation">Translation vector.</param>
        public Matrix(ref Vector4 unitX, ref Vector4 unitY, ref Vector4 unitZ, ref Vector4 translation)
        {
            this.M11 = unitX.X; this.M12 = unitY.X; this.M13 = unitZ.X; this.M14 = translation.X;
            this.M21 = unitX.Y; this.M22 = unitY.Y; this.M23 = unitZ.Y; this.M24 = translation.Y;
            this.M31 = unitX.Z; this.M32 = unitY.Z; this.M33 = unitZ.Z; this.M34 = translation.Z;
            this.M41 = unitX.W; this.M42 = unitY.W; this.M43 = unitZ.W; this.M44 = translation.W;
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
            if (obj is Matrix)
            {
                return Equals((Matrix)obj);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix other)
        {
            return
                this.M11 == other.M11 &&
                this.M12 == other.M12 &&
                this.M13 == other.M13 &&
                this.M14 == other.M14 &&
                this.M21 == other.M21 &&
                this.M22 == other.M22 &&
                this.M23 == other.M23 &&
                this.M24 == other.M24 &&
                this.M31 == other.M31 &&
                this.M32 == other.M32 &&
                this.M33 == other.M33 &&
                this.M34 == other.M34 &&
                this.M41 == other.M41 &&
                this.M42 == other.M42 &&
                this.M43 == other.M43 &&
                this.M44 == other.M44;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="delta">Maximum allowed error.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix other, float delta)
        {
            return
                System.Math.Abs(this.M11 - other.M11) <= delta &&
                System.Math.Abs(this.M12 - other.M12) <= delta &&
                System.Math.Abs(this.M13 - other.M13) <= delta &&
                System.Math.Abs(this.M14 - other.M14) <= delta &&
                System.Math.Abs(this.M21 - other.M21) <= delta &&
                System.Math.Abs(this.M22 - other.M22) <= delta &&
                System.Math.Abs(this.M23 - other.M23) <= delta &&
                System.Math.Abs(this.M24 - other.M24) <= delta &&
                System.Math.Abs(this.M31 - other.M31) <= delta &&
                System.Math.Abs(this.M32 - other.M32) <= delta &&
                System.Math.Abs(this.M33 - other.M33) <= delta &&
                System.Math.Abs(this.M34 - other.M34) <= delta &&
                System.Math.Abs(this.M41 - other.M41) <= delta &&
                System.Math.Abs(this.M42 - other.M42) <= delta &&
                System.Math.Abs(this.M43 - other.M43) <= delta &&
                System.Math.Abs(this.M44 - other.M44) <= delta;
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
                this.M13.GetHashCode() +
                this.M14.GetHashCode() +
                this.M21.GetHashCode() +
                this.M22.GetHashCode() +
                this.M23.GetHashCode() +
                this.M24.GetHashCode() +
                this.M31.GetHashCode() +
                this.M32.GetHashCode() +
                this.M33.GetHashCode() +
                this.M34.GetHashCode() +
                this.M41.GetHashCode() +
                this.M42.GetHashCode() +
                this.M43.GetHashCode() +
                this.M44.GetHashCode();
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
            sb.Append(' ');
            sb.Append(this.M13.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M14.ToString(format, formatProvider));
            sb.AppendLine("]");

            sb.Append('[');
            sb.Append(this.M21.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M22.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M23.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M24.ToString(format, formatProvider));
            sb.AppendLine("]");

            sb.Append('[');
            sb.Append(this.M31.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M32.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M33.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M34.ToString(format, formatProvider));
            sb.AppendLine("]");

            sb.Append('[');
            sb.Append(this.M41.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M42.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M43.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.M44.ToString(format, formatProvider));
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
                    this.M13, 
                    this.M14, 
                    this.M21, 
                    this.M22, 
                    this.M23, 
                    this.M24, 
                    this.M31, 
                    this.M32, 
                    this.M33, 
                    this.M34, 
                    this.M41, 
                    this.M42, 
                    this.M43, 
                    this.M44, 
                };
            }
            else
            {
                return new float[] { 
                    this.M11, 
                    this.M21, 
                    this.M31, 
                    this.M41, 
                    this.M12, 
                    this.M22, 
                    this.M32, 
                    this.M42, 
                    this.M13, 
                    this.M23, 
                    this.M33, 
                    this.M43, 
                    this.M14, 
                    this.M24, 
                    this.M34, 
                    this.M44, 
                };
            }
        }
        #endregion

        #region Operators

        public static Matrix operator *(Matrix value1, Matrix value2)
        {
            return Matrix.Multiply(value1, value2);
        }

        /// <summary>
        /// Determines whether two instances are equal.
        /// </summary>
        /// <param name="value1">A <see cref="Matrix4"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <returns>A boolean value indicating whether the instances are equal.</returns>
        public static bool operator ==(Matrix value1, Matrix value2)
        {
            return
                value1.M11 == value2.M11 &&
                value1.M12 == value2.M12 &&
                value1.M13 == value2.M13 &&
                value1.M14 == value2.M14 &&
                value1.M21 == value2.M21 &&
                value1.M22 == value2.M22 &&
                value1.M23 == value2.M23 &&
                value1.M24 == value2.M24 &&
                value1.M31 == value2.M31 &&
                value1.M32 == value2.M32 &&
                value1.M33 == value2.M33 &&
                value1.M34 == value2.M34 &&
                value1.M41 == value2.M41 &&
                value1.M42 == value2.M42 &&
                value1.M43 == value2.M43 &&
                value1.M44 == value2.M44;
        }

        /// <summary>
        /// Determines whether two instances are not equal.
        /// </summary>
        /// <param name="value1">A <see cref="Matrix4"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        /// <returns>A boolean value indicating whether the instances are not equal.</returns>
        public static bool operator !=(Matrix value1, Matrix value2)
        {
            return
                value1.M11 != value2.M11 ||
                value1.M12 != value2.M12 ||
                value1.M13 != value2.M13 ||
                value1.M14 != value2.M14 ||
                value1.M21 != value2.M21 ||
                value1.M22 != value2.M22 ||
                value1.M23 != value2.M23 ||
                value1.M24 != value2.M24 ||
                value1.M31 != value2.M31 ||
                value1.M32 != value2.M32 ||
                value1.M33 != value2.M33 ||
                value1.M34 != value2.M34 ||
                value1.M41 != value2.M41 ||
                value1.M42 != value2.M42 ||
                value1.M43 != value2.M43 ||
                value1.M44 != value2.M44;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the empty matrix.
        /// </summary>
        /// <value>
        /// A <see cref="Matrix4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Matrix Empty
        {
            get { return empty; }
        }

        /// <summary>
        /// Gets the identity matrix.
        /// </summary>
        /// <value>
        /// A <see cref="Matrix4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Matrix Identity
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
            get
            {
                return
                    this.M11 +
                    this.M22 +
                    this.M33 +
                    this.M44;
            }
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
            get
            {
                float temp1 = (M33 * M44) - (M34 * M43);
                float temp2 = (M32 * M44) - (M34 * M42);
                float temp3 = (M32 * M43) - (M33 * M42);
                float temp4 = (M31 * M44) - (M34 * M41);
                float temp5 = (M31 * M43) - (M33 * M41);
                float temp6 = (M31 * M42) - (M32 * M41);

                return ((((M11 * (((M22 * temp1) - (M23 * temp2)) + (M24 * temp3))) - (M12 * (((M21 * temp1) -
                    (M23 * temp4)) + (M24 * temp5)))) + (M13 * (((M21 * temp2) - (M22 * temp4)) + (M24 * temp6)))) -
                    (M14 * (((M21 * temp3) - (M22 * temp5)) + (M23 * temp6))));
                //float det1 = (this.M33 * this.M44) - (this.M34 * this.M43);
                //float det2 = (this.M32 * this.M44) - (this.M34 * this.M42);
                //float det3 = (this.M32 * this.M43) - (this.M33 * this.M42);
                //float det4 = (this.M31 * this.M44) - (this.M34 * this.M41);
                //float det5 = (this.M31 * this.M43) - (this.M33 * this.M41);
                //float det6 = (this.M31 * this.M42) - (this.M32 * this.M41);

                //return
                //    this.M11 * (this.M22 * det1 - this.M23 * det2 + this.M24 * det3) -
                //    this.M12 * (this.M21 * det1 - this.M23 * det4 + this.M24 * det5) +
                //    this.M13 * (this.M21 * det2 - this.M22 * det4 + this.M24 * det6) -
                //    this.M14 * (this.M21 * det3 - this.M22 * det5 + this.M23 * det6);
            }
        }

        /// <summary>
        /// Gets or sets the unit vector pointing to the left.
        /// </summary>
        /// <value>
        /// A <see cref="Vector3"/>.
        /// </value>
        [XmlIgnore]
        public Vector3 Left
        {
            get
            {
                Vector3 value;
                value.X = -this.M11;//-this.M11;
                value.Y = -this.M12;//-this.M21;
                value.Z = -this.M13;//-this.M31;
                return value;
            }
            set
            {
                this.M11 = -value.X;
                this.M12 = -value.Y;
                this.M13 = -value.Z;
            }
        }

        /// <summary>
        /// Gets or sets the unit vector pointing to the right.
        /// </summary>
        /// <value>
        /// A <see cref="Vector3"/>.
        /// </value>
        [XmlIgnore]
        public Vector3 Right
        {
            get
            {
                Vector3 value;
                value.X = this.M11;//this.M11;
                value.Y = this.M12;//this.M21;
                value.Z = this.M13;//this.M31;
                return value;
            }
            set
            {
                this.M11 = value.X;
                this.M12 = value.Y;
                this.M13 = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets the unit vector pointing up.
        /// </summary>
        /// <value>
        /// A <see cref="Vector3"/>.
        /// </value>
        [XmlIgnore]
        public Vector3 Up
        {
            get
            {
                Vector3 value;
                value.X = this.M21;//this.M12;
                value.Y = this.M22;//this.M22;
                value.Z = this.M23;//this.M32;
                return value;
            }
            set
            {
                this.M21 = value.X;
                this.M22 = value.Y;
                this.M23 = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets the unit vector pointing down.
        /// </summary>
        /// <value>
        /// A <see cref="Vector3"/>.
        /// </value>
        [XmlIgnore]
        public Vector3 Down
        {
            get
            {
                Vector3 value;
                value.X = -this.M21;//-this.M12;
                value.Y = -this.M22;//-this.M22;
                value.Z = -this.M23;//-this.M32;
                return value;
            }
            set
            {
                this.M21 = -value.X;
                this.M22 = -value.Y;
                this.M23 = -value.Z;
            }
        }

        /// <summary>
        /// Gets or sets the unit vector pointing forward.
        /// </summary>
        /// <value>
        /// A <see cref="Vector3"/>.
        /// </value>
        [XmlIgnore]
        public Vector3 Forward
        {
            get
            {
                Vector3 value;
                value.X = this.M31; //M13
                value.Y = this.M32; //M23
                value.Z = this.M33; //M33
                return value;
            }
            set
            {
                this.M31 = value.X; //M31
                this.M32 = value.Y; //M23
                this.M33 = value.Z; //M33
            }
        }

        /// <summary>
        /// Gets or sets the unit vector pointing backward.
        /// </summary>
        /// <value>
        /// A <see cref="Vector3"/>.
        /// </value>
        [XmlIgnore]
        public Vector3 Backward
        {
            get
            {
                Vector3 value;
                value.X = -this.M31;//-this.M13;
                value.Y = -this.M32;//-this.M23;
                value.Z = -this.M33;//-this.M33;
                return value;
            }
            set
            {
                this.M31 = -value.X;
                this.M32 = -value.Y;
                this.M33 = -value.Z;
            }
        }

        /// <summary>
        /// Gets or sets the translation vector.
        /// </summary>
        /// <value>
        /// A <see cref="Vector3"/>.
        /// </value>
        [XmlIgnore]
        public Vector3 Translation
        {
            get
            {
                Vector3 value;
                value.X = this.M41;//this.M14;
                value.Y = this.M42;//this.M24;
                value.Z = this.M43;//this.M34;
                return value;
            }
            set
            {
                this.M41 = value.X;
                this.M42 = value.Y;
                this.M43 = value.Z;
            }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Empty matrix.
        /// </summary>
        private static readonly Matrix empty = new Matrix();

        /// <summary>
        /// Identity matrix.
        /// </summary>
        private static readonly Matrix identity = new Matrix(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);

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
        /// Element at the 3rd row, 1st column.
        /// </summary>
        [XmlAttribute("m31")]
        public float M31;

        /// <summary>
        /// Element at the 4th row, 1st column.
        /// </summary>
        [XmlAttribute("m41")]
        public float M41;

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

        /// <summary>
        /// Element at the 3rd row, 2nd column.
        /// </summary>
        [XmlAttribute("m32")]
        public float M32;

        /// <summary>
        /// Element at the 4th row, 2nd column.
        /// </summary>
        [XmlAttribute("m42")]
        public float M42;

        /// <summary>
        /// Element at the 1st row, 3rd column.
        /// </summary>
        [XmlAttribute("m13")]
        public float M13;

        /// <summary>
        /// Element at the 2nd row, 3rd column.
        /// </summary>
        [XmlAttribute("m23")]
        public float M23;

        /// <summary>
        /// Element at the 3rd row, 3rd column.
        /// </summary>
        [XmlAttribute("m33")]
        public float M33;

        /// <summary>
        /// Element at the 4th row, 3rd column.
        /// </summary>
        [XmlAttribute("m43")]
        public float M43;

        /// <summary>
        /// Element at the 1st row, 4th column.
        /// </summary>
        [XmlAttribute("m14")]
        public float M14;

        /// <summary>
        /// Element at the 2nd row, 4th column.
        /// </summary>
        [XmlAttribute("m24")]
        public float M24;

        /// <summary>
        /// Element at the 3rd row, 4th column.
        /// </summary>
        [XmlAttribute("m34")]
        public float M34;

        /// <summary>
        /// Element at the 4th row, 4th column.
        /// </summary>
        [XmlAttribute("m44")]
        public float M44;
        #endregion

        public static Matrix RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            Matrix result = new Matrix();
            Quaternion quaternion = new Quaternion();
            Common.Rotate(out quaternion, yaw, pitch, roll);
            Common.Rotate(out result, ref quaternion);
            return result;
        }

        public static Matrix CreateTranslation(float translateX, float translateY, float translateZ)
        {
            Matrix result = new Matrix();
            Common.Translate(out result, translateX, translateY, translateZ);
            return result;
        }

        public static Matrix CreateScale(float scaleX, float scaleY, float scaleZ)
        {
            Matrix result = new Matrix();
            Vector3 scaleVector = new Vector3(scaleX, scaleY, scaleZ);
            Common.Scale(out result, ref scaleVector);
            return result;
        }

        public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
        {
            Matrix result = new Matrix();
            Common.Multiply(out result, ref matrix1, ref matrix2);
            return result;
        }

        public static Matrix CreateTranslation(Vector3 position)
        {
            return Matrix.CreateTranslation(position.X, position.Y, position.Z);
        }

        public static Matrix Scaling(Vector3 scale)
        {
            Matrix result = new Matrix();
            Common.Scale(out result, ref scale);
            return result;
        }

        public static Matrix RotationX(float angle)
        {
            Matrix result = new Matrix();
            Common.RotateX(out result, angle);
            return result;
        }
        
        public static Matrix RotationY(float angle)
        {
            Matrix result = new Matrix();
            Common.RotateY(out result, angle);
            return result;
        }

        public static Matrix RotationZ(float angle)
        {
            Matrix result = new Matrix();
            Common.RotateZ(out result, angle);
            return result;
        }

        public void Invert()
        {
            Matrix result = new Matrix();
            Common.Invert(out result, ref this);

            this.M11 = result.M11;
            this.M12 = result.M12;
            this.M13 = result.M13;
            this.M14 = result.M14;

            this.M21 = result.M21;
            this.M22 = result.M22;
            this.M23 = result.M23;
            this.M24 = result.M24;

            this.M31 = result.M31;
            this.M32 = result.M32;
            this.M33 = result.M33;
            this.M34 = result.M34;

            this.M41 = result.M41;
            this.M42 = result.M42;
            this.M43 = result.M43;
            this.M44 = result.M44;

        }
    }
}
