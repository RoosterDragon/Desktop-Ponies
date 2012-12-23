Imports CsDesktopPonies.SpriteManagement

Public Class PonySelectionControl
    Private ReadOnly initialDetailPanelHeight As Integer
    Friend PonyBase As PonyBase
    Friend PonyImage As AnimatedImage(Of BitmapFrame)
    Dim timeIndex As TimeSpan
    Dim flip As Boolean

    Friend Sub New(ponyTemplate As PonyBase, image As AnimatedImage(Of BitmapFrame), flipImage As Boolean)
        InitializeComponent()
        PonyBase = ponyTemplate
        PonyName.Text = PonyBase.Directory
        PonyImage = image
        flip = flipImage

        initialDetailPanelHeight = DetailPanel.Height
        ResizeToFit()
        Invalidate(New Rectangle(0, 0, CInt(PonyImage.Width * Options.ScaleFactor), CInt(PonyImage.Height * Options.ScaleFactor)))
    End Sub

    Public Sub AdvanceTimeIndex(amount As TimeSpan)
        Dim oldImage = PonyImage(timeIndex)
        timeIndex += amount
        Dim newImage = PonyImage(timeIndex)
        If Not Object.ReferenceEquals(oldImage, newImage) Then
            Invalidate(New Rectangle(0, 0, CInt(PonyImage.Width * Options.ScaleFactor), CInt(PonyImage.Height * Options.ScaleFactor)))
        End If
    End Sub

    Public Sub ResizeToFit()
        Dim borderWidth As Integer = 0
        Select Case BorderStyle
            Case BorderStyle.FixedSingle
                borderWidth = SystemInformation.FixedFrameBorderSize.Width
            Case BorderStyle.Fixed3D
                borderWidth = SystemInformation.Border3DSize.Width
        End Select

        Dim imageSize = Size.Empty
        If Not IsNothing(PonyImage) Then imageSize =
            New Size(CInt(PonyImage.Width * Options.ScaleFactor), CInt(PonyImage.Height * Options.ScaleFactor))

        Dim nameWidth = TextRenderer.MeasureText(PonyName.Text, PonyName.Font).Width + PonyName.Margin.Horizontal
        DetailPanel.Width = Math.Max(nameWidth, DetailPanel.MinimumSize.Width)
        Width = imageSize.Width + DetailPanel.Width + borderWidth
        Height = Math.Max(imageSize.Height, DetailPanel.MinimumSize.Height) + borderWidth
        DetailPanel.Location = New Point(Width - DetailPanel.Width, 0)
    End Sub

    Private Sub DetailPanel_ChildControl_VisibleChanged(sender As Object, e As EventArgs) Handles PonyName.VisibleChanged, PonyCountLabel.VisibleChanged, PonyCount.VisibleChanged
        ResizeToFit()
    End Sub

    Private Sub PonySelectionControl_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        PonyImage(timeIndex).Flip(flip)
        e.Graphics.DrawImage(PonyImage(timeIndex).Image, 0, 0, PonyImage.Width * Options.ScaleFactor, PonyImage.Height * Options.ScaleFactor)
    End Sub

    Private Sub MinusButton_Click(sender As Object, e As EventArgs) Handles MinusButton.Click
        Dim count As Integer
        If Integer.TryParse(PonyCount.Text, count) Then PonyCount.Text = CStr((count - 1))
    End Sub

    Private Sub PlusButton_Click(sender As Object, e As EventArgs) Handles PlusButton.Click
        Dim count As Integer
        If Integer.TryParse(PonyCount.Text, count) Then PonyCount.Text = CStr((count + 1))
    End Sub

    Private Sub PonyCount_TextChanged(sender As Object, e As EventArgs) Handles PonyCount.TextChanged
        Dim count As Integer
        Dim parsed = Integer.TryParse(PonyCount.Text, count)
        If parsed AndAlso count = 0 Then
            MinusButton.Enabled = False
        Else
            MinusButton.Enabled = True
        End If
    End Sub

    Private Sub PonyCount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PonyCount.KeyPress
        e.Handled = Not (Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar))
    End Sub

    Private Sub PonyCount_Leave(sender As Object, e As EventArgs) Handles PonyCount.Leave
        If String.IsNullOrEmpty(PonyCount.Text) Then PonyCount.Text = "0"
    End Sub
End Class
