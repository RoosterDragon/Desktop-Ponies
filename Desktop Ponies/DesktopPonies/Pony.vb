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
    Public Shared Function GetIssue(propertyName As String, name As String, collection As IEnumerable(Of IEquatable(Of String))) As ParseIssue
        If String.IsNullOrEmpty(name) Then Return Nothing
        Dim result = CheckReference(name, collection)
        If CheckReference(name, collection) <> ReferenceResult.Ok Then
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
            If candidateName.Equals(name) Then count += 1
            If count >= 2 Then Return ReferenceResult.NotUnique
        Next
        Return If(count = 0, ReferenceResult.NotFound, ReferenceResult.Ok)
    End Function
    Private Enum ReferenceResult
        Ok
        NotFound
        NotUnique
    End Enum
End Class
#End Region

#Region "PonyBase class"
Public Class PonyBase
    Public Const RootDirectory = "Ponies"
    Public Const ConfigFilename = "pony.ini"

    Private ReadOnly _collection As PonyCollection
    Public ReadOnly Property Collection As PonyCollection
        Get
            Return _collection
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
    Public ReadOnly Property SpeechesRandom() As IEnumerable(Of Speech)
        Get
            Return Speeches.Where(Function(s) Not s.Skip)
        End Get
    End Property
    Public ReadOnly Property SpeechesSpecific() As IEnumerable(Of Speech)
        Get
            Return Speeches.Where(Function(s) s.Skip)
        End Get
    End Property

    Private Sub New(collection As PonyCollection)
        _collection = Argument.EnsureNotNull(collection, "collection")
    End Sub

    Public Function ChangeDirectory(newDirectory As String) As Boolean
        If Directory Is Nothing Then Return Create(newDirectory)
        If String.Equals(Directory, newDirectory, PathEquality.Comparison) Then Return True
        Try
            Dim currentPath = Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, Directory)
            Dim newPath = Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, newDirectory)
            IO.Directory.Move(currentPath, newPath)
            Collection.ChangePonyDirectory(Directory, newDirectory)
            _directory = newDirectory
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function CreateInMemory(collection As PonyCollection) As PonyBase
        Return New PonyBase(collection)
    End Function

    Public Shared Function Create(directory As String) As Boolean
        Try
            Dim fullPath = Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, directory)
            If IO.Directory.Exists(fullPath) Then Return False
            IO.Directory.CreateDirectory(fullPath)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function Load(collection As PonyCollection, directory As String) As PonyBase
        Dim pony As PonyBase = Nothing
        Try
            Dim fullPath = Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, directory)
            Dim iniFileName = Path.Combine(fullPath, PonyBase.ConfigFilename)
            If IO.Directory.Exists(fullPath) Then
                pony = New PonyBase(collection)
                pony._directory = directory
                pony.DisplayName = directory
                If File.Exists(iniFileName) Then
                    Using reader = New StreamReader(iniFileName)
                        ParsePonyConfig(fullPath, reader, pony)
                    End Using
                End If
            End If
        Catch ex As Exception
            Return Nothing
        End Try
        Return pony
    End Function

    Private Shared Sub ParsePonyConfig(folder As String, reader As StreamReader, pony As PonyBase)
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
                            Dim category As CaseInsensitiveString = columns(i)
                            For Each item As String In Main.Instance.FilterOptionsBox.Items
                                If New CaseInsensitiveString(item) = category Then
                                    pony.Tags.Add(category)
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

    Private Shared Sub TryParse(Of T)(line As String, directory As String, parseFunc As TryParse(Of T), onSuccess As Action(Of T))
        Dim result As T
        If parseFunc(line, directory, result, Nothing) Then
            onSuccess(result)
        End If
    End Sub

    Private Shared Sub TryParse(Of T)(line As String, directory As String, pony As PonyBase, parseFunc As TryParse(Of T, PonyBase), onSuccess As Action(Of T))
        Dim result As T
        If parseFunc(line, directory, pony, result, Nothing) Then
            onSuccess(result)
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

        'When the pony if off-screen we overwrite the follow parameters to get them onscreen again.
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
        Dim configFilePath = IO.Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, Directory, PonyBase.ConfigFilename)

        Dim comments As New List(Of String)()
        If File.Exists(configFilePath) Then
            Using reader As New StreamReader(configFilePath)
                Do Until reader.EndOfStream
                    Dim line = reader.ReadLine()
                    If line.Length > 0 AndAlso line(0) = "'" Then comments.Add(line)
                Loop
            End Using
        End If

        Dim tempFileName = Path.GetTempFileName()
        Using writer As New StreamWriter(tempFileName, False, System.Text.Encoding.UTF8)
            For Each comment In comments
                writer.WriteLine(comment)
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
        End Using
        MoveOrReplace(tempFileName, configFilePath)

        Dim interactionsFilePath = Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, InteractionBase.ConfigFilename)
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

    Public Shared Function TryLoad(iniLine As String, ByRef result As InteractionBase, ByRef issues As ImmutableArray(Of ParseIssue)) As Boolean
        result = Nothing
        issues = Nothing

        Dim i = New InteractionBase()
        i.SourceIni = iniLine
        Dim p As New StringCollectionParser(CommaSplitQuoteBraceQualified(iniLine),
                                            {"Name", "Initiator", "Chance",
                                             "Proximity", "Targets", "Target Activation",
                                             "Behaviors", "Reactivation Delay"})
        i.Name = p.NotNull()
        i.InitiatorName = p.NotNull()
        i.Chance = p.ParseDouble(0, 0, 1)
        i.Proximity = p.ParseDouble(125, 0, 10000)
        i.TargetNames.UnionWith(CommaSplitQuoteQualified(p.NotNull("")).Where(Function(s) s.Length <> 0))
        i.Activation = p.Project(AddressOf TargetActivationFromString, TargetActivation.One)
        i.BehaviorNames.UnionWith(CommaSplitQuoteQualified(p.NotNull("")).Where(Function(s) s.Length <> 0).
                                  Select(Function(s) New CaseInsensitiveString(s)))
        i.ReactivationDelay = TimeSpan.FromSeconds(p.ParseDouble(60, 0, 3600))

        issues = p.Issues.ToImmutableArray()
        result = i
        Return p.AllParsingSuccessful
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
        Dim base = ponies.AllBases.FirstOrDefault(Function(pb) pb.Directory = directory)
        If base Is Nothing Then
            issues.Add(New ParseIssue(propertyName, directory, "", String.Format("No pony named '{0}' exists.", directory)))
        Else
            For Each behaviorName In BehaviorNames
                Dim behavior = base.Behaviors.OnlyOrDefault(Function(b) b.Name = behaviorName)
                If behavior Is Nothing Then
                    issues.Add(New ParseIssue("Behaviors", behaviorName, "",
                                              String.Format("'{0}' is missing behavior '{1}'.", directory, behaviorName)))
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
Public Class Interaction

    Private _base As InteractionBase
    Public ReadOnly Property Base As InteractionBase
        Get
            Return _base
        End Get
    End Property

    Public Property Initiator As Pony
    Public Property Trigger As Pony
    Private ReadOnly _targets As New HashSet(Of Pony)()
    Public ReadOnly Property Targets As HashSet(Of Pony)
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
#End Region

