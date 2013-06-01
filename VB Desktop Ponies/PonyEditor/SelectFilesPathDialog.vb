Imports System.Windows.Forms
Imports System.IO

Public Class SelectFilesPathDialog

    Private _selectedPath As String
    Public ReadOnly Property SelectedPath As String
        Get
            Return _selectedPath
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub Browse_Button_Click(sender As Object, e As EventArgs) Handles BrowsePathButton.Click
        If FolderBrowserDialog.ShowDialog() = DialogResult.OK Then
            _selectedPath = FolderBrowserDialog.SelectedPath
            PathTextBox.Text = _selectedPath
        End If
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If Not Directory.Exists(_selectedPath) Then
            MessageBox.Show(Me, "The selected path no longer exists, and cannot be used.",
                            "Directory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            _selectedPath = Nothing
            Return
        ElseIf Not Reference.ValidatePossibleScreensaverPath(_selectedPath) Then
            MessageBox.Show(Me, "The screensaver expects the '" & PonyBase.RootDirectory & "', '" & HouseBase.RootDirectory & "' and '" &
                            Game.RootDirectory & "' directories to exist in this location." &
                            " The path you selected does not contain these directories. Please review your selection.",
                            "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            _selectedPath = Nothing
            Return
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
