Public Class ImageCentersForm

    Dim previewpony As Pony
    Dim behavior_index As Integer = 0
    Dim animation_index As Integer = 0
    Dim max_frame As Integer = 0

    Dim left_previous_center As New Point
    Dim right_previous_center As New Point
    Dim right_center As New Point
    Dim left_center As New Point

    Dim right_image As Image
    Dim left_image As Image
    Dim left_graphics As Graphics = Nothing
    Dim right_graphics As Graphics = Nothing

    Dim right_image_framedimensions As Imaging.FrameDimension
    Dim right_image_framecount As Integer
    Dim left_image_framedimensions As Imaging.FrameDimension
    Dim left_image_framecount As Integer

    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        Icon = My.Resources.Twilight
        m_editor = editor
    End Sub

    Private Sub Image_Centers_Form_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        previewpony = m_editor.PreviewPony
        Me.Text = "Image Centering for " & previewpony.Directory

        behavior_index = 0
        animation_index = 0
        left_previous_center = Point.Empty
        right_previous_center = Point.Empty

        Left_ImageBox.Image = New Bitmap(Left_ImageBox.Width, Left_ImageBox.Height)
        Right_ImageBox.Image = New Bitmap(Right_ImageBox.Width, Right_ImageBox.Height)

        left_graphics = Graphics.FromImage(Left_ImageBox.Image)
        right_graphics = Graphics.FromImage(Right_ImageBox.Image)

        LoadBehavior(behavior_index)

    End Sub


    Sub RedrawMarker()
        If Not IsNothing(left_graphics) AndAlso Not IsNothing(right_graphics) Then

            If BG_Black_Radio.Checked = True Then
                right_graphics.Clear(Color.Black)
                left_graphics.Clear(Color.Black)
            Else
                right_graphics.Clear(Color.White)
                left_graphics.Clear(Color.White)
            End If


            right_graphics.DrawImage(right_image, New Point(0, 0))
            left_graphics.DrawImage(left_image, New Point(0, 0))

            left_graphics.DrawLine(New System.Drawing.Pen(Color.Red, 2), _
                                            New Point(left_center.X - 5, left_center.Y - 5), _
                                            New Point(left_center.X + 5, left_center.Y + 5))
            left_graphics.DrawLine(New System.Drawing.Pen(Color.Red, 2), _
                                           New Point(left_center.X + 5, left_center.Y - 5), _
                                           New Point(left_center.X - 5, left_center.Y + 5))
            right_graphics.DrawLine(New System.Drawing.Pen(Color.Red, 2), _
                                          New Point(right_center.X - 5, right_center.Y - 5), _
                                          New Point(right_center.X + 5, right_center.Y + 5))
            right_graphics.DrawLine(New System.Drawing.Pen(Color.Red, 2), _
                                           New Point(right_center.X + 5, right_center.Y - 5), _
                                           New Point(right_center.X - 5, right_center.Y + 5))

            Left_ImageBox.Invalidate()
            Right_ImageBox.Invalidate()

            left_center_label.Text = left_center.ToString()
            right_center_label.Text = right_center.ToString()

        End If
    End Sub

    Private Sub Left_ImageBox_Click(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Left_ImageBox.MouseClick

        left_center = e.Location
        previewpony.Behaviors(behavior_index).SetLeftImageCenter(left_center)
        RedrawMarker()

    End Sub

    Private Sub Right_ImageBox_Click(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Right_ImageBox.MouseClick

        right_center = e.Location
        previewpony.Behaviors(behavior_index).SetRightImageCenter(right_center)
        RedrawMarker()

    End Sub

    Sub LoadBehavior(ByVal index As Integer)

        animation_index = 0

        left_image = Image.FromFile(previewpony.Behaviors(index).LeftImagePath)
        right_image = Image.FromFile(previewpony.Behaviors(index).RightImagePath)

        Try
            left_image_framedimensions = New System.Drawing.Imaging.FrameDimension(left_image.FrameDimensionsList(0))
            left_image_framecount = left_image.GetFrameCount(left_image_framedimensions)
            left_image.SelectActiveFrame(left_image_framedimensions, animation_index)

            right_image_framedimensions = New System.Drawing.Imaging.FrameDimension(right_image.FrameDimensionsList(0))
            right_image_framecount = right_image.GetFrameCount(right_image_framedimensions)
            right_image.SelectActiveFrame(right_image_framedimensions, animation_index)
        Catch ex As Exception
            left_image_framedimensions = Nothing
            right_image_framedimensions = Nothing
            left_image_framecount = 0
            right_image_framecount = 0
        End Try

        left_center = previewpony.Behaviors(index).LeftImageCenter
        right_center = previewpony.Behaviors(index).RightImageCenter
        left_previous_center = left_center
        right_previous_center = right_center

        left_center_label.Text = left_center.ToString()
        right_center_label.Text = right_center.ToString()

        behavior_name_label.Text = previewpony.Behaviors(index).Name

        frame_label.Text = CStr(animation_index)

        If left_image_framecount < right_image_framecount Then
            max_frame = left_image_framecount
        Else
            max_frame = right_image_framecount
        End If

        frame_slider.Value = 0
        If max_frame = 0 Then
            frame_slider.Maximum = 0
        Else
            frame_slider.Maximum = max_frame - 1
        End If

        RedrawMarker()

    End Sub

    Private Sub Next_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Next_Button.Click

        If behavior_index = previewpony.Behaviors.Count - 1 Then
            behavior_index = 0
        Else
            behavior_index += 1
        End If

        LoadBehavior(behavior_index)

    End Sub

    Private Sub Prev_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Prev_Button.Click

        If behavior_index = 0 Then
            behavior_index = previewpony.Behaviors.Count - 1
        Else
            behavior_index -= 1
        End If

        LoadBehavior(behavior_index)

    End Sub

    Private Sub Frame_Slider_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles frame_slider.Scroll

        animation_index = frame_slider.Value
        frame_label.Text = CStr(animation_index)

        right_image.SelectActiveFrame(right_image_framedimensions, animation_index)
        left_image.SelectActiveFrame(left_image_framedimensions, animation_index)

        RedrawMarker()

    End Sub

    Private Sub Right_Image_Set_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Right_Image_Set_Button.Click
        right_center = right_previous_center
        previewpony.Behaviors(behavior_index).SetRightImageCenter(right_center)
        RedrawMarker()
    End Sub

    Private Sub Left_Image_Set_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Left_Image_Set_Button.Click
        left_center = left_previous_center
        previewpony.Behaviors(behavior_index).SetLeftImageCenter(left_center)
        RedrawMarker()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click
        Me.Close()
    End Sub

    Private Sub BG_Black_Radio_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles BG_Black_Radio.CheckedChanged
        RedrawMarker()
    End Sub
End Class