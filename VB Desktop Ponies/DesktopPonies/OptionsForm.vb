Public Class OptionsForm
    Private selectingMonitors As Boolean
    Private avoidanceZonePreviewGraphics As Graphics
    Private initializing As Boolean = True

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub OptionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Enabled = False
        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        Icon = My.Resources.Twilight

        AvoidanceZonePreview.Image = New Bitmap(AvoidanceZonePreview.Size.Width, AvoidanceZonePreview.Size.Height)
        avoidanceZonePreviewGraphics = Graphics.FromImage(AvoidanceZonePreview.Image)

        MonitorsSelection.Items.AddRange(Screen.AllScreens.Select(Function(s) s.DeviceName).ToArray())

        For i = 0 To MonitorsSelection.Items.Count - 1
            MonitorsSelection.SetSelected(i, True)
        Next

        If Not Reference.DirectXSoundAvailable Then
            SoundDisabledLabel.Visible = True
            Sound.Enabled = False
            Sound.Checked = False
            ScreensaverSounds.Enabled = False
            ScreensaverSounds.Checked = False
        Else
            SoundDisabledLabel.Visible = False
            Sound.Enabled = True
        End If

        Main.Instance.ResetToDefaultFilterCategories()

        ' Set initial volume value, event handler will update values as needed.
        Volume.Value = 650

        ' This option is no longer available as there is no code to reliably determine if a fullscreen window is active at the moment.
        SuspendForFullscreenApp.Visible = False
        SuspendForFullscreenApp.Enabled = False
        Options.SuspendForFullscreenApplication = False

        ' This option causes random crashes on Mac.
        ' TODO: Determine cause of errors - appears to be threading related.
        If OperatingSystemInfo.IsMacOSX Then
            SpeechDisabledLabel.Visible = True
            SpeechDisabled.Checked = True
            SpeechGroup.Enabled = False
            Options.PonySpeechEnabled = False
        End If

        initializing = False
        RefreshOptions()

        Enabled = True
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

        SelectMonitors()

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
        'SoundDisabledLabel.Visible = False
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

        Select Case Options.ScreensaverStyle
            Case Options.ScreensaverBackgroundStyle.Transparent
                ScreensaverTransparent.Checked = True
            Case Options.ScreensaverBackgroundStyle.SolidColor
                ScreensaverColor.Checked = True
            Case Options.ScreensaverBackgroundStyle.BackgroundImage
                ScreensaverImage.Checked = True
        End Select

    End Sub

    Private Sub SelectMonitors()
        selectingMonitors = True
        MonitorsSelection.SelectedItems.Clear()

        For Each monitorNameLoop In Options.MonitorNames
            Dim monitorName = monitorNameLoop
            For i = 0 To MonitorsSelection.Items.Count - 1
                If CStr(MonitorsSelection.Items(i)) = monitorName Then
                    MonitorsSelection.SetSelected(i, True)
                End If
            Next
        Next

        selectingMonitors = False
    End Sub

    Private Sub LoadButton_Click(sender As Object, e As EventArgs) Handles LoadButton.Click
        LoadProfile()
    End Sub

    Private Sub LoadProfile()
        Dim profile = Options.DefaultProfileName
        Try
            If Not String.IsNullOrWhiteSpace(Main.Instance.ProfileComboBox.Text) Then
                profile = Main.Instance.ProfileComboBox.Text.Trim()
            End If

            Options.LoadProfile(profile)

            RefreshOptions()
            If Main.Instance.FilterOptionsBox.Items.Count = 0 Then
                Main.Instance.ResetToDefaultFilterCategories()
            End If

            SizeScale_ValueChanged(Nothing, Nothing)
        Catch ex As IO.IOException
            My.Application.NotifyUserOfNonFatalException(ex, "Failed to load profile '" & profile & "'")
        End Try
        Try
            IO.File.WriteAllText(IO.Path.Combine(Options.ProfileDirectory, "current.txt"), profile, System.Text.Encoding.UTF8)
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
        Options.PonyCounts.Clear()

        For Each ponyPanel As PonySelectionControl In Main.Instance.PonySelectionPanel.Controls
            ponyPanel.PonyCount.Text = "0"
        Next
    End Sub

    Private Sub AvoidanceZoneArea_ValueChanged(sender As Object, e As EventArgs) Handles AvoidanceZoneHeight.ValueChanged, AvoidanceZoneWidth.ValueChanged, AvoidanceZoneY.ValueChanged, AvoidanceZoneX.ValueChanged
        If initializing Then Return
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
        If initializing Then Return
        If selectingMonitors Then Exit Sub

        If MonitorsSelection.SelectedItems.Count = 0 Then
            MonitorsMinimumLabel.Visible = True
            Exit Sub
        Else
            MonitorsMinimumLabel.Visible = False
        End If

        Options.MonitorNames.Clear()

        For i = 0 To MonitorsSelection.SelectedItems.Count - 1
            For Each monitor In Screen.AllScreens
                If monitor.DeviceName = CStr(MonitorsSelection.SelectedItems(i)) Then
                    Options.MonitorNames.Add(monitor.DeviceName)
                End If
            Next
        Next

        If IsNothing(Pony.CurrentViewer) Then
            'done
            Exit Sub
        ElseIf TypeOf Pony.CurrentViewer Is CSDesktopPonies.SpriteManagement.WinFormSpriteInterface Then
            Dim area = Options.GetCombinedScreenArea()
            DirectCast(Pony.CurrentViewer, CSDesktopPonies.SpriteManagement.WinFormSpriteInterface).DisplayBounds = area
        End If

    End Sub

    Private Sub CursorAvoidanceRadius_ValueChanged(sender As Object, e As EventArgs) Handles CursorAvoidanceRadius.ValueChanged
        If initializing Then Return
        Options.CursorAvoidanceSize = CursorAvoidanceRadius.Value
    End Sub

    Private Sub WindowAvoidance_CheckedChanged(sender As Object, e As EventArgs) Handles WindowAvoidance.CheckedChanged
        If initializing Then Return
        PoniesAvoidPonies.Enabled = WindowAvoidance.Checked
        PoniesStayInBoxes.Enabled = WindowAvoidance.Checked
        Options.WindowAvoidanceEnabled = WindowAvoidance.Checked
    End Sub

    Private Sub SizeScale_ValueChanged(sender As Object, e As EventArgs) Handles SizeScale.ValueChanged
        If initializing Then Return
        SizeScaleValueLabel.Text = Math.Round(SizeScale.Value / 100.0F, 2) & "x"
    End Sub

    Private Sub SizeScale_MouseUp(sender As Object, e As EventArgs) Handles SizeScale.MouseUp
        If initializing Then Return
        Options.ScaleFactor = SizeScale.Value / 100.0F
        Main.Instance.PonySelectionPanel.SuspendLayout()
        For Each control As PonySelectionControl In Main.Instance.PonySelectionPanel.Controls
            control.ResizeToFit()
            control.Invalidate()
        Next
        Main.Instance.PonySelectionPanel.ResumeLayout()
    End Sub

    Private Sub Volume_ValueChanged(sender As Object, e As EventArgs) Handles Volume.ValueChanged
        If initializing Then Return

        'The slider is in %, we need to convert that to the volume that an
        'Microsoft.DirectX.AudioVideoPlayback.Audio.volume would take.
        'which is from -10000 to 0 (0 being the loudest), on a logarithmic scale.

        'SoundVolume = CInt(4342 * Math.Log(Volume.Value / 100) - 10000)
        Options.SoundVolume = CSng(Volume.Value / 1000)

        VolumeValueLabel.Text = CStr(Volume.Value / 100)
    End Sub

    Private Sub CustomFiltersButton_Click(sender As Object, e As EventArgs) Handles CustomFiltersButton.Click
        FiltersForm.ShowDialog()
    End Sub

    Private Sub ScreensaverColorButton_Click(sender As Object, e As EventArgs) Handles ScreensaverColorButton.Click
        Using dialog As New ColorDialog
            dialog.Color = Options.ScreensaverBackgroundColor
            If dialog.ShowDialog() = DialogResult.OK Then
                Options.ScreensaverBackgroundColor = dialog.Color
                ScreensaverColorNeededLabel.Visible = False
            End If
        End Using
    End Sub

    Private Sub ScreensaverColor_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverColor.CheckedChanged
        If initializing Then Return
        If ScreensaverColor.Checked Then
            ScreensaverColorNeededLabel.Visible = Options.ScreensaverBackgroundColor.A < 255
            Options.ScreensaverStyle = Options.ScreensaverBackgroundStyle.SolidColor
        Else
            ScreensaverColorNeededLabel.Visible = False
        End If
    End Sub

    Private Sub ScreensaverImage_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverImage.CheckedChanged
        If initializing Then Return
        If ScreensaverImage.Checked Then
            If Options.ScreensaverBackgroundImagePath = "" OrElse Not IO.File.Exists(Options.ScreensaverBackgroundImagePath) Then
                ScreensaverImageNeededLabel.Visible = True
            End If
            Options.ScreensaverStyle = Options.ScreensaverBackgroundStyle.BackgroundImage
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
                        Image.FromFile(dialog.FileName)
                    Catch ex As Exception
                        My.Application.NotifyUserOfNonFatalException(ex, "Failed to load image: " & dialog.FileName)
                        Exit Sub
                    End Try

                    Options.ScreensaverBackgroundImagePath = dialog.FileName
                    ScreensaverImageNeededLabel.Visible = False
                End If
            End If
        End Using
    End Sub

    Private Sub AlwaysOnTop_CheckedChanged(sender As Object, e As EventArgs) Handles AlwaysOnTop.CheckedChanged
        If initializing Then Return
        Options.AlwaysOnTop = AlwaysOnTop.Checked
        If Not IsNothing(Pony.CurrentViewer) Then
            Pony.CurrentViewer.Topmost = Options.AlwaysOnTop
        End If
    End Sub

    Private Sub TimeScale_Scroll(sender As Object, e As EventArgs) Handles TimeScale.Scroll
        If initializing Then Return
        Options.TimeFactor = TimeScale.Value / 10.0F
        TimeScaleValueLabel.Text = Options.TimeFactor.ToString("0.0x", Globalization.CultureInfo.CurrentCulture)
    End Sub

    Private Sub PonySpeechChance_ValueChanged(sender As Object, e As EventArgs) Handles PonySpeechChance.ValueChanged
        If initializing Then Return
        Options.PonySpeechChance = PonySpeechChance.Value / 100
    End Sub

    Private Sub SpeechDisabled_CheckedChanged(sender As Object, e As EventArgs) Handles SpeechDisabled.CheckedChanged
        If initializing Then Return
        Options.PonySpeechEnabled = Not SpeechDisabled.Checked
    End Sub

    Private Sub CursorAvoidance_CheckedChanged(sender As Object, e As EventArgs) Handles CursorAvoidance.CheckedChanged
        If initializing Then Return
        Options.CursorAvoidanceEnabled = CursorAvoidance.Checked
    End Sub

    Private Sub PonyDragging_CheckedChanged(sender As Object, e As EventArgs) Handles PonyDragging.CheckedChanged
        If initializing Then Return
        Options.PonyDraggingEnabled = PonyDragging.Checked
    End Sub

    Private Sub Interactions_CheckedChanged(sender As Object, e As EventArgs) Handles Interactions.CheckedChanged
        If initializing Then Return
        Options.PonyInteractionsEnabled = Interactions.Checked
    End Sub

    Private Sub InteractionErrorsDisplayed_CheckedChanged(sender As Object, e As EventArgs) Handles InteractionErrorsDisplayed.CheckedChanged
        If initializing Then Return
        Options.DisplayPonyInteractionsErrors = InteractionErrorsDisplayed.Checked
    End Sub

    Private Sub MaxPonies_ValueChanged(sender As Object, e As EventArgs) Handles MaxPonies.ValueChanged
        If initializing Then Return
        Options.MaxPonyCount = CInt(MaxPonies.Value)
    End Sub

    Private Sub AlphaBlending_CheckedChanged(sender As Object, e As EventArgs) Handles AlphaBlending.CheckedChanged
        If initializing Then Return
        Options.AlphaBlendingEnabled = AlphaBlending.Checked
    End Sub

    Private Sub Effects_CheckedChanged(sender As Object, e As EventArgs) Handles Effects.CheckedChanged
        If initializing Then Return
        Options.PonyEffectsEnabled = Effects.Checked
    End Sub

    Private Sub PoniesAvoidPonies_CheckedChanged(sender As Object, e As EventArgs) Handles PoniesAvoidPonies.CheckedChanged
        If initializing Then Return
        Options.PonyAvoidsPonies = PoniesAvoidPonies.Checked
    End Sub

    Private Sub PoniesStayInBoxes_CheckedChanged(sender As Object, e As EventArgs) Handles PoniesStayInBoxes.CheckedChanged
        If initializing Then Return
        Options.PonyStaysInBox = PoniesStayInBoxes.Checked
    End Sub

    Private Sub Teleport_CheckedChanged(sender As Object, e As EventArgs) Handles Teleport.CheckedChanged
        If initializing Then Return
        Options.PonyTeleportEnabled = Teleport.Checked
    End Sub

    Private Sub Sound_CheckedChanged(sender As Object, e As EventArgs) Handles Sound.CheckedChanged
        If initializing Then Return
        Options.SoundEnabled = Sound.Checked
    End Sub

    Private Sub SoundLimitOneGlobally_CheckedChanged(sender As Object, e As EventArgs) Handles SoundLimitOneGlobally.CheckedChanged
        If initializing Then Return
        Options.SoundSingleChannelOnly = SoundLimitOneGlobally.Checked
    End Sub

    Private Sub SoundLimitOnePerPony_CheckedChanged(sender As Object, e As EventArgs) Handles SoundLimitOnePerPony.CheckedChanged
        If initializing Then Return
        Options.SoundSingleChannelOnly = Not SoundLimitOnePerPony.Checked
    End Sub

    Private Sub SuspendForFullscreenApp_CheckedChanged(sender As Object, e As EventArgs) Handles SuspendForFullscreenApp.CheckedChanged
        If initializing Then Return
        Options.SuspendForFullscreenApplication = SuspendForFullscreenApp.Checked
    End Sub

    Private Sub ScreensaverSounds_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverSounds.CheckedChanged
        If initializing Then Return
        Options.SoundEnabled = ScreensaverSounds.Checked
    End Sub

    Private Sub ScreensaverTransparent_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverTransparent.CheckedChanged
        If initializing Then Return
        If ScreensaverTransparent.Checked Then
            Options.ScreensaverStyle = Options.ScreensaverBackgroundStyle.Transparent
        End If
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Sub OptionsForm_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed
        If avoidanceZonePreviewGraphics IsNot Nothing Then avoidanceZonePreviewGraphics.Dispose()
    End Sub
End Class