namespace DesktopSprites.Core
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Defines extensions methods for <see cref="T:System.Windows.Forms.Control"/>.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Efficiently executes the specified method on the thread that owns the control's underlying window handle.
        /// </summary>
        /// <param name="control">The control to invoke upon.</param>
        /// <param name="method">The method to execute.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is null.-or-<paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">No appropriate window handle can be found.</exception>
        public static void SmartInvoke(this Control control, MethodInvoker method)
        {
            Argument.EnsureNotNull(control, "control");
            Argument.EnsureNotNull(method, "method");
            if (control.InvokeRequired)
                control.SafeInvoke(method);
            else
                method();
        }

        /// <summary>
        /// Efficiently executes the specified event handler on the thread that owns the control's underlying window handle.
        /// </summary>
        /// <param name="control">The control to invoke upon.</param>
        /// <param name="handler">The event handler to execute.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is null.-or-<paramref name="handler"/> is null.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">No appropriate window handle can be found.</exception>
        public static void SmartInvoke(this Control control, EventHandler handler)
        {
            Argument.EnsureNotNull(control, "control");
            Argument.EnsureNotNull(handler, "handler");
            if (control.InvokeRequired)
                control.SafeInvoke(handler);
            else
                handler(control, EventArgs.Empty);
        }

        /// <summary>
        /// Efficiently executes the specified event handler on the thread that owns the control's underlying window handle.
        /// </summary>
        /// <param name="control">The control to invoke upon.</param>
        /// <param name="handler">The event handler to execute.</param>
        /// <param name="sender">The source of the event.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is null.-or-<paramref name="handler"/> is null.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">No appropriate window handle can be found.</exception>
        public static void SmartInvoke(this Control control, EventHandler handler, object sender)
        {
            Argument.EnsureNotNull(control, "control");
            Argument.EnsureNotNull(handler, "handler");
            if (control.InvokeRequired)
                control.SafeInvoke(handler, sender);
            else
                handler(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Efficiently executes the specified event handler on the thread that owns the control's underlying window handle.
        /// </summary>
        /// <param name="control">The control to invoke upon.</param>
        /// <param name="handler">The event handler to execute.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Data about the event.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is null.-or-<paramref name="handler"/> is null.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">No appropriate window handle can be found.</exception>
        public static void SmartInvoke(this Control control, EventHandler handler, object sender, EventArgs e)
        {
            Argument.EnsureNotNull(control, "control");
            Argument.EnsureNotNull(handler, "handler");
            if (control.InvokeRequired)
                control.SafeInvoke(handler, sender, e);
            else
                handler(sender, e);
        }

        /// <summary>
        /// Efficiently executes the specified callback on the thread that owns the control's underlying window handle.
        /// </summary>
        /// <param name="control">The control to invoke upon.</param>
        /// <param name="callback">The callback to execute.</param>
        /// <param name="state">An object containing information to be used by the callback method.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is null.-or-<paramref name="callback"/> is null.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">No appropriate window handle can be found.</exception>
        public static void SmartInvoke(this Control control, WaitCallback callback, object state)
        {
            Argument.EnsureNotNull(control, "control");
            Argument.EnsureNotNull(callback, "callback");
            if (control.InvokeRequired)
                control.SafeInvoke(callback, state);
            else
                callback(state);
        }

        /// <summary>
        /// Invokes a method synchronously but using a begin/end pair in order to ensure the wait handle is disposed.
        /// </summary>
        /// <param name="control">The control to invoke upon.</param>
        /// <param name="method">The method to execute.</param>
        /// <exception cref="T:System.InvalidOperationException">No appropriate window handle can be found.</exception>
        private static void SafeInvoke(this Control control, Delegate method)
        {
            EndAndDisposeAsyncResult(control, control.BeginInvoke(method));
        }

        /// <summary>
        /// Invokes a method synchronously but using a begin/end pair in order to ensure the wait handle is disposed.
        /// </summary>
        /// <param name="control">The control to invoke upon.</param>
        /// <param name="method">The method to execute.</param>
        /// <param name="args">Arguments to pass to the method.</param>
        /// <exception cref="T:System.InvalidOperationException">No appropriate window handle can be found.</exception>
        private static void SafeInvoke(this Control control, Delegate method, params object[] args)
        {
            EndAndDisposeAsyncResult(control, control.BeginInvoke(method, args));
        }

        /// <summary>
        /// Calls EndInvoke on <paramref name="asyncResult"/> and disposes of the wait handle.
        /// </summary>
        /// <param name="control">The control to invoke upon.</param>
        /// <param name="asyncResult">The async result on which to await a result.</param>
        private static void EndAndDisposeAsyncResult(Control control, IAsyncResult asyncResult)
        {
            try
            {
                control.EndInvoke(asyncResult);
            }
            finally
            {
                asyncResult.AsyncWaitHandle.Dispose();
            }
        }

        /// <summary>
        /// Reliably enables the wait cursor on a control. Once work is complete, simply set UseWaitCursor to false.
        /// </summary>
        /// <param name="control">The control to use a wait cursor on.</param>
        /// <param name="disable">If set, the control will be disabled. You will need to re-enable the control once work is finished in
        /// this case.</param>
        /// <param name="allowDoEvents">When running under mono, a call to Application.DoEvents is required to force the cursor to appear
        /// if you will be blocking the UI thread. If this will cause unacceptable side effects, or you are not blocking the UI thread, you
        /// can leave this parameter unset. If the runtime is not mono, this parameter is ignored and Application.DoEvents is never called.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is null.</exception>
        public static void EnableWaitCursor(this Control control, bool disable, bool allowDoEvents)
        {
            Argument.EnsureNotNull(control, "control");
            // Set the cursor directly. This is required on native .NET when the method is called from the UI thread before a blocking
            // operation occurs. As soon the UI thread is freed it will reassess the cursor and revert back to the default. However whilst
            // the UI thread is blocked, the cursor remains.
            Cursor.Current = Cursors.WaitCursor;
            // Enable the wait cursor. This is required by mono in general, and native .NET when the UI will be freed but a background
            // thread is working. The wait cursor does not appear until the UI has a few moments free to process the update, making it
            // useless in a blocking UI context. Mono requires it for setting the cursor without being immediately lost.
            control.UseWaitCursor = true;
            // If the control should be disabled whilst working, do that now so we can update the form and cursor with one update.
            if (disable)
                control.Enabled = false;
            // Force an update to the control, which ensures the cursor appears under native .NET, and also handily repaints ensuring the
            // disabled UI is drawn.
            control.Update();
            // Calling Update on mono will not be sufficient to get the cursor to appear, instead messages must be forcibly pumped. Since
            // that can cause evil side effects (particularly under mono) we let the caller decide if that is sensible.
            if (allowDoEvents && Runtime.IsMono)
                Application.DoEvents();
        }
    }
}
