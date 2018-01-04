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
    /// Representation of a plane.
    /// </summary>
    [Serializable]
    [XmlType("plane")]
    [DebuggerDisplay("Normal = {Normal} D = {D}")]
    public struct Plane : IEquatable<Plane>, IFormattable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="a">A component of the plane equation</param>
        /// <param name="b">B component of the plane equation</param>
        /// <param name="c">C component of the plane equation</param>
        /// <param name="d">D component of the plane equation.</param>
        public Plane(float a, float b, float c, float d)
        {
            this.Normal.X = a;
            this.Normal.Y = b;
            this.Normal.Z = c;
            this.D = d;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="normal">Normal vector of the plane.</param>
        /// <param name="d">Negative distance to the origin.</param>
        public Plane(ref Vector3 normal, float d)
        {
            this.Normal = normal;
            this.D = d;
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
            if (obj is Plane)
            {
                return Equals((Plane)obj);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Plane other)
        {
            return
                this.Normal.X == other.Normal.X &&
                this.Normal.Y == other.Normal.Y &&
                this.Normal.Z == other.Normal.Z &&
                this.D == other.D;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="delta">Maximum allowed error.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Plane other, float delta)
        {
            return
                System.Math.Abs(this.Normal.X - other.Normal.X) <= delta &&
                System.Math.Abs(this.Normal.Y - other.Normal.Y) <= delta &&
                System.Math.Abs(this.Normal.Z - other.Normal.Z) <= delta &&
                System.Math.Abs(this.D - other.D) <= delta;
        }

        /// <summary>
        /// Returns the hash code for the current object.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return
                this.Normal.GetHashCode() +
                this.D.GetHashCode();
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
            sb.Append(this.Normal.X.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.Normal.Y.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.Normal.Z.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.D.ToString(format, formatProvider));
            sb.Append(']');
            return sb.ToString();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Determines whether two instances are equal.
        /// </summary>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Plane"/>.</param>
        /// <returns>A boolean value indicating whether the two planes are equal.</returns>
        public static bool operator ==(Plane value1, Plane value2)
        {
            return
                value1.Normal.X == value2.Normal.X &&
                value1.Normal.Y == value2.Normal.Y &&
                value1.Normal.Z == value2.Normal.Z &&
                value1.D == value2.D;
        }

        /// <summary>
        /// Determines whether two instances are not equal.
        /// </summary>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Plane"/>.</param>
        /// <returns>A boolean value indicating whether the two planes are not equal.</returns>
        public static bool operator !=(Plane value1, Plane value2)
        {
            return
                value1.Normal.X != value2.Normal.X ||
                value1.Normal.Y != value2.Normal.Y ||
                value1.Normal.Z != value2.Normal.Z ||
                value1.D != value2.D;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the empty plane.
        /// </summary>
        /// <value>
        /// A <see cref="Plane"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Plane Empty
        {
            get { return empty; }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Empty plane.
        /// </summary>
        private static readonly Plane empty = new Plane();

        /// <summary>
        /// Normal vector of the plane.
        /// </summary>
        [XmlElement("normal")]
        public Vector3 Normal;

        /// <summary>
        /// Negative distance from the origin.
        /// </summary>
        [XmlElement("d")]
        public float D;
        #endregion
    }
}
