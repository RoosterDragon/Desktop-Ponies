namespace DesktopSprites.Core
{
    using System;
    using System.Threading;

    /// <summary>
    /// Provides extensions to the <see cref="T:System.Threading.ReaderWriterLockSlim"/> class.
    /// </summary>
    public static class ReaderWriterLockSlimExtensions
    {
        /// <summary>
        /// Represents a read lock.
        /// </summary>
        public struct ReadMode : IDisposable
        {
            private readonly ReaderWriterLockSlim rwls;
            /// <summary>
            /// Tries to enter the lock in read mode.
            /// </summary>
            /// <param name="rwls">The lock to enter in read mode.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="rwls"/> is null.</exception>
            /// <exception cref="T:System.Threading.RecursionLockException">The
            /// <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy"/> property is
            /// <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion"/> and the current thread has already entered read mode.-or-
            /// The recursion number would exceed the capacity of the counter. This limit is so large that applications should never
            /// encounter it.</exception>
            public ReadMode(ReaderWriterLockSlim rwls)
            {
                this.rwls = Argument.EnsureNotNull(rwls, "rwls");
                rwls.EnterReadLock();
            }
            /// <summary>
            /// Reduces the recursion count for read mode, and exits read mode if the resulting count is 0 (zero).
            /// </summary>
            public void Dispose()
            {
                rwls.ExitReadLock();
            }
        }
        /// <summary>
        /// Represents an upgradeable read lock.
        /// </summary>
        public struct UpgradeableMode : IDisposable
        {
            private readonly ReaderWriterLockSlim rwls;
            /// <summary>
            /// Tries to enter the lock in upgradeable mode.
            /// </summary>
            /// <param name="rwls">The lock to enter in upgradeable mode.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="rwls"/> is null.</exception>
            /// <exception cref="T:System.Threading.RecursionLockException">The
            /// <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy"/> property is
            /// <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion"/> and the current thread has already entered the lock in any
            /// mode.-or-The current thread has entered read mode, so trying to enter upgradeable mode would create the possibility of a
            /// deadlock.-or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should
            /// never encounter it.</exception>
            public UpgradeableMode(ReaderWriterLockSlim rwls)
            {
                this.rwls = Argument.EnsureNotNull(rwls, "rwls");
                rwls.EnterUpgradeableReadLock();
            }
            /// <summary>
            /// Reduces the recursion count for upgradeable mode, and exits upgradeable mode if the resulting count is 0 (zero).
            /// </summary>
            public void Dispose()
            {
                rwls.ExitUpgradeableReadLock();
            }
        }
        /// <summary>
        /// Represents a write lock.
        /// </summary>
        public struct WriteMode : IDisposable
        {
            private readonly ReaderWriterLockSlim rwls;
            /// <summary>
            /// Tries to enter the lock in write mode.
            /// </summary>
            /// <param name="rwls">The lock to enter in write mode.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="rwls"/> is null.</exception>
            /// <exception cref="T:System.Threading.RecursionLockException">The
            /// <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy"/> property is
            /// <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion"/> and the current thread has already entered the lock in any
            /// mode.-or-The current thread has entered read mode, so trying to enter the lock in write mode would create the possibility
            /// of a deadlock.-or-The recursion number would exceed the capacity of the counter. The limit is so large that applications
            /// should never encounter it.</exception>
            public WriteMode(ReaderWriterLockSlim rwls)
            {
                this.rwls = Argument.EnsureNotNull(rwls, "rwls");
                rwls.EnterWriteLock();
            }
            /// <summary>
            /// Reduces the recursion count for write mode, and exits write mode if the resulting count is 0 (zero).
            /// </summary>
            public void Dispose()
            {
                rwls.ExitWriteLock();
            }
        }
        /// <summary>
        /// Uses the lock in read mode.
        /// </summary>
        /// <param name="rwls">The lock to use in read mode.</param>
        /// <returns>An object representing the lock to release.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="rwls"/> is null.</exception>
        /// <exception cref="T:System.Threading.RecursionLockException">The
        /// <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy"/> property is
        /// <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion"/> and the current thread has already entered read mode.-or-
        /// The recursion number would exceed the capacity of the counter. This limit is so large that applications should never
        /// encounter it.</exception>
        public static ReadMode InReadMode(this ReaderWriterLockSlim rwls)
        {
            return new ReadMode(rwls);
        }
        /// <summary>
        /// Uses the lock in upgradeable mode.
        /// </summary>
        /// <param name="rwls">The lock to use in upgradeable mode.</param>
        /// <returns>An object representing the lock to release.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="rwls"/> is null.</exception>
        /// <exception cref="T:System.Threading.RecursionLockException">The
        /// <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy"/> property is
        /// <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion"/> and the current thread has already entered the lock in any
        /// mode.-or-The current thread has entered read mode, so trying to enter upgradeable mode would create the possibility of a
        /// deadlock.-or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should
        /// never encounter it.</exception>
        public static UpgradeableMode InUpgradeableMode(this ReaderWriterLockSlim rwls)
        {
            return new UpgradeableMode(rwls);
        }
        /// <summary>
        /// Uses the lock in write mode.
        /// </summary>
        /// <param name="rwls">The lock to use in write mode.</param>
        /// <returns>An object representing the lock to release.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="rwls"/> is null.</exception>
        /// <exception cref="T:System.Threading.RecursionLockException">The
        /// <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy"/> property is
        /// <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion"/> and the current thread has already entered the lock in any
        /// mode.-or-The current thread has entered read mode, so trying to enter the lock in write mode would create the possibility
        /// of a deadlock.-or-The recursion number would exceed the capacity of the counter. The limit is so large that applications
        /// should never encounter it.</exception>
        public static WriteMode InWriteMode(this ReaderWriterLockSlim rwls)
        {
            return new WriteMode(rwls);
        }
    }
}
