Public Class InteractionEditor
    Private Shared typeValues As Object() =
        [Enum].GetValues(GetType(TargetActivation)).Cast(Of Object)().ToArray()

    Private originalInteraction As Interaction
    Private newInteraction As Interaction

    Public Sub New()
        InitializeComponent()
        TypeComboBox.Items.AddRange(typeValues)
    End Sub

    Public Overrides Sub NewItem(name As String)
        ' TODO.
    End Sub

    Public Overrides Sub LoadItem()
    End Sub

    Protected Overrides Sub OnItemPropertyChanged()
        MyBase.OnItemPropertyChanged()
        Source.Text = newInteraction.GetPonyIni()
    End Sub

    Private Sub NameTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles NameTextBox.KeyPress
        e.Handled = (e.KeyChar = """"c)
    End Sub

    Protected Overrides Sub SourceTextChanged()
        ' TODO.
    End Sub
End Class
