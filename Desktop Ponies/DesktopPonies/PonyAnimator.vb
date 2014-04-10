Imports DesktopSprites.SpriteManagement

Public Enum ExitRequest
    None
    ReturnToMenu
    ExitApplication
End Enum

Public MustInherit Class PonyAnimator
    Inherits AnimationLoopBase

    Protected Property ExitWhenNoSprites As Boolean = True
    Protected ReadOnly PonyCollection As PonyCollection
    Protected ReadOnly PonyContext As PonyContext
    Private _manualControlPlayerOne As Pony
    Protected Property ManualControlPlayerOne As Pony
        Get
            Return _manualControlPlayerOne
        End Get
        Set(value As Pony)
            If _manualControlPlayerOne IsNot Nothing Then
                _manualControlPlayerOne.SpeedOverride = Nothing
                _manualControlPlayerOne.DestinationOverride = Nothing
            End If
            If Object.ReferenceEquals(_manualControlPlayerTwo, value) Then
                _manualControlPlayerTwo = Nothing
            End If
            _manualControlPlayerOne = value
        End Set
    End Property
    Private _manualControlPlayerTwo As Pony
    Protected Property ManualControlPlayerTwo As Pony
        Get
            Return _manualControlPlayerTwo
        End Get
        Set(value As Pony)
            If _manualControlPlayerTwo IsNot Nothing Then
                _manualControlPlayerTwo.SpeedOverride = Nothing
                _manualControlPlayerTwo.DestinationOverride = Nothing
            End If
            If Object.ReferenceEquals(_manualControlPlayerOne, value) Then
                _manualControlPlayerOne = Nothing
            End If
            _manualControlPlayerTwo = value
        End Set
    End Property
    Public Property AllowManualControl As Boolean = True
    Protected Property ManualControlSpeed As Single = 100

    Private ReadOnly activeSounds As New List(Of Object)()
    Private globalSoundEnd As Date
    Private ReadOnly soundEndBySprite As New Dictionary(Of ISoundfulSprite, Date)()

    Private draggedSprite As IDraggableSprite
    Private initialCursorPosition As Point?
    Private interactionsNeedReinitializing As Boolean
    Private ReadOnly removeExpiredSpriteHandler As EventHandler = AddressOf RemoveExpiredSprite

    Private _exitRequested As ExitRequest
    Public ReadOnly Property ExitRequested As ExitRequest
        Get
            Return _exitRequested
        End Get
    End Property

    ''' <summary>
    ''' Provides the z-order comparison. This sorts ponies based on the y-coordinate of the baseline of their image.
    ''' </summary>
    Private Shared ReadOnly _zOrderer As Comparison(Of ISprite) = Function(a, b) a.Region.Bottom - b.Region.Bottom

    Protected Overridable ReadOnly Property ZOrderer As Comparison(Of ISprite)
        Get
            Return _zOrderer
        End Get
    End Property

    Public Sub New(spriteViewer As ISpriteCollectionView, spriteCollection As IEnumerable(Of ISprite),
                   ponyCollection As PonyCollection, ponyContext As PonyContext)
        MyBase.New(spriteViewer, spriteCollection)
        Me.PonyCollection = Argument.EnsureNotNull(ponyCollection, "ponyCollection")
        Me.PonyContext = Argument.EnsureNotNull(ponyContext, "ponyContext")
        Me.PonyContext.Sprites = Sprites

        MaximumFramesPerSecond = 25
        Viewer.WindowTitle = "Desktop Ponies"
        Viewer.WindowIconFilePath = "Twilight.ico"

        AddHandler Viewer.InterfaceClosed, Sub() _exitRequested = ExitRequest.ReturnToMenu

        If spriteCollection IsNot Nothing Then
            AttachExpiredHandlers(spriteCollection.OfType(Of IExpireableSprite)())
        End If

        AddHandler SpriteAdded, AddressOf InvalidateInteractions
        AddHandler SpritesAdded, AddressOf InvalidateInteractions
        AddHandler SpriteRemoved, AddressOf InvalidateInteractions
        AddHandler SpritesRemoved, AddressOf InvalidateInteractions

        AddHandler SpriteAdded, AddressOf AddExpiredHandlers
        AddHandler SpritesAdded, AddressOf AddExpiredHandlers
        AddHandler SpriteRemoved, AddressOf ExpireSprite
        AddHandler SpritesRemoved, AddressOf ExpireSprites
    End Sub

    Private Sub InvalidateInteractions(sender As Object, e As CollectionItemChangedEventArgs(Of ISprite))
        If TypeOf e.Item Is Pony Then interactionsNeedReinitializing = True
    End Sub

    Private Sub InvalidateInteractions(sender As Object, e As CollectionItemsChangedEventArgs(Of ISprite))
        If e.Items.Any(Function(s) TypeOf s Is Pony) Then interactionsNeedReinitializing = True
    End Sub

    Private Sub AddExpiredHandlers(sender As Object, e As CollectionItemChangedEventArgs(Of ISprite))
        Dim expireableSprite = TryCast(e.Item, IExpireableSprite)
        If expireableSprite IsNot Nothing Then AddHandler expireableSprite.Expired, removeExpiredSpriteHandler
    End Sub

    Private Sub AddExpiredHandlers(sender As Object, e As CollectionItemsChangedEventArgs(Of ISprite))
        AttachExpiredHandlers(e.Items.OfType(Of IExpireableSprite)())
    End Sub

    Private Sub AttachExpiredHandlers(expirableSprites As IEnumerable(Of IExpireableSprite))
        For Each expireableSprite In expirableSprites
            AddHandler expireableSprite.Expired, removeExpiredSpriteHandler
        Next
    End Sub

    Private Sub RemoveExpiredSprite(sender As Object, e As EventArgs)
        QueueRemove(DirectCast(sender, IExpireableSprite))
    End Sub

    Private Sub ExpireSprite(sender As Object, e As CollectionItemChangedEventArgs(Of ISprite))
        Dim expireableSprite = TryCast(e.Item, IExpireableSprite)
        If expireableSprite IsNot Nothing Then RemoveExpiredHandlerAndExpireSprite(expireableSprite)
    End Sub

    Private Sub ExpireSprites(sender As Object, e As CollectionItemsChangedEventArgs(Of ISprite))
        For Each expireableSprite In e.Items.OfType(Of IExpireableSprite)()
            RemoveExpiredHandlerAndExpireSprite(expireableSprite)
        Next
    End Sub

    Private Sub RemoveExpiredHandlerAndExpireSprite(expireableSprite As IExpireableSprite)
        RemoveHandler expireableSprite.Expired, removeExpiredSpriteHandler
        expireableSprite.Expire()
    End Sub

    Private Sub InitializeInteractions()
        For Each pony In Ponies
            pony.InitializeInteractions(Ponies)
        Next
    End Sub

    Public Overrides Sub Start()
        InitializeInteractions()
        ' We need to set this before showing the viewer on Windows. If we need to hide it from the taskbar after showing it, the window
        ' ordering gets a bit screwed up for some reason.
        Viewer.ShowInTaskbar = False
        MyBase.Start()
    End Sub

    ''' <summary>
    ''' Updates the ponies and effects.
    ''' </summary>
    Protected Overrides Sub Update()
        If PonyContext.PendingSprites.Count > 0 Then
            QueueAddRangeAndStart(PonyContext.PendingSprites)
            PonyContext.PendingSprites.Clear()
        End If
        PonyContext.CursorLocation = Viewer.CursorPosition
        SynchronizeContext()
        If Disposed Then Return
        SynchronizeViewer()
        If Disposed Then Return

        ' When the cursor moves, or a mouse button is pressed, end the screensaver.
        If Globals.InScreensaverMode Then
            If initialCursorPosition Is Nothing Then
                initialCursorPosition = Viewer.CursorPosition
            ElseIf initialCursorPosition.Value <> Viewer.CursorPosition OrElse Viewer.MouseButtonsDown <> SimpleMouseButtons.None Then
                Finish(ExitRequest.ExitApplication)
                Return
            End If
        End If

        ' Handle dragging and dropping on sprites.
        If Viewer.HasFocus AndAlso (Viewer.MouseButtonsDown And SimpleMouseButtons.Left) = SimpleMouseButtons.Left Then
            If Options.PonyDraggingEnabled AndAlso draggedSprite Is Nothing Then
                Dim dragCandidate = If(GetClosestUnderPoint(Of Pony)(Viewer.CursorPosition),
                                       GetClosestUnderPoint(Of IDraggableSprite)(Viewer.CursorPosition))
                If dragCandidate IsNot Nothing Then
                    draggedSprite = dragCandidate
                    draggedSprite.Drag = True
                End If
            End If
        Else
            If draggedSprite IsNot Nothing Then
                draggedSprite.Drag = False
                draggedSprite = Nothing
            End If
        End If

        ' Handle player controlled movement.
        If AllowManualControl Then
            ManualControl(ManualControlPlayerOne,
                          KeyboardState.IsKeyPressed(Keys.Up),
                          KeyboardState.IsKeyPressed(Keys.Down),
                          KeyboardState.IsKeyPressed(Keys.Left),
                          KeyboardState.IsKeyPressed(Keys.Right),
                          KeyboardState.IsKeyPressed(Keys.RShiftKey))
            ManualControl(ManualControlPlayerTwo,
                          KeyboardState.IsKeyPressed(Keys.W),
                          KeyboardState.IsKeyPressed(Keys.S),
                          KeyboardState.IsKeyPressed(Keys.A),
                          KeyboardState.IsKeyPressed(Keys.D),
                          KeyboardState.IsKeyPressed(Keys.LShiftKey))
        End If

        ' Process queued actions now, so the sprite collection is up to date. Then we can tell if interactions need to be reinitialized.
        ProcessQueuedActions()
        If interactionsNeedReinitializing Then
            InitializeInteractions()
            interactionsNeedReinitializing = False
        End If

        MyBase.Update()
        If ExitWhenNoSprites AndAlso Sprites.Count = 0 Then
            Finish(ExitRequest.ReturnToMenu)
            Return
        End If
        Sort(ZOrderer)
        If Globals.DirectXSoundAvailable Then
            PlaySounds()
            CleanupSounds(False)
        End If
    End Sub

    Protected Overridable Sub SynchronizeContext()
        PonyContext.SynchronizeWithGlobalOptions()
    End Sub

    Protected MustOverride Sub SynchronizeViewer()

    Private Sub ManualControl(pony As Pony, up As Boolean, down As Boolean, left As Boolean, right As Boolean, speedBoost As Boolean)
        If pony Is Nothing Then Return
        Dim movement As Vector2F
        If up Then movement.Y -= 1
        If down Then movement.Y += 1
        If left Then movement.X -= 1
        If right Then movement.X += 1
        Dim length = movement.Length()
        If length > 0 Then
            movement /= length
            Dim speed = If(speedBoost, ManualControlSpeed * 2, ManualControlSpeed)
            movement *= speed
            pony.SpeedOverride = speed
            pony.DestinationOverride = pony.Location + movement
        Else
            pony.SpeedOverride = 0
            pony.DestinationOverride = pony.Location
        End If
    End Sub

    Private Sub PlaySounds()
        ' Sound must be enabled for the mode we are in.
        If Disposed Then Return
        If Globals.InScreensaverMode Then
            If Not Options.ScreensaverSoundEnabled Then Return
        Else
            If Not Options.SoundEnabled Then Exit Sub
        End If

        ' If only one sound at a time is allowed, wait for it to finish.
        If Options.SoundSingleChannelOnly AndAlso globalSoundEnd > Date.UtcNow Then Return

        For Each sprite In Sprites
            Dim soundfulSprite = TryCast(sprite, ISoundfulSprite)
            If soundfulSprite Is Nothing OrElse soundfulSprite.SoundPath Is Nothing Then Continue For

            ' If one sound per sprite is allowed, wait for it to finish.
            If Not Options.SoundSingleChannelOnly Then
                Dim soundEndDate As Date
                If soundEndBySprite.TryGetValue(soundfulSprite, soundEndDate) Then
                    If soundEndDate > Date.UtcNow Then
                        Continue For
                    Else
                        soundEndBySprite.Remove(soundfulSprite)
                    End If
                End If
            End If

            Dim audio As Microsoft.DirectX.AudioVideoPlayback.Audio = Nothing
            Try
                audio = New Microsoft.DirectX.AudioVideoPlayback.Audio(soundfulSprite.SoundPath)
                ' Volume is between -10000 and 0, with 0 being the loudest.
                audio.Volume = CInt(Options.SoundVolume * 10000 - 10000)
                audio.Play()
            Catch ex As Exception
                ' Swallow any exception here. The sound file may be missing, inaccessible, not a playable format, etc.
                If audio IsNot Nothing Then audio.Dispose()
            Finally
                If audio IsNot Nothing AndAlso Not audio.Disposed Then
                    activeSounds.Add(audio)
                    If Options.SoundSingleChannelOnly Then
                        globalSoundEnd = Date.UtcNow + TimeSpan.FromSeconds(audio.Duration)
                    Else
                        soundEndBySprite(soundfulSprite) = Date.UtcNow + TimeSpan.FromSeconds(audio.Duration)
                    End If
                End If
            End Try
        Next
    End Sub

    Private Sub CleanupSounds(force As Boolean)
        Dim soundsToRemove As LinkedList(Of Microsoft.DirectX.AudioVideoPlayback.Audio) = Nothing

        For Each sound As Microsoft.DirectX.AudioVideoPlayback.Audio In activeSounds
            If force OrElse sound.CurrentPosition >= sound.Duration Then
                sound.StopWhenReady()
                sound.Dispose()
                If soundsToRemove Is Nothing Then soundsToRemove = New LinkedList(Of Microsoft.DirectX.AudioVideoPlayback.Audio)()
                soundsToRemove.AddLast(sound)
            End If
        Next

        If soundsToRemove IsNot Nothing Then
            For Each sound In soundsToRemove
                activeSounds.Remove(sound)
            Next
        End If
    End Sub

    Protected Friend Sub RemoveSprite(sprite As ISprite)
        QueueRemove(sprite)
    End Sub

    Protected Friend Sub Clear()
        QueueClear()
    End Sub

    Protected ReadOnly Property Ponies As IEnumerable(Of Pony)
        Get
            Return Sprites.OfType(Of Pony)()
        End Get
    End Property

    Public Overloads Sub Finish(exitMethod As ExitRequest)
        _exitRequested = exitMethod
        Finish()
    End Sub

    Public Overrides Sub Finish()
        Dim alreadyDisposed = Disposed
        If Not alreadyDisposed Then
            RemoveHandler SpriteAdded, AddressOf InvalidateInteractions
            RemoveHandler SpritesAdded, AddressOf InvalidateInteractions
            RemoveHandler SpriteRemoved, AddressOf InvalidateInteractions
            RemoveHandler SpritesRemoved, AddressOf InvalidateInteractions
            For Each expireableSprite In Sprites.OfType(Of IExpireableSprite)()
                RemoveExpiredHandlerAndExpireSprite(expireableSprite)
            Next
        End If
        MyBase.Finish()
        If Not alreadyDisposed AndAlso Globals.DirectXSoundAvailable Then CleanupSounds(True)
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
