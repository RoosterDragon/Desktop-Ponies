Imports System.Globalization

Public Class ImageCentersForm
    Private behaviorIndex As Integer
    Private animationIndex As Integer
    Private maxFrameIndex As Integer

    Private leftPreviousCenter As Point
    Private rightPreviousCenter As Point
    Private rightCenter As Point
    Private leftCenter As Point

    Private rightImage As Image
    Private leftImage As Image
    Private leftGraphics As Graphics
    Private rightGraphics As Graphics

    Private rightFrameDimension As Imaging.FrameDimension
    Private rightFrameCount As Integer
    Private leftFrameDimension As Imaging.FrameDimension
    Private leftFrameCount As Integer

    Private changingBehavior As Boolean
    Private _changesMade As Boolean
    Public ReadOnly Property ChangesMade As Boolean
        Get
            Return _changesMade
        End Get
    End Property

    Private base As PonyBase
    Public Sub New(ponyBase As PonyBase)
        base = Argument.EnsureNotNull(ponyBase, "ponyBase")
        If base.Behaviors.Count = 0 Then Throw New ArgumentException("ponyBase must contain at least one behavior.", "ponyBase")
        InitializeComponent()
        Icon = My.Resources.Twilight
        Text = "Image Centering for " & base.Directory
    End Sub

    Private Sub ImageCentersForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        LeftImageBox.Image = New Bitmap(LeftImageBox.Width, LeftImageBox.Height)
        RightImageBox.Image = New Bitmap(RightImageBox.Width, RightImageBox.Height)

        leftGraphics = Graphics.FromImage(LeftImageBox.Image)
        rightGraphics = Graphics.FromImage(RightImageBox.Image)

        LoadBehavior()
    End Sub

    Private Sub LoadBehavior()
        If base.Behaviors.Count = 0 Then Return
        changingBehavior = True

        animationIndex = 0

        Dim behavior = base.Behaviors(behaviorIndex)

        leftImage = Image.FromFile(behavior.LeftImage.Path)
        rightImage = Image.FromFile(behavior.RightImage.Path)

        Try
            leftFrameDimension = New Imaging.FrameDimension(leftImage.FrameDimensionsList(0))
            leftFrameCount = leftImage.GetFrameCount(leftFrameDimension)
            leftImage.SelectActiveFrame(leftFrameDimension, animationIndex)

            rightFrameDimension = New Imaging.FrameDimension(rightImage.FrameDimensionsList(0))
            rightFrameCount = rightImage.GetFrameCount(rightFrameDimension)
            rightImage.SelectActiveFrame(rightFrameDimension, animationIndex)
        Catch ex As Exception
            leftFrameDimension = Nothing
            rightFrameDimension = Nothing
            leftFrameCount = 0
            rightFrameCount = 0
        End Try

        leftCenter = behavior.LeftImage.Center
        rightCenter = behavior.RightImage.Center
        leftPreviousCenter = leftCenter
        rightPreviousCenter = rightCenter

        LeftCenterX.Value = leftCenter.X
        LeftCenterY.Value = leftCenter.Y
        RightCenterX.Value = rightCenter.X
        RightCenterY.Value = rightCenter.Y

        BehaviorNameLabel.Text = behavior.Name

        FrameIndexLabel.Text = animationIndex.ToString(CultureInfo.CurrentCulture)

        maxFrameIndex = Math.Min(leftFrameCount, rightFrameCount)

        FrameSlider.Value = 0
        If maxFrameIndex = 0 Then
            FrameSlider.Maximum = 0
        Else
            FrameSlider.Maximum = maxFrameIndex - 1
        End If

        RedrawMarker()

        changingBehavior = False
    End Sub

    Private Sub RedrawMarker()
        If leftGraphics IsNot Nothing AndAlso rightGraphics IsNot Nothing Then

            Dim clearColor = If(BackgroundOptionBlack.Checked, Color.Black, Color.White)
            rightGraphics.Clear(clearColor)
            leftGraphics.Clear(clearColor)

            Using redPen As New Pen(Color.FromArgb(196, Color.Red))
                Dim left = New Point(CInt(LeftImageBox.Width / 2 - leftImage.Width / 2),
                                     CInt(LeftImageBox.Height / 2 - leftImage.Height / 2))
                leftGraphics.DrawImage(leftImage, left)
                left += New Size(leftCenter)
                leftGraphics.DrawLine(redPen,
                                      New Point(left.X - 5, left.Y),
                                      New Point(left.X - 1, left.Y))
                leftGraphics.DrawLine(redPen,
                                      New Point(left.X + 1, left.Y),
                                      New Point(left.X + 5, left.Y))
                leftGraphics.DrawLine(redPen,
                                      New Point(left.X, left.Y - 5),
                                      New Point(left.X, left.Y - 1))
                leftGraphics.DrawLine(redPen,
                                      New Point(left.X, left.Y + 1),
                                      New Point(left.X, left.Y + 5))
                Dim right = New Point(CInt(RightImageBox.Width / 2 - rightImage.Width / 2),
                                      CInt(RightImageBox.Height / 2 - rightImage.Height / 2))
                rightGraphics.DrawImage(rightImage, right)
                right += New Size(rightCenter)
                rightGraphics.DrawLine(redPen,
                                       New Point(right.X - 5, right.Y),
                                       New Point(right.X - 1, right.Y))
                rightGraphics.DrawLine(redPen,
                                       New Point(right.X + 1, right.Y),
                                       New Point(right.X + 5, right.Y))
                rightGraphics.DrawLine(redPen,
                                       New Point(right.X, right.Y - 5),
                                       New Point(right.X, right.Y - 1))
                rightGraphics.DrawLine(redPen,
                                       New Point(right.X, right.Y + 1),
                                       New Point(right.X, right.Y + 5))
            End Using

            LeftImageBox.Invalidate()
            RightImageBox.Invalidate()
        End If
    End Sub

    Private Sub ImageBox_SizeChanged(sender As Object, e As EventArgs) Handles LeftImageBox.SizeChanged, RightImageBox.SizeChanged
        RedrawMarker()
    End Sub

    Private Sub LeftImageBox_Click(sender As Object, e As MouseEventArgs) Handles LeftImageBox.MouseClick
        SetLeftCenter(e.Location -
                      New Size(CInt(LeftImageBox.Width / 2), CInt(LeftImageBox.Height / 2)) +
                      New Size(CInt(leftImage.Width / 2), CInt(leftImage.Height / 2)))
    End Sub

    Private Sub RightImageBox_Click(sender As Object, e As MouseEventArgs) Handles RightImageBox.MouseClick
        SetRightCenter(e.Location -
                       New Size(CInt(RightImageBox.Width / 2), CInt(RightImageBox.Height / 2)) +
                       New Size(CInt(rightImage.Width / 2), CInt(rightImage.Height / 2)))
    End Sub

    Private Sub LeftCenter_ValueChanged(sender As Object, e As EventArgs) Handles LeftCenterX.ValueChanged, LeftCenterY.ValueChanged
        SetLeftCenter(New Point(CInt(LeftCenterX.Value), CInt(LeftCenterY.Value)))
    End Sub

    Private Sub RightCenter_ValueChanged(sender As Object, e As EventArgs) Handles RightCenterX.ValueChanged, RightCenterY.ValueChanged
        SetRightCenter(New Point(CInt(RightCenterX.Value), CInt(RightCenterY.Value)))
    End Sub

    Private Sub LeftImageResetButton_Click(sender As Object, e As EventArgs) Handles LeftImageResetButton.Click
        SetLeftCenter(leftPreviousCenter)
    End Sub

    Private Sub RightImageResetButton_Click(sender As Object, e As EventArgs) Handles RightImageResetButton.Click
        SetRightCenter(rightPreviousCenter)
    End Sub

    Private Sub LeftImageMirrorButton_Click(sender As Object, e As EventArgs) Handles LeftImageMirrorButton.Click
        SetRightCenter(New Point(leftImage.Width - 1 - leftCenter.X, leftCenter.Y))
    End Sub

    Private Sub RightImageMirrorButton_Click(sender As Object, e As EventArgs) Handles RightImageMirrorButton.Click
        SetLeftCenter(New Point(rightImage.Width - 1 - rightCenter.X, rightCenter.Y))
    End Sub

    Private Sub SetLeftCenter(center As Point)
        If base.Behaviors.Count = 0 OrElse changingBehavior Then Return
        _changesMade = True
        leftCenter = center
        base.Behaviors(behaviorIndex).LeftImage.CustomCenter = leftCenter
        LeftCenterX.Value = center.X
        LeftCenterY.Value = center.Y
        RedrawMarker()
    End Sub

    Private Sub SetRightCenter(center As Point)
        If base.Behaviors.Count = 0 OrElse changingBehavior Then Return
        _changesMade = True
        rightCenter = center
        base.Behaviors(behaviorIndex).RightImage.CustomCenter = rightCenter
        RightCenterX.Value = center.X
        RightCenterY.Value = center.Y
        RedrawMarker()
    End Sub

    Private Sub PreviousButton_Click(sender As Object, e As EventArgs) Handles PreviousButton.Click
        behaviorIndex -= 1
        If behaviorIndex <= -1 Then behaviorIndex = base.Behaviors.Count - 1
        LoadBehavior()
    End Sub

    Private Sub NextButton_Click(sender As Object, e As EventArgs) Handles NextButton.Click
        behaviorIndex += 1
        If behaviorIndex >= base.Behaviors.Count Then behaviorIndex = 0
        LoadBehavior()
    End Sub

    Private Sub BackgroundOptionBlack_CheckedChanged(sender As Object, e As EventArgs) Handles BackgroundOptionBlack.CheckedChanged
        RedrawMarker()
    End Sub

    Private Sub FrameSlider_Scroll(sender As Object, e As EventArgs) Handles FrameSlider.Scroll
        animationIndex = FrameSlider.Value
        FrameIndexLabel.Text = animationIndex.ToString(CultureInfo.CurrentCulture)

        rightImage.SelectActiveFrame(rightFrameDimension, animationIndex)
        leftImage.SelectActiveFrame(leftFrameDimension, animationIndex)

        RedrawMarker()
    End Sub

    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Close()
    End Sub
End Class