Public Class EffectsViewer
    Public Overrides Sub LoadFor(ponyBase As PonyBase)
        EffectsGrid.SuspendLayout()
        EffectsGrid.Rows.Clear()
        For Each effect In ponyBase.Effects
            Dim index = EffectsGrid.Rows.Add(
                Nothing,
                Nothing,
                effect.Name)
            EffectsGrid.Rows(index).Tag = effect
        Next
        EffectsGrid.ResumeLayout()
    End Sub

    Private Function GetEffectForRow(rowIndex As Integer) As EffectBase
        Return DirectCast(EffectsGrid.Rows(rowIndex).Tag, EffectBase)
    End Function

    Private Sub EffectsGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles EffectsGrid.CellContentClick
        If e.RowIndex < 0 Then Return
        Select Case e.ColumnIndex
            Case colPreview.Index
                OnPreviewRequested(New ViewerItemEventArgs(GetEffectForRow(e.RowIndex)))
            Case colEdit.Index
                OnEditRequested(New ViewerItemEventArgs(GetEffectForRow(e.RowIndex)))
        End Select
    End Sub
End Class
