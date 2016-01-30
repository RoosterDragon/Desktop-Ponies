Imports System.IO

Public Class PonyCollection
    Private NotInheritable Class CaseInsensitiveStringAsStringEqualityComparer
        Implements IEqualityComparer(Of CaseInsensitiveString)
        Public Shared ReadOnly Instance As New CaseInsensitiveStringAsStringEqualityComparer()
        Private Sub New()
        End Sub
        Public Overloads Function Equals(x As CaseInsensitiveString, y As CaseInsensitiveString) As Boolean Implements IEqualityComparer(Of CaseInsensitiveString).Equals
            If x Is Nothing AndAlso y Is Nothing Then Return True
            If x Is Nothing Or y Is Nothing Then Return False
            Return String.Equals(x.ToString(), y.ToString())
        End Function
        Public Overloads Function GetHashCode(obj As CaseInsensitiveString) As Integer Implements IEqualityComparer(Of CaseInsensitiveString).GetHashCode
            If obj Is Nothing Then Return StringComparer.Ordinal.GetHashCode(Nothing)
            Return obj.ToString().GetHashCode()
        End Function
    End Class

    Private _bases As ImmutableArray(Of PonyBase)
    Public ReadOnly Property Bases As ImmutableArray(Of PonyBase)
        Get
            Return _bases
        End Get
    End Property
    Private _randomBase As PonyBase
    Public ReadOnly Property RandomBase As PonyBase
        Get
            Return _randomBase
        End Get
    End Property
    Private _houses As ImmutableArray(Of HouseBase)
    Public ReadOnly Property Houses As ImmutableArray(Of HouseBase)
        Get
            Return _houses
        End Get
    End Property

    Public Sub New(removeInvalidItems As Boolean)
        Me.New(removeInvalidItems, Nothing, Nothing, Nothing, Nothing)
    End Sub

    Public Sub New(removeInvalidItems As Boolean,
                   ponyCountCallback As Action(Of Integer), ponyLoadCallback As Action(Of PonyBase),
                   houseCountCallback As Action(Of Integer), houseLoadCallback As Action(Of HouseBase))
        Dim ponyBaseDirectories = Directory.GetDirectories(PonyBase.RootDirectory)
        If ponyCountCallback IsNot Nothing Then ponyCountCallback(ponyBaseDirectories.Length)
        Dim houseDirectories = Directory.GetDirectories(HouseBase.RootDirectory)
        If houseCountCallback IsNot Nothing Then houseCountCallback(houseDirectories.Length)
        UpgradeInteractions()
        Threading.Tasks.Parallel.Invoke(
            Sub() LoadPonyBases(removeInvalidItems, ponyBaseDirectories, ponyLoadCallback),
            Sub() LoadHouses(houseDirectories, houseLoadCallback))
        ReuseStrings()
    End Sub

    Private Sub LoadPonyBases(removeInvalidItems As Boolean, ponyBaseDirectories As String(), ponyLoadCallback As Action(Of PonyBase))
        Dim ponies As New Collections.Concurrent.ConcurrentBag(Of PonyBase)()
        Threading.Tasks.Parallel.ForEach(
            ponyBaseDirectories,
            Sub(folder)
                Dim pony = PonyBase.Load(Me, folder.Substring(folder.LastIndexOf(Path.DirectorySeparatorChar) + 1), removeInvalidItems)
                If pony IsNot Nothing Then
                    UpdateImageSizes(pony)
                    ponies.Add(pony)
                    If ponyLoadCallback IsNot Nothing Then ponyLoadCallback(pony)
                End If
            End Sub)
        Dim allBases = ponies.OrderBy(Function(pb) pb.Directory, StringComparer.OrdinalIgnoreCase).ToList()
        Dim randomIndex = allBases.FindIndex(Function(pb) pb.Directory = PonyBase.RandomDirectory)
        If randomIndex <> -1 Then
            _randomBase = allBases(randomIndex)
            allBases.RemoveAt(randomIndex)
        End If
        _bases = allBases.ToImmutableArray()
    End Sub

    Private Sub LoadHouses(houseDirectories As String(), houseLoadCallback As Action(Of HouseBase))
        Dim houses As New Collections.Concurrent.ConcurrentBag(Of HouseBase)()
        Threading.Tasks.Parallel.ForEach(
            houseDirectories,
            Sub(folder)
                Try
                    Dim house = HouseBase.Load(folder.Substring(folder.LastIndexOf(Path.DirectorySeparatorChar) + 1), True)
                    If house IsNot Nothing Then
                        houses.Add(house)
                        If houseLoadCallback IsNot Nothing Then houseLoadCallback(house)
                    End If
                Catch ex As Exception
                    ' Ignore errors from loading badly configured houses.
                End Try
            End Sub)
        _houses = houses.OrderBy(Function(hb) hb.Name).ToImmutableArray()
    End Sub

    Private Sub UpgradeInteractions()
        Dim fileName = Path.Combine(PonyBase.RootDirectory, InteractionBase.LegacyConfigFilename)
        If Not File.Exists(fileName) Then Exit Sub

        Dim failedLines = New List(Of String)()
        Using reader As New StreamReader(fileName)
            Do Until reader.EndOfStream
                Dim line = reader.ReadLine()

                ' Ignore blank lines, and those commented out with a single quote.
                If String.IsNullOrWhiteSpace(line) OrElse line(0) = "'" Then Continue Do

                Dim i As InteractionBase = Nothing
                If InteractionBase.TryLoad(line, Nothing, i, Nothing) <> ParseResult.Failed AndAlso
                    File.Exists(Path.Combine(PonyBase.RootDirectory, i.InitiatorName, PonyBase.ConfigFilename)) Then
                    File.AppendAllLines(Path.Combine(PonyBase.RootDirectory, i.InitiatorName, PonyBase.ConfigFilename), New String() {i.GetPonyIni()})
                Else
                    failedLines.Add(line)
                End If
            Loop
        End Using
        File.Delete(fileName)
        If failedLines.Count > 0 Then File.WriteAllLines(fileName, failedLines)
    End Sub

    Private Sub ReuseStrings()
        Dim strings As New Dictionary(Of String, String)()
        Dim ciStrings As New Dictionary(Of CaseInsensitiveString, CaseInsensitiveString)(
            CaseInsensitiveStringAsStringEqualityComparer.Instance)
        For Each tag In PonyBase.StandardTags
            ciStrings(tag) = tag
        Next
        For Each base In _bases
            strings(base.Directory) = base.Directory
        Next
        For Each house In Houses
            strings(house.Directory) = house.Directory
        Next
        For Each base In _bases
            base.DisplayName = strings.GetOrAdd(base.DisplayName, base.DisplayName)
            For Each bg In base.BehaviorGroups
                bg.Name = ciStrings.GetOrAdd(bg.Name, bg.Name)
            Next
            For Each b In base.Behaviors
                b.EndLineName = ciStrings.GetOrAdd(b.EndLineName, b.EndLineName)
                b.FollowMovingBehaviorName = ciStrings.GetOrAdd(b.FollowMovingBehaviorName, b.FollowMovingBehaviorName)
                b.FollowStoppedBehaviorName = ciStrings.GetOrAdd(b.FollowStoppedBehaviorName, b.FollowStoppedBehaviorName)
                b.LinkedBehaviorName = ciStrings.GetOrAdd(b.LinkedBehaviorName, b.LinkedBehaviorName)
                b.Name = ciStrings.GetOrAdd(b.Name, b.Name)
                b.FollowTargetName = strings.GetOrAdd(b.FollowTargetName, b.FollowTargetName)
                b.StartLineName = ciStrings.GetOrAdd(b.StartLineName, b.StartLineName)
                b.LeftImage.Path = strings.GetOrAdd(b.LeftImage.Path, b.LeftImage.Path)
                b.RightImage.Path = strings.GetOrAdd(b.RightImage.Path, b.RightImage.Path)
            Next
            For Each e In base.Effects
                e.BehaviorName = ciStrings.GetOrAdd(e.BehaviorName, e.BehaviorName)
                e.Name = ciStrings.GetOrAdd(e.Name, e.Name)
                e.LeftImage.Path = strings.GetOrAdd(e.LeftImage.Path, e.LeftImage.Path)
                e.RightImage.Path = strings.GetOrAdd(e.RightImage.Path, e.RightImage.Path)
            Next
            For Each i In base.Interactions
                i.InitiatorName = strings.GetOrAdd(i.InitiatorName, i.InitiatorName)
                i.Name = ciStrings.GetOrAdd(i.Name, i.Name)
                Dim behaviorNames = i.BehaviorNames.ToArray()
                i.BehaviorNames.Clear()
                For Each bn In behaviorNames
                    i.BehaviorNames.Add(ciStrings.GetOrAdd(bn, bn))
                Next
                Dim targetsNames = i.TargetNames.ToArray()
                i.TargetNames.Clear()
                For Each tn In targetsNames
                    i.TargetNames.Add(strings.GetOrAdd(tn, tn))
                Next
            Next
            For Each s In base.Speeches
                s.Name = ciStrings.GetOrAdd(s.Name, s.Name)
                If s.SoundFile IsNot Nothing Then s.SoundFile = strings.GetOrAdd(s.SoundFile, s.SoundFile)
            Next
            Dim tags = base.Tags.ToArray()
            base.Tags.Clear()
            For Each t In tags
                base.Tags.Add(ciStrings.GetOrAdd(t, t))
            Next
        Next
        For Each house In Houses
            house.Name = ciStrings.GetOrAdd(house.Name, house.Name)
            house.LeftImage.Path = strings.GetOrAdd(house.LeftImage.Path, house.LeftImage.Path)
            house.RightImage.Path = strings.GetOrAdd(house.RightImage.Path, house.RightImage.Path)
            For i = 0 To house.Visitors.Count - 1
                Dim visitor = house.Visitors(i)
                house.Visitors(i) = strings.GetOrAdd(visitor, visitor)
            Next
        Next
    End Sub

    Private Shared Sub UpdateImageSizes(base As PonyBase)
        For Each behavior In base.Behaviors
            behavior.LeftImage.UpdateSize()
            behavior.RightImage.UpdateSize()
        Next
        For Each effect In base.Effects
            effect.LeftImage.UpdateSize()
            effect.RightImage.UpdateSize()
        Next
    End Sub
