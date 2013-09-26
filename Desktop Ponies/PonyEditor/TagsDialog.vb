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
        _editor.PreviewPony.Base.Tags.Clear()

        For Each Tag As String In PonyFilterList.CheckedItems
            _editor.PreviewPony.Base.Tags.Add(Tag)
        Next

        DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Tags_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PonyFilterList.Items.Clear()

        Me.Text = "Tags for " & _editor.PreviewPony.Directory

        For Each category As String In Main.Instance.FilterOptionsBox.Items
            If category = "Not Tagged" Then Continue For
            PonyFilterList.Items.Add(category)
        Next

        For i = 0 To PonyFilterList.Items.Count - 1
            Dim tag = DirectCast(PonyFilterList.Items(i), String)
            If _editor.PreviewPony.Base.Tags.Contains(tag) Then
                PonyFilterList.SetItemChecked(i, True)
            End If
        Next
    End Sub
End Class