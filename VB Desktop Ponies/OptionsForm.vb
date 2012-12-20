Public Class OptionsForm
    Friend Shared Instance As OptionsForm
    Private Shared alreadyLoaded As Boolean

    Private AvoidanceZonePreviewGraphics As Graphics

    Friend SoundVolume As Integer = -10000
    Friend TimeScaleFactor As Double = 1.0
    Friend ScreenSaverBackgroundColor As Color
    Friend ScreenSaverBackgroundImagePath As String = ""

    Private settingsFileLines As New List(Of String)
    Private selectingMonitors As Boolean

    Private Enum Settings_Categories

        Options = 1
        Monitors = 2
        PonyCount = 3
        FilterCategories = 4

    End Enum

    Private Enum Settings

        Cursor_Avoidance_Size = 1
        Forest_L_X = 2
        Forest_L_Y = 3
        Forest_S_X = 4
        Forest_S_Y = 5
        Max_Ponies = 6
        Pony_Drag = 7
        Pony_Cursor_Avoid = 8
        Pony_Speak_Chance = 9
        Effects_Enabled = 10
        Sounds_Enabled = 11
        Interactions_Enabled = 12
        Window_Avoidance_Enabled = 13
        Speech_Disabled = 14
        Show_Interaction_Errors = 15
        Ponies_Avoid_Ponies = 16
        Ponies_stayin_Boxes = 17
        Pony_Scale = 18
        ScreenSaver_Sounds = 19
        Limit_Sounds = 20
        No_Duplicate_Random_Ponies = 21
        Teleport_Enabled = 22
        ScreenSaver_SolidColor = 23
        ScreenSaver_BGImage = 24
        ScreenSaver_Style = 25
        PoniesAlwaysOnTop = 26
        SuspendOnFullScreen = 27
        Sound_Volume = 28
        Slowdown_Factor = 29

    End Enum

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CreateHandle()

        FirstLoad()
    End Sub

    Private Sub Options_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'FirstLoad()
    End Sub

    Private Sub FirstLoad()
        If alreadyLoaded = True Then Exit Sub

        Instance = Me

        Icon = My.Resources.Twilight

        ScreenSaverBackgroundColor = Nothing
        ScreenSaverBackgroundImagePath = ""
        ScreensaverTransparent.Checked = True
        Teleport.Checked = False

        'setup avoidance area

        CursorAvoidanceRadius.Value = Main.Instance.cursor_zone_size

        AvoidanceZonePreviewImage.Image = New Bitmap(AvoidanceZonePreviewImage.Size.Width, AvoidanceZonePreviewImage.Size.Height)

        AvoidanceZonePreviewGraphics = Graphics.FromImage(AvoidanceZonePreviewImage.Image)


        AvoidanceZoneX.Value = 0 '40
        AvoidanceZoneY.Value = 0 '40
        AvoidanceZoneWidth.Value = 0 '20
        AvoidanceZoneHeight.Value = 0 '20

        'get monitor names

        MonitorsSelection.Items.Clear()

        For Each monitor In Screen.AllScreens
            MonitorsSelection.Items.Add(monitor.DeviceName)
            Main.Instance.screens_to_use.Add(monitor)
        Next

        For i = 0 To MonitorsSelection.Items.Count - 1
            MonitorsSelection.SetSelected(i, True)
        Next

        MaxPonies.Value = 300

        TimeScale.Value = 10
        TimeScaleFactor = 1
        TimeScaleValueLabel.Text = "1x"

        CursorAvoidance.Checked = True
        PonyDragging.Checked = True
        Effects.Checked = True

        If Main.Instance.DisableSoundsDueToDirectXFailure = True Then
            SoundDisabledLabel.Visible = True
            Sound.Enabled = False
            Sound.Checked = False
            ScreensaverSounds.Enabled = False
            ScreensaverSounds.Checked = False
        Else
            SoundDisabledLabel.Visible = False
            Sound.Enabled = True
        End If

        ScreensaverSounds.Checked = True

        AlwaysOnTop.Checked = True

        SoundLimitOnePerPony.Checked = True
        Main.Instance.NoRandomDuplicates = True
        Interactions.Checked = True
        CursorAvoidanceRadius.Value = 100
        WindowAvoidance.Checked = False
        PonySpeechChance.Value = 1
        SpeechDisabled.Checked = False
        InteractionErrorsDisplayed.Checked = False
        PoniesAvoidPonies.Checked = False
        PoniesStayInBoxes.Checked = False
        Window_Avoidance_Enabled_CheckedChanged(Nothing, Nothing)
        Main.Instance.FilterOptionsBox.Items.Clear()

        SetDefaultFilterCategories()

        SizeScale.Value = 100
        SoundVolume = -7500 '~25% volume
        Volume.Value = 650

        ' This option is only available on windows. (And actually broken on Windows at the moment...)
        ' TODO: See if function can be restored, or else removed entirely.
        If True OrElse Not OperatingSystemInfo.IsWindows Then
            SuspendForFullscreenApp.Visible = False
            SuspendForFullscreenApp.Enabled = False
            Options.SuspendForFullscreenApplication = False
        End If

        ' This option causes random crashes on Mac.
        ' TODO: Determine cause of errors - appears to be threading related.
        If OperatingSystemInfo.IsMacOSX Then
            SpeechDisabledLabel.Visible = True
            SpeechDisabled.Checked = True
            SpeechGroup.Enabled = False
            Options.PonySpeechEnabled = False
        End If

        alreadyLoaded = True
    End Sub

    Private Sub Avoidance_Area_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AvoidanceZoneHeight.ValueChanged, AvoidanceZoneWidth.ValueChanged, AvoidanceZoneY.ValueChanged, AvoidanceZoneX.ValueChanged
        Options.ExclusionZone.X = AvoidanceZoneX.Value / 100
        Options.ExclusionZone.Y = AvoidanceZoneY.Value / 100
        Options.ExclusionZone.Width = AvoidanceZoneWidth.Value / 100
        Options.ExclusionZone.Height = AvoidanceZoneHeight.Value / 100
        If Not IsNothing(AvoidanceZonePreviewGraphics) Then
            AvoidanceZonePreviewGraphics.Clear(Color.White)
            AvoidanceZonePreviewGraphics.FillRectangle(
                Brushes.ForestGreen, Options.ExclusionZoneForBounds(Rectangle.Round(AvoidanceZonePreviewGraphics.ClipBounds)))
            AvoidanceZonePreviewImage.Invalidate()
        End If

    End Sub

    Private Sub ScreenSelection_Box_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MonitorsSelection.SelectedIndexChanged
        If selectingMonitors Then Exit Sub

        If MonitorsSelection.SelectedItems.Count = 0 Then
            MonitorsMinimumLabel.Visible = True
            Exit Sub
        Else
            MonitorsMinimumLabel.Visible = False
        End If


        Main.Instance.screens_to_use.Clear()
        Options.MonitorNames.Clear()

        For i = 0 To MonitorsSelection.SelectedItems.Count - 1
            For Each monitor In Screen.AllScreens
                If monitor.DeviceName = CStr(MonitorsSelection.SelectedItems(i)) Then
                    Main.Instance.screens_to_use.Add(monitor)
                    Options.MonitorNames.Add(monitor.DeviceName)
                End If
            Next
        Next

        If IsNothing(Pony.CurrentViewer) Then
            'done
            Exit Sub
        ElseIf TypeOf Pony.CurrentViewer Is SpriteManagement.WinFormSpriteInterface Then
            Dim area As Rectangle = Rectangle.Empty
            For Each screen In Main.Instance.screens_to_use
                area = Rectangle.Union(area, screen.WorkingArea)
            Next
            DirectCast(Pony.CurrentViewer, SpriteManagement.WinFormSpriteInterface).DisplayBounds = area
        End If

    End Sub

    Private Sub SetDefaultFilterCategories()

        Main.Instance.FilterOptionsBox.Items.Clear()

        Main.Instance.FilterOptionsBox.Items.Add("Main Ponies")
        Main.Instance.FilterOptionsBox.Items.Add("Supporting Ponies")
        Main.Instance.FilterOptionsBox.Items.Add("Alternate Art")
        Main.Instance.FilterOptionsBox.Items.Add("Fillies")
        Main.Instance.FilterOptionsBox.Items.Add("Colts")
        Main.Instance.FilterOptionsBox.Items.Add("Pets")
        Main.Instance.FilterOptionsBox.Items.Add("Stallions")
        Main.Instance.FilterOptionsBox.Items.Add("Mares")
        Main.Instance.FilterOptionsBox.Items.Add("Alicorns")
        Main.Instance.FilterOptionsBox.Items.Add("Unicorns")
        Main.Instance.FilterOptionsBox.Items.Add("Pegasi")
        Main.Instance.FilterOptionsBox.Items.Add("Earth Ponies")
        Main.Instance.FilterOptionsBox.Items.Add("Non-Ponies")
        Main.Instance.FilterOptionsBox.Items.Add("Not Tagged")

    End Sub

    Private Sub Cursor_zone_counter_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CursorAvoidanceRadius.ValueChanged
        Main.Instance.cursor_zone_size = CInt(CursorAvoidanceRadius.Value)
        Options.CursorAvoidanceSize = CursorAvoidanceRadius.Value
    End Sub

    Private Sub Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click

        Me.Visible = False

    End Sub

    Friend Sub Save_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs, Optional profile As String = "default") Handles SaveButton.Click

        If MonitorsSelection.SelectedItems.Count = 0 Then
            MsgBox("You need to select at least one monitor.")
            Exit Sub
        End If

        If Not IsNothing(sender) Then
            If Trim(Main.Instance.ProfileComboBox.Text) <> "" Then
                profile = Trim(Main.Instance.ProfileComboBox.Text)
            End If
        End If

        If String.Equals(profile, Options.DefaultProfileName, StringComparison.OrdinalIgnoreCase) Then
            MsgBox("Cannot save over the '" & Options.DefaultProfileName & "' profile. Create a new profile first.",
                   MsgBoxStyle.OkOnly, "Cannot Save")
            Exit Sub
        End If

        Options.SaveProfile(profile)

    End Sub

    Friend Sub Reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs, Optional ByVal counts_only As Boolean = False) Handles ResetButton.Click

        If counts_only = False Then
            alreadyLoaded = False
            Options_Load(Nothing, Nothing)
        End If

        Options.PonyCounts.Clear()

        For Each ponyPanel As PonySelectionControl In Main.Instance.PonySelectionPanel.Controls
            ponyPanel.PonyCount.Text = "1"
        Next

        Scale_Slider_Scroll(Nothing, Nothing)

    End Sub

    Friend Sub Load_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs, Optional selected_profile As String = "default", Optional ByVal silent As Boolean = False) Handles LoadButton.Click
        Try
            If Not IsNothing(sender) Then
                If Trim(Main.Instance.ProfileComboBox.Text) <> "" Then
                    selected_profile = Trim(Main.Instance.ProfileComboBox.Text)
                End If
            End If

            Options.LoadProfile(selected_profile)

            RefreshOptions()
            If Main.Instance.FilterOptionsBox.Items.Count = 0 Then
                SetDefaultFilterCategories()
            End If

            Scale_Slider_Scroll(Nothing, Nothing)

            IO.File.WriteAllText(IO.Path.Combine(Options.InstallLocation, "Profiles", "current.txt"),
                                 selected_profile, System.Text.Encoding.UTF8)
        Catch ex As IO.IOException
            MsgBox("Failed to load profile '" & selected_profile & "'")
        End Try
    End Sub

    Private Sub SelectMonitors(ByVal monitors As List(Of String))
        Argument.EnsureNotNull(monitors, "monitors")

        selectingMonitors = True
        MonitorsSelection.SelectedItems.Clear()

        For Each monitorLoop As String In monitors
            Dim monitor = monitorLoop
            For i = 0 To MonitorsSelection.Items.Count - 1
                If CStr(MonitorsSelection.Items(i)) = monitor Then
                    MonitorsSelection.SetSelected(i, True)
                    Main.Instance.screens_to_use.Add(Array.Find(Screen.AllScreens, Function(screen As Screen)
                                                                                       Return screen.DeviceName = monitor
                                                                                   End Function))
                End If
            Next
        Next

        If MonitorsSelection.SelectedItems.Count = 0 AndAlso MonitorsSelection.Items.Count > 0 Then
            MonitorsSelection.SetSelected(0, True)
        End If
        selectingMonitors = False

    End Sub

    Private Sub Window_Avoidance_Enabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WindowAvoidance.CheckedChanged

        If WindowAvoidance.Checked = False Then
            PoniesAvoidPonies.Enabled = False
            PoniesStayInBoxes.Enabled = False
        Else
            PoniesAvoidPonies.Enabled = True
            PoniesStayInBoxes.Enabled = True
        End If

        Options.WindowAvoidanceEnabled = WindowAvoidance.Checked
    End Sub

    Private Sub Scale_Slider_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SizeScale.ValueChanged
        SizeScaleValueLabel.Text = Math.Round(SizeScale.Value / 100.0F, 2) & "x"
    End Sub

    Private Sub Scale_Slider_MouseUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SizeScale.MouseUp
        Options.ScaleFactor = SizeScale.Value / 100.0F
        Main.Instance.PonySelectionPanel.SuspendLayout()
        For Each control As PonySelectionControl In Main.Instance.PonySelectionPanel.Controls
            control.ResizeToFit()
            control.Invalidate()
        Next
        Main.Instance.PonySelectionPanel.ResumeLayout()
    End Sub

    Private Sub Volume_Control_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Volume.ValueChanged

        'The slider is in %, we need to convert that to the volume that an
        'Microsoft.DirectX.AudioVideoPlayback.Audio.volume would take.
        'which is from -10000 to 0 (0 being the loudest), on a logarithmic scale.

        SoundVolume = CInt(4342 * Math.Log(Volume.Value / 100) - 10000)
        Options.SoundVolume = CSng(Volume.Value / 1000)

        VolumeValueLabel.Text = CStr(Volume.Value / 100)

        If Main.Instance.DisableSoundsDueToDirectXFailure = False Then
            Change_Active_Sound_Volume(SoundVolume)
        End If

    End Sub

    Private Sub Change_Active_Sound_Volume(Volume As Integer)
        For Each activeSound As Microsoft.DirectX.AudioVideoPlayback.Audio In Main.Instance.Active_Sounds
            activeSound.Volume = Volume
        Next
    End Sub

    Private Sub Custom_Filters_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomFiltersButton.Click
        FiltersForm.ShowDialog()
    End Sub

    Private Sub Color_Select_Button_Click(sender As System.Object, e As System.EventArgs) Handles ScreensaverColorButton.Click

        Dim colorpicker As New System.Windows.Forms.ColorDialog

        If colorpicker.ShowDialog() = DialogResult.OK Then
            ScreensaverColorNeededLabel.Visible = False

            ScreenSaverBackgroundColor = colorpicker.Color
            ScreensaverColorNeededLabel.Visible = False
        End If

    End Sub

    Private Sub Screensaver_solidcolor_checkbox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ScreensaverColor.CheckedChanged

        If ScreensaverColor.Checked Then
            If IsNothing(ScreenSaverBackgroundColor) OrElse ScreenSaverBackgroundColor = New Color Then
                ScreensaverColorNeededLabel.Visible = True
            End If
            Options.ScreenSaverBackgroundColor = ScreenSaverBackgroundColor
            Options.ScreenSaverStyle = Options.ScreenSaverBackgroundStyle.SolidColor
        Else
            ScreensaverColorNeededLabel.Visible = False
        End If
    End Sub

    Private Sub Screensaver_use_bgimage_checkbox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ScreensaverImage.CheckedChanged

        If ScreensaverImage.Checked Then
            If ScreenSaverBackgroundImagePath = "" OrElse Not System.IO.File.Exists(ScreenSaverBackgroundImagePath) Then
                ScreensaverImageNeededLabel.Visible = True
            End If
            Options.ScreenSaverBackgroundImagePath = ScreenSaverBackgroundImagePath
            Options.ScreenSaverStyle = Options.ScreenSaverBackgroundStyle.BackgroundImage
        Else
            ScreensaverImageNeededLabel.Visible = False
        End If

    End Sub

    Private Sub Screensaver_background_image_select_button_Click(sender As System.Object, e As System.EventArgs) Handles ScreensaverImageButton.Click

        Dim image_file_selection As New System.Windows.Forms.OpenFileDialog

        image_file_selection.Title = "Select your screensaver background image..."
        image_file_selection.Filter = "GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|All Files (*.*)|*.*"
        image_file_selection.FilterIndex = 4
        image_file_selection.Multiselect = False

        If image_file_selection.ShowDialog() = DialogResult.OK Then

            If System.IO.File.Exists(image_file_selection.FileName) Then

                Try
                    Dim test_image = Image.FromFile(image_file_selection.FileName)
                Catch ex As Exception
                    MsgBox("Error:  Could not load image '" & image_file_selection.FileName & "'.  Details: " & ex.Message)
                    Exit Sub
                End Try

                ScreenSaverBackgroundImagePath = image_file_selection.FileName
                ScreensaverImageNeededLabel.Visible = False
            End If

        End If


    End Sub

    Private Sub PoniesAlwaysOnTop_Checkbox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles AlwaysOnTop.CheckedChanged
        Options.AlwaysOnTop = AlwaysOnTop.Checked
        If Not IsNothing(Pony.CurrentViewer) Then
            Pony.CurrentViewer.Topmost = Options.AlwaysOnTop
        End If
    End Sub

    Private Sub SlowDownFactor_Slider_Scroll(sender As System.Object, e As System.EventArgs) Handles TimeScale.Scroll
        TimeScaleFactor = TimeScale.Value / 10
        TimeScaleValueLabel.Text = TimeScaleFactor & "x"
        Options.TimeFactor = TimeScale.Value / 10.0F
    End Sub

    Friend Sub DeleteProfile(profile_name As String)

        Dim newSettingsFileLines As New List(Of String)

        For Each line In settingsFileLines

            Dim columns = CommaSplitQuoteQualified(line)

            If columns(0)(0) = ControlChars.Tab Then
                Dim savedprofile_name = LCase(Replace(columns(0), ControlChars.Tab, ""))
                If LCase(profile_name) <> savedprofile_name Then
                    newSettingsFileLines.Add(line)
                Else
                    Dim oops = 0
                End If
            End If

        Next

        Options.PonyCounts.Clear()

        settingsFileLines = newSettingsFileLines

    End Sub

    Private Sub RefreshOptions()
        PonySpeechChance.Value = CInt(Options.PonySpeechChance * 100)
        SpeechDisabled.Checked = Not Options.PonySpeechEnabled

        CursorAvoidance.Checked = Options.CursorAvoidanceEnabled
        CursorAvoidanceRadius.Value = CDec(Options.CursorAvoidanceSize)

        PonyDragging.Checked = Options.PonyDraggingEnabled

        Interactions.Checked = Options.PonyInteractionsEnabled
        InteractionsMissingLabel.Visible = Options.PonyInteractionsExist
        'Interactions_error_label.Visible = False
        InteractionErrorsDisplayed.Checked = Options.DisplayPonyInteractionsErrors

        SelectMonitors(Options.MonitorNames)

        SizeScale.Value = CInt(Options.ScaleFactor * 100)
        MaxPonies.Value = Options.MaxPonyCount
        AlphaBlending.Checked = Options.AlphaBlendingEnabled
        Effects.Checked = Options.PonyEffectsEnabled
        WindowAvoidance.Checked = Options.WindowAvoidanceEnabled
        PoniesAvoidPonies.Checked = Options.PonyAvoidsPonies
        PoniesStayInBoxes.Checked = Options.PonyStaysInBox
        Teleport.Checked = Options.PonyTeleportEnabled
        TimeScale.Value = CInt(Options.TimeFactor * 10)
        Sound.Checked = Options.SoundEnabled
        'Sounds_Disabled_Label.Visible = False
        SoundLimitOneGlobally.Checked = Options.SoundSingleChannelOnly
        SoundLimitOnePerPony.Checked = Not Options.SoundSingleChannelOnly
        Volume.Value = CInt(Options.SoundVolume * 1000)
        AlwaysOnTop.Checked = Options.AlwaysOnTop
        SuspendForFullscreenApp.Checked = Options.SuspendForFullscreenApplication
        AvoidanceZoneX.Value = CDec(Options.ExclusionZone.X * 100)
        AvoidanceZoneY.Value = CDec(Options.ExclusionZone.Y * 100)
        AvoidanceZoneWidth.Value = CDec(Options.ExclusionZone.Width * 100)
        AvoidanceZoneHeight.Value = CDec(Options.ExclusionZone.Height * 100)
        ScreensaverSounds.Checked = Options.SoundEnabled

        Select Case Options.ScreenSaverStyle
            Case Options.ScreenSaverBackgroundStyle.Transparent
                ScreensaverTransparent.Checked = True
            Case Options.ScreenSaverBackgroundStyle.SolidColor
                ScreensaverColor.Checked = True
            Case Options.ScreenSaverBackgroundStyle.BackgroundImage
                ScreensaverImage.Checked = True
        End Select

    End Sub

    Private Sub Pony_Speak_Chance_Counter_ValueChanged(sender As System.Object, e As System.EventArgs) Handles PonySpeechChance.ValueChanged
        Options.PonySpeechChance = PonySpeechChance.Value / 100
    End Sub

    Private Sub Disable_Speech_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles SpeechDisabled.CheckedChanged
        Options.PonySpeechEnabled = Not SpeechDisabled.Checked
    End Sub

    Private Sub Cursor_Avoidance_Enabled_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CursorAvoidance.CheckedChanged
        Options.CursorAvoidanceEnabled = CursorAvoidance.Checked
    End Sub

    Private Sub Pony_Dragging_Enabled_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles PonyDragging.CheckedChanged
        Options.PonyDraggingEnabled = PonyDragging.Checked
    End Sub

    Private Sub Interactions_Enabled_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles Interactions.CheckedChanged
        Options.PonyInteractionsEnabled = Interactions.Checked
    End Sub

    Private Sub Interaction_Errors_Displayed_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles InteractionErrorsDisplayed.CheckedChanged
        Options.DisplayPonyInteractionsErrors = InteractionErrorsDisplayed.Checked
    End Sub

    Private Sub Max_Pony_Counter_ValueChanged(sender As System.Object, e As System.EventArgs) Handles MaxPonies.ValueChanged
        Options.MaxPonyCount = CInt(MaxPonies.Value)
    End Sub

    Private Sub Alpha_Blending_Enabled_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles AlphaBlending.CheckedChanged
        Options.AlphaBlendingEnabled = AlphaBlending.Checked
    End Sub

    Private Sub Effects_Enabled_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles Effects.CheckedChanged
        Options.PonyEffectsEnabled = Effects.Checked
    End Sub

    Private Sub PoniesAvoidPonies_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles PoniesAvoidPonies.CheckedChanged
        Options.PonyAvoidsPonies = PoniesAvoidPonies.Checked
    End Sub

    Private Sub PoniesStayInBoxes_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles PoniesStayInBoxes.CheckedChanged
        Options.PonyStaysInBox = PoniesStayInBoxes.Checked
    End Sub

    Private Sub Teleport_Checkbox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles Teleport.CheckedChanged
        Options.PonyTeleportEnabled = Teleport.Checked
    End Sub

    Private Sub Sounds_Enabled_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles Sound.CheckedChanged
        Options.SoundEnabled = Sound.Checked
    End Sub

    Private Sub Sounds_Limit1_Radio_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles SoundLimitOneGlobally.CheckedChanged
        Options.SoundSingleChannelOnly = SoundLimitOneGlobally.Checked
    End Sub

    Private Sub Sounds_Per_Pony_Radio_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles SoundLimitOnePerPony.CheckedChanged
        Options.SoundSingleChannelOnly = Not SoundLimitOnePerPony.Checked
    End Sub

    Private Sub Suspend_on_Fullscreen_Checkbox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles SuspendForFullscreenApp.CheckedChanged
        Options.SuspendForFullscreenApplication = SuspendForFullscreenApp.Checked
    End Sub

    Private Sub ScreenSaver_Sounds_Checkbox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ScreensaverSounds.CheckedChanged
        Options.SoundEnabled = ScreensaverSounds.Checked
    End Sub

    Private Sub Screensaver_Transparent_Checkbox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ScreensaverTransparent.CheckedChanged
        If ScreensaverTransparent.Checked Then
            Options.ScreenSaverStyle = Options.ScreenSaverBackgroundStyle.Transparent
        End If
    End Sub

    Private Sub OptionsForm_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed
        If AvoidanceZonePreviewGraphics IsNot Nothing Then AvoidanceZonePreviewGraphics.Dispose()
    End Sub
End Class