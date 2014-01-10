Imports Gdk
Imports Gtk

Friend Class MainWindow
    Inherits Gtk.Window

    Private ReadOnly invokeSync As New Object()
    Private ponies As PonyCollection
    Private ReadOnly ponyCounts As New Dictionary(Of PonyBase, Integer)()
    Private ReadOnly ponySelectors As New Dictionary(Of PonyBase, Container)()
    Private ReadOnly categoriesComboBox As ComboBox
    Private ReadOnly ponySelectionBox As VBox
    Private ReadOnly progressBar As ProgressBar

    Public Sub New()
        MyBase.New("Desktop Ponies v" & General.GetAssemblyVersion().ToDisplayString())
        Icon = New Pixbuf("Twilight.ico")
        SetSizeRequest(300, 150)
        DefaultSize = New Size(400, 550)
        WindowPosition = Gtk.WindowPosition.Center

        Dim layoutBox = New VBox(False, 4)
        Add(layoutBox)

        categoriesComboBox = ComboBox.NewText()
        categoriesComboBox.AppendText("All")
        For Each tag In PonyBase.StandardTags.Concat(Options.CustomTags)
            categoriesComboBox.AppendText(tag)
        Next
        AddHandler categoriesComboBox.Changed, AddressOf CategoriesComboBox_Changed
        layoutBox.PackStart(categoriesComboBox, False, False, 0)

        Dim scroll = New ScrolledWindow()
        Dim viewport = New Viewport()
        ponySelectionBox = New VBox(False, 2)
        viewport.Add(ponySelectionBox)
        scroll.Add(viewport)
        layoutBox.PackStart(scroll)

        progressBar = New ProgressBar()
        layoutBox.PackStart(progressBar, False, False, 0)

        Dim goButton = New Button("Give Me Ponies!")
        AddHandler goButton.Clicked, AddressOf GoButton_Clicked
        layoutBox.PackStart(goButton, False, False, 0)

        AddHandler Shown, AddressOf MainWindow_Shown
    End Sub

    Private Sub Invoke(action As System.Action)
        SyncLock invokeSync
            Application.Invoke(
                Sub()
                    action()
                    SyncLock invokeSync
                        Threading.Monitor.Pulse(invokeSync)
                    End SyncLock
                End Sub)
        Threading.Monitor.Wait(invokeSync)
        End SyncLock
    End Sub

    Private Sub MainWindow_Shown(sender As Object, e As EventArgs)
        If ponies IsNot Nothing Then Return
        Sensitive = False
        Threading.ThreadPool.QueueUserWorkItem(
            Sub()
                Dim total As Double
                Dim currentTotal As Double
                ponies = New PonyCollection(
                    True,
                    Sub(count) total = count,
                    Sub(base)
                        currentTotal += 1
                        Invoke(
                            Sub()
                                AddPonySelector(base)
                                progressBar.Fraction = currentTotal / total
                            End Sub)
                    End Sub)
                Invoke(
                    Sub()
                        For Each base In ponies.Bases
                            ponySelectionBox.Add(ponySelectors(base))
                        Next
                        categoriesComboBox.Active = 0
                        progressBar.Hide()
                        Sensitive = True
                    End Sub)
            End Sub)
    End Sub

    Private Sub AddPonySelector(base As PonyBase)
        Dim layoutBox = New HBox()

        Dim infoAlign = New Alignment(0, 0.5, 0, 0)
        Dim infoBox = New VBox()
        infoAlign.Add(infoBox)
        Dim label = New Label(base.Directory)
        label.SetAlignment(0.0, 0.5)
        infoBox.PackStart(label, False, False, 1)
        Dim spin = New SpinButton(0, 100, 1)
        AddHandler spin.ValueChanged, Sub() ponyCounts(base) = spin.ValueAsInt
        infoBox.PackStart(spin, False, False, 0)
        layoutBox.PackStart(infoAlign, False, False, 2)

        Dim image = New Gtk.Image(base.Behaviors(0).LeftImage.Path)
        layoutBox.PackEnd(image, False, False, 0)

        layoutBox.ShowAll()
        ponyCounts.Add(base, 0)
        ponySelectors.Add(base, layoutBox)
    End Sub

    Private Sub CategoriesComboBox_Changed(sender As Object, e As EventArgs)
        For Each base In ponies.Bases
            Dim selector = ponySelectors(base)
            If categoriesComboBox.Active = 0 OrElse base.Tags.Contains(categoriesComboBox.ActiveText) Then
                selector.Show()
            Else
                selector.Hide()
            End If
        Next
    End Sub

    Private Sub GoButton_Clicked(sender As Object, e As EventArgs)
        Dim startupPonies As New List(Of Pony)()
        For Each kvp In ponyCounts
            For i = 1 To kvp.Value
                startupPonies.Add(New Pony(kvp.Key))
            Next
        Next

        If startupPonies.Count = 0 Then
            Dim dialog = New MessageDialog(Me, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok,
                                           "You need to select some ponies first!")
            AddHandler dialog.Response, Sub() dialog.Destroy()
            dialog.Run()
            Return
        End If

        Sensitive = False
        progressBar.Fraction = 0
        progressBar.Show()
        Dim viewer = New DesktopSprites.SpriteManagement.GtkSpriteInterface(False)
        Threading.ThreadPool.QueueUserWorkItem(
            Sub()
                viewer.Topmost = Options.AlwaysOnTop
                viewer.ShowInTaskbar = Options.ShowInTaskbar

                Dim images As New HashSet(Of String)(PathEquality.Comparer)
                For Each pony In startupPonies
                    For Each behavior In pony.Behaviors
                        images.Add(behavior.LeftImage.Path)
                        images.Add(behavior.RightImage.Path)
                        For Each effect In behavior.Effects
                            images.Add(effect.LeftImage.Path)
                            images.Add(effect.RightImage.Path)
                        Next
                    Next
                Next
                Dim imagesLoadedCount = 0
                viewer.LoadImages(images, Sub()
                                              imagesLoadedCount += 1
                                              Invoke(Sub() progressBar.Fraction = imagesLoadedCount / images.Count)
                                          End Sub)

                Dim animator = New DesktopPonyAnimator(viewer, startupPonies, ponies)
                AddHandler animator.AnimationFinished, Sub() Threading.ThreadPool.QueueUserWorkItem(
                                                           Sub() Invoke(
                                                               Sub()
                                                                   EvilGlobals.CurrentViewer = Nothing
                                                                   EvilGlobals.CurrentAnimator = Nothing
                                                                   If animator.ExitRequested = ExitRequest.ExitApplication Then
                                                                       Gtk.Application.Quit()
                                                                   Else
                                                                       Show()
                                                                       ponySelectionBox.Show()
                                                                       General.FullCollect()
                                                                   End If
                                                               End Sub))

                EvilGlobals.CurrentViewer = viewer
                EvilGlobals.CurrentAnimator = animator

                Invoke(
                    Sub()
                        progressBar.Hide()
                        ponySelectionBox.Hide()
                        Sensitive = True
                        Hide()
                        animator.Start()
                    End Sub)
            End Sub)
    End Sub

End Class
