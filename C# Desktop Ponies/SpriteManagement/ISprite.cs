namespace CSDesktopPonies.SpriteManagement
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Defines a self-managing sprite which changes position, size and image as it is updated in time.
    /// </summary>
    public interface ISprite
    {
        /// <summary>
        /// Gets the path to the image file that should be used to display the sprite.
        /// </summary>
        string ImagePath { get; }
        /// <summary>
        /// Gets a value indicating whether the image should be flipped horizontally from its original orientation.
        /// </summary>
        bool FlipImage { get; }
        /// <summary>
        /// Gets the region the sprite currently occupies.
        /// </summary>
        Rectangle Region { get; }
        /// <summary>
        /// Gets the instant in time that represents the current state of the sprite, usually a time index in animations.
        /// </summary>
        TimeSpan CurrentTime { get; }
        /// <summary>
        /// Starts the sprite using the given time as a zero point.
        /// </summary>
        /// <param name="startTime">The time that will be used as a zero point against the time given in future updates.</param>
        void Start(TimeSpan startTime);
        /// <summary>
        /// Updates the sprite to the given instant in time.
        /// </summary>
        /// <param name="updateTime">The instant in time which the sprite should update itself to.</param>
        void Update(TimeSpan updateTime);
    }

    /// <summary>
    /// Defines a sprite which also has the ability to speak.
    /// </summary>
    public interface ISpeakingSprite : ISprite
    {
        /// <summary>
        /// Gets a value indicating whether the sprite is currently speaking (and thus if SpeechText is meaningful).
        /// </summary>
        bool IsSpeaking { get; }
        /// <summary>
        /// Gets the current speech text that is being spoken by the sprite.
        /// </summary>
        string SpeechText { get; }
    }
}
