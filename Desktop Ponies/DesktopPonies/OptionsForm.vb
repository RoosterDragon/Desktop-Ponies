Imports System.Globalization

Public Class OptionsForm
    Private Const PreviewMargin = 5
    Private Const SpeechMultiplier = 100
    Private Const SizeMultiplier = 20
    Private Const TimeMultiplier = 10
    Private Const ExclusionMultiplier = 100
    Private Const SoundMultiplier = 100
    Private totalScreenBounds As Rectangle
    Private updatingFromOptions As Boolean

    Public Sub New()
        updatingFromOptions = True
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub OptionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MonitorsSelection.Items.AddRange(Screen.AllScreens)

        SoundGroup.Visible = Globals.DirectXSoundAvailable

        If Options.GetInterfaceType = GetType(DesktopSprites.SpriteManagement.GtkSpriteInterface) Then
            ShowViewerInTaskbar.Visible = False
            ShowPerformanceGraph.Visible = False
        End If

        If Not OperatingSystemInfo.IsWindows OrElse Options.ProfileName <> Options.ScreensaverProfileName Then
            ScreensaverGroup.Visible = False
        End If

        If Not OperatingSystemInfo.IsWindows Then
            WindowAvoidance.Visible = False
            WindowContainment.Visible = False
            SizeScale.Enabled = False
        End If

        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        totalScreenBounds = Options.GetCombinedScreenBounds()

        EnableWaitCursor(True)

        RefreshOptions()
        ScreenCoveragePreview.Invalidate()

        Enabled = True
        UseWaitCursor = False
    End Sub

    Private Sub RefreshOptions()
        updatingFromOptions = True

        Effects.Checked = Options.PonyEffectsEnabled
        Speech.Checked = Options.PonySpeechEnabled
        PonySpeechChance.Value = CInt(Options.PonySpeechChance * SpeechMultiplier)
        Interactions.Checked = Options.PonyInteractionsEnabled
        PonyDragging.Checked = Options.PonyDraggingEnabled
        CursorAwareness.Checked = Options.CursorAvoidanceEnabled
        CursorAvoidanceRadius.Value = CDec(Options.CursorAvoidanceSize)
        PoniesAvoidPonies.Checked = Options.PonyAvoidsPonies
        SizeScale.Value = CInt(Options.ScaleFactor * SizeMultiplier)
        TimeScale.Value = CInt(Options.TimeFactor * TimeMultiplier)
        MaxPonies.Value = Options.MaxPonyCount

        ShowViewerInTaskbar.Checked = Options.ShowInTaskbar
        AlwaysOnTop.Checked = Options.AlwaysOnTop
        WindowAvoidance.Checked = Options.WindowAvoidanceEnabled
        WindowContainment.Checked = Options.WindowContainment

        Dim screenCoverageOption = If(Options.AllowedRegion Is Nothing, ScreenCoverageMonitors, ScreenCoverageArea)
        screenCoverageOption.Checked = True
        For Each screen In Options.Screens
            For i = 0 To MonitorsSelection.Items.Count - 1
                If Object.ReferenceEquals(MonitorsSelection.Items(i), screen) Then
                    MonitorsSelection.SetSelected(i, True)
                End If
            Next
        Next

        ScreenAreaX.Minimum = totalScreenBounds.X
        ScreenAreaX.Maximum = totalScreenBounds.X + totalScreenBounds.Width
        ScreenAreaY.Minimum = totalScreenBounds.Y
        ScreenAreaY.Maximum = totalScreenBounds.Y + totalScreenBounds.Height
        ScreenAreaWidth.Maximum = totalScreenBounds.Width
        ScreenAreaHeight.Maximum = totalScreenBounds.Height

        Dim area = Options.GetAllowedArea()
        ScreenAreaX.Value = area.X
        ScreenAreaY.Value = area.Y
        ScreenAreaWidth.Value = area.Width
        ScreenAreaHeight.Value = area.Height

        ScreenExclusion.Checked = Options.ExclusionZone.Width > 0 AndAlso Options.ExclusionZone.Height > 0
        ExculsionAreaX.Value = CDec(Options.ExclusionZone.X * ExclusionMultiplier)
        ExclusionAreaY.Value = CDec(Options.ExclusionZone.Y * ExclusionMultiplier)
        ExclusionAreaWidth.Value = CDec(If(ScreenExclusion.Checked,
                                           Options.ExclusionZone.Width * ExclusionMultiplier,
                                           ExclusionMultiplier / 2))
        ExclusionAreaHeight.Value = CDec(If(ScreenExclusion.Checked,
                                            Options.ExclusionZone.Height * ExclusionMultiplier,
                                            ExclusionMultiplier / 2))

        Dim outOfBoundsOption = If(Options.PonyTeleportEnabled, OutOfBoundsTeleport, OutOfBoundsWalk)
        outOfBoundsOption.Checked = True

        Sound.Checked = Options.SoundEnabled
        SoundLimitOneGlobally.Checked = Options.SoundSingleChannelOnly
        SoundLimitOnePerPony.Checked = Not Options.SoundSingleChannelOnly
        Volume.Value = CInt(Options.SoundVolume * SoundMultiplier)

        EnablePonyLogs.Checked = Options.EnablePonyLogs
        ShowPerformanceGraph.Checked = Options.ShowPerformanceGraph

        ScreensaverSounds.Checked = Options.SoundEnabled
        Select Case Options.ScreensaverStyle
            Case Options.ScreensaverBackgroundStyle.Transparent
                ScreensaverTransparent.Checked = True
            Case Options.ScreensaverBackgroundStyle.SolidColor
                ScreensaverColor.Checked = True
            Case Options.ScreensaverBackgroundStyle.BackgroundImage
                ScreensaverImage.Checked = True
        End Select

        updatingFromOptions = False
    End Sub

    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        Options.PonyCounts = New Dictionary(Of String, Integer)().AsReadOnly()
    End Sub

    Private Sub CustomFiltersButton_Click(sender As Object, e As EventArgs) Handles CustomFiltersButton.Click
        Using form = New FiltersForm()
            form.ShowDialog(Me)
        End Using
    End Sub

    Private Sub LoadButton_Click(sender As Object, e As EventArgs) Handles LoadButton.Click
        LoadProfile()
    End Sub

    Private Sub LoadProfile()
        Try
            Options.LoadProfile(Options.ProfileName, Not Globals.IsScreensaverExecutable())
            RefreshOptions()
        Catch ex As IO.IOException
            Program.NotifyUserOfNonFatalException(ex, "Failed to load profile '" & Options.ProfileName & "'")
        End Try
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        If MonitorsSelection.SelectedItems.Count = 0 Then
            MessageBox.Show(Me, "You need to select at least one monitor.",
                            "No Monitor Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If Options.ProfileName = Options.DefaultProfileName Then
            MessageBox.Show(Me, "Cannot save over the '" & Options.DefaultProfileName & "' profile. Create a new profile first.",
                            "Invalid Profile Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Try
            Options.SaveProfile(Options.ProfileName)
            MessageBox.Show(Me, "Profile '" & Options.ProfileName & "' saved.",
                            "Profile Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error attempting to save this profile.")
        End Try
    End Sub

    Private Sub Effects_CheckedChanged(sender As Object, e As EventArgs) Handles Effects.CheckedChanged
        If updatingFromOptions Then Return
        Options.PonyEffectsEnabled = Effects.Checked
    End Sub

    Private Sub Speech_CheckedChanged(sender As Object, e As EventArgs) Handles Speech.CheckedChanged
        PonySpeechChance.Enabled = Speech.Checked
        PonySpeechChanceLabel.Enabled = Speech.Checked
        If updatingFromOptions Then Return
        Options.PonySpeechEnabled = Speech.Checked
    End Sub

    Private Sub PonySpeechChance_ValueChanged(sender As Object, e As EventArgs) Handles PonySpeechChance.ValueChanged
        If updatingFromOptions Then Return
        Options.PonySpeechChance = PonySpeechChance.Value / SpeechMultiplier
    End Sub

    Private Sub Interactions_CheckedChanged(sender As Object, e As EventArgs) Handles Interactions.CheckedChanged
        If updatingFromOptions Then Return
        Options.PonyInteractionsEnabled = Interactions.Checked
    End Sub

    Private Sub PonyDragging_CheckedChanged(sender As Object, e As EventArgs) Handles PonyDragging.CheckedChanged
        If updatingFromOptions Then Return
        Options.PonyDraggingEnabled = PonyDragging.Checked
    End Sub

    Private Sub CursorAwareness_CheckedChanged(sender As Object, e As EventArgs) Handles CursorAwareness.CheckedChanged
        CursorAvoidanceRadius.Enabled = CursorAwareness.Checked
        CursorAvoidanceRadiusLabel.Enabled = CursorAwareness.Checked
        If updatingFromOptions Then Return
        Options.CursorAvoidanceEnabled = CursorAwareness.Checked
    End Sub

    Private Sub CursorAvoidanceRadius_ValueChanged(sender As Object, e As EventArgs) Handles CursorAvoidanceRadius.ValueChanged
        If updatingFromOptions Then Return
        Options.CursorAvoidanceSize = CursorAvoidanceRadius.Value
    End Sub

    Private Sub PoniesAvoidPonies_CheckedChanged(sender As Object, e As EventArgs) Handles PoniesAvoidPonies.CheckedChanged
        If updatingFromOptions Then Return
        Options.PonyAvoidsPonies = PoniesAvoidPonies.Checked
    End Sub

    Private Sub SizeScale_ValueChanged(sender As Object, e As EventArgs) Handles SizeScale.ValueChanged
        Dim value = CSng(SizeScale.Value / SizeMultiplier)
        SizeScaleValueLabel.Text = value.ToString("0.##x", CultureInfo.CurrentCulture)
        If updatingFromOptions Then Return
        Options.ScaleFactor = value
    End Sub

    Private Sub TimeScale_ValueChanged(sender As Object, e As EventArgs) Handles TimeScale.ValueChanged
        Dim value = CSng(TimeScale.Value / TimeMultiplier)
        TimeScaleValueLabel.Text = value.ToString("0.0x", CultureInfo.CurrentCulture)
        If updatingFromOptions Then Return
        Options.TimeFactor = value
    End Sub

    Private Sub MaxPonies_ValueChanged(sender As Object, e As EventArgs) Handles MaxPonies.ValueChanged
        If updatingFromOptions Then Return
        Options.MaxPonyCount = CInt(MaxPonies.Value)
    End Sub

    Private Sub ShowInTaskbar_CheckedChanged(sender As Object, e As EventArgs) Handles ShowViewerInTaskbar.CheckedChanged
        If updatingFromOptions Then Return
        Options.ShowInTaskbar = ShowViewerInTaskbar.Checked
    End Sub

    Private Sub AlwaysOnTop_CheckedChanged(sender As Object, e As EventArgs) Handles AlwaysOnTop.CheckedChanged
        If updatingFromOptions Then Return
        Options.AlwaysOnTop = AlwaysOnTop.Checked
    End Sub

    Private Sub WindowAvoidance_CheckedChanged(sender As Object, e As EventArgs) Handles WindowAvoidance.CheckedChanged
        If updatingFromOptions Then Return
        Options.WindowAvoidanceEnabled = WindowAvoidance.Checked
    End Sub

    Private Sub WindowContainment_CheckedChanged(sender As Object, e As EventArgs) Handles WindowContainment.CheckedChanged
        If updatingFromOptions Then Return
        Options.WindowContainment = WindowContainment.Checked
    End Sub

    Private Sub ScreenCoverageMonitors_CheckedChanged(sender As Object, e As EventArgs) Handles ScreenCoverageMonitors.CheckedChanged
        MonitorsSelection.Enabled = ScreenCoverageMonitors.Checked
        ScreenAreaTable.Enabled = Not ScreenCoverageMonitors.Checked
        If updatingFromOptions Then Return
        If Not ScreenCoverageMonitors.Checked Then Return
        UpdateScreens()
    End Sub

    Private Sub UpdateScreens()
        Options.AllowedRegion = Nothing
        If MonitorsSelection.SelectedItems.Count > 0 Then
            Options.Screens = MonitorsSelection.SelectedItems.Cast(Of Screen).ToImmutableArray()
        Else
            Options.Screens = {Screen.PrimaryScreen}.ToImmutableArray()
        End If
        ScreenCoveragePreview.Invalidate()
    End Sub

    Private Sub ScreenCoverageArea_CheckedChanged(sender As Object, e As EventArgs) Handles ScreenCoverageArea.CheckedChanged
        ScreenAreaTable.Enabled = ScreenCoverageArea.Checked
        If updatingFromOptions Then Return
        If Not ScreenCoverageArea.Checked Then Return
        UpdateArea()
    End Sub

    Private Sub UpdateArea()
        Options.AllowedRegion = Rectangle.Intersect(totalScreenBounds,
                                                    New Rectangle(CInt(ScreenAreaX.Value), CInt(ScreenAreaY.Value),
                                                                  CInt(ScreenAreaWidth.Value), CInt(ScreenAreaHeight.Value)))
        ScreenCoveragePreview.Invalidate()
    End Sub

    Private Sub ScreenAvoidance_CheckedChanged(sender As Object, e As EventArgs) Handles ScreenExclusion.CheckedChanged
        AvoidanceAreaTable.Enabled = ScreenExclusion.Checked
        If updatingFromOptions Then Return
        UpdateExclusionZone()
    End Sub

    Private Sub UpdateExclusionZone()
        If ScreenExclusion.Checked Then
            Options.ExclusionZone = New RectangleF(ExculsionAreaX.Value / 100, ExclusionAreaY.Value / 100,
                                                   ExclusionAreaWidth.Value / 100, ExclusionAreaHeight.Value / 100)
        Else
            Options.ExclusionZone = RectangleF.Empty
        End If
        ScreenCoveragePreview.Invalidate()
    End Sub

    Private Sub MonitorsSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MonitorsSelection.SelectedIndexChanged
        If updatingFromOptions Then Return
        If Not ScreenCoverageMonitors.Checked Then Return
        UpdateScreens()
    End Sub

    Private Sub ScreenArea_ValueChanged(sender As Object, e As EventArgs) Handles ScreenAreaX.ValueChanged,
        ScreenAreaY.ValueChanged, ScreenAreaWidth.ValueChanged, ScreenAreaHeight.ValueChanged
        If updatingFromOptions Then Return
        If Not ScreenCoverageArea.Checked Then Return
        UpdateArea()
    End Sub

    Private Sub AvoidanceArea_ValueChanged(sender As Object, e As EventArgs) Handles ExculsionAreaX.ValueChanged,
        ExclusionAreaY.ValueChanged, ExclusionAreaWidth.ValueChanged, ExclusionAreaHeight.ValueChanged
        If updatingFromOptions Then Return
        Options.ExclusionZone = New RectangleF(ExculsionAreaX.Value / ExclusionMultiplier,
                                               ExclusionAreaY.Value / ExclusionMultiplier,
                                               ExclusionAreaWidth.Value / ExclusionMultiplier,
                                               ExclusionAreaHeight.Value / ExclusionMultiplier)
        ScreenCoveragePreview.Invalidate()
    End Sub

    Private Sub OutOfBoundsTeleport_CheckedChanged(sender As Object, e As EventArgs) Handles OutOfBoundsTeleport.CheckedChanged
        If updatingFromOptions Then Return
        Options.PonyTeleportEnabled = OutOfBoundsTeleport.Checked
    End Sub

    Private Sub ScreenCoveragePreview_Paint(sender As Object, e As PaintEventArgs) Handles ScreenCoveragePreview.Paint
        Dim xScale = (ScreenCoveragePreview.ClientSize.Width - 2 * PreviewMargin) / totalScreenBounds.Width
        Dim yScale = (ScreenCoveragePreview.ClientSize.Height - 2 * PreviewMargin) / totalScreenBounds.Height
        Dim scale = Math.Min(xScale, yScale)

        Dim g = e.Graphics
        g.Clear(Color.White)

        For Each s In Screen.AllScreens
            Dim area = DrawScreenPreviewRectangle(scale, s.Bounds, 255, Color.Blue, Color.Cyan, g)
            g.DrawString(s.DeviceName, SystemFonts.SmallCaptionFont, Brushes.Navy, area.Location)
        Next

        Dim allowedArea = Options.GetAllowedArea()
        DrawScreenPreviewRectangle(scale, allowedArea, 128, Color.Green, Color.Lime, g)

        Dim exclusionArea = Options.GetExclusionArea(allowedArea, Options.ExclusionZone)
        DrawScreenPreviewRectangle(scale, exclusionArea, 128, Color.Red, Color.Orange, g)
    End Sub

    Private Function DrawScreenPreviewRectangle(scale As Double, rectArea As Rectangle, opacity As Byte,
                                           lineColor As Color, fillColor As Color, graphics As Graphics) As Rectangle
        Dim scaledRectArea = rectArea
        scaledRectArea.Location -= New Size(totalScreenBounds.Location)
        scaledRectArea.Location = New Point(CInt(scaledRectArea.X * scale), CInt(scaledRectArea.Y * scale))
        scaledRectArea.Location += New Size(PreviewMargin, PreviewMargin)
        scaledRectArea.Size = New Size(CInt(scaledRectArea.Width * scale), CInt(scaledRectArea.Height * scale))
        Using pen = New Pen(Color.FromArgb(opacity, lineColor)),
            brush = New SolidBrush(Color.FromArgb(opacity, fillColor))
            graphics.FillRectangle(brush, scaledRectArea)
            graphics.DrawRectangle(pen, scaledRectArea.X, scaledRectArea.Y, scaledRectArea.Width - 1, scaledRectArea.Height - 1)
        End Using
        Return scaledRectArea
    End Function

    Private Sub Sound_CheckedChanged(sender As Object, e As EventArgs) Handles Sound.CheckedChanged
        SoundLimitOnePerPony.Enabled = Sound.Checked
        SoundLimitOneGlobally.Enabled = Sound.Checked
        Volume.Enabled = Sound.Checked
        VolumeLabel.Enabled = Sound.Checked
        VolumeValueLabel.Enabled = Sound.Checked
        If updatingFromOptions Then Return
        Options.SoundEnabled = Sound.Checked
    End Sub

    Private Sub SoundLimitOnePerPony_CheckedChanged(sender As Object, e As EventArgs) Handles SoundLimitOnePerPony.CheckedChanged
        If updatingFromOptions Then Return
        Options.SoundSingleChannelOnly = Not SoundLimitOnePerPony.Checked
    End Sub

    Private Sub SoundLimitOneGlobally_CheckedChanged(sender As Object, e As EventArgs) Handles SoundLimitOneGlobally.CheckedChanged
        If updatingFromOptions Then Return
        Options.SoundSingleChannelOnly = SoundLimitOneGlobally.Checked
    End Sub

    Private Sub Volume_ValueChanged(sender As Object, e As EventArgs) Handles Volume.ValueChanged
        Dim value = CSng(Volume.Value / SoundMultiplier)
        VolumeValueLabel.Text = (value * 10).ToString("0.0", CultureInfo.CurrentCulture)
        If updatingFromOptions Then Return
        Options.SoundVolume = value
    End Sub

    Private Sub ShowPonyLogs_CheckedChanged(sender As Object, e As EventArgs) Handles EnablePonyLogs.CheckedChanged
        If updatingFromOptions Then Return
        Options.EnablePonyLogs = EnablePonyLogs.Checked
    End Sub

    Private Sub PerformanceGraph_CheckedChanged(sender As Object, e As EventArgs) Handles ShowPerformanceGraph.CheckedChanged
        If updatingFromOptions Then Return
        Options.ShowPerformanceGraph = ShowPerformanceGraph.Checked
    End Sub

    Private Sub ScreensaverSounds_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverSounds.CheckedChanged
        If updatingFromOptions Then Return
        Options.SoundEnabled = ScreensaverSounds.Checked
    End Sub

    Private Sub ScreensaverTransparent_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverTransparent.CheckedChanged
        If updatingFromOptions Then Return
        If ScreensaverTransparent.Checked Then Options.ScreensaverStyle = Options.ScreensaverBackgroundStyle.Transparent
    End Sub

    Private Sub ScreensaverColorButton_Click(sender As Object, e As EventArgs) Handles ScreensaverColorButton.Click
        Using dialog As New ColorDialog()
            dialog.Color = Options.ScreensaverBackgroundColor
            If dialog.ShowDialog() = DialogResult.OK Then
                Options.ScreensaverBackgroundColor = dialog.Color
                ScreensaverColorNeededLabel.Visible = False
            End If
        End Using
    End Sub

    Private Sub ScreensaverColor_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverColor.CheckedChanged
        ScreensaverColorNeededLabel.Visible = ScreensaverColor.Checked AndAlso Options.ScreensaverBackgroundColor.A < 255
        If updatingFromOptions Then Return
        If ScreensaverColor.Checked Then Options.ScreensaverStyle = Options.ScreensaverBackgroundStyle.SolidColor
    End Sub

    Private Sub ScreensaverImage_CheckedChanged(sender As Object, e As EventArgs) Handles ScreensaverImage.CheckedChanged
        ScreensaverImageNeededLabel.Visible = ScreensaverImage.Checked AndAlso
            (Options.ScreensaverBackgroundImagePath = "" OrElse Not IO.File.Exists(Options.ScreensaverBackgroundImagePath))
        If updatingFromOptions Then Return
        If ScreensaverImage.Checked Then Options.ScreensaverStyle = Options.ScreensaverBackgroundStyle.BackgroundImage
    End Sub

    Private Sub ScreensaverImageButton_Click(sender As Object, e As EventArgs) Handles ScreensaverImageButton.Click
        Using dialog As New OpenFileDialog
            dialog.Title = "Select your screensaver background image..."
            dialog.Filter = "GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|JPG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg" &
                "|Image Files (*.gif, *.png, *.jpg, *.jpeg)|*.gif;*.png;*.jpg;*.jpeg"
            dialog.FilterIndex = 4
            dialog.Multiselect = False

            If dialog.ShowDialog() = DialogResult.OK Then
                If IO.File.Exists(dialog.FileName) Then
                    Try
                        Image.FromFile(dialog.FileName)
                    Catch ex As Exception
                        Program.NotifyUserOfNonFatalException(ex, "Failed to load image: " & dialog.FileName)
                        Exit Sub
                    End Try

                    Options.ScreensaverBackgroundImagePath = dialog.FileName
                    ScreensaverImageNeededLabel.Visible = False
                End If
            End If
        End Using
    End Sub
End Class