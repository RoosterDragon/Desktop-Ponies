namespace DesktopSprites.SpriteManagement
{
    using System;

    /// <summary>
    /// Represents an RGB (red, green, blue) color.
    /// </summary>
    [Serializable]
    public struct RgbColor : IEquatable<RgbColor>
    {
        /// <summary>
        /// Gets the red component value of this <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure.
        /// </summary>
        public byte R { get; private set; }
        /// <summary>
        /// Gets the green component value of this <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure.
        /// </summary>
        public byte G { get; private set; }
        /// <summary>
        /// Gets the blue component value of this <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure.
        /// </summary>
        public byte B { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure from the specified color
        /// values (red, green, and blue).
        /// </summary>
        /// <param name="r">The red component value for the new <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/>.</param>
        /// <param name="g">The green component value for the new <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/>.</param>
        /// <param name="b">The blue component value for the new <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/>.</param>
        public RgbColor(byte r, byte g, byte b)
            : this()
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Implicitly converts a <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> to an
        /// <see cref="T:DesktopSprites.SpriteManagement.ArgbColor"/>.
        /// </summary>
        /// <param name="rgbColor">The <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> to convert.</param>
        /// <returns>Returns a new <see cref="T:DesktopSprites.SpriteManagement.ArgbColor"/> with an alpha value of 255.</returns>
        public static implicit operator ArgbColor(RgbColor rgbColor)
        {
            return new ArgbColor(255, rgbColor);
        }

        /// <summary>
        /// Tests whether two specified <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> that is to the left of the equality operator.
        /// </param>
        /// <param name="right">The <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> that is to the right of the equality
        /// operator.</param>
        /// <returns>Returns true if the two <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structures are equal; otherwise,
        /// false.</returns>
        public static bool operator ==(RgbColor left, RgbColor right)
        {
            return left.R == right.R && left.G == right.G && left.B == right.B;
        }

        /// <summary>
        /// Tests whether two specified <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> that is to the left of the inequality
        /// operator.</param>
        /// <param name="right">The <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> that is to the right of the inequality
        /// operator.</param>
        /// <returns>Returns true if the two <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structures are different; otherwise,
        /// false.</returns>
        public static bool operator !=(RgbColor left, RgbColor right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Tests whether the specified <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> is equivalent to this
        /// <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure.
        /// </summary>
        /// <param name="other">The color to test.</param>
        /// <returns>Returns true if <paramref name="other"/> is equivalent to this
        /// <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure; otherwise, false.</returns>
        public bool Equals(RgbColor other)
        {
            return this == other;
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure and is equivalent
        /// to this <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if <paramref name="obj"/> is a <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure
        /// equivalent to this <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is RgbColor))
                return false;

            return this == (RgbColor)obj;
        }

        /// <summary>
        /// Gets the 32-bit ARGB value of this <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure. The alpha component is
        /// implicitly opaque (255).
        /// </summary>
        /// <returns>The 32-bit ARGB value of this <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/>.</returns>
        public int ToArgb()
        {
            return (255 << 24) | (R << 16) | (G << 8) | B;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return ToArgb();
        }

        /// <summary>
        /// Converts this <see cref="T:DesktopSprites.SpriteManagement.RgbColor"/> structure to a human-readable string.
        /// </summary>
        /// <returns>A string that consists of the RGB component names and their values.</returns>
        public override string ToString()
        {
            return GetType().Name + " [R=" + R + ", G=" + G + ", B=" + B + "]";
        }
    }
}
