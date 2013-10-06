Public Class BehaviorsViewer
    Public Overrides Sub LoadFor(ponyBase As PonyBase)
        BehaviorsGrid.SuspendLayout()
        BehaviorsGrid.Rows.Clear()
        For Each behavior In ponyBase.Behaviors
            Dim index = BehaviorsGrid.Rows.Add(
                Nothing,
                Nothing,
                behavior.Name,
                GetGroupName(ponyBase, behavior.Group),
                behavior.Chance)
            BehaviorsGrid.Rows(index).Tag = behavior
        Next
        BehaviorsGrid.ResumeLayout()
    End Sub

    Private Function GetGroupName(ponyBase As PonyBase, groupNumber As Integer) As String
        If groupNumber = Behavior.AnyGroup Then Return "Any"
        Dim group = ponyBase.BehaviorGroups.FirstOrDefault(Function(bg) bg.Number = groupNumber)
        Return If(group Is Nothing, groupNumber.ToString(Globalization.CultureInfo.CurrentCulture), group.Name.ToString())
    End Function

    Private Sub BehaviorsGrid_SortCompare(sender As Object, e As DataGridViewSortCompareEventArgs) Handles BehaviorsGrid.SortCompare
        If Object.ReferenceEquals(e.Column, colChance) Then
            e.Handled = True
            e.SortResult = GetBehaviorForRow(e.RowIndex1).Chance.CompareTo(GetBehaviorForRow(e.RowIndex2).Chance)
        End If
    End Sub

    Private Function GetBehaviorForRow(rowIndex As Integer) As Behavior
        Return DirectCast(BehaviorsGrid.Rows(rowIndex).Tag, Behavior)
    End Function

    Private Sub BehaviorsGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles BehaviorsGrid.CellContentClick
        If e.RowIndex < 0 Then Return
        Select Case e.ColumnIndex
            Case colPreview.Index
                OnPreviewRequested(New ViewerItemEventArgs(GetBehaviorForRow(e.RowIndex)))
            Case colEdit.Index
                OnEditRequested(New ViewerItemEventArgs(GetBehaviorForRow(e.RowIndex)))
        End Select
    End Sub
End Class
