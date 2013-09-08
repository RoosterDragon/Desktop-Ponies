Public Class InteractionEditor
    Private Shared typeValues As Object() =
        [Enum].GetValues(GetType(TargetActivation)).Cast(Of Object)().ToArray()

    Private originalInteraction As Interaction
    Private newInteraction As Interaction

    Public Sub New()
        InitializeComponent()
        TypeComboBox.Items.AddRange(typeValues)
    End Sub

    Public Overrides ReadOnly Property ItemName As String
        Get
            Return originalInteraction.Name
        End Get
    End Property

    Public Overrides Sub NewItem(name As String)
        ' TODO.
    End Sub

    Public Overrides Sub LoadItem(interactionName As String)
        originalInteraction = Base.Interactions.Single(Function(i) i.Name = interactionName)
        newInteraction = originalInteraction.MemberwiseClone()

        Source.Text = newInteraction.GetPonyIni()
    End Sub

    Public Overrides Sub SaveItem()
        If Base.Interactions.Any(Function(e) e.Name = newInteraction.Name AndAlso Not Object.ReferenceEquals(e, originalInteraction)) Then
            MessageBox.Show(Me, "An interaction with the name '" & newInteraction.Name &
                            "' already exists for this pony. Please choose another name.",
                            "Name Not Unique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Base.Interactions.Remove(originalInteraction)
        Base.Interactions.Add(newInteraction)

        MyBase.SaveItem()

        originalInteraction = newInteraction
        newInteraction = originalInteraction.MemberwiseClone()
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
