namespace CsDesktopPonies
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    /// <summary>
    /// Defines RemapColors and PreMultiplyAlpha extension methods for <see cref="T:System.Drawing.Bitmap"/>.
    /// </summary>
    public static class BitmapColors
    {
        /// <summary>
        /// Maps colors in the bitmap to new colors according to the giving mapping.
        /// </summary>
        /// <param name="bitmap">The bitmap whose colors should be remapped.</param>
        /// <param name="map">A mapping of source to destination colors. Colors not in this mapping are not changed.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="bitmap"/> is null.-or-<paramref name="map"/> is null.
        /// </exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void RemapColors(this Bitmap bitmap, IDictionary<Color, Color> map)
        {
            Argument.EnsureNotNull(bitmap, "bitmap");
            Argument.EnsureNotNull(map, "map");

            if (map.Count == 0)
                return;

            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                // We need to replace the actual pixels in the bitmap.
                BitmapData data =
                    bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                byte[] colors = new byte[data.Stride];
                for (int row = 0; row < data.Height; row++)
                {
                    IntPtr rowPtr = IntPtr.Add(data.Scan0, row * data.Stride);

                    // Copy the data to a managed array.
                    Marshal.Copy(rowPtr, colors, 0, data.Width * 4);

                    // Check each pixel, and map those that match to the destination color.
                    for (int x = 0; x < data.Width; x++)
                    {
                        int offset = x * 4;
                        Color mapSource = Color.FromArgb(BitConverter.ToInt32(colors, offset));
                        Color mapDestination;
                        if (map.TryGetValue(mapSource, out mapDestination))
                        {
                            colors[offset + 0] = mapDestination.B;
                            colors[offset + 1] = mapDestination.G;
                            colors[offset + 2] = mapDestination.R;
                            colors[offset + 3] = mapDestination.A;
                        }
                    }

                    // Copy the array back into the bitmap.
                    Marshal.Copy(colors, 0, rowPtr, data.Width * 4);
                }
                bitmap.UnlockBits(data);
            }
            else
            {
                // We're using a color palette, so we can just remap the colors in that.
                ColorPalette palette = bitmap.Palette;
                for (int paletteIndex = 0; paletteIndex < palette.Entries.Length; paletteIndex++)
                {
                    Color mapDestination;
                    if (map.TryGetValue(palette.Entries[paletteIndex], out mapDestination))
                        palette.Entries[paletteIndex] = mapDestination;
                }
                bitmap.Palette = palette;
            }
        }

        /// <summary>
        /// Pre-multiples the alpha channel with each of the RGB color channels. This is, for each channel the value is multiplied by the
        /// value of the alpha channel, and then divided by 255.
        /// </summary>
        /// <param name="bitmap">The bitmap whose colors should be pre-multiplied with their alpha values.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="bitmap"/> is null.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void PreMultiplyAlpha(this Bitmap bitmap)
        {
            Argument.EnsureNotNull(bitmap, "bitmap");

            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                // We need to replace the actual pixels in the bitmap.
                BitmapData data =
                    bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                byte[] colors = new byte[data.Stride];
                for (int row = 0; row < data.Height; row++)
                {
                    IntPtr rowPtr = IntPtr.Add(data.Scan0, row * data.Stride);

                    // Copy the data to a managed array.
                    Marshal.Copy(rowPtr, colors, 0, data.Width * 4);

                    // Multiply the color channels in each pixel.
                    for (int i = 0; i < colors.Length; i += 4)
                    {
                        int alpha = colors[i + 3];
                        colors[i + 0] = (byte)(colors[i + 0] * alpha / 255);
                        colors[i + 1] = (byte)(colors[i + 1] * alpha / 255);
                        colors[i + 2] = (byte)(colors[i + 2] * alpha / 255);
                    }

                    // Copy the array back into the bitmap.
                    Marshal.Copy(colors, 0, rowPtr, data.Width * 4);
                }
                bitmap.UnlockBits(data);
            }
            else
            {
                // We're using a color palette, so we can just pre-multiply colors in that.
                ColorPalette palette = bitmap.Palette;
                for (int i = 0; i < palette.Entries.Length; i++)
                {
                    Color color = palette.Entries[i];
                    palette.Entries[i] =
                        Color.FromArgb(color.A, color.R * color.A / 255, color.G * color.A / 255, color.B * color.A / 255);
                }
                bitmap.Palette = palette;
            }
        }
    }
}
