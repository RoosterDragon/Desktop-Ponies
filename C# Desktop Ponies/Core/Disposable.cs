namespace CSDesktopPonies.Core
{
    using System;
    using System.Threading;

    /// <summary>
    /// A class capable of releasing allocated resources.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        /// <summary>
        /// Value that indicates disposal has begun.
        /// </summary>
        private const int IsDisposing = 1;
        /// <summary>
        /// Indicates whether disposal has begun on this object.
        /// </summary>
        private int disposalState = IsDisposing - 1;
        /// <summary>
        /// Gets a value indicating whether the object is being, or has been, disposed.
        /// </summary>
        public bool Disposed
        {
            get { return disposalState == IsDisposing; }
        }

        /// <summary>
        /// Releases all resources allocated by the object.
        /// </summary>
        /// <remarks>This method guarantees to only call the underlying disposal method once, even if invoked multiple times. It is also
        /// thread-safe.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
            Justification = "Implementation is correct, but not an exact match which misleads analysis.")]
        public void Dispose()
        {
            if (Interlocked.Exchange(ref disposalState, IsDisposing) != IsDisposing)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <param name="disposing">Indicates if managed resources should be disposed in addition to unmanaged resources; otherwise, only
        /// unmanaged resources should be disposed.</param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Checks the current instance has not been disposed, otherwise throws an <see cref="T:System.ObjectDisposedException"/>.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed.</exception>
        protected void EnsureNotDisposed()
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        /// <summary>
        /// Performs additional setup on an <see cref="T:System.IDisposable"/> whilst ensuring the resource is disposed if an exception
        /// occurs. This is useful for methods that own a resource but intend to relinquish ownership to their caller, as they are
        /// responsible for the resource until it is relinquished, and thus must ensure it is released under exceptional circumstances.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the resource to setup.</typeparam>
        /// <param name="resource">A resource to be setup .</param>
        /// <param name="setup">Actions to perform on the resource. This is done in a try-catch block to ensure the resource is released if
        /// an exception occurs.</param>
        /// <returns>A reference to the <see cref="T:System.IDisposable"/>.</returns>
        public static TDisposable SetupSafely<TDisposable>(TDisposable resource, Action<TDisposable> setup)
            where TDisposable : IDisposable
        {
            Argument.EnsureNotNull(resource, "resource");
            Argument.EnsureNotNull(setup, "setup");
            try
            {
                setup(resource);
                return resource;
            }
            catch (Exception)
            {
                resource.Dispose();
                throw;
            }
        }
    }
}
