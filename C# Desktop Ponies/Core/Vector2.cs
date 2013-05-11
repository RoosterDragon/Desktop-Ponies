namespace CSDesktopPonies.Core
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
        /// Gets or sets the x component of the vector.
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Gets or sets the y component of the vector.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Vector2"/> structure.
        /// </summary>
        /// <param name='x'>The x component of the vector.</param>
        /// <param name='y'>The y component of the vector.</param>
        public Vector2(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Vector2"/> structure.
        /// </summary>
        /// <param name='point'>The point whose x- and y-coordinates are used to initialize the vector.</param>
        public Vector2(Point point)
            : this()
        {
            X = point.X;
            Y = point.Y;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Vector2"/> structure.
        /// </summary>
        /// <param name='size'>The size whose width and height values are used to initialize the vector.</param>
        public Vector2(Size size)
            : this()
        {
            X = size.Width;
            Y = size.Height;
        }

        /// <summary>
        /// Returns the negation of this vector.
        /// </summary>
        /// <param name="v">The vector to negate.</param>
        /// <returns>A new vector whose components have both been negated.</returns>
        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }
        /// <summary>
        /// Performs vector subtraction.
        /// </summary>
        /// <param name="left">The vector to the left of the subtraction operator.</param>
        /// <param name="right">The vector to the right of the subtraction operator.</param>
        /// <returns>A new vector where both components are calculated by subtracting the values in the second vector from the first.</returns>
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X - right.X, left.Y - right.Y);
        }
        /// <summary>
        /// Performs vector addition.
        /// </summary>
        /// <param name="left">The vector to the left of the addition operator.</param>
        /// <param name="right">The vector to the right of the addition operator.</param>
        /// <returns>A new vector where both components are calculated by adding the components from the source vectors.</returns>
        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X + right.X, left.Y + right.Y);
        }
        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="v">The vector to multiply.</param>
        /// <param name="scalar">The scale factor to apply.</param>
        /// <returns>A new vector where both components are calculated by multiplying their value by the scale factor.</returns>
        public static Vector2 operator *(Vector2 v, int scalar)
        {
            return new Vector2(v.X * scalar, v.Y * scalar);
        }
        /// <summary>
        /// Divides a vector by a scalar, using integer division.
        /// </summary>
        /// <param name="v">The vector to divide.</param>
        /// <param name="scalar">The scale factor to apply.</param>
        /// <returns>A new vector where both components are calculated by dividing their value by the scale factor, using integer division.
        /// </returns>
        public static Vector2 operator /(Vector2 v, int scalar)
        {
            return new Vector2(v.X / scalar, v.Y / scalar);
        }
        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="v">The vector to multiply.</param>
        /// <param name="scalar">The scale factor to apply.</param>
        /// <returns>A new vector where both components are calculated by multiplying their value by the scale factor.</returns>
        public static Vector2F operator *(Vector2 v, float scalar)
        {
            return new Vector2F(v.X * scalar, v.Y * scalar);
        }
        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="v">The vector to divide.</param>
        /// <param name="scalar">The scale factor to apply.</param>
        /// <returns>A new vector where both components are calculated by dividing their value by the scale factor.
        /// </returns>
        public static Vector2F operator /(Vector2 v, float scalar)
        {
            return new Vector2F(v.X / scalar, v.Y / scalar);
        }
        /// <summary>
        /// Determines whether two vectors are equal.
        /// </summary>
        /// <param name="left">The vector to the left of the equality operator.</param>
        /// <param name="right">The vector to the right of the equality operator.</param>
        /// <returns>Returns true if each component pair is considered equal; otherwise, false.</returns>
        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        /// <summary>
        /// Determines whether two vectors are unequal.
        /// </summary>
        /// <param name="left">The vector to the left of the inequality operator.</param>
        /// <param name="right">The vector to the right of the inequality operator.</param>
        /// <returns>Returns true any component pair is considered unequal; otherwise, false.</returns>
        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !(left == right);
        }
        /// <summary>
        /// Returns a representation of this vector in Cartesian notation, e.g. "(0, 0)".
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "({0}, {1})", X, Y);
        }
        /// <summary>
        /// Tests whether the specified object is a vector structure and is equal to this vector.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true is <paramref name="obj"/> is a vector structure equivalent to this vector; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector2))
                return false;

            return this == (Vector2)obj;
        }
        /// <summary>
        /// Returns a hash code for this vector structure.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this vector.</returns>
        public override int GetHashCode()
        {
            return X ^ Y;
        }

        /// <summary>
        /// Implicitly converts a point into a vector structure.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>A new vector whose x- and y- coordinates are initialized from the specified point.</returns>
        public static implicit operator Vector2(Point p)
        {
            return new Vector2(p);
        }
        /// <summary>
        /// Implicitly converts a size into a vector structure.
        /// </summary>
        /// <param name="sz">The size to convert.</param>
        /// <returns>A new vector whose x- and y- coordinates are initialized from the specified size.</returns>
        public static implicit operator Vector2(Size sz)
        {
            return new Vector2(sz);
        }

        /// <summary>
        /// Implicitly converts a vector with integer components into a vector with floating point components.
        /// </summary>
        /// <param name="v">The vector to convert.</param>
        /// <returns>A new vector initialized with the same component values as the specified vector.</returns>
        public static implicit operator Vector2F(Vector2 v)
        {
            return new Vector2F(v.X, v.Y);
        }
        /// <summary>
        /// Implicitly converts a vector into a point structure.
        /// </summary>
        /// <param name="v">The vector to convert.</param>
        /// <returns>A new point whose horizontal and vertical components are initialized from the specified vector.</returns>
        public static implicit operator Point(Vector2 v)
        {
            return new Point(v.X, v.Y);
        }
        /// <summary>
        /// Implicitly converts a vector into a size structure.
        /// </summary>
        /// <param name="v">The vector to convert.</param>
        /// <returns>A new size whose width and height components are initialized from the specified vector.</returns>
        public static implicit operator Size(Vector2 v)
        {
            return new Size(v.X, v.Y);
        }

        /// <summary>
        /// Determines whether two vectors are equal.
        /// </summary>
        /// <param name="left">The vector to the left of the equality operator.</param>
        /// <param name="right">The vector to the right of the equality operator.</param>
        /// <returns>Returns true if each component pair is considered equal; otherwise, false.</returns>
        public static bool Equals(Vector2 left, Vector2 right)
        {
            return left == right;
        }
        /// <summary>
        /// Determines whether the specified vector is equal to this vector.
        /// </summary>
        /// <param name="other">The vector to test.</param>
        /// <returns>Returns true if each component pair is considered equal; otherwise, false.</returns>
        public bool Equals(Vector2 other)
        {
            return this == other;
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static float Distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(DistanceSquared(a, b));
        }
        /// <summary>
        /// Calculates the square of the distance between two vectors.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>The square of the distance between the two vectors.</returns>
        public static int DistanceSquared(Vector2 a, Vector2 b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }
        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The distance of this vector from the origin.</returns>
        public float Length()
        {
            return Distance(this, Zero);
        }

        /// <summary>
        /// Converts the specified <see cref="T:CSDesktopPonies.Vector2F"/> structure to a <see cref="T:CSDesktopPonies.Vector2"/>
        /// structure by truncating the values of the <see cref="T:CSDesktopPonies.Vector2F"/> to the next lower integer values.
        /// </summary>
        /// <param name="v">The <see cref="T:CSDesktopPonies.Vector2F"/> structure to convert.</param>
        /// <returns>The <see cref="T:CSDesktopPonies.Vector2"/> structure this method converts to.</returns>
        public static Vector2 Truncate(Vector2F v)
        {
            return new Vector2((int)v.X, (int)v.Y);
        }
        /// <summary>
        /// Converts the specified <see cref="T:CSDesktopPonies.Vector2F"/> structure to a <see cref="T:CSDesktopPonies.Vector2"/>
        /// structure by rounding the values of the <see cref="T:CSDesktopPonies.Vector2F"/> structure to the next higher integer values.
        /// </summary>
        /// <param name="v">The <see cref="T:CSDesktopPonies.Vector2F"/> structure to convert.</param>
        /// <returns>The <see cref="T:CSDesktopPonies.Vector2"/> structure this method converts to.</returns>
        public static Vector2 Ceiling(Vector2F v)
        {
            return new Vector2((int)Math.Ceiling(v.X), (int)Math.Ceiling(v.Y));
        }
    }
}
