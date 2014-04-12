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
        /// <summary>
        /// Compares two strings in the same manner as the file-system and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="left">The first string to compare.</param>
        /// <param name="right">The second string to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="left"/> and <paramref name="right"/>. Less than
        /// zero indicates <paramref name="left"/> is less than <paramref name="right"/>.-or-<paramref name="left"/> is null. Zero
        /// indicates <paramref name="left"/> is equal to <paramref name="right"/>. Greater than zero indicates <paramref name="left"/> is
        /// greater than <paramref name="right"/>.-or-<paramref name="right"/> is null.</returns>
        public static int Compare(string left, string right)
        {
            return Comparer.Compare(left, right);
        }
        /// <summary>
        /// Indicates whether two strings are equal in the same manner as the file-system.
        /// </summary>
        /// <param name="left">The first string to compare.</param>
        /// <param name="right">The second string to compare.</param>
        /// <returns>Returns true if <paramref name="left"/> and <paramref name="right"/> refer to the same object, or
        /// <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, false.</returns>
        public static bool Equals(string left, string right)
        {
            return Comparer.Equals(left, right);
        }
        /// <summary>
        /// Gets the hash code for the specified string in the same manner as the file-system.
        /// </summary>
        /// <param name="obj">A string.</param>
        /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/> parameter.</returns>
        public static int GetHashCode(string obj)
        {
            return Comparer.GetHashCode(obj);
        }
    }
}
