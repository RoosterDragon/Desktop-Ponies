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

    Private _changesMade As Boolean
    Public ReadOnly Property ChangesMade As Boolean
        Get
            Return _changesMade
        End Get
    End Property

    Private base As PonyBase
    Public Sub New(ponyBase As PonyBase)
        base = Argument.EnsureNotNull(ponyBase, "ponyBase")
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

        leftCenter = If(behavior.LeftImage.CustomCenter, behavior.LeftImage.Center)
        rightCenter = If(behavior.RightImage.CustomCenter, behavior.RightImage.Center)
        leftPreviousCenter = leftCenter
        rightPreviousCenter = rightCenter

        LeftCenterLabel.Text = leftCenter.ToString()
        RightCenterLabel.Text = rightCenter.ToString()

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
    End Sub

    Private Sub RedrawMarker()
        If leftGraphics IsNot Nothing AndAlso rightGraphics IsNot Nothing Then

            Dim clearColor = If(BackgroundOptionBlack.Checked, Color.Black, Color.White)
            rightGraphics.Clear(clearColor)
            leftGraphics.Clear(clearColor)

            rightGraphics.DrawImage(rightImage, Point.Empty)
            leftGraphics.DrawImage(leftImage, Point.Empty)

            Using redPen As New Pen(Color.Red, 2)
                leftGraphics.DrawLine(redPen,
                                       New Point(leftCenter.X - 5, leftCenter.Y - 5),
                                       New Point(leftCenter.X + 5, leftCenter.Y + 5))
                leftGraphics.DrawLine(redPen,
                                       New Point(leftCenter.X + 5, leftCenter.Y - 5),
                                       New Point(leftCenter.X - 5, leftCenter.Y + 5))
                rightGraphics.DrawLine(redPen,
                                        New Point(rightCenter.X - 5, rightCenter.Y - 5),
                                        New Point(rightCenter.X + 5, rightCenter.Y + 5))
                rightGraphics.DrawLine(redPen,
                                        New Point(rightCenter.X + 5, rightCenter.Y - 5),
                                        New Point(rightCenter.X - 5, rightCenter.Y + 5))
            End Using

            LeftImageBox.Invalidate()
            RightImageBox.Invalidate()

            LeftCenterLabel.Text = leftCenter.ToString()
            RightCenterLabel.Text = rightCenter.ToString()
        End If
    End Sub

    Private Sub LeftImageBox_Click(sender As Object, e As MouseEventArgs) Handles LeftImageBox.MouseClick
        If base.Behaviors.Count = 0 Then Return
        _changesMade = True
        leftCenter = e.Location
        base.Behaviors(behaviorIndex).LeftImage.CustomCenter = leftCenter
        RedrawMarker()
    End Sub

    Private Sub RightImageBox_Click(sender As Object, e As MouseEventArgs) Handles RightImageBox.MouseClick
        If base.Behaviors.Count = 0 Then Return
        _changesMade = True
        rightCenter = e.Location
        base.Behaviors(behaviorIndex).RightImage.CustomCenter = rightCenter
        RedrawMarker()
    End Sub

    Private Sub NextButton_Click(sender As Object, e As EventArgs) Handles NextButton.Click
        behaviorIndex += 1
        If behaviorIndex >= base.Behaviors.Count Then behaviorIndex = 0
        LoadBehavior()
    End Sub

    Private Sub PreviousButton_Click(sender As Object, e As EventArgs) Handles PreviousButton.Click
        behaviorIndex -= 1
        If behaviorIndex <= -1 Then behaviorIndex = base.Behaviors.Count - 1
        LoadBehavior()
    End Sub

    Private Sub FrameSlider_Scroll(sender As Object, e As EventArgs) Handles FrameSlider.Scroll
        animationIndex = FrameSlider.Value
        FrameIndexLabel.Text = animationIndex.ToString(CultureInfo.CurrentCulture)

        rightImage.SelectActiveFrame(rightFrameDimension, animationIndex)
        leftImage.SelectActiveFrame(leftFrameDimension, animationIndex)

        RedrawMarker()
    End Sub

    Private Sub RightImageResetButton_Click(sender As Object, e As EventArgs) Handles RightImageResetButton.Click
        If base.Behaviors.Count = 0 Then Return
        _changesMade = True
        rightCenter = rightPreviousCenter
        base.Behaviors(behaviorIndex).RightImage.CustomCenter = rightCenter
        RedrawMarker()
    End Sub

    Private Sub LeftImageResetButton_Click(sender As Object, e As EventArgs) Handles LeftImageResetButton.Click
        If base.Behaviors.Count = 0 Then Return
        _changesMade = True
        leftCenter = leftPreviousCenter
        base.Behaviors(behaviorIndex).LeftImage.CustomCenter = leftCenter
        RedrawMarker()
    End Sub

    Private Sub BackgroundOptionBlack_CheckedChanged(sender As Object, e As EventArgs) Handles BackgroundOptionBlack.CheckedChanged
        RedrawMarker()
    End Sub

    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub
End Class