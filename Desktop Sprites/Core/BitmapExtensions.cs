namespace DesktopSprites.Core
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    /// <summary>
    /// Defines extension methods for <see cref="T:System.Drawing.Bitmap"/>.
    /// </summary>
    public static class BitmapExtensions
    {
        /// <summary>
        /// Maps colors in the bitmap to new colors according to the giving mapping.
        /// </summary>
        /// <param name="bitmap">The bitmap whose colors should be remapped.</param>
        /// <param name="map">A mapping of source to destination colors. Colors not in this mapping are not changed.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="bitmap"/> is null.-or-<paramref name="map"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">The pixel format of <paramref name="bitmap"/> means colors cannot be remapped.
        /// </exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void RemapColors(this Bitmap bitmap, IDictionary<Color, Color> map)
        {
            Argument.EnsureNotNull(bitmap, "bitmap");
            Argument.EnsureNotNull(map, "map");

            if (map.Count == 0)
                return;

            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                    // We need to replace the actual pixels in the bitmap.
                    BitmapData data =
                        bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                    int[] colors = new int[data.Stride / 4];
                    for (int row = 0; row < data.Height; row++)
                    {
                        IntPtr rowPtr = IntPtr.Add(data.Scan0, row * data.Stride / 4);

                        // Copy the data to a managed array.
                        Marshal.Copy(rowPtr, colors, 0, data.Width);

                        // Check each pixel, and map those that match to the destination color.
                        for (int i = 0; i < data.Width; i++)
                        {
                            Color mapSource = Color.FromArgb(colors[i]);
                            Color mapDestination;
                            if (map.TryGetValue(mapSource, out mapDestination))
                                colors[i] = mapDestination.ToArgb();
                        }

                        // Copy the array back into the bitmap.
                        Marshal.Copy(colors, 0, rowPtr, data.Width);
                    }
                    bitmap.UnlockBits(data);
                    break;
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    // We're using a color palette, so we can just remap the colors in that.
                    ColorPalette palette = bitmap.Palette;
                    for (int paletteIndex = 0; paletteIndex < palette.Entries.Length; paletteIndex++)
                    {
                        Color mapDestination;
                        if (map.TryGetValue(palette.Entries[paletteIndex], out mapDestination))
                            palette.Entries[paletteIndex] = mapDestination;
                    }
                    bitmap.Palette = palette;
                    break;
                default:
                    throw new ArgumentException("Remapping colors of a bitmap with this pixel format is not supported.", "bitmap");
            }
        }

        /// <summary>
        /// Premultiplies the alpha channel with each of the RGB color channels. This is, for each channel the value is multiplied by the
        /// value of the alpha channel, and then divided by 255.
        /// </summary>
        /// <param name="bitmap">The bitmap whose colors should be pre-multiplied with their alpha values.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="bitmap"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">The pixel format of <paramref name="bitmap"/> means it cannot be alpha blended.
        /// </exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void PremultiplyAlpha(this Bitmap bitmap)
        {
            Argument.EnsureNotNull(bitmap, "bitmap");

            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format64bppPArgb:
                case PixelFormat.PAlpha:
                    // We already have premultiplied alpha.
                    break;
                case PixelFormat.Format32bppArgb:
                    // We need to replace the actual pixels in the bitmap.
                    BitmapData data =
                        bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                    int[] colors = new int[data.Stride / 4];
                    for (int row = 0; row < data.Height; row++)
                    {
                        IntPtr rowPtr = IntPtr.Add(data.Scan0, row * data.Stride);

                        // Copy the data to a managed array.
                        Marshal.Copy(rowPtr, colors, 0, data.Width * 4);

                        // Multiply the color channels in each pixel.
                        for (int i = 0; i < colors.Length; i++)
                            colors[i] = Color.FromArgb(colors[i]).PremultipliedAlpha().ToArgb();

                        // Copy the array back into the bitmap.
                        Marshal.Copy(colors, 0, rowPtr, data.Width * 4);
                    }
                    data.PixelFormat = PixelFormat.Format32bppPArgb;
                    bitmap.UnlockBits(data);
                    break;
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    // We're using a color palette, so we can just pre-multiply colors in that.
                    ColorPalette palette = bitmap.Palette;
                    for (int i = 0; i < palette.Entries.Length; i++)
                        palette.Entries[i] = palette.Entries[i].PremultipliedAlpha();
                    bitmap.Palette = palette;
                    break;
                default:
                    throw new ArgumentException("Alpha blending a bitmap with this pixel format is not supported.", "bitmap");
            }
        }
    }
}
