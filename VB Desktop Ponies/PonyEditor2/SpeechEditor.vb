Imports System.IO

Friend Class SpeechEditor
    Private originalSpeech As Behavior.SpeakingLine
    Private newSpeech As Behavior.SpeakingLine

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

    Public Overrides Sub LoadItem(ponyBase As PonyBase, speechName As String)
        LoadingItem = True
        MyBase.LoadItem(ponyBase, speechName)

        originalSpeech = Base.SpeakingLines.Single(Function(s) s.Name = speechName)
        newSpeech = originalSpeech.MemberwiseClone()
        NameTextBox.Text = newSpeech.Name
        LineTextBox.Text = newSpeech.Text
        RandomCheckBox.Checked = Not newSpeech.Skip
        GroupNumber.Value = newSpeech.Group

        Dim sounds =
            Directory.GetFiles(PonyBasePath, "*.mp3").Concat(Directory.GetFiles(PonyBasePath, "*.ogg")).
            Select(Function(filePath) Path.GetFileName(filePath)).ToArray()
        ReplaceItemsInComboBox(SoundFileSelector.FilePathComboBox, sounds, True)
        SelectItemElseAddItem(SoundFileSelector.FilePathComboBox, Path.GetFileName(newSpeech.SoundFile))

        Source.Text = newSpeech.GetPonyIni()

        LoadingItem = False
    End Sub

    Public Overrides Sub SaveItem()
        If Base.SpeakingLines.Any(Function(s) s.Name = newSpeech.Name AndAlso Not Object.ReferenceEquals(s, originalSpeech)) Then
            MessageBox.Show(Me, "A speech with the name '" & newSpeech.Name &
                            "' already exists for this pony. Please choose another name.",
                            "Name Not Unique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Base.SpeakingLines.Remove(originalSpeech)
        Base.SpeakingLines.Add(newSpeech)

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
End Class
