Public Class FiltersForm
    Private Sub Filters_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Icon = My.Resources.Twilight
        Filters_Box.Lines = Options.CustomTags.ToArray()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub Save_Button_Click(sender As Object, e As EventArgs) Handles Save_Button.Click
        Options.CustomTags.Clear()
        Options.CustomTags.AddRange(Filters_Box.Lines)
        Me.Close()
    End Sub
End Class