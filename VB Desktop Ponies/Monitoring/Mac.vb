Friend NotInheritable Class Mac
    Public Shared Sub WriteLine(value As String)
        If OperatingSystemInfo.IsMacOSX Then
            Console.WriteLine(value)
        End If
    End Sub
End Class