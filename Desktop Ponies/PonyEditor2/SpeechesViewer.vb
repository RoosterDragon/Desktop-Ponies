Public Class SpeechesViewer
    Public Overrides Sub LoadFor(ponyBase As PonyBase)
        SpeechesGrid.SuspendLayout()
        SpeechesGrid.Rows.Clear()
        For Each speech In ponyBase.Speeches
            Dim index = SpeechesGrid.Rows.Add(
                Nothing,
                speech.Name)
            SpeechesGrid.Rows(index).Tag = speech
        Next
        SpeechesGrid.ResumeLayout()
    End Sub

    Private Function GetEffectForRow(rowIndex As Integer) As Speech
        Return DirectCast(SpeechesGrid.Rows(rowIndex).Tag, Speech)
    End Function

    Private Sub SpeechesGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles SpeechesGrid.CellContentClick
        If e.RowIndex < 0 Then Return
        Select Case e.ColumnIndex
            Case colEdit.Index
                OnEditRequested(New ViewerItemEventArgs(GetEffectForRow(e.RowIndex)))
        End Select
    End Sub
End Class
