Imports System.Windows.Forms

Public Class SelectFilesPathDialog

    Public Sub New()
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click

        If Not My.Computer.FileSystem.DirectoryExists(Main.Instance.screen_saver_path) Then
            MsgBox("Selected path does not exist.")
            Exit Sub
        ElseIf Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Main.Instance.screen_saver_path, PonyBase.RootDirectory)) OrElse
            Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Main.Instance.screen_saver_path, HouseBase.RootDirectory)) OrElse
            Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Main.Instance.screen_saver_path, Game.RootDirectory)) Then
            MsgBox("The screensaver expects the '" & PonyBase.RootDirectory & "', '" & HouseBase.RootDirectory &
                   "' and '" & Game.RootDirectory & "' directories to exist in this location. Please check your selection.")
            Exit Sub
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Browse_Button_Click(sender As Object, e As EventArgs) Handles BrowsePathButton.Click

        If FolderBrowserDialog.ShowDialog() = DialogResult.OK Then
            Main.Instance.screen_saver_path = FolderBrowserDialog.SelectedPath
            PathTextBox.Text = Main.Instance.screen_saver_path
        End If

    End Sub
End Class
