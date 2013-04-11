Imports System.IO

Friend Class SpeechEditor
    Private speechName As String

    Public Overrides Sub LoadItem(ponyBase As PonyBase, speechName As String)
        MyBase.LoadItem(ponyBase, speechName)

        Me.speechName = speechName
        Dim speech = base.SpeakingLines.First(Function(s) s.Name = speechName)
        NameTextBox.Text = speech.Name
        LineTextBox.Text = speech.Text

        Dim sounds =
            Directory.GetFiles(PonyBasePath, "*.mp3").Concat(Directory.GetFiles(PonyBasePath, "*.ogg")).
            Select(Function(filePath) Path.GetFileName(filePath)).ToArray()
        ReplaceItemsInComboBox(SoundFileSelector.FilePathComboBox, sounds, True)
        SelectItemElseNoneOption(SoundFileSelector.FilePathComboBox, Path.GetFileName(speech.SoundFile))
        RandomCheckBox.Checked = Not speech.Skip
        GroupNumber.Value = speech.Group

        Source.Text = speech.GetPonyIni()
    End Sub
End Class
