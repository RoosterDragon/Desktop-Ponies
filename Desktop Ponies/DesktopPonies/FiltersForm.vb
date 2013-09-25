Public Class FiltersForm
    Public Sub New()
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub Filters_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Filters_Box.Lines = Options.CustomTags.ToArray()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub Save_Button_Click(sender As Object, e As EventArgs) Handles Save_Button.Click
        Dim lines = Filters_Box.Lines.ToList()
        If lines.RemoveAll(Function(line) line.IndexOf(ControlChars.Quote) <> -1) > 0 Then
            MessageBox.Show(Me, "You cannot use the quote character in custom tags. Any tags with this character have been removed.",
                            "Invalid Tags", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        Options.CustomTags.Clear()
        Options.CustomTags.AddRange(lines)
        Me.Close()
    End Sub
End Class