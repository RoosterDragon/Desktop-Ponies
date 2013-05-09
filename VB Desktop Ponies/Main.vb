Imports System.Globalization
Imports System.IO
Imports CSDesktopPonies.SpriteManagement

''' <summary>
''' This is the Main form that handles startup and pony selection.
''' </summary>
Public Class Main
    Friend Shared Instance As Main

#Region "Fields and Properties"
    Private initialized As Boolean = False
    Private loading As Boolean = False
    Private loadWatch As New Diagnostics.Stopwatch()
    Private ReadOnly idleWorker As IdleWorker = idleWorker.CurrentThreadWorker

    Private oldWindowState As FormWindowState
    Private layoutPendingFromRestore As Boolean

    Friend DesktopHandle As IntPtr
    Friend ShellHandle As IntPtr
    Friend processId As IntPtr
    Friend Suspended_For_FullScreenApp As Boolean = False

    Private animator As DesktopPonyAnimator
    Private ponyViewer As ISpriteCollectionView
    Friend Startup_Ponies As New List(Of Pony)
    Friend SelectablePonies As New List(Of PonyBase)
    Friend DeadEffects As New List(Of Effect)
    Friend ActiveSounds As New List(Of Object)
    Friend HouseBases As New List(Of HouseBase)
    Private screensaverForms As List(Of ScreensaverBackgroundForm)

    'How big the area around the cursor used for cursor detection should be, in pixels.
    Friend CursorZoneSize As Integer = 100
    'used to tell when we should come out of screensaver mode
    Friend cursor_position As New Point

    'Are ponies currently walking around the desktop?
    Friend Ponies_Have_Launched As Boolean = False

    'Is any pony being dragged by the mouse?
    Friend Dragging As Boolean = False
    Friend controlled_pony As String = ""

    Friend Audio_Last_Played As DateTime = DateTime.UtcNow
    Friend Last_Audio_Length As Integer = 0 'milliseconds

    'Used in the editor.
    Friend InPreviewMode As Boolean = False

    'Were we told to auto-start from the command line?
    Friend AutoStarted As Boolean = False

    Friend ScreensaverMode As Boolean = False
    Friend screen_saver_path As String = ""
    Dim screensaver_settings_file_path As String = Path.Combine(Path.GetTempPath, "DesktopPonies_ScreenSaver_Settings.ini")

    Friend games As New List(Of Game)
    Friend CurrentGame As Game = Nothing

    'the following are used when in manual control mode
    Friend PonyUp As Boolean = False
    Friend PonyDown As Boolean = False
    Friend PonyLeft As Boolean = False
    Friend PonyRight As Boolean = False
    Friend PonySpeed As Boolean = False 'shift key is being pressed, so go faster.
    Friend PonyAction As Boolean = False

    'Keys for player 2
    Friend PonyUp_2 As Boolean = False
    Friend PonyDown_2 As Boolean = False
    Friend PonyLeft_2 As Boolean = False
    Friend PonyRight_2 As Boolean = False
    Friend PonySpeed_2 As Boolean = False 'shift key is being pressed, so go faster.
    Friend PonyAction_2 As Boolean = False

    Friend NoRandomDuplicates As Boolean = True

    Friend DirectXSoundAvailable As Boolean = False

    Dim all_sleeping As Boolean = False

    Friend AudioErrorShown As Boolean = False

    'A temporary list of selected filter settings.
    Dim Temp_Filters As New List(Of String)

    Dim dont_load_profile As Boolean = False
    Dim startup_profile As String = Options.DefaultProfileName

    Private previewWindowRectangle As Func(Of Rectangle)

    Private ReadOnly selectionControlFilter As New Dictionary(Of PonySelectionControl, Boolean)
    Private ponyOffset As Integer
    Private ReadOnly selectionControlsFilteredVisible As IEnumerable(Of PonySelectionControl) =
        selectionControlFilter.Where(Function(kvp) kvp.Value).Select(Function(kvp) kvp.Key)

    Private ReadOnly screensToUse As New List(Of Screen)
#End Region

    Enum BehaviorOption
        Name = 1
        Probability = 2
        MaxDuration = 3
        MinDuration = 4
        Speed = 5 'specified in pixels per tick of the timer
        RightImagePath = 6
        LeftImagePath = 7
        MovementType = 8
        LinkedBehavior = 9
        SpeakingStart = 10
        SpeakingEnd = 11
        Skip = 12 'Should we skip this behavior when considering ones to randomly choose (part of an interaction/chain?)
        XCoord = 13  'used when following/moving to a point on the screen.
        YCoord = 14
        ObjectToFollow = 15
        AutoSelectImages = 16
        FollowStoppedBehavior = 17
        FollowMovingBehavior = 18
        RightImageCenter = 19
        LeftImageCenter = 20
        DoNotRepeatImageAnimations = 21
        Group = 22
    End Enum

#Region "Initialization"

    Public Sub New()
        loadWatch.Start()
        InitializeComponent()
        initialized = True
    End Sub

    'Read all configuration files and pony folders.
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BeginInvoke(New Action(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        Console.WriteLine("Main Loading after {0:0.00s}", loadWatch.Elapsed.TotalSeconds)
        Instance = Me

        Text = "Desktop Ponies v" & My.MyApplication.GetProgramVersion()
        Me.Icon = My.Resources.Twilight

        Application.DoEvents()

        'DesktopHandle = DetectFulLScreen_m.GetDesktopWindow()
        'ShellHandle = DetectFulLScreen_m.GetShellWindow()

        'need our own PID for window avoidance (ignoring collisions with other ponies)
        Using currentProcess = Diagnostics.Process.GetCurrentProcess()
            processId = currentProcess.Handle
        End Using

        ' Check to see if the right version of DirectX is installed for sounds.
        Try
            System.Reflection.Assembly.Load(
                "Microsoft.DirectX.AudioVideoPlayback, Version=1.0.2902.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")
            DirectXSoundAvailable = True
        Catch ex As Exception
            ' If we can't load the assembly, just don't enable sound.
        End Try

        Try
            Dim Arguments = My.Application.CommandLineArgs

            If Arguments.Count = 0 Then

                If Environment.GetCommandLineArgs()(0).EndsWith(".scr", StringComparison.OrdinalIgnoreCase) Then
                    'for some versions of windows, starting with no parameters is the same as /c (configure)
                    SetScreensaverPath()
                    Me.Close()
                    Exit Sub
                End If

                Exit Try
            End If

            'handle any comment line arguments
            If Arguments.Count > 0 Then
                Select Case Split(LCase(Trim(Arguments(0))), ":")(0)
                    Case "autostart"
                        AutoStarted = True
                        Me.ShowInTaskbar = False
                        ShowInTaskbar = False

                        Try
                            Options.LoadProfile("autostart")
                        Catch
                            Options.LoadDefaultProfile()
                        End Try

                        'windows is telling us "start as a screensaver"
                    Case "/s"
                        GetScreensaverPath()
                        If screen_saver_path = "" Then Me.Close()
                        Options.InstallLocation = screen_saver_path
                        ScreensaverMode = True
                        AutoStarted = True
                        ShowInTaskbar = False
                        WindowState = FormWindowState.Minimized

                        Try
                            Options.LoadProfile("screensaver")
                        Catch
                            Options.LoadDefaultProfile()
                        End Try

                        'windows says: "preview screensaver".  This isn't implemented so just quit
                    Case "/p"
                        Me.Close()
                        Exit Sub
                        'windows says:  "configure screensaver"
                    Case "/c"
                        SetScreensaverPath()
                        Me.Close()
                        Exit Sub
                    Case Else
                        MsgBox("Invalid command line argument.  Usage: " & ControlChars.NewLine & _
                               "desktop ponies.exe autostart - Automatically start with saved settings (or defaults if no settings are saved)" & ControlChars.NewLine & _
                               "desktop ponies.exe /s - Start in screensaver mode (you need to run /c first to configure the path to the pony files)" & ControlChars.NewLine & _
                               "desktop ponies.exe /c - Configure the path to pony files, only used for Screensaver mode." & ControlChars.NewLine & _
                               "desktop ponies.exe /p - Screensaver preview use only.  Not implemented.")
                        Me.Close()
                        Exit Sub
                End Select
            End If

        Catch ex As Exception
            MsgBox("Error processing command line arguments." & ControlChars.NewLine & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try

        loading = True

        If Not AutoStarted Then
            WindowState = FormWindowState.Normal
        End If

        'temporarily save filter selections, if any, in the case that we are reloading after making a change in the editor.
        '(Loading options resets the filter, and will cause havoc otherwise)
        SaveFilterSelections()

        'the options form is needed now as the preferences are read directly from the controls on that form.
        'we load it invisibly here
        ' IMPORTANT: Porting note. The My.Forms class provides access to forms, but provides a new one per thread.
        ' This annoying global state is now a problem due to callbacks from worker threads.
        ' Affected forms have an instance class which must now be referenced.
        ' Ideally, this global state would not be used and instances would get passed down the chain, but I am making do.
        ' Ensure therefore that the instances are initialized on this application thread for later.
        ' The PonyEditor also requires this, but must wait until templates are loaded.
        OptionsForm.Instance = New OptionsForm()
        'OptionsForm.Instance.Load_Button_Click(Nothing, Nothing, Option.DefaultProfileName, True)
        AddHandler Options.MonitorNames.CollectionChanged, AddressOf MonitorNames_CollectionChanged

        LoadFilterSelections()

        ' Load the profile that was last in use by this user.
        Dim profile = Options.DefaultProfileName
        Dim profileFile As IO.StreamReader = Nothing
        Try
            profileFile = New IO.StreamReader(IO.Path.Combine(Options.ProfileDirectory, "current.txt"),
                                              System.Text.Encoding.UTF8)
            profile = profileFile.ReadLine()
        Catch ex As IO.FileNotFoundException
            ' We don't mind if no preferred profile is saved.
        Catch ex As IO.DirectoryNotFoundException
            ' In screensaver mode, the user might set a bad path. We'll ignore it for now.
        Finally
            If profileFile IsNot Nothing Then profileFile.Close()
        End Try
        GetProfiles(profile)

        Dim loadTemplates = True
        Dim startedAsScr = Environment.GetCommandLineArgs()(0).EndsWith(".scr", StringComparison.OrdinalIgnoreCase)
        If startedAsScr Then
            ' Check screensaver path is still valid.
            If Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Main.Instance.screen_saver_path, PonyBase.RootDirectory)) OrElse
            Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Main.Instance.screen_saver_path, HouseBase.RootDirectory)) OrElse
            Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Main.Instance.screen_saver_path, Game.RootDirectory)) Then
                MsgBox("The screensaver path does not appear to be correct. Please adjust it.")
                Dim result = SetScreensaverPath()
                If Not result Then
                    MsgBox("Will not be able to run screensaver mode until the correct path is specified.")
                Else
                    MsgBox("Restart the screensaver for changes to take effect.")
                End If
                loadTemplates = False
                loading = False
                Close()
            End If

            ' Start in a minimized state to load, and attempt to open the screensaver profile.
            ShowInTaskbar = False
            WindowState = FormWindowState.Minimized
            Try
                Options.LoadProfile("screensaver")
            Catch
                Options.LoadDefaultProfile()
            End Try
        End If

        ' Force any pending messages to be processed for Mono, which may get caught up with the background loading before the form gets
        ' fully drawn.
        Application.DoEvents()
        Console.WriteLine("Main Loaded after {0:0.00s}", loadWatch.Elapsed.TotalSeconds)

        If loadTemplates Then
            Threading.ThreadPool.QueueUserWorkItem(Sub() Me.LoadTemplates())
        End If
    End Sub

    Sub GetScreensaverPath()
        Try
            'We can't use isolated storage as windows uses 8 character names when starting as a screensaver for some reason, and then
            'gets confused when it can't find the assembly name "DESKTO~1"...
            'instead, we just place a file in the temporary folder.

            'Dim UserIsolatedStorage = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly()
            'Dim UserIsolatedStorageFile = New System.IO.IsolatedStorage.IsolatedStorageFileStream("DesktopPonies_ScreenSaver_Settings.txt", _
            '                                                                      IO.FileMode.Open, IO.FileAccess.Read, UserIsolatedStorage)

            'Dim SettingsFile As New System.IO.StreamReader(UserIsolatedStorageFile)

            If Not My.Computer.FileSystem.FileExists(screensaver_settings_file_path) Then
                SetScreensaverPath()
            End If

            Using reader As New StreamReader(screensaver_settings_file_path)
                screen_saver_path = reader.ReadLine()
            End Using
            'UserIsolatedStorageFile.Close()
            'UserIsolatedStorage.Close()

            While Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Main.Instance.screen_saver_path, PonyBase.RootDirectory)) OrElse
            Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Main.Instance.screen_saver_path, HouseBase.RootDirectory)) OrElse
            Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Main.Instance.screen_saver_path, Game.RootDirectory))
                MsgBox("The screensaver path does not appear to be correct. Please adjust it." & vbNewLine &
                       "The '" & PonyBase.RootDirectory & "', '" & HouseBase.RootDirectory &
                   "' and '" & Game.RootDirectory & "' directories are expected to exist in this location. Please check your selection.")
                If Not SetScreensaverPath() Then Exit Sub
            End While

        Catch ex As Exception
            MsgBox("Error: You need to set the settings of the screensaver first before using it." & ControlChars.NewLine & _
                   "Unable to read settings file.  Screensaver mode will not work.  Details: " & ex.Message)
        End Try
    End Sub

    Private Function SetScreensaverPath() As Boolean
        Try

            Try
                If My.Computer.FileSystem.FileExists(screensaver_settings_file_path) Then
                    Using existing_file As New StreamReader(screensaver_settings_file_path)
                        screen_saver_path = existing_file.ReadLine()
                        SelectFilesPathDialog.PathTextBox.Text = screen_saver_path
                    End Using
                End If
            Catch ex As Exception
                MsgBox("Error reading current settings: " & ex.Message)
            End Try

            If SelectFilesPathDialog.ShowDialog() <> DialogResult.OK Then
                Return False
            End If

            'Dim UserIsolatedStorage = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly()
            'Dim UserIsolatedStorageFile = New System.IO.IsolatedStorage.IsolatedStorageFileStream("DesktopPonies_ScreenSaver_Settings.txt", _
            '                                                                      IO.FileMode.Create, IO.FileAccess.Write, UserIsolatedStorage)

            'Dim SettingsFile As New System.IO.StreamWriter(UserIsolatedStorageFile, System.Text.Encoding.Unicode)

            'we use Unicode here, and any time we save a file as other languages can cause problems.
            Using writer = New StreamWriter(screensaver_settings_file_path, False, System.Text.Encoding.UTF8)
                writer.WriteLine(screen_saver_path)
            End Using
            'UserIsolatedStorageFile.Close()
            'UserIsolatedStorage.Close()

        Catch ex As Exception
            MsgBox("Error:  Unable to create settings file.  Screensaver mode will not work.  Details: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Sub SaveFilterSelections()

        Temp_Filters.Clear()
        For Each item As String In FilterOptionsBox.CheckedItems
            Temp_Filters.Add(item)
        Next

    End Sub

    Sub LoadFilterSelections()

        For Each item As String In Temp_Filters
            Try
                FilterOptionsBox.SetItemChecked(FilterOptionsBox.Items.IndexOf(item), True)
            Catch ex As Exception
                'Filter is not valid at time of restoring.  Do nothing
            End Try
        Next

    End Sub

    Private Sub LoadTemplates()
        Try
            Dim ponyBaseDirectories = Directory.GetDirectories(Path.Combine(Options.InstallLocation, PonyBase.RootDirectory))
            Array.Sort(ponyBaseDirectories, StringComparer.CurrentCultureIgnoreCase)

            While Not IsHandleCreated
            End While

            idleWorker.QueueTask(Sub() LoadingProgressBar.Maximum = ponyBaseDirectories.Count)

            Dim skipLoadingErrors As Boolean = False
            For Each folder In Directory.GetDirectories(Path.Combine(Options.InstallLocation, HouseBase.RootDirectory))
                skipLoadingErrors = LoadHouse(folder, skipLoadingErrors)
            Next

            Dim ponyBasesToAdd As New List(Of PonyBase)
            For Each folder In ponyBaseDirectories
                Try
                    Dim pony = New PonyBase(folder)
                    ponyBasesToAdd.Add(pony)
                    idleWorker.QueueTask(Sub()
                                             AddToMenu(pony)
                                             LoadingProgressBar.Value += 1
                                         End Sub)
                Catch ex As InvalidDataException
                    If skipLoadingErrors = False Then
                        Select Case MsgBox("Error: Invalid data in " & PonyBase.ConfigFilename & " configuration file in " & folder _
                           & ControlChars.NewLine & "Won't load this pony..." & ControlChars.NewLine _
                           & "Do you want to skip seeing these errors?  Press No to see the error for each pony.  Press cancel to quit.", MsgBoxStyle.YesNoCancel)
                            Case MsgBoxResult.Yes
                                skipLoadingErrors = True
                            Case MsgBoxResult.No
                                'do nothing
                            Case MsgBoxResult.Cancel
                                Me.Close()
                        End Select
                    End If
                Catch ex As FileNotFoundException
                    If skipLoadingErrors = False Then
                        Select Case MsgBox("Error: No " & PonyBase.ConfigFilename & " configuration file found for folder: " & folder _
                           & ControlChars.NewLine & "Won't load this pony..." & ControlChars.NewLine _
                           & "Do you want to skip seeing these errors?  Press No to see the error for each pony.  Press cancel to quit.", MsgBoxStyle.YesNoCancel)
                            Case MsgBoxResult.Yes
                                skipLoadingErrors = True
                            Case MsgBoxResult.No
                                'do nothing
                            Case MsgBoxResult.Cancel
                                Me.Close()
                        End Select
                    End If
                End Try
            Next

            idleWorker.WaitOnAllTasks()
            If SelectablePonies.Count = 0 Then
                MessageBox.Show(Me, "Sorry, but you don't seem to have any ponies installed. " &
                                "There should have at least been a 'Derpy' folder in the same spot as this program.",
                                "No Ponies Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                GoButton.Enabled = False
            End If

            'Load pony counts.
            idleWorker.QueueTask(Sub() Options.LoadPonyCounts())

            ' Wait for all images to load.
            idleWorker.QueueTask(Sub()
                                     For Each control As PonySelectionControl In PonySelectionPanel.Controls
                                         control.ShowPonyImage = True
                                         control.Invalidate()
                                     Next
                                 End Sub)

        Catch ex As Exception
#If Not Debug Then
            MsgBox("Error starting up!  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
            Exit Sub
#Else
            Throw
#End If
        End Try

        idleWorker.QueueTask(Sub()
                                 Console.WriteLine("Templates Loaded after {0:0.00s}", loadWatch.Elapsed.TotalSeconds)

                                 If AutoStarted = True Then
                                     'Me.Opacity = 0
                                     GoButton_Click(Nothing, Nothing)
                                 Else
                                     'Me.Opacity = 100
                                 End If

                                 CountSelectedPonies()

                                 If OperatingSystemInfo.IsWindows Then LoadingProgressBar.Visible = False
                                 LoadingProgressBar.Value = 0
                                 LoadingProgressBar.Maximum = 1

                                 PoniesPerPage.Maximum = PonySelectionPanel.Controls.Count
                                 PonyPaginationLabel.Text = String.Format(
                                     CultureInfo.CurrentCulture, "Viewing {0} ponies", PonySelectionPanel.Controls.Count)
                                 PaginationEnabled.Enabled = True
                                 PaginationEnabled.Checked = OperatingSystemInfo.IsMacOSX

                                 PonySelectionPanel.Enabled = True
                                 SelectionControlsPanel.Enabled = True
                                 AnimationTimer.Enabled = True
                                 loading = False
                                 General.FullCollect()

                                 loadWatch.Stop()
                                 Console.WriteLine("Loaded in {0:0.00s} ({1} templates)",
                                                   loadWatch.Elapsed.TotalSeconds, PonySelectionPanel.Controls.Count)
                             End Sub)
    End Sub

    Function LoadHouse(folder As String, skipErrors As Boolean) As Boolean
        Try
            Dim base = New HouseBase(folder)
            HouseBases.Add(base)
            Return True
        Catch ex As Exception
            If skipErrors = False Then
                Select Case MsgBox("Error: No " & HouseBase.ConfigFilename & " configuration file found for folder: " & folder _
                   & ControlChars.NewLine & "Won't load this house/structure..." & ControlChars.NewLine _
                   & "Do you want to skip seeing these errors?  Press No to see the error for each folder.  Press cancel to quit.",
                   MsgBoxStyle.YesNoCancel)

                    Case MsgBoxResult.Yes
                        Return True
                    Case MsgBoxResult.No
                        'do nothing
                    Case MsgBoxResult.Cancel
                        Me.Close()
                End Select
            End If
            Return skipErrors
        End Try
    End Function

    Friend Shared Function GetDirection(setting As String) As Direction
        Select Case setting
            Case "top_left"
                Return Direction.TopLeft
            Case "top"
                Return Direction.TopCenter
            Case "top_right"
                Return Direction.TopRight
            Case "left"
                Return Direction.MiddleLeft
            Case "center"
                Return Direction.MiddleCenter
            Case "right"
                Return Direction.MiddleRight
            Case "bottom_left"
                Return Direction.BottomLeft
            Case "bottom"
                Return Direction.BottomCenter
            Case "bottom_right"
                Return Direction.BottomRight
            Case "any"
                Return Direction.Random
            Case "any-not_center"
                Return Direction.RandomNotCenter
            Case Else
                Dim result As Direction
                If [Enum].TryParse(setting, result) Then
                    Return result
                Else
                    Throw New ArgumentException("Invalid placement direction or centering for effect.", "setting")
                End If
        End Select
    End Function

    Private Sub AddToMenu(ponyBase As PonyBase)
        SelectablePonies.Add(ponyBase)

        Dim ponySelection As New PonySelectionControl(ponyBase, ponyBase.Behaviors(0).RightImagePath, False)
        AddHandler ponySelection.PonyCount.TextChanged, AddressOf HandleCountChange
        If ponyBase.Directory = "Random Pony" Then
            ponySelection.NoDuplicates.Visible = True
            ponySelection.NoDuplicates.Checked = NoRandomDuplicates
            AddHandler ponySelection.NoDuplicates.CheckedChanged, Sub() NoRandomDuplicates = ponySelection.NoDuplicates.Checked
        End If
        If OperatingSystemInfo.IsMacOSX Then ponySelection.Visible = False

        selectionControlFilter.Add(ponySelection, ponySelection.Visible)
        PonySelectionPanel.Controls.Add(ponySelection)
        ponySelection.Update()
    End Sub

    Private Sub HandleCountChange(sender As Object, e As EventArgs)
        CountSelectedPonies()
    End Sub

    Private Sub CountSelectedPonies()

        Dim total_ponies As Integer = 0

        For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
            Dim count As Integer
            If Integer.TryParse(ponyPanel.PonyCount.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, count) Then
                total_ponies += count
            End If
        Next

        PonyCountValueLabel.Text = CStr(total_ponies)

    End Sub
#End Region

#Region "Selection"
    Friend Sub ZeroPoniesButton_Click(sender As Object, e As EventArgs) Handles ZeroPoniesButton.Click
        For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
            ponyPanel.PonyCount.Text = "0"
        Next
    End Sub

    Private Sub SaveProfileButton_Click(sender As Object, e As EventArgs) Handles SaveProfileButton.Click
        Dim profileToSave = ProfileComboBox.Text

        If profileToSave = "" Then
            MsgBox("Enter a profile name first!")
            Exit Sub
        End If

        If String.Equals(profileToSave, Options.DefaultProfileName, StringComparison.OrdinalIgnoreCase) Then
            MsgBox("Cannot save over the '" & Options.DefaultProfileName & "' profile. " &
                   "To create a new profile, type a new name for the profile into the box. You will then be able to save the profile.")
            Exit Sub
        End If

        If Not ProfileComboBox.Items.Contains(profileToSave) Then
            ProfileComboBox.Items.Add(profileToSave)
        End If
        ProfileComboBox.SelectedItem = profileToSave

        Options.SaveProfile(profileToSave)
        MessageBox.Show(Me, "Profile '" & profileToSave & "' saved.", "Profile Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub LoadProfileButton_Click(sender As Object, e As EventArgs) Handles LoadProfileButton.Click
        'Options.LoadProfile(ProfileComboBox.Text)
        OptionsForm.Instance.LoadButton_Click(sender, e, ProfileComboBox.Text)
    End Sub

    Private Sub OnePoniesButton_Click(sender As Object, e As EventArgs) Handles OnePoniesButton.Click
        For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
            ponyPanel.PonyCount.Text = "1"
        Next
    End Sub

    Private Sub OptionsButton_Click(sender As Object, e As EventArgs) Handles OptionsButton.Click
        Main.Instance.Invoke(Sub()
                                 OptionsForm.Instance.Show()
                             End Sub)
    End Sub

    Private Sub PonyEditorButton_Click(sender As Object, e As EventArgs) Handles PonyEditorButton.Click

        InPreviewMode = True
        Me.Visible = False
        Using form = New PonyEditor()
            previewWindowRectangle = AddressOf form.GetPreviewWindowScreenRectangle
            form.ShowDialog(Me)

            PonyShutdown()

            InPreviewMode = False
            If Not Me.IsDisposed Then
                Me.Visible = True
            End If

            OptionsForm.Instance.Hide()

            If form.changes_made Then
                DisposeMenu()
                LoadingProgressBar.Visible = True
                '(We need to reload everything to account for anything changed while in the editor)
                Main_Load(Nothing, Nothing)
            End If
        End Using

    End Sub

    Friend Function GetPreviewWindowRectangle() As Rectangle
        Return previewWindowRectangle()
    End Function

    Private Sub GamesButton_Click(sender As Object, e As EventArgs) Handles GamesButton.Click
        Try
            If games.Count <> 0 Then
                games.Clear()
            End If

            Dim gameDirectories = Directory.GetDirectories(Path.Combine(Options.InstallLocation, Game.RootDirectory))

            For Each gameDirectory In gameDirectories
                Try
                    Dim config_file_name = Path.Combine(gameDirectory, Game.ConfigFilename)

                    Dim new_game As New Game(gameDirectory)

                    games.Add(new_game)

                Catch ex As Exception
                    MsgBox("Error loading game: " & gameDirectory & ex.Message & ex.StackTrace)
                End Try
            Next

            Me.Visible = False
            If New GameSelectionForm().ShowDialog() = DialogResult.OK Then
                Startup_Ponies.Clear()
                PonyStartup()
                CurrentGame.Setup()
                animator.Start()
            Else
                If Me.IsDisposed = False Then
                    Me.Visible = True
                End If
            End If
        Catch ex As Exception
            MsgBox("Error loading games: " & ex.Message & ex.StackTrace)
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
        If profileIndex <> -1 Then ProfileComboBox.SelectedIndex = profileIndex
    End Sub

    Private Sub CopyProfileButton_Click(sender As Object, e As EventArgs) Handles CopyProfileButton.Click
        dont_load_profile = True

        Dim copiedProfileName = InputBox("Enter name of new profile to copy to:")
        copiedProfileName = Trim(copiedProfileName)
        If copiedProfileName = "" Then
            MsgBox("Can't enter a blank profile name!  Try again.")
            Exit Sub
        End If

        If String.Equals(copiedProfileName, Options.DefaultProfileName, StringComparison.OrdinalIgnoreCase) Then
            MsgBox("Cannot copy over the '" & Options.DefaultProfileName & "' profile")
            Exit Sub
        End If

        Options.SaveProfile(copiedProfileName)
        GetProfiles(copiedProfileName)

        dont_load_profile = False
    End Sub

    Private Sub DeleteProfileButton_Click(sender As Object, e As EventArgs) Handles DeleteProfileButton.Click
        If String.Equals(ProfileComboBox.Text, Options.DefaultProfileName, StringComparison.OrdinalIgnoreCase) Then
            MsgBox("Cannot delete the '" & Options.DefaultProfileName & "' profile")
            Exit Sub
        End If

        dont_load_profile = True

        If Options.DeleteProfile(ProfileComboBox.Text) Then
            MsgBox("Profile Deleted", MsgBoxStyle.OkOnly, "Success")
        Else
            MsgBox("Error attempting to delete profile. It may have already been deleted", MsgBoxStyle.OkOnly, "Error")
        End If
        GetProfiles(Options.DefaultProfileName)

        dont_load_profile = False
    End Sub

    Private Sub ProfileComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ProfileComboBox.SelectedIndexChanged
        If Not dont_load_profile Then
            'Options.LoadProfile(ProfileComboBox.Text)
            OptionsForm.Instance.LoadButton_Click(sender, e, ProfileComboBox.Text)
        End If
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

    Private Sub FilterAllRadio_CheckedChanged(sender As Object, e As EventArgs) Handles FilterAllRadio.CheckedChanged
        If FilterAllRadio.Checked AndAlso Me.Visible Then
            FilterOptionsBox.Enabled = False
            RefilterSelection()
        End If
    End Sub

    Private Sub RefilterSelection(Optional tags As IEnumerable(Of String) = Nothing)
        If tags Is Nothing Then tags = FilterOptionsBox.CheckedItems.Cast(Of String)()

        For Each selectionControl As PonySelectionControl In PonySelectionPanel.Controls
            'reshow all ponies in show all mode.
            If FilterAllRadio.Checked Then
                selectionControlFilter(selectionControl) = True
            End If

            'don't show ponies that don't have at least one of the desired tags in Show Any.. mode
            If FilterAnyRadio.Checked Then
                Dim visible = False
                For Each tag_to_show In tags
                    If selectionControl.PonyBase.Tags.Contains(tag_to_show) OrElse
                        (tag_to_show <> "Not Tagged" AndAlso selectionControl.PonyBase.Tags.Count = 0) Then
                        visible = True
                        Exit For
                    End If
                Next
                selectionControlFilter(selectionControl) = visible
            End If

            'don't show ponies that don't have all of the desired tags in Show Exactly.. mode
            If FilterExactlyRadio.Checked Then
                Dim visible = True
                For Each tag_to_show In tags
                    If Not (selectionControl.PonyBase.Tags.Contains(tag_to_show) OrElse
                        (tag_to_show <> "Not Tagged" AndAlso selectionControl.PonyBase.Tags.Count = 0)) Then
                        visible = False
                        Exit For
                    End If
                Next
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
        For Each selectionControl As PonySelectionControl In PonySelectionPanel.Controls
            Dim makeVisible = False
            If Not PaginationEnabled.Checked Then
                ' If pagination is disabled, simply show/hide the control according to the current filter.
                makeVisible = selectionControlFilter(selectionControl)
            ElseIf selectionControlFilter(selectionControl) Then
                ' If pagination is enabled, we will show it if it is filtered visible and within the page range.
                makeVisible = localOffset >= ponyOffset AndAlso visibleCount < PoniesPerPage.Value
                localOffset += 1
            End If
            If makeVisible Then visibleCount += 1
            Dim visibleChanged = selectionControl.Visible <> makeVisible
            selectionControl.Visible = makeVisible
            ' Force an update on Mac to try and get visibility change applied.
            If OperatingSystemInfo.IsMacOSX AndAlso visibleChanged Then
                selectionControl.Invalidate()
                selectionControl.Update()
            End If
        Next

        ' Force an update on Mac to try and clear leftover graphics.
        If OperatingSystemInfo.IsMacOSX Then
            PonySelectionPanel.Invalidate()
            PonySelectionPanel.Update()
        End If

        PonySelectionPanel.ResumeLayout()

        If Not PaginationEnabled.Checked OrElse visibleCount = 0 Then
            PonyPaginationLabel.Text = String.Format(CultureInfo.CurrentCulture, "Viewing {0} ponies", visibleCount)
        Else
            PonyPaginationLabel.Text =
            String.Format(CultureInfo.CurrentCulture,
                          "Viewing {0} to {1} of {2} ponies",
                          ponyOffset + 1,
                          Math.Min(ponyOffset + PoniesPerPage.Value, selectionControlsFilteredVisible.Count),
                          selectionControlsFilteredVisible.Count)
        End If

        Dim min = ponyOffset = 0
        Dim max = ponyOffset >= selectionControlsFilteredVisible.Count - PoniesPerPage.Value
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
                If selectionControl.PonyName.Text.Length > 0 Then
                    Dim compare = String.Compare(selectionControl.PonyName.Text(0), e.KeyChar, StringComparison.OrdinalIgnoreCase)
                    If compare = 0 Then
                        PonySelectionPanel.ScrollControlIntoView(selectionControl)
                        selectionControl.PonyCount.Focus()
                    End If
                    If compare >= 0 Then Exit For
                End If
            Next
        ElseIf e.KeyChar = "#" Then
#If DEBUG Then
            Using newEditor = New PonyEditorForm2()
                newEditor.ShowDialog(Me)
            End Using
#End If
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
        ponyOffset += Math.Min(selectionControlsFilteredVisible.Count - CInt(PoniesPerPage.Value) - ponyOffset, 1)
        RepaginateSelection()
    End Sub

    Private Sub NextPageButton_Click(sender As Object, e As EventArgs) Handles NextPageButton.Click
        ponyOffset += Math.Min(selectionControlsFilteredVisible.Count - CInt(PoniesPerPage.Value) - ponyOffset, CInt(PoniesPerPage.Value))
        RepaginateSelection()
    End Sub

    Private Sub LastPageButton_Click(sender As Object, e As EventArgs) Handles LastPageButton.Click
        ponyOffset = selectionControlsFilteredVisible.Count - CInt(PoniesPerPage.Value)
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
        Dim tags = FilterOptionsBox.CheckedItems.Cast(Of String).ToList()
        If e.CurrentValue <> e.NewValue Then
            Dim changedTag = CStr(FilterOptionsBox.Items(e.Index))
            If e.NewValue = CheckState.Checked Then
                tags.Add(changedTag)
            Else
                tags.Remove(changedTag)
            End If
        End If
        RefilterSelection(tags)
    End Sub
#End Region

#Region "Pony Startup"
    Private Sub GoButton_Click(sender As Object, e As EventArgs) Handles GoButton.Click
        If PonyLoader.IsBusy Then
            MessageBox.Show(Me, "Already busy loading ponies. Cannot start any more at this time.",
                            "Busy", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            loading = True
            SelectionControlsPanel.Enabled = False
            LoadingProgressBar.Visible = True
            loadWatch.Restart()
            PonyLoader.RunWorkerAsync()
        End If
    End Sub

    Private Sub PonyLoader_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles PonyLoader.DoWork
        Try
            Startup_Ponies.Clear()

            Dim number_of_ponies As New List(Of Integer)
            Dim pony_names As New List(Of String)

            Dim Total_Ponies As Integer = 0

            Dim random_ponies As Integer = 0

            'Go through each of the textboxes in the menu and record their names and the number of them wanted.

            For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
                Try
                    Dim ponyName = ponyPanel.PonyName.Text
                    Dim count As Integer
                    If Integer.TryParse(ponyPanel.PonyCount.Text, count) AndAlso count > 0 Then
                        If ponyName = "Random Pony" Then
                            random_ponies = count
                        End If

                        pony_names.Add(ponyName)
                        number_of_ponies.Add(count)
                        Total_Ponies += count
                    Else
                        pony_names.Add(ponyName)
                        number_of_ponies.Add(0)
                    End If
                Catch ex As Exception
                    MsgBox("Error: Too much pony!" & ControlChars.NewLine &
                           "Details: You entered something crazy in one of the boxes." & ControlChars.NewLine &
                           "Real Details: " & ex.Message)
                    e.Cancel = True
                    Exit Sub
                End Try
            Next

            If Total_Ponies = 0 Then
                If ScreensaverMode Then
                    Total_Ponies = 1
                    random_ponies = 1
                Else
                    MsgBox("The total is... no ponies...  That's TOO FEW PONY.")
                    e.Cancel = True
                    Exit Sub
                End If
            End If

            Dim maxPonies = 0
            Main.Instance.Invoke(Sub()
                                     maxPonies = CInt(OptionsForm.Instance.MaxPonies.Value)
                                 End Sub)
            If Total_Ponies > maxPonies Then
                MsgBox("Sorry, you selected " & Total_Ponies & " ponies, which is more than the limit specified in the options menu." &
                       ControlChars.NewLine & "Try less than " & maxPonies & " total." & ControlChars.NewLine &
                       "(Or override this limit in the options window)")
                e.Cancel = True
                Exit Sub
            End If

            Invoke(Sub()
                       LoadingProgressBar.Value = 0
                       LoadingProgressBar.Maximum = Total_Ponies
                   End Sub)

            'Make duplicates of each type of pony, up to the number needed
            For i = 0 To number_of_ponies.Count - 1
                If number_of_ponies(i) > 0 Then

                    Dim pony_template = FindPonyBaseByDirectory(pony_names(i))

                    If pony_template.Directory = "Random Pony" Then
                        Continue For
                    End If

                    For z = 1 To number_of_ponies(i)
                        Dim new_pony As Pony = New Pony(pony_template)
                        Startup_Ponies.Add(new_pony)
                    Next
                End If
            Next

            'Select random ponies, if necessary
            For i = 1 To random_ponies
                Dim selection = Rng.Next(SelectablePonies.Count)

                Dim selected_pony = SelectablePonies(selection)

                If NoRandomDuplicates Then

                    Dim duplicate As Boolean = False
                    Dim still_unique_available As Boolean = False

                    For Each pony In Startup_Ponies
                        If pony.Directory = selected_pony.Directory Then
                            duplicate = True
                        End If

                        If Startup_Ponies.Count + 1 >= SelectablePonies.Count Then
                            still_unique_available = False
                        Else
                            still_unique_available = True
                        End If

                        If duplicate AndAlso Not still_unique_available Then Exit For
                    Next

                    If duplicate AndAlso still_unique_available Then
                        i -= 1
                        Continue For
                    End If
                End If


                If selected_pony.Directory = "Random Pony" Then
                    i -= 1
                Else
                    Startup_Ponies.Add(New Pony(SelectablePonies(selection)))
                End If
            Next

            Try
                If OptionsForm.Instance.Interactions.Checked Then
                    InitializeInteractions()
                End If
            Catch ex As Exception
                MsgBox("Unable to initialize interactions.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
            End Try

            PonyStartup()
        Catch ex As Exception
#If Not Debug Then
            MsgBox("Error launching ponies... Details: " & ex.Message & ControlChars.NewLine _
                   & ex.StackTrace)
            e.Cancel = True
#Else
            Throw
#End If
        End Try
    End Sub

    Private Function FindPonyBaseByDirectory(directory As String) As PonyBase
        For Each base As PonyBase In SelectablePonies
            If base.Directory = directory Then
                Return base
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' After all of the ponies, and all of their interactions are loaded, we need to go through and see
    ''' which interactions can actually be used with which ponies are loaded, and see which ponies each 
    ''' interaction should interact with.
    ''' </summary>
    ''' <remarks></remarks>
    Sub InitializeInteractions()
        For Each pony In Startup_Ponies
            pony.InitializeInteractions(Startup_Ponies)
        Next
    End Sub

    Friend Sub PonyStartup()
        If ScreensaverMode Then
            Invoke(Sub()
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
                   End Sub)
        End If

        AddHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, AddressOf ReturnToMenuOnResolutionChange
        ponyViewer = GetInterface()
        ponyViewer.Topmost = Options.AlwaysOnTop

        If Not InPreviewMode Then
            ' Get a collection of all images to be loaded.
            Dim images As New HashSet(Of String)(StringComparer.Ordinal)
            For Each pony In Startup_Ponies
                For Each behavior In pony.Behaviors
                    images.Add(behavior.LeftImagePath)
                    images.Add(behavior.RightImagePath)
                    For Each effect In behavior.Effects
                        images.Add(effect.LeftImagePath)
                        images.Add(effect.RightImagePath)
                    Next
                Next
            Next
            For Each house In HouseBases
                images.Add(house.LeftImagePath)
            Next

            Invoke(Sub()
                       LoadingProgressBar.Value = 0
                       LoadingProgressBar.Maximum = images.Count
                   End Sub)
            Dim imagesLoaded = 0
            Dim loaded = Sub(sender As Object, e As EventArgs)
                             imagesLoaded += 1
                             PonyLoader.ReportProgress(imagesLoaded)
                         End Sub
            ponyViewer.LoadImages(images, loaded)
        End If

        animator = New DesktopPonyAnimator(ponyViewer, Startup_Ponies, OperatingSystemInfo.IsMacOSX)
        Pony.CurrentViewer = ponyViewer
        Pony.CurrentAnimator = animator
    End Sub

    Public Function GetInterface() As ISpriteCollectionView
        'This should already be set in the options, but in case it isn't, use all monitors.
        If Options.MonitorNames.Count = 0 Then
            For Each monitor In Screen.AllScreens
                Options.MonitorNames.Add(monitor.DeviceName)
            Next
        End If

        ' Begin Glue Code
        Dim area = GetCombinedScreenArea()

        Dim viewer As ISpriteCollectionView
        Dim alphaBlending As Boolean = Options.AlphaBlendingEnabled
        If OperatingSystemInfo.IsWindows Then
            viewer = New WinFormSpriteInterface(area, alphaBlending)
        Else
            viewer = New GtkSpriteInterface(alphaBlending)
        End If
        viewer.ShowInTaskbar = Options.ShowInTaskbar
        'End Glue Code

        Return viewer
    End Function

    Private Sub ReturnToMenuOnResolutionChange(sender As Object, e As EventArgs)
        If Not Disposing AndAlso Not IsDisposed Then
            PonyShutdown()
            Main.Instance.Invoke(Sub()
                                     MessageBox.Show("You will be returned to the menu because your screen resolution has changed.",
                                                     "Resolution Changed - Desktop Ponies", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                     Main.Instance.Show()
                                 End Sub)
        End If
    End Sub

    Private Sub PonyLoader_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles PonyLoader.ProgressChanged
        ' Lazy solution to invoking issues and deadlocking the main thread.
        If Not InvokeRequired Then
            LoadingProgressBar.Value = e.ProgressPercentage
        End If
    End Sub

    Private Sub PonyLoader_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles PonyLoader.RunWorkerCompleted
        loading = False
        Dim totalImages = LoadingProgressBar.Maximum

        LoadingProgressBar.Value = 0
        LoadingProgressBar.Maximum = 1
        SelectionControlsPanel.Enabled = True
        If OperatingSystemInfo.IsWindows Then LoadingProgressBar.Visible = False

        Dim oldLoader = PonyLoader
        PonyLoader = New System.ComponentModel.BackgroundWorker() With {
            .Site = oldLoader.Site,
            .WorkerReportsProgress = oldLoader.WorkerReportsProgress,
            .WorkerSupportsCancellation = oldLoader.WorkerSupportsCancellation
        }
        oldLoader.Dispose()

        If Not e.Cancelled Then
            Ponies_Have_Launched = True
            Temp_Save_Counts()
            Visible = False
            animator.Start()
            loadWatch.Stop()
            Console.WriteLine("Loaded in {0:0.00s} ({1} images)", loadWatch.Elapsed.TotalSeconds, totalImages)
        End If
    End Sub
#End Region

    ' ''' <summary>
    ' ''' 'If we are set to auto-start in the command line, try to hide the menu as soon as possible.
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub Main_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
    '    If Not auto_started Then
    '        Me.Opacity = 100
    '    End If
    'End Sub

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

    Friend Sub PonyShutdown()
        If Not IsNothing(animator) Then animator.Finish()
        Ponies_Have_Launched = False
        If Not IsNothing(animator) Then animator.Clear()

        If Not IsNothing(CurrentGame) Then
            CurrentGame.CleanUp()
            CurrentGame = Nothing
        End If

        If screensaverForms IsNot Nothing Then
            For Each screensaverForm In screensaverForms
                screensaverForm.Dispose()
            Next
            screensaverForms = Nothing
        End If

        If Object.ReferenceEquals(animator, Pony.CurrentAnimator) Then
            Pony.CurrentAnimator = Nothing
        End If
        animator = Nothing

        If Not IsNothing(ponyViewer) Then
            ponyViewer.Close()
        End If

        RemoveHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, AddressOf ReturnToMenuOnResolutionChange
    End Sub

    ''' <summary>
    ''' Save pony counts so they are preserved through clicking on and off filters.
    ''' </summary>
    Friend Sub Temp_Save_Counts()
        If PonySelectionPanel.Controls.Count = 0 Then Exit Sub

        Options.PonyCounts.Clear()

        For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
            Dim count As Integer
            Integer.TryParse(ponyPanel.PonyCount.Text, count)
            Options.PonyCounts(ponyPanel.PonyBase.Directory) = count
        Next
    End Sub

    ''' <summary>
    ''' Removes all ponies from the menu.
    ''' </summary>
    Sub DisposeMenu()
        SelectablePonies.Clear()
        SelectionControlsPanel.Enabled = False
        selectionControlFilter.Clear()
        PonySelectionPanel.SuspendLayout()
        For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
            ponyPanel.Dispose()
        Next
        PonySelectionPanel.Controls.Clear()
        PonySelectionPanel.ResumeLayout()
    End Sub

    Friend Sub CleanupSounds()
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

    ''' <summary>
    ''' Put all ponies to sleep... 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub sleep_all()

        If Not all_sleeping Then

            all_sleeping = True

            For Each pony In animator.Ponies()
                'Pony.sleep()
                pony.ShouldBeSleeping = True
            Next

        Else

            all_sleeping = False

            For Each pony In animator.Ponies()
                'Pony.wake_up()
                pony.ShouldBeSleeping = False
            Next
        End If

    End Sub

    Private Sub MonitorNames_CollectionChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)
        screensToUse.Clear()
        screensToUse.AddRange(Screen.AllScreens.Where(Function(screen) Options.MonitorNames.Contains(screen.DeviceName)))
    End Sub

    Friend Function GetScreensToUse() As List(Of Screen)
        Return screensToUse
    End Function

    Friend Function GetCombinedScreenArea() As Rectangle
        Dim area As Rectangle = Rectangle.Empty
        For Each screen In GetScreensToUse()
            If area = Rectangle.Empty Then
                area = screen.WorkingArea
            Else
                area = Rectangle.Union(area, screen.WorkingArea)
            End If
        Next
        Return area
    End Function

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

    Private Sub AnimationTimer_Tick(sender As Object, e As EventArgs) Handles AnimationTimer.Tick
        For Each selectionControl As PonySelectionControl In PonySelectionPanel.Controls
            selectionControl.AdvanceTimeIndex(TimeSpan.FromMilliseconds(AnimationTimer.Interval))
        Next
    End Sub

    Private Sub Main_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        AnimationTimer.Enabled = Visible AndAlso Not loading
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = loading AndAlso Not My.Application.IsFaulted
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            RemoveHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, AddressOf ReturnToMenuOnResolutionChange
            If disposing Then
                If components IsNot Nothing Then components.Dispose()
                If animator IsNot Nothing Then animator.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
End Class