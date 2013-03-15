namespace CSDesktopPonies
{
    using System;
    using System.Drawing;

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
        /// Gets or sets the x component of the vector.
        /// </summary>
        public float X { get; set; }
        /// <summary>
        /// Gets or sets the y component of the vector.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Vector2F"/> structure.
        /// </summary>
        /// <param name='x'>The x component of the vector.</param>
        /// <param name='y'>The y component of the vector.</param>
        public Vector2F(float x, float y)
            : this()
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Vector2F"/> structure.
        /// </summary>
        /// <param name='point'>The point whose x- and y-coordinates are used to initialize the vector.</param>
        public Vector2F(PointF point)
            : this()
        {
            X = point.X;
            Y = point.Y;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Vector2F"/> structure.
        /// </summary>
        /// <param name='size'>The size whose width and height values are used to initialize the vector.</param>
        public Vector2F(SizeF size)
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
        public static Vector2F operator -(Vector2F v)
        {
            return new Vector2F(-v.X, -v.Y);
        }
        /// <summary>
        /// Performs vector subtraction.
        /// </summary>
        /// <param name="left">The vector to the left of the subtraction operator.</param>
        /// <param name="right">The vector to the right of the subtraction operator.</param>
        /// <returns>A new vector where both components are calculated by subtracting the values in the second vector from the first.</returns>
        public static Vector2F operator -(Vector2F left, Vector2F right)
        {
            return new Vector2F(left.X - right.X, left.Y - right.Y);
        }
        /// <summary>
        /// Performs vector addition.
        /// </summary>
        /// <param name="left">The vector to the left of the addition operator.</param>
        /// <param name="right">The vector to the right of the addition operator.</param>
        /// <returns>A new vector where both components are calculated by adding the components from the source vectors.</returns>
        public static Vector2F operator +(Vector2F left, Vector2F right)
        {
            return new Vector2F(left.X + right.X, left.Y + right.Y);
        }
        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="v">The vector to multiply.</param>
        /// <param name="scalar">The scale factor to apply.</param>
        /// <returns>A new vector where both components are calculated by multiplying their value by the scale factor.</returns>
        public static Vector2F operator *(Vector2F v, float scalar)
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
        public static Vector2F operator /(Vector2F v, float scalar)
        {
            return new Vector2F(v.X / scalar, v.Y / scalar);
        }
        /// <summary>
        /// Determines whether two vectors are equal.
        /// </summary>
        /// <param name="left">The vector to the left of the equality operator.</param>
        /// <param name="right">The vector to the right of the equality operator.</param>
        /// <returns>Returns true if each component pair is considered equal; otherwise, false.</returns>
        public static bool operator ==(Vector2F left, Vector2F right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        /// <summary>
        /// Determines whether two vectors are unequal.
        /// </summary>
        /// <param name="left">The vector to the left of the inequality operator.</param>
        /// <param name="right">The vector to the right of the inequality operator.</param>
        /// <returns>Returns true any component pair is considered unequal; otherwise, false.</returns>
        public static bool operator !=(Vector2F left, Vector2F right)
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
            if (obj == null || !(obj is Vector2F))
                return false;

            return this == (Vector2F)obj;
        }
        /// <summary>
        /// Returns a hash code for this vector structure.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this vector.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Implicitly converts a point into a vector structure.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>A new vector whose x- and y- coordinates are initialized from the specified point.</returns>
        public static implicit operator Vector2F(PointF p)
        {
            return new Vector2F(p);
        }
        /// <summary>
        /// Implicitly converts a size into a vector structure.
        /// </summary>
        /// <param name="sz">The size to convert.</param>
        /// <returns>A new vector whose x- and y- coordinates are initialized from the specified size.</returns>
        public static implicit operator Vector2F(SizeF sz)
        {
            return new Vector2F(sz);
        }

        /// <summary>
        /// Implicitly converts a vector into a point structure.
        /// </summary>
        /// <param name="v">The vector to convert.</param>
        /// <returns>A new point whose horizontal and vertical components are initialized from the specified vector.</returns>
        public static implicit operator PointF(Vector2F v)
        {
            return new PointF(v.X, v.Y);
        }
        /// <summary>
        /// Implicitly converts a vector into a size structure.
        /// </summary>
        /// <param name="v">The vector to convert.</param>
        /// <returns>A new size whose width and height components are initialized from the specified vector.</returns>
        public static implicit operator SizeF(Vector2F v)
        {
            return new SizeF(v.X, v.Y);
        }

        /// <summary>
        /// Determines whether two vectors are equal.
        /// </summary>
        /// <param name="left">The vector to the left of the equality operator.</param>
        /// <param name="right">The vector to the right of the equality operator.</param>
        /// <returns>Returns true if each component pair is considered equal; otherwise, false.</returns>
        public static bool Equals(Vector2F left, Vector2F right)
        {
            return left == right;
        }
        /// <summary>
        /// Determines whether the specified vector is equal to this vector.
        /// </summary>
        /// <param name="other">The vector to test.</param>
        /// <returns>Returns true if each component pair is considered equal; otherwise, false.</returns>
        public bool Equals(Vector2F other)
        {
            return this == other;
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static float Distance(Vector2F a, Vector2F b)
        {
            return (float)Math.Sqrt(DistanceSquared(a, b));
        }
        /// <summary>
        /// Calculates the square of the distance between two vectors.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>The square of the distance between the two vectors.</returns>
        public static float DistanceSquared(Vector2F a, Vector2F b)
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
    }
}
