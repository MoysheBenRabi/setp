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
    /// Representation of a convex volume that is delimited by six clip planes.
    /// </summary>
    [Serializable]
    [XmlType("volume")]
    [DebuggerDisplay("{ClipPlane1} {ClipPlane2} {ClipPlane3} {ClipPlane4} {ClipPlane5} {ClipPlane6}")]
    public struct Volume : IEquatable<Volume>, IFormattable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="clipPlane1">1st clip plane.</param>
        /// <param name="clipPlane2">2nd clip plane.</param>
        /// <param name="clipPlane3">3rd clip plane.</param>
        /// <param name="clipPlane4">4th clip plane.</param>
        /// <param name="clipPlane5">5th clip plane.</param>
        /// <param name="clipPlane6">6th clip plane.</param>
        public Volume(ref Plane clipPlane1, ref Plane clipPlane2, ref Plane clipPlane3, ref Plane clipPlane4, ref Plane clipPlane5, ref Plane clipPlane6)
        {
            this.ClipPlane1 = clipPlane1;
            this.ClipPlane2 = clipPlane2;
            this.ClipPlane3 = clipPlane3;
            this.ClipPlane4 = clipPlane4;
            this.ClipPlane5 = clipPlane5;
            this.ClipPlane6 = clipPlane6;
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
            if (obj is Volume)
            {
                return Equals((Volume)obj);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Volume other)
        {
            return
                this.ClipPlane1 == other.ClipPlane1 &&
                this.ClipPlane2 == other.ClipPlane2 &&
                this.ClipPlane3 == other.ClipPlane3 &&
                this.ClipPlane4 == other.ClipPlane4 &&
                this.ClipPlane5 == other.ClipPlane5 &&
                this.ClipPlane6 == other.ClipPlane6;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="delta">Maximum allowed error.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Volume other, float delta)
        {
            return
                this.ClipPlane1.Equals(other.ClipPlane1, delta) &&
                this.ClipPlane2.Equals(other.ClipPlane2, delta) &&
                this.ClipPlane3.Equals(other.ClipPlane3, delta) &&
                this.ClipPlane4.Equals(other.ClipPlane4, delta) &&
                this.ClipPlane5.Equals(other.ClipPlane5, delta) &&
                this.ClipPlane6.Equals(other.ClipPlane6, delta);
        }

        /// <summary>
        /// Returns the hash code for the current object. 
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return
                this.ClipPlane1.GetHashCode() +
                this.ClipPlane2.GetHashCode() +
                this.ClipPlane3.GetHashCode() +
                this.ClipPlane4.GetHashCode() +
                this.ClipPlane5.GetHashCode() +
                this.ClipPlane6.GetHashCode();
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
            sb.Append(this.ClipPlane1.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.ClipPlane2.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.ClipPlane3.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.ClipPlane4.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.ClipPlane5.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.ClipPlane6.ToString(format, formatProvider));
            sb.Append('}');
            return sb.ToString();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Determines whether two instances are equal.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/>.</param>
        /// <param name="value2">A <see cref="Volume"/>.</param>
        /// <returns>A boolean value indicating whether the instances are equal.</returns>
        public static bool operator ==(Volume value1, Volume value2)
        {
            return
                value1.ClipPlane1 == value2.ClipPlane1 &&
                value1.ClipPlane2 == value2.ClipPlane2 &&
                value1.ClipPlane3 == value2.ClipPlane3 &&
                value1.ClipPlane4 == value2.ClipPlane4 &&
                value1.ClipPlane5 == value2.ClipPlane5 &&
                value1.ClipPlane6 == value2.ClipPlane6;
        }

        /// <summary>
        /// Determines whether two instances are not equal.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/>.</param>
        /// <param name="value2">A <see cref="Volume"/>.</param>
        /// <returns>A boolean value indicating whether the instances are not equal.</returns>
        public static bool operator !=(Volume value1, Volume value2)
        {
            return
                value1.ClipPlane1 != value2.ClipPlane1 ||
                value1.ClipPlane2 != value2.ClipPlane2 ||
                value1.ClipPlane3 != value2.ClipPlane3 ||
                value1.ClipPlane4 != value2.ClipPlane4 ||
                value1.ClipPlane5 != value2.ClipPlane5 ||
                value1.ClipPlane6 != value2.ClipPlane6;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the empty volume.
        /// </summary>
        /// <value>
        /// A <see cref="Volume"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Volume Empty
        {
            get { return empty; }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Empty volume.
        /// </summary>
        private static readonly Volume empty = new Volume();

        /// <summary>
        /// Clip plane 1.
        /// </summary>
        [XmlElement("clipPlane1")]
        public Plane ClipPlane1;

        /// <summary>
        /// Clip plane 2.
        /// </summary>
        [XmlElement("clipPlane2")]
        public Plane ClipPlane2;

        /// <summary>
        /// Clip plane 3.
        /// </summary>
        [XmlElement("clipPlane3")]
        public Plane ClipPlane3;

        /// <summary>
        /// Clip plane 4.
        /// </summary>
        [XmlElement("clipPlane4")]
        public Plane ClipPlane4;

        /// <summary>
        /// Clip plane 5.
        /// </summary>
        [XmlElement("clipPlane5")]
        public Plane ClipPlane5;

        /// <summary>
        /// Clip plane 6.
        /// </summary>
        [XmlElement("clipPlane6")]
        public Plane ClipPlane6;
        #endregion
    }
}
