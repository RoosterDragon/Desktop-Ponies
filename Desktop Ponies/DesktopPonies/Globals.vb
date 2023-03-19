''' <summary>
''' Application global properties.
''' </summary>
Public NotInheritable Class Globals
    Private Sub New()
    End Sub
    Public Shared Property InScreensaverMode As Boolean

    Public Shared Function IsScreensaverExecutable() As Boolean
        Return Environment.GetCommandLineArgs()(0).EndsWith(".scr", PathEquality.Comparison)
    End Function

    Private Shared ReadOnly soundAvailableSync As New Object()
    Private Shared _SoundAvailable As Boolean?
    Public Shared ReadOnly Property SoundAvailable As Boolean
        Get
            SyncLock soundAvailableSync
                If Not _SoundAvailable.HasValue Then
                    _SoundAvailable = IsSoundAvailable()
                End If
                Return _SoundAvailable.Value
            End SyncLock
        End Get
    End Property

    Private Shared Function IsSoundAvailable() As Boolean
        Try
            Reflection.Assembly.Load("NAudio")
            Reflection.Assembly.Load("NAudio.WinMM")
            Return OperatingSystemInfo.IsWindows
        Catch ex As Exception
            ' If we can't load the assembly, just don't enable sound.
        End Try
        Return False
    End Function
End Class
