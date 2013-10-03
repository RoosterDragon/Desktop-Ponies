namespace DesktopSprites.Core
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides information about the operating system.
    /// </summary>
    public static class OperatingSystemInfo
    {
        /// <summary>
        /// Contains information about the current operating system.
        /// </summary>
        private static readonly OperatingSystem OsInfo = Environment.OSVersion;

        /// <summary>
        /// Initializes static members of the <see cref="T:DesktopSprites.Core.OperatingSystemInfo"/> class.
        /// </summary>
        static OperatingSystemInfo()
        {
            // Check for MacOSX, as PlatformID will simply return Unix under Mono.
            IntPtr buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.AllocHGlobal(8196);
                if (DesktopSprites.Interop.Unix.NativeMethods.uname(buffer) == 0)
                {
                    // The buffer contains 5 or 6 null-terminated char arrays, we will marshal the first one, containing the system name.
                    string osName = Marshal.PtrToStringAnsi(buffer);
                    IsMacOSX = osName == "Darwin";
                }
            }
            catch (Exception)
            {
                // Assume an exception indicates we are not on the MacOSX platform.
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Version"/> object that identifies the operating system.
        /// </summary>
        public static Version OSVersion
        {
            get { return OsInfo.Version; }
        }

        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows.
        /// </summary>
        public static bool IsWindows
        {
            get
            {
                return
                    OsInfo.Platform == PlatformID.Win32NT ||
                    OsInfo.Platform == PlatformID.Win32S ||
                    OsInfo.Platform == PlatformID.Win32Windows ||
                    OsInfo.Platform == PlatformID.WinCE;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Macintosh.
        /// </summary>
        public static bool IsMacOSX { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Unix.
        /// </summary>
        public static bool IsUnix
        {
            get { return OsInfo.Platform == PlatformID.Unix; }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Xbox 360.
        /// </summary>
        public static bool IsXbox
        {
            get { return OsInfo.Platform == PlatformID.Xbox; }
        }

        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows 95.
        /// </summary>
        public static bool IsWindows95
        {
            get
            {
                return
                    OsInfo.Platform == PlatformID.Win32Windows &&
                    OsInfo.Version.Minor == 0;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows 98.
        /// </summary>
        public static bool IsWindows98
        {
            get
            {
                return
                    OsInfo.Platform == PlatformID.Win32Windows &&
                    OsInfo.Version.Minor == 10;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows Me.
        /// </summary>
        public static bool IsWindowsMe
        {
            get
            {
                return
                    OsInfo.Platform == PlatformID.Win32Windows &&
                    OsInfo.Version.Minor == 90;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows NT.
        /// </summary>
        public static bool IsWindowsNT
        {
            get
            {
                return
                    OsInfo.Platform == PlatformID.Win32NT &&
                    (OsInfo.Version.Major == 3 || OsInfo.Version.Major == 4);
            }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows 2000.
        /// </summary>
        public static bool IsWindows2000
        {
            get
            {
                return
                    OsInfo.Platform == PlatformID.Win32NT &&
                    OsInfo.Version.Major == 5 &&
                    OsInfo.Version.Minor == 0;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows XP.
        /// </summary>
        public static bool IsWindowsXP
        {
            get
            {
                return
                    OsInfo.Platform == PlatformID.Win32NT &&
                    OsInfo.Version.Major == 5 &&
                    (OsInfo.Version.Minor == 1 || OsInfo.Version.Minor == 2);
            }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows Vista.
        /// </summary>
        public static bool IsWindowsVista
        {
            get
            {
                return
                    OsInfo.Platform == PlatformID.Win32NT &&
                    OsInfo.Version.Major == 6 &&
                    OsInfo.Version.Minor == 0;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows 7.
        /// </summary>
        public static bool IsWindows7
        {
            get
            {
                return
                    OsInfo.Platform == PlatformID.Win32NT &&
                    OsInfo.Version.Major == 6 &&
                    OsInfo.Version.Minor == 1;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows 8.
        /// </summary>
        public static bool IsWindows8
        {
            get
            {
                return
                    OsInfo.Platform == PlatformID.Win32NT &&
                    OsInfo.Version.Major == 6 &&
                    OsInfo.Version.Minor == 2;
            }
        }
    }
}
