using System;

namespace Mapbox.Utils
{
    using System.Globalization;
    using UnityEngine;

    /// <summary>
    /// Represents a three-dimensional vector with double precision.
    /// </summary>
    [Serializable]
    public struct Vector3d
    {
        public const double kEpsilon = 1E-05d;
        public double x;
        public double y;
        public double z;

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3d index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3d index!");
                }
            }
        }

        public Vector3d normalized
        {
            get
            {
                Vector3d vector3d = new Vector3d(x, y, z);
                vector3d.Normalize();
                return vector3d;
            }
        }

        public double magnitude
        {
            get
            {
                return Math.Sqrt(x * x + y * y + z * z);
            }
        }

        public double sqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        public static Vector3d zero
        {
            get
            {
                return new Vector3d(0.0d, 0.0d, 0.0d);
            }
        }

        public static Vector3d one
        {
            get
            {
                return new Vector3d(1d, 1d, 1d);
            }
        }

        public static Vector3d forward
        {
            get
            {
                return new Vector3d(0.0d, 0.0d, 1d);
            }
        }

        public static Vector3d up
        {
            get
            {
                return new Vector3d(0.0d, 1d, 0.0d);
            }
        }

        public static Vector3d right
        {
            get
            {
                return new Vector3d(1d, 0.0d, 0.0d);
            }
        }

        public Vector3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3d operator +(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3d operator -(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3d operator -(Vector3d a)
        {
            return new Vector3d(-a.x, -a.y, -a.z);
        }

        public static Vector3d operator *(Vector3d a, double d)
        {
            return new Vector3d(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3d operator *(double d, Vector3d a)
        {
            return new Vector3d(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3d operator /(Vector3d a, double d)
        {
            return new Vector3d(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Vector3d lhs, Vector3d rhs)
        {
            return Vector3d.SqrMagnitude(lhs - rhs) < 0.0 / 1.0;
        }

        public static bool operator !=(Vector3d lhs, Vector3d rhs)
        {
            return Vector3d.SqrMagnitude(lhs - rhs) >= 0.0 / 1.0;
        }

        public void Set(double new_x, double new_y, double new_z)
        {
            x = new_x;
            y = new_y;
            z = new_z;
        }

        public static Vector3d Lerp(Vector3d from, Vector3d to, double t)
        {
            t = Mathd.Clamp01(t);
            return new Vector3d(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
        }

        public static Vector3d MoveTowards(Vector3d current, Vector3d target, double maxDistanceDelta)
        {
            Vector3d vector3d = target - current;
            double magnitude = vector3d.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0.0d)
                return target;
            else
                return current + vector3d / magnitude * maxDistanceDelta;
        }

        public static Vector3d Scale(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public void Scale(Vector3d scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        public void Normalize()
        {
            double magnitude = this.magnitude;
            if (magnitude > 9.99999974737875E-06)
                this = this / magnitude;
            else
                this = Vector3d.zero;
        }

        public override string ToString()
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "{0:F5},{1:F5},{2:F5}", x, y, z);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector3d))
                return false;
            Vector3d vector3d = (Vector3d)other;
            if (x.Equals(vector3d.x))
                return y.Equals(vector3d.y) && z.Equals(vector3d.z);
            else
                return false;
        }

        public static double Dot(Vector3d lhs, Vector3d rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        public static double Angle(Vector3d from, Vector3d to)
        {
            return Mathd.Acos(Mathd.Clamp(Vector3d.Dot(from.normalized, to.normalized), -1d, 1d)) * 57.29578d;
        }

        public static double Distance(Vector3d a, Vector3d b)
        {
            return (a - b).magnitude;
        }

        public static Vector3d ClampMagnitude(Vector3d vector, double maxLength)
        {
            if (vector.sqrMagnitude > maxLength * maxLength)
                return vector.normalized * maxLength;
            else
                return vector;
        }

        public double[] ToArray()
        {
            return new double[] { x, y, z };
        }

        // 保留 Vector2d 的所有方法
        public static Vector3d Min(Vector3d lhs, Vector3d rhs)
        {
            return new Vector3d(Mathd.Min(lhs.x, rhs.x), Mathd.Min(lhs.y, rhs.y), Mathd.Min(lhs.z, rhs.z));
        }

        public static Vector3d Max(Vector3d lhs, Vector3d rhs)
        {
            return new Vector3d(Mathd.Max(lhs.x, rhs.x), Mathd.Max(lhs.y, rhs.y), Mathd.Max(lhs.z, rhs.z));
        }

        public double SqrMagnitude()
        {
            return (this.x * this.x + this.y * this.y + this.z * this.z);
        }

        public static double SqrMagnitude(Vector3d a)
        {
            return (a.x * a.x + a.y * a.y + a.z * a.z);
        }
        public static Vector3d GetVector3d(Vector3 v)
        {
            return new Vector3d(v.x, v.y, v.z);
        }

    }


}