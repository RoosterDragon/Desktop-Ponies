namespace DesktopSprites.Core
{
    using System;

    /// <summary>
    /// Exposes properties of the runtime environment.
    /// </summary>
    public static class Runtime
    {
        /// <summary>
        /// Gets a value indicating whether the mono runtime is being used.
        /// </summary>
        public static bool IsMono { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="T:DesktopSprites.Core.Runtime"/> class.
        /// </summary>
        static Runtime()
        {
            IsMono = Type.GetType("Mono.Runtime") != null;
        }
    }
}
