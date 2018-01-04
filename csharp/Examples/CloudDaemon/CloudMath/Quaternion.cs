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
    /// Representation of a quaternion.
    /// </summary>
    [Serializable]
    [XmlType("quaternion")]
    [DebuggerDisplay("W = {W} I = {I} J = {J} K = {K}")]
    public struct Quaternion : IEquatable<Quaternion>, IFormattable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="w">W component.</param>
        /// <param name="i">I component.</param>
        /// <param name="j">J component.</param>
        /// <param name="k">K component.</param>
        public Quaternion(float w, float i, float j, float k)
        {
            this.W = w;
            this.I = i;
            this.J = j;
            this.K = k;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Indicates whether the quaternion and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> and the quaternion are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Quaternion)
            {
                return Equals((Quaternion)obj);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Quaternion other)
        {
            return
                this.W == other.W &&
                this.I == other.I &&
                this.J == other.J &&
                this.K == other.K;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="delta">Maximum allowed error.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Quaternion other, float delta)
        {
            return
                System.Math.Abs(this.W - other.W) <= delta &&
                System.Math.Abs(this.I - other.I) <= delta &&
                System.Math.Abs(this.J - other.J) <= delta &&
                System.Math.Abs(this.K - other.K) <= delta;
        }

        /// <summary>
        /// Returns the hash code for the quaternion.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for the quaternion.</returns>
        public override int GetHashCode()
        {
            return
                this.W.GetHashCode() +
                this.I.GetHashCode() +
                this.J.GetHashCode() +
                this.K.GetHashCode();
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
            sb.Append(this.W.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.I.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.J.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.K.ToString(format, formatProvider));
            sb.Append(']');
            return sb.ToString();
        }

        /// <summary>
        /// Gets an array containing the quaternion elements.
        /// </summary>
        /// <returns>An array containing the quaternion elements.</returns>
        public float[] ToArray()
        {
            return new float[] { 
                this.W,
                this.I, 
                this.J,
                this.K,
            };
        }
        #endregion

        #region Operators
        /// <summary>
        /// Determines whether two instances are equal.
        /// </summary>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        /// <returns>A boolean value indicating whether the instances are equal.</returns>
        public static bool operator ==(Quaternion value1, Quaternion value2)
        {
            return
                value1.W == value2.W &&
                value1.I == value2.I &&
                value1.J == value2.J &&
                value1.K == value2.K;
        }

        /// <summary>
        /// Determines whether two instances are not equal.
        /// </summary>
        /// <param name="value1">A <see cref="Quaternion"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        /// <returns>A boolean value indicating whether the instances are not equal.</returns>
        public static bool operator !=(Quaternion value1, Quaternion value2)
        {
            return
                value1.W != value2.W ||
                value1.I != value2.I ||
                value1.J != value2.J ||
                value1.K != value2.K;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the empty quaternion.
        /// </summary>
        /// <value>
        /// A <see cref="Quaternion"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Quaternion Empty
        {
            get { return empty; }
        }

        /// <summary>
        /// Gets the identity quaternion.
        /// </summary>
        /// <value>
        /// A <see cref="Quaternion"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Quaternion Identity
        {
            get { return identity; }
        }

        /// <summary>
        /// Gets or sets the length of the quaternion.
        /// </summary>
        /// <value>
        /// Length of the quaternion.
        /// </value>
        [XmlIgnore]
        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt(
                    this.W * this.W +
                    this.I * this.I +
                    this.J * this.J +
                    this.K * this.K);
            }
            set
            {
                float s = value / this.Length;
                this.W *= s;
                this.I *= s;
                this.J *= s;
                this.K *= s;
            }
        }

        /// <summary>
        /// Gets or sets the squared length of the quaternion.
        /// </summary>
        /// <value>
        /// Squared length of the quaternion.
        /// </value>
        [XmlIgnore]
        public float LengthSquared
        {
            get
            {
                return
                    this.W * this.W +
                    this.I * this.I +
                    this.J * this.J +
                    this.K * this.K;
            }
            set
            {
                float s = value / this.LengthSquared;
                this.W *= s;
                this.I *= s;
                this.J *= s;
                this.K *= s;
            }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Empty quaternion.
        /// </summary>
        private static readonly Quaternion empty = new Quaternion();

        /// <summary>
        /// Identity quaternion.
        /// </summary>
        private static readonly Quaternion identity = new Quaternion(1, 0, 0, 0);

        /// <summary>
        /// W component.
        /// </summary>
        [XmlAttribute("w")]
        public float W;

        /// <summary>
        /// I component.
        /// </summary>
        [XmlAttribute("i")]
        public float I;

        /// <summary>
        /// J component.
        /// </summary>
        [XmlAttribute("j")]
        public float J;

        /// <summary>
        /// K component.
        /// </summary>
        [XmlAttribute("k")]
        public float K;
        #endregion
    }
}
