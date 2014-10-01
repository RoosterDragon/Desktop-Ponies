namespace DesktopSprites.Core
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Defines extensions methods for <see cref="T:System.Windows.Forms.Control"/>.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Attempts to execute the specified action on the thread that owns the specified control, if the control is in a valid state.
        /// </summary>
        /// <param name="control">The control in whose thread context the specified action should be executed.</param>
        /// <param name="action">The action to execute on the owning thread of the specified control. If the calling thread owns the
        /// control the delegate is executed immediately, otherwise the action is invoked across threads.</param>
        /// <returns>A value indicating whether the action was executed. If the control lacks a valid window handle by the time the action
        /// is ready to execute, then the action is not executed and the return value is false; otherwise it is true.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is null.-or-<paramref name="action"/> is null.
        /// </exception>
        public static bool TryInvoke(this Control control, Action action)
        {
            Argument.EnsureNotNull(control, "control");
            Argument.EnsureNotNull(action, "action");
            // When creating or recreating its handle, a control locks on itself. We'll lock on the control to prevent race conditions
            // where the handle is swapped out from under us whilst we are determining if we are in a cross-thread call or not. This is
            // required because InvokeRequired returns false if the handle has yet to be created, as well as if we are on the UI thread.
            bool isCrossThreadCall;
            lock (control)
            {
                // If the control lacks an accessible window handle then we can't run our actions.
                if (!control.IsHandleCreated)
                    return false;
                // We know the handle is created, so we can rely on InvokeRequired to accurately report if this is a cross-thread call.
                isCrossThreadCall = control.InvokeRequired;
            }
            if (isCrossThreadCall)
                return CrossThreadInvoke(control, action);
            // If invoking is not required, we know we are on the UI thread with a valid control, so we can execute the specified actions
            // directly.
            action();
            return true;
        }

        /// <summary>
        /// Attempts to execute the specified action on the thread that owns the specified control, if the control is in a valid state.
        /// </summary>
        /// <param name="control">The control in whose thread context the specified action should be executed.</param>
        /// <param name="action">The action to execute on the owning thread of the specified control. The action is invoked across threads.
        /// </param>
        /// <returns>A value indicating whether the action was executed. If the control has been disposed at the time the action is ready
        /// to execute then it will not execute and the return value is false; otherwise, it is true.</returns>
        private static bool CrossThreadInvoke(Control control, Action action)
        {
            // On entering this method, we know implicitly the control has a valid window handle and a cross-thread call is required.
            IAsyncResult asyncResult;
            bool invoked = false;
            try
            {
                // We use BeginInvoke so we can get access to the wait handle being used. The normal Invoke also uses a wait handle whilst
                // it waits for the message to be processed but fails to release it, requiring that the handle be finalized. We'll step in
                // and release it deterministically.
                // The use of the MethodInvoker delegate here is intentional. The internal mechanism that process invoked calls will
                // attempt to cast to this delegate (and several others) and invoke them directly which is faster than having to invoke a
                // dynamic delegate. Despite having the same signature, an Action delegate cannot be cast to the MethodInvoker delegate.
                asyncResult = control.BeginInvoke(new MethodInvoker(() =>
                {
                    // It is possible our request was posted, but that a message earlier in the queue destroys the control. Our request
                    // will still be processed but the control will no longer be valid so we will not attempt to run the specified actions.
                    if (control.IsDisposed)
                        return;
                    // The control is still valid and since we are on the UI thread, we can now safely run the actions without fear of
                    // disposal.
                    invoked = true;
                    action();
                }));
            }
            catch (InvalidOperationException)
            {
                // If a window handle no longer exists, the control was disposed before we could make our call to BeginInvoke.
                return false;
            }
            try
            {
                // We need to wait on the completion of our action so our method completes synchronously.
                control.EndInvoke(asyncResult);
            }
            catch (ObjectDisposedException)
            {
                // The control can be disposed before we are able to wait on the result. We can ignore this as we have our flag to see if
                // the action was executed.
            }
            // Release the wait handle so it does not have to be finalized.
            asyncResult.AsyncWaitHandle.Dispose();
            // If we get this far, we know our wrapper method was executed but we need to use this flag in case we bailed from executing
            // the specified actions because the control was disposed by the time our wrapper came to execute.
            return invoked;
        }

        /// <summary>
        /// Reliably enables the wait cursor on a control. Once work is complete, simply set UseWaitCursor to false.
        /// </summary>
        /// <param name="control">The control to use a wait cursor on.</param>
        /// <param name="disable">If set, the control will be disabled. You will need to re-enable the control once work is finished in
        /// this case.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is null.</exception>
        public static void EnableWaitCursor(this Control control, bool disable)
        {
            Argument.EnsureNotNull(control, "control");
            // Mono is basically useless at doing this with any consistency - we'll only bother for native .NET on Windows.
            if (!OperatingSystemInfo.IsWindows || Runtime.IsMono)
                return;
            // Set the cursor directly. This is required when the method is called from the UI thread before a blocking operation occurs.
            // As soon the UI thread is freed it will reassess the cursor and revert back to the default. However whilst the UI thread is
            // blocked, the cursor remains.
            Cursor.Current = Cursors.WaitCursor;
            // Enable the wait cursor. This is required when the UI will be freed but a background thread is working. The wait cursor does
            // not appear until the UI has a few moments free to process the update, making it useless in a blocking UI context.
            control.UseWaitCursor = true;
            // If the control should be disabled whilst working, do that now so we can update the form and cursor with one update.
            if (disable)
                control.Enabled = false;
            // Force an update to the control, which ensures the cursor appears, and also handily repaints ensuring the disabled UI is
            // drawn.
            control.Update();
        }
    }
}
