Public Class TagsForm
    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        Icon = My.Resources.Twilight
        m_editor = editor
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        m_editor.PreviewPony.Tags.Clear()

        For Each Tag As String In PonyFilterList.CheckedItems
            m_editor.PreviewPony.Tags.Add(Tag)
        Next

        Me.Close()

    End Sub

    Private Sub Tags_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        PonyFilterList.Items.Clear()

        Me.Text = "Tags for " & m_editor.PreviewPony.Directory

        For Each category As String In Main.Instance.FilterOptionsBox.Items
            If category = "Not Tagged" Then Continue For
            PonyFilterList.Items.Add(category)
        Next

        For Each Tag As String In m_editor.PreviewPony.Tags
            For Each category As String In PonyFilterList.Items
                If LCase(Tag) = LCase(category) Then
                    PonyFilterList.SetItemChecked(PonyFilterList.Items.IndexOf(category), True)
                    Exit For
                End If
            Next
        Next


    End Sub

End Class