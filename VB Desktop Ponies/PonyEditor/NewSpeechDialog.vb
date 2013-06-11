Imports System.Windows.Forms

Public Class NewSpeechDialog

    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        m_editor = editor
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click

        If Trim(Name_Textbox.Text) = "" Then
            MsgBox("You must enter a name for the new speech.")
            Exit Sub
        End If

        If Trim(Text_TextBox.Text) = "" Then
            MsgBox("You need to enter some sort of text for the pony to say.")
            Exit Sub

        End If

        Dim filename = m_editor.GetFilename(Sound_Textbox.Text)


        Dim new_speech As New Behavior.SpeakingLine(m_editor.PreviewPony.Name, _
                                                          Name_Textbox.Text, _
                                                          Text_TextBox.Text, _
                                                          Replace(Sound_Textbox.Text, filename, ""), _
                                                          filename, _
                                                          Not Random_Checkbox.Checked, CInt(Group_NumberBox.Value))

        m_editor.PreviewPonyBase.SpeakingLines.Add(new_speech)

        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click

        Me.Close()
    End Sub

    Private Sub New_Behavior_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Text_TextBox.Text = ""
        Name_Textbox.Text = ""


    End Sub


    Private Sub SetSound_Button_Click(sender As Object, e As EventArgs) Handles SetSound_Button.Click

        OpenSoundDialog.Filter = "MP3 Files (*.mp3)|*.mp3"
        OpenSoundDialog.InitialDirectory = IO.Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, m_editor.PreviewPony.Directory)

        Dim sound_path As String = Nothing

        If OpenSoundDialog.ShowDialog() = DialogResult.OK Then
            sound_path = OpenSoundDialog.FileName
        End If

        If IsNothing(sound_path) Then
            Exit Sub
        End If

        Dim new_path = IO.Path.Combine(Options.InstallLocation, PonyBase.RootDirectory,
                                       m_editor.PreviewPony.Directory, m_editor.GetFilename(sound_path))

        If new_path <> sound_path Then
            If Not IO.File.Exists(new_path) Then
                IO.File.Create(new_path).Close()
            End If
            IO.File.Copy(sound_path, new_path, True)
        End If

        Sound_Textbox.Text = sound_path

    End Sub


End Class