#Region "Behavior class"
Public Class Behavior
    Implements IPonyIniSourceable, IReferential
    Private ReadOnly pony As PonyBase

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
    Public ReadOnly Property LinkedBehavior As Behavior
        Get
            Return pony.Behaviors.OnlyOrDefault(Function(b) b.Name = LinkedBehaviorName)
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
    Public ReadOnly Property StartLine As Speech
        Get
            Return pony.Speeches.OnlyOrDefault(Function(sl) sl.Name = StartLineName)
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
    Public ReadOnly Property EndLine As Speech
        Get
            Return pony.Speeches.OnlyOrDefault(Function(sl) sl.Name = EndLineName)
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
            Return New EffectsEnumerator(behavior)
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

    Public Shared Function TryLoad(iniLine As String, imageDirectory As String, pony As PonyBase, ByRef result As Behavior, ByRef issues As ImmutableArray(Of ParseIssue)) As Boolean
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
        b.Name = p.NotNull()
        b.Chance = p.ParseDouble(0, 0, 1)
        b.MaxDuration = p.ParseDouble(15, 0, 300)
        b.MinDuration = p.ParseDouble(5, 0, 300)
        b.Speed = p.ParseDouble(3, 0, 25)
        b.RightImage.Path = p.NoParse()
        If p.Assert(b.RightImage.Path, Function(s) Not String.IsNullOrEmpty(s), "An image path has not been set.", Nothing) Then
            b.RightImage.Path = p.SpecifiedCombinePath(imageDirectory, b.RightImage.Path, "Image will not be loaded.")
            p.SpecifiedFileExists(b.RightImage.Path, "Image will not be loaded.")
        End If
        b.LeftImage.Path = p.NoParse()
        If p.Assert(b.LeftImage.Path, Function(s) Not String.IsNullOrEmpty(s), "An image path has not been set.", Nothing) Then
            b.LeftImage.Path = p.SpecifiedCombinePath(imageDirectory, b.LeftImage.Path, "Image will not be loaded.")
            p.SpecifiedFileExists(b.LeftImage.Path, "Image will not be loaded.")
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
        Return p.AllParsingSuccessful
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
        Return {Referential.GetIssue("Linked Behavior", LinkedBehaviorName, pony.Behaviors.Select(Function(b) b.Name)),
                Referential.GetIssue("Start Speech", StartLineName, pony.Speeches.Select(Function(s) s.Name)),
                Referential.GetIssue("End Speech", EndLineName, pony.Speeches.Select(Function(s) s.Name)),
                Referential.GetIssue("Follow Stopped Behavior", FollowStoppedBehaviorName, pony.Behaviors.Select(Function(b) b.Name)),
                Referential.GetIssue("Follow Moving Behavior", FollowMovingBehaviorName, pony.Behaviors.Select(Function(b) b.Name)),
                Referential.GetIssue("Follow Target", OriginalFollowTargetName, ponies.Bases.Select(Function(pb) pb.Directory))}.
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

    Public Shared Function TryLoad(iniLine As String, soundDirectory As String, ByRef result As Speech, ByRef issues As ImmutableArray(Of ParseIssue)) As Boolean
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
        s.Name = p.NotNull("Unnamed")
        s.Text = p.NotNull()
        s.SoundFile = p.NoParse()
        If s.SoundFile IsNot Nothing Then
            s.SoundFile = p.SpecifiedCombinePath(soundDirectory, s.SoundFile, "Sound file will not be loaded.")
            p.SpecifiedFileExists(s.SoundFile, "Sound file will not be loaded.")
        End If
        s.Skip = p.ParseBoolean(False)
        s.Group = p.ParseInt32(Behavior.AnyGroup, 0, 100)

        issues = p.Issues.ToImmutableArray()
        result = s
        Return p.AllParsingSuccessful
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
    Implements ISpeakingSprite

    ''' <summary>
    ''' Number of milliseconds by which the internal temporal state of the sprite should be advanced with each call to UpdateOnce().
    ''' </summary>
    Private Const StepRate = 1000.0 / 30.0

    Public Shared Property CursorLocation As Point
    Public Shared Property CurrentAnimator As PonyAnimator
    Public Shared Property CurrentViewer As ISpriteCollectionView
    Public Shared Property PreviewWindowRectangle As Rectangle

    Private Shared audioErrorShown As Boolean

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
    Private _base As PonyBase
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
    Friend ReadOnly Property InteractionBases() As List(Of InteractionBase)
        Get
            Return Base.Interactions
        End Get
    End Property
#End Region

    Private ReadOnly interactions As New List(Of Interaction)()

    Public Property ShouldBeSleeping As Boolean
    Private _sleeping As Boolean
    Public Property Sleeping() As Boolean
        Get
            Return _sleeping
        End Get
        Private Set(value As Boolean)
            _sleeping = value
        End Set
    End Property

    Public Property BeingDragged() As Boolean

    Public Property CurrentBehaviorGroup As Integer

    Public Property InteractionActive As Boolean = False
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

    Public Property IsInteracting As Boolean = False
    Public Property PlayingGame As Boolean = False

    Private verticalMovementAllowed As Boolean = False
    Private horizontalMovementAllowed As Boolean = False
    Public Property facingUp As Boolean = False
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
    ''' <remarks></remarks>
    Private interactionDelayUntil As TimeSpan

    Private _currentBehavior As Behavior
    Public Property CurrentBehavior As Behavior
        Get
            Return _currentBehavior
        End Get
        Friend Set(value As Behavior)
            Diagnostics.Debug.Assert(value IsNot Nothing)
            _currentBehavior = value
            If Not (ManualControlPlayerOne OrElse ManualControlPlayerTwo) Then
                SetAllowableDirections()
            End If
        End Set
    End Property

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
    Public Property visualOverrideBehavior As Behavior

    Private _returningToScreenArea As Boolean = False
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
    Public Property GoingHome As Boolean = False
    ''' <summary>
    ''' Used when a pony has been recalled and is just about to "enter" a house
    ''' </summary>
    Public Property OpeningDoor As Boolean = False

    ''' <summary>
    ''' Should we stop because the cursor is hovered over?
    ''' </summary>
    Private CursorOverPony As Boolean = False

    ''' <summary>
    ''' Are we actually halted now?
    ''' </summary>
    Private HaltedForCursor As Boolean = False
    ''' <summary>
    ''' Number of ticks for which the pony is immune to cursor interaction.
    ''' </summary>
    Private CursorImmunity As Integer = 0

    Public Property Destination As Vector2
    Public Property AtDestination As Boolean = False
    Private ReadOnly Property hasDestination As Boolean
        Get
            Return Destination <> Vector2.Zero
        End Get
    End Property

    ''' <summary>
    ''' Used in the Paint() sub to help stop flickering between left and right images under certain circumstances.
    ''' </summary>
    Private paintStop As Boolean = False

    ''' <summary>
    ''' The location on the screen.
    ''' </summary>
    Public Property TopLeftLocation As Point

    ''' <summary>
    ''' Used for predicting future movement (just more of what we last did)
    ''' </summary>
    Private lastMovement As Vector2F

    Private ReadOnly _activeEffects As New List(Of Effect)()
    Public ReadOnly Property ActiveEffects As List(Of Effect)
        Get
            Return _activeEffects
        End Get
    End Property

    ' User has the option of limiting songs to one-total at a time, or one-per-pony at a time.
    ' These two options are used for the one-at-a-time option.
    Private Shared AnyAudioLastPlayed As Date = DateTime.UtcNow
    Private Shared LastLengthAnyAudio As Integer
    ' These two options are used for the one-per-pony option.
    Private AudioLastPlayed As Date = DateTime.UtcNow
    Private LastLengthAudio As Integer

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

    Private effectsToRemove As New List(Of Effect)

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
    Friend internalTime As TimeSpan
    Private lastUpdateTime As TimeSpan

    Private ReadOnly possibleMoveModes As New List(Of AllowedMoves)(3)
#End Region

    Public ReadOnly Property Scale() As Double
        Get
            Return If(Base.Scale <> 0, Base.Scale, Options.ScaleFactor)
        End Get
    End Property

    Public Sub New(base As PonyBase)
        Argument.EnsureNotNull(base, "base")
        _base = base
        If Options.EnablePonyLogs Then UpdateRecord = New List(Of Record)()
    End Sub

    ''' <summary>
    ''' Starts the sprite.
    ''' </summary>
    ''' <param name="startTime">The current time of the animator, which will be the temporal zero point for this sprite.</param>
    Public Sub Start(startTime As TimeSpan) Implements ISprite.Start
        CurrentBehavior = Behaviors(0)
        internalTime = startTime
        lastUpdateTime = startTime
        Teleport()
        'UpdateOnce()
    End Sub

    ''' <summary>
    ''' Teleport the pony to a random location within bounds.
    ''' </summary>
    Friend Sub Teleport()
        ' If we are in preview mode, just teleport into the top-left corner for consistency.
        If Reference.InPreviewMode Then
            TopLeftLocation = Point.Add(Pony.PreviewWindowRectangle.Location, New Size(10, 10))
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
            If Not InAvoidanceArea(teleportLocation) Then Exit For
        Next
        TopLeftLocation = teleportLocation
    End Sub

    ''' <summary>
    ''' Updates the sprite, bringing its state as close to the specified time as possible.
    ''' </summary>
    ''' <param name="updateTime">The current time of the animator, to which the sprite should match its state.</param>
    Public Sub Update(updateTime As TimeSpan) Implements ISprite.Update
        ' Find out how far behind the sprite is since its last update, and catch up.
        ' The time factor here means the internal time of the sprite can be advanced at different rates than the external time.
        ' This fixed timestep method of updating is prone to temporal aliasing, but this is largely unnoticeable compared to the generally
        ' low frame rate of animations and lack of spatial anti-aliasing since the images are pixel art. That said, the time scaling should
        ' be constrained from being too low (which will exaggerate the temporal aliasing until it is noticeable) or too high (which kills
        ' performance as UpdateOnce must be evaluated many times to catch up).
        Dim difference = updateTime - lastUpdateTime
        While difference.TotalMilliseconds > 0
            UpdateOnce()
            difference -= TimeSpan.FromMilliseconds(StepRate / Options.TimeFactor)
        End While
        lastUpdateTime = updateTime - difference
    End Sub

    ''' <summary>
    ''' Advances the internal time state of the pony by the step rate with each call.
    ''' </summary>
    Private Sub UpdateOnce()
        internalTime += TimeSpan.FromMilliseconds(StepRate)

        ' If there are no behaviors that can be undertaken, there's nothing that needs updating anyway.
        If Behaviors.Count = 0 Then Exit Sub

        ' Handle switching pony between active and asleep.
        If ShouldBeSleeping Then
            If Sleeping Then
                If BeingDragged Then TopLeftLocation = CursorLocation - GetImageCenterOffset()
            Else
                Sleep()
            End If
            AddUpdateRecord("Pony should be sleeping.")
            Exit Sub
        Else
            If Sleeping Then WakeUp()
        End If

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
    End Sub

    ''' <summary>
    ''' Chooses a behavior to use for sleeping and activates it with no timeout.
    ''' </summary>
    Public Sub Sleep()
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
        Sleeping = True
    End Sub

    ''' <summary>
    ''' Wakes a pony from their sleeping behavior.
    ''' </summary>
    Public Sub WakeUp()
        Sleeping = False
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

        AddUpdateRecord("Cancelled interaction. IsInteractionInitiator: ", isInteractionInitiator.ToString())

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
                CurrentBehavior = Behaviors(0)
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
                Not Reference.InPreviewMode Then
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
        ElseIf CurrentBehavior.EndLine Is Nothing AndAlso followTargetName = "" AndAlso
            Not IsInteracting AndAlso Rng.NextDouble() <= Options.PonySpeechChance Then
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
        If (CurrentBehavior.AllowedMovement And AllowedMoves.HorizontalOnly) = AllowedMoves.HorizontalOnly Then
            possibleMoveModes.Add(AllowedMoves.HorizontalOnly)
        End If
        If (CurrentBehavior.AllowedMovement And AllowedMoves.VerticalOnly) = AllowedMoves.VerticalOnly Then
            possibleMoveModes.Add(AllowedMoves.VerticalOnly)
        End If
        If (CurrentBehavior.AllowedMovement And AllowedMoves.DiagonalOnly) = AllowedMoves.DiagonalOnly Then
            possibleMoveModes.Add(AllowedMoves.DiagonalOnly)
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

    ''' <summary>
    ''' Prompts the pony to speak a line if it has not done so recently. A random line is chosen unless one is specified.
    ''' </summary>
    ''' <param name="line">The line the pony should speak, or null to choose one at random.</param>
    Public Sub PonySpeak(Optional line As Speech = Nothing)
        'When the cursor is over us, don't talk too often.
        If CursorOverPony AndAlso (internalTime - lastSpeakTime).TotalSeconds < 15 Then
            Exit Sub
        End If

        ' Select a line at random from the lines that may be played at random that are in the current group.
        If line Is Nothing Then
            If Base.SpeechesRandom.Count = 0 Then
                Exit Sub
            Else
                Dim randomGroupLines = Base.SpeechesRandom.Where(
                    Function(l) l.Group = 0 OrElse l.Group = CurrentBehavior.Group).ToArray()
                If randomGroupLines.Length = 0 Then Exit Sub
                line = randomGroupLines(Rng.Next(randomGroupLines.Count))
            End If
        End If

        ' Set the line text to be displayed.
        If Options.PonySpeechEnabled Then
            lastSpeakTime = internalTime
            lastSpeakLine = Me.DisplayName & ": " & ControlChars.Quote & line.Text & ControlChars.Quote
        End If

        ' Start the sound file playing.
        If line.SoundFile <> "" AndAlso Reference.DirectXSoundAvailable Then
            PlaySound(line.SoundFile)
        End If
    End Sub

    ''' <summary>
    ''' Plays the sound file located at the specified path.
    ''' </summary>
    ''' <param name="filePath">The path to the sound file to be played.</param>
    Private Sub PlaySound(filePath As String)
        ' Sound must be enabled for the mode we are in.
        If Not Options.SoundEnabled Then Exit Sub
        If Reference.InScreensaverMode AndAlso Not Options.ScreensaverSoundEnabled Then Exit Sub

        ' Don't play sounds over other ones - wait until they finish.
        If Not Options.SoundSingleChannelOnly Then
            If DateTime.UtcNow.Subtract(Me.AudioLastPlayed).TotalMilliseconds <= Me.LastLengthAudio Then Exit Sub
        Else
            If DateTime.UtcNow.Subtract(AnyAudioLastPlayed).TotalMilliseconds <= LastLengthAnyAudio Then Exit Sub
        End If

        ' Quick sanity check that the file exists on disk.
        If Not File.Exists(filePath) Then Exit Sub

        Try
            ' If you get a MDA warning about loader locking - you'll just have to disable that exception message.  
            ' Apparently it is a bug with DirectX that only occurs with Visual Studio...
            ' We use DirectX now so that we can use MP3 instead of WAV files
            Dim audio As New Microsoft.DirectX.AudioVideoPlayback.Audio(filePath)
            CurrentAnimator.ActiveSounds.Add(audio)

            ' Volume is between -10000 and 0, with 0 being the loudest.
            audio.Volume = CInt(Options.SoundVolume * 10000 - 10000)
            audio.Play()

            If Not Options.SoundSingleChannelOnly Then
                Me.LastLengthAudio = CInt(audio.Duration * 1000)
                Me.AudioLastPlayed = DateTime.UtcNow
            Else
                LastLengthAnyAudio = CInt(audio.Duration * 1000) 'to milliseconds
                AnyAudioLastPlayed = DateTime.UtcNow
            End If
        Catch ex As Exception
            If Not audioErrorShown AndAlso Not Reference.InScreensaverMode Then
                audioErrorShown = True
                My.Application.NotifyUserOfNonFatalException(
                    ex, String.Format(CultureInfo.CurrentCulture,
                                      "There was an error trying to play a sound. Maybe the file is corrupt?{0}" &
                                      "You will not receive further notifications about sound errors.{0}{0}" &
                                      "File: {1}{0}" &
                                      "Pony: {2}{0}",
                                      Environment.NewLine, filePath, Directory))
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Checks the current mouseover state of the pony and toggles between mouseover modes accordingly.
    ''' </summary>
    Private Sub ChangeMouseOverMode()
        If CursorOverPony AndAlso Not HaltedForCursor Then
            ' The cursor has moved over us and we should halt.
            HaltedForCursor = True
            previousBehavior = CurrentBehavior

            ' Select a stationary behavior, or if possible a dedicated mouseover behavior.
            CurrentBehavior = GetAppropriateBehaviorOrCurrent(AllowedMoves.None, False)
            For Each behavior In Behaviors
                If behavior.Group <> CurrentBehaviorGroup Then Continue For
                If behavior.AllowedMovement = AllowedMoves.MouseOver Then
                    CurrentBehavior = behavior
                    effectsAlreadyPlayedForBehavior.Clear()
                    Exit For
                End If
            Next
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

        If Game.CurrentGame Is Nothing OrElse
            (Game.CurrentGame IsNot Nothing AndAlso
             Game.CurrentGame.Status <> Game.GameStatus.Setup) Then
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
             (Game.CurrentGame IsNot Nothing AndAlso Game.CurrentGame.Status = Game.GameStatus.Setup)) Then
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

            ' We do not want to detect if we are at the destination if we are trying to move onscreen - we might stop at the destination
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

        Dim newTopLeftLocation = Point.Round(CType(TopLeftLocation, Vector2) + movement)

        Dim isNearCursorNow = IsPonyNearMouseCursor(TopLeftLocation)
        Dim isNearCursorFuture = IsPonyNearMouseCursor(newTopLeftLocation)

        Dim isOnscreenNow = IsPonyOnScreen(TopLeftLocation)
        Dim isOnscreenFuture = IsPonyOnScreen(newTopLeftLocation)

        ' TODO: Refactor and extract.
        'Dim playingGameAndOutOfBounds = PlayingGame AndAlso
        '    Game.CurrentGame.Status <> Game.GameStatus.Setup AndAlso
        '    Not IsPonyInBox(newTopLeftLocation, Game.Position.Allowed_Area)
        Dim playingGameAndOutOfBounds = False

        Dim isEnteringWindowNow = False
        If Options.WindowAvoidanceEnabled AndAlso Not ReturningToScreenArea Then
            isEnteringWindowNow = IsPonyEnteringWindow(TopLeftLocation, newTopLeftLocation, movement)
        End If

        Dim isInAvoidanceZoneNow = InAvoidanceArea(TopLeftLocation)
        Dim isInAvoidanceZoneFuture = InAvoidanceArea(newTopLeftLocation)

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
                CurrentBehavior = GetAppropriateBehaviorOrCurrent(CurrentBehavior.AllowedMovement, False)
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
            TopLeftLocation = newTopLeftLocation
            lastMovement = movement

            Dim useVisualOverride = (followTarget IsNot Nothing AndAlso
                                     (CurrentBehavior.AutoSelectImagesOnFollow OrElse
                                      CurrentBehavior.FollowMovingBehavior IsNot Nothing OrElse
                                      CurrentBehavior.FollowStoppedBehavior IsNot Nothing)) OrElse
                              (Game.CurrentGame IsNot Nothing AndAlso AtDestination)
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
                    (InAvoidanceArea(Destination) OrElse Not IsPonyOnScreen(Destination)) Then
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
                If Reference.InPreviewMode OrElse Options.PonyTeleportEnabled Then
                    Teleport()
                    AddUpdateRecord("Teleporting back onscreen.")
                    Exit Sub
                End If

                Dim safespot = FindSafeDestination()
                ReturningToScreenArea = True

                If CurrentBehavior.Speed = 0 Then
                    CurrentBehavior = GetAppropriateBehaviorOrCurrent(AllowedMoves.All, True)
                End If

                followTarget = Nothing
                followTargetName = ""
                destinationCoords = safespot

                Paint(False)
                AddUpdateRecord("Walking back onscreen.")
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
                Return New Point(CurrentInteraction.Initiator.TopLeftLocation.X + destinationCoords.X,
                                 CurrentInteraction.Initiator.TopLeftLocation.Y + destinationCoords.Y)
            End If

            ' If not interacting, or following a different pony, we need to figure out which ones and follow one at random.
            Dim poniesToFollow As New List(Of Pony)
            For Each ponyToFollow In CurrentAnimator.Ponies()
                If ponyToFollow.Directory = followTargetName Then
                    poniesToFollow.Add(ponyToFollow)
                End If
            Next
            If poniesToFollow.Count <> 0 Then
                Dim ponyToFollow = poniesToFollow(Rng.Next(poniesToFollow.Count))
                followTarget = ponyToFollow
                Return New Point(ponyToFollow.TopLeftLocation.X + destinationCoords.X,
                                 ponyToFollow.TopLeftLocation.Y + destinationCoords.Y)
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
            Not Sleeping AndAlso
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

                    Dim newEffect = New Effect(effect, Not facingRight)

                    If newEffect.Base.Duration <> 0 Then
                        newEffect.DesiredDuration = newEffect.Base.Duration
                        newEffect.CloseOnNewBehavior = False
                    Else
                        If Me.HaltedForCursor Then
                            newEffect.DesiredDuration = TimeSpan.FromSeconds(CurrentBehavior.MaxDuration).TotalSeconds
                        Else
                            newEffect.DesiredDuration = (BehaviorDesiredDuration - Me.CurrentTime).TotalSeconds
                        End If
                        newEffect.CloseOnNewBehavior = True
                    End If

                    newEffect.Location = GetEffectLocation(newEffect.CurrentImageSize,
                                                            newEffect.PlacementDirection,
                                                            TopLeftLocation,
                                                            CurrentImageSize,
                                                            newEffect.Centering, CSng(Scale))

                    newEffect.OwningPony = Me

                    Pony.CurrentAnimator.AddEffect(newEffect)
                    ActiveEffects.Add(newEffect)

                    effectsLastUsed(effect) = currentTime

                End If
            Next
        End If

    End Sub

    'reverse directions as if we were bouncing off a boundary.
    Friend Sub Bounce(pony As Pony, currentLocation As Point, newLocation As Point, movement As SizeF)
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

        Dim new_location_x As New Point(newLocation.X, currentLocation.Y)
        Dim new_location_y As New Point(currentLocation.X, newLocation.Y)

        If movement.Width <> 0 AndAlso movement.Height <> 0 Then
            If Not pony.IsPonyOnScreen(new_location_x) OrElse
                pony.InAvoidanceArea(new_location_x) OrElse
                pony.IsPonyEnteringWindow(currentLocation, new_location_x, New SizeF(movement.Width, 0)) Then
                x_bad = True
            End If
            If Not pony.IsPonyOnScreen(new_location_y) OrElse
                pony.InAvoidanceArea(new_location_y) OrElse
                pony.IsPonyEnteringWindow(currentLocation, new_location_y, New SizeF(0, movement.Height)) Then
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
        Dim Number_Of_Interations = ticks / (1000.0F / Pony.CurrentAnimator.MaximumFramesPerSecond)  'get the # of intervals in one second
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
            'This allows pegasi to fly up to reach their target instead of walking straight up.
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
                    appropriateBehavior = GetAppropriateBehaviorOrCurrent(allowedMovement, True)
                End If
                visualOverrideBehavior = appropriateBehavior
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
            TopLeftLocation = New Point(TopLeftLocation.X - newCenter.Width + currentCustomImageCenter.Width,
                                 TopLeftLocation.Y - newCenter.Height + currentCustomImageCenter.Height)
            currentCustomImageCenter = newCenter
        End If

        effectsToRemove.Clear()

        For Each effect In ActiveEffects
            If effect.CloseOnNewBehavior Then
                If CurrentBehavior.Name <> effect.Base.BehaviorName Then
                    effectsToRemove.Add(effect)
                End If
            End If
        Next

        For Each effect In effectsToRemove
            ActiveEffects.Remove(effect)
        Next

    End Sub

    'You can place effects at an offset to the pony, and also set them to the left or the right of themselves for big effects.
    Friend Shared Function GetEffectLocation(EffectImageSize As Size, dir As Direction,
                                      ParentLocation As Point, ParentSize As Vector2, centering As Direction, scale As Single) As Point

        Dim point As Point

        With ParentSize * CSng(scale)
            Select Case dir
                Case Direction.BottomCenter
                    point = New Point(CInt(ParentLocation.X + .X / 2), CInt(ParentLocation.Y + .Y))
                Case Direction.BottomLeft
                    point = New Point(ParentLocation.X, CInt(ParentLocation.Y + .Y))
                Case Direction.BottomRight
                    point = New Point(CInt(ParentLocation.X + .X), CInt(ParentLocation.Y + .Y))
                Case Direction.MiddleCenter, Direction.Random, Direction.RandomNotCenter
                    point = New Point(CInt(ParentLocation.X + .X / 2), CInt(ParentLocation.Y + .Y / 2))
                Case Direction.MiddleLeft
                    point = New Point(ParentLocation.X, CInt(ParentLocation.Y + .Y / 2))
                Case Direction.MiddleRight
                    point = New Point(CInt(ParentLocation.X + .X), CInt(ParentLocation.Y + .Y / 2))
                Case Direction.TopCenter
                    point = New Point(CInt(ParentLocation.X + .X / 2), ParentLocation.Y)
                Case Direction.TopLeft
                    point = New Point(ParentLocation.X, ParentLocation.Y)
                Case Direction.TopRight
                    point = New Point(CInt(ParentLocation.X + .X), ParentLocation.Y)
            End Select

        End With

        Dim effectscaling = Options.ScaleFactor

        Select Case centering
            Case Direction.BottomCenter
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width) / 2), CInt(point.Y - (effectscaling * EffectImageSize.Height)))
            Case Direction.BottomLeft
                point = New Point(point.X, CInt(point.Y - (effectscaling * EffectImageSize.Height)))
            Case Direction.BottomRight
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width)), CInt(point.Y - (effectscaling * EffectImageSize.Height)))
            Case Direction.MiddleCenter, Direction.Random, Direction.RandomNotCenter
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width) / 2), CInt(point.Y - (effectscaling * EffectImageSize.Height) / 2))
            Case Direction.MiddleLeft
                point = New Point(point.X, CInt(point.Y - (effectscaling * EffectImageSize.Height) / 2))
            Case Direction.MiddleRight
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width)), CInt(point.Y - (effectscaling * EffectImageSize.Height) / 2))
            Case Direction.TopCenter
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width) / 2), point.Y)
            Case Direction.TopLeft
                'no change
            Case Direction.TopRight
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width)), point.Y)
        End Select

        Return point

    End Function

    Private Function GetAppropriateBehavior(movement As AllowedMoves, speed As Boolean,
                                           Optional suggestedBehavior As Behavior = Nothing) As Behavior
        'does the current behavior work?
        If CurrentBehavior IsNot Nothing Then
            If movement = AllowedMoves.All OrElse (CurrentBehavior.AllowedMovement And movement) = movement Then
                If CurrentBehavior.Speed = 0 AndAlso movement = AllowedMoves.None Then Return CurrentBehavior
                If CurrentBehavior.Speed <> 0 AndAlso movement = AllowedMoves.All Then Return CurrentBehavior
            End If
        End If

        For Each behavior In Behaviors
            If behavior.Group <> CurrentBehaviorGroup Then Continue For

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

    'Pick a behavior that matches the speed (fast or slow) and direction we want to go in.
    'Use the specified behavior if it works.
    ''' <summary>
    ''' Returns a behavior that best matches the desired allowable movement and speed, or else the current behavior.
    ''' </summary>
    ''' <param name="movement">The movement to match (as best as possible).</param>
    ''' <param name="speed">Is the user pressing the "speed" override key.</param>
    ''' <param name="suggestedBehavior">A suggested behavior to test first. This will be returned if it meets the requirements
    ''' sufficiently.</param>
    ''' <returns>The suggested behavior, if it meets the requirements, otherwise any behavior with meets the requirements sufficiently. If 
    ''' no behavior matches sufficiently the current behavior is returned.
    ''' </returns>
    Friend Function GetAppropriateBehaviorOrCurrent(movement As AllowedMoves, speed As Boolean,
                                           Optional suggestedBehavior As Behavior = Nothing) As Behavior
        Return If(GetAppropriateBehavior(movement, speed, suggestedBehavior), CurrentBehavior)
    End Function

    Shared Function GetScreenContainingPoint(point As Point) As Screen
        For Each screen In Options.Screens
            If screen.WorkingArea.Contains(point) Then Return screen
        Next
        Return Nothing
    End Function

    'Test to see if we overlap with another application's window.
    <Security.Permissions.PermissionSet(Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
    Private Function IsPonyEnteringWindow(current_location As Point, new_location As Point, movement As SizeF) As Boolean
        If Not OperatingSystemInfo.IsWindows Then Return False

        Try
            If Reference.InPreviewMode Then Return False
            If Not Options.WindowAvoidanceEnabled Then Return False

            If movement = SizeF.Empty Then Return False

            Dim current_window_1 = Win32.WindowFromPoint(New Win32.POINT(current_location.X, current_location.Y))
            Dim current_window_2 = Win32.WindowFromPoint(New Win32.POINT(CInt(current_location.X + (Scale * CurrentImageSize.X)), CInt(current_location.Y + (Scale * CurrentImageSize.Y))))
            Dim current_window_3 = Win32.WindowFromPoint(New Win32.POINT(CInt(current_location.X + (Scale * CurrentImageSize.X)), current_location.Y))
            Dim current_window_4 = Win32.WindowFromPoint(New Win32.POINT(current_location.X, CInt(current_location.Y + (Scale * CurrentImageSize.Y))))

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
                    new_window_2 = Win32.WindowFromPoint(New Win32.POINT(CInt(new_location.X + (Scale * CurrentImageSize.X)), CInt(new_location.Y + (Scale * CurrentImageSize.Y))))
                    new_window_3 = Win32.WindowFromPoint(New Win32.POINT(CInt(new_location.X + (Scale * CurrentImageSize.X)), new_location.Y))
                Case Is < 0
                    new_window_1 = Win32.WindowFromPoint(New Win32.POINT(new_location.X, new_location.Y))
                    new_window_4 = Win32.WindowFromPoint(New Win32.POINT(new_location.X, CInt(new_location.Y + (Scale * CurrentImageSize.Y))))
            End Select

            Select Case movement.Height
                Case Is > 0
                    If (new_window_2) = IntPtr.Zero Then new_window_2 = Win32.WindowFromPoint(New Win32.POINT(CInt(new_location.X + (Scale * CurrentImageSize.X)), CInt(new_location.Y + (Scale * CurrentImageSize.Y))))
                    If (new_window_4) = IntPtr.Zero Then new_window_4 = Win32.WindowFromPoint(New Win32.POINT(new_location.X, CInt(new_location.Y + (Scale * CurrentImageSize.Y))))
                Case Is < 0
                    If (new_window_1) = IntPtr.Zero Then new_window_1 = Win32.WindowFromPoint(New Win32.POINT(new_location.X, new_location.Y))
                    If (new_window_3) = IntPtr.Zero Then new_window_3 = Win32.WindowFromPoint(New Win32.POINT(CInt(new_location.X + (Scale * CurrentImageSize.X)), new_location.Y))
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
                        If IsPonyInBox(current_location, Rectangle.FromLTRB(
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

    'Is the pony at least partially on any of the main screens?
    Friend Function IsPonyOnScreen(location As Point) As Boolean
        Return IsLocationInRect(location, Options.GetCombinedScreenArea())
    End Function

    'Is the pony at least partially on the supplied screens?
    Friend Function IsPonyOnScreen(location As Point, screen As Screen) As Boolean
        Return IsLocationInRect(location, screen.WorkingArea)
    End Function

    Private Function IsLocationInRect(location As Point, rect As Rectangle) As Boolean
        If Reference.InPreviewMode Then Return True
        Return rect.Contains(New Rectangle(location, New Size(CInt(CurrentImageSize.X * Scale), CInt(CurrentImageSize.Y * Scale))))
    End Function

    Shared Function IsPonyInBox(location As Point, box As Rectangle) As Boolean
        Return box.IsEmpty OrElse box.Contains(location)
    End Function

    ''are we inside the user specified "Everfree Forest"?
    Function InAvoidanceArea(new_location As Point) As Boolean

        If Reference.InPreviewMode Then
            Dim previewArea = Pony.PreviewWindowRectangle

            If CurrentImageSize.Y > previewArea.Height OrElse _
                CurrentImageSize.X > previewArea.Width Then
                Return False
            End If

            If IsPonyInBox(new_location, previewArea) AndAlso _
               IsPonyInBox(New Point(new_location.X, new_location.Y + CurrentImageSize.Y), previewArea) AndAlso _
               IsPonyInBox(New Point(new_location.X + CurrentImageSize.X, new_location.Y), previewArea) AndAlso _
               IsPonyInBox(New Point(new_location.X + CurrentImageSize.X, new_location.Y + CurrentImageSize.Y), previewArea) Then

                Return False
            Else
                Return True
            End If

        End If


        If IsNothing(CurrentBehavior) Then Return False

        If Options.ExclusionZone.IsEmpty Then
            Return False
        End If

        Dim points As New List(Of Point)

        'add center (or upper right if no center is defined)
        Dim center As Point = New Point(
                              CInt(new_location.X + (Scale * currentCustomImageCenter.Width)),
                              CInt(new_location.Y + (Scale * currentCustomImageCenter.Height)))

        points.Add(New Point(center.X - 45, center.Y - 45)) 'top left
        points.Add(New Point(center.X + 45, center.Y - 45)) ' top right
        points.Add(New Point(center.X - 45, center.Y + 45)) 'bottom left
        points.Add(New Point(center.X + 45, center.Y + 45)) 'bottom right

        'return true if any of the points hit the bad area
        For Each point In points
            Dim screen = GetScreenContainingPoint(point)
            If screen IsNot Nothing Then
                Dim area = Options.ExclusionZoneForBounds(screen.WorkingArea)
                If area.Contains(point) Then Return True
            End If
        Next

        Return False
    End Function

    Function IsPonyNearMouseCursor(location As Point) As Boolean
        If Not Options.CursorAvoidanceEnabled Then Return False
        If Reference.InScreensaverMode Then Return False
        If CursorImmunity > 0 Then Return False
        If IsInteracting Then Return False
        If ManualControlPlayerOne OrElse ManualControlPlayerTwo Then Return False

        For Each behavior In Behaviors
            If behavior.AllowedMovement = AllowedMoves.MouseOver Then
                Dim loc = New Vector2F(location)
                Dim s = CSng(Scale)
                Dim cursorLoc = New Vector2F(CursorLocation)
                If Vector2F.Distance(loc + (behavior.LeftImage.Center * s), cursorLoc) <= Options.CursorAvoidanceSize Then Return True
                If Vector2F.Distance(loc + (behavior.RightImage.Center * s), cursorLoc) <= Options.CursorAvoidanceSize Then Return True
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' Returns a random location that is within the allowable regions to be in.
    ''' </summary>
    ''' <returns>The center of the preview area in preview mode, otherwise a random location within the allowable region, if one can be
    ''' found; otherwise Point.Empty.</returns>
    Friend Function FindSafeDestination() As Point
        If Reference.InPreviewMode Then Return Point.Round(Pony.PreviewWindowRectangle.Center())

        For i = 0 To 300
            Dim randomScreen = Options.Screens(Rng.Next(Options.Screens.Count))
            Dim teleportLocation = New Point(
                CInt(randomScreen.WorkingArea.X + Math.Round(Rng.NextDouble() * randomScreen.WorkingArea.Width)),
                CInt(randomScreen.WorkingArea.Y + Math.Round(Rng.NextDouble() * randomScreen.WorkingArea.Height)))
            If Not InAvoidanceArea(teleportLocation) Then Return teleportLocation
        Next

        Return Point.Empty
    End Function

    Friend Function GetDestinationDirectionHorizontal(destination As Vector2) As Direction
        Dim rightImageCenterX = CInt(TopLeftLocation.X + (Scale * CurrentBehavior.RightImage.Center.X))
        Dim leftImageCenterX = CInt(TopLeftLocation.X + (Scale * CurrentBehavior.LeftImage.Center.X))
        If (rightImageCenterX > destination.X AndAlso leftImageCenterX < destination.X) OrElse
            destination.X - CenterLocation.X <= 0 Then
            Return Direction.MiddleLeft
        Else
            Return Direction.MiddleRight
        End If
    End Function

    Friend Function GetDestinationDirectionVertical(destination As Vector2) As Direction
        Dim rightImageCenterY = CInt(TopLeftLocation.Y + (Scale * CurrentBehavior.RightImage.Center.Y))
        Dim leftImageCenterY = CInt(TopLeftLocation.Y + (Scale * CurrentBehavior.LeftImage.Center.Y))
        If (rightImageCenterY > destination.Y AndAlso leftImageCenterY < destination.Y) OrElse
           (rightImageCenterY < destination.Y AndAlso leftImageCenterY > destination.Y) OrElse
           destination.Y - CenterLocation.Y <= 0 Then
            Return Direction.TopCenter
        Else
            Return Direction.BottomCenter
        End If
    End Function

    Friend ReadOnly Property CurrentImageSize As Vector2
        Get
            Dim behavior = If(visualOverrideBehavior, CurrentBehavior)
            Return If(facingRight, behavior.RightImage.Size, behavior.LeftImage.Size)
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
        For Each interactionBase In InteractionBases
            Dim interaction = New Interaction(interactionBase)

            ' Get all actual instances of target ponies.
            interaction.Targets.UnionWith(otherPonies.Where(Function(p) interactionBase.TargetNames.Contains(p.Directory)))
            ' If no instances of the target ponies are present, we can forget this interaction.
            ' Alternatively, if it is specified all targets must be present but some are missing, the interaction cannot be used.
            If interaction.Targets.Count = 0 OrElse
                (interaction.Base.Activation = TargetActivation.All AndAlso
                interaction.Targets.Count <> interaction.Base.TargetNames.Count) Then
                Continue For
            End If

            ' Determine the common set of behaviors by name actually implemented by this pony and all discovered targets.
            Dim commonBehaviors As New HashSet(Of CaseInsensitiveString)(interactionBase.BehaviorNames)
            For Each pony In {Me}.Concat(otherPonies)
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
                targetPony.StartInteractionAsTarget(CurrentBehavior.Name, interaction)
            Next
        Else
            interaction.Trigger.StartInteractionAsTarget(CurrentBehavior.Name, interaction)
        End If
    End Sub

    Private Sub StartInteractionAsTarget(behaviorName As CaseInsensitiveString, interaction As Interaction)
        isInteractionInitiator = False
        IsInteracting = True
        CurrentInteraction = interaction
        SelectBehavior(Behaviors.First(Function(b) b.Name = behaviorName))
    End Sub

    Private Function GetReadiedInteraction() As Interaction
        'If we recently ran an interaction, don't start a new one until the delay expires.
        If internalTime < interactionDelayUntil Then
            Return Nothing
        End If

        For Each interaction In interactions
            For Each target As Pony In interaction.Targets
                ' Don't attempt to interact with a busy target, or with self.
                If target.IsInteracting OrElse ReferenceEquals(Me, target) Then Continue For

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

    Public ReadOnly Property IsSpeaking As Boolean Implements ISpeakingSprite.IsSpeaking
        Get
            Return internalTime - lastSpeakTime < TimeSpan.FromSeconds(2)
        End Get
    End Property

    Public ReadOnly Property SpeechText As String Implements ISpeakingSprite.SpeechText
        Get
            Return lastSpeakLine
        End Get
    End Property

    Public ReadOnly Property CurrentTime As TimeSpan Implements ISprite.CurrentTime
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
            Dim path = If(facingRight, behavior.RightImage.Path, behavior.LeftImage.Path)
            Diagnostics.Debug.Assert(Not String.IsNullOrEmpty(path))
            Return path
        End Get
    End Property

    Public ReadOnly Property Region As System.Drawing.Rectangle Implements ISprite.Region
        Get
            Diagnostics.Debug.Assert(CurrentBehavior IsNot Nothing)
            Dim width = CInt(CurrentImageSize.X * Options.ScaleFactor)
            Dim height = CInt(CurrentImageSize.Y * Options.ScaleFactor)
            Return New Rectangle(TopLeftLocation, New Size(width, height))
        End Get
    End Property

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
        CurrentBehavior = GetAppropriateBehaviorOrCurrent(appropriateMovement, ponySpeed)
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

    Public Shared Function TryLoad(iniLine As String, imageDirectory As String, pony As PonyBase, ByRef result As EffectBase, ByRef issues As ImmutableArray(Of ParseIssue)) As Boolean
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
        e.Name = p.NotNull()
        e.BehaviorName = p.NotNull()
        e.RightImage.Path = p.NoParse()
        If p.Assert(e.RightImage.Path, Function(s) Not String.IsNullOrEmpty(s), "An image path has not been set.", Nothing) Then
            e.RightImage.Path = p.SpecifiedCombinePath(imageDirectory, e.RightImage.Path, "Image will not be loaded.")
            p.SpecifiedFileExists(e.RightImage.Path, "Image will not be loaded.")
        End If
        e.LeftImage.Path = p.NoParse()
        If p.Assert(e.LeftImage.Path, Function(s) Not String.IsNullOrEmpty(s), "An image path has not been set.", Nothing) Then
            e.LeftImage.Path = p.SpecifiedCombinePath(imageDirectory, e.LeftImage.Path, "Image will not be loaded.")
            p.SpecifiedFileExists(e.LeftImage.Path, "Image will not be loaded.")
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
        Return p.AllParsingSuccessful
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
        Return {Referential.GetIssue("Behavior", BehaviorName, ParentPonyBase.Behaviors.Select(Function(b) b.Name))}.
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
    Implements ISprite
    Private Shared ReadOnly DirectionCount As Integer = [Enum].GetValues(GetType(Direction)).Length

    Private _base As EffectBase
    Public ReadOnly Property Base As EffectBase
        Get
            Return _base
        End Get
    End Property

    Private startTime As TimeSpan
    Private internalTime As TimeSpan

    Public Property OwningPony As Pony
    Public Property DesiredDuration As Double
    Public Property CloseOnNewBehavior As Boolean

    Public Property Location As Point
    Public Property TranslatedLocation As Point
    Public Property FacingLeft As Boolean
    Public Property BeingDragged As Boolean
    Public Property PlacementDirection As Direction
    Public Property Centering As Direction

    Public Sub New(base As EffectBase, startFacingLeft As Boolean)
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
    End Sub

    Public Sub Teleport()
        Dim screen = Options.Screens(Rng.Next(Options.Screens.Count))
        Location = New Point(
            CInt(screen.WorkingArea.X + Math.Round(Rng.NextDouble() * (screen.WorkingArea.Width - CurrentImageSize.Width), 0)),
            CInt(screen.WorkingArea.Y + Math.Round(Rng.NextDouble() * (screen.WorkingArea.Height - CurrentImageSize.Height), 0)))
    End Sub

    Public Function Center() As Point
        Dim scale As Double
        If OwningPony IsNot Nothing Then
            scale = OwningPony.Scale
        Else
            scale = 1
        End If

        Return New Point(CInt(Me.Location.X + ((scale * CurrentImageSize.Width) / 2)),
                         CInt(Me.Location.Y + ((scale * CurrentImageSize.Height) / 2)))
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
    End Sub

    Public Sub Update(updateTime As TimeSpan) Implements ISprite.Update
        internalTime = updateTime
        If BeingDragged Then
            Location = Pony.CursorLocation - New Size(CInt(CurrentImageSize.Width / 2), CInt(CurrentImageSize.Height / 2))
        End If
    End Sub

    Public ReadOnly Property CurrentTime As TimeSpan Implements ISprite.CurrentTime
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
            Return New Rectangle(Location, New Size(width, height))
        End Get
    End Property

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
        Dim fullDirectory = Path.Combine(Options.InstallLocation, RootDirectory, Directory)
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

    Private deployedPonies As New List(Of Pony)

    Private lastCycleTime As TimeSpan

    Private _houseBase As HouseBase
    Public ReadOnly Property HouseBase() As HouseBase
        Get
            Return _houseBase
        End Get
    End Property

    Public Sub New(houseBase As HouseBase)
        MyBase.New(houseBase, True)
        _houseBase = houseBase
        DesiredDuration = TimeSpan.FromDays(100).TotalSeconds
    End Sub

    Public Sub InitializeVisitorList()
        deployedPonies.Clear()
        For Each Pony As Pony In Pony.CurrentAnimator.Ponies()
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
                If deployedPonies.Count < HouseBase.MaximumPonies AndAlso Pony.CurrentAnimator.Ponies().Count < Options.MaxPonyCount Then
                    DeployPony(Me, ponyBases)
                Else
                    Console.WriteLine(Me.Base.Name & " - Cannot deploy. Pony limit reached.")
                End If
            Else
                If deployedPonies.Count > HouseBase.MinimumPonies AndAlso Pony.CurrentAnimator.Ponies().Count > 1 Then
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

        For Each p In Pony.CurrentAnimator.Ponies()
            choices.Remove(p.Directory)
        Next

        choices.Remove("Random Pony")

        If choices.Count = 0 Then
            Exit Sub
        End If

        Dim selected_name = choices(Rng.Next(choices.Count))

        For Each ponyBase In ponyBases
            If ponyBase.Directory = selected_name Then

                Dim deployed_pony = New Pony(ponyBase)

                deployed_pony.SelectBehavior()

                deployed_pony.TopLeftLocation = instance.Location + New Size(HouseBase.DoorPosition) - deployed_pony.GetImageCenterOffset()

                Dim groups As New List(Of Integer)
                Dim Alternate_Group_Behaviors As New List(Of Behavior)

                For Each Behavior In deployed_pony.Behaviors
                    If Not groups.Contains(Behavior.Group) Then groups.Add(Behavior.Group)

                    If Behavior.Group <> 0 AndAlso Behavior.Skip = False Then
                        Alternate_Group_Behaviors.Add(Behavior)
                    End If
                Next

                Dim selected_group = Rng.Next(groups.Count)

                If selected_group <> 0 AndAlso Alternate_Group_Behaviors.Count > 0 Then
                    deployed_pony.SelectBehavior(Alternate_Group_Behaviors(Rng.Next(Alternate_Group_Behaviors.Count)))
                End If

                Pony.CurrentAnimator.AddPony(deployed_pony)
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
                    For Each Pony As Pony In Pony.CurrentAnimator.Ponies()
                        choices.Add(Pony.Directory)
                    Next
                    all = True
                    Exit For
                End If
            Next

            If all = False Then
                For Each Pony As Pony In Pony.CurrentAnimator.Ponies()
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

        For Each pony As Pony In pony.CurrentAnimator.Ponies()
            If pony.Directory = selected_name Then

                If pony.IsInteracting Then Exit Sub
                If pony.BeingDragged Then Exit Sub

                If pony.Sleeping Then pony.WakeUp()

                pony.Destination = instance.Location + New Size(HouseBase.DoorPosition)
                pony.GoingHome = True
                pony.CurrentBehavior = pony.GetAppropriateBehaviorOrCurrent(AllowedMoves.All, False)
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
