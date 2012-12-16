namespace CsDesktopPonies.SpriteManagement.MacOSX
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Exposes MacOSX native functions.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Location of the OS X Objective-C 2.0 runtime library.
        /// </summary>
        private const string objc = "/usr/lib/libobjc.dylib";

        /// <summary>
        /// Registers a method with the Objective-C runtime system, maps the method name to a selector, and returns the selector value.
        /// </summary>
        /// <param name="str">A pointer to a C string. Pass the name of the method you wish to register.</param>
        /// <returns>A pointer of type SEL specifying the selector for the named method.</returns>
        /// <remarks>You must register a method name with the Objective-C runtime system to obtain the method’s selector before you can add
        /// the method to a class definition. If the method name has already been registered, this function simply returns the selector.
        /// </remarks>
        [DllImport(objc)]
        public static extern IntPtr sel_registerName(string str);

        /// <summary>
        /// Sends a message with a simple return value to an instance of a class.
        /// </summary>
        /// <param name="theReceiver">A pointer that points to the instance of the class that is to receive the message.</param>
        /// <param name="theSelector">The selector of the method that handles the message.</param>
        /// <param name="arg">A Boolean argument to the method.</param>
        [DllImport(objc)]
        public static extern void objc_msgSend(IntPtr theReceiver, IntPtr theSelector, bool arg);
    }
}
