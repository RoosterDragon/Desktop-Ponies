namespace CsDesktopPonies
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Provides methods to validate arguments.
    /// </summary>
    public static class Argument
    {
        /// <summary>
        /// Checks that an argument is not null.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="arg"/> is null.</exception>
        public static void EnsureNotNull(object arg, string paramName)
        {
            if (arg == null)
                throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Checks that an argument is greater than or equal to zero.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than zero.</exception>
        public static void EnsureNonnegative(int arg, string paramName)
        {
            if (arg < 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + " must be non-negative.");
        }

        /// <summary>
        /// Checks that an argument is greater than zero.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to zero.</exception>
        public static void EnsurePositive(int arg, string paramName)
        {
            if (arg <= 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + " must be positive.");
        }

        /// <summary>
        /// Checks that an argument is a valid member of its enumeration. A value is valid if it is a defined member of a non-flagged
        /// enumeration, or any combination of defined members in a flagged enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration, which may be flagged.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="T:System.ArgumentException"><typeparamref name="TEnum"/> is not an <see cref="System.Enum"/> type.</exception>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException"><paramref name="arg"/> is not a valid member of its
        /// enumeration. That is, the enumeration is non-flagged and the value is not a defined member, or the enumeration is flagged and
        /// the value contains a flag that is not a defined member.</exception>
        public static void EnsureEnumIsValid<TEnum>(TEnum arg, string paramName) where TEnum : struct
        {
            Type enumType = typeof(TEnum);
            if (!enumType.IsEnum)
                throw new ArgumentException("TEnum must be an Enum type.", "TEnum");

            bool flagged = enumType.IsDefined(typeof(FlagsAttribute), false);
            TEnum[] enumValues = (TEnum[])Enum.GetValues(enumType);
            if (!flagged)
            {
                // Search for a matching value in the enumeration.
                bool found = false;
                foreach (TEnum enumValue in enumValues)
                    if (arg.Equals(enumValue))
                    {
                        found = true;
                        break;
                    }
                if (!found)
                    ThrowInvalidEnumArgumentException(arg, paramName, enumType);
            }
            else
            {
                // Get a set of flags which are not in the enumeration.
                ulong badFlags = ulong.MaxValue;
                foreach (TEnum enumValue in enumValues)
                    badFlags ^= Convert.ToUInt64(enumValue, CultureInfo.InvariantCulture);

                // Check none of the bad flags is set.
                ulong checkFlag = 1;
                ulong flags = Convert.ToUInt64(arg, CultureInfo.InvariantCulture);
                while (checkFlag <= flags || checkFlag == 0)
                {
                    if ((flags & checkFlag & badFlags) > 0)
                        ThrowInvalidEnumArgumentException(arg, paramName, enumType);
                    checkFlag <<= 1;
                }
            }
        }

        /// <summary>
        /// Throws an <see cref="T:System.ComponentModel.InvalidEnumArgumentException"/>.
        /// </summary>
        /// <param name="arg">The invalid argument.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="enumType">An enumeration type.</param>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">Thrown when this method is invoked.</exception>
        private static void ThrowInvalidEnumArgumentException(object arg, string paramName, Type enumType)
        {
            TypeCode underlyingTypeCode = Type.GetTypeCode(Enum.GetUnderlyingType(enumType));
            if (underlyingTypeCode == TypeCode.Int64 || underlyingTypeCode == TypeCode.UInt64 || underlyingTypeCode == TypeCode.UInt32)
                throw new InvalidEnumArgumentException(string.Format(CultureInfo.CurrentCulture,
                    "The value of argument '{0}' ({1}) is invalid for Enum type '{2}'.\nParameter name: {0}",
                    paramName, arg, enumType.Name));
            else
                throw new InvalidEnumArgumentException(paramName, (int)arg, enumType);
        }
    }
}
