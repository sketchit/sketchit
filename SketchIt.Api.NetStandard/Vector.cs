using SketchIt.Api.Static;
using System;

namespace SketchIt.Api
{
    public struct Vector : ICloneable
    {
        public static readonly Vector Empty = new Vector();

        public Vector(float x, float y)
            : this(x, y, 0)
        {
        }

        public Vector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool IsEmpty
        {
            get
            {
                return !(X != 0 || Y != 0 || Z != 0);
            }
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Z
        {
            get;
            set;
        }

        public void Add(float scalar)
        {
            X += scalar;
            Y += scalar;
        }

        public void Add(Vector vector)
        {
            X += vector.X;
            Y += vector.Y;
        }

        public void Subtract(float scalar)
        {
            X -= scalar;
            Y -= scalar;
        }

        public void Subtract(Vector vector)
        {
            X -= vector.X;
            Y -= vector.Y;
        }

        public void Multiply(float scalar)
        {
            X *= scalar;
            Y *= scalar;
        }

        public void Multiply(Vector vector)
        {
            X *= vector.X;
            Y *= vector.Y;
        }

        public void Divide(float scalar)
        {
            X /= scalar;
            Y /= scalar;
        }

        public void Divide(Vector vector)
        {
            X /= vector.X;
            Y /= vector.Y;
        }

        public float GetMagnitude()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public float GetHeading()
        {
            if (Functions.AngleMode == AngleMode.Radians)
            {
                return (float)(Math.Atan2(this.Y, this.X));
            }
            else
            {
                return (float)(Math.Atan2(this.Y, this.X) * 180.0 / Math.PI);
            }
        }

        public void SetMagnitude(float value)
        {
            Normalize();
            Multiply(value);
        }

        public void Normalize()
        {
            float m = GetMagnitude();

            X /= m;
            Y /= m;

            if (float.IsNaN(X)) X = 0;
            if (float.IsNaN(Y)) Y = 0;
        }

        public void Limit(float max)
        {
            if (GetMagnitude() > max)
            {
                Normalize();
                Multiply(max);
            }
        }

        public Vector Clone()
        {
            return new Vector(X, Y, Z);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", X, Y, Z);
        }

        public static Vector Random2D()
        {
            return new Vector(Functions.Random(), Functions.Random());
        }

        public static Vector FromAngle(float angle)
        {
            return new Vector(Functions.Cos(angle), Functions.Sin(angle));
        }

        public static Vector operator +(Vector v1, float scalar) { return Add(v1, new Vector(scalar, scalar)); }
        public static Vector operator +(Vector v1, Vector v2) { return Add(v1, v2); }
        public static Vector Add(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v1, float scalar) { return Subtract(v1, new Vector(scalar, scalar)); }
        public static Vector operator -(Vector v1, Vector v2) { return Subtract(v1, v2); }
        public static Vector Subtract(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector operator *(Vector v1, float scalar) { return Multiply(v1, new Vector(scalar, scalar)); }
        public static Vector operator *(Vector v1, Vector v2) { return Multiply(v1, v2); }
        public static Vector Multiply(Vector v1, Vector v2)
        {
            return new Vector(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static Vector operator /(Vector v1, float scalar) { return Divide(v1, new Vector(scalar, scalar)); }
        public static Vector operator /(Vector v1, Vector v2) { return Divide(v1, v2); }
        public static Vector Divide(Vector v1, Vector v2)
        {
            return new Vector(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }
    }
}
