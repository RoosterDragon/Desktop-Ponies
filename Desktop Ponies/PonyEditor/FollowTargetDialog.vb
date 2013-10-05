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
        RedrawFollowPoint()

        FollowComboBox.Items.Clear()
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
        Using pen As New System.Drawing.Pen(Color.Red, 2)
            previewGraphics.DrawLine(pen,
                                     New Point(previewPoint.X - 5, previewPoint.Y - 5),
                                     New Point(previewPoint.X + 5, previewPoint.Y + 5))
            previewGraphics.DrawLine(pen,
                                     New Point(previewPoint.X + 5, previewPoint.Y - 5),
                                     New Point(previewPoint.X - 5, previewPoint.Y + 5))
        End Using

        PointPreviewArea.Invalidate()
    End Sub

    Private Sub FollowOption_CheckedChanged(sender As Object, e As EventArgs) Handles FollowOption.CheckedChanged
        If Not FollowOption.Checked Then Return
        GetThumbnail()
        PointX.Maximum = 500
        PointX.Minimum = -500
        PointX.Value = 0
        PointY.Maximum = 500
        PointY.Minimum = -500
        PointY.Value = 0
        PointX.Enabled = True
        PointY.Enabled = True
        FollowComboBox.Enabled = True
        RelativeToLabel.Text = "(Relative to pony center)"
        UnitsLabel.Text = "Location (X/Y) (in pixels)"
    End Sub

    Private Sub GoToPointOption_CheckedChanged(sender As Object, e As EventArgs) Handles GoToPointOption.CheckedChanged
        If Not GoToPointOption.Checked Then Return
        ponyThumbnail = Nothing
        PointX.Maximum = 100
        PointX.Minimum = 0
        PointX.Value = 50
        PointY.Maximum = 100
        PointY.Minimum = 0
        PointY.Value = 50
        PointX.Enabled = True
        PointY.Enabled = True
        FollowComboBox.Enabled = False
        RelativeToLabel.Text = "(Relative to top right of screen)"
        UnitsLabel.Text = "Location (X/Y) (in % of screen height/width)"
    End Sub

    Private Sub NoTargetOption_CheckedChanged(sender As Object, e As EventArgs) Handles NoTargetOption.CheckedChanged
        If Not NoTargetOption.Checked Then Return
        ponyThumbnail = Nothing
        PointX.Enabled = False
        PointY.Enabled = False
        FollowComboBox.Enabled = False
        RedrawFollowPoint()
    End Sub

    Private Sub FollowComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FollowComboBox.SelectedIndexChanged
        GetThumbnail()
    End Sub

    Private Sub GetThumbnail()
        For Each ponyBase In behaviorToChange.Base.Collection.AllBases
            If ponyBase.Directory = DirectCast(FollowComboBox.SelectedItem, String) Then
                Try
                    ponyThumbnail = Image.FromFile(ponyBase.Behaviors(0).RightImage.Path)
                Catch ex As Exception
                    ponyThumbnail = Nothing
                    My.Application.NotifyUserOfNonFatalException(ex, "Failed to load image for pony " & ponyBase.Directory)
                End Try
                RedrawFollowPoint()
                Exit Sub
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
        If AutoSelectImageCheckbox.Checked = False AndAlso
            (MovingComboBox.SelectedItem.ToString() = "" OrElse StoppedComboBox.SelectedItem.ToString() = "") Then
            MessageBox.Show(Me, "If you disable auto-selection of images, " &
                            "then you must specify a moving behavior and a stopped behavior to get the images from." &
                            ControlChars.NewLine &
                            "(You can select the behavior you are editing once you have saved it first.)",
                            "Images Undefined", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
