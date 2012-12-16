Public Class FiltersForm
    Private Sub Filters_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Icon = My.Resources.Twilight
        Filters_Box.Lines = Options.CustomTags.ToArray()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub Save_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save_Button.Click
        Options.CustomTags.Clear()
        Options.CustomTags.AddRange(Filters_Box.Lines)
        Me.Close()
    End Sub
End Class