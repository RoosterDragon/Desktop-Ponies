Public Class FollowTargetDialog

    Private ReadOnly behaviorToChange As Behavior
    Private previewGraphics As Graphics
    Private ponyThumbnail As Image

    Public Sub New(behavior As Behavior)
        behaviorToChange = Argument.EnsureNotNull(behavior, "behavior")
        InitializeComponent()
        Text = "Select following parameters for " & behaviorToChange.Name
    End Sub

    Private Sub FollowTargetDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        Enabled = False
        Update()

        PointPreviewArea.Image = New Bitmap(PointPreviewArea.Size.Width, PointPreviewArea.Size.Height)
        previewGraphics = Graphics.FromImage(PointPreviewArea.Image)

        FollowComboBox.BeginUpdate()
        For Each base In behaviorToChange.Base.Collection.AllBases
            FollowComboBox.Items.Add(base.Directory)
        Next
        FollowComboBox.EndUpdate()

        MovingComboBox.BeginUpdate()
        StoppedComboBox.BeginUpdate()
        For Each otherBehavior In behaviorToChange.Base.Behaviors
            MovingComboBox.Items.Add(otherBehavior.Name)
            StoppedComboBox.Items.Add(otherBehavior.Name)
        Next
        MovingComboBox.EndUpdate()
        StoppedComboBox.EndUpdate()

        AutoSelectImageCheckbox.Checked = behaviorToChange.AutoSelectImagesOnFollow

        Dim targetIsPoint = behaviorToChange.OriginalDestinationXCoord <> 0 OrElse behaviorToChange.OriginalDestinationYCoord <> 0
        Dim targetIsPony = False
        If behaviorToChange.OriginalFollowTargetName <> "" Then
            targetIsPony = True
            FollowComboBox.SelectedItem = behaviorToChange.OriginalFollowTargetName
        End If

        If behaviorToChange.FollowMovingBehaviorName <> "" Then
            MovingComboBox.SelectedItem = behaviorToChange.FollowMovingBehaviorName
        End If

        If behaviorToChange.FollowStoppedBehaviorName <> "" Then
            StoppedComboBox.SelectedItem = behaviorToChange.FollowStoppedBehaviorName
        End If

        If targetIsPoint Then
            PointX.Value = behaviorToChange.OriginalDestinationXCoord
            PointY.Value = behaviorToChange.OriginalDestinationYCoord
        End If

        If targetIsPony AndAlso Not targetIsPoint Then
            FollowOption.Checked = True
        ElseIf Not targetIsPony AndAlso targetIsPoint Then
            GoToPointOption.Checked = True
        ElseIf targetIsPony AndAlso targetIsPoint Then
            FollowOption.Checked = True
        Else
            NoTargetOption.Checked = True
            ponyThumbnail = Nothing
        End If

        RedrawFollowPoint()

        Enabled = True
    End Sub

    Private Sub RedrawFollowPoint()
        If previewGraphics Is Nothing Then Return

        Dim location = New Vector2(CInt(PointX.Value), CInt(PointY.Value))
        Dim previewPoint As Vector2
        If FollowOption.Checked Then
            previewPoint = New Vector2(PointPreviewArea.Image.Size) / 2 + location
        Else
            previewPoint = Vector2.Truncate(New Vector2F(0.01F * location.X * PointPreviewArea.Image.Width,
                                                         0.01F * location.Y * PointPreviewArea.Image.Height))
        End If

        previewGraphics.Clear(Color.White)
        If ponyThumbnail IsNot Nothing Then
            previewGraphics.DrawImage(ponyThumbnail, New Vector2(PointPreviewArea.Image.Size) / 2 - New Vector2(ponyThumbnail.Size) / 2)
        End If
        If Not NoTargetOption.Checked Then
            Using pen As New Pen(Color.Red, 2)
                previewGraphics.DrawLine(pen,
                                         New Point(previewPoint.X - 5, previewPoint.Y - 5),
                                         New Point(previewPoint.X + 5, previewPoint.Y + 5))
                previewGraphics.DrawLine(pen,
                                         New Point(previewPoint.X + 5, previewPoint.Y - 5),
                                         New Point(previewPoint.X - 5, previewPoint.Y + 5))
            End Using
        End If

        PointPreviewArea.Invalidate()
    End Sub

    Private Sub FollowOption_CheckedChanged(sender As Object, e As EventArgs) Handles FollowOption.CheckedChanged,
        GoToPointOption.CheckedChanged, NoTargetOption.CheckedChanged
        If Not DirectCast(sender, RadioButton).Checked Then Return
        If FollowOption.Checked Then
            UpdateThumbnail()
        Else
            ponyThumbnail = Nothing
        End If
        FollowComboBox.Enabled = FollowOption.Checked

        AutoSelectImageCheckbox.Enabled = Not NoTargetOption.Checked
        Dim enableBehaviorCombo = Not NoTargetOption.Checked AndAlso Not AutoSelectImageCheckbox.Checked
        StoppedComboBox.Enabled = enableBehaviorCombo
        MovingComboBox.Enabled = enableBehaviorCombo

        PointX.Enabled = Not NoTargetOption.Checked
        PointY.Enabled = Not NoTargetOption.Checked

        Dim min = If(FollowOption.Checked, -500, 0)
        Dim max = If(FollowOption.Checked, 500, 100)
        Dim value = If(FollowOption.Checked, 0, 50)
        PointX.Minimum = min
        PointX.Maximum = max
        PointX.Value = value
        PointY.Minimum = min
        PointY.Maximum = max
        PointY.Value = value

        Dim relativeTo = ""
        Dim units = ""
        If FollowOption.Checked Then
            relativeTo = "(relative to pony center)"
            units = "Offset in pixels"
        ElseIf GoToPointOption.Checked Then
            relativeTo = "(relative to screen)"
            units = "Location in % of width/height"
        End If
        RelativeToLabel.Text = relativeTo
        UnitsLabel.Text = units

        RedrawFollowPoint()
    End Sub

    Private Sub FollowComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FollowComboBox.SelectedIndexChanged
        UpdateThumbnail()
        RedrawFollowPoint()
    End Sub

    Private Sub UpdateThumbnail()
        For Each ponyBase In behaviorToChange.Base.Collection.AllBases
            If ponyBase.Directory = DirectCast(FollowComboBox.SelectedItem, String) Then
                Try
                    If ponyBase.Behaviors.Count > 0 Then
                        ponyThumbnail = Image.FromFile(ponyBase.Behaviors(0).RightImage.Path)
                    Else
                        ponyThumbnail = Nothing
                    End If
                Catch ex As Exception
                    ponyThumbnail = Nothing
                    My.Application.NotifyUserOfNonFatalException(ex, "Failed to load image for pony " & ponyBase.Directory)
                End Try
                Return
            End If
        Next
    End Sub

    Private Sub Point_ValueChanged(sender As Object, e As EventArgs) Handles PointX.ValueChanged, PointY.ValueChanged
        RedrawFollowPoint()
    End Sub

    Private Sub AutoSelectImageCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles AutoSelectImageCheckbox.CheckedChanged
        MovingComboBox.Enabled = Not AutoSelectImageCheckbox.Checked
        StoppedComboBox.Enabled = Not AutoSelectImageCheckbox.Checked
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If Not AutoSelectImageCheckbox.Checked AndAlso
            (MovingComboBox.SelectedItem Is Nothing OrElse StoppedComboBox.SelectedItem Is Nothing) Then
            MessageBox.Show(Me, "If you disable auto-selection of images, " &
                            "then you must specify a moving behavior and a stopped behavior to get the images from." &
                            ControlChars.NewLine &
                            "(You can select the behavior you are editing once you have saved it first.)",
                            "Images Undefined", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        ElseIf FollowOption.Checked AndAlso FollowComboBox.SelectedItem Is Nothing Then
            MessageBox.Show(Me, "Please choose a pony to follow, or chose a different mode.",
                            "No Target Defined", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If Not NoTargetOption.Checked Then
            behaviorToChange.OriginalDestinationXCoord = CInt(PointX.Value)
            behaviorToChange.OriginalDestinationYCoord = CInt(PointY.Value)
        Else
            behaviorToChange.OriginalDestinationXCoord = 0
            behaviorToChange.OriginalDestinationYCoord = 0
        End If

        If FollowOption.Checked Then
            behaviorToChange.OriginalFollowTargetName = DirectCast(FollowComboBox.SelectedItem, String)
        Else
            behaviorToChange.OriginalFollowTargetName = ""
        End If

        If AutoSelectImageCheckbox.Checked Then
            behaviorToChange.AutoSelectImagesOnFollow = True
        Else
            behaviorToChange.AutoSelectImagesOnFollow = False
            behaviorToChange.FollowMovingBehaviorName = DirectCast(MovingComboBox.SelectedItem, CaseInsensitiveString)
            behaviorToChange.FollowStoppedBehaviorName = DirectCast(StoppedComboBox.SelectedItem, CaseInsensitiveString)
        End If

        DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class