End Class

Public NotInheritable Class IniParser
    Private Sub New()
    End Sub
    Public Shared Sub TryParse(Of T)(line As String, removeInvalidItems As Boolean,
                                     parseFunc As TryParse(Of T), onParse As Action(Of T))
        Argument.EnsureNotNull(parseFunc, "parseFunc")
        Argument.EnsureNotNull(onParse, "onParse")
        Dim result As T
        If parseFunc(line, result, Nothing) <> ParseResult.Failed OrElse Not removeInvalidItems Then
            onParse(result)
        End If
    End Sub
    Public Shared Sub TryParse(Of T)(line As String, directory As String, removeInvalidItems As Boolean,
                                     parseFunc As TryParseDirectory(Of T), onParse As Action(Of T))
        Argument.EnsureNotNull(parseFunc, "parseFunc")
        Argument.EnsureNotNull(onParse, "onParse")
        Dim result As T
        If parseFunc(line, directory, result, Nothing) <> ParseResult.Failed OrElse Not removeInvalidItems Then
            onParse(result)
        End If
    End Sub
    Public Shared Function TryParse(Of T)(ByRef result As T, ByRef issues As ImmutableArray(Of ParseIssue),
                                          parser As StringCollectionParser,
                                          parse As Func(Of StringCollectionParser, T)) As ParseResult
        Argument.EnsureNotNull(parser, "parser")
        result = Argument.EnsureNotNull(parse, "parse")(parser)
        issues = parser.Issues.ToImmutableArray()
        Return parser.Result
    End Function
