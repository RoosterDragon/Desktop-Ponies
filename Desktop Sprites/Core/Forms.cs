namespace DesktopSprites.Core
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Provides utility methods related to <see cref="N:System.Windows.Forms"/>.
    /// </summary>
    public static class Forms
    {
        /// <summary>
        /// Gets the border size for the specified border style.
        /// </summary>
        /// <param name="borderStyle">The border style whose size should be determined.</param>
        /// <returns>The size of the specified border style.</returns>
        public static Size GetBorderSize(BorderStyle borderStyle)
        {
            switch (borderStyle)
            {
                case BorderStyle.Fixed3D:
                    return SystemInformation.Border3DSize;
                case BorderStyle.FixedSingle:
                    return SystemInformation.BorderSize;
                case BorderStyle.None:
                    return Size.Empty;
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException("borderStyle", (int)borderStyle, typeof(BorderStyle));
            }
        }
    }
}
