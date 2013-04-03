Public Class FileSelector
    Public Event FilePathSelected As EventHandler

    Public ReadOnly Property FilePath As String
        Get
            Return If(FilePathComboBox.SelectedIndex <= 0, Nothing, FilePathComboBox.SelectedItem.ToString())
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
        FilePathComboBox.SelectedIndex = 0
    End Sub

    Private Sub FilePathChooseButton_Click(sender As Object, e As EventArgs) Handles FilePathChooseButton.Click
        Using dialog As New OpenFileDialog()
            If dialog.ShowDialog(Me.ParentForm) = DialogResult.OK Then
                FilePathComboBox.Items.Add(dialog.FileName)
                FilePathComboBox.SelectedItem = dialog.FileName
            End If
        End Using
    End Sub

    Private Sub FilePathComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FilePathComboBox.SelectedIndexChanged
        RaiseEvent FilePathSelected(Me, EventArgs.Empty)
    End Sub
End Class
