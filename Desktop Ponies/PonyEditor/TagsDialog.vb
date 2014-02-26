Public Class TagsDialog
    Private _editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        Icon = My.Resources.Twilight
        _editor = editor
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        _editor.CurrentBase.Tags.Clear()
        _editor.CurrentBase.Tags.RemoveWhere(
            Function(tag) Options.CustomTags.Contains(tag) OrElse PonyBase.StandardTags.Contains(tag))
        _editor.CurrentBase.Tags.UnionWith(PonyFilterList.CheckedItems.Cast(Of CaseInsensitiveString)())

        DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Tags_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Tags for " & _editor.CurrentBase.Directory

        For Each tag As CaseInsensitiveString In PonyBase.StandardTags.Concat(Options.CustomTags)
            PonyFilterList.Items.Add(tag)
        Next

        For i = 0 To PonyFilterList.Items.Count - 1
            Dim tag = DirectCast(PonyFilterList.Items(i), CaseInsensitiveString)
            If _editor.CurrentBase.Tags.Contains(tag) Then
                PonyFilterList.SetItemChecked(i, True)
            End If
        Next
    End Sub
End Class