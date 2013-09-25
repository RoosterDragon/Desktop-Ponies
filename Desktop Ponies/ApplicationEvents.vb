Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        ''' <summary>
        ''' Returns the file version of the calling assembly, excluding the minor, build or revision components if they are zero.
        ''' </summary>
        ''' <returns>Returns the file version of the calling assembly, excluding the minor, build or revision components if they are
        ''' zero.</returns>
        <Security.Permissions.PermissionSet(Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
        Public Shared Function GetProgramVersion() As String
            Dim fileVersionInfo = Diagnostics.FileVersionInfo.GetVersionInfo(Reflection.Assembly.GetCallingAssembly().Location)
            Dim fileVersion = New Version(fileVersionInfo.FileVersion)
            Dim versionFields As Integer = 1
            If fileVersion.Revision <> 0 Then
                versionFields = 4
            ElseIf fileVersion.Build <> 0 Then
                versionFields = 3
            ElseIf fileVersion.Minor <> 0 Then
                versionFields = 2
            End If

            Return fileVersion.ToString(versionFields)
        End Function

        Private exceptionHandlersAttached As Boolean

        Protected Overrides Function OnInitialize(commandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
                                                  ) As Boolean
            ' This is the first method that fires, allowing exception handlers to be set up.
            AttachExceptionHandlers()
            Return MyBase.OnInitialize(commandLineArgs)
        End Function

        Protected Overrides Sub OnRun()
            ' Mono doesn't fire all the methods in this class, so we'll wait for this one (which does fire) to attach handlers.
            AttachExceptionHandlers()
            MyBase.OnRun()
        End Sub

        Private Sub AttachExceptionHandlers()
            If Not exceptionHandlersAttached Then
                AddHandler Threading.Tasks.TaskScheduler.UnobservedTaskException, AddressOf TaskScheduler_UnobservedTaskException
                AddHandler System.Windows.Forms.Application.ThreadException, AddressOf Application_ThreadException
                AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf AppDomain_UnhandledException
                exceptionHandlersAttached = True
            End If
        End Sub

        Private Sub TaskScheduler_UnobservedTaskException(sender As Object, e As Threading.Tasks.UnobservedTaskExceptionEventArgs)
            ' If a debugger is attached, this event is not raised (instead the exception is allowed to propagate to the debugger),
            ' therefore we'll just log since ending the process gains no additional safety at this point.
            e.SetObserved()
            LogErrorToConsole(e.Exception, "Unobserved Task Exception")
            LogErrorToDisk(e.Exception)
        End Sub

        Private Sub Application_ThreadException(sender As Object, e As Threading.ThreadExceptionEventArgs)
            NotifyUserOfFatalExceptionAndExit(e.Exception)
        End Sub

        Private Sub AppDomain_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
            NotifyUserOfFatalExceptionAndExit(DirectCast(e.ExceptionObject, Exception))
        End Sub

        Public Sub NotifyUserOfNonFatalException(ex As Exception, message As String)
            LogErrorToConsole(ex, "WARNING: " & message)
            ExceptionDialog.Show(ex, message, "Warning - Desktop Ponies v" & GetProgramVersion(), False)
        End Sub

        Public Sub NotifyUserOfFatalExceptionAndExit(ex As Exception)
            Try
                ' Attempt to log error.
                Try
                    LogErrorToConsole(ex, "FATAL: An unexpected error occurred and Desktop Ponies must close.")
                    LogErrorToDisk(ex)
                Catch
                    ' Logging might fail, but we'll just have to live with that.
                    Console.WriteLine("An unexpected error occurred and Desktop Ponies must close. (An error file could not be generated)")
                End Try

                Dim version = GetProgramVersion()
                If TypeOf ex Is InvalidOperationException AndAlso
                    ex.InnerException IsNot Nothing AndAlso
                    TypeOf ex.InnerException Is ArgumentException AndAlso
                    ex.InnerException.Message = "The requested FontFamily could not be found [GDI+ status: FontFamilyNotFound]" AndAlso
                    OperatingSystemInfo.IsMacOSX Then
                    ' This is a known error with mono on Mac installations. The default fonts it attempts to find do not exist.
                    Dim message = "Your system lacks fonts required by Desktop Ponies." & Environment.NewLine &
                        "You can get these fonts by downloading XQuartz from xquartz.macosforge.org" & Environment.NewLine &
                        "The program will now exit."
                    Console.WriteLine(message)
                    MessageBox.Show(message, "Font Not Found - Desktop Ponies v" & version, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    ' Attempt to notify user of an unknown error.
                    ExceptionDialog.Show(ex, "An unexpected error occurred and Desktop Ponies must close." &
                                         " Please report this error so it can be fixed.",
                                         "Unexpected Error - Desktop Ponies v" & version, True)
                End If
            Catch
                ' The application is already in an unreliable state, we're just trying to exit as cleanly as possible now.
            Finally
                ' Exit the program with an error code, unless a debugger is attached in which case we'll let the exception bubble to the
                ' debugger for analysis.
                If Not Diagnostics.Debugger.IsAttached Then Environment.Exit(1)
            End Try
        End Sub

        Private Sub LogErrorToConsole(ex As Exception, message As String)
            Console.WriteLine("-----")
            Console.WriteLine(message)
            Console.WriteLine("Error in Desktop Ponies v" & GetProgramVersion() & " occurred " & DateTime.UtcNow.ToString("u"))
            Console.WriteLine()
            Console.WriteLine(ex.ToString())
            Console.WriteLine("-----")
        End Sub

        Private Sub LogErrorToDisk(ex As Exception)
            Dim path = IO.Path.Combine(Options.InstallLocation, "error.txt")
            Using errorFile As New IO.StreamWriter(path, False, System.Text.Encoding.UTF8)
                errorFile.WriteLine(
                    "Unhandled error in Desktop Ponies v" & GetProgramVersion() & " occurred " & DateTime.UtcNow.ToString("u"))
                errorFile.WriteLine()
                errorFile.WriteLine(ex.ToString())
                Console.WriteLine("An error file can be found at " & path)
            End Using
        End Sub
    End Class
End Namespace
