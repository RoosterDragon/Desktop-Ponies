Public Class ItemTabPage
    Inherits TabPage

    Private _isDirty As Boolean
    Public Property IsDirty As Boolean
        Get
            Return _isDirty
        End Get
        Set(value As Boolean)
            _isDirty = value
            UpdateBaseText()
        End Set
    End Property
    Private _text As String
    Public Overrides Property Text As String
        Get
            Return _text
        End Get
        Set(value As String)
            _text = value
            UpdateBaseText()
        End Set
    End Property

    Private Sub UpdateBaseText()
        If IsDirty Then
            MyBase.Text = text & "*"
        Else
            MyBase.Text = text
        End If
    End Sub
End Class
