namespace DesktopSprites.Core
{
    using System.Drawing;

    /// <summary>
    /// Defines extension methods for <see cref="T:System.Drawing.Color"/>.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Returns a new <see cref="T:System.Drawing.Color"/> structure where the alpha component has been premultiplied with each of the
        /// RGB color components. This is, the value of each color component is multiplied by the value of the alpha component, and then
        /// divided by 255.
        /// </summary>
        /// <param name="color">A color to premultiply.</param>
        /// <returns>A new <see cref="T:System.Drawing.Color"/> structure with premultiplied components.</returns>
        public static Color PremultipliedAlpha(this Color color)
        {
            return Color.FromArgb(color.A, color.R * color.A / 255, color.G * color.A / 255, color.B * color.A / 255);
        }
    }
}
