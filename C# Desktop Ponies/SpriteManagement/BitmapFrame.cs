namespace CsDesktopPonies.SpriteManagement
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines a <see cref="T:CsDesktopPonies.SpriteManagement.SpriteFrame`1"/> whose underlying image is a
    /// <see cref="T:System.Drawing.Bitmap"/>.
    /// </summary>
    public sealed class BitmapFrame : SpriteFrame<Bitmap>, IDisposable
    {
        /// <summary>
        /// Gets the method for creating a <see cref="T:CsDesktopPonies.SpriteManagement.BitmapFrame"/> from a buffer.
        /// </summary>
        public static BufferToImage<BitmapFrame> FromBuffer
        {
            get { return FromBufferMethod; }
        }

        /// <summary>
        /// Creates a new <see cref="T:CsDesktopPonies.SpriteManagement.BitmapFrame"/> from the raw buffer.
        /// </summary>
        /// <param name="buffer">The raw buffer.</param>
        /// <param name="palette">The color palette.</param>
        /// <param name="transparentIndex">The index of the transparent color.</param>
        /// <param name="stride">The stride width of the buffer.</param>
        /// <param name="width">The logical width of the buffer.</param>
        /// <param name="height">The logical height of the buffer.</param>
        /// <param name="depth">The bit depth of the buffer.</param>
        /// <param name="hashCode">The hash code of the frame.</param>
        /// <returns>A new <see cref="T:CsDesktopPonies.SpriteManagement.BitmapFrame"/> for the frame held in the raw buffer.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="depth"/> is not 1, 4 or 8.</exception>
        private static BitmapFrame FromBufferMethod(
            byte[] buffer, RgbColor[] palette, int transparentIndex, int stride, int width, int height, int depth, int hashCode)
        {
            PixelFormat targetFormat;
            if (depth == 1)
                targetFormat = PixelFormat.Format1bppIndexed;
            else if (depth == 4)
                targetFormat = PixelFormat.Format4bppIndexed;
            else if (depth == 8)
                targetFormat = PixelFormat.Format8bppIndexed;
            else
                throw new ArgumentOutOfRangeException("depth", depth, "depth must be 1, 4 or 8.");

            // Create the bitmap and lock it.
            Bitmap bitmap = new Bitmap(width, height, targetFormat);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            // Copy the frame buffer to the bitmap. To account for stride padding, copy row by row. Then unlock it.
            for (int row = 0; row < data.Height; row++)
                Marshal.Copy(
                    buffer,
                    row * stride,
                    IntPtr.Add(data.Scan0, row * data.Stride),
                    stride);
            bitmap.UnlockBits(data);

            // Fill in the color palette from the current table.
            ColorPalette bitmapPalette = bitmap.Palette;
            for (int i = 0; i < palette.Length; i++)
                bitmapPalette.Entries[i] = Color.FromArgb(palette[i].R, palette[i].G, palette[i].B);

            // Apply transparency.
            if (transparentIndex != -1)
                bitmapPalette.Entries[transparentIndex] = Color.Transparent;

            // Set palette on bitmap.
            bitmap.Palette = bitmapPalette;

            return new BitmapFrame(bitmap, hashCode);
        }

        /// <summary>
        /// Gets the set of allowable bit depths for a <see cref="T:CsDesktopPonies.SpriteManagement.BitmapFrame"/>.
        /// </summary>
        public static BitDepths AllowableBitDepths
        {
            get { return BitDepths.Indexed1Bpp | BitDepths.Indexed4Bpp | BitDepths.Indexed8Bpp; }
        }

        /// <summary>
        /// The hash code of the frame image.
        /// </summary>
        private int hashCode;

        /// <summary>
        /// Gets the dimensions of the frame.
        /// </summary>
        public override Size Size
        {
            get { return Image.Size; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.SpriteManagement.BitmapFrame"/> class from the given
        /// <see cref="T:System.Drawing.Bitmap"/>.
        /// </summary>
        /// <param name="bitmap">The <see cref="T:System.Drawing.Bitmap"/> to use in the frame.</param>
        /// <param name="hash">The hash code of the frame.</param>
        public BitmapFrame(Bitmap bitmap, int hash)
            : base(bitmap)
        {
            hashCode = hash;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.SpriteManagement.BitmapFrame"/> class from the given file.
        /// </summary>
        /// <param name="fileName">The path to a static image file from which to create a new <see cref="T:CsDesktopPonies.SpriteManagement.BitmapFrame"/>.
        /// </param>
        public BitmapFrame(string fileName)
            : this(new Bitmap(fileName), fileName.GetHashCode())
        {
        }

        /// <summary>
        /// Ensures the frame is facing the desired direction by possibly flipping it horizontally.
        /// </summary>
        /// <param name="flipFromOriginal">Pass true to ensure the frame is facing the opposing direction as when it was loaded. Pass false
        /// to ensure the frame is facing the same direction as when it was loaded.</param>
        public override void Flip(bool flipFromOriginal)
        {
            if (Flipped != flipFromOriginal)
            {
                Flipped = !Flipped;
                Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
        }

        /// <summary>
        /// Gets the hash code of the frame image.
        /// </summary>
        /// <returns>A hash code for this frame image.</returns>
        public override int GetFrameHashCode()
        {
            return hashCode;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="T:CsDesktopPonies.SpriteManagement.BitmapFrame"/> object.
        /// </summary>
        public void Dispose()
        {
            Image.Dispose();
        }
    }
}
