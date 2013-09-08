Imports System.Globalization

Public Class ComboBoxCaseSensitive
    Inherits ComboBox

    Public Event ValueChanged(sender As Object, e As EventArgs)

    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            If DropDownStyle = ComboBoxStyle.DropDownList AndAlso
                Not IsHandleCreated AndAlso
                Not String.IsNullOrEmpty(value) AndAlso
                FindStringExact(value, -1, False) = -1 Then
                Return
            End If
            SetText(value)
            Dim selectedItemTemp = SelectedItem
            If DesignMode Then Return
            If value Is Nothing Then
                SelectedIndex = -1
            Else
                If selectedItemTemp IsNot Nothing AndAlso
                    String.Compare(value, GetItemText(selectedItemTemp), False, CultureInfo.CurrentCulture) = 0 Then
                    Return
                End If
                Dim stringHonorCase = FindStringExact(value, -1, False)
                If stringHonorCase = -1 Then Return
                SelectedIndex = stringHonorCase
            End If
        End Set
    End Property

    Private ignoreTextChanges As Boolean

    Public Overrides Property SelectedIndex As Integer
        Get
            Return If(dropDownIndex = Integer.MinValue, MyBase.SelectedIndex, dropDownIndex)
        End Get
        Set(value As Integer)
            If Not ignoreTextChanges Then MyBase.SelectedIndex = value
        End Set
    End Property

    Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
        MyBase.OnSelectedIndexChanged(e)
        OnValueChanged(EventArgs.Empty)
    End Sub

    Private Sub SetText(value As String)
        ignoreTextChanges = True
        MyBase.Text = value
        ignoreTextChanges = False
    End Sub

    Private dropDownText As String = Nothing
    Private dropDownIndex As Integer = Integer.MinValue
    Private dropDownTextStatus As DropDownTextState

    Private Enum DropDownTextState As Byte
        Unmodifed
        CodeModified
        UserModified
    End Enum

    Protected Overrides Sub OnDropDown(e As EventArgs)
        dropDownText = Text
        dropDownIndex = SelectedIndex
        If FindStringExact(dropDownText, -1, False) = -1 Then
            SetText(dropDownText & " ")
            dropDownTextStatus = DropDownTextState.CodeModified
        End If
        MyBase.OnDropDown(e)
    End Sub

    Protected Overrides Sub OnDropDownClosed(e As EventArgs)
        If dropDownTextStatus = DropDownTextState.CodeModified Then Text = dropDownText
        dropDownTextStatus = DropDownTextState.Unmodifed
        dropDownText = Nothing
        dropDownIndex = Integer.MinValue
        MyBase.OnDropDownClosed(e)
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If dropDownTextStatus = DropDownTextState.CodeModified Then
            SetText(dropDownText)
            dropDownTextStatus = DropDownTextState.Unmodifed
        End If
        If dropDownText <> Text Then
            dropDownTextStatus = DropDownTextState.UserModified
        End If
        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        If Not ignoreTextChanges Then
            MyBase.OnTextChanged(e)
            OnValueChanged(EventArgs.Empty)
        End If
    End Sub

    Protected Overridable Sub OnValueChanged(e As EventArgs)
        RaiseEvent ValueChanged(Me, e)
    End Sub


    Public Overloads Function FindStringExact(s As String, startIndex As Integer, ignoreCase As Boolean) As Integer
        For i = startIndex + 1 To Items.Count - 1
            If String.Compare(s, GetItemText(Items(i)), ignoreCase, CultureInfo.CurrentCulture) = 0 Then
                Return i
            End If
        Next
        Return -1
    End Function

End Class
