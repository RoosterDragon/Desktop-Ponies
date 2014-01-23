Imports DesktopSprites.SpriteManagement

Public NotInheritable Class GifProcessing
    Private Sub New()
    End Sub
    ''' <summary>
    ''' Attempts to downscale the image by a factor of 2, if this is a lossless transformation. This reduces the memory required. This
    ''' processing step exists because most sprites in Desktop Ponies are aligned to a 2x2 pixel grid as part of the style, so we can often
    ''' save memory for these specific images
    ''' </summary>
    ''' <param name="buffer">The values that make up the image.</param>
    ''' <param name="palette">The color palette for the image.</param>
    ''' <param name="transparentIndex">The index of the transparent color, or null to indicate no transparency.</param>
    ''' <param name="stride">The stride width, in bytes, of the buffer.</param>
    ''' <param name="width">The logical width of the image the buffer contains.</param>
    ''' <param name="height">The logical height of the image the buffer contains.</param>
    ''' <param name="depth">The bit depth of the buffer (either 1, 2, 4 or 8).</param>
    Public Shared Sub LosslessDownscale(ByRef buffer As Byte(), ByRef palette As RgbColor(), ByRef transparentIndex As Byte?,
                                        ByRef stride As Integer, ByRef width As Integer, ByRef height As Integer, ByRef depth As Byte)
        ' Need image dimensions to be multiples of 2.
        If width Mod 2 <> 0 OrElse height Mod 2 <> 0 Then Return
        ' Most images are 8bbp or 4bbp encoded. There's no point handling the others.
        If depth = 8 Then
            LosslessDownscale8bbp(buffer, stride, width, height)
        ElseIf depth = 4 Then
            LosslessDownscale4bbp(buffer, stride, width, height)
        End If
    End Sub
    ''' <summary>
    ''' Attempts to downscale an 8bbp image by a factor of 2, if this is a lossless transformation.
    ''' </summary>
    ''' <param name="buffer">The values that make up the image.</param>
    ''' <param name="stride">The stride width, in bytes, of the buffer.</param>
    ''' <param name="width">The logical width of the image the buffer contains.</param>
    ''' <param name="height">The logical height of the image the buffer contains.</param>
    Private Shared Sub LosslessDownscale8bbp(ByRef buffer As Byte(), ByRef stride As Integer,
                                             ByRef width As Integer, ByRef height As Integer)
        ' Ensure each 2x2 block contains the same value.
        Dim yMax = height - 1
        For y = 0 To yMax - 1 Step 2
            Dim currentRow = y * stride
            Dim nextRow = (y + 1) * stride
            Dim xMax = stride - 1
            For x = 0 To xMax - 1 Step 2
                Dim xPlusOne = x + 1
                Dim topLeft = buffer(currentRow + x)
                If topLeft <> buffer(currentRow + xPlusOne) OrElse
                    topLeft <> buffer(nextRow + x) OrElse
                    topLeft <> buffer(nextRow + xPlusOne) Then Return
            Next
        Next

        ' We can downscale this image!
        Dim newHeight = CInt(height / 2)
        Dim newWidth = CInt(width / 2)
        Dim newBuffer(newWidth * newHeight - 1) As Byte
        For y = 0 To newHeight - 1
            Dim newBufferRow = y * newWidth
            Dim bufferRow = y * 2 * stride
            For x = 0 To newWidth - 1
                newBuffer(newBufferRow + x) = buffer(bufferRow + x * 2)
            Next
        Next

        ' Override with new data.
        buffer = newBuffer
        stride = CInt(stride / 2)
        height = newHeight
        width = newWidth
    End Sub
    ''' <summary>
    ''' Attempts to downscale a 4bbp image by a factor of 2, if this is a lossless transformation.
    ''' </summary>
    ''' <param name="buffer">The values that make up the image.</param>
    ''' <param name="stride">The stride width, in bytes, of the buffer.</param>
    ''' <param name="width">The logical width of the image the buffer contains.</param>
    ''' <param name="height">The logical height of the image the buffer contains.</param>
    Private Shared Sub LosslessDownscale4bbp(ByRef buffer As Byte(), ByRef stride As Integer,
                                             ByRef width As Integer, ByRef height As Integer)
        ' Ensure each 2x2 block contains the same value.
        Dim yMax = height - 1
        For y = 0 To yMax - 1 Step 2
            Dim currentRow = y * stride
            Dim nextRow = (y + 1) * stride
            For x = 0 To stride - 1
                Dim top = buffer(currentRow + x)
                Dim bottom = buffer(nextRow + x)
                Dim topLeft = top >> 4
                If topLeft <> (top And &HF) OrElse
                    topLeft <> bottom >> 4 OrElse
                    topLeft <> (bottom And &HF) Then Return
            Next
        Next

        ' We can downscale this image!
        Dim newStride = CInt(Math.Ceiling(stride / 2.0F))
        Dim newHeight = CInt(height / 2)
        Dim newWidth = CInt(width / 2)
        Dim aligned = newWidth Mod 2 = 0
        Dim newBuffer(newWidth * newHeight - 1) As Byte
        For y = 0 To newHeight - 1
            Dim newBufferRow = y * newStride
            Dim bufferRow = y * 2 * stride
            For x = 0 To newStride - 1
                Dim oldIndex = bufferRow + x * 2
                Dim a = buffer(oldIndex) >> 4
                Dim b = 0
                If aligned OrElse x * 2 + 1 < newWidth Then b = buffer(oldIndex + 1) >> 4
                newBuffer(newBufferRow + x) = CByte(a << 4 Or b)
            Next
        Next

        ' Override with new data.
        buffer = newBuffer
        stride = newStride
        height = newHeight
        width = newWidth
    End Sub
End Class
