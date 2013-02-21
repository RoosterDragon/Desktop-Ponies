Public Class FollowTargetDialog

    Dim PreviewPoint As Point = New Point(0, 0)
    Dim Point_Preview_Graphics As Graphics = Nothing
    Dim pony_thumbnail As Image = Nothing
    Friend behavior_to_change As PonyBase.Behavior = Nothing

    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        m_editor = editor
    End Sub

    Private Sub Loc_X_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Point_Loc_X.ValueChanged
        If IsNothing(Point_Preview_Image.Image) Then Exit Sub

        SetPreviewPoint()

        RedrawFollowPoint()
    End Sub

    Private Sub Loc_Y_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Point_Loc_Y.ValueChanged
        If IsNothing(Point_Preview_Image.Image) Then Exit Sub

        SetPreviewPoint()

        RedrawFollowPoint()
    End Sub

    Sub SetPreviewPoint()

        'do X
        If Pony_Radio.Checked Then
            PreviewPoint = New Point(CInt((Point_Preview_Image.Image.Size.Width / 2) + Point_Loc_X.Value), _
                         CInt((Point_Preview_Image.Image.Size.Height / 2) + Point_Loc_Y.Value))
        Else
            PreviewPoint = New Point(CInt((0.01 * Point_Loc_X.Value) * Point_Preview_Image.Image.Size.Width), PreviewPoint.Y)
        End If

        'do Y
        If Pony_Radio.Checked Then
            PreviewPoint = New Point(CInt((Point_Preview_Image.Image.Size.Width / 2) + Point_Loc_X.Value), _
                         CInt((Point_Preview_Image.Image.Size.Height / 2) + Point_Loc_Y.Value))
        Else
            PreviewPoint = New Point(PreviewPoint.X, CInt((Point_Loc_Y.Value * 0.01) * Point_Preview_Image.Image.Size.Height))
        End If
    End Sub

    Sub RedrawFollowPoint()
        If Not IsNothing(Point_Preview_Graphics) Then

            SetPreviewPoint()

            Point_Preview_Graphics.Clear(Color.White)

            If Not IsNothing(pony_thumbnail) Then
                Point_Preview_Graphics.DrawImage(pony_thumbnail, New Point(
                                                 CInt((Point_Preview_Image.Image.Size.Width / 2) - (pony_thumbnail.Size.Width / 2)),
                                                 CInt((Point_Preview_Image.Image.Size.Height / 2) - (pony_thumbnail.Size.Height / 2))))
            End If
            Using pen As New System.Drawing.Pen(Color.Red, 2)
                Point_Preview_Graphics.DrawLine(pen,
                                                New Point(PreviewPoint.X - 5, PreviewPoint.Y - 5),
                                                New Point(PreviewPoint.X + 5, PreviewPoint.Y + 5))
                Point_Preview_Graphics.DrawLine(pen,
                                               New Point(PreviewPoint.X + 5, PreviewPoint.Y - 5),
                                               New Point(PreviewPoint.X - 5, PreviewPoint.Y + 5))
            End Using
            Point_Preview_Image.Invalidate()
        End If
    End Sub


    Private Sub Form_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        If IsNothing(behavior_to_change) Then
            Throw New InvalidOperationException("Behavior to modify is not set.  Call Change_Behavior() first before loading this form.")
        End If

        Dim bitmap As New Bitmap(Point_Preview_Image.Size.Width, Point_Preview_Image.Size.Height)

        Point_Preview_Image.Image = bitmap

        Point_Preview_Graphics = Graphics.FromImage(Point_Preview_Image.Image)

        RedrawFollowPoint()

    End Sub

    Friend Sub Change_Behavior(ByVal behavior As PonyBase.Behavior)
        Argument.EnsureNotNull(behavior, "behavior")

        behavior_to_change = behavior

        Follow_ComboBox.Items.Clear()

        Dim effects_list = PonyEditor.get_effect_list()

        For Each Available_Pony In Main.Instance.SelectablePonies
            Follow_ComboBox.Items.Add(Available_Pony.Directory)
        Next

        For Each effect In effects_list
            Follow_ComboBox.Items.Add(effect.Owning_Pony.Name & "'s " & effect.Name)
        Next

        moving_behavior_box.Items.Clear()
        stopped_behavior_box.Items.Clear()

        For Each otherbehavior In m_editor.PreviewPony.Behaviors
            moving_behavior_box.Items.Add(otherbehavior.Name)
            stopped_behavior_box.Items.Add(otherbehavior.Name)
        Next

        If behavior.AutoSelectImagesOnFollow = False Then
            Auto_Select_Images_Checkbox.Checked = False
        Else
            Auto_Select_Images_Checkbox.Checked = True
        End If

        Dim point As Boolean = False
        Dim pony As Boolean = False

        If (behavior.OriginalDestinationXCoord <> 0 OrElse behavior.OriginalDestinationYCoord <> 0) Then
            point = True
        End If

        If (Not IsNothing(behavior.OriginalFollowObjectName) AndAlso behavior.OriginalFollowObjectName <> "") Then
            pony = True
            For Each item As String In Follow_ComboBox.Items
                If String.Equals(string_to_effectname(item), behavior_to_change.OriginalFollowObjectName, StringComparison.OrdinalIgnoreCase) Then
                    Follow_ComboBox.SelectedItem = item
                End If
            Next
        End If

        If (Not IsNothing(behavior.FollowMovingBehavior) AndAlso behavior.FollowMovingBehaviorName <> "") Then
            For Each item As String In moving_behavior_box.Items
                If String.Equals(item, behavior.FollowMovingBehaviorName, StringComparison.OrdinalIgnoreCase) Then
                    moving_behavior_box.SelectedItem = item
                End If
            Next
        End If

        If (Not IsNothing(behavior.FollowStoppedBehavior) AndAlso behavior.FollowStoppedBehaviorName <> "") Then
            For Each item As String In stopped_behavior_box.Items
                If String.Equals(item, behavior.FollowStoppedBehaviorName, StringComparison.OrdinalIgnoreCase) Then
                    stopped_behavior_box.SelectedItem = item
                End If
            Next
        End If

        If pony AndAlso Not point Then
            Pony_Radio.Checked = True
            PreviewPoint = (New Point(CInt((0.01 * 50) * Point_Preview_Image.Size.Width), CInt((50 * 0.01) * Point_Preview_Image.Size.Height)))
        End If

        If point AndAlso Not pony Then
            Point_Radio.Checked = True
            Point_Loc_X.Value = behavior.OriginalDestinationXCoord
            Point_Loc_Y.Value = behavior.OriginalDestinationYCoord
        End If

        If pony AndAlso point Then
            Pony_Radio.Checked = True
            Point_Loc_X.Value = behavior.OriginalDestinationXCoord
            Point_Loc_Y.Value = behavior.OriginalDestinationYCoord
        End If

        If Not pony AndAlso Not point Then
            Pony_Radio.Checked = True
            pony_thumbnail = Nothing
            PreviewPoint = (New Point(
                            CInt((0.01 * 50) * Point_Preview_Image.Size.Width),
                            CInt((50 * 0.01) * Point_Preview_Image.Size.Height)))
        End If

        Me.Text = "Select following parameters for " & behavior.Name

    End Sub


    Private Sub Pony_Radio_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Pony_Radio.CheckedChanged, Point_Radio.CheckedChanged

        Point_Loc_X.Enabled = True
        Point_Loc_Y.Enabled = True

        If Pony_Radio.Checked Then
            Point_Loc_X.Maximum = 500
            Point_Loc_X.Minimum = -500
            Point_Loc_X.Value = 0
            Point_Loc_Y.Maximum = 500
            Point_Loc_Y.Minimum = -500
            Point_Loc_Y.Value = 0
            Follow_ComboBox.Enabled = True
            relativeto_label.Text = "(Relative to pony/effect center)"
            units_label.Text = "Location (X/Y) (in pixels)"
        Else
            If Point_Radio.Checked Then

                pony_thumbnail = Nothing
                Point_Loc_X.Maximum = 100
                Point_Loc_X.Minimum = 0
                Point_Loc_X.Value = 50
                Point_Loc_Y.Maximum = 100
                Point_Loc_Y.Minimum = 0
                Point_Loc_Y.Value = 50
                Follow_ComboBox.Enabled = False
                relativeto_label.Text = "(Relative to top right of screen)"
                units_label.Text = "Location (X/Y) (in % of screen height/width)"

            Else

                Follow_ComboBox.Enabled = False
                Point_Loc_X.Enabled = False
                Point_Loc_Y.Enabled = False

            End If

        End If

    End Sub


    Private Sub Follow_ComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Follow_ComboBox.SelectedIndexChanged

        For Each Pony In Main.Instance.SelectablePonies
            If Pony.Directory = CStr(Follow_ComboBox.SelectedItem) Then
                Try
                    pony_thumbnail = Image.FromFile(Pony.Behaviors(0).RightImagePath)
                Catch ex As Exception
                    MsgBox("Note:  Failed to load image for pony " & Pony.Directory)
                End Try

                RedrawFollowPoint()
                Exit Sub
            End If
        Next

        For Each effect In PonyEditor.get_effect_list()
            If effect.Name = string_to_effectname(CStr(Follow_ComboBox.SelectedItem)) Then
                Try
                    pony_thumbnail = Image.FromFile(effect.right_image_path)
                Catch ex As Exception
                    MsgBox("Note:  Failed to load image for effect " & effect.Name)
                End Try
                RedrawFollowPoint()
                Exit Sub
            End If
        Next
    End Sub


    Private Shared Function string_to_effectname(ByVal name As String) As String
        For Each effect In PonyEditor.get_effect_list()
            If (effect.Owning_Pony.Name & "'s " & effect.Name) = name Then
                Return effect.Name
            End If
        Next
        Return name
    End Function

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click

        If Auto_Select_Images_Checkbox.Checked = False AndAlso (CStr(moving_behavior_box.SelectedItem) = "" OrElse CStr(stopped_behavior_box.SelectedItem) = "") Then
            MsgBox("If you disable auto-selection of images, then you must specifiy a moving behavior and a stopped behavior to get the images from." _
                   & ControlChars.NewLine & "(You can select the behavior you are editing once you have saved it first.)")
            Exit Sub
        End If

        If Not DisableRadio.Checked Then
            behavior_to_change.OriginalDestinationXCoord = CInt(Point_Loc_X.Value)
            behavior_to_change.OriginalDestinationYCoord = CInt(Point_Loc_Y.Value)
        Else
            behavior_to_change.OriginalDestinationXCoord = 0
            behavior_to_change.OriginalDestinationYCoord = 0
        End If

        If Pony_Radio.Checked Then
            behavior_to_change.OriginalFollowObjectName = string_to_effectname(CStr(Follow_ComboBox.SelectedItem))
        Else
            behavior_to_change.OriginalFollowObjectName = ""
        End If

        If Auto_Select_Images_Checkbox.Checked = True Then
            behavior_to_change.AutoSelectImagesOnFollow = True
        Else
            behavior_to_change.AutoSelectImagesOnFollow = False
            behavior_to_change.FollowMovingBehaviorName = CStr(moving_behavior_box.SelectedItem)
            behavior_to_change.FollowStoppedBehaviorName = CStr(stopped_behavior_box.SelectedItem)
        End If

        Me.Close()

    End Sub

    Private Sub Auto_Select_Images_Checkbox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Auto_Select_Images_Checkbox.CheckedChanged
        If Auto_Select_Images_Checkbox.Checked = True Then
            moving_behavior_box.Enabled = False
            stopped_behavior_box.Enabled = False
        Else
            moving_behavior_box.Enabled = True
            stopped_behavior_box.Enabled = True
        End If
    End Sub
End Class
