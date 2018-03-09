namespace DesktopSprites.SpriteManagement
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using DesktopSprites.Core;
    using DesktopSprites.Interop.Win32;

    /// <summary>
    /// Extends a <see cref="T:System.Windows.Forms.Form"/> to provide the ability to use alpha blending.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
        Justification = "Following Windows API conventions, which use Hungarian notation.")]
    internal class AlphaForm : Form
    {
        /// <summary>
        /// Handle to a device context for the screen.
        /// </summary>
        private IntPtr hdcScreen;
        /// <summary>
        /// Handle to a device context for the background graphics buffer.
        /// </summary>
        private IntPtr hdcBackground;
        /// <summary>
        /// Handle to a bitmap for the background buffer.
        /// </summary>
        private IntPtr hBitmap;
        /// <summary>
        /// Estimate of the number of unmanaged bytes thought to be allocated by the background buffer.
        /// </summary>
        private uint bitmapSizeInBytes;
        /// <summary>
        /// Size covered by the current buffers.
        /// </summary>
        private Size bitmapSize;
        /// <summary>
        /// Handle to the previous bitmap in the device context of the background buffer.
        /// </summary>
        private IntPtr hPrevBitmap;
        /// <summary>
        /// Graphics object operating on the background DC.
        /// </summary>
        private Graphics backgroundGraphics;
        /// <summary>
        /// Gets a graphics buffer which may be drawn upon. Once drawing is complete, calling
        /// <see cref="M:DesktopSprites.SpriteManagement.AlphaForm.UpdateBackgroundGraphics"/> will update the form background with the
        /// graphics drawn onto this buffer. This buffer is recreated whenever the form is resized.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed.</exception>
        public Graphics BackgroundGraphics
        {
            get
            {
                if (Disposing || IsDisposed)
                    throw new ObjectDisposedException(GetType().FullName);

                // Lazily initialize the graphics buffer.
                if (backgroundGraphics == null)
                {
                    InitializeBuffer();
                    backgroundGraphics = Graphics.FromHdc(hdcBackground);
                }
                return backgroundGraphics;
            }
        }
        /// <summary>
        /// A pointer to the array of raw pixels of the background graphics buffer.
        /// </summary>
        private unsafe int* backgroundData;
        /// <summary>
        /// Gets a pointer to the array of raw pixels of the background graphics buffer which may be operated on directly for speed. The
        /// array is of length Width * Height. Each element is a packed 32-bit ARGB value. This buffer is recreated whenever the form is
        /// resized.
        /// </summary>
        /// <returns>A lazily initialized pointer to the array of raw pixels for the background graphics buffer.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed.</exception>
        public unsafe int* GetBackgroundData()
        {
            if (Disposing || IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            InitializeBuffer();
            return backgroundData;
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

            if ((hdcScreen = NativeMethods.GetDC(new HandleRef(this, IntPtr.Zero))) == IntPtr.Zero)
                throw new Win32Exception();
            if ((hdcBackground = NativeMethods.CreateCompatibleDC(new HandleRef(this, hdcScreen))) == IntPtr.Zero)
                throw new Win32Exception();
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
            if (new Size(Width, Height) != bitmapSize)
                ReleaseBuffers(true);
            base.OnResize(e);
        }

        /// <summary>
        /// Initializes the unmanaged graphics buffer if it has yet to be initialized.
        /// </summary>
        private void InitializeBuffer()
        {
            unsafe
            {
                if (backgroundData != null)
                    return;
            }
            var bitmapInfo = new BITMAPINFO(new BITMAPINFOHEADER(
                Width, -Height, 32, BiFlags.BI_RGB, (uint)(Width * Height * 4), 0, 0, 0, 0));
            hBitmap = NativeMethods.CreateDIBSection(
                hdcBackground, ref bitmapInfo, DibFlags.DIB_RGB_COLORS, out IntPtr backgroundBits, IntPtr.Zero, 0);
            if (hBitmap == IntPtr.Zero)
                throw new Win32Exception();
            GC.AddMemoryPressure(bitmapSizeInBytes = bitmapInfo.bmiHeader.biSizeImage);
            bitmapSize = new Size(Width, Height);
            hPrevBitmap = NativeMethods.SelectObject(new HandleRef(this, hdcBackground), new HandleRef(this, hBitmap));
            if (hPrevBitmap == IntPtr.Zero)
                throw new Win32Exception();
            unsafe
            {
                backgroundData = (int*)backgroundBits.ToPointer();
            }
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
            unsafe
            {
                backgroundData = null;
            }
            if (hPrevBitmap != IntPtr.Zero)
            {
                NativeMethods.SelectObject(new HandleRef(this, hdcBackground), new HandleRef(this, hPrevBitmap));
                hPrevBitmap = IntPtr.Zero;
            }
            if (hBitmap != IntPtr.Zero)
            {
                GC.RemoveMemoryPressure(bitmapSizeInBytes);
                bitmapSizeInBytes = 0;
                bitmapSize = Size.Empty;
                if (!NativeMethods.DeleteObject(new HandleRef(this, hBitmap)))
                    throw new Win32Exception();
                hBitmap = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Updates the form to display the image currently rendered in the
        /// <see cref="P:DesktopSprites.SpriteManagement.AlphaForm.BackgroundGraphics"/> object. Semi-transparent areas will be
        /// alpha-blended.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed.</exception>
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
        /// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed.</exception>
        /// <exception cref="T:System.ComponentModel.Win32Exception">A Win32 error occurred.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
            Justification = "Following Windows API conventions, which use Hungarian notation.")]
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand,
            Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        public void UpdateBackgroundGraphics(byte opacity)
        {
            if (Disposing || IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            POINT dstPos = new POINT(Left, Top);
            SIZE dstSize = new SIZE(Width, Height);
            POINT srcPos = POINT.Empty;
            BLENDFUNCTION blend = new BLENDFUNCTION(BlendOp.AC_SRC_OVER, opacity, AlphaFormat.AC_SRC_ALPHA);
            if (!NativeMethods.UpdateLayeredWindow(new HandleRef(this, Handle), new HandleRef(this, hdcScreen), ref dstPos, ref dstSize,
                new HandleRef(this, hdcBackground), ref srcPos, new COLORREF(), ref blend, UlwFlags.ULW_ALPHA))
                throw new Win32Exception();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                ReleaseBuffers(disposing);
                Win32Exception cleanupEx = null;
                if (hdcScreen != IntPtr.Zero)
                {
                    if (NativeMethods.ReleaseDC(new HandleRef(this, IntPtr.Zero), new HandleRef(this, hdcScreen)) == 0)
                        cleanupEx = new Win32Exception();
                    else
                        hdcScreen = IntPtr.Zero;
                }
                if (hdcBackground != IntPtr.Zero)
                {
                    if (!NativeMethods.DeleteDC(new HandleRef(this, hdcBackground)))
                        cleanupEx = new Win32Exception();
                    else
                        hdcBackground = IntPtr.Zero;
                }
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
