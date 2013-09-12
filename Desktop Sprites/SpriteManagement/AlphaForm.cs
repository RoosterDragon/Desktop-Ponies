namespace DesktopSprites.SpriteManagement
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using DesktopSprites.Core;
    using DesktopSprites.Interop.Win32;

    /// <summary>
    /// Extends a <see cref="T:System.Windows.Forms.Form"/> to provide the ability to use alpha blending.
    /// </summary>
    internal class AlphaForm : Form
    {
        /// <summary>
        /// Handle to a device context for the screen.
        /// </summary>
        private readonly IntPtr hdcScreen;
        /// <summary>
        /// Handle to a device context for the background graphics buffer.
        /// </summary>
        private readonly IntPtr hdcBackground;
        /// <summary>
        /// Handle to a bitmap for the background buffer.
        /// </summary>
        private IntPtr hBitmap;
        /// <summary>
        /// Handle to the previous bitmap in the device context of the background buffer.
        /// </summary>
        private IntPtr hPrevBitmap;
        /// <summary>
        /// Graphics object operating on the background DC.
        /// </summary>
        private Graphics backgroundGraphics;
        /// <summary>
        /// Graphics buffer which may be drawn upon. Once drawing is complete, calling
        /// <see cref="M:DesktopSprites.SpriteManagement.AlphaForm.UpdateBackgroundGraphics"/> will update the form background with the
        /// graphics drawn onto this buffer. This buffer is recreated whenever the form is resized.
        /// </summary>
        public Graphics BackgroundGraphics
        {
            get
            {
                // Lazily initialize the graphics buffer.
                if (backgroundGraphics == null)
                {
                    using (var bitmap = new Bitmap(Width, Height))
                        hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                    hPrevBitmap = NativeMethods.SelectObject(hdcBackground, hBitmap);
                    if (hPrevBitmap == IntPtr.Zero)
                        throw new Win32Exception();
                    backgroundGraphics = Graphics.FromHdc(hdcBackground);
                }
                return backgroundGraphics;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.AlphaForm"/> class.
        /// </summary>
        /// <exception cref="T:System.PlatformNotSupportedException">The operating system is not Windows.</exception>
        public AlphaForm()
        {
            if (!OperatingSystemInfo.IsWindows)
                throw new PlatformNotSupportedException(
                    "Cannot create an instance of this class on non-Windows platforms due to use of platform invoke.");
            FormBorderStyle = FormBorderStyle.None;

            hdcScreen = NativeMethods.GetDC(IntPtr.Zero);
            hdcBackground = NativeMethods.CreateCompatibleDC(hdcScreen);
        }

        /// <summary>
        /// Gives the form the layered extended window style.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= (int)ExtendedWindowStyles.WS_EX_LAYERED;
                return cp;
            }
        }

        /// <summary>
        /// Releases the buffers when the form resizes.
        /// </summary>
        /// <param name="e">Data about the event.</param>
        protected override void OnResize(EventArgs e)
        {
            ReleaseBuffers(true);
            base.OnResize(e);
        }

        /// <summary>
        /// Releases the buffer resources.
        /// </summary>
        /// <param name="disposing">Indicates if managed resources should be disposed in addition to unmanaged resources; otherwise, only
        /// unmanaged resources should be disposed.</param>
        private void ReleaseBuffers(bool disposing)
        {
            if (disposing && backgroundGraphics != null)
            {
                backgroundGraphics.Dispose();
                backgroundGraphics = null;
            }
            if (hBitmap != IntPtr.Zero && hPrevBitmap != IntPtr.Zero)
            {
                NativeMethods.SelectObject(hdcBackground, hPrevBitmap);
                NativeMethods.DeleteObject(hBitmap);
            }
        }

        /// <summary>
        /// Updates the form to display the image currently rendered in the
        /// <see cref="P:DesktopSprites.SpriteManagement.AlphaForm.BackgroundGraphics"/> object. Semi-transparent areas will be
        /// alpha-blended.
        /// </summary>
        /// <exception cref="T:System.ComponentModel.Win32Exception">A Win32 error occurred.</exception>
        public void UpdateBackgroundGraphics()
        {
            UpdateBackgroundGraphics(255);
        }

        /// <summary>
        /// Updates the form to display the image currently rendered in the
        /// <see cref="P:DesktopSprites.SpriteManagement.AlphaForm.BackgroundGraphics"/> object. Semi-transparent areas will be
        /// alpha-blended. The transparency of the whole image is also scaled based on the <paramref name="opacity"/> value.
        /// </summary>
        /// <param name="opacity">The opacity of the image. Where 255 is opaque, and 0 is transparent.</param>
        /// <exception cref="T:System.ComponentModel.Win32Exception">A Win32 error occurred.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
            Justification = "Following Windows API conventions, which use Hungarian notation.")]
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand,
            Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        public void UpdateBackgroundGraphics(byte opacity)
        {
            POINT dstPos = new POINT(Left, Top);
            SIZE dstSize = new SIZE(Width, Height);
            POINT srcPos = POINT.Empty;
            BLENDFUNCTION blend = new BLENDFUNCTION(BlendOp.AC_SRC_OVER, opacity, AlphaFormat.AC_SRC_ALPHA);
            if (!NativeMethods.UpdateLayeredWindow(new HandleRef(this, Handle), hdcScreen, ref dstPos, ref dstSize,
                hdcBackground, ref srcPos, new COLORREF(), ref blend, UlwFlags.ULW_ALPHA))
                throw new Win32Exception();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (Disposing || IsDisposed)
                return;
            try
            {
                ReleaseBuffers(disposing);
                Win32Exception cleanupEx = null; 
                if (NativeMethods.ReleaseDC(IntPtr.Zero, hdcScreen) == 0)
                    cleanupEx = new Win32Exception();
                if (!NativeMethods.DeleteDC(hdcBackground))
                    cleanupEx = new Win32Exception();
                if (cleanupEx != null)
                    throw cleanupEx;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}