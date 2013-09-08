Imports System.IO

Friend Class SpeechEditor
    Private originalSpeech As Speech
    Private newSpeech As Speech

    Public Sub New()
        InitializeComponent()
        AddHandler NameTextBox.TextChanged, Sub() UpdateProperty(Sub() newSpeech.Name = NameTextBox.Text)
        AddHandler LineTextBox.TextChanged, Sub() UpdateProperty(Sub() newSpeech.Text = LineTextBox.Text)
        AddHandler SoundFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim filePath = If(SoundFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, SoundFileSelector.FilePath))
                                     newSpeech.SoundFile = filePath
                                 End Sub)
        AddHandler RandomCheckBox.CheckedChanged, Sub() UpdateProperty(Sub() newSpeech.Skip = Not RandomCheckBox.Checked)
        AddHandler GroupNumber.ValueChanged, Sub() UpdateProperty(Sub() newSpeech.Group = CInt(GroupNumber.Value))
    End Sub

    Public Overrides ReadOnly Property ItemName As String
        Get
            Return originalSpeech.Name
        End Get
    End Property

    Public Overrides Sub NewItem(name As String)
        ' TODO.
    End Sub

    Public Overrides Sub LoadItem(speechName As String)
        originalSpeech = Base.Speeches.Single(Function(s) s.Name = speechName)
        newSpeech = originalSpeech.MemberwiseClone()
        LoadItemCommon()

        Dim sounds =
            Directory.GetFiles(PonyBasePath, "*.mp3").Concat(Directory.GetFiles(PonyBasePath, "*.ogg")).
            Select(Function(filePath) Path.GetFileName(filePath)).ToArray()
        ReplaceItemsInComboBox(SoundFileSelector.FilePathComboBox, sounds, True)
        If newSpeech.SoundFile IsNot Nothing Then
            SelectItemElseAddItem(SoundFileSelector.FilePathComboBox, Path.GetFileName(newSpeech.SoundFile))
        End If

        Source.Text = newSpeech.GetPonyIni()
    End Sub

    Private Sub LoadItemCommon()
        NameTextBox.Text = newSpeech.Name
        LineTextBox.Text = newSpeech.Text
        RandomCheckBox.Checked = Not newSpeech.Skip
        GroupNumber.Value = newSpeech.Group
    End Sub

    Public Overrides Sub SaveItem()
        If Base.Speeches.Any(Function(s) s.Name = newSpeech.Name AndAlso Not Object.ReferenceEquals(s, originalSpeech)) Then
            MessageBox.Show(Me, "A speech with the name '" & newSpeech.Name &
                            "' already exists for this pony. Please choose another name.",
                            "Name Not Unique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Base.Speeches.Remove(originalSpeech)
        Base.Speeches.Add(newSpeech)

        MyBase.SaveItem()

        originalSpeech = newSpeech
        newSpeech = originalSpeech.MemberwiseClone()
    End Sub

    Protected Overrides Sub OnItemPropertyChanged()
        MyBase.OnItemPropertyChanged()
        Source.Text = newSpeech.GetPonyIni()
    End Sub

    Private Sub NameTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles NameTextBox.KeyPress
        e.Handled = (e.KeyChar = """"c)
    End Sub

    Protected Overrides Sub SourceTextChanged()
        Dim s As Speech = Nothing
        Speech.TryLoad(Source.Text, PonyBasePath, s, ParseIssues)
        OnIssuesChanged(Me, EventArgs.Empty)
        newSpeech = s
        LoadItemCommon()
    End Sub
End Class
