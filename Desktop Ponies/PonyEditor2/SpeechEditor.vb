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
End Class
