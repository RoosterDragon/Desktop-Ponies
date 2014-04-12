namespace DesktopSprites.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
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
    /// Creates a single window used as a canvas on which sprites are drawn. Using one window as a canvas gives reasonably scalable
    /// performance (for CPU side graphics, at least). There is no overhead in modifying the collection of sprites to be drawn each call.
    /// There is an overhead for maintaining a window that covers the whole drawing area. This will be negligible on most systems, but is
    /// quite costly when running in a virtual machine as the whole surface must be transmitted over the wire. However, in return the
    /// overhead for each additional sprite is very low.
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
            /// <param name="displayBounds">The initial display boundary of the form.</param>
            public GraphicsForm(Rectangle displayBounds)
            {
                // Create the form.
                Name = GetType().Name;
                Text = "WinForm Sprite Window";
                FormBorderStyle = FormBorderStyle.None;
                AutoScaleMode = AutoScaleMode.None;
                Visible = false;
                DesktopBounds = displayBounds;

                // Tell the OS we'll be handling the drawing.
                // This causes the OnPaint and OnPaintBackground events to be raised for us.
                SetStyle(ControlStyles.UserPaint, true);
                // Prevents the OS from raising the OnPaintBackground event on us, which prevents flicker.
                // This event will now only occur when we raise it.
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);

                // Force creation of the window handle, so properties may be altered before the form is shown.
                if (!this.IsHandleCreated)
                    CreateHandle();
            }

            /// <summary>
            /// Sets the interpolation mode of the background graphics buffer to nearest neighbor whenever it is recreated.
            /// </summary>
            /// <param name="e">Data about the event.</param>
            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                BackgroundGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
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
            private readonly WinFormSpriteInterface owner;
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
                            RemoveQueuedClickHandler();
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
                        RemoveQueuedClickHandler();

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
            /// Removes the queued activated method handler from the item click event.
            /// </summary>
            private void RemoveQueuedClickHandler()
            {
                if (queuedActivatedMethod != null)
                    item.Click -= queuedActivatedMethod;
            }

            /// <summary>
            /// Releases all resources used by the
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenuItem"/> object.
            /// </summary>
            public void Dispose()
            {
                RemoveQueuedClickHandler();

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

                    // An internal WinForms class, ToolStripScrollButton, is not disposed correctly when you dispose of the control and it
                    // must then be finalized. Unfortunately this keeps the parent class alive which in turn keeps the whole interface
                    // alive since we have a reference to it! This means all the cached images are kept around and absorb managed memory.
                    // To rectify this, we will explicitly break our references so the finalizer thread isn't keeping stuff alive.
                    owner = null;
                    Items = null;
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
                Argument.EnsureNotNull(menuItem, "menuItem");
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

        #region ImageData and ImageFrame classes
        /// <summary>
        /// Contains either an image stored as a GDI+ bitmap, or as an indexed array and color palette.
        /// </summary>
        private sealed class ImageData : Disposable
        {
            /// <summary>
            /// Gets the GDI+ bitmap of the image. This will be null if the image is instead made up of an indexed array and color palette.
            /// </summary>
            public Bitmap Bitmap { get; private set; }
            /// <summary>
            /// Gets the indexed array of image data. Each value refers to an index in the color palette. This will be null if the image is
            /// instead made up of a bitmap.
            /// </summary>
            public byte[] Data { get; private set; }
            /// <summary>
            /// Gets the array containing packed ARGB colors that define the color palette of the image. This will be null if the image is
            /// instead made up of a bitmap.
            /// </summary>
            public int[] ArgbPalette { get; private set; }
            /// <summary>
            /// Gets the width of the image, in pixels.
            /// </summary>
            public int Width { get; private set; }
            /// <summary>
            /// Gets the height of the image, in pixels.
            /// </summary>
            public int Height { get; private set; }
            /// <summary>
            /// Gets the stride width of the image, in bytes.
            /// </summary>
            public int Stride { get; private set; }
            /// <summary>
            /// Gets the bit depth of the image, either 8bbp or 4bbp. Does not apply for bitmaps.
            /// </summary>
            public byte Depth { get; private set; }
            /// <summary>
            /// A hash code for the image.
            /// </summary>
            private readonly int hashCode;
            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageData"/> class by
            /// loading a GDI+ bitmap from file.
            /// </summary>
            /// <param name="path">The path to an image to be loaded.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.</exception>
            public ImageData(string path)
            {
                Argument.EnsureNotNull(path, "path");
                Bitmap = new Bitmap(path);
                Width = Bitmap.Width;
                Height = Bitmap.Height;
                this.hashCode = path.GetHashCode();
            }
            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageData"/> class by
            /// creating a new image from a raw buffer and color palette.
            /// </summary>
            /// <param name="data">The 4bbp or 8bbp array of color palette indexes to use.</param>
            /// <param name="palette">The source color palette to use.</param>
            /// <param name="transparentIndex">The index in the source palette that should be replaced with a transparent color, or null if
            /// there is no transparent color in the image.</param>
            /// <param name="stride">The stride width of the data buffer, in bytes.</param>
            /// <param name="width">The width of the image, in pixels.</param>
            /// <param name="height">The height of the image, in pixels.</param>
            /// <param name="depth">The bit depth of the image, only 8bbp and 4bbp are supported.</param>
            /// <param name="paletteCache">The cache of color palettes, so that the new palette can be shared among images.</param>
            public ImageData(byte[] data, RgbColor[] palette, byte? transparentIndex,
                int stride, int width, int height, byte depth, Dictionary<int[], int[]> paletteCache)
            {
                Data = data;
                Stride = stride;
                Width = width;
                Height = height;
                Depth = depth;
                ArgbPalette = new int[palette.Length];
                for (int i = 0; i < ArgbPalette.Length; i++)
                    ArgbPalette[i] = new ArgbColor(255, palette[i]).ToArgb();
                if (transparentIndex != null)
                    ArgbPalette[transparentIndex.Value] = new ArgbColor().ToArgb();
                lock (paletteCache)
                    ArgbPalette = paletteCache.GetOrAdd(ArgbPalette, ArgbPalette);
                hashCode = GifImage.GetHash(data, palette, transparentIndex, width, height);
            }
            /// <summary>
            /// Returns a hash code for this <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageData"/> class.
            /// </summary>
            /// <returns>An integer value that specifies the hash code for this
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageData"/> instance.</returns>
            public override int GetHashCode()
            {
                return hashCode;
            }
            /// <summary>
            /// Cleans up any resources being used.
            /// </summary>
            /// <param name="disposing">Indicates if managed resources should be disposed in addition to unmanaged resources; otherwise,
            /// only unmanaged resources should be disposed.</param>
            protected override void Dispose(bool disposing)
            {
                if (Bitmap != null)
                    Bitmap.Dispose();
            }
        }

        /// <summary>
        /// Defines a <see cref="T:DesktopSprites.SpriteManagement.Frame`1"/> whose underlying image is an
        /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageData"/> instance.
        /// </summary>
        private sealed class ImageFrame : Frame<ImageData>, IDisposable
        {
            /// <summary>
            /// Represents the allowable set of depths that can be used when generating a
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/>.
            /// </summary>
            public const BitDepths AllowableBitDepths = BitDepths.Indexed4Bpp | BitDepths.Indexed8Bpp;

            /// <summary>
            /// Gets the dimensions of the frame.
            /// </summary>
            public override Size Size
            {
                get { return new Size(Image.Width, Image.Height); }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/> class.
            /// </summary>
            /// <param name="imageData">The image data to use.</param>
            public ImageFrame(ImageData imageData)
                : base(imageData)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/> class.
            /// </summary>
            /// <param name="path">A path to the image to load. A GDI+ bitmap will be loaded from this file.</param>
            public ImageFrame(string path)
                : base(new ImageData(path))
            {
            }

            /// <summary>
            /// Gets the hash code of the frame image.
            /// </summary>
            /// <returns>A hash code for this frame image.</returns>
            public override int GetFrameHashCode()
            {
                return Image.GetHashCode();
            }

            /// <summary>
            /// Releases all resources used by the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/>
            /// object.
            /// </summary>
            public void Dispose()
            {
                Image.Dispose();
            }
        }
        #endregion

        #region ArgbPaletteEqualityComparer class
        /// <summary>
        /// Compares arrays on 32-bit integers being used as ARGB color palettes for equality.
        /// </summary>
        private sealed class ArgbPaletteEqualityComparer : IEqualityComparer<int[]>
        {
            /// <summary>
            /// Gets the comparer instance.
            /// </summary>
            public static readonly ArgbPaletteEqualityComparer Instance = new ArgbPaletteEqualityComparer();
            /// <summary>
            /// Prevents a default instance of the
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ArgbPaletteEqualityComparer"/> class from being
            /// created.
            /// </summary>
            private ArgbPaletteEqualityComparer()
            {
            }
            /// <summary>
            /// Determines whether the specified arrays are structurally equal. Both arrays are assumed not null.
            /// </summary>
            /// <param name="x">The first array to compare.</param>
            /// <param name="y">The second array to compare.</param>
            /// <returns>Returns true if the specified arrays are structurally equal; otherwise, false.</returns>
            public bool Equals(int[] x, int[] y)
            {
                if (x.Length != y.Length)
                    return false;
                for (int i = 0; i < x.Length; i++)
                    if (x[i] != y[i])
                        return false;
                return true;
            }
            /// <summary>
            /// Returns a hash code for the specified array based on the elements it contains. The array is assumed not null.
            /// </summary>
            /// <param name="obj">The array for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified array.</returns>
            public int GetHashCode(int[] obj)
            {
                const int OffsetBasis32 = unchecked((int)2166136261);
                const int FnvPrime32 = 16777619;
                int hash = OffsetBasis32;
                foreach (int i in obj)
                {
                    hash ^= (i >> 24) & 0xFF;
                    hash *= FnvPrime32;
                    hash ^= (i >> 16) & 0xFF;
                    hash *= FnvPrime32;
                    hash ^= (i >> 8) & 0xFF;
                    hash *= FnvPrime32;
                    hash ^= (i >> 0) & 0xFF;
                    hash *= FnvPrime32;
                }
                return hash;
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
        /// Indicates if the form has been requested to close.
        /// </summary>
        private volatile bool closePending;
        /// <summary>
        /// Indicates if the form should be prevented from closing itself for the moment.
        /// </summary>
        private volatile bool preventSelfClose;
        /// <summary>
        /// Stores the images for each sprite as a series of
        /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/>, indexed by filename.
        /// </summary>
        private readonly Dictionary<string, AnimatedImage<ImageFrame>> images =
            new Dictionary<string, AnimatedImage<ImageFrame>>(SpriteImagePaths.Comparer);
        /// <summary>
        /// Stores the animation pairs to use when drawing sprites, indexed by their path pairs.
        /// </summary>
        private readonly Dictionary<SpriteImagePaths, AnimationPair<ImageFrame>> animationPairsByPaths =
            new Dictionary<SpriteImagePaths, AnimationPair<ImageFrame>>();
        /// <summary>
        /// Delegate to the CreatePair function for use in single-threaded calls only.
        /// </summary>
        private readonly Func<SpriteImagePaths, AnimationPair<ImageFrame>> generatePair;
        /// <summary>
        /// Cache of color palettes so that memory can be shared.
        /// </summary>
        private readonly Dictionary<int[], int[]> paletteCache = new Dictionary<int[], int[]>(ArgbPaletteEqualityComparer.Instance);

        /// <summary>
        /// The underlying form on which graphics are displayed.
        /// </summary>
        private GraphicsForm form;
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
        private ICollection<ISprite> sprites;
        /// <summary>
        /// Synchronizes access to the sprites field.
        /// </summary>
        private readonly object spritesGuard = new object();

        /// <summary>
        /// Number of additional threads used for parallel blending operations.
        /// </summary>
        private static readonly int ParallelBlendThreads = Environment.ProcessorCount - 1;
        /// <summary>
        /// Number of threads total used for parallel blending operations (additional threads plus the main thread).
        /// </summary>
        private static readonly int ParallelBlendTotalSections = ParallelBlendThreads + 1;
        /// <summary>
        /// The current image to be blended in parallel.
        /// </summary>
        private ImageData parallelBlendImage;
        /// <summary>
        /// The current area to be blended in parallel.
        /// </summary>
        private Rectangle parallelBlendArea;
        /// <summary>
        /// Indicates if the current image to be blended in parallel should be mirrored horizontally.
        /// </summary>
        private bool parallelBlendMirror;
        /// <summary>
        /// The current number of blending sections.
        /// </summary>
        private int parallelBlendSections;
        /// <summary>
        /// A pointer to the current buffer from the form background.
        /// </summary>
        private unsafe int* backgroundData;
        /// <summary>
        /// Synchronization object for parallel rendering threads.
        /// </summary>
        private readonly Barrier parallelBlend;

        /// <summary>
        /// List of <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.WinFormContextMenu"/> which have been created by
        /// the interface.
        /// </summary>
        private readonly LinkedList<WinFormContextMenu> contextMenus = new LinkedList<WinFormContextMenu>();
        /// <summary>
        /// The full <see cref="T:System.Drawing.Font"/> definition to be used when drawing text to the screen.
        /// </summary>
        private readonly Font font;
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
        /// Sets the preventSelfClose variable to true.
        /// </summary>
        private readonly MethodInvoker setPreventSelfCloseTrue;
        /// <summary>
        /// Sets the preventSelfClose variable to false, and if a close is pending closes the form.
        /// </summary>
        private readonly MethodInvoker setPreventSelfCloseFalse;

        /// <summary>
        /// Gets or sets a value indicating whether the interface will be allowed to close itself. If the interface needs to close itself,
        /// it will attempt to do so as soon as this property is set to true.
        /// </summary>
        public bool PreventSelfClose
        {
            get
            {
                return preventSelfClose;
            }
            set
            {
                if (preventSelfClose != value)
                    if (value)
                        ApplicationInvoke(setPreventSelfCloseTrue);
                    else
                        ApplicationInvoke(setPreventSelfCloseFalse);
            }
        }
        /// <summary>
        /// Sets a value indicating whether the interface will be allowed to close itself.
        /// </summary>
        /// <param name="preventSelfClose">A value indicating whether the interface will be allowed to close itself.</param>
        private void SetPreventSelfClose(bool preventSelfClose)
        {
            ApplicationInvoke(() =>
            {
                if (!(this.preventSelfClose = preventSelfClose) && closePending)
                    Close();
            });
        }
        /// <summary>
        /// Gets or sets the text associated with the form.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public string WindowTitle
        {
            get
            {
                EnsureNotDisposed();
                return form.Text;
            }
            set
            {
                EnsureNotDisposed();
                ApplicationInvoke(() => form.Text = value);
            }
        }
        /// <summary>
        /// Gets or sets the icon for the form.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public string WindowIconFilePath
        {
            get
            {
                EnsureNotDisposed();
                return windowIconFilePath;
            }
            set
            {
                EnsureNotDisposed();
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
        /// Gets or sets a value indicating whether the form should be displayed as a topmost form. This will cause a momentary graphical
        /// stutter if the form has already been opened and alpha blending is disabled. If the form is not paused, this will not take
        /// effect until Draw is called.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public bool Topmost
        {
            get
            {
                EnsureNotDisposed();
                return form.TopMost;
            }
            set
            {
                EnsureNotDisposed();
                if (form.TopMost != value)
                    SetTopmost(value);
            }
        }
        /// <summary>
        /// Sets a value indicating whether the form should be displayed as a topmost form.
        /// </summary>
        /// <param name="topmost">A value indicating whether the form should be displayed as a topmost form.</param>
        private void SetTopmost(bool topmost)
        {
            ApplicationInvoke(() => form.TopMost = topmost);
        }
        /// <summary>
        /// Gets or sets a value indicating whether the form is displayed in the taskbar.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public bool ShowInTaskbar
        {
            get
            {
                EnsureNotDisposed();
                return form.ShowInTaskbar;
            }
            set
            {
                EnsureNotDisposed();
                if (form.ShowInTaskbar != value)
                    SetShowInTaskbar(value);
            }
        }
        /// <summary>
        /// Sets a value indicating whether the form is displayed in the taskbar.
        /// </summary>
        /// <param name="showInTaskbar">A value indicating whether the form should be displayed in the taskbar.</param>
        private void SetShowInTaskbar(bool showInTaskbar)
        {
            ApplicationInvoke(() => form.ShowInTaskbar = showInTaskbar);
        }
        /// <summary>
        /// Gets or sets the display boundary of the form. When setting, the form will be cleared of any drawn sprites.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public Rectangle DisplayBounds
        {
            get
            {
                EnsureNotDisposed();
                return form.DesktopBounds;
            }
            set
            {
                EnsureNotDisposed();
                if (form.DesktopBounds != value)
                    SetDisplayBounds(value);
            }
        }
        /// <summary>
        /// Sets the display boundary of the form.
        /// </summary>
        /// <param name="displayBounds">The display boundary to use.</param>
        private void SetDisplayBounds(Rectangle displayBounds)
        {
            ApplicationInvoke(() => form.DesktopBounds = displayBounds);
        }
        /// <summary>
        /// Gets the current location of the cursor.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public Point CursorPosition
        {
            get
            {
                EnsureNotDisposed();
                return Control.MousePosition;
            }
        }
        /// <summary>
        /// Gets the mouse buttons which are currently held down.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public SimpleMouseButtons MouseButtonsDown
        {
            get
            {
                EnsureNotDisposed();
                return GetButtonsFromNative(Control.MouseButtons);
            }
        }
        /// <summary>
        /// Gets a value indicating whether the interface has input focus.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public bool HasFocus
        {
            get
            {
                EnsureNotDisposed();
                return Form.ActiveForm == form;
            }
        }
        /// <summary>
        /// Gets or sets an optional function that pre-processes a decoded GIF buffer before the buffer is used by the viewer.
        /// </summary>
        public BufferPreprocess BufferPreprocess { get; set; }
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
            if ((buttons & MouseButtons.Left) == MouseButtons.Left)
                simpleButtons |= SimpleMouseButtons.Left;
            if ((buttons & MouseButtons.Right) == MouseButtons.Right)
                simpleButtons |= SimpleMouseButtons.Right;
            if ((buttons & MouseButtons.Middle) == MouseButtons.Middle)
                simpleButtons |= SimpleMouseButtons.Middle;
            return simpleButtons;
        }
        /// <summary>
        /// Raised when a mouse button has been clicked.
        /// Raises the MouseClick event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GraphicsForm_MouseClick(object sender, MouseEventArgs e)
        {
            var button = GetButtonsFromNative(e.Button);
            if (button == SimpleMouseButtons.None)
                return;
            MouseClick.Raise(this, () => new SimpleMouseEventArgs(button, e.X + form.Left, e.Y + form.Top));
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
        /// Occurs when the window is clicked by the mouse.
        /// </summary>
        public event EventHandler<SimpleMouseEventArgs> MouseClick;
        /// <summary>
        /// Occurs when the interface gains input focus.
        /// </summary>
        public event EventHandler Focused;
        /// <summary>
        /// Occurs when the interface loses input focus.
        /// </summary>
        public event EventHandler Unfocused;
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
        public WinFormSpriteInterface(Rectangle displayBounds)
        {
            Thread appThread = new Thread(ApplicationRun) { Name = "WinFormSpriteInterface.ApplicationRun" };
            appThread.SetApartmentState(ApartmentState.STA);
            object appInitiated = new object();
            lock (appInitiated)
            {
                appThread.Start(Tuple.Create(appInitiated, displayBounds));

                // Do other initialization in parallel whilst the UI thread is spinning up.
                generatePair = paths => CreatePair(paths, null);
                render = Render;
                setPreventSelfCloseTrue = () => preventSelfClose = true;
                setPreventSelfCloseFalse = () =>
                {
                    preventSelfClose = false;
                    if (closePending)
                        Close();
                };
                using (var family = FontFamily.GenericSansSerif)
                    font = new Font(family, 12, GraphicsUnit.Pixel);
                postUpdateInvalidRegion.MakeEmpty();
                if (ParallelBlendThreads > 0)
                {
                    parallelBlend = new Barrier(ParallelBlendTotalSections);
                    for (int i = 0; i < ParallelBlendThreads; i++)
                        new Thread(AlphaBlendWorker) { Name = "WinFormSpriteInterface.AlphaBlendWorker" }.Start(i);
                }

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

            form = new GraphicsForm(parameters.Item2);
            form.MouseClick += GraphicsForm_MouseClick;
            form.KeyPress += GraphicsForm_KeyPress;
            form.Activated += (sender, e) => Focused.Raise(this);
            form.Deactivate += (sender, e) => Unfocused.Raise(this);
            form.FormClosing += GraphicsForm_FormClosing;
            form.Disposed += GraphicsForm_Disposed;

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
            form.SmartInvoke(method);
        }

        /// <summary>
        /// Creates a new <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/> from the given file, loading
        /// extra transparency information and adjusting the colors as required by the transparency.
        /// </summary>
        /// <param name="fileName">The path to a static image file from which to create a new
        /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/>.</param>
        /// <returns>A new <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/> created from the given file.
        /// </returns>
        private ImageFrame ImageFrameFromFile(string fileName)
        {
            return Disposable.SetupSafely(new ImageFrame(fileName), frame =>
            {
                // Check for an alpha remapping table, and apply it if one exists.
                string mapFilePath = Path.ChangeExtension(fileName, AlphaRemappingTable.FileExtension);
                if (File.Exists(mapFilePath))
                {
                    AlphaRemappingTable map = new AlphaRemappingTable();
                    map.LoadMap(mapFilePath);
                    frame.Image.Bitmap.RemapColors(map.GetMap().ToDictionary(
                        kvp => Color.FromArgb(kvp.Key.ToArgb()), kvp => Color.FromArgb(kvp.Value.ToArgb())));
                }
                frame.Image.Bitmap.PremultiplyAlpha();
            });
        }

        /// <summary>
        /// Creates a new <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/> from the raw buffer, loading
        /// extra transparency information and adjusting the colors as required by the transparency.
        /// </summary>
        /// <param name="buffer">The raw buffer.</param>
        /// <param name="palette">The color palette.</param>
        /// <param name="transparentIndex">The index of the transparent color.</param>
        /// <param name="stride">The stride width of the buffer.</param>
        /// <param name="width">The logical width of the buffer.</param>
        /// <param name="height">The logical height of the buffer.</param>
        /// <param name="depth">The bit depth of the buffer.</param>
        /// <param name="fileName">The path to the GIF file being loaded.</param>
        /// <returns>A new <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/> for the frame held in the raw
        /// buffer.</returns>
        private ImageFrame ImageFrameFromBuffer(byte[] buffer, RgbColor[] palette, byte? transparentIndex,
            int stride, int width, int height, byte depth, string fileName)
        {
            if (BufferPreprocess != null)
                BufferPreprocess(ref buffer, ref palette, ref transparentIndex, ref stride, ref width, ref height, ref depth);
            return Disposable.SetupSafely(
                new ImageFrame(new ImageData(buffer, palette, transparentIndex, stride, width, height, depth, paletteCache)),
                frame =>
                {
                    // Check for an alpha remapping table, and apply it if one exists.
                    string mapFilePath = Path.ChangeExtension(fileName, AlphaRemappingTable.FileExtension);
                    if (File.Exists(mapFilePath))
                    {
                        AlphaRemappingTable map = new AlphaRemappingTable();
                        map.LoadMap(mapFilePath);
                        var colorPalette = frame.Image.ArgbPalette;
                        for (int i = 0; i < colorPalette.Length; i++)
                        {
                            ArgbColor paletteColor = new ArgbColor(colorPalette[i]);
                            if (paletteColor.A != 255)
                                continue;
                            ArgbColor argbColor;
                            if (map.TryGetMapping(new RgbColor(paletteColor.R, paletteColor.G, paletteColor.B), out argbColor))
                                colorPalette[i] = argbColor.PremultipliedAlpha().ToArgb();
                        }
                    }
                });
        }

        /// <summary>
        /// Loads the given collection of file paths as images in a format that this
        /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> can display.
        /// </summary>
        /// <param name="imageFilePaths">The collection of paths to image files that should be loaded by the interface. Any images not
        /// loaded by this method will be loaded on demand.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="imageFilePaths"/> is null.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void LoadImages(IEnumerable<SpriteImagePaths> imageFilePaths)
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
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        /// <exception cref="T:System.AggregateException">One or more images failed to load.</exception>
        public void LoadImages(IEnumerable<SpriteImagePaths> imageFilePaths, EventHandler imageLoadedHandler)
        {
            Argument.EnsureNotNull(imageFilePaths, "imageFilePaths");
            EnsureNotDisposed();

            object syncObject = new object();
            int remaining = 0;
            imageLoadedHandler += (sender, e) =>
                {
                    if (--remaining == 0)
                        lock (syncObject)
                            Monitor.Pulse(syncObject);
                };

            var badPaths = new HashSet<string>(PathEquality.Comparer);
            List<Exception> exceptions = null;
            lock (syncObject)
            {
                lock (imageLoadedHandler)
                    foreach (var paths in imageFilePaths)
                    {
                        remaining++;
                        ThreadPool.QueueUserWorkItem(o =>
                        {
                            try
                            {
                                LoadPaths(paths, imageLoadedHandler, badPaths);
                            }
                            catch (Exception ex)
                            {
                                lock (badPaths)
                                {
                                    if (exceptions == null)
                                        exceptions = new List<Exception>();
                                    exceptions.Add(ex);
                                }
                            }
                        });
                    }
                if (remaining > 0)
                    Monitor.Wait(syncObject);
            }
            if (exceptions != null)
                throw new AggregateException("One or more images did not load successfully.", exceptions);
        }

        /// <summary>
        /// Ensures an animation pair exists for the given paths.
        /// </summary>
        /// <param name="paths">A pair of paths for which an animation pair should be created, if one does not yet exists.</param>
        /// <param name="imageLoadedHandler">An event handler to raise unconditionally at the end of the method.</param>
        /// <param name="badPaths">When this method is being called from multiple threads, this should be an initially empty collection
        /// which compares based on the path equality of the file system that the caller provides between all intended callees. If
        /// provided, this method will maintain the collection, the caller need not do anything. If null, this collection is ignored and
        /// the method must be called from a single thread.</param>
        private void LoadPaths(SpriteImagePaths paths, EventHandler imageLoadedHandler, HashSet<string> badPaths)
        {
            bool needPair = true;
            lock (animationPairsByPaths)
                if (animationPairsByPaths.ContainsKey(paths))
                    needPair = false;
                else
                    animationPairsByPaths.Add(paths, new AnimationPair<ImageFrame>());
            try
            {
                if (needPair)
                {
                    var pair = CreatePair(paths, badPaths);
                    lock (animationPairsByPaths)
                        animationPairsByPaths[paths] = pair;
                }
            }
            catch (Exception)
            {
                animationPairsByPaths.Remove(paths);
                throw;
            }
            finally
            {
                lock (imageLoadedHandler)
                    imageLoadedHandler.Raise(this);
            }
        }

        /// <summary>
        /// Creates an animation pair for a specified pair of paths.
        /// </summary>
        /// <param name="paths">The paths for which an animation pair should be generated. Where possible, the animations within a pair
        /// will reuse images for efficiency.</param>
        /// <param name="badPaths">When this method is being called from multiple threads, this should be an initially empty collection
        /// which compares based on the path equality of the file system that the caller provides between all intended callees. If
        /// provided, this method will maintain the collection, the caller need not do anything. If null, this collection is ignored and
        /// the method must be called from a single thread.</param>
        /// <returns>An animation pair for displaying the specified paths if loading succeeded. If loading was aborted due to an error this
        /// method returns a default pair instance.</returns>
        private AnimationPair<ImageFrame> CreatePair(SpriteImagePaths paths, HashSet<string> badPaths)
        {
            AnimatedImage<ImageFrame> leftImage;
            bool leftFound;
            bool badPath = false;
            lock (images)
            {
                while ((leftFound = images.TryGetValue(paths.Left, out leftImage)) && leftImage == null &&
                    (badPaths != null && !(badPath = badPaths.Contains(paths.Left))))
                    Monitor.Wait(images);
                if (!leftFound && !badPath)
                    images.Add(paths.Left, null);
            }
            if (badPath)
                return new AnimationPair<ImageFrame>();
            if (!leftFound)
            {
                try
                {
                    leftImage = CreateAnimatedImage(paths.Left);
                }
                catch (Exception)
                {
                    badPath = true;
                    if (badPaths != null)
                        lock (images)
                            badPaths.Add(paths.Left);
                    throw;
                }
                finally
                {
                    lock (images)
                    {
                        if (badPath)
                            images.Remove(paths.Left);
                        else
                            images[paths.Left] = leftImage;
                        Monitor.PulseAll(images);
                    }
                }
            }
            var leftAnimation = new Animation<ImageFrame>(leftImage);

            if (SpriteImagePaths.Comparer.Equals(paths.Left, paths.Right))
                return new AnimationPair<ImageFrame>(leftAnimation, leftAnimation);

            AnimatedImage<ImageFrame> rightImage;
            bool mirrored = false;
            bool rightFound;
            lock (images)
                while ((rightFound = images.TryGetValue(paths.Right, out rightImage)) && rightImage == null &&
                    (badPaths != null && !(badPath = badPaths.Contains(paths.Right))))
                    Monitor.Wait(images);
            if (badPath)
                return new AnimationPair<ImageFrame>();
            if (!rightFound)
            {
                try
                {
                    rightImage = CreateAnimatedImage(paths.Right);
                }
                catch (Exception)
                {
                    if (badPaths != null)
                        lock (images)
                            badPaths.Add(paths.Right);
                    throw;
                }
                mirrored = AnimationsAreHorizontallyMirrored(leftImage, rightImage);
                if (!mirrored)
                    lock (images)
                    {
                        var originalRightImage = rightImage;
                        while ((rightFound = images.TryGetValue(paths.Right, out rightImage)) && rightImage == null &&
                            (badPaths != null && !(badPath = badPaths.Contains(paths.Right))))
                            Monitor.Wait(images);
                        if (!rightFound && !badPath)
                            images.Add(paths.Right, rightImage = originalRightImage);
                    }
            }
            var rightAnimation = new Animation<ImageFrame>(mirrored ? leftImage : rightImage, mirrored);
            return new AnimationPair<ImageFrame>(leftAnimation, rightAnimation);
        }

        /// <summary>
        /// Creates an animated image by loading it from file.
        /// </summary>
        /// <param name="path">The path to the file that should be loaded.</param>
        /// <returns>A new animated image created from the specified file.</returns>
        private AnimatedImage<ImageFrame> CreateAnimatedImage(string path)
        {
            return new AnimatedImage<ImageFrame>(path, ImageFrameFromFile,
                (b, p, tI, s, w, h, d) => ImageFrameFromBuffer(b, p, tI, s, w, h, d, path), ImageFrame.AllowableBitDepths);
        }

        /// <summary>
        /// Checks whether two animated images are horizontally mirrored.
        /// </summary>
        /// <param name="left">The first image to check.</param>
        /// <param name="right">The second image to check.</param>
        /// <returns>Returns true if the animated images are the horizontal mirror of each other.</returns>
        private static bool AnimationsAreHorizontallyMirrored(AnimatedImage<ImageFrame> left, AnimatedImage<ImageFrame> right)
        {
            if (left.Size != right.Size || left.LoopCount != right.LoopCount ||
                left.ImageDuration != right.ImageDuration || left.FrameCount != right.FrameCount ||
                left[0].Image.Bitmap != null || right[0].Image.Bitmap != null)
                return false;

            for (int frameIndex = 0; frameIndex < left.FrameCount; frameIndex++)
            {
                if (left.GetDuration(frameIndex) != right.GetDuration(frameIndex))
                    return false;
                var leftFrame = left[frameIndex].Image;
                var rightFrame = right[frameIndex].Image;
                // If as a result of the 2x downscaling the images no longer match in size, they cannot be mirror candidates.
                if (leftFrame.Depth != rightFrame.Depth || leftFrame.Width != rightFrame.Width || leftFrame.Height != rightFrame.Height)
                    return false;
                // Check for an exact horizontal mirror match by comparing pixels in each image.
                byte[] leftData = leftFrame.Data;
                byte[] rightData = rightFrame.Data;
                int[] leftPalette = leftFrame.ArgbPalette;
                int[] rightPalette = rightFrame.ArgbPalette;
                int rightMax = rightFrame.Stride - 1;
                if (leftFrame.Depth == 8)
                {
                    // If the images share a palette, we can check faster by eliding the deference of the palette and just compare indexes.
                    if (leftPalette == rightPalette)
                    {
                        for (int y = 0; y < leftFrame.Height; y++)
                        {
                            int leftRow = y * leftFrame.Stride;
                            int rightRow = y * rightFrame.Stride;
                            for (int x = 0; x < leftFrame.Width; x++)
                                if (leftData[leftRow + x] != rightData[rightRow + rightMax - x])
                                    return false;
                        }
                    }
                    else
                    {
                        for (int y = 0; y < leftFrame.Height; y++)
                        {
                            int leftRow = y * leftFrame.Stride;
                            int rightRow = y * rightFrame.Stride;
                            for (int x = 0; x < leftFrame.Width; x++)
                                if (leftPalette[leftData[leftRow + x]] != rightPalette[rightData[rightRow + rightMax - x]])
                                    return false;
                        }
                    }
                }
                else
                {
                    bool hasPadding = rightFrame.Width % 2 != 0;
                    for (int y = 0; y < leftFrame.Height; y++)
                    {
                        int leftRow = y * leftFrame.Stride;
                        int rightRow = y * rightFrame.Stride;
                        for (int x = 0; x < leftFrame.Width; x++)
                        {
                            bool xIsEven = x % 2 == 0;
                            int halfX = x / 2;
                            int rightOffset = hasPadding && !xIsEven ? 1 : 0;
                            var leftIndex = leftData[leftRow + halfX];
                            var rightIndex = rightData[rightRow + rightMax - halfX - rightOffset];
                            if (xIsEven)
                            {
                                leftIndex >>= 4;
                                if (hasPadding)
                                    rightIndex >>= 4;
                                else
                                    rightIndex &= 0xF;
                            }
                            else
                            {
                                leftIndex &= 0xF;
                                if (hasPadding)
                                    rightIndex &= 0xF;
                                else
                                    rightIndex >>= 4;
                            }
                            if (leftPalette[leftIndex] != rightPalette[rightIndex])
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Creates an <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> specific context menu for the given set of
        /// menu items.
        /// </summary>
        /// <param name="menuItems">The collections of items to be displayed in the menu.</param>
        /// <returns>An <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> specific context menu.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="menuItems"/> is null.</exception>
        public ISimpleContextMenu CreateContextMenu(IEnumerable<ISimpleContextMenuItem> menuItems)
        {
            EnsureNotDisposed();
            WinFormContextMenu menu = null;
            ApplicationInvoke(() => menu = new WinFormContextMenu(this, menuItems));
            contextMenus.AddLast(menu);
            return menu;
        }

        /// <summary>
        /// Opens the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/>.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void Open()
        {
            Show();
            opened = true;
        }

        /// <summary>
        /// Hides the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/>.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void Hide()
        {
            EnsureNotDisposed();
            ApplicationInvoke(form.Hide);
        }

        /// <summary>
        /// Shows the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/>.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void Show()
        {
            ApplicationInvoke(() =>
            {
                // TopMost interferes with initial window focus. To workaround this, we will only set it once the form has become visible.
                bool topMost = form.TopMost;
                form.TopMost = false;
                form.Show();
                form.TopMost = topMost;
            });
        }

        /// <summary>
        /// Freezes the display of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/>.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void Pause()
        {
            EnsureNotDisposed();
            paused = true;
        }

        /// <summary>
        /// Resumes display of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> from a paused state.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void Resume()
        {
            Show();
            paused = false;
        }

        /// <summary>
        /// Draws the given collection of sprites.
        /// </summary>
        /// <param name="sprites">The collection of sprites to draw.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sprites"/> is null.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void Draw(ICollection<ISprite> sprites)
        {
            Argument.EnsureNotNull(sprites, "sprites");
            EnsureNotDisposed();

            if (paused)
                return;

            lock (spritesGuard)
                this.sprites = sprites;
            ApplicationInvoke(render);
            lock (spritesGuard)
                this.sprites = null;
        }

        /// <summary>
        /// Renders the current sprites to the window.
        /// </summary>
        private void Render()
        {
            lock (spritesGuard)
            {
                if (sprites == null)
                    return;

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
                    Size timingsSize = Size.Ceiling(form.BackgroundGraphics.MeasureString(timingsInfo, font));

                    // Set location and get area of graph draw.
                    Point offset = new Point(10, 10);
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
                const int ClippingThreshold = 15;
                if (sprites.Count <= ClippingThreshold)
                {
                    foreach (ISprite sprite in sprites)
                    {
                        var invalidRect = OffsetRectangle(sprite.Region, translate);
                        invalidRect.Size += new Size(1, 1);
                        postUpdateInvalidRegion.Union(invalidRect);
                        ISpeakingSprite speakingSprite = sprite as ISpeakingSprite;
                        if (speakingSprite != null && speakingSprite.SpeechText != null)
                            postUpdateInvalidRegion.Union(
                                OffsetRectangle(GetSpeechBubbleRegion(speakingSprite, form.BackgroundGraphics), translate));
                    }
                    postUpdateInvalidRegion.Intersect(OffsetRectangle(DisplayBounds, translate));
                }
                else
                {
                    postUpdateInvalidRegion.Union(OffsetRectangle(DisplayBounds, translate));
                }

                // Determine the current clipping area required, and clear it of old graphics.
                form.BackgroundGraphics.SetClip(preUpdateInvalidRegion, CombineMode.Replace);
                form.BackgroundGraphics.Clear(Color.FromArgb(0));

                // Set the clipping area to the region we'll be drawing in for this frame.
                form.BackgroundGraphics.SetClip(postUpdateInvalidRegion, CombineMode.Replace);

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
                        form.BackgroundGraphics.DrawRectangle(Pens.Blue, invalidRectangle);
                    }
                }
                #endregion

                // When rendering a large number of sprites, we will use multiple threads to improve the frame rate. When the number of
                // sprites is small we will render using only a single thread to reduce CPU by eliminating parallelization overhead.
                const int ParallelBlendingThreshold = 75;
                // Draw all the sprites.
                foreach (ISprite sprite in sprites)
                {
                    // Draw the sprite image.
                    var imagePath = sprite.FacingRight ? sprite.ImagePaths.Right : sprite.ImagePaths.Left;
                    if (imagePath != null)
                    {
                        var area = OffsetRectangle(sprite.Region, translate);
                        var pair = animationPairsByPaths.GetOrAdd(sprite.ImagePaths, generatePair);
                        var animation = sprite.FacingRight ? pair.Right : pair.Left;
                        var frame = animation.Image[sprite.ImageTimeIndex];
                        unsafe
                        {
                            backgroundData = form.GetBackgroundData();
                        }
                        if (frame.Image.Bitmap != null)
                            form.BackgroundGraphics.DrawImage(frame.Image.Bitmap, area);
                        else if (sprites.Count <= ParallelBlendingThreshold)
                            AlphaBlend(frame.Image, area, animation.Mirror);
                        else
                            AlphaBlendParallel(frame.Image, area, animation.Mirror);
                    }

                    // Draw a speech bubble for a speaking sprite.
                    ISpeakingSprite speakingSprite = sprite as ISpeakingSprite;
                    if (speakingSprite != null && speakingSprite.SpeechText != null)
                    {
                        Rectangle bubble = OffsetRectangle(GetSpeechBubbleRegion(speakingSprite, form.BackgroundGraphics), translate);
                        form.BackgroundGraphics.FillRectangle(Brushes.White,
                            bubble.X + 1, bubble.Y + 1, bubble.Width - 2, bubble.Height - 2);
                        form.BackgroundGraphics.DrawRectangle(Pens.Black,
                            bubble.X, bubble.Y, bubble.Width - 1, bubble.Height - 1);
                        form.BackgroundGraphics.DrawString(speakingSprite.SpeechText, font, Brushes.Black, bubble.Location);
                    }
                }

                #region Timings Graph
                if (Collector != null && ShowPerformanceGraph && Collector.Count != 0)
                {
                    // Display a graph of frame times and garbage collections.
                    Collector.DrawGraph(form.BackgroundGraphics);

                    // Display how long this frame took.
                    form.BackgroundGraphics.FillRectangle(Brushes.Black, timingsArea);
                    form.BackgroundGraphics.DrawString(timingsInfo, font, Brushes.White, timingsArea.Left, timingsArea.Top);
                }
                #endregion
            }

            // Render the result.
            form.UpdateBackgroundGraphics();
        }

        /// <summary>
        /// Draws, with alpha blending, the specified image within the specified rectangle onto the form. Scaling is done using nearest
        /// neighbor interpolation. Drawing is done in parallel across multiple threads.
        /// </summary>
        /// <param name="image">An image specified by a byte array and color palette to be alpha blended onto the form surface.</param>
        /// <param name="area">The area on the form onto which the image should be drawn.</param>
        /// <param name="mirror">Indicates if the image should be horizontally mirrored when drawing.</param>
        private unsafe void AlphaBlendParallel(ImageData image, Rectangle area, bool mirror)
        {
            // Publish current image details.
            parallelBlendImage = image;
            parallelBlendArea = area;
            parallelBlendMirror = mirror;
            parallelBlendSections = Math.Min(ParallelBlendTotalSections, area.Height);
            Thread.MemoryBarrier();
            // Start work.
            parallelBlend.SignalAndWait();
            // The main thread is responsible for blending the last section.
            AlphaBlend(parallelBlendImage, parallelBlendArea, mirror, parallelBlendSections - 1, parallelBlendSections);
            // Wait for work to finish.
            parallelBlend.SignalAndWait();
        }

        /// <summary>
        /// Method to be executed on parallel blending threads to execute blending of certain image sections.
        /// </summary>
        /// <param name="sectionObject">The section number this thread is responsible for blending.</param>
        private void AlphaBlendWorker(object sectionObject)
        {
            // Update cache to ensure we get the initialized synchronization object.
            Thread.MemoryBarrier();
            int section = (int)sectionObject;
            while (true)
            {
                // Wait for work or signal to exit.
                parallelBlend.SignalAndWait();
                if (Disposed)
                    return;
                // Update cache of current work and do it.
                Thread.MemoryBarrier();
                if (section < parallelBlendSections - 1)
                    AlphaBlend(parallelBlendImage, parallelBlendArea, parallelBlendMirror, section, parallelBlendSections);
                // Signal work is complete.
                parallelBlend.SignalAndWait();
            }
        }

        /// <summary>
        /// Draws, with alpha blending, the specified image within the specified rectangle onto the form. Scaling is done using nearest
        /// neighbor interpolation.
        /// </summary>
        /// <param name="image">An image specified by a byte array and color palette to be alpha blended onto the form surface.</param>
        /// <param name="area">The area on the form onto which the image should be drawn.</param>
        /// <param name="mirror">Indicates if the image should be horizontally mirrored when drawing.</param>
        private unsafe void AlphaBlend(ImageData image, Rectangle area, bool mirror)
        {
            AlphaBlend(image, area, mirror, 0, 1);
        }

        /// <summary>
        /// Draws, with alpha blending, the specified image within the specified rectangle onto the form. Scaling is done using nearest
        /// neighbor interpolation.
        /// </summary>
        /// <param name="image">An image specified by a byte array and color palette to be alpha blended onto the form surface.</param>
        /// <param name="area">The area on the form onto which the image should be drawn.</param>
        /// <param name="mirror">Indicates if the image should be horizontally mirrored when drawing.</param>
        /// <param name="section">The scanline section to render. Only this portion will be rendered.</param>
        /// <param name="sectionCount">The number of scanline sections.</param>
        private unsafe void AlphaBlend(ImageData image, Rectangle area, bool mirror, int section, int sectionCount)
        {
            if (area.Width <= 0 || area.Height <= 0)
                return;

            // Check if we can use the non-scaling methods for a performance boost.
            // Note the code duplication in these methods is required: they are on the hot path and we need to avoid function call
            // overhead. The methods are too long for the jitter to consider inlining them, so this is done manually.
            if (image.Width == area.Width && image.Height == area.Height)
            {
                if (image.Depth == 8)
                    AlphaBlend8bbpUnscaled(image, area.Location, mirror, section, sectionCount);
                else
                    AlphaBlend4bbpUnscaled(image, area.Location, mirror, section, sectionCount);
            }
            else
            {
                if (image.Depth == 8)
                    AlphaBlend8bbp(image, area, mirror, section, sectionCount);
                else
                    AlphaBlend4bbp(image, area, mirror, section, sectionCount);
            }
        }

        /// <summary>
        /// Initializes some common alpha blending parameters.
        /// </summary>
        /// <param name="location">The desired location of the image to draw.</param>
        /// <param name="size">The desired size of the image to draw.</param>
        /// <param name="mirror">Indicates if the image should be horizontally mirrored when drawing.</param>
        /// <param name="section">The scanline section to render. Only this portion will be rendered.</param>
        /// <param name="sectionCount">The number of scanline sections.</param>
        /// <param name="xMin">When this methods returns, contains the minimum x-coordinate in scaled image space.</param>
        /// <param name="xMax">When this methods returns, contains the maximum x-coordinate in scaled image space.</param>
        /// <param name="yMin">When this methods returns, contains the minimum y-coordinate in scaled image space.</param>
        /// <param name="yMax">When this methods returns, contains the maximum y-coordinate in scaled image space.</param>
        /// <param name="backgroundIndex">When this methods returns, contains the initial index into the form background data buffer.
        /// </param>
        /// <param name="backgroundIndexChange">When this methods returns, contains the value to add to the backgroundIndex as a scanline
        /// is iterated.</param>
        /// <param name="backgroundIndexRowChange">When this methods returns, contains the value to add to the backgroundIndex after each
        /// image scanline is iterated.</param>
        private void AlphaBlendInitialize(Point location, Size size, bool mirror, int section, int sectionCount,
            out int xMin, out int xMax, out int yMin, out int yMax,
            out int backgroundIndex, out int backgroundIndexChange, out int backgroundIndexRowChange)
        {
            xMin = Math.Max(0, -location.X);
            xMax = Math.Min(size.Width, form.Width - location.X);
            yMin = Math.Max(0, -location.Y);
            yMax = Math.Min(size.Height, form.Height - location.Y);

            int sectionSize = (yMax - yMin) / sectionCount;
            yMin = yMin + section * sectionSize;
            if (section + 1 != sectionCount)
                yMax = yMin + sectionSize;

            backgroundIndex = (location.Y + yMin) * form.Width + location.X + (mirror ? (xMax - 1) : xMin);
            backgroundIndexChange = mirror ? -1 : 1;
            backgroundIndexRowChange = mirror ? form.Width + xMax - xMin : form.Width - xMax + xMin;

            if (mirror)
            {
                if (location.X < 0)
                {
                    xMin += location.X;
                    xMax += location.X;
                }
                var rightAdjustment = size.Width - (form.Width - location.X);
                if (rightAdjustment > 0)
                {
                    xMin += rightAdjustment;
                    xMax += rightAdjustment;
                }
            }
        }

        /// <summary>
        /// Initializes some common alpha blending parameters when performing nearest neighbor interpolation when rescaling.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="area">The area to draw the image within.</param>
        /// <param name="xMin">The minimum x value in scaled image space.</param>
        /// <param name="xMax">The maximum x value in scaled image space.</param>
        /// <param name="yMin">The minimum y value in scaled image space.</param>
        /// <param name="yMax">The maximum y value in scaled image space.</param>
        /// <param name="xShift">The shift to apply to the x fixed point value to bring it back in range.</param>
        /// <param name="yShift">The shift to apply to the y fixed point value to bring it back in range</param>
        /// <param name="xScaleFixedPoint">The scaled x value computed in fixed point arithmetic for speed.</param>
        /// <param name="yScaleFixedPoint">The scaled y value computed in fixed point arithmetic for speed.</param>
        /// <param name="dataRowIndexFixedPoint">The initial row index into the image data buffer, computed in fixed point arithmetic.
        /// </param>
        /// <param name="dataColumnIndexFixedPointInitial">The initial logical column index into the image data buffer, computed in fixed
        /// point arithmetic.</param>
        private void AlphaBlendScalingInitialize(ImageData image, Rectangle area, int xMin, int xMax, int yMin, int yMax,
            out int xShift, out int yShift, out int xScaleFixedPoint, out int yScaleFixedPoint,
            out int dataRowIndexFixedPoint, out int dataColumnIndexFixedPointInitial)
        {
            float xScale = (float)image.Width / area.Width;
            float yScale = (float)image.Height / area.Height;

            xShift = Math.Min((int)Math.Log(int.MaxValue / (xScale * xMax), 2), 30);
            xScaleFixedPoint = (int)(xScale * (1 << xShift));

            yShift = Math.Min((int)Math.Log(int.MaxValue / (yScale * yMax), 2), 30);
            yScaleFixedPoint = (int)(yScale * (1 << yShift));

            dataRowIndexFixedPoint = yMin * yScaleFixedPoint;
            dataColumnIndexFixedPointInitial = xMin * xScaleFixedPoint;
        }

        /// <summary>
        /// Draws, with alpha blending, the specified 8bbp image at the specified location onto the form at its native size.
        /// </summary>
        /// <param name="image">An image specified by an 8bbp array and color palette to be alpha blended onto the form surface.</param>
        /// <param name="location">The location on the form onto which the image should be drawn.</param>
        /// <param name="mirror">Indicates if the image should be horizontally mirrored when drawing.</param>
        /// <param name="section">The scanline section to render. Only this portion will be rendered.</param>
        /// <param name="sectionCount">The number of scanline sections.</param>
        private unsafe void AlphaBlend8bbpUnscaled(ImageData image, Point location, bool mirror, int section, int sectionCount)
        {
            int xMin, xMax, yMin, yMax, backgroundIndex, backgroundIndexChange, backgroundIndexRowChange;
            AlphaBlendInitialize(location, new Size(image.Width, image.Height), mirror, section, sectionCount,
                out xMin, out xMax, out yMin, out yMax, out backgroundIndex, out backgroundIndexChange, out backgroundIndexRowChange);

            int dataIndex = yMin * image.Stride;
            int dataIndexRowChange = image.Stride;

            byte[] data = image.Data;
            int[] palette = image.ArgbPalette;
            for (int y = yMin; y < yMax; y++)
            {
                for (int x = xMin; x < xMax; x++)
                {
                    byte paletteIndex = data[dataIndex + x];
                    int srcColor = palette[paletteIndex];
                    int srcAlpha = (srcColor >> 24) & 0xFF;
                    if (srcAlpha == byte.MaxValue)
                        backgroundData[backgroundIndex] = srcColor;
                    else if (srcAlpha > 0)
                        AlphaBlendPixel(backgroundIndex, srcColor, srcAlpha);
                    backgroundIndex += backgroundIndexChange;
                }
                backgroundIndex += backgroundIndexRowChange;
                dataIndex += dataIndexRowChange;
            }
        }

        /// <summary>
        /// Draws, with alpha blending, the specified 4bbp image at the specified location onto the form at its native size.
        /// </summary>
        /// <param name="image">An image specified by an 4bbp array and color palette to be alpha blended onto the form surface.</param>
        /// <param name="location">The location on the form onto which the image should be drawn.</param>
        /// <param name="mirror">Indicates if the image should be horizontally mirrored when drawing.</param>
        /// <param name="section">The scanline section to render. Only this portion will be rendered.</param>
        /// <param name="sectionCount">The number of scanline sections.</param>
        private unsafe void AlphaBlend4bbpUnscaled(ImageData image, Point location, bool mirror, int section, int sectionCount)
        {
            int xMin, xMax, yMin, yMax, backgroundIndex, backgroundIndexChange, backgroundIndexRowChange;
            AlphaBlendInitialize(location, new Size(image.Width, image.Height), mirror, section, sectionCount,
                out xMin, out xMax, out yMin, out yMax, out backgroundIndex, out backgroundIndexChange, out backgroundIndexRowChange);

            int dataIndex = yMin * image.Stride;
            int dataIndexRowChange = image.Stride;

            byte[] data = image.Data;
            int[] palette = image.ArgbPalette;
            for (int y = yMin; y < yMax; y++)
            {
                for (int x = xMin; x < xMax; x++)
                {
                    byte paletteIndexes = data[dataIndex + x / 2];
                    int paletteIndex;
                    if (x % 2 == 0)
                        paletteIndex = paletteIndexes >> 4;
                    else
                        paletteIndex = paletteIndexes & 0xF;
                    int srcColor = palette[paletteIndex];
                    int srcAlpha = (srcColor >> 24) & 0xFF;
                    if (srcAlpha == byte.MaxValue)
                        backgroundData[backgroundIndex] = srcColor;
                    else if (srcAlpha > 0)
                        AlphaBlendPixel(backgroundIndex, srcColor, srcAlpha);
                    backgroundIndex += backgroundIndexChange;
                }
                backgroundIndex += backgroundIndexRowChange;
                dataIndex += dataIndexRowChange;
            }
        }

        /// <summary>
        /// Draws, with alpha blending, the specified 8bbp image within the specified rectangle onto the form. Scaling is done using
        /// nearest neighbor interpolation.
        /// </summary>
        /// <param name="image">An image specified by an 8bbp array and color palette to be alpha blended onto the form surface.</param>
        /// <param name="area">The area on the form onto which the image should be drawn.</param>
        /// <param name="mirror">Indicates if the image should be horizontally mirrored when drawing.</param>
        /// <param name="section">The scanline section to render. Only this portion will be rendered.</param>
        /// <param name="sectionCount">The number of scanline sections.</param>
        private unsafe void AlphaBlend8bbp(ImageData image, Rectangle area, bool mirror, int section, int sectionCount)
        {
            int xMin, xMax, yMin, yMax, backgroundIndex, backgroundIndexChange, backgroundIndexRowChange;
            AlphaBlendInitialize(area.Location, area.Size, mirror, section, sectionCount,
                out xMin, out xMax, out yMin, out yMax, out backgroundIndex, out backgroundIndexChange, out backgroundIndexRowChange);

            int xShift, yShift, xScaleFixedPoint, yScaleFixedPoint, dataRowIndexFixedPoint, dataColumnIndexFixedPointInitial;
            AlphaBlendScalingInitialize(image, area, xMin, xMax, yMin, yMax,
                out xShift, out yShift, out xScaleFixedPoint, out yScaleFixedPoint,
                out dataRowIndexFixedPoint, out dataColumnIndexFixedPointInitial);

            byte[] data = image.Data;
            int[] palette = image.ArgbPalette;
            int imageStride = image.Stride;
            for (int y = yMin; y < yMax; y++)
            {
                int dataRowIndex = (dataRowIndexFixedPoint >> yShift) * imageStride;
                int dataColumnIndexFixedPoint = dataColumnIndexFixedPointInitial;
                for (int x = xMin; x < xMax; x++)
                {
                    int dataIndex = dataRowIndex + (dataColumnIndexFixedPoint >> xShift);
                    dataColumnIndexFixedPoint += xScaleFixedPoint;
                    byte paletteIndex = data[dataIndex];
                    int srcColor = palette[paletteIndex];
                    int srcAlpha = (srcColor >> 24) & 0xFF;
                    if (srcAlpha == byte.MaxValue)
                        backgroundData[backgroundIndex] = srcColor;
                    else if (srcAlpha > 0)
                        AlphaBlendPixel(backgroundIndex, srcColor, srcAlpha);
                    backgroundIndex += backgroundIndexChange;
                }
                backgroundIndex += backgroundIndexRowChange;
                dataRowIndexFixedPoint += yScaleFixedPoint;
            }
        }

        /// <summary>
        /// Draws, with alpha blending, the specified 4bbp image within the specified rectangle onto the form. Scaling is done using
        /// nearest neighbor interpolation.
        /// </summary>
        /// <param name="image">An image specified by an 4bbp array and color palette to be alpha blended onto the form surface.</param>
        /// <param name="area">The area on the form onto which the image should be drawn.</param>
        /// <param name="mirror">Indicates if the image should be horizontally mirrored when drawing.</param>
        /// <param name="section">The scanline section to render. Only this portion will be rendered.</param>
        /// <param name="sectionCount">The number of scanline sections.</param>
        private unsafe void AlphaBlend4bbp(ImageData image, Rectangle area, bool mirror, int section, int sectionCount)
        {
            int xMin, xMax, yMin, yMax, backgroundIndex, backgroundIndexChange, backgroundIndexRowChange;
            AlphaBlendInitialize(area.Location, area.Size, mirror, section, sectionCount,
                out xMin, out xMax, out yMin, out yMax, out backgroundIndex, out backgroundIndexChange, out backgroundIndexRowChange);

            int xShift, yShift, xScaleFixedPoint, yScaleFixedPoint, dataRowIndexFixedPoint, dataColumnIndexFixedPointInitial;
            AlphaBlendScalingInitialize(image, area, xMin, xMax, yMin, yMax,
                out xShift, out yShift, out xScaleFixedPoint, out yScaleFixedPoint,
                out dataRowIndexFixedPoint, out dataColumnIndexFixedPointInitial);
            
            byte[] data = image.Data;
            int[] palette = image.ArgbPalette;
            int imageStride = image.Stride;
            for (int y = yMin; y < yMax; y++)
            {
                int dataRowIndex = (dataRowIndexFixedPoint >> yShift) * imageStride;
                int dataColumnIndexFixedPoint = dataColumnIndexFixedPointInitial;
                for (int x = xMin; x < xMax; x++)
                {
                    int dataColumnIndex = dataColumnIndexFixedPoint >> xShift;
                    dataColumnIndexFixedPoint += xScaleFixedPoint;
                    int dataIndex = dataRowIndex + dataColumnIndex / 2;
                    byte paletteIndexes = data[dataIndex];
                    int paletteIndex;
                    if (dataColumnIndex % 2 == 0)
                        paletteIndex = paletteIndexes >> 4;
                    else
                        paletteIndex = paletteIndexes & 0xF;
                    int srcColor = palette[paletteIndex];
                    int srcAlpha = (srcColor >> 24) & 0xFF;
                    if (srcAlpha == byte.MaxValue)
                        backgroundData[backgroundIndex] = srcColor;
                    else if (srcAlpha > 0)
                        AlphaBlendPixel(backgroundIndex, srcColor, srcAlpha);
                    backgroundIndex += backgroundIndexChange;
                }
                backgroundIndex += backgroundIndexRowChange;
                dataRowIndexFixedPoint += yScaleFixedPoint;
            }
        }

        /// <summary>
        /// Blends a translucent source color with the form background.
        /// </summary>
        /// <param name="backgroundIndex">The index into the background data array that represents the destination pixel.
        /// </param>
        /// <param name="srcColor">The source pixel to blend with the form.</param>
        /// <param name="srcAlpha">The alpha value of the source pixel.</param>
        private unsafe void AlphaBlendPixel(int backgroundIndex, int srcColor, int srcAlpha)
        {
            int dstColor = backgroundData[backgroundIndex];
            int dstAlpha = (dstColor >> 24) & 0xFF;
            int inverseSrcAlpha = byte.MaxValue - srcAlpha;
            int dstAG = ((dstColor >> 8) & 0x00FF00FF) * inverseSrcAlpha;
            int dstRB = (dstColor & 0x00FF00FF) * inverseSrcAlpha;
            backgroundData[backgroundIndex] =
                srcColor +
                ((dstRB >> 8) & 0x00FF00FF) +
                (dstAG & unchecked((int)0xFF00FF00));
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
            if (speakingSprite.SpeechText == null)
                return Rectangle.Empty;

            Size speechSize = Size.Ceiling(surface.MeasureString(speakingSprite.SpeechText, font));
            Point location = new Point(
                speakingSprite.Region.X + speakingSprite.Region.Width / 2 - speechSize.Width / 2 - 1,
                speakingSprite.Region.Y - speechSize.Height - 1);
            speechSize.Width += 1;
            speechSize.Height += 1;
            var region = new Rectangle(location, speechSize);
            if (region.Left < DisplayBounds.Left)
                region.X += DisplayBounds.Left - region.Left;
            else if (region.Right > DisplayBounds.Right)
                region.X -= region.Right - DisplayBounds.Right;
            if (region.Top < DisplayBounds.Top)
                region.Y += DisplayBounds.Top - region.Top;
            else if (region.Bottom > DisplayBounds.Bottom)
                region.Y -= region.Bottom - DisplayBounds.Bottom;
            return region;
        }

        /// <summary>
        /// Closes the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/>.
        /// </summary>
        public void Close()
        {
            if (Disposed)
                return;
            // Remove the handler that cancels closing since this is an external call to close so we want to proceed regardless of the
            // self-close flag.
            form.FormClosing -= GraphicsForm_FormClosing;
            ApplicationInvoke(form.Close);
        }

        /// <summary>
        /// Raised before the form is about to close.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            closePending = true;
            e.Cancel = preventSelfClose;
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
            if (opened)
            {
                opened = false;
                InterfaceClosed.Raise(this);
            }
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
                
                form.Dispose();

                foreach (var image in images.Values)
                    image.Dispose();

                if (windowIcon != null)
                    windowIcon.Dispose();

                foreach (WinFormContextMenu contextMenu in contextMenus)
                    contextMenu.Dispose();

                if (parallelBlend != null)
                {
                    parallelBlend.SignalAndWait();
                    parallelBlend.Dispose();
                }

                preUpdateInvalidRegion.Dispose();
                postUpdateInvalidRegion.Dispose();

                identityMatrix.Dispose();
                font.Dispose();
            }
        }
    }
}