Imports System.IO

Public Class NewPonyDialog2
    Private _newDirectory As String
    Public ReadOnly Property NewDirectory As String
        Get
            Return _newDirectory
        End Get
    End Property

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Dim newName = txtName.Text.Trim()

        If newName = "" Then
            MessageBox.Show(Me, "You must enter a name for the new pony first.",
                            "No Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim badChars = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, "{"c, "}"c, ","c, """"c}.
            Concat(Path.GetInvalidPathChars()).Concat(Path.GetInvalidFileNameChars()).Distinct().ToArray()

        If newName.IndexOfAny(badChars) <> -1 Then
            MessageBox.Show(Me, "The pony's name cannot contain any of the following characters:" & Environment.NewLine &
                            String.Join(" ", badChars.Where(Function(c) Not Char.IsWhiteSpace(c) AndAlso Asc(c) <> 0)),
                            "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' TODO: Unique name check.

        If PonyBase.Create(newName) Then
            _newDirectory = newName
        Else
            MessageBox.Show(Me, "Failed to create this pony. Try again, or perhaps try another name.",
                            "Creation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class
