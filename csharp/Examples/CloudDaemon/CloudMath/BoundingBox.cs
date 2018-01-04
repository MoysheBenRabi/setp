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
    /// Representation of a box.
    /// </summary>
    [Serializable]
    [XmlType("box")]
    [DebuggerDisplay("Min = {Min} Max = {Max}")]
    public struct BoundingBox : IEquatable<BoundingBox>, IFormattable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="minX">Minimum coordinate on the X axis.</param>
        /// <param name="minY">Minimum coordinate on the Y axis.</param>
        /// <param name="minZ">Minimum coordinate on the Z axis.</param>
        /// <param name="maxX">Maximum coordinate on the X axis.</param>
        /// <param name="maxY">Maximum coordinate on the Y axis.</param>
        /// <param name="maxZ">Maximum coordinate on the Z axis.</param>
        public BoundingBox(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            this.Minimum.X = minX;
            this.Minimum.Y = minY;
            this.Minimum.Z = minZ;
            this.Maximum.X = maxX;
            this.Maximum.Y = maxY;
            this.Maximum.Z = maxZ;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="min">Minimum corner of the box.</param>
        /// <param name="max">Maximum corner of the box.</param>
        public BoundingBox(Vector3 min, Vector3 max)
        {
            this.Minimum = min;
            this.Maximum = max;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Indicates whether the current object and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> and the current object are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is BoundingBox)
            {
                return Equals((BoundingBox)obj);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(BoundingBox other)
        {
            return
                this.Minimum.X == other.Minimum.X &&
                this.Minimum.Y == other.Minimum.Y &&
                this.Minimum.Z == other.Minimum.Z &&
                this.Maximum.X == other.Maximum.X &&
                this.Maximum.Y == other.Maximum.Y &&
                this.Maximum.Z == other.Maximum.Z;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="delta">Maximum allowed error.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(BoundingBox other, float delta)
        {
            return
                System.Math.Abs(this.Minimum.X - other.Minimum.X) <= delta &&
                System.Math.Abs(this.Minimum.Y - other.Minimum.Y) <= delta &&
                System.Math.Abs(this.Minimum.Z - other.Minimum.Z) <= delta &&
                System.Math.Abs(this.Maximum.X - other.Maximum.X) <= delta &&
                System.Math.Abs(this.Maximum.Y - other.Maximum.Y) <= delta &&
                System.Math.Abs(this.Maximum.Z - other.Maximum.Z) <= delta;
        }

        /// <summary>
        /// Returns the hash code for the current object. 
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return
                this.Minimum.GetHashCode() +
                this.Maximum.GetHashCode();
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
            sb.Append('{');
            sb.Append(this.Minimum.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.Maximum.ToString(format, formatProvider));
            sb.Append('}');
            return sb.ToString();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Determines whether two instances are equal.
        /// </summary>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        /// <returns>A boolean value indicating whether the instances are equal.</returns>
        public static bool operator ==(BoundingBox value1, BoundingBox value2)
        {
            return
                value1.Minimum.X == value2.Minimum.X &&
                value1.Minimum.Y == value2.Minimum.Y &&
                value1.Minimum.Z == value2.Minimum.Z &&
                value1.Maximum.X == value2.Maximum.X &&
                value1.Maximum.Y == value2.Maximum.Y &&
                value1.Maximum.Z == value2.Maximum.Z;
        }

        /// <summary>
        /// Determines whether two instances are not equal.
        /// </summary>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        /// <returns>A boolean value indicating whether the instances are not equal.</returns>
        public static bool operator !=(BoundingBox value1, BoundingBox value2)
        {
            return
                value1.Minimum.X != value2.Minimum.X ||
                value1.Minimum.Y != value2.Minimum.Y ||
                value1.Minimum.Z != value2.Minimum.Z ||
                value1.Maximum.X != value2.Maximum.X ||
                value1.Maximum.Y != value2.Maximum.Y ||
                value1.Maximum.Z != value2.Maximum.Z;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the empty box.
        /// </summary>
        /// <value>
        /// A <see cref="Box"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static BoundingBox Empty
        {
            get { return empty; }
        }

        /// <summary>
        /// Gets a boolean value indicating whether the box is empty.
        /// </summary>
        /// <value>
        /// Indicates whether the box is empty.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        [XmlIgnore]
        public bool IsEmpty
        {
            get { return this.Width <= 0 || this.Height <= 0 || this.Depth <= 0; }
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of the top edge of the box.
        /// </summary>
        /// <value>
        /// Y-coordinate of the top edge of the box.
        /// </value>
        [XmlIgnore]
        public float Top
        {
            get { return this.Maximum.Y; }
            set { this.Maximum.Y = value; }
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of the bottom edge of the box.
        /// </summary>
        /// <value>
        /// Y-coordinate of the bottom edge of the box.
        /// </value>
        [XmlIgnore]
        public float Bottom
        {
            get { return this.Minimum.Y; }
            set { this.Minimum.Y = value; }
        }

        /// <summary>
        /// Gets or sets the X-coordinate of the left edge of the box.
        /// </summary>
        /// <value>
        /// X-coordinate of the left edge of the box.
        /// </value>
        [XmlIgnore]
        public float Left
        {
            get { return this.Minimum.X; }
            set { this.Minimum.X = value; }
        }

        /// <summary>
        /// Gets or sets the X-coordinate of the right edge of the box.
        /// </summary>
        /// <value>
        /// X-coordinate of the right edge of the box.
        /// </value>
        [XmlIgnore]
        public float Right
        {
            get { return this.Maximum.X; }
            set { this.Maximum.X = value; }
        }

        /// <summary>
        /// Gets or sets the Z-coordinate of the near edge of the box.
        /// </summary>
        /// <value>
        /// Z-coordinate of the near edge of the box.
        /// </value>
        [XmlIgnore]
        public float Near
        {
            get { return this.Maximum.Z; }
            set { this.Maximum.Z = value; }
        }

        /// <summary>
        /// Gets or sets the Z-coordinate of the far edge of the box.
        /// </summary>
        /// <value>
        /// Z-coordinate of the far edge of the box.
        /// </value>
        [XmlIgnore]
        public float Far
        {
            get { return this.Minimum.Z; }
            set { this.Minimum.Z = value; }
        }

        /// <summary>
        /// Gets the width of the box.
        /// </summary>
        /// <value>
        /// Width of the box.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        [XmlIgnore]
        public float Width
        {
            get { return this.Maximum.X - this.Minimum.X; }
        }

        /// <summary>
        /// Gets the height of the box.
        /// </summary>
        /// <value>
        /// Height of the box.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        [XmlIgnore]
        public float Height
        {
            get { return this.Maximum.Y - this.Minimum.Y; }
        }

        /// <summary>
        /// Gets the depth of the box.
        /// </summary>
        /// <value>
        /// Depth of the box.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        [XmlIgnore]
        public float Depth
        {
            get { return this.Maximum.Z - this.Minimum.Z; }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Empty box.
        /// </summary>
        private static readonly BoundingBox empty = new BoundingBox();

        /// <summary>
        /// Minimum corner of the box.
        /// </summary>
        [XmlElement("min")]
        public Vector3 Minimum;

        /// <summary>
        /// Maximum corner of the box.
        /// </summary>
        [XmlElement("max")]
        public Vector3 Maximum;
        #endregion

        public static BoundingBox FromPoints(Vector3[] points)
        {
            Vector3 vMax = new Vector3(float.MaxValue);
            Vector3 vMin = new Vector3(float.MinValue);

            foreach (Vector3 vector in points)
            {
                Vector3 vCompare = vector;
                Common.Min(out vMin, ref vMin, ref vCompare);
                Common.Max(out vMax, ref vMax, ref vCompare);
            }

            return new BoundingBox(vMin, vMax);
        }
    }
}
