namespace CSDesktopPonies.SpriteManagement
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using CSDesktopPonies.Core;
    using CSDesktopPonies.Interop.Win32;

    /// <summary>
    /// Extends a <see cref="T:System.Windows.Forms.Form"/> to provide the ability to use alpha blending.
    /// </summary>
    internal class AlphaForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.AlphaForm"/> class.
        /// </summary>
        /// <exception cref="T:System.PlatformNotSupportedException">The operating system is not Windows.</exception>
        public AlphaForm()
        {
            if (!OperatingSystemInfo.IsWindows)
                throw new PlatformNotSupportedException(
                    "Cannot create an instance of this class on non-Windows platforms due to use of platform invoke.");
            FormBorderStyle = FormBorderStyle.None;
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
        /// Displays the 32bpp bitmap as the image of the form. Semi-transparent areas are alpha blended. Colors must use pre-multiplied
        /// alpha.
        /// </summary>
        /// <param name="bitmap">The bitmap to use as the image of the form.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="bitmap"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">The <see cref="T:System.Drawing.Imaging.PixelFormat"/> of
        /// <paramref name="bitmap"/> was not <see cref="P:System.Drawing.Imaging.PixelFormat.Format32bppArgb"/>.</exception>
        /// <exception cref="T:System.ComponentModel.Win32Exception">A Win32 error occurred.</exception>
        public void SetBitmap(Bitmap bitmap)
        {
            SetBitmap(bitmap, 255);
        }

        /// <summary>
        /// Displays the 32bpp bitmap as the image of the form. Semi-transparent areas are alpha blended. Colors must use pre-multiplied
        /// alpha. The transparency of the whole image is also scaled based on the <paramref name="opacity"/> value.
        /// </summary>
        /// <param name="bitmap">The bitmap to use as the image of the form.</param>
        /// <param name="opacity">The opacity of the image. Where 255 is opaque, and 0 is transparent.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="bitmap"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">The <see cref="T:System.Drawing.Imaging.PixelFormat"/> of
        /// <paramref name="bitmap"/> was not <see cref="P:System.Drawing.Imaging.PixelFormat.Format32bppArgb"/>.</exception>
        /// <exception cref="T:System.ComponentModel.Win32Exception">A Win32 error occurred.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
            Justification = "Following Windows API conventions, which use Hungarian notation.")]
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand,
            Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        public void SetBitmap(Bitmap bitmap, byte opacity)
        {
            Argument.EnsureNotNull(bitmap, "bitmap");

            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ArgumentException("The PixelFormat of bitmap must be Format32bppArgb.");

            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hPrevBitmap = IntPtr.Zero;
            IntPtr screenDC = NativeMethods.GetDC(IntPtr.Zero);
            IntPtr memDC = NativeMethods.CreateCompatibleDC(screenDC);

            Win32Exception cleanupEx = null;
            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                hPrevBitmap = NativeMethods.SelectObject(memDC, hBitmap);
                if (hPrevBitmap == IntPtr.Zero)
                    throw new Win32Exception();

                SIZE dstSize = new SIZE(bitmap.Width, bitmap.Height);
                UpdateWindow(screenDC, memDC, ref dstSize, opacity);
            }
            finally
            {
                if (NativeMethods.ReleaseDC(IntPtr.Zero, screenDC) == 0)
                    cleanupEx = new Win32Exception();
                if (hBitmap != IntPtr.Zero)
                {
                    NativeMethods.SelectObject(memDC, hPrevBitmap);
                    NativeMethods.DeleteObject(hBitmap);
                }
                if (!NativeMethods.DeleteDC(memDC))
                    cleanupEx = new Win32Exception();
            }
            if (cleanupEx != null)
                throw cleanupEx;
        }

        /// <summary>
        /// Updates the window by applying a new surface graphic.
        /// </summary>
        /// <param name="screenDC">A handle to the DC for the screen.</param>
        /// <param name="memDC">A handle to the DC for the surface that defines the window.</param>
        /// <param name="dstSize">A pointer to a structure that specifies the new size of the window.</param>
        /// <param name="opacity">Specifies an alpha transparency value to be used on the entire source bitmap.</param>
        private void UpdateWindow(IntPtr screenDC, IntPtr memDC, ref SIZE dstSize, byte opacity)
        {
            POINT dstPos = new POINT(Left, Top);
            POINT srcPos = POINT.Empty;
            BLENDFUNCTION blend = new BLENDFUNCTION(BlendOp.AC_SRC_OVER, opacity, AlphaFormat.AC_SRC_ALPHA);
            if (!NativeMethods.UpdateLayeredWindow(new HandleRef(this, Handle), screenDC, ref dstPos, ref dstSize,
                memDC, ref srcPos, new COLORREF(), ref blend, UlwFlags.ULW_ALPHA))
                throw new Win32Exception();
        }
    }
}