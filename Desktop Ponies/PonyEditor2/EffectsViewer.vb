Public Class EffectsViewer
    Protected Overrides ReadOnly Property Grid As DataGridView
        Get
            Return EffectsGrid
        End Get
    End Property

    Protected Overrides Iterator Function GetGridRows(ponyBase As PonyBase) As IEnumerable(Of Tuple(Of IPonyIniSourceable, Object()))
        For Each effect In ponyBase.Effects
            Yield Tuple.Create(Of IPonyIniSourceable, Object())(effect, {Nothing, Nothing, effect.Name})
        Next
    End Function

    Private Sub EffectsGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles EffectsGrid.CellContentClick
        If e.RowIndex < 0 Then Return
        Select Case e.ColumnIndex
            Case colPreview.Index
                OnPreviewRequested(New ViewerItemEventArgs(GetItemForRow(e.RowIndex)))
            Case colEdit.Index
                OnEditRequested(New ViewerItemEventArgs(GetItemForRow(e.RowIndex)))
        End Select
    End Sub
End Class
