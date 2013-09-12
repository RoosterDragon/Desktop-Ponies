namespace DesktopSprites.Collections
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="E:DesktopSprites.Collections.LazyDictionary`2.ItemInitialized"/> event.
    /// </summary>
    public class ItemInitializedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the number of items initialized.
        /// </summary>
        public int Initialized { get; private set; }
        /// <summary>
        /// Gets the number of items that remain to be initialized.
        /// </summary>
        public int Uninitialized { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.Collections.ItemInitializedEventArgs"/> class.
        /// </summary>
        /// <param name="initialized">The number of items initialized.</param>
        /// <param name="uninitialized">The number of items that remain to be initialized.</param>
        public ItemInitializedEventArgs(int initialized, int uninitialized)
        {
            Initialized = initialized;
            Uninitialized = uninitialized;
        }
    }
}