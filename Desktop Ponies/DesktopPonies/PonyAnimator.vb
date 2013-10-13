Imports DesktopSprites.SpriteManagement

Public Enum ExitRequest
    None
    ReturnToMenu
    ExitApplication
End Enum

Public Class PonyAnimator
    Inherits AnimationLoopBase

    Protected Property ExitWhenNoSprites As Boolean = True
    Protected ReadOnly PonyCollection As PonyCollection
    Protected Friend ReadOnly ActiveSounds As New List(Of Object)()
    Private draggedPony As Pony
    Private draggedEffect As Effect
    Private draggingPonyOrEffect As Boolean
    Private draggedPonyWasSleeping As Boolean
    Private cursorPosition As Point?
    Private interactionsNeedReinitializing As Boolean

    Private _exitRequested As ExitRequest
    Public ReadOnly Property ExitRequested As ExitRequest
        Get
            Return _exitRequested
        End Get
    End Property

    ''' <summary>
    ''' Provides the z-order comparison. This sorts ponies based on the y-coordinate of the baseline of their image.
    ''' </summary>
    Private Shared ReadOnly zOrder As Comparison(Of ISprite) = Function(a, b)
                                                                   If EvilGlobals.CurrentGame IsNot Nothing Then
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
        Viewer.WindowIconFilePath = IO.Path.Combine(EvilGlobals.InstallLocation, "Twilight.ico")

        AddHandler Viewer.MouseUp, AddressOf Viewer_MouseUp
        AddHandler Viewer.MouseDown, AddressOf Viewer_MouseDown
        AddHandler Viewer.InterfaceClosed, AddressOf HandleReturnToMenu

        AddHandler SpriteAdded, AddressOf SpriteChanged
        AddHandler SpritesAdded, AddressOf SpritesChanged
        AddHandler SpriteRemoved, AddressOf SpriteChanged
        AddHandler SpritesRemoved, AddressOf SpritesChanged
    End Sub

    Private Sub SpriteChanged(sender As Object, e As CollectionItemChangedEventArgs(Of ISprite))
        If TypeOf e.Item Is Pony Then interactionsNeedReinitializing = True
    End Sub

    Private Sub SpritesChanged(sender As Object, e As CollectionItemsChangedEventArgs(Of ISprite))
        If e.Items.Any(Function(s) TypeOf s Is Pony) Then interactionsNeedReinitializing = True
    End Sub

    Private Sub InitializeInteractions()
        Dim currentPonies = Ponies()
        For Each pony In currentPonies
            pony.InitializeInteractions(currentPonies)
        Next
    End Sub

    Public Overrides Sub Start()
        InitializeInteractions()
        MyBase.Start()
    End Sub

    ''' <summary>
    ''' Updates the ponies and effect. Cycles houses.
    ''' </summary>
    Protected Overrides Sub Update()
        EvilGlobals.CursorLocation = Viewer.CursorPosition
        If EvilGlobals.InScreensaverMode Then
            ' Keep track of the cursor, and when it moves, end the screensaver.
            If cursorPosition Is Nothing Then
                cursorPosition = Viewer.CursorPosition
            ElseIf cursorPosition.Value <> Viewer.CursorPosition Then
                Finish(ExitRequest.ExitApplication)
                Return
            End If
        End If

        For Each sprite In Sprites
            Dim updated = UpdateIfPony(sprite) OrElse UpdateIfEffect(sprite) OrElse UpdateIfHouse(sprite)
        Next

        If EvilGlobals.CurrentGame IsNot Nothing Then
            EvilGlobals.CurrentGame.Update()
        End If

        If EvilGlobals.DirectXSoundAvailable Then
            CleanupSounds()
        End If

        ProcessQueuedActions()
        If interactionsNeedReinitializing Then
            InitializeInteractions()
            interactionsNeedReinitializing = False
        End If
        MyBase.Update()
        If ExitWhenNoSprites AndAlso Sprites.Count = 0 Then Finish(ExitRequest.ReturnToMenu)
        Sort(zOrder)
    End Sub

    Private Function UpdateIfPony(sprite As ISprite) As Boolean
        Dim pony = TryCast(sprite, Pony)
        If pony Is Nothing Then Return False
        If pony.AtDestination AndAlso pony.GoingHome AndAlso pony.OpeningDoor AndAlso pony.Delay <= 0 Then
            RemovePony(pony)
        End If
        Return True
    End Function

    Private Function UpdateIfEffect(sprite As ISprite) As Boolean
        Dim effect = TryCast(sprite, Effect)
        If effect Is Nothing Then Return False
        If effect.CurrentTime > TimeSpan.FromSeconds(effect.DesiredDuration) Then
            effect.OwningPony.ActiveEffects.Remove(effect)
            QueueRemove(effect)
        End If
        Return True
    End Function

    Private Function UpdateIfHouse(sprite As ISprite) As Boolean
        Dim house = TryCast(sprite, House)
        If house Is Nothing Then Return False
        house.Cycle(ElapsedTime, PonyCollection.Bases)
        Return True
    End Function

    Protected Friend Sub AddSprites(_sprites As IEnumerable(Of ISprite))
        QueueAddRangeAndStart(_sprites)
    End Sub

    Protected Friend Sub AddPony(pony As Pony)
        QueueAddAndStart(pony)
    End Sub

    Protected Friend Sub RemovePony(pony As Pony)
        QueueRemove(pony)
        For Each effect In pony.ActiveEffects
            QueueRemove(effect)
        Next
    End Sub

    Protected Friend Sub AddEffect(effect As Effect)
        QueueAddAndStart(effect)
    End Sub

    Protected Friend Sub RemoveEffect(effect As Effect)
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

    Public Overloads Sub Finish(exitMethod As ExitRequest)
        _exitRequested = exitMethod
        Finish()
    End Sub

    Public Overrides Sub Finish()
        RemoveHandler Viewer.InterfaceClosed, AddressOf HandleReturnToMenu
        RemoveHandler SpriteAdded, AddressOf SpriteChanged
        RemoveHandler SpritesAdded, AddressOf SpritesChanged
        RemoveHandler SpriteRemoved, AddressOf SpriteChanged
        RemoveHandler SpritesRemoved, AddressOf SpritesChanged
        MyBase.Finish()
    End Sub

    Private Sub HandleReturnToMenu(sender As Object, e As EventArgs)
        Finish(ExitRequest.ReturnToMenu)
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
        If EvilGlobals.InScreensaverMode Then
            Finish(ExitRequest.ExitApplication)
            Return
        End If

        If e.Buttons.HasFlag(SimpleMouseButtons.Left) Then
            If Not Options.PonyDraggingEnabled Then Return

            Dim selectedForDragPony = GetClosestUnderPoint(Of Pony)(e.Location)
            If selectedForDragPony IsNot Nothing Then
                selectedForDragPony.BeingDragged = True
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
