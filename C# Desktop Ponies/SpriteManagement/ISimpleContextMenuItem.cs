namespace CsDesktopPonies.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Defines a simple set of methods for a context menu item.
    /// </summary>
    public interface ISimpleContextMenuItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether the item is a separator.
        /// </summary>
        bool IsSeparator { get; set; }
        /// <summary>
        /// Gets or sets the text displayed to represent this item.
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// Gets or sets the method that runs when the item is activated by the user.
        /// </summary>
        EventHandler Activated { get; set; }
        /// <summary>
        /// Gets the sub-items in an item that displays a new sub-menu of items.
        /// </summary>
        ReadOnlyCollection<ISimpleContextMenuItem> SubItems { get; }
    }

    /// <summary>
    /// Provides a minimal implementation of the <see cref="T:CsDesktopPonies.SpriteManagement.ISimpleContextMenuItem"/> interface that
    /// does not have an underlying UI.
    /// </summary>
    public class SimpleContextMenuItem : ISimpleContextMenuItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether the item is a separator.
        /// </summary>
        public bool IsSeparator { get; set; }
        /// <summary>
        /// Gets or sets the text displayed to represent this item.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the method that runs when the item is activated by the user.
        /// </summary>
        public EventHandler Activated { get; set; }
        /// <summary>
        /// Gets the sub-items in an item that displays a new sub-menu of items.
        /// </summary>
        public ReadOnlyCollection<ISimpleContextMenuItem> SubItems { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.SpriteManagement.SimpleContextMenuItem"/> class that represents
        /// a separator.
        /// </summary>
        public SimpleContextMenuItem()
        {
            IsSeparator = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.SpriteManagement.SimpleContextMenuItem"/> class that represents
        /// an activatable item.
        /// </summary>
        /// <param name="text">The text to be displayed that represents this item.</param>
        /// <param name="activated">The method that will run when the item is activated by the user.</param>
        public SimpleContextMenuItem(string text, EventHandler activated)
        {
            Text = text;
            Activated += activated;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.SpriteManagement.SimpleContextMenuItem"/> class that represents
        /// an item with a sub-menu of items.
        /// </summary>
        /// <param name="text">The text to be displayed that represents this item.</param>
        /// <param name="subItems">The collection of items that appears in the sub-menu when the item is activated by the user.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="subItems"/> is null.</exception>
        public SimpleContextMenuItem(string text, IEnumerable<ISimpleContextMenuItem> subItems)
        {
            Text = text;
            SubItems = new ReadOnlyCollection<ISimpleContextMenuItem>(new List<ISimpleContextMenuItem>(subItems));
        }
    }
}