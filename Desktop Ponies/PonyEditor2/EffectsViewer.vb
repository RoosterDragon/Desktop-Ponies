Public Class EffectsViewer
    Protected Overrides ReadOnly Property Content As PageContent
        Get
            Return PageContent.Effects
        End Get
    End Property
    Protected Overrides ReadOnly Property Grid As DataGridView
        Get
            Return EffectsGrid
        End Get
    End Property

    Protected Overrides Iterator Function GetGridRows(ponyBase As PonyBase) As IEnumerable(Of Tuple(Of IPonyIniSourceable, Object()))
        For Each effect In ponyBase.Effects
            Yield Tuple.Create(Of IPonyIniSourceable, Object())(
                effect, {Nothing, Nothing, effect.Name, effect.Duration, effect.RepeatDelay, effect.BehaviorName,
                         effect.Follow, effect.DoNotRepeatImageAnimations,
                         GetFileNameRelaxed(effect.LeftImage.Path), GetFileNameRelaxed(effect.RightImage.Path),
                         effect.PlacementDirectionLeft, effect.CenteringLeft, effect.PlacementDirectionRight, effect.CenteringRight
                        })
        Next
    End Function

    Private Sub EffectsGrid_SortCompare(sender As Object, e As DataGridViewSortCompareEventArgs) Handles EffectsGrid.SortCompare
        e.Handled = True
        Select Case e.Column.Index
            Case colDuration.Index
                e.SortResult = GetEffectForRow(e.RowIndex1).Duration.CompareTo(GetEffectForRow(e.RowIndex2).Duration)
            Case colRepeatDelay.Index
                e.SortResult = GetEffectForRow(e.RowIndex1).RepeatDelay.CompareTo(GetEffectForRow(e.RowIndex2).RepeatDelay)
            Case colLeftImage.Index, colRightImage.Index
                e.SortResult = PathEquality.Comparer.Compare(e.CellValue1, e.CellValue2)
            Case Else
                e.Handled = False
        End Select
    End Sub

    Private Function GetEffectForRow(rowIndex As Integer) As EffectBase
        Return DirectCast(EffectsGrid.Rows(rowIndex).Tag, EffectBase)
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
