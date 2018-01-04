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
        /// Determines whether the specified ray and the specified sphere intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <returns>Intersection distance along the ray.</returns>
        public static float? Intersect(ref Ray value1, ref Vector3 value2)
        {
            Vector3 vector;
            vector.X = value2.X - value1.Position.X;
            vector.Y = value2.Y - value1.Position.Y;
            vector.Z = value2.Z - value1.Position.Z;

            float lengthSquared = vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z;

            if (lengthSquared <= Epsilon)
            {
                return 0;
            }
            else
            {
                float dot =
                    value1.Direction.X * vector.X +
                    value1.Direction.Y * vector.Y +
                    value1.Direction.Z * vector.Z;

                if (dot >= 0)
                {
                    if ((lengthSquared - (dot * dot)) <= Epsilon)
                    {
                        return dot;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified ray and the specified plane intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Plane"/>.</param>
        /// <returns>Intersection distance along the ray.</returns>
        public static float? Intersect(ref Ray value1, ref Plane value2)
        {
            float dot =
                value1.Direction.X * value2.Normal.X +
                value1.Direction.Y * value2.Normal.Y +
                value1.Direction.Z * value2.Normal.Z;

            if (System.Math.Abs(dot) > Epsilon)
            {
                float distance = (value2.Normal.X * value1.Position.X + value2.Normal.Y * value1.Position.Y + value2.Normal.Z * value1.Position.Z + value2.D) / -dot;

                if (distance >= 0)
                {
                    return distance;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified ray and the specified box intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        /// <returns>Intersection distance along the ray.</returns>
        public static float? Intersect(ref Ray value1, ref BoundingBox value2)
        {
            float n = float.MinValue;
            float f = float.MaxValue;

            if (System.Math.Abs(value1.Direction.X) < Epsilon)
            {
                if (value1.Position.X < value2.Minimum.X || value1.Position.X > value2.Maximum.X)
                {
                    return null;
                }
            }
            else
            {
                float inv = 1 / value1.Direction.X;
                float d1 = (value2.Minimum.X - value1.Position.X) * inv;
                float d2 = (value2.Maximum.X - value1.Position.X) * inv;

                n = System.Math.Max(n, System.Math.Min(d1, d2));
                f = System.Math.Min(f, System.Math.Max(d1, d2));

                if (n > f || f < 0)
                {
                    return null;
                }
            }

            if (System.Math.Abs(value1.Direction.Y) < Epsilon)
            {
                if (value1.Position.Y < value2.Minimum.Y || value1.Position.Y > value2.Maximum.Y)
                {
                    return null;
                }
            }
            else
            {
                float inv = 1 / value1.Direction.Y;
                float d1 = (value2.Minimum.Y - value1.Position.Y) * inv;
                float d2 = (value2.Maximum.Y - value1.Position.Y) * inv;

                n = System.Math.Max(n, System.Math.Min(d1, d2));
                f = System.Math.Min(f, System.Math.Max(d1, d2));

                if (n > f || f < 0)
                {
                    return null;
                }
            }

            if (System.Math.Abs(value1.Direction.Z) < Epsilon)
            {
                if (value1.Position.Z < value2.Minimum.Z || value1.Position.Z > value2.Maximum.Z)
                {
                    return null;
                }
            }
            else
            {
                float inv = 1 / value1.Direction.Z;
                float d1 = (value2.Minimum.Z - value1.Position.Z) * inv;
                float d2 = (value2.Maximum.Z - value1.Position.Z) * inv;

                n = System.Math.Max(n, System.Math.Min(d1, d2));
                f = System.Math.Min(f, System.Math.Max(d1, d2));

                if (n > f || f < 0)
                {
                    return null;
                }
            }

            return n;
        }

        /// <summary>
        /// Determines whether the specified ray and the specified sphere intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>Intersection distance along the ray.</returns>
        public static float? Intersect(ref Ray value1, ref Sphere value2)
        {
            Vector3 vector;
            vector.X = value2.Position.X - value1.Position.X;
            vector.Y = value2.Position.Y - value1.Position.Y;
            vector.Z = value2.Position.Z - value1.Position.Z;

            float lengthSquared = vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z;
            float radiusSquared = value2.Radius * value2.Radius;

            if (lengthSquared <= radiusSquared)
            {
                return 0;
            }
            else
            {
                float dot =
                    value1.Direction.X * vector.X +
                    value1.Direction.Y * vector.Y +
                    value1.Direction.Z * vector.Z;

                if (dot >= 0)
                {
                    float discriminant = lengthSquared - (dot * dot);

                    if (discriminant <= radiusSquared)
                    {
                        return dot - (float)System.Math.Sqrt(radiusSquared - discriminant);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified plane and the specified point intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <returns>A boolean value indicating whether the plane and the point intersect.</returns>
        public static bool Intersect(ref Plane value1, ref Vector3 value2)
        {
            float distance =
                value1.Normal.X * value2.X +
                value1.Normal.Y * value2.Y +
                value1.Normal.Z * value2.Z +
                value1.D;

            if (distance > Epsilon || distance < -Epsilon)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified planes intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Plane"/>.</param>
        /// <returns>A boolean value indicating whether the planes intersect.</returns>
        public static bool Intersect(ref Plane value1, ref Plane value2)
        {
            Vector3 axis;
            Cross(out axis, ref value1.Normal, ref value2.Normal);
            
            return (axis.LengthSquared > Epsilon);
        }

        /// <summary>
        /// Determines the intersection line between the specified planes.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Plane"/>.</param>
        /// <returns>A boolean value indicating whether the planes intersect.</returns>
        public static bool Intersect(out Ray result, ref Plane value1, ref Plane value2)
        {
            Vector3 axis;
            Cross(out axis, ref value1.Normal, ref value2.Normal);

            if (axis.LengthSquared > Epsilon)
            {
                float dot = Dot(ref value1.Normal, ref value2.Normal);
                float det = 1 / (dot * dot - 1);
                
                Vector3 n1;
                Scale(out n1, ref value1.Normal, (value1.D - value2.D * dot) * det);

                Vector3 n2;
                Scale(out n2, ref value2.Normal, (value2.D - value1.D * dot) * det);

                result.Direction = axis;
                result.Position.X = n1.X + n2.X;
                result.Position.Y = n1.Y + n2.Y;
                result.Position.Z = n1.Z + n2.Z;
                return true;
            }
            else
            {
                result = Ray.Empty;
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified planes intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Plane"/>.</param>
        /// <param name="value3">A <see cref="Plane"/>.</param>
        /// <returns>A boolean value indicating whether the planes intersect.</returns>
        public static bool Intersect(ref Plane value1, ref Plane value2, ref Plane value3)
        {
            Vector3 c1;
            Cross(out c1, ref value2.Normal, ref value3.Normal);

            float dot =
                value1.Normal.X * c1.X +
                value1.Normal.Y * c1.Y +
                value1.Normal.Z * c1.Z;
            
            return (System.Math.Abs(dot) > Epsilon);
        }

        /// <summary>
        /// Determines the intersection point of the specified planes.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Plane"/>.</param>
        /// <param name="value3">A <see cref="Plane"/>.</param>
        /// <returns>A boolean value indicating whether the planes intersect.</returns>
        public static bool Intersect(out Vector3 result, ref Plane value1, ref Plane value2, ref Plane value3)
        {
            Vector3 c1;
            Cross(out c1, ref value2.Normal, ref value3.Normal);

            float dot =
                value1.Normal.X * c1.X +
                value1.Normal.Y * c1.Y +
                value1.Normal.Z * c1.Z;

            if (System.Math.Abs(dot) > Epsilon)
            {
                Vector3 c2;
                Cross(out c2, ref value3.Normal, ref value1.Normal);

                Vector3 c3;
                Cross(out c3, ref value1.Normal, ref value2.Normal);

                dot = 1 / -dot;
                result.X = (c1.X * value1.D + c2.X * value2.D + c3.X * value3.D) * dot;
                result.Y = (c1.Y * value1.D + c2.Y * value2.D + c3.Y * value3.D) * dot;
                result.Z = (c1.Z * value1.D + c2.Z * value2.D + c3.Z * value3.D) * dot;
                return true;
            }
            else
            {
                result = Vector3.Zero;
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified plane and the specified box intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        /// <returns>A boolean value indicating whether the plane and the box intersect.</returns>
        public static bool Intersect(ref Plane value1, ref BoundingBox value2)
        {
            float x = (value1.Normal.X > 0) ? value2.Minimum.X : value2.Maximum.X;
            float y = (value1.Normal.Y > 0) ? value2.Minimum.Y : value2.Maximum.Y;
            float z = (value1.Normal.Z > 0) ? value2.Minimum.Z : value2.Maximum.Z;

            float distance =
                value1.Normal.X * x +
                value1.Normal.Y * y +
                value1.Normal.Z * z +
                value1.D;

            if (distance > 0)
            {
                return false;
            }
            else if (MaxDistance(ref value1, ref value2) < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines the intersection point between the specified plane and the specified box.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        /// <returns>A boolean value indicating whether the plane and the box intersect.</returns>
        public static bool Intersect(out Vector3 result, ref Plane value1, ref BoundingBox value2)
        {
            float x = (value1.Normal.X > 0) ? value2.Minimum.X : value2.Maximum.X;
            float y = (value1.Normal.Y > 0) ? value2.Minimum.Y : value2.Maximum.Y;
            float z = (value1.Normal.Z > 0) ? value2.Minimum.Z : value2.Maximum.Z;

            float distance =
                value1.Normal.X * x +
                value1.Normal.Y * y +
                value1.Normal.Z * z +
                value1.D;

            if (distance > 0)
            {
                result.X = 0;
                result.Y = 0;
                result.Z = 0;
                return false;
            }
            else if (MaxDistance(ref value1, ref value2) < 0)
            {
                result.X = 0;
                result.Y = 0;
                result.Z = 0;
                return false;
            }
            else
            {
                result.X = x - value1.Normal.X * distance;
                result.Y = y - value1.Normal.Y * distance;
                result.Z = z - value1.Normal.Z * distance;
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified plane and the specified sphere intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>A boolean value indicating whether the plane and the sphere intersect.</returns>
        public static bool Intersect(ref Plane value1, ref Sphere value2)
        {
            float distance = 
                value1.Normal.X * value2.Position.X +
                value1.Normal.Y * value2.Position.Y +
                value1.Normal.Z * value2.Position.Z +
                value1.D;

            if (distance > value2.Radius || distance < -value2.Radius)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines the intersection point between the specified plane and the specified sphere.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>A boolean value indicating whether the plane and the sphere intersect.</returns>
        public static bool Intersect(out Vector3 result, ref Plane value1, ref Sphere value2)
        {
            float distance =
                value1.Normal.X * value2.Position.X +
                value1.Normal.Y * value2.Position.Y +
                value1.Normal.Z * value2.Position.Z +
                value1.D;

            if (distance > value2.Radius || distance < -value2.Radius)
            {
                result.X = 0;
                result.Y = 0;
                result.Z = 0;
                return false;
            }
            else
            {
                result.X = value2.Position.X - value1.Normal.X * distance;
                result.Y = value2.Position.Y - value1.Normal.Y * distance;
                result.Z = value2.Position.Z - value1.Normal.Z * distance;
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified box and the specified point intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <returns>A boolean value indicating whether the box and the point intersect.</returns>
        public static bool Intersect(ref BoundingBox value1, ref Vector3 value2)
        {
            return
                value1.Minimum.X <= value2.X && value2.X <= value1.Maximum.X &&
                value1.Minimum.Y <= value2.Y && value2.Y <= value1.Maximum.Y &&
                value1.Minimum.Z <= value2.Z && value2.Z <= value1.Maximum.Z;
        }

        /// <summary>
        /// Determines whether the specified boxes intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        /// <returns>A boolean value indicating whether the boxes intersect.</returns>
        public static bool Intersect(ref BoundingBox value1, ref BoundingBox value2)
        {
            return
                value1.Minimum.X < value2.Maximum.X &&
                value1.Minimum.Y < value2.Maximum.Y &&
                value1.Minimum.Z < value2.Maximum.Z &&
                value1.Maximum.X > value2.Minimum.X &&
                value1.Maximum.Y > value2.Minimum.Y &&
                value1.Maximum.Z > value2.Minimum.Z;
        }

        /// <summary>
        /// Determines the intersection between the specified boxes.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        /// <returns>A boolean value indicating whether the boxes intersect.</returns>
        public static bool Intersect(out BoundingBox result, ref BoundingBox value1, ref BoundingBox value2)
        {
            float L = System.Math.Max(value1.Minimum.X, value2.Minimum.X);
            float R = System.Math.Min(value1.Maximum.X, value2.Maximum.X);
            float B = System.Math.Max(value1.Minimum.Y, value2.Minimum.Y);
            float T = System.Math.Min(value1.Maximum.Y, value2.Maximum.Y);
            float F = System.Math.Max(value1.Minimum.Z, value2.Minimum.Z);
            float N = System.Math.Min(value1.Maximum.Z, value2.Maximum.Z);

            if (R >= L && T >= B && F >= N)
            {
                result.Minimum.X = L;
                result.Minimum.Y = B;
                result.Minimum.Z = N;
                result.Maximum.X = R;
                result.Maximum.Y = T;
                result.Maximum.Z = F;
                return true;
            }
            else
            {
                result = BoundingBox.Empty;
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified box and the specified sphere intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>A boolean value indicating whether the box and the sphere intersect.</returns>
        public static bool Intersect(ref BoundingBox value1, ref Sphere value2)
        {
            float x = value2.Position.X;
            x = (x > value1.Maximum.X) ? value1.Maximum.X : x;
            x = (x < value1.Minimum.X) ? value1.Minimum.X : x;
            x = x - value2.Position.X;

            float y = value2.Position.Y;
            y = (y > value1.Maximum.Y) ? value1.Maximum.Y : y;
            y = (y < value1.Minimum.Y) ? value1.Minimum.Y : y;
            y = y - value2.Position.Y;

            float z = value2.Position.Z;
            z = (z > value1.Maximum.Z) ? value1.Maximum.Z : z;
            z = (z < value1.Minimum.Z) ? value1.Minimum.Z : z;
            z = z - value2.Position.Z;
           
            return (value2.Radius * value2.Radius) >= (x * x + y * y + z * z);
        }

        /// <summary>
        /// Determines the intersection point between the specified box and the specified sphere.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>A boolean value indicating whether the box and the sphere intersect.</returns>
        public static bool Intersect(out Vector3 result, ref BoundingBox value1, ref Sphere value2)
        {
            float x = value2.Position.X;
            x = (x > value1.Maximum.X) ? value1.Maximum.X : x;
            x = (x < value1.Minimum.X) ? value1.Minimum.X : x;

            float y = value2.Position.Y;
            y = (y > value1.Maximum.Y) ? value1.Maximum.Y : y;
            y = (y < value1.Minimum.Y) ? value1.Minimum.Y : y;

            float z = value2.Position.Z;
            z = (z > value1.Maximum.Z) ? value1.Maximum.Z : z;
            z = (z < value1.Minimum.Z) ? value1.Minimum.Z : z;

            float dx = x - value2.Position.X;
            float dy = y - value2.Position.Y;
            float dz = z - value2.Position.Z;

            if ((value2.Radius * value2.Radius) >= (dx * dx + dy * dy + dz * dz))
            {
                result.X = x;
                result.Y = y;
                result.Z = z;
                return true;
            }
            else
            {
                result = Vector3.Zero;
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified sphere and the specified point intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <returns>A boolean value indicating whether the spheres intersect.</returns>
        public static bool Intersect(ref Sphere value1, ref Vector3 value2)
        {
            float x = value1.Position.X - value2.X;
            float y = value1.Position.Y - value2.Y;
            float z = value1.Position.Z - value2.Z;

            return (value1.Radius * value1.Radius) >= (x * x + y * y + z * z);
        }

        /// <summary>
        /// Determines whether the specified spheres intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>A boolean value indicating whether the spheres intersect.</returns>
        public static bool Intersect(ref Sphere value1, ref Sphere value2)
        {
            float x = value1.Position.X - value2.Position.X;
            float y = value1.Position.Y - value2.Position.Y;
            float z = value1.Position.Z - value2.Position.Z;

            float radiusSquared1 = value1.Radius * value1.Radius;
            float radiusSquared2 = value2.Radius * value2.Radius;
            float distanceSquared = x * x + y * y + z * z;
            
            return (radiusSquared1 + radiusSquared2 >= distanceSquared);
        }

        /// <summary>
        /// Determines the intersection point between the specified spheres.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>A boolean value indicating whether the spheres intersect.</returns>
        public static bool Intersect(out Vector3 result, ref Sphere value1, ref Sphere value2)
        {
            float x = value2.Position.X - value1.Position.X;
            float y = value2.Position.Y - value1.Position.Y;
            float z = value2.Position.Z - value1.Position.Z;

            float distance = (float)System.Math.Sqrt(x * x + y * y + z * z);

            if (value1.Radius + value2.Radius <= distance)
            {
                result = Vector3.Zero;
                return false;
            }
            else if (value1.Radius - value2.Radius >= distance)
            {
                result = value2.Position;
                return true;
            }
            else if (value2.Radius - value1.Radius >= distance)
            {
                result = value1.Position;
                return true;
            }
            else
            {
                float r1 = value1.Radius * value1.Radius;
                float r2 = value2.Radius * value2.Radius;
                float d1 = distance * distance;
                float d2 = (d1 + r1 - r2) / (2 * d1);

                result.X = value1.Position.X + x * d2;
                result.Y = value1.Position.Y + y * d2;
                result.Z = value1.Position.Z + z * d2;
                return true;
            }
        }

        /// <summary>
        /// Determines the intersection between the specified spheres.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>A boolean value indicating whether the spheres intersect.</returns>
        public static bool Intersect(out Sphere result, ref Sphere value1, ref Sphere value2)
        {
            float x = value2.Position.X - value1.Position.X;
            float y = value2.Position.Y - value1.Position.Y;
            float z = value2.Position.Z - value1.Position.Z;

            float distance = (float)System.Math.Sqrt(x * x + y * y + z * z);

            if (value1.Radius + value2.Radius <= distance)
            {
                result = Sphere.Empty;
                return false;
            }
            else if (value1.Radius - value2.Radius >= distance)
            {
                result = value2;
                return true;
            }
            else if (value2.Radius - value1.Radius >= distance)
            {
                result = value1;
                return true;
            }
            else
            {
                float radius = (float)(System.Math.Sqrt(
                    (value1.Radius - value2.Radius - distance) *
                    (value2.Radius - value1.Radius - distance) *
                    (value1.Radius + value2.Radius - distance) *
                    (value2.Radius + value1.Radius + distance)) / distance);

                float r1 = value1.Radius * value1.Radius;
                float r2 = value2.Radius * value2.Radius;
                float d1 = distance * distance;
                float d2 = (d1 + r1 - r2) / (2 * d1);

                result.Position.X = value1.Position.X + x * d2;
                result.Position.Y = value1.Position.Y + y * d2;
                result.Position.Z = value1.Position.Z + z * d2;
                result.Radius = radius;
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified volume and the specified point intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/>.</param>
        /// <param name="value2">A <see cref="Vector3"/>.</param>
        /// <returns>A boolean value indicating whether the volume and the point intersect.</returns>
        public static bool Intersect(ref Volume value1, ref Vector3 value2)
        {
            return
                Distance(ref value1.ClipPlane1, ref value2) >= 0 &&
                Distance(ref value1.ClipPlane2, ref value2) >= 0 &&
                Distance(ref value1.ClipPlane3, ref value2) >= 0 &&
                Distance(ref value1.ClipPlane4, ref value2) >= 0 &&
                Distance(ref value1.ClipPlane5, ref value2) >= 0 &&
                Distance(ref value1.ClipPlane6, ref value2) >= 0;
        }

        /// <summary>
        /// Determines whether the specified volume and the specified box intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/>.</param>
        /// <param name="value2">A <see cref="Box"/>.</param>
        /// <returns>A boolean value indicating whether the volume and the box intersect.</returns>
        public static bool Intersect(ref Volume value1, ref BoundingBox value2)
        {
            return
                MaxDistance(ref value1.ClipPlane1, ref value2) >= 0 &&
                MaxDistance(ref value1.ClipPlane2, ref value2) >= 0 &&
                MaxDistance(ref value1.ClipPlane3, ref value2) >= 0 &&
                MaxDistance(ref value1.ClipPlane4, ref value2) >= 0 &&
                MaxDistance(ref value1.ClipPlane5, ref value2) >= 0 &&
                MaxDistance(ref value1.ClipPlane6, ref value2) >= 0;
        }

        /// <summary>
        /// Determines whether the specified volume and the specified sphere intersect.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/>.</param>
        /// <param name="value2">A <see cref="Sphere"/>.</param>
        /// <returns>A boolean value indicating whether the volume and the sphere intersect.</returns>
        public static bool Intersect(ref Volume value1, ref Sphere value2)
        {
            float R = -value2.Radius;

            return
                Distance(ref value1.ClipPlane1, ref value2.Position) >= R &&
                Distance(ref value1.ClipPlane2, ref value2.Position) >= R &&
                Distance(ref value1.ClipPlane3, ref value2.Position) >= R &&
                Distance(ref value1.ClipPlane4, ref value2.Position) >= R &&
                Distance(ref value1.ClipPlane5, ref value2.Position) >= R &&
                Distance(ref value1.ClipPlane6, ref value2.Position) >= R;
        }
    }
}
