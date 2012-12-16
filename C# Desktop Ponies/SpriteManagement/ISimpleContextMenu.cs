namespace CsDesktopPonies.SpriteManagement
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Defines a simple set of methods for a context menu.
    /// </summary>
    public interface ISimpleContextMenu
    {
        /// <summary>
        /// Gets the collection of menu items in this menu.
        /// </summary>
        ReadOnlyCollection<ISimpleContextMenuItem> Items { get; }

        /// <summary>
        /// Displays the context menu at the given co-ordinates.
        /// </summary>
        /// <param name="x">The x co-ordinate of the location where the menu should be shown.</param>
        /// <param name="y">The y co-ordinate of the location where the menu should be shown.</param>
        void Show(int x, int y);
    }
}