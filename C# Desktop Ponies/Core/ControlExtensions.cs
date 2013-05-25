namespace CSDesktopPonies.Core
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/>-or-<paramref name="method"/> is null.</exception>
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/>-or-<paramref name="handler"/> is null.</exception>
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/>-or-<paramref name="handler"/> is null.</exception>
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/>-or-<paramref name="handler"/> is null.</exception>
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/>-or-<paramref name="callback"/> is null.</exception>
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
    }
}
