Imports DesktopSprites.SpriteManagement

Public Class DesktopPonyAnimator
    Inherits PonyAnimator

    Private ponyMenu As ISimpleContextMenu
    Private houseMenu As ISimpleContextMenu
    Protected selectedHouse As House
    Protected selectedPony As Pony

    Private ReadOnly controlFormLock As New Object()
    Private controlForm As DesktopControlForm

    Private spriteDebugForm As SpriteDebugForm
    Private countSinceLastDebug As Integer

    Public Sub New(spriteViewer As ISpriteCollectionView, spriteCollection As IEnumerable(Of ISprite), ponyCollection As PonyCollection)
        MyBase.New(spriteViewer, spriteCollection, ponyCollection)

        If OperatingSystemInfo.IsMacOSX Then
            Main.Instance.SmartInvoke(Sub() controlForm = New DesktopControlForm(Me))
            controlForm.SmartInvoke(Sub() controlForm.PonyComboBox.Items.AddRange(Ponies().ToArray()))
            AddHandler SpriteAdded, AddressOf ControlFormItemAdded
            AddHandler SpritesAdded, AddressOf ControlFormItemsAdded
            AddHandler SpriteRemoved, AddressOf ControlFormItemRemoved
            AddHandler SpritesRemoved, AddressOf ControlFormItemsRemoved
        End If

        If Options.EnablePonyLogs AndAlso Not Reference.InPreviewMode Then
            Main.Instance.SmartInvoke(Sub()
                                          spriteDebugForm = New SpriteDebugForm()
                                          spriteDebugForm.Show()
                                      End Sub)
            AddHandler spriteDebugForm.FormClosed, Sub() spriteDebugForm = Nothing
        End If

        AddHandler Viewer.MouseDown, AddressOf Viewer_MouseDown

        CreatePonyMenu()
        CreateHouseMenu()
    End Sub

    Private Sub ControlFormItemAdded(sender As Object, e As CollectionItemChangedEventArgs(Of ISprite))
        Dim pony = TryCast(e.Item, Pony)
        If pony IsNot Nothing Then
            ControlFormInvoke(Sub() controlForm.PonyComboBox.Items.Add(pony))
        End If
    End Sub

    Private Sub ControlFormItemsAdded(sender As Object, e As CollectionItemsChangedEventArgs(Of ISprite))
        Dim ponies = e.Items.OfType(Of Pony)().ToArray()
        If ponies.Length > 0 Then
            ControlFormInvoke(Sub() controlForm.PonyComboBox.Items.AddRange(ponies))
        End If
    End Sub

    Private Sub ControlFormItemRemoved(sender As Object, e As CollectionItemChangedEventArgs(Of ISprite))
        Dim pony = TryCast(e.Item, Pony)
        If pony IsNot Nothing Then
            ControlFormInvoke(Sub()
                                  controlForm.PonyComboBox.Items.Remove(pony)
                                  controlForm.NotifyRemovedPonyItems()
                              End Sub)
        End If
    End Sub

    Private Sub ControlFormItemsRemoved(sender As Object, e As CollectionItemsChangedEventArgs(Of ISprite))
        ControlFormInvoke(Sub()
                              For Each item In e.Items
                                  Dim pony = TryCast(item, Pony)
                                  If pony IsNot Nothing Then
                                      controlForm.PonyComboBox.Items.Remove(pony)
                                  End If
                              Next
                              controlForm.NotifyRemovedPonyItems()
                          End Sub)
    End Sub

    Private Sub ControlFormInvoke(method As MethodInvoker)
        SyncLock controlFormLock
            If Not controlForm.Disposing AndAlso Not controlForm.IsDisposed Then
                controlForm.SmartInvoke(method)
            End If
        End SyncLock
    End Sub

    Public Sub EmulateMouseDown(e As SimpleMouseEventArgs)
        Viewer_MouseDown(Viewer, e)
    End Sub

    Private Sub Viewer_MouseDown(sender As Object, e As SimpleMouseEventArgs)
        If e.Buttons.HasFlag(SimpleMouseButtons.Right) Then
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

    Private Sub DisplayHouseMenu(location As Point)
        Dim i = 0
        houseMenu.Items(i).Text = "Edit " & selectedHouse.HouseBase.Name
        i += 1
        houseMenu.Items(i).Text = "Remove " & selectedHouse.HouseBase.Name
        i += 1
        houseMenu.Show(location.X, location.Y)
    End Sub

    Private Sub CreatePonyMenu()
        Dim menuItems = New LinkedList(Of SimpleContextMenuItem)()
        menuItems.AddLast(
            New SimpleContextMenuItem(
                Nothing,
                Sub()
                    Main.Instance.SmartInvoke(
                        Sub()
                            If Game.CurrentGame IsNot Nothing Then
                                MessageBox.Show("Cannot remove ponies during a game.",
                                                "Cannot Remove", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return
                            End If
                            QueueRemove(Function(sprite)
                                            If Object.ReferenceEquals(sprite, selectedPony) Then Return True
                                            Dim effect = TryCast(sprite, Effect)
                                            If effect IsNot Nothing AndAlso
                                                Object.ReferenceEquals(effect.OwningPony, selectedPony) Then
                                                Return True
                                            End If
                                            Return False
                                        End Function)
                        End Sub)
                End Sub))
        menuItems.AddLast(
            New SimpleContextMenuItem(
                Nothing,
                Sub()
                    Main.Instance.SmartInvoke(
                        Sub()
                            If Game.CurrentGame IsNot Nothing Then
                                MessageBox.Show("Cannot remove ponies during a game.",
                                                "Cannot Remove", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return
                            End If
                            QueueRemove(Function(sprite)
                                            Dim pony = TryCast(sprite, Pony)
                                            If pony IsNot Nothing AndAlso pony.Directory = selectedPony.Directory Then
                                                Return True
                                            End If
                                            Dim effect = TryCast(sprite, Effect)
                                            If effect IsNot Nothing AndAlso
                                                effect.OwningPony IsNot Nothing AndAlso
                                                effect.OwningPony.Directory = selectedPony.Directory Then
                                                Return True
                                            End If
                                            Return False
                                        End Function)
                        End Sub)
                End Sub))
        menuItems.AddLast(New SimpleContextMenuItem())
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub()
                                                                 If Game.CurrentGame IsNot Nothing OrElse
                                                                     selectedPony Is Nothing Then Return
                                                                 selectedPony.ShouldBeSleeping = Not selectedPony.ShouldBeSleeping
                                                             End Sub))
        Dim allSleeping = False
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub() Main.Instance.SmartInvoke(
                                                                 Sub()
                                                                     If Game.CurrentGame IsNot Nothing Then Return
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
                                                                FinishViewer()
                                                                Main.Instance.SmartInvoke(AddressOf Main.Instance.Close)
                                                            End Sub))
        If controlForm Is Nothing Then
            ponyMenu = Viewer.CreateContextMenu(menuItems)
        Else
            ponyMenu = controlForm.CreateContextMenu(menuItems)
        End If
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
                                Using houseForm As New HouseOptionsForm(selectedHouse, PonyCollection.Bases)
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

    Public Overrides Sub Start()
        MyBase.Start()
        If controlForm IsNot Nothing Then ControlFormInvoke(AddressOf controlForm.Show)
    End Sub

    Protected Overrides Sub Update()
        MyBase.Update()
        countSinceLastDebug += 1
        If spriteDebugForm IsNot Nothing AndAlso countSinceLastDebug = 5 Then
            Main.Instance.SmartInvoke(Sub() If spriteDebugForm IsNot Nothing Then spriteDebugForm.UpdateSprites(Sprites))
            countSinceLastDebug = 0
        End If
    End Sub

    Public Overrides Sub Finish()
        If controlForm IsNot Nothing Then
            SyncLock controlFormLock
                If Not controlForm.Disposing AndAlso Not controlForm.IsDisposed Then
                    controlForm.BeginInvoke(New MethodInvoker(AddressOf controlForm.ForceClose))
                End If
            End SyncLock
            RemoveHandler SpriteAdded, AddressOf ControlFormItemAdded
            RemoveHandler SpritesAdded, AddressOf ControlFormItemsAdded
            RemoveHandler SpriteRemoved, AddressOf ControlFormItemRemoved
            RemoveHandler SpritesRemoved, AddressOf ControlFormItemsRemoved
            controlForm = Nothing
        End If
        MyBase.Finish()
    End Sub

    Private Function PonySelectionList() As List(Of ISimpleContextMenuItem)

        Dim tagList = New List(Of ISimpleContextMenuItem)

        For Each tag As String In Main.Instance.FilterOptionsBox.Items
            Dim ponyList = New List(Of ISimpleContextMenuItem)
            Dim bases As IEnumerable(Of PonyBase) = PonyCollection.Bases
            If PonyCollection.RandomBase IsNot Nothing Then bases = {PonyCollection.RandomBase}.Concat(PonyCollection.Bases)
            For Each loopPonyBase In bases
                Dim ponyBase = loopPonyBase
                For Each ponyTag In ponyBase.Tags
                    If tag = ponyTag OrElse
                        (ponyBase.Tags.Count = 0 AndAlso tag = "Not Tagged") Then
                        ponyList.Add(New SimpleContextMenuItem(ponyBase.Directory, Sub() AddPonySelection(ponyBase.Directory)))
                    End If
                Next
            Next
            If ponyList.Count > 0 Then
                tagList.Add(New SimpleContextMenuItem(tag, ponyList))
            End If
        Next

        Return tagList
    End Function

    Private Function HouseSelectionList() As List(Of ISimpleContextMenuItem)
        Dim houseBaseList = New List(Of ISimpleContextMenuItem)

        For Each loopHouseBase As HouseBase In Main.Instance.HouseBases
            Dim houseBase = loopHouseBase
            houseBaseList.Add(New SimpleContextMenuItem(houseBase.Name, Sub(sender, e)
                                                                            AddHouseSelection(houseBase)
                                                                        End Sub))
        Next

        Return houseBaseList
    End Function

    Private Sub AddPonySelection(ponyName As String)
        Main.Instance.SmartInvoke(Sub()
                                      If Game.CurrentGame IsNot Nothing Then
                                          MessageBox.Show("Cannot add ponies during a game.",
                                                          "Cannot Add", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                          Return
                                      End If
                                      If ponyName = "Random Pony" Then
                                          Dim selection = Rng.Next(PonyCollection.Bases.Count())
                                          ponyName = PonyCollection.Bases(selection).Directory
                                      End If
                                      For Each ponyBase In PonyCollection.Bases
                                          If ponyBase.Directory = ponyName Then
                                              Dim newPony = New Pony(ponyBase)
                                              AddPony(newPony)
                                          End If
                                      Next
                                  End Sub)
    End Sub

    Private Sub AddHouseSelection(houseBase As HouseBase)
        Main.Instance.SmartInvoke(Sub()
                                      If Game.CurrentGame IsNot Nothing Then
                                          MessageBox.Show("Cannot add houses during a game.",
                                                          "Cannot Add", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                          Return
                                      End If
                                      Dim newHouse = New House(houseBase)
                                      newHouse.InitializeVisitorList()
                                      newHouse.Teleport()
                                      QueueAddAndStart(newHouse)
                                  End Sub)
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        If disposing Then
            If controlForm IsNot Nothing Then ControlFormInvoke(AddressOf controlForm.Dispose)
            If spriteDebugForm IsNot Nothing Then Main.Instance.SmartInvoke(AddressOf spriteDebugForm.Dispose)
        End If
    End Sub
End Class
