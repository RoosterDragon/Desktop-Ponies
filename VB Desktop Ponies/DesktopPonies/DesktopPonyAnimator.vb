Imports CSDesktopPonies.SpriteManagement

Public Class DesktopPonyAnimator
    Inherits AnimationLoopBase

    Protected Property ExitWhenNoSprites As Boolean = True

    Private ponyMenu As ISimpleContextMenu
    Private houseMenu As ISimpleContextMenu
    Private selectedHouse As House
    Private selectedPony As Pony
    Private draggedPony As Pony
    Private draggedEffect As Effect
    Private draggingPonyOrEffect As Boolean
    Private draggedPonyWasSleeping As Boolean
    Private poniesToRemove As New List(Of Pony)
    Private cursorPosition As Point

    Private controlForm As DesktopControlForm

#If DEBUG Then
    Private spriteDebugForm As SpriteDebugForm
#End If

    ''' <summary>
    ''' Provides the z-order comparison. This sorts ponies based on the y-coordinate of the baseline of their image.
    ''' </summary>
    Private zOrder As Comparison(Of ISprite) = Function(a, b)
                                                   If Main.Instance.CurrentGame IsNot Nothing Then
                                                       Dim aIsDisplay = TypeOf a Is Game.GameScoreboard.ScoreDisplay
                                                       Dim bIsDisplay = TypeOf b Is Game.GameScoreboard.ScoreDisplay
                                                       If aIsDisplay Xor bIsDisplay Then Return If(aIsDisplay, 1, -1)
                                                   End If
                                                   Dim aIsHouse = TypeOf a Is House
                                                   Dim bIsHouse = TypeOf b Is House
                                                   If aIsHouse Xor bIsHouse Then Return If(aIsHouse, -1, 1)
                                                   Return a.Region.Bottom - b.Region.Bottom
                                               End Function

    Public Sub New(spriteViewer As ISpriteCollectionView, spriteCollection As IEnumerable(Of ISprite), createDesktopControlForm As Boolean)
        MyBase.New(spriteViewer, spriteCollection)
        MaximumFramesPerSecond = 30
        Viewer.WindowTitle = "Desktop Ponies"
        Viewer.WindowIconFilePath = IO.Path.Combine(Options.InstallLocation, "Twilight.ico")
        AddHandler Viewer.MouseUp, AddressOf Viewer_MouseUp
        AddHandler Viewer.MouseDown, AddressOf Viewer_MouseDown
        AddHandler Viewer.InterfaceClosed, AddressOf HandleReturnToMenu

        If createDesktopControlForm Then
            Main.Instance.SmartInvoke(Sub() controlForm = New DesktopControlForm(Me))
            controlForm.SmartInvoke(Sub() controlForm.PonyComboBox.Items.AddRange(Ponies.ToArray()))
            AddHandler Sprites.ItemAdded, AddressOf ControlFormItemAdded
            AddHandler Sprites.ItemsAdded, AddressOf ControlFormItemsAdded
            AddHandler Sprites.ItemRemoved, AddressOf ControlFormItemRemoved
            AddHandler Sprites.ItemsRemoved, AddressOf ControlFormItemsRemoved
        End If

        CreatePonyMenu()
        CreateHouseMenu()
    End Sub

    Private Sub ControlFormItemAdded(sender As Object, e As CollectionItemChangedEventArgs(Of ISprite))
        Dim pony = TryCast(e.Item, Pony)
        If pony IsNot Nothing Then
            controlForm.SmartInvoke(Sub() controlForm.PonyComboBox.Items.Add(pony))
        End If
    End Sub

    Private Sub ControlFormItemsAdded(sender As Object, e As CollectionItemsChangedEventArgs(Of ISprite))
        Dim ponies = e.Items.OfType(Of Pony)().ToArray()
        controlForm.SmartInvoke(Sub() controlForm.PonyComboBox.Items.AddRange(ponies))
    End Sub

    Private Sub ControlFormItemRemoved(sender As Object, e As CollectionItemChangedEventArgs(Of ISprite))
        Dim pony = TryCast(e.Item, Pony)
        If pony IsNot Nothing AndAlso Not controlForm.Disposing AndAlso Not controlForm.IsDisposed Then
            Try
                controlForm.SmartInvoke(Sub()
                                            controlForm.PonyComboBox.Items.Remove(pony)
                                            controlForm.NotifyRemovedPonyItems()
                                        End Sub)
            Catch ex As ObjectDisposedException
                If ex.ObjectName <> controlForm.GetType().Name Then
                    Throw
                End If
            End Try
        End If
    End Sub

    Private Sub ControlFormItemsRemoved(sender As Object, e As CollectionItemsChangedEventArgs(Of ISprite))
        controlForm.SmartInvoke(Sub()
                                    For Each item In e.Items
                                        Dim pony = TryCast(item, Pony)
                                        If pony IsNot Nothing Then
                                            controlForm.PonyComboBox.Items.Remove(pony)
                                        End If
                                    Next
                                    controlForm.NotifyRemovedPonyItems()
                                End Sub)
    End Sub

    Public Overrides Sub Start()
        MyBase.Start()
        If controlForm IsNot Nothing Then controlForm.SmartInvoke(AddressOf controlForm.Show)
#If DEBUG Then
        If Not Reference.InPreviewMode Then
            Main.Instance.Invoke(Sub()
                                     spriteDebugForm = New SpriteDebugForm()
                                     spriteDebugForm.Show()
                                 End Sub)
            AddHandler spriteDebugForm.FormClosed, Sub() spriteDebugForm = Nothing
        End If
#End If
    End Sub

    ''' <summary>
    ''' Updates the ponies and effect. Cycles houses.
    ''' </summary>
    Protected Overrides Sub Update()
        If ExitWhenNoSprites AndAlso Sprites.Count = 0 Then ReturnToMenu()

        Pony.CursorLocation = Viewer.CursorPosition
        With Main.Instance
            If Reference.InScreensaverMode Then
                'keep track of the cursor and, if it moves, quit (we are supposed to act like a screensaver)
                If cursorPosition.IsEmpty Then
                    cursorPosition = Cursor.Position
                End If

                If cursorPosition <> Cursor.Position Then
                    Finish()
                    .SmartInvoke(AddressOf Main.Instance.Close)
                    Exit Sub
                End If
            End If

            poniesToRemove.Clear()

            For Each sprite In Sprites
                Dim pony = TryCast(sprite, Pony)
                If pony Is Nothing Then Continue For
                If pony.AtDestination AndAlso pony.GoingHome AndAlso pony.OpeningDoor AndAlso pony.Delay <= 0 Then
                    poniesToRemove.Add(pony)
                End If
            Next

            For Each Pony In poniesToRemove
                RemovePony(Pony)
            Next

            If Not IsNothing(.CurrentGame) Then
                .CurrentGame.Update()
            End If

            For Each sprite In Sprites
                Dim effect = TryCast(sprite, Effect)
                If effect Is Nothing Then Continue For
                If effect.CurrentTime > TimeSpan.FromSeconds(effect.DesiredDuration) Then
                    effect.OwningPony.ActiveEffects.Remove(effect)
                    Sprites.Remove(effect)
                    .DeadEffects.Add(effect)
                End If
            Next

            If .DeadEffects.Count > 0 Then
                Dim toRemove As New List(Of Effect)(.DeadEffects.Count)
                For Each effect In .DeadEffects
                    If effect.CurrentTime > TimeSpan.FromSeconds(effect.DesiredDuration) Then
                        toRemove.Add(effect)
                    End If
                Next
                For Each effect In toRemove
                    .DeadEffects.Remove(effect)
                Next
            End If

            If Reference.DirectXSoundAvailable Then
                .CleanupSounds()
            End If

            For Each sprite In Sprites
                Dim house = TryCast(sprite, House)
                If house Is Nothing Then Continue For
                house.Cycle(ElapsedTime)
            Next

            If Reference.InPreviewMode Then
                Pony.PreviewWindowRectangle = Main.Instance.GetPreviewWindowRectangle()
            End If

            MyBase.Update()
            Sprites.Sort(zOrder)
        End With
