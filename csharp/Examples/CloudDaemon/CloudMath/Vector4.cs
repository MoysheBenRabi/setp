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
    /// Representation of a vector with 4 components.
    /// </summary>
    [Serializable]
    [XmlType("vector4")]
    [DebuggerDisplay("X = {X} Y = {Y} Z = {Z} W = {W}")]
    public struct Vector4 : IEquatable<Vector4>, IComparable, IComparable<Vector4>, IFormattable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        /// <param name="x">X component.</param>
        /// <param name="y">Y component.</param>
        /// <param name="z">Z component.</param>
        /// <param name="w">W component.</param>
        public Vector4(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Vector4(Vector3 vector, float w)
            : this(vector.X, vector.Y, vector.Z, w)
        { }

        #endregion

        #region Methods
        /// <summary>
        /// Indicates whether the current object and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> and the current object are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector4)
            {
                return Equals((Vector4)obj);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Vector4 other)
        {
            return
                this.X == other.X &&
                this.Y == other.Y &&
                this.Z == other.Z &&
                this.W == other.W;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="delta">Maximum allowed error.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Vector4 other, float delta)
        {
            return
                System.Math.Abs(this.X - other.X) <= delta &&
                System.Math.Abs(this.Y - other.Y) <= delta &&
                System.Math.Abs(this.Z - other.Z) <= delta &&
                System.Math.Abs(this.W - other.W) <= delta;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(object other)
        {
            if (other is Vector4)
            {
                return System.Math.Sign(this.LengthSquared - ((Vector4)other).LengthSquared);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(Vector4 other)
        {
            return System.Math.Sign(this.LengthSquared - other.LengthSquared);
        }

        /// <summary>
        /// Returns the hash code for the current object. 
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return
                this.X.GetHashCode() +
                this.Y.GetHashCode() +
                this.Z.GetHashCode() +
                this.W.GetHashCode();
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
            sb.Append(this.X.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.Y.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.Z.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(this.W.ToString(format, formatProvider));
            sb.Append(']');
            return sb.ToString();
        }

        /// <summary>
        /// Gets an array containing the vector elements.
        /// </summary>
        /// <returns>An array containing the vector elements.</returns>
        public float[] ToArray()
        {
            return new float[] { 
                this.X, 
                this.Y,
                this.Z,
                this.W,
            };
        }
        #endregion

        #region Operators
        /// <summary>
        /// Determines whether two instances are equal.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>A boolean value indicating whether the instances are equal.</returns>
        public static bool operator ==(Vector4 value1, Vector4 value2)
        {
            return
                value1.X == value2.X &&
                value1.Y == value2.Y &&
                value1.Z == value2.Z &&
                value1.W == value2.W;
        }

        /// <summary>
        /// Determines whether two instances are not equal.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>A boolean value indicating whether the instances are not equal.</returns>
        public static bool operator !=(Vector4 value1, Vector4 value2)
        {
            return
                value1.X != value2.X ||
                value1.Y != value2.Y ||
                value1.Z != value2.Z ||
                value1.W != value2.W;
        }

        /// <summary>
        /// Determines whether the magnitude the specified vector is less than or equal to the magnitude of the other.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>A boolean value indicating whether the magnitude is less than or equal to the other.</returns>
        public static bool operator <=(Vector4 value1, Vector4 value2)
        {
            return value1.LengthSquared <= value2.LengthSquared;
        }

        /// <summary>
        /// Determines whether the magnitude the specified vector is greater than or equal to the magnitude of the other.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>A boolean value indicating whether the magnitude is greater than or equal to the other.</returns>
        public static bool operator >=(Vector4 value1, Vector4 value2)
        {
            return value1.LengthSquared >= value2.LengthSquared;
        }

        /// <summary>
        /// Determines whether the magnitude the specified vector is less than the magnitude of the other.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>A boolean value indicating whether the magnitude is less than the other.</returns>
        public static bool operator <(Vector4 value1, Vector4 value2)
        {
            return value1.LengthSquared < value2.LengthSquared;
        }

        /// <summary>
        /// Determines whether the magnitude the specified vector is greater than the magnitude of the other.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>A boolean value indicating whether the magnitude is greater than the other.</returns>
        public static bool operator >(Vector4 value1, Vector4 value2)
        {
            return value1.LengthSquared > value2.LengthSquared;
        }

        /// <summary>
        /// Calculates the negation the specified vector.
        /// </summary>
        /// <param name="value">A <see cref="Vector4"/>.</param>
        /// <returns>Negation the specified vector.</returns>
        public static Vector4 operator -(Vector4 value)
        {
            Common.Negate(out value, ref value);
            return value;
        }

        /// <summary>
        /// Calculates the sum of the specified vectors.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>Sum of the specified vectors.</returns>
        public static Vector4 operator +(Vector4 value1, Vector4 value2)
        {
            Common.Add(out value1, ref value1, ref value2);
            return value1;
        }

        /// <summary>
        /// Calculates the difference of the specified vectors.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>Difference of the specified vectors.</returns>
        public static Vector4 operator -(Vector4 value1, Vector4 value2)
        {
            Common.Subtract(out value1, ref value1, ref value2);
            return value1;
        }

        /// <summary>
        /// Calculates the dot product of the specified vectors.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>Dot product of the specified vectors.</returns>
        public static float operator *(Vector4 value1, Vector4 value2)
        {
            return Common.Dot(ref value1, ref value2);
        }

        /// <summary>
        /// Scales the specified vector.
        /// </summary>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">Scaling factor.</param>
        /// <returns>Scaled vector.</returns>
        public static Vector4 operator *(Vector4 value1, float value2)
        {
            Common.Scale(out value1, ref value1, value2);
            return value1;
        }

        /// <summary>
        /// Scales the specified vector.
        /// </summary>
        /// <param name="value1">Scaling factor.</param>
        /// <param name="value2">A <see cref="Vector4"/>.</param>
        /// <returns>Scaled vector.</returns>
        public static Vector4 operator *(float value1, Vector4 value2)
        {
            Common.Scale(out value2, ref value2, value1);
            return value2;
        }

        public static Vector4 operator /(Vector4 value, float amount)
        {
            Vector4 result = new Vector4();
            result.X = value.X / amount;
            result.Y = value.Y / amount;
            result.Z = value.Z / amount;
            result.W = value.W / amount;

            return result;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets the empty vector.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 Empty
        {
            get { return empty; }
        }

        /// <summary>
        /// Gets the unit vector on X axis.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 UnitX
        {
            get { return unitX; }
        }

        /// <summary>
        /// Gets the unit vector on Y axis.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 UnitY
        {
            get { return unitY; }
        }

        /// <summary>
        /// Gets the unit vector on Z axis.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 UnitZ
        {
            get { return unitZ; }
        }

        /// <summary>
        /// Gets the unit vector on W axis.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 UnitW
        {
            get { return unitW; }
        }

        /// <summary>
        /// Gets the unit vector pointing to the left.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 Left
        {
            get { return left; }
        }

        /// <summary>
        /// Gets the unit vector pointing to the right.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 Right
        {
            get { return right; }
        }

        /// <summary>
        /// Gets the unit vector pointing up.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 Up
        {
            get { return up; }
        }

        /// <summary>
        /// Gets the unit vector pointing down.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 Down
        {
            get { return down; }
        }

        /// <summary>
        /// Gets the unit vector pointing forward.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 Forward
        {
            get { return forward; }
        }

        /// <summary>
        /// Gets the unit vector pointing backward.
        /// </summary>
        /// <value>
        /// A <see cref="Vector4"/>.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static Vector4 Backward
        {
            get { return backward; }
        }

        /// <summary>
        /// Gets or sets the length of the vector.
        /// </summary>
        /// <value>
        /// Length of the vector.
        /// </value>
        [XmlIgnore]
        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt(
                    this.X * this.X +
                    this.Y * this.Y +
                    this.Z * this.Z +
                    this.W * this.W);
            }
            set
            {
                float s = value / this.Length;
                this.X *= s;
                this.Y *= s;
                this.Z *= s;
                this.W *= s;
            }
        }

        /// <summary>
        /// Gets or sets the squared length of the vector.
        /// </summary>
        /// <value>
        /// Squared length of the vector.
        /// </value>
        [XmlIgnore]
        public float LengthSquared
        {
            get
            {
                return
                    this.X * this.X +
                    this.Y * this.Y +
                    this.Z * this.Z +
                    this.W * this.W;
            }
            set
            {
                float s = value / this.LengthSquared;
                this.X *= s;
                this.Y *= s;
                this.Z *= s;
                this.W *= s;
            }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Empty vector.
        /// </summary>
        private static readonly Vector4 empty = new Vector4();

        /// <summary>
        /// Unit vector on X axis.
        /// </summary>
        private static readonly Vector4 unitX = new Vector4(1, 0, 0, 0);

        /// <summary>
        /// Unit vector on Y axis.
        /// </summary>
        private static readonly Vector4 unitY = new Vector4(0, 1, 0, 0);

        /// <summary>
        /// Unit vector on Z axis.
        /// </summary>
        private static readonly Vector4 unitZ = new Vector4(0, 0, 1, 0);

        /// <summary>
        /// Unit vector on W axis.
        /// </summary>
        private static readonly Vector4 unitW = new Vector4(0, 0, 0, 1);

        /// <summary>
        /// Unit vector pointing to the left.
        /// </summary>
        private static readonly Vector4 left = new Vector4(-1, 0, 0, 0);

        /// <summary>
        /// Unit vector pointing to the right.
        /// </summary>
        private static readonly Vector4 right = new Vector4(1, 0, 0, 0);

        /// <summary>
        /// Unit vector pointing up.
        /// </summary>
        private static readonly Vector4 up = new Vector4(0, 1, 0, 0);

        /// <summary>
        /// Unit vector pointing down.
        /// </summary>
        private static readonly Vector4 down = new Vector4(0, -1, 0, 0);

        /// <summary>
        /// Unit vector pointing forward.
        /// </summary>
        private static readonly Vector4 forward = new Vector4(0, 0, -1, 0);

        /// <summary>
        /// Unit vector pointing backward.
        /// </summary>
        private static readonly Vector4 backward = new Vector4(0, 0, 1, 0);

        /// <summary>
        /// X component.
        /// </summary>
        [XmlAttribute("x")]
        public float X;

        /// <summary>
        /// Y component.
        /// </summary>
        [XmlAttribute("y")]
        public float Y;

        /// <summary>
        /// Z component.
        /// </summary>
        [XmlAttribute("z")]
        public float Z;

        /// <summary>
        /// W component.
        /// </summary>
        [XmlAttribute("w")]
        public float W;
        #endregion

        public static float Distance(Vector4 value1, Vector4 value2)
        {
            return Common.Distance(ref value1, ref value2);
        }

        public static Vector4 Normalize(Vector4 value)
        {
            Vector4 result = new Vector4();
            Common.Normalize(out result, ref value);
            return result;
        }
    }
}
