Imports System.IO

Public NotInheritable Class Reference
    Private Sub New()
    End Sub

    Private Shared screensaverSettingsPath As String = Path.Combine(Path.GetTempPath, "DesktopPonies_ScreenSaver_Settings.ini")

    Private Shared _directXSoundAvailable As Boolean
    Public Shared ReadOnly Property DirectXSoundAvailable As Boolean
        Get
            Return _directXSoundAvailable
        End Get
    End Property
    ''' <summary>
    ''' Are ponies currently walking around the desktop?
    ''' </summary>
    Public Shared Property PoniesHaveLaunched As Boolean
    Public Shared Property InScreensaverMode As Boolean
    Public Shared Property InPreviewMode As Boolean

    Shared Sub New()
        ' Check to see if the right version of DirectX is installed for sounds.
        Try
            System.Reflection.Assembly.Load(
                "Microsoft.DirectX.AudioVideoPlayback, Version=1.0.2902.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")
            _directXSoundAvailable = True
        Catch ex As Exception
            ' If we can't load the assembly, just don't enable sound.
        End Try
    End Sub

    Public Shared Function TryGetScreensaverPath() As String
        Try
            Dim path As String
            Using reader As New StreamReader(screensaverSettingsPath)
                path = reader.ReadLine()
            End Using
            If Not ValidatePossibleScreensaverPath(path) Then Return Nothing
            Return path
        Catch ex As IOException
            Return Nothing
        End Try
    End Function

    Public Shared Function ValidatePossibleScreensaverPath(possiblePath As String) As Boolean
        Return Directory.Exists(Path.Combine(possiblePath, PonyBase.RootDirectory)) AndAlso
            Directory.Exists(Path.Combine(possiblePath, HouseBase.RootDirectory)) AndAlso
            Directory.Exists(Path.Combine(possiblePath, Game.RootDirectory))
    End Function

    Public Shared Function SetScreensaverPath() As Boolean
        Try
            Try
                If File.Exists(screensaverSettingsPath) Then
                    Using settingsFile As New StreamReader(screensaverSettingsPath)
                        SelectFilesPathDialog.PathTextBox.Text = settingsFile.ReadLine()
                    End Using
                End If
            Catch ex As IOException
                ' Ignore any problem trying to load current settings, it's just a user convenience.
            End Try

            Dim newPath As String = Nothing
            Using dialog = New SelectFilesPathDialog()
                If dialog.ShowDialog() <> DialogResult.OK Then Return False
                newPath = dialog.SelectedPath
            End Using

            Using writer = New StreamWriter(screensaverSettingsPath, False, System.Text.Encoding.UTF8)
                writer.WriteLine(newPath)
            End Using
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Unable to save settings! Screensaver mode will not work.")
            Return False
        End Try
        Return True
    End Function
End Class
