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
    /// Representation of a sphere.
    /// </summary>
    [Serializable]
    [XmlType("sphere")]
    [DebuggerDisplay("Position = {Position} Radius = {Radius}")]
    public struct Sphere : IEquatable<Sphere>, IFormattable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="position">Sphere position.</param>
        /// <param name="radius">Sphere radius.</param>
        public Sphere(Vector3 position, float radius)
        {
            this.Position = position;
            this.Radius = radius;
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
            if (obj is Sphere)
            {
                return Equals((Sphere)obj);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Sphere other)
        {
            return
                this.Position.X == other.Position.X &&
                this.Position.Y == other.Position.Y &&
                this.Position.Z == other.Position.Z &&
                this.Radius == other.Radius;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="delta">Maximum allowed error.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Sphere other, float delta)
        {
            return
                System.Math.Abs(this.Position.X - other.Position.X) <= delta &&
                System.Math.Abs(this.Position.Y - other.Position.Y) <= delta &&
                System.Math.Abs(this.Position.Z - other.Position.Z) <= delta &&
                System.Math.Abs(this.Radius - other.Radius) <= delta;
        }

        /// <summary>
        /// Returns the hash code for the current object.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return
                this.Position.GetHashCode() +
                this.Radius.GetHashCode();
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
            sb.Append(this.Position.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.Radius.ToString(format, formatProvider));
            sb.Append('}');
            return sb.ToString();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Determines whether two instances are equal.
        /// </summary>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>A boolean value indicating whether the two planes are equal.</returns>
        public static bool operator ==(Sphere value1, Sphere value2)
        {
            return
                value1.Position.X == value2.Position.X &&
                value1.Position.Y == value2.Position.Y &&
                value1.Position.Z == value2.Position.Z &&
                value1.Radius == value2.Radius;
        }

        /// <summary>
        /// Determines whether two instances are not equal.
        /// </summary>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>A boolean value indicating whether the two planes are not equal.</returns>
        public static bool operator !=(Sphere value1, Sphere value2)
        {
            return
                value1.Position.X != value2.Position.X ||
                value1.Position.Y != value2.Position.Y ||
                value1.Position.Z != value2.Position.Z ||
                value1.Radius != value2.Radius;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the empty sphere.
        /// </summary>
        /// <value>
        /// A <see cref="Sphere"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Sphere Empty
        {
            get { return empty; }
        }

        /// <summary>
        /// Gets a boolean value indicating whether the sphere is empty.
        /// </summary>
        /// <value>
        /// Indicates whether the sphere is empty.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        [XmlIgnore]
        public bool IsEmpty
        {
            get { return this.Radius <= 0; }
        }

        /// <summary>
        /// Gets the squared sphere radius.
        /// </summary>
        /// <value>
        /// Squared sphere radius.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        [XmlIgnore]
        public float RadiusSquared
        {
            get { return this.Radius * this.Radius; }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Empty sphere.
        /// </summary>
        private static readonly Sphere empty = new Sphere();

        /// <summary>
        /// Sphere position.
        /// </summary>
        [XmlElement("position")]
        public Vector3 Position;

        /// <summary>
        /// Sphere radius.
        /// </summary>
        [XmlElement("radius")]
        public float Radius;
        #endregion
    }
}
