Imports System.Windows.Forms
Imports System.IO

Public Class NewPonyDialog
    Private createdDirectory As String
    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        m_editor = editor
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If Not ValidateName(False) Then Return

        If m_editor.PreviewPony.Behaviors.Count = 0 Then
            MsgBox("You need to create at least one new behavior before the pony can be saved.")
            Return
        End If

        If Not m_editor.SavePony() Then Return

        ' Remove reference to the directory we created so we don't delete it when the form closes.
        createdDirectory = Nothing
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub NewPonyDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtName.Text = ""
        txtName.Enabled = True
        Right_ImageBox.Image = Nothing
    End Sub

    Private Sub First_Behavior_Button_Click(sender As Object, e As EventArgs) Handles First_Behavior_Button.Click
        If Not ValidateName(True) Then Return

        Using dialog = New NewBehaviorDialog(m_editor)
            dialog.ShowDialog()
        End Using

        For Each behavior In m_editor.PreviewPony.Behaviors
            If File.Exists(behavior.RightImagePath) Then
                Right_ImageBox.Image = Image.FromFile(behavior.RightImagePath)
                Exit For
            End If
        Next

        First_Behavior_Button.Enabled = False
    End Sub

    Private Function ValidateName(directoryMustBeNew As Boolean) As Boolean
        Dim newName = txtName.Text.Trim()

        If newName = "" Then
            MessageBox.Show(Me, "You must enter a name for the new pony first.",
                            "No Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        Dim badChars = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, "{"c, "}"c, ","c}.
            Union(Path.GetInvalidPathChars()).Union(Path.GetInvalidFileNameChars()).Distinct().ToArray()

        If newName.IndexOfAny(badChars) <> -1 Then
            MessageBox.Show(Me, "The pony's name cannot contain any of the following characters:" & Environment.NewLine &
                            String.Join(" ", badChars.Where(Function(c) Not Char.IsWhiteSpace(c) AndAlso Asc(c) <> 0)),
                            "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        For Each ponyBase In m_editor.PonyBases
            If String.Equals(ponyBase.Directory, newName, StringComparison.OrdinalIgnoreCase) Then
                MsgBox("A pony with this name already exists!  Please select another name or rename the other pony.")
                Return False
            End If
        Next

        Try
            Dim newPonyPath = Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, newName)
            If directoryMustBeNew AndAlso Directory.Exists(newPonyPath) Then
                MessageBox.Show(Me, "A pony with this name was detected. " &
                                "Please choose a different name or delete the folder for the other pony." & Environment.NewLine &
                                "Folder: " & newPonyPath, "Name Conflict", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            Directory.CreateDirectory(newPonyPath)
            RemoveOldDirectory()
            createdDirectory = newPonyPath
            m_editor.PreviewPonyBase.Directory = newPonyPath
            m_editor.PreviewPonyBase.DisplayName = newName
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Unable to create a directory for the new pony.")
            Return False
        End Try
        Return True
    End Function

    Private Sub RemoveOldDirectory()
        If createdDirectory IsNot Nothing Then
            Try
                Directory.Delete(createdDirectory)
                createdDirectory = Nothing
            Catch ex As Exception
                ' We'll try not to leave the directory around, but it's not a big problem if we can't delete it for some reason.
            End Try
        End If
    End Sub

    Private Sub NewPonyDialog_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        RemoveOldDirectory()
    End Sub
End Class