#If DEBUG Then
        countSinceLastDebug += 1
        If spriteDebugForm IsNot Nothing AndAlso countSinceLastDebug = 5 Then
            Main.Instance.Invoke(Sub() If spriteDebugForm IsNot Nothing Then spriteDebugForm.UpdateSprites(Sprites))
            countSinceLastDebug = 0
        End If
#End If
    End Sub

#If DEBUG Then
    Private countSinceLastDebug As Integer
#End If

    Public Overrides Sub Finish()
        If controlForm IsNot Nothing Then
            controlForm.SmartInvoke(AddressOf controlForm.ForceClose)
            RemoveHandler Sprites.ItemAdded, AddressOf ControlFormItemAdded
            RemoveHandler Sprites.ItemsAdded, AddressOf ControlFormItemsAdded
            RemoveHandler Sprites.ItemRemoved, AddressOf ControlFormItemRemoved
            RemoveHandler Sprites.ItemsRemoved, AddressOf ControlFormItemsRemoved
        End If
#If DEBUG Then
        If spriteDebugForm IsNot Nothing Then Main.Instance.Invoke(Sub() spriteDebugForm.Close())
#End If
        MyBase.Finish()
    End Sub

    Private Sub CreatePonyMenu()
        Dim menuItems = New LinkedList(Of SimpleContextMenuItem)()
        menuItems.AddLast(
            New SimpleContextMenuItem(
                Nothing,
                Sub()
                    Main.Instance.SmartInvoke(
                        Sub()
                            Sprites.RemoveAll(Function(sprite)
                                                  If Object.ReferenceEquals(sprite, selectedPony) Then Return True
                                                  Dim effect = TryCast(sprite, Effect)
                                                  If effect IsNot Nothing AndAlso
                                                      Object.ReferenceEquals(effect.OwningPony, selectedPony) Then
                                                      Return True
                                                  End If
                                                  Return False
                                              End Function)
                            For Each other_pony In Sprites.OfType(Of Pony)()
                                'we need to set up interactions again to account for removed ponies.
                                other_pony.InitializeInteractions(Sprites.OfType(Of Pony)())
                            Next
                        End Sub)
                End Sub))
        menuItems.AddLast(
            New SimpleContextMenuItem(
                Nothing,
                Sub()
                    Main.Instance.SmartInvoke(
                        Sub()
                            Sprites.RemoveAll(Function(sprite)
                                                  Dim pony = TryCast(sprite, Pony)
                                                  If pony IsNot Nothing AndAlso pony.Directory = selectedPony.Directory Then
                                                      Return True
                                                  End If
                                                  Dim effect = TryCast(sprite, Effect)
                                                  If effect IsNot Nothing AndAlso
                                                      effect.OwningPony.Directory = selectedPony.Directory Then
                                                      Return True
                                                  End If
                                                  Return False
                                              End Function)
                            For Each other_pony In Sprites.OfType(Of Pony)()
                                'we need to set up interactions again to account for removed ponies.
                                other_pony.InitializeInteractions(Sprites.OfType(Of Pony)())
                            Next
                        End Sub)
                End Sub))
        menuItems.AddLast(New SimpleContextMenuItem())
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub()
                                                                 If selectedPony Is Nothing Then Return
                                                                 selectedPony.ShouldBeSleeping = Not selectedPony.ShouldBeSleeping
                                                             End Sub))
        Dim allSleeping = False
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub() Main.Instance.SmartInvoke(Sub()
                                                                                                 allSleeping = Not allSleeping
                                                                                                 For Each pony In Me.Ponies()
                                                                                                     pony.ShouldBeSleeping = allSleeping
                                                                                                 Next
                                                                                             End Sub)))
        menuItems.AddLast(New SimpleContextMenuItem())
        Dim ponies As IEnumerable(Of ISimpleContextMenuItem) = Nothing
        Main.Instance.SmartInvoke(Sub() ponies = PonySelectionList())
        menuItems.AddLast(New SimpleContextMenuItem("Add Pony", ponies))
        Dim houses As IEnumerable(Of ISimpleContextMenuItem) = Nothing
        Main.Instance.SmartInvoke(Sub() houses = HouseSelectionList())
        menuItems.AddLast(New SimpleContextMenuItem("Add House", houses))
        menuItems.AddLast(New SimpleContextMenuItem())

        If Not OperatingSystemInfo.IsMacOSX Then
            menuItems.AddLast(
                New SimpleContextMenuItem(
                    Nothing,
                    Sub()
                        If selectedPony Is Nothing Then Return
                        selectedPony.ManualControlPlayerOne = Not selectedPony.ManualControlPlayerOne
                        If selectedPony.ManualControlPlayerOne Then selectedPony.ManualControlPlayerTwo = False
                    End Sub))
            menuItems.AddLast(
                New SimpleContextMenuItem(
                    Nothing,
                    Sub()
                        If selectedPony Is Nothing Then Return
                        selectedPony.ManualControlPlayerTwo = Not selectedPony.ManualControlPlayerTwo
                        If selectedPony.ManualControlPlayerTwo Then selectedPony.ManualControlPlayerOne = False
                    End Sub))
            menuItems.AddLast(New SimpleContextMenuItem())
        End If

        menuItems.AddLast(New SimpleContextMenuItem("Show Options", Sub() Main.Instance.SmartInvoke(Sub()
                                                                                                        Dim form = New OptionsForm()
                                                                                                        form.Show()
                                                                                                    End Sub)))
        menuItems.AddLast(New SimpleContextMenuItem("Return To Menu", AddressOf HandleReturnToMenu))
        menuItems.AddLast(New SimpleContextMenuItem("Exit", Sub()
                                                                Finish()
                                                                Main.Instance.SmartInvoke(AddressOf Main.Instance.Close)
                                                            End Sub))
        If controlForm Is Nothing Then
            ponyMenu = Viewer.CreateContextMenu(menuItems)
        Else
            ponyMenu = controlForm.CreateContextMenu(menuItems)
        End If
    End Sub

    Private Sub HandleReturnToMenu(sender As Object, e As EventArgs)
        ReturnToMenu()
    End Sub

    Private Sub ReturnToMenu()
        RemoveHandler Viewer.InterfaceClosed, AddressOf HandleReturnToMenu
        Finish()
        Main.Instance.SmartInvoke(Sub()
                                      Main.Instance.PonyShutdown()
                                      Main.Instance.Opacity = 100 'for when autostarted
                                      Main.Instance.Show()
                                  End Sub)
    End Sub

    Private Sub CreateHouseMenu()
        Dim menuItems As LinkedList(Of SimpleContextMenuItem) = New LinkedList(Of SimpleContextMenuItem)()
        menuItems.AddLast(
            New SimpleContextMenuItem(
                Nothing,
                Sub()
                    Main.Instance.SmartInvoke(
                        Sub()
                            If selectedHouse.HouseBase.OptionsForm IsNot Nothing Then
                                selectedHouse.HouseBase.OptionsForm.BringToFront()
                            Else
                                Using houseForm As New HouseOptionsForm(selectedHouse)
                                    selectedHouse.HouseBase.OptionsForm = houseForm
                                    houseForm.ShowDialog()
                                    houseForm.BringToFront()
                                End Using
                                selectedHouse.HouseBase.OptionsForm = Nothing
                            End If
                        End Sub)
                End Sub))
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub() RemoveEffect(selectedHouse)))
        If controlForm Is Nothing Then
            houseMenu = Viewer.CreateContextMenu(menuItems)
        Else
            houseMenu = controlForm.CreateContextMenu(menuItems)
        End If
    End Sub

    Friend Function PonySelectionList() As List(Of ISimpleContextMenuItem)

        Dim tagList = New List(Of ISimpleContextMenuItem)

        For Each tag As String In Main.Instance.FilterOptionsBox.Items
            Dim ponyList = New List(Of ISimpleContextMenuItem)
            For Each loopPony In Main.Instance.SelectablePonies
                Dim pony = loopPony
                For Each ponyTag In pony.Tags
                    If String.Equals(tag, ponyTag, StringComparison.OrdinalIgnoreCase) OrElse
                        (pony.Tags.Count = 0 AndAlso tag = "Not Tagged") Then
                        ponyList.Add(New SimpleContextMenuItem(pony.Directory, Sub(sender, e)
                                                                                   AddPonySelection(pony.Directory)
                                                                               End Sub))
                    End If
                Next
            Next
            If ponyList.Count > 0 Then
                tagList.Add(New SimpleContextMenuItem(tag, ponyList))
            End If
        Next

        Return tagList
    End Function

    Friend Function HouseSelectionList() As List(Of ISimpleContextMenuItem)
        Dim houseBaseList = New List(Of ISimpleContextMenuItem)

        For Each loopHouseBase As HouseBase In Main.Instance.HouseBases
            Dim houseBase = loopHouseBase
            houseBaseList.Add(New SimpleContextMenuItem(houseBase.Name, Sub(sender, e)
                                                                            AddHouseSelection(houseBase)
                                                                        End Sub))
        Next

        Return houseBaseList
    End Function

    Friend Sub AddPonySelection(ponyName As String)
        Main.Instance.SmartInvoke(Sub()
                                      Dim pony_to_add = ponyName

                                      If pony_to_add = "Random Pony" Then
                                          Dim selection = Rng.Next(Main.Instance.SelectablePonies.Count)

                                          pony_to_add = Main.Instance.SelectablePonies(selection).Directory

                                          If pony_to_add = "Random Pony" Then
                                              pony_to_add = Main.Instance.SelectablePonies(selection + 1).Directory
                                          End If
                                      End If
                                      For Each ponyBase In Main.Instance.SelectablePonies
                                          If ponyBase.Directory = pony_to_add Then
                                              Dim newPony = New Pony(ponyBase)
                                              AddPony(newPony)
                                          End If
                                      Next
                                  End Sub)
    End Sub

    Friend Sub AddPony(pony As Pony)
        Sprites.AddLast(pony)
        For Each other_pony In Sprites.OfType(Of Pony)()
            'we need to set up interactions again to account for added ponies.
            other_pony.InitializeInteractions(Sprites.OfType(Of Pony)())
        Next
    End Sub

    Friend Sub AddHouse(house As House)
        Sprites.AddLast(house)
    End Sub

    Friend Sub RemovePony(pony As Pony)
        Sprites.Remove(pony)
        For Each other_pony In Sprites.OfType(Of Pony)()
            'we need to set up interactions again to account for removed ponies.
            other_pony.InitializeInteractions(Sprites.OfType(Of Pony)())
        Next
        For Each effect In pony.ActiveEffects
            Sprites.Remove(effect)
        Next
    End Sub

    Friend Sub AddEffect(effect As Effect)
        Sprites.AddLast(effect)
    End Sub

    Friend Sub RemoveEffect(effect As Effect)
        Sprites.Remove(effect)
    End Sub

    Friend Sub AddSprites(_sprites As IEnumerable(Of ISprite))
        Sprites.AddRangeLast(_sprites)
    End Sub

    Friend Sub Clear()
        Sprites.Clear()
    End Sub

    Friend Function Ponies() As IEnumerable(Of Pony)
        Return Sprites.OfType(Of Pony)()
    End Function

    Friend Function Effects() As IEnumerable(Of Effect)
        Return Sprites.OfType(Of Effect)()
    End Function

    Private Sub AddHouseSelection(houseBase As HouseBase)
        Main.Instance.SmartInvoke(Sub()
                                      Dim newHouse = New House(houseBase)
                                      newHouse.InitializeVisitorList()
                                      newHouse.Teleport()
                                      Sprites.AddLast(newHouse)
                                  End Sub)
    End Sub

    Private Function GetClosestUnderPoint(Of T)(location As Point) As T
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
    Friend Sub Viewer_MouseDown(sender As Object, e As SimpleMouseEventArgs)
        If e.Buttons.HasFlag(SimpleMouseButtons.Left) Then
            If Not Options.PonyDraggingEnabled Then Exit Sub

            Dim selectedForDragPony = GetClosestUnderPoint(Of Pony)(e.Location)
            If IsNothing(selectedForDragPony) Then
                Dim selectedEffect = GetClosestUnderPoint(Of Effect)(e.Location)
                If IsNothing(selectedEffect) Then Exit Sub
                DragEffect(selectedEffect)
                Exit Sub
            End If

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
        ElseIf e.Buttons.HasFlag(SimpleMouseButtons.Right) Then
            selectedPony = GetClosestUnderPoint(Of Pony)(e.Location)
            If IsNothing(selectedPony) Then
                selectedHouse = GetClosestUnderPoint(Of House)(e.Location)
                If selectedHouse IsNot Nothing Then DisplayHouseMenu(e.Location)
                Exit Sub
            End If

            If Reference.InScreensaverMode Then Main.Instance.Close()

            Dim directory = If(selectedPony Is Nothing, "", selectedPony.Directory)
            Dim shouldBeSleeping = If(selectedPony Is Nothing, True, selectedPony.ShouldBeSleeping)
            Dim manualControlP1 = If(selectedPony Is Nothing, False, selectedPony.ManualControlPlayerOne)
            Dim manualControlP2 = If(selectedPony Is Nothing, False, selectedPony.ManualControlPlayerTwo)

            Dim i = 0
            ponyMenu.Items(i).Text = "Remove " & directory
            i += 1
            ponyMenu.Items(i).Text = "Remove Every " & directory
            i += 1
            ' Separator.
            i += 1
            ponyMenu.Items(i).Text = If(shouldBeSleeping, "Wake up/Resume", "Sleep/Pause")
            i += 1
            ponyMenu.Items(i).Text = If(Not Paused, "Wake up/Resume All", "Sleep/Pause All")
            i += 1
            ' Separator.
            i += 1
            ' Add Pony.
            i += 1
            ' Add House.
            i += 1
            ' Separator.
            i += 1

            If Not OperatingSystemInfo.IsMacOSX Then
                ponyMenu.Items(i).Text = If(Not manualControlP1, "Take Control - Player 1", "Release Control - Player 1")
                i += 1
                ponyMenu.Items(i).Text = If(Not manualControlP2, "Take Control - Player 2", "Release Control - Player 2")
                i += 1
                ' Separator.
                i += 1
            End If

            ponyMenu.Show(e.X, e.Y)
        End If
    End Sub

    Friend Sub ShowPonyMenu(x As Integer, y As Integer)
        ponyMenu.Show(x, y)
    End Sub

    Friend Sub DisplayHouseMenu(location As Point)
        Dim i = 0
        houseMenu.Items(i).Text = "Edit " & selectedHouse.HouseBase.Name
        i += 1
        houseMenu.Items(i).Text = "Remove " & selectedHouse.HouseBase.Name
        i += 1
        houseMenu.Show(location.X, location.Y)
    End Sub

    Friend Sub DragEffect(effect As Effect)
        effect.BeingDragged = True
        draggingPonyOrEffect = True
        draggedEffect = effect
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        If disposing Then
            If controlForm IsNot Nothing Then controlForm.Dispose()
#If DEBUG Then
            If spriteDebugForm IsNot Nothing Then Main.Instance.Invoke(Sub() spriteDebugForm.Dispose())
#End If
        End If
    End Sub
End Class
