namespace CSDesktopPonies.SpriteManagement
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines a <see cref="T:CSDesktopPonies.SpriteManagement.SpriteFrame`1"/> whose underlying image is a
    /// <see cref="T:System.Drawing.Bitmap"/>.
    /// </summary>
    public sealed class BitmapFrame : SpriteFrame<Bitmap>, IDisposable
    {
        /// <summary>
        /// Represents the method that converts a buffer into an <see cref="T:CSDesktopPonies.SpriteManagement.BitmapFrame"/>.
        /// </summary>
        public static BufferToImage<BitmapFrame> FromBuffer
        {
            get { return FromBufferInternal; }
        }

        private static readonly BufferToImage<BitmapFrame> FromBufferInternal =
            (byte[] buffer, RgbColor[] palette, int transparentIndex, int stride, int width, int height, int depth, int hashCode) =>
            {
                Bitmap bitmap = GifImage.BufferToImageOfBitmap(buffer, palette, transparentIndex, stride, width, height, depth, hashCode);
                return new BitmapFrame(bitmap, hashCode);
            };

        /// <summary>
        /// Represents the allowable set of depths that can be used when generating a
        /// <see cref="T:CSDesktopPonies.SpriteManagement.BitmapFrame"/>.
        /// </summary>
        public const BitDepths AllowableBitDepths =  GifImage.AllowableDepthsForBitmap;

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
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.BitmapFrame"/> class from the given
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
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.BitmapFrame"/> class from the given file.
        /// </summary>
        /// <param name="fileName">The path to a static image file from which to create a new <see cref="T:CSDesktopPonies.SpriteManagement.BitmapFrame"/>.
        /// </param>
        public BitmapFrame(string fileName)
            : this(new Bitmap(fileName), Argument.EnsureNotNull(fileName, "fileName").GetHashCode())
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
        /// Releases all resources used by the <see cref="T:CSDesktopPonies.SpriteManagement.BitmapFrame"/> object.
        /// </summary>
        public void Dispose()
        {
            Image.Dispose();
        }
    }
}
