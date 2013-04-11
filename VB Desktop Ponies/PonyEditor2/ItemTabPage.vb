Public Class ItemTabPage
    Inherits TabPage

    Public Property IsDirty As Boolean
    Private _text As String
    Public Overrides Property Text As String
        Get
            Return _text
        End Get
        Set(value As String)
            _text = value
            If IsDirty Then
                MyBase.Text = Text & "*"
            Else
                MyBase.Text = Text
            End If
        End Set
    End Property
End Class
