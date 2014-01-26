Imports DesktopSprites.SpriteManagement

Public Class DesktopPonyAnimator
    Inherits PonyAnimator

    Private ponyMenu As ISimpleContextMenu
    Private houseMenu As ISimpleContextMenu
    Protected selectedHouse As House
    Protected selectedPony As Pony

    Private spriteDebugForm As SpriteDebugForm
    Private countSinceLastDebug As Integer

    Public Sub New(spriteViewer As ISpriteCollectionView, spriteCollection As IEnumerable(Of ISprite),
                   ponyCollection As PonyCollection)
        MyBase.New(spriteViewer, spriteCollection, ponyCollection)

        If Options.EnablePonyLogs AndAlso Not EvilGlobals.InPreviewMode AndAlso Not OperatingSystemInfo.IsMacOSX Then
            EvilGlobals.Main.SmartInvoke(Sub()
                                             spriteDebugForm = New SpriteDebugForm()
                                             spriteDebugForm.Show()
                                         End Sub)
            AddHandler spriteDebugForm.Disposed, Sub() spriteDebugForm = Nothing
        End If

        AddHandler Viewer.MouseClick, AddressOf Viewer_MouseClick

        CreatePonyMenu()
        CreateHouseMenu()
    End Sub

    Public Sub EmulateMouseClick(e As SimpleMouseEventArgs)
        Viewer_MouseClick(Viewer, e)
    End Sub

    Private Sub Viewer_MouseClick(sender As Object, e As SimpleMouseEventArgs)
        If (e.Buttons And SimpleMouseButtons.Right) = SimpleMouseButtons.Right Then
            selectedPony = GetClosestUnderPoint(Of Pony)(e.Location)
            If selectedPony IsNot Nothing Then
                DisplayPonyMenu(e.Location)
            Else
                selectedHouse = GetClosestUnderPoint(Of House)(e.Location)
                If selectedHouse IsNot Nothing Then DisplayHouseMenu(e.Location)
            End If
        End If
    End Sub

    Private Sub DisplayPonyMenu(location As Point)
        Dim directory = If(selectedPony Is Nothing, "", selectedPony.Directory)
        Dim shouldBeSleeping = If(selectedPony Is Nothing, True, selectedPony.Sleep)
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

        If OperatingSystemInfo.IsWindows Then
            ponyMenu.Items(i).Text = If(Not manualControlP1, "Take Control - Player 1", "Release Control - Player 1")
            i += 1
            ponyMenu.Items(i).Text = If(Not manualControlP2, "Take Control - Player 2", "Release Control - Player 2")
            i += 1
            ' Separator.
            i += 1
        End If

        ponyMenu.Show(location.X, location.Y)
    End Sub

    Private Sub DisplayHouseMenu(location As Point)
        Dim i = 0
        If Not OperatingSystemInfo.IsMacOSX Then
            houseMenu.Items(i).Text = "Edit " & selectedHouse.HouseBase.Name
            i += 1
        End If
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
                    If EvilGlobals.CurrentGame IsNot Nothing Then
                        MessageBox.Show("Cannot remove ponies during a game.",
                                        "Cannot Remove", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                    QueueRemove(Function(sprite) Object.ReferenceEquals(sprite, selectedPony))
                End Sub))
        menuItems.AddLast(
            New SimpleContextMenuItem(
                Nothing,
                Sub()
                    If EvilGlobals.CurrentGame IsNot Nothing Then
                        MessageBox.Show("Cannot remove ponies during a game.",
                                        "Cannot Remove", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                    QueueRemove(Function(sprite)
                                    Dim pony = TryCast(sprite, Pony)
                                    Return pony IsNot Nothing AndAlso pony.Directory = selectedPony.Directory
                                End Function)
                End Sub))
        menuItems.AddLast(New SimpleContextMenuItem())
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub()
                                                                 If EvilGlobals.CurrentGame IsNot Nothing OrElse
                                                                     selectedPony Is Nothing Then Return
                                                                 selectedPony.Sleep = Not selectedPony.Sleep
                                                             End Sub))
        Dim allSleeping = False
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub()
                                                                 If EvilGlobals.CurrentGame IsNot Nothing Then Return
                                                                 allSleeping = Not allSleeping
                                                                 For Each pony In Me.Ponies()
                                                                     pony.Sleep = allSleeping
                                                                 Next
                                                             End Sub))
        menuItems.AddLast(New SimpleContextMenuItem())
        menuItems.AddLast(New SimpleContextMenuItem("Add Pony", PonySelectionList()))
        menuItems.AddLast(New SimpleContextMenuItem("Add House", HouseSelectionList()))
        menuItems.AddLast(New SimpleContextMenuItem())

        If OperatingSystemInfo.IsWindows Then
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
        If Not OperatingSystemInfo.IsMacOSX Then
            menuItems.AddLast(New SimpleContextMenuItem(
                              "Show Options",
                              Sub()
                                  If EvilGlobals.Main Is Nothing Then Return
                                  EvilGlobals.Main.SmartInvoke(
                                      Sub()
                                          Dim form = New OptionsForm()
                                          Dim currentScale = Options.ScaleFactor
                                          form.Show()
                                          AddHandler form.FormClosed, Sub()
                                                                          EvilGlobals.Main.ReloadFilterCategories()
                                                                          If currentScale <> Options.ScaleFactor Then
                                                                              EvilGlobals.Main.ResizePreviewImages()
                                                                          End If
                                                                      End Sub
                                      End Sub)
                              End Sub))
        End If

        menuItems.AddLast(New SimpleContextMenuItem("Return To Menu", Sub() Finish(ExitRequest.ReturnToMenu)))
        menuItems.AddLast(New SimpleContextMenuItem("Exit", Sub() Finish(ExitRequest.ExitApplication)))
        ponyMenu = Viewer.CreateContextMenu(menuItems)
    End Sub

    Private Sub CreateHouseMenu()
        Dim menuItems As LinkedList(Of SimpleContextMenuItem) = New LinkedList(Of SimpleContextMenuItem)()
        If Not OperatingSystemInfo.IsMacOSX Then
            menuItems.AddLast(
                New SimpleContextMenuItem(
                    Nothing,
                    Sub()
                        If EvilGlobals.Main Is Nothing Then Return
                        EvilGlobals.Main.SmartInvoke(
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
        End If
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub() selectedHouse.Expire()))
        houseMenu = Viewer.CreateContextMenu(menuItems)
    End Sub

    Protected Overrides Sub Update()
        MyBase.Update()
        countSinceLastDebug += 1
        If spriteDebugForm IsNot Nothing AndAlso countSinceLastDebug = 5 Then
            spriteDebugForm.SmartInvoke(Sub() spriteDebugForm.UpdateSprites(Sprites))
            countSinceLastDebug = 0
        End If
    End Sub

    Public Overrides Sub Finish()
        If spriteDebugForm IsNot Nothing Then
            spriteDebugForm.SmartInvoke(AddressOf spriteDebugForm.Close)
            spriteDebugForm = Nothing
        End If
        MyBase.Finish()
    End Sub

    Private Function PonySelectionList() As List(Of ISimpleContextMenuItem)
        Dim tagList = New List(Of ISimpleContextMenuItem)()
        tagList.Add(New SimpleContextMenuItem(PonyBase.RandomDirectory, Sub() AddPonySelection(PonyBase.RandomDirectory)))

        For Each tag In PonyBase.StandardTags.Concat(Options.CustomTags)
            Dim ponyList = New List(Of ISimpleContextMenuItem)
            For Each base In PonyCollection.Bases
                If base.Tags.Contains(tag) Then
                    ponyList.Add(New SimpleContextMenuItem(base.Directory, Sub() AddPonySelection(base.Directory)))
                End If
            Next
            If ponyList.Count > 0 Then
                tagList.Add(New SimpleContextMenuItem(tag, ponyList))
            End If
        Next

        Dim untaggedList = New List(Of ISimpleContextMenuItem)
        For Each base In PonyCollection.Bases
            If base.Tags.Count = 0 Then
                untaggedList.Add(New SimpleContextMenuItem(base.Directory, Sub() AddPonySelection(base.Directory)))
            End If
        Next
        If untaggedList.Count > 0 Then
            tagList.Add(New SimpleContextMenuItem("[Not Tagged]", untaggedList))
        End If

        Return tagList
    End Function

    Private Function HouseSelectionList() As List(Of ISimpleContextMenuItem)
        Dim houseBaseList = New List(Of ISimpleContextMenuItem)

        For Each loopHouseBase In PonyCollection.Houses
            Dim houseBase = loopHouseBase
            houseBaseList.Add(New SimpleContextMenuItem(houseBase.Name, Sub() AddHouseSelection(houseBase)))
        Next

        If houseBaseList.Count = 0 Then houseBaseList.Add(New SimpleContextMenuItem())

        Return houseBaseList
    End Function

    Private Sub AddPonySelection(ponyName As String)
        If EvilGlobals.CurrentGame IsNot Nothing Then
            MessageBox.Show("Cannot add ponies during a game.",
                            "Cannot Add", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        If ponyName = PonyBase.RandomDirectory Then
            ponyName = PonyCollection.Bases.RandomElement().Directory
        End If
        For Each ponyBase In PonyCollection.Bases
            If ponyBase.Directory = ponyName Then
                Dim newPony = New Pony(ponyBase)
                AddPony(newPony)
            End If
        Next
    End Sub

    Private Sub AddHouseSelection(houseBase As HouseBase)
        If EvilGlobals.CurrentGame IsNot Nothing Then
            MessageBox.Show("Cannot add houses during a game.",
                            "Cannot Add", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim newHouse = New House(houseBase)
        newHouse.InitializeVisitorList()
        newHouse.Teleport()
        QueueAddAndStart(newHouse)
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        If disposing Then
            If spriteDebugForm IsNot Nothing Then EvilGlobals.Main.SmartInvoke(AddressOf spriteDebugForm.Dispose)
        End If
    End Sub
End Class
