Public NotInheritable Class PathEquality
    Private Sub New()
    End Sub
    Public Shared ReadOnly Property Comparison As StringComparison
        Get
            Return If(OperatingSystemInfo.IsWindows, StringComparison.OrdinalIgnoreCase, StringComparison.Ordinal)
        End Get
    End Property
    Public Shared ReadOnly Property Comparer As StringComparer
        Get
            Return If(OperatingSystemInfo.IsWindows, StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal)
        End Get
    End Property
End Class
