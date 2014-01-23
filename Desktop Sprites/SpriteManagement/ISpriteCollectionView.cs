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
        void Draw(ICollection<ISprite> sprites);
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
        /// Gets the current location of the cursor.
        /// </summary>
        Point CursorPosition { get; }
        /// <summary>
        /// Gets the mouse buttons which are currently held down.
        /// </summary>
        SimpleMouseButtons MouseButtonsDown { get; }
        /// <summary>
        /// Gets a value indicating whether the interface has input focus.
        /// </summary>
        bool HasFocus { get; }
        /// <summary>
        /// Gets or sets an optional function that pre-processes a decoded GIF buffer before the buffer is used by the viewer.
        /// </summary>
        BufferPreprocess BufferPreprocess { get; set; }

        /// <summary>
        /// Occurs when a key is pressed while the interface has focus.
        /// </summary>
        event EventHandler<SimpleKeyEventArgs> KeyPress;
        /// <summary>
        /// Occurs when the interface is clicked by the mouse.
        /// </summary>
        event EventHandler<SimpleMouseEventArgs> MouseClick;
        /// <summary>
        /// Occurs when the interface gains input focus.
        /// </summary>
        event EventHandler Focused;
        /// <summary>
        /// Occurs when the interface loses input focus.
        /// </summary>
        event EventHandler Unfocused;
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
    /// Provides data for the MouseClick event.
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

    /// <summary>
    /// Defines a function that takes a decoded GIF buffer and applies a transformation. Any specified restrictions must be maintained by
    /// the transforming function.
    /// </summary>
    /// <param name="buffer">The values that make up the image. The buffer is of logical size stride * height. It is of physical size
    /// stride * height * depth / 8.</param>
    /// <param name="palette">The color palette for the image. Each value in the buffer is an index into the array. Note the size of the
    /// palette can exceed the maximum size addressable by values in the buffer.</param>
    /// <param name="transparentIndex">The index of the transparent color, or -1 to indicate no transparency. Where possible, this index in
    /// the palette should be replaced by transparency in the resulting frame, or else some suitable replacement. The color value in the
    /// palette for this index is undefined.</param>
    /// <param name="stride">The stride width, in bytes, of the buffer.</param>
    /// <param name="width">The logical width of the image the buffer contains.</param>
    /// <param name="height">The logical height of the image the buffer contains.</param>
    /// <param name="depth">The bit depth of the buffer (either 1, 2, 4 or 8).</param>
    public delegate void BufferPreprocess(
    ref byte[] buffer, ref RgbColor[] palette, ref int transparentIndex, ref int stride, ref int width, ref int height, ref byte depth);
}