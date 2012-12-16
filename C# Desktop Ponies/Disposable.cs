namespace CsDesktopPonies
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
        /// Releases all resources used by the <see cref="CsDesktopPonies.Disposable"/>.
        /// </summary>
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
    }
}
