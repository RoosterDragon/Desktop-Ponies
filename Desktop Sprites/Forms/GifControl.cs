namespace DesktopSprites.Forms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using DesktopSprites.Core;
    using DesktopSprites.SpriteManagement;

    /// <summary>
    /// Displays one frame bitmap and information about it.
    /// </summary>
    public sealed partial class GifControl : UserControl
    {
        /// <summary>
        /// The frame this control displays.
        /// </summary>
        private GifFrame<BitmapFrame> frame;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.DesktopPonies.GifControl"/> class to display the given frame.
        /// </summary>
        /// <param name="frame">The frame to be displayed.</param>
        /// <param name="info">The information string to be displayed.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="frame"/> is null.</exception>
        public GifControl(GifFrame<BitmapFrame> frame, string info)
        {
            Argument.EnsureNotNull(frame, "frame");

            InitializeComponent();
            
            this.frame = frame;
            FrameInfo.Text = info;

            Width = Math.Max(frame.Image.Width + 8, FrameInfo.Width) + Padding.Horizontal;
            Height = frame.Image.Height + FrameInfo.Height + Padding.Vertical + 4;

            Disposed += (sender, e) => frame.Image.Dispose();
        }

        /// <summary>
        /// Raised when the control is painted.
        /// Draws the frame bitmap to the screen.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GifControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(frame.Image.Image, 3, 3);
            e.Graphics.DrawRectangle(Pens.Black, 2, 2, frame.Image.Width + 1, frame.Image.Height + 1);
        }
    }
}