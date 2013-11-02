namespace DesktopSprites.Core
{
    using System;

    /// <summary>
    /// Defines extension methods for <see cref="T:System.Version"/>.
    /// </summary>
    public static class VersionExtensions
    {
        /// <summary>
        /// Converts the value of the current <see cref="T:System.Version"/> object to its equivalent <see cref="T:System.String"/>
        /// representation.
        /// </summary>
        /// <param name="v">The <see cref="T:System.Version"/> to convert.</param>
        /// <returns>The <see cref="T:System.String"/> representation of the values of the major, minor, build, and revision components of
        /// the current <see cref="T:System.Version"/> object, each separated by a period character ('.'). Trailing non-positive components
        /// after the major component are elided.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="v"/> is null.</exception>
        public static string ToDisplayString(this Version v)
        {
            Argument.EnsureNotNull(v, "v");
            int fieldCount = 1;
            if (v.Revision > 0)
                fieldCount = 4;
            else if (v.Build > 0)
                fieldCount = 3;
            else if (v.Minor > 0)
                fieldCount = 2;
            return v.ToString(fieldCount);
        }
    }
}
