namespace DesktopSprites.SpriteManagement
{
    /// <summary>
    /// Contains a pair of animations for use when a sprite is facing left and facing right.
    /// </summary>
    /// <typeparam name="T">The type of frames in the animations.</typeparam>
    public struct AnimationPair<T> where T : Frame
    {
        /// <summary>
        /// Gets the animation to use when a sprite is facing left.
        /// </summary>
        public Animation<T> Left { get; private set; }
        /// <summary>
        /// Gets the animation to use when a sprite is facing right.
        /// </summary>
        public Animation<T> Right { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.AnimationPair`1"/>
        /// structure.
        /// </summary>
        /// <param name="left">The animation to use when a sprite is facing left.</param>
        /// <param name="right">The animation to use when a sprite is facing right.</param>
        public AnimationPair(Animation<T> left, Animation<T> right)
            : this()
        {
            Left = left;
            Right = right;
        }
    }
}
