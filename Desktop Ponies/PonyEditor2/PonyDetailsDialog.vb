Imports System.IO

Public Class PonyDetailsDialog

    Private ReadOnly base As PonyBase

    Public Sub New(base As PonyBase)
        Me.base = Argument.EnsureNotNull(base, "base")
        InitializeComponent()
        Icon = My.Resources.Twilight

        NameTextBox.Text = base.Directory
        DisplayNameTextBox.Text = base.DisplayName
        TagsList.Items.AddRange(EvilGlobals.Main.FilterOptionsBox.Items)
        For i = 0 To TagsList.Items.Count - 1
            If base.Tags.Contains(DirectCast(TagsList.Items(i), CaseInsensitiveString)) Then
                TagsList.SetItemChecked(i, True)
            End If
        Next
    End Sub

    Private Sub DisplayNameTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DisplayNameTextBox.KeyPress
        e.Handled = e.KeyChar = """"c
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click
        Dim newName = NameTextBox.Text.Trim()

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

        If base.Directory <> newName Then
            If MessageBox.Show(Me, "Renaming this pony will break other references. Continue with save?",
                               "Rename?", MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Return
            End If
        End If

        If Not base.ChangeDirectory(newName) Then
            MessageBox.Show(Me, "Error attempting to rename pony. Please try again.",
                            "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        base.DisplayName = DisplayNameTextBox.Text
        base.Tags.Clear()
        base.Tags.UnionWith(TagsList.CheckedItems.Cast(Of CaseInsensitiveString)())

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class
