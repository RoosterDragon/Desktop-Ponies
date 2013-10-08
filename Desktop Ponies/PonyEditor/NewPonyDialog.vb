Imports System.Windows.Forms
Imports System.IO

Public Class NewPonyDialog
    Private _editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        _editor = editor
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If Not ValidateName() Then Return
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Function ValidateName() As Boolean
        Dim newName = txtName.Text.Trim()

        If newName = "" Then
            MessageBox.Show(Me, "You must enter a name for the new pony first.",
                            "No Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        Dim badChars = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, "{"c, "}"c, ","c, """"c}.
            Concat(Path.GetInvalidPathChars()).Concat(Path.GetInvalidFileNameChars()).Distinct().ToArray()

        If newName.IndexOfAny(badChars) <> -1 Then
            MessageBox.Show(Me, "The pony's name cannot contain any of the following characters:" & Environment.NewLine &
                            String.Join(" ", badChars.Where(Function(c) Not Char.IsWhiteSpace(c) AndAlso Asc(c) <> 0)),
                            "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        For Each ponyBase In _editor.Ponies.AllBases
            If String.Equals(ponyBase.Directory, newName, StringComparison.OrdinalIgnoreCase) Then
                MsgBox("A pony with this name already exists!  Please select another name or rename the other pony.")
                Return False
            End If
        Next

        If _editor.PreviewPony.Base.ChangeDirectory(newName) Then
            _editor.PreviewPony.Base.DisplayName = newName
            Return True
        Else
            MessageBox.Show(Me, "Failed to create this pony. Try again, or perhaps try another name.",
                            "Creation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
    End Function
End Class
