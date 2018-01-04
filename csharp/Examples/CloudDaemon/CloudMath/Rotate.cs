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
        /// Rotates the specified vector with the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out Vector2 result, ref Vector2 value1, ref Quaternion value2)
        {
            float ii = value2.I * value2.I;
            float ij = value2.I * value2.J;
            float jj = value2.J * value2.J;
            float kk = value2.K * value2.K;
            float kw = value2.K * value2.W;

            float t11 = 1 - 2 * (jj + kk);
            float t12 = 2 * (ij - kw);
            float t21 = 2 * (ij + kw);
            float t22 = 1 - 2 * (ii + kk);

            float x =
                value1.X * t11 +
                value1.Y * t12;

            float y =
                value1.X * t21 +
                value1.Y * t22;

            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Rotates the specified vector with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Matrix2"/>.</param>
        public static void Rotate(out Vector2 result, ref Vector2 value1, ref Matrix2 value2)
        {
            float x = value1.X * value2.M11 + value1.Y * value2.M12;
            float y = value1.X * value2.M21 + value1.Y * value2.M22;

            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Rotates the specified vector with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Matrix3"/>.</param>
        public static void Rotate(out Vector2 result, ref Vector2 value1, ref Matrix3 value2)
        {
            float x = value1.X * value2.M11 + value1.Y * value2.M12;
            float y = value1.X * value2.M21 + value1.Y * value2.M22;

            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Rotates the specified vector with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector2"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Rotate(out Vector2 result, ref Vector2 value1, ref Matrix value2)
        {
            float x = value1.X * value2.M11 + value1.Y * value2.M12;
            float y = value1.X * value2.M21 + value1.Y * value2.M22;

            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Rotates the specified vector with the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out Vector3 result, ref Vector3 value1, ref Quaternion value2)
        {
            float ii = value2.I * value2.I;
            float ij = value2.I * value2.J;
            float ik = value2.I * value2.K;
            float iw = value2.I * value2.W;
            float jj = value2.J * value2.J;
            float jk = value2.J * value2.K;
            float jw = value2.J * value2.W;
            float kk = value2.K * value2.K;
            float kw = value2.K * value2.W;

            float t11 = 1 - 2 * (jj + kk);
            float t12 = 2 * (ij - kw);
            float t13 = 2 * (ik + jw);
            float t21 = 2 * (ij + kw);
            float t22 = 1 - 2 * (ii + kk);
            float t23 = 2 * (jk - iw);
            float t31 = 2 * (ik - jw);
            float t32 = 2 * (jk + iw);
            float t33 = 1 - 2 * (ii + jj);

            float x =
                value1.X * t11 +
                value1.Y * t12 +
                value1.Z * t13;

            float y =
                value1.X * t21 +
                value1.Y * t22 +
                value1.Z * t23;

            float z =
                value1.X * t31 +
                value1.Y * t32 +
                value1.Z * t33;

            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Rotates the specified vector with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Matrix3"/>.</param>
        public static void Rotate(out Vector3 result, ref Vector3 value1, ref Matrix value2)
        {
            float x = value1.X * value2.M11 + value1.Y * value2.M12 + value1.Z * value2.M13;
            float y = value1.X * value2.M21 + value1.Y * value2.M22 + value1.Z * value2.M23;
            float z = value1.X * value2.M31 + value1.Y * value2.M32 + value1.Z * value2.M33;

            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Rotates the specified vector with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector3"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Rotate(out Vector3 result, ref Vector3 value1, ref Matrix3 value2)
        {
            float x = value1.X * value2.M11 + value1.Y * value2.M12 + value1.Z * value2.M13;
            float y = value1.X * value2.M21 + value1.Y * value2.M22 + value1.Z * value2.M23;
            float z = value1.X * value2.M31 + value1.Y * value2.M32 + value1.Z * value2.M33;

            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Rotates the specified vector with the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Vector4"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out Vector4 result, ref Vector4 value1, ref Quaternion value2)
        {
            float ii = value2.I * value2.I;
            float ij = value2.I * value2.J;
            float ik = value2.I * value2.K;
            float iw = value2.I * value2.W;
            float jj = value2.J * value2.J;
            float jk = value2.J * value2.K;
            float jw = value2.J * value2.W;
            float kk = value2.K * value2.K;
            float kw = value2.K * value2.W;

            float t11 = 1 - 2 * (jj + kk);
            float t12 = 2 * (ij - kw);
            float t13 = 2 * (ik + jw);
            float t21 = 2 * (ij + kw);
            float t22 = 1 - 2 * (ii + kk);
            float t23 = 2 * (jk - iw);
            float t31 = 2 * (ik - jw);
            float t32 = 2 * (jk + iw);
            float t33 = 1 - 2 * (ii + jj);

            float x =
                value1.X * t11 +
                value1.Y * t12 +
                value1.Z * t13;

            float y =
                value1.X * t21 +
                value1.Y * t22 +
                value1.Z * t23;

            float z =
                value1.X * t31 +
                value1.Y * t32 +
                value1.Z * t33;

            result.X = x;
            result.Y = y;
            result.Z = z;
            result.W = value1.W;
        }

        /// <summary>
        /// Creates a rotation quaternion from the specified Euler angles.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="yawAngleInRadians">Yaw angle in radians.</param>
        /// <param name="pitchAngleInRadians">Pitch angle in radians.</param>
        /// <param name="rollAngleInRadians">Roll angle in radians.</param>
        public static void Rotate(out Quaternion result, float yawAngleInRadians, float pitchAngleInRadians, float rollAngleInRadians)
        {
            //pitchAngleInRadians *= 0.5f;
            //float xsin = (float)System.Math.Sin(pitchAngleInRadians);
            //float xcos = (float)System.Math.Cos(pitchAngleInRadians);

            //yawAngleInRadians *= 0.5f;
            //float ysin = (float)System.Math.Sin(yawAngleInRadians);
            //float ycos = (float)System.Math.Cos(yawAngleInRadians);

            //rollAngleInRadians *= 0.5f;
            //float zsin = (float)System.Math.Sin(rollAngleInRadians);
            //float zcos = (float)System.Math.Cos(rollAngleInRadians);

            //result.W = (xcos * ycos * zcos) + (xsin * ysin * zsin);
            //result.I = (xsin * ycos * zcos) + (xcos * ysin * zsin);
            //result.J = (xcos * ysin * zcos) - (xsin * ycos * zsin);
            //result.K = (xcos * ycos * zsin) - (xsin * ysin * zcos);

            float halfRoll = rollAngleInRadians * 0.5f;
		    float sinRoll = (float)System.Math.Sin(halfRoll);
		    float cosRoll = (float)System.Math.Cos(halfRoll);
            float halfPitch = pitchAngleInRadians * 0.5f;
		    float sinPitch = (float)System.Math.Sin(halfPitch);
		    float cosPitch = (float)System.Math.Cos(halfPitch);
            float halfYaw = yawAngleInRadians * 0.5f;
		    float sinYaw = (float)System.Math.Sin(halfYaw);
            float cosYaw = (float)System.Math.Cos(halfYaw);
            result.I = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
            result.J = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
            result.K = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
            result.W = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);
        }

        /// <summary>
        /// Rotates the specified quaternion with the specified Euler angles.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        /// <param name="yawAngleInRadians">Yaw angle in radians.</param>
        /// <param name="pitchAngleInRadians">Pitch angle in radians.</param>
        /// <param name="rollAngleInRadians">Roll angle in radians.</param>
        public static void Rotate(out Quaternion result, ref Quaternion value, float yawAngleInRadians, float pitchAngleInRadians, float rollAngleInRadians)
        {
            pitchAngleInRadians *= 0.5f;
            float xsin = (float)System.Math.Sin(pitchAngleInRadians);
            float xcos = (float)System.Math.Cos(pitchAngleInRadians);

            yawAngleInRadians *= 0.5f;
            float ysin = (float)System.Math.Sin(yawAngleInRadians);
            float ycos = (float)System.Math.Cos(yawAngleInRadians);

            rollAngleInRadians *= 0.5f;
            float zsin = (float)System.Math.Sin(rollAngleInRadians);
            float zcos = (float)System.Math.Cos(rollAngleInRadians);

            float t = (xcos * ycos * zcos) + (xsin * ysin * zsin);
            float x = (xsin * ycos * zcos) + (xcos * ysin * zsin);
            float y = (xcos * ysin * zcos) - (xsin * ycos * zsin);
            float z = (xcos * ycos * zsin) - (xsin * ysin * zcos);

            float w = t * value.W - x * value.I - y * value.J - z * value.K;
            float i = t * value.I + x * value.W + y * value.K - z * value.J;
            float j = t * value.J - x * value.K + y * value.W + z * value.I;
            float k = t * value.K + x * value.J - y * value.I + z * value.W;

            result.W = w;
            result.I = i;
            result.J = j;
            result.K = k;
        }

        /// <summary>
        /// Creates a rotation quaternion from angular velocity.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="angularVelocity">Angular velocity vector.</param>
        public static void Rotate(out Quaternion result, ref Vector3 angularVelocity)
        {
            float len = angularVelocity.Length;

            if (len > 0.0001f)
            {
                float ang = len * 0.5f;
                float sin = (float)System.Math.Sin(ang) / len;
                float cos = (float)System.Math.Cos(ang);

                result.W = cos;
                result.I = sin * angularVelocity.X;
                result.J = sin * angularVelocity.Y;
                result.K = sin * angularVelocity.Z;
            }
            else
            {
                result.W = 1;
                result.I = 0;
                result.J = 0;
                result.K = 0;
            }
        }

        /// <summary>
        /// Rotates the specified quaternion around with the specified angular velocity.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        /// <param name="angularVelocity">Angular velocity vector.</param>
        public static void Rotate(out Quaternion result, ref Quaternion value, ref Vector3 angularVelocity)
        {
            float len = angularVelocity.Length;

            if (len > 0.0001f)
            {
                float ang = len * 0.5f;
                float sin = (float)System.Math.Sin(ang) / len;
                float cos = (float)System.Math.Cos(ang);

                float sinX = sin * angularVelocity.X;
                float sinY = sin * angularVelocity.Y;
                float sinZ = sin * angularVelocity.Z;

                float w = cos * value.W - sinX * value.I - sinY * value.J - sinZ * value.K;
                float i = cos * value.I + sinX * value.W + sinY * value.K - sinZ * value.J;
                float j = cos * value.J - sinX * value.K + sinY * value.W + sinZ * value.I;
                float k = cos * value.K + sinX * value.J - sinY * value.I + sinZ * value.W;

                result.W = w;
                result.I = i;
                result.J = j;
                result.K = k;
            }
            else
            {
                result.W = value.W;
                result.I = value.I;
                result.J = value.J;
                result.K = value.K;
            }
        }

        /// <summary>
        /// Creates a rotation quaternion from the specified axis and angle.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="axis">Rotation axis.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void Rotate(out Quaternion result, ref Vector3 axis, float angleInRadians)
        {
            angleInRadians *= 0.5f;
            float sin = (float)System.Math.Sin(angleInRadians);
            float cos = (float)System.Math.Cos(angleInRadians);

            result.W = cos;
            result.I = sin * axis.X;
            result.J = sin * axis.Y;
            result.K = sin * axis.Z;
        }

        /// <summary>
        /// Rotates the specified quaternion around the specified axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        /// <param name="axis">Rotation axis.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void Rotate(out Quaternion result, ref Quaternion value, ref Vector3 axis, float angleInRadians)
        {
            angleInRadians *= 0.5f;
            float sin = (float)System.Math.Sin(angleInRadians);
            float cos = (float)System.Math.Cos(angleInRadians);

            float sinX = sin * axis.X;
            float sinY = sin * axis.Y;
            float sinZ = sin * axis.Z;

            float w = cos * value.W - sinX * value.I - sinY * value.J - sinZ * value.K;
            float i = cos * value.I + sinX * value.W + sinY * value.K - sinZ * value.J;
            float j = cos * value.J - sinX * value.K + sinY * value.W + sinZ * value.I;
            float k = cos * value.K + sinX * value.J - sinY * value.I + sinZ * value.W;

            result.W = w;
            result.I = i;
            result.J = j;
            result.K = k;
        }

        /// <summary>
        /// Creates a rotation quaternion from the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix3"/>.</param>
        public static void Rotate(out Quaternion result, ref Matrix3 value)
        {
            if (value.M11 + value.M22 + value.M33 > 0)
            {
                float t = 1 + value.M11 + value.M22 + value.M33;
                float s = 0.5f / (float)System.Math.Sqrt(t);

                result.W = s * t;
                result.I = (value.M23 - value.M32) * s;
                result.J = (value.M31 - value.M13) * s;
                result.K = (value.M12 - value.M21) * s;
            }
            else if (value.M11 > value.M22 && value.M11 > value.M33)
            {
                float t = 1 + value.M11 - value.M22 - value.M33;
                float s = 0.5f / (float)System.Math.Sqrt(t);

                result.W = (value.M23 - value.M32) * s;
                result.I = s * t;
                result.J = (value.M12 + value.M21) * s;
                result.K = (value.M31 + value.M13) * s;
            }
            else if (value.M22 > value.M33)
            {
                float t = 1 - value.M11 + value.M22 - value.M33;
                float s = 0.5f / (float)System.Math.Sqrt(t);

                result.W = (value.M31 - value.M13) * s;
                result.I = (value.M12 + value.M21) * s;
                result.J = s * t;
                result.K = (value.M23 + value.M32) * s;
            }
            else
            {
                float t = 1 - value.M11 - value.M22 + value.M33;
                float s = 0.5f / (float)System.Math.Sqrt(t);

                result.W = (value.M12 - value.M21) * s;
                result.I = (value.M31 + value.M13) * s;
                result.J = (value.M23 + value.M32) * s;
                result.K = s * t;
            }
        }

        /// <summary>
        /// Creates a rotation quaternion from the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        public static void Rotate(out Quaternion result, ref Matrix value)
        {
            if (value.M11 + value.M22 + value.M33 > 0)
            {
                float t = 1 + value.M11 + value.M22 + value.M33;
                float s = 0.5f / (float)System.Math.Sqrt(t);

                result.W = s * t;
                result.I = (value.M23 - value.M32) * s;
                result.J = (value.M31 - value.M13) * s;
                result.K = (value.M12 - value.M21) * s;
            }
            else if (value.M11 > value.M22 && value.M11 > value.M33)
            {
                float t = 1 + value.M11 - value.M22 - value.M33;
                float s = 0.5f / (float)System.Math.Sqrt(t);

                result.W = (value.M23 - value.M32) * s;
                result.I = s * t;
                result.J = (value.M12 + value.M21) * s;
                result.K = (value.M31 + value.M13) * s;
            }
            else if (value.M22 > value.M33)
            {
                float t = 1 - value.M11 + value.M22 - value.M33;
                float s = 0.5f / (float)System.Math.Sqrt(t);

                result.W = (value.M31 - value.M13) * s;
                result.I = (value.M12 + value.M21) * s;
                result.J = s * t;
                result.K = (value.M23 + value.M32) * s;
            }
            else
            {
                float t = 1 - value.M11 - value.M22 + value.M33;
                float s = 0.5f / (float)System.Math.Sqrt(t);

                result.W = (value.M12 - value.M21) * s;
                result.I = (value.M31 + value.M13) * s;
                result.J = (value.M23 + value.M32) * s;
                result.K = s * t;
            }
        }

        /// <summary>
        /// Creates a transformation matrix from the specified Euler angles.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="yawAngleInRadians">Yaw angle in radians.</param>
        /// <param name="pitchAngleInRadians">Pitch angle in radians.</param>
        /// <param name="rollAngleInRadians">Roll angle in radians.</param>
        public static void Rotate(out Matrix3 result, float yawAngleInRadians, float pitchAngleInRadians, float rollAngleInRadians)
        {
            float sinR = (float)System.Math.Sin(rollAngleInRadians);
            float sinP = (float)System.Math.Sin(pitchAngleInRadians);
            float sinY = (float)System.Math.Sin(yawAngleInRadians);
            float cosR = (float)System.Math.Cos(rollAngleInRadians);
            float cosP = (float)System.Math.Cos(pitchAngleInRadians);
            float cosY = (float)System.Math.Cos(yawAngleInRadians);

            result.M11 = cosR * cosY - sinR * sinP * sinY;
            result.M12 = sinR * cosP;
            result.M13 = cosR * sinY + sinR * sinP * cosY;
            result.M21 = sinR * cosY + cosR * sinP * sinY;
            result.M22 = cosR * cosP;
            result.M23 = sinR * sinY - cosR * sinP * cosY;
            result.M31 = -cosP * sinY;
            result.M32 = sinP;
            result.M33 = cosP * cosY;
        }

        /// <summary>
        /// Rotates the specified matrix with the specified Euler angles.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix3"/>.</param>
        /// <param name="yawAngleInRadians">Yaw angle in radians.</param>
        /// <param name="pitchAngleInRadians">Pitch angle in radians.</param>
        /// <param name="rollAngleInRadians">Roll angle in radians.</param>
        public static void Rotate(out Matrix3 result, ref Matrix3 value, float yawAngleInRadians, float pitchAngleInRadians, float rollAngleInRadians)
        {
            float sinR = (float)System.Math.Sin(rollAngleInRadians);
            float sinP = (float)System.Math.Sin(pitchAngleInRadians);
            float sinY = (float)System.Math.Sin(yawAngleInRadians);
            float cosR = (float)System.Math.Cos(rollAngleInRadians);
            float cosP = (float)System.Math.Cos(pitchAngleInRadians);
            float cosY = (float)System.Math.Cos(yawAngleInRadians);

            float t11 = cosR * cosY - sinR * sinP * sinY;
            float t12 = sinR * cosP;
            float t13 = cosR * sinY + sinR * sinP * cosY;
            float t21 = sinR * cosY + cosR * sinP * sinY;
            float t22 = cosR * cosP;
            float t23 = sinR * sinY - cosR * sinP * cosY;
            float t31 = -cosP * sinY;
            float t32 = sinP;
            float t33 = cosP * cosY;

            float m11 = t11 * value.M11 + t12 * value.M21 + t13 * value.M31;
            float m12 = t11 * value.M12 + t12 * value.M22 + t13 * value.M32;
            float m13 = t11 * value.M13 + t12 * value.M23 + t13 * value.M33;
            float m21 = t21 * value.M11 + t22 * value.M21 + t23 * value.M31;
            float m22 = t21 * value.M12 + t22 * value.M22 + t23 * value.M32;
            float m23 = t21 * value.M13 + t22 * value.M23 + t23 * value.M33;
            float m31 = t31 * value.M11 + t32 * value.M21 + t33 * value.M31;
            float m32 = t31 * value.M12 + t32 * value.M22 + t33 * value.M32;
            float m33 = t31 * value.M13 + t32 * value.M23 + t33 * value.M33;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
        }

        /// <summary>
        /// Creates a transformation matrix from the specified rotation axis and angle.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="axis">Rotation axis vector.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void Rotate(out Matrix3 result, ref Vector3 axis, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            float oneMinusCosX = (1 - cos) * axis.X, sinX = sin * axis.X;
            float oneMinusCosY = (1 - cos) * axis.Y, sinY = sin * axis.Y;
            float oneMinusCosZ = (1 - cos) * axis.Z, sinZ = sin * axis.Z;

            result.M11 = oneMinusCosX * axis.X + cos;
            result.M12 = oneMinusCosX * axis.Y - sinZ;
            result.M13 = oneMinusCosX * axis.Z + sinY;
            result.M21 = oneMinusCosY * axis.X + sinZ;
            result.M22 = oneMinusCosY * axis.Y + cos;
            result.M23 = oneMinusCosY * axis.Z - sinX;
            result.M31 = oneMinusCosZ * axis.X - sinY;
            result.M32 = oneMinusCosZ * axis.Y + sinX;
            result.M33 = oneMinusCosZ * axis.Z + cos;
        }

        /// <summary>
        /// Rotates the specified matrix around the specified axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix3"/>.</param>
        /// <param name="axis">Rotation axis vector.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void Rotate(out Matrix3 result, ref Matrix3 value, ref Vector3 axis, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            float oneMinusCosX = (1 - cos) * axis.X, sinX = sin * axis.X;
            float oneMinusCosY = (1 - cos) * axis.Y, sinY = sin * axis.Y;
            float oneMinusCosZ = (1 - cos) * axis.Z, sinZ = sin * axis.Z;

            float t11 = oneMinusCosX * axis.X + cos;
            float t12 = oneMinusCosX * axis.Y - sinZ;
            float t13 = oneMinusCosX * axis.Z + sinY;
            float t21 = oneMinusCosY * axis.X + sinZ;
            float t22 = oneMinusCosY * axis.Y + cos;
            float t23 = oneMinusCosY * axis.Z - sinX;
            float t31 = oneMinusCosZ * axis.X - sinY;
            float t32 = oneMinusCosZ * axis.Y + sinX;
            float t33 = oneMinusCosZ * axis.Z + cos;

            float m11 = t11 * value.M11 + t12 * value.M21 + t13 * value.M31;
            float m12 = t11 * value.M12 + t12 * value.M22 + t13 * value.M32;
            float m13 = t11 * value.M13 + t12 * value.M23 + t13 * value.M33;
            float m21 = t21 * value.M11 + t22 * value.M21 + t23 * value.M31;
            float m22 = t21 * value.M12 + t22 * value.M22 + t23 * value.M32;
            float m23 = t21 * value.M13 + t22 * value.M23 + t23 * value.M33;
            float m31 = t31 * value.M11 + t32 * value.M21 + t33 * value.M31;
            float m32 = t31 * value.M12 + t32 * value.M22 + t33 * value.M32;
            float m33 = t31 * value.M13 + t32 * value.M23 + t33 * value.M33;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
        }

        /// <summary>
        /// Creates a transformation matrix from the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out Matrix3 result, ref Quaternion value)
        {
            float ii = value.I * value.I;
            float ij = value.I * value.J;
            float ik = value.I * value.K;
            float iw = value.I * value.W;
            float jj = value.J * value.J;
            float jk = value.J * value.K;
            float jw = value.J * value.W;
            float kk = value.K * value.K;
            float kw = value.K * value.W;

            result.M11 = 1 - 2 * (jj + kk);
            result.M12 = 2 * (ij - kw);
            result.M13 = 2 * (ik + jw);
            result.M21 = 2 * (ij + kw);
            result.M22 = 1 - 2 * (ii + kk);
            result.M23 = 2 * (jk - iw);
            result.M31 = 2 * (ik - jw);
            result.M32 = 2 * (jk + iw);
            result.M33 = 1 - 2 * (ii + jj);
        }

        /// <summary>
        /// Rotates the specified matrix with the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix3"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out Matrix3 result, ref Matrix3 value1, ref Quaternion value2)
        {
            float ii = value2.I * value2.I;
            float ij = value2.I * value2.J;
            float ik = value2.I * value2.K;
            float iw = value2.I * value2.W;
            float jj = value2.J * value2.J;
            float jk = value2.J * value2.K;
            float jw = value2.J * value2.W;
            float kk = value2.K * value2.K;
            float kw = value2.K * value2.W;

            float t11 = 1 - 2 * (jj + kk);
            float t12 = 2 * (ij - kw);
            float t13 = 2 * (ik + jw);
            float t21 = 2 * (ij + kw);
            float t22 = 1 - 2 * (ii + kk);
            float t23 = 2 * (jk - iw);
            float t31 = 2 * (ik - jw);
            float t32 = 2 * (jk + iw);
            float t33 = 1 - 2 * (ii + jj);

            float m11 = t11 * value1.M11 + t12 * value1.M21 + t13 * value1.M31;
            float m12 = t11 * value1.M12 + t12 * value1.M22 + t13 * value1.M32;
            float m13 = t11 * value1.M13 + t12 * value1.M23 + t13 * value1.M33;
            float m21 = t21 * value1.M11 + t22 * value1.M21 + t23 * value1.M31;
            float m22 = t21 * value1.M12 + t22 * value1.M22 + t23 * value1.M32;
            float m23 = t21 * value1.M13 + t22 * value1.M23 + t23 * value1.M33;
            float m31 = t31 * value1.M11 + t32 * value1.M21 + t33 * value1.M31;
            float m32 = t31 * value1.M12 + t32 * value1.M22 + t33 * value1.M32;
            float m33 = t31 * value1.M13 + t32 * value1.M23 + t33 * value1.M33;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
        }

        /// <summary>
        /// Creates a transformation matrix from the specified Euler angles.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="yawAngleInRadians">Yaw angle in radians.</param>
        /// <param name="pitchAngleInRadians">Pitch angle in radians.</param>
        /// <param name="rollAngleInRadians">Roll angle in radians.</param>
        public static void Rotate(out Matrix result, float yawAngleInRadians, float pitchAngleInRadians, float rollAngleInRadians)
        {
            float sinR = (float)System.Math.Sin(rollAngleInRadians);
            float sinP = (float)System.Math.Sin(pitchAngleInRadians);
            float sinY = (float)System.Math.Sin(yawAngleInRadians);
            float cosR = (float)System.Math.Cos(rollAngleInRadians);
            float cosP = (float)System.Math.Cos(pitchAngleInRadians);
            float cosY = (float)System.Math.Cos(yawAngleInRadians);

            result.M11 = cosR * cosY - sinR * sinP * sinY;
            result.M12 = sinR * cosP;
            result.M13 = cosR * sinY + sinR * sinP * cosY;
            result.M14 = 0;
            result.M21 = sinR * cosY + cosR * sinP * sinY;
            result.M22 = cosR * cosP;
            result.M23 = sinR * sinY - cosR * sinP * cosY;
            result.M24 = 0;
            result.M31 = -cosP * sinY;
            result.M32 = sinP;
            result.M33 = cosP * cosY;
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Rotates the specified matrix with the specified Euler angles.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        /// <param name="yawAngleInRadians">Yaw angle in radians.</param>
        /// <param name="pitchAngleInRadians">Pitch angle in radians.</param>
        /// <param name="rollAngleInRadians">Roll angle in radians.</param>
        public static void Rotate(out Matrix result, ref Matrix value, float yawAngleInRadians, float pitchAngleInRadians, float rollAngleInRadians)
        {
            float sinR = (float)System.Math.Sin(rollAngleInRadians);
            float sinP = (float)System.Math.Sin(pitchAngleInRadians);
            float sinY = (float)System.Math.Sin(yawAngleInRadians);
            float cosR = (float)System.Math.Cos(rollAngleInRadians);
            float cosP = (float)System.Math.Cos(pitchAngleInRadians);
            float cosY = (float)System.Math.Cos(yawAngleInRadians);

            float t11 = cosR * cosY - sinR * sinP * sinY;
            float t12 = sinR * cosP;
            float t13 = cosR * sinY + sinR * sinP * cosY;
            float t21 = sinR * cosY + cosR * sinP * sinY;
            float t22 = cosR * cosP;
            float t23 = sinR * sinY - cosR * sinP * cosY;
            float t31 = -cosP * sinY;
            float t32 = sinP;
            float t33 = cosP * cosY;

            float m11 = t11 * value.M11 + t12 * value.M21 + t13 * value.M31;
            float m12 = t11 * value.M12 + t12 * value.M22 + t13 * value.M32;
            float m13 = t11 * value.M13 + t12 * value.M23 + t13 * value.M33;
            float m14 = t11 * value.M14 + t12 * value.M24 + t13 * value.M34;
            float m21 = t21 * value.M11 + t22 * value.M21 + t23 * value.M31;
            float m22 = t21 * value.M12 + t22 * value.M22 + t23 * value.M32;
            float m23 = t21 * value.M13 + t22 * value.M23 + t23 * value.M33;
            float m24 = t21 * value.M14 + t22 * value.M24 + t23 * value.M34;
            float m31 = t31 * value.M11 + t32 * value.M21 + t33 * value.M31;
            float m32 = t31 * value.M12 + t32 * value.M22 + t33 * value.M32;
            float m33 = t31 * value.M13 + t32 * value.M23 + t33 * value.M33;
            float m34 = t31 * value.M14 + t32 * value.M24 + t33 * value.M34;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M14 = m14;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M24 = m24;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
            result.M34 = m34;
            result.M41 = value.M41;
            result.M42 = value.M42;
            result.M43 = value.M43;
            result.M44 = value.M44;
        }

        /// <summary>
        /// Creates a transformation matrix from the specified rotation axis and angle.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="axis">Rotation axis vector.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void Rotate(out Matrix result, ref Vector3 axis, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            float oneMinusCosX = (1 - cos) * axis.X, sinX = sin * axis.X;
            float oneMinusCosY = (1 - cos) * axis.Y, sinY = sin * axis.Y;
            float oneMinusCosZ = (1 - cos) * axis.Z, sinZ = sin * axis.Z;

            result.M11 = oneMinusCosX * axis.X + cos;
            result.M12 = oneMinusCosX * axis.Y - sinZ;
            result.M13 = oneMinusCosX * axis.Z + sinY;
            result.M14 = 0;
            result.M21 = oneMinusCosY * axis.X + sinZ;
            result.M22 = oneMinusCosY * axis.Y + cos;
            result.M23 = oneMinusCosY * axis.Z - sinX;
            result.M24 = 0;
            result.M31 = oneMinusCosZ * axis.X - sinY;
            result.M32 = oneMinusCosZ * axis.Y + sinX;
            result.M33 = oneMinusCosZ * axis.Z + cos;
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Rotates the specified matrix around the specified axis.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Matrix4"/>.</param>
        /// <param name="axis">Rotation axis vector.</param>
        /// <param name="angleInRadians">Rotation angle in radians.</param>
        public static void Rotate(out Matrix result, ref Matrix value, ref Vector3 axis, float angleInRadians)
        {
            float cos = (float)System.Math.Cos(angleInRadians);
            float sin = (float)System.Math.Sin(angleInRadians);

            float oneMinusCosX = (1 - cos) * axis.X, sinX = sin * axis.X;
            float oneMinusCosY = (1 - cos) * axis.Y, sinY = sin * axis.Y;
            float oneMinusCosZ = (1 - cos) * axis.Z, sinZ = sin * axis.Z;

            float t11 = oneMinusCosX * axis.X + cos;
            float t12 = oneMinusCosX * axis.Y - sinZ;
            float t13 = oneMinusCosX * axis.Z + sinY;
            float t21 = oneMinusCosY * axis.X + sinZ;
            float t22 = oneMinusCosY * axis.Y + cos;
            float t23 = oneMinusCosY * axis.Z - sinX;
            float t31 = oneMinusCosZ * axis.X - sinY;
            float t32 = oneMinusCosZ * axis.Y + sinX;
            float t33 = oneMinusCosZ * axis.Z + cos;

            float m11 = t11 * value.M11 + t12 * value.M21 + t13 * value.M31;
            float m12 = t11 * value.M12 + t12 * value.M22 + t13 * value.M32;
            float m13 = t11 * value.M13 + t12 * value.M23 + t13 * value.M33;
            float m14 = t11 * value.M14 + t12 * value.M24 + t13 * value.M34;
            float m21 = t21 * value.M11 + t22 * value.M21 + t23 * value.M31;
            float m22 = t21 * value.M12 + t22 * value.M22 + t23 * value.M32;
            float m23 = t21 * value.M13 + t22 * value.M23 + t23 * value.M33;
            float m24 = t21 * value.M14 + t22 * value.M24 + t23 * value.M34;
            float m31 = t31 * value.M11 + t32 * value.M21 + t33 * value.M31;
            float m32 = t31 * value.M12 + t32 * value.M22 + t33 * value.M32;
            float m33 = t31 * value.M13 + t32 * value.M23 + t33 * value.M33;
            float m34 = t31 * value.M14 + t32 * value.M24 + t33 * value.M34;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M14 = m14;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M24 = m24;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
            result.M34 = m34;
            result.M41 = value.M41;
            result.M42 = value.M42;
            result.M43 = value.M43;
            result.M44 = value.M44;
        }

        /// <summary>
        /// Creates a transformation matrix from the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out Matrix result, ref Quaternion quaternion)
        {
            float xx = quaternion.I * quaternion.I;
            float yy = quaternion.J * quaternion.J;
            float zz = quaternion.K * quaternion.K;
            float xy = quaternion.I * quaternion.J;
            float zw = quaternion.K * quaternion.W;
            float zx = quaternion.K * quaternion.I;
            float yw = quaternion.J * quaternion.W;
            float yz = quaternion.J * quaternion.K;
            float xw = quaternion.I * quaternion.W;
            result.M11 = 1.0f - (2.0f * (yy + zz));
            result.M12 = 2.0f * (xy + zw);
            result.M13 = 2.0f * (zx - yw);
            result.M14 = 0.0f;
            result.M21 = 2.0f * (xy - zw);
            result.M22 = 1.0f - (2.0f * (zz + xx));
            result.M23 = 2.0f * (yz + xw);
            result.M24 = 0.0f;
            result.M31 = 2.0f * (zx + yw);
            result.M32 = 2.0f * (yz - xw);
            result.M33 = 1.0f - (2.0f * (yy + xx));
            result.M34 = 0.0f;
            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
            result.M44 = 1.0f;
            //float ii = value.I * value.I;
            //float ij = value.I * value.J;
            //float ik = value.I * value.K;
            //float iw = value.I * value.W;
            //float jj = value.J * value.J;
            //float jk = value.J * value.K;
            //float jw = value.J * value.W;
            //float kk = value.K * value.K;
            //float kw = value.K * value.W;

            //result.M11 = 1 - 2 * (jj + kk);
            //result.M12 = 2 * (ij - kw);
            //result.M13 = 2 * (ik + jw);
            //result.M14 = 0;
            //result.M21 = 2 * (ij + kw);
            //result.M22 = 1 - 2 * (ii + kk);
            //result.M23 = 2 * (jk - iw);
            //result.M24 = 0;
            //result.M31 = 2 * (ik - jw);
            //result.M32 = 2 * (jk + iw);
            //result.M33 = 1 - 2 * (ii + jj);
            //result.M34 = 0;
            //result.M41 = 0;
            //result.M42 = 0;
            //result.M43 = 0;
            //result.M44 = 1;
        }

        /// <summary>
        /// Rotates the specified matrix with the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Matrix4"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out Matrix result, ref Matrix value1, ref Quaternion value2)
        {
            float ii = value2.I * value2.I;
            float ij = value2.I * value2.J;
            float ik = value2.I * value2.K;
            float iw = value2.I * value2.W;
            float jj = value2.J * value2.J;
            float jk = value2.J * value2.K;
            float jw = value2.J * value2.W;
            float kk = value2.K * value2.K;
            float kw = value2.K * value2.W;

            float t11 = 1 - 2 * (jj + kk);
            float t12 = 2 * (ij - kw);
            float t13 = 2 * (ik + jw);
            float t21 = 2 * (ij + kw);
            float t22 = 1 - 2 * (ii + kk);
            float t23 = 2 * (jk - iw);
            float t31 = 2 * (ik - jw);
            float t32 = 2 * (jk + iw);
            float t33 = 1 - 2 * (ii + jj);

            float m11 = t11 * value1.M11 + t12 * value1.M21 + t13 * value1.M31;
            float m12 = t11 * value1.M12 + t12 * value1.M22 + t13 * value1.M32;
            float m13 = t11 * value1.M13 + t12 * value1.M23 + t13 * value1.M33;
            float m14 = t11 * value1.M14 + t12 * value1.M24 + t13 * value1.M34;
            float m21 = t21 * value1.M11 + t22 * value1.M21 + t23 * value1.M31;
            float m22 = t21 * value1.M12 + t22 * value1.M22 + t23 * value1.M32;
            float m23 = t21 * value1.M13 + t22 * value1.M23 + t23 * value1.M33;
            float m24 = t21 * value1.M14 + t22 * value1.M24 + t23 * value1.M34;
            float m31 = t31 * value1.M11 + t32 * value1.M21 + t33 * value1.M31;
            float m32 = t31 * value1.M12 + t32 * value1.M22 + t33 * value1.M32;
            float m33 = t31 * value1.M13 + t32 * value1.M23 + t33 * value1.M33;
            float m34 = t31 * value1.M14 + t32 * value1.M24 + t33 * value1.M34;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M14 = m14;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M24 = m24;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
            result.M34 = m34;
            result.M41 = value1.M41;
            result.M42 = value1.M42;
            result.M43 = value1.M43;
            result.M44 = value1.M44;
        }

        /// <summary>
        /// Rotates the specified ray with the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out Ray result, ref Ray value1, ref Quaternion value2)
        {
            float ii = value2.I * value2.I;
            float ij = value2.I * value2.J;
            float ik = value2.I * value2.K;
            float iw = value2.I * value2.W;
            float jj = value2.J * value2.J;
            float jk = value2.J * value2.K;
            float jw = value2.J * value2.W;
            float kk = value2.K * value2.K;
            float kw = value2.K * value2.W;

            float t11 = 1 - 2 * (jj + kk);
            float t12 = 2 * (ij - kw);
            float t13 = 2 * (ik + jw);
            float t21 = 2 * (ij + kw);
            float t22 = 1 - 2 * (ii + kk);
            float t23 = 2 * (jk - iw);
            float t31 = 2 * (ik - jw);
            float t32 = 2 * (jk + iw);
            float t33 = 1 - 2 * (ii + jj);

            float x =
                value1.Direction.X * t11 +
                value1.Direction.Y * t12 +
                value1.Direction.Z * t13;

            float y =
                value1.Direction.X * t21 +
                value1.Direction.Y * t22 +
                value1.Direction.Z * t23;

            float z =
                value1.Direction.X * t31 +
                value1.Direction.Y * t32 +
                value1.Direction.Z * t33;

            result.Position.X = value1.Position.X;
            result.Position.Y = value1.Position.Y;
            result.Position.Z = value1.Position.Z;
            result.Direction.X = x;
            result.Direction.Y = y;
            result.Direction.Z = z;
        }

        /// <summary>
        /// Rotates the specified ray with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Matrix3"/>.</param>
        public static void Rotate(out Ray result, ref Ray value1, ref Matrix3 value2)
        {
            float dx = value1.Direction.X;
            float dy = value1.Direction.Y;
            float dz = value1.Direction.Z;

            result.Position.X = value1.Position.X;
            result.Position.Y = value1.Position.Y;
            result.Position.Z = value1.Position.Z;
            result.Direction.X = dx * value2.M11 + dy * value2.M12 + dz * value2.M13;
            result.Direction.Y = dx * value2.M21 + dy * value2.M22 + dz * value2.M23;
            result.Direction.Z = dx * value2.M31 + dy * value2.M32 + dz * value2.M33;
        }

        /// <summary>
        /// Rotates the specified ray with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Ray"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Rotate(out Ray result, ref Ray value1, ref Matrix value2)
        {
            float dx = value1.Direction.X;
            float dy = value1.Direction.Y;
            float dz = value1.Direction.Z;

            result.Position.X = value1.Position.X;
            result.Position.Y = value1.Position.Y;
            result.Position.Z = value1.Position.Z;
            result.Direction.X = dx * value2.M11 + dy * value2.M12 + dz * value2.M13;
            result.Direction.Y = dx * value2.M21 + dy * value2.M22 + dz * value2.M23;
            result.Direction.Z = dx * value2.M31 + dy * value2.M32 + dz * value2.M33;
        }

        /// <summary>
        /// Rotates the specified plane with the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out Plane result, ref Plane value1, ref Quaternion value2)
        {
            float ii = value2.I * value2.I;
            float ij = value2.I * value2.J;
            float ik = value2.I * value2.K;
            float iw = value2.I * value2.W;
            float jj = value2.J * value2.J;
            float jk = value2.J * value2.K;
            float jw = value2.J * value2.W;
            float kk = value2.K * value2.K;
            float kw = value2.K * value2.W;

            float t11 = 1 - 2 * (jj + kk);
            float t12 = 2 * (ij - kw);
            float t13 = 2 * (ik + jw);
            float t21 = 2 * (ij + kw);
            float t22 = 1 - 2 * (ii + kk);
            float t23 = 2 * (jk - iw);
            float t31 = 2 * (ik - jw);
            float t32 = 2 * (jk + iw);
            float t33 = 1 - 2 * (ii + jj);

            float x = value1.Normal.X;
            float y = value1.Normal.Y;
            float z = value1.Normal.Z;

            result.Normal.X = x * t11 + y * t12 + z * t13;
            result.Normal.Y = x * t21 + y * t22 + z * t23;
            result.Normal.Z = x * t31 + y * t32 + z * t33;
            result.D = value1.D;
        }

        /// <summary>
        /// Rotates the specified plane with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Matrix3"/>.</param>
        public static void Rotate(out Plane result, ref Plane value1, ref Matrix3 value2)
        {
            Matrix3 matrix;
            Invert(out matrix, ref value2);

            float x = value1.Normal.X;
            float y = value1.Normal.Y;
            float z = value1.Normal.Z;
            float d = value1.D;

            result.Normal.X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31;
            result.Normal.Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32;
            result.Normal.Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33;
            result.D = d;
        }

        /// <summary>
        /// Rotates the specified plane with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Plane"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Rotate(out Plane result, ref Plane value1, ref Matrix value2)
        {
            Matrix matrix;
            Invert(out matrix, ref value2);

            float x = value1.Normal.X;
            float y = value1.Normal.Y;
            float z = value1.Normal.Z;
            float d = value1.D;

            result.Normal.X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31;
            result.Normal.Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32;
            result.Normal.Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33;
            result.D = d;
        }

        /// <summary>
        /// Rotates the specified box with the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out BoundingBox result, ref BoundingBox value1, ref Quaternion value2)
        {
            float ii = value2.I * value2.I;
            float ij = value2.I * value2.J;
            float ik = value2.I * value2.K;
            float iw = value2.I * value2.W;
            float jj = value2.J * value2.J;
            float jk = value2.J * value2.K;
            float jw = value2.J * value2.W;
            float kk = value2.K * value2.K;
            float kw = value2.K * value2.W;

            float t11 = 1 - 2 * (jj + kk);
            float t12 = 2 * (ij - kw);
            float t13 = 2 * (ik + jw);
            float t21 = 2 * (ij + kw);
            float t22 = 1 - 2 * (ii + kk);
            float t23 = 2 * (jk - iw);
            float t31 = 2 * (ik - jw);
            float t32 = 2 * (jk + iw);
            float t33 = 1 - 2 * (ii + jj);

            float radiusX = (value1.Maximum.X - value1.Minimum.X) * 0.5f;
            float radiusY = (value1.Maximum.Y - value1.Minimum.Y) * 0.5f;
            float radiusZ = (value1.Maximum.Z - value1.Minimum.Z) * 0.5f;
            float centerX = (value1.Minimum.X + value1.Maximum.X) * 0.5f;
            float centerY = (value1.Minimum.Y + value1.Maximum.Y) * 0.5f;
            float centerZ = (value1.Minimum.Z + value1.Maximum.Z) * 0.5f;

            float x =
                centerX * t11 +
                centerY * t12 +
                centerZ * t13;

            float y =
                centerX * t21 +
                centerY * t22 +
                centerZ * t23;

            float z =
                centerX * t31 +
                centerY * t32 +
                centerZ * t33;

            result.Minimum.X = x - radiusX;
            result.Minimum.Y = y - radiusY;
            result.Minimum.Z = z - radiusZ;
            result.Maximum.X = x + radiusX;
            result.Maximum.Y = y + radiusY;
            result.Maximum.Z = z + radiusZ;
        }

        /// <summary>
        /// Rotates the specified box with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Matrix3"/>.</param>
        public static void Rotate(out BoundingBox result, ref BoundingBox value1, ref Matrix3 value2)
        {
            float radiusX = (value1.Maximum.X - value1.Minimum.X) * 0.5f;
            float radiusY = (value1.Maximum.Y - value1.Minimum.Y) * 0.5f;
            float radiusZ = (value1.Maximum.Z - value1.Minimum.Z) * 0.5f;
            float centerX = (value1.Minimum.X + value1.Maximum.X) * 0.5f;
            float centerY = (value1.Minimum.Y + value1.Maximum.Y) * 0.5f;
            float centerZ = (value1.Minimum.Z + value1.Maximum.Z) * 0.5f;

            float x = centerX * value2.M11 + centerY * value2.M12 + centerZ * value2.M13;
            float y = centerX * value2.M21 + centerY * value2.M22 + centerZ * value2.M23;
            float z = centerX * value2.M31 + centerY * value2.M32 + centerZ * value2.M33;

            result.Minimum.X = x - radiusX;
            result.Minimum.Y = y - radiusY;
            result.Minimum.Z = z - radiusZ;
            result.Maximum.X = x + radiusX;
            result.Maximum.Y = y + radiusY;
            result.Maximum.Z = z + radiusZ;
        }

        /// <summary>
        /// Rotates the specified box with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Box"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Rotate(out BoundingBox result, ref BoundingBox value1, ref Matrix value2)
        {
            float radiusX = (value1.Maximum.X - value1.Minimum.X) * 0.5f;
            float radiusY = (value1.Maximum.Y - value1.Minimum.Y) * 0.5f;
            float radiusZ = (value1.Maximum.Z - value1.Minimum.Z) * 0.5f;
            float centerX = (value1.Minimum.X + value1.Maximum.X) * 0.5f;
            float centerY = (value1.Minimum.Y + value1.Maximum.Y) * 0.5f;
            float centerZ = (value1.Minimum.Z + value1.Maximum.Z) * 0.5f;

            float x = centerX * value2.M11 + centerY * value2.M12 + centerZ * value2.M13;
            float y = centerX * value2.M21 + centerY * value2.M22 + centerZ * value2.M23;
            float z = centerX * value2.M31 + centerY * value2.M32 + centerZ * value2.M33;

            result.Minimum.X = x - radiusX;
            result.Minimum.Y = y - radiusY;
            result.Minimum.Z = z - radiusZ;
            result.Maximum.X = x + radiusX;
            result.Maximum.Y = y + radiusY;
            result.Maximum.Z = z + radiusZ;
        }

        /// <summary>
        /// Rotates the specified sphere with the specified quaternion.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Quaternion"/>.</param>
        public static void Rotate(out Sphere result, ref Sphere value1, ref Quaternion value2)
        {
            float ii = value2.I * value2.I;
            float ij = value2.I * value2.J;
            float ik = value2.I * value2.K;
            float iw = value2.I * value2.W;
            float jj = value2.J * value2.J;
            float jk = value2.J * value2.K;
            float jw = value2.J * value2.W;
            float kk = value2.K * value2.K;
            float kw = value2.K * value2.W;

            float t11 = 1 - 2 * (jj + kk);
            float t12 = 2 * (ij - kw);
            float t13 = 2 * (ik + jw);
            float t21 = 2 * (ij + kw);
            float t22 = 1 - 2 * (ii + kk);
            float t23 = 2 * (jk - iw);
            float t31 = 2 * (ik - jw);
            float t32 = 2 * (jk + iw);
            float t33 = 1 - 2 * (ii + jj);

            float x =
                value1.Position.X * t11 +
                value1.Position.Y * t12 +
                value1.Position.Z * t13;

            float y =
                value1.Position.X * t21 +
                value1.Position.Y * t22 +
                value1.Position.Z * t23;

            float z =
                value1.Position.X * t31 +
                value1.Position.Y * t32 +
                value1.Position.Z * t33;

            result.Position.X = x;
            result.Position.Y = y;
            result.Position.Z = z;
            result.Radius = value1.Radius;
        }

        /// <summary>
        /// Rotates the specified sphere with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Matrix3"/>.</param>
        public static void Rotate(out Sphere result, ref Sphere value1, ref Matrix3 value2)
        {
            float x = value1.Position.X;
            float y = value1.Position.Y;
            float z = value1.Position.Z;

            result.Position.X = x * value2.M11 + y * value2.M12 + z * value2.M13;
            result.Position.Y = x * value2.M21 + y * value2.M22 + z * value2.M23;
            result.Position.Z = x * value2.M31 + y * value2.M32 + z * value2.M33;
            result.Radius = value1.Radius;
        }

        /// <summary>
        /// Rotates the specified sphere with the specified matrix.
        /// </summary>
        /// <param name="result">Output variable for the result.</param>
        /// <param name="value1">A <see cref="Sphere"/>.</param>
        /// <param name="value2">A <see cref="Matrix4"/>.</param>
        public static void Rotate(out Sphere result, ref Sphere value1, ref Matrix value2)
        {
            float x = value1.Position.X;
            float y = value1.Position.Y;
            float z = value1.Position.Z;

            result.Position.X = x * value2.M11 + y * value2.M12 + z * value2.M13;
            result.Position.Y = x * value2.M21 + y * value2.M22 + z * value2.M23;
            result.Position.Z = x * value2.M31 + y * value2.M32 + z * value2.M33;
            result.Radius = value1.Radius;
        }
    }
}
