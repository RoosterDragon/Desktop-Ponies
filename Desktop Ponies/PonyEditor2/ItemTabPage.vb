Public Class ItemTabPage
    Inherits TabPage

    Private _isDirty As Boolean
    Public Property IsDirty As Boolean
        Get
            Return _isDirty
        End Get
        Set(value As Boolean)
            _isDirty = value
        End Set
    End Property
End Class
