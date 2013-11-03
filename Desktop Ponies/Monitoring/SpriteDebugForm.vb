Imports System.Globalization
Imports DesktopSprites.SpriteManagement

Public Class SpriteDebugForm
    Public Sub New()
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Public Sub UpdateSprites(sprites As IEnumerable(Of ISprite))
        PonyDataGridView.SuspendLayout()
        Dim i = 0
        For Each pony In sprites.OfType(Of Pony)().OrderBy(Function(p) p.DisplayName)
            If i = PonyDataGridView.Rows.Count Then PonyDataGridView.Rows.Add()
            PonyDataGridView.Rows(i).SetValues(pony.DisplayName, pony.Region.Location, pony.CurrentBehavior.Name,
                                      ((pony.BehaviorStartTime + pony.BehaviorDesiredDuration) - pony.internalTime).
                                      TotalSeconds.ToString("0.00s", CultureInfo.CurrentCulture),
                                      pony.destinationCoords, pony.Destination,
                                      pony.CurrentBehavior.OriginalFollowTargetName, pony.followTargetName,
                                      If(pony.visualOverrideBehavior IsNot Nothing, pony.visualOverrideBehavior.Name, Nothing))
            PonyDataGridView.Rows(i).Tag = pony
            i += 1
        Next
        PonyDataGridView.ResumeLayout()
    End Sub

    Private Sub PonyDataGridView_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles PonyDataGridView.CellContentClick
        If e.RowIndex < 0 Then Return

        If e.ColumnIndex = colLog.Index Then
            Dim pony = TryCast(PonyDataGridView.Rows(e.RowIndex).Tag, Pony)
            If pony IsNot Nothing Then
                Dim form = New PonyLogForm(pony)
                form.Show(Me)
                PonyDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Tag = form
            End If
        End If
    End Sub
End Class