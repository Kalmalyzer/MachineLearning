using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public struct Vector2i : IEquatable<Vector2i>
    {
        public int X, Y;

        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return (obj.GetType() == typeof(Vector2i)) && this.Equals((Vector2i)obj);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public bool Equals(Vector2i v)
        {
            return (X == v.X) && (Y == v.Y);
        }

        public override string ToString()
        {
            return "<" + X.ToString() + ", " + Y.ToString() + ">";
        }

        public static readonly Vector2i Zero = new Vector2i(0, 0);
        public static readonly Vector2i One = new Vector2i(1, 1);
        public static readonly Vector2i UnitX = new Vector2i(1, 0);
        public static readonly Vector2i UnitY = new Vector2i(0, 1);

        public static Vector2i Abs(Vector2i v)
        {
            return new Vector2i(Math.Abs(v.X), Math.Abs(v.Y));
        }

        public static Vector2i Max(Vector2i v0, Vector2i v1)
        {
            return new Vector2i(Math.Max(v0.X, v1.X), Math.Max(v0.Y, v1.Y));
        }

        public static Vector2i Min(Vector2i v0, Vector2i v1)
        {
            return new Vector2i(Math.Min(v0.X, v1.X), Math.Min(v0.Y, v1.Y));
        }

        public static Vector2i Add(Vector2i v0, Vector2i v1)
        {
            return new Vector2i(v0.X + v1.X, v0.Y + v1.Y);
        }

        public static Vector2i Subtract(Vector2i v0, Vector2i v1)
        {
            return new Vector2i(v0.X - v1.X, v0.Y - v1.Y);
        }

        public static Vector2i Negate(Vector2i v)
        {
            return new Vector2i(-v.X, -v.Y);
        }

        public static Vector2i operator +(Vector2i v0, Vector2i v1)
        {
            return Add(v0, v1);
        }

        public static Vector2i operator -(Vector2i v)
        {
            return Negate(v);
        }

        public static Vector2i operator -(Vector2i v0, Vector2i v1)
        {
            return Subtract(v0, v1);
        }

        public static bool operator ==(Vector2i v0, Vector2i v1)
        {
            return v0.Equals(v1);
        }

        public static bool operator !=(Vector2i v0, Vector2i v1)
        {
            return !v0.Equals(v1);
        }
    }
}
