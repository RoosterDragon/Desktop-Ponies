Imports System.IO

Public Class PonyCollection
    Private ReadOnly ponies As New Collections.Concurrent.ConcurrentBag(Of PonyBase)()

    Public Sub LoadAll(countCallback As Action(Of Integer), loadCallback As Action(Of MutablePonyBase))
        Dim watch = Diagnostics.Stopwatch.StartNew()
        Dim ponyBaseDirectories = Directory.GetDirectories(Path.Combine(Options.InstallLocation, PonyBase.RootDirectory))
        If countCallback IsNot Nothing Then countCallback(ponyBaseDirectories.Length)
        Threading.Tasks.Parallel.ForEach(
            ponyBaseDirectories,
            Sub(folder)
                Dim pony = LoadOne(folder)
                If loadCallback IsNot Nothing AndAlso pony IsNot Nothing Then loadCallback(pony)
            End Sub)
        Console.WriteLine("LoadAll templates Total: {0:0ms} Avg {1:0ms}",
                          watch.Elapsed.TotalMilliseconds,
                          watch.Elapsed.TotalMilliseconds / ponyBaseDirectories.Length)
    End Sub

    Private Function LoadOne(folder As String) As MutablePonyBase
        Dim iniFileName = Path.Combine(folder, PonyBase.ConfigFilename)
        If File.Exists(iniFileName) Then
            Dim reader As StreamReader = Nothing
            Try
                reader = New StreamReader(iniFileName)
            Catch ex As DirectoryNotFoundException
            Catch ex As FileNotFoundException
            End Try

            If reader IsNot Nothing Then
                Try
                    Dim pony = New MutablePonyBase()
                    pony.Directory = folder.Substring(folder.LastIndexOf(Path.DirectorySeparatorChar) + 1)
                    ParsePonyConfig(folder, reader, pony)
                    ponies.Add(pony)
                    Return pony
                Finally
                    reader.Dispose()
                End Try
            End If
        End If
        Return Nothing
    End Function

    Private Sub ParsePonyConfig(folder As String, reader As StreamReader, pony As MutablePonyBase)
        Do Until reader.EndOfStream
            Dim line = reader.ReadLine()

            ' Ignore blank lines, and those commented out with a single quote.
            If String.IsNullOrWhiteSpace(line) OrElse line(0) = "'" Then Continue Do

            Dim firstComma = line.IndexOf(","c)
            If firstComma <> -1 Then
                Select Case line.Substring(0, firstComma).ToLowerInvariant()
                    Case "name"
                        TryParse(Of String)(line, folder, AddressOf PonyIniParser.TryParseName, Sub(n) pony.DisplayName = n)
                    Case "scale"
                        TryParse(Of Double)(line, folder, AddressOf PonyIniParser.TryParseScale, Sub(s) pony.Scale = s)
                    Case "behaviorgroup"
                        TryParse(Of BehaviorGroup)(line, folder, AddressOf PonyIniParser.TryParseBehaviorGroup, Sub(bg) pony.BehaviorGroups.Add(bg))
                    Case "behavior"
                        TryParse(Of Behavior)(line, folder, pony, AddressOf Behavior.TryLoad, Sub(b) pony.Behaviors.Add(b))
                    Case "effect"
                        TryParse(Of EffectBase)(line, folder, pony, AddressOf EffectBase.TryLoad, Sub(e) pony.Effects.Add(e))
                    Case "speak"
                        TryParse(Of Speech)(line, folder, AddressOf Speech.TryLoad, Sub(sl) pony.Speeches.Add(sl))
                    Case "categories"
                        Dim columns = CommaSplitQuoteQualified(line)
                        For i = 1 To columns.Count - 1
                            For Each item As String In Main.Instance.FilterOptionsBox.Items
                                If String.Equals(item, columns(i), StringComparison.OrdinalIgnoreCase) Then
                                    pony.Tags.Add(columns(i))
                                    Exit For
                                End If
                            Next
                        Next
                    Case Else
                        ' TODO: Handle unrecognized identifier, or lack of first comma.
                End Select
            End If
        Loop
    End Sub

    Private Sub TryParse(Of T)(line As String, directory As String, parseFunc As TryParse(Of T), onSuccess As Action(Of T))
        Dim result As T
        Dim issues As ParseIssue() = Nothing
        If parseFunc(line, directory, result, issues) Then
            onSuccess(result)
        End If
    End Sub

    Private Sub TryParse(Of T)(line As String, directory As String, pony As PonyBase, parseFunc As TryParse(Of T, PonyBase), onSuccess As Action(Of T))
        Dim result As T
        Dim issues As ParseIssue() = Nothing
        If parseFunc(line, directory, pony, result, issues) Then
            onSuccess(result)
        End If
    End Sub
End Class

Public Class PonyIniParser
    Private Sub New()
    End Sub

    Private Shared Function TryParse(Of T)(ByRef result As T, ByRef issues As ParseIssue(),
                                                  parser As StringCollectionParser,
                                                  parse As Func(Of StringCollectionParser, T)) As Boolean
        result = parse(parser)
        issues = parser.Issues.ToArray()
        Return parser.AllParsingSuccessful
    End Function

    Public Shared Function TryParseName(iniLine As String, directory As String, ByRef result As String, ByRef issues As ParseIssue()) As Boolean
        Return TryParse(result, issues,
                                New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", "Name"}),
                                Function(p)
                                    p.NoParse()
                                    Return p.NoParse()
                                End Function)
    End Function

    Public Shared Function TryParseScale(iniLine As String, directory As String, ByRef result As Double, ByRef issues As ParseIssue()) As Boolean
        Return TryParse(result, issues,
                                   New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", "Scale"}),
                                   Function(p)
                                       p.NoParse()
                                       Return p.ParseDouble(0, 0, 16)
                                   End Function)
    End Function

    Public Shared Function TryParseBehaviorGroup(iniLine As String, directory As String, ByRef result As BehaviorGroup, ByRef issues As ParseIssue()) As Boolean
        Return TryParse(result, issues,
                                   New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", "Number", "Name"}),
                                   Function(p)
                                       p.NoParse()
                                       Dim bg As New BehaviorGroup(Nothing, 0)
                                       bg.Number = p.ParseInt32(0, 100)
                                       bg.Name = p.NotNull("")
                                       Return bg
                                   End Function)
    End Function
End Class

Public Delegate Function TryParse(Of T)(iniLine As String, directory As String, ByRef result As T, ByRef issues As ParseIssue()) As Boolean

Public Delegate Function TryParse(Of T, PonyBase)(iniLine As String, directory As String, pony As PonyBase, ByRef result As T, ByRef issues As ParseIssue()) As Boolean