namespace DesktopSprites.Core
{
    using System.Globalization;

    /// <summary>
    /// Provides methods for working with the numeric types.
    /// </summary>
    public static class Number
    {
        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed integer equivalent using invariant culture formatting
        /// information.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="s"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="T:System.FormatException"><paramref name="s"/> is not in the correct format.</exception>
        /// <exception cref="T:System.OverflowException"><paramref name="s"/> represents a number less than
        /// <see cref="F:System.Int32.MinValue"/> or greater than <see cref="F:System.Int32.MaxValue"/>.</exception>
        public static int ParseInt32Invariant(string s)
        {
            return int.Parse(s, NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed integer equivalent using invariant culture formatting
        /// information. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">When this method returns, contains the 32-bit signed integer value equivalent to the number contained in
        /// <paramref name="s"/>, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the
        /// <paramref name="s"/> parameter is null, is not of the correct format, or represents a number less than
        /// <see cref="F:System.Int32.MinValue"/> or greater than <see cref="F:System.Int32.MaxValue"/>. This parameter is passed
        /// uninitialized.</param>
        /// <returns>Return true if <paramref name="s"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParseInt32Invariant(string s, out int result)
        {
            return int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result);
        }
        /// <summary>
        /// Converts the string representation of a number to its single-precision floating-point number equivalent using invariant culture
        /// formatting information.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>A single-precision floating-point number equivalent to the number contained in <paramref name="s"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="T:System.FormatException"><paramref name="s"/> is not in the correct format.</exception>
        /// <exception cref="T:System.OverflowException"><paramref name="s"/> represents a number less than
        /// <see cref="F:System.Single.MinValue"/> or greater than <see cref="F:System.Single.MaxValue"/>.</exception>
        public static float ParseSingleInvariant(string s)
        {
            return float.Parse(s, NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the string representation of a number to its single-precision floating-point number equivalent using invariant culture
        /// formatting information. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">When this method returns, contains a single-precision floating-point number equivalent to the number
        /// contained in <paramref name="s"/>, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the
        /// <paramref name="s"/> parameter is null, is not of the correct format, or represents a number less than
        /// <see cref="F:System.Single.MinValue"/> or greater than <see cref="F:System.Single.MaxValue"/>. This parameter is passed
        /// uninitialized.</param>
        /// <returns>Return true if <paramref name="s"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParseSingleInvariant(string s, out float result)
        {
            return float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo, out result);
        }
        /// <summary>
        /// Converts the string representation of a number to its double-precision floating-point number equivalent using invariant culture
        /// formatting information.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>A double-precision floating-point number equivalent to the number contained in <paramref name="s"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="T:System.FormatException"><paramref name="s"/> is not in the correct format.</exception>
        /// <exception cref="T:System.OverflowException"><paramref name="s"/> represents a number less than
        /// <see cref="F:System.Double.MinValue"/> or greater than <see cref="F:System.Double.MaxValue"/>.</exception>
        public static double ParseDoubleInvariant(string s)
        {
            return double.Parse(s, NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the string representation of a number to its double-precision floating-point number equivalent using invariant culture
        /// formatting information. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">When this method returns, contains a double-precision floating-point number equivalent to the number
        /// contained in <paramref name="s"/>, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the
        /// <paramref name="s"/> parameter is null, is not of the correct format, or represents a number less than
        /// <see cref="F:System.Double.MinValue"/> or greater than <see cref="F:System.Double.MaxValue"/>. This parameter is passed
        /// uninitialized.</param>
        /// <returns>Return true if <paramref name="s"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParseDoubleInvariant(string s, out double result)
        {
            return double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo, out result);
        }
        /// <summary>
        /// Converts the string representation of a number to its <see cref="T:System.Decimal"/> equivalent using invariant culture
        /// formatting information.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>A <see cref="T:System.Decimal"/> equivalent to the number contained in <paramref name="s"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="T:System.FormatException"><paramref name="s"/> is not in the correct format.</exception>
        /// <exception cref="T:System.OverflowException"><paramref name="s"/> represents a number less than
        /// <see cref="F:System.Decimal.MinValue"/> or greater than <see cref="F:System.Decimal.MaxValue"/>.</exception>
        public static decimal ParseDecimalInvariant(string s)
        {
            return decimal.Parse(s, NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the string representation of a number to its <see cref="T:System.Decimal"/> equivalent using invariant culture
        /// formatting information. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="T:System.Decimal"/> equivalent to the number contained
        /// in <paramref name="s"/>, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the
        /// <paramref name="s"/> parameter is null, is not of the correct format, or represents a number less than
        /// <see cref="F:System.Decimal.MinValue"/> or greater than <see cref="F:System.Decimal.MaxValue"/>. This parameter is passed
        /// uninitialized.</param>
        /// <returns>Return true if <paramref name="s"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParseDecimalInvariant(string s, out decimal result)
        {
            return decimal.TryParse(s, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out result);
        }

        /// <summary>
        /// Converts the numeric value of the specified <see cref="T:System.Byte"/> to its equivalent string representation using invariant
        /// culture formatting information.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The string representation of the value of this object.</returns>
        public static string ToStringInvariant(this byte value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the numeric value of the specified <see cref="T:System.Byte"/> to its equivalent string representation using the
        /// specified format and invariant culture formatting information.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns>The string representation of the value of this object.</returns>
        /// <exception cref="T:System.FormatException"><paramref name="format"/> includes an unsupported specifier.</exception>
        public static string ToStringInvariant(this byte value, string format)
        {
            return value.ToString(format, NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the numeric value of the specified <see cref="T:System.Int32"/> to its equivalent string representation using
        /// invariant culture formatting information.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The string representation of the value of this object.</returns>
        public static string ToStringInvariant(this int value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the numeric value of the specified <see cref="T:System.Int32"/> to its equivalent string representation using the
        /// specified format and invariant culture formatting information.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns>The string representation of the value of this object.</returns>
        /// <exception cref="T:System.FormatException"><paramref name="format"/> includes an unsupported specifier.</exception>
        public static string ToStringInvariant(this int value, string format)
        {
            return value.ToString(format, NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the numeric value of the specified <see cref="T:System.Single"/> to its equivalent string representation using
        /// invariant culture formatting information.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The string representation of the value of this object.</returns>
        public static string ToStringInvariant(this float value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the numeric value of the specified <see cref="T:System.Single"/> to its equivalent string representation using the
        /// specified format and invariant culture formatting information.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns>The string representation of the value of this object.</returns>
        /// <exception cref="T:System.FormatException"><paramref name="format"/> includes an unsupported specifier.</exception>
        public static string ToStringInvariant(this float value, string format)
        {
            return value.ToString(format, NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the numeric value of the specified <see cref="T:System.Double"/> to its equivalent string representation using
        /// invariant culture formatting information.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The string representation of the value of this object.</returns>
        public static string ToStringInvariant(this double value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the numeric value of the specified <see cref="T:System.Double"/> to its equivalent string representation using the
        /// specified format and invariant culture formatting information.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns>The string representation of the value of this object.</returns>
        /// <exception cref="T:System.FormatException"><paramref name="format"/> includes an unsupported specifier.</exception>
        public static string ToStringInvariant(this double value, string format)
        {
            return value.ToString(format, NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the numeric value of the specified <see cref="T:System.Decimal"/> to its equivalent string representation using
        /// invariant culture formatting information.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The string representation of the value of this object.</returns>
        public static string ToStringInvariant(this decimal value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the numeric value of the specified <see cref="T:System.Decimal"/> to its equivalent string representation using the
        /// specified format and invariant culture formatting information.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns>The string representation of the value of this object.</returns>
        /// <exception cref="T:System.FormatException"><paramref name="format"/> includes an unsupported specifier.</exception>
        public static string ToStringInvariant(this decimal value, string format)
        {
            return value.ToString(format, NumberFormatInfo.InvariantInfo);
        }
    }
}
