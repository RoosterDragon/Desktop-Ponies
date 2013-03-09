''' <summary>
''' Finds out what keys are currently pressed.
''' </summary>
Public Module KeyboardState
    Public Function IsKeyPressed(key As Keys) As Boolean
        If Not OperatingSystemInfo.IsWindows Then
            Return False
        End If

        Return HighBitSet(Win32.GetKeyState(CInt(key)))
    End Function

    Private Function HighBitSet(keyState As Short) As Boolean
        Return (keyState And &H80) = &H80
    End Function
End Module