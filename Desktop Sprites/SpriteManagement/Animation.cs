namespace DesktopSprites.SpriteManagement
{
    /// <summary>
    /// Defines an image to be drawn and whether it should be horizontally mirrored.
    /// </summary>
    /// <typeparam name="T">The type of frames in the animated image.</typeparam>
    public struct Animation<T> where T : Frame
    {
        /// <summary>
        /// Gets the animated image to draw.
        /// </summary>
        public AnimatedImage<T> Image { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the image should be horizontally mirrored when drawing.
        /// </summary>
        public bool Mirror { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.Animation`1"/>
        /// structure.
        /// </summary>
        /// <param name="image">The animated image to draw.</param>
        public Animation(AnimatedImage<T> image)
            : this(image, false)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.Animation`1"/>
        /// structure.
        /// </summary>
        /// <param name="image">The animated image to draw.</param>
        /// <param name="mirror">Indicates whether the image should be horizontally mirrored when drawing.</param>
        public Animation(AnimatedImage<T> image, bool mirror)
            : this()
        {
            Image = image;
            Mirror = mirror;
        }
    }
}
