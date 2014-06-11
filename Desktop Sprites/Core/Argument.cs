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
        private const string MustBeNonnegative = " must be non-negative.";
        private const string MustBePositive = " must be positive.";
        private const string MustBeGreaterThan = " must be greater than ";
        private const string MustBeGreaterThanOrEqualTo = " must be greater than or equal to ";
        private const string MustBeLessThan = " must be less than ";
        private const string MustBeLessThanOrEqualTo = " must be less than or equal to ";
        private const string InRangeInclusiveFormat = "{0} must be between {1} and {2} inclusive.";
        private const string InRangeExclusiveFormat = "{0} must be between {1} and {2} exclusive.";

        #region ValidatedNotNullAttribute class
        /// <summary>
        /// Identifies a parameter as having been validated to ensure it was not null to static analysis tools.
        /// </summary>
        [AttributeUsage(AttributeTargets.Parameter)]
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
            if (Argument.EnsureNotNull(arg, paramName).Length == 0)
                throw new ArgumentException(paramName + " must not be empty.", paramName);
            return arg;
        }

        /// <summary>
        /// Checks that a sequence argument is not null or an empty sequence.
        /// </summary>
        /// <typeparam name="T">The type of the sequence elements.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="arg"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="arg"/> contains no elements.</exception>
        [DebuggerStepThrough]
        public static void EnsureNotNullOrEmpty<T>([ValidatedNotNull] IEnumerable<T> arg, string paramName)
        {
            if (!Argument.EnsureNotNull(arg, paramName).Any())
                throw new ArgumentException(paramName + " must contain at least one element.", paramName);
        }

        /// <summary>
        /// Checks that a sequence argument is not null or an empty sequence.
        /// </summary>
        /// <typeparam name="TEnumerable">The type of the sequence.</typeparam>
        /// <typeparam name="T">The type of the sequence elements.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>A reference to <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="arg"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="arg"/> contains no elements.</exception>
        [DebuggerStepThrough]
        public static TEnumerable EnsureNotNullOrEmpty<TEnumerable, T>([ValidatedNotNull] TEnumerable arg, string paramName)
            where TEnumerable : IEnumerable<T>
        {
            EnsureNotNullOrEmpty(arg, paramName);
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
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeNonnegative);
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
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBePositive);
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly greater than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static int EnsureGreaterThan(int arg, string paramName, int value)
        {
            if (arg <= value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than or equal to a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be greater than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static int EnsureGreaterThanOrEqualTo(int arg, string paramName, int value)
        {
            if (arg < value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static int EnsureLessThan(int arg, string paramName, int value)
        {
            if (arg >= value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than or equal to a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static int EnsureLessThanOrEqualTo(int arg, string paramName, int value)
        {
            if (arg > value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be greater than or equal to.</param>
        /// <param name="max">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="min"/>.-or-
        /// <paramref name="arg"/> is greater than <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static int EnsureInRangeInclusive(int arg, string paramName, int min, int max)
        {
            if (arg < min || arg > max)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeInclusiveFormat.FormatWith(paramName, min, max));
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be strictly greater than.</param>
        /// <param name="max">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to <paramref name="min"/>.
        /// -or-<paramref name="arg"/> is greater than or equal to <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static int EnsureInRangeExclusive(int arg, string paramName, int min, int max)
        {
            if (arg <= min || arg >= max)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeExclusiveFormat.FormatWith(paramName, min, max));
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
        public static long EnsureNonnegative(long arg, string paramName)
        {
            if (arg < 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeNonnegative);
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
        public static long EnsurePositive(long arg, string paramName)
        {
            if (arg <= 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBePositive);
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly greater than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static long EnsureGreaterThan(long arg, string paramName, long value)
        {
            if (arg <= value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than or equal to a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be greater than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static long EnsureGreaterThanOrEqualTo(long arg, string paramName, long value)
        {
            if (arg < value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static long EnsureLessThan(long arg, string paramName, long value)
        {
            if (arg >= value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than or equal to a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static long EnsureLessThanOrEqualTo(long arg, string paramName, long value)
        {
            if (arg > value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be greater than or equal to.</param>
        /// <param name="max">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="min"/>.-or-
        /// <paramref name="arg"/> is greater than <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static long EnsureInRangeInclusive(long arg, string paramName, long min, long max)
        {
            if (arg < min || arg > max)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeInclusiveFormat.FormatWith(paramName, min, max));
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be strictly greater than.</param>
        /// <param name="max">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to <paramref name="min"/>.
        /// -or-<paramref name="arg"/> is greater than or equal to <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static long EnsureInRangeExclusive(long arg, string paramName, long min, long max)
        {
            if (arg <= min || arg >= max)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeExclusiveFormat.FormatWith(paramName, min, max));
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
        public static float EnsureNonnegative(float arg, string paramName)
        {
            if (arg < 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeNonnegative);
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
        public static float EnsurePositive(float arg, string paramName)
        {
            if (arg <= 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBePositive);
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly greater than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static float EnsureGreaterThan(float arg, string paramName, float value)
        {
            if (arg <= value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than or equal to a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be greater than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static float EnsureGreaterThanOrEqualTo(float arg, string paramName, float value)
        {
            if (arg < value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static float EnsureLessThan(float arg, string paramName, float value)
        {
            if (arg >= value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than or equal to a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static float EnsureLessThanOrEqualTo(float arg, string paramName, float value)
        {
            if (arg > value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be greater than or equal to.</param>
        /// <param name="max">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="min"/>.-or-
        /// <paramref name="arg"/> is greater than <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static float EnsureInRangeInclusive(float arg, string paramName, float min, float max)
        {
            if (arg < min || arg > max)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeInclusiveFormat.FormatWith(paramName, min, max));
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be strictly greater than.</param>
        /// <param name="max">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to <paramref name="min"/>.
        /// -or-<paramref name="arg"/> is greater than or equal to <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static float EnsureInRangeExclusive(float arg, string paramName, float min, float max)
        {
            if (arg <= min || arg >= max)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeExclusiveFormat.FormatWith(paramName, min, max));
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
        public static double EnsureNonnegative(double arg, string paramName)
        {
            if (arg < 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeNonnegative);
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
        public static double EnsurePositive(double arg, string paramName)
        {
            if (arg <= 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBePositive);
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly greater than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static double EnsureGreaterThan(double arg, string paramName, double value)
        {
            if (arg <= value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than or equal to a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be greater than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static double EnsureGreaterThanOrEqualTo(double arg, string paramName, double value)
        {
            if (arg < value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static double EnsureLessThan(double arg, string paramName, double value)
        {
            if (arg >= value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than or equal to a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static double EnsureLessThanOrEqualTo(double arg, string paramName, double value)
        {
            if (arg > value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be greater than or equal to.</param>
        /// <param name="max">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="min"/>.-or-
        /// <paramref name="arg"/> is greater than <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static double EnsureInRangeInclusive(double arg, string paramName, double min, double max)
        {
            if (arg < min || arg > max)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeInclusiveFormat.FormatWith(paramName, min, max));
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be strictly greater than.</param>
        /// <param name="max">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to <paramref name="min"/>.
        /// -or-<paramref name="arg"/> is greater than or equal to <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static double EnsureInRangeExclusive(double arg, string paramName, double min, double max)
        {
            if (arg <= min || arg >= max)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeExclusiveFormat.FormatWith(paramName, min, max));
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
        public static TimeSpan EnsureNonnegative(TimeSpan arg, string paramName)
        {
            if (arg < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeNonnegative);
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
        public static TimeSpan EnsurePositive(TimeSpan arg, string paramName)
        {
            if (arg <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBePositive);
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly greater than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static TimeSpan EnsureGreaterThan(TimeSpan arg, string paramName, TimeSpan value)
        {
            if (arg <= value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than or equal to a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be greater than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static TimeSpan EnsureGreaterThanOrEqualTo(TimeSpan arg, string paramName, TimeSpan value)
        {
            if (arg < value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static TimeSpan EnsureLessThan(TimeSpan arg, string paramName, TimeSpan value)
        {
            if (arg >= value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than or equal to a specified value.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static TimeSpan EnsureLessThanOrEqualTo(TimeSpan arg, string paramName, TimeSpan value)
        {
            if (arg > value)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be greater than or equal to.</param>
        /// <param name="max">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="min"/>.-or-
        /// <paramref name="arg"/> is greater than <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static TimeSpan EnsureInRangeInclusive(TimeSpan arg, string paramName, TimeSpan min, TimeSpan max)
        {
            if (arg < min || arg > max)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeInclusiveFormat.FormatWith(paramName, min, max));
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be strictly greater than.</param>
        /// <param name="max">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to <paramref name="min"/>.
        /// -or-<paramref name="arg"/> is greater than or equal to <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static TimeSpan EnsureInRangeExclusive(TimeSpan arg, string paramName, TimeSpan min, TimeSpan max)
        {
            if (arg <= min || arg >= max)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeExclusiveFormat.FormatWith(paramName, min, max));
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the argument and value.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly greater than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static T EnsureGreaterThan<T>(T arg, string paramName, T value) where T : IComparable<T>
        {
            if (arg.CompareTo(value) <= 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is greater than or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the argument and value.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be greater than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static T EnsureGreaterThanOrEqualTo<T>(T arg, string paramName, T value) where T : IComparable<T>
        {
            if (arg.CompareTo(value) < 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeGreaterThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the argument and value.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than or equal to
        /// <paramref name="value"/>.</exception>
        [DebuggerStepThrough]
        public static T EnsureLessThan<T>(T arg, string paramName, T value) where T : IComparable<T>
        {
            if (arg.CompareTo(value) >= 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThan + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is less than or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the argument and value.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is greater than <paramref name="value"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static T EnsureLessThanOrEqualTo<T>(T arg, string paramName, T value) where T : IComparable<T>
        {
            if (arg.CompareTo(value) > 0)
                throw new ArgumentOutOfRangeException(paramName, arg, paramName + MustBeLessThanOrEqualTo + value + ".");
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <typeparam name="T">The type of the argument and value.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be greater than or equal to.</param>
        /// <param name="max">The value that the argument should be less than or equal to.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than <paramref name="min"/>.-or-
        /// <paramref name="arg"/> is greater than <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static T EnsureInRangeInclusive<T>(T arg, string paramName, T min, T max) where T : IComparable<T>
        {
            if (arg.CompareTo(min) < 0 || arg.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeInclusiveFormat.FormatWith(paramName, min, max));
            return arg;
        }

        /// <summary>
        /// Checks that an argument is within the specified range.
        /// </summary>
        /// <typeparam name="T">The type of the argument and value.</typeparam>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="min">The value that the argument should be strictly greater than.</param>
        /// <param name="max">The value that the argument should be strictly less than.</param>
        /// <returns>The value of <paramref name="arg"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arg"/> is less than or equal to <paramref name="min"/>.
        /// -or-<paramref name="arg"/> is greater than or equal to <paramref name="max"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static T EnsureInRangeExclusive<T>(T arg, string paramName, T min, T max) where T : IComparable<T>
        {
            if (arg.CompareTo(min) <= 0 || arg.CompareTo(max) >= 0)
                throw new ArgumentOutOfRangeException(paramName, arg, InRangeExclusiveFormat.FormatWith(paramName, min, max));
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
                    badFlags &= ~Convert.ToUInt64(enumValue, CultureInfo.InvariantCulture);

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
                return new InvalidEnumArgumentException(
                    "The value of argument '{0}' ({1}) is invalid for Enum type '{2}'.\nParameter name: {0}".FormatWith(
                    paramName, arg, enumType.Name));
            else
                return new InvalidEnumArgumentException(paramName, Convert.ToInt32(arg, CultureInfo.InvariantCulture), enumType);
        }
    }
}
