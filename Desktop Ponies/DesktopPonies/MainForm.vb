Imports System.Globalization
Imports System.IO
Imports DesktopSprites.SpriteManagement

''' <summary>
''' This is the form that handles startup and pony selection.
''' </summary>
Public Class MainForm
#Region "Fields and Properties"
    Private Const Autostart = "autostart"
    Private initialized As Boolean
    Private loading As Boolean
    Private ReadOnly loadWatch As New Diagnostics.Stopwatch()
    Private worker As IdleWorker

    Private oldWindowState As FormWindowState
    Private layoutPendingFromRestore As Boolean

    Private autoStarted As Boolean

    Private animator As DesktopPonyAnimator
    Private ponyViewer As ISpriteCollectionView
    Private ponies As PonyCollection
    Private screensaverForms As List(Of ScreensaverBackgroundForm)

    Private notTaggedFilterIndex As Integer = -1
    Private ReadOnly selectionControlFilter As New Dictionary(Of PonySelectionControl, Boolean)()
    Private ponyOffset As Integer
    Private ReadOnly selectionControlsFilteredVisible As IEnumerable(Of PonySelectionControl)
#End Region

#Region "Initialization"
    Public Sub New()
        InitializeComponent()
        selectionControlsFilteredVisible =
            PonySelectionPanel.Controls.Cast(Of PonySelectionControl).Where(Function(control) selectionControlFilter(control))
        Icon = My.Resources.Twilight
        Text = "Desktop Ponies v" & General.GetAssemblyVersion().ToDisplayString()
        initialized = True
    End Sub

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        worker = New IdleWorker(Me)
        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
        Threading.ThreadPool.QueueUserWorkItem(Sub() CheckForNewVersion())
    End Sub

    ''' <summary>
    ''' Read all configuration files and pony folders.
    ''' </summary>
    Private Sub LoadInternal()
        loadWatch.Restart()
        UseWaitCursor = True
        loading = True

        PonyPaginationPanel.Enabled = False
        PonySelectionPanel.Enabled = False
        SelectionControlsPanel.Enabled = False

        Update()

        If Not Directory.Exists(PonyBase.RootDirectory) Then
            Const message =
                "The " & PonyBase.RootDirectory & " directory could not be found. We can't start without that! " &
                "If you just downloaded the full program then please make sure it has extracted correctly. " &
                "On Mac/Unix it is important to preserve the directory structure when extracting. " &
                "If you have downloaded a patch - you need to copy these files over an existing install and overwrite if prompted."
            MessageBox.Show(Me, message, "Directory Missing", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Return
        End If

        Dim profile = Options.DefaultProfileName
        ProcessCommandLine()
        If Globals.IsScreensaverExecutable() Then
            profile = Options.ScreensaverProfileName
            If Globals.InScreensaverMode Then
                ' We are starting in screensaver mode. Hide the program during loading.
                ActivateAutostart()
            End If
        Else
            ' Load the profile that was last in use by this user.
            Dim profileFile As StreamReader = Nothing
            Try
                profileFile = New StreamReader(Path.Combine(Options.ProfileDirectory, "current.txt"), System.Text.Encoding.UTF8)
                profile = profileFile.ReadLine()
            Catch ex As IOException
                ' We don't mind if no preferred profile is saved.
            Finally
                If profileFile IsNot Nothing Then profileFile.Close()
            End Try
        End If
        ' Get the profile listing.
        GetProfiles(profile)
        ' Force the screensaver profile to be loaded when running as the screensaver, even if it didn't exist.
        If Globals.IsScreensaverExecutable() Then
            Options.LoadProfile(profile, False)
            If ProfileComboBox.Items.IndexOf(profile) = -1 Then
                ProfileComboBox.SelectedIndex = ProfileComboBox.Items.Add(profile)
            End If
        End If

        Threading.ThreadPool.QueueUserWorkItem(Sub() LoadTemplates())
    End Sub

    Private Sub ProcessCommandLine()
        Try
            Dim args = Environment.GetCommandLineArgs()

            ' On some versions of Windows, starting a screensaver with no arguments indicates the screensaver should be configured.
            If Globals.IsScreensaverExecutable() AndAlso args.Length = 1 Then
                ConfigureScreensaver()
                Return
            End If

            ' Process command line arguments.
            If args.Length >= 2 Then
                Select Case Split(args(1).Trim(), ":")(0).ToLowerInvariant()
                    Case Autostart
                        ' Immediately start the ponies, using the autostart profile if available.
                        ActivateAutostart()

                        Try
                            Options.LoadProfile(Autostart, False)
                        Catch
                            Options.LoadDefaultProfile()
                        End Try
                        LoadPonyCounts()
                    Case "/s"
                        ' Screensaver option for starting the screensaver.
                        Globals.InScreensaverMode = True
                    Case "/c"
                        ' Screensaver option for configuring the screensaver.
                        ConfigureScreensaver()
                    Case "/p"
                        ' Screensaver option for previewing the screensaver. There is no preview mode.
                        Environment.Exit(0)
                    Case Else
                        Dim executable = """" & Path.GetFileName(Reflection.Assembly.GetEntryAssembly().Location) & """"
                        MessageBox.Show(
                            Me,
                            "Invalid command line arguments. They will be ignored. Usage: " & vbNewLine & vbNewLine &
                            executable & vbNewLine &
                            "Starts Desktop Ponies normally." & vbNewLine & vbNewLine &
                            executable & " " & Autostart & vbNewLine &
                            "Start and show ponies straight away, using the '" & Autostart & "' profile." & vbNewLine & vbNewLine &
                            executable & " /s" & vbNewLine &
                            "Start in screensaver mode. This uses the '" & Options.ScreensaverProfileName & "' profile." & vbNewLine & vbNewLine &
                            executable & " /c" & vbNewLine &
                            "Configure screensaver mode." & vbNewLine & vbNewLine &
                            executable & " /p" & vbNewLine &
                            "Preview screensaver mode. This does nothing.",
                            "Invalid Arguments", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Select
            End If
        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error processing command line arguments. They will be ignored.")
        End Try
    End Sub

    Private Sub ActivateAutostart()
        autoStarted = True
        ShowInTaskbar = False
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub DeactivateAutostart()
        autoStarted = False
        ShowInTaskbar = True
        WindowState = FormWindowState.Normal
    End Sub

    Private Sub ConfigureScreensaver()
        MessageBox.Show(
            Me,
            "The 'screensaver' profile will been loaded. Make changes to this profile to configure the screensaver." & vbNewLine &
            vbNewLine &
            " - Set the number of ponies you want to appear by entering a value for each pony." & vbNewLine &
            " - Open the options menu and change any settings you wish to be used when the screensaver is active." & vbNewLine &
            vbNewLine &
            "Then save the profile using the save button on the main menu or the options menu. You can then close the program. " &
            "When the screensaver starts it will use these settings.",
            "Screensaver Help", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub LoadTemplates()
        ' Don't spend time laying out whilst adding new controls.
        worker.QueueTask(Sub() PonySelectionPanel.SuspendLayout())

        ' Load ponies.
        ponies = New PonyCollection(
            True,
            Sub(count) worker.QueueTask(Sub() LoadingProgressBar.Maximum += count),
            Sub(pony)
                worker.QueueTask(Sub()
                                     AddToMenu(pony)
                                     LoadingProgressBar.Value += 1
                                 End Sub)
            End Sub,
            Sub(count) worker.QueueTask(Sub() LoadingProgressBar.Maximum += count),
            Sub(house) worker.QueueTask(Sub() LoadingProgressBar.Value += 1))

        ' Sort controls by name.
        worker.QueueTask(Sub()
                             Dim selectionControls = PonySelectionPanel.Controls.Cast(Of PonySelectionControl)().ToArray()
                             Array.Sort(selectionControls,
                                        Function(a, b) StringComparer.OrdinalIgnoreCase.Compare(
                                            a.PonyBase.Directory, b.PonyBase.Directory))
                             ' Move random pony to the top of the sort.
                             Dim randomBaseIndex = -1
                             For i = 0 To selectionControls.Length - 1
                                 If ReferenceEquals(selectionControls(i).PonyBase, ponies.RandomBase) Then
                                     randomBaseIndex = i
                                     Exit For
                                 End If
                             Next
                             If randomBaseIndex <> -1 Then
                                 Dim randomBaseControl = selectionControls(randomBaseIndex)
                                 For i = randomBaseIndex To 1 Step -1
                                     selectionControls(i) = selectionControls(i - 1)
                                 Next
                                 selectionControls(0) = randomBaseControl
                             End If
                             For i = 0 To selectionControls.Length - 1
                                 Dim selectionControl = selectionControls(i)
                                 selectionControl.TabIndex = i
                                 PonySelectionPanel.Controls.SetChildIndex(selectionControl, i)
                             Next
                             ' Now controls are added and sorted, resume layouts.
                             PonySelectionPanel.ResumeLayout()
                         End Sub)

        ' Wait for ponies and houses to load.
        worker.WaitOnAllTasks()
        If Not ponies.Bases.Any() Then
            If Not TryInvoke(Sub()
                                 MessageBox.Show(Me, "Sorry, but you don't seem to have any usable ponies installed. " &
                                                 "If you have downloaded a patch version, you need to copy the new files over an " &
                                                 "existing installation of Desktop Ponies to update it.",
                                                 "No Ponies Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                 GoButton.Enabled = False
                             End Sub) Then Return
        End If

        ' Load pony counts.
        worker.QueueTask(AddressOf LoadPonyCounts)

        ' Show images in unison (although images still loading will appear as they become available).
        worker.QueueTask(Sub()
                             For Each control As PonySelectionControl In PonySelectionPanel.Controls
                                 control.ShowPonyImage = True
                             Next
                         End Sub)

        ' Finish loading.
        worker.QueueTask(Sub()
                             Console.WriteLine("Templates Loaded in {0:0.00s}", loadWatch.Elapsed.TotalSeconds)

                             PonyPaginationLabel.Text = "Viewing {0} ponies".FormatWith(PonySelectionPanel.Controls.Count)

                             If Not Runtime.IsMono Then LoadingProgressBar.Visible = False
                             LoadingProgressBar.Value = 0
                             LoadingProgressBar.Maximum = 1

                             CountSelectedPonies()

                             PoniesPerPage.Maximum = PonySelectionPanel.Controls.Count
                             PaginationEnabled.Enabled = True
                             PaginationEnabled.Checked = Not OperatingSystemInfo.IsWindows

                             PonySelectionPanel.Enabled = True
                             SelectionControlsPanel.Enabled = True
                             AnimationTimer.Enabled = True

                             If autoStarted Then LoadPonies()

                             General.FullCollect()
                             loading = False
                             UseWaitCursor = False

                             loadWatch.Stop()
                             Console.WriteLine("Loaded in {0:0.00s} ({1} templates)",
                                               loadWatch.Elapsed.TotalSeconds, PonySelectionPanel.Controls.Count)
                         End Sub)
    End Sub

    Private Sub AddToMenu(ponyBase As PonyBase)
        Dim ponySelection As New PonySelectionControl(ponyBase, ponyBase.Behaviors(0).RightImage.Path) With {
            .Location = New Point(-Width, -Height)}
        AddHandler ponySelection.PonyCount.TextChanged, Sub() HandleCountChange(ponySelection.PonyBase, ponySelection.Count)
        If ponyBase.Directory = PonyBase.RandomDirectory Then
            ponySelection.NoDuplicates.Visible = True
            ponySelection.NoDuplicates.Checked = Options.NoRandomDuplicates
            AddHandler ponySelection.NoDuplicates.CheckedChanged, Sub() Options.NoRandomDuplicates = ponySelection.NoDuplicates.Checked
        End If

        selectionControlFilter.Add(ponySelection, True)
        PonySelectionPanel.Controls.Add(ponySelection)

        ' Since we may be adding several hundred controls, delaying layouts until all are adding may freeze the UI for a period if it has
        ' to immediately lay out all those controls in bulk. If we allow it to process the new controls in small chunks we can break up the
        ' large delay into less disruptive chunks.
        If PonySelectionPanel.Controls.Count Mod 32 = 0 Then
            PonySelectionPanel.ResumeLayout(True)
            PonySelectionPanel.SuspendLayout()
        End If
    End Sub

    Private Sub HandleCountChange(base As PonyBase, newCount As Integer)
        Dim newCounts = New Dictionary(Of String, Integer)(Options.PonyCounts)
        If newCount = 0 Then
            newCounts.Remove(base.Directory)
        Else
            newCounts(base.Directory) = newCount
        End If
        Options.PonyCounts = newCounts.AsReadOnly()
        CountSelectedPonies()
    End Sub

    Public Sub LoadPonyCounts()
        For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
            If Options.PonyCounts.ContainsKey(ponyPanel.PonyBase.Directory) Then
                ponyPanel.Count = Options.PonyCounts(ponyPanel.PonyBase.Directory)
            Else
                ponyPanel.Count = 0
            End If
        Next
    End Sub

    Private Sub CountSelectedPonies()
        Dim totalPonies = 0
        For Each selectionControl As PonySelectionControl In PonySelectionPanel.Controls
            Dim subTotal As Integer
            If Options.PonyCounts.TryGetValue(selectionControl.PonyBase.Directory, subTotal) Then
                totalPonies += subTotal
            End If
        Next
        PonyCountValueLabel.Text = totalPonies.ToString(CultureInfo.CurrentCulture)
    End Sub

    Private Sub CheckForNewVersion()
        Dim info = CommunityDialog.CommunityInfo.Retrieve()
        If info IsNot Nothing Then
            worker.QueueTask(Sub()
                                 CommunityLink.Visible = True
                                 AddHandler CommunityLink.LinkClicked,
                                     Sub()
                                         Using dialog = New CommunityDialog(info)
                                             dialog.ShowDialog(Me)
                                         End Using
                                     End Sub
                                 If info.NewerVersionAvailable Then CommunityLink.Text &= " [New Version Available!]"
                             End Sub)
        End If
    End Sub
#End Region

#Region "Selection"
    Private Sub ZeroPoniesButton_Click(sender As Object, e As EventArgs) Handles ZeroPoniesButton.Click
        For Each ponyPanel In selectionControlsFilteredVisible
            If ReferenceEquals(ponyPanel.PonyBase, ponies.RandomBase) Then Continue For
            ponyPanel.Count = 0
        Next
    End Sub

    Private Sub OnePoniesButton_Click(sender As Object, e As EventArgs) Handles OnePoniesButton.Click
        For Each ponyPanel In selectionControlsFilteredVisible
            If ReferenceEquals(ponyPanel.PonyBase, ponies.RandomBase) Then Continue For
            ponyPanel.Count = 1
        Next
    End Sub

    Private Sub SaveProfileButton_Click(sender As Object, e As EventArgs) Handles SaveProfileButton.Click
        Dim profileToSave = ProfileComboBox.Text

        If profileToSave = "" Then
            MessageBox.Show(Me, "Enter a profile name first!", "No Profile Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If profileToSave = Options.DefaultProfileName Then
            MessageBox.Show(
                Me, "Cannot save over the '" & Options.DefaultProfileName & "' profile. " &
                "To create a new profile, type a new name for the profile into the box. You will then be able to save the profile.",
                "Invalid Profile Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If profileToSave.IndexOfAny(Path.GetInvalidFileNameChars()) <> -1 OrElse
            profileToSave.IndexOfAny(Path.GetInvalidPathChars()) <> -1 Then
            MessageBox.Show(Me, "Cannot save the profile. Remove any special characters from the profile name first.",
                "Invalid Profile Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Options.SaveProfile(profileToSave)

        If Not ProfileComboBox.Items.Contains(profileToSave) Then
            ProfileComboBox.Items.Add(profileToSave)
        End If
        ProfileComboBox.SelectedItem = profileToSave

        MessageBox.Show(Me, "Profile '" & profileToSave & "' saved.", "Profile Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub LoadProfileButton_Click(sender As Object, e As EventArgs) Handles LoadProfileButton.Click
        LoadProfile()
    End Sub

    Private Sub OptionsButton_Click(sender As Object, e As EventArgs) Handles OptionsButton.Click
        Using form = New OptionsForm()
            Dim currentScale = Options.ScaleFactor
            form.ShowDialog(Me)
            LoadPonyCounts()
            ReloadFilterCategories()
            If currentScale <> Options.ScaleFactor Then
                ResizePreviewImages()
            End If
        End Using
    End Sub

    Private Sub PonyEditorButton_Click(sender As Object, e As EventArgs) Handles PonyEditorButton.Click
        Visible = False
        Using form = New PonyEditor()
            form.ShowDialog(Me)

            PonyShutdown()

            If Not IsDisposed Then Visible = True

            If form.ChangesMade Then
                ResetPonySelection()
                FilterAllRadio.Checked = True
                LoadingProgressBar.Visible = True
                '(We need to reload everything to account for anything changed while in the editor)
                LoadInternal()
            End If
        End Using

    End Sub

    Private Sub GamesButton_Click(sender As Object, e As EventArgs) Handles GamesButton.Click
        Try
            Visible = False
            Using gameForm As New GameSelectionForm(ponies)
                If gameForm.ShowDialog(Me) = DialogResult.OK Then
                    PonyStartup(Function() New Game.GameAnimator(ponyViewer, {}, ponies, gameForm.PonyContext, gameForm.Game, Me), {})
                    gameForm.Game.Setup()
                    animator.Start()
                Else
                    If Not IsDisposed Then Visible = True
                End If
            End Using
        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error loading games.")
#If DEBUG Then
            Throw
#End If
        End Try
    End Sub

    Private Sub GetProfiles(profileToAttemptToLoad As String)
        ProfileComboBox.Items.Clear()
        ProfileComboBox.Items.Add(Options.DefaultProfileName)
        Dim profiles = Options.GetKnownProfiles()
        If profiles IsNot Nothing Then ProfileComboBox.Items.AddRange(profiles)
        Dim profileIndex = ProfileComboBox.Items.IndexOf(profileToAttemptToLoad)
        If profileIndex <> -1 Then
            ProfileComboBox.SelectedIndex = profileIndex
        Else
            ProfileComboBox.SelectedIndex = 0 ' Default profile.
        End If
    End Sub

    Private Sub CopyProfileButton_Click(sender As Object, e As EventArgs) Handles CopyProfileButton.Click
        Dim copiedProfileName = InputBox("Enter name of new profile to copy to:")
        copiedProfileName = Trim(copiedProfileName)
        If copiedProfileName = "" Then
            MessageBox.Show(Me, "Can't copy to a profile with a blank name! Please choose another name.", "Invalid Profile Name",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If copiedProfileName = Options.DefaultProfileName Then
            MessageBox.Show(Me, "Cannot copy over the '" & Options.DefaultProfileName & "' profile. Please choose another name.",
                            "Invalid Profile Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If copiedProfileName.IndexOfAny(Path.GetInvalidFileNameChars()) <> -1 OrElse
            copiedProfileName.IndexOfAny(Path.GetInvalidPathChars()) <> -1 Then
            MessageBox.Show(Me, "Cannot copy as the new profile name is invalid. Please choose another name.",
                            "Invalid Profile Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Options.SaveProfile(copiedProfileName)
        GetProfiles(copiedProfileName)
    End Sub

    Private Sub DeleteProfileButton_Click(sender As Object, e As EventArgs) Handles DeleteProfileButton.Click
        If ProfileComboBox.Text = Options.DefaultProfileName Then
            MessageBox.Show(Me, "Cannot delete the '" & Options.DefaultProfileName & "' profile.",
                            "Invalid Profile", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If ProfileComboBox.Text.IndexOfAny(Path.GetInvalidFileNameChars()) <> -1 OrElse
            ProfileComboBox.Text.IndexOfAny(Path.GetInvalidPathChars()) <> -1 Then
            MessageBox.Show(Me, "Cannot delete the profile as the name is invalid.",
                            "Invalid Profile Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If Options.DeleteProfile(ProfileComboBox.Text) Then
            MessageBox.Show(Me, "Profile deleted successfully", "Profile Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show(Me, "Error attempting to delete this profile. Perhaps it has already been deleted.",
                            "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        GetProfiles(Options.DefaultProfileName)
    End Sub

    Private Sub ProfileComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ProfileComboBox.SelectedIndexChanged
        LoadProfile()
    End Sub

    Private Sub LoadProfile()
        Options.LoadProfile(ProfileComboBox.Text, Not Globals.IsScreensaverExecutable())
        LoadPonyCounts()
        ReloadFilterCategories()
    End Sub

    Private Sub FilterAnyRadio_CheckedChanged(sender As Object, e As EventArgs) Handles FilterAnyRadio.CheckedChanged
        If FilterAnyRadio.Checked Then
            FilterOptionsBox.Enabled = True
            RefilterSelection()
        End If
    End Sub

    Private Sub FilterExactlyRadio_CheckedChanged(sender As Object, e As EventArgs) Handles FilterExactlyRadio.CheckedChanged
        If FilterExactlyRadio.Checked Then
            FilterOptionsBox.Enabled = True
            RefilterSelection()
        End If
    End Sub

    Private Sub FilterExceptRadio_CheckedChanged(sender As Object, e As EventArgs) Handles FilterExceptRadio.CheckedChanged
        If FilterExceptRadio.Checked Then
            FilterOptionsBox.Enabled = True
            RefilterSelection()
        End If
    End Sub

    Private Sub FilterAllRadio_CheckedChanged(sender As Object, e As EventArgs) Handles FilterAllRadio.CheckedChanged
        If FilterAllRadio.Checked AndAlso Visible Then
            FilterOptionsBox.Enabled = False
            RefilterSelection()
        End If
    End Sub

    Private Sub RefilterSelection(Optional tags As HashSet(Of CaseInsensitiveString) = Nothing,
                                  Optional notTaggedChecked As Boolean? = Nothing)
        If tags Is Nothing Then tags =
            New HashSet(Of CaseInsensitiveString)(FilterOptionsBox.CheckedItems.OfType(Of CaseInsensitiveString)())
        Dim notTaggedFlag As Boolean
        If notTaggedChecked Is Nothing Then
            notTaggedFlag = FilterOptionsBox.GetItemChecked(notTaggedFilterIndex)
        Else
            notTaggedFlag = notTaggedChecked.Value
        End If

        For Each selectionControl As PonySelectionControl In PonySelectionPanel.Controls
            ' Show all ponies.
            If FilterAllRadio.Checked Then
                selectionControlFilter(selectionControl) = True
            End If

            ' Show ponies with at least one matching tag.
            If FilterAnyRadio.Checked Then
                Dim visible = selectionControl.PonyBase.Tags.Any(Function(tag) tags.Contains(tag)) OrElse
                (selectionControl.PonyBase.Tags.Count = 0 AndAlso notTaggedFlag)
                selectionControlFilter(selectionControl) = visible
            End If

            ' Show ponies which match all tags.
            If FilterExactlyRadio.Checked Then
                Dim visible = If(notTaggedFlag,
                                 selectionControl.PonyBase.Tags.Count = 0 AndAlso tags.Count = 0,
                                 selectionControl.PonyBase.Tags.IsSupersetOf(tags))
                selectionControlFilter(selectionControl) = visible
            End If

            ' Show ponies with no matching tags.
            If FilterExceptRadio.Checked Then
                Dim visible = Not (selectionControl.PonyBase.Tags.Any(Function(tag) tags.Contains(tag)) OrElse
                (selectionControl.PonyBase.Tags.Count = 0 AndAlso notTaggedFlag))
                selectionControlFilter(selectionControl) = visible
            End If
        Next

        ponyOffset = 0
        RepaginateSelection()
    End Sub

    Private Sub RepaginateSelection()
        PonySelectionPanel.SuspendLayout()

        Dim localOffset = 0
        Dim visibleCount = 0
        Dim filterCount = 0
        For Each selectionControl As PonySelectionControl In PonySelectionPanel.Controls
            Dim makeVisible = False
            Dim filteredVisible = selectionControlFilter(selectionControl)
            If Not PaginationEnabled.Checked Then
                ' If pagination is disabled, simply show/hide the control according to the current filter.
                makeVisible = filteredVisible
            ElseIf filteredVisible Then
                ' If pagination is enabled, we will show it if it is filtered visible and within the page range.
                makeVisible = localOffset >= ponyOffset AndAlso visibleCount < PoniesPerPage.Value
                localOffset += 1
            End If
            If filteredVisible Then filterCount += 1
            If makeVisible Then visibleCount += 1
            selectionControl.Visible = makeVisible
        Next

        PonySelectionPanel.ResumeLayout()

        If Not PaginationEnabled.Checked OrElse visibleCount = 0 Then
            PonyPaginationLabel.Text = "Viewing {0} ponies".FormatWith(filterCount)
        Else
            PonyPaginationLabel.Text =
                "Viewing {0} to {1} of {2} ponies".FormatWith(
                    ponyOffset + 1,
                    Math.Min(ponyOffset + PoniesPerPage.Value, filterCount),
                    filterCount)
        End If

        Dim min = ponyOffset = 0
        Dim max = ponyOffset >= filterCount - PoniesPerPage.Value
        FirstPageButton.Enabled = Not min
        PreviousPageButton.Enabled = Not min
        PreviousPonyButton.Enabled = Not min
        NextPonyButton.Enabled = Not max
        NextPageButton.Enabled = Not max
        LastPageButton.Enabled = Not max
    End Sub

    Private Sub Main_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        If ProfileComboBox.Focused Then Exit Sub

        If Char.IsLetter(e.KeyChar) Then
            e.Handled = True
            For Each selectionControl In selectionControlsFilteredVisible
                If ReferenceEquals(selectionControl.PonyBase, ponies.RandomBase) Then Continue For
                If selectionControl.PonyName.Text.Length > 0 Then
                    Dim compare = String.Compare(selectionControl.PonyName.Text(0), e.KeyChar, StringComparison.OrdinalIgnoreCase)
                    If compare = 0 Then
                        PonySelectionPanel.ScrollControlIntoView(selectionControl)
                        selectionControl.PonyCount.Focus()
                        Exit For
                    End If
                End If
            Next
        ElseIf e.KeyChar = "#"c Then
            Using newEditor = New PonyEditorForm2()
                newEditor.ShowDialog(Me)
                If newEditor.ChangesMade Then
                    ResetPonySelection()
                    FilterAllRadio.Checked = True
                    LoadingProgressBar.Visible = True
                    LoadInternal()
                End If
            End Using
        End If
    End Sub

    Private Sub FirstPageButton_Click(sender As Object, e As EventArgs) Handles FirstPageButton.Click
        ponyOffset = 0
        RepaginateSelection()
    End Sub

    Private Sub PreviousPageButton_Click(sender As Object, e As EventArgs) Handles PreviousPageButton.Click
        ponyOffset -= Math.Min(ponyOffset, CInt(PoniesPerPage.Value))
        RepaginateSelection()
    End Sub

    Private Sub PreviousPonyButton_Click(sender As Object, e As EventArgs) Handles PreviousPonyButton.Click
        ponyOffset -= Math.Min(ponyOffset, 1)
        RepaginateSelection()
    End Sub

    Private Sub NextPonyButton_Click(sender As Object, e As EventArgs) Handles NextPonyButton.Click
        ponyOffset += Math.Min(selectionControlsFilteredVisible.Count() - CInt(PoniesPerPage.Value) - ponyOffset, 1)
        RepaginateSelection()
    End Sub

    Private Sub NextPageButton_Click(sender As Object, e As EventArgs) Handles NextPageButton.Click
        ponyOffset += Math.Min(selectionControlsFilteredVisible.Count() - CInt(PoniesPerPage.Value) - ponyOffset, CInt(PoniesPerPage.Value))
        RepaginateSelection()
    End Sub

    Private Sub LastPageButton_Click(sender As Object, e As EventArgs) Handles LastPageButton.Click
        ponyOffset = selectionControlsFilteredVisible.Count() - CInt(PoniesPerPage.Value)
        RepaginateSelection()
    End Sub

    Private Sub PoniesPerPage_ValueChanged(sender As Object, e As EventArgs) Handles PoniesPerPage.ValueChanged
        If initialized Then RepaginateSelection()
    End Sub

    Private Sub PaginationEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles PaginationEnabled.CheckedChanged
        PonyPaginationPanel.Enabled = PaginationEnabled.Checked
        RepaginateSelection()
    End Sub

    Private Sub FilterOptionsBox_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles FilterOptionsBox.ItemCheck
        Dim tags = New HashSet(Of CaseInsensitiveString)(FilterOptionsBox.CheckedItems.OfType(Of CaseInsensitiveString)())
        Dim notTaggedChecked As Boolean?
        If e.CurrentValue <> e.NewValue Then
            If e.Index <> notTaggedFilterIndex Then
                Dim changedTag = DirectCast(FilterOptionsBox.Items(e.Index), CaseInsensitiveString)
                If e.NewValue = CheckState.Checked Then
                    tags.Add(changedTag)
                Else
                    tags.Remove(changedTag)
                End If
            Else
                notTaggedChecked = e.NewValue = CheckState.Checked
            End If
        End If
        RefilterSelection(tags, notTaggedChecked)
    End Sub
#End Region

#Region "Pony Startup"
    Private Sub GoButton_Click(sender As Object, e As EventArgs) Handles GoButton.Click
        LoadPonies()
    End Sub

    Private Sub LoadPonies()
        loading = True
        SelectionControlsPanel.Enabled = False
        LoadingProgressBar.Visible = True
        loadWatch.Restart()
        Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadPoniesAsync)
    End Sub

    Private Sub LoadPoniesAsync(o As Object)
        Try
            ' Note down the number of each pony that is wanted.
            Dim totalPonies As Integer
            Dim ponyBasesWanted As New List(Of Tuple(Of String, Integer))()
            For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
                Dim ponyName = ponyPanel.PonyName.Text
                Dim count As Integer
                If Integer.TryParse(ponyPanel.PonyCount.Text, count) AndAlso count > 0 Then
                    ponyBasesWanted.Add(Tuple.Create(ponyName, count))
                    totalPonies += count
                End If
            Next

            If totalPonies = 0 Then
                If Globals.InScreensaverMode Then
                    ponyBasesWanted.Add(Tuple.Create(PonyBase.RandomDirectory, 1))
                    totalPonies = 1
                Else
                    LoadPoniesAsyncEnd(
                        True,
                        Sub()
                            If autoStarted Then
                                DeactivateAutostart()
                                MessageBox.Show(Me, ("You haven't created a profile with ponies to use.{0}" &
                                                "Create a profile named 'autostart', choose some ponies and save the profile.{0}" &
                                                "In future, these ponies will be loaded automatically when using autostart.").
                                                FormatWith(Environment.NewLine),
                                                "No Ponies Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MessageBox.Show(Me, "You haven't selected any ponies! Choose some ponies to roam your desktop first.",
                                                "No Ponies Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        End Sub)
                    Return
                End If
            End If

            If totalPonies > Options.MaxPonyCount Then
                LoadPoniesAsyncEnd(
                    True,
                    Sub()
                        MessageBox.Show(
                            Me, ("Sorry you selected {1} ponies, which is more than the limit specified in the options menu.{0}" &
                            "Try choosing no more than {2} in total.{0}" &
                            "(or, you can increase the limit via the options menu)").FormatWith(
                                Environment.NewLine, totalPonies, Options.MaxPonyCount),
                            "Too Many Ponies", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Sub)
                Return
            End If

            ' Create the initial set of ponies to start.
            Dim startupPonies As New List(Of Pony)()
            Dim context = New PonyContext()
            Dim randomPoniesWanted As Integer
            For Each ponyBaseWanted In ponyBasesWanted
                If ponyBaseWanted.Item1 = PonyBase.RandomDirectory Then
                    randomPoniesWanted = ponyBaseWanted.Item2
                    Continue For
                End If
                Dim base = ponies.Bases.Single(Function(ponyBase) ponyBase.Directory = ponyBaseWanted.Item1)
                ' Add the designated amount of a given pony.
                For i = 1 To ponyBaseWanted.Item2
                    startupPonies.Add(New Pony(context, base))
                Next
            Next

            ' Add a random amount of ponies.
            If randomPoniesWanted > 0 Then
                Dim remainingPonyBases = ponies.Bases.ToList()
                If Options.NoRandomDuplicates Then
                    remainingPonyBases.RemoveAll(Function(pb) ponyBasesWanted.Any(Function(t) t.Item1 = pb.Directory))
                End If
                For i = 1 To randomPoniesWanted
                    If remainingPonyBases.Count = 0 Then Exit For
                    Dim index = Rng.Next(remainingPonyBases.Count)
                    startupPonies.Add(New Pony(context, remainingPonyBases(index)))
                    If Options.NoRandomDuplicates Then remainingPonyBases.RemoveAt(index)
                Next
            End If

            PonyStartup(Function() New DesktopPonyAnimator(ponyViewer, startupPonies, ponies, context, True, Me), startupPonies)
            LoadPoniesAsyncEnd(False)
        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error attempting to launch ponies.")
            LoadPoniesAsyncEnd(True)
#If DEBUG Then
            Throw
#End If
        End Try
    End Sub

    Private Sub PonyStartup(createAnimator As Func(Of DesktopPonyAnimator), startupPonies As IEnumerable(Of Pony))
        If Globals.InScreensaverMode Then TryInvoke(AddressOf CreateScreensaverForms)

        AddHandlerDisplaySettingsChanged(AddressOf ReturnToMenuOnResolutionChange)
        ponyViewer = Options.GetInterface()
        ponyViewer.Topmost = Options.AlwaysOnTop
        If TypeOf ponyViewer Is WinFormSpriteInterface Then
            DirectCast(ponyViewer, WinFormSpriteInterface).ShowPerformanceGraph = Options.ShowPerformanceGraph
            DirectCast(ponyViewer, WinFormSpriteInterface).BackgroundColor = Options.BackgroundColor
        End If

        ' Get a collection of all images to be loaded.
        Dim images = startupPonies.SelectMany(Function(p) p.Base.Behaviors).Select(
            Function(b) New SpriteImagePaths(b.LeftImage.Path, b.RightImage.Path)).Concat(
            startupPonies.SelectMany(Function(p) p.Base.Effects).Select(
                Function(e) New SpriteImagePaths(e.LeftImage.Path, e.RightImage.Path))).Distinct().ToArray()
        worker.QueueTask(Sub()
                             LoadingProgressBar.Value = 0
                             LoadingProgressBar.Maximum = images.Length
                         End Sub)
        Try
            ponyViewer.LoadImages(images, Sub() worker.QueueTask(Sub() LoadingProgressBar.Value += 1))
            animator = createAnimator()
            AddHandler animator.AnimationFinished, AddressOf ShutdownOnFinish
        Catch ex As Exception
            ponyViewer.Dispose()
            Throw
        End Try
    End Sub

    Private Sub ShutdownOnFinish(sender As Object, e As EventArgs)
        Threading.ThreadPool.QueueUserWorkItem(
            Sub() TryInvoke(
                Sub()
                    Dim exitRequest = animator.ExitRequested
                    PonyShutdown()
                    If exitRequest = ExitRequest.ExitApplication Then
                        Close()
                    Else
                        DeactivateAutostart()
                        Show()
                        General.FullCollect()
                    End If
                End Sub))
    End Sub

    Private Sub CreateScreensaverForms()
        If Options.ScreensaverStyle <> Options.ScreensaverBackgroundStyle.Transparent Then
            screensaverForms = New List(Of ScreensaverBackgroundForm)()

            Dim backgroundColor As Color = Color.Black
            Dim backgroundImage As Image = Nothing
            If Options.ScreensaverStyle = Options.ScreensaverBackgroundStyle.SolidColor Then
                backgroundColor = Color.FromArgb(255, Options.ScreensaverBackgroundColor)
            End If
            If Options.ScreensaverStyle = Options.ScreensaverBackgroundStyle.BackgroundImage Then
                Try
                    backgroundImage = Image.FromFile(Options.ScreensaverBackgroundImagePath)
                Catch
                    ' Image failed to load, so we'll fall back to a background color.
                End Try
            End If

            For Each monitor In Screen.AllScreens
                Dim screensaverBackground As New ScreensaverBackgroundForm()
                screensaverForms.Add(screensaverBackground)

                If backgroundImage IsNot Nothing Then
                    screensaverBackground.BackgroundImage = backgroundImage
                Else
                    screensaverBackground.BackColor = backgroundColor
                End If

                screensaverBackground.Size = monitor.Bounds.Size
                screensaverBackground.Location = monitor.Bounds.Location

                screensaverBackground.Show()
            Next
        End If
        Cursor.Hide()
    End Sub

    Private Sub ReturnToMenuOnResolutionChange(sender As Object, e As EventArgs)
        TryInvoke(Sub()
                      PonyShutdown()
                      DeactivateAutostart()
                      Show()
                      MessageBox.Show(Me, "You have been returned to the menu because your screen resolution has changed.",
                                      "Resolution Changed - Desktop Ponies", MessageBoxButtons.OK, MessageBoxIcon.Information)
                  End Sub)
    End Sub

    Private Sub LoadPoniesAsyncEnd(cancelled As Boolean, Optional uiAction As Action = Nothing)
        If (Disposing OrElse IsDisposed) AndAlso animator IsNot Nothing Then animator.Dispose()
        worker.QueueTask(
            Sub()
                Dim totalImages = LoadingProgressBar.Maximum

                LoadingProgressBar.Value = 0
                LoadingProgressBar.Maximum = 1
                If Not Runtime.IsMono Then LoadingProgressBar.Visible = False

                If uiAction IsNot Nothing Then uiAction()
                SelectionControlsPanel.Enabled = True

                loading = False
                If Not cancelled Then
                    Visible = False
                    animator.Start()
                    loadWatch.Stop()
                    Console.WriteLine("Loaded in {0:0.00s} ({1} images)", loadWatch.Elapsed.TotalSeconds, totalImages)
                End If
            End Sub)
    End Sub
#End Region

    Private Sub PonySelectionPanel_Resize(sender As Object, e As EventArgs) Handles PonySelectionPanel.Resize
        ' If a horizontal scrollbar has appeared, renew the layout to forcibly remove it.
        If PonySelectionPanel.HorizontalScroll.Visible Then
            PonySelectionPanel.SuspendLayout()
            For Each selectionControl As PonySelectionControl In PonySelectionPanel.Controls
                selectionControl.Visible = False
            Next
            PonySelectionPanel.ResumeLayout()
            ' Perform a layout so cached positions are cleared, then restore visibility to its previous state.
            RepaginateSelection()
        End If
    End Sub

    Private Sub PonyShutdown()
        RemoveHandlerDisplaySettingsChanged(AddressOf ReturnToMenuOnResolutionChange)

        If animator IsNot Nothing Then
            RemoveHandler animator.AnimationFinished, AddressOf ShutdownOnFinish
            animator.Finish()
            animator.Clear()
            animator = Nothing
        End If

        If screensaverForms IsNot Nothing Then
            For Each screensaverForm In screensaverForms
                screensaverForm.Dispose()
            Next
            screensaverForms = Nothing
        End If

        If ponyViewer IsNot Nothing Then
            ponyViewer.Close()
            ponyViewer = Nothing
        End If

        ReloadFilterCategories()
        ResizePreviewImages()
    End Sub

    ''' <summary>
    ''' Resets pony selection related controls, which will require them to be reloaded from disk.
    ''' </summary>
    Private Sub ResetPonySelection()
        ponies = Nothing
        SelectionControlsPanel.Enabled = False
        selectionControlFilter.Clear()
        PonySelectionPanel.SuspendLayout()
        For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
            ponyPanel.Dispose()
        Next
        PonySelectionPanel.Controls.Clear()
        PonySelectionPanel.ResumeLayout()
    End Sub

    Friend Sub ReloadFilterCategories()
        Dim currentSelection = New HashSet(Of CaseInsensitiveString)(FilterOptionsBox.CheckedItems.OfType(Of CaseInsensitiveString)())
        Dim notTaggedChecked = notTaggedFilterIndex <> -1 AndAlso FilterOptionsBox.GetItemChecked(notTaggedFilterIndex)
        FilterOptionsBox.SuspendLayout()
        FilterOptionsBox.Items.Clear()
        FilterOptionsBox.Items.AddRange(PonyBase.StandardTags.Concat(Options.CustomTags).ToArray())
        notTaggedFilterIndex = FilterOptionsBox.Items.Add("[Not Tagged]")
        For i = 0 To FilterOptionsBox.Items.Count - 1
            If i = notTaggedFilterIndex Then
                FilterOptionsBox.SetItemChecked(i, notTaggedChecked)
            ElseIf currentSelection.Contains(DirectCast(FilterOptionsBox.Items(i), CaseInsensitiveString)) Then
                FilterOptionsBox.SetItemChecked(i, True)
            End If
        Next
        FilterOptionsBox.ResumeLayout()
        RefilterSelection()
    End Sub

    Friend Sub ResizePreviewImages()
        PonySelectionPanel.SuspendLayout()
        For Each control As PonySelectionControl In PonySelectionPanel.Controls
            control.ResizeToFit()
            control.Invalidate()
        Next
        PonySelectionPanel.ResumeLayout()
    End Sub

    Private Sub Main_LocationChanged(sender As Object, e As EventArgs) Handles MyBase.LocationChanged
        ' If we have just returned from the minimized state, the flow panel will have an incorrect scrollbar.
        ' Force a layout to get the bar re-evaluated and fixed.
        If oldWindowState = FormWindowState.Minimized AndAlso WindowState <> FormWindowState.Minimized Then
            layoutPendingFromRestore = True
        End If
        oldWindowState = WindowState
    End Sub

    Private Sub PonySelectionPanel_Paint(sender As Object, e As PaintEventArgs) Handles PonySelectionPanel.Paint
        If layoutPendingFromRestore Then
            PonySelectionPanel.PerformLayout()
            layoutPendingFromRestore = False
        End If
    End Sub

    Private Sub Main_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        AnimationTimer.Enabled = Not loading
    End Sub

    Private Sub Main_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        AnimationTimer.Enabled = False
    End Sub

    Private Sub AnimationTimer_Tick(sender As Object, e As EventArgs) Handles AnimationTimer.Tick
        For Each selectionControl As PonySelectionControl In PonySelectionPanel.Controls
            selectionControl.AdvanceTimeIndex(TimeSpan.FromMilliseconds(AnimationTimer.Interval))
        Next
    End Sub

    <Security.Permissions.PermissionSet(Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
    Private Shared Sub AddHandlerDisplaySettingsChanged(handler As EventHandler)
        AddHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, handler
    End Sub

    <Security.Permissions.PermissionSet(Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
    Private Shared Sub RemoveHandlerDisplaySettingsChanged(handler As EventHandler)
        RemoveHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, handler
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            RemoveHandlerDisplaySettingsChanged(AddressOf ReturnToMenuOnResolutionChange)
            If disposing Then
                If components IsNot Nothing Then components.Dispose()
                If animator IsNot Nothing Then animator.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
End Class
