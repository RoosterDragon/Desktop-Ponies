Imports System.Runtime.InteropServices

Namespace Win32
    Friend Structure POINT
        Public X As Integer
        Public Y As Integer

        Public Sub New(_x As Integer, _y As Integer)
            X = _x
            Y = _y
        End Sub
    End Structure

    Friend Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer

        Public Sub New(_left As Integer, _top As Integer, _right As Integer, _bottom As Integer)
            Left = _left
            Top = _top
            Right = _right
            Bottom = _bottom
        End Sub
    End Structure

    Friend Module NativeMethods
        Public Declare Function WindowFromPoint Lib "user32.dll" (<[In]()> point As POINT) As IntPtr
        Public Declare Function GetWindowThreadProcessId Lib "user32.dll" _
            (<[In]()> hWnd As IntPtr, <[Out](), [Optional]()> ByRef lpdwProcessId As IntPtr) As UInteger
        <DllImport("user32.dll", SetLastError:=True)>
        Public Function GetWindowRect(<[In]()> hWnd As IntPtr, <Out()> ByRef lpRect As RECT) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function
        <System.Runtime.InteropServices.DllImport("user32")>
        Public Function GetKeyState(vKey As Integer) As Short
        End Function
    End Module
End Namespace
