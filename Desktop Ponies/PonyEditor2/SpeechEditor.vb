Imports System.IO

Friend Class SpeechEditor
    Private Shadows Property Edited As Speech
        Get
            Return DirectCast(MyBase.Edited, Speech)
        End Get
        Set(value As Speech)
            MyBase.Edited = value
        End Set
    End Property
    Protected Overrides ReadOnly Property Collection As System.Collections.IList
        Get
            Return CType(Base.Speeches, Collections.IList)
        End Get
    End Property
    Protected Overrides ReadOnly Property ItemTypeName As String
        Get
            Return "speech"
        End Get
    End Property

    Private sound As Object

    Public Sub New()
        InitializeComponent()
        AddHandler Disposed, Sub()
                                 Dim disposable = TryCast(sound, IDisposable)
                                 If disposable IsNot Nothing Then disposable.Dispose()
                             End Sub
    End Sub

    Protected Overrides Sub CreateBindings()
        Bind(Function() Edited.Name, NameTextBox)
        Bind(Function() Edited.Text, LineTextBox)
        Bind(Function() Edited.SoundFile, SoundFileSelector)
        Bind(Function() Edited.Skip, RandomCheckBox, True)
        Bind(Function() Edited.Group, GroupNumber, Function(int) CDec(int), Function(dec) CInt(dec))
    End Sub

    Protected Overrides Sub ChangeItem()
        SoundFileSelector.InitializeFromDirectory(PonyBasePath, "*.mp3", "*.ogg")
    End Sub

    Protected Overrides Sub ReparseSource(ByRef parseIssues As ImmutableArray(Of ParseIssue))
        Dim s As Speech = Nothing
        Speech.TryLoad(Source.Text, PonyBasePath, s, parseIssues)
        Edited = s
    End Sub

    Private Sub SoundFileSelector_FilePathSelected(sender As Object, e As EventArgs) Handles SoundFileSelector.FilePathSelected
        StopSound()
        SoundPreviewPanel.Enabled = SoundFileSelector.FilePath IsNot Nothing AndAlso EvilGlobals.DirectXSoundAvailable
        UpdateSoundLabel(0, 0)
    End Sub

    Private Sub SoundPreviewButton_Click(sender As Object, e As EventArgs) Handles SoundPreviewButton.Click
        If sound Is Nothing Then
            PlaySound()
        Else
            StopSound()
        End If
    End Sub

    Private Sub PlaySound()
        Try
            Dim fullPath = IO.Path.Combine(SoundFileSelector.BaseDirectory, SoundFileSelector.FilePath)
            Dim audio As New Microsoft.DirectX.AudioVideoPlayback.Audio(fullPath)
            audio.Volume = CInt(Options.SoundVolume * 10000 - 10000)
            audio.Play()
            sound = audio
            SoundPreviewButton.Text = "Stop"
            UpdateSoundLabel(audio.CurrentPosition, audio.Duration)
            SoundTimer.Start()
        Catch ex As Exception
            SoundPreviewLabel.Text = "Unable to play sound file."
            My.Application.NotifyUserOfNonFatalException(ex, "Unable to play sound file.")
        End Try
    End Sub

    Private Sub SoundTimer_Tick(sender As Object, e As EventArgs) Handles SoundTimer.Tick
        Dim audio = DirectCast(sound, Microsoft.DirectX.AudioVideoPlayback.Audio)
        Dim currentPosition As Double = 0
        Dim duration As Double = audio.Duration
        If audio.CurrentPosition >= duration Then
            StopSound()
        Else
            currentPosition = audio.CurrentPosition
        End If
        UpdateSoundLabel(currentPosition, duration)
    End Sub

    Private Sub UpdateSoundLabel(currentPosition As Double, duration As Double)
        SoundPreviewLabel.Text = String.Format("{0:0.000} / {1:0.000}",
                                               TimeSpan.FromSeconds(currentPosition).TotalSeconds,
                                               TimeSpan.FromSeconds(duration).TotalSeconds)
    End Sub

    Private Sub StopSound()
        SoundTimer.Stop()
        If sound Is Nothing Then Return
        Dim audio = DirectCast(sound, Microsoft.DirectX.AudioVideoPlayback.Audio)
        sound = Nothing
        audio.Stop()
        UpdateSoundLabel(0, audio.Duration)
        audio.Dispose()
        SoundPreviewButton.Text = "Play"
    End Sub
End Class
