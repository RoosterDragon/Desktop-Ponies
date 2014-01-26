Imports System.Globalization

Friend Class PonyLogForm
    Private _pony As Pony
    Private index As Integer
    Private similarRecords As Integer = 0
    Private firstSimilarRecordTime As TimeSpan
    Private lastRecord As New Pony.Record()

    Public Sub New(pony As Pony)
        InitializeComponent()
        Icon = My.Resources.Twilight

        _pony = pony
        Text = _pony.DisplayName & " Logs - Desktop Ponies"
        lblPony.Text = _pony.DisplayName
        UpdateLogs()
    End Sub

    Private Sub chkActiveRefresh_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkActiveRefresh.CheckedChanged
        RefreshTimer.Enabled = chkActiveRefresh.Checked
    End Sub

    Private Sub RefreshTimer_Tick(sender As System.Object, e As System.EventArgs) Handles RefreshTimer.Tick
        UpdateLogs()
    End Sub

    Private Sub UpdateLogs()
        LogView.SuspendLayout()
        SyncLock _pony.UpdateRecord
            While index < _pony.UpdateRecord.Count
                Dim record = _pony.UpdateRecord(index)
                index += 1
                If lastRecord.Info = record.Info Then
                    similarRecords += 1
                    LogView.Nodes(LogView.Nodes.Count - 1).Text =
                        String.Format(CultureInfo.CurrentCulture, "{0:000.000}-{1:000.000} {2} x{3}",
                                      firstSimilarRecordTime.TotalSeconds, record.Time.TotalSeconds,
                                      record.Info, similarRecords)
                Else
                    lastRecord = record
                    firstSimilarRecordTime = record.Time
                    LogView.Nodes.Add(record.ToString())
                    similarRecords = 1
                End If
            End While
        End SyncLock
        While LogView.Nodes.Count > 800
            LogView.Nodes.RemoveAt(0)
        End While
        If LogView.Nodes.Count > 0 Then LogView.Nodes(LogView.Nodes.Count - 1).EnsureVisible()
        LogView.ResumeLayout()
    End Sub
End Class