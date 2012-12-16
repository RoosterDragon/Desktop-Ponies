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
End Module
