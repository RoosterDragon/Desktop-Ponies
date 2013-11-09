Public Class InteractionsViewer
    Public Overrides Sub LoadFor(ponyBase As PonyBase)
        InteractionsGrid.SuspendLayout()
        InteractionsGrid.Rows.Clear()
        For Each interaction In ponyBase.Interactions
            Dim index = InteractionsGrid.Rows.Add(
                Nothing,
                interaction.Name)
            InteractionsGrid.Rows(index).Tag = interaction
        Next
        InteractionsGrid.ResumeLayout()
    End Sub

    Private Function GetEffectForRow(rowIndex As Integer) As InteractionBase
        Return DirectCast(InteractionsGrid.Rows(rowIndex).Tag, InteractionBase)
    End Function

    Private Sub InteractionsGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles InteractionsGrid.CellContentClick
        If e.RowIndex < 0 Then Return
        Select Case e.ColumnIndex
            Case colEdit.Index
                OnEditRequested(New ViewerItemEventArgs(GetEffectForRow(e.RowIndex)))
        End Select
    End Sub
End Class
