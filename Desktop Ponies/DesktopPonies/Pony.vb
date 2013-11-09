Imports System.Globalization
Imports System.IO
Imports DesktopSprites.SpriteManagement

#Region "Interfaces"
Public Interface IPonyIniSourceable
    Inherits IPonyIniSerializable
    Property SourceIni As String
    Function Clone() As IPonyIniSourceable
End Interface

Public Interface IPonyIniSerializable
    Property Name As CaseInsensitiveString
    Function GetPonyIni() As String
End Interface

Public Interface IReferential
    Function GetReferentialIssues(ponies As PonyCollection) As ImmutableArray(Of ParseIssue)
End Interface

Public NotInheritable Class Referential
    Private Sub New()
    End Sub
    Public Shared Function CheckNotCircular(Of T)(propertyName As String, name As String, start As T, nextElement As Func(Of T, T),
                                                  elementName As Func(Of T, IEquatable(Of String))) As ParseIssue
        If String.IsNullOrEmpty(name) Then Return Nothing
        If start Is Nothing Then Return Nothing
        Dim currentElement = start
        Dim currentName = elementName(currentElement)
        Do While currentName IsNot Nothing
            If currentName.Equals(name) Then
                Return New ParseIssue(propertyName, name, "", "A circular loop has been detected.")
            End If
            Dim resolvedNextElement = nextElement(currentElement)
            If Object.ReferenceEquals(resolvedNextElement, currentElement) Then
                Return New ParseIssue(propertyName, name, "", "A circular loop has been detected.")
            End If
            currentElement = resolvedNextElement
            If currentElement Is Nothing Then Exit Do
            currentName = elementName(currentElement)
        Loop
        Return Nothing
    End Function
    Public Shared Function CheckUnique(propertyName As String, name As String, collection As IEnumerable(Of IEquatable(Of String))) As ParseIssue
        If String.IsNullOrEmpty(name) Then Return Nothing
        Dim result = CheckReference(name, collection)
        If result <> ReferenceResult.Unique Then
            Dim reason = If(result = ReferenceResult.NotFound,
                            String.Format(CultureInfo.CurrentCulture, "There is no element with the name '{0}'.", name),
                            String.Format(CultureInfo.CurrentCulture, "The name '{0}' does not refer to a unique element.", name))
            Return New ParseIssue(propertyName, name, "", reason)
        End If
        Return Nothing
    End Function
    Private Shared Function CheckReference(name As String, collection As IEnumerable(Of IEquatable(Of String))) As ReferenceResult
        Dim count = 0
        For Each candidateName In collection
            If candidateName.Equals(name) Then
                count += 1
                If count >= 2 Then Return ReferenceResult.NotUnique
            End If
        Next
        Return If(count = 0, ReferenceResult.NotFound, ReferenceResult.Unique)
    End Function
    Private Enum ReferenceResult
        Unique
        NotFound
        NotUnique
    End Enum
End Class
#End Region

#Region "PonyBase class"
Public Class PonyBase
    Public Const RootDirectory = "Ponies"
    Public Const ConfigFilename = "pony.ini"
    Public Const RandomDirectory = "Random Pony"
    Public Shared ReadOnly StandardTags As ImmutableArray(Of CaseInsensitiveString) =
        New CaseInsensitiveString() {
            "Main Ponies",
            "Supporting Ponies",
            "Alternate Art",
            "Fillies",
            "Colts",
            "Pets",
            "Stallions",
            "Mares",
            "Alicorns",
            "Unicorns",
            "Pegasi",
            "Earth Ponies",
            "Non-Ponies"}.ToImmutableArray()

    Private ReadOnly _collection As PonyCollection
    Public ReadOnly Property Collection As PonyCollection
        Get
            Return _collection
        End Get
    End Property

    Private _validatedOnLoad As Boolean
    Public ReadOnly Property ValidatedOnLoad As Boolean
        Get
            Return _validatedOnLoad
        End Get
    End Property
    Private _directory As String
    Public ReadOnly Property Directory As String
        Get
            Return _directory
        End Get
    End Property
    Public Property DisplayName As String
    Public Property Scale As Double
    Private _tags As New HashSet(Of CaseInsensitiveString)()
    Public ReadOnly Property Tags As HashSet(Of CaseInsensitiveString)
        Get
            Return _tags
        End Get
    End Property
    Private _behaviorGroups As New List(Of BehaviorGroup)()
    Public ReadOnly Property BehaviorGroups() As List(Of BehaviorGroup)
        Get
            Return _behaviorGroups
        End Get
    End Property
    Private _behaviors As New List(Of Behavior)()
    Public ReadOnly Property Behaviors() As List(Of Behavior)
        Get
            Return _behaviors
        End Get
    End Property
    Private _effects As New List(Of EffectBase)()
    Public ReadOnly Property Effects() As List(Of EffectBase)
        Get
            Return _effects
        End Get
    End Property
    Public ReadOnly Property Interactions() As List(Of InteractionBase)
        Get
            Return Collection.Interactions(Directory)
        End Get
    End Property
    Private _speeches As New List(Of Speech)()
    Public ReadOnly Property Speeches() As List(Of Speech)
        Get
            Return _speeches
        End Get
    End Property
    Public ReadOnly Property SpeechesRandom() As SpeechesEnumerable
        Get
            Return New SpeechesEnumerable(Me, False)
        End Get
    End Property
    Public ReadOnly Property SpeechesSpecific() As SpeechesEnumerable
        Get
            Return New SpeechesEnumerable(Me, True)
        End Get
    End Property
    Private ReadOnly commentLines As New List(Of String)()
    Private ReadOnly invalidLines As New List(Of String)()

#Region "Speeches Enumeration"
    Public Structure SpeechesEnumerable
        Implements IEnumerable(Of Speech)

        Private ReadOnly base As PonyBase
        Private ReadOnly skip As Boolean

        Public Sub New(base As PonyBase, skip As Boolean)
            Me.base = base
            Me.skip = skip
        End Sub

        Public Function GetEnumerator() As SpeechesEnumerator
            Return New SpeechesEnumerator(base, skip)
        End Function

        Private Function GetEnumerator1() As IEnumerator(Of Speech) Implements IEnumerable(Of Speech).GetEnumerator
            Return GetEnumerator()
        End Function

        Private Function GetEnumerator2() As Collections.IEnumerator Implements Collections.IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
    End Structure

    Public Structure SpeechesEnumerator
        Implements IEnumerator(Of Speech)

        Private ReadOnly skip As Boolean
        Private ReadOnly speeches As List(Of Speech)
        Private index As Integer

        Public Sub New(base As PonyBase, skip As Boolean)
            Argument.EnsureNotNull(base, "base")
            Me.speeches = base.Speeches
            Me.skip = skip
            Me.index = -1
        End Sub

        Public ReadOnly Property Current As Speech Implements IEnumerator(Of Speech).Current
            Get
                Return speeches(index)
            End Get
        End Property

        Private ReadOnly Property Current1 As Object Implements Collections.IEnumerator.Current
            Get
                Return Current
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements Collections.IEnumerator.MoveNext
            Do
                index += 1
            Loop While index < speeches.Count AndAlso Current.Skip <> skip
            Return index < speeches.Count
        End Function

        Public Sub Reset() Implements Collections.IEnumerator.Reset
            index = -1
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub
    End Structure
#End Region

    Private Sub New(collection As PonyCollection)
        _collection = Argument.EnsureNotNull(collection, "collection")
    End Sub

    Public Function ChangeDirectory(newDirectory As String) As Boolean
        If newDirectory = RandomDirectory Then Throw New ArgumentException("Cannot change directory to the random pony directory.")
        If Directory Is Nothing Then Return Create(newDirectory)
        If String.Equals(Directory, newDirectory, PathEquality.Comparison) Then Return True
        Try
            If newDirectory.Contains("""") Then
                Throw New ArgumentException("newDirectory may not contain any quote characters.", "newDirectory")
            End If
            Dim currentPath = Path.Combine(EvilGlobals.InstallLocation, PonyBase.RootDirectory, Directory)
            Dim newPath = Path.Combine(EvilGlobals.InstallLocation, PonyBase.RootDirectory, newDirectory)
            IO.Directory.Move(currentPath, newPath)
        Catch ex As Exception
            Return False
        End Try
        Collection.ChangePonyDirectory(Directory, newDirectory)
        For Each behavior In Behaviors
            behavior.LeftImage.Path = behavior.LeftImage.Path.Replace(Directory, newDirectory)
            behavior.RightImage.Path = behavior.RightImage.Path.Replace(Directory, newDirectory)
        Next
        For Each effect In Effects
            effect.LeftImage.Path = effect.LeftImage.Path.Replace(Directory, newDirectory)
            effect.RightImage.Path = effect.RightImage.Path.Replace(Directory, newDirectory)
        Next
        For Each speech In Speeches
            If speech.SoundFile IsNot Nothing Then speech.SoundFile = speech.SoundFile.Replace(Directory, newDirectory)
        Next
        _directory = newDirectory
        Return True
    End Function

    Public Shared Function CreateInMemory(collection As PonyCollection) As PonyBase
        Return New PonyBase(collection)
    End Function

    Public Shared Function Create(directory As String) As Boolean
        Try
            If directory.Contains("""") Then Throw New ArgumentException("directory may not contain any quote characters.", "directory")
            Dim fullPath = Path.Combine(EvilGlobals.InstallLocation, PonyBase.RootDirectory, directory)
            If IO.Directory.Exists(fullPath) Then Return False
            IO.Directory.CreateDirectory(fullPath)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function Load(collection As PonyCollection, directory As String, removeInvalidItems As Boolean) As PonyBase
        Dim pony As PonyBase = Nothing
        Try
            Dim fullPath = Path.Combine(EvilGlobals.InstallLocation, PonyBase.RootDirectory, directory)
            Dim iniFileName = Path.Combine(fullPath, PonyBase.ConfigFilename)
            If IO.Directory.Exists(fullPath) Then
                pony = New PonyBase(collection)
                pony._validatedOnLoad = removeInvalidItems
                pony._directory = directory
                pony.DisplayName = directory
                If File.Exists(iniFileName) Then
                    Using reader = New StreamReader(iniFileName)
                        ParsePonyConfig(fullPath, reader, pony, removeInvalidItems)
                    End Using
                End If
                If removeInvalidItems AndAlso pony.Behaviors.Count = 0 Then
                    Return Nothing
                End If
            End If
        Catch ex As Exception
            Return Nothing
        End Try
        Return pony
    End Function

    Private Shared Sub ParsePonyConfig(folder As String, reader As StreamReader, pony As PonyBase, removeInvalidItems As Boolean)
        Do Until reader.EndOfStream
            Dim line = reader.ReadLine()

            ' Ignore blank lines.
            If String.IsNullOrWhiteSpace(line) Then Continue Do
            ' Lines starting with a single quote are comments.
            If line(0) = "'" Then
                pony.commentLines.Add(line)
                Continue Do
            End If

            Dim firstComma = line.IndexOf(","c)
            If firstComma = -1 Then
                pony.invalidLines.Add(line)
            Else
                Select Case line.Substring(0, firstComma).ToLowerInvariant()
                    Case "name"
                        TryParse(Of String)(line, folder, removeInvalidItems,
                                            AddressOf PonyIniParser.TryParseName, Sub(n) pony.DisplayName = n)
                    Case "scale"
                        TryParse(Of Double)(line, folder, removeInvalidItems,
                                            AddressOf PonyIniParser.TryParseScale, Sub(s) pony.Scale = s)
                    Case "behaviorgroup"
                        TryParse(Of BehaviorGroup)(line, folder, removeInvalidItems,
                                                   AddressOf PonyIniParser.TryParseBehaviorGroup, Sub(bg) pony.BehaviorGroups.Add(bg))
                    Case "behavior"
                        TryParse(Of Behavior)(line, folder, removeInvalidItems, pony,
                                              AddressOf Behavior.TryLoad, Sub(b) pony.Behaviors.Add(b))
                    Case "effect"
                        TryParse(Of EffectBase)(line, folder, removeInvalidItems, pony,
                                                AddressOf EffectBase.TryLoad, Sub(e) pony.Effects.Add(e))
                    Case "speak"
                        TryParse(Of Speech)(line, folder, removeInvalidItems,
                                            AddressOf Speech.TryLoad, Sub(sl) pony.Speeches.Add(sl))
                    Case "categories"
                        Dim columns = CommaSplitQuoteQualified(line)
                        For i = 1 To columns.Count - 1
                            pony.Tags.Add(columns(i))
                        Next
                    Case Else
                        pony.invalidLines.Add(line)
                End Select
            End If
        Loop
    End Sub

    Private Shared Sub TryParse(Of T)(line As String, directory As String, removeInvalidItems As Boolean,
                                      parseFunc As TryParse(Of T), onParse As Action(Of T))
        Dim result As T
        If parseFunc(line, directory, result, Nothing) <> ParseResult.Failed OrElse Not removeInvalidItems Then
            onParse(result)
        End If
    End Sub

    Private Shared Sub TryParse(Of T)(line As String, directory As String, removeInvalidItems As Boolean, pony As PonyBase,
                                      parseFunc As TryParse(Of T, PonyBase), onParse As Action(Of T))
        Dim result As T
        If parseFunc(line, directory, pony, result, Nothing) <> ParseResult.Failed OrElse Not removeInvalidItems Then
            onParse(result)
        End If
    End Sub

    Public Sub AddBehavior(name As CaseInsensitiveString, chance As Double,
                       maxDuration As Double, minDuration As Double, speed As Double,
                       rightImagePath As String, leftImagePath As String,
                       allowedMoves As AllowedMoves, linkedBehaviorName As CaseInsensitiveString,
                       startLineName As CaseInsensitiveString, endLineName As CaseInsensitiveString,
                       followStoppedBehaviorName As CaseInsensitiveString,
                       followMovingBehaviorName As CaseInsensitiveString,
                       Optional skip As Boolean = False,
                       Optional xCoord As Integer = Nothing, Optional yCoord As Integer = Nothing,
                       Optional followObjectName As String = "",
                       Optional autoSelectImagesOnFollow As Boolean = True,
                       Optional rightImageCenter As Point = Nothing, Optional leftImageCenter As Point = Nothing,
                       Optional doNotRepeatImageAnimations As Boolean = False, Optional group As Integer = 0)

        Dim newBehavior As New Behavior(Me)

        If Not File.Exists(rightImagePath) Then
            Throw New FileNotFoundException("Image file does not exists for behavior " & name & " for pony " & Me.Directory & ". Path: " & rightImagePath)
        End If

        If Not File.Exists(leftImagePath) Then
            Throw New FileNotFoundException("Image file does not exists for behavior " & name & " for pony " & Me.Directory & ". Path: " & leftImagePath)
        End If

        newBehavior.Name = name
        newBehavior.Chance = chance
        newBehavior.MaxDuration = maxDuration
        newBehavior.MinDuration = minDuration
        newBehavior.Speed = speed
        newBehavior.AllowedMovement = allowedMoves
        newBehavior.DoNotRepeatImageAnimations = doNotRepeatImageAnimations
        newBehavior.StartLineName = startLineName
        newBehavior.EndLineName = endLineName
        newBehavior.Group = group
        newBehavior.Skip = skip

        'These coordinates are either a position on the screen to go to, if no object to follow is specified,
        'or, the offset from the center of the object to go to (upper left, below, etc)
        newBehavior.AutoSelectImagesOnFollow = autoSelectImagesOnFollow

        'When the pony if off-screen we overwrite the follow parameters to get them on-screen again.
        'we save the original parameters here.
        newBehavior.OriginalDestinationXCoord = xCoord
        newBehavior.OriginalDestinationYCoord = yCoord
        newBehavior.OriginalFollowTargetName = followObjectName

        newBehavior.FollowMovingBehaviorName = followMovingBehaviorName
        newBehavior.FollowStoppedBehaviorName = followStoppedBehaviorName

        newBehavior.LinkedBehaviorName = linkedBehaviorName

        newBehavior.LeftImage.Path = leftImagePath
        newBehavior.RightImage.Path = rightImagePath

        newBehavior.RightImage.CustomCenter = If(rightImageCenter = Point.Empty, Nothing, rightImageCenter)
        newBehavior.LeftImage.CustomCenter = If(leftImageCenter = Point.Empty, Nothing, leftImageCenter)

        Behaviors.Add(newBehavior)

    End Sub

    Public Sub Save()
        If Directory Is Nothing Then Throw New InvalidOperationException("Directory must be set before Save can be called.")
        Dim configFilePath = IO.Path.Combine(EvilGlobals.InstallLocation, PonyBase.RootDirectory, Directory, PonyBase.ConfigFilename)

        Dim tempFileName = Path.GetTempFileName()
        Using writer As New StreamWriter(tempFileName, False, System.Text.Encoding.UTF8)
            For Each commentLine In commentLines
                writer.WriteLine(commentLine)
            Next

            writer.WriteLine(String.Join(",", "Name", DisplayName))
            writer.WriteLine(String.Join(",", "Categories", String.Join(",", Tags.Select(Function(tag) Quoted(tag)))))

            For Each behaviorGroup In BehaviorGroups
                writer.WriteLine(behaviorGroup.GetPonyIni())
            Next

            For Each behavior In Behaviors
                writer.WriteLine(behavior.GetPonyIni())
            Next

            For Each effect In Effects
                writer.WriteLine(effect.GetPonyIni())
            Next

            For Each speech In Speeches
                writer.WriteLine(speech.GetPonyIni())
            Next

            For Each invalidLine In invalidLines
                writer.WriteLine(invalidLine)
            Next
        End Using
        MoveOrReplace(tempFileName, configFilePath)

        Dim interactionsFilePath = Path.Combine(EvilGlobals.InstallLocation, PonyBase.RootDirectory, InteractionBase.ConfigFilename)
        Dim interactionFileLines As New List(Of String)()
        Using reader = New StreamReader(interactionsFilePath)
            Do Until reader.EndOfStream
                Dim line = reader.ReadLine()
                Dim lineParts = CommaSplitQuoteQualified(line)
                ' Only save interactions not belonging to this pony.
                If lineParts.Length < 2 OrElse lineParts(1) <> Directory Then interactionFileLines.Add(line)
            Loop
        End Using

        tempFileName = Path.GetTempFileName()
        Using writer = New StreamWriter(tempFileName, False, System.Text.Encoding.UTF8)
            For Each line In interactionFileLines
                writer.WriteLine(line)
            Next

            For Each interaction In Interactions
                writer.WriteLine(interaction.GetPonyIni())
            Next
        End Using
        MoveOrReplace(tempFileName, interactionsFilePath)
    End Sub

    Private Shared Sub MoveOrReplace(tempFileName As String, destinationFileName As String)
        ' File.Replace cannot be used across different partitions - we must move the temporary file to the same partition first.
        ' We can skip this step on Windows if the root drive is the same, but on Mac/Unix it's easiest to just blindly do the move.
        ' The disadvantage of making the move is that the temporary file might get left behind in our destination directory if something
        ' goes wrong, rather than the OS temporary directory which wouldn't really matter.
        If Not OperatingSystemInfo.IsWindows OrElse
            Path.GetPathRoot(Path.GetFullPath(tempFileName)) <> Path.GetPathRoot(Path.GetFullPath(destinationFileName)) Then
            Dim tempFileNameInSameDirectory As String
            Dim destinationDirectory = Path.GetDirectoryName(destinationFileName)
            Do
                tempFileNameInSameDirectory = Path.Combine(destinationDirectory, Path.GetRandomFileName())
            Loop While File.Exists(tempFileNameInSameDirectory)
            File.Move(tempFileName, tempFileNameInSameDirectory)
            tempFileName = tempFileNameInSameDirectory
        End If

        If Not File.Exists(destinationFileName) Then File.Create(destinationFileName).Dispose()
        File.Replace(tempFileName, destinationFileName, Nothing)
        File.Delete(tempFileName)
    End Sub

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Directory: " & Directory
    End Function
End Class
#End Region

#Region "InteractionBase class"
Public Class InteractionBase
    Implements IPonyIniSourceable, IReferential

    Public Const ConfigFilename = "interactions.ini"

    Public Property Name As CaseInsensitiveString Implements IPonyIniSerializable.Name
    Public Property InitiatorName As String
    Public Property Chance As Double
    Public Property Proximity As Double
    Private _targetNames As New HashSet(Of String)()
    Public ReadOnly Property TargetNames As HashSet(Of String)
        Get
            Return _targetNames
        End Get
    End Property
    Public Property Activation As TargetActivation
    Private _behaviorNames As New HashSet(Of CaseInsensitiveString)()
    Public ReadOnly Property BehaviorNames As HashSet(Of CaseInsensitiveString)
        Get
            Return _behaviorNames
        End Get
    End Property
    Public Property ReactivationDelay As TimeSpan

    Public Shared Function TryLoad(iniLine As String, ByRef result As InteractionBase, ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        result = Nothing
        issues = Nothing

        Dim i = New InteractionBase()
        i.SourceIni = iniLine
        Dim p As New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine),
                                            {"Name", "Initiator", "Chance",
                                             "Proximity", "Targets", "Target Activation",
                                             "Behaviors", "Reactivation Delay"})
        i.Name = If(p.NotNullOrWhiteSpace(), "")
        i.InitiatorName = If(p.NotNullOrWhiteSpace(), "")
        i.Chance = p.ParseDouble(0, 0, 1)
        i.Proximity = p.ParseDouble(125, 0, 10000)
        i.TargetNames.UnionWith(CommaSplitQuoteQualified(p.NotNullOrEmpty("")).Where(Function(s) Not String.IsNullOrWhiteSpace(s)))
        i.Activation = p.Project(AddressOf TargetActivationFromString, TargetActivation.One)
        i.BehaviorNames.UnionWith(CommaSplitQuoteQualified(p.NotNullOrEmpty("")).Where(Function(s) Not String.IsNullOrWhiteSpace(s)).
                                  Select(Function(s) New CaseInsensitiveString(s)))
        i.ReactivationDelay = TimeSpan.FromSeconds(p.ParseDouble(60, 0, 3600))

        issues = p.Issues.ToImmutableArray()
        result = i
        Return p.Result
    End Function

    Public Function Clone() As IPonyIniSourceable Implements IPonyIniSourceable.Clone
        Dim copy = DirectCast(MyBase.MemberwiseClone(), InteractionBase)
        copy._targetNames = New HashSet(Of String)(_targetNames)
        copy._behaviorNames = New HashSet(Of CaseInsensitiveString)(_behaviorNames)
        Return copy
    End Function

    Public Function GetPonyIni() As String Implements IPonyIniSerializable.GetPonyIni
        Return String.Join(",",
            Name,
            Quoted(InitiatorName),
            Chance.ToString(CultureInfo.InvariantCulture),
            Proximity.ToString(CultureInfo.InvariantCulture),
            Braced(String.Join(",", TargetNames.Select(Function(n) Quoted(n)))),
            Activation.ToString(),
            Braced(String.Join(",", BehaviorNames.Select(Function(n) Quoted(n)))),
            ReactivationDelay.TotalSeconds.ToString(CultureInfo.InvariantCulture))
    End Function

    Public Function GetReferentialIssues(ponies As PonyCollection) As ImmutableArray(Of ParseIssue) Implements IReferential.GetReferentialIssues
        Dim issues As New List(Of ParseIssue)()
        CheckInteractionReference(ponies, InitiatorName, "Initiator", issues)

        For Each targetName In TargetNames
            CheckInteractionReference(ponies, targetName, "Targets", issues)
        Next
        Return issues.ToImmutableArray()
    End Function

    Private Sub CheckInteractionReference(ponies As PonyCollection, directory As String,
                                               propertyName As String, issues As List(Of ParseIssue))
        Dim base = ponies.Bases.FirstOrDefault(Function(pb) pb.Directory = directory)
        If base Is Nothing Then
            issues.Add(New ParseIssue(propertyName, directory, "",
                                      String.Format(CultureInfo.CurrentCulture, "No pony named '{0}' exists.", directory)))
        Else
            For Each behaviorName In BehaviorNames
                Dim behavior = base.Behaviors.OnlyOrDefault(Function(b) b.Name = behaviorName)
                If behavior Is Nothing Then
                    issues.Add(New ParseIssue("Behaviors", behaviorName, "",
                                              String.Format(CultureInfo.CurrentCulture,
                                                            "'{0}' is missing behavior '{1}'.", directory, behaviorName)))
                End If
            Next
        End If
    End Sub

    Public Property SourceIni As String Implements IPonyIniSourceable.SourceIni

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Name: " & Name
    End Function
End Class
#End Region

#Region "Interaction class"
Public Class Interaction(Of TPony)

    Private _base As InteractionBase
    Public ReadOnly Property Base As InteractionBase
        Get
            Return _base
        End Get
    End Property

    Public Property Initiator As TPony
    Private ReadOnly _involvedTargets As New HashSet(Of TPony)()
    Public ReadOnly Property InvolvedTargets As HashSet(Of TPony)
        Get
            Return _involvedTargets
        End Get
    End Property
    Private ReadOnly _targets As New List(Of TPony)()
    Public ReadOnly Property Targets As List(Of TPony)
        Get
            Return _targets
        End Get
    End Property
    Private ReadOnly _behaviors As New List(Of Behavior)()
    Public ReadOnly Property Behaviors As List(Of Behavior)
        Get
            Return _behaviors
        End Get
    End Property

    Public Sub New(base As InteractionBase)
        _base = Argument.EnsureNotNull(base, "base")
    End Sub

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Base.Name: " & Base.Name
    End Function
End Class

