Imports CsDesktopPonies.SpriteManagement

Public Class SpriteDebugForm
    Public Sub New()
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Public Sub UpdateSprites(sprites As AsyncLinkedList(Of ISprite))
        PonyDataGridView.SuspendLayout()
        If PonyDataGridView.Rows.Count < sprites.Count Then
            PonyDataGridView.Rows.Add(sprites.Count - PonyDataGridView.Rows.Count)
        End If
        Dim i = 0
        For Each sprite In sprites
            Dim pony = TryCast(sprite, Pony)
            If pony IsNot Nothing Then
                PonyDataGridView.Rows(i).SetValues(pony.Name, pony.Location, pony.CurrentBehavior.Name,
                                          ((pony.BehaviorStartTime + pony.BehaviorDesiredDuration) - pony.internalTime).TotalSeconds.ToString("0.00s"),
                                          pony.destination_coords, pony.Destination,
                                          pony.CurrentBehavior.original_follow_object_name, pony.follow_object_name,
                                          If(pony.visual_override_behavior IsNot Nothing, pony.visual_override_behavior.Name, Nothing))
                i += 1
            End If
        Next
        PonyDataGridView.ResumeLayout()
    End Sub
End Class