Imports System.Globalization
Imports DesktopSprites.SpriteManagement

Public Class SpriteDebugForm
    Public Sub New()
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Public Sub UpdateSprites(sprites As IEnumerable(Of ISprite))
        If Disposing OrElse IsDisposed Then Return
        PonyDataGridView.SuspendLayout()
        Dim i = 0
        For Each pony In sprites.OfType(Of Pony)().OrderBy(Function(p) If(p.Base.Directory, p.Base.DisplayName))
            If i = PonyDataGridView.Rows.Count Then PonyDataGridView.Rows.Add()
            Dim gamePony = TryCast(pony, Game.GamePony)
            PonyDataGridView.Rows(i).SetValues(
                If(pony.Base.Directory, pony.Base.DisplayName),
                pony.Location.ToString("0.00"),
                pony.CurrentBehavior.Name,
                pony.BehaviorRemainingDuration.TotalSeconds.ToString("0.00s", CultureInfo.CurrentCulture),
                pony.Movement.ToString("0.00"),
                If(pony.Destination IsNot Nothing, pony.Destination.Value.ToString("0.00"), Nothing),
                pony.CurrentBehavior.OriginalFollowTargetName,
                If(pony.FollowTarget IsNot Nothing, If(pony.FollowTarget.Base.Directory, pony.FollowTarget.Base.DisplayName), Nothing),
                If(pony.VisualOverrideBehavior IsNot Nothing, pony.VisualOverrideBehavior.Name, Nothing),
                Nothing,
                If(gamePony IsNot Nothing AndAlso gamePony.CurrentPosition IsNot Nothing,
                   gamePony.CurrentPosition.CurrentAction.ToString(), Nothing))
            PonyDataGridView.Rows(i).Tag = pony
            i += 1
        Next
        While i < PonyDataGridView.RowCount
            PonyDataGridView.Rows(i).SetValues(New Object(PonyDataGridView.ColumnCount - 1) {})
            PonyDataGridView.Rows(i).Tag = Nothing
            i += 1
        End While
        PonyDataGridView.ResumeLayout()
    End Sub

    Private Sub PonyDataGridView_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles PonyDataGridView.CellContentClick
        If e.RowIndex < 0 Then Return

        If e.ColumnIndex = colLog.Index Then
            Dim pony = TryCast(PonyDataGridView.Rows(e.RowIndex).Tag, Pony)
            If pony IsNot Nothing Then
                Dim form = New PonyLogForm(pony)
                form.Show(Me)
            End If
        End If
    End Sub
End Class