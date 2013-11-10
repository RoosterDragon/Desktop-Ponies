Public Class SpeechesViewer
    Protected Overrides ReadOnly Property Grid As DataGridView
        Get
            Return SpeechesGrid
        End Get
    End Property

    Protected Overrides Iterator Function GetGridRows(ponyBase As PonyBase) As IEnumerable(Of Tuple(Of IPonyIniSourceable, Object()))
        For Each speech In ponyBase.Speeches
            Yield Tuple.Create(Of IPonyIniSourceable, Object())(speech, {Nothing, speech.Name})
        Next
    End Function

    Private Sub SpeechesGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles SpeechesGrid.CellContentClick
        If e.RowIndex < 0 Then Return
        Select Case e.ColumnIndex
            Case colEdit.Index
                OnEditRequested(New ViewerItemEventArgs(GetItemForRow(e.RowIndex)))
        End Select
    End Sub
End Class
