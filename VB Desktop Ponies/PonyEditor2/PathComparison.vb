Public Class PathComparison
    Private Sub New()
    End Sub
    Public Shared ReadOnly Property Current As StringComparison
        Get
            Return If(OperatingSystemInfo.IsWindows, StringComparison.OrdinalIgnoreCase, StringComparison.Ordinal)
        End Get
    End Property
End Class
