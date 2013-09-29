<Diagnostics.DebuggerDisplay("{str}")>
Public NotInheritable Class CaseInsensitiveString
    Implements IComparable(Of CaseInsensitiveString), IEquatable(Of CaseInsensitiveString), IEquatable(Of String)
    Public Shared ReadOnly Empty As CaseInsensitiveString = New CaseInsensitiveString(String.Empty)
    Private str As String
    Public Sub New(value As String)
        str = Argument.EnsureNotNull(value, "value")
    End Sub
    Public Overrides Function Equals(obj As Object) As Boolean
        Dim cis = TryCast(obj, CaseInsensitiveString)
        If cis IsNot Nothing Then Return Equals(cis)
        Dim s = TryCast(obj, String)
        If s IsNot Nothing Then Return Equals(s)
        Return False
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return StringComparer.OrdinalIgnoreCase.GetHashCode(str)
    End Function
    Public Overrides Function ToString() As String
        Return str
    End Function
    Public Overloads Shared Widening Operator CType(value As CaseInsensitiveString) As String
        Return If(value Is Nothing, Nothing, value.str)
    End Operator
    Public Overloads Shared Widening Operator CType(value As String) As CaseInsensitiveString
        Return If(value Is Nothing, Nothing, New CaseInsensitiveString(value))
    End Operator
    Public Shared Operator =(left As CaseInsensitiveString, right As CaseInsensitiveString) As Boolean
        Return Equals(left, right)
    End Operator
    Public Shared Operator <>(left As CaseInsensitiveString, right As CaseInsensitiveString) As Boolean
        Return Not left = right
    End Operator
    Public Shared Operator =(left As CaseInsensitiveString, right As String) As Boolean
        Return Equals(left, right)
    End Operator
    Public Shared Operator <>(left As CaseInsensitiveString, right As String) As Boolean
        Return Not left = right
    End Operator
    Public Shared Operator =(left As String, right As CaseInsensitiveString) As Boolean
        Return Equals(left, right)
    End Operator
    Public Shared Operator <>(left As String, right As CaseInsensitiveString) As Boolean
        Return Not left = right
    End Operator
    Public Shared Operator &(left As CaseInsensitiveString, right As CaseInsensitiveString) As CaseInsensitiveString
        Return New CaseInsensitiveString(ValueOrNull(left) & ValueOrNull(right))
    End Operator
    Public Shared Operator &(left As CaseInsensitiveString, right As String) As String
        Return ValueOrNull(left) & right
    End Operator
    Public Shared Operator &(left As String, right As CaseInsensitiveString) As String
        Return left & ValueOrNull(right)
    End Operator
    Public Shared Operator +(left As CaseInsensitiveString, right As CaseInsensitiveString) As CaseInsensitiveString
        Return New CaseInsensitiveString(ValueOrNull(left) & ValueOrNull(right))
    End Operator
    Public Shared Operator +(left As CaseInsensitiveString, right As String) As String
        Return ValueOrNull(left) + right
    End Operator
    Public Shared Operator +(left As String, right As CaseInsensitiveString) As String
        Return left + ValueOrNull(right)
    End Operator
    Public Function CompareTo(other As CaseInsensitiveString) As Integer Implements IComparable(Of CaseInsensitiveString).CompareTo
        Return Compare(Me, other)
    End Function
    Public Function CompareTo(other As String) As Integer
        Return Compare(Me, other)
    End Function
    Public Overloads Function Equals(other As CaseInsensitiveString) As Boolean Implements IEquatable(Of CaseInsensitiveString).Equals
        Return Equals(Me, other)
    End Function
    Public Overloads Function Equals(other As String) As Boolean Implements IEquatable(Of String).Equals
        Return Equals(Me, other)
    End Function
    Public Overloads Shared Function Equals(left As CaseInsensitiveString, right As CaseInsensitiveString) As Boolean
        If left Is Nothing AndAlso right Is Nothing Then
            Return True
        ElseIf left IsNot Nothing AndAlso right IsNot Nothing Then
            Return String.Equals(left.str, right.str, StringComparison.OrdinalIgnoreCase)
        Else
            Return False
        End If
    End Function
    Public Shared Function Compare(left As CaseInsensitiveString, right As CaseInsensitiveString) As Integer
        If left Is Nothing AndAlso right Is Nothing Then
            Return 0
        ElseIf left IsNot Nothing AndAlso right IsNot Nothing Then
            Return String.Compare(left.str, right.str, StringComparison.OrdinalIgnoreCase)
        ElseIf left Is Nothing Then
            Return -1
        Else
            Return 1
        End If
    End Function
    Private Shared Function ValueOrNull(s As CaseInsensitiveString) As String
        If s Is Nothing Then Return Nothing
        Return s.str
    End Function
End Class