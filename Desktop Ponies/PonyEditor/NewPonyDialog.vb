Public Class NewPonyDialog
    Private ReadOnly _ponies As PonyCollection
    Private ReadOnly _newBase As PonyBase

    Private _base As PonyBase
    Public ReadOnly Property Base As PonyBase
        Get
            Return _base
        End Get
    End Property

    Public Sub New(ponies As PonyCollection)
        InitializeComponent()
        _ponies = ponies
        _newBase = PonyBase.CreateInMemory(ponies)
        _newBase.DisplayName = "New Pony"
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If Not ValidateAndCreate() Then Return
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Function ValidateAndCreate() As Boolean
        Dim newName = txtName.Text.Trim()
        If Not EditorCommon.ValidateName(Me, "pony", newName) Then Return False

        Dim badChars = {IO.Path.DirectorySeparatorChar, IO.Path.AltDirectorySeparatorChar}.
            Concat(IO.Path.GetInvalidPathChars()).Concat(IO.Path.GetInvalidFileNameChars()).Distinct().ToArray()
        If newName.IndexOfAny(badChars) <> -1 Then
            MessageBox.Show(Me, "The pony's name cannot contain any of the following characters:" & Environment.NewLine &
                            String.Join(" ", badChars.Where(Function(c) Not Char.IsWhiteSpace(c) AndAlso Asc(c) <> 0)),
                            "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        For Each ponyBase In _ponies.Bases
            If String.Equals(ponyBase.Directory, newName, StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show(Me, "A pony with this name already exists! Please select another name or rename the other pony first.",
                                "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        Next

        Dim changed As Boolean
        Try
            changed = _newBase.ChangeDirectory(newName)
        Catch ex As Exception
            changed = False
        End Try
        If changed Then
            _newBase.DisplayName = newName
            _base = _newBase
            Return True
        Else
            MessageBox.Show(Me, "Failed to create this pony. Try again, or perhaps try another name.",
                            "Creation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
    End Function
End Class
