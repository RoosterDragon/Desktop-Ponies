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
        ''' Returns the file version of the executing assembly, excluding the minor, build or revision components if they are zero.
        ''' </summary>
        ''' <returns>Returns the file version of the executing assembly, excluding the minor, build or revision components if they are
        ''' zero.</returns>
        <Security.Permissions.PermissionSet(Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
        Public Shared Function GetProgramVersion() As String
            Dim fileVersionInfo = Diagnostics.FileVersionInfo.GetVersionInfo(Reflection.Assembly.GetExecutingAssembly().Location)
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

        Private faulted As Boolean
        Public ReadOnly Property IsFaulted() As Boolean
            Get
                Return faulted
            End Get
        End Property

        Protected Overrides Function OnInitialize(commandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
                                                  ) As Boolean
            If Not Diagnostics.Debugger.IsAttached Then
                AddHandler Windows.Forms.Application.ThreadException, AddressOf Application_ThreadException
                AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf AppDomain_UnhandledException
            End If

            Return MyBase.OnInitialize(commandLineArgs)
        End Function

        Private Sub Application_ThreadException(sender As Object, e As Threading.ThreadExceptionEventArgs)
            NotifyUserOfUnhandledExceptionAndExit(e.Exception)
        End Sub

        Private Sub AppDomain_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
            NotifyUserOfUnhandledExceptionAndExit(DirectCast(e.ExceptionObject, Exception))
        End Sub

        Private Sub NotifyUserOfUnhandledExceptionAndExit(ex As Exception)
            Try
                Dim exceptionString = ex.ToString()
                Dim version = GetProgramVersion()

                ' Attempt to log error.
                Try
                    Using errorFile As New IO.StreamWriter(
                        IO.Path.Combine(Options.InstallLocation, "error.txt"), False, System.Text.Encoding.UTF8)
                        errorFile.WriteLine("Unhandled error in Desktop Ponies v" & version & " occurred " & DateTime.UtcNow.ToString("u"))
                        errorFile.WriteLine()
                        errorFile.WriteLine(exceptionString)
                    End Using
                Catch
                    ' Logging might fail, but we'll just have to live with that.
                End Try

                ' Attempt to notify user of error.
                MessageBox.Show("An unexpected error occurred and Desktop Ponies must close." &
                                " Please report this error so it can be fixed." &
                                vbNewLine & vbNewLine & exceptionString,
                                "Unhandled Error - Desktop Ponies v" & version,
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch
                ' The application is already in an unreliable state, we're just trying to exit as cleanly as possible now.
            Finally
                ' Signal that the application has faulted and exit.
                faulted = True
                Windows.Forms.Application.Exit()
            End Try
        End Sub
    End Class
End Namespace

