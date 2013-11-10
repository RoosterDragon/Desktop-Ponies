Public Class InteractionsViewer
    Protected Overrides ReadOnly Property Content As PageContent
        Get
            Return PageContent.Interactions
        End Get
    End Property
    Protected Overrides ReadOnly Property Grid As DataGridView
        Get
            Return InteractionsGrid
        End Get
    End Property

    Protected Overrides Iterator Function GetGridRows(ponyBase As PonyBase) As IEnumerable(Of Tuple(Of IPonyIniSourceable, Object()))
        For Each interaction In ponyBase.Interactions
            Yield Tuple.Create(Of IPonyIniSourceable, Object())(
                interaction, {Nothing, interaction.Name, interaction.Chance,
                              interaction.Proximity, interaction.ReactivationDelay.TotalSeconds, interaction.Activation,
                              String.Join(", ", interaction.TargetNames), String.Join(", ", interaction.BehaviorNames)})
        Next
    End Function

    Private Sub InteractionsGrid_SortCompare(sender As Object, e As DataGridViewSortCompareEventArgs) Handles InteractionsGrid.SortCompare
        e.Handled = True
        Select Case e.Column.Index
            Case colChance.Index
                e.SortResult = GetInteractionForRow(e.RowIndex1).Chance.CompareTo(GetInteractionForRow(e.RowIndex2).Chance)
            Case colProximity.Index
                e.SortResult = GetInteractionForRow(e.RowIndex1).Proximity.CompareTo(GetInteractionForRow(e.RowIndex2).Proximity)
            Case colReactivationDelay.Index
                e.SortResult =
                    GetInteractionForRow(e.RowIndex1).ReactivationDelay.CompareTo(GetInteractionForRow(e.RowIndex2).ReactivationDelay)
            Case Else
                e.Handled = False
        End Select
    End Sub

    Private Function GetInteractionForRow(rowIndex As Integer) As InteractionBase
        Return DirectCast(InteractionsGrid.Rows(rowIndex).Tag, InteractionBase)
    End Function

    Private Sub InteractionsGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles InteractionsGrid.CellContentClick
        If e.RowIndex < 0 Then Return
        Select Case e.ColumnIndex
            Case colEdit.Index
                OnEditRequested(New ViewerItemEventArgs(GetItemForRow(e.RowIndex)))
        End Select
    End Sub
End Class
