Public Class ScreensaverBackgroundForm
    ''' <summary>
    ''' Don't steal focus when first shown.
    ''' </summary>
    ''' <returns>True</returns>
    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
        End Get
    End Property
End Class