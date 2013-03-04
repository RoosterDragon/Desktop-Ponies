Imports System.Globalization
Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Class PonyBase
    Public Const RootDirectory = "Ponies"
    Public Const ConfigFilename = "pony.ini"

    Sub New()
    End Sub

    Private _directory As String
    Public Property Directory() As String
        Get
            Return _directory
        End Get
        Set(value As String)
            _directory = value
        End Set
    End Property

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    Private _scale As Double
    Public ReadOnly Property Scale() As Double
        Get
            Return _scale
        End Get
    End Property

    Private ReadOnly _tags As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Public ReadOnly Property Tags() As HashSet(Of String)
        Get
            Return _tags
        End Get
    End Property

    Private ReadOnly _behaviorGroups As New List(Of BehaviorGroup)
    Public ReadOnly Property BehaviorGroups() As List(Of BehaviorGroup)
        Get
            Return _behaviorGroups
        End Get
    End Property

    Private ReadOnly _behaviors As New List(Of Behavior)
    Public ReadOnly Property Behaviors() As List(Of Behavior)
        Get
            Return _behaviors
        End Get
    End Property

    Private ReadOnly _interactions As New List(Of Interaction)
    Public ReadOnly Property Interactions() As List(Of Interaction)
        Get
            Return _interactions
        End Get
    End Property

    Private ReadOnly _speakingLines As New List(Of Behavior.SpeakingLine)
    Public ReadOnly Property SpeakingLines() As List(Of Behavior.SpeakingLine)
        Get
            Return _speakingLines
        End Get
    End Property

    Private _speakingLinesRandom As New List(Of Behavior.SpeakingLine)
    Public ReadOnly Property SpeakingLinesRandom() As List(Of Behavior.SpeakingLine)
        Get
            Return _speakingLinesRandom
        End Get
    End Property

    Private _speakingLinesSpecific As New List(Of Behavior.SpeakingLine)
    Public ReadOnly Property SpeakingLinesSpecific() As List(Of Behavior.SpeakingLine)
        Get
            Return _speakingLinesSpecific
        End Get
    End Property

    Public Sub New(directory As String)
        Argument.EnsureNotNull(directory, "directory")

        Dim lastSeparator = directory.LastIndexOfAny({Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar})
        If lastSeparator <> -1 Then
            _directory = directory.Substring(lastSeparator + 1)
        Else
            _directory = directory
        End If

        LoadFromIni()
    End Sub

    Private Sub LoadFromIni()
        Dim fullDirectory = Path.Combine(Options.InstallLocation, RootDirectory, Directory)
        Using configFile As New StreamReader(Path.Combine(fullDirectory, ConfigFilename))
            Dim behaviorNames As New List(Of String)
            Dim effectNames As New List(Of String)

            Do Until configFile.EndOfStream

                Dim line = configFile.ReadLine

                ' Ignore blank lines, and those commented out with a single quote.
                If String.IsNullOrWhiteSpace(line) OrElse line(0) = "'" Then Continue Do

                Dim columns = CommaSplitQuoteBraceQualified(line)

                If UBound(columns) < 1 Then Continue Do

                Select Case LCase(columns(0))
                    Case "name"
                        _name = columns(1)
                    Case "scale"
                        _scale = Double.Parse(columns(1), CultureInfo.InvariantCulture)
                    Case "behaviorgroup"
                        BehaviorGroups.Add(New PonyBase.BehaviorGroup(columns(2), Integer.Parse(columns(1), CultureInfo.InvariantCulture)))
                    Case "behavior"
                        behaviorNames.Add(line)
                    Case "categories"
                        For i = 1 To columns.Count - 1
                            For Each item As String In Main.Instance.FilterOptionsBox.Items
                                If String.Equals(item, columns(i), StringComparison.OrdinalIgnoreCase) Then
                                    Tags.Add(columns(i))
                                    Exit For
                                End If
                            Next
                        Next
                    Case "speak"
                        'Speak options can be in THREE forms:
                        '1 line text
                        'OR
                        '1 line name
                        '2 line text
                        '3 line sound file 
                        '4 skip for normal use (used for chains or interactions)
                        'OR
                        '1 line name
                        '2 line text
                        '3 {}'d list of sound files (the first one that works is used - this is to support other ports, like 'Browser Ponies' 
                        '4 skip for normal use (used for chains or interactions)

                        Try
                            Dim newLine As Behavior.SpeakingLine = Nothing
                            Select Case UBound(columns)
                                Case 1
                                    newLine = New Behavior.SpeakingLine(Name, "Unnamed", Replace(columns(1), ControlChars.Quote, ""),
                                                                        "", "", False, 0)
                                Case Is >= 4
                                    Dim sound_files_list_column = Replace(Replace(columns(3), "{", ""), "}", "")
                                    If IsNothing(sound_files_list_column) Then
                                        sound_files_list_column = ""
                                    End If

                                    Dim sound_files_list = CommaSplitQuoteQualified(sound_files_list_column)

                                    Dim group As Integer = 0

                                    If UBound(columns) = 5 Then
                                        group = Integer.Parse(columns(5), CultureInfo.InvariantCulture)
                                    End If

                                    If UBound(sound_files_list) > 0 Then
                                        Dim found_sound = False
                                        For Each soundfile_path In sound_files_list
                                            If File.Exists(Path.Combine(fullDirectory, soundfile_path)) Then
                                                newLine = New Behavior.SpeakingLine(Name, Trim(columns(1)),
                                                                                    Replace(columns(2), ControlChars.Quote, ""),
                                                                                    fullDirectory & Path.DirectorySeparatorChar,
                                                                                    Replace(Trim(soundfile_path), ControlChars.Quote, ""),
                                                                                    Boolean.Parse(Trim(columns(4))), group)
                                                found_sound = True
                                                Exit For
                                            End If
                                        Next

                                        If found_sound = False Then
                                            Throw New InvalidDataException("None of the listed sound files could be found.")
                                        End If
                                    Else
                                        newLine = New Behavior.SpeakingLine(Name, columns(1), Replace(columns(2), ControlChars.Quote, ""),
                                                                            "", "", Boolean.Parse(Trim(columns(4))), group)
                                    End If

                                Case Else
                                    MsgBox("Invalid 'speak' line in " & ConfigFilename & " file for pony named " & Name & ":" &
                                           ControlChars.NewLine & line & ControlChars.NewLine &
                                           "Line must contain a name for the entry, the text to be displayed, optional: soundfile, true if entry is for a specific behavior and should be skipped normally")
                            End Select
                            SpeakingLines.Add(newLine)
                        Catch ex As Exception
                            MsgBox("Invalid 'speak' line in " & ConfigFilename & " file for pony named " & Name & ":" & ControlChars.NewLine _
                             & line & ControlChars.NewLine & "Error: " & ex.Message)
                        End Try

                    Case "effect"
                        effectNames.Add(line)

                    Case Else
                        MsgBox("Unknown command in " & ConfigFilename & " for pony " & Name & ": " & columns(0) _
                               & ControlChars.NewLine & "Skipping line: " & _
                               ControlChars.NewLine & line)


                End Select
            Loop

            If Name = "" Then
                MsgBox("Couldn't find pony name in configuration file, poni.ini.  Skipping " & Directory)
            End If

            SetLines(SpeakingLines)

            'Now that we have a list of all the behaviors, process them
            For Each behaviorName In behaviorNames
                Try

                    Dim columns = CommaSplitQuoteQualified(behaviorName)

                    Dim movement As Pony.AllowedMoves

                    'movements are bytes so that they can be composite:
                    '"diagonal" means vertical AND horizontal at the same time.
                    'See the definition in the pony class for more information.
                    Select Case Trim(LCase(columns(Main.BehaviorOption.MovementType)))

                        Case "none"
                            movement = Pony.AllowedMoves.None
                        Case "horizontal_only"
                            movement = Pony.AllowedMoves.HorizontalOnly
                        Case "vertical_only"
                            movement = Pony.AllowedMoves.VerticalOnly
                        Case "horizontal_vertical"
                            movement = Pony.AllowedMoves.HorizontalVertical
                        Case "diagonal_only"
                            movement = Pony.AllowedMoves.DiagonalOnly
                        Case "diagonal_horizontal"
                            movement = Pony.AllowedMoves.DiagonalHorizontal
                        Case "diagonal_vertical"
                            movement = Pony.AllowedMoves.DiagonalVertical
                        Case "all"
                            movement = Pony.AllowedMoves.All
                        Case "mouseover"
                            movement = Pony.AllowedMoves.MouseOver
                        Case "sleep"
                            movement = Pony.AllowedMoves.Sleep
                        Case "dragged"
                            movement = Pony.AllowedMoves.Dragged
                        Case Else
                            MsgBox("Unknown movement type: " & columns(Main.BehaviorOption.MovementType) _
                                   & ControlChars.NewLine & "Skipping behavior " & columns(Main.BehaviorOption.Name) & " for " & Name)
                            Continue For
                    End Select

                    Dim linked_behavior As String = ""
                    Dim speak_start As String = ""
                    Dim speak_end As String = ""
                    Dim xcoord As Integer = 0
                    Dim ycoord As Integer = 0
                    Dim follow As String = ""
                    Dim follow_stopped_behavior As String = ""
                    Dim follow_moving_behavior As String = ""

                    Dim auto_select_images As Boolean = True
                    Dim skip As Boolean = False

                    Dim right_image_center As New Point
                    Dim left_image_center As New Point
                    Dim dont_repeat_image_animations As Boolean = False
                    Dim group As Integer = 0

                    If UBound(columns) > Main.BehaviorOption.MovementType Then

                        linked_behavior = Trim(columns(Main.BehaviorOption.LinkedBehavior))
                        speak_start = Trim(columns(Main.BehaviorOption.SpeakingStart))
                        speak_end = Trim(columns(Main.BehaviorOption.SpeakingEnd))
                        skip = Boolean.Parse(Trim(columns(Main.BehaviorOption.Skip)))
                        xcoord = Integer.Parse(columns(Main.BehaviorOption.XCoord), CultureInfo.InvariantCulture)
                        ycoord = Integer.Parse(columns(Main.BehaviorOption.YCoord), CultureInfo.InvariantCulture)
                        follow = LCase(Trim(columns(Main.BehaviorOption.ObjectToFollow)))
                        If UBound(columns) >= Main.BehaviorOption.AutoSelectImages Then
                            auto_select_images = Boolean.Parse(Trim(columns(Main.BehaviorOption.AutoSelectImages)))
                        End If
                        If UBound(columns) >= Main.BehaviorOption.FollowStoppedBehavior Then
                            follow_stopped_behavior = Trim(columns(Main.BehaviorOption.FollowStoppedBehavior))
                        End If
                        If UBound(columns) >= Main.BehaviorOption.FollowMovingBehavior Then
                            follow_moving_behavior = Trim(columns(Main.BehaviorOption.FollowMovingBehavior))
                        End If
                        If UBound(columns) >= Main.BehaviorOption.LeftImageCenter Then
                            Dim center = Split(Trim(columns(Main.BehaviorOption.RightImageCenter)), ",")
                            right_image_center = New Point(Integer.Parse(center(0), CultureInfo.InvariantCulture),
                                                           Integer.Parse(center(1), CultureInfo.InvariantCulture))
                            center = Split(Trim(columns(Main.BehaviorOption.LeftImageCenter)), ",")
                            left_image_center = New Point(Integer.Parse(center(0), CultureInfo.InvariantCulture),
                                                          Integer.Parse(center(1), CultureInfo.InvariantCulture))
                        End If

                        If UBound(columns) >= Main.BehaviorOption.DoNotRepeatImageAnimations Then
                            dont_repeat_image_animations = Boolean.Parse(Trim(columns(Main.BehaviorOption.DoNotRepeatImageAnimations)))
                        End If

                        If UBound(columns) >= Main.BehaviorOption.Group Then
                            group = Integer.Parse(columns(Main.BehaviorOption.Group), CultureInfo.InvariantCulture)
                        End If

                    End If


                    '                    Load images now?,  name,     , Probability, Max_Secs  , Min_Secs  , Speed     , image path, left image path, move_type, Linked behavior, speaking line_start, speaking line_end , skip normally unless processing links, x coord, ycoord, object to follow
                    AddBehavior(columns(Main.BehaviorOption.Name),
                                         Double.Parse(columns(Main.BehaviorOption.Probability), CultureInfo.InvariantCulture),
                                         Double.Parse(columns(Main.BehaviorOption.MaxDuration), CultureInfo.InvariantCulture),
                                         Double.Parse(columns(Main.BehaviorOption.MinDuration), CultureInfo.InvariantCulture),
                                         Double.Parse(columns(Main.BehaviorOption.Speed), CultureInfo.InvariantCulture),
                                         Path.Combine(fullDirectory, Trim(columns(Main.BehaviorOption.RightImagePath))),
                                         Path.Combine(fullDirectory, Trim(columns(Main.BehaviorOption.LeftImagePath))),
                                         movement, linked_behavior, speak_start, speak_end, skip, xcoord, ycoord,
                                         follow, auto_select_images, follow_stopped_behavior, follow_moving_behavior,
                                         right_image_center, left_image_center, dont_repeat_image_animations, group)

                Catch ex As Exception
                    If Main.Instance.auto_started = False Then
                        If TypeOf ex Is IndexOutOfRangeException Then
                            MsgBox("Warning:  You are missing a required parameter for pony " & Name & " in behavior:" & ControlChars.NewLine _
                            & behaviorName)
                        Else
                            MsgBox("Invalid behavior line in configuration file for pony " & Name & ":" & ControlChars.NewLine _
                           & behaviorName & ControlChars.NewLine & _
                           "Details: " & ex.Message)
                        End If
                    End If
                    Exit For

                End Try
            Next

            For Each effectName In effectNames

                Try

                    Dim columns = CommaSplitQuoteQualified(effectName)

                    '1 = effect name
                    '2 = behavior name
                    '3 = right image
                    '4 = left image
                    '5 = duration
                    '6 = delay before next
                    '7 = location relative to pony, right
                    '8 = center of effect, right
                    '9 = location going left
                    '10 = centering going left
                    '11 = effect follows pony
                    '12 = animations shouldn't repeat

                    Dim found_behavior As Boolean = False
                    For Each behavior In Behaviors

                        If behavior.Name = Trim(columns(2)) Then

                            Dim direction_right = Direction.Center
                            Dim centering_right = Direction.Center
                            Dim direction_left = Direction.Center
                            Dim centering_left = Direction.Center
                            Dim dont_repeat_image_animations As Boolean = False

                            Try
                                direction_right = Main.GetDirection(Trim(LCase(columns(7))))
                                centering_right = Main.GetDirection(Trim(LCase(columns(8))))
                                direction_left = Main.GetDirection(Trim(LCase(columns(9))))
                                centering_left = Main.GetDirection(Trim(LCase(columns(10))))

                            Catch ex As Exception
                                MsgBox("Invalid placement direction or centering for effect " & columns(1) & " for pony " & Name & ":" & ControlChars.NewLine & effectName)
                            End Try

                            If UBound(columns) >= 12 Then
                                dont_repeat_image_animations = Boolean.Parse(Trim(columns(12)))

                            Else
                                dont_repeat_image_animations = False
                            End If


                            Dim right_imagepath = Path.Combine(fullDirectory, Trim(columns(3)))
                            Dim left_imagepath = Path.Combine(fullDirectory, Trim(columns(4)))

                            behavior.AddEffect(columns(1), right_imagepath, left_imagepath,
                                               Double.Parse(columns(5), CultureInfo.InvariantCulture),
                                               Double.Parse(columns(6), CultureInfo.InvariantCulture),
                                               direction_right, centering_right, direction_left, centering_left,
                                               Boolean.Parse(Trim(columns(11))), dont_repeat_image_animations, Me)
                            found_behavior = True
                            Exit For

                        End If

                    Next

                    If Not found_behavior Then
                        MsgBox("Could not find behavior for effect " & columns(1) & " for pony " & Name & ":" & ControlChars.NewLine _
                           & effectName)
                    End If

                Catch ex As Exception
                    MsgBox("Invalid effect in configuration file for pony " & Name & ":" & ControlChars.NewLine _
                           & effectName & ControlChars.NewLine & _
                          "Details: " & ex.Message)
                End Try
            Next

            ' Behaviors that "chain" or link to another behavior to be played after they are done need to be set up now that we have a list
            ' of all of them.
            LinkBehaviors()
        End Using
    End Sub

    Friend Sub AddInteraction(interaction_name As String, name As String, probability As Double, proximity As String, _
                           target_list As String, target_selection As PonyBase.Interaction.TargetActivation, _
                           behaviorlist As String, repeat_delay As Integer, displaywarnings As Boolean)

        Dim new_interaction As New PonyBase.Interaction

        new_interaction.Name = interaction_name
        new_interaction.Targets_String = target_list
        new_interaction.Targets_Activated = target_selection
        new_interaction.PonyName = name
        new_interaction.Probability = probability
        new_interaction.ReactivationDelay = repeat_delay



        Select Case LCase(Trim(proximity))
            Case "default"
            Case Else
                Dim proximityValue As Double
                If Double.TryParse(proximity, NumberStyles.Float, CultureInfo.InvariantCulture, proximityValue) Then
                    new_interaction.Proximity_Activation_Distance = proximityValue
                Else
                    If Not Main.Instance.ScreensaverMode Then
                        Throw New ArgumentException("Invalid option for proximity. Enter either a number or 'default'." _
                                    & " Interaction file specified: '" & proximity & "'", proximity)
                    Else
                        Exit Sub
                    End If
                End If
        End Select

        Dim targets = CommaSplitQuoteQualified(target_list)
        Dim interaction_behaviors = CommaSplitQuoteQualified(behaviorlist)

        For Each iBehavior In interaction_behaviors

            Dim found = False

            For Each Behavior In Me.Behaviors
                If String.Equals(Trim(Behavior.Name), Trim(iBehavior), StringComparison.OrdinalIgnoreCase) Then
                    new_interaction.BehaviorList.Add(Behavior)
                    found = True
                End If
            Next
            If found = False AndAlso Options.DisplayPonyInteractionsErrors AndAlso Not Main.Instance.ScreensaverMode Then
                MsgBox("Warning: Pony '" & Me.Directory & "' does not have required behavior '" & iBehavior & "' for interaction: '" & _
                       interaction_name & "'. This interaction is disabled.")
                Exit Sub
            End If
        Next

        Dim ok_targets As New List(Of String)

        For Each target In targets

            Dim ponyfound = False

            For Each Pony In Main.Instance.SelectablePonies

                If String.Equals(Trim(target), Trim(Pony.Directory), StringComparison.OrdinalIgnoreCase) Then
                    ponyfound = True

                    ok_targets.Add(Pony.Directory)

                    For Each Behavior In interaction_behaviors

                        Dim found = False

                        For Each ponybehavior In Pony.Behaviors
                            If String.Equals(Trim(Behavior), Trim(ponybehavior.Name), StringComparison.OrdinalIgnoreCase) Then
                                found = True
                                Exit For
                            End If
                        Next

                        If found = False Then
                            ok_targets.Remove(Pony.Directory)
                            If displaywarnings AndAlso Not Main.Instance.ScreensaverMode Then
                                MsgBox("Warning:  Pony " & Pony.Name & " (" & Pony.Directory & ") " & _
                                " does not have required behavior '" & _
                               Behavior & "' as specified in interaction " & interaction_name & _
                               ControlChars.NewLine & "Interaction is disabled for this pony.")
                            End If
                        End If

                    Next
                End If
            Next

            If ponyfound = False AndAlso displaywarnings AndAlso Not Main.Instance.ScreensaverMode Then

                MsgBox("Warning: There is no pony with name " & target & " loaded.  Interaction '" & name & _
                       "' has this pony listed as a target.")
            End If
        Next

        'the displaywarnings = false part of the next line handles the case when we are viewing one pony in the editor.
        If (ok_targets.Count <> 0 AndAlso new_interaction.BehaviorList.Count <> 0) OrElse displaywarnings = False Then
            For Each PonyName In ok_targets
                new_interaction.InteractsWithByDirectory.Add(PonyName)
            Next

            Interactions.Add(new_interaction)
        End If

    End Sub

    Overloads Sub AddBehavior(name As String, chance As Double,
                           max_duration As Double, min_duration As Double, speed As Double,
                           right_image_path As String, left_image_path As String,
                           Allowed_Moves As Pony.AllowedMoves, _Linked_Behavior As String,
                           _Startline As String, _Endline As String, Optional _skip As Boolean = False,
                           Optional _xcoord As Integer = Nothing, Optional _ycoord As Integer = Nothing,
                           Optional _object_to_follow As String = "",
                           Optional _auto_select_images_on_follow As Boolean = True,
                           Optional _follow_stopped_behavior As String = "",
                           Optional _follow_moving_behavior As String = "",
                           Optional right_image_center As Point = Nothing, Optional left_image_center As Point = Nothing,
                           Optional _dont_repeat_image_animations As Boolean = False, Optional _group As Integer = 0)

        Dim new_behavior As New Behavior(right_image_path, left_image_path)

        If Not My.Computer.FileSystem.FileExists(right_image_path) Then
            Throw New FileNotFoundException("Image file does not exists for behavior " & name & " for pony " & Me.Directory & ". Path: " & right_image_path)
        End If

        If Not My.Computer.FileSystem.FileExists(left_image_path) Then
            Throw New FileNotFoundException("Image file does not exists for behavior " & name & " for pony " & Me.Directory & ". Path: " & left_image_path)
        End If

        new_behavior.Name = Trim(name)
        new_behavior.ChanceOfOccurance = chance
        new_behavior.MaxDuration = max_duration
        new_behavior.MinDuration = min_duration
        new_behavior.SetSpeed(speed)
        new_behavior.AllowedMovement = Allowed_Moves
        new_behavior.dont_repeat_image_animations = _dont_repeat_image_animations
        new_behavior.StartLineName = _Startline
        new_behavior.EndLineName = _Endline
        new_behavior.Group = _group
        new_behavior.Skip = _skip

        'These coordinates are either a position on the screen to go to, if no object to follow is specified,
        'or, the offset from the center of the object to go to (upper left, below, etc)
        new_behavior.AutoSelectImagesOnFollow = _auto_select_images_on_follow

        'When the pony if off-screen we overwrite the follow parameters to get them onscreen again.
        'we save the original parameters here.
        new_behavior.OriginalDestinationXCoord = _xcoord
        new_behavior.OriginalDestinationYCoord = _ycoord
        new_behavior.OriginalFollowObjectName = _object_to_follow

        new_behavior.FollowMovingBehaviorName = _follow_moving_behavior
        new_behavior.FollowStoppedBehaviorName = _follow_stopped_behavior

        If _Linked_Behavior <> "" Then
            'We just record the name of the linked behavior for now
            'Later, when we call "Link_Behaviors()" from the main form, we 
            'will get references to the actual behaviors.
            new_behavior.LinkedBehaviorName = _Linked_Behavior
        End If

        If right_image_center = Point.Empty Then
            new_behavior.SetRightImageCenter(new_behavior.RightImageSize / 2)
        Else
            new_behavior.SetRightImageCenter(right_image_center)
        End If

        If left_image_center = Point.Empty Then
            new_behavior.SetLeftImageCenter(new_behavior.LeftImageSize / 2)
        Else
            new_behavior.SetLeftImageCenter(left_image_center)
        End If

        Behaviors.Add(new_behavior)

    End Sub

    ''' <summary>
    ''' This overload is in case the editor happens upon a very incomplete pony that has no behaviors (wasn't created by the editor).
    ''' </summary>
    Overloads Sub AddBehavior(name As String, chance As Double, max_duration As Double, min_duration As Double, speed As Double,
                              Allowed_Moves As Pony.AllowedMoves, _Linked_Behavior As String, _Startline As String, _Endline As String)

        Dim new_behavior As New Behavior("", "")

        new_behavior.Name = Trim(name)
        new_behavior.ChanceOfOccurance = chance
        new_behavior.MaxDuration = max_duration
        new_behavior.MinDuration = min_duration
        new_behavior.SetSpeed(speed)
        new_behavior.AllowedMovement = Allowed_Moves

        new_behavior.StartLineName = _Startline
        new_behavior.EndLineName = _Endline

        If _Linked_Behavior <> "" Then
            'We just record the name of the linked behavior for now
            'Later, when we call "Link_Behaviors()" from the main form, we 
            'will get references to the actual behaviors.
            new_behavior.LinkedBehaviorName = _Linked_Behavior
        End If

        Behaviors.Add(new_behavior)

    End Sub

    ''' <summary>
    ''' Resolves links from behavior names to their actual Behavior objects.
    ''' </summary>
    Friend Sub LinkBehaviors()

        For Each behavior In Behaviors

            ' Link chained behaviors.
            If behavior.LinkedBehaviorName = "" OrElse behavior.LinkedBehaviorName = "None" Then
                behavior.LinkedBehavior = Nothing
            Else
                For Each otherBehavior In Behaviors
                    If String.Equals(behavior.LinkedBehaviorName, otherBehavior.Name, StringComparison.OrdinalIgnoreCase) Then
                        behavior.LinkedBehavior = otherBehavior
                        Exit For
                    End If
                Next
            End If

            ' Get start and end lines.
            For Each line In SpeakingLines
                If behavior.StartLineName <> "" AndAlso
                    String.Equals(line.Name.Trim(), behavior.StartLineName.Trim(), StringComparison.OrdinalIgnoreCase) Then
                    behavior.StartLine = line
                End If
                If behavior.EndLineName <> "" AndAlso
                    String.Equals(line.Name.Trim(), behavior.EndLineName.Trim(), StringComparison.OrdinalIgnoreCase) Then
                    behavior.EndLine = line
                End If
            Next

            ' Link following behaviors.
            If behavior.FollowStoppedBehaviorName <> "" Then
                For Each otherBehavior In Behaviors
                    If String.Equals(behavior.FollowStoppedBehaviorName, otherBehavior.Name, StringComparison.OrdinalIgnoreCase) Then
                        behavior.FollowStoppedBehavior = otherBehavior
                        Exit For
                    End If
                Next
            End If

            If behavior.FollowMovingBehaviorName <> "" Then
                For Each otherBehavior In Behaviors
                    If String.Equals(behavior.FollowMovingBehaviorName, otherBehavior.Name, StringComparison.OrdinalIgnoreCase) Then
                        behavior.FollowMovingBehavior = otherBehavior
                        Exit For
                    End If
                Next
            End If

        Next

    End Sub

    ''' <summary>
    ''' Resets the specific and random sets of speaking lines and repopulates them from the given collection of speaking lines.
    ''' </summary>
    ''' <param name="lines">The collection of speaking lines that should be used to repopulate the specific and random speaking lines.
    ''' </param>
    Friend Sub SetLines(lines As IEnumerable(Of Behavior.SpeakingLine))
        SpeakingLinesSpecific.Clear()
        SpeakingLinesRandom.Clear()

        For Each line In lines
            If line.Skip Then
                SpeakingLinesSpecific.Add(line)
            Else
                SpeakingLinesRandom.Add(line)
            End If
        Next
    End Sub

#Region "Interaction class"
    Public Class Interaction
        Public Const ConfigFilename = "interactions.ini"

        Friend Name As String
        Friend PonyName As String
        Friend Probability As Double
        Friend Proximity_Activation_Distance As Double = 125 'the distance to the target inside of which we start the interaction.

        Friend Targets_String As String = ""

        Friend Targets_Activated As TargetActivation
        Friend BehaviorList As New List(Of Behavior)

        Friend InteractsWith As New List(Of Pony)
        Friend InteractsWithByDirectory As New List(Of String)

        Friend Trigger As Pony = Nothing  'The pony we ran into that cause us to start
        Friend Initiator As Pony = Nothing 'The main pony than runs around waiting until she runs into a target.

        Friend ReactivationDelay As Integer = 60 'in seconds

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
    End Class
#End Region

#Region "Behavior class"
    Public Class Behavior

        Public Shared ReadOnly AnyGroup As Integer = 0

        Friend Name As String
        Friend ChanceOfOccurance As Double
        Friend MaxDuration As Double 'seconds
        Friend MinDuration As Double 'seconds

        Private right_image_path As String
        Private right_image_center As Vector2
        Private left_image_path As String
        Private left_image_center As Vector2
        Private left_image_size As Vector2
        Private right_image_size As Vector2
        Friend ReadOnly Property LeftImageCenter As Vector2
            Get
                Return left_image_center
            End Get
        End Property
        Friend ReadOnly Property RightImageCenter As Vector2
            Get
                Return right_image_center
            End Get
        End Property
        Friend ReadOnly Property LeftImageSize As Vector2
            Get
                Return left_image_size
            End Get
        End Property
        Friend ReadOnly Property RightImageSize As Vector2
            Get
                Return right_image_size
            End Get
        End Property
        Friend ReadOnly Property LeftImagePath As String
            Get
                Return left_image_path
            End Get
        End Property
        Friend ReadOnly Property RightImagePath As String
            Get
                Return right_image_path
            End Get
        End Property

        Private m_speed As Double
        Public ReadOnly Property Speed() As Double
            Get
                Return m_speed
            End Get
        End Property

        Friend Sub SetSpeed(speed As Double)
            m_speed = speed
        End Sub

        Friend dont_repeat_image_animations As Boolean = False

        Friend AllowedMovement As Pony.AllowedMoves

        Friend LinkedBehaviorName As String = ""
        Friend LinkedBehavior As Behavior = Nothing

        Friend StartLineName As String = ""
        Friend EndLineName As String = ""

        Friend StartLine As SpeakingLine = Nothing
        Friend EndLine As SpeakingLine = Nothing

        Friend Skip As Boolean = False

        'Friend destination_xcoord As Integer = 0
        'Friend destination_ycoord As Integer = 0
        'Friend follow_object_name As String = ""
        'Friend follow_object As ISprite

        Friend OriginalDestinationXCoord As Integer = 0
        Friend OriginalDestinationYCoord As Integer = 0
        Friend OriginalFollowObjectName As String = ""

        Friend FollowStoppedBehaviorName As String = ""
        Friend FollowMovingBehaviorName As String = ""
        Friend FollowStoppedBehavior As Behavior = Nothing
        Friend FollowMovingBehavior As Behavior = Nothing
        Friend AutoSelectImagesOnFollow As Boolean = True
        Friend Group As Integer = AnyGroup

        Friend Effects As New List(Of Effect)

        Public Sub New(rightImagePath As String, leftImagePath As String)
            If IsNothing(rightImagePath) AndAlso IsNothing(leftImagePath) Then Throw New ArgumentException("Both paths were null.")
            SetRightImagePath(rightImagePath)
            SetLeftImagePath(leftImagePath)
        End Sub

        Friend Sub SetRightImagePath(path As String)
            right_image_path = path
            right_image_size = Vector2.Zero
            If Not String.IsNullOrEmpty(right_image_path) Then
                right_image_size = New Vector2(ImageSize.GetSize(right_image_path))
            End If
        End Sub

        Friend Sub SetLeftImagePath(path As String)
            left_image_path = path
            left_image_size = Vector2.Zero
            If Not String.IsNullOrEmpty(left_image_path) Then
                left_image_size = New Vector2(ImageSize.GetSize(left_image_path))
            End If
        End Sub

        Friend Sub SetRightImageCenter(center As Point)
            right_image_center = center
        End Sub

        Friend Sub SetLeftImageCenter(center As Point)
            left_image_center = center
        End Sub

        Friend Sub AddEffect(effectname As String, right_path As String, left_path As String, duration As Double, repeat_delay As Double,
                             direction_right As Direction, centering_right As Direction,
                             direction_left As Direction, centering_left As Direction,
                             follow As Boolean, _dont_repeat_image_animations As Boolean, owner As PonyBase)

            Dim new_effect As New Effect(right_path, left_path)

            new_effect.BehaviorName = Me.Name
            new_effect.Name = effectname
            new_effect.Duration = duration
            new_effect.Repeat_Delay = repeat_delay
            new_effect.placement_direction_right = direction_right
            new_effect.centering_right = centering_right
            new_effect.placement_direction_left = direction_left
            new_effect.centering_left = centering_left
            new_effect.follow = follow
            new_effect.dont_repeat_image_animations = _dont_repeat_image_animations
            new_effect.OwnerPony = owner

            Effects.Add(new_effect)

        End Sub

        Class SpeakingLine

            Friend Name As String = ""
            Friend Text As String = ""
            Friend SoundFile As String = ""
            Friend Skip As Boolean = False 'don't use randomly if true
            Friend Group As Integer = 0 'the behavior group that this line is assigned to.  0 = all

            Friend Sub New(ponyname As String, _name As String, _text As String, _path As String, _soundfile As String, _skip As Boolean, _group As Integer)

                Name = _name
                Text = _text
                Skip = _skip
                Group = _group

                If _soundfile <> "" AndAlso Not My.Computer.FileSystem.FileExists(_path & _soundfile) Then
                    MsgBox("Error loading sound file for speaking line " & Name & " for pony " & ponyname & ControlChars.NewLine _
                           & "Sound file: " & SoundFile & " does not exist. (Speaking_Line.New())")
                    Exit Sub
                End If

                If _soundfile <> "" Then
                    SoundFile = _path & _soundfile
                End If

            End Sub
        End Class
    End Class
#End Region

#Region "BehaviorGroup class"
    Public Class BehaviorGroup

        Friend Name As String = ""
        Friend Number As Integer = -1

        Sub New(_name As String, _number As Integer)
            Name = _name
            Number = _number
        End Sub

    End Class
#End Region

End Class

Class Pony
    Implements ISpeakingSprite

    ''' <summary>
    ''' Number of milliseconds by which the internal temporal state of the sprite should be advanced with each call to UpdateOnce().
    ''' </summary>
    Private Const StepRate = 1000.0 / 30.0

    Friend Shared CursorLocation As Point
    Friend Shared CurrentAnimator As DesktopPonyAnimator
    Friend Shared CurrentViewer As ISpriteCollectionView
    Friend Shared PreviewWindowRectangle As Rectangle

#Region "DEBUG conditional code"
#If DEBUG Then
    Friend UpdateRecord As New List(Of Record)

    Private Enum RecordKind
        Unspecified
        ShouldBeSleeping
        SelectedNewRandomBehavior
        RepeatedCurrentBehavior
        SelectedChainedBehavior
        MouseOverToggle
        StoppedReturningToScreenArea
        TruncateBehavior
        UnspecifiedPaint
        UnspecifiedBounce
        Teleport
    End Enum

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
#End If

    <Diagnostics.Conditional("DEBUG")>
    Private Sub AddUpdateRecord(info As String)
#If DEBUG Then
        SyncLock UpdateRecord
            UpdateRecord.Add(New Record(internalTime, info))
        End SyncLock
#End If
    End Sub

    <Diagnostics.Conditional("DEBUG")>
    Private Sub AddUpdateRecord(info As String, info2 As String)
#If DEBUG Then
        SyncLock UpdateRecord
            UpdateRecord.Add(New Record(internalTime, info + info2))
        End SyncLock
#End If
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
    Public ReadOnly Property Name() As String
        Get
            Return Base.Name
        End Get
    End Property
    Friend ReadOnly Property Tags() As HashSet(Of String)
        Get
            Return Base.Tags
        End Get
    End Property
    Friend ReadOnly Property BehaviorGroups() As List(Of PonyBase.BehaviorGroup)
        Get
            Return Base.BehaviorGroups
        End Get
    End Property
    Friend ReadOnly Property Behaviors() As List(Of PonyBase.Behavior)
        Get
            Return Base.Behaviors
        End Get
    End Property
    Friend ReadOnly Property Interactions() As List(Of PonyBase.Interaction)
        Get
            Return Base.Interactions
        End Get
    End Property
#End Region

    Public Property ShouldBeSleeping As Boolean
    Private _sleeping As Boolean
    Public Property Sleeping() As Boolean
        Get
            Return _sleeping
        End Get
        Private Set(ByVal value As Boolean)
            _sleeping = value
        End Set
    End Property

    Public Property BeingDragged() As Boolean

    Friend CurrentBehaviorGroup As Integer

    Friend Interaction_Active As Boolean = False
    Private _currentInteraction As PonyBase.Interaction = Nothing
    Friend Property CurrentInteraction As PonyBase.Interaction
        Get
            Return _currentInteraction
        End Get
        Private Set(value As PonyBase.Interaction)
            _currentInteraction = value
        End Set
    End Property
    Private IsInteractionInitiator As Boolean = False

    Friend IsInteracting As Boolean = False

    Friend PlayingGame As Boolean = False

    Private verticalMovementAllowed As Boolean = False
    Private horizontalMovementAllowed As Boolean = False
    Friend facingUp As Boolean = False
    Friend facingRight As Boolean = True
    ''' <summary>
    ''' The angle to travel in, if moving diagonally (in radians).
    ''' </summary>
    Friend diagonal As Double = 0

    ''' <summary>
    ''' Time until interactions should be disabled.
    ''' Stops interactions from repeating too soon after one another.
    ''' Only affects the triggering pony and not targets.
    ''' </summary>
    ''' <remarks></remarks>
    Private interactionDelayUntil As TimeSpan

    Private _currentBehavior As PonyBase.Behavior
    Public Property CurrentBehavior As PonyBase.Behavior
        Get
            Return _currentBehavior
        End Get
        Friend Set(value As PonyBase.Behavior)
            Diagnostics.Debug.Assert(value IsNot Nothing)
            _currentBehavior = value
            SetAllowableDirections()
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
    Private previousBehavior As PonyBase.Behavior

    ''' <summary>
    ''' When set, specifics the alternate set of images that should replace those of the current behavior.
    ''' </summary>
    Friend visualOverrideBehavior As PonyBase.Behavior

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
    Friend GoingHome As Boolean = False
    ''' <summary>
    ''' Used when a pony has been recalled and is just about to "enter" a house
    ''' </summary>
    Friend OpeningDoor As Boolean = False

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

    Friend Destination As Vector2
    Friend AtDestination As Boolean = False
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
    Friend TopLeftLocation As Point

    ''' <summary>
    ''' Used for predicting future movement (just more of what we last did)
    ''' </summary>
    Private LastMovement As Vector2F

    Friend ActiveEffects As New List(Of Effect)

    'User has the option of limiting songs to one-total at a time, or one-per-pony at a time.
    'these two options are used for the one-per-pony option.
    Private AudioLastPlayed As Date = DateTime.UtcNow
    Private LastAudioLength As Integer = 0

    Friend BehaviorStartTime As TimeSpan
    Friend BehaviorDesiredDuration As TimeSpan

    Friend ManualControlPlayerOne As Boolean
    Friend ManualControlPlayerTwo As Boolean
    Private effectsToRemove As New List(Of Effect)

    Private ReadOnly EffectsLastUsed As New Dictionary(Of Effect, TimeSpan)

    Friend destinationCoords As Point
    Friend followObjectName As String = ""
    Friend followObject As ISprite
    'Try to get the point where an object is going to, and go to that instead of where it is currently at.
    Friend leadTarget As Boolean

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
#End Region

    Public ReadOnly Property Scale() As Double
        Get
            Return If(Base.Scale <> 0, Base.Scale, Options.ScaleFactor)
        End Get
    End Property

    <Flags()>
    Friend Enum AllowedMoves As Byte
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

    Public Sub New(base As PonyBase)
        Argument.EnsureNotNull(base, "base")
        _base = base
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
        If Main.Instance.InPreviewMode Then
            TopLeftLocation = Point.Add(Pony.PreviewWindowRectangle.Location, New Size(10, 10))
            Exit Sub
        End If

        ' Try an arbitrary number of times to find a point a point in bounds that is not also in the exclusion zone.
        ' TODO: Create method that will uniformly choose a random location from allowable points, also taking into account image sizing.
        Dim screens = Main.Instance.ScreensToUse
        Dim teleportLocation As Point
        For tries = 0 To 300
            Dim area = screens(Rng.Next(screens.Count)).WorkingArea
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
                                    If(CurrentBehavior.LinkedBehavior IsNot Nothing, CurrentBehavior.LinkedBehavior.Name, "<null>"))
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

        Dim sleepBehavior As PonyBase.Behavior = Nothing
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

        If IsInteractionInitiator Then
            For Each pony In CurrentInteraction.InteractsWith
                ' Check the target is still running the interaction that the current pony initiated, then cancel it.
                If Not ReferenceEquals(Me, pony) AndAlso
                    pony.CurrentInteraction IsNot Nothing AndAlso
                    pony.CurrentInteraction.Initiator IsNot Nothing AndAlso
                    ReferenceEquals(Me, pony.CurrentInteraction.Initiator) Then
                    pony.CancelInteraction()
                End If
            Next
        End If

        interactionDelayUntil = internalTime + TimeSpan.FromSeconds(CurrentInteraction.ReactivationDelay)
        CurrentInteraction = Nothing
        IsInteractionInitiator = False
    End Sub

    ''' <summary>
    ''' Ends the current behavior and begins a new behavior. One is chosen at random unless a behavior is specified.
    ''' </summary>
    ''' <param name="specifiedBehavior">The behavior that the pony should switch to, or null to choose one at random.</param>
    Public Sub SelectBehavior(Optional specifiedBehavior As PonyBase.Behavior = Nothing)
        ' Having no specified behavior when interacting means we've run to the last part of a chain and should end the interaction.
        If IsInteracting AndAlso IsInteractionInitiator AndAlso specifiedBehavior Is Nothing Then CancelInteraction()

        ' Clear following state.
        followObject = Nothing
        followObjectName = ""

        If specifiedBehavior Is Nothing Then
            ' Pick a behavior at random. If a valid behavior cannot be selected after an arbitrary number of tries, just continue using the
            ' current behavior for now.
            Dim foundAtRandom As Boolean
            For i = 0 To 200
                Dim potentialBehavior = Behaviors(Rng.Next(Behaviors.Count))

                ' The behavior can't be disallowed from running randomly, and it must in in the same group or the "any" group.
                ' Then, do a random test against the chance the behavior can occur.
                If Not potentialBehavior.Skip AndAlso
                    (potentialBehavior.Group = CurrentBehaviorGroup OrElse potentialBehavior.Group = PonyBase.Behavior.AnyGroup) AndAlso
                    Rng.NextDouble() <= potentialBehavior.ChanceOfOccurance Then

                    ' See if the behavior specifies that we follow another object.
                    followObjectName = potentialBehavior.OriginalFollowObjectName
                    Destination = DetermineDestination()

                    ' The behavior specifies an object to follow, but no instance of that object is present.
                    ' We can't use this behavior, so we'll have to choose another.
                    If Not hasDestination AndAlso potentialBehavior.OriginalFollowObjectName <> "" Then
                        followObjectName = ""
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
            If Not foundAtRandom AndAlso IsInteracting AndAlso specifiedBehavior Is Nothing Then
                CurrentBehavior = Behaviors(0)
                AddUpdateRecord(
                    "Selected the default behavior as random selection failed (SelectBehavior). Behavior: ", CurrentBehavior.Name)
            End If
        Else
            followObjectName = specifiedBehavior.OriginalFollowObjectName
            Destination = DetermineDestination()

            ' The behavior specifies an object to follow, but no instance of that object is present.
            ' We can't use this behavior, so we'll have to choose another at random.
            If Not hasDestination AndAlso specifiedBehavior.OriginalFollowObjectName <> "" AndAlso
                Not Main.Instance.InPreviewMode Then
                SelectBehavior()
                Exit Sub
            End If
            CurrentBehavior = specifiedBehavior
            AddUpdateRecord("Selected a specified behavior (SelectBehavior). Behavior: ", CurrentBehavior.Name)
        End If

        CurrentBehaviorGroup = CurrentBehavior.Group

        ' Reset effects.
        ' TODO: Make an immutable effect base from which new instances are spawned, as they are currently cloned...
        For Each effect In CurrentBehavior.Effects
            effect.already_played_for_currentbehavior = False
        Next

        BehaviorStartTime = internalTime
        BehaviorDesiredDuration = TimeSpan.FromSeconds(
            (Rng.NextDouble() * (CurrentBehavior.MaxDuration - CurrentBehavior.MinDuration) + CurrentBehavior.MinDuration))

        ' Speak the starting line now, if one is specified; otherwise speak a random line by chance, but only if it won't get in the way
        ' later.
        If CurrentBehavior.StartLine IsNot Nothing Then
            PonySpeak(CurrentBehavior.StartLine)
        ElseIf CurrentBehavior.EndLine Is Nothing AndAlso followObjectName = "" AndAlso
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
        Dim possibleMoveModes As New List(Of AllowedMoves)(3)
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
                diagonal = 0
            Case AllowedMoves.HorizontalOnly
                horizontalMovementAllowed = True
                verticalMovementAllowed = False
                diagonal = 0
            Case AllowedMoves.VerticalOnly
                horizontalMovementAllowed = False
                verticalMovementAllowed = True
                diagonal = 0
            Case AllowedMoves.DiagonalOnly
                horizontalMovementAllowed = True
                verticalMovementAllowed = True
                ' Pick a random angle to travel at.
                If facingUp Then
                    diagonal = ((Rng.NextDouble() * 35) + 15) * (Math.PI / 180)
                Else
                    diagonal = ((Rng.NextDouble() * 35) + 310) * (Math.PI / 180)
                End If
                If Not facingRight Then diagonal = Math.PI - diagonal
        End Select
    End Sub

    ''' <summary>
    ''' Prompts the pony to speak a line if it has not done so recently. A random line is chosen unless one is specified.
    ''' </summary>
    ''' <param name="line">The line the pony should speak, or null to choose one at random.</param>
    Public Sub PonySpeak(Optional line As PonyBase.Behavior.SpeakingLine = Nothing)
        'When the cursor is over us, don't talk too often.
        If CursorOverPony AndAlso (internalTime - lastSpeakTime).TotalSeconds < 15 Then
            Exit Sub
        End If

        ' Select a line at random from the lines that may be played at random that are in the current group.
        If line Is Nothing Then
            If Base.SpeakingLinesRandom.Count = 0 Then
                Exit Sub
            Else
                Dim randomGroupLines = Base.SpeakingLinesRandom.Where(
                    Function(l) l.Group = 0 OrElse l.Group = CurrentBehavior.Group).ToArray()
                If randomGroupLines.Length = 0 Then Exit Sub
                line = randomGroupLines(Rng.Next(randomGroupLines.Count))
            End If
        End If

        ' Set the line text to be displayed.
        If Options.PonySpeechEnabled Then
            lastSpeakTime = internalTime
            lastSpeakLine = Me.Name & ": " & ControlChars.Quote & line.Text & ControlChars.Quote
        End If

        ' Start the sound file playing.
        If line.SoundFile <> "" AndAlso Main.Instance.DirectXSoundAvailable Then
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
        If Main.Instance.ScreensaverMode AndAlso Not Options.ScreensaverSoundEnabled Then Exit Sub

        ' Don't play sounds over other ones - wait until they finish.
        If Not Options.SoundSingleChannelOnly Then
            If DateTime.UtcNow.Subtract(Me.AudioLastPlayed).TotalMilliseconds <= Me.LastAudioLength Then Exit Sub
        Else
            If DateTime.UtcNow.Subtract(Main.Instance.Audio_Last_Played).TotalMilliseconds <= Main.Instance.Last_Audio_Length Then Exit Sub
        End If

        ' Quick sanity check that the file exists on disk.
        If Not My.Computer.FileSystem.FileExists(filePath) Then Exit Sub

        ' If you get a MDA warning about loader locking - you'll just have to disable that exception message.  
        ' Apparently it is a bug with DirectX that only occurs with Visual Studio...
        ' We use DirectX now so that we can use MP3 instead of WAV files
        Dim audio As New Microsoft.DirectX.AudioVideoPlayback.Audio(filePath)
        Try
            Main.Instance.ActiveSounds.Add(audio)

            ' Volume is between -10000 and 0, with 0 being the loudest.
            audio.Volume = CInt(Options.SoundVolume * 10000 - 10000)
            audio.Play()

            If Not Options.SoundSingleChannelOnly Then
                Me.LastAudioLength = CInt(audio.Duration * 1000)
                Me.AudioLastPlayed = DateTime.UtcNow
            Else
                Main.Instance.Last_Audio_Length = CInt(audio.Duration * 1000) 'to milliseconds
                Main.Instance.Audio_Last_Played = DateTime.UtcNow
            End If
        Catch ex As Exception
            If Not Main.Instance.AudioErrorShown AndAlso Not Main.Instance.ScreensaverMode Then
                Main.Instance.AudioErrorShown = True
                MessageBox.Show(String.Format(CultureInfo.CurrentCulture,
                                              "There was an error trying to play a sound. Maybe the file is corrupt?{0}" &
                                              "You will not receive further notifications about sound errors.{0}{0}" &
                                              "File: {1}{0}" &
                                              "Pony: {2}{0}" &
                                              "{3}", vbNewLine, filePath, Directory, ex),
                                          "Sound Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                    For Each effect In CurrentBehavior.Effects
                        effect.already_played_for_currentbehavior = False
                    Next
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

        blocked = False

        If CursorImmunity > 0 Then CursorImmunity -= 1

        If ReturningToScreenArea AndAlso Options.PonyTeleportEnabled Then
            StopReturningToScreenArea()
            AddUpdateRecord("Stopped returning to screen area: Teleport option is enabled.")
            Exit Sub
        End If

        Dim speed As Double = CurrentBehavior.Speed * Scale

        If Main.Instance.CurrentGame Is Nothing OrElse
            (Main.Instance.CurrentGame IsNot Nothing AndAlso
             Main.Instance.CurrentGame.Status <> Game.GameStatus.Setup) Then
            With Main.Instance
                ' User input will dictate our movement.
                If ManualControlPlayerOne Then
                    speed = ManualControl(.PonyAction, .PonyUp, .PonyDown, .PonyLeft, .PonyRight, .PonySpeed)
                ElseIf ManualControlPlayerTwo Then
                    speed = ManualControl(.PonyAction_2, .PonyUp_2, .PonyDown_2, .PonyLeft_2, .PonyRight_2, .PonySpeed_2)
                End If
            End With
        End If

        'If the behavior specified a follow object, or a point to go to, figure out where that is.
        Destination = DetermineDestination()

        Dim movement As Vector2F

        ' Don't follow a destination if we are under player control unless there is a game playing and it is in setup mode.
        Dim distance As Double
        If hasDestination AndAlso
            ((Not ManualControlPlayerOne AndAlso Not ManualControlPlayerTwo) OrElse
             (Main.Instance.CurrentGame IsNot Nothing AndAlso Main.Instance.CurrentGame.Status = Game.GameStatus.Setup)) Then
            ' A destination has been specified and the pony should head there.
            distance = Vector.Distance(CenterLocation, Destination)
            ' Avoid division by zero.
            If distance = 0 Then distance = 1

            If GetDestinationDirectionHorizontal(Destination) = Direction.Left Then
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
            If diagonal <> 0 Then
                'Opposite = Hypotenuse * cosine of the angle
                movement.X = CSng(Math.Sqrt((speed ^ 2) * 2) * Math.Cos(diagonal))
                'Adjacent = Hypotenuse * cosine of the angle
                '(negative because we are using pixel coordinates - down is positive)
                movement.Y = CSng(-Math.Sqrt((speed ^ 2) * 2) * Math.Sin(diagonal))
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

        Dim newTopLeftLocation = Point.Round(CType(TopLeftLocation, PointF) + movement)

        Dim isNearCursorNow = IsPonyNearMouseCursor(TopLeftLocation)
        Dim isNearCursorFuture = IsPonyNearMouseCursor(newTopLeftLocation)

        Dim isOnscreenNow = IsPonyOnScreen(TopLeftLocation, Main.Instance.ScreensToUse)
        Dim isOnscreenFuture = IsPonyOnScreen(newTopLeftLocation, Main.Instance.ScreensToUse)

        ' TODO: Refactor and extract.
        'Dim playingGameAndOutOfBounds = PlayingGame AndAlso
        '    Main.Instance.CurrentGame.Status <> Game.GameStatus.Setup AndAlso
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
                Else
                    CurrentBehavior = Nothing
                End If
                AddUpdateRecord("Avoiding window.")
                Exit Sub
            End If

            ' Everything's cool. Move and repaint.
            TopLeftLocation = newTopLeftLocation
            LastMovement = movement

            Dim useVisualOverride = followObject IsNot Nothing AndAlso
                (CurrentBehavior.AutoSelectImagesOnFollow OrElse
                 CurrentBehavior.FollowMovingBehavior IsNot Nothing OrElse
                 CurrentBehavior.FollowStoppedBehavior IsNot Nothing)
            Paint(useVisualOverride)
            AddUpdateRecord("Standard paint. VisualOverride: ", useVisualOverride.ToString())

            ' If we can, we should try and start an interaction.
            If Options.PonyInteractionsEnabled AndAlso Not IsInteracting AndAlso Not ReturningToScreenArea Then
                Dim interact As PonyBase.Interaction = GetReadiedInteraction()
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
                    (InAvoidanceArea(Destination) OrElse Not IsPonyOnScreen(Destination, Main.Instance.ScreensToUse)) Then
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
                If Main.Instance.InPreviewMode OrElse Options.PonyTeleportEnabled Then
                    Teleport()
                    AddUpdateRecord("Teleporting back onscreen.")
                    Exit Sub
                End If

                Dim safespot = FindSafeDestination()
                ReturningToScreenArea = True

                If CurrentBehavior.Speed = 0 Then
                    CurrentBehavior = GetAppropriateBehaviorOrCurrent(AllowedMoves.All, True)
                End If

                followObject = Nothing
                followObjectName = ""
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
            If IsNothing(followObject) Then
                'CurrentBehavior = Nothing
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
        If (followObjectName <> "" AndAlso IsNothing(followObject)) Then
            ' If we are interacting, and the name of the pony we should be following matches that of the trigger, follow that one.
            ' Otherwise, we may end up following the wrong copy if there are more than one.
            If IsInteracting AndAlso
                String.Equals(Trim(followObjectName), Trim(CurrentInteraction.Trigger.Directory), StringComparison.OrdinalIgnoreCase) Then
                followObject = CurrentInteraction.Trigger
                Return New Point(CurrentInteraction.Trigger.CenterLocation.X + destinationCoords.X,
                                 CurrentInteraction.Trigger.CenterLocation.Y + destinationCoords.Y)
            End If
            ' For the reverse case of a trigger pony trying to find out which initiator to follow when interacting.
            If IsInteracting AndAlso Not IsNothing(CurrentInteraction.Initiator) AndAlso
                String.Equals(Trim(followObjectName), Trim(CurrentInteraction.Initiator.Directory), StringComparison.OrdinalIgnoreCase) Then
                followObject = CurrentInteraction.Initiator
                Return New Point(CurrentInteraction.Initiator.TopLeftLocation.X + destinationCoords.X,
                                 CurrentInteraction.Initiator.TopLeftLocation.Y + destinationCoords.Y)
            End If

            ' If not interacting, or following a different pony, we need to figure out which ones and follow one at random.
            Dim poniesToFollow As New List(Of Pony)
            For Each ponyToFollow In CurrentAnimator.Ponies()
                If String.Equals(ponyToFollow.Directory, followObjectName, StringComparison.OrdinalIgnoreCase) Then
                    poniesToFollow.Add(ponyToFollow)
                End If
            Next
            If poniesToFollow.Count <> 0 Then
                Dim ponyToFollow = poniesToFollow(Rng.Next(poniesToFollow.Count))
                followObject = ponyToFollow
                Return New Point(ponyToFollow.TopLeftLocation.X + destinationCoords.X,
                                 ponyToFollow.TopLeftLocation.Y + destinationCoords.Y)
            End If


            ' We may be following an effect instead.
            Dim effectsToFollow As New List(Of Effect)
            For Each effect In CurrentAnimator.Effects()
                If String.Equals(effect.Name, followObjectName, StringComparison.OrdinalIgnoreCase) Then
                    effectsToFollow.Add(effect)
                End If
            Next
            If effectsToFollow.Count <> 0 Then
                Dim dice = Rng.Next(effectsToFollow.Count)
                followObject = effectsToFollow(dice)
                Return New Point(effectsToFollow(dice).Location.X + destinationCoords.X,
                                 effectsToFollow(dice).Location.Y + destinationCoords.Y)
            End If

            ' We can't find the object to follow, so specify no destination.
            Return Point.Empty
        End If

        If followObject IsNot Nothing Then
            ' We've already selected an object to follow previously.
            Dim followPony = TryCast(followObject, Pony)
            If followPony IsNot Nothing Then
                If leadTarget Then
                    Return followPony.FutureLocation()
                Else
                    Return New Point(CInt(followPony.CenterLocation.X + (followPony.Scale * destinationCoords.X)), _
                                     CInt(followPony.CenterLocation.Y + (followPony.Scale * destinationCoords.Y)))
                End If
            Else
                Dim followEffect As Effect = DirectCast(followObject, Effect)
                Return New Point(followEffect.Center.X + destinationCoords.X, followEffect.Center.Y + destinationCoords.Y)
            End If
        End If

        ' We are not following an object, but going to a point on the screen.
        If destinationCoords.X <> 0 AndAlso destinationCoords.Y <> 0 Then
            Return New Point(CInt(0.01 * destinationCoords.X), CInt(0.01 * destinationCoords.Y))
        End If

        ' We have no given destination.
        Return Point.Empty
    End Function

    Private Sub StopReturningToScreenArea()
        ReturningToScreenArea = False
        destinationCoords = New Point(CurrentBehavior.OriginalDestinationXCoord, CurrentBehavior.OriginalDestinationYCoord)
        followObjectName = CurrentBehavior.OriginalFollowObjectName
        Paint()
    End Sub

    Friend Sub ActivateEffects(currentTime As TimeSpan)

        If Options.PonyEffectsEnabled AndAlso Sleeping = False _
          AndAlso Main.Instance.Dragging = False AndAlso ReturningToScreenArea = False Then

            For Each effect In CurrentBehavior.Effects
                If Not EffectsLastUsed.ContainsKey(effect) Then
                    EffectsLastUsed(effect) = TimeSpan.Zero
                End If
                If (currentTime - EffectsLastUsed(effect)).TotalMilliseconds >= effect.Repeat_Delay * 1000 Then

                    If effect.Repeat_Delay = 0 Then
                        If effect.already_played_for_currentbehavior = True Then Continue For
                    End If

                    effect.already_played_for_currentbehavior = True

                    Dim new_effect As Effect = effect.Clone()

                    If new_effect.Duration <> 0 Then
                        new_effect.DesiredDuration = new_effect.Duration
                        new_effect.CloseOnNewBehavior = False
                    Else
                        If Me.HaltedForCursor Then
                            new_effect.DesiredDuration = TimeSpan.FromSeconds(CurrentBehavior.MaxDuration).TotalSeconds
                        Else
                            new_effect.DesiredDuration = (BehaviorDesiredDuration - Me.CurrentTime).TotalSeconds
                        End If
                        new_effect.CloseOnNewBehavior = True
                    End If

                    'new_effect.Text = Name & "'s " & new_effect.name

                    If facingRight Then
                        new_effect.direction = new_effect.placement_direction_right
                        new_effect.centering = new_effect.centering_right
                        new_effect.current_image_path = new_effect.right_image_path
                    Else
                        new_effect.direction = new_effect.placement_direction_left
                        new_effect.centering = new_effect.centering_left
                        new_effect.current_image_path = new_effect.left_image_path
                    End If

                    Dim directionsCount = [Enum].GetValues(GetType(Direction)).Length

                    If new_effect.direction = Direction.Random Then
                        new_effect.direction = CType(Math.Round(Rng.NextDouble() * directionsCount - 2, 0), Direction)
                    End If
                    If new_effect.centering = Direction.Random Then
                        new_effect.centering = CType(Math.Round(Rng.NextDouble() * directionsCount - 2, 0), Direction)
                    End If

                    If new_effect.direction = Direction.RandomNotCenter Then
                        new_effect.direction = CType(Math.Round(Rng.NextDouble() * directionsCount - 3, 0), Direction)
                    End If
                    If new_effect.centering = Direction.RandomNotCenter Then
                        new_effect.centering = CType(Math.Round(Rng.NextDouble() * directionsCount - 3, 0), Direction)
                    End If

                    If (facingRight) Then
                        new_effect.Facing_Left = False
                    Else
                        new_effect.Facing_Left = True
                    End If

                    new_effect.Location = GetEffectLocation(new_effect.CurrentImageSize(),
                     new_effect.direction, TopLeftLocation, CurrentImageSize, new_effect.centering)

                    new_effect.BehaviorName = CurrentBehavior.Name

                    new_effect.OwningPony = Me

                    Pony.CurrentAnimator.AddEffect(new_effect)
                    Me.ActiveEffects.Add(new_effect)

                    EffectsLastUsed(effect) = currentTime

                End If
            Next
        End If

    End Sub

    'reverse directions as if we were bouncing off a boundary.
    Friend Sub Bounce(ByRef pony As Pony, ByRef current_location As Point, ByRef new_location As Point, movement As SizeF)

        If movement = SizeF.Empty Then
            Exit Sub
        End If

        'if we are moving in a simple direction (up/down, left/right) just reverse direction
        If movement.Width = 0 AndAlso movement.Height <> 0 Then
            facingUp = Not facingUp
            If diagonal <> 0 Then
                diagonal = 2 * Math.PI - diagonal
            End If
            Exit Sub
        End If
        If movement.Width <> 0 AndAlso movement.Height = 0 Then
            facingRight = Not facingRight
            If diagonal <> 0 Then
                diagonal = Math.PI - diagonal
            End If
            Exit Sub
        End If

        'if we were moving in a composite direction, we need to determine which component is bad

        Dim x_bad = False
        Dim y_bad = False


        Dim new_location_x As New Point(new_location.X, current_location.Y)
        Dim new_location_y As New Point(current_location.X, new_location.Y)


        If movement.Width <> 0 AndAlso movement.Height <> 0 Then

            If Not pony.IsPonyOnScreen(new_location_x, Main.Instance.ScreensToUse) OrElse pony.InAvoidanceArea(new_location_x) _
                OrElse pony.IsPonyEnteringWindow(current_location, new_location_x, New SizeF(movement.Width, 0)) Then
                x_bad = True
            End If

            If Not pony.IsPonyOnScreen(new_location_y, Main.Instance.ScreensToUse) OrElse pony.InAvoidanceArea(new_location_y) _
                OrElse pony.IsPonyEnteringWindow(current_location, new_location_y, New SizeF(0, movement.Height)) Then
                y_bad = True
            End If

        End If

        If Not x_bad AndAlso Not y_bad Then
            facingUp = Not facingUp
            facingRight = Not facingRight
            If diagonal <> 0 Then
                diagonal = Math.PI - diagonal
                diagonal = 2 * Math.PI - diagonal
            End If
            Exit Sub
        End If

        If x_bad AndAlso y_bad Then
            facingUp = Not facingUp
            facingRight = Not facingRight
            If diagonal <> 0 Then
                diagonal = Math.PI - diagonal
                diagonal = 2 * Math.PI - diagonal
            End If
            Exit Sub
        End If

        If x_bad Then
            facingRight = Not facingRight
            If diagonal <> 0 Then
                diagonal = Math.PI - diagonal
            End If
            Exit Sub
        End If
        If y_bad Then
            facingUp = Not facingUp
            If diagonal <> 0 Then
                diagonal = 2 * Math.PI - diagonal
            End If
        End If

    End Sub

    Friend Function GetBehaviorGroupName(groupnumber As Integer) As String

        If groupnumber = 0 Then
            Return "Any"
        End If

        For Each group In BehaviorGroups
            If group.Number = groupnumber Then
                Return group.Name
            End If
        Next

        Return "Unnamed"

    End Function

    'Return our future location in one second if we go straight in the current direction
    Friend Function FutureLocation(Optional ticks As Integer = 1000) As Point
        Dim Number_Of_Interations = ticks / (1000.0F / Pony.CurrentAnimator.MaximumFramesPerSecond)  'get the # of intervals in one second
        Return Point.Round(CType(TopLeftLocation, PointF) + LastMovement * Number_Of_Interations)
    End Function

    Private Sub Paint(Optional useOverrideBehavior As Boolean = True)
        visualOverrideBehavior = Nothing

        'If we are going to a particular point or following something, we need to pick the 
        'appropriate graphics to how we are moving instead of using what the behavior specifies.
        If hasDestination AndAlso Not ManualControlPlayerOne AndAlso Not ManualControlPlayerTwo Then ' AndAlso Not Playing_Game Then

            Dim horizontalDistance = Math.Abs(Destination.X - CenterLocation.X)
            Dim verticalDistance = Math.Abs(Destination.Y - CenterLocation.Y)

            'We are supposed to be following, so say we can move any direction to do that.
            Dim allowed_movement = AllowedMoves.All

            'if the distance to the destination is mostly horizontal, or mostly vertical, set the movement to either of those
            'This allows pegasi to fly up to reach their target instead of walking straight up.
            'This is weighted more on the vertical side for better effect
            If horizontalDistance * 0.75 > verticalDistance Then
                allowed_movement = allowed_movement And AllowedMoves.HorizontalOnly
            Else
                allowed_movement = allowed_movement And AllowedMoves.VerticalOnly
            End If

            If AtDestination OrElse blocked OrElse CurrentBehavior.Speed = 0 OrElse Delay > 0 Then
                allowed_movement = AllowedMoves.None
                Dim paint_stop_now = paintStop
                paintStop = True

                'If at our destination, we want to allow one final animation change.  
                'However after that, we want to stop painting as we may be stuck in a left-right loop
                'Detect here if the destination is between the right and left image centers, which would cause flickering between the two.
                If paint_stop_now Then

                    If Destination.X >= CurrentBehavior.LeftImageCenter.X + TopLeftLocation.X AndAlso
                        Destination.X < CurrentBehavior.RightImageCenter.X + TopLeftLocation.X Then
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
                Dim appropriateBehavior As PonyBase.Behavior = Nothing
                If allowed_movement = AllowedMoves.None Then
                    appropriateBehavior = CurrentBehavior.FollowStoppedBehavior
                Else
                    appropriateBehavior = CurrentBehavior.FollowMovingBehavior
                End If
                If CurrentBehavior.AutoSelectImagesOnFollow OrElse appropriateBehavior Is Nothing Then
                    appropriateBehavior = GetAppropriateBehaviorOrCurrent(allowed_movement, True, Nothing)
                End If
                visualOverrideBehavior = appropriateBehavior
            End If
        Else
            paintStop = False
        End If

        Dim newCenter = Size.Round(If(facingRight, CurrentBehavior.RightImageCenter, CurrentBehavior.LeftImageCenter) * CSng(Scale))

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

        For Each effect As Effect In Me.ActiveEffects
            If effect.CloseOnNewBehavior Then
                If CurrentBehavior.Name <> effect.BehaviorName Then
                    effectsToRemove.Add(effect)
                End If
            End If
        Next

        For Each effect In effectsToRemove
            Me.ActiveEffects.Remove(effect)
            Main.Instance.DeadEffects.Add(effect)
        Next

    End Sub

    'You can place effects at an offset to the pony, and also set them to the left or the right of themselves for big effects.
    Friend Function GetEffectLocation(ByRef EffectImageSize As Size, ByRef direction As Direction,
                                      ByRef ParentLocation As Point, ByRef ParentSize As Vector2, ByRef centering As Direction) As Point

        Dim point As Point

        With ParentSize * CSng(Scale)
            Select Case direction
                Case Direction.Bottom
                    point = New Point(CInt(ParentLocation.X + .X / 2), CInt(ParentLocation.Y + .Y))
                Case Direction.BottomLeft
                    point = New Point(ParentLocation.X, CInt(ParentLocation.Y + .Y))
                Case Direction.BottomRight
                    point = New Point(CInt(ParentLocation.X + .X), CInt(ParentLocation.Y + .Y))
                Case Direction.Center
                    point = New Point(CInt(ParentLocation.X + .X / 2), CInt(ParentLocation.Y + .Y / 2))
                Case Direction.Left
                    point = New Point(ParentLocation.X, CInt(ParentLocation.Y + .Y / 2))
                Case Direction.Right
                    point = New Point(CInt(ParentLocation.X + .X), CInt(ParentLocation.Y + .Y / 2))
                Case Direction.Top
                    point = New Point(CInt(ParentLocation.X + .X / 2), ParentLocation.Y)
                Case Direction.TopLeft
                    point = New Point(ParentLocation.X, ParentLocation.Y)
                Case Direction.TopRight
                    point = New Point(CInt(ParentLocation.X + .X), ParentLocation.Y)
            End Select

        End With

        Dim effectscaling = Options.ScaleFactor

        Select Case centering
            Case Direction.Bottom
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width) / 2), CInt(point.Y - (effectscaling * EffectImageSize.Height)))
            Case Direction.BottomLeft
                point = New Point(point.X, CInt(point.Y - (effectscaling * EffectImageSize.Height)))
            Case Direction.BottomRight
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width)), CInt(point.Y - (effectscaling * EffectImageSize.Height)))
            Case Direction.Center
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width) / 2), CInt(point.Y - (effectscaling * EffectImageSize.Height) / 2))
            Case Direction.Left
                point = New Point(point.X, CInt(point.Y - (effectscaling * EffectImageSize.Height) / 2))
            Case Direction.Right
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width)), CInt(point.Y - (effectscaling * EffectImageSize.Height) / 2))
            Case Direction.Top
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width) / 2), point.Y)
            Case Direction.TopLeft
                'no change
            Case Direction.TopRight
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width)), point.Y)
        End Select

        Return point

    End Function

    Private Function GetAppropriateBehavior(movement As AllowedMoves, speed As Boolean,
                                           Optional suggestedBehavior As PonyBase.Behavior = Nothing) As PonyBase.Behavior
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
                            facingRight = (GetDestinationDirectionHorizontal(Destination) = Direction.Right)
                        End If
                        Return suggestedBehavior
                    End If
                End If

                'if this behavior has a destination or an object to follow, don't use it.
                If (destinationCoords.X <> 0 OrElse destinationCoords.Y <> 0 OrElse followObjectName <> "") AndAlso
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
                                           Optional suggestedBehavior As PonyBase.Behavior = Nothing) As PonyBase.Behavior
        Return If(GetAppropriateBehavior(movement, speed, suggestedBehavior), CurrentBehavior)
    End Function

    Shared Function GetScreenContainingPoint(point As Point) As Screen
        For Each screen In Main.Instance.ScreensToUse
            If (screen.WorkingArea.Contains(point)) Then Return screen
        Next
        Return Main.Instance.ScreensToUse(0)
    End Function

    'Test to see if we overlap with another application's window.
    Function IsPonyEnteringWindow(ByRef current_location As Point, ByRef new_location As Point, movement As SizeF) As Boolean
        If Not OperatingSystemInfo.IsWindows Then Return False

        Try
            If Main.Instance.InPreviewMode Then Return False
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


            Dim collision_windows As New List(Of IntPtr)

            If (new_window_1 <> IntPtr.Zero AndAlso new_window_1 <> current_window_1) Then collision_windows.Add(new_window_1)
            If (new_window_2 <> IntPtr.Zero AndAlso new_window_2 <> current_window_2) Then collision_windows.Add(new_window_2)
            If (new_window_3 <> IntPtr.Zero AndAlso new_window_3 <> current_window_3) Then collision_windows.Add(new_window_3)
            If (new_window_4 <> IntPtr.Zero AndAlso new_window_4 <> current_window_4) Then collision_windows.Add(new_window_4)

            If collision_windows.Count <> 0 Then

                Dim pony_collision_count = 0
                Dim ignored_collision_count = 0

                For Each collision In collision_windows

                    If Options.PonyAvoidsPonies AndAlso Options.PonyStaysInBox Then
                        Exit For
                    End If

                    Dim process_id As IntPtr
                    Win32.GetWindowThreadProcessId(collision, process_id)

                    'ignore collisions with other ponies or effects
                    If Options.PonyAvoidsPonies AndAlso process_id = Main.Instance.process_id Then
                        pony_collision_count += 1
                    Else

                        'we are colliding with another window boundary.
                        'are we already inside of it, and therefore should go through to the outside?
                        'or are we on the outside, and need to stay out?

                        If Options.PonyStaysInBox Then Continue For

                        Dim collisionArea As New Win32.RECT
                        Win32.GetWindowRect(collision, collisionArea)
                        If IsPonyInBox(current_location, Rectangle.FromLTRB(
                                       collisionArea.Left, collisionArea.Top, collisionArea.Right, collisionArea.Bottom)) Then
                            ignored_collision_count += 1
                        End If
                    End If
                Next

                If pony_collision_count + ignored_collision_count = collision_windows.Count Then
                    Return False
                End If

                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Options.WindowAvoidanceEnabled = False
            MsgBox("Warning:  Error attempting to avoid windows.  Window avoidance disabled.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
            Return False
        End Try

    End Function

    'Is the pony at least partially on any of the supplied screens?
    Friend Function IsPonyOnScreen(location As Point, screenList As List(Of Screen)) As Boolean
        If Main.Instance.InPreviewMode Then Return True

        For Each screen In screenList
            If EveryLocationPointContainedInBounds(location, screen.WorkingArea) Then Return True
        Next

        Return False
    End Function

    Function EveryLocationPointContainedInBounds(location As Point, bounds As Rectangle) As Boolean
        'test center (or upper right if no center is defined)
        If Not bounds.Contains(location.X + currentCustomImageCenter.Width,
                               location.Y + currentCustomImageCenter.Height) Then Return False

        If facingUp AndAlso facingRight Then
            'test upper right corner
            If Not bounds.Contains(CInt(location.X + (Scale * CurrentImageSize.X)), location.Y) Then Return False
        End If

        If facingUp AndAlso Not facingRight Then
            'top left
            If Not bounds.Contains(location) Then Return False
        End If

        If Not facingUp AndAlso facingRight Then
            'bottom right
            If Not bounds.Contains(CInt(location.X + (Scale * CurrentImageSize.X)),
                               CInt(location.Y + (Scale * CurrentImageSize.Y))) Then Return False
        End If

        If Not facingUp AndAlso Not facingRight Then
            'bottom left
            If Not bounds.Contains(location.X, CInt(location.Y + (Scale * CurrentImageSize.Y))) Then Return False
        End If

        Return True
    End Function

    Shared Function IsPonyInBox(ByRef location As Point, ByRef box As Rectangle) As Boolean
        Return box.IsEmpty OrElse box.Contains(location)
    End Function

    ''are we inside the user specified "Everfree Forest"?
    Function InAvoidanceArea(ByRef new_location As System.Drawing.Point) As Boolean

        If Main.Instance.InPreviewMode Then
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
            Dim area = Options.ExclusionZoneForBounds(screen.WorkingArea)
            If area.Contains(point) Then Return True
        Next

        Return False
    End Function

    Function IsPonyNearMouseCursor(ByRef location As System.Drawing.Point) As Boolean

        If Not Options.CursorAvoidanceEnabled Then Return False
        If Main.Instance.ScreensaverMode Then Return False

        If CursorImmunity > 0 Then Return False

        'ignore this if we are interacting - we don't want to cancel it.
        If Me.IsInteracting Then Return False

        If ManualControlPlayerOne OrElse ManualControlPlayerTwo Then Return False

        With Main.Instance
            For Each behavior In Behaviors
                If behavior.AllowedMovement = Pony.AllowedMoves.MouseOver Then
                    Dim rightCenter As Point
                    If facingRight AndAlso behavior.RightImageCenter <> Vector2.Zero Then
                        rightCenter = New Point(CInt(location.X + (Scale * (behavior.RightImageCenter.X))),
                                                CInt(location.Y + (Scale * (behavior.RightImageCenter.Y))))
                    Else
                        rightCenter = New Point(CInt(location.X + (Scale * (Behaviors(0).RightImageSize.X)) / 2),
                          CInt(location.Y + (Scale * (Behaviors(0).RightImageSize.Y)) / 2))
                    End If
                    Dim leftCenter As Point
                    If Not facingRight AndAlso behavior.LeftImageCenter <> Vector2.Zero Then
                        leftCenter = New Point(CInt(location.X + (Scale * (behavior.LeftImageCenter.X))),
                                             CInt(location.Y + (Scale * (behavior.LeftImageCenter.Y))))
                    Else
                        leftCenter = New Point(CInt(location.X + (Scale * (Behaviors(0).LeftImageSize.X)) / 2),
                              CInt(location.Y + (Scale * (Behaviors(0).LeftImageSize.Y)) / 2))
                    End If

                    For i As Integer = 0 To 1
                        Dim pony_location = rightCenter
                        If i = 1 Then
                            pony_location = leftCenter
                        End If

                        Dim distance = Vector.Distance(pony_location, CursorLocation)
                        If distance <= .cursor_zone_size Then
                            Return True
                        End If
                    Next
                End If
            Next
        End With

        Return False

    End Function

    ''' <summary>
    ''' Returns a random location that is within the allowable regions to be in.
    ''' </summary>
    ''' <returns>The center of the preview area in preview mode, otherwise a random location within the allowable region, if one can be
    ''' found; otherwise Point.Empty.</returns>
    Friend Function FindSafeDestination() As Point
        If Main.Instance.InPreviewMode Then Return Point.Round(Pony.PreviewWindowRectangle.Center())

        Dim usableScreens = Main.Instance.ScreensToUse
        For i = 0 To 300
            Dim randomScreen = usableScreens(Rng.Next(usableScreens.Count))
            Dim teleportLocation = New Point(
                CInt(randomScreen.WorkingArea.X + Math.Round(Rng.NextDouble() * randomScreen.WorkingArea.Width)),
                CInt(randomScreen.WorkingArea.Y + Math.Round(Rng.NextDouble() * randomScreen.WorkingArea.Height)))
            If Not InAvoidanceArea(teleportLocation) Then Return teleportLocation
        Next

        Return Point.Empty
    End Function

    Friend Function GetDestinationDirectionHorizontal(destination As Vector2) As Direction
        Dim rightImageCenterX = CInt(TopLeftLocation.X + (Scale * CurrentBehavior.RightImageCenter.X))
        Dim leftImageCenterX = CInt(TopLeftLocation.X + (Scale * CurrentBehavior.LeftImageCenter.X))
        If (rightImageCenterX > destination.X AndAlso leftImageCenterX < destination.X) OrElse
            destination.X - CenterLocation.X <= 0 Then
            Return Direction.Left
        Else
            Return Direction.Right
        End If
    End Function

    Friend Function GetDestinationDirectionVertical(destination As Vector2) As Direction
        Dim rightImageCenterY = CInt(TopLeftLocation.Y + (Scale * CurrentBehavior.RightImageCenter.Y))
        Dim leftImageCenterY = CInt(TopLeftLocation.Y + (Scale * CurrentBehavior.LeftImageCenter.Y))
        If (rightImageCenterY > destination.Y AndAlso leftImageCenterY < destination.Y) OrElse
           (rightImageCenterY < destination.Y AndAlso leftImageCenterY > destination.Y) OrElse
           destination.Y - CenterLocation.Y <= 0 Then
            Return Direction.Top
        Else
            Return Direction.Bottom
        End If
    End Function

    Friend ReadOnly Property CurrentImageSize As Vector2
        Get
            Dim behavior = If(visualOverrideBehavior, CurrentBehavior)
            Return If(facingRight, behavior.RightImageSize, behavior.LeftImageSize)
        End Get
    End Property

    Friend Function CenterLocation() As Point
        Return TopLeftLocation + GetImageCenterOffset()
    End Function

    'Make a lists of targets from what ponies exist, and get their references.
    Friend Sub InitializeInteractions(otherPonies As IEnumerable(Of Pony))

        For Each Interaction In Interactions

            Interaction.InteractsWith.Clear()

            For Each directory As String In Interaction.InteractsWithByDirectory
                For Each pony In otherPonies
                    If directory = pony.Directory Then
                        Dim already_added = False
                        For Each otherPony In Interaction.InteractsWith
                            If ReferenceEquals(otherPony, pony) Then
                                already_added = True
                                Exit For
                            End If
                        Next

                        If already_added Then
                            Continue For
                        End If

                        Interaction.InteractsWith.Add(pony)
                    End If
                Next
            Next
        Next

    End Sub

    Friend Sub StartInteraction(ByRef interaction As PonyBase.Interaction)

        IsInteractionInitiator = True
        CurrentInteraction = interaction
        SelectBehavior(interaction.BehaviorList(Rng.Next(interaction.BehaviorList.Count)))

        'do we interact with ALL targets, including copies, or just the pony that we ran into?
        If interaction.Targets_Activated <> PonyBase.Interaction.TargetActivation.One Then
            For Each targetPony In interaction.InteractsWith
                targetPony.StartInteractionAsTarget(CurrentBehavior.Name, Me, interaction)
            Next
        Else
            interaction.Trigger.StartInteractionAsTarget(CurrentBehavior.Name, Me, interaction)
        End If


        IsInteracting = True

    End Sub

    Friend Sub StartInteractionAsTarget(ByRef BehaviorName As String, ByRef initiator As Pony, interaction As PonyBase.Interaction)
        For Each behavior In Behaviors
            If BehaviorName = behavior.Name Then
                SelectBehavior(behavior)
                Exit For
            End If
        Next

        interaction.Initiator = initiator
        IsInteractionInitiator = False
        CurrentInteraction = interaction
        IsInteracting = True
    End Sub

    Private Function GetReadiedInteraction() As PonyBase.Interaction
        'If we recently ran an interaction, don't start a new one until the delay expires.
        If internalTime < interactionDelayUntil Then
            Return Nothing
        End If

        For Each interaction In Interactions
            For Each target As Pony In interaction.InteractsWith
                ' Don't attempt to interact with a busy target, or with self.
                If target.IsInteracting OrElse ReferenceEquals(Me, target) Then Continue For

                ' Make sure that all targets are present, if all are required.
                If interaction.Targets_Activated = PonyBase.Interaction.TargetActivation.All AndAlso
                    interaction.InteractsWith.Count <> interaction.InteractsWithByDirectory.Count Then
                    Continue For
                End If

                ' Get distance between the pony and the possible target.
                Dim distance = Vector.Distance(TopLeftLocation + New Size(CInt(CurrentImageSize.X / 2),
                                                                   CInt(CurrentImageSize.Y / 2)),
                                               target.TopLeftLocation + New Size(CInt(target.CurrentImageSize.X / 2),
                                                                          CInt(target.CurrentImageSize.Y / 2)))

                ' Check target is in range, and perform a random check against the chance the interaction can occur.
                If distance <= interaction.Proximity_Activation_Distance AndAlso Rng.NextDouble() <= interaction.Probability Then
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
            Dim path = If(facingRight, behavior.RightImagePath, behavior.LeftImagePath)
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
        diagonal = 0
        If Not PlayingGame AndAlso ponyAction Then
            CursorOverPony = True
            Paint() 'enable effects on mouseover.
            Return CurrentBehavior.Speed * Scale
        Else
            'if we're not in the cursor's way, but still flagged that we are, exit mouseover mode.
            If HaltedForCursor Then
                CursorOverPony = False
                Return CurrentBehavior.Speed * Scale
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
        Return If(appropriateMovement = AllowedMoves.None, 0, CurrentBehavior.Speed * Scale * speedupFactor)
    End Function
End Class

Class Effect
    Implements ISprite

    Friend Name As String = ""
    Friend Location As Point
    Friend translated_location As Point

    Friend beingDragged As Boolean = False

    Friend dont_repeat_image_animations As Boolean = False

    Friend BehaviorName As String

    Friend OwnerPony As PonyBase
    Friend OwningPony As Pony

    Friend right_image_path As String
    Friend right_image_size As Size
    Friend left_image_path As String
    Friend left_image_size As Size
    Friend current_image_path As String
    Friend Duration As Double
    Friend DesiredDuration As Double

    Friend Repeat_Delay As Double

    Friend placement_direction_right As Direction
    Friend centering_right As Direction
    Friend placement_direction_left As Direction
    Friend centering_left As Direction

    Friend centering As Direction
    Friend direction As Direction

    Friend Facing_Left As Boolean = False

    Private start_time As TimeSpan
    Friend CloseOnNewBehavior As Boolean = False

    Friend follow As Boolean = False

    Friend already_played_for_currentbehavior As Boolean = False

    Public Sub New(rightImagePath As String, leftImagePath As String)
        SetRightImagePath(rightImagePath)
        SetLeftImagePath(leftImagePath)
    End Sub

    Friend Sub SetRightImagePath(path As String)
        Argument.EnsureNotNull(path, "path")
        right_image_path = path
        right_image_size = ImageSize.GetSize(right_image_path)
    End Sub

    Friend Sub SetLeftImagePath(path As String)
        Argument.EnsureNotNull(path, "path")
        left_image_path = path
        left_image_size = ImageSize.GetSize(left_image_path)
    End Sub

    Sub Teleport()
        Dim screens = Main.Instance.ScreensToUse
        Dim screen = screens(Rng.Next(screens.Count))
        Location = New Point(
            CInt(screen.WorkingArea.X + Math.Round(Rng.NextDouble() * (screen.WorkingArea.Width - left_image_size.Width), 0)),
            CInt(screen.WorkingArea.Y + Math.Round(Rng.NextDouble() * (screen.WorkingArea.Height - left_image_size.Height), 0)))
    End Sub

    Public Function Clone() As Effect
        Dim new_effect As New Effect(right_image_path, left_image_path)

        new_effect.Name = Name
        new_effect.BehaviorName = BehaviorName

        new_effect.Duration = Duration
        new_effect.Repeat_Delay = Repeat_Delay
        new_effect.placement_direction_right = placement_direction_right
        new_effect.centering_right = centering_right
        new_effect.placement_direction_left = placement_direction_left
        new_effect.centering_left = centering_left

        new_effect.dont_repeat_image_animations = dont_repeat_image_animations

        new_effect.follow = follow

        new_effect.already_played_for_currentbehavior = already_played_for_currentbehavior
        new_effect.OwnerPony = OwnerPony

        Return new_effect
    End Function

    Friend Function Center() As Point
        Dim scale As Double

        If OwningPony IsNot Nothing Then
            scale = OwningPony.Scale
        Else
            scale = 1
        End If

        If IsNothing(current_image_path) Then

            If Not IsNothing(left_image_path) Then
                Return New Point(CInt(Me.Location.X + ((scale * left_image_size.Width) / 2)), CInt(Me.Location.Y + ((scale * left_image_size.Height) / 2)))
            End If

            If Not IsNothing(right_image_path) Then
                Return New Point(CInt(Me.Location.X + ((scale * right_image_size.Width) / 2)), CInt(Me.Location.Y + ((scale * right_image_size.Height) / 2)))
            End If

            Return Location
        End If


        Return New Point(CInt(Me.Location.X + ((scale * CurrentImageSize().Width) / 2)), CInt(Me.Location.Y + ((scale * CurrentImageSize().Height) / 2)))
    End Function

    Friend Function CurrentImageSize() As Size
        If current_image_path = right_image_path Then
            Return right_image_size
        Else
            Return left_image_size
        End If
    End Function

    Private internalTime As TimeSpan

    Public Sub Start(startTime As TimeSpan) Implements ISprite.Start
        current_image_path = If(Facing_Left, left_image_path, right_image_path)
        start_time = startTime
        internalTime = startTime
    End Sub

    Public Sub Update(updateTime As TimeSpan) Implements ISprite.Update
        internalTime = updateTime
        current_image_path = If(Facing_Left, left_image_path, right_image_path)
        If beingDragged Then
            Location = Pony.CursorLocation - New Size(CInt(CurrentImageSize.Width / 2), CInt(CurrentImageSize.Height / 2))
        End If
    End Sub

    Public ReadOnly Property CurrentTime As TimeSpan Implements ISprite.CurrentTime
        Get
            Dim time = internalTime - start_time
            If time < TimeSpan.Zero Then Stop
            Return internalTime - start_time
        End Get
    End Property

    Public ReadOnly Property FlipImage As Boolean Implements ISprite.FlipImage
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property ImagePath As String Implements ISprite.ImagePath
        Get
            Return current_image_path
        End Get
    End Property

    Public ReadOnly Property Region As System.Drawing.Rectangle Implements ISprite.Region
        Get
            Dim width = CInt(CurrentImageSize.Width * Options.ScaleFactor)
            Dim height = CInt(CurrentImageSize.Height * Options.ScaleFactor)
            Return New Rectangle(Location, New Size(width, height))
        End Get
    End Property
End Class

Class HouseBase
    Public Const RootDirectory = "Houses"
    Public Const ConfigFilename = "house.ini"

    Friend OptionsForm As HouseOptionsForm

    Private ReadOnly _directory As String
    Public ReadOnly Property Directory() As String
        Get
            Return _directory
        End Get
    End Property

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
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

    Private _imageFilename As String
    Public Property ImageFilename() As String
        Get
            Return _imageFilename
        End Get
        Set(value As String)
            _imageFilename = value
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
    Public ReadOnly Property Visitors() As IList(Of String)
        Get
            Return _visitors
        End Get
    End Property

    Public Sub New(directory As String)
        Argument.EnsureNotNull(directory, "directory")

        Dim lastSeparator = directory.LastIndexOfAny({Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar})
        If lastSeparator <> -1 Then
            _directory = directory.Substring(lastSeparator + 1)
        Else
            _directory = directory
        End If

        LoadFromIni()
    End Sub

    Private Sub LoadFromIni()
        Dim fullDirectory = Path.Combine(Options.InstallLocation, RootDirectory, Directory)
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
                        ImageFilename = Path.Combine(fullDirectory, columns(1))
                        If Not File.Exists(ImageFilename) Then
                            Throw New FileNotFoundException(ImageFilename, ImageFilename)
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

            If String.IsNullOrEmpty(Name) OrElse String.IsNullOrEmpty(ImageFilename) OrElse
                Visitors.Count = 0 Then
                Throw New InvalidDataException("Unable to load 'House' at: " & fullDirectory &
                                               ".INI file does not contain all necessary parameters: " & ControlChars.NewLine &
                                               "name, image, and at least one pony's name")
            End If
        End Using
    End Sub

End Class

Class House
    Inherits Effect

    Private deployedPonies As New List(Of Pony)

    Private lastCycleTime As TimeSpan

    Private _base As HouseBase
    Public ReadOnly Property Base() As HouseBase
        Get
            Return _base
        End Get
    End Property

    Friend Shared Function ImageScale(size As Size) As Size
        Dim scale = Options.ScaleFactor
        Return New Size(CInt(size.Width * scale), CInt(size.Height * scale))
    End Function

    Public Sub New(houseBase As HouseBase)
        MyBase.New(houseBase.ImageFilename, houseBase.ImageFilename)
        _base = houseBase
        DesiredDuration = TimeSpan.FromDays(100).TotalSeconds
    End Sub

    Friend Sub InitializeVisitorList()
        deployedPonies.Clear()
        For Each Pony As Pony In Pony.CurrentAnimator.Ponies()
            For Each guest In base.Visitors
                If String.Equals(Pony.Directory, guest, StringComparison.OrdinalIgnoreCase) Then
                    deployedPonies.Add(Pony)
                    Exit For
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' Checks to see if it is time to deploy/recall a pony and does so. 
    ''' </summary>
    ''' <param name="currentTime">The current time.</param>
    Friend Sub Cycle(currentTime As TimeSpan)

        If currentTime - lastCycleTime > base.CycleInterval Then
            lastCycleTime = currentTime

            Console.WriteLine(Me.Name & " - Cycling. Deployed ponies: " & deployedPonies.Count)

            If Rng.NextDouble() < 0.5 Then
                'skip this round
                Console.WriteLine(Me.Name & " - Decided to skip this round of cycling.")
                Exit Sub
            End If

            If Rng.NextDouble() < Base.Bias Then
                If deployedPonies.Count < Base.MaximumPonies AndAlso Pony.CurrentAnimator.Ponies().Count < Options.MaxPonyCount Then
                    DeployPony(Me)
                Else
                    Console.WriteLine(Me.Name & " - Cannot deploy. Pony limit reached.")
                End If
            Else
                If deployedPonies.Count > Base.MinimumPonies AndAlso Pony.CurrentAnimator.Ponies().Count > 1 Then
                    RecallPony(Me)
                Else
                    Console.WriteLine(Me.Name & " - Cannot recall. Too few ponies deployed.")
                End If
            End If

        End If

    End Sub

    Private Sub DeployPony(instance As Effect)

        Dim choices As New List(Of String)

        Dim all As Boolean = False

        For Each entry In base.Visitors
            If String.Equals(entry, "all", StringComparison.OrdinalIgnoreCase) Then
                For Each Pony In Main.Instance.SelectablePonies
                    choices.Add(Pony.Directory)
                Next
                all = True
                Exit For
            End If
        Next

        If all = False Then
            For Each Pony In base.Visitors
                choices.Add(Pony)
            Next
        End If

        For Each Pony As Pony In Pony.CurrentAnimator.Ponies()
            choices.Remove(Pony.Directory)
        Next

        choices.Remove("Random Pony")

        If choices.Count = 0 Then
            Exit Sub
        End If

        Dim selected_name = choices(Rng.Next(choices.Count))

        For Each ponyBase In Main.Instance.SelectablePonies
            If ponyBase.Directory = selected_name Then

                Dim deployed_pony = New Pony(ponyBase)

                deployed_pony.SelectBehavior()

                deployed_pony.TopLeftLocation = instance.Location + New Size(Base.DoorPosition) - deployed_pony.GetImageCenterOffset()

                Dim groups As New List(Of Integer)
                Dim Alternate_Group_Behaviors As New List(Of PonyBase.Behavior)

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

                Console.WriteLine(Me.Name & " - Deployed " & ponyBase.Directory)

                For Each other_Pony In Pony.CurrentAnimator.Ponies()
                    'we need to set up interactions again to account for new ponies.
                    other_Pony.InitializeInteractions(Pony.CurrentAnimator.Ponies())
                Next

                Exit Sub
            End If
        Next

    End Sub

    Private Sub RecallPony(instance As Effect)

        Dim choices As New List(Of String)

        Dim all As Boolean = False

        For Each entry In Base.Visitors
            If String.Equals(entry, "all", StringComparison.OrdinalIgnoreCase) Then
                For Each Pony As Pony In Pony.CurrentAnimator.Ponies()
                    choices.Add(Pony.Directory)
                Next
                all = True
                Exit For
            End If
        Next

        If all = False Then
            For Each Pony As Pony In Pony.CurrentAnimator.Ponies()
                For Each otherpony In Base.Visitors
                    If Pony.Directory = otherpony Then
                        choices.Add(Pony.Directory)
                        Exit For
                    End If
                Next
            Next
        End If

        If choices.Count = 0 Then Exit Sub

        Dim selected_name = choices(Rng.Next(choices.Count))

        For Each pony As Pony In pony.CurrentAnimator.Ponies()
            If pony.Directory = selected_name Then

                If pony.IsInteracting Then Exit Sub
                If pony.BeingDragged Then Exit Sub

                If pony.Sleeping Then pony.WakeUp()

                pony.Destination = instance.Location + New Size(Base.DoorPosition)
                pony.GoingHome = True
                pony.CurrentBehavior = pony.GetAppropriateBehaviorOrCurrent(pony.AllowedMoves.All, False)
                pony.BehaviorDesiredDuration = TimeSpan.FromMinutes(5)

                deployedPonies.Remove(pony)

                Console.WriteLine(Me.Name & " - Recalled " & pony.Directory)

                Exit Sub
            End If
        Next

    End Sub

End Class

Friend Enum Direction
    Top = 0
    Bottom = 1
    Left = 2
    Right = 3
    BottomRight = 4
    BottomLeft = 5
    TopRight = 6
    TopLeft = 7
    Center = 8
    Random = 9
    RandomNotCenter = 10
End Enum