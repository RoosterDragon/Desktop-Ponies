Module IniLineParser
    Public Function CommaSplitQuoteQualified(source As String) As String()
        Try
            Return CommaSplitQuoteQualifiedInternal(source)
        Catch ex As ArgumentException
            Return CommaSplitQuoteQualifiedInternal(source & ControlChars.Quote)
        End Try
    End Function

    Private Function CommaSplitQuoteQualifiedInternal(source As String) As String()
        Return source.SplitQualified({","c}, {{ControlChars.Quote, ControlChars.Quote}}, StringSplitOptions.None)
    End Function

    Public Function CommaSplitBraceQualified(source As String) As String()
        Try
            Return CommaSplitBraceQualifiedInternal(source)
        Catch ex As ArgumentException
            Return CommaSplitBraceQualifiedInternal(source & "}")
        End Try
    End Function

    Private Function CommaSplitBraceQualifiedInternal(source As String) As String()
        Return source.SplitQualified({","c}, {{"{"c, "}"c}}, StringSplitOptions.None)
    End Function

    Public Function CommaSplitQuoteBraceQualified(source As String) As String()
        Try
            Return CommaSplitQuoteBraceQualifiedInternal(source)
        Catch ex As ArgumentException
            Try
                Return CommaSplitQuoteBraceQualifiedInternal(source & ControlChars.Quote)
            Catch ex2 As ArgumentException
                Return CommaSplitQuoteBraceQualifiedInternal(source & "}")
            End Try
        End Try
    End Function

    Private Function CommaSplitQuoteBraceQualifiedInternal(source As String) As String()
        Return source.SplitQualified({","c}, {{ControlChars.Quote, ControlChars.Quote}, {"{"c, "}"c}}, StringSplitOptions.None)
    End Function

    Public Function Quoted(text As String) As String
        If text IsNot Nothing AndAlso text.IndexOf(ControlChars.Quote) <> -1 Then
            Throw New ArgumentException("The source text must not contain any quote ("") characters." & Environment.NewLine &
                                        "Text: " & text, "text")
        End If
        Return ControlChars.Quote & text & ControlChars.Quote
    End Function

    Public Function Braced(text As String) As String
        If text.IndexOfAny({"{"c, "}"c}) <> -1 Then
            Throw New ArgumentException("The source text must not contain any curly brace ({}) characters." & Environment.NewLine &
                                        "Text: " & text, "text")
        End If
        Return "{" & text & "}"
    End Function

    Public Function Space_To_Under(text As String) As String
        Return Replace(Replace(text, " ", "_"), "/", "_")
    End Function

    Public Function Location_ToString(location As Direction) As String
        Argument.EnsureEnumIsValid(location, "location")
        Select Case location
            Case Direction.TopLeft
                Return "Top Left"
            Case Direction.TopCenter
                Return "Top"
            Case Direction.TopRight
                Return "Top Right"
            Case Direction.MiddleLeft
                Return "Left"
            Case Direction.MiddleCenter
                Return "Center"
            Case Direction.MiddleRight
                Return "Right"
            Case Direction.BottomLeft
                Return "Bottom Left"
            Case Direction.BottomCenter
                Return "Bottom"
            Case Direction.BottomRight
                Return "Bottom Right"
            Case Direction.Random
                Return "Any"
            Case Direction.RandomNotCenter
                Return "Any-Not Center"
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function GetDirection(setting As String) As Direction
        Select Case setting
            Case "top_left"
                Return Direction.TopLeft
            Case "top"
                Return Direction.TopCenter
            Case "top_right"
                Return Direction.TopRight
            Case "left"
                Return Direction.MiddleLeft
            Case "center"
                Return Direction.MiddleCenter
            Case "right"
                Return Direction.MiddleRight
            Case "bottom_left"
                Return Direction.BottomLeft
            Case "bottom"
                Return Direction.BottomCenter
            Case "bottom_right"
                Return Direction.BottomRight
            Case "any"
                Return Direction.Random
            Case "any-not_center"
                Return Direction.RandomNotCenter
            Case Else
                Dim result As Direction
                If [Enum].TryParse(setting, result) Then
                    Return result
                Else
                    Throw New ArgumentException("Invalid placement direction or centering for effect.", "setting")
                End If
        End Select
    End Function

    Private ReadOnly _directionFromIni As New Dictionary(Of String, Direction)(StringComparer.OrdinalIgnoreCase) From
        {
            {"top_left", Direction.TopLeft},
            {"top", Direction.TopCenter},
            {"top_right", Direction.TopRight},
            {"left", Direction.MiddleLeft},
            {"center", Direction.MiddleCenter},
            {"right", Direction.MiddleRight},
            {"bottom_left", Direction.BottomLeft},
            {"bottom", Direction.BottomCenter},
            {"bottom_right", Direction.BottomRight},
            {"any", Direction.Random},
            {"any-not_center", Direction.RandomNotCenter}
        }
    Public ReadOnly Property DirectionFromIni As Dictionary(Of String, Direction)
        Get
            Return _directionFromIni
        End Get
    End Property

    Private ReadOnly _allowedMovesFromIni As New Dictionary(Of String, AllowedMoves)(StringComparer.OrdinalIgnoreCase) From
        {
            {"none", AllowedMoves.None},
            {"horizontal_only", AllowedMoves.HorizontalOnly},
            {"vertical_only", AllowedMoves.VerticalOnly},
            {"horizontal_vertical", AllowedMoves.HorizontalVertical},
            {"diagonal_only", AllowedMoves.DiagonalOnly},
            {"diagonal_horizontal", AllowedMoves.DiagonalHorizontal},
            {"diagonal_vertical", AllowedMoves.DiagonalVertical},
            {"all", AllowedMoves.All},
            {"mouseover", AllowedMoves.MouseOver},
            {"sleep", AllowedMoves.Sleep},
            {"dragged", AllowedMoves.Dragged}
        }
    Public ReadOnly Property AllowedMovesFromIni As Dictionary(Of String, AllowedMoves)
        Get
            Return _allowedMovesFromIni
        End Get
    End Property
End Module
