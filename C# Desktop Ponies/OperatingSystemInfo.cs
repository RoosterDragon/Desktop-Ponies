namespace CSDesktopPonies
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
        private static OperatingSystem osInfo = Environment.OSVersion;
        /// <summary>
        /// Indicates whether the current operating system is Macintosh.
        /// </summary>
        private static bool isMacOSX;

        /// <summary>
        /// Stores null-terminated strings of information identifying the current system into the structure referenced by
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Pointer to a structure into which system information is stored.</param>
        /// <returns>Returns the value 0 if successful; otherwise the value -1 is returned.</returns>
        [DllImport("libc")]
        private static extern int uname(IntPtr name);

        /// <summary>
        /// Initializes static members of the <see cref="T:CSDesktopPonies.OperatingSystemInfo"/> class.
        /// </summary>
        static OperatingSystemInfo()
        {
            // Check for MacOSX, as PlatformID will simply return Unix under Mono.
            IntPtr buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.AllocHGlobal(8196);
                if (uname(buffer) == 0)
                {
                    // The buffer contains 5 null-terminated strings, we will marshal the first one, containing the system name.
                    string osName = Marshal.PtrToStringAnsi(buffer);
                    isMacOSX = osName == "Darwin";
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
            get { return osInfo.Version; }
        }

        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows.
        /// </summary>
        public static bool IsWindows
        {
            get
            {
                return
                    osInfo.Platform == PlatformID.Win32NT ||
                    osInfo.Platform == PlatformID.Win32S ||
                    osInfo.Platform == PlatformID.Win32Windows ||
                    osInfo.Platform == PlatformID.WinCE;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Macintosh.
        /// </summary>
        public static bool IsMacOSX
        {
            get { return isMacOSX; }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Unix.
        /// </summary>
        public static bool IsUnix
        {
            get { return osInfo.Platform == PlatformID.Unix; }
        }
        /// <summary>
        /// Gets a value indicating whether the current operating system is Xbox 360.
        /// </summary>
        public static bool IsXbox
        {
            get { return osInfo.Platform == PlatformID.Xbox; }
        }

        /// <summary>
        /// Gets a value indicating whether the current operating system is Windows 95.
        /// </summary>
        public static bool IsWindows95
        {
            get
            {
                return
                    osInfo.Platform == PlatformID.Win32Windows &&
                    osInfo.Version.Minor == 0;
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
                    osInfo.Platform == PlatformID.Win32Windows &&
                    osInfo.Version.Minor == 10;
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
                    osInfo.Platform == PlatformID.Win32Windows &&
                    osInfo.Version.Minor == 90;
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
                    osInfo.Platform == PlatformID.Win32NT &&
                    (osInfo.Version.Major == 3 || osInfo.Version.Major == 4);
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
                    osInfo.Platform == PlatformID.Win32NT &&
                    osInfo.Version.Major == 5 &&
                    osInfo.Version.Minor == 0;
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
                    osInfo.Platform == PlatformID.Win32NT &&
                    osInfo.Version.Major == 5 &&
                    (osInfo.Version.Minor == 1 || osInfo.Version.Minor == 2);
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
                    osInfo.Platform == PlatformID.Win32NT &&
                    osInfo.Version.Major == 6 &&
                    osInfo.Version.Minor == 0;
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
                    osInfo.Platform == PlatformID.Win32NT &&
                    osInfo.Version.Major == 6 &&
                    osInfo.Version.Minor == 1;
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
                    osInfo.Platform == PlatformID.Win32NT &&
                    osInfo.Version.Major == 6 &&
                    osInfo.Version.Minor == 2;
            }
        }
    }
}
