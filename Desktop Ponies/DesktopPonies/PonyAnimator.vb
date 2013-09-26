Imports DesktopSprites.SpriteManagement

Public Class PonyAnimator
    Inherits AnimationLoopBase

    Protected Property ExitWhenNoSprites As Boolean = True
    Protected ReadOnly PonyCollection As PonyCollection
    Protected Friend ReadOnly ActiveSounds As New List(Of Object)()
    Private draggedPony As Pony
    Private draggedEffect As Effect
    Private draggingPonyOrEffect As Boolean
    Private draggedPonyWasSleeping As Boolean
    Private cursorPosition As Point

    ''' <summary>
    ''' Provides the z-order comparison. This sorts ponies based on the y-coordinate of the baseline of their image.
    ''' </summary>
    Private Shared ReadOnly zOrder As Comparison(Of ISprite) = Function(a, b)
                                                                   If Game.CurrentGame IsNot Nothing Then
                                                                       Dim aIsDisplay = TypeOf a Is Game.GameScoreboard.ScoreDisplay
                                                                       Dim bIsDisplay = TypeOf b Is Game.GameScoreboard.ScoreDisplay
                                                                       If aIsDisplay Xor bIsDisplay Then Return If(aIsDisplay, 1, -1)
                                                                   End If
                                                                   Dim aIsHouse = TypeOf a Is House
                                                                   Dim bIsHouse = TypeOf b Is House
                                                                   If aIsHouse Xor bIsHouse Then Return If(aIsHouse, -1, 1)
                                                                   Return a.Region.Bottom - b.Region.Bottom
                                                               End Function

    Public Sub New(spriteViewer As ISpriteCollectionView, spriteCollection As IEnumerable(Of ISprite), ponyCollection As PonyCollection)
        MyBase.New(spriteViewer, spriteCollection)
        Me.PonyCollection = Argument.EnsureNotNull(ponyCollection, "ponyCollection")

        MaximumFramesPerSecond = 30
        Viewer.WindowTitle = "Desktop Ponies"
        Viewer.WindowIconFilePath = IO.Path.Combine(Options.InstallLocation, "Twilight.ico")

        AddHandler Viewer.MouseUp, AddressOf Viewer_MouseUp
        AddHandler Viewer.MouseDown, AddressOf Viewer_MouseDown
        AddHandler Viewer.InterfaceClosed, AddressOf HandleReturnToMenu

        AddHandler SpriteAdded, AddressOf SpriteChanged
        AddHandler SpritesAdded, AddressOf SpritesChanged
        AddHandler SpriteRemoved, AddressOf SpriteChanged
        AddHandler SpritesRemoved, AddressOf SpritesChanged
    End Sub

    Private Sub SpriteChanged(sender As Object, e As CollectionItemChangedEventArgs(Of ISprite))
        If TypeOf e.Item Is Pony Then ReinitializeInteractions()
    End Sub

    Private Sub SpritesChanged(sender As Object, e As CollectionItemsChangedEventArgs(Of ISprite))
        If e.Items.Any(Function(s) TypeOf s Is Pony) Then ReinitializeInteractions()
    End Sub

    Protected Sub ReinitializeInteractions()
        Dim ponies = Sprites.OfType(Of Pony)()
        For Each pony In ponies
            pony.InitializeInteractions(ponies)
        Next
    End Sub

    Public Overrides Sub Start()
        MyBase.Start()
        If Options.EnablePonyLogs AndAlso Not Reference.InPreviewMode Then
            Main.Instance.SmartInvoke(Sub()
                                          spriteDebugForm = New SpriteDebugForm()
                                          spriteDebugForm.Show()
                                      End Sub)
            AddHandler spriteDebugForm.FormClosed, Sub() spriteDebugForm = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Updates the ponies and effect. Cycles houses.
    ''' </summary>
    Protected Overrides Sub Update()
        Pony.CursorLocation = Viewer.CursorPosition
        If Reference.InScreensaverMode Then
            'keep track of the cursor and, if it moves, quit (we are supposed to act like a screensaver)
            If cursorPosition.IsEmpty Then
                cursorPosition = Cursor.Position
            End If

            If cursorPosition <> Cursor.Position Then
                Finish()
                Main.Instance.SmartInvoke(AddressOf Main.Instance.Close)
                Exit Sub
            End If
        End If

        Dim poniesRemoved = False
        For Each sprite In Sprites
            Dim pony = TryCast(sprite, Pony)
            If pony Is Nothing Then Continue For
            If pony.AtDestination AndAlso pony.GoingHome AndAlso pony.OpeningDoor AndAlso pony.Delay <= 0 Then
                RemovePony(pony)
                poniesRemoved = True
            End If
        Next
        If poniesRemoved Then ReinitializeInteractions()

        If Not IsNothing(Game.CurrentGame) Then
            Game.CurrentGame.Update()
        End If

        For Each sprite In Sprites
            Dim effect = TryCast(sprite, Effect)
            If effect Is Nothing Then Continue For
            If effect.CurrentTime > TimeSpan.FromSeconds(effect.DesiredDuration) Then
                effect.OwningPony.ActiveEffects.Remove(effect)
                QueueRemove(effect)
            End If
        Next

        If Reference.DirectXSoundAvailable Then
            CleanupSounds()
        End If

        For Each sprite In Sprites
            Dim house = TryCast(sprite, House)
            If house Is Nothing Then Continue For
            house.Cycle(ElapsedTime, PonyCollection.Bases)
        Next

        If Reference.InPreviewMode Then
            Pony.PreviewWindowRectangle = Main.Instance.GetPreviewWindowRectangle()
        End If

        MyBase.Update()
        If ExitWhenNoSprites AndAlso Sprites.Count = 0 Then ReturnToMenu()
        Sort(zOrder)
    End Sub

    Protected Friend Sub AddSprites(_sprites As IEnumerable(Of ISprite))
        QueueAddRangeAndStart(_sprites)
    End Sub

    Protected Friend Sub AddPony(pony As Pony)
        QueueAddAndStart(pony)
    End Sub

    Protected Sub RemovePony(pony As Pony)
        QueueRemove(pony)
        For Each effect In pony.ActiveEffects
            QueueRemove(effect)
        Next
    End Sub

    Public Sub RemovePonyAndReinitializeInteractions(pony As Pony)
        RemovePony(pony)
        ReinitializeInteractions()
    End Sub

    Protected Friend Sub AddEffect(effect As Effect)
        QueueAddAndStart(effect)
    End Sub

    Protected Sub RemoveEffect(effect As Effect)
        QueueRemove(effect)
    End Sub

    Public Sub Clear()
        QueueClear()
    End Sub

    Public Function Ponies() As IEnumerable(Of Pony)
        Return Sprites.OfType(Of Pony)()
    End Function

    Public Function Effects() As IEnumerable(Of Effect)
        Return Sprites.OfType(Of Effect)()
    End Function

    Private Sub CleanupSounds()
        Dim soundsToRemove As LinkedList(Of Microsoft.DirectX.AudioVideoPlayback.Audio) = Nothing

        For Each sound As Microsoft.DirectX.AudioVideoPlayback.Audio In ActiveSounds
            If sound.State = Microsoft.DirectX.AudioVideoPlayback.StateFlags.Paused OrElse
                sound.CurrentPosition >= sound.Duration Then
                sound.Dispose()
                If soundsToRemove Is Nothing Then soundsToRemove = New LinkedList(Of Microsoft.DirectX.AudioVideoPlayback.Audio)
                soundsToRemove.AddLast(sound)
            End If
        Next

        If soundsToRemove IsNot Nothing Then
            For Each sound In soundsToRemove
                ActiveSounds.Remove(sound)
            Next
        End If
    End Sub

    Public Overrides Sub Finish()
        RemoveHandler SpriteAdded, AddressOf SpriteChanged
        RemoveHandler SpritesAdded, AddressOf SpritesChanged
        RemoveHandler SpriteRemoved, AddressOf SpriteChanged
        RemoveHandler SpritesRemoved, AddressOf SpritesChanged
        If spriteDebugForm IsNot Nothing Then
            Main.Instance.SmartInvoke(AddressOf spriteDebugForm.Close)
            spriteDebugForm = Nothing
        End If
        MyBase.Finish()
    End Sub

    Protected Sub ReturnToMenu()
        FinishViewer()
        Main.Instance.SmartInvoke(Sub()
                                      Main.Instance.PonyShutdown()
                                      Main.Instance.Opacity = 100 'for when autostarted
                                      Main.Instance.Show()
                                  End Sub)
    End Sub

    Protected Sub FinishViewer()
        RemoveHandler Viewer.InterfaceClosed, AddressOf HandleReturnToMenu
        Finish()
    End Sub

    Protected Sub HandleReturnToMenu(sender As Object, e As EventArgs)
        ReturnToMenu()
    End Sub

    ''' <summary>
    ''' Occurs when a mouse button is raised.
    ''' Any dragged pony will be dropped.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The event data.</param>
    Private Sub Viewer_MouseUp(sender As Object, e As SimpleMouseEventArgs)

        If e.Buttons.HasFlag(SimpleMouseButtons.Left) Then
            If draggingPonyOrEffect Then
                If Not IsNothing(draggedPony) Then
                    draggedPony.BeingDragged = False
                    If draggedPonyWasSleeping = False Then
                        draggedPony.ShouldBeSleeping = False
                    End If
                End If
                If Not IsNothing(draggedEffect) Then
                    draggedEffect.BeingDragged = False
                End If
                draggingPonyOrEffect = False

                draggedPony = Nothing
                draggedEffect = Nothing
            End If
        End If

    End Sub

    ''' <summary>
    ''' Occurs when a mouse button is pressed down initially.
    ''' Selects the nearest pony to be dragged by the mouse.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The event data.</param>
    Private Sub Viewer_MouseDown(sender As Object, e As SimpleMouseEventArgs)
        If e.Buttons.HasFlag(SimpleMouseButtons.Left) Then
            If Not Options.PonyDraggingEnabled Then Exit Sub

            Dim selectedForDragPony = GetClosestUnderPoint(Of Pony)(e.Location)
            If selectedForDragPony IsNot Nothing Then                selectedForDragPony.BeingDragged = True
                draggedPony = selectedForDragPony
                draggingPonyOrEffect = True
                If Not Paused Then
                    selectedForDragPony.BeingDragged = True
                    If selectedForDragPony.Sleeping = False Then
                        selectedForDragPony.ShouldBeSleeping = True
                        draggedPonyWasSleeping = False
                    Else
                        draggedPonyWasSleeping = True
                    End If
                End If
            Else
                Dim selectedforDragEffect = GetClosestUnderPoint(Of Effect)(e.Location)
                If selectedforDragEffect IsNot Nothing Then
                    selectedforDragEffect.BeingDragged = True
                    draggedEffect = selectedforDragEffect
                    draggingPonyOrEffect = True
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Protected Function GetClosestUnderPoint(Of T)(location As Point) As T
        Dim selected As T = Nothing
        Dim smallestDistance = Single.MaxValue

        For Each sprite In Sprites
            If Not TypeOf sprite Is T Then
                Continue For
            End If
            Dim currentDistance = Vector2F.DistanceSquared(sprite.Region.Center(), CType(location, Vector2))
            If currentDistance < smallestDistance AndAlso sprite.Region.Contains(location) Then
                smallestDistance = currentDistance
                selected = CType(sprite, T)
            End If
        Next

        Return selected
    End Function
End Class
