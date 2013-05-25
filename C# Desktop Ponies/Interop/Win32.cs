namespace CSDesktopPonies.Interop.Win32
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The <see cref="T:CSDesktopPonies.Win32.POINT"/> structure defines the x- and y- coordinates of a point.
    /// </summary>
    internal struct POINT
    {
        /// <summary>
        /// Represents a <see cref="T:CSDesktopPonies.Win32.POINT"/> structure with its x- and y-coordinates set to zero.
        /// </summary>
        public static readonly POINT Empty = new POINT();

        /// <summary>
        /// The x-coordinate of the point.
        /// </summary>
        public int X;
        /// <summary>
        /// The y-coordinate of the point.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Win32.POINT"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns a string representation of this <see cref="T:CSDesktopPonies.Win32.POINT"/> structure.
        /// </summary>
        /// <returns>A string representation of this <see cref="T:CSDesktopPonies.Win32.POINT"/> structure.</returns>
        public override string ToString()
        {
            return "{X=" + X + " Y=" + Y + "}";
        }
    }

    /// <summary>
    /// The <see cref="T:CSDesktopPonies.Win32.SIZE"/> structure specifies the width and height of a rectangle.
    /// </summary>
    internal struct SIZE
    {
        /// <summary>
        /// Represents a <see cref="T:CSDesktopPonies.Win32.SIZE"/> structure with its width and height set to zero.
        /// </summary>
        public static readonly SIZE Empty = new SIZE();

        /// <summary>
        /// Specifies the rectangle's width. The units depend on which function uses this.
        /// </summary>
        public int CX;
        /// <summary>
        /// Specifies the rectangle's height. The units depend on which function uses this.
        /// </summary>
        public int CY;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Win32.SIZE"/> structure.
        /// </summary>
        /// <param name="cx">The width of the rectangle.</param>
        /// <param name="cy">The height of the rectangle.</param>
        public SIZE(int cx, int cy)
        {
            CX = cx;
            CY = cy;
        }

        /// <summary>
        /// Returns a string representation of this <see cref="T:CSDesktopPonies.Win32.SIZE"/> structure.
        /// </summary>
        /// <returns>A string representation of this <see cref="T:CSDesktopPonies.Win32.SIZE"/> structure.</returns>
        public override string ToString()
        {
            return "{CX=" + CX + " CY=" + CY + "}";
        } 
    }

    /// <summary>
    /// The <see cref="T:CSDesktopPonies.Win32.BLENDFUNCTION"/> structure controls blending by specifying the blending functions for source
    /// and destination bitmaps.
    /// </summary>
    internal struct BLENDFUNCTION
    {
        /// <summary>
        /// The source blend operation.
        /// </summary>
        public BlendOp BlendOp;
        /// <summary>
        /// Must be zero.
        /// </summary>
        public byte BlendFlags;
        /// <summary>
        /// Specifies an alpha transparency value to be used on the entire source bitmap.
        /// </summary>
        public byte SourceConstantAlpha;
        /// <summary>
        /// This member controls the way the source and destination bitmaps are interpreted.
        /// </summary>
        public AlphaFormat AlphaFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Win32.BLENDFUNCTION"/> structure.
        /// </summary>
        /// <param name="blendOp">The source blend operation.</param>
        /// <param name="sourceConstantAlpha">Specifies an alpha transparency value to be used on the entire source bitmap. The
        /// <see cref="P:CSDesktopPonies.Win32.BLENDFUNCTION.SourceConstantAlpha"/> value is combined with any per-pixel
        /// alpha values in the source bitmap. If you set <see cref="P:CSDesktopPonies.Win32.BLENDFUNCTION.SourceConstantAlpha"/> to 0, it
        /// is assumed that your image is transparent. Set the <see cref="P:CSDesktopPonies.Win32.BLENDFUNCTION.SourceConstantAlpha"/>
        /// value to 255 (opaque) when you only want to use per-pixel alpha values.</param>
        /// <param name="alphaFormat">This member controls the way the source and destination bitmaps are interpreted.</param>
        public BLENDFUNCTION(BlendOp blendOp, byte sourceConstantAlpha, AlphaFormat alphaFormat)
        {
            BlendOp = blendOp;
            BlendFlags = 0;
            SourceConstantAlpha = sourceConstantAlpha;
            AlphaFormat = alphaFormat;
        }
    }

    /// <summary>
    /// Specifies the blend operation.
    /// </summary>
    internal enum BlendOp : byte
    {
        /// <summary>
        /// The source bitmap is placed over the destination bitmap based on the alpha values of the source pixels.
        /// </summary>
        AC_SRC_OVER = 0x00
    }

    /// <summary>
    /// Specifies the format of the alpha channel.
    /// </summary>
    internal enum AlphaFormat : byte
    {
        /// <summary>
        /// This flag is set when the bitmap has an Alpha channel (that is, per-pixel alpha). Note that the APIs use pre-multiplied
        /// alpha, which means that the red, green and blue channel values in the bitmap must be pre-multiplied with the alpha channel
        /// value. For example, if the alpha channel value is x, the red, green and blue channels must be multiplied by x and divided
        /// by 0xff prior to the call.
        /// </summary>
        AC_SRC_ALPHA = 0x01
    }

    /// <summary>
    /// Flags for <see cref="M:CSDesktopPonies.Win32.NativeMethods.UpdateLayeredWindow"/>.
    /// </summary>
    internal enum UlwFlags : uint
    {
        /// <summary>
        /// Use when hdcSrc is NULL.
        /// </summary>
        ZERO = 0,
        /// <summary>
        /// Use crKey as the transparency color.
        /// </summary>
        ULW_COLORKEY = 0x00000001,
        /// <summary>
        /// Use pblend as the blend function. If the display mode is 256 colors or less, the effect of this value is the same as the
        /// effect of ULW_OPAQUE.
        /// </summary>
        ULW_ALPHA = 0x00000002,
        /// <summary>
        /// Draw an opaque layered window.
        /// </summary>
        ULW_OPAQUE = 0x00000004
    }

    /// <summary>
    /// The following are the extended window styles.
    /// </summary>
    [Flags]
    internal enum ExtendedWindowStyles : int
    {
        /// <summary>
        /// The window accepts drag-drop files.
        /// </summary>
        WS_EX_ACCEPTFILES = 0x00000010,
        /// <summary>
        /// Forces a top-level window onto the taskbar when the window is visible.
        /// </summary>
        WS_EX_APPWINDOW = 0x00040000,
        /// <summary>
        /// The window has a border with a sunken edge.
        /// </summary>
        WS_EX_CLIENTEDGE = 0x00000200,
        /// <summary>
        /// Paints all descendants of a window in bottom-to-top painting order using double-buffering. With WS_EX_COMPOSITED set, all
        /// descendants of a window get bottom-to-top painting order using double-buffering. Bottom-to-top painting order allows a
        /// descendent window to have translucency (alpha) and transparency (color-key) effects, but only if the descendent window also has
        /// the WS_EX_TRANSPARENT bit set. Double-buffering allows the window and its descendents to be painted without flicker. This
        /// cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. Windows 2000: This style is not supported.
        /// </summary>
        WS_EX_COMPOSITED = 0x02000000,
        /// <summary>
        /// The title bar of the window includes a question mark. When the user clicks the question mark, the cursor changes to a question
        /// mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message. The child window should pass
        /// the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help
        /// application displays a pop-up window that typically contains help for the child window. WS_EX_CONTEXTHELP cannot be used with
        /// the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
        /// </summary>
        WS_EX_CONTEXTHELP = 0x00000400,
        /// <summary>
        /// The window itself contains child windows that should take part in dialog box navigation. If this style is specified, the dialog
        /// manager recurses into children of this window when performing navigation operations such as handling the TAB key, an arrow key,
        /// or a keyboard mnemonic.
        /// </summary>
        WS_EX_CONTROLPARENT = 0x00010000,
        /// <summary>
        /// The window has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in
        /// the dwStyle parameter.
        /// </summary>
        WS_EX_DLGMODALFRAME = 0x00000001,
        /// <summary>
        /// The window is a layered window. This style cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
        /// Windows 8: The WS_EX_LAYERED style is supported for top-level windows and child windows. Previous Windows versions support
        /// WS_EX_LAYERED only for top-level windows.
        /// </summary>
        WS_EX_LAYERED = 0x00080000,
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the horizontal origin of
        /// the window is on the right edge. Increasing horizontal values advance to the left.
        /// </summary>
        WS_EX_LAYOUTRTL = 0x00400000,
        /// <summary>
        /// The window has generic left-aligned properties. This is the default.
        /// </summary>
        WS_EX_LEFT = 0x00000000,
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if
        /// present) is to the left of the client area. For other languages, the style is ignored.
        /// </summary>
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        /// <summary>
        /// The window text is displayed using left-to-right reading-order properties. This is the default.
        /// </summary>
        WS_EX_LTRREADING = 0x00000000,
        /// <summary>
        /// The window is a MDI child window.
        /// </summary>
        WS_EX_MDICHILD = 0x00000040,
        /// <summary>
        /// A top-level window created with this style does not become the foreground window when the user clicks it. The system does not
        /// bring this window to the foreground when the user minimizes or closes the foreground window. To activate the window, use the
        /// SetActiveWindow or SetForegroundWindow function. The window does not appear on the taskbar by default. To force the window to
        /// appear on the taskbar, use the WS_EX_APPWINDOW style.
        /// </summary>
        WS_EX_NOACTIVATE = 0x08000000,
        /// <summary>
        /// The window does not pass its window layout to its child windows.
        /// </summary>
        WS_EX_NOINHERITLAYOUT = 0x00100000,
        /// <summary>
        /// The child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or
        /// destroyed.
        /// </summary>
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        /// <summary>
        /// The window is an overlapped window.
        /// </summary>
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
        /// <summary>
        /// The window is palette window, which is a modeless dialog box that presents an array of commands.
        /// </summary>
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
        /// <summary>
        /// The window has generic "right-aligned" properties. This depends on the window class. This style has an effect only if the shell
        /// language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored. Using
        /// the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT style, respectively.
        /// Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.
        /// </summary>
        WS_EX_RIGHT = 0x00001000,
        /// <summary>
        /// The vertical scroll bar (if present) is to the right of the client area. This is the default.
        /// </summary>
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is
        /// displayed using right-to-left reading-order properties. For other languages, the style is ignored.
        /// </summary>
        WS_EX_RTLREADING = 0x00002000,
        /// <summary>
        /// The window has a three-dimensional border style intended to be used for items that do not accept user input.
        /// </summary>
        WS_EX_STATICEDGE = 0x00020000,
        /// <summary>
        /// The window is intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar,
        /// and the window title is drawn using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears
        /// when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on the title bar. However, you can
        /// display the system menu by right-clicking or by typing ALT+SPACE.
        /// </summary>
        WS_EX_TOOLWINDOW = 0x00000080,
        /// <summary>
        /// The window should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To
        /// add or remove this style, use the SetWindowPos function.
        /// </summary>
        WS_EX_TOPMOST = 0x00000008,
        /// <summary>
        /// The window should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
        /// The window appears transparent because the bits of underlying sibling windows have already been painted. To achieve
        /// transparency without these restrictions, use the SetWindowRgn function.
        /// </summary>
        WS_EX_TRANSPARENT = 0x00000020,
        /// <summary>
        /// The window has a border with a raised edge.
        /// </summary>
        WS_EX_WINDOWEDGE = 0x00000100
    }

    /// <summary>
    /// The <see cref="T:CSDesktopPonies.Win32.COLORREF"/> value is used to specify an RGB color.
    /// </summary>
    internal struct COLORREF
    {
        /// <summary>
        /// The value of this color in a 0x00bbggrr format.
        /// </summary>
        public int Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Win32.COLORREF"/> structure from a Win32 color.
        /// </summary>
        /// <param name="win32color">A win32 color in the format 0x00bbggrr.</param>
        public COLORREF(int win32color)
        {
            Value = win32color & 0x00FFFFFF;
        }
    }

    /// <summary>
    /// Exposes Windows API functions.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
        Justification = "Following Windows API conventions, which use Hungarian notation.")]
    internal static class NativeMethods
    {
        /// <summary>
        /// Location of the library implementing the Windows USER component.
        /// </summary>
        private const string user = "user32.dll";
        /// <summary>
        /// Location of the library implementing the Graphics Device Interface.
        /// </summary>
        private const string gdi = "gdi32.dll";

        /// <summary>
        /// Updates the position, size, shape, content, and translucency of a layered window.
        /// </summary>
        /// <param name="hwnd">A handle to a layered window. A layered window is created by specifying WS_EX_LAYERED when creating the
        /// window with the CreateWindowEx function.</param>
        /// <param name="hdcDst">A handle to a DC for the screen. This handle is obtained by specifying NULL when calling the function. It
        /// is used for palette color matching when the window contents are updated. If hdcDst is NULL, the default palette will be used.
        /// If hdcSrc is NULL, hdcDst must be NULL.</param>
        /// <param name="pptDst">A pointer to a structure that specifies the new screen position of the layered window. If the current
        /// position is not changing, pptDst can be NULL.</param>
        /// <param name="psize">A pointer to a structure that specifies the new size of the layered window. If the size of the window is
        /// not changing, psize can be NULL. If hdcSrc is NULL, psize must be NULL.</param>
        /// <param name="hdcSrc">A handle to a DC for the surface that defines the layered window. This handle can be obtained by calling
        /// the CreateCompatibleDC function. If the shape and visual context of the window are not changing, hdcSrc can be NULL.</param>
        /// <param name="pptSrc">A pointer to a structure that specifies the location of the layer in the device context. If hdcSrc is
        /// NULL, pptSrc should be NULL.</param>
        /// <param name="crKey">A structure that specifies the color key to be used when composing the layered window. To generate a
        /// COLORREF, use the RGB macro.</param>
        /// <param name="pblend">A pointer to a structure that specifies the transparency value to be used when composing the layered
        /// window.</param>
        /// <param name="dwFlags">Flags. If hdcSrc is NULL, dwFlags should be zero.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get
        /// extended error information, call GetLastError.</returns>
        [DllImport(user, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UpdateLayeredWindow(HandleRef hwnd, IntPtr hdcDst, [In] ref POINT pptDst, [In] ref SIZE psize,
            IntPtr hdcSrc, [In] ref POINT pptSrc, COLORREF crKey, [In] ref BLENDFUNCTION pblend, UlwFlags dwFlags);

        /// <summary>
        /// The GetDC function retrieves a handle to a device context (DC) for the client area of a specified window or for the entire
        /// screen. You can use the returned handle in subsequent GDI functions to draw in the DC. The device context is an opaque data
        /// structure, whose values are used internally by GDI.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose DC is to be retrieved. If this value is NULL, GetDC retrieves the DC for the
        /// entire screen.</param>
        /// <returns>If the function succeeds, the return value is a handle to the DC for the specified window's client area. If the
        /// function fails, the return value is NULL.</returns>
        [DllImport(user, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// The ReleaseDC function releases a device context (DC), freeing it for use by other applications. The effect of the ReleaseDC
        /// function depends on the type of DC. It frees only common and window DCs. It has no effect on class or private DCs.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose DC is to be released.</param>
        /// <param name="hDC">A handle to the DC to be released.</param>
        /// <returns>The return value indicates whether the DC was released. If the DC was released, the return value is 1. If the DC was
        /// not released, the return value is zero.</returns>
        [DllImport(user, SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// The CreateCompatibleDC function creates a memory device context (DC) compatible with the specified device.
        /// </summary>
        /// <param name="hDC">A handle to an existing DC. If this handle is NULL, the function creates a memory DC compatible with the
        /// application's current screen.</param>
        /// <returns>If the function succeeds, the return value is the handle to a memory DC. If the function fails, the return value is
        /// NULL.</returns>
        [DllImport(gdi, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        /// <summary>
        /// The DeleteDC function deletes the specified device context (DC).
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.</returns>
        [DllImport(gdi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// The SelectObject function selects an object into the specified device context (DC). The new object replaces the previous object
        /// of the same type.
        /// </summary>
        /// <param name="hDC">A handle to the DC.</param>
        /// <param name="hObject">A handle to the object to be selected.</param>
        /// <returns>If the selected object is not a region and the function succeeds, the return value is a handle to the object being
        /// replaced. If the selected object is a region and the function succeeds, a region is returned. If an error occurs and the
        /// selected object is not a region, the return value is NULL. Otherwise, it is HGDI_ERROR.</returns>
        [DllImport(gdi, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        /// <summary>
        /// The DeleteObject function deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources
        /// associated with the object. After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="hObject">A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the specified handle is not valid or is currently selected
        /// into a DC, the return value is zero.</returns>
        [DllImport(gdi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}