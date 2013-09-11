Imports System.IO

Public Class PonyCollection
    Private ReadOnly _bases As ImmutableArray(Of PonyBase)
    Public ReadOnly Property Bases As ImmutableArray(Of PonyBase)
        Get
            Return _bases
        End Get
    End Property
    Private ReadOnly _randomBase As PonyBase
    Public ReadOnly Property RandomBase As PonyBase
        Get
            Return _randomBase
        End Get
    End Property

    Public Sub New()
        Me.New(Nothing, Nothing)
    End Sub

    Public Sub New(countCallback As Action(Of Integer), loadCallback As Action(Of PonyBase))
        Dim ponies As New Collections.Concurrent.ConcurrentBag(Of PonyBase)()
        Dim ponyBaseDirectories = Directory.GetDirectories(Path.Combine(Options.InstallLocation, PonyBase.RootDirectory))
        If countCallback IsNot Nothing Then countCallback(ponyBaseDirectories.Length)
        Threading.Tasks.Parallel.ForEach(
            ponyBaseDirectories,
            Sub(folder)
                Dim pony = PonyBase.Load(folder.Substring(folder.LastIndexOf(Path.DirectorySeparatorChar) + 1))
                If pony IsNot Nothing Then
                    ponies.Add(pony)
                    If loadCallback IsNot Nothing Then loadCallback(pony)
                End If
            End Sub)
        Dim allBases = ponies.OrderBy(Function(pb) pb.Directory, StringComparer.OrdinalIgnoreCase).ToList()
        Dim randomIndex = allBases.FindIndex(Function(pb) pb.Directory = "Random Pony")
        If randomIndex <> -1 Then
            _randomBase = allBases(randomIndex)
            allBases.RemoveAt(randomIndex)
        End If
        _bases = allBases.ToImmutableArray()
    End Sub
End Class

Public NotInheritable Class PonyIniParser
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