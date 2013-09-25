namespace DesktopSprites.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using DesktopSprites.Collections;

    /// <summary>
    /// Defines a visual interface capable of displaying a collection of objects which implement
    /// <see cref="T:DesktopSprites.SpriteManagement.ISprite"/>.
    /// </summary>
    public interface ISpriteCollectionView : IDisposable
    {
        /// <summary>
        /// Loads the given collection of file paths as images in a format that this interface can display.
        /// </summary>
        /// <param name="imageFilePaths">The collection of paths to image files that should be loaded by the interface. Any images not
        /// loaded by this method will be loaded on demand.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="imageFilePaths"/> is null.</exception>
        void LoadImages(IEnumerable<string> imageFilePaths);
        /// <summary>
        /// Loads the given collection of file paths as images in a format that this interface can display.
        /// </summary>
        /// <param name="imageFilePaths">The collection of paths to image files that should be loaded by the interface. Any images not
        /// loaded by this method will be loaded on demand.</param>
        /// <param name="imageLoadedHandler">An <see cref="T:System.EventHandler"/> that is raised when an image is loaded.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="imageFilePaths"/> is null.</exception>
        void LoadImages(IEnumerable<string> imageFilePaths, EventHandler imageLoadedHandler);
        /// <summary>
        /// Creates an interface specific context menu for the given set of menu items.
        /// </summary>
        /// <param name="menuItems">The collections of items to be displayed in the menu. If you need to edit these items after creation,
        /// use the <see cref="P:ISimpleContextMenu.Items"/> property to effect changes.</param>
        /// <returns>An interface specific context menu.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="menuItems"/> is null.</exception>
        ISimpleContextMenu CreateContextMenu(IEnumerable<ISimpleContextMenuItem> menuItems);

        /// <summary>
        /// Opens the interface.
        /// </summary>
        void Open();
        /// <summary>
        /// Hides the interface.
        /// </summary>
        void Hide();
        /// <summary>
        /// Shows the interface.
        /// </summary>
        void Show();
        /// <summary>
        /// Freezes the display of the interface.
        /// </summary>
        void Pause();
        /// <summary>
        /// Resumes display of the interface from a paused state.
        /// </summary>
        void Resume();
        /// <summary>
        /// Draws the given collection of sprites.
        /// </summary>
        /// <param name="sprites">The collection of sprites to draw.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sprites"/> is null.</exception>
        void Draw(ReadOnlyCollection<ISprite> sprites);
        /// <summary>
        /// Closes the interface.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets or sets the text to use in the title frame of any interface windows.
        /// </summary>
        string WindowTitle { get; set; }
        /// <summary>
        /// Gets or sets the file path which points to a file that should be used as the icon image of any interface windows.
        /// </summary>
        string WindowIconFilePath { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the interface will keep above other windows.
        /// </summary>
        bool Topmost { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether a window should appear in the taskbar.
        /// </summary>
        bool ShowInTaskbar { get; set; }
        /// <summary>
        /// Gets a value indicating whether alpha blending is in use. If true, pixels which are partially transparent will be blended with
        /// those behind them to achieve proper transparency; otherwise these pixels will be rendered opaque, and only fully transparent
        /// pixels will render as transparent, resulting in simple 1-bit transparency.
        /// </summary>
        bool IsAlphaBlended { get; }
        /// <summary>
        /// Gets the location of the cursor.
        /// </summary>
        Point CursorPosition { get; }

        /// <summary>
        /// Occurs when a key is pressed while the interface has focus.
        /// </summary>
        event EventHandler<SimpleKeyEventArgs> KeyPress;
        /// <summary>
        /// Occurs when the mouse pointer is over the interface and a mouse button is pressed.
        /// </summary>
        event EventHandler<SimpleMouseEventArgs> MouseDown;
        /// <summary>
        /// Occurs when the interface is clicked by the mouse.
        /// </summary>
        event EventHandler<SimpleMouseEventArgs> MouseClick;
        /// <summary>
        /// Occurs when the mouse pointer is over the interface and a mouse button is released.
        /// </summary>
        event EventHandler<SimpleMouseEventArgs> MouseUp;
        /// <summary>
        /// Occurs when the interface is closed, either via the
        /// <see cref="M:DesktopSprites.SpriteManagement.ISpriteCollectionView.Close"/> method or by other means such as user request.
        /// </summary>
        event EventHandler InterfaceClosed;
    }

    /// <summary>
    /// Provides data for the KeyPress event.
    /// </summary>
    public class SimpleKeyEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the character of the key that was pressed.
        /// </summary>
        public char KeyChar { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.SimpleKeyEventArgs"/> class.
        /// </summary>
        /// <param name="keyChar">The character of the key that was pressed.</param>
        public SimpleKeyEventArgs(char keyChar)
        {
            KeyChar = keyChar;
        }

        /// <summary>
        /// Returns a string instance that represents the <see cref="T:DesktopSprites.SpriteManagement.SimpleKeyEventArgs"/>.
        /// </summary>
        /// <returns>A string instance that represents the <see cref="T:DesktopSprites.SpriteManagement.SimpleKeyEventArgs"/>.</returns>
        public override string ToString()
        {
            return "{KeyChar=" + KeyChar + "}";
        }
    }

    /// <summary>
    /// Provides data for the MouseDown, MouseClick and MouseUp events.
    /// </summary>
    public class SimpleMouseEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the buttons that were pressed.
        /// </summary>
        public SimpleMouseButtons Buttons { get; private set; }
        /// <summary>
        /// Gets the x co-ordinate of the point that was clicked, in relation to the screen.
        /// </summary>
        public int X { get; private set; }
        /// <summary>
        /// Gets the y co-ordinate of the point that was clicked, in relation to the screen.
        /// </summary>
        public int Y { get; private set; }
        /// <summary>
        /// Gets the co-ordinates of the point that was clicked, in relation to the screen.
        /// </summary>
        public Point Location
        {
            get { return new Point(X, Y); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.SimpleMouseEventArgs"/> class.
        /// </summary>
        /// <param name="buttons">The buttons that were pressed.</param>
        /// <param name="x">The x co-ordinate of the point that was clicked, in relation to the screen.</param>
        /// <param name="y">The y co-ordinate of the point that was clicked, in relation to the screen.</param>
        public SimpleMouseEventArgs(SimpleMouseButtons buttons, int x, int y)
        {
            Buttons = buttons;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns a string instance that represents the <see cref="T:DesktopSprites.SpriteManagement.SimpleMouseEventArgs"/>.
        /// </summary>
        /// <returns>A string instance that represents the <see cref="T:DesktopSprites.SpriteManagement.SimpleMouseEventArgs"/>.</returns>
        public override string ToString()
        {
            return "{X=" + X + ",Y=" + Y + ",Buttons=" + Buttons.ToString() + "}";
        }
    }

    /// <summary>
    /// Specifies a basic set of mouse buttons.
    /// </summary>
    [Flags]
    public enum SimpleMouseButtons
    {
        /// <summary>
        /// Specifies no buttons.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies the left mouse button.
        /// </summary>
        Left = 1,
        /// <summary>
        /// Specifies the middle mouse button.
        /// </summary>
        Middle = 2,
        /// <summary>
        /// Specifies the right mouse button.
        /// </summary>
        Right = 4,
    }
}