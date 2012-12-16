
'IMPORTANT NOTE:  Transparancy is achieved by setting the transparancy key of the form, and the background color to the same shade of awful pink that hopefully no one uses.

Public Class ScreensaverBackgroundForm


    'don't steal focus when first shown.
    Protected Overrides ReadOnly Property ShowWithoutActivation() As Boolean

        Get

            Return True

        End Get

    End Property

End Class