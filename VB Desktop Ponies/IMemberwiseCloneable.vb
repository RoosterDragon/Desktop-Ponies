Public Interface IMemberwiseCloneable
    Function MemberwiseClone() As Object
End Interface

Public Interface IMemberwiseCloneable(Of T)
    Function MemberwiseClone() As T
End Interface
