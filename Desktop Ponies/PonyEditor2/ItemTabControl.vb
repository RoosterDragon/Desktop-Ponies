Public Class ItemTabControl
    Inherits TabControl

    Private Const ButtonPadding As Integer = 15

    Protected Overrides Sub OnControlAdded(e As ControlEventArgs)
        MyBase.OnControlAdded(e)
        Dim padding = e.Control.Padding
        padding.Right += ButtonPadding
        e.Control.Padding = padding
    End Sub

    Protected Overrides Sub OnControlRemoved(e As ControlEventArgs)
        MyBase.OnControlRemoved(e)
        Dim padding = e.Control.Padding
        padding.Right -= ButtonPadding
        e.Control.Padding = padding
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        MyBase.OnDrawItem(e)
        e.DrawBackground()
        e.DrawFocusRectangle()
        ButtonRenderer.DrawButton(e.Graphics, New Rectangle(e.Bounds.Right - ButtonPadding, 1, ButtonPadding, e.Bounds.Height - 2),
                                  VisualStyles.PushButtonState.Default)
    End Sub
End Class