End Class

Public NotInheritable Class PonyIniParser
    Private Sub New()
    End Sub

    Public Shared Function TryParseName(iniLine As String, directory As String,
                                        ByRef result As String,
                                        ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        Return IniParser.TryParse(result, issues,
                                  New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", "Name"}),
                                  Function(p)
                                      p.NoParse()
                                      Return p.NoParse()
                                  End Function)
    End Function

    Public Shared Function TryParseBehaviorGroup(iniLine As String, directory As String,
                                                 ByRef result As BehaviorGroup,
                                                 ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        Return IniParser.TryParse(result, issues,
                                  New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", "Number", "Name"}),
                                  Function(p)
                                      p.NoParse()
                                      Dim bg As New BehaviorGroup("", 0)
                                      bg.Number = p.ParseInt32(0, 100)
                                      bg.Name = p.NotNullOrWhiteSpace(bg.Number.ToStringInvariant())
                                      Return bg
                                  End Function)
    End Function
End Class

Public NotInheritable Class HouseIniParser
    Private Sub New()
    End Sub

    Public Shared Function TryParseName(iniLine As String,
                                        ByRef result As String,
                                        ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        Return IniParser.TryParse(result, issues,
                                  New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", "Name"}),
                                  Function(p)
                                      p.NoParse()
                                      Return p.NotNullOrWhiteSpace()
                                  End Function)
    End Function

    Public Shared Function TryParseImage(iniLine As String, directory As String,
                                         ByRef result As String,
                                         ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        Return IniParser.TryParse(result, issues,
                                  New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", "Image"}),
                                  Function(p)
                                      p.NoParse()
                                      Dim path = p.NoParse()
                                      If p.Assert(path, Not String.IsNullOrEmpty(path), "An image path has not been set.", Nothing) Then
                                          path = p.SpecifiedCombinePath(directory, path, "Image will not be loaded.")
                                          p.SpecifiedFileExists(path)
                                      End If
                                      Return path
                                  End Function)
    End Function

    Public Shared Function TryParseDoorPosition(iniLine As String,
                                                ByRef result As Vector2,
                                                ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        Return IniParser.TryParse(result, issues,
                                  New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", "Door Position"}),
                                  Function(p)
                                      p.NoParse()
                                      Dim position As Vector2
                                      position.X = p.ParseInt32(0)
                                      position.Y = p.ParseInt32(0)
                                      Return position
                                  End Function)
    End Function

    Public Shared Function TryParseCycleInterval(iniLine As String,
                                                ByRef result As Integer,
                                                ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        Return IniParser.TryParse(result, issues,
                                  New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", "Cycle Interval"}),
                                  Function(p)
                                      p.NoParse()
                                      Return p.ParseInt32(300, 0, 3600)
                                  End Function)
    End Function

    Public Shared Function TryParseMinSpawn(iniLine As String,
                                            ByRef result As Integer,
                                            ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        Return TryParseSpawn(iniLine, "Min", 1, result, issues)
    End Function

    Public Shared Function TryParseMaxSpawn(iniLine As String,
                                            ByRef result As Integer,
                                            ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        Return TryParseSpawn(iniLine, "Max", 50, result, issues)
    End Function

    Private Shared Function TryParseSpawn(iniLine As String,
                                         name As String,
                                         fallback As Integer,
                                         ByRef result As Integer,
                                         ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        Return IniParser.TryParse(result, issues,
                                  New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", name & " Spawn"}),
                                  Function(p)
                                      p.NoParse()
                                      Return p.ParseInt32(fallback, 1, 9999)
                                  End Function)
    End Function

    Public Shared Function TryParseBias(iniLine As String,
                                        ByRef result As Decimal,
                                        ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        Return IniParser.TryParse(result, issues,
                                  New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine), {"Identifier", "Bias"}),
                                  Function(p)
                                      p.NoParse()
                                      Return p.ParseDecimal(0.5D, 0D, 1D)
                                  End Function)
    End Function
End Class

Public Delegate Function TryParse(Of T)(iniLine As String,
                                        ByRef result As T, ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult

Public Delegate Function TryParseDirectory(Of T)(iniLine As String, directory As String,
                                                 ByRef result As T, ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
