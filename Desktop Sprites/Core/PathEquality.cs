namespace DesktopSprites.Core
{
    using System;

    /// <summary>
    /// Provides equality information for file-system paths of the current operating system.
    /// </summary>
    public static class PathEquality
    {
        /// <summary>
        /// Gets a <see cref="T:System.StringComparison"/> that specifies how paths should be compared.
        /// </summary>
        public static StringComparison Comparison
        {
            get { return OperatingSystemInfo.IsWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal; }
        }
        /// <summary>
        /// Gets a <see cref="T:System.StringComparer"/> that can be used to compare paths for equality.
        /// </summary>
        public static StringComparer Comparer
        {
            get { return OperatingSystemInfo.IsWindows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal; }
        }
    }
}
