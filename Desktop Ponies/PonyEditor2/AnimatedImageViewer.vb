Imports System.ComponentModel
Imports DesktopSprites.SpriteManagement

Public Class AnimatedImageViewer
    <Description("The time index into the animated image.")>
    <DefaultValue(0)>
    Protected Property Time As TimeSpan
    Private WithEvents animationTimer As New Timer() With {.Interval = 33}
    Private _image As AnimatedImage(Of BitmapFrame)
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Image As AnimatedImage(Of BitmapFrame)
        Get
            Return _image
        End Get
        Set(value As AnimatedImage(Of BitmapFrame))
            _image = value
            Time = TimeSpan.Zero
            If AutoSize Then Size = PreferredSize
            Invalidate()
        End Set
    End Property
    <Description("Indicates whether the image should be animated, or else displayed statically.")>
    <DefaultValue(True)>
    Public Property Animate As Boolean
        Get
            Return animationTimer.Enabled
        End Get
        Set(value As Boolean)
            animationTimer.Enabled = value
        End Set
    End Property
    Private oldSize As Size

    Public Sub ShowError(message As String)
        ErrorLabel.Text = message
        ErrorLabel.Visible = True
        Dim oldDock = Dock
        Dock = DockStyle.Fill
        Dim targetSize = ErrorLabel.GetPreferredSize(Size)
        Dock = oldDock
        oldSize = Size
        targetSize.Width += Margin.Horizontal
        targetSize.Height += Margin.Vertical
        Size = targetSize
    End Sub

    Public Sub ClearError()
        If ErrorLabel.Visible Then
            ErrorLabel.Visible = False
            Size = oldSize
        End If
    End Sub

    Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
        If ErrorLabel.Visible Then
            Return ErrorLabel.GetPreferredSize(proposedSize)
        ElseIf Image IsNot Nothing Then
            Return Image.Size
        Else
            Return Size.Empty
        End If
    End Function

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        PaintImageInCenter(Image, e)
    End Sub

    Protected Sub PaintImageInCenter(image As AnimatedImage(Of BitmapFrame), e As PaintEventArgs)
        Argument.EnsureNotNull(e, "e")
        If image Is Nothing Then Return

        Dim loopTime = TimeSpan.FromMilliseconds(image.ImageDuration)
        If Time > loopTime Then Time -= loopTime

        Dim bitmap = image(Time).Image
        Dim controlCenter = New Size(CInt(Width / 2), CInt(Height / 2))
        Dim imageCenter = New Size(CInt(bitmap.Width / 2), CInt(bitmap.Height / 2))
        Dim location = New Point(controlCenter.Width - imageCenter.Width, controlCenter.Height - imageCenter.Height)
        e.Graphics.DrawImageUnscaled(bitmap, location)
        e.Graphics.DrawRectangle(Pens.LightGray, location.X - 1, location.Y - 1, bitmap.Width + 1, bitmap.Height + 1)
    End Sub

    Private Sub animationTimer_Tick(sender As Object, e As EventArgs) Handles animationTimer.Tick
        Time += TimeSpan.FromMilliseconds(animationTimer.Interval)
        Invalidate()
    End Sub

    Private Sub AnimatedImageViewer_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        animationTimer.Dispose()
    End Sub
End Class
