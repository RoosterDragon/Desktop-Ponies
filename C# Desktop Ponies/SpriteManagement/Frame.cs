namespace CSDesktopPonies.SpriteManagement
{
    using System.Drawing;

    /// <summary>
    /// Defines an image that can be used as a frame in an animation.
    /// </summary>
    public abstract class Frame
    {
        /// <summary>
        /// Gets the dimensions of the frame.
        /// </summary>
        public abstract Size Size { get; }
        /// <summary>
        /// Gets the width of the frame.
        /// </summary>
        public int Width
        {
            get { return Size.Width; }
        }
        /// <summary>
        /// Gets the height of the frame.
        /// </summary>
        public int Height
        {
            get { return Size.Height; }
        }

        /// <summary>
        /// Gets the hash code for the frame image.
        /// </summary>
        /// <returns>A hash code for this frame image.</returns>
        public abstract int GetFrameHashCode();
    }

    /// <summary>
    /// Defines an image that can be used as a frame in an animation.
    /// </summary>
    /// <typeparam name="TImage">The type of the image that displays the frame.</typeparam>
    public abstract class Frame<TImage> : Frame
    {
        /// <summary>
        /// Gets the image that represents this frame.
        /// </summary>
        public TImage Image { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.Frame`1"/> class.
        /// </summary>
        /// <param name="image">The image that displays this frame.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="image"/> is null.</exception>
        protected Frame(TImage image)
        {
            if (image == null)
                throw new System.ArgumentNullException("image");
            Image = image;
        }
    }
}
