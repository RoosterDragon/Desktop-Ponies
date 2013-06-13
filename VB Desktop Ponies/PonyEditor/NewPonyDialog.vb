Imports System.Windows.Forms
Imports System.IO

Public Class NewPonyDialog
    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        m_editor = editor
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Dim newName = Trim(Name_Textbox.Text)

        If newName = "" Then
            MsgBox("You must enter a name for the new pony first.")
            Exit Sub
        End If

        If InStr(newName, ",") <> 0 Then
            MsgBox("The pony's name can't have a comma in it.")
            Exit Sub
        End If

        For Each Pony In Main.Instance.SelectablePonies
            If String.Equals(Pony.Directory, newName, StringComparison.OrdinalIgnoreCase) Then
                MsgBox("A pony with this name already exists!  Please select another name or rename the other pony.")
                Exit Sub
            End If
        Next

        If InStr(newName, "{") <> 0 Then
            MsgBox("The pony's name can't have a { in it.")
            Exit Sub
        End If

        If InStr(newName, "}") <> 0 Then
            MsgBox("The pony's name can't have a } in it.")
            Exit Sub
        End If

        If m_editor.PreviewPony.Behaviors.Count = 0 Then
            MsgBox("You need to create at least one new behavior before the pony can be saved.")
            Exit Sub
        End If

        Try
            Dim newPonyPath = IO.Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, newName)
            If Directory.Exists(newPonyPath) Then
                MessageBox.Show(Me, "A pony with this name was detected. " &
                                "Please choose a different name or delete the folder for the other pony." & Environment.NewLine &
                                "Folder: " & newPonyPath, "Name Conflict", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            Directory.CreateDirectory(newPonyPath)
            m_editor.PreviewPonyBase.Directory = newPonyPath
            m_editor.PreviewPonyBase.Name = newName
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Unable to create a directory for the new pony.")
            Exit Sub
        End Try

        If m_editor.SavePony() Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub New_Pony_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Name_Textbox.Text = ""
        Name_Textbox.Enabled = True
        Right_ImageBox.Image = Nothing
    End Sub

    Private Sub First_Behavior_Button_Click(sender As Object, e As EventArgs) Handles First_Behavior_Button.Click
        Using dialog = New NewBehaviorDialog(m_editor)
            dialog.ShowDialog()
        End Using

        For Each behavior In m_editor.PreviewPony.Behaviors
            If File.Exists(behavior.RightImagePath) Then
                Right_ImageBox.Image = Image.FromFile(behavior.RightImagePath)
                Exit For
            End If
        Next
    End Sub
End Class
