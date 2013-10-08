Imports DesktopSprites.SpriteManagement

Public Class PonySelectionControl
    Friend PonyBase As PonyBase
    Private _count As Integer
    Friend Property Count As Integer
        Get
            Return _count
        End Get
        Set(value As Integer)
            Argument.EnsureNonnegative(value, "value")
            If value > 99999 Then Throw New ArgumentOutOfRangeException("value", "value must be no more than 99999.")
            _count = value
            PonyCount.Text = _count.ToString(Globalization.CultureInfo.CurrentCulture)
        End Set
    End Property
    Friend PonyImage As AnimatedImage(Of BitmapFrame)
    Private _showPonyImage As Boolean
    Public Property ShowPonyImage As Boolean
        Get
            Return _showPonyImage
        End Get
        Set(value As Boolean)
            If _showPonyImage <> value Then
                _showPonyImage = value
                InvalidatePonyImageArea()
            End If
        End Set
    End Property
    Private imageSize As Size
    Private timeIndex As TimeSpan
    Private flip As Boolean

    Friend Sub New(ponyTemplate As PonyBase, imagePath As String, flipImage As Boolean)
        Argument.EnsureNotNull(ponyTemplate, "ponyTemplate")
        Argument.EnsureNotNull(imagePath, "imagePath")
        InitializeComponent()
        PonyBase = ponyTemplate
        PonyName.Text = PonyBase.Directory
        Try
            imageSize = DesktopSprites.Core.ImageSize.GetSize(imagePath)
        Catch ex As Exception
            ' Leave size empty if it cannot be determined.
        End Try
        imageSize = New Size(CInt(imageSize.Width * Options.ScaleFactor), CInt(imageSize.Height * Options.ScaleFactor))
        flip = flipImage

        Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadImage, imagePath)

        ResizeToFit()
    End Sub

    Private Sub LoadImage(imagePath As Object)
        Try
            PonyImage = New AnimatedImage(Of BitmapFrame)(
                                                       imagePath.ToString(), Function(file As String) New BitmapFrame(file),
                                                       BitmapFrame.FromBuffer, BitmapFrame.AllowableBitDepths)
            Threading.Thread.MemoryBarrier()
        Catch ex As Exception
            ' Do without an image if it failed to load.
        End Try

        If Disposing OrElse IsDisposed Then
            If PonyImage IsNot Nothing Then PonyImage.Dispose()
        Else
            If Not IsHandleCreated Then Return
            BeginInvoke(New MethodInvoker(Sub()
                                              ResizeToFit()
                                              If ShowPonyImage Then InvalidatePonyImageArea()
                                          End Sub))
        End If
    End Sub

    Public Sub AdvanceTimeIndex(amount As TimeSpan)
        If PonyImage Is Nothing Then
            timeIndex += amount
            Return
        End If

        Dim oldImage = PonyImage(timeIndex)
        timeIndex += amount
        Dim newImage = PonyImage(timeIndex)
        If Not Object.ReferenceEquals(oldImage, newImage) Then
            InvalidatePonyImageArea()
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

        If PonyImage IsNot Nothing Then imageSize =
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

    Private Sub InvalidatePonyImageArea()
        Invalidate(New Rectangle(0, 0, CInt(imageSize.Width * Options.ScaleFactor), CInt(imageSize.Height * Options.ScaleFactor)))
    End Sub

    Private Sub PonySelectionControl_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        If ShowPonyImage Then
            Dim image = PonyImage
            If image Is Nothing Then Return
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            image(timeIndex).Flip(flip)
            Dim bitmap = image(timeIndex).Image
            e.Graphics.DrawImage(bitmap, 0, 0, bitmap.Width * Options.ScaleFactor, bitmap.Height * Options.ScaleFactor)
        End If
    End Sub

    Private Sub MinusButton_Click(sender As Object, e As EventArgs) Handles MinusButton.Click
        Count -= 1
    End Sub

    Private Sub PlusButton_Click(sender As Object, e As EventArgs) Handles PlusButton.Click
        Count += 1
    End Sub

    Private Sub PonyCount_TextChanged(sender As Object, e As EventArgs) Handles PonyCount.TextChanged
        If PonyCount.Text <> "" Then
            _count = Integer.Parse(PonyCount.Text, Globalization.NumberStyles.None, Globalization.CultureInfo.CurrentCulture)
        Else
            _count = 0
        End If
        MinusButton.Enabled = _count > 0
        PlusButton.Enabled = _count < 99999
    End Sub

    Private Sub PonyCount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PonyCount.KeyPress
        e.Handled = Not (Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar))
    End Sub

    Private Sub PonyCount_Leave(sender As Object, e As EventArgs) Handles PonyCount.Leave
        If String.IsNullOrEmpty(PonyCount.Text) Then PonyCount.Text = "0"
    End Sub

    Private Sub PonySelectionControl_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged
        ' Force child controls to match parent state on Mac.
        If OperatingSystemInfo.IsMacOSX Then
            For Each control As Control In Controls
                control.Visible = Visible
            Next
            If PonyName.Text <> PonyBase.RandomDirectory Then
                NoDuplicates.Visible = False
            End If
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing Then
                If components IsNot Nothing Then components.Dispose()
                If PonyImage IsNot Nothing Then PonyImage.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
End Class
