namespace CsDesktopPonies.SpriteManagement
{
    using System;

    /// <summary>
    /// Represents an ARGB (alpha, red, green, blue) color.
    /// </summary>
    [Serializable]
    public struct ArgbColor
    {
        /// <summary>
        /// Gets the alpha component value of this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure.
        /// </summary>
        public byte A { get; private set; }
        /// <summary>
        /// Gets the red component value of this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure.
        /// </summary>
        public byte R { get; private set; }
        /// <summary>
        /// Gets the green component value of this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure.
        /// </summary>
        public byte G { get; private set; }
        /// <summary>
        /// Gets the blue component value of this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure.
        /// </summary>
        public byte B { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure from the specified
        /// alpha, and with the same red, green and blue values as the given <see cref="T:CsDesktopPonies.SpriteManagement.RgbColor"/>.
        /// </summary>
        /// <param name="a">The alpha component value for the new <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/>.</param>
        /// <param name="color">An <see cref="T:CsDesktopPonies.SpriteManagement.RgbColor"/> which specifies the red, green and blue components for the new
        /// <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/>.</param>
        public ArgbColor(byte a, RgbColor color)
            : this(a, color.R, color.G, color.B)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure from the specified color
        /// values (alpha, red, green, and blue).
        /// </summary>
        /// <param name="a">The alpha component value for the new <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/>.</param>
        /// <param name="r">The red component value for the new <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/>.</param>
        /// <param name="g">The green component value for the new <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/>.</param>
        /// <param name="b">The blue component value for the new <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/>.</param>
        public ArgbColor(byte a, byte r, byte g, byte b)
            : this()
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Explicitly converts an <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> to a
        /// <see cref="T:CsDesktopPonies.SpriteManagement.RgbColor"/>.
        /// </summary>
        /// <param name="argbColor">The <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> to convert.</param>
        /// <returns>Returns a new <see cref="T:CsDesktopPonies.SpriteManagement.RgbColor"/> with no alpha value.</returns>
        public static explicit operator RgbColor(ArgbColor argbColor)
        {
            return new RgbColor(argbColor.R, argbColor.G, argbColor.B);
        }

        /// <summary>
        /// Tests whether two specified <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> that is to the left of the equality
        /// operator.</param>
        /// <param name="right">The <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> that is to the right of the equality
        /// operator.</param>
        /// <returns>Returns true if the two <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structures are equal; otherwise,
        /// false.</returns>
        public static bool operator ==(ArgbColor left, ArgbColor right)
        {
            return left.R == right.R && left.G == right.G && left.B == right.B && left.A == right.A;
        }

        /// <summary>
        /// Tests whether two specified <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> that is to the left of the inequality
        /// operator.</param>
        /// <param name="right">The <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> that is to the right of the inequality
        /// operator.</param>
        /// <returns>Returns true if the two <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structures are different;
        /// otherwise, false.</returns>
        public static bool operator !=(ArgbColor left, ArgbColor right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure and is equivalent
        /// to this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if <paramref name="obj"/> is a <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure
        /// equivalent to this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ArgbColor))
                return false;

            return this == (ArgbColor)obj;
        }

        /// <summary>
        /// Gets the 32-bit ARGB value of this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure..
        /// </summary>
        /// <returns>The 32-bit ARGB value of this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/>.</returns>
        public int ToArgb()
        {
            return (A << 24) | (R << 16) | (G << 8) | B;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return ToArgb();
        }

        /// <summary>
        /// Converts this <see cref="T:CsDesktopPonies.SpriteManagement.ArgbColor"/> structure to a human-readable string.
        /// </summary>
        /// <returns>A string that consists of the ARGB component names and their values.</returns>
        public override string ToString()
        {
            return GetType().Name + " [A=" + A + ", R=" + R + ", G=" + G + ", B=" + B + "]";
        }
    }
}