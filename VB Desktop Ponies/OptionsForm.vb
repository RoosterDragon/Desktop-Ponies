Public Class OptionsForm
    Friend Shared Instance As OptionsForm
    Private Shared alreadyLoaded As Boolean

    Friend SoundVolume As Integer
    Friend TimeScaleFactor As Double = 1.0
    Friend ScreenSaverBackgroundColor As Color
    Friend ScreenSaverBackgroundImagePath As String = ""

    Private settingsFileLines As New List(Of String)
    Private selectingMonitors As Boolean
    Private avoidanceZonePreviewGraphics As Graphics

    Public Sub New()
        InitializeComponent()
        CreateHandle()
        FirstLoad()
    End Sub

    Private Sub FirstLoad()
        If alreadyLoaded Then Exit Sub

        Instance = Me
        Icon = My.Resources.Twilight

        'setup avoidance area
        CursorAvoidanceRadius.Value = Main.Instance.cursor_zone_size

        AvoidanceZonePreview.Image = New Bitmap(AvoidanceZonePreview.Size.Width, AvoidanceZonePreview.Size.Height)
        avoidanceZonePreviewGraphics = Graphics.FromImage(AvoidanceZonePreview.Image)

        'get monitor names
        For Each monitor In Screen.AllScreens
            MonitorsSelection.Items.Add(monitor.DeviceName)
            Main.Instance.screens_to_use.Add(monitor)
        Next

        For i = 0 To MonitorsSelection.Items.Count - 1
            MonitorsSelection.SetSelected(i, True)
        Next

        If Not Main.Instance.DirectXSoundAvailable Then
            SoundDisabledLabel.Visible = True
            Sound.Enabled = False
            Sound.Checked = False
            ScreensaverSounds.Enabled = False
            ScreensaverSounds.Checked = False
        Else
            SoundDisabledLabel.Visible = False
            Sound.Enabled = True
        End If

        Main.Instance.NoRandomDuplicates = True

        SetDefaultFilterCategories()

        ' Set initial volume value, event handler will update values as needed.
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

    Private Sub SelectMonitors(monitors As List(Of String))
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

    Friend Sub LoadButton_Click(sender As Object, e As EventArgs, Optional profile As String = Options.DefaultProfileName) Handles LoadButton.Click
        Try
            If Not IsNothing(sender) Then
                If Trim(Main.Instance.ProfileComboBox.Text) <> "" Then
                    profile = Trim(Main.Instance.ProfileComboBox.Text)
                End If
            End If

            Options.LoadProfile(profile)

            RefreshOptions()
            If Main.Instance.FilterOptionsBox.Items.Count = 0 Then
                SetDefaultFilterCategories()
            End If

            SizeScale_ValueChanged(Nothing, Nothing)
        Catch ex As IO.IOException
            MsgBox("Failed to load profile '" & profile & "'")
        End Try
        Try
            IO.File.WriteAllText(IO.Path.Combine(Options.ProfileDirectory, "current.txt"),
                     profile, System.Text.Encoding.UTF8)
        Catch ex As IO.IOException
            ' If we cannot write out the file that remembers the last used profile, that is unfortunate but not a fatal problem.
            Console.WriteLine("Warning: Failed to save current.txt file.")
        End Try
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        If MonitorsSelection.SelectedItems.Count = 0 Then
            MsgBox("You need to select at least one monitor.")
            Exit Sub
        End If

        Dim profile = Options.DefaultProfileName

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

    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        alreadyLoaded = False

        Options.PonyCounts.Clear()

        For Each ponyPanel As PonySelectionControl In Main.Instance.PonySelectionPanel.Controls
            ponyPanel.PonyCount.Text = "0"
        Next
    End Sub

    Private Sub AvoidanceZoneArea_ValueChanged(sender As Object, e As EventArgs) Handles AvoidanceZoneHeight.ValueChanged, AvoidanceZoneWidth.ValueChanged, AvoidanceZoneY.ValueChanged, AvoidanceZoneX.ValueChanged
        Options.ExclusionZone.X = AvoidanceZoneX.Value / 100
        Options.ExclusionZone.Y = AvoidanceZoneY.Value / 100
        Options.ExclusionZone.Width = AvoidanceZoneWidth.Value / 100
        Options.ExclusionZone.Height = AvoidanceZoneHeight.Value / 100
        If Not IsNothing(avoidanceZonePreviewGraphics) Then
            avoidanceZonePreviewGraphics.Clear(Color.White)
            avoidanceZonePreviewGraphics.FillRectangle(
                Brushes.ForestGreen, Options.ExclusionZoneForBounds(Rectangle.Round(avoidanceZonePreviewGraphics.VisibleClipBounds)))
            AvoidanceZonePreview.Invalidate()
            AvoidanceZonePreview.Update()
        End If
    End Sub

    Private Sub MonitorsSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MonitorsSelection.SelectedIndexChanged
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

    Private Sub CursorAvoidanceRadius_ValueChanged(sender As Object, e As EventArgs) Handles CursorAvoidanceRadius.ValueChanged
        Main.Instance.cursor_zone_size = CInt(CursorAvoidanceRadius.Value)
        Options.CursorAvoidanceSize = CursorAvoidanceRadius.Value
    End Sub

    Private Sub WindowAvoidance_CheckedChanged(sender As Object, e As EventArgs) Handles WindowAvoidance.CheckedChanged
        PoniesAvoidPonies.Enabled = WindowAvoidance.Checked
        PoniesStayInBoxes.Enabled = WindowAvoidance.Checked
        Options.WindowAvoidanceEnabled = WindowAvoidance.Checked
    End Sub

    Private Sub SizeScale_ValueChanged(sender As Object, e As EventArgs) Handles SizeScale.ValueChanged
        SizeScaleValueLabel.Text = Math.Round(SizeScale.Value / 100.0F, 2) & "x"
    End Sub

    Private Sub SizeScale_MouseUp(sender As Object, e As EventArgs) Handles SizeScale.MouseUp
        Options.ScaleFactor = SizeScale.Value / 100.0F
        Main.Instance.PonySelectionPanel.SuspendLayout()
        For Each control As PonySelectionControl In Main.Instance.PonySelectionPanel.Controls
            control.ResizeToFit()
            control.Invalidate()
        Next
        Main.Instance.PonySelectionPanel.ResumeLayout()
    End Sub

    Private Sub Volume_ValueChanged(sender As Object, e As EventArgs) Handles Volume.ValueChanged

        'The slider is in %, we need to convert that to the volume that an
        'Microsoft.DirectX.AudioVideoPlayback.Audio.volume would take.
        'which is from -10000 to 0 (0 being the loudest), on a logarithmic scale.

        SoundVolume = CInt(4342 * Math.Log(Volume.Value / 100) - 10000)
        Options.SoundVolume = CSng(Volume.Value / 1000)

        VolumeValueLabel.Text = CStr(Volume.Value / 100)

        If Main.Instance.DirectXSoundAvailable Then
            Main.Instance.SetVolumeOnAllSounds(SoundVolume)
        End If
    End Sub

    Private Sub CustomFiltersButton_Click(sender As Object, e As EventArgs) Handles CustomFiltersButton.Click
        FiltersForm.ShowDialog()
    End Sub

    Private Sub ScreensaverColorButton_Click(sender As Object, e As EventArgs) Handles ScreensaverColorButton.Click
        Using dialog As New ColorDialog
            If dialog.ShowDialog() = DialogResult.OK Then
                ScreenSaverBackgroundColor = dialog.Color
                ScreensaverColorNeededLabel.Visible = False
            End If
        End Using
    End Sub

    Private Sub ScreensaverColor_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverColor.CheckedChanged
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

    Private Sub ScreensaverImage_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverImage.CheckedChanged
        If ScreensaverImage.Checked Then
            If ScreenSaverBackgroundImagePath = "" OrElse Not IO.File.Exists(ScreenSaverBackgroundImagePath) Then
                ScreensaverImageNeededLabel.Visible = True
            End If
            Options.ScreenSaverBackgroundImagePath = ScreenSaverBackgroundImagePath
            Options.ScreenSaverStyle = Options.ScreenSaverBackgroundStyle.BackgroundImage
        Else
            ScreensaverImageNeededLabel.Visible = False
        End If
    End Sub

    Private Sub ScreensaverImageButton_Click(sender As Object, e As EventArgs) Handles ScreensaverImageButton.Click
        Using dialog As New OpenFileDialog
            dialog.Title = "Select your screensaver background image..."
            dialog.Filter = "GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|All Files (*.*)|*.*"
            dialog.FilterIndex = 4
            dialog.Multiselect = False

            If dialog.ShowDialog() = DialogResult.OK Then
                If IO.File.Exists(dialog.FileName) Then
                    Try
                        Dim test_image = Image.FromFile(dialog.FileName)
                    Catch ex As Exception
                        MsgBox("Error:  Could not load image '" & dialog.FileName & "'.  Details: " & ex.Message)
                        Exit Sub
                    End Try

                    ScreenSaverBackgroundImagePath = dialog.FileName
                    ScreensaverImageNeededLabel.Visible = False
                End If
            End If
        End Using
    End Sub

    Private Sub AlwaysOnTop_CheckedChanged(sender As Object, e As EventArgs) Handles AlwaysOnTop.CheckedChanged
        Options.AlwaysOnTop = AlwaysOnTop.Checked
        If Not IsNothing(Pony.CurrentViewer) Then
            Pony.CurrentViewer.Topmost = Options.AlwaysOnTop
        End If
    End Sub

    Private Sub TimeScale_Scroll(sender As Object, e As EventArgs) Handles TimeScale.Scroll
        TimeScaleFactor = TimeScale.Value / 10
        TimeScaleValueLabel.Text = TimeScaleFactor & "x"
        Options.TimeFactor = TimeScale.Value / 10.0F
    End Sub

    Private Sub PonySpeechChance_ValueChanged(sender As Object, e As EventArgs) Handles PonySpeechChance.ValueChanged
        Options.PonySpeechChance = PonySpeechChance.Value / 100
    End Sub

    Private Sub SpeechDisabled_CheckedChanged(sender As Object, e As EventArgs) Handles SpeechDisabled.CheckedChanged
        Options.PonySpeechEnabled = Not SpeechDisabled.Checked
    End Sub

    Private Sub CursorAvoidance_CheckedChanged(sender As Object, e As EventArgs) Handles CursorAvoidance.CheckedChanged
        Options.CursorAvoidanceEnabled = CursorAvoidance.Checked
    End Sub

    Private Sub PonyDragging_CheckedChanged(sender As Object, e As EventArgs) Handles PonyDragging.CheckedChanged
        Options.PonyDraggingEnabled = PonyDragging.Checked
    End Sub

    Private Sub Interactions_CheckedChanged(sender As Object, e As EventArgs) Handles Interactions.CheckedChanged
        Options.PonyInteractionsEnabled = Interactions.Checked
    End Sub

    Private Sub InteractionErrorsDisplayed_CheckedChanged(sender As Object, e As EventArgs) Handles InteractionErrorsDisplayed.CheckedChanged
        Options.DisplayPonyInteractionsErrors = InteractionErrorsDisplayed.Checked
    End Sub

    Private Sub MaxPonies_ValueChanged(sender As Object, e As EventArgs) Handles MaxPonies.ValueChanged
        Options.MaxPonyCount = CInt(MaxPonies.Value)
    End Sub

    Private Sub AlphaBlending_CheckedChanged(sender As Object, e As EventArgs) Handles AlphaBlending.CheckedChanged
        Options.AlphaBlendingEnabled = AlphaBlending.Checked
    End Sub

    Private Sub Effects_CheckedChanged(sender As Object, e As EventArgs) Handles Effects.CheckedChanged
        Options.PonyEffectsEnabled = Effects.Checked
    End Sub

    Private Sub PoniesAvoidPonies_CheckedChanged(sender As Object, e As EventArgs) Handles PoniesAvoidPonies.CheckedChanged
        Options.PonyAvoidsPonies = PoniesAvoidPonies.Checked
    End Sub

    Private Sub PoniesStayInBoxes_CheckedChanged(sender As Object, e As EventArgs) Handles PoniesStayInBoxes.CheckedChanged
        Options.PonyStaysInBox = PoniesStayInBoxes.Checked
    End Sub

    Private Sub Teleport_CheckedChanged(sender As Object, e As EventArgs) Handles Teleport.CheckedChanged
        Options.PonyTeleportEnabled = Teleport.Checked
    End Sub

    Private Sub Sound_CheckedChanged(sender As Object, e As EventArgs) Handles Sound.CheckedChanged
        Options.SoundEnabled = Sound.Checked
    End Sub

    Private Sub SoundLimitOneGlobally_CheckedChanged(sender As Object, e As EventArgs) Handles SoundLimitOneGlobally.CheckedChanged
        Options.SoundSingleChannelOnly = SoundLimitOneGlobally.Checked
    End Sub

    Private Sub SoundLimitOnePerPony_CheckedChanged(sender As Object, e As EventArgs) Handles SoundLimitOnePerPony.CheckedChanged
        Options.SoundSingleChannelOnly = Not SoundLimitOnePerPony.Checked
    End Sub

    Private Sub SuspendForFullscreenApp_CheckedChanged(sender As Object, e As EventArgs) Handles SuspendForFullscreenApp.CheckedChanged
        Options.SuspendForFullscreenApplication = SuspendForFullscreenApp.Checked
    End Sub

    Private Sub ScreensaverSounds_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverSounds.CheckedChanged
        Options.SoundEnabled = ScreensaverSounds.Checked
    End Sub

    Private Sub ScreensaverTransparent_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverTransparent.CheckedChanged
        If ScreensaverTransparent.Checked Then
            Options.ScreenSaverStyle = Options.ScreenSaverBackgroundStyle.Transparent
        End If
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Me.Visible = False
    End Sub

    Private Sub OptionsForm_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed
        If avoidanceZonePreviewGraphics IsNot Nothing Then avoidanceZonePreviewGraphics.Dispose()
    End Sub
End Class