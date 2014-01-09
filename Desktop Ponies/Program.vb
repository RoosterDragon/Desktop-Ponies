''' <summary>
''' Contains the main entry point for the program.
''' </summary>
Public NotInheritable Class Program
    Private Sub New()
    End Sub

    Public Shared Sub Main()
        AttachExceptionHandlers()
        If Not OperatingSystemInfo.IsMacOSX Then
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New MainForm())
        Else
            Gtk.Application.Init()
            Dim window = New MainWindow()
            AddHandler window.DeleteEvent, Sub() Gtk.Application.Quit()
            window.ShowAll()
            Gtk.Application.Run()
        End If
    End Sub

    Private Shared Sub AttachExceptionHandlers()
        AddHandler Threading.Tasks.TaskScheduler.UnobservedTaskException, AddressOf TaskScheduler_UnobservedTaskException
        AddHandler Application.ThreadException, AddressOf Application_ThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf AppDomain_UnhandledException
    End Sub

    Private Shared Sub TaskScheduler_UnobservedTaskException(sender As Object, e As Threading.Tasks.UnobservedTaskExceptionEventArgs)
        ' If a debugger is attached, this event is not raised (instead the exception is allowed to propagate to the debugger),
        ' therefore we'll just log since ending the process gains no additional safety at this point.
        e.SetObserved()
        LogErrorToConsole(e.Exception, "Unobserved Task Exception")
        LogErrorToDisk(e.Exception)
    End Sub

    Private Shared Sub Application_ThreadException(sender As Object, e As Threading.ThreadExceptionEventArgs)
        NotifyUserOfFatalExceptionAndExit(e.Exception)
    End Sub

    Private Shared Sub AppDomain_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
        NotifyUserOfFatalExceptionAndExit(DirectCast(e.ExceptionObject, Exception))
    End Sub

    Public Shared Sub NotifyUserOfNonFatalException(ex As Exception, message As String)
        LogErrorToConsole(ex, "WARNING: " & message)
        If Not OperatingSystemInfo.IsMacOSX Then
            ExceptionDialog.Show(ex, message, "Warning - Desktop Ponies v" & General.GetAssemblyVersion().ToDisplayString(), False)
        End If
    End Sub

    Public Shared Sub NotifyUserOfFatalExceptionAndExit(ex As Exception)
        Try
            ' Attempt to log error.
            Try
                LogErrorToConsole(ex, "FATAL: An unexpected error occurred and Desktop Ponies must close.")
                LogErrorToDisk(ex)
            Catch
                ' Logging might fail, but we'll just have to live with that.
                Console.WriteLine("An unexpected error occurred and Desktop Ponies must close. (An error file could not be generated)")
            End Try

            If Not OperatingSystemInfo.IsMacOSX Then
                ' Attempt to notify user of an unknown error.
                ExceptionDialog.Show(ex, "An unexpected error occurred and Desktop Ponies must close." &
                                     " Please report this error so it can be fixed.",
                                     "Unexpected Error - Desktop Ponies v" & General.GetAssemblyVersion().ToDisplayString(), True)
            End If
        Catch
            ' The application is already in an unreliable state, we're just trying to exit as cleanly as possible now.
        Finally
            ' Exit the program with an error code, unless a debugger is attached in which case we'll let the exception bubble to the
            ' debugger for analysis.
            If Not Diagnostics.Debugger.IsAttached Then Environment.Exit(1)
        End Try
    End Sub

    Private Shared Sub LogErrorToConsole(ex As Exception, message As String)
        Console.WriteLine("-----")
        Console.WriteLine(message)
        Console.WriteLine(
            "Error in Desktop Ponies v" & General.GetAssemblyVersion().ToDisplayString() & " occurred " & DateTime.UtcNow.ToString("u"))
        Console.WriteLine()
        Console.WriteLine(ex.ToString())
        Console.WriteLine("-----")
    End Sub

    Private Shared Sub LogErrorToDisk(ex As Exception)
        Dim path = IO.Path.Combine(EvilGlobals.InstallLocation, "error.txt")
        Using errorFile As New IO.StreamWriter(path, False, System.Text.Encoding.UTF8)
            errorFile.WriteLine(
                "Unhandled error in Desktop Ponies v" & General.GetAssemblyVersion().ToDisplayString() &
                " occurred " & DateTime.UtcNow.ToString("u"))
            errorFile.WriteLine()
            errorFile.WriteLine(ex.ToString())
            Console.WriteLine("An error file can be found at " & path)
        End Using
    End Sub
End Class
