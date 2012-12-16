namespace CsDesktopPonies.SpriteManagement
{
    using System;

    /// <summary>
    /// Specifies a set of bit depths for a indexed bitmap.
    /// </summary>
    [Flags]
    public enum BitDepths
    {
        /// <summary>
        /// Indicates 1 bit per pixel, allowing 2 colors.
        /// </summary>
        Indexed1Bpp = 1,
        /// <summary>
        /// Indicates 2 bits per pixel, allowing 4 colors.
        /// </summary>
        Indexed2Bpp = 2,
        /// <summary>
        /// Indicates 4 bits per pixel, allowing 16 colors.
        /// </summary>
        Indexed4Bpp = 4,
        /// <summary>
        /// Indicates 8 bits per pixel, allowing 256 colors.
        /// </summary>
        Indexed8Bpp = 8
    }
}
