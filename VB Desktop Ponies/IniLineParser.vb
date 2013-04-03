Module IniLineParser
    Public Function CommaSplitQuoteQualified(source As String) As String()
        Return source.SplitQualified({","c}, {{ControlChars.Quote, ControlChars.Quote}}, StringSplitOptions.None)
    End Function

    Public Function CommaSplitBraceQualified(source As String) As String()
        Return source.SplitQualified({","c}, {{"{"c, "}"c}}, StringSplitOptions.None)
    End Function

    Public Function CommaSplitQuoteBraceQualified(source As String) As String()
        Return source.SplitQualified({","c}, {{ControlChars.Quote, ControlChars.Quote}, {"{"c, "}"c}}, StringSplitOptions.None)
    End Function

    Public Function Quoted(text As String) As String
        Return ControlChars.Quote & text & ControlChars.Quote
    End Function

    Public Function Braced(text As String) As String
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
End Module
