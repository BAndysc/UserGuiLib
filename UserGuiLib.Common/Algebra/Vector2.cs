using System;

namespace UserGuiLib.Common.Algebra
{
    public struct Vector2
    {
        public static Vector2 Zero = new Vector2(0, 0);
        public static Vector2 One = new Vector2(1, 1);
        public static Vector2 Half = new Vector2(0.5f, 0.5f);
        public float x;
        public float y;

        public float length
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y);
            }
        }

        public Vector2(float x, float y)
        {
            if (float.IsNaN(x))
                throw new InvalidOperationException();
            this.x = x;
            this.y = y;
        }

        public Vector2 ToUnitVector()
        {
            float len = length;
            if (len == 0)
                return Zero;
            return new Vector2(x / len, y / len);
        }

        public Vector2 Clamp(Vector2 min, Vector2 max)
        {
            float x = this.x.Clamp(min.x, max.x);
            float y = this.y.Clamp(min.y, max.y);
            return new Vector2(x, y);
        }

        public double GetAngle(Vector2 other)
        {
            var diff = this - other;
            return Math.Atan2(diff.x, diff.y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }

        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(a.x / b, a.y / b);
        }

        public static Vector2 operator /(float a, Vector2 b)
        {
            return new Vector2(a / b.x, a / b.y);
        }
    }
}
