Public Class NewSpeechDialog

    Private ReadOnly editor As PonyEditor

    Public Sub New(editor As PonyEditor)
        Me.editor = Argument.EnsureNotNull(editor, "editor")
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub SoundSelectButton_Click(sender As Object, e As EventArgs) Handles SoundSelectButton.Click
        Dim path = EditorCommon.PromptUserForSoundPath(Me, editor.CurrentBase)
        If path Is Nothing Then Return
        SoundTextBox.Text = IO.Path.GetFileName(path)
        SoundClearButton.Enabled = True
    End Sub

    Private Sub SoundClearButton_Click(sender As Object, e As EventArgs) Handles SoundClearButton.Click
        SoundTextBox.Text = Nothing
        SoundClearButton.Enabled = False
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Dim newName = NameTextBox.Text.Trim()
        If Not EditorCommon.ValidateName(Me, "speech", newName, editor.CurrentBase.Speeches) Then Return

        If String.IsNullOrEmpty(LineTextBox.Text) Then
            MessageBox.Show(Me, "You need to enter something for the pony to say.",
                            "No Text Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim newSpeech = New Speech() With {.Name = newName,
                                           .Text = LineTextBox.Text,
                                           .Skip = Not UseRandomlyCheckBox.Checked,
                                           .Group = CInt(GroupNumber.Value),
                                           .SoundFile = If(String.IsNullOrEmpty(SoundTextBox.Text), Nothing, SoundTextBox.Text)}
        editor.CurrentBase.Speeches.Add(newSpeech)

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class
