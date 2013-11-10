Public Class InteractionsViewer
    Protected Overrides ReadOnly Property Grid As DataGridView
        Get
            Return InteractionsGrid
        End Get
    End Property

    Protected Overrides Iterator Function GetGridRows(ponyBase As PonyBase) As IEnumerable(Of Tuple(Of IPonyIniSourceable, Object()))
        For Each interaction In ponyBase.Interactions
            Yield Tuple.Create(Of IPonyIniSourceable, Object())(interaction, {Nothing, interaction.Name})
        Next
    End Function

    Private Sub InteractionsGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles InteractionsGrid.CellContentClick
        If e.RowIndex < 0 Then Return
        Select Case e.ColumnIndex
            Case colEdit.Index
                OnEditRequested(New ViewerItemEventArgs(GetItemForRow(e.RowIndex)))
        End Select
    End Sub
End Class
