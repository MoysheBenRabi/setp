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
    /// Representation of a ray.
    /// </summary>
    [Serializable]
    [XmlType("ray")]
    [DebuggerDisplay("Origin = {Origin} Direction = {Direction}")]
    public struct Ray : IEquatable<Ray>, IFormattable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="positionX">X component of the ray position.</param>
        /// <param name="positionY">Y component of the ray position.</param>
        /// <param name="positionZ">Z component of the ray position.</param>
        /// <param name="directionX">X component of the ray direction vector.</param>
        /// <param name="directionY">Y component of the ray direction vector.</param>
        /// <param name="directionZ">Z component of the ray direction vector.</param>
        public Ray(float positionX, float positionY, float positionZ, float directionX, float directionY, float directionZ)
        {
            this.Position.X = positionX;
            this.Position.Y = positionY;
            this.Position.Z = positionZ;
            this.Direction.X = directionX;
            this.Direction.Y = directionY;
            this.Direction.Z = directionZ;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="position">Ray position.</param>
        /// <param name="direction">Ray direction vector.</param>
        public Ray(ref Vector3 position, ref Vector3 direction)
        {
            this.Position = position;
            this.Direction = direction;
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
            if (obj is Ray)
            {
                return Equals((Ray)obj);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Ray other)
        {
            return
                this.Position.X == other.Position.X &&
                this.Position.Y == other.Position.Y &&
                this.Position.Z == other.Position.Z &&
                this.Direction.X == other.Direction.X &&
                this.Direction.Y == other.Direction.Y &&
                this.Direction.Z == other.Direction.Z;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="delta">Maximum allowed error.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Ray other, float delta)
        {
            return
                System.Math.Abs(this.Position.X - other.Position.X) <= delta &&
                System.Math.Abs(this.Position.Y - other.Position.Y) <= delta &&
                System.Math.Abs(this.Position.Z - other.Position.Z) <= delta &&
                System.Math.Abs(this.Direction.X - other.Direction.X) <= delta &&
                System.Math.Abs(this.Direction.Y - other.Direction.Y) <= delta &&
                System.Math.Abs(this.Direction.Z - other.Direction.Z) <= delta;
        }

        /// <summary>
        /// Returns the hash code for the current object. 
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return
                this.Position.GetHashCode() +
                this.Direction.GetHashCode();
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
            sb.Append(this.Direction.ToString(format, formatProvider));
            sb.Append('}');
            return sb.ToString();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Determines whether two instances are equal.
        /// </summary>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Ray"/>.</param>
        /// <returns>A boolean value indicating whether the instances are equal.</returns>
        public static bool operator ==(Ray value1, Ray value2)
        {
            return
                value1.Position.X == value2.Position.X &&
                value1.Position.Y == value2.Position.Y &&
                value1.Position.Z == value2.Position.Z &&
                value1.Direction.X == value2.Direction.X &&
                value1.Direction.Y == value2.Direction.Y &&
                value1.Direction.Z == value2.Direction.Z;
        }

        /// <summary>
        /// Determines whether two instances are not equal.
        /// </summary>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Ray"/>.</param>
        /// <returns>A boolean value indicating whether the instances are not equal.</returns>
        public static bool operator !=(Ray value1, Ray value2)
        {
            return
                value1.Position.X != value2.Position.X ||
                value1.Position.Y != value2.Position.Y ||
                value1.Position.Z != value2.Position.Z ||
                value1.Direction.X != value2.Direction.X ||
                value1.Direction.Y != value2.Direction.Y ||
                value1.Direction.Z != value2.Direction.Z;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the empty ray.
        /// </summary>
        /// <value>
        /// A <see cref="Ray"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Ray Empty
        {
            get { return empty; }
        }

        /// <summary>
        /// Gets a boolean value indicating whether the ray is empty.
        /// </summary>
        /// <value>
        /// Indicates whether the ray is empty.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        [XmlIgnore]
        public bool IsEmpty
        {
            get { return (Direction.LengthSquared == 0); }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Empty ray.
        /// </summary>
        private static readonly Ray empty = new Ray();

        /// <summary>
        /// Ray position.
        /// </summary>
        [XmlElement("position")]
        public Vector3 Position;

        /// <summary>
        /// Ray direction vector.
        /// </summary>
        [XmlElement("direction")]
        public Vector3 Direction;
        #endregion
    }
}
