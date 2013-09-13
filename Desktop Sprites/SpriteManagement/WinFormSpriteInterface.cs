namespace DesktopSprites.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;
    using DesktopSprites.Collections;
    using DesktopSprites.Core;

    /// <summary>
    /// Creates a single Windows Form that is used as a canvas to display sprites.
    /// </summary>
    /// <remarks>
    /// Creates a single window that can support either 1-bit or 8-bit transparency. Using one window as a canvas gives reasonably scalable
    /// performance (for CPU side graphics, at least). There is no overhead in modifying the collection of sprites to be drawn each call.
    /// There is an overhead for maintaining a window that covers the whole drawing area. This will be negligible on most systems, but is
    /// quite costly when running in a virtual machine as the whole surface must be transmitted over the wire. However, in return the
    /// overhead for each additional sprite is very low.
    /// When alpha blending is disabled, this interface requires one color to be reserved for transparency. On Windows XP, this is
    /// RGB 0, 0, 0; i.e. black. This is due to a bug on that platform. On Vista and later the default key is RGB 0, 1, 0; but you may
    /// specify your own. When images are loaded, any colors conflicting with the key will be remapped to a similar color that does not
    /// conflict so they display correctly.
    /// When alpha blending is enabled, the transparency key is not used. The use of alpha blending forces the whole area to be refreshed
    /// with each draw, which will cause higher CPU usage compared to a non-blended form that can reduce the overall clipping rectangle.
    /// </remarks>
    public sealed class WinFormSpriteInterface : Disposable, ISpriteCollectionView
    {
        #region GraphicsForm class
        /// <summary>
        /// Transparent form that handles drawing and display of graphics.
        /// </summary>
        private class GraphicsForm : AlphaForm
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.GraphicsForm"/>
            /// class.
            /// </summary>
            /// <param name="useTransparencyKey">Pass true to set a transparency key; otherwise pass false.</param>
            public GraphicsForm(bool useTransparencyKey)
            {
                // Create the form.
                Name = GetType().Name;
                Text = "WinForm Sprite Window";
                FormBorderStyle = FormBorderStyle.None;
                AutoScaleMode = AutoScaleMode.None;
                Visible = false;

                // Tell the OS we'll be handling the drawing.
                // This causes the OnPaint and OnPaintBackground events to be raised for us.
                SetStyle(ControlStyles.UserPaint, true);
                // Prevents the OS from raising the OnPaintBackground event on us, which prevents flicker.
                // This event will now only occur when we raise it.
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);

                if (useTransparencyKey)
                    if (OperatingSystemInfo.IsWindowsXP)
                    {
                        // Certain versions of XP won't support transparency with the settings given so far. Using the
                        // SupportsTransparentBackColor draws the form onto an all zeroed buffer. The flag is only meant to support
                        // transparent controls on or above other controls and not to provide actual transparency on the desktop but we can
                        // abuse the all zero buffer. With the buffer being transparent black as a result of being zeroed, we can then set
                        // transparent black as the TransparencyKey to get achieve transparency.
                        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                        BackColor = Color.FromArgb(255, 0, 0, 0);
                        TransparencyKey = Color.FromArgb(0, 0, 0, 0);
                    }
                    else
                    {
                        // Forms don't truly support a transparent background, however a transparency key can be used to tell it to treat a
                        // certain color as transparent. So we'll try and use an uncommon color. This also has the bonus effect of making
                        // interaction fall through the transparent areas. This means interaction with the desktop is possible.
                        BackColor = Color.FromArgb(0, 1, 0);
                        TransparencyKey = BackColor;
                    }

                // Force creation of the window handle, so properties may be altered before the form is shown.
                if (!this.IsHandleCreated)
                    CreateHandle();
            }
        }
        #endregion

        #region WinFormContextMenuItem class
        /// <summary>
        /// Wraps a <see cref="T:System.Windows.Forms.ToolStripMenuItem"/> or <see cref="T:System.Windows.Forms.ToolStripSeparator"/> in
        /// order to expose the <see cref="T:DesktopSprites.SpriteManagement.ISimpleContextMenuItem"/> interface.
        /// </summary>
        private class WinFormContextMenuItem : ISimpleContextMenuItem, IDisposable
        {
            /// <summary>
            /// The <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> that owns this
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenuItem"/>.
            /// </summary>
            private WinFormSpriteInterface owner;
            /// <summary>
            /// The wrapped <see cref="T:System.Windows.Forms.ToolStripMenuItem"/>.
            /// </summary>
            private ToolStripMenuItem item;
            /// <summary>
            /// The wrapped <see cref="T:System.Windows.Forms.ToolStripSeparator"/>.
            /// </summary>
            private ToolStripSeparator separator;
            /// <summary>
            /// The method that runs on activation.
            /// </summary>
            private EventHandler activatedMethod;
            /// <summary>
            /// The method that runs on activation, queued to the <see cref="T:System.Threading.ThreadPool"/>.
            /// </summary>
            private EventHandler queuedActivatedMethod;

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenuItem"/> class for the given
            /// <see cref="T:System.Windows.Forms.ToolStripSeparator"/>.
            /// </summary>
            /// <param name="parent">The <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> that will own this
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenuItem"/>.</param>
            /// <param name="separatorItem">The underlying <see cref="T:System.Windows.Forms.ToolStripSeparator"/> that this class wraps.
            /// </param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="parent"/> is null.-or-<paramref name="separatorItem"/> is
            /// null.</exception>
            public WinFormContextMenuItem(WinFormSpriteInterface parent, ToolStripSeparator separatorItem)
            {
                Argument.EnsureNotNull(parent, "parent");
                Argument.EnsureNotNull(separatorItem, "separatorItem");

                owner = parent;
                separator = separatorItem;
            }
            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenuItem"/> class for the given
            /// <see cref="T:System.Windows.Forms.ToolStripMenuItem"/>, and links up the given activation method.
            /// </summary>
            /// <param name="parent">The <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> that will own this
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenuItem"/>.</param>
            /// <param name="menuItem">The underlying <see cref="T:System.Windows.Forms.ToolStripMenuItem"/> that this class wraps.</param>
            /// <param name="activated">The method to be run when the item is activated.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="parent"/> is null.-or-<paramref name="menuItem"/> is null.
            /// </exception>
            public WinFormContextMenuItem(WinFormSpriteInterface parent, ToolStripMenuItem menuItem, EventHandler activated)
            {
                Argument.EnsureNotNull(parent, "parent");
                Argument.EnsureNotNull(menuItem, "menuItem");

                owner = parent;
                item = menuItem;
                Activated = activated;
            }
            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenuItem"/> class for the given
            /// <see cref="T:System.Windows.Forms.ToolStripMenuItem"/>, which is assumed to produce a sub-menu of the given sub-items.
            /// </summary>
            /// <param name="parent">The <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> that will own this
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenuItem"/>.</param>
            /// <param name="menuItem">The underlying <see cref="T:System.Windows.Forms.ToolStripMenuItem"/> that this class wraps.</param>
            /// <param name="subItems">The items to appear in the sub-menu.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="parent"/> is null.-or-<paramref name="menuItem"/> is null.
            /// -or-<paramref name="subItems"/> is null.</exception>
            /// <exception cref="T:System.ArgumentException"><paramref name="menuItem"/> does not have drop down items.-or-The number of
            /// drop down items in <paramref name="menuItem"/> does not match the number of <paramref name="subItems"/>.</exception>
            public WinFormContextMenuItem(WinFormSpriteInterface parent, ToolStripMenuItem menuItem,
                IEnumerable<ISimpleContextMenuItem> subItems)
            {
                Argument.EnsureNotNull(parent, "parent");
                Argument.EnsureNotNull(menuItem, "menuItem");
                Argument.EnsureNotNull(subItems, "subItems");
                if (!menuItem.HasDropDownItems)
                    throw new ArgumentException("menuItem must have drop down items.", "menuItem");

                var subItemsArray = subItems.ToArray();

                if (menuItem.DropDownItems.Count != subItemsArray.Length)
                    throw new ArgumentException(
                        "The number of sub-items in menuItem is not the same as the number in the subItems collection.");

                var winFormSubItemsList = new ISimpleContextMenuItem[subItemsArray.Length];
                int index = 0;
                foreach (ToolStripItem toolStripItem in menuItem.DropDownItems)
                {
                    if (subItemsArray[index].IsSeparator)
                        winFormSubItemsList[index] =
                            new WinFormContextMenuItem(parent, (ToolStripSeparator)toolStripItem);
                    else if (subItemsArray[index].SubItems == null)
                        winFormSubItemsList[index] =
                            new WinFormContextMenuItem(parent, (ToolStripMenuItem)toolStripItem, subItemsArray[index].Activated);
                    else
                        winFormSubItemsList[index] =
                            new WinFormContextMenuItem(parent, (ToolStripMenuItem)toolStripItem, subItemsArray[index].SubItems);
                    index++;
                }

                SubItems = winFormSubItemsList;

                owner = parent;
                item = menuItem;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the item is a separator.
            /// </summary>
            public bool IsSeparator
            {
                get
                {
                    return separator != null;
                }
                set
                {
                    if (IsSeparator && !value)
                    {
                        owner.ApplicationInvoke(() =>
                        {
                            item.Dispose();
                            item = new ToolStripMenuItem();
                        });
                        separator = null;
                    }
                    else if (!IsSeparator && value)
                    {
                        owner.ApplicationInvoke(() =>
                        {
                            separator.Dispose();
                            separator = new ToolStripSeparator();
                        });
                        item = null;
                    }
                }
            }
            /// <summary>
            /// Gets or sets the text displayed to represent this item.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The item is a separator.</exception>
            public string Text
            {
                get
                {
                    if (IsSeparator)
                        throw new InvalidOperationException("Cannot get the text from a separator item.");
                    return item.Text;
                }
                set
                {
                    if (IsSeparator)
                        throw new InvalidOperationException("Cannot set the text on a separator item.");
                    owner.ApplicationInvoke(() => item.Text = value);
                }
            }
            /// <summary>
            /// Gets or sets the method that runs when the item is activated by the user.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The item does not support an activation method.-or-The interface has
            /// been closed.</exception>
            public EventHandler Activated
            {
                get
                {
                    if (IsSeparator || SubItems != null)
                        throw new InvalidOperationException("Cannot get the activation method from this type of item.");
                    return activatedMethod;
                }
                set
                {
                    if (IsSeparator || SubItems != null)
                        throw new InvalidOperationException("Cannot set the activation method on this type of item.");
                    owner.ApplicationInvoke(() =>
                    {
                        if (queuedActivatedMethod != null)
                            item.Click -= queuedActivatedMethod;

                        activatedMethod = value;
                        queuedActivatedMethod = null;

                        if (activatedMethod != null)
                        {
                            queuedActivatedMethod = (sender, e) => ThreadPool.QueueUserWorkItem(o => activatedMethod(sender, e));
                            item.Click += queuedActivatedMethod;
                        }
                    });
                }
            }
            /// <summary>
            /// Gets the sub-items in an item that displays a new sub-menu of items.
            /// </summary>
            public IList<ISimpleContextMenuItem> SubItems { get; private set; }

            /// <summary>
            /// Releases all resources used by the
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenuItem"/> object.
            /// </summary>
            public void Dispose()
            {
                if (item != null)
                    item.Dispose();

                if (separator != null)
                    separator.Dispose();

                if (SubItems != null)
                    foreach (IDisposable subitem in SubItems)
                        subitem.Dispose();
            }
        }
        #endregion

        #region WinFormContextMenu class
        /// <summary>
        /// Extends a <see cref="T:System.Windows.Forms.ContextMenuStrip"/> in order to expose the
        /// <see cref="T:DesktopSprites.SpriteManagement.ISimpleContextMenu"/> interface.
        /// </summary>
        private class WinFormContextMenu : ContextMenuStrip, ISimpleContextMenu
        {
            /// <summary>
            /// The <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> that owns this
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenu"/>.
            /// </summary>
            private WinFormSpriteInterface owner;
            /// <summary>
            /// Gets the collection of menu items in this menu.
            /// </summary>
            public new IList<ISimpleContextMenuItem> Items { get; private set; }

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenu"/> class to display the given menu
            /// items.
            /// </summary>
            /// <param name="parent">The <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> that will own this
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenu"/>.</param>
            /// <param name="menuItems">The items which should be displayed in this menu.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="parent"/> is null.-or-<paramref name="menuItems"/> is
            /// null.</exception>
            public WinFormContextMenu(WinFormSpriteInterface parent, IEnumerable<ISimpleContextMenuItem> menuItems)
            {
                Argument.EnsureNotNull(parent, "parent");
                Argument.EnsureNotNull(menuItems, "menuItems");

                owner = parent;

                var items = new List<ISimpleContextMenuItem>();
                Items = items;

                foreach (ISimpleContextMenuItem menuItem in menuItems)
                {
                    ToolStripItem winFormMenuItem;
                    WinFormContextMenuItem winFormContextMenuItem;
                    if (menuItem.IsSeparator)
                    {
                        winFormMenuItem = new ToolStripSeparator();
                        winFormContextMenuItem =
                            new WinFormContextMenuItem(parent, (ToolStripSeparator)winFormMenuItem);
                    }
                    else if (menuItem.SubItems == null)
                    {
                        winFormMenuItem = new ToolStripMenuItem(menuItem.Text);
                        winFormContextMenuItem =
                            new WinFormContextMenuItem(parent, (ToolStripMenuItem)winFormMenuItem, menuItem.Activated);
                    }
                    else
                    {
                        winFormMenuItem = CreateItemWithSubitems(menuItem);
                        winFormContextMenuItem =
                            new WinFormContextMenuItem(parent, (ToolStripMenuItem)winFormMenuItem, menuItem.SubItems);
                    }

                    base.Items.Add(winFormMenuItem);
                    items.Add(winFormContextMenuItem);
                }

                Disposed += (sender, e) =>
                {
                    foreach (WinFormContextMenuItem item in Items)
                        item.Dispose();
                };
            }

            /// <summary>
            /// Creates a <see cref="T:System.Windows.Forms.ToolStripMenuItem"/> from a specified
            /// <see cref="T:DesktopSprites.SpriteManagement.ISimpleContextMenuItem"/> that has sub items. If necessary, recursively calls
            /// itself to create another sub-menu for a sub-item that itself has sub-items.
            /// </summary>
            /// <param name="menuItem">The <see cref="T:DesktopSprites.SpriteManagement.ISimpleContextMenu"/> for which to create a
            /// <see cref="T:System.Windows.Forms.ToolStripMenuItem"/>.</param>
            /// <returns>A <see cref="T:System.Windows.Forms.ToolStripMenuItem"/> whose tree of sub-menus is fully initialized.</returns>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="menuItem"/> is null.</exception>
            /// <exception cref="T:System.ArgumentException">The sub-items collection in <paramref name="menuItem"/> is null or empty.
            /// </exception>
            private ToolStripMenuItem CreateItemWithSubitems(ISimpleContextMenuItem menuItem)
            {
                Argument.EnsureNotNull(menuItem, "menuItems");
                if (menuItem.SubItems == null || menuItem.SubItems.Count == 0)
                    throw new ArgumentException("menuItem.SubItems must not be null or empty.");

                ToolStripItem[] subitems = new ToolStripItem[menuItem.SubItems.Count];
                for (int i = 0; i < subitems.Length; i++)
                {
                    ISimpleContextMenuItem subitem = menuItem.SubItems[i];

                    ToolStripItem winFormMenuItem;
                    if (subitem.IsSeparator)
                        winFormMenuItem = new ToolStripSeparator();
                    else if (subitem.SubItems == null)
                        winFormMenuItem = new ToolStripMenuItem(subitem.Text);
                    else
                        winFormMenuItem = CreateItemWithSubitems(subitem);
                    subitems[i] = winFormMenuItem;
                }
                return new ToolStripMenuItem(menuItem.Text, null, subitems);
            }

            /// <summary>
            /// Displays the context menu at the given co-ordinates.
            /// </summary>
            /// <param name="x">The x co-ordinate of the location where the menu should be shown.</param>
            /// <param name="y">The y co-ordinate of the location where the menu should be shown.</param>
            public new void Show(int x, int y)
            {
                owner.ApplicationInvoke(() => base.Show(x, y));
            }
        }
        #endregion

        #region Fields and Properties
        /// <summary>
        /// Gets or sets the FrameRecordCollector for debugging purposes.
        /// </summary>
        internal AnimationLoopBase.FrameRecordCollector Collector { get; set; }

        /// <summary>
        /// Indicates if the form has been opened.
        /// </summary>
        private bool opened;
        /// <summary>
        /// Indicates if drawing is paused.
        /// </summary>
        private bool paused;
        /// <summary>
        /// Indicates if the form has begun the process of closing (or has closed).
        /// </summary>
        private bool closing;
        /// <summary>
        /// Stores the images for each sprite as a series of <see cref="T:DesktopSprites.SpriteManagement.BitmapFrame"/>, indexed by
        /// filename.
        /// </summary>
        private LazyDictionary<string, AnimatedImage<BitmapFrame>> images;

        /// <summary>
        /// The underlying form on which graphics are displayed.
        /// </summary>
        private GraphicsForm form;
        /// <summary>
        /// The context that allocates graphics buffers.
        /// </summary>
        private BufferedGraphicsContext bufferedGraphicsContext;
        /// <summary>
        /// The graphics buffer of the form to which drawing is done and from which rendering to the screen is performed.
        /// </summary>
        private BufferedGraphics bufferedGraphics;
        /// <summary>
        /// The graphics surface of the form (unbuffered), used to provide the pixel format required when allocating graphics buffers.
        /// </summary>
        private Graphics graphics;
        /// <summary>
        /// Indicates if any manual painting is taking place.
        /// </summary>
        private bool manualPainting;
        /// <summary>
        /// The <see cref="T:System.Windows.Forms.PaintEventHandler"/> that ensures the form is repainted when paused.
        /// </summary>
        private PaintEventHandler manualRender;
        /// <summary>
        /// The <see cref="T:System.Windows.Forms.PaintEventHandler"/> that ensures the form can be manually cleared.
        /// </summary>
        private PaintEventHandler manualClear;
        /// <summary>
        /// Gets a value indicating whether alpha blending is in use. If true, pixels which are partially transparent will be blended with
        /// those behind them to achieve proper transparency; otherwise these pixels will be rendered opaque, and only fully transparent
        /// pixels will render as transparent, resulting in simple 1-bit transparency.
        /// </summary>
        public bool IsAlphaBlended { get; private set; }
        /// <summary>
        /// Specifies the palette mapping that maps the transparency key to another color to avoid conflict.
        /// </summary>
        private readonly Dictionary<Color, Color> paletteMapping = new Dictionary<Color, Color>(1);
        /// <summary>
        /// Represents the area that becomes invalidated before updating. This area needs to be cleared.
        /// </summary>
        private readonly Region preUpdateInvalidRegion = new Region();
        /// <summary>
        /// Represents the area that becomes invalidated after updating. This area needs to be drawn.
        /// </summary>
        private readonly Region postUpdateInvalidRegion = new Region();
        /// <summary>
        /// Method call that renders the current sprite collection to the window.
        /// </summary>
        private readonly MethodInvoker render;
        /// <summary>
        /// Collection of sprites to be rendered.
        /// </summary>
        private AsyncLinkedList<ISprite> sprites;

        /// <summary>
        /// List of <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenu"/> which have been created by
        /// the interface.
        /// </summary>
        private readonly LinkedList<WinFormContextMenu> contextMenus = new LinkedList<WinFormContextMenu>();
        /// <summary>
        /// The full <see cref="T:System.Drawing.Font"/> definition to be used when drawing text to the screen.
        /// </summary>
        private readonly Font font = new Font(FontFamily.GenericSansSerif, 12, GraphicsUnit.Pixel);
        /// <summary>
        /// A <see cref="T:System.Drawing.Brush"/> whose color is roughly white.
        /// </summary>
        private Brush whiteBrush;
        /// <summary>
        /// A <see cref="T:System.Drawing.Brush"/> whose color is roughly black.
        /// </summary>
        private Brush blackBrush;
        /// <summary>
        /// A <see cref="T:System.Drawing.Pen"/> whose color is roughly black.
        /// </summary>
        private Pen blackPen;
        /// <summary>
        /// The identity matrix.
        /// </summary>
        private readonly Matrix identityMatrix = new Matrix();
        /// <summary>
        /// Gets or sets a value indicating whether the performance graph should be displayed.
        /// </summary>
        public bool ShowPerformanceGraph { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the clipping region should be displayed.
        /// </summary>
        public bool ShowClippingRegion { get; set; }

        /// <summary>
        /// Path to the icon for the form.
        /// </summary>
        private string windowIconFilePath = null;
        /// <summary>
        /// Custom icon for the form.
        /// </summary>
        private Icon windowIcon = null;

        /// <summary>
        /// Gets or sets the text associated with the form.
        /// </summary>
        public string WindowTitle
        {
            get { return form.Text; }
            set { ApplicationInvoke(() => form.Text = value); }
        }
        /// <summary>
        /// Gets or sets the icon for the form.
        /// </summary>
        public string WindowIconFilePath
        {
            get
            {
                return windowIconFilePath;
            }
            set
            {
                windowIconFilePath = value;
                ApplicationInvoke(() =>
                {
                    if (windowIcon != null)
                        windowIcon.Dispose();
                    if (value == null)
                        form.Icon = windowIcon = null;
                    else
                        form.Icon = windowIcon = new Icon(value);
                });
            }
        }
        /// <summary>
        /// Gets or sets the color that will appear as transparent when displaying sprites. This only applies if alpha-blending is
        /// disabled.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The operating system is Windows XP (black is the only supported key on this
        /// platform).-or-Images have been loaded. The key must be set before any images are loaded by the interface.</exception>
        /// <exception cref="System.ArgumentException">The operation was set and the given color was not opaque.</exception>
        public Color TransparencyKey
        {
            get
            {
                return form.TransparencyKey;
            }
            set
            {
                if (OperatingSystemInfo.IsWindowsXP)
                    throw new InvalidOperationException("The transparency key may not be altered on Windows XP.");
                if (images.Count > 0)
                    throw new InvalidOperationException("The transparency key may not be altered if images have been loaded.");
                if (value.A != 255)
                    throw new ArgumentException("value must be an opaque color, i.e. its alpha component must be 255.");

                ApplicationInvoke(() =>
                {
                    form.BackColor = value;
                    form.TransparencyKey = value;
                    RemapKeyConflicts();
                });
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the form should be displayed as a topmost form. This will cause a momentary graphical
        /// stutter if the form has already been opened and alpha blending is disabled. If the form is not paused, this will not take
        /// effect until Draw is called.
        /// </summary>
        public bool Topmost
        {
            get
            {
                return form.TopMost;
            }
            set
            {
                if (form.TopMost != value)
                    ApplicationInvoke(() =>
                    {
                        if (!opened)
                            form.TopMost = value;
                        else
                            ToggleTopmost(paused);
                    });
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the form is displayed in the taskbar.
        /// </summary>
        public bool ShowInTaskbar
        {
            get { return form.ShowInTaskbar; }
            set { ApplicationInvoke(() => form.ShowInTaskbar = value); }
        }
        /// <summary>
        /// Gets or sets the display boundary of the form.
        /// </summary>
        public Rectangle DisplayBounds
        {
            get
            {
                return form.DesktopBounds;
            }
            set
            {
                if (form.DesktopBounds != value)
                    ApplicationInvoke(() =>
                    {
                        form.DesktopBounds = value;
                        AllocateBuffers();
                        Render();
                    });
            }
        }
        /// <summary>
        /// Gets the current position of the cursor.
        /// </summary>
        public Point CursorPosition
        {
            get { return Cursor.Position; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Gets the equivalent <see cref="T:DesktopSprites.SpriteManagement.SimpleMouseButtons"/> enumeration from the native button
        /// enumeration.
        /// </summary>
        /// <param name="buttons">The <see cref="T:System.Windows.Forms.MouseButtons"/> enumeration of the mouse button that was pressed.
        /// </param>
        /// <returns>The equivalent <see cref="T:DesktopSprites.SpriteManagement.SimpleMouseButtons"/> enumeration for this button.
        /// </returns>
        private static SimpleMouseButtons GetButtonsFromNative(MouseButtons buttons)
        {
            SimpleMouseButtons simpleButtons = SimpleMouseButtons.None;
            if (buttons.HasFlag(MouseButtons.Left))
                simpleButtons |= SimpleMouseButtons.Left;
            if (buttons.HasFlag(MouseButtons.Right))
                simpleButtons |= SimpleMouseButtons.Right;
            if (buttons.HasFlag(MouseButtons.Middle))
                simpleButtons |= SimpleMouseButtons.Middle;
            return simpleButtons;
        }
        /// <summary>
        /// Raised when a mouse button has been pressed down.
        /// Raises the MouseDown event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GraphicsForm_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDown.Raise(this, () => new SimpleMouseEventArgs(GetButtonsFromNative(e.Button), e.X + form.Left, e.Y + form.Top));
        }
        /// <summary>
        /// Raised when a mouse button has been clicked.
        /// Raises the MouseClick event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GraphicsForm_MouseClick(object sender, MouseEventArgs e)
        {
            MouseClick.Raise(this, () => new SimpleMouseEventArgs(GetButtonsFromNative(e.Button), e.X + form.Left, e.Y + form.Top));
        }
        /// <summary>
        /// Raised when a mouse button has been released.
        /// Raises the MouseUp event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GraphicsForm_MouseUp(object sender, MouseEventArgs e)
        {
            MouseUp.Raise(this, () => new SimpleMouseEventArgs(GetButtonsFromNative(e.Button), e.X + form.Left, e.Y + form.Top));
        }
        /// <summary>
        /// Raised when a key has been pressed.
        /// Raises the KeyPress event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GraphicsForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPress.Raise(this, () => new SimpleKeyEventArgs(e.KeyChar));
        }

        /// <summary>
        /// Occurs when a key is pressed while the window has focus.
        /// </summary>
        public event EventHandler<SimpleKeyEventArgs> KeyPress;
        /// <summary>
        /// Occurs when the mouse pointer is over the window and a mouse button is pressed.
        /// </summary>
        public event EventHandler<SimpleMouseEventArgs> MouseDown;
        /// <summary>
        /// Occurs when the window is clicked by the mouse.
        /// </summary>
        public event EventHandler<SimpleMouseEventArgs> MouseClick;
        /// <summary>
        /// Occurs when the mouse pointer is over the window and a mouse button is released.
        /// </summary>
        public event EventHandler<SimpleMouseEventArgs> MouseUp;
        /// <summary>
        /// Occurs when the interface is closed.
        /// </summary>
        public event EventHandler InterfaceClosed;
        #endregion

        /// <summary>
        /// Gets a value indicating whether a <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> can be used in the
        /// current environment.
        /// </summary>
        public static bool IsRunable
        {
            get
            {
                try
                {
                    AppDomain domain = AppDomain.CreateDomain("Assembly Test Domain");
                    domain.Load("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                    domain.Load("System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                    AppDomain.Unload(domain);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                // Cannot run this on a non-Windows platform due to platform invoke.
                if (!OperatingSystemInfo.IsWindows)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> class.
        /// </summary>
        /// <param name="displayBounds">The initial display boundary the interface should cover. Sprites outside this area will not be
        /// drawn.</param>
        /// <param name="useAlphaBlending">Indicates if alpha blending should be supported. If true, the transparency value in images is
        /// respected, and the color is blended with the color behind it. If false, semi-transparent values will appear opaque. Only fully
        /// transparent pixels will continue to render as transparent.</param>
        public WinFormSpriteInterface(Rectangle displayBounds, bool useAlphaBlending)
        {
            IsAlphaBlended = useAlphaBlending;
            images = new LazyDictionary<string, AnimatedImage<BitmapFrame>>(
                fileName => new AnimatedImage<BitmapFrame>(
                    fileName, BitmapFrameFromFile,
                    (b, p, tI, s, w, h, d, hC) => BitmapFrameFromBuffer(b, p, tI, s, w, h, d, hC, fileName),
                    BitmapFrame.AllowableBitDepths));
            render = new MethodInvoker(Render);

            Thread appThread = new Thread(ApplicationRun) { Name = "WinFormSpriteInterface.ApplicationRun" };
            appThread.SetApartmentState(ApartmentState.STA);
            object appInitiated = new object();
            lock (appInitiated)
            {
                appThread.Start(Tuple.Create(appInitiated, displayBounds));
                Monitor.Wait(appInitiated);
            }
        }

        /// <summary>
        /// Runs the main application loop.
        /// </summary>
        /// <param name="parameter">Tuple containing the object to pulse and the display bounds.</param>
        private void ApplicationRun(object parameter)
        {
            var parameters = (Tuple<object, Rectangle>)parameter;

            // Create the form.
            form = new GraphicsForm(!IsAlphaBlended);
            form.FormClosing += GraphicsForm_FormClosing;
            form.Disposed += GraphicsForm_Disposed;

            DisplayBounds = parameters.Item2;
            RemapKeyConflicts();

            // Hook up to form events.
            form.MouseDown += GraphicsForm_MouseDown;
            form.MouseClick += GraphicsForm_MouseClick;
            form.MouseUp += GraphicsForm_MouseUp;
            form.KeyPress += GraphicsForm_KeyPress;

            // Create manual painting handlers.
            manualRender = (sender, e) =>
            {
                if (IsAlphaBlended)
                    form.UpdateBackgroundGraphics();
                else
                    bufferedGraphics.Render(e.Graphics);
            };
            manualClear = (sender, e) => e.Graphics.Clear(form.TransparencyKey);

            postUpdateInvalidRegion.MakeEmpty();

            lock (parameters.Item1)
                Monitor.Pulse(parameters.Item1);
            Application.Run();
        }

        /// <summary>
        /// Invokes a method synchronously on the main application thread.
        /// </summary>
        /// <param name="method">The method to invoke. The method should take no parameters and return void.</param>
        private void ApplicationInvoke(MethodInvoker method)
        {
            // Check disposal has not begun, as we don't want to process any more actions if the interface is being closed.
            // The is a race condition since disposal might begin after this check, hence the try-catch block.
            if (closing)
                return;
            try
            {
                form.SmartInvoke(method);
            }
            catch (ObjectDisposedException ex)
            {
                // Handle the race condition where the form was disposed just after we checked.
                // If some other object was disposed, then we still need to let that exception get passed upwards.
                if (ex.ObjectName != form.GetType().Name)
                    throw;
            }
        }

        /// <summary>
        /// Maps drawing brushes and pens around the transparency key, and sets the paletteMapping for images to avoid conflict.
        /// </summary>
        private void RemapKeyConflicts()
        {
            Color key = form.BackColor;
            Color remappedKey;

            if (key.G == 255)
                remappedKey = Color.FromArgb(key.R, 254, key.B);
            else
                remappedKey = Color.FromArgb(key.R, key.G + 1, key.B);

            paletteMapping.Clear();
            paletteMapping.Add(key, remappedKey);

            if (whiteBrush != null)
                whiteBrush.Dispose();
            if (blackBrush != null)
                blackBrush.Dispose();
            if (blackPen != null)
                blackPen.Dispose();

            if (key.R == 255 && key.G == 255 && key.B == 255)
                whiteBrush = new SolidBrush(Color.FromArgb(255, 254, 255));
            else
                whiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255));

            if (key.R == 0 && key.G == 0 && key.B == 0)
            {
                blackBrush = new SolidBrush(Color.FromArgb(0, 1, 0));
                blackPen = new Pen(Color.FromArgb(0, 1, 0));
            }
            else
            {
                blackBrush = new SolidBrush(Color.FromArgb(0, 0, 0));
                blackPen = new Pen(Color.FromArgb(0, 0, 0));
            }
        }

        /// <summary>
        /// Creates a new <see cref="T:DesktopSprites.SpriteManagement.BitmapFrame"/> from the given file, loading extra transparency
        /// information and adjusting the colors as required by the transparency.
        /// </summary>
        /// <param name="fileName">The path to a static image file from which to create a new
        /// <see cref="T:DesktopSprites.SpriteManagement.BitmapFrame"/>.
        /// </param>
        /// <returns>A new <see cref="T:DesktopSprites.SpriteManagement.BitmapFrame"/> created from the given file.</returns>
        private BitmapFrame BitmapFrameFromFile(string fileName)
        {
            return Disposable.SetupSafely(new BitmapFrame(fileName), frame =>
            {
                if (IsAlphaBlended)
                    frame.Image.PreMultiplyAlpha();
                else
                    frame.Image.RemapColors(paletteMapping);
            });
        }

        /// <summary>
        /// Creates a new <see cref="T:DesktopSprites.SpriteManagement.BitmapFrame"/> from the raw buffer, loading extra transparency
        /// information and adjusting the colors as required by the transparency.
        /// </summary>
        /// <param name="buffer">The raw buffer.</param>
        /// <param name="palette">The color palette.</param>
        /// <param name="transparentIndex">The index of the transparent color.</param>
        /// <param name="stride">The stride width of the buffer.</param>
        /// <param name="width">The logical width of the buffer.</param>
        /// <param name="height">The logical height of the buffer.</param>
        /// <param name="depth">The bit depth of the buffer.</param>
        /// <param name="hashCode">The hash code of the frame.</param>
        /// <param name="fileName">The path to the GIF file being loaded.</param>
        /// <returns>A new <see cref="T:DesktopSprites.SpriteManagement.BitmapFrame"/> for the frame held in the raw buffer.</returns>
        private BitmapFrame BitmapFrameFromBuffer(byte[] buffer, RgbColor[] palette, int transparentIndex,
            int stride, int width, int height, int depth, int hashCode, string fileName)
        {
            return Disposable.SetupSafely(
                BitmapFrame.FromBuffer(buffer, palette, transparentIndex, stride, width, height, depth, hashCode),
                frame =>
                {
                    if (IsAlphaBlended)
                    {
                        // Check for an alpha remapping table, and apply it if one exists.
                        string mapFilePath = Path.ChangeExtension(fileName, AlphaRemappingTable.FileExtension);
                        if (File.Exists(mapFilePath))
                        {
                            AlphaRemappingTable map = new AlphaRemappingTable();
                            map.LoadMap(mapFilePath);
                            ColorPalette colorPalette = frame.Image.Palette;
                            for (int i = 0; i < colorPalette.Entries.Length; i++)
                            {
                                Color color = colorPalette.Entries[i];
                                ArgbColor argbColor;
                                if (map.TryGetMapping(new RgbColor(color.R, color.G, color.B), out argbColor))
                                    colorPalette.Entries[i] = Color.FromArgb(argbColor.ToArgb());
                            }
                            frame.Image.Palette = colorPalette;
                            // We only need to pre-multiply when we add an alpha channel ourselves, as GIFs only support 1 bit transparency.
                            frame.Image.PreMultiplyAlpha();
                        }
                    }
                    else
                    {
                        frame.Image.RemapColors(paletteMapping);
                    }
                });
        }

        /// <summary>
        /// Toggles the topmost display of the form and recreates the graphics surfaces since they become invalidated.
        /// </summary>
        /// <param name="beginManualPainting">If true, indicates the paint method of the form should be hooked on to in order to ensure the
        /// graphics get redrawn.</param>
        private void ToggleTopmost(bool beginManualPainting)
        {
            // Toggle the topmost setting.
            form.TopMost = !form.TopMost;

            if (!IsAlphaBlended)
            {
                // Recreate the buffers as they will have become invalid.
                graphics.Dispose();
                graphics = form.CreateGraphics();
                graphics.SetClip(DisplayBounds);
                bufferedGraphics.Dispose();
                bufferedGraphics = bufferedGraphicsContext.Allocate(graphics, DisplayBounds);
                bufferedGraphics.Graphics.SetClip(DisplayBounds);
                bufferedGraphics.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            }

            if (beginManualPainting && !manualPainting)
            {
                if (!IsAlphaBlended)
                {
                    // We must offset the form as rendering the buffer does not account for its transparent edges.
                    form.Location = Point.Truncate(postUpdateInvalidRegion.GetBounds(bufferedGraphics.Graphics).Location);
                }
                // Hook up to form painting so our buffer still gets drawn.
                form.Paint += manualRender;
                manualPainting = true;
            }
        }

        /// <summary>
        /// Loads the given collection of file paths as images in a format that this
        /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> can display.
        /// </summary>
        /// <param name="imageFilePaths">The collection of paths to image files that should be loaded by the interface. Any images not
        /// loaded by this method will be loaded on demand.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="imageFilePaths"/> is null.</exception>
        public void LoadImages(IEnumerable<string> imageFilePaths)
        {
            LoadImages(imageFilePaths, null);
        }

        /// <summary>
        /// Loads the given collection of file paths as images in a format that this
        /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> can display.
        /// </summary>
        /// <param name="imageFilePaths">The collection of paths to image files that should be loaded by the interface. Any images not
        /// loaded by this method will be loaded on demand.</param>
        /// <param name="imageLoadedHandler">An <see cref="T:System.EventHandler"/> that is raised when an image is loaded.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="imageFilePaths"/> is null.</exception>
        public void LoadImages(IEnumerable<string> imageFilePaths, EventHandler imageLoadedHandler)
        {
            Argument.EnsureNotNull(imageFilePaths, "imageFilePaths");

            foreach (string imageFilePath in imageFilePaths)
                images.Add(imageFilePath);
            images.InitializeAll(true, (sender, e) => imageLoadedHandler.Raise(this));
        }

        /// <summary>
        /// Creates an <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> specific context menu for the given set of
        /// menu items.
        /// </summary>
        /// <param name="menuItems">The collections of items to be displayed in the menu.</param>
        /// <returns>An <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> specific context menu.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="menuItems"/> is null.</exception>
        public ISimpleContextMenu CreateContextMenu(IEnumerable<ISimpleContextMenuItem> menuItems)
        {
            WinFormContextMenu menu = null;
            ApplicationInvoke(() => menu = new WinFormContextMenu(this, menuItems));
            contextMenus.AddLast(menu);
            return menu;
        }

        /// <summary>
        /// Opens the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/>.
        /// </summary>
        public void Open()
        {
            ApplicationInvoke(() =>
            {
                AllocateBuffers();
                form.Show();
                // Reapply this setting, as otherwise it may not be applied when the form first opens.
                if (form.TopMost)
                    form.TopMost = true;
                opened = true;
            });
        }

        /// <summary>
        /// Allocates the graphics buffers to cover the display area of the form.
        /// </summary>
        private void AllocateBuffers()
        {
            if (IsAlphaBlended)
            {
                form.BackgroundGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            }
            else
            {
                // Windows Forms provides easy ways to double buffer forms. Double buffering prevents flicker by drawing to a back buffer,
                // then displaying it all at once. It can be quickly set with DoubleBuffered = true or
                // SetStyle(ControlStyles.OptimizedDoubleBuffer, true). However this method creates a lot of garbage, so it's better to
                // handle it manually. We create our own buffer surface, draw to that and render when done. Allocating a full size buffer
                // will prevent the context from having to create a larger surface when resizing and creating garbage later. Instead it can
                // reuse portions of the existing surface.
                if (graphics == null)
                    graphics = form.CreateGraphics();
                graphics.SetClip(DisplayBounds);
                if (bufferedGraphicsContext == null)
                    bufferedGraphicsContext = new BufferedGraphicsContext();
                bufferedGraphicsContext.MaximumBuffer = DisplayBounds.Size;
                if (bufferedGraphics != null)
                    bufferedGraphics.Dispose();
                bufferedGraphics = bufferedGraphicsContext.Allocate(graphics, DisplayBounds);
            }
        }

        /// <summary>
        /// Hides the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/>.
        /// </summary>
        public void Hide()
        {
            ApplicationInvoke(form.Hide);
        }

        /// <summary>
        /// Shows the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/>.
        /// </summary>
        public void Show()
        {
            ApplicationInvoke(form.Show);
        }

        /// <summary>
        /// Freezes the display of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/>.
        /// </summary>
        public void Pause()
        {
            paused = true;
        }

        /// <summary>
        /// Resumes display of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> from a paused state.
        /// </summary>
        public void Resume()
        {
            if (opened && paused)
            {
                // Reset any manual painting that is taking place.
                if (manualPainting)
                {
                    ApplicationInvoke(() =>
                    {
                        form.Paint -= manualRender;
                        form.Paint += manualClear;
                        form.Invalidate();
                        form.Update();
                        form.Paint -= manualClear;
                        form.Location = Point.Empty;
                        manualPainting = false;
                    });
                }

                Show();
                paused = false;
            }
        }

        /// <summary>
        /// Draws the given collection of sprites.
        /// </summary>
        /// <param name="sprites">The collection of sprites to draw.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sprites"/> is null.</exception>
        public void Draw(AsyncLinkedList<ISprite> sprites)
        {
            Argument.EnsureNotNull(sprites, "sprites");

            if (paused)
                return;

            this.sprites = sprites;
            ApplicationInvoke(render);
            this.sprites = null;
        }

        /// <summary>
        /// Renders the current sprites to the window.
        /// </summary>
        private void Render()
        {
            if (sprites == null)
                return;

            // Get the target graphics surface.
            Graphics surface;
            if (IsAlphaBlended)
                surface = form.BackgroundGraphics;
            else
                surface = bufferedGraphics.Graphics;

            // Translation offset so the top-left of the form and drawing surface coincide.
            Size translate = new Size(-form.Left, -form.Top);

            // Save the invalid region from last frame.
            preUpdateInvalidRegion.MakeEmpty();
            preUpdateInvalidRegion.Union(postUpdateInvalidRegion);

            // Reset the post-update clipping region.
            postUpdateInvalidRegion.MakeEmpty();

            #region Timings Graph Area
            Rectangle timingsArea = Rectangle.Empty;
            string timingsInfo = null;
            if (Collector != null && ShowPerformanceGraph && Collector.Count != 0)
            {
                // Create info string.
                timingsInfo = "fps: " +
                    Collector.FramesPerSecond.ToString("0.0", System.Globalization.CultureInfo.CurrentCulture) + "/" +
                    Collector.AchievableFramesPerSecond.ToString("0.0", System.Globalization.CultureInfo.CurrentCulture);
                Size timingsSize = Size.Ceiling(surface.MeasureString(timingsInfo, font));

                // Set location and get area of graph draw.
                Point offset = new Point(10, 10) + translate;
                Point graphLocation = new Point(offset.X, offset.Y + timingsSize.Height);
                Rectangle recorderGraphArea = Collector.SetGraphingAttributes(graphLocation, 150, 1, 1.5f);
                postUpdateInvalidRegion.Union(recorderGraphArea);

                // Set location of info string draw.
                timingsArea = new Rectangle(
                    recorderGraphArea.Left, recorderGraphArea.Top - timingsSize.Height - 1,
                    timingsSize.Width + 1, timingsSize.Height + 1);
                postUpdateInvalidRegion.Union(timingsArea);
            }
            #endregion

            // Determine and apply the clipping area, this is only beneficial when there are few sprites on screen. As more are added
            // the cost of hit testing that combined area becomes larger, and so it's just cheaper to redraw everything than to perform
            // hit testing on a complex area, and then end up redrawing most everything anyway.
            const int Threshold = 15;
            if (sprites.Count < Threshold)
            {
                foreach (ISprite sprite in sprites)
                {
                    postUpdateInvalidRegion.Union(OffsetRectangle(sprite.Region, translate));
                    ISpeakingSprite speakingSprite = sprite as ISpeakingSprite;
                    if (speakingSprite != null && speakingSprite.IsSpeaking)
                        postUpdateInvalidRegion.Union(OffsetRectangle(GetSpeechBubbleRegion(speakingSprite, surface), translate));
                }
                postUpdateInvalidRegion.Intersect(OffsetRectangle(DisplayBounds, translate));
            }
            else
            {
                postUpdateInvalidRegion.Union(OffsetRectangle(DisplayBounds, translate));
            }

            // Determine the current clipping area required, and clear it of old graphics.
            if (IsAlphaBlended)
            {
                surface.SetClip(preUpdateInvalidRegion, CombineMode.Replace);
                surface.Clear(Color.FromArgb(0));
            }
            else
            {
                surface = ResizeGraphicsBuffer();
                surface.Clear(form.TransparencyKey);
            }

            // Set the clipping area to the region we'll be drawing in for this frame.
            surface.SetClip(postUpdateInvalidRegion, CombineMode.Replace);

            #region Show Clipping Region
            if (ShowClippingRegion)
            {
                // Get the clipping region as a series of non-overlapping rectangles.
                RectangleF[] invalidRectangles = postUpdateInvalidRegion.GetRegionScans(identityMatrix);

                // Display the clipping rectangles.
                foreach (RectangleF invalidRectangleF in invalidRectangles)
                {
                    Rectangle invalidRectangle = new Rectangle(
                        (int)invalidRectangleF.X, (int)invalidRectangleF.Y,
                        (int)invalidRectangleF.Width - 1, (int)invalidRectangleF.Height - 1);
                    surface.DrawRectangle(Pens.Blue, invalidRectangle);
                }
            }
            #endregion

            // Draw all the sprites.
            foreach (ISprite sprite in sprites)
            {
                // Draw the sprite image.
                BitmapFrame frame = images[sprite.ImagePath][sprite.CurrentTime];
                frame.Flip(sprite.FlipImage);
                surface.DrawImage(frame.Image, OffsetRectangle(sprite.Region, translate));

                // Draw a speech bubble for a speaking sprite.
                ISpeakingSprite speakingSprite = sprite as ISpeakingSprite;
                if (speakingSprite != null && speakingSprite.IsSpeaking)
                {
                    Rectangle bubble = OffsetRectangle(GetSpeechBubbleRegion(speakingSprite, surface), translate);
                    surface.FillRectangle(whiteBrush, bubble.X + 1, bubble.Y + 1, bubble.Width - 2, bubble.Height - 2);
                    surface.DrawRectangle(blackPen,
                        new Rectangle(bubble.X, bubble.Y, bubble.Width - 1, bubble.Height - 1));
                    surface.DrawString(speakingSprite.SpeechText, font, blackBrush, bubble.Location);
                }
            }

            #region Timings Graph
            if (Collector != null && ShowPerformanceGraph && Collector.Count != 0)
            {
                // Display a graph of frame times and garbage collections.
                Collector.DrawGraph(surface);

                // Display how long this frame took.
                surface.FillRectangle(blackBrush, timingsArea);
                surface.DrawString(timingsInfo, font, whiteBrush, timingsArea.Left, timingsArea.Top);
            }
            #endregion

            // Render the result.
            if (IsAlphaBlended)
                form.UpdateBackgroundGraphics();
            else
                bufferedGraphics.Render();
        }

        /// <summary>
        /// Returns a copy of the specified rectangle, whose location is offset by the specified amount.
        /// </summary>
        /// <param name="rectangle">The rectangle to offset.</param>
        /// <param name="offset">The amount to offset the location of the rectangle.</param>
        /// <returns>A new rectangle offset by the specified amount.</returns>
        private static Rectangle OffsetRectangle(Rectangle rectangle, Size offset)
        {
            Rectangle offsetRectangle = rectangle;
            offsetRectangle.Location += offset;
            return offsetRectangle;
        }

        /// <summary>
        /// Gets the region covered by a speech bubble for a speaking sprite.
        /// </summary>
        /// <param name="speakingSprite">A sprite with speaking capabilities, for which a speech bubble should be found.</param>
        /// <param name="surface">The <see cref="T:System.Drawing.Graphics"/> surface onto which to draw the text.</param>
        /// <returns>The region the internal area of a speech bubble for this sprite will occupy, or
        /// <see cref="P:System.Drawing.Rectangle.Empty"/> if the sprite is not speaking.</returns>
        private Rectangle GetSpeechBubbleRegion(ISpeakingSprite speakingSprite, Graphics surface)
        {
            if (!speakingSprite.IsSpeaking)
                return Rectangle.Empty;

            Size speechSize = Size.Ceiling(surface.MeasureString(speakingSprite.SpeechText, font));
            Point location = new Point(
                speakingSprite.Region.X + speakingSprite.Region.Width / 2 - speechSize.Width / 2 - 1,
                speakingSprite.Region.Y - speechSize.Height - 1);
            speechSize.Width += 1;
            speechSize.Height += 1;
            return new Rectangle(location, speechSize);
        }

        /// <summary>
        /// Resizes the graphics surface of the bufferedGraphics object to cover the area given by the current invalidation regions.
        /// </summary>
        /// <returns>The newly created surface in use by the bufferedGraphics object.</returns>
        private Graphics ResizeGraphicsBuffer()
        {
            // Determine the bounds of the current draw.
            // This needs to cover areas to be cleared and areas to be drawn.
            RectangleF boundsF =
                RectangleF.Union(preUpdateInvalidRegion.GetBounds(graphics), postUpdateInvalidRegion.GetBounds(graphics));
            // See if the bounds differ from the current buffer.
            if (bufferedGraphics.Graphics.ClipBounds != boundsF)
            {
                // Reallocate the buffer at the new size. We'll need to clip the whole area, so it can be filled with the right color. 
                // Doing this doesn't have any real effect on our program, but it does help the desktop composition program out.
                // dwm.exe (Vista and later) maintains bitmaps of all running programs so it can do the fancy stuff for the Aero theme.
                // By redrawing a full size buffer every tick, it needs to update a large area and uses significant CPU time. By
                // redrawing a minimally sized buffer, we can reduce the pixel count significantly and thus it will use less CPU.
                // Whilst helpful, this is obviously limited by the size we can set the buffer to. For example, if there are graphics
                // in two opposite corners then we still need a full size buffer and thus no real savings results. On the plus side
                // even small reductions in width and height result in big savings on area (e.g. 90% width and 90% height is only 81%
                // of the area), and the saved CPU indirectly benefits our frame time.
                bufferedGraphics.Dispose();
                bufferedGraphics = bufferedGraphicsContext.Allocate(graphics, boundsF.BoundingRectangle());
                bufferedGraphics.Graphics.SetClip(OffsetRectangle(DisplayBounds, new Size(-form.Left, -form.Top)));

                // Set the quality of drawing.
                // As a result of having to use a transparency key, we can't use anti-aliasing functions.
                // If we did, semi-transparent pixels would be blended with the form background color.
                bufferedGraphics.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            }
            else
            {
                // We can reuse the existing buffer, and so reduce the area needing to be cleared.
                bufferedGraphics.Graphics.SetClip(preUpdateInvalidRegion, CombineMode.Replace);
            }
            return bufferedGraphics.Graphics;
        }

        /// <summary>
        /// Closes the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/>.
        /// </summary>
        public void Close()
        {
            if (closing)
                return;
            ApplicationInvoke(form.Close);
        }

        /// <summary>
        /// Raised before the form is about to close.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;
            if (opened)
            {
                opened = false;
                ThreadPool.QueueUserWorkItem(o => InterfaceClosed.Raise(this));
            }
        }

        /// <summary>
        /// Raised when the form is disposed.
        /// Cleans up any resources being used.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GraphicsForm_Disposed(object sender, EventArgs e)
        {
            Application.ExitThread();
            Dispose();
        }

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <param name="disposing">Indicates if managed resources should be disposed in addition to unmanaged resources; otherwise, only
        /// unmanaged resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (form.InvokeRequired)
                {
                    ApplicationInvoke(() => Dispose(disposing));
                    return;
                }

                if (form != null)
                    form.Dispose();

                foreach (AnimatedImage<BitmapFrame> image in images.InitializedValues)
                    image.Dispose();

                if (graphics != null)
                    graphics.Dispose();
                if (bufferedGraphics != null)
                    bufferedGraphics.Dispose();
                if (bufferedGraphicsContext != null)
                    bufferedGraphicsContext.Dispose();
                if (windowIcon != null)
                    windowIcon.Dispose();

                foreach (WinFormContextMenu contextMenu in contextMenus)
                    contextMenu.Dispose();

                preUpdateInvalidRegion.Dispose();
                postUpdateInvalidRegion.Dispose();

                identityMatrix.Dispose();
                font.Dispose();
                whiteBrush.Dispose();
                blackBrush.Dispose();
                blackPen.Dispose();
            }
        }
    }
}