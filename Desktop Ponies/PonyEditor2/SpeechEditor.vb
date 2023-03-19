Friend Class SpeechEditor
    Private Shadows Property Edited As Speech
        Get
            Return DirectCast(MyBase.Edited, Speech)
        End Get
        Set(value As Speech)
            MyBase.Edited = value
        End Set
    End Property
    Protected Overrides ReadOnly Property Collection As Collections.IList
        Get
            Return Base.Speeches
        End Get
    End Property
    Protected Overrides ReadOnly Property ItemTypeName As String
        Get
            Return "speech"
        End Get
    End Property

    Private sound As Tuple(Of Object, Object)

    Public Sub New()
        InitializeComponent()
        AddHandler Disposed, Sub()
                                 If sound Is Nothing Then Return
                                 Dim disposable1 = TryCast(sound.Item1, IDisposable)
                                 If disposable1 IsNot Nothing Then disposable1.Dispose()
                                 Dim disposable2 = TryCast(sound.Item2, IDisposable)
                                 If disposable2 IsNot Nothing Then disposable2.Dispose()
                             End Sub
    End Sub

    Protected Overrides Sub CreateBindings()
        Bind(Function() Edited.Name, NameTextBox)
        Bind(Function() Edited.Text, LineTextBox)
        Bind(Function() Edited.SoundFile, SoundFileSelector)
        Bind(Function() Edited.Skip, RandomCheckBox, True)
        Bind(Function() Edited.Group, GroupNumber, Function(int) int, Function(dec) CInt(dec))
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
        SoundPreviewPanel.Enabled = SoundFileSelector.FilePath IsNot Nothing AndAlso Globals.SoundAvailable
        UpdateSoundLabel(TimeSpan.Zero, TimeSpan.Zero)
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
            Dim audio As New NAudio.Wave.AudioFileReader(fullPath) With {
                .Volume = Options.SoundVolume
            }
            Dim output = New NAudio.Wave.WaveOutEvent()
            output.Init(audio)
            output.Play()
            sound = New Tuple(Of Object, Object)(audio, output)
            SoundPreviewButton.Text = "Stop"
            UpdateSoundLabel(audio.CurrentTime, audio.TotalTime)
            SoundTimer.Start()
        Catch ex As Exception
            SoundPreviewLabel.Text = "Unable to play sound file."
            Program.NotifyUserOfNonFatalException(ex, "Unable to play sound file.")
        End Try
    End Sub

    Private Sub SoundTimer_Tick(sender As Object, e As EventArgs) Handles SoundTimer.Tick
        Dim audio = DirectCast(sound.Item1, NAudio.Wave.AudioFileReader)
        Dim currentPosition As TimeSpan = TimeSpan.Zero
        Dim duration As TimeSpan = audio.TotalTime
        If audio.CurrentTime >= duration Then
            StopSound()
        Else
            currentPosition = audio.CurrentTime
        End If
        UpdateSoundLabel(currentPosition, duration)
    End Sub

    Private Sub UpdateSoundLabel(currentPosition As TimeSpan, duration As TimeSpan)
        SoundPreviewLabel.Text = "{0:0.000} / {1:0.000}".FormatWith(
            currentPosition.TotalSeconds, duration.TotalSeconds)
    End Sub

    Private Sub StopSound()
        SoundTimer.Stop()
        If sound Is Nothing Then Return
        Dim audio = DirectCast(sound.Item1, NAudio.Wave.AudioFileReader)
        Dim output = DirectCast(sound.Item2, NAudio.Wave.WaveOutEvent)
        sound = Nothing
        output.Stop()
        UpdateSoundLabel(TimeSpan.Zero, audio.TotalTime)
        output.Dispose()
        audio.Dispose()
        SoundPreviewButton.Text = "Play"
    End Sub
End Class
