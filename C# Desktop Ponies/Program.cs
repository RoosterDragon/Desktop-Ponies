namespace CSDesktopPonies
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// The main class for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Gets the directory storing the pony files.
        /// </summary>
        public static string PonyDirectory { get; private set; }
        /// <summary>
        /// Gets a value indicating whether more relaxed parsing should be used, either swallowing or skipping over errors.
        /// </summary>
        public static bool UseRelaxedParsing { get; private set; }
        /// <summary>
        /// Gets a value indicating whether .xml files should be used instead of .ini files.
        /// </summary>
        public static bool UseXmlFiles { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Initialize properties.
            PonyDirectory = "Ponies";
            UseXmlFiles = false;
            UseRelaxedParsing = true;

            // TEMP: Convert filenames to lowercase.
            //foreach (string filePath in System.IO.Directory.GetFiles(PonyDirectory, "*.mp3", System.IO.SearchOption.AllDirectories))
            //{
            //    string tempName = System.IO.Path.Combine(PonyDirectory, "temp-file-name");
            //    string fileNameOnly = System.IO.Path.GetFileName(filePath);
            //    string destPath = System.IO.Path.Combine(
            //        filePath.Remove(filePath.IndexOf(fileNameOnly)),
            //        fileNameOnly.ToLowerInvariant().Replace("_", " ").Replace("-", " "));
            //    if (filePath != destPath)
            //    {
            //        System.IO.File.Move(filePath, tempName);
            //        System.IO.File.Move(tempName, destPath);
            //    }
            //}

            // Hook up events to handle otherwise uncaught exceptions.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ApplicationExit += Application_ApplicationExit;

            // Start application.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CSDesktopPonies.DesktopPonies.PonySelectionForm());
        }

        /// <summary>
        /// Handles UI exceptions.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            NotifyUserOfException(e.Exception);
        }

        /// <summary>
        /// Handles non-UI exceptions. Note: If this occurs, the CLR will terminate even if the exception is handled.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            NotifyUserOfException((Exception)e.ExceptionObject);
        }

        /// <summary>
        /// Attempt to display a message to the user that the program is about to terminate due to an unhandled exception. Then terminate
        /// the program.
        /// </summary>
        /// <param name="ex">The unhandled exception that caused the termination.</param>
        private static void NotifyUserOfException(Exception ex)
        {
            try
            {
                string message = "Desktop Ponies encountered an error it could not handle and needs to close.";
                message += Environment.NewLine + Environment.NewLine + ex.Message;
                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    message += Environment.NewLine + Environment.NewLine + innerException.Message;
                    innerException = innerException.InnerException;
                }
                message += Environment.NewLine + Environment.NewLine + ex.StackTrace;
                MessageBox.Show(message, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Perform clean up on exit.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            // Unhook static events.
            Application.ThreadException -= Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
        }
    }
}