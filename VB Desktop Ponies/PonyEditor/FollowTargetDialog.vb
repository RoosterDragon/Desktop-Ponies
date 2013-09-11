Public Class FollowTargetDialog

    Private previewPoint As Vector2
    Private previewGraphics As Graphics
    Private ponyThumbnail As Image
    Private behaviorToChange As Behavior

    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor, behavior As Behavior)
        Argument.EnsureNotNull(behavior, "behavior")
        InitializeComponent()
        m_editor = editor
        behaviorToChange = behavior
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

        Change_Behavior()

        Enabled = True
    End Sub

    Private Sub RedrawFollowPoint()
        If previewGraphics Is Nothing Then Return

        Dim location = New Vector2(CInt(PointX.Value), CInt(PointY.Value))
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

    Private Sub Change_Behavior()
        FollowComboBox.Items.Clear()

        For Each Available_Pony In m_editor.Ponies.Bases
            FollowComboBox.Items.Add(Available_Pony.Directory)
        Next

        For Each effect In m_editor.GetAllEffects()
            FollowComboBox.Items.Add(effect.ParentPonyBase.DisplayName & "'s " & effect.Name)
        Next

        MovingComboBox.Items.Clear()
        StoppedComboBox.Items.Clear()

        For Each otherbehavior In m_editor.PreviewPony.Behaviors
            MovingComboBox.Items.Add(otherbehavior.Name)
            StoppedComboBox.Items.Add(otherbehavior.Name)
        Next

        If behaviorToChange.AutoSelectImagesOnFollow = False Then
            AutoSelectImageCheckbox.Checked = False
        Else
            AutoSelectImageCheckbox.Checked = True
        End If

        Dim point As Boolean = False
        Dim pony As Boolean = False

        If (behaviorToChange.OriginalDestinationXCoord <> 0 OrElse behaviorToChange.OriginalDestinationYCoord <> 0) Then
            point = True
        End If

        If (Not IsNothing(behaviorToChange.OriginalFollowObjectName) AndAlso behaviorToChange.OriginalFollowObjectName <> "") Then
            pony = True
            For Each item As String In FollowComboBox.Items
                If String.Equals(StringToEffectName(item), behaviorToChange.OriginalFollowObjectName, StringComparison.OrdinalIgnoreCase) Then
                    FollowComboBox.SelectedItem = item
                End If
            Next
        End If

        If (Not IsNothing(behaviorToChange.FollowMovingBehavior) AndAlso behaviorToChange.FollowMovingBehaviorName <> "") Then
            For Each item As String In MovingComboBox.Items
                If String.Equals(item, behaviorToChange.FollowMovingBehaviorName, StringComparison.OrdinalIgnoreCase) Then
                    MovingComboBox.SelectedItem = item
                End If
            Next
        End If

        If (Not IsNothing(behaviorToChange.FollowStoppedBehavior) AndAlso behaviorToChange.FollowStoppedBehaviorName <> Nothing) Then
            For Each item As String In StoppedComboBox.Items
                If String.Equals(item, behaviorToChange.FollowStoppedBehaviorName, StringComparison.OrdinalIgnoreCase) Then
                    StoppedComboBox.SelectedItem = item
                End If
            Next
        End If

        If pony AndAlso Not point Then
            FollowOption.Checked = True
            previewPoint = (New Point(CInt((0.01 * 50) * PointPreviewArea.Size.Width), CInt((50 * 0.01) * PointPreviewArea.Size.Height)))
        End If

        If point AndAlso Not pony Then
            GoToPointOption.Checked = True
            PointX.Value = behaviorToChange.OriginalDestinationXCoord
            PointY.Value = behaviorToChange.OriginalDestinationYCoord
        End If

        If pony AndAlso point Then
            FollowOption.Checked = True
            PointX.Value = behaviorToChange.OriginalDestinationXCoord
            PointY.Value = behaviorToChange.OriginalDestinationYCoord
        End If

        If Not pony AndAlso Not point Then
            FollowOption.Checked = True
            ponyThumbnail = Nothing
            previewPoint = (New Point(
                            CInt((0.01 * 50) * PointPreviewArea.Size.Width),
                            CInt((50 * 0.01) * PointPreviewArea.Size.Height)))
        End If

    End Sub

    Private Sub Pony_Radio_CheckedChanged(sender As Object, e As EventArgs) Handles FollowOption.CheckedChanged
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
        RelativeToLabel.Text = "(Relative to pony/effect center)"
        UnitsLabel.Text = "Location (X/Y) (in pixels)"
    End Sub

    Private Sub Point_Radio_CheckedChanged(sender As Object, e As EventArgs) Handles GoToPointOption.CheckedChanged
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

    Private Sub DisableRadio_CheckedChanged(sender As Object, e As EventArgs) Handles NoTargetOption.CheckedChanged
        If Not NoTargetOption.Checked Then Return
        ponyThumbnail = Nothing
        PointX.Enabled = False
        PointY.Enabled = False
        FollowComboBox.Enabled = False
        RedrawFollowPoint()
    End Sub

    Private Sub Follow_ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FollowComboBox.SelectedIndexChanged
        GetThumbnail()
    End Sub

    Private Sub GetThumbnail()
        If m_editor Is Nothing Then Return
        For Each ponyBase In m_editor.Ponies.Bases
            If ponyBase.Directory = CStr(FollowComboBox.SelectedItem) Then
                Try
                    ponyThumbnail = Image.FromFile(ponyBase.Behaviors(0).RightImage.Path)
                Catch ex As Exception
                    My.Application.NotifyUserOfNonFatalException(ex, "Failed to load image for pony " & ponyBase.Directory)
                End Try
                RedrawFollowPoint()
                Exit Sub
            End If
        Next

        For Each effect In m_editor.GetAllEffects()
            If effect.Name = StringToEffectName(CStr(FollowComboBox.SelectedItem)) Then
                Try
                    ponyThumbnail = Image.FromFile(effect.RightImage.Path)
                Catch ex As Exception
                    My.Application.NotifyUserOfNonFatalException(ex, "Failed to load image for effect " & effect.Name)
                End Try
                RedrawFollowPoint()
                Exit Sub
            End If
        Next
    End Sub

    Private Sub Point_Loc_ValueChanged(sender As Object, e As EventArgs) Handles PointX.ValueChanged, PointY.ValueChanged
        RedrawFollowPoint()
    End Sub

    Private Function StringToEffectName(name As String) As String
        For Each effect In m_editor.GetAllEffects()
            If (effect.ParentPonyBase.DisplayName & "'s " & effect.Name) = name Then
                Return effect.Name
            End If
        Next
        Return name
    End Function

    Private Sub Auto_Select_Images_Checkbox_CheckedChanged(sender As Object, e As EventArgs) Handles AutoSelectImageCheckbox.CheckedChanged
        If AutoSelectImageCheckbox.Checked = True Then
            MovingComboBox.Enabled = False
            StoppedComboBox.Enabled = False
        Else
            MovingComboBox.Enabled = True
            StoppedComboBox.Enabled = True
        End If
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click

        If AutoSelectImageCheckbox.Checked = False AndAlso
            (CStr(MovingComboBox.SelectedItem) = "" OrElse CStr(StoppedComboBox.SelectedItem) = "") Then
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
            behaviorToChange.OriginalFollowObjectName = StringToEffectName(CStr(FollowComboBox.SelectedItem))
        Else
            behaviorToChange.OriginalFollowObjectName = ""
        End If

        If AutoSelectImageCheckbox.Checked Then
            behaviorToChange.AutoSelectImagesOnFollow = True
        Else
            behaviorToChange.AutoSelectImagesOnFollow = False
            behaviorToChange.FollowMovingBehaviorName = CStr(MovingComboBox.SelectedItem)
            behaviorToChange.FollowStoppedBehaviorName = CStr(StoppedComboBox.SelectedItem)
        End If

        Me.Close()

    End Sub
End Class
