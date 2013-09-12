namespace DesktopSprites.Interop.Unix
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Exposes native functions for Unix and Unix-like systems.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Location of the GNU C library.
        /// </summary>
        private const string glibc = "libc";

        /// <summary>
        /// Stores null-terminated strings of information identifying the current system into the structure referenced by
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Pointer to a structure into which system information is stored.</param>
        /// <returns>Returns the value 0 if successful; otherwise the value -1 is returned.</returns>
        [DllImport(glibc)]
        public static extern int uname(IntPtr name);
    }
}
