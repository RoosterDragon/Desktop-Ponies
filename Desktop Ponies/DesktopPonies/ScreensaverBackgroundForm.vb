
'IMPORTANT NOTE:  Transparancy is achieved by setting the transparancy key of the form, and the background color to the same shade of awful pink that hopefully no one uses.

Public Class ScreensaverBackgroundForm

    Private screensaverToCloseOnNextMouseMove As Boolean
    Private initialMouseLocation As Point

    ' Don't steal focus when first shown.
    Protected Overrides ReadOnly Property ShowWithoutActivation() As Boolean
        Get
            Return True
        End Get
    End Property

    ' Close screensaver mode when user moves the mouse.
    Private Sub ScreensaverBackgroundForm_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        If Not screensaverToCloseOnNextMouseMove Then
            screensaverToCloseOnNextMouseMove = True
            initialMouseLocation = e.Location
        Else
            If initialMouseLocation = e.Location Then Return
            If EvilGlobals.CurrentAnimator IsNot Nothing Then EvilGlobals.CurrentAnimator.Finish()
            EvilGlobals.Main.SmartInvoke(Sub()
                                          EvilGlobals.Main.PonyShutdown()
                                          EvilGlobals.Main.Close()
                                      End Sub)
        End If
    End Sub
End Class