Public Class Interaction
    Inherits Interaction(Of Pony)
    Public Property Trigger As Pony
    Public Sub New(base As InteractionBase)
        MyBase.New(base)
    End Sub
End Class
#End Region

#Region "Behavior class"
Public Class Behavior
    Implements IPonyIniSourceable, IReferential
    Private ReadOnly pony As PonyBase
    Public ReadOnly Property Base As PonyBase
        Get
            Return pony
        End Get
    End Property

    Public Shared ReadOnly AnyGroup As Integer = 0

    Public Property Name As CaseInsensitiveString Implements IPonyIniSerializable.Name
    Public Property Chance As Double
    Public Property MaxDuration As Double 'seconds
    Public Property MinDuration As Double 'seconds

    Private _rightImage As New CenterableSpriteImage()
    Private _leftImage As New CenterableSpriteImage()
    Public ReadOnly Property RightImage As CenterableSpriteImage
        Get
            Return _rightImage
        End Get
    End Property
    Public ReadOnly Property LeftImage As CenterableSpriteImage
        Get
            Return _leftImage
        End Get
    End Property

    Public Property Speed As Double
    Public ReadOnly Property SpeedInPixelsPerSecond As Double
        Get
            Return Speed * (1000.0 / 30.0)
        End Get
    End Property

    Public Property DoNotRepeatImageAnimations As Boolean = False

    Public Property AllowedMovement As AllowedMoves

    Private _linkedBehaviorName As CaseInsensitiveString = ""
    Public Property LinkedBehaviorName As CaseInsensitiveString
        Get
            Return _linkedBehaviorName
        End Get
        Set(value As CaseInsensitiveString)
            _linkedBehaviorName = Argument.EnsureNotNull(value, "value")
        End Set
    End Property
    Private ReadOnly linkedBehaviorNamePredicate As New Func(Of Behavior, Boolean)(Function(b) b.Name = LinkedBehaviorName)
    Public ReadOnly Property LinkedBehavior As Behavior
        Get
            Return pony.Behaviors.OnlyOrDefault(linkedBehaviorNamePredicate)
        End Get
    End Property
    Private _startLineName As CaseInsensitiveString = ""
    Public Property StartLineName As CaseInsensitiveString
        Get
            Return _startLineName
        End Get
        Set(value As CaseInsensitiveString)
            _startLineName = Argument.EnsureNotNull(value, "value")
        End Set
    End Property
    Private ReadOnly startLineNamePredicate As New Func(Of Speech, Boolean)(Function(s) s.Name = StartLineName)
    Public ReadOnly Property StartLine As Speech
        Get
            Return pony.Speeches.OnlyOrDefault(startLineNamePredicate)
        End Get
    End Property
    Private _endLineName As CaseInsensitiveString = ""
    Public Property EndLineName As CaseInsensitiveString
        Get
            Return _endLineName
        End Get
        Set(value As CaseInsensitiveString)
            _endLineName = Argument.EnsureNotNull(value, "value")
        End Set
    End Property
    Private ReadOnly endLineNamePredicate As New Func(Of Speech, Boolean)(Function(s) s.Name = EndLineName)
    Public ReadOnly Property EndLine As Speech
        Get
            Return pony.Speeches.OnlyOrDefault(endLineNamePredicate)
        End Get
    End Property

    Public Property Skip As Boolean = False

    Public Property OriginalDestinationXCoord As Integer = 0
    Public Property OriginalDestinationYCoord As Integer = 0
    Private _originalFollowTargetName As String = ""
    Public Property OriginalFollowTargetName As String
        Get
            Return _originalFollowTargetName
        End Get
        Set(value As String)
            _originalFollowTargetName = Argument.EnsureNotNull(value, "value")
        End Set
    End Property

    Public Property FollowStoppedBehaviorName As CaseInsensitiveString
    Public ReadOnly Property FollowStoppedBehavior As Behavior
        Get
            Return pony.Behaviors.OnlyOrDefault(Function(b) b.Name = FollowStoppedBehaviorName)
        End Get
    End Property
    Public Property FollowMovingBehaviorName As CaseInsensitiveString
    Public ReadOnly Property FollowMovingBehavior As Behavior
        Get
            Return pony.Behaviors.OnlyOrDefault(Function(b) b.Name = FollowMovingBehaviorName)
        End Get
    End Property
    Public Property AutoSelectImagesOnFollow As Boolean = True
    Public Property Group As Integer = AnyGroup

    Public ReadOnly Property Effects As EffectsEnumerable
        Get
            Return New EffectsEnumerable(Me)
        End Get
    End Property

#Region "Effects Enumeration"
    Public Structure EffectsEnumerable
        Implements IEnumerable(Of EffectBase)

        Private ReadOnly behavior As Behavior

        Public Sub New(behavior As Behavior)
            Me.behavior = behavior
        End Sub

        Public Function GetEnumerator() As EffectsEnumerator
            Return New EffectsEnumerator(behavior)
        End Function

        Private Function GetEnumerator1() As IEnumerator(Of EffectBase) Implements IEnumerable(Of EffectBase).GetEnumerator
            Return GetEnumerator()
        End Function

        Private Function GetEnumerator2() As Collections.IEnumerator Implements Collections.IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
    End Structure

    Public Structure EffectsEnumerator
        Implements IEnumerator(Of EffectBase)

        Private ReadOnly behaviorName As CaseInsensitiveString
        Private ReadOnly effects As List(Of EffectBase)
        Private index As Integer

        Public Sub New(behavior As Behavior)
            Argument.EnsureNotNull(behavior, "behavior")
            Me.behaviorName = behavior.Name
            Me.effects = behavior.pony.Effects
            Me.index = -1
        End Sub

        Public ReadOnly Property Current As EffectBase Implements IEnumerator(Of EffectBase).Current
            Get
                Return effects(index)
            End Get
        End Property

        Private ReadOnly Property Current1 As Object Implements Collections.IEnumerator.Current
            Get
                Return Current
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements Collections.IEnumerator.MoveNext
            Do
                index += 1
            Loop While index < effects.Count AndAlso Current.BehaviorName <> behaviorName
            Return index < effects.Count
        End Function

        Public Sub Reset() Implements Collections.IEnumerator.Reset
            index = -1
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub
    End Structure
