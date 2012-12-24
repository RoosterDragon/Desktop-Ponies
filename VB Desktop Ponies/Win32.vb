Imports System.Runtime.InteropServices

Module Win32

    'Functions for detecting other windows - thanks to WindowPonies for this.
    Public Declare Function WindowFromPoint Lib "user32.dll" (point As Point) As IntPtr
    Public Declare Function GetWindowThreadProcessId Lib "user32.dll" (hWnd As IntPtr, Optional ByRef processId As Integer = 0) As Integer

    Private Declare Function GetForegroundWindow Lib "user32.dll" () As IntPtr
    Private Declare Function GetDesktopWindow Lib "user32.dll" () As IntPtr
    Private Declare Function GetShellWindow Lib "user32.dll" () As IntPtr

    'Converted to vb.net from http://www.richard-banks.org/2007/09/how-to-detect-if-another-application-is.html 

    Public Structure RECT
        Dim Left As Integer
        Dim Top As Integer
        Dim Right As Integer
        Dim Bottom As Integer
    End Structure

    <DllImport("user32.dll", SetLastError:=True)> _
    Public Function GetWindowRect(hwnd As IntPtr, ByRef rc As RECT) As Integer
    End Function

    Friend Function DetectFullScreenWindow() As Boolean
        If Not OperatingSystemInfo.IsWindows Then
            Return False
        End If

        'Now Broken.
        Return False

        'Detect if the current app is running in full screen
        Dim appBounds As RECT
        Dim screenBounds As Rectangle
        Dim hWnd As IntPtr

        'get the dimensions of the active window
        hWnd = GetForegroundWindow()

        'If Main.Instance.PonyViewer.Handle = hWnd Then
        '    Return False
        'End If

        If Not IsNothing(hWnd) AndAlso Not hWnd.Equals(IntPtr.Zero) Then
            'Check we haven't picked up the desktop or the shell
            If Not (hWnd.Equals(GetDesktopWindow()) OrElse hWnd.Equals(GetShellWindow())) Then
                GetWindowRect(hWnd, appBounds)
                'determine if window is full screen
                screenBounds = Screen.FromHandle(hWnd).Bounds
                If (appBounds.Bottom - appBounds.Top) = screenBounds.Height AndAlso
                    (appBounds.Right - appBounds.Left) = screenBounds.Width Then
                    Return True
                End If
            End If
        End If

        Return False

    End Function

End Module
