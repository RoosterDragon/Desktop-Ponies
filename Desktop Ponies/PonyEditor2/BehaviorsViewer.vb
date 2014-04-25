Public Class BehaviorsViewer
    Private base As PonyBase

    Protected Overrides ReadOnly Property Content As PageContent
        Get
            Return PageContent.Behaviors
        End Get
    End Property
    Protected Overrides ReadOnly Property Grid As DataGridView
        Get
            Return BehaviorsGrid
        End Get
    End Property

    Protected Overrides Iterator Function GetGridRows(ponyBase As PonyBase) As IEnumerable(Of Tuple(Of IPonyIniSourceable, Object()))
        base = ponyBase
        For Each behavior In ponyBase.Behaviors
            Yield Tuple.Create(Of IPonyIniSourceable, Object())(
                behavior, {Nothing, Nothing, behavior.Name, GetGroupName(ponyBase, behavior.Group),
                           behavior.Chance, behavior.Speed, behavior.AllowedMovement,
                           behavior.MinDuration.TotalSeconds, behavior.MaxDuration.TotalSeconds,
                           GetFileNameRelaxed(behavior.LeftImage.Path), GetFileNameRelaxed(behavior.RightImage.Path),
                           behavior.StartLineName, behavior.EndLineName, behavior.LinkedBehaviorName,
                           GetTargetName(behavior), GetTargetImageBehavior(behavior, True), GetTargetImageBehavior(behavior, False)})
        Next
    End Function

    Private Shared Function GetTargetName(behavior As Behavior) As String
        Select Case behavior.TargetMode
            Case TargetMode.Pony
                Return behavior.FollowTargetName
            Case TargetMode.Point
                Return behavior.TargetVector.ToString()
            Case Else
                Return Nothing
        End Select
    End Function

    Private Shared Function GetTargetImageBehavior(behavior As Behavior, stopped As Boolean) As String
        If behavior.TargetMode = TargetMode.None Then
            Return Nothing
        ElseIf behavior.AutoSelectImagesOnFollow Then
            Return "[Auto]"
        Else
            Return If(stopped, behavior.FollowStoppedBehaviorName, behavior.FollowMovingBehaviorName)
        End If
    End Function

    Private Sub BehaviorsGrid_SortCompare(sender As Object, e As DataGridViewSortCompareEventArgs) Handles BehaviorsGrid.SortCompare
        e.Handled = True
        Select Case e.Column.Index
            Case colChance.Index
                e.SortResult = GetBehaviorForRow(e.RowIndex1).Chance.CompareTo(GetBehaviorForRow(e.RowIndex2).Chance)
            Case colSpeed.Index
                e.SortResult = GetBehaviorForRow(e.RowIndex1).Speed.CompareTo(GetBehaviorForRow(e.RowIndex2).Speed)
            Case colMinDuration.Index
                e.SortResult = GetBehaviorForRow(e.RowIndex1).MinDuration.CompareTo(GetBehaviorForRow(e.RowIndex2).MinDuration)
            Case colMaxDuration.Index
                e.SortResult = GetBehaviorForRow(e.RowIndex1).MaxDuration.CompareTo(GetBehaviorForRow(e.RowIndex2).MaxDuration)
            Case colLeftImage.Index, colRightImage.Index
                e.SortResult = PathEquality.Compare(DirectCast(e.CellValue1, String), DirectCast(e.CellValue2, String))
            Case Else
                e.Handled = False
        End Select
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

    Private Sub GroupNamesButton_Click(sender As Object, e As EventArgs) Handles GroupNamesButton.Click
        Using dialog = New GroupNamesDialog(base)
            If dialog.ShowDialog(Me) = DialogResult.OK Then
                Me.LoadFor(base)
            End If
        End Using
    End Sub
End Class
