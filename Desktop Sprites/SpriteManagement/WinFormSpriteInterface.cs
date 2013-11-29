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
            public GraphicsForm()
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
        /// Contains either an image stored as a GDI+ bitmap, or as a 8bbp indexed array and color palette.
        /// </summary>
        private sealed class ImageData : Disposable
        {
            /// <summary>
            /// A GDI+ bitmap of the image. This will be null if the image is instead made up of an indexed array and color palette.
            /// </summary>
            public readonly Bitmap Bitmap;
            /// <summary>
            /// An indexed array of image data. Each value refers to an index in the color palette. This will be null if the image is
            /// instead made up of a bitmap.
            /// </summary>
            public readonly byte[] Data;
            /// <summary>
            /// An array containing packed ARGB colors that define the color palette of the image. This will be null if the image is
            /// instead made up of a bitmap.
            /// </summary>
            public readonly int[] ArgbPalette;
            /// <summary>
            /// The width of the image, in pixels.
            /// </summary>
            public readonly int Width;
            /// <summary>
            /// The height of the image, in pixels.
            /// </summary>
            public readonly int Height;
            /// <summary>
            /// The stride width of the image, in bytes.
            /// </summary>
            public readonly int Stride;
            /// <summary>
            /// A hash code for the image.
            /// </summary>
            private readonly int hashCode;
            /// <summary>
            /// Gets the bit depth of the image, either 8bbp or 4bbp. Does not apply for bitmaps.
            /// </summary>
            public int Depth
            {
                get { return Width == Stride ? 8 : 4; }
            }
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
            /// <param name="transparentIndex">The index in the source palette that should be replaced with a transparent color, or -1 if
            /// there is no transparent color in the image.</param>
            /// <param name="stride">The stride width of the data buffer, in bytes.</param>
            /// <param name="width">The width of the image, in pixels.</param>
            /// <param name="height">The height of the image, in pixels.</param>
            /// <param name="hashCode">A pre-generated hash code for the image.</param>
            public ImageData(byte[] data, RgbColor[] palette, int transparentIndex, int stride, int width, int height, int hashCode)
            {
                Data = data;
                Height = height;
                Width = width;
                Stride = stride;
                this.hashCode = hashCode;
                ArgbPalette = new int[palette.Length];
                for (int i = 0; i < ArgbPalette.Length; i++)
                    ArgbPalette[i] = new ArgbColor(255, palette[i]).ToArgb();
                if (transparentIndex != -1)
                    ArgbPalette[transparentIndex] = new ArgbColor().ToArgb();
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
        /// Defines a <see cref="T:DesktopSprites.SpriteManagement.SpriteFrame`1"/> whose underlying image is an
        /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageData"/> instance.
        /// </summary>
        private sealed class ImageFrame : SpriteFrame<ImageData>, IDisposable
        {
            /// <summary>
            /// Gets the method that converts a buffer into an
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/>.
            /// </summary>
            public static BufferToImage<ImageFrame> FromBuffer
            {
                get { return FromBufferInternal; }
            }
            /// <summary>
            /// The method that converts a buffer into an
            /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/>.
            /// </summary>
            private static readonly BufferToImage<ImageFrame> FromBufferInternal =
                (byte[] buffer, RgbColor[] palette, int transparentIndex, int stride, int width, int height, int depth, int hashCode) =>
                {
                    return new ImageFrame(new ImageData(buffer, palette, transparentIndex, stride, width, height, hashCode));
                };

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
            private ImageFrame(ImageData imageData)
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
            /// Ensures the image is facing the desired direction by possibly flipping it horizontally.
            /// </summary>
            /// <param name="flipFromOriginal">Pass true to ensure the frame is facing the opposing direction as when it was loaded. Pass
            /// false  to ensure the frame is facing the same direction as when it was loaded.</param>
            public override void Flip(bool flipFromOriginal)
            {
                if (Flipped != flipFromOriginal)
                {
                    Flipped = !Flipped;
                    // TODO: Support flipping the raw bytes somehow?
                    if (Image.Bitmap != null)
                        Image.Bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    else
                        throw new NotImplementedException();
                }
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
        /// Stores the images for each sprite as a series of
        /// <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/>, indexed by filename.
        /// </summary>
        private LazyDictionary<string, AnimatedImage<ImageFrame>> images;

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
                    ApplicationInvoke(() => form.TopMost = value);
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
        /// Gets or sets the display boundary of the form. When setting, the form will be cleared of any drawn sprites.
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
                    ApplicationInvoke(() => form.DesktopBounds = value);
            }
        }
        /// <summary>
        /// Gets the current location of the cursor.
        /// </summary>
        public Point CursorPosition
        {
            get { return Control.MousePosition; }
        }
        /// <summary>
        /// Gets the mouse buttons which are currently held down.
        /// </summary>
        public SimpleMouseButtons MouseButtonsDown
        {
            get { return GetButtonsFromNative(Control.MouseButtons); }
        }
        /// <summary>
        /// Gets a value indicating whether the interface has input focus.
        /// </summary>
        public bool HasFocus
        {
            get { return Form.ActiveForm == form; }
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
            images = new LazyDictionary<string, AnimatedImage<ImageFrame>>(
                fileName => new AnimatedImage<ImageFrame>(
                    fileName, ImageFrameFromFile,
                    (b, p, tI, s, w, h, d, hC) => ImageFrameFromBuffer(b, p, tI, s, w, h, d, hC, fileName),
                    ImageFrame.AllowableBitDepths));
            render = new MethodInvoker(Render);
            using (var family = FontFamily.GenericSansSerif)
                font = new Font(family, 12, GraphicsUnit.Pixel);

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
            form = new GraphicsForm();
            form.FormClosing += GraphicsForm_FormClosing;
            form.Disposed += GraphicsForm_Disposed;

            form.Activated += (sender, e) => Focused.Raise(this);
            form.Deactivate += (sender, e) => Unfocused.Raise(this);

            DisplayBounds = parameters.Item2;

            // Hook up to form events.
            form.MouseClick += GraphicsForm_MouseClick;
            form.KeyPress += GraphicsForm_KeyPress;

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
        /// <param name="hashCode">The hash code of the frame.</param>
        /// <param name="fileName">The path to the GIF file being loaded.</param>
        /// <returns>A new <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface.ImageFrame"/> for the frame held in the raw
        /// buffer.</returns>
        private ImageFrame ImageFrameFromBuffer(byte[] buffer, RgbColor[] palette, int transparentIndex,
            int stride, int width, int height, int depth, int hashCode, string fileName)
        {
            return Disposable.SetupSafely(
                ImageFrame.FromBuffer(buffer, palette, transparentIndex, stride, width, height, depth, hashCode),
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
            Show();
            opened = true;
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
        public void Pause()
        {
            paused = true;
        }

        /// <summary>
        /// Resumes display of the <see cref="T:DesktopSprites.SpriteManagement.WinFormSpriteInterface"/> from a paused state.
        /// </summary>
        public void Resume()
        {
            paused = false;
            Show();
        }

        /// <summary>
        /// Draws the given collection of sprites.
        /// </summary>
        /// <param name="sprites">The collection of sprites to draw.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sprites"/> is null.</exception>
        public void Draw(ICollection<ISprite> sprites)
        {
            Argument.EnsureNotNull(sprites, "sprites");

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

                // Draw all the sprites.
                foreach (ISprite sprite in sprites)
                {
                    // Draw the sprite image.
                    if (sprite.ImagePath != null)
                    {
                        var area = OffsetRectangle(sprite.Region, translate);
                        ImageFrame frame = images[sprite.ImagePath][sprite.ImageTimeIndex];
                        frame.Flip(sprite.FlipImage);
                        if (frame.Image.Bitmap != null)
                            form.BackgroundGraphics.DrawImage(frame.Image.Bitmap, area);
                        else
                            AlphaBlend(frame.Image, area);
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
        /// neighbor interpolation.
        /// </summary>
        /// <param name="image">An image specified by a byte array and color palette to be alpha blended onto the form surface.</param>
        /// <param name="area">The area on the form onto which the image should be drawn.</param>
        private unsafe void AlphaBlend(ImageData image, Rectangle area)
        {
            // Check if we can use the non-scaling methods for a performance boost.
            // Note the code duplication in these methods is required: they are on the hot path and we need to avoid function call
            // overhead. The methods are too long for the jitter to consider inlining them, so this is done manually.
            if (image.Width == area.Width && image.Height == area.Height)
            {
                if (image.Depth == 8)
                    AlphaBlend8bbpUnscaled(image, area.Location);
                else
                    AlphaBlend4bbpUnscaled(image, area.Location);
            }
            else
            {
                if (image.Depth == 8)
                    AlphaBlend8bbp(image, area);
                else
                    AlphaBlend4bbp(image, area);
            }
        }

        /// <summary>
        /// Draws, with alpha blending, the specified 8bbp image at the specified location onto the form at its native size.
        /// </summary>
        /// <param name="image">An image specified by an 8bbp array and color palette to be alpha blended onto the form surface.</param>
        /// <param name="location">The location on the form onto which the image should be drawn.</param>
        private unsafe void AlphaBlend8bbpUnscaled(ImageData image, Point location)
        {
            int xMin = Math.Max(0, -location.X);
            int yMin = Math.Max(0, -location.Y);
            int xMax = Math.Min(image.Width, form.Width - location.X);
            int yMax = Math.Min(image.Height, form.Height - location.Y);

            int backgroundIndex = (location.Y + yMin) * form.Width + location.X + xMin;
            int backgroundIndexRowChange = form.Width - xMax + xMin;

            int dataIndex = yMin * image.Stride;
            int dataIndexRowChange = image.Stride;

            byte[] data = image.Data;
            int[] palette = image.ArgbPalette;
            int* backgroundData = form.BackgroundData;
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
                        AlphaBlendPixel(backgroundData, backgroundIndex, srcColor, srcAlpha);
                    backgroundIndex++;
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
        private unsafe void AlphaBlend4bbpUnscaled(ImageData image, Point location)
        {
            int xMin = Math.Max(0, -location.X);
            int yMin = Math.Max(0, -location.Y);
            int xMax = Math.Min(image.Width, form.Width - location.X);
            int yMax = Math.Min(image.Height, form.Height - location.Y);

            int backgroundIndex = (location.Y + yMin) * form.Width + location.X + xMin;
            int backgroundIndexRowChange = form.Width - xMax + xMin;

            int dataIndex = yMin * image.Stride;
            int dataIndexRowChange = image.Stride;

            byte[] data = image.Data;
            int[] palette = image.ArgbPalette;
            int* backgroundData = form.BackgroundData;
            for (int y = yMin; y < yMax; y++)
            {
                for (int x = xMin; x < xMax; x++)
                {
                    byte paletteIndexes = data[dataIndex + x / 2];
                    int paletteIndex;
                    if (x % 2 == 0)
                        paletteIndex = paletteIndexes >> 4;
                    paletteIndex = paletteIndexes & 0xF;
                    int srcColor = palette[paletteIndex];
                    int srcAlpha = (srcColor >> 24) & 0xFF;
                    if (srcAlpha == byte.MaxValue)
                        backgroundData[backgroundIndex] = srcColor;
                    else if (srcAlpha > 0)
                        AlphaBlendPixel(backgroundData, backgroundIndex, srcColor, srcAlpha);
                    backgroundIndex++;
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
        private unsafe void AlphaBlend8bbp(ImageData image, Rectangle area)
        {
            int xMin = Math.Max(0, -area.Left);
            int yMin = Math.Max(0, -area.Top);
            int xMax = Math.Min(area.Width, form.Width - area.Left);
            int yMax = Math.Min(area.Height, form.Height - area.Top);

            int backgroundIndex = (area.Top + yMin) * form.Width + area.Left + xMin;
            int backgroundIndexRowChange = form.Width - xMax + xMin;

            float xScale = (float)image.Stride / area.Width;
            float yScale = (float)image.Height / area.Height;

            byte[] data = image.Data;
            int[] palette = image.ArgbPalette;
            int* backgroundData = form.BackgroundData;
            int imageStride = image.Stride;
            for (int y = yMin; y < yMax; y++)
            {
                int dataRowIndex = (int)(y * yScale) * imageStride;
                for (int x = xMin; x < xMax; x++)
                {
                    int dataIndex = dataRowIndex + (int)(x * xScale);
                    byte paletteIndex = data[dataIndex];
                    int srcColor = palette[paletteIndex];
                    int srcAlpha = (srcColor >> 24) & 0xFF;
                    if (srcAlpha == byte.MaxValue)
                        backgroundData[backgroundIndex] = srcColor;
                    else if (srcAlpha > 0)
                        AlphaBlendPixel(backgroundData, backgroundIndex, srcColor, srcAlpha);
                    backgroundIndex++;
                }
                backgroundIndex += backgroundIndexRowChange;
            }
        }

        /// <summary>
        /// Draws, with alpha blending, the specified 4bbp image within the specified rectangle onto the form. Scaling is done using
        /// nearest neighbor interpolation.
        /// </summary>
        /// <param name="image">An image specified by an 4bbp array and color palette to be alpha blended onto the form surface.</param>
        /// <param name="area">The area on the form onto which the image should be drawn.</param>
        private unsafe void AlphaBlend4bbp(ImageData image, Rectangle area)
        {
            int xMin = Math.Max(0, -area.Left);
            int yMin = Math.Max(0, -area.Top);
            int xMax = Math.Min(area.Width, form.Width - area.Left);
            int yMax = Math.Min(area.Height, form.Height - area.Top);

            int backgroundIndex = (area.Top + yMin) * form.Width + area.Left + xMin;
            int backgroundIndexRowChange = form.Width - xMax + xMin;

            float xScale = (float)image.Stride / area.Width;
            float yScale = (float)image.Height / area.Height;

            byte[] data = image.Data;
            int[] palette = image.ArgbPalette;
            int* backgroundData = form.BackgroundData;
            int imageStride = image.Stride;
            for (int y = yMin; y < yMax; y++)
            {
                int dataRowIndex = (int)(y * yScale) * imageStride;
                for (int x = xMin; x < xMax; x++)
                {
                    int dataIndex = dataRowIndex + (int)(x * xScale);
                    byte paletteIndexes = data[dataIndex];
                    int paletteIndex;
                    if (dataIndex % 2 == 0)
                        paletteIndex = paletteIndexes >> 4;
                    paletteIndex = paletteIndexes & 0xF;
                    int srcColor = palette[paletteIndex];
                    int srcAlpha = (srcColor >> 24) & 0xFF;
                    if (srcAlpha == byte.MaxValue)
                        backgroundData[backgroundIndex] = srcColor;
                    else if (srcAlpha > 0)
                        AlphaBlendPixel(backgroundData, backgroundIndex, srcColor, srcAlpha);
                    backgroundIndex++;
                }
                backgroundIndex += backgroundIndexRowChange;
            }
        }

        /// <summary>
        /// Blends a translucent source color with the form background.
        /// </summary>
        /// <param name="backgroundData">A pointer to the BackgroundData of the form.</param>
        /// <param name="backgroundIndex">The index into the <paramref name="backgroundData"/> array that represents the destination pixel.
        /// </param>
        /// <param name="srcColor">The source pixel to blend with the form.</param>
        /// <param name="srcAlpha">The alpha value of the source pixel.</param>
        private unsafe void AlphaBlendPixel(int* backgroundData, int backgroundIndex, int srcColor, int srcAlpha)
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
            return new Rectangle(location, speechSize);
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

                foreach (var image in images.InitializedValues)
                    image.Dispose();

                if (windowIcon != null)
                    windowIcon.Dispose();

                foreach (WinFormContextMenu contextMenu in contextMenus)
                    contextMenu.Dispose();

                preUpdateInvalidRegion.Dispose();
                postUpdateInvalidRegion.Dispose();

                identityMatrix.Dispose();
                font.Dispose();
            }
        }
    }
}