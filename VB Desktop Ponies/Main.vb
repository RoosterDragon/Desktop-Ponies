Imports System.Globalization
Imports System.IO
Imports CsDesktopPonies.SpriteManagement

''' <summary>
''' This is the Main form that handles startup and pony selection.
''' </summary>
Public Class Main
    Friend Shared Instance As Main

#Region "Fields and Properties"
    Private initialized As Boolean = False
    Private loading As Boolean = False

    Friend DesktopHandle As IntPtr
    Friend ShellHandle As IntPtr
    Friend process_id As Integer = 0
    Friend Suspended_For_FullScreenApp As Boolean = False

    Private Animator As DesktopPonyAnimator
    Private PonyViewer As ISpriteCollectionView
    Friend Startup_Ponies As New List(Of Pony)
    Friend SelectablePonies As New List(Of PonyBase)
    Friend Dead_Effects As New List(Of Effect)
    Friend Active_Sounds As New List(Of Object)
    Friend HouseBases As New List(Of HouseBase)

    'Variables used for displaying the pony selection menu
    Private ReadOnly ponyImages As New LazyDictionary(Of String, AnimatedImage(Of BitmapFrame))(
        Function(fileName As String)
            Return New AnimatedImage(Of BitmapFrame)(fileName, Function(file As String) New BitmapFrame(file),
                                                     BitmapFrame.FromBuffer, BitmapFrame.AllowableBitDepths)
        End Function)

    'How big the area around the cursor used for cursor detection should be, in pixels.
    Friend cursor_zone_size As Integer = 100
    'used to tell when we should come out of screensaver mode
    Friend cursor_position As New Point

    'A list of monitors the user has selected to use, from the options screen.
    Friend screens_to_use As New List(Of Screen) From {Screen.PrimaryScreen}

    'Are ponies currently walking around the desktop?
    Friend Ponies_Have_Launched As Boolean = False

    'Is any pony being dragged by the mouse?
    Friend Dragging As Boolean = False
    Friend controlled_pony As String = ""

    Friend Audio_Last_Played As DateTime = DateTime.UtcNow
    Friend Last_Audio_Length As Integer = 0 'milliseconds

    'Used in the editor.
    Friend Preview_Mode As Boolean = False

    'Were we told to auto-start from the command line?
    Friend auto_started As Boolean = False

    Friend screen_saver_mode As Boolean = False
    Friend screen_saver_path As String = ""
    Dim screensaver_settings_file_path As String = Path.Combine(Path.GetTempPath, "DesktopPonies_ScreenSaver_Settings.ini")

    Friend games As New List(Of Game)
    Friend current_game As Game = Nothing

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

    Friend DisableSoundsDueToDirectXFailure As Boolean = False

    Dim all_sleeping As Boolean = False

    Friend Audio_Error_Shown As Boolean = False

    'A temporary list of selected filter settings.
    Dim Temp_Filters As New List(Of String)

    Dim dont_load_profile As Boolean = False
    Dim startup_profile As String = Options.DefaultProfileName

    Private previewWindowRectangle As Func(Of Rectangle)

    Private ReadOnly selectionControlFilter As New Dictionary(Of PonySelectionControl, Boolean)
    Private ponyOffset As Integer
    Private ReadOnly selectionControlsFilteredVisible As IEnumerable(Of PonySelectionControl) =
        selectionControlFilter.Where(Function(kvp) kvp.Value).Select(Function(kvp) kvp.Key)
#End Region

    Enum BehaviorOption
        name = 1
        probability = 2
        max_duration = 3
        min_duration = 4
        speed = 5 'specified in pixels per tick of the timer
        right_image_path = 6
        left_image_path = 7
        movement_type = 8
        linked_behavior = 9
        speaking_start = 10
        speaking_end = 11
        skip = 12 'Should we skip this behavior when considering ones to randomly choose (part of an interaction/chain?)
        xcoord = 13  'used when following/moving to a point on the screen.
        ycoord = 14
        object_to_follow = 15
        auto_select_images = 16
        follow_stopped_behavior = 17
        follow_moving_behavior = 18
        right_image_center = 19
        left_image_center = 20
        dont_repeat_image_animations = 21
        group = 22
    End Enum

    Enum InteractionParameter
        Name = 0
        Initiator = 1  'which pony triggers the interaction?
        Probability = 2
        Proximity = 3
        Target_List = 4
        Target_Selection_Option = 5  'do we interact with only the pony we ran into, or all of them on the list (even multiple instances)
        Behavior_List = 6
        Repeat_Delay = 7
    End Enum

#Region "Initialization"

    Public Sub New()
        InitializeComponent()
        initialized = True
    End Sub

    'Read all configuration files and pony folders.
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Instance = Me

        Dim fileVersionInfo = Diagnostics.FileVersionInfo.GetVersionInfo(Reflection.Assembly.GetExecutingAssembly().Location)
        Dim fileVersion = New Version(fileVersionInfo.FileVersion)
        Dim versionFields As Integer = 1
        If fileVersion.Revision <> 0 Then
            versionFields = 4
        ElseIf fileVersion.Build <> 0 Then
            versionFields = 3
        ElseIf fileVersion.Minor <> 0 Then
            versionFields = 2
        End If

        Text = "Desktop Ponies v" & fileVersion.ToString(versionFields)
        Me.Icon = My.Resources.Twilight

        Application.DoEvents()

        'Unfortunately, some things like tooltips and windows graphics can cause exceptions that are not
        'handled in any try() block.  Catch these and try to show a helpful message to the user.
#If Not Debug Then
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf Unhandled_Exception_Catch
        AddHandler Application.ThreadException, AddressOf ThreadException_Catch
#End If

        'DesktopHandle = DetectFulLScreen_m.GetDesktopWindow()
        'ShellHandle = DetectFulLScreen_m.GetShellWindow()

        'need our own PID for window avoidance (ignoring collisions with other ponies)
        process_id = System.Diagnostics.Process.GetCurrentProcess().Id

        'Try to determine our dependencies and if all are available on this system.
        'Primarily to check to see if the right version of DirectX is installed for sounds.
        Dim self As System.Reflection.Assembly = System.Reflection.Assembly.GetEntryAssembly()

        For Each dependency As System.Reflection.AssemblyName In self.GetReferencedAssemblies()

            If InStr(dependency.ToString, "DirectX") <> 0 Then
                Try
                    System.Reflection.Assembly.Load(dependency)
                Catch ex As Exception
                    DisableSoundsDueToDirectXFailure = True
                End Try
            End If
        Next

        'Fix errors for cultures that don't use , as separator (Russian, others).
        'Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture

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
                        auto_started = True
                        Me.ShowInTaskbar = False
                        ShowInTaskbar = False

                        Try
                            Options.LoadProfile("autostart")
                        Catch
                            Options.LoadDefaultProfile()
                        End Try

                        'windows is telling us "start as a screensaver"
                    Case "/s"
                        Get_ScreenSaver_Path()
                        If screen_saver_path = "" Then Me.Close()
                        Options.InstallLocation = screen_saver_path
                        screen_saver_mode = True
                        auto_started = True
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

        If Not auto_started Then
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

        If loadTemplates Then
            TemplateLoader.RunWorkerAsync()
        End If
    End Sub

    Sub Unhandled_Exception_Catch(sender As Object, e As UnhandledExceptionEventArgs)
        UnhandledException(DirectCast(e.ExceptionObject, Exception))
    End Sub

    Sub ThreadException_Catch(sender As Object, e As System.Threading.ThreadExceptionEventArgs)
        UnhandledException(e.Exception)
    End Sub

    Private Sub UnhandledException(ex As Exception)
        MessageBox.Show("An unexpected error occurred and Desktop Ponies must close. Please report this error so it can be fixed." &
                        vbNewLine & vbNewLine & ex.ToString(), "Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        loading = False
        Application.Exit()
    End Sub

    Sub Get_ScreenSaver_Path()
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

            Dim SettingsFile As New System.IO.StreamReader(screensaver_settings_file_path)

            screen_saver_path = SettingsFile.ReadLine()

            SettingsFile.Close()
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
                    Using existing_file As New System.IO.StreamReader(screensaver_settings_file_path)
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
            Dim SettingsFile As New System.IO.StreamWriter(screensaver_settings_file_path, False, System.Text.Encoding.UTF8)

            SettingsFile.WriteLine(screen_saver_path)

            SettingsFile.Close()
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

    Private Sub TemplateLoader_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles TemplateLoader.DoWork
        Try
            Dim ponyBaseDirectories = Directory.GetDirectories(Path.Combine(Options.InstallLocation, PonyBase.RootDirectory))

            Dim skipLoadingErrors As Boolean = False

            Dim ponyBasesToAdd As New List(Of PonyBase)

            Dim foldersLoaded = 0
            While Not IsHandleCreated
            End While

            Invoke(Sub()
                       LoadingProgressBar.Maximum = ponyBaseDirectories.Count
                   End Sub)

            For Each folder In Directory.GetDirectories(Path.Combine(Options.InstallLocation, HouseBase.RootDirectory))
                skipLoadingErrors = LoadHouse(folder, skipLoadingErrors)
            Next

            For Each folder In ponyBaseDirectories
                Try
                    Dim pony = New PonyBase(folder)
                    ponyBasesToAdd.Add(pony)
                    Invoke(Sub()
                               Add_to_Menu(pony, False)
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
                foldersLoaded += 1
                TemplateLoader.ReportProgress(foldersLoaded)
            Next

            ' Sort loaded ponies and add them to the panel.
            'Ponies_to_add.Sort(Function(a, b) a.Name.CompareTo(b.Name))
            'For Each loopPony In Ponies_to_add
            '    Dim pony = loopPony
            '    Invoke(Sub()
            '               Add_to_Menu(pony, False)
            '           End Sub)
            'Next

            If SelectablePonies.Count = 0 Then
                MsgBox("Sorry, but you don't seem to have any ponies installed.  There should have at least been a 'Derpy' folder in the same spot as this program.")
                GoButton.Enabled = False
            End If

            'Load pony counts.
            Main.Instance.Invoke(Sub()
                                     Options.LoadPonyCounts()
                                 End Sub)

            'We first load interactions to get a list of names 
            'that each pony should interact with.
            'Latter, we "initialize" interactions to get
            'a list of each pony object.
            Try
                If Options.PonyInteractionsEnabled Then
                    Dim displaywarnings =
                        Options.DisplayPonyInteractionsErrors AndAlso Not auto_started AndAlso Not screen_saver_mode
                    Main.Instance.Invoke(Sub()
                                             LoadInteractions(displaywarnings)
                                         End Sub)
                End If
            Catch ex As Exception
                MsgBox("Unable to load interactions.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
            End Try

        Catch ex As Exception
#If Not Debug Then
            MsgBox("Error starting up!  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
            Exit Sub
#Else
            Throw
#End If
        End Try
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

    Friend Function GetDirection(setting As String) As Directions

        Select Case setting

            Case "top"
                Return Directions.top
            Case "bottom"
                Return Directions.bottom
            Case "left"
                Return Directions.left
            Case "right"
                Return Directions.right
            Case "bottom_right"
                Return Directions.bottom_right
            Case "bottom_left"
                Return Directions.bottom_left
            Case "top_right"
                Return Directions.top_right
            Case "top_left"
                Return Directions.top_left
            Case "center"
                Return Directions.center
            Case "any"
                Return Directions.random
            Case "any-not_center"
                Return Directions.random_not_center
            Case Else
                Throw New ArgumentException("Invalid placement direction or centering for effect.", "setting")
        End Select

    End Function

    Private Sub Add_to_Menu(ponyBase As PonyBase, redraw As Boolean)

        If Not redraw Then
            SelectablePonies.Add(ponyBase)
        End If

        'don't show ponies that don't have at least one of the desired tags in Show Any.. mode
        If FilterAnyRadio.Checked Then
            Dim match = False
            For Each tag_to_show As String In FilterOptionsBox.CheckedItems
                If ponyBase.Tags.Contains(tag_to_show) OrElse
                   (ponyBase.Tags.Count = 0 AndAlso tag_to_show = "Not Tagged") Then
                    match = True
                    Exit For
                End If
            Next
            If match = False Then
                Exit Sub
            End If
        End If

        'don't show ponies that don't have all of the desired tags in Show Exactly.. mode
        If FilterExactlyRadio.Checked Then
            Dim all_match = False
            For Each tag_to_show As String In FilterOptionsBox.CheckedItems
                Dim match = False
                If ponyBase.Tags.Contains(tag_to_show) Then
                    match = True
                    all_match = True
                End If
                If ponyBase.Tags.Count = 0 AndAlso tag_to_show = "Not Tagged" Then
                    match = True
                    all_match = True
                End If
                If Not match Then
                    Exit Sub
                End If
            Next
            If Not all_match Then
                Exit Sub
            End If
        End If

        Dim ponyImageName = ponyBase.Behaviors(0).RightImagePath
        ponyImages.Add(ponyImageName)

        Dim ponySelection As New PonySelectionControl(ponyBase, ponyImages(ponyImageName), False)
        AddHandler ponySelection.PonyCount.TextChanged, AddressOf HandleCountChange
        If ponyBase.Directory = "Random Pony" Then
            ponySelection.NoDuplicates.Visible = True
            ponySelection.NoDuplicates.Checked = NoRandomDuplicates
            AddHandler ponySelection.NoDuplicates.CheckedChanged, Sub()
                                                                      NoRandomDuplicates = ponySelection.NoDuplicates.Checked
                                                                  End Sub
        End If
        If OperatingSystemInfo.IsMacOSX Then ponySelection.Visible = False

        PonySelectionPanel.Controls.Add(ponySelection)
        selectionControlFilter.Add(ponySelection, True)
    End Sub

    Sub LoadInteractions(Optional displayWarnings As Boolean = True)

        If Not My.Computer.FileSystem.FileExists(System.IO.Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, PonyBase.Interaction.ConfigFilename)) Then
            Options.PonyInteractionsExist = False
            Exit Sub
        End If

        Using interactions_file As New System.IO.StreamReader(
            System.IO.Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, PonyBase.Interaction.ConfigFilename))
            Do Until interactions_file.EndOfStream

                Dim line = interactions_file.ReadLine

                If InStr(line, "'") = 1 OrElse Trim(line) = "" Then Continue Do

                Dim columns = CommaSplitBraceQualified(line)

                Dim ponyfound = False

                Dim ponyname = Trim(LCase(CommaSplitQuoteQualified(columns(InteractionParameter.Initiator))(0)))
                For Each Pony In SelectablePonies
                    Try
                        If Trim(LCase(Pony.Directory)) = ponyname Then

                            ponyfound = True

                            Dim repeat_delay = 60

                            If UBound(columns) >= InteractionParameter.Repeat_Delay Then
                                repeat_delay = Integer.Parse(columns(InteractionParameter.Repeat_Delay), CultureInfo.InvariantCulture)
                            End If

                            Dim targetsActivated As PonyBase.Interaction.TargetActivation
                            If Not [Enum].TryParse(Trim(columns(InteractionParameter.Target_Selection_Option)), targetsActivated) Then
                                If Not Main.Instance.screen_saver_mode Then
                                    Throw New ArgumentException("Invalid option for target selection. Use either 'One', 'Any' or 'All'." _
                                                        & ControlChars.NewLine & " Interaction file specified: " & columns(InteractionParameter.Target_Selection_Option) & _
                                                        " for interaction named: " & columns(InteractionParameter.Name), "target_selection")
                                End If
                            End If

                            Pony.AddInteraction(columns(InteractionParameter.Name), _
                                            ponyname, _
                                            Double.Parse(columns(InteractionParameter.Probability), CultureInfo.InvariantCulture), _
                                            columns(InteractionParameter.Proximity), _
                                            columns(InteractionParameter.Target_List), _
                                            targetsActivated, _
                                            columns(InteractionParameter.Behavior_List), _
                                            repeat_delay, _
                                            displayWarnings)

                        End If
                    Catch ex As Exception
                        If displayWarnings Then
                            MsgBox("Error loading interaction for Pony: " & Pony.Directory & _
                             ControlChars.NewLine & line & ControlChars.NewLine & _
                             ex.Message)
                        End If
                    End Try
                Next

                If ponyfound = False Then
                    If displayWarnings Then
                        MsgBox("Warning:  Interaction specifies a non-existent pony: " & _
                               line)
                    End If
                End If

            Loop
        End Using
    End Sub

    Private Sub TemplateLoader_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles TemplateLoader.ProgressChanged
        LoadingProgressBar.Value = e.ProgressPercentage
    End Sub

    Private Sub TemplateLoader_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles TemplateLoader.RunWorkerCompleted
        TemplateLoader.Dispose()

        If auto_started = True Then
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
        PonyPaginationLabel.Text = String.Format("Viewing {0} ponies", PonySelectionPanel.Controls.Count)
        PaginationEnabled.Enabled = True
        PaginationEnabled.Checked = OperatingSystemInfo.IsMacOSX

        PonySelectionPanel.Enabled = True
        SelectionControlsPanel.Enabled = True
        AnimationTimer.Enabled = True
        loading = False
        GC.Collect()
    End Sub

    Private Sub HandleCountChange(sender As Object, e As EventArgs)
        CountSelectedPonies()
    End Sub

    Private Sub CountSelectedPonies()

        Dim total_ponies As Integer = 0

        For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
            Dim count As Integer
            If Integer.TryParse(ponyPanel.PonyCount.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, count) Then
                total_ponies += count
            End If
        Next

        PonyCountValueLabel.Text = CStr(total_ponies)

    End Sub
#End Region

#Region "Selection"
    ''' <summary>
    ''' Set each pony's count to 0
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub ZeroPoniesButton_Click(sender As Object, e As EventArgs) Handles ZeroPoniesButton.Click
        For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
            ponyPanel.PonyCount.Text = "0"
        Next
    End Sub

    Private Sub SaveProfileButton_Click(sender As Object, e As EventArgs) Handles SaveProfileButton.Click
        Dim profileToSave = Trim(ProfileComboBox.Text)

        If profileToSave = "" Then
            MsgBox("Enter a profile name first!")
            Exit Sub
        End If

        If String.Equals(profileToSave, Options.DefaultProfileName, StringComparison.OrdinalIgnoreCase) Then
            MsgBox("Cannot save over the '" & Options.DefaultProfileName & "' profile")
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
        OptionsForm.Instance.Load_Button_Click(sender, e, ProfileComboBox.Text)
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

        Preview_Mode = True
        Me.Visible = False
        Using form = New PonyEditor()
            previewWindowRectangle = AddressOf form.GetPreviewWindowScreenRectangle
            form.ShowDialog(Me)

            Pony_Shutdown()

            Preview_Mode = False
            If Not Me.IsDisposed Then
                Me.Visible = True
            End If

            OptionsForm.Instance.Hide()

            If form.changes_made Then
                DisposeMenu()
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

            Dim Install_Folder = Directory.GetDirectories(Path.Combine(Options.InstallLocation, Game.RootDirectory))

            For Each folder In Install_Folder

                Try
                    'some languages don't use \ as a separator between folders
                    Dim config_file_name = Path.Combine(folder, Game.ConfigFilename)

                    Dim new_game As New Game(config_file_name, folder & System.IO.Path.DirectorySeparatorChar)

                    games.Add(new_game)

                Catch ex As Exception
                    MsgBox("Error loading game: " & folder & ex.Message & ex.StackTrace)
                End Try
            Next

            Me.Visible = False
            If GameSelectionForm.ShowDialog() = DialogResult.OK Then
                Startup_Ponies.Clear()
                Pony_Startup()
                current_game.Setup()
                Animator.Begin()
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
            OptionsForm.Instance.Load_Button_Click(sender, e, Trim(ProfileComboBox.Text))
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

        Dim localOffset = -1
        Dim visibleCount = 0
        For Each selectionControl As PonySelectionControl In PonySelectionPanel.Controls
            If Not PaginationEnabled.Checked Then
                Dim makeVisible = selectionControlFilter(selectionControl)
                If makeVisible Then visibleCount += 1
                selectionControl.Visible = makeVisible
            Else
                Dim makeVisible = False
                If selectionControlFilter(selectionControl) Then
                    localOffset += 1
                    Dim inPageRange = True
                    If localOffset < ponyOffset Then inPageRange = False
                    If visibleCount >= PoniesPerPage.Value Then inPageRange = False
                    makeVisible = inPageRange
                End If
                If makeVisible Then visibleCount += 1
                selectionControl.Visible = makeVisible
            End If
        Next

        PonySelectionPanel.ResumeLayout()

        If Not PaginationEnabled.Checked OrElse visibleCount = 0 Then
            PonyPaginationLabel.Text = String.Format("Viewing {0} ponies", visibleCount)
        Else
            PonyPaginationLabel.Text =
            String.Format("Viewing {0} to {1} of {2} ponies",
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

        Dim character = e.KeyChar
        If Char.IsLetter(character) Then
            For Each selectionControl In selectionControlsFilteredVisible
                Dim compare = String.Compare(selectionControl.PonyName.Text(0), character, StringComparison.OrdinalIgnoreCase)
                If compare = 0 Then
                    PonySelectionPanel.ScrollControlIntoView(selectionControl)
                    selectionControl.PonyCount.Focus()
                    e.Handled = True
                End If
                If compare >= 0 Then Exit Sub
            Next
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
                If screen_saver_mode Then
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

            Pony_Startup()
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

    Friend Sub Pony_Startup()
        If screen_saver_mode Then
            If OptionsForm.Instance.ScreensaverTransparent.Checked = False Then
                For Each monitor In Screen.AllScreens
                    Dim screensaver_background As New ScreensaverBackgroundForm
                    screensaver_background.Show()

                    If IsNothing(Options.ScreenSaverBackgroundColor) Then
                        screensaver_background.BackColor = Color.Black
                    Else
                        screensaver_background.BackColor = Options.ScreenSaverBackgroundColor
                    End If

                    If Options.ScreenSaverBackgroundImagePath <> "" AndAlso
                        Options.ScreenSaverStyle = Options.ScreenSaverBackgroundStyle.BackgroundImage Then
                        Try
                            screensaver_background.BackgroundImage = Image.FromFile(Options.ScreenSaverBackgroundImagePath)
                        Catch ex As Exception
                            'could not load the image
                        End Try
                    End If

                    screensaver_background.Size = monitor.Bounds.Size
                    screensaver_background.Location = monitor.Bounds.Location
                Next
            End If
            Cursor.Hide()
        End If

        PonyViewer = GetInterface()
        PonyViewer.Topmost = Options.AlwaysOnTop

        If Not Preview_Mode Then
            ' Get a collection of all images to be loaded.
            Dim images As LinkedList(Of String) = New LinkedList(Of String)()
            For Each pony In Startup_Ponies
                For Each behavior In pony.Behaviors
                    images.AddLast(behavior.LeftImagePath)
                    images.AddLast(behavior.RightImagePath)
                    For Each effect In behavior.Effects
                        images.AddLast(effect.left_image_path)
                        images.AddLast(effect.right_image_path)
                    Next
                Next
            Next
            For Each house In HouseBases
                images.AddLast(house.ImageFilename)
            Next

            Dim imagesToLoad = images.Distinct(StringComparer.Ordinal)

            Invoke(Sub()
                       LoadingProgressBar.Value = 0
                       LoadingProgressBar.Maximum = imagesToLoad.Count
                   End Sub)
            Dim imagesLoaded = 0
            Dim loaded = New EventHandler(Sub(ilSender As Object, ilE As EventArgs)
                                              imagesLoaded += 1
                                              PonyLoader.ReportProgress(imagesLoaded)
                                          End Sub)
            PonyViewer.LoadImages(imagesToLoad, loaded)
        End If

        Animator = New DesktopPonyAnimator(PonyViewer, Startup_Ponies, OperatingSystemInfo.IsMacOSX)
        Pony.CurrentViewer = PonyViewer
        Pony.CurrentAnimator = Animator
    End Sub

    Public Function GetInterface() As ISpriteCollectionView
        'In case the cursor size wasn't set by the options menu, set it to something based on the screen size.
        If cursor_zone_size = Nothing Then
            For Each monitor In Screen.AllScreens
                cursor_zone_size = CInt(0.03 * monitor.WorkingArea.Height)
                Exit For
            Next
        End If

        'This should already be set in the options, but in case it isn't, use all monitors.
        If screens_to_use.Count = 0 Then
            For Each monitor In Screen.AllScreens
                screens_to_use.Add(monitor)
                cursor_zone_size = CInt(0.03 * monitor.WorkingArea.Height)
            Next
        End If

        ' Begin Glue Code
        Dim area As Rectangle = Rectangle.Empty
        For Each screen In screens_to_use
            area = Rectangle.Union(area, screen.WorkingArea)
        Next

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

    Private Sub PonyLoader_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles PonyLoader.ProgressChanged
        ' Lazy solution to invoking issues and deadlocking the main thread.
        If Not InvokeRequired Then
            LoadingProgressBar.Value = e.ProgressPercentage
        End If
    End Sub

    Private Sub PonyLoader_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles PonyLoader.RunWorkerCompleted
        loading = False

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
            Animator.Begin()

            ' Hide the menu form now.
            Me.Visible = False
            Temp_Save_Counts()

            GC.Collect()
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
            For Each selectionControl As PonySelectionControl In PonySelectionPanel.Controls
                selectionControl.Visible = selectionControlFilter(selectionControl)
            Next
        End If
    End Sub

    Friend Sub Pony_Shutdown()
        If Not IsNothing(Animator) Then Animator.Finish()
        Ponies_Have_Launched = False
        If Not IsNothing(Animator) Then Animator.Clear()

        If Not IsNothing(current_game) Then
            current_game.CleanUp()
            current_game = Nothing
        End If

        If Object.ReferenceEquals(Animator, Pony.CurrentAnimator) Then
            Pony.CurrentAnimator = Nothing
        End If
        Animator = Nothing

        If Not IsNothing(PonyViewer) Then
            PonyViewer.Close()
        End If
    End Sub

    Friend Sub Redraw_Menu()

        Temp_Save_Counts()

        DisposeMenu()

        PonySelectionPanel.Visible = False
        For Each Pony In SelectablePonies
            Add_to_Menu(Pony, True)
        Next
        PonySelectionPanel.Visible = True

        Options.LoadPonyCounts()

    End Sub

    ''' <summary>
    ''' Save pony counts so they are preserved through clicking on and off filters.
    ''' </summary>
    ''' <remarks></remarks>
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
    ''' Even when it is not visible, animations on the main form still take CPU.  This sub disposes all of the controls 
    ''' to bring its CPU usage back down to 0.
    ''' </summary>
    ''' <remarks></remarks>
    Sub DisposeMenu()
        SelectionControlsPanel.Enabled = False
        For Each ponyPanel As PonySelectionControl In PonySelectionPanel.Controls
            ponyPanel.Dispose()
        Next
        PonySelectionPanel.Controls.Clear()
    End Sub

    Friend Sub Cleanup_Sounds()

        Dim sounds_to_remove As New List(Of Microsoft.DirectX.AudioVideoPlayback.Audio)

        For Each sound As Microsoft.DirectX.AudioVideoPlayback.Audio In Active_Sounds
            If sound.State = Microsoft.DirectX.AudioVideoPlayback.StateFlags.Paused OrElse sound.Duration = sound.CurrentPosition Then
                sound.Dispose()
                sounds_to_remove.Add(sound)
            End If
        Next

        For Each sound In sounds_to_remove
            Active_Sounds.Remove(sound)
        Next

    End Sub

    ''' <summary>
    ''' Put all ponies to sleep... 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub sleep_all()

        If Not all_sleeping Then

            all_sleeping = True

            For Each pony In Animator.Ponies()
                'Pony.sleep()
                pony.should_be_sleeping = True
            Next

        Else

            all_sleeping = False

            For Each pony In Animator.Ponies()
                'Pony.wake_up()
                pony.should_be_sleeping = False
            Next
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
        e.Cancel = loading
    End Sub

    Private Sub Main_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed
        For Each kvp In ponyImages.InitializedItems
            If kvp.Value IsNot Nothing Then
                kvp.Value.Dispose()
            Else
                Console.WriteLine("Main_Disposed encountered a null image. Key: " & kvp.Key)
            End If
        Next
    End Sub
End Class