#End Region

    Public Sub New(pony As PonyBase)
        Me.pony = Argument.EnsureNotNull(pony, "pony")
    End Sub

    Public Shared Function TryLoad(iniLine As String, imageDirectory As String, pony As PonyBase, ByRef result As Behavior, ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        result = Nothing
        issues = Nothing

        Dim b = New Behavior(pony)
        b.SourceIni = iniLine
        Dim p As New StringCollectionParser(CommaSplitQuoteQualified(iniLine),
                                            {"Identifier", "Name", "Chance",
                                             "Max Duration", "Min Duration", "Speed",
                                             "Right Image", "Left Image", "Movement", "Linked Behavior",
                                             "Start Speech", "End Speech", "Skip", "Destination X", "Destination Y",
                                             "Follow Target", "Auto Select Follow Images",
                                             "Follow Stopped Behavior", "Follow Moving Behavior",
                                             "Right Image Center", "Left Image Center",
                                             "Prevent Animation Loop", "Group"})
        p.NoParse()
        b.Name = If(p.NotNullOrWhiteSpace(), "")
        b.Chance = p.ParseDouble(0, 0, 1)
        b.MaxDuration = p.ParseDouble(15, 0, 300)
        b.MinDuration = p.ParseDouble(5, 0, 300)
        b.Speed = p.ParseDouble(3, 0, 25)
        b.RightImage.Path = p.NoParse()
        If p.Assert(b.RightImage.Path, Function(s) Not String.IsNullOrEmpty(s), "An image path has not been set.", Nothing) Then
            b.RightImage.Path = p.SpecifiedCombinePath(imageDirectory, b.RightImage.Path, "Image will not be loaded.")
            p.SpecifiedFileExists(b.RightImage.Path)
        End If
        b.LeftImage.Path = p.NoParse()
        If p.Assert(b.LeftImage.Path, Function(s) Not String.IsNullOrEmpty(s), "An image path has not been set.", Nothing) Then
            b.LeftImage.Path = p.SpecifiedCombinePath(imageDirectory, b.LeftImage.Path, "Image will not be loaded.")
            p.SpecifiedFileExists(b.LeftImage.Path)
        End If
        b.AllowedMovement = p.Map(AllowedMovesFromIni, AllowedMoves.All)
        b.LinkedBehaviorName = p.NotNull("").Trim()
        b.StartLineName = p.NotNull("").Trim()
        b.EndLineName = p.NotNull("").Trim()
        b.Skip = p.ParseBoolean(False)
        b.OriginalDestinationXCoord = p.ParseInt32(0)
        b.OriginalDestinationYCoord = p.ParseInt32(0)
        b.OriginalFollowTargetName = p.NotNull("").Trim()
        b.AutoSelectImagesOnFollow = p.ParseBoolean(True)
        b.FollowStoppedBehaviorName = p.NotNull("").Trim()
        b.FollowMovingBehaviorName = p.NotNull("").Trim()
        b.RightImage.CustomCenter = p.ParseVector2(Vector2.Zero)
        b.LeftImage.CustomCenter = p.ParseVector2(Vector2.Zero)
        If b.RightImage.CustomCenter = Vector2.Zero Then b.RightImage.CustomCenter = Nothing
        If b.LeftImage.CustomCenter = Vector2.Zero Then b.LeftImage.CustomCenter = Nothing
        b.DoNotRepeatImageAnimations = p.ParseBoolean(False)
        b.Group = p.ParseInt32(AnyGroup, 0, 100)

        issues = p.Issues.ToImmutableArray()
        result = b
        Return p.Result
    End Function

    Public Sub AddEffect(effectname As String, right_path As String, left_path As String, duration As Double, repeat_delay As Double,
                         direction_right As Direction, centering_right As Direction,
                         direction_left As Direction, centering_left As Direction,
                         follow As Boolean, _dont_repeat_image_animations As Boolean, owner As PonyBase)

        Dim newEffect As New EffectBase(effectname, left_path, right_path) With {
            .BehaviorName = Name,
            .Duration = duration,
            .RepeatDelay = repeat_delay,
            .PlacementDirectionLeft = direction_left,
            .CenteringLeft = centering_left,
            .PlacementDirectionRight = direction_right,
            .CenteringRight = centering_right,
            .Follow = follow,
            .DoNotRepeatImageAnimations = _dont_repeat_image_animations,
            .ParentPonyBase = owner
        }
        pony.Effects.Add(newEffect)
    End Sub

    Public Function GetPonyIni() As String Implements IPonyIniSerializable.GetPonyIni
        Dim rightCenter = If(RightImage.CustomCenter, Vector2.Zero)
        Dim leftCenter = If(LeftImage.CustomCenter, Vector2.Zero)
        Return String.Join(
            ",", "Behavior",
            Quoted(Name),
            Chance.ToString(CultureInfo.InvariantCulture),
            MaxDuration.ToString(CultureInfo.InvariantCulture),
            MinDuration.ToString(CultureInfo.InvariantCulture),
            Speed.ToString(CultureInfo.InvariantCulture),
            Quoted(Path.GetFileName(RightImage.Path)),
            Quoted(Path.GetFileName(LeftImage.Path)),
            Space_To_Under(AllowedMovesToString(AllowedMovement)),
            Quoted(LinkedBehaviorName),
            Quoted(StartLineName),
            Quoted(EndLineName),
            Skip,
            OriginalDestinationXCoord.ToString(CultureInfo.InvariantCulture),
            OriginalDestinationYCoord.ToString(CultureInfo.InvariantCulture),
            Quoted(OriginalFollowTargetName),
            AutoSelectImagesOnFollow,
            FollowStoppedBehaviorName,
            FollowMovingBehaviorName,
            Quoted(rightCenter.X.ToString(CultureInfo.InvariantCulture) & "," &
                   rightCenter.Y.ToString(CultureInfo.InvariantCulture)),
            Quoted(leftCenter.X.ToString(CultureInfo.InvariantCulture) & "," &
                   leftCenter.Y.ToString(CultureInfo.InvariantCulture)),
            DoNotRepeatImageAnimations,
            Group.ToString(CultureInfo.InvariantCulture))
    End Function

    Public Function Clone() As IPonyIniSourceable Implements IPonyIniSourceable.Clone
        Dim copy = DirectCast(MyBase.MemberwiseClone(), Behavior)
        copy._leftImage = New CenterableSpriteImage()
        copy._leftImage.Path = _leftImage.Path
        copy._leftImage.CustomCenter = _leftImage.CustomCenter
        copy._rightImage = New CenterableSpriteImage()
        copy._rightImage.Path = _rightImage.Path
        copy._rightImage.CustomCenter = _rightImage.CustomCenter
        Return copy
    End Function

    Public Function GetReferentialIssues(ponies As PonyCollection) As ImmutableArray(Of ParseIssue) Implements IReferential.GetReferentialIssues
        Return {Referential.CheckUnique("Linked Behavior", LinkedBehaviorName, pony.Behaviors.Select(Function(b) b.Name)),
                Referential.CheckNotCircular("Linked Behavior", Name, LinkedBehavior,
                                             Function(b) b.LinkedBehavior, Function(b) b.Name),
                Referential.CheckUnique("Start Speech", StartLineName, pony.Speeches.Select(Function(s) s.Name)),
                Referential.CheckUnique("End Speech", EndLineName, pony.Speeches.Select(Function(s) s.Name)),
                Referential.CheckUnique("Follow Stopped Behavior", FollowStoppedBehaviorName, pony.Behaviors.Select(Function(b) b.Name)),
                Referential.CheckUnique("Follow Moving Behavior", FollowMovingBehaviorName, pony.Behaviors.Select(Function(b) b.Name)),
                Referential.CheckUnique("Follow Target", OriginalFollowTargetName, ponies.Bases.Select(Function(pb) pb.Directory))}.
        Where(Function(pi) pi.PropertyName IsNot Nothing).ToImmutableArray()
    End Function

    Public Property SourceIni As String Implements IPonyIniSourceable.SourceIni

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Name: " & Name
    End Function
End Class
#End Region

#Region "BehaviorGroup class"
Public Class BehaviorGroup
    Implements IPonyIniSerializable

    Public Property Name As CaseInsensitiveString Implements IPonyIniSerializable.Name
    Public Property Number As Integer

    Public Sub New(_name As CaseInsensitiveString, _number As Integer)
        Name = _name
        Number = _number
    End Sub

    Public Function GetPonyIni() As String Implements IPonyIniSerializable.GetPonyIni
        Return String.Join(",", "behaviorgroup", Number, Name)
    End Function

    Public Overrides Function ToString() As String
        Return "[" & Number & ", " & Name & "]"
    End Function
End Class
#End Region

#Region "Speech class"
Public Class Speech
    Implements IPonyIniSourceable

    Public Property Name As CaseInsensitiveString Implements IPonyIniSerializable.Name
    Public Property Text As String = ""
    Public Property SoundFile As String
    Public Property Skip As Boolean = False 'don't use randomly if true
    Public Property Group As Integer = 0 'the behavior group that this line is assigned to.  0 = all

    Public Shared Function TryLoad(iniLine As String, soundDirectory As String,
                                   ByRef result As Speech, ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        result = Nothing
        issues = Nothing

        Dim s = New Speech()
        s.SourceIni = iniLine
        Dim iniComponents = CommaSplitQuoteBraceQualified(iniLine)
        If iniComponents.Length = 1 Then iniComponents = {Nothing, iniComponents(0)}
        If iniComponents.Length > 3 Then
            Dim soundFilePaths = CommaSplitQuoteQualified(iniComponents(3))
            iniComponents(3) = Nothing
            For Each filePath In soundFilePaths
                If String.Equals(Path.GetExtension(filePath), ".mp3", PathEquality.Comparison) Then
                    iniComponents(3) = filePath
                    Exit For
                End If
            Next
        End If
        Dim p As New StringCollectionParser(iniComponents,
                                            {"Identifier", "Name", "Text", "Sound Files", "Skip", "Group"})
        p.NoParse()
        s.Name = p.NotNullOrWhiteSpace("Unnamed")
        s.Text = p.NotNull()
        s.SoundFile = p.NoParse()
        If s.SoundFile IsNot Nothing Then
            s.SoundFile = p.SpecifiedCombinePath(soundDirectory, s.SoundFile, "Sound file will not be played.")
            p.SpecifiedFileExists(s.SoundFile, "Sound file will not be played.")
        End If
        s.Skip = p.ParseBoolean(False)
        s.Group = p.ParseInt32(Behavior.AnyGroup, 0, 100)

        issues = p.Issues.ToImmutableArray()
        result = s
        Return p.Result
    End Function

    Public Function GetPonyIni() As String Implements IPonyIniSerializable.GetPonyIni
        'For compatibility with 'Browser Ponies', we write an .OGG file as the 2nd option.
        If SoundFile Is Nothing Then
            Return String.Join(
                              ",", "Speak",
                              Quoted(Name),
                              Quoted(Text),
                              "",
                              Skip,
                              Group)
        Else
            Dim soundFilePath = Path.GetFileName(SoundFile)
            Return String.Join(
                              ",", "Speak",
                              Quoted(Name),
                              Quoted(Text),
                              Braced(String.Join(",",
                                                 Quoted(soundFilePath),
                                                 Quoted(Path.ChangeExtension(soundFilePath, ".ogg"))
                                                 )),
                              Skip,
                              Group)
        End If
    End Function

    Public Function Clone() As IPonyIniSourceable Implements IPonyIniSourceable.Clone
        Return DirectCast(MyBase.MemberwiseClone(), Speech)
    End Function

    Public Property SourceIni As String Implements IPonyIniSourceable.SourceIni

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Name: " & Name
    End Function
End Class
#End Region

#Region "Pony class"
Public Class Pony
    Implements ISpeakingSprite, IDraggableSprite, IExpireableSprite, ISoundfulSprite

    ''' <summary>
    ''' Number of milliseconds by which the internal temporal state of the sprite should be advanced with each call to UpdateOnce().
    ''' </summary>
    Private Const StepRate = 1000.0 / 30.0

#Region "Update Records"
    Friend UpdateRecord As List(Of Record)

    Friend Structure Record
        Public Time As TimeSpan
        Public Info As String
        Public Sub New(time As TimeSpan, info As String)
            Me.Time = time
            Me.Info = info
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format(CultureInfo.CurrentCulture, "{0:000.000} {1}", Time.TotalSeconds, Info)
        End Function
    End Structure

    Private Sub AddUpdateRecord(info As String)
        If UpdateRecord Is Nothing Then Return
        SyncLock UpdateRecord
            UpdateRecord.Add(New Record(internalTime, info))
        End SyncLock
    End Sub

    Private Sub AddUpdateRecord(info As String, info2 As String)
        If UpdateRecord Is Nothing Then Return
        SyncLock UpdateRecord
            UpdateRecord.Add(New Record(internalTime, info + info2))
        End SyncLock
    End Sub
#End Region

#Region "Fields"
    Private ReadOnly _base As PonyBase
    Public ReadOnly Property Base() As PonyBase
        Get
            Return _base
        End Get
    End Property

#Region "Compatibility Properties"
    Friend ReadOnly Property Directory() As String
        Get
            Return Base.Directory
        End Get
    End Property
    Public ReadOnly Property DisplayName() As String
        Get
            Return Base.DisplayName
        End Get
    End Property
    Friend ReadOnly Property Behaviors() As List(Of Behavior)
        Get
            Return Base.Behaviors
        End Get
    End Property
#End Region

    Private _expired As Boolean
    Private ReadOnly interactions As New List(Of Interaction)()

    Public Property Sleep As Boolean
    Private _sleeping As Boolean
    Public ReadOnly Property Sleeping As Boolean
        Get
            Return _sleeping
        End Get
    End Property

    Public Property BeingDragged As Boolean Implements IDraggableSprite.Drag

    Public Property CurrentBehaviorGroup As Integer

    Private _currentInteraction As Interaction = Nothing
    Public Property CurrentInteraction As Interaction
        Get
            Return _currentInteraction
        End Get
        Private Set(value As Interaction)
            _currentInteraction = value
        End Set
    End Property
    Private isInteractionInitiator As Boolean

    Public Property IsInteracting As Boolean
    Public Property PlayingGame As Boolean

    Private verticalMovementAllowed As Boolean
    Private horizontalMovementAllowed As Boolean
    Public Property facingUp As Boolean
    Public Property facingRight As Boolean = True
    ''' <summary>
    ''' The angle to travel in, if moving diagonally (in radians).
    ''' </summary>
    Public Property Diagonal As Double

    ''' <summary>
    ''' Time until interactions should be disabled.
    ''' Stops interactions from repeating too soon after one another.
    ''' Only affects the triggering pony and not targets.
    ''' </summary>
    Private interactionDelayUntil As TimeSpan

    Private _currentBehavior As Behavior
    Public Property CurrentBehavior As Behavior
        Get
            Return _currentBehavior
        End Get
        Friend Set(value As Behavior)
            If Not ValidateBehavior(value) Then Return
            _currentBehavior = value
            If Not (ManualControlPlayerOne OrElse ManualControlPlayerTwo) Then
                SetAllowableDirections()
            End If
        End Set
    End Property

    Private Function ValidateBehavior(behavior As Behavior) As Boolean
        Return behavior Is Nothing OrElse Base.ValidatedOnLoad OrElse
            (File.Exists(behavior.LeftImage.Path) AndAlso File.Exists(behavior.RightImage.Path))
    End Function

    Private currentCustomImageCenter As Size
    Private ReadOnly Property isCustomImageCenterDefined As Boolean
        Get
            Return currentCustomImageCenter <> Size.Empty
        End Get
    End Property

    ''' <summary>
    ''' Only used when temporarily pausing, like when the mouse hovers over us.
    ''' </summary>
    Private previousBehavior As Behavior

    ''' <summary>
    ''' When set, specifics the alternate set of images that should replace those of the current behavior.
    ''' </summary>
    Friend visualOverrideBehavior As Behavior

    Private _returningToScreenArea As Boolean
    Public Property ReturningToScreenArea As Boolean
        Get
            Return _returningToScreenArea
        End Get
        Private Set(value As Boolean)
            _returningToScreenArea = value
        End Set
    End Property

    ''' <summary>
    ''' Used when going back "in" houses.
    ''' </summary>
    Public Property GoingHome As Boolean
    ''' <summary>
    ''' Used when a pony has been recalled and is just about to "enter" a house
    ''' </summary>
    Public Property OpeningDoor As Boolean

    ''' <summary>
    ''' Should we stop because the cursor is hovered over?
    ''' </summary>
    Private CursorOverPony As Boolean

    ''' <summary>
    ''' Are we actually halted now?
    ''' </summary>
    Private HaltedForCursor As Boolean
    ''' <summary>
    ''' Number of ticks for which the pony is immune to cursor interaction.
    ''' </summary>
    Private CursorImmunity As Integer = 0
    Private currentMouseoverBehavior As Behavior

    Public Property Destination As Vector2
    Public Property AtDestination As Boolean
    Private ReadOnly Property hasDestination As Boolean
        Get
            Return Destination <> Vector2.Zero
        End Get
    End Property

    ''' <summary>
    ''' Used in the Paint() sub to help stop flickering between left and right images under certain circumstances.
    ''' </summary>
    Private paintStop As Boolean

    Private _topLeftLocation As Point
    ''' <summary>
    ''' The location on the screen.
    ''' </summary>
    Public ReadOnly Property TopLeftLocation As Point
        Get
            Return _topLeftLocation
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the location of the center of the pony.
    ''' </summary>
    Public Property Location As Point
        Get
            Return CenterLocation()
        End Get
        Set(value As Point)
            _topLeftLocation = Point.Round(value - GetImageCenterOffset())
        End Set
    End Property

    ''' <summary>
    ''' Used for predicting future movement (just more of what we last did)
    ''' </summary>
    Private lastMovement As Vector2F

    Private ReadOnly _activeEffects As New HashSet(Of Effect)()

    Public Property BehaviorStartTime As TimeSpan
    Public Property BehaviorDesiredDuration As TimeSpan

    Public Property ManualControlPlayerOne As Boolean
    Public Property ManualControlPlayerTwo As Boolean
    Private _manualControlAction As Boolean
    Public ReadOnly Property ManualControlAction As Boolean
        Get
            Return _manualControlAction
        End Get
    End Property

    Private ReadOnly effectsLastUsed As New Dictionary(Of EffectBase, TimeSpan)()
    Private ReadOnly effectsAlreadyPlayedForBehavior As New HashSet(Of EffectBase)()

    Public Property destinationCoords As Point
    Public Property followTargetName As String = ""
    Public Property followTarget As ISprite
    'Try to get the point where an object is going to, and go to that instead of where it is currently at.
    Public Property leadTarget As Boolean

    'Used when following.
    Private _delay As Integer
    Public Property Delay As Integer
        Get
            Return _delay
        End Get
        Private Set(value As Integer)
            _delay = value
        End Set
    End Property
    Private blocked As Boolean

    Private lastSpeakTime As TimeSpan = TimeSpan.FromDays(-1)
    Private lastSpeakLine As String
    Private lastSpeakSound As String
    Friend internalTime As TimeSpan
    Private lastUpdateTime As TimeSpan

    Private ReadOnly possibleMoveModes As New List(Of AllowedMoves)(3)

    Public ReadOnly Property Scale() As Double
        Get
            Return If(Base.Scale <> 0, Base.Scale, Options.ScaleFactor)
        End Get
    End Property
#End Region

    ' TODO: Review accessibility.

    Public Sub New(base As PonyBase)
        _base = Argument.EnsureNotNull(base, "base")
        If Options.EnablePonyLogs Then UpdateRecord = New List(Of Record)()
    End Sub

    ''' <summary>
    ''' Starts the sprite.
    ''' </summary>
    ''' <param name="startTime">The current time of the animator, which will be the temporal zero point for this sprite.</param>
    Public Sub Start(startTime As TimeSpan) Implements ISprite.Start
        CurrentBehavior = Behaviors.FirstOrDefault(AddressOf ValidateBehavior)
        internalTime = startTime
        lastUpdateTime = startTime
        Teleport()
    End Sub

    ''' <summary>
    ''' Teleport the pony to a random location within bounds.
    ''' </summary>
    Private Sub Teleport()
        ' If we are in preview mode, just teleport into the center for consistency.
        If EvilGlobals.InPreviewMode Then
            Location = Point.Round(EvilGlobals.PreviewWindowRectangle.Center())
            Exit Sub
        End If

        ' Try an arbitrary number of times to find a point a point in bounds that is not also in the exclusion zone.
        ' TODO: Create method that will uniformly choose a random location from allowable points, also taking into account image sizing.
        Dim teleportLocation As Point
        For tries = 0 To 300
            Dim area = Options.Screens(Rng.Next(Options.Screens.Count)).WorkingArea
            teleportLocation = New Point(
                CInt(area.X + Rng.NextDouble() * area.Width),
                CInt(area.Y + Rng.NextDouble() * area.Height))
            If Not IsPonyIntersectingWithAvoidanceArea(teleportLocation) Then Exit For
        Next
        Location = teleportLocation
    End Sub

    ''' <summary>
    ''' Updates the sprite, bringing its state as close to the specified time as possible.
    ''' </summary>
    ''' <param name="updateTime">The current time of the animator, to which the sprite should match its state.</param>
    Public Sub Update(updateTime As TimeSpan) Implements ISprite.Update
        ' Find out how far behind the sprite is since its last update, and catch up.
        ' The time factor here means the internal time of the sprite can be advanced at different rates than the external time.
        ' This fixed time step method of updating is prone to temporal aliasing, but this is largely unnoticeable compared to the generally
        ' low frame rate of animations and lack of spatial anti-aliasing since the images are pixel art. That said, the time scaling should
        ' be constrained from being too low (which will exaggerate the temporal aliasing until it is noticeable) or too high (which kills
        ' performance as UpdateOnce must be evaluated many times to catch up).
        lastSpeakSound = Nothing
        Dim difference = updateTime - lastUpdateTime
        While difference.TotalMilliseconds > 0 AndAlso Not _expired
            UpdateOnce()
            difference -= TimeSpan.FromMilliseconds(StepRate / Options.TimeFactor)
        End While
        lastUpdateTime = updateTime - difference
    End Sub

    ''' <summary>
    ''' Advances the internal time state of the pony by the step rate with each call.
    ''' </summary>
    Private Sub UpdateOnce()
        ' If there are no behaviors that can be undertaken, there's nothing that needs updating anyway.
        If Not Behaviors.Any(AddressOf ValidateBehavior) Then
            CurrentBehavior = Nothing
            Return
        End If

        internalTime += TimeSpan.FromMilliseconds(StepRate)

        ' Expire any speech.
        If internalTime - lastSpeakTime > TimeSpan.FromSeconds(2) Then lastSpeakLine = Nothing

        ' Handle switching pony between active and asleep.
        Dim wantToSleep = Sleep OrElse BeingDragged
        If wantToSleep AndAlso Not _sleeping Then
            PutToSleep()
            AddUpdateRecord("Pony should be sleeping.")
            Exit Sub
        ElseIf Not wantToSleep AndAlso _sleeping Then
            WakeUp()
        End If

        If BeingDragged Then Location = EvilGlobals.CursorLocation

        ' If we have no specified behavior, make sure the returning to screen flag is not set.
        If CurrentBehavior Is Nothing Then ReturningToScreenArea = False

        ' If we're not in a special mode, we need to check if behaviors should be cycled.
        If Not PlayingGame AndAlso Not ReturningToScreenArea Then
            If CurrentBehavior Is Nothing Then
                ' If a current behavior has yet to be specified, we need to pick something to do.
                CancelInteraction()
                SelectBehavior()
                AddUpdateRecord("Selected a new behavior at random (UpdateOnce). Behavior: ", CurrentBehavior.Name)
            ElseIf internalTime > (BehaviorStartTime + BehaviorDesiredDuration) AndAlso
                Not ManualControlPlayerOne AndAlso
                Not ManualControlPlayerTwo Then
                ' The behavior has expired and we are not under manual control.

                ' If the cursor is hovered over the pony, just keep repeating the current behavior. Otherwise, the current behavior should 
                ' be ended and a new one selected.
                If CursorOverPony Then
                    SelectBehavior(CurrentBehavior)
                    AddUpdateRecord("Repeating current behavior; cursor is over the pony. Behavior: ", CurrentBehavior.Name)
                Else
                    ' Speak the end line for the behavior, if one is specified.
                    If CurrentBehavior.EndLine IsNot Nothing Then PonySpeak(CurrentBehavior.EndLine)
                    ' Use the next behavior in the chain if one is specified, else select one at random.
                    AddUpdateRecord("Switching to the next behavior in the chain. Linked: ",
                                    If(CurrentBehavior.LinkedBehavior IsNot Nothing,
                                       CurrentBehavior.LinkedBehavior.Name.ToString(), "<null>"))
                    SelectBehavior(CurrentBehavior.LinkedBehavior)
                End If
            End If

            ' Account for changes in mouseover state.
            ChangeMouseOverMode()
        End If

        ' Now a behavior has been set, move accordingly.
        Move()
        ' Activate any effects associated with the new behavior.
        ActivateEffects(internalTime)

        If AtDestination AndAlso GoingHome AndAlso OpeningDoor AndAlso Delay <= 0 Then
            Expire()
        End If
    End Sub

    ''' <summary>
    ''' Chooses a behavior to use for sleeping and activates it with no timeout.
    ''' </summary>
    Public Sub PutToSleep()
        ' Choose, in descending order of preference:
        ' - The dragging behavior, when actively being dragged
        ' - The dedicated sleeping behavior
        ' - The dedicated mouseover behavior
        ' - Any no movement behavior
        ' - The current behavior as a last resort

        Dim sleepBehavior As Behavior = Nothing
        If BeingDragged Then sleepBehavior = GetAppropriateBehavior(AllowedMoves.Dragged, False)
        If sleepBehavior Is Nothing Then sleepBehavior = GetAppropriateBehavior(AllowedMoves.Sleep, False)
        If sleepBehavior Is Nothing Then sleepBehavior = GetAppropriateBehavior(AllowedMoves.MouseOver, False)
        If sleepBehavior Is Nothing Then sleepBehavior = GetAppropriateBehavior(AllowedMoves.None, False)
        If sleepBehavior Is Nothing Then sleepBehavior = CurrentBehavior

        SelectBehavior(sleepBehavior)
        BehaviorDesiredDuration = TimeSpan.FromHours(8)
        Paint()
        _sleeping = True
    End Sub

    ''' <summary>
    ''' Wakes a pony from their sleeping behavior.
    ''' </summary>
    Public Sub WakeUp()
        _sleeping = False
        CursorOverPony = False

        'Ponies added during sleep will not be initialized yet, so don't paint them.
        If CurrentBehavior IsNot Nothing Then
            BehaviorDesiredDuration = TimeSpan.Zero
            Paint()
        End If
    End Sub

    ''' <summary>
    ''' Cancels the interaction the pony is involved in. If the pony is the initiator of an interaction, ensures it is canceled for all the
    ''' targets of the interaction.
    ''' </summary>
    Private Sub CancelInteraction()
        IsInteracting = False

        If CurrentInteraction Is Nothing Then Exit Sub

        If isInteractionInitiator Then
            For Each pony In CurrentInteraction.Targets
                ' Check the target is still running the interaction that the current pony initiated, then cancel it.
                If Not ReferenceEquals(Me, pony) AndAlso
                    pony.CurrentInteraction IsNot Nothing AndAlso
                    pony.CurrentInteraction.Initiator IsNot Nothing AndAlso
                    ReferenceEquals(Me, pony.CurrentInteraction.Initiator) Then
                    pony.CancelInteraction()
                End If
            Next
        End If

        AddUpdateRecord("Canceled interaction. IsInteractionInitiator: ", isInteractionInitiator.ToString())

        interactionDelayUntil = internalTime + CurrentInteraction.Base.ReactivationDelay
        CurrentInteraction = Nothing
        isInteractionInitiator = False
    End Sub

    ''' <summary>
    ''' Ends the current behavior and begins a new behavior. One is chosen at random unless a behavior is specified.
    ''' </summary>
    ''' <param name="specifiedBehavior">The behavior that the pony should switch to, or null to choose one at random.</param>
    Public Sub SelectBehavior(Optional specifiedBehavior As Behavior = Nothing)
        ' Having no specified behavior when interacting means we've run to the last part of a chain and should end the interaction.
        If IsInteracting AndAlso isInteractionInitiator AndAlso specifiedBehavior Is Nothing Then CancelInteraction()

        ' Clear following state.
        followTarget = Nothing
        followTargetName = ""

        If specifiedBehavior Is Nothing Then
            ' Pick a behavior at random. If a valid behavior cannot be selected after an arbitrary number of tries, just continue using the
            ' current behavior for now.
            Dim foundAtRandom As Boolean
            For i = 0 To 200
                Dim potentialBehavior = Behaviors(Rng.Next(Behaviors.Count))

                ' The behavior can't be disallowed from running randomly, and it must in in the same group or the "any" group.
                ' Then, do a random test against the chance the behavior can occur.
                If Not potentialBehavior.Skip AndAlso
                    (potentialBehavior.Group = CurrentBehaviorGroup OrElse potentialBehavior.Group = Behavior.AnyGroup) AndAlso
                    Rng.NextDouble() <= potentialBehavior.Chance Then

                    ' See if the behavior specifies that we follow another object.
                    followTargetName = potentialBehavior.OriginalFollowTargetName
                    Destination = DetermineDestination()

                    ' The behavior specifies an object to follow, but no instance of that object is present.
                    ' We can't use this behavior, so we'll have to choose another.
                    If Not hasDestination AndAlso potentialBehavior.OriginalFollowTargetName <> "" Then
                        followTargetName = ""
                        Continue For
                    End If

                    ' We managed to decide on a behavior at random.
                    CurrentBehavior = potentialBehavior
                    foundAtRandom = True
                    AddUpdateRecord("Selected a new behavior at random (SelectBehavior). Behavior: ", CurrentBehavior.Name)
                    Exit For
                End If
            Next

            ' If we couldn't find one at random, we need to switch to a default behavior. The current interaction behavior is likely not
            ' suitable to repeat.
            If Not foundAtRandom AndAlso (IsInteracting OrElse CurrentBehavior Is Nothing) Then
                CurrentBehavior = Behaviors.First(AddressOf ValidateBehavior)
                AddUpdateRecord(
                    If(IsInteracting,
                       "Random selection failed. Using default behavior as interaction is running. (SelectBehavior). Behavior: ",
                       "Random selection failed. Using default behavior as no behavior has been set yet. (SelectBehavior). Behavior: "),
                    CurrentBehavior.Name)
            ElseIf Not foundAtRandom Then
                AddUpdateRecord(
                    "Random selection failed. Continuing current behavior as no interaction is running. (SelectBehavior). Behavior: ",
                    CurrentBehavior.Name)
            End If
        Else
            followTargetName = specifiedBehavior.OriginalFollowTargetName
            Destination = DetermineDestination()

            ' The behavior specifies an object to follow, but no instance of that object is present.
            ' We can't use this behavior, so we'll have to choose another at random.
            If Not hasDestination AndAlso specifiedBehavior.OriginalFollowTargetName <> "" AndAlso
                Not EvilGlobals.InPreviewMode Then
                SelectBehavior()
                Exit Sub
            End If
            CurrentBehavior = specifiedBehavior
            AddUpdateRecord("Selected a specified behavior (SelectBehavior). Behavior: ", CurrentBehavior.Name)
        End If

        Diagnostics.Debug.Assert(CurrentBehavior IsNot Nothing)

        CurrentBehaviorGroup = CurrentBehavior.Group

        ' Reset effects.
        effectsAlreadyPlayedForBehavior.Clear()

        BehaviorStartTime = internalTime
        BehaviorDesiredDuration = TimeSpan.FromSeconds(
            (Rng.NextDouble() * (CurrentBehavior.MaxDuration - CurrentBehavior.MinDuration) + CurrentBehavior.MinDuration))

        ' Speak the starting line now, if one is specified; otherwise speak a random line by chance, but only if it won't get in the way
        ' later.
        If CurrentBehavior.StartLine IsNot Nothing Then
            PonySpeak(CurrentBehavior.StartLine)
        ElseIf followTargetName = "" AndAlso Not IsInteracting AndAlso
            CurrentBehavior.EndLine Is Nothing AndAlso Rng.NextDouble() <= Options.PonySpeechChance Then
            PonySpeak()
        End If

        If CurrentBehavior.AllowedMovement = AllowedMoves.None OrElse
            CurrentBehavior.AllowedMovement = AllowedMoves.MouseOver OrElse
            CurrentBehavior.AllowedMovement = AllowedMoves.Sleep OrElse
            CurrentBehavior.AllowedMovement = AllowedMoves.Dragged Then
            ' Prevent any movement for these states.
            horizontalMovementAllowed = False
            verticalMovementAllowed = False
        Else
            ' Set directions that may be moved in for this behavior.
            SetAllowableDirections()
        End If

        ' Choose to face/move along each axis at random.
        facingUp = Rng.NextDouble() < 0.5
        facingRight = Rng.NextDouble() < 0.5
    End Sub

    ''' <summary>
    ''' Chooses allowable movements states for the pony based on its current behavior.
    ''' </summary>
    Private Sub SetAllowableDirections()
        ' Determine move modes that can be used.
        possibleMoveModes.Clear()
        If CurrentBehavior IsNot Nothing Then
            If (CurrentBehavior.AllowedMovement And AllowedMoves.HorizontalOnly) = AllowedMoves.HorizontalOnly Then
                possibleMoveModes.Add(AllowedMoves.HorizontalOnly)
            End If
            If (CurrentBehavior.AllowedMovement And AllowedMoves.VerticalOnly) = AllowedMoves.VerticalOnly Then
                possibleMoveModes.Add(AllowedMoves.VerticalOnly)
            End If
            If (CurrentBehavior.AllowedMovement And AllowedMoves.DiagonalOnly) = AllowedMoves.DiagonalOnly Then
                possibleMoveModes.Add(AllowedMoves.DiagonalOnly)
            End If
        End If

        ' Select a mode at random, or else deny movement.
        Dim selectedMoveMode As AllowedMoves = AllowedMoves.None
        If possibleMoveModes.Count > 0 Then
            selectedMoveMode = possibleMoveModes(Rng.Next(possibleMoveModes.Count))
        End If

        ' Depending on mode, set allowable movement state for the pony.
        Select Case selectedMoveMode
            Case AllowedMoves.None
                verticalMovementAllowed = False
                horizontalMovementAllowed = False
                Diagonal = 0
            Case AllowedMoves.HorizontalOnly
                horizontalMovementAllowed = True
                verticalMovementAllowed = False
                Diagonal = 0
            Case AllowedMoves.VerticalOnly
                horizontalMovementAllowed = False
                verticalMovementAllowed = True
                Diagonal = 0
            Case AllowedMoves.DiagonalOnly
                horizontalMovementAllowed = True
                verticalMovementAllowed = True
                ' Pick a random angle to travel at.
                If facingUp Then
                    Diagonal = ((Rng.NextDouble() * 35) + 15) * (Math.PI / 180)
                Else
                    Diagonal = ((Rng.NextDouble() * 35) + 310) * (Math.PI / 180)
                End If
                If Not facingRight Then Diagonal = Math.PI - Diagonal
        End Select
    End Sub

    Private ReadOnly isSpeechInUsableGroupPredicate As New Func(Of Speech, Boolean)(
        Function(s) s.Group = Behavior.AnyGroup OrElse s.Group = CurrentBehavior.Group)
    ''' <summary>
    ''' Prompts the pony to speak a line if it has not done so recently. A random line is chosen unless one is specified.
    ''' </summary>
    ''' <param name="line">The line the pony should speak, or null to choose one at random.</param>
    Public Sub PonySpeak(Optional line As Speech = Nothing)
        'When the cursor is over us, don't talk too often.
        If CursorOverPony AndAlso (internalTime - lastSpeakTime).TotalSeconds < 15 Then
            Return
        End If

        ' Select a line at random from the lines that may be played at random that are in the current group.
        If line Is Nothing Then
            If Base.Speeches.Count = 0 Then Return
            Dim randomGroupLines = Base.SpeechesRandom.Where(isSpeechInUsableGroupPredicate).ToArray()
            If randomGroupLines.Length = 0 Then Return
            line = randomGroupLines(Rng.Next(randomGroupLines.Length))
        End If

        ' Set the line text to be displayed.
        If Options.PonySpeechEnabled Then
            lastSpeakTime = internalTime
            lastSpeakLine = Me.DisplayName & ": " & ControlChars.Quote & line.Text & ControlChars.Quote
        End If

        ' Start the sound file playing.
        If line.SoundFile <> "" Then
            lastSpeakSound = line.SoundFile
        End If
    End Sub

    ''' <summary>
    ''' Checks the current mouseover state of the pony and toggles between mouseover modes accordingly.
    ''' </summary>
    Private Sub ChangeMouseOverMode()
        If CursorOverPony AndAlso Not HaltedForCursor Then
            ' The cursor has moved over us and we should halt.
            HaltedForCursor = True
            previousBehavior = CurrentBehavior

            ' Remove effects for the previous behavior. We don't want them lingering when the behavior restarts after the mouse moves away.
            If _activeEffects.Count > 0 Then
                For Each effect In _activeEffects.Where(Function(e) e.Base.BehaviorName = previousBehavior.Name).ToImmutableArray()
                    effect.Expire()
                Next
            End If

            ' Change to mouseover behavior.
            CurrentBehavior = currentMouseoverBehavior
            effectsAlreadyPlayedForBehavior.Clear()
            Paint()
            AddUpdateRecord("Changed into mouseover state.")
        ElseIf Not CursorOverPony And HaltedForCursor Then
            ' The cursor has moved away from us and we no longer need to be halted.
            HaltedForCursor = False
            CurrentBehavior = previousBehavior
            Paint()
            AddUpdateRecord("Changed out of mouseover state.")
        End If
    End Sub

    Friend Sub Move()
        Diagnostics.Debug.Assert(CurrentBehavior IsNot Nothing)

        If Not PlayingGame AndAlso Not ReturningToScreenArea Then
            destinationCoords = New Point(CurrentBehavior.OriginalDestinationXCoord,
                                          CurrentBehavior.OriginalDestinationYCoord)
        End If

        blocked = False

        If CursorImmunity > 0 Then CursorImmunity -= 1

        If ReturningToScreenArea AndAlso Options.PonyTeleportEnabled Then
            StopReturningToScreenArea()
            AddUpdateRecord("Stopped returning to screen area: Teleport option is enabled.")
            Exit Sub
        End If

        Dim speed As Double = ScaledSpeed()

        If EvilGlobals.CurrentGame Is Nothing OrElse
            (EvilGlobals.CurrentGame IsNot Nothing AndAlso
             EvilGlobals.CurrentGame.Status <> Game.GameStatus.Setup) Then
            ' User input will dictate our movement.
            If ManualControlPlayerOne Then
                speed = ManualControl(KeyboardState.IsKeyPressed(Keys.RControlKey),
                                      KeyboardState.IsKeyPressed(Keys.Up),
                                      KeyboardState.IsKeyPressed(Keys.Down),
                                      KeyboardState.IsKeyPressed(Keys.Left),
                                      KeyboardState.IsKeyPressed(Keys.Right),
                                      KeyboardState.IsKeyPressed(Keys.RShiftKey))
            ElseIf ManualControlPlayerTwo Then
                speed = ManualControl(KeyboardState.IsKeyPressed(Keys.LControlKey),
                                      KeyboardState.IsKeyPressed(Keys.W),
                                      KeyboardState.IsKeyPressed(Keys.S),
                                      KeyboardState.IsKeyPressed(Keys.A),
                                      KeyboardState.IsKeyPressed(Keys.D),
                                      KeyboardState.IsKeyPressed(Keys.LShiftKey))
            End If
        End If

        'If the behavior specified a follow object, or a point to go to, figure out where that is.
        Destination = DetermineDestination()

        Dim movement As Vector2F

        ' Don't follow a destination if we are under player control unless there is a game playing and it is in setup mode.
        Dim distance As Double
        If hasDestination AndAlso
            ((Not ManualControlPlayerOne AndAlso Not ManualControlPlayerTwo) OrElse
             (EvilGlobals.CurrentGame IsNot Nothing AndAlso EvilGlobals.CurrentGame.Status = Game.GameStatus.Setup)) Then
            ' A destination has been specified and the pony should head there.
            distance = Vector2.Distance(CenterLocation, Destination)
            ' Avoid division by zero.
            If distance = 0 Then distance = 1

            If GetDestinationDirectionHorizontal(Destination) = Direction.MiddleLeft Then
                facingRight = False
                movement.X = CSng(((CenterLocation.X - Destination.X) / (distance)) * -speed)
            Else
                facingRight = True
                movement.X = CSng(((Destination.X - CenterLocation.X) / (distance)) * speed)
            End If

            movement.Y = CSng(((CenterLocation.Y - Destination.Y) / (distance)) * -speed)

            ' We do not want to detect if we are at the destination if we are trying to move on-screen - we might stop at the destination
            ' and not get out of the area we want to avoid.
            ' However, we DO want to detect if we are exactly at our destination - our speed will go to 0 and we will be forever stuck
            ' there.
            If (distance <= 7) OrElse
                (ReturningToScreenArea AndAlso Vector2.Equals(CenterLocation(), Destination) AndAlso movement = Vector2F.Zero) Then
                movement = Vector2F.Zero

                AtDestination = True
                If ReturningToScreenArea Then
                    StopReturningToScreenArea()
                    AddUpdateRecord("Stopped returning to screen area; reached onscreen destination.")
                    Exit Sub
                End If

                If GoingHome Then
                    ' Don't disappear immediately when reaching a "house" - wait a bit.
                    If Not OpeningDoor Then
                        Delay = 90
                        OpeningDoor = True
                    Else
                        Delay -= 1
                    End If
                Else
                    'If this behavior links to another, we should end this one so we can move on to the next link.
                    If Not IsNothing(CurrentBehavior.LinkedBehavior) AndAlso speed <> 0 Then
                        BehaviorDesiredDuration = TimeSpan.Zero
                        Destination = Vector2.Zero
                        AddUpdateRecord("Reached destination, readying to switch to next behavior in sequence.")
                    End If
                End If

            Else
                'We're not yet at our destination

                'If we were marked as being at our destination in our last move,
                'if means the target moved slightly.  We should pause a bit before continuing to follow.
                If AtDestination Then
                    Delay = 60
                End If

                'Only continue if the delay has expired.
                If Delay > 0 Then
                    AtDestination = False
                    Delay -= 1
                    Paint()
                    AddUpdateRecord("Delay finished (destination). Terminating move function.")
                    Exit Sub
                End If

                AtDestination = False
            End If
        Else
            ' There is no destination, go wherever.

            If Delay > 0 Then
                Delay -= 1
                Paint()
                AddUpdateRecord("Delay finished (no destination). Terminating move function.")
                Exit Sub
            End If

            ' If moving diagonally.
            If Diagonal <> 0 Then
                'Opposite = Hypotenuse * cosine of the angle
                movement.X = CSng(Math.Sqrt((speed ^ 2) * 2) * Math.Cos(Diagonal))
                'Adjacent = Hypotenuse * cosine of the angle
                '(negative because we are using pixel coordinates - down is positive)
                movement.Y = CSng(-Math.Sqrt((speed ^ 2) * 2) * Math.Sin(Diagonal))
                facingRight = (movement.X >= 0)
            Else
                ' Not moving diagonally.
                movement.X = If(horizontalMovementAllowed, CSng(speed), 0)
                movement.Y = If(verticalMovementAllowed, CSng(speed), 0)

                If Not facingRight Then movement.X = -movement.X

                If (facingUp AndAlso movement.Y > 0) OrElse
                    (Not facingUp AndAlso movement.Y < 0) Then
                    movement.Y = -movement.Y
                End If
            End If
        End If

        Dim newLocation = Vector2.Round(New Vector2F(Location) + movement)
        Dim newTopLeftLocation = newLocation - New Vector2(GetImageCenterOffset())

        UpdateCurrentMouseoverBehavior()
        Dim isNearCursorNow = IsPonyNearMouseCursor(Location)
        Dim isNearCursorFuture = IsPonyNearMouseCursor(newLocation)

        Dim isOnscreenNow = IsPonyContainedInCanvas(Location)
        Dim isOnscreenFuture = IsPonyContainedInCanvas(newLocation)

        ' TODO: Refactor and extract.
        'Dim playingGameAndOutOfBounds = PlayingGame AndAlso
        '    EvilGlobals.CurrentGame.Status <> Game.GameStatus.Setup AndAlso
        '    Not IsPonyInBox(newTopLeftLocation, Game.Position.Allowed_Area)
        Dim playingGameAndOutOfBounds = False

        Dim isEnteringWindowNow = False
        If Options.WindowAvoidanceEnabled AndAlso Not ReturningToScreenArea Then
            isEnteringWindowNow = IsPonyEnteringWindow(TopLeftLocation, Vector2.Round(New Vector2F(TopLeftLocation) + movement), movement)
        End If

        Dim isInAvoidanceZoneNow = IsPonyIntersectingWithAvoidanceArea(Location)
        Dim isInAvoidanceZoneFuture = IsPonyIntersectingWithAvoidanceArea(newLocation)

        'if we ARE currently in the cursor's zone, then say that we should be halted (cursor_halt), save our current behavior so we 
        'can continue later, and set the current behavior to nothing so it will be changed.
        CursorOverPony = isNearCursorNow
        If isNearCursorNow Then
            If ReturningToScreenArea Then
                StopReturningToScreenArea() 'clear destination if moving_onscreen, otherwise we will get confused later.
                AddUpdateRecord("Stopped returning to screen area; pony is near the cursor now.")
            End If
            Paint() 'enable effects on mouseover.
            PonySpeak()
            AddUpdateRecord("Painted and terminated move; pony is near the cursor now.")
            Exit Sub
        ElseIf HaltedForCursor Then
            'if we're not in the cursor's way, but still flagged that we are, exit mouseover mode.
            CursorOverPony = False
            CursorImmunity = 30
            AddUpdateRecord("Exiting mouseover mode state.")
            Exit Sub
        End If

        ' if we are heading into the cursor, change directions
        If isNearCursorFuture Then
            CursorOverPony = False
            CursorImmunity = 60

            ' For normal movement, simply bounce to move away from the cursor.
            ' If we have a destination, then the cursor is blocking the way and the behavior should be aborted.
            ' TODO: Review abortion.
            If Not hasDestination Then
                Bounce(Me, TopLeftLocation, newTopLeftLocation, movement)
            Else
                CurrentBehavior = GetAppropriateBehaviorOrFallback(CurrentBehavior.AllowedMovement, False)
            End If
            AddUpdateRecord("Avoiding cursor.")
            Exit Sub
        End If

        ' Check whether movement leaves in us an allowable area.
        If ReturningToScreenArea OrElse (isOnscreenFuture AndAlso Not isInAvoidanceZoneFuture AndAlso Not playingGameAndOutOfBounds) Then
            ' We are in and will remain in an allowable area, or at least moving towards an allowable area.

            ' Check if we need to rebound off a window.
            If isEnteringWindowNow Then
                If Not hasDestination Then
                    Bounce(Me, TopLeftLocation, newTopLeftLocation, movement)
                    AddUpdateRecord("Avoiding window.")
                Else
                    AddUpdateRecord("Wanted to avoid window, but instead pressing to destination.")
                End If
                Exit Sub
            End If

            ' Everything's cool. Move and repaint.
            Location = newLocation
            lastMovement = movement

            Dim useVisualOverride = (followTarget IsNot Nothing AndAlso
                                     (CurrentBehavior.AutoSelectImagesOnFollow OrElse
                                      CurrentBehavior.FollowMovingBehavior IsNot Nothing OrElse
                                      CurrentBehavior.FollowStoppedBehavior IsNot Nothing)) OrElse
                              (EvilGlobals.CurrentGame IsNot Nothing AndAlso AtDestination)
            Paint(useVisualOverride)
            AddUpdateRecord("Standard paint. VisualOverride: ", useVisualOverride.ToString())

            ' If we can, we should try and start an interaction.
            If Options.PonyInteractionsEnabled AndAlso Not IsInteracting AndAlso Not ReturningToScreenArea Then
                Dim interact As Interaction = GetReadiedInteraction()
                If interact IsNot Nothing Then StartInteraction(interact)
            End If

            ' If we were trying to get out of a bad spot, and we find ourselves in a good area, continue on as normal...
            If ReturningToScreenArea AndAlso isOnscreenNow AndAlso Not isInAvoidanceZoneFuture AndAlso Not playingGameAndOutOfBounds Then
                StopReturningToScreenArea()
                AddUpdateRecord("Stopped returning to screen area; made it out of disallowed region.")
            Else
                'except if the user made changes to the avoidance area to include our current safe spot (we were already trying to avoid the area),
                'then get a new safe spot.
                If ReturningToScreenArea AndAlso
                    (IsPonyIntersectingWithAvoidanceArea(Destination) OrElse Not IsPonyContainedInCanvas(Destination)) Then
                    destinationCoords = FindSafeDestination()
                End If
            End If

            'if we were trying to get out of a bad area, but we are not moving, then continue on as normal.
            If ReturningToScreenArea AndAlso CurrentBehavior.Speed = 0 Then
                StopReturningToScreenArea()
                AddUpdateRecord("Stopped returning to screen area; current behavior has speed of zero.")
            End If

            'We are done.
            Exit Sub
        Else
            ' The anticipated move puts us in a disallowed area.
            ' Sanity check time - are we even on screen now?
            If isInAvoidanceZoneNow OrElse Not isOnscreenNow Then
                'we are no where! Find out where it is safe to be and run!
                If EvilGlobals.InPreviewMode OrElse Options.PonyTeleportEnabled Then
                    Teleport()
                    AddUpdateRecord("Teleporting back onscreen.")
                    Exit Sub
                End If

                Dim safespot = FindSafeDestination()
                ReturningToScreenArea = True

                If CurrentBehavior.Speed = 0 Then
                    CurrentBehavior = GetAppropriateBehaviorOrFallback(AllowedMoves.All, True)
                End If

                followTarget = Nothing
                followTargetName = ""
                destinationCoords = safespot

                Paint(False)
                AddUpdateRecord("Walking back on-screen.")
                Exit Sub
            End If
        End If

        ' Nothing to worry about, we are on screen, but our current behavior would take us off-screen in the next move. Just do something
        ' else.
        ' If we are moving to a destination, our path is blocked: we'll wait for a bit.
        ' If we are just moving normally, just "bounce" off of the barrier.
        If Not hasDestination Then
            Bounce(Me, TopLeftLocation, newTopLeftLocation, movement)
            'we need to paint to reset the image centers
            Paint()
            AddUpdateRecord("Bounced and painted - rebounded off screen edge.")
        Else
            If IsNothing(followTarget) Then
                speed = 0
            Else
                'do nothing but stare longingly in the direction of the object we want to follow...
                blocked = True
                Paint()
                AddUpdateRecord("Painted; but currently blocked from following target.")
            End If
        End If
    End Sub

    Private Sub UpdateCurrentMouseoverBehavior()
        Dim updateNeeded = currentMouseoverBehavior Is Nothing OrElse currentMouseoverBehavior.Group <> CurrentBehaviorGroup
        If Not updateNeeded Then Return
        Dim fallback = GetAppropriateBehaviorOrFallback(AllowedMoves.None, False)
        Dim preferred = Behaviors.FirstOrDefault(
            Function(b) b.AllowedMovement = AllowedMoves.MouseOver AndAlso b.Group = CurrentBehaviorGroup)
        currentMouseoverBehavior = If(preferred, fallback)
    End Sub

    Private Function DetermineDestination() As Point
        ' If we are off-screen and trying to get back on, just return the pre-calculated coordinates.
        If ReturningToScreenArea Then Return destinationCoords

        ' If being recalled to a house.
        If GoingHome Then Return Destination

        ' If we should be following something, but we don't know what yet, select a pony/effect to follow.
        If (followTargetName <> "" AndAlso IsNothing(followTarget)) Then
            ' If we are interacting, and the name of the pony we should be following matches that of the trigger, follow that one.
            ' Otherwise, we may end up following the wrong copy if there are more than one.
            If IsInteracting AndAlso followTargetName = CurrentInteraction.Trigger.Directory Then
                followTarget = CurrentInteraction.Trigger
                Return New Point(CurrentInteraction.Trigger.CenterLocation.X + destinationCoords.X,
                                 CurrentInteraction.Trigger.CenterLocation.Y + destinationCoords.Y)
            End If
            ' For the reverse case of a trigger pony trying to find out which initiator to follow when interacting.
            If IsInteracting AndAlso Not IsNothing(CurrentInteraction.Initiator) AndAlso
                followTargetName = CurrentInteraction.Initiator.Directory Then
                followTarget = CurrentInteraction.Initiator
                Return New Point(CurrentInteraction.Initiator.CenterLocation.X + destinationCoords.X,
                                 CurrentInteraction.Initiator.CenterLocation.Y + destinationCoords.Y)
            End If

            ' If not interacting, or following a different pony, we need to figure out which ones and follow one at random.
            Dim poniesToFollow As New List(Of Pony)
            For Each ponyToFollow In EvilGlobals.CurrentAnimator.Ponies()
                If ponyToFollow.Directory = followTargetName Then
                    poniesToFollow.Add(ponyToFollow)
                End If
            Next
            If poniesToFollow.Count <> 0 Then
                Dim ponyToFollow = poniesToFollow(Rng.Next(poniesToFollow.Count))
                followTarget = ponyToFollow
                Return New Point(ponyToFollow.CenterLocation.X + destinationCoords.X,
                                 ponyToFollow.CenterLocation.Y + destinationCoords.Y)
            End If

            ' We can't find the object to follow, so specify no destination.
            Return Point.Empty
        End If

        If followTarget IsNot Nothing Then
            ' We've already selected an object to follow previously.
            Dim followPony = TryCast(followTarget, Pony)
            If followPony IsNot Nothing Then
                If leadTarget Then
                    Return followPony.FutureLocation()
                Else
                    Return New Point(CInt(followPony.CenterLocation.X + (followPony.Scale * destinationCoords.X)), _
                                     CInt(followPony.CenterLocation.Y + (followPony.Scale * destinationCoords.Y)))
                End If
            Else
                Dim followEffect As Effect = DirectCast(followTarget, Effect)
                Return New Point(followEffect.Center.X + destinationCoords.X, followEffect.Center.Y + destinationCoords.Y)
            End If
        End If

        ' We are not following an object, but going to a point on the screen.
        If destinationCoords.X <> 0 AndAlso destinationCoords.Y <> 0 Then
            Dim area = Options.GetCombinedScreenArea()
            Return New Point(CInt(0.01 * destinationCoords.X * area.Width),
                             CInt(0.01 * destinationCoords.Y * area.Height))
        End If

        ' We have no given destination.
        Return Point.Empty
    End Function

    Private Sub StopReturningToScreenArea()
        ReturningToScreenArea = False
        destinationCoords = New Point(CurrentBehavior.OriginalDestinationXCoord, CurrentBehavior.OriginalDestinationYCoord)
        followTargetName = CurrentBehavior.OriginalFollowTargetName
        Paint()
    End Sub

    Friend Sub ActivateEffects(currentTime As TimeSpan)

        If Options.PonyEffectsEnabled AndAlso
            Not _sleeping AndAlso
            Not BeingDragged AndAlso
            Not ReturningToScreenArea Then
            For Each effect In CurrentBehavior.Effects
                If Not effectsLastUsed.ContainsKey(effect) Then
                    effectsLastUsed(effect) = TimeSpan.Zero
                End If
                If (currentTime - effectsLastUsed(effect)).TotalMilliseconds >= effect.RepeatDelay * 1000 Then

                    If effect.RepeatDelay = 0 Then
                        If effectsAlreadyPlayedForBehavior.Contains(effect) Then Continue For
                    End If

                    effectsAlreadyPlayedForBehavior.Add(effect)

                    Dim newEffect = New Effect(effect, Not facingRight,
                                               Function() New Vector2F(Me.TopLeftLocation),
                                               Function() Me.CurrentImageSize,
                                               Function() CSng(Me.Scale))

                    If newEffect.Base.Duration <> 0 Then
                        newEffect.DesiredDuration = TimeSpan.FromSeconds(newEffect.Base.Duration)
                    Else
                        If Me.HaltedForCursor Then
                            newEffect.DesiredDuration = TimeSpan.FromSeconds(CurrentBehavior.MaxDuration)
                        Else
                            newEffect.DesiredDuration = BehaviorDesiredDuration - Me.ImageTimeIndex
                        End If
                    End If

                    EvilGlobals.CurrentAnimator.AddEffect(newEffect)
                    AddHandler newEffect.Expired, Sub() _activeEffects.Remove(newEffect)
                    _activeEffects.Add(newEffect)

                    effectsLastUsed(effect) = currentTime

                End If
            Next
        End If

    End Sub

    'reverse directions as if we were bouncing off a boundary.
    Private Sub Bounce(pony As Pony, topLeftLocation As Point, newTopLeftLocation As Point, movement As SizeF)
        If movement = SizeF.Empty Then Exit Sub

        'if we are moving in a simple direction (up/down, left/right) just reverse direction
        If movement.Width = 0 AndAlso movement.Height <> 0 Then
            facingUp = Not facingUp
            If Diagonal <> 0 Then Diagonal = 2 * Math.PI - Diagonal
            Exit Sub
        End If
        If movement.Width <> 0 AndAlso movement.Height = 0 Then
            facingRight = Not facingRight
            If Diagonal <> 0 Then Diagonal = Math.PI - Diagonal
            Exit Sub
        End If

        'if we were moving in a composite direction, we need to determine which component is bad

        Dim x_bad = False
        Dim y_bad = False

        Dim newTopLeftLocationXOnly As New Point(newTopLeftLocation.X, topLeftLocation.Y)
        Dim newTopLeftLocationYOnly As New Point(topLeftLocation.X, newTopLeftLocation.Y)
        Dim centerOffset = GetImageCenterOffset()
        Dim newLocationXOnly = newTopLeftLocationXOnly + centerOffset
        Dim newLocationYOnly = newTopLeftLocationYOnly + centerOffset

        If movement.Width <> 0 AndAlso movement.Height <> 0 Then
            If Not pony.IsPonyContainedInCanvas(newLocationXOnly) OrElse
                pony.IsPonyIntersectingWithAvoidanceArea(newLocationXOnly) OrElse
                pony.IsPonyEnteringWindow(topLeftLocation, newTopLeftLocationXOnly, New SizeF(movement.Width, 0)) Then
                x_bad = True
            End If
            If Not pony.IsPonyContainedInCanvas(newLocationYOnly) OrElse
                pony.IsPonyIntersectingWithAvoidanceArea(newLocationYOnly) OrElse
                pony.IsPonyEnteringWindow(topLeftLocation, newTopLeftLocationYOnly, New SizeF(0, movement.Height)) Then
                y_bad = True
            End If
        End If

        If Not x_bad AndAlso Not y_bad Then
            facingUp = Not facingUp
            facingRight = Not facingRight
            If Diagonal <> 0 Then
                Diagonal = Math.PI - Diagonal
                Diagonal = 2 * Math.PI - Diagonal
            End If
            Exit Sub
        End If

        If x_bad AndAlso y_bad Then
            facingUp = Not facingUp
            facingRight = Not facingRight
            If Diagonal <> 0 Then
                Diagonal = Math.PI - Diagonal
                Diagonal = 2 * Math.PI - Diagonal
            End If
            Exit Sub
        End If

        If x_bad Then
            facingRight = Not facingRight
            If Diagonal <> 0 Then
                Diagonal = Math.PI - Diagonal
            End If
            Exit Sub
        End If
        If y_bad Then
            facingUp = Not facingUp
            If Diagonal <> 0 Then
                Diagonal = 2 * Math.PI - Diagonal
            End If
        End If

    End Sub

    Friend Function GetBehaviorGroupName(groupnumber As Integer) As String

        If groupnumber = 0 Then
            Return "Any"
        End If

        For Each group In Base.BehaviorGroups
            If group.Number = groupnumber Then
                Return group.Name
            End If
        Next

        Return "Unnamed"

    End Function

    'Return our future location in one second if we go straight in the current direction
    Friend Function FutureLocation(Optional ticks As Integer = 1000) As Point
        Dim Number_Of_Interations = ticks / (1000.0F / EvilGlobals.CurrentAnimator.MaximumFramesPerSecond)  'get the # of intervals in one second
        Return Point.Round(CType(TopLeftLocation, Vector2) + lastMovement * Number_Of_Interations)
    End Function

    Private Sub Paint(Optional useOverrideBehavior As Boolean = True)
        visualOverrideBehavior = Nothing

        'If we are going to a particular point or following something, we need to pick the 
        'appropriate graphics to how we are moving instead of using what the behavior specifies.
        If hasDestination AndAlso Not ManualControlPlayerOne AndAlso Not ManualControlPlayerTwo Then ' AndAlso Not Playing_Game Then

            Dim horizontalDistance = Math.Abs(Destination.X - CenterLocation.X)
            Dim verticalDistance = Math.Abs(Destination.Y - CenterLocation.Y)

            'We are supposed to be following, so say we can move any direction to do that.
            Dim allowedMovement = AllowedMoves.All

            'if the distance to the destination is mostly horizontal, or mostly vertical, set the movement to either of those
            'This allows Pegasi to fly up to reach their target instead of walking straight up.
            'This is weighted more on the vertical side for better effect
            If horizontalDistance * 0.75 > verticalDistance Then
                allowedMovement = allowedMovement And AllowedMoves.HorizontalOnly
            Else
                allowedMovement = allowedMovement And AllowedMoves.VerticalOnly
            End If

            If AtDestination OrElse blocked OrElse CurrentBehavior.Speed = 0 OrElse Delay > 0 Then
                allowedMovement = AllowedMoves.None
                Dim paint_stop_now = paintStop
                paintStop = True

                'If at our destination, we want to allow one final animation change.  
                'However after that, we want to stop painting as we may be stuck in a left-right loop
                'Detect here if the destination is between the right and left image centers, which would cause flickering between the two.
                If paint_stop_now Then

                    If Destination.X >= CurrentBehavior.LeftImage.Center.X + TopLeftLocation.X AndAlso
                        Destination.X < CurrentBehavior.RightImage.Center.X + TopLeftLocation.X Then
                        '  Console.WriteLine(Me.Name & " paint stopped")
                        Exit Sub
                    End If

                End If

            Else
                paintStop = False
            End If

            If useOverrideBehavior Then
                ' Chosen an appropriate behavior to use for visual override.
                ' Use the specified behaviors for following if possible, otherwise find a suitable one given our movement requirements.
                Dim appropriateBehavior As Behavior = Nothing
                If allowedMovement = AllowedMoves.None Then
                    appropriateBehavior = CurrentBehavior.FollowStoppedBehavior
                Else
                    appropriateBehavior = CurrentBehavior.FollowMovingBehavior
                End If
                If CurrentBehavior.AutoSelectImagesOnFollow OrElse appropriateBehavior Is Nothing Then
                    appropriateBehavior = GetAppropriateBehaviorOrFallback(allowedMovement, True)
                End If
                If ValidateBehavior(appropriateBehavior) Then visualOverrideBehavior = appropriateBehavior
            End If
        Else
            paintStop = False
        End If

        Dim newCenter = Size.Round(If(facingRight, CurrentBehavior.RightImage.Center, CurrentBehavior.LeftImage.Center) * CSng(Scale))

        If Not isCustomImageCenterDefined Then
            currentCustomImageCenter = newCenter
        End If

        'reposition the form based on the new image center, if different:
        If isCustomImageCenterDefined AndAlso currentCustomImageCenter <> newCenter Then
            Location = New Point(Location.X - newCenter.Width + currentCustomImageCenter.Width,
                                 Location.Y - newCenter.Height + currentCustomImageCenter.Height)
            currentCustomImageCenter = newCenter
        End If

    End Sub

    'You can place effects at an offset to the pony, and also set them to the left or the right of themselves for big effects.
    Friend Shared Function GetEffectLocation(effectImageSize As Size, dir As Direction,
                                             parentTopLeftLocation As Vector2F, parentSize As Vector2,
                                             centering As Direction, scale As Single) As Vector2

        Dim scaledParentSize = parentSize * CSng(scale)
        scaledParentSize.X *= DirectionWeightHorizontal(dir)
        scaledParentSize.Y *= DirectionWeightVertical(dir)

        Dim locationOnParent = parentTopLeftLocation + scaledParentSize

        Dim scaledEffectSize = New Vector2F(effectImageSize) * Options.ScaleFactor
        scaledEffectSize.X *= DirectionWeightHorizontal(centering)
        scaledEffectSize.Y *= DirectionWeightVertical(centering)

        Return Vector2.Round(locationOnParent - scaledEffectSize)
    End Function

    Private Shared Function DirectionWeightHorizontal(dir As Direction) As Single
        Select Case dir
            Case Direction.TopLeft, Direction.MiddleLeft, Direction.BottomLeft
                Return 0
            Case Direction.TopCenter, Direction.MiddleCenter, Direction.BottomCenter
                Return 0.5
            Case Direction.TopRight, Direction.MiddleRight, Direction.BottomRight
                Return 1
            Case Direction.Random, Direction.RandomNotCenter
                Return CSng(Rng.NextDouble())
        End Select
        Return Single.NaN
    End Function

    Private Shared Function DirectionWeightVertical(dir As Direction) As Single
        Select Case dir
            Case Direction.TopLeft, Direction.TopCenter, Direction.TopRight
                Return 0
            Case Direction.MiddleLeft, Direction.MiddleCenter, Direction.MiddleRight
                Return 0.5
            Case Direction.BottomLeft, Direction.BottomCenter, Direction.BottomRight
                Return 1
            Case Direction.Random, Direction.RandomNotCenter
                Return CSng(Rng.NextDouble())
        End Select
        Return Single.NaN
    End Function

    Private Function GetAppropriateBehavior(movement As AllowedMoves, speed As Boolean,
                                           Optional suggestedBehavior As Behavior = Nothing,
                                           Optional currentGroupOnly As Boolean = True) As Behavior
        'does the current behavior work?
        If CurrentBehavior IsNot Nothing Then
            If movement = AllowedMoves.All OrElse (CurrentBehavior.AllowedMovement And movement) = movement Then
                If CurrentBehavior.Speed = 0 AndAlso movement = AllowedMoves.None Then Return CurrentBehavior
                If CurrentBehavior.Speed <> 0 AndAlso movement = AllowedMoves.All Then Return CurrentBehavior
            End If
        End If

        For Each behavior In Behaviors
            If currentGroupOnly AndAlso behavior.Group <> CurrentBehaviorGroup Then Continue For

            If behavior.AllowedMovement = AllowedMoves.Sleep AndAlso
                movement <> AllowedMoves.Sleep AndAlso
                movement <> AllowedMoves.Dragged Then
                Continue For
            End If

            'skip behaviors that are parts of a chain and shouldn't be used individually
            'however, when being dragged or sleeping, we may still need to consider these.
            If behavior.Skip AndAlso
                movement <> AllowedMoves.Dragged AndAlso
                movement <> AllowedMoves.Sleep Then
                Continue For
            End If

            If movement = AllowedMoves.All OrElse (behavior.AllowedMovement And movement) = movement Then

                If behavior.Speed = 0 AndAlso movement <> AllowedMoves.All Then Return behavior

                'see if the specified behavior works.  If not, we'll find another.
                If suggestedBehavior IsNot Nothing Then
                    If movement = AllowedMoves.All OrElse (suggestedBehavior.AllowedMovement And movement) = movement Then
                        If hasDestination Then
                            facingRight = (GetDestinationDirectionHorizontal(Destination) = Direction.MiddleRight)
                        End If
                        Return suggestedBehavior
                    End If
                End If

                'if this behavior has a destination or an object to follow, don't use it.
                If (destinationCoords.X <> 0 OrElse destinationCoords.Y <> 0 OrElse followTargetName <> "") AndAlso
                    Not PlayingGame AndAlso
                    Not ReturningToScreenArea Then
                    Continue For
                End If

                'If the user is pressing shift while "taking control"
                If speed Then
                    If Math.Abs(behavior.Speed) > 0 Then Return behavior
                Else
                    If behavior.Speed <> 0 Then Return behavior
                End If
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Returns a behavior that best matches the desired allowable movement and speed. If no such behavior exists, a suitable fallback is 
    ''' returned (typically the current behavior).
    ''' </summary>
    ''' <param name="movement">The movement to match (as best as possible).</param>
    ''' <param name="speed">Is the user pressing the "speed" override key.</param>
    ''' <param name="suggestedBehavior">A suggested behavior to test first. This will be returned if it meets the requirements
    ''' sufficiently.</param>
    ''' <returns>The suggested behavior, if it meets the requirements, otherwise any behavior with meets the requirements sufficiently. If 
    ''' no behavior matches sufficiently a fallback is returned, typically the current behavior.
    ''' </returns>
    Friend Function GetAppropriateBehaviorOrFallback(movement As AllowedMoves, speed As Boolean,
                                           Optional suggestedBehavior As Behavior = Nothing) As Behavior
        Dim behavior = GetAppropriateBehavior(movement, speed, suggestedBehavior)
        If behavior Is Nothing Then behavior = CurrentBehavior
        If behavior Is Nothing Then behavior = GetAppropriateBehavior(movement, speed, suggestedBehavior, False)
        If behavior Is Nothing Then behavior = Behaviors(Rng.Next(Behaviors.Count))
        Return behavior
    End Function

    'Test to see if we overlap with another application's window.
    <Security.Permissions.PermissionSet(Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
    Private Function IsPonyEnteringWindow(topLeftLocation As Point, newTopLeftLocation As Point, movement As SizeF) As Boolean
        If Not OperatingSystemInfo.IsWindows Then Return False

        Try
            If EvilGlobals.InPreviewMode Then Return False
            If Not Options.WindowAvoidanceEnabled Then Return False

            If movement = SizeF.Empty Then Return False

            Dim current_window_1 = Win32.WindowFromPoint(New Win32.POINT(topLeftLocation.X, topLeftLocation.Y))
            Dim current_window_2 = Win32.WindowFromPoint(New Win32.POINT(CInt(topLeftLocation.X + (Scale * CurrentImageSize.X)), CInt(topLeftLocation.Y + (Scale * CurrentImageSize.Y))))
            Dim current_window_3 = Win32.WindowFromPoint(New Win32.POINT(CInt(topLeftLocation.X + (Scale * CurrentImageSize.X)), topLeftLocation.Y))
            Dim current_window_4 = Win32.WindowFromPoint(New Win32.POINT(topLeftLocation.X, CInt(topLeftLocation.Y + (Scale * CurrentImageSize.Y))))

            'the current position is already half-way between windows.  don't worry about it
            If current_window_1 <> current_window_2 OrElse current_window_1 <> current_window_3 OrElse current_window_1 <> current_window_4 Then
                Return False
            End If

            'find out where we are going
            Dim new_window_1 As IntPtr = IntPtr.Zero 'top_left
            Dim new_window_2 As IntPtr = IntPtr.Zero  'bottom_right
            Dim new_window_3 As IntPtr = IntPtr.Zero  'top_right
            Dim new_window_4 As IntPtr = IntPtr.Zero  'bottom_left

            Select Case movement.Width
                Case Is > 0
                    new_window_2 = Win32.WindowFromPoint(New Win32.POINT(CInt(newTopLeftLocation.X + (Scale * CurrentImageSize.X)), CInt(newTopLeftLocation.Y + (Scale * CurrentImageSize.Y))))
                    new_window_3 = Win32.WindowFromPoint(New Win32.POINT(CInt(newTopLeftLocation.X + (Scale * CurrentImageSize.X)), newTopLeftLocation.Y))
                Case Is < 0
                    new_window_1 = Win32.WindowFromPoint(New Win32.POINT(newTopLeftLocation.X, newTopLeftLocation.Y))
                    new_window_4 = Win32.WindowFromPoint(New Win32.POINT(newTopLeftLocation.X, CInt(newTopLeftLocation.Y + (Scale * CurrentImageSize.Y))))
            End Select

            Select Case movement.Height
                Case Is > 0
                    If (new_window_2) = IntPtr.Zero Then new_window_2 = Win32.WindowFromPoint(New Win32.POINT(CInt(newTopLeftLocation.X + (Scale * CurrentImageSize.X)), CInt(newTopLeftLocation.Y + (Scale * CurrentImageSize.Y))))
                    If (new_window_4) = IntPtr.Zero Then new_window_4 = Win32.WindowFromPoint(New Win32.POINT(newTopLeftLocation.X, CInt(newTopLeftLocation.Y + (Scale * CurrentImageSize.Y))))
                Case Is < 0
                    If (new_window_1) = IntPtr.Zero Then new_window_1 = Win32.WindowFromPoint(New Win32.POINT(newTopLeftLocation.X, newTopLeftLocation.Y))
                    If (new_window_3) = IntPtr.Zero Then new_window_3 = Win32.WindowFromPoint(New Win32.POINT(CInt(newTopLeftLocation.X + (Scale * CurrentImageSize.X)), newTopLeftLocation.Y))
            End Select


            Dim collisionWindows As New List(Of IntPtr)

            If (new_window_1 <> IntPtr.Zero AndAlso new_window_1 <> current_window_1) Then collisionWindows.Add(new_window_1)
            If (new_window_2 <> IntPtr.Zero AndAlso new_window_2 <> current_window_2) Then collisionWindows.Add(new_window_2)
            If (new_window_3 <> IntPtr.Zero AndAlso new_window_3 <> current_window_3) Then collisionWindows.Add(new_window_3)
            If (new_window_4 <> IntPtr.Zero AndAlso new_window_4 <> current_window_4) Then collisionWindows.Add(new_window_4)

            If collisionWindows.Count <> 0 Then

                Dim pony_collision_count = 0
                Dim ignored_collision_count = 0

                'need our own PID for window avoidance (ignoring collisions with other ponies)
                Dim currentProcessId As IntPtr
                If Options.PonyAvoidsPonies Then
                    Using currentProcess = Diagnostics.Process.GetCurrentProcess()
                        currentProcessId = currentProcess.Handle
                    End Using
                End If

                For Each collisionWindow In collisionWindows

                    If Options.PonyAvoidsPonies AndAlso Options.PonyStaysInBox Then
                        Exit For
                    End If

                    'ignore collisions with other ponies or effects
                    If Options.PonyAvoidsPonies Then
                        Dim collisisonWindowProcessId As IntPtr
                        Win32.GetWindowThreadProcessId(collisionWindow, collisisonWindowProcessId)
                        If collisisonWindowProcessId = currentProcessId Then
                            pony_collision_count += 1
                        End If
                    Else

                        'we are colliding with another window boundary.
                        'are we already inside of it, and therefore should go through to the outside?
                        'or are we on the outside, and need to stay out?

                        If Options.PonyStaysInBox Then Continue For

                        Dim collisionArea As New Win32.RECT
                        Win32.GetWindowRect(collisionWindow, collisionArea)
                        If IsPonyIntersectingWithRect(topLeftLocation + GetImageCenterOffset(), Rectangle.FromLTRB(
                                       collisionArea.Left, collisionArea.Top, collisionArea.Right, collisionArea.Bottom)) Then
                            ignored_collision_count += 1
                        End If
                    End If
                Next

                If pony_collision_count + ignored_collision_count = collisionWindows.Count Then
                    Return False
                End If

                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Options.WindowAvoidanceEnabled = False
            My.Application.NotifyUserOfNonFatalException(ex, "Error attempting to avoid windows. Window avoidance has been disabled.")
            Return False
        End Try

    End Function

    Friend Function IsPonyContainedInCanvas(centerLocation As Point) As Boolean
        Return IsPonyContainedInRect(centerLocation, Options.GetCombinedScreenArea())
    End Function

    Friend Function IsPonyContainedInScreen(centerLocation As Point, screen As Screen) As Boolean
        Return IsPonyContainedInRect(centerLocation, screen.WorkingArea)
    End Function

    Friend Function IsPonyContainedInRect(centerLocation As Point, rect As Rectangle) As Boolean
        Dim sz = New Size(CInt(CurrentImageSize.X * Scale), CInt(CurrentImageSize.Y * Scale))
        Return rect.Contains(New Rectangle(New Vector2(centerLocation) - currentImage.Center, sz))
    End Function

    Friend Function IsPonyIntersectingWithRect(centerLocation As Point, rect As Rectangle) As Boolean
        Dim sz = New Size(CInt(CurrentImageSize.X * Scale), CInt(CurrentImageSize.Y * Scale))
        Return rect.IntersectsWith(New Rectangle(New Vector2(centerLocation) - currentImage.Center, sz))
    End Function

    Private Function IsPonyIntersectingWithAvoidanceArea(centerLocation As Point) As Boolean
        If CurrentBehavior Is Nothing Then Return False

        If EvilGlobals.InPreviewMode Then
            Dim previewArea = EvilGlobals.PreviewWindowRectangle
            If CurrentImageSize.X > previewArea.Width OrElse CurrentImageSize.Y > previewArea.Height Then
                Return False
            End If
            Return Not IsPonyContainedInRect(centerLocation, previewArea)
        End If

        If Options.ExclusionZone.IsEmpty Then
            Return False
        End If

        Return IsPonyIntersectingWithRect(centerLocation, Options.ExclusionZoneForBounds(Options.GetCombinedScreenArea()))
    End Function

    Private Function IsPonyNearMouseCursor(centerLocation As Point) As Boolean
        If Not Options.CursorAvoidanceEnabled Then Return False
        If EvilGlobals.InScreensaverMode Then Return False
        If CursorImmunity > 0 Then Return False
        If IsInteracting Then Return False
        If ManualControlPlayerOne OrElse ManualControlPlayerTwo Then Return False

        Return Vector2.Distance(centerLocation, EvilGlobals.CursorLocation) <= Options.CursorAvoidanceSize
    End Function

    ''' <summary>
    ''' Returns a random location that is within the allowable regions to be in.
    ''' </summary>
    ''' <returns>The center of the preview area in preview mode, otherwise a random location within the allowable region, if one can be
    ''' found; otherwise Point.Empty.</returns>
    Private Function FindSafeDestination() As Point
        If EvilGlobals.InPreviewMode Then Return Point.Round(EvilGlobals.PreviewWindowRectangle.Center())

        For i = 0 To 300
            Dim randomScreen = Options.Screens(Rng.Next(Options.Screens.Count))
            Dim teleportLocation = New Point(
                CInt(randomScreen.WorkingArea.X + Math.Round(Rng.NextDouble() * randomScreen.WorkingArea.Width)),
                CInt(randomScreen.WorkingArea.Y + Math.Round(Rng.NextDouble() * randomScreen.WorkingArea.Height)))
            If Not IsPonyIntersectingWithAvoidanceArea(teleportLocation) Then Return teleportLocation
        Next

        Return Point.Empty
    End Function

    Friend Function GetDestinationDirectionHorizontal(destination As Vector2F) As Direction
        Dim rightImageCenterX = TopLeftLocation.X + (Scale * CurrentBehavior.RightImage.Center.X)
        Dim leftImageCenterX = TopLeftLocation.X + (Scale * CurrentBehavior.LeftImage.Center.X)
        If (rightImageCenterX > destination.X AndAlso leftImageCenterX < destination.X) OrElse
            destination.X - CenterLocation.X <= 0 Then
            Return Direction.MiddleLeft
        Else
            Return Direction.MiddleRight
        End If
    End Function

    Friend Function GetDestinationDirectionVertical(destination As Vector2F) As Direction
        Dim rightImageCenterY = TopLeftLocation.Y + (Scale * CurrentBehavior.RightImage.Center.Y)
        Dim leftImageCenterY = TopLeftLocation.Y + (Scale * CurrentBehavior.LeftImage.Center.Y)
        If (rightImageCenterY > destination.Y AndAlso leftImageCenterY < destination.Y) OrElse
           (rightImageCenterY < destination.Y AndAlso leftImageCenterY > destination.Y) OrElse
           destination.Y - CenterLocation.Y <= 0 Then
            Return Direction.TopCenter
        Else
            Return Direction.BottomCenter
        End If
    End Function

    Private ReadOnly Property currentImage As CenterableSpriteImage
        Get
            Dim behavior = If(visualOverrideBehavior, CurrentBehavior)
            If behavior Is Nothing Then Return Nothing
            Return If(facingRight, behavior.RightImage, behavior.LeftImage)
        End Get
    End Property

    Private ReadOnly Property CurrentImageSize As Vector2
        Get
            Return If(currentImage Is Nothing, Vector2.Zero, currentImage.Size)
        End Get
    End Property

    Friend Function CenterLocation() As Point
        Return TopLeftLocation + GetImageCenterOffset()
    End Function

    ''' <summary>
    ''' Using the collection of interaction bases available to the base of this pony, generates interactions that can be used depending on
    ''' the other available sprites that can be interacted with. These interactions will be triggered during update cycles.
    ''' </summary>
    ''' <param name="otherPonies">The other ponies available to interact with.</param>
    Public Sub InitializeInteractions(otherPonies As IEnumerable(Of Pony))
        Argument.EnsureNotNull(otherPonies, "otherPonies")

        If Directory Is Nothing Then Return

        interactions.Clear()
        For Each interactionBase In Base.Interactions
            Dim interaction = New Interaction(interactionBase)

            ' Get all actual instances of target ponies.
            interaction.Targets.Clear()
            Dim missingTargetNames = New HashSet(Of String)(interactionBase.TargetNames)
            For Each candidatePony In otherPonies
                If Not ReferenceEquals(Me, candidatePony) AndAlso interactionBase.TargetNames.Contains(candidatePony.Base.Directory) Then
                    missingTargetNames.Remove(candidatePony.Base.Directory)
                    interaction.Targets.Add(candidatePony)
                End If
            Next
            ' If no instances of the target ponies are present, we can forget this interaction.
            ' Alternatively, if it is specified all targets must be present but some are missing, the interaction cannot be used.
            If interaction.Targets.Count = 0 OrElse
                (interaction.Base.Activation = TargetActivation.All AndAlso
                missingTargetNames.Count > 0) Then
                Continue For
            End If

            ' Determine the common set of behaviors by name actually implemented by this pony and all discovered targets.
            Dim commonBehaviors As New HashSet(Of CaseInsensitiveString)(interactionBase.BehaviorNames)
            For Each pony In {Me}.Concat(interaction.Targets)
                commonBehaviors.IntersectWith(pony.Base.Behaviors.Select(Function(b) b.Name))
                If commonBehaviors.Count = 0 Then Exit For
            Next
            ' We need at least one common behavior to exist on all targets, or we cannot run the interaction.
            If commonBehaviors.Count = 0 Then Continue For
            ' Keep a copy of behaviors for the current pony for use when the interaction is activated later.
            interaction.Behaviors.AddRange(Base.Behaviors.Where(Function(b) commonBehaviors.Contains(b.Name)))

            ' We can list this as a possible interaction.
            interactions.Add(interaction)
        Next
    End Sub

    Private Sub StartInteraction(interaction As Interaction)
        isInteractionInitiator = True
        IsInteracting = True
        CurrentInteraction = interaction
        SelectBehavior(interaction.Behaviors(Rng.Next(interaction.Behaviors.Count)))

        interaction.Initiator = Me

        'do we interact with ALL targets, including copies, or just the pony that we ran into?
        If interaction.Base.Activation <> TargetActivation.One Then
            For Each targetPony In interaction.Targets
                If Not targetPony.IsInteracting Then
                    targetPony.StartInteractionAsTarget(CurrentBehavior.Name, interaction)
                End If
            Next
        Else
            interaction.Trigger.StartInteractionAsTarget(CurrentBehavior.Name, interaction)
        End If
    End Sub

    Private Sub StartInteractionAsTarget(behaviorName As CaseInsensitiveString, interaction As Interaction)
        Dim behavior = Behaviors.FirstOrDefault(Function(b) b.Name = behaviorName)
        If behavior Is Nothing Then
            Diagnostics.Debug.Assert(behavior IsNot Nothing, "Could not find interaction behavior.")
            Return
        End If
        isInteractionInitiator = False
        IsInteracting = True
        CurrentInteraction = interaction
        SelectBehavior(behavior)
    End Sub

    Private Function GetReadiedInteraction() As Interaction
        'If we recently ran an interaction, don't start a new one until the delay expires.
        If internalTime < interactionDelayUntil Then
            Return Nothing
        End If

        For Each interaction In interactions
            For Each target As Pony In interaction.Targets
                ' Don't attempt to interact with a busy target.
                If target.IsInteracting Then
                    If interaction.Base.Activation = TargetActivation.All Then
                        ' Need all targets but one is busy? Can't use this interaction then.
                        Exit For
                    Else
                        ' Try a different target.
                        Continue For
                    End If
                End If

                ' Get distance between the pony and the possible target.
                Dim distance = Vector2.Distance(TopLeftLocation + New Size(CInt(CurrentImageSize.X / 2),
                                                                           CInt(CurrentImageSize.Y / 2)),
                                               target.TopLeftLocation + New Size(CInt(target.CurrentImageSize.X / 2),
                                                                                 CInt(target.CurrentImageSize.Y / 2)))

                ' Check target is in range, and perform a random check against the chance the interaction can occur.
                If distance <= interaction.Base.Proximity AndAlso Rng.NextDouble() <= interaction.Base.Chance Then
                    interaction.Trigger = target
                    Return interaction
                End If
            Next
        Next

        ' No interactions ready to start at this time.
        Return Nothing
    End Function

    Public Function GetImageCenterOffset() As Size
        If isCustomImageCenterDefined Then
            Return currentCustomImageCenter
        ElseIf CurrentBehavior IsNot Nothing Then
            Return New Size(CInt(CurrentImageSize.X * Scale / 2.0), CInt(CurrentImageSize.Y * Scale / 2.0))
        Else
            Return Size.Empty
        End If
    End Function

    Public ReadOnly Property SpeechText As String Implements ISpeakingSprite.SpeechText
        Get
            Return lastSpeakLine
        End Get
    End Property

    Public ReadOnly Property SoundPath As String Implements ISoundfulSprite.SoundPath
        Get
            Return lastSpeakSound
        End Get
    End Property

    Public ReadOnly Property ImageTimeIndex As TimeSpan Implements ISprite.ImageTimeIndex
        Get
            Return internalTime - BehaviorStartTime
        End Get
    End Property

    Public ReadOnly Property FlipImage As Boolean Implements ISprite.FlipImage
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property ImagePath As String Implements ISprite.ImagePath
        Get
            Dim behavior = If(visualOverrideBehavior, CurrentBehavior)
            If behavior Is Nothing Then Return Nothing
            Dim path = If(facingRight, behavior.RightImage.Path, behavior.LeftImage.Path)
            Return path
        End Get
    End Property

    Public ReadOnly Property Region As System.Drawing.Rectangle Implements ISprite.Region
        Get

            Return New Rectangle(TopLeftLocation, Vector2.Truncate(CurrentImageSize * Options.ScaleFactor))
        End Get
    End Property

    Public Sub Expire() Implements IExpireableSprite.Expire
        If _expired Then Return
        _expired = True
        For Each effect In _activeEffects.ToImmutableArray()
            effect.Expire()
        Next
        RaiseEvent Expired(Me, EventArgs.Empty)
    End Sub

    Public Event Expired As EventHandler Implements IExpireableSprite.Expired

    Private Function ManualControl(ponyAction As Boolean,
                              ponyUp As Boolean, ponyDown As Boolean, ponyLeft As Boolean, ponyRight As Boolean,
                              ponySpeed As Boolean) As Double
        Diagonal = 0
        _manualControlAction = ponyAction
        If Not PlayingGame AndAlso ponyAction Then
            CursorOverPony = True
            Paint() 'enable effects on mouseover.
            Return ScaledSpeed()
        Else
            'if we're not in the cursor's way, but still flagged that we are, exit mouseover mode.
            If HaltedForCursor Then
                CursorOverPony = False
                Return ScaledSpeed()
            End If
        End If

        Dim appropriateMovement = AllowedMoves.None
        verticalMovementAllowed = False
        horizontalMovementAllowed = False
        If ponyUp AndAlso Not ponyDown Then
            facingUp = True
            verticalMovementAllowed = True
            appropriateMovement = appropriateMovement Or AllowedMoves.VerticalOnly
        End If
        If ponyDown AndAlso Not ponyUp Then
            facingUp = False
            verticalMovementAllowed = True
            appropriateMovement = appropriateMovement Or AllowedMoves.VerticalOnly
        End If
        If ponyRight AndAlso Not ponyLeft Then
            facingRight = True
            horizontalMovementAllowed = True
            appropriateMovement = appropriateMovement Or AllowedMoves.HorizontalOnly
        End If
        If ponyLeft AndAlso Not ponyRight Then
            facingRight = False
            horizontalMovementAllowed = True
            appropriateMovement = appropriateMovement Or AllowedMoves.HorizontalOnly
        End If
        If appropriateMovement = (AllowedMoves.HorizontalOnly Or AllowedMoves.VerticalOnly) Then
            appropriateMovement = AllowedMoves.DiagonalOnly
        End If
        CurrentBehavior = GetAppropriateBehaviorOrFallback(appropriateMovement, ponySpeed)
        Dim speedupFactor = If(ponySpeed, 2, 1)
        Return If(appropriateMovement = AllowedMoves.None, 0, ScaledSpeed() * speedupFactor)
    End Function

    Public Property SpeedOverride As Double?

    Private Function ScaledSpeed() As Double
        Return If(SpeedOverride Is Nothing, CurrentBehavior.Speed * Scale, SpeedOverride.Value)
    End Function

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Base.Directory: " & Base.Directory
    End Function
End Class
#End Region

#Region "Pony2 class"
Public Class PonyContext
    Public Property SpeechEnabled As Boolean = True
    Public Property InteractionsEnabled As Boolean = True

    ' TODO: Work out best method of specifying chances like this - maybe just remove entirely?
    Public Property RandomSpeechChance As Double = 0.01

    Private _timeFactor As Double = 1
    Public Property TimeFactor As Double
        Get
            Return _timeFactor
        End Get
        Set(value As Double)
            If value < 0.1 OrElse value > 10 Then
                Throw New ArgumentOutOfRangeException("value", value, "value must be between 0.1 and 10 inclusive.")
            End If
            _timeFactor = value
        End Set
    End Property
    Private _scaleFactor As Single = 1
    Public Property ScaleFactor As Single
        Get
            Return _scaleFactor
        End Get
        Set(value As Single)
            If value < 0.25 OrElse value > 4 Then
                Throw New ArgumentOutOfRangeException("value", value, "value must be between 0.25 and 4 inclusive.")
            End If
            _scaleFactor = value
        End Set
    End Property
    Private _region As Rectangle = New Rectangle(0, 0, 800, 600)
    Public Property Region As Rectangle
        Get
            Return _region
        End Get
        Set(value As Rectangle)
            If _region.Width < 0 OrElse _region.Height < 0 Then
                Throw New ArgumentException("region must have non-negative width and height.", "value")
            End If
            _region = value
        End Set
    End Property
    Public Property CursorLocation As Vector2 = New Vector2(Integer.MinValue, Integer.MinValue)

    Private ReadOnly _pendingSprites As New List(Of ISprite)()
    Public ReadOnly Property PendingSprites As List(Of ISprite)
        Get
            Return _pendingSprites
        End Get
    End Property
    Public Property Sprites As IEnumerable(Of ISprite)
End Class

''' <summary>
''' Defines a sprite instance modeled on a <see cref="PonyBase"/>.
''' </summary>
Public Class Pony2
    Implements ISpeakingSprite, IDraggableSprite, IExpireableSprite, ISoundfulSprite
    Private ReadOnly _context As PonyContext
    ''' <summary>
    ''' Gets the context that affects how this pony behaviors.
    ''' </summary>
    Public ReadOnly Property Context() As PonyContext
        Get
            Return _context
        End Get
    End Property
    Private ReadOnly _base As PonyBase
    ''' <summary>
    ''' Gets the base on which this sprite instance is modeled.
    ''' </summary>
    Public ReadOnly Property Base() As PonyBase
        Get
            Return _base
        End Get
    End Property

#Region "Private State"
    ''' <summary>
    ''' Represents an arbitrary small non-zero floating-point value that should be used to specify a range within which floating-point
    ''' values should be considered equal.
    ''' </summary>
    Private Const Epsilon As Single = 1 / 2 ^ 24
    ''' <summary>
    ''' Number of milliseconds by which the internal temporal state of the sprite should be advanced with each call to StepOnce().
    ''' </summary>
    Private Const StepSize = 1000.0 / StepRate
    ''' <summary>
    ''' Number of simulation steps that are taken per second.
    ''' </summary>
    Private Const StepRate = 25.0
    ''' <summary>
    ''' Represents the current temporal dimension of the pony. This will be a multiple of the number of steps taken.
    ''' </summary>
    Private _currentTime As TimeSpan
    ''' <summary>
    ''' The external time value when Start() or Update() was last called.
    ''' </summary>
    Private _lastUpdateTime As TimeSpan
    ''' <summary>
    ''' Indicates whether Expire() has been called.
    ''' </summary>
    Private _expired As Boolean

    ''' <summary>
    ''' The time when a behavior was started.
    ''' </summary>
    Private _behaviorStartTime As TimeSpan
    ''' <summary>
    ''' The desired duration of the current behavior, after which it should be ended.
    ''' </summary>
    Private _behaviorDesiredDuration As TimeSpan
    ''' <summary>
    ''' The currently active behavior which defines how the pony acts.
    ''' </summary>
    Protected _currentBehavior As Behavior
    ''' <summary>
    ''' A behavior which, is specified, should be used to provide the current image instead of the current behavior (but nothing else).
    ''' </summary>
    Private _visualOverrideBehavior As Behavior
    ''' <summary>
    ''' Indicates if the pony is facing left or right.
    ''' </summary>
    Private _facingRight As Boolean
    ''' <summary>
    ''' The location of the center point of the pony.
    ''' </summary>
    Private _location As Vector2F = New Vector2F(Single.NaN, Single.NaN)
    ''' <summary>
    ''' A vector defining the movement to be applied to the location with each step.
    ''' </summary>
    Private _movement As Vector2F
    ''' <summary>
    ''' Indicates if a movement vector based on the allowed movement of the current behavior should be generated during this step.
    ''' </summary>
    Private _movementWithoutDestinationNeeded As Boolean
    ''' <summary>
    ''' Another pony instance which should be followed, if specified.
    ''' </summary>
    Private _followTarget As Pony2
    ''' <summary>
    ''' The destination vector that should be reached. This will either be an absolute screen location, or based on the location of the
    ''' follow target. If not specified, the pony may move freely as specified by the current behavior.
    ''' </summary>
    Private _destination As Vector2F?

    ''' <summary>
    ''' Minimum duration that must elapse after a speech ends before a random speech can be activated.
    ''' </summary>
    Private Shared ReadOnly _randomSpeechDelayDuration As TimeSpan = TimeSpan.FromSeconds(10)
    ''' <summary>
    ''' Duration for which a speech should last.
    ''' </summary>
    Private Shared ReadOnly _speechDuration As TimeSpan = TimeSpan.FromSeconds(3.5)
    ''' <summary>
    ''' The time a speech was last started.
    ''' </summary>
    Private _speechStartTime As TimeSpan = -(_randomSpeechDelayDuration + _speechDuration)
    ''' <summary>
    ''' The current speech text to be displayed, if specified.
    ''' </summary>
    Private _currentSpeechText As String
    ''' <summary>
    ''' The current speech sound file to be started, if specified.
    ''' </summary>
    Private _currentSpeechSound As String

    ''' <summary>
    ''' A collection of effect bases that should be repeated until the current behavior ends.
    ''' </summary>
    Private ReadOnly _effectBasesToRepeat As New List(Of EffectBaseRepeat)()
    ''' <summary>
    ''' A collection of effects that should be expired once the current behavior ends.
    ''' </summary>
    Private ReadOnly _effectsToManuallyExpire As New List(Of Effect)()
    ''' <summary>
    ''' A collection of unexpired effects that were started by this pony.
    ''' </summary>
    Private ReadOnly _activeEffects As New HashSet(Of Effect)()

    ''' <summary>
    ''' A collection of valid interactions (depending on what sprites were available when interactions were last initialized).
    ''' </summary>
    Private ReadOnly interactions As New List(Of Interaction(Of Pony2))()
    ''' <summary>
    ''' The interaction that this pony is currently involved with.
    ''' </summary>
    Private _currentInteraction As Interaction(Of Pony2)
    ''' <summary>
    ''' Time after which the last interaction has cooled off. Before this, the pony should not be considered eligible for further
    ''' interactions.
    ''' </summary>
    Private _interactionCooldownEndTime As TimeSpan

    ''' <summary>
    ''' Indicates if the pony is currently in a state reacting to mouseover.
    ''' </summary>
    Private _inMouseoverState As Boolean
    ''' <summary>
    ''' Indicates if the pony is currently in a state reacting to being dragged.
    ''' </summary>
    ''' <remarks></remarks>
    Private _inDragState As Boolean
    ''' <summary>
    ''' Indicates if the pony is currently in a state reacting to being asleep.
    ''' </summary>
    Private _inSleepState As Boolean
    ''' <summary>
    ''' Records the last active behavior before the mouseover or sleep state was activated.
    ''' </summary>
    Private _behaviorBeforeSpecialStateOverride As Behavior
    ''' <summary>
    ''' The behavior to use during dragging.
    ''' </summary>
    Private _dragBehavior As Behavior
    ''' <summary>
    ''' The behavior to use during mouseover.
    ''' </summary>
    Private _mouseoverBehavior As Behavior
    ''' <summary>
    ''' The behavior to use when asleep.
    ''' </summary>
    Private _sleepBehavior As Behavior

    ''' <summary>
    ''' Gets a value indicating if the pony is busy and should not be considered for interactions.
    ''' </summary>
    Private ReadOnly Property isBusy As Boolean
        Get
            Return _currentInteraction IsNot Nothing OrElse
                _inMouseoverState OrElse
                _inDragState OrElse
                _inSleepState
        End Get
    End Property

#Region "Objects"
    ''' <summary>
    ''' Tracks when a repeating effect was last used.
    ''' </summary>
    Private Structure EffectBaseRepeat
        ''' <summary>
        ''' The effect base.
        ''' </summary>
        Public ReadOnly EffectBase As EffectBase
        ''' <summary>
        ''' The time an instance of this effect was last started.
        ''' </summary>
        Public ReadOnly LastStartTime As TimeSpan
        ''' <summary>
        ''' Initializes a new instance of the <see cref="EffectBaseRepeat"/> structure.
        ''' </summary>
        ''' <param name="effectBase">The base for the repeating effect.</param>
        ''' <param name="lastStartTime">The time an instance of this effect was last started.</param>
        Public Sub New(effectBase As EffectBase, lastStartTime As TimeSpan)
            Me.EffectBase = effectBase
            Me.LastStartTime = lastStartTime
        End Sub
    End Structure

    ''' <summary>
    ''' Compares ponies for equality via the base directory.
    ''' </summary>
    Private Class PonyDirectoryEqualityComparer
        Implements IEqualityComparer(Of Pony2)
        ''' <summary>
        ''' Gets the instance of this comparer.
        ''' </summary>
        Public Shared ReadOnly Instance As PonyDirectoryEqualityComparer = New PonyDirectoryEqualityComparer()
        Private Sub New()
        End Sub
        ''' <summary>
        ''' Determines whether the base directories of each specified pony are equal.
        ''' </summary>
        ''' <param name="x">The first pony.</param>
        ''' <param name="y">The second pony.</param>
        ''' <returns>Return true if the base directories are equal; otherwise, false.</returns>
        Public Overloads Function Equals(x As Pony2, y As Pony2) As Boolean Implements IEqualityComparer(Of Pony2).Equals
            Return (x IsNot Nothing AndAlso y IsNot Nothing AndAlso x.Base.Directory = y.Base.Directory) OrElse
                (x Is Nothing AndAlso y Is Nothing)
        End Function
        ''' <summary>
        ''' Gets the hash code of the base directory of the specified pony.
        ''' </summary>
        ''' <param name="obj">The pony.</param>
        ''' <returns>The hash code of the base directory of the specified pony.</returns>
        Public Overloads Function GetHashCode(obj As Pony2) As Integer Implements IEqualityComparer(Of Pony2).GetHashCode
            Return If(obj Is Nothing, 0, obj.Base.Directory.GetHashCode())
        End Function
    End Class

    ''' <summary>
    ''' Represents an eligible interactions and its targets.
    ''' </summary>
    Private Structure EligableInteraction
        ''' <summary>
        ''' The interaction.
        ''' </summary>
        Public ReadOnly Interaction As Interaction(Of Pony2)
        ''' <summary>
        ''' The non-busy targets that may be activated. For interactions specifying a single target, they are also within range of the
        ''' interaction.
        ''' </summary>
        Public ReadOnly Targets As ImmutableArray(Of Pony2)
        ''' <summary>
        ''' Initializes a new instance of the <see cref="EligableInteraction"/> structure.
        ''' </summary>
        ''' <param name="interaction">The eligible interaction.</param>
        ''' <param name="targets">The targets that may be activated. These ponies must not be busy. For interactions specifying a single
        ''' target, these must also all be in range of the interaction.</param>
        Public Sub New(interaction As Interaction(Of Pony2), targets As ImmutableArray(Of Pony2))
            Me.Interaction = interaction
            Me.Targets = targets
        End Sub
    End Structure
#End Region
#End Region

    ''' <summary>
    ''' Initializes a new instance of the <see cref="Pony2"/> class.
    ''' </summary>
    ''' <param name="context">The context within which the pony is contained. This will affect how it acts.</param>
    ''' <param name="base">The base on which this instance should be modeled.</param>
    ''' <exception cref="ArgumentNullException"><paramref name="context"/> is null.-or-<paramref name="base"/> is null.</exception>
    ''' <exception cref="ArgumentException"><paramref name="base"/> does not contain at least one behavior that can be used at random from
    ''' the 'any' group, or it does not contain a suitable mouseover behavior.</exception>
    Public Sub New(context As PonyContext, base As PonyBase)
        _context = Argument.EnsureNotNull(context, "context")
        _base = Argument.EnsureNotNull(base, "base")
        If GetRandomBehavior(Behavior.AnyGroup) Is Nothing Then
            Throw New ArgumentException("base must contain at least one behavior that can be used at random in the 'Any' group.", "base")
        End If
        Dim fallbackStationaryBehavior = GetBehaviorMatching(
            Function(b) b.SpeedInPixelsPerSecond = 0 AndAlso Not b.Skip AndAlso Not BehaviorHasTarget(b),
            Function(b) b.SpeedInPixelsPerSecond = 0)
        _mouseoverBehavior = If(GetBehaviorMatching(Function(b) b.AllowedMovement.HasFlag(AllowedMoves.MouseOver)),
                                fallbackStationaryBehavior)
        If _mouseoverBehavior Is Nothing Then
            Throw New ArgumentException("base must contain a suitable mouseover behavior.", "base")
        End If
        Dim flaggedSleepBehavior = GetBehaviorMatching(Function(b) b.AllowedMovement.HasFlag(AllowedMoves.Sleep))
        _sleepBehavior = If(flaggedSleepBehavior, fallbackStationaryBehavior)
        _dragBehavior = If(GetBehaviorMatching(Function(b) b.AllowedMovement.HasFlag(AllowedMoves.Dragged)),
                           If(flaggedSleepBehavior, _mouseoverBehavior))
    End Sub

    ''' <summary>
    ''' Gets the current image (based on the visual override behavior, current behavior and facing).
    ''' </summary>
    Private ReadOnly Property currentImage As CenterableSpriteImage
        Get
            Dim behavior = If(If(_visualOverrideBehavior, _currentBehavior), Base.Behaviors(0))
            Return If(_facingRight, behavior.RightImage, behavior.LeftImage)
        End Get
    End Property

    ''' <summary>
    ''' Gets the path to the image file that should be used to display the sprite.
    ''' </summary>
    Public ReadOnly Property ImagePath As String Implements ISprite.ImagePath
        Get
            Return currentImage.Path
        End Get
    End Property

    ''' <summary>
    ''' Gets a value indicating whether the image should be flipped horizontally from its original orientation.
    ''' </summary>
    Private ReadOnly Property FlipImage As Boolean Implements ISprite.FlipImage
        Get
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Gets the non-aligned region the sprite currently occupies.
    ''' </summary>
    Private ReadOnly Property regionF As RectangleF
        Get
            Return GetRegionFForImage(currentImage)
        End Get
    End Property

    ''' <summary>
    ''' Gets the region the sprite currently occupies.
    ''' </summary>
    Public ReadOnly Property Region As Rectangle Implements ISprite.Region
        Get
            Return New Rectangle(Vector2.Round(regionF.Location), Vector2.Truncate(regionF.Size))
        End Get
    End Property

    ''' <summary>
    ''' Gets the time index into the current image (for animated images).
    ''' </summary>
    Public ReadOnly Property ImageTimeIndex As TimeSpan Implements ISprite.ImageTimeIndex
        Get
            Return _currentTime - _behaviorStartTime
        End Get
    End Property

    ''' <summary>
    ''' Gets the current speech text that is being spoken by the sprite, or null to indicate nothing is being spoken.
    ''' </summary>
    Public ReadOnly Property SpeechText As String Implements ISpeakingSprite.SpeechText
        Get
            Return _currentSpeechText
        End Get
    End Property

    ''' <summary>
    ''' Gets the path to the sound file that should be played starting as of this update, or null to indicate nothing new should be
    ''' started.
    ''' </summary>
    Public ReadOnly Property SoundPath As String Implements ISoundfulSprite.SoundPath
        Get
            Return _currentSpeechSound
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets whether the sprite should be in a state where it acts as if dragged by the cursor.
    ''' </summary>
    Public Property Drag As Boolean Implements IDraggableSprite.Drag

    ''' <summary>
    ''' Occurs when <see cref="Expire"/> is called.
    ''' </summary>
    Public Event Expired As EventHandler Implements IExpireableSprite.Expired

    ''' <summary>
    ''' Gets or sets whether the sprite should be in a state where it 'sleeps' and does not move, interact or react.
    ''' </summary>
    Public Property Sleep As Boolean

    ''' <summary>
    ''' Starts the sprite using the given time as a zero point.
    ''' </summary>
    ''' <param name="startTime">The time that will be used as a zero point against the time given in future updates.</param>
    Public Sub Start(startTime As TimeSpan) Implements ISprite.Start
        _currentTime = startTime
        _lastUpdateTime = startTime
        SetBehavior()
        Dim area = New Vector2(Context.Region.Size) - New Vector2F(regionF.Size)
        _location = currentImage.Center * Context.ScaleFactor +
            New Vector2F(CSng(area.X * Rng.NextDouble()), CSng(area.Y * Rng.NextDouble()))
    End Sub

    ''' <summary>
    ''' Updates the sprite to the given instant in time.
    ''' </summary>
    ''' <param name="updateTime">The instant in time which the sprite should update itself to.</param>
    Public Sub Update(updateTime As TimeSpan) Implements ISprite.Update
        ' Find out how far behind the sprite is since its last update, and catch up.
        ' The time factor here means the internal time of the sprite can be advanced at different rates than the external time.
        ' This fixed time step method of updating is prone to temporal aliasing, but this is largely unnoticeable compared to the generally
        ' low frame rate of animations and lack of spatial anti-aliasing since the images are pixel art. That said, the time scaling should
        ' be constrained from being too low (which will exaggerate the temporal aliasing until it is noticeable) or too high (which kills
        ' performance as StepOnce must be evaluated many times to catch up).
        _currentSpeechSound = Nothing
        Dim difference = updateTime - _lastUpdateTime
        While Not _expired AndAlso difference.TotalMilliseconds > 0
            StepOnce()
            difference -= TimeSpan.FromMilliseconds(StepSize / Context.TimeFactor)
        End While
        _lastUpdateTime = updateTime - difference
    End Sub

    ''' <summary>
    ''' Advances the temporal state of the pony by a single fixed time step.
    ''' </summary>
    Private Sub StepOnce()
        _currentTime += TimeSpan.FromMilliseconds(StepSize)

        HandleSleep()
        HandleMouseoverAndDrag()
        SetInteraction()
        If _currentTime - _behaviorStartTime > _behaviorDesiredDuration Then
            ' Having no linked behavior when interacting means we've run to the last part of a chain and should end the interaction.
            If _currentInteraction IsNot Nothing AndAlso _currentBehavior.LinkedBehavior Is Nothing Then EndInteraction()

            If _currentBehavior.EndLine IsNot Nothing Then Speak(_currentBehavior.EndLine)
            SetBehavior(_currentBehavior.LinkedBehavior)
            If _currentBehavior.StartLine IsNot Nothing Then
                Speak(_currentBehavior.StartLine)
            ElseIf _currentBehavior.EndLine Is Nothing AndAlso Rng.NextDouble() < Context.RandomSpeechChance Then
                Speak()
            End If
            StartEffects()
        End If
        If _currentTime - _speechStartTime > _speechDuration Then _currentSpeechText = Nothing
        If _followTarget IsNot Nothing AndAlso _followTarget._expired Then _followTarget = Nothing
        UpdateDestination()
        UpdateMovement()
        SetVisualOverrideBehavior()
        UpdateLocation()
        RepeatEffects()
    End Sub

    ''' <summary>
    ''' Transfers the pony in and out of the sleeping state. The sleep state will be set. The behavior will be set as a side effect of
    ''' state transitions. The behavior desired duration will be modified to prevent the behavior expiring whilst in the sleep state.
    ''' </summary>
    Private Sub HandleSleep()
        If Sleep AndAlso Not _inSleepState Then
            ' Enter sleep state.
            _inSleepState = True
            _behaviorBeforeSpecialStateOverride = _currentBehavior
            SetBehavior(_sleepBehavior)
        ElseIf Not Sleep AndAlso _inSleepState Then
            ' Exit sleep state.
            _inSleepState = False
            If _currentInteraction Is Nothing Then SetBehavior(_behaviorBeforeSpecialStateOverride)
            _behaviorBeforeSpecialStateOverride = Nothing
        End If
        If _inSleepState Then ExtendBehaviorDurationIndefinitely()
    End Sub

    ''' <summary>
    ''' If not asleep or interacting, transfers the pony in and out of the mouseover and drag states. The mouseover state, drag state and
    ''' behavior before mouseover will be set. The behavior will be set as a side effect of state transitions. Transitioning into the
    ''' mouseover state will trigger a random speech. The behavior desired duration will be modified to prevent behaviors expiring whilst
    ''' in the mouseover or drag states.
    ''' </summary>
    Private Sub HandleMouseoverAndDrag()
        If _inSleepState OrElse _currentInteraction IsNot Nothing Then Return
        Dim mouseoverImage = If(_facingRight, _mouseoverBehavior.RightImage, _mouseoverBehavior.LeftImage)
        Dim mouseoverRegion = RectangleF.Intersect(regionF, GetRegionFForImage(mouseoverImage))
        Dim isMouseOver = mouseoverRegion.Contains(Context.CursorLocation.X, Context.CursorLocation.Y)
        If isMouseOver AndAlso Not _inMouseoverState Then
            ' Enter mouseover state.
            _inMouseoverState = True
            _behaviorBeforeSpecialStateOverride = _currentBehavior
            SetBehavior(_mouseoverBehavior)
            Speak()
        End If
        If Drag AndAlso Not _inDragState Then
            ' Enter drag state.
            _inDragState = True
            SetBehavior(_dragBehavior)
        ElseIf Not Drag AndAlso _inDragState Then
            ' Exit drag state.
            _inDragState = False
            SetBehavior(_mouseoverBehavior)
        End If
        If Not isMouseOver AndAlso _inMouseoverState Then
            ' Exit mouseover state.
            _inMouseoverState = False
            If _currentInteraction Is Nothing Then SetBehavior(_behaviorBeforeSpecialStateOverride)
            _behaviorBeforeSpecialStateOverride = Nothing
        End If
        If _inMouseoverState OrElse _inDragState Then ExtendBehaviorDurationIndefinitely()
    End Sub

    ''' <summary>
    ''' Extends the behavior desired duration to last indefinitely - preventing it expiring.
    ''' </summary>
    Private Sub ExtendBehaviorDurationIndefinitely()
        _behaviorDesiredDuration = _currentTime + TimeSpan.FromSeconds(_currentBehavior.MaxDuration)
    End Sub

    ''' <summary>
    ''' If the context allows speech; speaks the suggested speech, or else a random speech. The start speech time and current speech text
    ''' are set.
    ''' </summary>
    ''' <param name="suggested">A speech to speak, or null to choose one at random. Randomly chosen speeches are limited: 10 seconds since
    ''' the last speech must have passed and no interaction or follow target may be active. A random speech is then uniformly selected from
    ''' the any group, or current behavior group.</param>
    Private Sub Speak(Optional suggested As Speech = Nothing)
        If Not Context.SpeechEnabled Then Return

        ' Select a line at random from the lines that may be played at random that are in the current group.
        If suggested Is Nothing Then
            If Base.Speeches.Count = 0 Then Return
            If _currentInteraction IsNot Nothing OrElse
                _followTarget IsNot Nothing OrElse
                _currentTime - (_speechStartTime + _speechDuration) < _randomSpeechDelayDuration Then Return
            Dim randomGroupLines = Base.SpeechesRandom.Where(
                Function(s) s.Group = Behavior.AnyGroup OrElse s.Group = _currentBehavior.Group).ToImmutableArray()
            If randomGroupLines.Length = 0 Then Return
            suggested = randomGroupLines(Rng.Next(randomGroupLines.Length))
        End If

        ' Set the line text to be displayed.
        _speechStartTime = _currentTime
        _currentSpeechText = Base.DisplayName & ": " & ControlChars.Quote & suggested.Text & ControlChars.Quote
        _currentSpeechSound = suggested.SoundFile
    End Sub

    ''' <summary>
    ''' Activates the suggested behavior, or else a random behavior. The current behavior, behavior start time, behavior desired duration
    ''' and follow target will be set. Any effects for the last behavior tied to its duration will end, and any repeating effects no longer
    ''' repeated. The movement without destination flag will be set.
    ''' </summary>
    ''' <param name="suggested">A behavior to activate, or null to choose one at random. A random behavior is uniformly selected from the
    ''' any group, or current behavior group. If there are no suitable behaviors, an exception is thrown.</param>
    Protected Sub SetBehavior(Optional suggested As Behavior = Nothing)
        _followTarget = Nothing
        _destination = Nothing
        _movementWithoutDestinationNeeded = True
        Dim currentBehaviorGroup = If(_currentBehavior Is Nothing, Behavior.AnyGroup, _currentBehavior.Group)
        _currentBehavior = If(suggested, GetRandomBehavior(currentBehaviorGroup))
        If _currentBehavior Is Nothing Then Throw New InvalidOperationException("Could not find a suitable behavior.")
        _behaviorStartTime = _currentTime
        If _inMouseoverState OrElse _inDragState Then
            _behaviorDesiredDuration = TimeSpan.FromDays(1)
        Else
            _behaviorDesiredDuration = TimeSpan.FromSeconds(
                _currentBehavior.MinDuration + Rng.NextDouble() * (_currentBehavior.MaxDuration - _currentBehavior.MinDuration))
        End If
        SetFollowTarget()

        ' Clean up old effects from the previous behavior.
        For Each effect In _effectsToManuallyExpire
            effect.Expire()
        Next
        _effectsToManuallyExpire.Clear()
        _effectBasesToRepeat.Clear()
    End Sub

    ''' <summary>
    ''' Uniformly selects a random behavior from those in the any group or the specified group.
    ''' </summary>
    ''' <param name="group">The group for additional candidate behaviors to those in the any group.</param>
    ''' <returns>A behavior selected uniformly from available candidates, or null if there were no matching candidate behaviors.</returns>
    Private Function GetRandomBehavior(group As Integer) As Behavior
        Dim candidates = Base.Behaviors.Where(
                Function(b) Not b.Skip AndAlso (b.Group = Behavior.AnyGroup OrElse b.Group = group)).ToImmutableArray()
        If candidates.Length = 0 Then
            Return Nothing
        ElseIf candidates.Length = 1 Then
            Return candidates(0)
        Else
            Dim totalChance = candidates.Sum(Function(b) b.Chance)
            Dim randomChance = Rng.NextDouble() * totalChance
            Dim currentChance = 0.0
            Dim randomChoice As Behavior = Nothing
            For Each candidate In candidates
                randomChoice = candidate
                currentChance += candidate.Chance
                If currentChance >= randomChance Then Exit For
            Next
            Return randomChoice
        End If
    End Function

    ''' <summary>
    ''' Iterates over all specified predicates searching the first the behavior that satisfies the predicate.
    ''' </summary>
    ''' <param name="predicates">An ordered set of predicates to match by, in descending order of preference.</param>
    ''' <returns>The first behavior that matches the predicates as tested in descending order, otherwise null.</returns>
    Private Function GetBehaviorMatching(ParamArray predicates As Predicate(Of Behavior)()) As Behavior
        For Each predicate In predicates
            For Each behavior In Base.Behaviors
                If predicate(behavior) Then Return behavior
            Next
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Indicates if the specified behavior has a follow target, or screen destination specified.
    ''' </summary>
    ''' <param name="behavior">The behavior to check for a follow target object or destination.</param>
    ''' <returns>Returns true if a follow target is specified (regardless of if a matching target object is present) or a screen
    ''' destination is specified.</returns>
    Private Shared Function BehaviorHasTarget(behavior As Behavior) As Boolean
        Return behavior.OriginalFollowTargetName <> "" OrElse
            behavior.OriginalDestinationXCoord <> 0 OrElse
            behavior.OriginalDestinationYCoord <> 0
    End Function

    ''' <summary>
    ''' Sets an actual pony to follow, based on the desired target. If an interaction is running, the initiator will prefer to follow any
    ''' involved interaction targets (if suitable) and any targets will prefer to follow the initiator, or then other targets (if
    ''' suitable). Otherwise, a target is chosen uniformly from all available targets.
    ''' </summary>
    Private Sub SetFollowTarget()
        If _followTarget Is Nothing AndAlso _currentBehavior.OriginalFollowTargetName <> "" Then
            ' If an interaction is running, we want to prefer those ponies involved in the interaction before trying other ponies.
            If _currentInteraction IsNot Nothing Then
                If ReferenceEquals(Me, _currentInteraction.Initiator) Then
                    ' If we are the interaction initiator, prefer an involved target.
                    _followTarget = GetRandomFollowTarget(_currentInteraction.InvolvedTargets)
                Else
                    ' If we are an interaction target, prefer to follow the initiator if they are suitable.
                    ' Otherwise follow any of the other targets - but avoid following ourselves since we are a target.
                    If _currentBehavior.OriginalFollowTargetName = _currentInteraction.Initiator.Base.Directory Then
                        _followTarget = _currentInteraction.Initiator
                    Else
                        _followTarget = GetRandomFollowTarget(_currentInteraction.InvolvedTargets.Where(
                                                              Function(p) Not ReferenceEquals(Me, p)))
                    End If
                End If
            End If
            If _followTarget Is Nothing Then
                ' Pick any pony at random.
                _followTarget = GetRandomFollowTarget(Context.Sprites.OfType(Of Pony2)().Where(
                                                      Function(p) Not ReferenceEquals(Me, p)))
            End If
        End If
    End Sub

    ''' <summary>
    ''' Uniformly selects at random a follow target from all specified candidate ponies which are eligible to be followed for the current
    ''' behavior.
    ''' </summary>
    ''' <param name="allCandidates">A selection of candidate ponies. This may include ponies unsuitable for following according to the
    ''' current behavior, as these will be filtered out. Expired candidates are also filtered out. The current instance must not be present
    ''' in the collection.</param>
    ''' <returns>A randomly selected target from all suitable candidates for the current behavior, or null if one could not be found.
    ''' </returns>
    Private Function GetRandomFollowTarget(allCandidates As IEnumerable(Of Pony2)) As Pony2
        Dim suitableCandidates = allCandidates.Where(
            Function(p) Not p._expired AndAlso p.Base.Directory = _currentBehavior.OriginalFollowTargetName).ToImmutableArray()
        If suitableCandidates.Length > 0 Then
            Return suitableCandidates(Rng.Next(suitableCandidates.Length))
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Sets the visual override behavior. The behavior used is either directly specified by the current behavior, or else a suitable
    ''' behavior is determined automatically. If a suitable override is not found, or one is not required, the override will be cleared.
    ''' Thus, the images to use fall back to the current behavior.
    ''' </summary>
    Private Sub SetVisualOverrideBehavior()
        _visualOverrideBehavior = Nothing
        If _followTarget IsNot Nothing Then
            Dim currentSpeed = _movement.Length()
            If Not _currentBehavior.AutoSelectImagesOnFollow Then
                If currentSpeed = 0 Then
                    _visualOverrideBehavior = _currentBehavior.FollowStoppedBehavior
                Else
                    _visualOverrideBehavior = _currentBehavior.FollowMovingBehavior
                End If
            End If
            ' TODO: Better matching.
            If _visualOverrideBehavior Is Nothing Then
                If currentSpeed = 0 Then
                    _visualOverrideBehavior = GetBehaviorMatching(Function(b) b.SpeedInPixelsPerSecond = 0)
                Else
                    _visualOverrideBehavior = GetBehaviorMatching(Function(b) b.SpeedInPixelsPerSecond > 0)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Updates the destination vector. It will be adjusted for the current follow target (if any), point to an absolute screen location
    ''' (if specified) or else be cleared.
    ''' </summary>
    Private Sub UpdateDestination()
        Dim offsetVector = New Vector2(_currentBehavior.OriginalDestinationXCoord, _currentBehavior.OriginalDestinationYCoord)
        If _currentBehavior.OriginalFollowTargetName <> "" Then
            ' Move to follow target.
            ' Here the offset represents a custom offset from the center of the target.
            If _followTarget IsNot Nothing Then
                'If Not _followTarget._facingRight Then offsetVector.X = -offsetVector.X
                _destination = _followTarget._location + offsetVector
                Return
            End If
        ElseIf offsetVector <> Vector2.Zero Then
            ' We need to head to some point relative to the display area.
            ' Here the offset represents the relative location normalized to a scale of 0-100 along each axis.
            Dim relativeDestination = offsetVector * 0.01F
            Dim region = Context.Region
            _destination = New Vector2F(region.Location) +
                New Vector2F(relativeDestination.X * region.Width, relativeDestination.Y * region.Height)
            Return
        End If
        _destination = Nothing
    End Sub

    ''' <summary>
    ''' Updates the movement vector. The will be zeroed when in the mouseover or drag states - will be calculated so as to move towards a
    ''' destination (if specified) or else be set to a free movement state as specified by the current behavior if the movement without
    ''' destination needed flag is set (this flag then becomes unset). Otherwise no change is made. The facing state may also be set.
    ''' </summary>
    Private Sub UpdateMovement()
        If _inMouseoverState OrElse _inDragState OrElse _inSleepState Then
            ' No movement whilst in special state modes.
            _movement = Vector2F.Zero
        ElseIf _destination IsNot Nothing Then
            ' Set movement with destination.
            _movement = _destination.Value - _location
            Dim magnitude = _movement.Length()
            If magnitude > Epsilon Then
                Dim speed = CSng(_currentBehavior.SpeedInPixelsPerSecond / StepRate)
                If magnitude > speed Then _movement = _movement / magnitude * speed
                _facingRight = _movement.X > 0
            End If
        ElseIf _movementWithoutDestinationNeeded Then
            ' Move freely based on current behavior with no defined destination.
            SetMovementWithoutDestination()
            _movementWithoutDestinationNeeded = False
        End If
    End Sub

    ''' <summary>
    ''' Sets the movement vector depending on the allowed moves and speed of the current behavior. The facing state will also be set
    ''' randomly.
    ''' </summary>
    Private Sub SetMovementWithoutDestination()
        _movement = Vector2F.Zero
        Dim speed = _currentBehavior.SpeedInPixelsPerSecond / StepRate
        If speed > 0 Then
            Dim moves = _currentBehavior.AllowedMovement And AllowedMoves.All
            If moves > 0 Then
                Dim movesList As New List(Of AllowedMoves)()
                If (moves And AllowedMoves.HorizontalOnly) > 0 Then movesList.Add(AllowedMoves.HorizontalOnly)
                If (moves And AllowedMoves.VerticalOnly) > 0 Then movesList.Add(AllowedMoves.VerticalOnly)
                If (moves And AllowedMoves.DiagonalOnly) > 0 Then movesList.Add(AllowedMoves.DiagonalOnly)
                Dim selectedDirection = movesList(Rng.Next(movesList.Count))
                Select Case selectedDirection
                    Case AllowedMoves.HorizontalOnly
                        _movement = New Vector2F(CSng(speed), 0)
                    Case AllowedMoves.VerticalOnly
                        _movement = New Vector2F(0, CSng(speed))
                    Case AllowedMoves.DiagonalOnly
                        _movement = New Vector2F(CSng(speed * Math.Sin(Math.PI / 4)), CSng(speed * Math.Cos(Math.PI / 4)))
                End Select
                _facingRight = Rng.NextDouble() < 0.5
                If Not _facingRight Then _movement.X = -_movement.X
                If Rng.NextDouble() < 0.5 Then _movement.Y = -_movement.Y
            End If
        End If
    End Sub

    ''' <summary>
    ''' Starts any effects specified by the current behavior immediately, and sets up repeating effects if required.
    ''' </summary>
    Private Sub StartEffects()
        For Each effectBase In _currentBehavior.Effects
            StartNewEffect(effectBase)
            If effectBase.RepeatDelay > 0 Then
                _effectBasesToRepeat.Add(New EffectBaseRepeat(effectBase, _currentTime))
            End If
        Next
    End Sub

    ''' <summary>
    ''' Starts a new effect modeled on the specified base, and adds it to the pending sprites for the assigned context. Effects which last
    ''' until the next behavior change are remembered.
    ''' </summary>
    ''' <param name="effectBase">The base which is used as a model for the new effect instance.</param>
    Private Sub StartNewEffect(effectBase As EffectBase)
        Dim effect = New Effect(effectBase, Not _facingRight,
                                Function() Me.regionF.Location,
                                Function() Me.Region.Size,
                                Function() 1)
        If effectBase.Duration = 0 Then
            _effectsToManuallyExpire.Add(effect)
        Else
            effect.DesiredDuration = TimeSpan.FromSeconds(effectBase.Duration)
        End If
        _activeEffects.Add(effect)
        AddHandler effect.Expired, Sub() _activeEffects.Remove(effect)
        Context.PendingSprites.Add(effect)
    End Sub

    ''' <summary>
    ''' Updates the collection of effect bases to repeat, by starting a new effect each time the repeat delay elapses since an effect of
    ''' its type was last deployed.
    ''' </summary>
    Private Sub RepeatEffects()
        ' TODO: Update this method to start effects at the correct time - i.e. at the time in the past when the delay expired.
        For i = 0 To _effectBasesToRepeat.Count - 1
            Dim repeatState = _effectBasesToRepeat(i)
            Dim base = repeatState.EffectBase
            If repeatState.LastStartTime - _currentTime > TimeSpan.FromSeconds(base.RepeatDelay) Then
                StartNewEffect(base)
                _effectBasesToRepeat(i) = New EffectBaseRepeat(base, _currentTime)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Updates the location vector. When in the drag state, this will be the cursor location according to the current context, otherwise
    ''' the movement vector will be applied. When moving according to this vector, ponies may not stray entirely outside the context
    ''' boundary and will be teleported to its outer edge if they stray too far. If no destination is specified, ponies will rebound off
    ''' the inner boundary edge when moving. The movement vector and facing will be updated when teleporting or rebounding.
    ''' </summary>
    Private Sub UpdateLocation()
        If _inDragState Then
            _location = Context.CursorLocation
        Else
            _location += _movement
            TeleportToBoundaryIfOutside()
            If _destination Is Nothing Then ReboundOffBoundary()
        End If
    End Sub

    ''' <summary>
    ''' If there is no overlap between the pony region and the context region, the location is adjusted so the the nearest two edges become
    ''' collinear. In effect, the pony is clamped to the outer edge of the boundary. The movement vector and facing state will be changed
    ''' to put the pony on a course to within the context region.
    ''' </summary>
    Private Sub TeleportToBoundaryIfOutside()
        Dim contextRegion = Context.Region
        Dim currentRegion = regionF
        If contextRegion.Top > currentRegion.Bottom Then
            _Location.Y += contextRegion.Top - currentRegion.Bottom
            If _movement.Y < 0 Then _movement.Y = -_movement.Y
        ElseIf currentRegion.Top > contextRegion.Bottom Then
            _Location.Y -= currentRegion.Top - contextRegion.Bottom
            If _movement.Y > 0 Then _movement.Y = -_movement.Y
        End If
        If contextRegion.Left > currentRegion.Right Then
            _Location.X += contextRegion.Left - currentRegion.Right
            If _movement.X < 0 Then _movement.X = -_movement.X
        ElseIf currentRegion.Left > contextRegion.Right Then
            _Location.X -= currentRegion.Left - contextRegion.Right
            If _movement.X > 0 Then _movement.X = -_movement.X
        End If
        If _movement.X <> 0 Then _facingRight = _movement.X > 0
    End Sub

    ''' <summary>
    ''' If any of the pony region extends outside the context region, the location will be mirrored around the context edge to give the
    ''' effect of rebounding off of this edge (this assumes the location was previously in bounds). The movement vector and facing state
    ''' will be updated if required.
    ''' </summary>
    Private Sub ReboundOffBoundary()
        Dim contextRegion = Context.Region
        Dim currentRegion = regionF
        If contextRegion.Top > currentRegion.Top Then
            _Location.Y += contextRegion.Top - currentRegion.Top
            If _movement.Y < 0 Then _movement.Y = -_movement.Y
        ElseIf currentRegion.Bottom > contextRegion.Bottom Then
            _Location.Y -= currentRegion.Bottom - contextRegion.Bottom
            If _movement.Y > 0 Then _movement.Y = -_movement.Y
        End If
        If contextRegion.Left > currentRegion.Left Then
            _Location.X += contextRegion.Left - currentRegion.Left
            If _movement.X < 0 Then _movement.X = -_movement.X
        ElseIf currentRegion.Right > contextRegion.Right Then
            _Location.X -= currentRegion.Right - contextRegion.Right
            If _movement.X > 0 Then _movement.X = -_movement.X
        End If
        If _movement.X <> 0 Then _facingRight = _movement.X > 0
    End Sub

    ''' <summary>
    ''' Using the collection of interaction bases available to the base of this pony, generates interactions that can be used depending on
    ''' the other available sprites that can be interacted with. These interactions will be triggered during update cycles.
    ''' </summary>
    ''' <param name="currentPonies">The ponies available to interact with. This may include the current instance, but must not contain
    ''' duplicate references.</param>
    Public Sub InitializeInteractions(currentPonies As IEnumerable(Of Pony2))
        Argument.EnsureNotNull(currentPonies, "currentPonies")

        If Base.Directory Is Nothing Then Return

        interactions.Clear()
        For Each interactionBase In Base.Interactions
            Dim interaction = New Interaction(Of Pony2)(interactionBase)

            ' Get all actual instances of target ponies.
            interaction.Targets.Clear()
            Dim missingTargetNames = New HashSet(Of String)(interactionBase.TargetNames)
            For Each candidatePony In currentPonies
                If Not ReferenceEquals(Me, candidatePony) AndAlso interactionBase.TargetNames.Contains(candidatePony.Base.Directory) Then
                    missingTargetNames.Remove(candidatePony.Base.Directory)
                    interaction.Targets.Add(candidatePony)
                End If
            Next
            ' If no instances of the target ponies are present, we can forget this interaction.
            ' Alternatively, if it is specified all targets must be present but some are missing, the interaction cannot be used.
            If interaction.Targets.Count = 0 OrElse
                (interaction.Base.Activation = TargetActivation.All AndAlso
                missingTargetNames.Count > 0) Then
                Continue For
            End If

            ' Determine the common set of behaviors by name actually implemented by this pony and all discovered targets.
            Dim commonBehaviors As New HashSet(Of CaseInsensitiveString)(interactionBase.BehaviorNames)
            For Each pony In {Me}.Concat(interaction.Targets)
                commonBehaviors.IntersectWith(pony.Base.Behaviors.Select(Function(b) b.Name))
                If commonBehaviors.Count = 0 Then Exit For
            Next
            ' We need at least one common behavior to exist on all targets, or we cannot run the interaction.
            If commonBehaviors.Count = 0 Then Continue For
            ' Keep a copy of behaviors for the current pony for use when the interaction is activated later.
            interaction.Behaviors.AddRange(Base.Behaviors.Where(Function(b) commonBehaviors.Contains(b.Name)))

            ' We can list this as a possible interaction.
            interactions.Add(interaction)
        Next
    End Sub

    ''' <summary>
    ''' Starts an interaction at random if the context allows interactions, the pony is not busy and the interaction cool down has expired.
    ''' Interactions eligible for starting are those that pass their chance check, and then have any free target in proximity. Interactions
    ''' requiring all targets additionally need a free target matching each target name (though only one need be in range). Of all eligible
    ''' interactions, one is uniformly selected at random to be started. When StartInteraction() is called, it will be passed free targets
    ''' in range for interactions that need one target, otherwise it will be passed all free targets regardless of range. See
    ''' StartInteraction() for affected state. If no interactions are eligible, no interactions are started.
    ''' </summary>
    Private Sub SetInteraction()
        'If we are running or recently ran an interaction, don't start a new one yet.
        If Not Context.InteractionsEnabled OrElse isBusy OrElse _currentTime < _interactionCooldownEndTime Then
            Return
        End If

        Dim eligableInteractions As List(Of EligableInteraction) = Nothing
        For Each interaction In interactions
            If Rng.NextDouble() >= interaction.Base.Chance Then Continue For
            Dim freeTargets = interaction.Targets.Where(Function(p) Not p.isBusy).ToImmutableArray()
            If freeTargets.Length = 0 Then Continue For
            Dim freeTargetsInRange = freeTargets.Where(
                Function(p) Vector2F.Distance(_location, p._location) <= interaction.Base.Proximity).ToImmutableArray()
            If freeTargetsInRange.Length = 0 Then Continue For

            If interaction.Base.Activation <> TargetActivation.All OrElse
                (interaction.Base.Activation = TargetActivation.All AndAlso
                interaction.Base.TargetNames.Count = freeTargets.Distinct(PonyDirectoryEqualityComparer.Instance).Count()) Then
                If eligableInteractions Is Nothing Then eligableInteractions = New List(Of EligableInteraction)()
                eligableInteractions.Add(
                    New EligableInteraction(interaction,
                                            If(interaction.Base.Activation = TargetActivation.One, freeTargetsInRange, freeTargets)))
            End If
        Next
        If eligableInteractions IsNot Nothing Then
            StartInteraction(eligableInteractions(Rng.Next(eligableInteractions.Count)))
        End If
    End Sub

    ''' <summary>
    ''' Starts the specified interaction. This sets the current interaction and effects a behavior transition in the current pony and
    ''' affected targets. This pony is noted as the initiator on the interaction. If the interaction specifies one target, one is activated
    ''' at random. Otherwise, a single target is uniformly selected to satisfy each target name, and all these are activated.
    ''' StartInteractionAsTarget() is called on the selected targets.
    ''' </summary>
    ''' <param name="eligableInteraction">The eligible interaction to start. For interactions activating one pony, one is uniformly
    ''' selected at random from the targets. Otherwise the targets are grouped by target name and one if uniformly activated at random from
    ''' each group.</param>
    Private Sub StartInteraction(eligableInteraction As EligableInteraction)
        _currentInteraction = eligableInteraction.Interaction
        _currentInteraction.Initiator = Me
        SetBehavior(_currentInteraction.Behaviors(Rng.Next(_currentInteraction.Behaviors.Count)))

        Dim targets = eligableInteraction.Targets
        If _currentInteraction.Base.Activation = TargetActivation.One Then
            targets(Rng.Next(targets.Length)).StartInteractionAsTarget(_currentBehavior.Name, _currentInteraction)
        Else
            ' If we have multiple instances of a single target name, we need to choose one at random as we go.
            Dim uniqueTargets = targets.GroupBy(Function(p) p.Base.Directory).Select(
                Function(g)
                    Dim duplicateTargets = g.ToImmutableArray()
                    Return duplicateTargets(Rng.Next(duplicateTargets.Length))
                End Function)
            For Each targetPony In uniqueTargets
                targetPony.StartInteractionAsTarget(_currentBehavior.Name, _currentInteraction)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Starts the specified interaction as a target. This sets the current interaction and effects a behavior transition. This pony will
    ''' be added to the involved targets of the interaction.
    ''' </summary>
    ''' <param name="behaviorName">The name of the behavior to run (which references the same behavior the initiator chose).</param>
    ''' <param name="interaction">The interaction which becomes the current interaction, and in which this pony becomes and involved
    ''' target.</param>
    Private Sub StartInteractionAsTarget(behaviorName As CaseInsensitiveString, interaction As Interaction(Of Pony2))
        _currentInteraction = interaction
        _currentInteraction.InvolvedTargets.Add(Me)
        SetBehavior(Base.Behaviors.First(Function(b) b.Name = behaviorName))
    End Sub

    ''' <summary>
    ''' Ends the current interaction. If this pony is the initiator, calls EndInteraction() on all target ponies still running the
    ''' interaction and removes itself as the interaction initiator. If this pony is a target, removes itself from the involved targets of
    ''' the interaction. The current interaction and interaction cool down will be set.
    ''' </summary>
    Private Sub EndInteraction()
        If ReferenceEquals(Me, _currentInteraction.Initiator) Then
            For Each pony In _currentInteraction.Targets
                ' Check the target is still running the same interaction.
                If ReferenceEquals(_currentInteraction, pony._currentInteraction) Then
                    pony.EndInteraction()
                End If
            Next
            _currentInteraction.Initiator = Nothing
        Else
            _currentInteraction.InvolvedTargets.Remove(Me)
        End If

        '_interactionCooldownEndTime = _currentTime + _currentInteraction.Base.ReactivationDelay
        _interactionCooldownEndTime = _currentTime + TimeSpan.FromSeconds(5)
        _currentInteraction = Nothing
    End Sub

    ''' <summary>
    ''' Gets the non-aligned region the pony would occupy for the specified image, scaled by the context scale factor.
    ''' </summary>
    ''' <param name="image">The image which defines a size and center which determines the region around the current location.</param>
    ''' <returns>A region where the current location of the pony and image center coincide, whose size is that of the image scaled by the
    ''' context scale factor.</returns>
    Private Function GetRegionFForImage(image As SpriteImage) As RectangleF
        Return New RectangleF(_location - image.Center * Context.ScaleFactor, image.Size * Context.ScaleFactor)
    End Function

    ''' <summary>
    ''' Marks the pony as expired, and expires any effects belonging to this pony.
    ''' </summary>
    Public Sub Expire() Implements IExpireableSprite.Expire
        If _expired Then Return
        _expired = True
        If _activeEffects.Count > 0 Then
            For Each effect In _activeEffects.ToImmutableArray()
                effect.Expire()
            Next
        End If
        RaiseEvent Expired(Me, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' Returns a string that represents the current pony.
    ''' </summary>
    ''' <returns>A string that represents the current pony.</returns>
    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Base.Directory: " & Base.Directory
    End Function
End Class

'Public Class Pony
'    Inherits Pony2
'    'You can place effects at an offset to the pony, and also set them to the left or the right of themselves for big effects.
'    Friend Shared Function GetEffectLocation(effectImageSize As Size, dir As Direction,
'                                             parentTopLeftLocation As Vector2F, parentSize As Vector2,
'                                             centering As Direction, scale As Single) As Vector2

'        Dim scaledParentSize = parentSize * CSng(scale)
'        scaledParentSize.X *= DirectionWeightHorizontal(dir)
'        scaledParentSize.Y *= DirectionWeightVertical(dir)

'        Dim locationOnParent = parentTopLeftLocation + scaledParentSize

'        Dim scaledEffectSize = New Vector2F(effectImageSize) * Options.ScaleFactor
'        scaledEffectSize.X *= DirectionWeightHorizontal(centering)
'        scaledEffectSize.Y *= DirectionWeightVertical(centering)

'        Return Vector2.Round(locationOnParent - scaledEffectSize)
'    End Function
'    Private Shared Function DirectionWeightHorizontal(dir As Direction) As Single
'        Select Case dir
'            Case Direction.TopLeft, Direction.MiddleLeft, Direction.BottomLeft
'                Return 0
'            Case Direction.TopCenter, Direction.MiddleCenter, Direction.BottomCenter
'                Return 0.5
'            Case Direction.TopRight, Direction.MiddleRight, Direction.BottomRight
'                Return 1
'            Case Direction.Random, Direction.RandomNotCenter
'                Return CSng(Rng.NextDouble())
'        End Select
'        Return Single.NaN
'    End Function
'    Private Shared Function DirectionWeightVertical(dir As Direction) As Single
'        Select Case dir
'            Case Direction.TopLeft, Direction.TopCenter, Direction.TopRight
'                Return 0
'            Case Direction.MiddleLeft, Direction.MiddleCenter, Direction.MiddleRight
'                Return 0.5
'            Case Direction.BottomLeft, Direction.BottomCenter, Direction.BottomRight
'                Return 1
'            Case Direction.Random, Direction.RandomNotCenter
'                Return CSng(Rng.NextDouble())
'        End Select
'        Return Single.NaN
'    End Function
'    Friend Function GetBehaviorGroupName(groupnumber As Integer) As String
'        If groupnumber = 0 Then
'            Return "Any"
'        End If

'        For Each group In Base.BehaviorGroups
'            If group.Number = groupnumber Then
'                Return group.Name
'            End If
'        Next

'        Return "Unnamed"
'    End Function

'    Public Sub New(base As PonyBase)
'        ' TODO: Fix/remove this ctor.
'        MyBase.New(New PonyContext(), base)
'    End Sub
'    Public Sub New(context As PonyContext, base As PonyBase)
'        MyBase.New(context, base)
'    End Sub
'    Public ReadOnly Property DisplayName As String
'        Get
'            Return Base.DisplayName
'        End Get
'    End Property
'    Public ReadOnly Property Directory As String
'        Get
'            Return Base.Directory
'        End Get
'    End Property
'    Public ReadOnly Property Behaviors As List(Of Behavior)
'        Get
'            Return Base.Behaviors
'        End Get
'    End Property
'    Public ReadOnly Property Scale As Single
'        Get
'            Return 1
'        End Get
'    End Property
'    Public ReadOnly Property CurrentBehaviorGroup As Integer
'        Get
'            Return If(CurrentBehavior Is Nothing, 0, CurrentBehavior.Group)
'        End Get
'    End Property
'    Public Property CurrentBehavior As Behavior
'        Get
'            Return _currentBehavior
'        End Get
'        Set(value As Behavior)
'            SelectBehavior(value)
'        End Set
'    End Property
'    Public Sub SelectBehavior(Optional suggested As Behavior = Nothing)
'        SetBehavior(suggested)
'    End Sub

'    ' TODO: Implement these.
'    Public Property ManualControlPlayerOne As Boolean
'    Public Property ManualControlPlayerTwo As Boolean
'End Class
#End Region

#Region "EffectBase class"
Public Class EffectBase
    Implements IPonyIniSourceable, IReferential

    Public Property Name As CaseInsensitiveString Implements IPonyIniSerializable.Name
    Public Property BehaviorName As CaseInsensitiveString
    Public Property ParentPonyBase As PonyBase
    Private _leftImage As New SpriteImage()
    Private _rightImage As New SpriteImage()
    Public ReadOnly Property LeftImage As SpriteImage
        Get
            Return _leftImage
        End Get
    End Property
    Public ReadOnly Property RightImage As SpriteImage
        Get
            Return _rightImage
        End Get
    End Property
    Public Property Duration As Double
    Public Property RepeatDelay As Double

    Public Property PlacementDirectionRight As Direction
    Public Property CenteringRight As Direction
    Public Property PlacementDirectionLeft As Direction
    Public Property CenteringLeft As Direction

    Public Property Follow As Boolean
    Public Property DoNotRepeatImageAnimations As Boolean

    Public Shared Function TryLoad(iniLine As String, imageDirectory As String, pony As PonyBase,
                                   ByRef result As EffectBase, ByRef issues As ImmutableArray(Of ParseIssue)) As ParseResult
        result = Nothing
        issues = Nothing

        Dim e = New EffectBase(pony)
        e.SourceIni = iniLine
        Dim p As New StringCollectionParser(CommaSplitQuoteQualified(iniLine),
                                            {"Identifier", "Effect Name", "Behavior Name",
                                             "Right Image", "Left Image", "Duration", "Repeat Delay",
                                             "Placement Right", "Centering Right",
                                             "Placement Left", "Centering Left",
                                             "Follow", "Prevent Repeat"})
        p.NoParse()
        e.Name = If(p.NotNullOrWhiteSpace(), "")
        e.BehaviorName = If(p.NotNullOrWhiteSpace(), "")
        e.RightImage.Path = p.NoParse()
        If p.Assert(e.RightImage.Path, Function(s) Not String.IsNullOrEmpty(s), "An image path has not been set.", Nothing) Then
            e.RightImage.Path = p.SpecifiedCombinePath(imageDirectory, e.RightImage.Path, "Image will not be loaded.")
            p.SpecifiedFileExists(e.RightImage.Path)
        End If
        e.LeftImage.Path = p.NoParse()
        If p.Assert(e.LeftImage.Path, Function(s) Not String.IsNullOrEmpty(s), "An image path has not been set.", Nothing) Then
            e.LeftImage.Path = p.SpecifiedCombinePath(imageDirectory, e.LeftImage.Path, "Image will not be loaded.")
            p.SpecifiedFileExists(e.LeftImage.Path)
        End If
        e.Duration = p.ParseDouble(5, 0, 300)
        e.RepeatDelay = p.ParseDouble(0, 0, 300)
        e.PlacementDirectionRight = p.Map(DirectionFromIni, Direction.Random)
        e.CenteringRight = p.Map(DirectionFromIni, Direction.Random)
        e.PlacementDirectionLeft = p.Map(DirectionFromIni, Direction.Random)
        e.CenteringLeft = p.Map(DirectionFromIni, Direction.Random)
        e.Follow = p.ParseBoolean(False)
        e.DoNotRepeatImageAnimations = p.ParseBoolean(False)

        issues = p.Issues.ToImmutableArray()
        result = e
        Return p.Result
    End Function

    Protected Sub New()
    End Sub

    Public Sub New(pony As PonyBase)
        Me.ParentPonyBase = Argument.EnsureNotNull(pony, "pony")
    End Sub

    Public Sub New(_name As String, leftImagePath As String, rightImagePath As String)
        Name = _name
        LeftImage.Path = leftImagePath
        RightImage.Path = rightImagePath
    End Sub

    Public Function GetPonyIni() As String Implements IPonyIniSerializable.GetPonyIni
        Return String.Join(
            ",", "Effect",
            Quoted(Name),
            Quoted(BehaviorName),
            Quoted(Path.GetFileName(RightImage.Path)),
            Quoted(Path.GetFileName(LeftImage.Path)),
            Duration.ToString(CultureInfo.InvariantCulture),
            RepeatDelay.ToString(CultureInfo.InvariantCulture),
            Space_To_Under(Location_ToString(PlacementDirectionRight)),
            Space_To_Under(Location_ToString(CenteringRight)),
            Space_To_Under(Location_ToString(PlacementDirectionLeft)),
            Space_To_Under(Location_ToString(CenteringLeft)),
            Follow,
            DoNotRepeatImageAnimations)
    End Function

    Public Function Clone() As IPonyIniSourceable Implements IPonyIniSourceable.Clone
        Dim copy = DirectCast(MyBase.MemberwiseClone(), EffectBase)
        copy._leftImage = New CenterableSpriteImage()
        copy._leftImage.Path = _leftImage.Path
        copy._rightImage = New CenterableSpriteImage()
        copy._rightImage.Path = _rightImage.Path
        Return copy
    End Function

    Public Function GetReferentialIssues(ponies As PonyCollection) As ImmutableArray(Of ParseIssue) Implements IReferential.GetReferentialIssues
        Return {Referential.CheckUnique("Behavior", BehaviorName, ParentPonyBase.Behaviors.Select(Function(b) b.Name))}.
            Where(Function(pi) pi.PropertyName IsNot Nothing).ToImmutableArray()
    End Function

    Public Property SourceIni As String Implements IPonyIniSourceable.SourceIni

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Name: " & Name
    End Function
End Class
#End Region

#Region "Effect class"
Public Class Effect
    Implements IDraggableSprite, IExpireableSprite
    Private Shared ReadOnly DirectionCount As Integer = [Enum].GetValues(GetType(Direction)).Length

    Private _base As EffectBase
    Public ReadOnly Property Base As EffectBase
        Get
            Return _base
        End Get
    End Property

    Private startTime As TimeSpan
    Private internalTime As TimeSpan

    Public Property DesiredDuration As TimeSpan?
    Private _expired As Boolean

    Public Property TopLeftLocation As Point
    Public Property FacingLeft As Boolean
    Public Property BeingDragged As Boolean Implements IDraggableSprite.Drag
    Public Property PlacementDirection As Direction
    Public Property Centering As Direction

    Private ReadOnly locationProvider As Func(Of Vector2F)
    Private ReadOnly sizeProvider As Func(Of Vector2)
    Private ReadOnly scaleProvider As Func(Of Single)

    Public Sub New(base As EffectBase, startFacingLeft As Boolean,
                   locationProvider As Func(Of Vector2F), sizeProvider As Func(Of Vector2), scaleProvider As Func(Of Single))
        Argument.EnsureNotNull(base, "base")
        _base = base
        FacingLeft = startFacingLeft

        If PlacementDirection = Direction.RandomNotCenter Then
            PlacementDirection = CType(Math.Round(Rng.NextDouble() * DirectionCount - 3, 0), Direction)
            If PlacementDirection = Direction.MiddleCenter Then PlacementDirection = Direction.BottomRight
        ElseIf PlacementDirection = Direction.Random Then
            PlacementDirection = CType(Math.Round(Rng.NextDouble() * DirectionCount - 2, 0), Direction)
        Else
            PlacementDirection = If(FacingLeft, base.PlacementDirectionLeft, base.PlacementDirectionRight)
        End If

        If Centering = Direction.RandomNotCenter Then
            Centering = CType(Math.Round(Rng.NextDouble() * DirectionCount - 3, 0), Direction)
            If Centering = Direction.MiddleCenter Then Centering = Direction.BottomRight
        ElseIf Centering = Direction.Random Then
            Centering = CType(Math.Round(Rng.NextDouble() * DirectionCount - 2, 0), Direction)
        Else
            Centering = If(FacingLeft, base.CenteringLeft, base.CenteringRight)
        End If

        Me.locationProvider = locationProvider
        Me.sizeProvider = sizeProvider
        Me.scaleProvider = scaleProvider
    End Sub

    Public Sub Teleport()
        Dim screen = Options.Screens(Rng.Next(Options.Screens.Count))
        TopLeftLocation = New Point(
            CInt(screen.WorkingArea.X + Math.Round(Rng.NextDouble() * (screen.WorkingArea.Width - CurrentImageSize.Width), 0)),
            CInt(screen.WorkingArea.Y + Math.Round(Rng.NextDouble() * (screen.WorkingArea.Height - CurrentImageSize.Height), 0)))
    End Sub

    Public Function Center() As Point
        Dim scale = If(scaleProvider Is Nothing, 1, scaleProvider())
        Return New Point(CInt(Me.TopLeftLocation.X + ((scale * CurrentImageSize.Width) / 2)),
                         CInt(Me.TopLeftLocation.Y + ((scale * CurrentImageSize.Height) / 2)))
    End Function

    Public ReadOnly Property CurrentImagePath() As String
        Get
            Return If(FacingLeft, Base.LeftImage.Path, Base.RightImage.Path)
        End Get
    End Property

    Public ReadOnly Property CurrentImageSize() As Size
        Get
            Return If(FacingLeft, Base.LeftImage.Size, Base.RightImage.Size)
        End Get
    End Property

    Public Sub Start(_startTime As TimeSpan) Implements ISprite.Start
        startTime = _startTime
        internalTime = startTime
        If locationProvider IsNot Nothing AndAlso sizeProvider IsNot Nothing AndAlso scaleProvider IsNot Nothing Then
            TopLeftLocation = Pony.GetEffectLocation(CurrentImageSize,
                                              PlacementDirection,
                                              locationProvider(),
                                              sizeProvider(),
                                              Centering,
                                              scaleProvider())
        End If
    End Sub

    Public Sub Update(updateTime As TimeSpan) Implements ISprite.Update
        internalTime = updateTime
        If _expired Then Return
        If Base.Follow Then
            TopLeftLocation = Pony.GetEffectLocation(CurrentImageSize,
                                              PlacementDirection,
                                              locationProvider(),
                                              sizeProvider(),
                                              Centering,
                                              scaleProvider())
        ElseIf BeingDragged Then
            TopLeftLocation = EvilGlobals.CursorLocation - New Size(CInt(CurrentImageSize.Width / 2), CInt(CurrentImageSize.Height / 2))
        End If
        If DesiredDuration IsNot Nothing AndAlso ImageTimeIndex > DesiredDuration.Value Then
            Expire()
        End If
    End Sub

    Public ReadOnly Property ImageTimeIndex As TimeSpan Implements ISprite.ImageTimeIndex
        Get
            Return internalTime - startTime
        End Get
    End Property

    Public ReadOnly Property FlipImage As Boolean Implements ISprite.FlipImage
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property ImagePath As String Implements ISprite.ImagePath
        Get
            Return CurrentImagePath()
        End Get
    End Property

    Public ReadOnly Property Region As System.Drawing.Rectangle Implements ISprite.Region
        Get
            Dim width = CInt(CurrentImageSize.Width * Options.ScaleFactor)
            Dim height = CInt(CurrentImageSize.Height * Options.ScaleFactor)
            Return New Rectangle(TopLeftLocation, New Size(width, height))
        End Get
    End Property

    Public Sub Expire() Implements IExpireableSprite.Expire
        If _expired Then Return
        _expired = True
        RaiseEvent Expired(Me, EventArgs.Empty)
    End Sub

    Public Event Expired As EventHandler Implements IExpireableSprite.Expired

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Base.Name: " & Base.Name
    End Function
End Class
#End Region

#Region "HouseBase class"
Public Class HouseBase
    Inherits EffectBase
    Public Const RootDirectory = "Houses"
    Public Const ConfigFilename = "house.ini"

    ' TODO: Move this somewhere sensible.
    Friend OptionsForm As HouseOptionsForm

    Private ReadOnly _directory As String
    Public ReadOnly Property Directory() As String
        Get
            Return _directory
        End Get
    End Property

    Private _doorPosition As Point
    Public Property DoorPosition() As Point
        Get
            Return _doorPosition
        End Get
        Set(value As Point)
            _doorPosition = value
        End Set
    End Property

    Private _cycleInterval As TimeSpan = TimeSpan.FromMinutes(5)
    Public Property CycleInterval() As TimeSpan
        Get
            Return _cycleInterval
        End Get
        Set(value As TimeSpan)
            _cycleInterval = value
        End Set
    End Property

    Private _minimumPonies As Integer = 1
    Public Property MinimumPonies() As Integer
        Get
            Return _minimumPonies
        End Get
        Set(value As Integer)
            _minimumPonies = value
        End Set
    End Property

    Private _maximumPonies As Integer = 50
    Public Property MaximumPonies() As Integer
        Get
            Return _maximumPonies
        End Get
        Set(value As Integer)
            _maximumPonies = value
        End Set
    End Property

    Private _bias As Decimal = 0.5D
    Public Property Bias() As Decimal
        Get
            Return _bias
        End Get
        Set(value As Decimal)
            _bias = value
        End Set
    End Property

    Private ReadOnly _visitors As New List(Of String)
    Public ReadOnly Property Visitors() As List(Of String)
        Get
            Return _visitors
        End Get
    End Property

    Public Sub New(directory As String)
        Argument.EnsureNotNull(directory, "directory")

        Dim lastSeparator = directory.LastIndexOf(Path.DirectorySeparatorChar)
        If lastSeparator <> -1 Then
            _directory = directory.Substring(lastSeparator + 1)
        Else
            _directory = directory
        End If

        LoadFromIni()
    End Sub

    Private Sub LoadFromIni()
        Dim fullDirectory = Path.Combine(EvilGlobals.InstallLocation, RootDirectory, Directory)
        Dim imageFilename As String = Nothing
        Using configFile = File.OpenText(Path.Combine(fullDirectory, ConfigFilename))
            Do Until configFile.EndOfStream

                Dim line = configFile.ReadLine

                'ignore blank or 'commented out' lines.
                If line = "" OrElse line(0) = "'" Then
                    Continue Do
                End If

                Dim columns = CommaSplitQuoteBraceQualified(line)

                Select Case LCase(columns(0))

                    Case "name"
                        Name = columns(1)
                    Case "image"
                        imageFilename = Path.Combine(fullDirectory, columns(1))
                        If Not File.Exists(imageFilename) Then
                            Throw New FileNotFoundException(imageFilename, imageFilename)
                        Else
                            LeftImage.Path = imageFilename
                            RightImage.Path = imageFilename
                        End If
                    Case "door"
                        DoorPosition = New Point(Integer.Parse(columns(1), CultureInfo.InvariantCulture),
                                         Integer.Parse(columns(2), CultureInfo.InvariantCulture))
                    Case "cycletime"
                        CycleInterval = TimeSpan.FromSeconds(Integer.Parse(columns(1), CultureInfo.InvariantCulture))
                    Case "minspawn"
                        MinimumPonies = Integer.Parse(columns(1), CultureInfo.InvariantCulture)
                    Case "maxspawn"
                        MaximumPonies = Integer.Parse(columns(1), CultureInfo.InvariantCulture)
                    Case "bias"
                        Bias = Decimal.Parse(columns(1), CultureInfo.InvariantCulture)
                    Case Else
                        Visitors.Add(Trim(line))
                End Select
            Loop

            If String.IsNullOrEmpty(Name) OrElse String.IsNullOrEmpty(imageFilename) OrElse
                Visitors.Count = 0 Then
                Throw New InvalidDataException("Unable to load 'House' at: " & fullDirectory &
                                               ".INI file does not contain all necessary parameters: " & ControlChars.NewLine &
                                               "name, image, and at least one pony's name")
            End If
        End Using
    End Sub

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Name: " & Name
    End Function
End Class
#End Region

#Region "House class"
Public Class House
    Inherits Effect

    Private ReadOnly deployedPonies As New List(Of Pony)()

    Private lastCycleTime As TimeSpan

    Private _houseBase As HouseBase
    Public ReadOnly Property HouseBase() As HouseBase
        Get
            Return _houseBase
        End Get
    End Property

    Public Sub New(houseBase As HouseBase)
        MyBase.New(houseBase, True, Nothing, Nothing, Nothing)
        _houseBase = houseBase
    End Sub

    Public Sub InitializeVisitorList()
        deployedPonies.Clear()
        For Each Pony As Pony In EvilGlobals.CurrentAnimator.Ponies()
            SyncLock HouseBase.Visitors
                For Each guest In HouseBase.Visitors
                    If Pony.Directory = guest Then
                        deployedPonies.Add(Pony)
                        Exit For
                    End If
                Next
            End SyncLock
        Next
    End Sub

    ''' <summary>
    ''' Checks to see if it is time to deploy/recall a pony and does so. 
    ''' </summary>
    ''' <param name="currentTime">The current time.</param>
    Public Sub Cycle(currentTime As TimeSpan, ponyBases As IEnumerable(Of PonyBase))

        If currentTime - lastCycleTime > HouseBase.CycleInterval Then
            lastCycleTime = currentTime

            Console.WriteLine(Me.Base.Name & " - Cycling. Deployed ponies: " & deployedPonies.Count)

            If Rng.NextDouble() < 0.5 Then
                'skip this round
                Console.WriteLine(Me.Base.Name & " - Decided to skip this round of cycling.")
                Exit Sub
            End If

            If Rng.NextDouble() < HouseBase.Bias Then
                If deployedPonies.Count < HouseBase.MaximumPonies AndAlso EvilGlobals.CurrentAnimator.Ponies().Count < Options.MaxPonyCount Then
                    DeployPony(Me, ponyBases)
                Else
                    Console.WriteLine(Me.Base.Name & " - Cannot deploy. Pony limit reached.")
                End If
            Else
                If deployedPonies.Count > HouseBase.MinimumPonies AndAlso EvilGlobals.CurrentAnimator.Ponies().Count > 1 Then
                    RecallPony(Me)
                Else
                    Console.WriteLine(Me.Base.Name & " - Cannot recall. Too few ponies deployed.")
                End If
            End If

        End If

    End Sub

    Private Sub DeployPony(instance As Effect, ponyBases As IEnumerable(Of PonyBase))

        Dim choices As New List(Of String)

        Dim all As Boolean = False
        SyncLock HouseBase.Visitors
            For Each visitor In HouseBase.Visitors
                If String.Equals("all", visitor, StringComparison.OrdinalIgnoreCase) Then
                    For Each ponyBase In ponyBases
                        choices.Add(ponyBase.Directory)
                    Next
                    all = True
                    Exit For
                End If
            Next

            If all = False Then
                For Each ponyName In HouseBase.Visitors
                    choices.Add(ponyName)
                Next
            End If
        End SyncLock

        For Each p In EvilGlobals.CurrentAnimator.Ponies()
            choices.Remove(p.Directory)
        Next

        choices.Remove(PonyBase.RandomDirectory)

        If choices.Count = 0 Then
            Exit Sub
        End If

        Dim selected_name = choices(Rng.Next(choices.Count))

        For Each ponyBase In ponyBases
            If ponyBase.Directory = selected_name Then

                Dim deployed_pony = New Pony(ponyBase)

                deployed_pony.Location = instance.TopLeftLocation + New Size(HouseBase.DoorPosition)

                EvilGlobals.CurrentAnimator.AddPony(deployed_pony)
                deployedPonies.Add(deployed_pony)

                Console.WriteLine(Me.Base.Name & " - Deployed " & ponyBase.Directory)

                Exit Sub
            End If
        Next

    End Sub

    Private Sub RecallPony(instance As Effect)

        Dim choices As New List(Of String)

        Dim all As Boolean = False
        SyncLock HouseBase.Visitors
            For Each visitor In HouseBase.Visitors
                If String.Equals("all", visitor, StringComparison.OrdinalIgnoreCase) Then
                    For Each Pony As Pony In EvilGlobals.CurrentAnimator.Ponies()
                        choices.Add(Pony.Directory)
                    Next
                    all = True
                    Exit For
                End If
            Next

            If all = False Then
                For Each Pony As Pony In EvilGlobals.CurrentAnimator.Ponies()
                    For Each otherpony In HouseBase.Visitors
                        If Pony.Directory = otherpony Then
                            choices.Add(Pony.Directory)
                            Exit For
                        End If
                    Next
                Next
            End If
        End SyncLock

        If choices.Count = 0 Then Exit Sub

        Dim selected_name = choices(Rng.Next(choices.Count))

        For Each pony As Pony In EvilGlobals.CurrentAnimator.Ponies()
            If pony.Directory = selected_name Then

                If pony.IsInteracting Then Exit Sub
                If pony.BeingDragged Then Exit Sub

                If pony.Sleeping Then pony.WakeUp()

                pony.Destination = instance.TopLeftLocation + New Size(HouseBase.DoorPosition)
                pony.GoingHome = True
                pony.CurrentBehavior = pony.GetAppropriateBehaviorOrFallback(AllowedMoves.All, False)
                pony.BehaviorDesiredDuration = TimeSpan.FromMinutes(5)

                deployedPonies.Remove(pony)

                Console.WriteLine(Me.Base.Name & " - Recalled " & pony.Directory)

                Exit Sub
            End If
        Next

    End Sub

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Base.Name: " & Base.Name
    End Function
End Class
#End Region

#Region "SpriteImage classes"
Public Class SpriteImage
    Private _path As String
    Public Property Path As String
        Get
            Return _path
        End Get
        Set(value As String)
            _path = value
            _size = Nothing
        End Set
    End Property
    Public Overridable ReadOnly Property Center As Vector2
        Get
            Return Size / 2
        End Get
    End Property
    Private _size As Vector2?
    Public ReadOnly Property Size As Vector2
        Get
            If String.IsNullOrWhiteSpace(Path) Then Return Vector2.Zero
            If _size Is Nothing Then
                _size = Vector2.Zero
                Try
                    _size = ImageSize.GetSize(Path)
                Catch ex As ArgumentException
                    ' Leave size empty by default.
                Catch ex As IOException
                    ' Leave size empty by default.
                Catch ex As UnauthorizedAccessException
                    ' Leave size empty by default.
                End Try
            End If
            Return _size.Value
        End Get
    End Property
    Public Overrides Function ToString() As String
        Return MyBase.ToString() & ", Path: " & If(Path, "")
    End Function
End Class

Public Class CenterableSpriteImage
    Inherits SpriteImage
    Public Property CustomCenter As Vector2?
    Public Overrides ReadOnly Property Center As Vector2
        Get
            If CustomCenter IsNot Nothing Then
                Return CustomCenter.Value
            Else
                Return MyBase.Center
            End If
        End Get
    End Property
End Class
#End Region

#Region "Enums"
Public Enum Direction
    TopLeft = 0
    TopCenter = 1
    TopRight = 2
    MiddleLeft = 3
    MiddleCenter = 4
    MiddleRight = 5
    BottomLeft = 6
    BottomCenter = 8
    BottomRight = 9
    Random = 10
    RandomNotCenter = 11
End Enum

<Flags()>
Public Enum AllowedMoves As Byte
    None = 0
    HorizontalOnly = 1
    VerticalOnly = 2
    DiagonalOnly = 4
    HorizontalVertical = HorizontalOnly Or VerticalOnly
    DiagonalHorizontal = DiagonalOnly Or HorizontalOnly
    DiagonalVertical = DiagonalOnly Or VerticalOnly
    All = HorizontalOnly Or VerticalOnly Or DiagonalOnly
    MouseOver = 8
    Sleep = 16
    Dragged = 32
End Enum

Public Enum BehaviorOption
    Name = 1
    Probability = 2
    MaxDuration = 3
    MinDuration = 4
    Speed = 5 'specified in pixels per tick of the timer
    RightImagePath = 6
    LeftImagePath = 7
    MovementType = 8
    LinkedBehavior = 9
    SpeakingStart = 10
    SpeakingEnd = 11
    Skip = 12 'Should we skip this behavior when considering ones to randomly choose (part of an interaction/chain?)
    XCoord = 13  'used when following/moving to a point on the screen.
    YCoord = 14
    ObjectToFollow = 15
    AutoSelectImages = 16
    FollowStoppedBehavior = 17
    FollowMovingBehavior = 18
    RightImageCenter = 19
    LeftImageCenter = 20
    DoNotRepeatImageAnimations = 21
    Group = 22
End Enum

''' <summary>
''' Specifies how the interaction is activated when dealing with multiple targets.
''' </summary>
Public Enum TargetActivation
    ''' <summary>
    ''' Only one target from the list participates in the interaction.
    ''' </summary>
    One
    ''' <summary>
    ''' Any available targets participate in the interaction, even if some are not present.
    ''' </summary>
    Any
    ''' <summary>
    ''' All targets must participate in the interaction, the interaction cannot proceed unless all targets are present.
    ''' </summary>
    All
End Enum

Public Module EnumConversions
    Public Function AllowedMovesFromString(movement As String) As AllowedMoves
        Select Case movement
            Case "None"
                Return AllowedMoves.None
            Case "Horizontal Only"
                Return AllowedMoves.HorizontalOnly
            Case "Vertical Only"
                Return AllowedMoves.VerticalOnly
            Case "Horizontal/Vertical"
                Return AllowedMoves.HorizontalVertical
            Case "Diagonal Only"
                Return AllowedMoves.DiagonalOnly
            Case "Diagonal/horizontal"
                Return AllowedMoves.DiagonalHorizontal
            Case "Diagonal/Vertical"
                Return AllowedMoves.DiagonalVertical
            Case "All"
                Return AllowedMoves.All
            Case "MouseOver"
                Return AllowedMoves.MouseOver
            Case "Sleep"
                Return AllowedMoves.Sleep
            Case "Dragged"
                Return AllowedMoves.Dragged
            Case Else
                Throw New ArgumentException("Invalid movement string:" & movement, "movement")
        End Select
    End Function

    Public Function DirectionFromString(location As String) As Direction
        Select Case location
            Case "Top"
                Return Direction.TopCenter
            Case "Bottom"
                Return Direction.BottomCenter
            Case "Left"
                Return Direction.MiddleLeft
            Case "Right"
                Return Direction.MiddleRight
            Case "Bottom Right"
                Return Direction.BottomRight
            Case "Bottom Left"
                Return Direction.BottomLeft
            Case "Top Right"
                Return Direction.TopRight
            Case "Top Left"
                Return Direction.TopLeft
            Case "Center"
                Return Direction.MiddleCenter
            Case "Any"
                Return Direction.Random
            Case "Any-Not Center"
                Return Direction.RandomNotCenter
            Case Else
                Throw New ArgumentException("Invalid Location/Direction option: " & location, "location")
        End Select
    End Function

    Public Function AllowedMovesToString(movement As AllowedMoves) As String
        Select Case movement
            Case AllowedMoves.None
                Return "None"
            Case AllowedMoves.HorizontalOnly
                Return "Horizontal Only"
            Case AllowedMoves.VerticalOnly
                Return "Vertical Only"
            Case AllowedMoves.HorizontalVertical
                Return "Horizontal/Vertical"
            Case AllowedMoves.DiagonalOnly
                Return "Diagonal Only"
            Case AllowedMoves.DiagonalHorizontal
                Return "Diagonal/horizontal"
            Case AllowedMoves.DiagonalVertical
                Return "Diagonal/Vertical"
            Case AllowedMoves.All
                Return "All"
            Case AllowedMoves.MouseOver
                Return "MouseOver"
            Case AllowedMoves.Sleep
                Return "Sleep"
            Case AllowedMoves.Dragged
                Return "Dragged"
            Case Else
                Throw New ArgumentException("Invalid movement option: " & movement, "movement")
        End Select
    End Function

    Public Function TargetActivationFromString(activation As String) As TargetActivation
        Dim targetsOut As TargetActivation
        If Not [Enum].TryParse(activation, targetsOut) Then
            ' If direct parsing failed, assume we've got some old definitions instead.
            ' The old code accepted the following values irrespective of case.
            ' It should be noted that title-cased "All" will be recognized as a new value which has stricter semantics.
            ' However, the old code used to serialize a Boolean value and thus wrote "True" or "False". The chances of
            ' encountering "random" or "all" in practice are therefore almost nil, as they would have to be manually
            ' edited in.
            If String.Equals(activation, "False", StringComparison.OrdinalIgnoreCase) OrElse
                String.Equals(activation, "random", StringComparison.OrdinalIgnoreCase) Then
                targetsOut = TargetActivation.One
            ElseIf String.Equals(activation, "True", StringComparison.OrdinalIgnoreCase) OrElse
                String.Equals(activation, "all", StringComparison.OrdinalIgnoreCase) Then
                targetsOut = TargetActivation.Any
            Else
                Throw New ArgumentException("Invalid value. Valid values are: " &
                                            String.Join(", ", [Enum].GetNames(GetType(TargetActivation))), "activation")
            End If
        End If
        Return targetsOut
    End Function
End Module
#End Region
