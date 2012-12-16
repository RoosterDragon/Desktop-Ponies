namespace CsDesktopPonies
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Represents an ordered pair of integer x- and y-coordinates that defines a point in a two-dimensional plane.
    /// </summary>
    [Serializable]
    public struct Vector2 : IEquatable<Vector2>
    {
        /// <summary>
        /// Represents the vector whose components are all zero.
        /// </summary>
        public static readonly Vector2 Zero;

        /// <summary>
        /// The x component of the vector.
        /// </summary>
        public int X;
        /// <summary>
        /// The y component of the vector.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.Vector2"/> structure.
        /// </summary>
        /// <param name='x'>The x component of the vector.</param>
        /// <param name='y'>The y component of the vector.</param>
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.Vector2"/> structure.
        /// </summary>
        /// <param name='point'>The point whose x- and y-coordinates are used to initialize the vector.</param>
        public Vector2(Point point)
        {
            X = point.X;
            Y = point.Y;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.Vector2"/> structure.
        /// </summary>
        /// <param name='size'>The size whose width and height values are used to initialize the vector.</param>
        public Vector2(Size size)
        {
            X = size.Width;
            Y = size.Height;
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X - right.X, left.Y - right.Y);
        }
        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X + right.X, left.Y + right.Y);
        }
        public static Vector2 operator *(Vector2 v, int scalar)
        {
            return new Vector2(v.X * scalar, v.Y * scalar);
        }
        public static Vector2 operator /(Vector2 v, int scalar)
        {
            return new Vector2(v.X / scalar, v.Y / scalar);
        }
        public static Vector2F operator *(Vector2 v, float scalar)
        {
            return new Vector2F(v.X * scalar, v.Y * scalar);
        }
        public static Vector2F operator /(Vector2 v, float scalar)
        {
            return new Vector2F(v.X / scalar, v.Y / scalar);
        }
        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !(left == right);
        }
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "({0}, {1})", X, Y);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector2))
                return false;

            return this == (Vector2)obj;
        }
        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public static implicit operator Vector2(Point p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static implicit operator Vector2F(Vector2 v)
        {
            return new Vector2F(v.X, v.Y);
        }
        public static implicit operator Point(Vector2 v)
        {
            return new Point(v.X, v.Y);
        }
        public static implicit operator Size(Vector2 v)
        {
            return new Size(v.X, v.Y);
        }

        public static bool Equals(Vector2 left, Vector2 right)
        {
            return left == right;
        }
        public bool Equals(Vector2 other)
        {
            return this == other;
        }
    }
    
    //     
    /// <summary>
    /// Represents an ordered pair of floating-point x- and y-coordinates that defines a point in a two-dimensional plane.
    /// </summary>
    [Serializable]
    public struct Vector2F : IEquatable<Vector2F>
    {
        /// <summary>
        /// Represents the vector whose components are all zero.
        /// </summary>
        public static readonly Vector2F Zero;

        /// <summary>
        /// The x component of the vector.
        /// </summary>
        public float X;
        /// <summary>
        /// The y component of the vector.
        /// </summary>
        public float Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.Vector2F"/> structure.
        /// </summary>
        /// <param name='x'>The x component of the vector.</param>
        /// <param name='y'>The y component of the vector.</param>
        public Vector2F(float x, float y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.Vector2F"/> structure.
        /// </summary>
        /// <param name='point'>The point whose x- and y-coordinates are used to initialize the vector.</param>
        public Vector2F(PointF point)
        {
            X = point.X;
            Y = point.Y;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.Vector2F"/> structure.
        /// </summary>
        /// <param name='size'>The size whose width and height values are used to initialize the vector.</param>
        public Vector2F(SizeF size)
        {
            X = size.Width;
            Y = size.Height;
        }

        public static Vector2F operator -(Vector2F v)
        {
            return new Vector2F(-v.X, -v.Y);
        }
        public static Vector2F operator -(Vector2F left, Vector2F right)
        {
            return new Vector2F(left.X - right.X, left.Y - right.Y);
        }
        public static Vector2F operator +(Vector2F left, Vector2F right)
        {
            return new Vector2F(left.X + right.X, left.Y + right.Y);
        }
        public static Vector2F operator *(Vector2F v, float scalar)
        {
            return new Vector2F(v.X * scalar, v.Y * scalar);
        }
        public static Vector2F operator /(Vector2F v, float scalar)
        {
            return new Vector2F(v.X / scalar, v.Y / scalar);
        }
        public static bool operator ==(Vector2F left, Vector2F right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        public static bool operator !=(Vector2F left, Vector2F right)
        {
            return !(left == right);
        }
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "({0}, {1})", X, Y);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector2F))
                return false;

            return this == (Vector2F)obj;
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static implicit operator PointF(Vector2F v)
        {
            return new PointF(v.X, v.Y);
        }
        public static implicit operator SizeF(Vector2F v)
        {
            return new SizeF(v.X, v.Y);
        }

        public static bool Equals(Vector2F left, Vector2F right)
        {
            return left == right;
        }
        public bool Equals(Vector2F other)
        {
            return this == other;
        }
    }
}
