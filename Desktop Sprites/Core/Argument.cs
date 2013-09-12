namespace DesktopSprites.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Provides methods to validate arguments.
    /// </summary>
    public static class Argument
    {
        #region ValidatedNotNullAttribute class
        /// <summary>
        /// Identifies a parameter as having been validated to ensure it was not null to static analysis tools.
        /// </summary>
        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
        private sealed class ValidatedNotNullAttribute : Attribute
        {
        }
        #endregion

        /// <summary>
        /// Checks that an argument is not null.
        /// </summary>
        /// <typeparam name="T">The type of the argument to validate.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>A reference to <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="arg"/> is null.</exception>
        [DebuggerStepThrough]
        public static T EnsureNotNull<T>([ValidatedNotNull] T arg, string paramName)
        {
            if (arg == null)
                throw new ArgumentNullException(paramName);
            return arg;
        }

        /// <summary>
        /// Checks that a string argument is not null or the empty string.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>A reference to <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="arg"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="arg"/> is the empty string.</exception>
        [DebuggerStepThrough]
        public static string EnsureNotNullOrEmpty([ValidatedNotNull] string arg, string paramName)
        {
            if (Argument.EnsureNotNull(arg, paramName) == string.Empty)
                throw new ArgumentException(paramName + " must not be empty.", paramName);
            return arg;
        }

        /// <summary>
        /// Checks that a collection argument is not null or an empty collection.
        /// </summary>
        /// <typeparam name="T">The type of the collection elements.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>A reference to <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="arg"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="arg"/> contains no elements.</exception>
        [DebuggerStepThrough]
        public static IEnumerable<T> EnsureNotNullOrEmpty<T>([ValidatedNotNull] IEnumerable<T> arg, string paramName) 
        {
            if (!Argument.EnsureNotNull(arg, paramName).Any())
                throw new ArgumentException(paramName + " must contain at least one element.", paramName);
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than or equal to zero.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than zero.</exception>
        [DebuggerStepThrough]
        public static int EnsureNonnegative(int arg, string paramName)
        {
            if (arg < 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + " must be non-negative.");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than zero.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to zero.</exception>
        [DebuggerStepThrough]
        public static int EnsurePositive(int arg, string paramName)
        {
            if (arg <= 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + " must be positive.");
            return arg;
        }

        /// <summary>
        /// Cache that remembers whether a type has the <see cref="T:System.FlagsAttribute"/>.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, bool> TypeFlagged = new ConcurrentDictionary<Type, bool>();

        /// <summary>
        /// Checks that an argument is a valid member of its enumeration. A value is valid if it is a defined member of a non-flagged
        /// enumeration, or any combination of defined members in a flagged enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration, which may be flagged.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentException"><typeparamref name="TEnum"/> is not an <see cref="System.Enum"/> type.</exception>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException"><paramref name="arg"/> is not a valid member of its
        /// enumeration. That is, the enumeration is non-flagged and the value is not a defined member, or the enumeration is flagged and
        /// the value contains a flag that is not a defined member.</exception>
        [DebuggerStepThrough]
        public static TEnum EnsureEnumIsValid<TEnum>(TEnum arg, string paramName) where TEnum : struct
        {
            Type enumType = typeof(TEnum);
            if (!enumType.IsEnum)
                throw new ArgumentException("TEnum must be an Enum type.", "TEnum");

            bool flagged = TypeFlagged.GetOrAdd(enumType, type => type.IsDefined(typeof(FlagsAttribute), false));
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
                    throw NewInvalidEnumArgumentException(arg, paramName, enumType);
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
                        throw NewInvalidEnumArgumentException(arg, paramName, enumType);
                    checkFlag <<= 1;
                }
            }

            return arg;
        }

        /// <summary>
        /// Creates a new <see cref="T:System.ComponentModel.InvalidEnumArgumentException"/>.
        /// </summary>
        /// <param name="arg">The invalid argument.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="enumType">An enumeration type.</param>
        /// <returns>A new <see cref="T:System.ComponentModel.InvalidEnumArgumentException"/>.</returns>
        private static InvalidEnumArgumentException NewInvalidEnumArgumentException(object arg, string paramName, Type enumType)
        {
            TypeCode underlyingTypeCode = Type.GetTypeCode(Enum.GetUnderlyingType(enumType));
            if (underlyingTypeCode == TypeCode.Int64 || underlyingTypeCode == TypeCode.UInt64 || underlyingTypeCode == TypeCode.UInt32)
                return new InvalidEnumArgumentException(string.Format(CultureInfo.CurrentCulture,
                    "The value of argument '{0}' ({1}) is invalid for Enum type '{2}'.\nParameter name: {0}",
                    paramName, arg, enumType.Name));
            else
                return new InvalidEnumArgumentException(paramName, (int)arg, enumType);
        }
    }
}
