namespace DesktopSprites.SpriteManagement
{
    using System;
    using DesktopSprites.Core;

    /// <summary>
    /// Identifies image paths to be used when a sprite is facing to the left and to the right.
    /// </summary>
    public struct SpriteImagePaths : IEquatable<SpriteImagePaths>
    {
        /// <summary>
        /// Gets the path to the image to be used when facing left.
        /// </summary>
        public string Left { get; private set; }
        /// <summary>
        /// Gets the path to the image to be used when facing right.
        /// </summary>
        public string Right { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.SpriteImagePaths"/> structure.
        /// </summary>
        /// <param name="left">The path to the image to be used when facing left.</param>
        /// <param name="right">The path to the image to be used when facing right.</param>
        public SpriteImagePaths(string left, string right)
            : this()
        {
            Left = left;
            Right = right;
        }
        /// <summary>
        /// Determines whether the specified pair of paths is equal to this pair of paths.
        /// </summary>
        /// <param name="other">The pair of paths to test.</param>
        /// <returns>Returns true if each pair of paths is considered equal; otherwise, false.</returns>
        public bool Equals(SpriteImagePaths other)
        {
            return this == other;
        }
        /// <summary>
        /// Tests whether the specified object is a paths structure and is equal to this pair of paths.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true is <paramref name="obj"/> is a paths structure equivalent to this pair of paths; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SpriteImagePaths))
                return false;

            return this == (SpriteImagePaths)obj;
        }
        /// <summary>
        /// Determines whether two pairs of paths are equal.
        /// </summary>
        /// <param name="left">The pair of paths to the left of the equality operator.</param>
        /// <param name="right">The pair of paths to the right of the equality operator.</param>
        /// <returns>Returns true if each pair of paths is considered equal; otherwise, false.</returns>
        public static bool operator ==(SpriteImagePaths left, SpriteImagePaths right)
        {
            return Comparer.Equals(left.Left, right.Left) && Comparer.Equals(left.Right, right.Right);
        }
        /// <summary>
        /// Determines whether two pairs of paths are unequal.
        /// </summary>
        /// <param name="left">The pair of paths to the left of the inequality operator.</param>
        /// <param name="right">The pair of paths to the right of the inequality operator.</param>
        /// <returns>Returns true any pair of paths is considered unequal; otherwise, false.</returns>
        public static bool operator !=(SpriteImagePaths left, SpriteImagePaths right)
        {
            return !(left == right);
        }
        /// <summary>
        /// Gets a <see cref="T:System.StringComparer"/> that is used to compare each path for equality.
        /// </summary>
        public static StringComparer Comparer
        {
            get { return PathEquality.Comparer; }
        }
        /// <summary>
        /// Returns a hash code for this paths structure.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this pairs of paths.</returns>
        public override int GetHashCode()
        {
            return Comparer.GetHashCode(Left) ^ Comparer.GetHashCode(Right);
        }
        /// <summary>
        /// Converts this <see cref="T:DesktopSprites.SpriteManagement.SpriteImagePaths"/> structure to a string.
        /// </summary>
        /// <returns>A string that consists of the left and right values.</returns>
        public override string ToString()
        {
            return GetType().Name + " [Left=" + Left + ", Right=" + Right + "]";
        }
    }
}
