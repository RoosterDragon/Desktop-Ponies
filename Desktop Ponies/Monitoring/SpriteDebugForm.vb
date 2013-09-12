Imports DesktopSprites.SpriteManagement

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
                PonyDataGridView.Rows(i).SetValues(pony.DisplayName, pony.TopLeftLocation, pony.CurrentBehavior.Name,
                                          ((pony.BehaviorStartTime + pony.BehaviorDesiredDuration) - pony.internalTime).TotalSeconds.ToString("0.00s"),
                                          pony.destinationCoords, pony.Destination,
                                          pony.CurrentBehavior.OriginalFollowObjectName, pony.followObjectName,
                                          If(pony.visualOverrideBehavior IsNot Nothing, pony.visualOverrideBehavior.Name, Nothing))
                i += 1
            End If
        Next
        PonyDataGridView.ResumeLayout()
    End Sub
End Class