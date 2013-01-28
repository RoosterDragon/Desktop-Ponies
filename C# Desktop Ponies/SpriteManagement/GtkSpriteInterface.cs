namespace CSDesktopPonies.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Cairo;
    using CSDesktopPonies.Collections;
    using Gdk;
    using Gtk;
    using SD = System.Drawing;
    using SWF = System.Windows.Forms;

    // TODO: See what can be done to improve the memory leak. The way Gtk+ seems to handle unmanaged resources does not seem to perform
    // well in a managed environment, and Dispose() does not follow the usual C# idiom.
    // TODO: Fix abysmal performance.

    /// <summary>
    /// Creates a series of windows using Gtk# to display sprites.
    /// </summary>
    /// <remarks>
    /// Creates an individual window for every sprite that is to be drawn. Transparency is handled automatically and the system works on
    /// all platforms and with practically no overhead. It should run reasonably under a virtual machine.
    /// The system does not scale well, as the underlying window system must handle an increasingly large number of windows. The system as
    /// a whole has no overhead, but each additional sprite incurs moderate and increasingly large overhead to manage and layer its window.
    /// There is a cost in modifying the collection of sprites between calls, as windows must be created for new sprites and destroyed for
    /// sprites no longer in the collection. It is extremely important to batch draws into one call, to prevent needlessly creating and
    /// destroying windows.
    /// </remarks>
    public sealed class GtkSpriteInterface : ISpriteCollectionView, IDisposable
    {
        #region NSWindow class
        /// <summary>
        /// Provides static methods that operate on a MacOSX NSWindow object.
        /// </summary>
        private static class NSWindow
        {
            /// <summary>
            /// Indicates whether the class is supported on the current operating system.
            /// </summary>
            private static bool isSupported = OperatingSystemInfo.IsMacOSX && OperatingSystemInfo.OSVersion >= new Version(10, 0);
            /// <summary>
            /// Gets a value indicating whether the class is supported on the current operating system.
            /// </summary>
            public static bool IsSupported
            {
                get { return isSupported; }
            }

            /// <summary>
            /// Pointer to the selector for the setHasShadow method.
            /// </summary>
            private static IntPtr setHasShadowSelector;

            /// <summary>
            /// Gets the native NSWindow given the pointer to a <see cref="T:Gdk.Window"/> instance.
            /// </summary>
            /// <param name="window">The pointer to a <see cref="T:Gdk.Window"/>.</param>
            /// <returns>A pointer to the native NSWindow for the GDK window instance.</returns>
            [DllImport("libgtk-quartz-2.0.dylib")]
            private static extern IntPtr gdk_quartz_window_get_nswindow(IntPtr window);

            /// <summary>
            /// Sets a value indicating whether to apply a drop shadow to the window.
            /// </summary>
            /// <param name="window">An instance of <see cref="T:Gdk.Window"/> whose underlying native window must be a MacOSX NSWindow.
            /// </param>
            /// <param name="hasShadow">Indicates whether to apply a drop shadow.</param>
            public static void SetHasShadow(Gdk.Window window, bool hasShadow)
            {
                // Get the native window handle.
                IntPtr nsWindow = gdk_quartz_window_get_nswindow(window.Handle);

                // Register the method with the runtime, if it has not yet been.
                if (setHasShadowSelector == IntPtr.Zero)
                    setHasShadowSelector = MacOSX.NativeMethods.sel_registerName("setHasShadow:");

                // Send a message to the window, indicating the set shadow method and specified argument.
                MacOSX.NativeMethods.objc_msgSend(nsWindow, setHasShadowSelector, hasShadow);
            }
        }
        #endregion

        #region GraphicsWindow class
        /// <summary>
        /// Represents a single <see cref="T:Gtk.Window"/> for use as a canvas to draw a single sprite.
        /// </summary>
        private class GraphicsWindow : Gtk.Window
        {
            #region SpeechWindow class
            /// <summary>
            /// Represents a small popup window used to display speech bubbles.
            /// </summary>
            private class SpeechWindow : Gtk.Window
            {
                /// <summary>
                /// Gets or sets the text that appears inside the speech window.
                /// </summary>
                public string Text
                {
                    get { return ((Label)Child).Text; }
                    set { ((Label)Child).Text = value; }
                }

                /// <summary>
                /// Initializes a new instance of the
                /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow.SpeechWindow"/> class.
                /// </summary>
                public SpeechWindow()
                    : base(Gtk.WindowType.Popup)
                {
                    Decorated = false;
                    DoubleBuffered = false;

                    Child = new Label();
                    Child.Show();
                }

                /// <summary>
                /// Show the speech window centered and above the given location.
                /// </summary>
                /// <param name="x">The x co-ordinate of the location where the speech window should be horizontally centered.</param>
                /// <param name="y">The y co-ordinate of the location where the bottom of the speech window should coincide.</param>
                public void ShowAboveCenter(int x, int y)
                {
                    const int XPadding = 6;
                    const int YPadding = 2;
                    Gtk.Requisition size = Child.SizeRequest();
                    Move(x - size.Width / 2 - XPadding, y - size.Height - YPadding);
                    Resize(size.Width + 2 * XPadding, size.Height + 2 * YPadding);
                    if (!Visible)
                        Show();
                }
            }
            #endregion

            /// <summary>
            /// Indicates if the current windowing system supports RGBA (instead of RGB) for the surface of the window.
            /// </summary>
            public bool SupportsRgba { get; private set; }
            /// <summary>
            /// Indicates if the clipping mask is currently being actively updated.
            /// </summary>
            private bool updatingMask = false;
            /// <summary>
            /// The <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow.SpeechWindow"/> that provides the
            /// ability to display speech bubbles for this
            /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/>.
            /// </summary>
            private SpeechWindow speechBubble;
            /// <summary>
            /// Gets or sets the current <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.ClippedImage"/> that will be used
            /// for clipping and drawing a sprite.
            /// </summary>
            public ClippedImage CurrentImage { get; set; }
            /// <summary>
            /// The last image that was drawn, used to prevent re-drawing.
            /// </summary>
            private Pixbuf lastImage;
            /// <summary>
            /// A counter that ensures an initial drawing of the image is done, particularly so static images appear. Hacky.
            /// </summary>
            private int initialDrawCountHack = 0;
            /// <summary>
            /// The last clip that was applied, used to prevent re-applying.
            /// </summary>
            private Pixmap lastClip;
            /// <summary>
            /// The last known width of the window, used to prevent clearing the existing portion of a resized window.
            /// </summary>
            private int lastWidth;
            /// <summary>
            /// The last known height of the window, used to prevent clearing the existing portion of a resized window.
            /// </summary>
            private int lastHeight;

            /// <summary>
            /// Gets or sets the <see cref="T:CSDesktopPonies.SpriteManagement.ISprite"/> that this
            /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/> is responsible for drawing.
            /// </summary>
            public ISprite Sprite { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/> class.
            /// </summary>
            public GraphicsWindow()
                : base(Gtk.WindowType.Popup)
            {
                DetermineAlphaSupport();

                AppPaintable = true;
                Decorated = false;
                DoubleBuffered = false;
                SkipTaskbarHint = true;

                speechBubble = new SpeechWindow();

                Events |= EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | EventMask.KeyPressMask | EventMask.EnterNotifyMask;
            }

            /// <summary>
            /// Determines whether the screen the window is on supports an alpha channel, and sets the color map accordingly.
            /// </summary>
            private void DetermineAlphaSupport()
            {
                Colormap = Screen.RgbaColormap ?? Screen.RgbColormap;
                SupportsRgba = Screen.RgbaColormap != null;
            }

            /// <summary>
            /// Raises the realize event.
            /// Removes drop shadow on MacOSX systems.
            /// </summary>
            protected override void OnRealized()
            {
                base.OnRealized();
                if (NSWindow.IsSupported)
                    NSWindow.SetHasShadow(GdkWindow, false);
            }

            /// <summary>
            /// Raises the screen changed event.
            /// Determines whether the new screen supports an alpha channel.
            /// </summary>
            /// <param name="previousScreen">The previous screen.</param>
            protected override void OnScreenChanged(Screen previousScreen)
            {
                DetermineAlphaSupport();
                base.OnScreenChanged(previousScreen);
            }

            /// <summary>
            /// Raises the configure event.
            /// Clears new areas of a window who size has increased to be transparent, and ensures clipping mask is up to date.
            /// </summary>
            /// <param name='evnt'>Data about the event.</param>
            /// <returns>Returns true to stop other handlers being invoked; otherwise, false.</returns>
            protected override bool OnConfigureEvent(EventConfigure evnt)
            {
                Argument.EnsureNotNull(evnt, "evnt");

                //return base.OnConfigureEvent(evnt);
                int newWidth = evnt.Width;
                int newHeight = evnt.Height;

                // We can only clear newly exposed areas if we support RGBA drawing.
                if (SupportsRgba)
                {
                    // Clear the window to be transparent in the newly exposed areas.
                    if (newWidth > lastWidth || newHeight > lastHeight)
                    {
                        using (Region newRegion = new Region())
                        {
                            // Right edge.
                            if (newWidth > lastWidth)
                                newRegion.UnionWithRect(new Gdk.Rectangle(
                                    lastWidth, 0, newWidth - lastWidth, lastHeight));
                            // Bottom edge.
                            if (newHeight > lastHeight)
                                newRegion.UnionWithRect(new Gdk.Rectangle(
                                    0, lastHeight, lastWidth, newHeight - lastHeight));
                            // Bottom-right corner.
                            if (newWidth > lastWidth && newHeight > lastHeight)
                                newRegion.UnionWithRect(new Gdk.Rectangle(
                                    lastWidth, lastHeight, newWidth - lastWidth, newHeight - lastHeight));

                            // Create the Cairo context for possible alpha drawing. A context may not be reused, and must be recreated
                            // each draw. The context also implements IDisposable, and thus should be disposed after use.
                            using (Context context = CairoHelper.Create(GdkWindow))
                            {
                                context.Antialias = Antialias.None;
                                if (newRegion == null)
                                    Console.WriteLine("Region empty.");
                                CairoHelper.Region(context, newRegion);

                                if (SupportsRgba)
                                {
                                    // Clear the window to be transparent.
                                    context.Operator = Operator.Source;
                                    context.SetSourceRGBA(0, 0, 0, 0);
                                    context.Paint();
                                }
                            }
                        }
                    }
                }
                lastWidth = newWidth;
                lastHeight = newHeight;

                // Update clipping area to cover the whole of the newly resized window.
                if (SupportsRgba && !updatingMask)
                {
                    using (Region all = new Region())
                    {
                        all.UnionWithRect(new Gdk.Rectangle(0, 0, newWidth, newHeight));
                        GdkWindow.InputShapeCombineRegion(all, 0, 0);
                    }
                    lastClip = null;
                }

                return base.OnConfigureEvent(evnt);
            }

            /// <summary>
            /// Raises the enter notify event.
            /// Triggers active updating of the input mask on RGBA supported windows.
            /// </summary>
            /// <param name="evnt">Data about the event.</param>
            /// <returns>Returns true to stop other handlers being invoked; otherwise, false.</returns>
            protected override bool OnEnterNotifyEvent(EventCrossing evnt)
            {
                //return base.OnEnterNotifyEvent(evnt);
                if (SupportsRgba)
                {
                    // Start actively updating the input mask for RGBA supported windows.
                    updatingMask = true;
                    int width, height;
                    GetSize(out width, out height);
                    SetClip(width, height);
                }
                return base.OnEnterNotifyEvent(evnt);
            }

            /// <summary>
            /// Prevents raising of the delete event.
            /// </summary>
            /// <param name="evnt">Data about the event.</param>
            /// <returns>Returns true to stop other handlers being invoked.</returns>
            protected override bool OnDeleteEvent(Event evnt)
            {
                return true;
            }

            /// <summary>
            /// Raised when the window is destroyed.
            /// Also destroys the speech bubble window.
            /// </summary>
            protected override void OnDestroyed()
            {
                speechBubble.Destroy();
                base.OnDestroyed();
            }

            /// <summary>
            /// Sets the clipping region of the window, based on the current sprite.
            /// </summary>
            /// <param name="width">The width to fit the clipping region to, scaling as required.</param>
            /// <param name="height">The height to fit the clipping region to, scaling as required.</param>
            public void SetClip(int width, int height)
            {
                if (!SupportsRgba)
                {
                    if (lastClip != CurrentImage.Clip)
                    {
                        // If an alpha channel is not supported, we must constantly update the clipping area for visual and input
                        // transparency.
                        GdkWindow.ShapeCombineMask(CurrentImage.Clip, 0, 0);
                        lastClip = CurrentImage.Clip;
                    }
                }
                else if (updatingMask)
                {
                    // Only update the input mask, since alpha is already taken care of.
                    int x, y;
                    GetPointer(out x, out y);
                    if (x < 0 || y < 0 || x > width || y > height)
                    {
                        if (lastClip != null)
                        {
                            // The cursor is no longer over the window, so we can clear the region to something cheaper to evaluate.
                            updatingMask = false;
                            using (Region all = new Region())
                            {
                                all.UnionWithRect(new Gdk.Rectangle(0, 0, width, height));
                                GdkWindow.InputShapeCombineRegion(all, 0, 0);
                            }
                            lastClip = null;
                        }
                    }
                    else
                    {
                        if (lastClip != CurrentImage.Clip)
                        {
                            // Update the mask if the cursor is still over us.
                            GdkWindow.InputShapeCombineMask(CurrentImage.Clip, 0, 0);
                            lastClip = CurrentImage.Clip;
                        }
                    }
                }
            }

            /// <summary>
            /// Draws the current sprite onto the window.
            /// </summary>
            /// <param name="width">The width to draw the image at, scaling as required.</param>
            /// <param name="height">The height to draw the image at, scaling as required.</param>
            public void DrawFrame(int width, int height)
            {
                // Prevent redrawing of the same image.
                if (initialDrawCountHack < 1)
                {
                    initialDrawCountHack++;
                    lastImage = CurrentImage.Image;
                }
                else if (lastImage == CurrentImage.Image)
                {
                    return;
                }
                else
                {
                    initialDrawCountHack = 0;
                    lastImage = CurrentImage.Image;
                }

                // Create the Cairo context for possible alpha drawing. A context may not be reused, and must be recreated each draw.
                // The context also implements IDisposable, and thus should be disposed after use.
                using (Context context = CairoHelper.Create(GdkWindow))
                {
                    context.Antialias = Antialias.None;

                    // Draw the current sprite.
                    context.Operator = Operator.Source;
                    CairoHelper.SetSourcePixbuf(context, CurrentImage.Image, 0, 0);
                    context.Paint();
                }
            }

            /// <summary>
            /// Shows a speech bubble with the given text, centered and above the given point.
            /// </summary>
            /// <param name="text">The speech to display inside the bubble.</param>
            /// <param name="x">The x co-ordinate of the location where the speech window should be horizontally centered.</param>
            /// <param name="y">The y co-ordinate of the location where the bottom of the speech window should coincide.</param>
            public void ShowSpeech(string text, int x, int y)
            {
                speechBubble.Text = text;
                speechBubble.ShowAboveCenter(x, y);
            }

            /// <summary>
            /// Hides the speech bubble.
            /// </summary>
            public void HideSpeech()
            {
                if (speechBubble.Visible)
                    speechBubble.Hide();
            }
        }
        #endregion

        #region ClippedImage class
        /// <summary>
        /// Represents an image and its associated clipping mask.
        /// </summary>
        private class ClippedImage : IDisposable
        {
            /// <summary>
            /// Gets or sets the image itself.
            /// </summary>
            public Pixbuf Image { get; set; }
            /// <summary>
            /// Gets or sets the clipping mask for the image.
            /// </summary>
            public Pixmap Clip { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.ClippedImage"/> class.
            /// </summary>
            public ClippedImage()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.ClippedImage"/> class
            /// from the given file.
            /// </summary>
            /// <param name="fileName">The path to a static image file from which to create a new
            /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.ClippedImage"/>.</param>
            public ClippedImage(string fileName)
            {
                // These operations must be invoked on the application thread to work.
                object invokeFinished = new object();
                lock (invokeFinished)
                {
                    Application.Invoke((o, args) =>
                    {
                        // Create the image and get its clipping mask.
                        Pixmap clipMap;
                        Pixmap clipMask;
                        Image = new Pixbuf(fileName);
                        Image.RenderPixmapAndMask(out clipMap, out clipMask, 255);
                        Clip = clipMask;
                        clipMap.Dispose();
                        lock (invokeFinished)
                            Monitor.Pulse(invokeFinished);
                    });
                    Monitor.Wait(invokeFinished);
                }
            }

            /// <summary>
            /// Releases all resources used by the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.ClippedImage"/> object.
            /// </summary>
            public void Dispose()
            {
                // The image is IDisposable and should be disposed, despite lacking a public method.
                ((IDisposable)Image).Dispose();
                Clip.Dispose();
            }
        }
        #endregion

        #region GtkFrame class
        /// <summary>
        /// Defines a <see cref="T:CSDesktopPonies.SpriteManagement.SpriteFrame`1"/> whose underlying image is a
        /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.ClippedImage"/>.
        /// </summary>
        private class GtkFrame : SpriteFrame<ClippedImage>, IDisposable
        {
            /// <summary>
            /// Gets the method for creating a <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/> frame from a
            /// buffer.
            /// </summary>
            public static BufferToImage<GtkFrame> FromBuffer
            {
                get { return FromBufferMethod; }
            }

            /// <summary>
            /// Gets or sets the interface. This is a slightly dirty hack so that the image can be created on the application thread.
            /// </summary>
            public static GtkSpriteInterface Interface { get; set; }

            /// <summary>
            /// Creates a new <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/> from the raw buffer.
            /// </summary>
            /// <param name="buffer">The raw buffer.</param>
            /// <param name="palette">The color palette.</param>
            /// <param name="transparentIndex">The index of the transparent color.</param>
            /// <param name="stride">The stride width of the buffer.</param>
            /// <param name="width">The logical width of the buffer.</param>
            /// <param name="height">The logical height of the buffer.</param>
            /// <param name="depth">The bit depth of the buffer.</param>
            /// <param name="hashCode">The hash code of the frame.</param>
            /// <returns>A new <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/> for the frame held in the raw
            /// buffer.</returns>
            /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="depth"/> is not 8.</exception>
            private static GtkFrame FromBufferMethod(
                byte[] buffer, RgbColor[] palette, int transparentIndex, int stride, int width, int height, int depth, int hashCode)
            {
                if (depth != 8)
                    throw new ArgumentOutOfRangeException("depth", depth, "depth must be 8.");

                ClippedImage frameImage = new ClippedImage();
                List<Gdk.Point> points = new List<Gdk.Point>((int)Math.Ceiling((float)width * (float)height / 8f));

                // Create a data buffer to hold 32bbp RGBA values.
                byte[] data = new byte[width * height * 4];

                // Loop over the pixels in each row (to account for stride width of the source).
                for (int row = 0; row < height; row++)
                    for (int x = 0; x < width; x++)
                    {
                        // Get the index value from the 8bbp source.
                        byte index = buffer[row * stride + x];
                        // Get the destination offset in the 32bbp array.
                        int offset = 4 * (width * row + x);
                        if (index != transparentIndex)
                        {
                            // Get the color from the palette, and set the RGB values.
                            RgbColor color = palette[index];
                            data[offset + 0] = color.R;
                            data[offset + 1] = color.G;
                            data[offset + 2] = color.B;
                            data[offset + 3] = 255;

                            // Save the point for creating the mask later.
                            points.Add(new Gdk.Point(x, row));
                        }
                        else
                        {
                            // This color is transparent.
                            data[offset + 3] = 0;
                        }
                    }

                // Create the clipping mask by setting all the pixels in the mask from the list of points we draw on.
                Interface.ApplicationInvoke(() => frameImage.Clip = new Pixmap(null, width, height, 1));
                if (points.Count > 0)
                    using (Gdk.GC context = new Gdk.GC(frameImage.Clip))
                    {
                        context.Function = Gdk.Function.Set;
                        frameImage.Clip.DrawPoints(context, points.ToArray());
                    }

                // Create the image from the data array.
                frameImage.Image = new Pixbuf(data, true, 8, width, height, width * 4);

                return new GtkFrame(frameImage, hashCode);
            }

            /// <summary>
            /// Gets the set of allowable bit depths for a <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/>.
            /// </summary>
            public static BitDepths AllowableBitDepths
            {
                get { return BitDepths.Indexed8Bpp; }
            }

            /// <summary>
            /// Contains the image that is the horizontal mirror the of base image.
            /// </summary>
            private ClippedImage flippedImage;
            /// <summary>
            /// The hash code of the frame.
            /// </summary>
            private int hashCode;

            /// <summary>
            /// Gets the dimensions of the frame.
            /// </summary>
            public override SD.Size Size
            {
                get { return new SD.Size(Image.Image.Width, Image.Image.Height); }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/> class from
            /// the given <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.ClippedImage"/>.
            /// </summary>
            /// <param name="clippedImage">The <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.ClippedImage"/> to use in
            /// the frame.</param>
            /// <param name="hash">The hash code of the frame.</param>
            public GtkFrame(ClippedImage clippedImage, int hash)
                : base(clippedImage)
            {
                hashCode = hash;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/> class from
            /// the given file.
            /// </summary>
            /// <param name="fileName">The path to a static image file from which to create a new
            /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/>.</param>
            public GtkFrame(string fileName)
                : this(new ClippedImage(fileName), fileName.GetHashCode())
            {
            }

            /// <summary>
            /// Ensures the frame is facing the desired direction by possibly flipping it horizontally.
            /// </summary>
            /// <param name="flipFromOriginal">Pass true to ensure the frame is facing the opposing direction as when it was loaded. Pass
            /// false to ensure the frame is facing the same direction as when it was loaded.</param>
            public override void Flip(bool flipFromOriginal)
            {
                if (Flipped != flipFromOriginal)
                {
                    Flipped = !Flipped;

                    // Create the mirrored frame, if we have yet to do so.
                    if (flippedImage == null)
                    {
                        flippedImage = new ClippedImage();

                        // Flip the frame image horizontally.
                        flippedImage.Image = base.Image.Image.Flip(true);

                        // Determine the new clipping mask.
                        Pixmap map;
                        Pixmap mask;
                        flippedImage.Image.RenderPixmapAndMask(out map, out mask, 255);
                        flippedImage.Clip = mask;
                        map.Dispose();
                    }
                }
            }

            /// <summary>
            /// Gets the image that represents this frame.
            /// </summary>
            public new ClippedImage Image
            {
                // Hide the base definition, so we can return either our flipped image, or the base.
                get { return Flipped ? flippedImage : base.Image; }
            }

            /// <summary>
            /// Gets the hash code of the frame.
            /// </summary>
            /// <returns>A hash code for this frame.</returns>
            public override int GetFrameHashCode()
            {
                return hashCode;
            }

            /// <summary>
            /// Releases all resources used by the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/> object.
            /// </summary>
            public void Dispose()
            {
                Image.Dispose();
            }
        }
        #endregion

        #region GtkContextMenuItem class
        /// <summary>
        /// Wraps a <see cref="T:Gtk.MenuItem"/> in order to expose the
        /// <see cref="T:CSDesktopPonies.SpriteManagement.ISimpleContextMenuItem"/> interface.
        /// </summary>
        private class GtkContextMenuItem : ISimpleContextMenuItem, IDisposable
        {
            /// <summary>
            /// The wrapped <see cref="T:Gtk.MenuItem"/>.
            /// </summary>
            private MenuItem item;
            /// <summary>
            /// The method that runs on activation.
            /// </summary>
            private EventHandler activatedMethod;
            /// <summary>
            /// The method that runs on activation, queued to the <see cref="T:System.Threading.ThreadPool"/>.
            /// </summary>
            private EventHandler queuedActivatedMethod;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkContextMenuItem"/>
            /// class for the given <see cref="T:Gtk.SeparatorMenuItem"/>.
            /// </summary>
            /// <param name="separatorItem">The underlying <see cref="T:Gtk.SeparatorMenuItem"/> that this class wraps.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="separatorItem"/> is null.</exception>
            public GtkContextMenuItem(SeparatorMenuItem separatorItem)
            {
                Argument.EnsureNotNull(separatorItem, "separatorItem");

                item = separatorItem;
            }
            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkContextMenuItem"/>
            /// class for the given <see cref="T:Gtk.MenuItem"/>, and links up the given activation method.
            /// </summary>
            /// <param name="menuItem">The underlying <see cref="T:Gtk.MenuItem"/> that this class wraps.</param>
            /// <param name="activated">The method to be run when the item is activated.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="menuItem"/> is null.</exception>
            public GtkContextMenuItem(MenuItem menuItem, EventHandler activated)
            {
                Argument.EnsureNotNull(menuItem, "menuItem");

                item = menuItem;
                Activated = activated;
            }
            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkContextMenuItem"/>
            /// class for the given <see cref="T:Gtk.MenuItem"/>, and links up the activation method to display a new sub-menu.
            /// </summary>
            /// <param name="menuItem">The underlying <see cref="T:Gtk.MenuItem"/> that this class wraps.</param>
            /// <param name="subItems">The items to appear in the sub-menu.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="menuItem"/> is null.-or-<paramref name="subItems"/> is
            /// null.-or-<paramref name="parent"/> is null.</exception>
            /// <param name="parent">The <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> that will own this
            /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkContextMenuItem"/>.</param>
            /// <exception cref="T:System.ArgumentException"><paramref name="subItems"/> is empty.</exception>
            public GtkContextMenuItem(MenuItem menuItem, IEnumerable<ISimpleContextMenuItem> subItems, GtkSpriteInterface parent)
            {
                Argument.EnsureNotNull(menuItem, "menuItem");
                Argument.EnsureNotNull(subItems, "subItems");

                List<ISimpleContextMenuItem> subItemsList = new List<ISimpleContextMenuItem>(subItems);

                if (subItemsList.Count == 0)
                    throw new ArgumentException("subItems must not be empty.", "subItems");

                item = menuItem;
                GtkContextMenu gtkContextMenu = new GtkContextMenu(parent, subItemsList);
                item.Submenu = gtkContextMenu;
                SubItems = gtkContextMenu.Items;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the item is a separator.
            /// </summary>
            public bool IsSeparator
            {
                get
                {
                    return item is SeparatorMenuItem;
                }
                set
                {
                    if (IsSeparator && !value)
                        item = new MenuItem();
                    else if (!IsSeparator && value)
                        item = new SeparatorMenuItem();
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
                    return ((Label)item.Child).Text;
                }
                set
                {
                    if (IsSeparator)
                        throw new InvalidOperationException("Cannot set the text on a separator item.");
                    ((Label)item.Child).Text = value;
                }
            }
            /// <summary>
            /// Gets or sets the method that runs when the item is activated by the user.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The item does not support an activation method.</exception>
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
                    if (queuedActivatedMethod != null)
                        item.Activated -= queuedActivatedMethod;

                    activatedMethod = value;
                    queuedActivatedMethod = null;

                    if (activatedMethod != null)
                    {
                        queuedActivatedMethod = (o, args) => ThreadPool.QueueUserWorkItem(obj => activatedMethod(o, args));
                        item.Activated += queuedActivatedMethod;
                    }
                }
            }
            /// <summary>
            /// Gets the sub-items in an item that displays a new sub-menu of items.
            /// </summary>
            public ReadOnlyCollection<ISimpleContextMenuItem> SubItems { get; private set; }

            /// <summary>
            /// Releases all resources used by the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkContextMenuItem"/>
            /// object.
            /// </summary>
            public void Dispose()
            {
                item.Dispose();

                if (SubItems != null)
                    foreach (IDisposable subitem in SubItems)
                        subitem.Dispose();
            }
        }
        #endregion

        #region GtkContextMenu class
        /// <summary>
        /// Extends a <see cref="T:Gtk.Menu"/> in order to expose the <see cref="T:CSDesktopPonies.SpriteManagement.ISimpleContextMenu"/>
        /// interface.
        /// </summary>
        private class GtkContextMenu : Menu, ISimpleContextMenu, IDisposable
        {
            /// <summary>
            /// The <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> that owns this
            /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkContextMenu"/>.
            /// </summary>
            private GtkSpriteInterface owner;
            /// <summary>
            /// The underlying list of menu items.
            /// </summary>
            private List<ISimpleContextMenuItem> items;
            /// <summary>
            /// Gets the collection of menu items in this menu.
            /// </summary>
            public ReadOnlyCollection<ISimpleContextMenuItem> Items { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkContextMenu"/> class
            /// to display the given menu items.
            /// </summary>
            /// <param name="parent">The <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> that will own this
            /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkContextMenu"/>.</param>
            /// <param name="menuItems">The items which should be displayed in this menu.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="parent"/> is null.-or-<paramref name="menuItems"/> is 
            /// null.</exception>
            public GtkContextMenu(GtkSpriteInterface parent, IEnumerable<ISimpleContextMenuItem> menuItems)
            {
                Argument.EnsureNotNull(parent, "parent");
                Argument.EnsureNotNull(menuItems, "menuItems");

                owner = parent;

                items = new List<ISimpleContextMenuItem>();
                Items = new ReadOnlyCollection<ISimpleContextMenuItem>(items);

                foreach (ISimpleContextMenuItem menuItem in menuItems)
                {
                    MenuItem gtkMenuItem;
                    if (menuItem.IsSeparator)
                        gtkMenuItem = new SeparatorMenuItem();
                    else
                        gtkMenuItem = new MenuItem(menuItem.Text);
                    Append(gtkMenuItem);
                    gtkMenuItem.Show();

                    GtkContextMenuItem gtkContextMenuItem;
                    if (menuItem.IsSeparator)
                        gtkContextMenuItem = new GtkContextMenuItem((SeparatorMenuItem)gtkMenuItem);
                    else if (menuItem.SubItems == null)
                        gtkContextMenuItem = new GtkContextMenuItem(gtkMenuItem, menuItem.Activated);
                    else
                        gtkContextMenuItem = new GtkContextMenuItem(gtkMenuItem, menuItem.SubItems, owner);

                    items.Add(gtkContextMenuItem);
                }
            }

            /// <summary>
            /// Displays the context menu at the given co-ordinates.
            /// </summary>
            /// <param name="x">The x co-ordinate of the location where the menu should be shown.</param>
            /// <param name="y">The y co-ordinate of the location where the menu should be shown.</param>
            public void Show(int x, int y)
            {
                owner.ApplicationInvoke(() => Popup());
            }

            /// <summary>
            /// Releases all resources used by the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkContextMenu"/> object.
            /// </summary>
            public override void Dispose()
            {
                foreach (GtkContextMenuItem item in items)
                    item.Dispose();
            }
        }
        #endregion

        #region Fields and Properties
        /// <summary>
        /// Indicates if we have disposed of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/>.
        /// </summary>
        private static bool disposed = true;
        /// <summary>
        /// Indicates if we have yet attached to the static <see cref="E:GLib.ExceptionManager.UnhandledException"/> event.
        /// </summary>
        private static bool exceptionsRaised;
        /// <summary>
        /// Stores the images for each sprite as a series of <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/>,
        /// indexed by filename.
        /// </summary>
        private LazyDictionary<string, AnimatedImage<GtkFrame>> images;
        /// <summary>
        /// List of <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkContextMenu"/> which have been created by the
        /// interface.
        /// </summary>
        private readonly LinkedList<GtkContextMenu> contextMenus = new LinkedList<GtkContextMenu>();
        /// <summary>
        /// The thread running the application.
        /// </summary>
        private readonly Thread appThread;
        /// <summary>
        /// Indicates if drawing is paused.
        /// </summary>
        private bool paused;
        /// <summary>
        /// Synchronization object used to prevent other operations conflicting with drawing.
        /// </summary>
        private readonly object drawSync = new object();
        /// <summary>
        /// Links a <see cref="T:CSDesktopPonies.SpriteManagement.ISprite"/> to the
        /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/> responsible for drawing it.
        /// </summary>
        private readonly Dictionary<ISprite, GraphicsWindow> spriteWindows = new Dictionary<ISprite, GraphicsWindow>();
        /// <summary>
        /// Maintains the list of <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/> in the desired draw
        /// order.
        /// </summary>
        private readonly List<GraphicsWindow> drawOrderedWindows = new List<GraphicsWindow>(0);
        /// <summary>
        /// Maintains the list of <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/> that were removed
        /// since last draw.
        /// </summary>
        private readonly List<ISprite> removedSprites = new List<ISprite>();
        /// <summary>
        /// Title for instances of <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/>.
        /// </summary>
        private string windowTitle = "Gtk# Sprite Window";
        /// <summary>
        /// Path to the icon for instances of <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/>.
        /// </summary>
        private string windowIconFilePath = null;
        /// <summary>
        /// Icon for instances of <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/>.
        /// </summary>
        private Pixbuf windowIcon = null;
        /// <summary>
        /// Indicates if each <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/> should act as a topmost
        /// window.
        /// </summary>
        private bool windowTopmost = true;
        /// <summary>
        /// Records the time the last mouse down event occurred.
        /// </summary>
        private DateTime mouseDownTime;

        /// <summary>
        /// Synchronization object for use when invoking a method on the application thread.
        /// </summary>
        private readonly object invokeSync = new object();

        /// <summary>
        /// Gets or sets the text to use in the title frame of each window.
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return windowTitle;
            }
            set
            {
                windowTitle = value;
                ApplicationInvoke(() =>
                {
                    foreach (GraphicsWindow window in spriteWindows.Values)
                        window.Title = windowTitle;
                });
            }
        }
        /// <summary>
        /// Gets or sets the icon used for each window to the icon at the given path.
        /// </summary>
        public string WindowIconFilePath
        {
            get
            {
                return windowIconFilePath;
            }
            set
            {
                Argument.EnsureNotNull(value, "value");

                if (windowIconFilePath != value)
                {
                    windowIconFilePath = value;
                    ApplicationInvoke(() =>
                    {
                        if (windowIcon != null)
                            windowIcon.Dispose();
                        if (windowIconFilePath == null)
                            windowIcon = null;
                        else
                            windowIcon = new Pixbuf(windowIconFilePath);

                        foreach (GraphicsWindow window in spriteWindows.Values)
                            window.Icon = windowIcon;
                    });
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether windows will display as topmost windows.
        /// </summary>
        public bool Topmost
        {
            get
            {
                return windowTopmost;
            }
            set
            {
                windowTopmost = value;
                ApplicationInvoke(() =>
                {
                    foreach (GraphicsWindow window in spriteWindows.Values)
                        window.KeepAbove = windowTopmost;
                });
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether a window should appear in the taskbar.
        /// </summary>
        public bool ShowInTaskbar
        {
            get { return false; }
            set { }
        }
        /// <summary>
        /// Gets the location of the cursor.
        /// </summary>
        public SD.Point CursorPosition
        {
            get { return SWF.Cursor.Position; }
        }

        /// <summary>
        /// Gets a value indicating whether alpha blending is in use. If true, pixels which are partially transparent will be blended with
        /// those behind them to achieve proper transparency; otherwise these pixels will be rendered opaque, and only fully transparent
        /// pixels will render as transparent, resulting in simple 1-bit transparency.
        /// </summary>
        public bool IsAlphaBlended { get; private set; }

        /// <summary>
        /// Gets or sets the FrameRecordCollector for debugging purposes.
        /// </summary>
        internal AnimationLoopBase.FrameRecordCollector Collector { get; set; }
        #endregion

        #region Events
        /// <summary>
        /// Gets the equivalent <see cref="T:CSDesktopPonies.SpriteManagement.SimpleMouseButtons"/> enumeration from the native button
        /// code.
        /// </summary>
        /// <param name="button">The code of the mouse button that was pressed.</param>
        /// <returns>The equivalent <see cref="T:CSDesktopPonies.SpriteManagement.SimpleMouseButtons"/> enumeration for this button.
        /// </returns>
        private static SimpleMouseButtons GetButtonsFromNative(uint button)
        {
            switch (button)
            {
                case 1: return SimpleMouseButtons.Left;
                case 2: return SimpleMouseButtons.Middle;
                case 3: return SimpleMouseButtons.Right;
                default: return SimpleMouseButtons.None;
            }
        }
        /// <summary>
        /// Raised when a mouse button has been pressed down.
        /// Raises the MouseDown event.
        /// </summary>
        /// <param name="o">The object that raised the event.</param>
        /// <param name="args">Data about the event.</param>
        private void GraphicsWindow_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            //Console.WriteLine("GraphicsWindow_ButtonPressEvent raised. Button: " +
            //    GetButtonsFromNative(args.Event.Button) + "(" + args.Event.Button + ")");
            mouseDownTime = DateTime.UtcNow;
            MouseDown.Raise(this, () => new SimpleMouseEventArgs(
                GetButtonsFromNative(args.Event.Button), (int)args.Event.XRoot, (int)args.Event.YRoot));
            //Console.WriteLine("GraphicsWindow_ButtonPressEvent finished.");
        }
        /// <summary>
        /// Raised when a mouse button has been released.
        /// Raises the MouseClick and MouseUp events.
        /// </summary>
        /// <param name="o">The object that raised the event.</param>
        /// <param name="args">Data about the event.</param>
        private void GraphicsWindow_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
        {
            SimpleMouseEventArgs e = new SimpleMouseEventArgs(
                GetButtonsFromNative(args.Event.Button), (int)args.Event.XRoot, (int)args.Event.YRoot);
            if (DateTime.UtcNow - mouseDownTime <= TimeSpan.FromMilliseconds(SWF.SystemInformation.DoubleClickTime))
                MouseClick.Raise(this, e);
            MouseUp.Raise(this, e);
        }
        /// <summary>
        /// Raised when a key has been pressed.
        /// Raises the KeyPress event.
        /// </summary>
        /// <param name="o">The object that raised the event.</param>
        /// <param name="args">Data about the event.</param>
        private void GraphicsWindow_KeyPressEvent(object o, KeyPressEventArgs args)
        {
            KeyPress.Raise(this, () => new SimpleKeyEventArgs((char)args.Event.KeyValue));
        }

        /// <summary>
        /// Occurs when a key is pressed while a window has focus.
        /// </summary>
        public event EventHandler<SimpleKeyEventArgs> KeyPress;
        /// <summary>
        /// Occurs when the mouse pointer is over a window and a mouse button is pressed.
        /// </summary>
        public event EventHandler<SimpleMouseEventArgs> MouseDown;
        /// <summary>
        /// Occurs when a window is clicked by the mouse.
        /// </summary>
        public event EventHandler<SimpleMouseEventArgs> MouseClick;
        /// <summary>
        /// Occurs when the mouse pointer is over a window and a mouse button is released.
        /// </summary>
        public event EventHandler<SimpleMouseEventArgs> MouseUp;
        /// <summary>
        /// Occurs when the interface is closed.
        /// </summary>
        public event EventHandler InterfaceClosed;
        #endregion

        /// <summary>
        /// Gets a value indicating whether a <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> can be used in the
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
                    domain.Load("atk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f");
                    domain.Load("gdk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f");
                    domain.Load("glib-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f");
                    //domain.Load("gtk-dotnet, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f");
                    domain.Load("gtk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f");
                    domain.Load("Mono.Cairo, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");

                    domain.Load("System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                    AppDomain.Unload(domain);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> class.
        /// </summary>
        /// <param name="useAlphaBlending">Indicates if alpha blending should be supported. If true, the transparency value in images is
        /// respected, and the color is blended with the color behind it. If false, semi-transparent values will appear opaque. Only fully
        /// transparent pixels will continue to render as transparent.</param>
        public GtkSpriteInterface(bool useAlphaBlending)
        {
            lock (drawSync)
            {
                if (!disposed)
                    throw new InvalidOperationException("Only one instance of GtkSpriteInterface may be active at any time.");
                disposed = false;

                // Catch unhandled exceptions on the UI thread and re-throw them.
                if (!exceptionsRaised)
                {
                    GLib.ExceptionManager.UnhandledException += (args) =>
                    {
                        throw (Exception)args.ExceptionObject;
                    };
                    exceptionsRaised = true;
                }
            }

            GtkFrame.Interface = this;
            IsAlphaBlended = useAlphaBlending;
            images = new LazyDictionary<string, AnimatedImage<GtkFrame>>(
                fileName => new AnimatedImage<GtkFrame>(
                    fileName, GtkFrameFromFile,
                    (b, p, tI, s, w, h, d, hC) => GtkFrameFromBuffer(b, p, tI, s, w, h, d, hC, fileName),
                    GtkFrame.AllowableBitDepths));

            appThread = new Thread(ApplicationRun) { Name = "GtkSpriteInterface.ApplicationRun" };
            appThread.SetApartmentState(ApartmentState.STA);
            appThread.Start();
        }

        /// <summary>
        /// Runs the main application loop.
        /// </summary>
        private void ApplicationRun()
        {
            Application.Init();
            Application.Run();
        }

        /// <summary>
        /// Invokes a method synchronously on the main application thread.
        /// </summary>
        /// <param name="method">The method to invoke. The method should take no parameters and return void.</param>
        private void ApplicationInvoke(System.Action method)
        {
            if (Thread.CurrentThread == appThread)
            {
                method();
            }
            else
            {
                lock (invokeSync)
                {
                    Application.Invoke((o, args) =>
                    {
                        method();
                        lock (invokeSync)
                            Monitor.Pulse(invokeSync);
                    });
                    Monitor.Wait(invokeSync);
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/> from the given file, loading extra
        /// transparency information and adjusting the colors as required by the transparency.
        /// </summary>
        /// <param name="fileName">The path to a static image file from which to create a new
        /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/>.</param>
        /// <returns>A new <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/> created from the given file.
        /// </returns>
        private GtkFrame GtkFrameFromFile(string fileName)
        {
            return new GtkFrame(fileName).SetupSafely(frame => AlterPixbufForTransparency(fileName, frame.Image.Image));
        }

        /// <summary>
        /// Creates a new <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/> from the raw buffer, loading extra
        /// transparency information and adjusting the colors as required by the transparency.
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
        /// <returns>A new <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GtkFrame"/> for the frame held in the raw
        /// buffer.</returns>
        private GtkFrame GtkFrameFromBuffer(byte[] buffer, RgbColor[] palette, int transparentIndex,
            int stride, int width, int height, int depth, int hashCode, string fileName)
        {
            GtkFrame frame = GtkFrame.FromBuffer(buffer, palette, transparentIndex, stride, width, height, depth, hashCode);
            AlterPixbufForTransparency(fileName, frame.Image.Image);
            return frame;
        }

        /// <summary>
        /// Alters a pixel buffer to account for transparency settings.
        /// </summary>
        /// <param name="fileName">The path to the GIF file from which the image was loaded, in case an alpha color table exists.</param>
        /// <param name="pixbuf">The <see cref="T:Gdk.Pixbuf"/> to be altered.</param>
        private void AlterPixbufForTransparency(string fileName, Pixbuf pixbuf)
        {
            if (!IsAlphaBlended)
                return;

            string mapFilePath = System.IO.Path.ChangeExtension(fileName, AlphaRemappingTable.FileExtension);
            if (File.Exists(mapFilePath))
            {
                AlphaRemappingTable map = new AlphaRemappingTable();
                map.LoadMap(mapFilePath);

                // Loop over the pixels in each row (to account for stride width of the source).
                IntPtr start = pixbuf.Pixels;
                byte[] scan = new byte[pixbuf.Rowstride];
                for (int row = 0; row < pixbuf.Height; row++)
                {
                    // Copy the scan line into a managed array.
                    IntPtr rowPtr = IntPtr.Add(start, row * pixbuf.Rowstride);
                    Marshal.Copy(rowPtr, scan, 0, pixbuf.Rowstride);
                    for (int x = 0; x < pixbuf.Width; x++)
                    {
                        // Map RGB colors to ARGB colors.
                        int offset = 4 * x;
                        ArgbColor argbColor;
                        if (map.TryGetMapping(new RgbColor(scan[offset + 0], scan[offset + 1], scan[offset + 2]), out argbColor))
                        {
                            scan[offset + 0] = argbColor.R;
                            scan[offset + 1] = argbColor.G;
                            scan[offset + 2] = argbColor.B;
                            scan[offset + 3] = argbColor.A;
                        }
                    }
                    // Copy the altered array back into the source.
                    Marshal.Copy(scan, 0, rowPtr, pixbuf.Rowstride);
                }
            }
        }

        /// <summary>
        /// Loads the given collection of file paths as images in a format that this
        /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> can display.
        /// </summary>
        /// <param name="imageFilePaths">The collection of paths to image files that should be loaded by the interface. Any images not
        /// loaded by this method will be loaded on demand. This method can be called asynchronously to ensure all images become loaded but
        /// without incurring the delay up front.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="imageFilePaths"/> is null.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void LoadImages(IEnumerable<string> imageFilePaths)
        {
            LoadImages(imageFilePaths, null);
        }

        /// <summary>
        /// Loads the given collection of file paths as images in a format that this
        /// <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> can display.
        /// </summary>
        /// <param name="imageFilePaths">The collection of paths to image files that should be loaded by the interface. Any images not
        /// loaded by this method will be loaded on demand.</param>
        /// <param name="imageLoadedHandler">An <see cref="T:System.EventHandler"/> that is raised when an image is loaded.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="imageFilePaths"/> is null.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void LoadImages(IEnumerable<string> imageFilePaths, EventHandler imageLoadedHandler)
        {
            Argument.EnsureNotNull(imageFilePaths, "imageFilePaths");

            if (disposed)
                throw new ObjectDisposedException(GetType().FullName);

            foreach (string imageFilePath in imageFilePaths)
                images.Add(imageFilePath);
            images.InitializeAll(false, (sender, e) => imageLoadedHandler.Raise(this));
        }

        /// <summary>
        /// Creates an <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> specific context menu for the given set of menu
        /// items.
        /// </summary>
        /// <param name="menuItems">The collections of items to be displayed in the menu.</param>
        /// <returns>An <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> specific context menu.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="menuItems"/> is null.</exception>
        public ISimpleContextMenu CreateContextMenu(IEnumerable<ISimpleContextMenuItem> menuItems)
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().FullName);

            GtkContextMenu menu = null;
            ApplicationInvoke(() => menu = new GtkContextMenu(this, menuItems));
            contextMenus.AddLast(menu);
            return menu;
        }

        /// <summary>
        /// Opens the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/>.
        /// </summary>
        public void Open()
        {
        }

        /// <summary>
        /// Hides the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/>.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void Hide()
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().FullName);

            ApplicationInvoke(() =>
            {
                foreach (GraphicsWindow window in spriteWindows.Values)
                    window.Hide();
            });
        }

        /// <summary>
        /// Shows the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/>.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void Show()
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().FullName);

            ApplicationInvoke(() =>
            {
                foreach (GraphicsWindow window in spriteWindows.Values)
                    window.Show();
            });
        }

        /// <summary>
        /// Freezes the display of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/>.
        /// </summary>
        public void Pause()
        {
            paused = true;
        }

        /// <summary>
        /// Resumes display of the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> from a paused state.
        /// </summary>
        public void Unpause()
        {
            paused = false;
        }

        /// <summary>
        /// Creates a new <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/> for the given sprite with the
        /// current window settings, attaches the appropriate event handlers and realizes the window.
        /// </summary>
        /// <param name="sprite">The <see cref="T:CSDesktopPonies.SpriteManagement.ISprite"/> that should be drawn by this window.</param>
        /// <returns>The new <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface.GraphicsWindow"/>.</returns>
        private GraphicsWindow CreateWindow(ISprite sprite)
        {
            GraphicsWindow window = null;
            ApplicationInvoke(() =>
            {
                window = new GraphicsWindow().SetupSafely(win =>
                {
                    win.Sprite = sprite;
                    win.Title = windowTitle;
                    win.Icon = windowIcon;
                    win.KeepAbove = windowTopmost;
                    win.ButtonPressEvent += GraphicsWindow_ButtonPressEvent;
                    win.ButtonReleaseEvent += GraphicsWindow_ButtonReleaseEvent;
                    win.KeyPressEvent += GraphicsWindow_KeyPressEvent;
                    win.Realize();
                });
            });
            return window;
        }

        /// <summary>
        /// Draws the given collection of sprites.
        /// </summary>
        /// <param name="sprites">The collection of sprites to draw.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sprites"/> is null.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The interface has been disposed.</exception>
        public void Draw(AsyncLinkedList<ISprite> sprites)
        {
            Argument.EnsureNotNull(sprites, "sprites");

            if (disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (paused)
                return;

            lock (drawSync)
            {
                // Remove all back references from windows to sprites. Any that are not restored indicate removed sprites.
                foreach (GraphicsWindow window in spriteWindows.Values)
                    window.Sprite = null;

                // Link sprites to windows, creating new windows as needed for new sprites.
                drawOrderedWindows.Clear();
                foreach (ISprite sprite in sprites)
                {
                    // Create a new window for a new sprite.
                    if (!spriteWindows.ContainsKey(sprite))
                        spriteWindows[sprite] = CreateWindow(sprite);

                    // Save the windows position in the draw order.
                    drawOrderedWindows.Add(spriteWindows[sprite]);

                    // Create a back reference to the sprite the window is responsible for, which indicates it is in use.
                    spriteWindows[sprite].Sprite = sprite;
                }

                /*
                // Set the stacking order of the windows.
                foreach (GraphicsWindow window in drawOrderedWindows)
                    if (window.Visible)
                        window.GdkWindow.Raise();
                    else
                        window.Show();
                */
                // FIXME: Refreshing the whole stacking order is highly draining and leads to flickering.
                // Implement a LinkedList based minimum moves restack.
                foreach (GraphicsWindow window in drawOrderedWindows)
                    ApplicationInvoke(() =>
                    {
                        if (!window.Visible)
                            window.Show();
                    });

                // Remove windows whose sprites have been removed (and thus were not re-linked).
                foreach (var kvp in spriteWindows)
                    if (kvp.Value.Sprite == null)
                    {
                        removedSprites.Add(kvp.Key);
                        ApplicationInvoke(() => kvp.Value.Destroy());
                    }
                foreach (ISprite sprite in removedSprites)
                    spriteWindows.Remove(sprite);
                removedSprites.Clear();

                // Draw each sprite in the collection to its own window, in the correct order.
                foreach (GraphicsWindow loopWindow in drawOrderedWindows)
                {
                    // C# 4 behavior. Using a loop variable in an anonymous expression will capture the final value, and not the current
                    // iteration. To do that a local copy must be made. This is fixed in C# 5.
                    GraphicsWindow window = loopWindow;

                    ISprite sprite = window.Sprite;
                    GtkFrame frame = images[sprite.ImagePath][sprite.CurrentTime];

                    // Gtk# operations need to be invoked on the main thread. Although they will usually succeed, eventually an invalid
                    // unmanaged memory access is likely to result.
                    // By invoking within the loop, the actions are chunked up so that the message pump doesn't become tied down for too
                    // long, which allows it to continue to respond to other messages in a timely manner.
                    ApplicationInvoke(() =>
                    {
                        // Flip the image, and set it on the window, as later operations rely on it.
                        frame.Flip(sprite.FlipImage);
                        window.CurrentImage = frame.Image;

                        // The window takes on the location and size of the sprite to draw.
                        window.GdkWindow.MoveResize(sprite.Region.X, sprite.Region.Y, sprite.Region.Width, sprite.Region.Height);

                        // Apply the image now the window is set up, by updating the clipping region and drawing it.
                        window.SetClip(sprite.Region.Width, sprite.Region.Height);
                        window.DrawFrame(sprite.Region.Width, sprite.Region.Height);

                        // Display any speech.
                        ISpeakingSprite speakingSprite = sprite as ISpeakingSprite;
                        if (speakingSprite != null && speakingSprite.IsSpeaking)
                            window.ShowSpeech(speakingSprite.SpeechText, sprite.Region.X + sprite.Region.Width / 2, sprite.Region.Y - 2);
                        else
                            window.HideSpeech();
                    });
                }
            }
        }

        /*
        private GraphicsWindow mainWindow;
        private static Gdk.Rectangle mainWindowRect = new Gdk.Rectangle(SWF.Screen.PrimaryScreen.WorkingArea.X,
                                                                        SWF.Screen.PrimaryScreen.WorkingArea.Y,
                                                                        SWF.Screen.PrimaryScreen.WorkingArea.Width,
                                                                        SWF.Screen.PrimaryScreen.WorkingArea.Height);
        private Pixmap mainWindowClip;

        public void Draw(AsyncLinkedList<ISprite> sprites)
        {
            Argument.EnsureNotNull(sprites, "sprites");

            if (disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (paused)
                return;

            lock (drawSync)
            {
                ApplicationInvoke(() => DrawInternal(sprites));
            }
        }

        private void DrawInternal(AsyncLinkedList<ISprite> sprites)
        {
            if (mainWindow == null)
            {
                mainWindow = CreateWindow(null);
                mainWindow.Move(0, 0);
                mainWindow.Resize(mainWindowRect.Width, mainWindowRect.Height);
                mainWindow.Show();
                using (Region region = Region.Rectangle(new Gdk.Rectangle(0, 0, 1, 1)))
                    mainWindow.GdkWindow.InputShapeCombineRegion(region, 0, 0);
                mainWindowClip = new Pixmap(null, mainWindowRect.Width, mainWindowRect.Height, 1);
            }

            mainWindow.GdkWindow.BeginPaintRect(mainWindowRect);
            using (Context context = CairoHelper.Create(mainWindow.GdkWindow),
                clipContext = CairoHelper.Create(mainWindowClip))
            {
                context.Antialias = Antialias.None;
                clipContext.Antialias = Antialias.None;

                context.SetSourceRGBA(0, 0, 0, 0);
                context.Operator = Operator.Source;
                context.Paint();

                clipContext.SetSourceRGB(0, 0, 0);
                clipContext.Operator = Operator.Source;
                clipContext.Paint();

                context.Operator = Operator.Over;
                clipContext.Operator = Operator.Add;
                foreach (ISprite sprite in sprites)
                {
                    GtkFrame frame = images[sprite.ImagePath][sprite.CurrentTime];
                    frame.Flip(sprite.FlipImage);

                    context.Save();
                    clipContext.Save();

                    context.Translate(sprite.Region.X, sprite.Region.Y);
                    CairoHelper.SetSourcePixbuf(context, frame.Image.Image, 0, 0);
                    context.Paint();

                    clipContext.Translate(sprite.Region.X, sprite.Region.Y);
                    CairoHelper.SetSourcePixmap(clipContext, frame.Image.Clip, 0, 0);
                    clipContext.Paint();

                    ISpeakingSprite speakingSprite = sprite as ISpeakingSprite;
                    if (speakingSprite != null && speakingSprite.IsSpeaking)
                    {
                        const int padding = 3;

                        context.SelectFontFace("Sans", FontSlant.Normal, FontWeight.Normal);
                        context.SetFontSize(12);
                        TextExtents extents = context.TextExtents(speakingSprite.SpeechText);
                        PointD location = new PointD(sprite.Region.Width / 2d, 0);
                        location.X -= extents.Width / 2 + padding;
                        location.Y -= extents.Height + 2 * padding + 2;

                        context.Translate(location.X, location.Y);
                        context.Rectangle(-padding, -padding, extents.Width + 2 * padding, extents.Height + 2 * padding);
                        context.SetSourceRGB(1, 1, 1);
                        context.FillPreserve();
                        context.SetSourceRGB(0, 0, 0);
                        context.LineWidth = 1;
                        context.Stroke();

                        clipContext.Translate(location.X, location.Y);
                        clipContext.Rectangle(-padding, -padding, extents.Width + 2 * padding, extents.Height + 2 * padding);
                        clipContext.SetSourceRGB(1, 1, 1);
                        clipContext.FillPreserve();
                        clipContext.LineWidth = 1;
                        clipContext.Stroke();

                        context.Translate(0, extents.Height);
                        context.ShowText(speakingSprite.SpeechText);
                    }

                    context.Restore();
                    clipContext.Restore();
                }

                #region Timings Graph
                if (Collector != null && Collector.Count != 0)
                {
                    // Set location and get area of graph draw.
                    SD.Point graphLocation = new SD.Point(10, 10);
                    SD.Rectangle recorderGraphArea = Collector.SetGraphingAttributes(graphLocation, 150, 1, 1.5f);
                    clipContext.SetSourceRGB(1, 1, 1);
                    clipContext.Rectangle(recorderGraphArea.X, recorderGraphArea.Y, recorderGraphArea.Width, recorderGraphArea.Height);
                    clipContext.Fill();

                    // Display a graph of frame times and garbage collections.
                    Collector.DrawGraph(context);
                }
                #endregion
            }
            if (mainWindow.SupportsRgba)
                mainWindow.GdkWindow.InputShapeCombineMask(mainWindowClip, 0, 0);
            else
                mainWindow.GdkWindow.ShapeCombineMask(mainWindowClip, 0, 0);
            mainWindow.GdkWindow.EndPaint();
        }
        */

        /// <summary>
        /// Closes the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/>.
        /// </summary>
        public void Close()
        {
            Dispose();
            InterfaceClosed.Raise(this);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="T:CSDesktopPonies.SpriteManagement.GtkSpriteInterface"/> object.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                lock (drawSync)
                {
                    ApplicationInvoke(() =>
                    {
                        /*
                        mainWindow.Destroy();
                        mainWindowClip.Dispose();
                        */

                        foreach (GraphicsWindow window in spriteWindows.Values)
                            window.Hide();
                        foreach (GraphicsWindow window in spriteWindows.Values)
                            window.Destroy();
                        foreach (GtkContextMenu menu in contextMenus)
                            menu.Destroy();
                    });
                    
                    Application.Quit();
                    spriteWindows.Clear();

                    if (windowIcon != null)
                        windowIcon.Dispose();

                    foreach (AnimatedImage<GtkFrame> image in images.InitializedValues)
                        image.Dispose();

                    // The images occupy a large amount of managed memory, so reclaim that now.
                    System.GC.Collect();
                }
            }
        }
    }
}