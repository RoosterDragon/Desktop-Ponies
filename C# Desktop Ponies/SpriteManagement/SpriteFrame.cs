namespace CSDesktopPonies.SpriteManagement
{
    /// <summary>
    /// Defines an image that can be used as a frame in an animation of sprites.
    /// </summary>
    /// <typeparam name="TImage">The type of the image that displays the frame.</typeparam>
    public abstract class SpriteFrame<TImage> : Frame<TImage>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the frame has been flipped from the direction it was facing when initially loaded.
        /// </summary>
        protected bool Flipped { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.SpriteFrame`1"/> class.
        /// </summary>
        /// <param name="image">The image that displays this frame.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="image"/> is null.</exception>
        protected SpriteFrame(TImage image)
            : base(image)
        {
        }

        /// <summary>
        /// Ensures the image is facing the desired direction by possibly flipping it horizontally.
        /// </summary>
        /// <param name="flipFromOriginal">Pass true to ensure the frame is facing the opposing direction as when it was loaded. Pass false
        /// to ensure the frame is facing the same direction as when it was loaded.</param>
        public abstract void Flip(bool flipFromOriginal);
    }
}