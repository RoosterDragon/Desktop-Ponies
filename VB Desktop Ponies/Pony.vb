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

                    Dim movement As Pony.Allowed_Moves

                    'movements are bytes so that they can be composite:
                    '"diagonal" means vertical AND horizontal at the same time.
                    'See the definition in the pony class for more information.
                    Select Case Trim(LCase(columns(Main.BehaviorOption.MovementType)))

                        Case "none"
                            movement = Pony.Allowed_Moves.None
                        Case "horizontal_only"
                            movement = Pony.Allowed_Moves.Horizontal_Only
                        Case "vertical_only"
                            movement = Pony.Allowed_Moves.Vertical_Only
                        Case "horizontal_vertical"
                            movement = Pony.Allowed_Moves.Horizontal_Vertical
                        Case "diagonal_only"
                            movement = Pony.Allowed_Moves.Diagonal_Only
                        Case "diagonal_horizontal"
                            movement = Pony.Allowed_Moves.Diagonal_Horizontal
                        Case "diagonal_vertical"
                            movement = Pony.Allowed_Moves.Diagonal_Vertical
                        Case "all"
                            movement = Pony.Allowed_Moves.All
                        Case "mouseover"
                            movement = Pony.Allowed_Moves.MouseOver
                        Case "sleep"
                            movement = Pony.Allowed_Moves.Sleep
                        Case "dragged"
                            movement = Pony.Allowed_Moves.Dragged
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

                            Dim direction_right = Directions.center
                            Dim centering_right = Directions.center
                            Dim direction_left = Directions.center
                            Dim centering_left = Directions.center
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
                                               Boolean.Parse(Trim(columns(11))), dont_repeat_image_animations)
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
        new_interaction.Reactivation_Delay = repeat_delay



        Select Case LCase(Trim(proximity))
            Case "default"
            Case Else
                Dim proximityValue As Double
                If Double.TryParse(proximity, NumberStyles.Float, CultureInfo.InvariantCulture, proximityValue) Then
                    new_interaction.Proximity_Activation_Distance = proximityValue
                Else
                    If Not Main.Instance.screen_saver_mode Then
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
            If found = False AndAlso Options.DisplayPonyInteractionsErrors AndAlso Not Main.Instance.screen_saver_mode Then
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
                            If displaywarnings AndAlso Not Main.Instance.screen_saver_mode Then
                                MsgBox("Warning:  Pony " & Pony.Name & " (" & Pony.Directory & ") " & _
                                " does not have required behavior '" & _
                               Behavior & "' as specified in interaction " & interaction_name & _
                               ControlChars.NewLine & "Interaction is disabled for this pony.")
                            End If
                        End If

                    Next
                End If
            Next

            If ponyfound = False AndAlso displaywarnings AndAlso Not Main.Instance.screen_saver_mode Then

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
                           Allowed_Moves As Pony.Allowed_Moves, _Linked_Behavior As String,
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
        new_behavior.Pony_Name = Me.Directory
        new_behavior.chance_of_occurance = chance
        new_behavior.MaxDuration = max_duration
        new_behavior.MinDuration = min_duration
        new_behavior.SetSpeed(speed)
        new_behavior.Allowed_Movement = Allowed_Moves
        new_behavior.dont_repeat_image_animations = _dont_repeat_image_animations
        new_behavior.StartLineName = _Startline
        new_behavior.EndLineName = _Endline
        new_behavior.Group = _group
        new_behavior.Skip = _skip

        'These coordinates are either a position on the screen to go to, if no object to follow is specified,
        'or, the offset from the center of the object to go to (upper left, below, etc)
        new_behavior.Auto_Select_Images_On_Follow = _auto_select_images_on_follow

        'When the pony if off-screen we overwrite the follow parameters to get them onscreen again.
        'we save the original parameters here.
        new_behavior.original_destination_xcoord = _xcoord
        new_behavior.original_destination_ycoord = _ycoord
        new_behavior.original_follow_object_name = _object_to_follow

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
                              Allowed_Moves As Pony.Allowed_Moves, _Linked_Behavior As String, _Startline As String, _Endline As String)

        Dim new_behavior As New Behavior("", "")

        new_behavior.Name = Trim(name)
        new_behavior.Pony_Name = Me.Directory
        new_behavior.chance_of_occurance = chance
        new_behavior.MaxDuration = max_duration
        new_behavior.MinDuration = min_duration
        new_behavior.SetSpeed(speed)
        new_behavior.Allowed_Movement = Allowed_Moves

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

        Friend Reactivation_Delay As Integer = 60 'in seconds

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

        Friend Name As String
        Friend Pony_Name As String
        Friend chance_of_occurance As Double
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

        Friend Allowed_Movement As Pony.Allowed_Moves

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

        Friend original_destination_xcoord As Integer = 0
        Friend original_destination_ycoord As Integer = 0
        Friend original_follow_object_name As String = ""

        Friend FollowStoppedBehaviorName As String = ""
        Friend FollowMovingBehaviorName As String = ""
        Friend FollowStoppedBehavior As Behavior = Nothing
        Friend FollowMovingBehavior As Behavior = Nothing
        Friend Auto_Select_Images_On_Follow As Boolean = True
        Friend Group As Integer = 0 'the group # this behavior belongs to

        'Used when following.
        Friend delay As Integer = 0
        Friend blocked As Boolean = False

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
                             direction_right As Directions, centering_right As Directions,
                             direction_left As Directions, centering_left As Directions,
                             follow As Boolean, _dont_repeat_image_animations As Boolean)

            Dim new_effect As New Effect(right_path, left_path)

            new_effect.behavior_name = Me.Name
            new_effect.Pony_Name = Me.Pony_Name
            new_effect.Name = effectname
            new_effect.Duration = duration
            new_effect.Repeat_Delay = repeat_delay
            new_effect.placement_direction_right = direction_right
            new_effect.centering_right = centering_right
            new_effect.placement_direction_left = direction_left
            new_effect.centering_left = centering_left
            new_effect.follow = follow
            new_effect.dont_repeat_image_animations = _dont_repeat_image_animations

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

    Friend Shared CursorLocation As Point
    Friend Shared CurrentAnimator As DesktopPonyAnimator
    Friend Shared CurrentViewer As ISpriteCollectionView
    Friend Shared PreviewWindowRectangle As Rectangle

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

    Friend Current_BehaviorGroup As Integer = -1 '-1 is invalid and will be changed on first SelectBehavior(), 0 = use any group

    Friend Interaction_Active As Boolean = False
    Private _currentInteraction As PonyBase.Interaction = Nothing
    Friend ReadOnly Property CurrentInteraction As PonyBase.Interaction
        Get
            Return _currentInteraction
        End Get
    End Property
    Private Is_Interaction_Initiator As Boolean = False

    Friend Is_Interacting As Boolean = False

    Friend Playing_Game As Boolean = False

    Private vertical As Boolean = False
    Private horizontal As Boolean = False
    Friend up As Boolean = False
    Friend right As Boolean = True
    ''' <summary>
    '''The angle to travel in, if moving diagonally (in radians)
    ''' </summary>
    Friend diagonal As Double = 0

    ''' <summary>
    ''' Time until interactions should be disabled.
    ''' Stops interactions from repeating too soon after one another.
    ''' Only affects the triggering pony and not targets.
    ''' </summary>
    ''' <remarks></remarks>
    Private interactionDelayUntil As TimeSpan

    Private m_currentBehavior As PonyBase.Behavior
    Friend Property CurrentBehavior As PonyBase.Behavior
        Get
            Return m_currentBehavior
        End Get
        Set(value As PonyBase.Behavior)
            If value Is Nothing Then
                Console.WriteLine("Tried to set a null Behavior")
                Console.WriteLine(New Diagnostics.StackTrace(True))
            Else
                m_currentBehavior = value
                SetAllowableDirections()
                If vertical = False AndAlso horizontal = False AndAlso
                    (m_currentBehavior.Allowed_Movement.HasFlag(Allowed_Moves.Diagonal_Only) OrElse
                    m_currentBehavior.Allowed_Movement.HasFlag(Allowed_Moves.Horizontal_Only) OrElse
                    m_currentBehavior.Allowed_Movement.HasFlag(Allowed_Moves.Vertical_Only)) Then
                    Stop
                End If
            End If
        End Set
    End Property
    Friend current_image_center As Point

    ''' <summary>
    ''' Only used when temporarily pausing, like when the mouse hovers over us.
    ''' </summary>
    Private previous_behavior As PonyBase.Behavior

    ''' <summary>
    ''' When set, specifics the alternate set of images that should replace those of the current behavior.
    ''' </summary>
    Friend visual_override_behavior As PonyBase.Behavior

    Private _movingOnscreen As Boolean = False
    Friend ReadOnly Property MovingOnscreen As Boolean
        Get
            Return _movingOnscreen
        End Get
    End Property

    ''' <summary>
    ''' Used when going back "in" houses.
    ''' </summary>
    Friend Going_Home As Boolean = False
    ''' <summary>
    ''' Used when a pony has been recalled and is just about to "enter" a house
    ''' </summary>
    Friend Opening_Door As Boolean = False

    ''' <summary>
    ''' Should we stop because the cursor is hovered over?
    ''' </summary>
    Private Cursor_Halt As Boolean = False

    ''' <summary>
    ''' Are we actually halted now?
    ''' </summary>
    Private Is_cursor_halted As Boolean = False
    Private Cursor_Immunity As Integer = 0

    Friend Destination As Vector2
    Friend AtDestination As Boolean = False

    ''' <summary>
    ''' Used in the Paint() sub to help stop flickering between left and right images under certain circumstances.
    ''' </summary>
    Private Paint_stop As Boolean = False

    ''' <summary>
    ''' The location on the screen.
    ''' </summary>=
    Friend Location As New Point

    ''' <summary>
    ''' Used for predicting future movement (just more of what we last did)
    ''' </summary>
    Private LastMovement As Vector2F

    Friend Active_Effects As New List(Of Effect)

    Friend should_be_sleeping As Boolean = False
    Friend sleeping As Boolean = False

    'User has the option of limiting songs to one-total at a time, or one-per-pony at a time.
    'these two options are used for the one-per-pony option.
    Private Audio_Last_Played As Date = DateTime.UtcNow
    Private Last_Audio_Length As Integer = 0

    Friend BehaviorStartTime As TimeSpan
    Friend BehaviorDesiredDuration As TimeSpan
    Friend BeingDragged As Boolean

    Friend ManualControl_P1 As Boolean = False
    Friend ManualControl_P2 As Boolean = False
    Private Effects_to_Remove As New List(Of Effect)

    Private ReadOnly EffectsLastUsed As New Dictionary(Of Effect, TimeSpan)

    Friend destination_coords As Point
    Friend follow_object_name As String = ""
    Friend follow_object As ISprite
    'Try to get the point where an object is going to, and go to that instead of where it is currently at.
    Friend lead_target As Boolean = False
#End Region

    <Flags()>
    Friend Enum Allowed_Moves As Byte
        None = 0
        Horizontal_Only = 1
        Vertical_Only = 2
        Diagonal_Only = 4
        Horizontal_Vertical = Horizontal_Only Or Vertical_Only
        Diagonal_Horizontal = Diagonal_Only Or Horizontal_Only
        Diagonal_Vertical = Diagonal_Only Or Vertical_Only
        All = Horizontal_Only Or Vertical_Only Or Diagonal_Only
        MouseOver = 8
        Sleep = 16
        Dragged = 32
    End Enum

    Public Sub New(base As PonyBase)
        Argument.EnsureNotNull(base, "base")
        _base = base
    End Sub

    Friend Sub PonySpeak(Optional line As PonyBase.Behavior.SpeakingLine = Nothing)
        'When hovering over with the mouse, don't talk more than once every x seconds.
        If Cursor_Halt AndAlso (internalTime - lastSpeakTime).TotalSeconds < 15 Then
            Exit Sub
        End If

        Dim lineToSpeak As String = ""

        If Not IsNothing(line) Then
            lineToSpeak = line.Text
            If line.SoundFile <> "" AndAlso Main.Instance.DirectXSoundAvailable Then
                PonyPlaySound(line.SoundFile)
            End If
        Else
            If Base.SpeakingLinesRandom.Count = 0 Then
                Exit Sub
            Else
                Dim randomGroupLines = Base.SpeakingLinesRandom.Where(
                    Function(lineByGroup) lineByGroup.Group = CurrentBehavior.Group).ToArray()
                If randomGroupLines.Length = 0 Then Exit Sub

                Dim randomLine = randomGroupLines(Rng.Next(randomGroupLines.Count))
                lineToSpeak = randomLine.Text
                If randomLine.SoundFile <> "" AndAlso Main.Instance.DirectXSoundAvailable Then
                    PonyPlaySound(randomLine.SoundFile)
                End If
            End If
        End If

        If Options.PonySpeechEnabled Then
            lastSpeakTime = internalTime
            lastSpeakLine = Me.Name & ": " & ControlChars.Quote & lineToSpeak & ControlChars.Quote
        End If

    End Sub

    Private Sub PonyPlaySound(filename As String)

        If My.Computer.FileSystem.FileExists(filename) Then

            If Not Options.SoundEnabled Then Exit Sub

            If Main.Instance.screen_saver_mode AndAlso Not Options.ScreensaverSoundEnabled Then
                Exit Sub
            End If

            'don't play sounds over other ones - wait until they finish.

            If Not Options.SoundSingleChannelOnly Then
                If DateTime.UtcNow.Subtract(Me.Audio_Last_Played).TotalMilliseconds <= Me.Last_Audio_Length Then Exit Sub
            Else
                If DateTime.UtcNow.Subtract(Main.Instance.Audio_Last_Played).TotalMilliseconds <= Main.Instance.Last_Audio_Length Then Exit Sub
            End If

            'If you get a MDA warning about loader locking - you'll just have to disable that exception message.  
            'Apparently it is a bug with DirectX that only occurs with Visual Studio...
            'We use DirectX now so that we can use MP3 instead of WAV files
            Dim audio As New Microsoft.DirectX.AudioVideoPlayback.Audio(filename)
            Try
                'volume is between -10000 and 0, with 0 being the loudest.
                audio.Volume = CInt(Options.SoundVolume * 10000 - 10000)

                Main.Instance.Active_Sounds.Add(audio)

                audio.Play()

                If Not Options.SoundSingleChannelOnly Then
                    Me.Last_Audio_Length = CInt(audio.Duration * 1000)
                    Me.Audio_Last_Played = DateTime.UtcNow
                Else
                    Main.Instance.Last_Audio_Length = CInt(audio.Duration * 1000) 'to milliseconds
                    Main.Instance.Audio_Last_Played = DateTime.UtcNow
                End If

            Catch ex As Exception
                If Main.Instance.Audio_Error_Shown = False AndAlso Main.Instance.screen_saver_mode = False Then
                    Main.Instance.Audio_Error_Shown = True
                    MsgBox("Error playing sound " & filename & " for " & Me.Directory & ControlChars.NewLine _
                           & "Further sound errors will be suppressed." & ControlChars.NewLine & ex.Message)
                End If
            Finally
                audio.Dispose()
            End Try
        End If
    End Sub

    Friend Sub ActivateEffects(currentTime As TimeSpan)

        If Options.PonyEffectsEnabled AndAlso sleeping = False _
          AndAlso Main.Instance.Dragging = False AndAlso _movingOnscreen = False Then

            For Each effect In CurrentBehavior.Effects
                If Not EffectsLastUsed.ContainsKey(effect) Then
                    EffectsLastUsed(effect) = TimeSpan.Zero
                End If
                If (currentTime - EffectsLastUsed(effect)).TotalMilliseconds >= effect.Repeat_Delay * 1000 Then

                    If effect.Repeat_Delay = 0 Then
                        If effect.already_played_for_currentbehavior = True Then Continue For
                    End If

                    effect.already_played_for_currentbehavior = True

                    Dim new_effect As Effect = effect.duplicate()

                    If new_effect.Duration <> 0 Then
                        new_effect.DesiredDuration = new_effect.Duration
                        new_effect.Close_On_New_Behavior = False
                    Else
                        If Me.Is_cursor_halted Then
                            new_effect.DesiredDuration = TimeSpan.FromSeconds(CurrentBehavior.MaxDuration).TotalSeconds
                        Else
                            new_effect.DesiredDuration = (BehaviorDesiredDuration - Me.CurrentTime).TotalSeconds
                        End If
                        new_effect.Close_On_New_Behavior = True
                    End If

                    'new_effect.Text = Name & "'s " & new_effect.name

                    If right Then
                        new_effect.direction = new_effect.placement_direction_right
                        new_effect.centering = new_effect.centering_right
                        new_effect.current_image_path = new_effect.right_image_path
                    Else
                        new_effect.direction = new_effect.placement_direction_left
                        new_effect.centering = new_effect.centering_left
                        new_effect.current_image_path = new_effect.left_image_path
                    End If

                    Dim directionsCount = [Enum].GetValues(GetType(Directions)).Length

                    If new_effect.direction = Directions.random Then
                        new_effect.direction = CType(Math.Round(Rng.NextDouble() * directionsCount - 2, 0), Directions)
                    End If
                    If new_effect.centering = Directions.random Then
                        new_effect.centering = CType(Math.Round(Rng.NextDouble() * directionsCount - 2, 0), Directions)
                    End If

                    If new_effect.direction = Directions.random_not_center Then
                        new_effect.direction = CType(Math.Round(Rng.NextDouble() * directionsCount - 3, 0), Directions)
                    End If
                    If new_effect.centering = Directions.random_not_center Then
                        new_effect.centering = CType(Math.Round(Rng.NextDouble() * directionsCount - 3, 0), Directions)
                    End If

                    If (right) Then
                        new_effect.Facing_Left = False
                    Else
                        new_effect.Facing_Left = True
                    End If

                    new_effect.Location = GetEffectLocation(new_effect.CurrentImageSize(),
                     new_effect.direction, Location, CurrentImageSize, new_effect.centering)

                    new_effect.behavior_name = CurrentBehavior.Name

                    new_effect.Owning_Pony = Me

                    Pony.CurrentAnimator.AddEffect(new_effect)
                    Me.Active_Effects.Add(new_effect)

                    EffectsLastUsed(effect) = currentTime

                End If
            Next
        End If

    End Sub

    'Decide what to do, move, and redraw our image.  The main loop.
    Private Sub UpdateOnce()
        internalTime += TimeSpan.FromMilliseconds(1000.0 / 30.0 * Options.TimeFactor)

        If Behaviors.Count = 0 Then
            Exit Sub
        End If

        If should_be_sleeping Then

            If sleeping Then
                If BeingDragged Then
                    Dim scale = GetScale()
                    If current_image_center = New Point Then
                        Location = CursorLocation - Size.Round(CurrentImageSize * CSng(scale) / 2)
                    Else
                        Location = CursorLocation - New Size(current_image_center)
                    End If
                End If
                Exit Sub
            Else
                Sleep()
                Exit Sub
            End If

        Else
            If sleeping Then
                WakeUp()
            End If
        End If

        If _movingOnscreen AndAlso IsNothing(CurrentBehavior) Then
            _movingOnscreen = False
        End If

        If Not Playing_Game AndAlso Not _movingOnscreen Then
            'In other parts of the code, setting the current behavior to nothing
            'means that we ran into some problem, and we should pick something else to do.
            If IsNothing(CurrentBehavior) Then
                'For Each effect In Active_Effects
                '    If effect.Close_On_New_Behavior = True Then
                '        'effect.Close()
                '    End If
                'Next
                CancelInteraction()
                SelectBehavior()

                'If the time left on the current behavior as expired, and we are not in some special mode, end the behavior.
            ElseIf internalTime > (BehaviorStartTime + BehaviorDesiredDuration) AndAlso Cursor_Halt = False AndAlso Not ManualControl_P1 AndAlso Not ManualControl_P2 Then
                If Not IsNothing(CurrentBehavior.EndLine) Then
                    PonySpeak(CurrentBehavior.EndLine)
                End If

                'Move to the next chain if there is one in a set of linked behaviors.
                If Not IsNothing(CurrentBehavior.LinkedBehavior) Then
                    SelectBehavior(CurrentBehavior.LinkedBehavior)
                Else
                    SelectBehavior()
                End If
                'If we are only halted because the cursor if hovering over us, re-play the behavior so that effects can continue if needed
            ElseIf internalTime > (BehaviorStartTime + BehaviorDesiredDuration) AndAlso Cursor_Halt = True AndAlso Not ManualControl_P1 AndAlso Not ManualControl_P2 Then
                SelectBehavior(CurrentBehavior)
            End If

            'if we should be, or are halted because we are/were in the cursor's way, 
            'either go to mouseover mode, or come out of it.
            If Cursor_Halt OrElse Is_cursor_halted Then
                ChangeMouseOverMode()
            End If
        End If

        'Decide our next movement, and repaint
        Move()

        If Not IsNothing(CurrentBehavior) Then ActivateEffects(internalTime)

    End Sub

    Friend Sub SelectBehavior(Optional ByRef Specified_Behavior As PonyBase.Behavior = Nothing)

        'If a pony has not been set in a valid group yet (just created), select a valid group.
        If Current_BehaviorGroup = -1 Then
            Dim groups As New List(Of Integer)

            For Each Behavior In Behaviors
                If Not groups.Contains(Behavior.Group) Then groups.Add(Behavior.Group)
            Next

            'If groups.Count = 0 Then
            '    groups.Add(0)
            'End If

            'Current_BehaviorGroup = groups(Rng.NextDouble() * (groups.Count - 1))
            Current_BehaviorGroup = groups(0)
        End If


        'Having no specified behavior when interacting means we've run to the last part of a chain and should end the interaction.
        If Is_Interacting AndAlso Is_Interaction_Initiator AndAlso IsNothing(Specified_Behavior) Then CancelInteraction()

        Dim selection = 0

        If IsNothing(Specified_Behavior) Then

            'Pick a random behavior until we get one that works, or we take too long deciding.

            Dim loop_total = 0
            Do Until loop_total > 200
                loop_total += 1

                selection = Rng.Next(Behaviors.Count)

                'only pick a behavior if its chance matches the dice, and is one we can pick randomly.
                If Rng.NextDouble() <= Behaviors(selection).chance_of_occurance AndAlso Behaviors(selection).Skip = False AndAlso _
                    (Behaviors(selection).Group = Current_BehaviorGroup OrElse Behaviors(selection).Group = 0) Then

                    'See if the behavior specifies that we follow another object
                    follow_object = Nothing
                    follow_object_name = Behaviors(selection).original_follow_object_name

                    Destination = Get_Destination()

                    If Destination = Vector2.Zero AndAlso Behaviors(selection).original_follow_object_name <> "" Then
                        'we picked a behavior that is designed to follow other object, but that object doesn't exist.
                        'We can't do this now, so pick another behavior.
                        Continue Do
                    End If

                    'We've decided on a behavior
                    CurrentBehavior = Behaviors(selection)
                    Exit Do
                End If

            Loop

            If loop_total > 200 Then
                'If we couldn't make up our minds and took to long, just pick the first one.
                CurrentBehavior = Behaviors(0)
            End If

            ' Console.WriteLine(loop_total)
        Else
            follow_object = Nothing
            follow_object_name = Specified_Behavior.original_follow_object_name

            Destination = Get_Destination()

            If Not Main.Instance.Preview_Mode AndAlso Destination = Vector2.Zero AndAlso Specified_Behavior.original_follow_object_name <> "" Then
                'We were told to do a specific behavior which follows an object, but that object doesn't exist.
                'Ignore the request, and go with a randomly selected behavior.
                'This will cancel an interaction if we were trying to run it.
                SelectBehavior()
                Exit Sub
            End If
            CurrentBehavior = Specified_Behavior
            Current_BehaviorGroup = CurrentBehavior.Group
        End If

        For Each effect In CurrentBehavior.Effects
            effect.already_played_for_currentbehavior = False
        Next

        BehaviorDesiredDuration = TimeSpan.FromSeconds((Rng.NextDouble() * (CurrentBehavior.MaxDuration - CurrentBehavior.MinDuration) + CurrentBehavior.MinDuration))
        'reset animations to frame 0 (otherwise all ponies with the same behavior will be synced up animation wise)
        BehaviorStartTime = internalTime

        'Speak if the behavior says to, or on a random chance otherwise.
        If Not IsNothing(CurrentBehavior.StartLine) Then
            PonySpeak(CurrentBehavior.StartLine)

            'Specified lines from behaviors should take precedence over random lines
            'Don't say a random line if we are following or have an ending line
            'or if we are interacting.
        ElseIf IsNothing(CurrentBehavior.EndLine) AndAlso follow_object_name = "" AndAlso
            Not Is_Interacting AndAlso Rng.NextDouble() <= Options.PonySpeechChance Then
            PonySpeak()
        End If

        If (CurrentBehavior.Allowed_Movement) = Allowed_Moves.None OrElse (CurrentBehavior.Allowed_Movement) = Allowed_Moves.MouseOver _
            OrElse (CurrentBehavior.Allowed_Movement) = Allowed_Moves.Sleep OrElse CurrentBehavior.Allowed_Movement = Allowed_Moves.Dragged Then

            horizontal = False
            vertical = False

            'Do we face right or left?
            right = Rng.NextDouble() < 0.5
            Exit Sub
        End If
        SetAllowableDirections()

        up = Rng.NextDouble() < 0.5
        right = Rng.NextDouble() < 0.5
    End Sub

    Private Sub SetAllowableDirections()
        'The rest of this function decides what direction we can/should go.
        Dim modes As New List(Of Allowed_Moves)
        If (CurrentBehavior.Allowed_Movement And Allowed_Moves.Vertical_Only) = Allowed_Moves.Vertical_Only Then
            modes.Add(Allowed_Moves.Vertical_Only)
        End If
        If (CurrentBehavior.Allowed_Movement And Allowed_Moves.Diagonal_Only) = Allowed_Moves.Diagonal_Only Then
            modes.Add(Allowed_Moves.Diagonal_Only)
        End If
        If (CurrentBehavior.Allowed_Movement And Allowed_Moves.Horizontal_Only) = Allowed_Moves.Horizontal_Only Then
            modes.Add(Allowed_Moves.Horizontal_Only)
        End If

        Dim selected_mode As Allowed_Moves

        If modes.Count = 0 Then
            'Throw New Exception("Unhandled movement type in SelectBehavior(): " & CurrentBehavior.Allowed_Movement)
            vertical = False
            horizontal = False
            diagonal = 0
            selected_mode = Allowed_Moves.None
        Else

            Dim selection = Rng.Next(modes.Count)
            selected_mode = modes(selection)
        End If

        Select Case selected_mode
            Case Allowed_Moves.Vertical_Only
                horizontal = False
                vertical = True
                diagonal = 0
            Case Allowed_Moves.Diagonal_Only
                horizontal = True
                vertical = True
                '(pick a random angle to go in)
                If up Then
                    diagonal = ((Rng.NextDouble() * 35) + 15) * (Math.PI / 180) ' converted to radians
                Else
                    diagonal = ((Rng.NextDouble() * 35) + 310) * (Math.PI / 180) ' converted to radians
                End If
                If right = False Then
                    diagonal = Math.PI - diagonal
                End If
            Case Allowed_Moves.Horizontal_Only
                horizontal = True
                vertical = False
                diagonal = 0
        End Select
    End Sub

    ''' <summary>
    ''' Used when the mouse hovers over us.
    ''' </summary>
    Sub ChangeMouseOverMode()

        'Should be halted, so halting now.
        If Cursor_Halt AndAlso Not Is_cursor_halted Then
            Is_cursor_halted = True

            previous_behavior = CurrentBehavior

            CurrentBehavior = GetAppropriateBehavior(Allowed_Moves.None, False)

            'Select a behavior one that is marked for mouseover mode
            For Each Behavior In Behaviors
                If Behavior.Group <> Current_BehaviorGroup Then Continue For
                If Behavior.Allowed_Movement = Allowed_Moves.MouseOver Then
                    CurrentBehavior = Behavior
                    For Each effect In CurrentBehavior.Effects
                        effect.already_played_for_currentbehavior = False
                    Next
                    Exit For
                End If
            Next

            Paint()

        End If

        'Returning out of being halted
        If Not Cursor_Halt And Is_cursor_halted Then
            CurrentBehavior = previous_behavior
            Is_cursor_halted = False
            Paint()
        End If

    End Sub

    Friend Sub Move()

        If IsNothing(CurrentBehavior) Then Exit Sub

        CurrentBehavior.blocked = False

        If Cursor_Immunity > 0 Then Cursor_Immunity -= 1

        If _movingOnscreen AndAlso Options.PonyTeleportEnabled Then
            StopMovingOnscreen()
            Exit Sub
        End If

        Dim speed As Double = CurrentBehavior.Speed * GetScale()

        'If the user selected to "control" us via the right click menu, decide what to do based on key presses.

        'if there is a game in progress, and it is in "setup", then override the take_control
        If IsNothing(Main.Instance.current_game) OrElse
            (Not IsNothing(Main.Instance.current_game) AndAlso
             Main.Instance.current_game.Status <> Game.GameStatus.Setup) Then
            With Main.Instance
                If ManualControl_P1 Then
                    speed = ManualControl(.PonyAction, .PonyUp, .PonyDown, .PonyLeft, .PonyRight, .PonySpeed)
                ElseIf ManualControl_P2 Then
                    speed = ManualControl(.PonyAction_2, .PonyUp_2, .PonyDown_2, .PonyLeft_2, .PonyRight_2, .PonySpeed_2)
                End If
            End With
        End If

        'If the behavior specified a follow object, or a point to go to, figure out where that is.
        Destination = Get_Destination()

        Dim movement As Vector2F

        'don't follow a destination if we are under player control
        'unless, there is a game playing and it is in setup mode (the only time in a game we should ignore the player input).
        If (Not Destination = Vector2.Zero AndAlso Not ManualControl_P1 AndAlso Not ManualControl_P2) OrElse _
            (Not Destination = Vector2.Zero AndAlso Not IsNothing(Main.Instance.current_game) AndAlso Main.Instance.current_game.Status = Game.GameStatus.Setup) Then

            'We DO have a destination to go to

            'good old Pythagorean theorem
            Dim distance = Vector.Distance(Center, Destination)

            'avoid overflows
            If distance = 0 Then distance = 1

            Dim direction = GetDestinationDirections(Destination)

            If direction(0) = Directions.left Then
                right = False
                movement.X = CSng(((Center.X - Destination.X) / (distance)) * -speed)
            Else
                right = True
                movement.X = CSng(((Destination.X - Center.X) / (distance)) * speed)
            End If

            movement.Y = CSng(((Center.Y - Destination.Y) / (distance)) * -speed)

            'we do not want to detect if we are at the destination if we are trying to move onscreen - we might stop at the destination and not
            'get out of the area we want to avoid.
            'However, we DO want to detect if we are exactly at our destination - our speed will go to 0 and we will be forever stuck there.

            If (distance <= 7) OrElse (_movingOnscreen AndAlso Vector2.Equals(Center(), Destination) AndAlso movement = SizeF.Empty) Then
                movement = Vector2F.Zero

                AtDestination = True
                If _movingOnscreen Then
                    StopMovingOnscreen()
                    Exit Sub
                End If

                'reached destination.

                If Going_Home Then
                    'don't disappear immediately when reaching a "house" - wait a bit.
                    If Not Opening_Door Then
                        CurrentBehavior.delay = 90
                        Opening_Door = True
                    Else
                        CurrentBehavior.delay -= 1
                    End If
                Else
                    'If this behavior links to another, we should end this one so we can move on to the next link.
                    If Not IsNothing(CurrentBehavior.LinkedBehavior) AndAlso speed <> 0 Then
                        BehaviorDesiredDuration = TimeSpan.Zero
                        Destination = Point.Empty
                    End If
                End If

            Else
                'We're not yet at our destination

                'If we were marked as being at our destination in our last move,
                'if means the target moved slightly.  We should pause a bit before continuing to follow.
                If AtDestination = True Then
                    CurrentBehavior.delay = 60
                End If

                'Only continue if the delay has expired.
                If CurrentBehavior.delay > 0 Then
                    AtDestination = False
                    CurrentBehavior.delay -= 1

                    Paint()
                    Exit Sub
                End If

                AtDestination = False

            End If

        Else

            'There is no destination, go wherever.

            If CurrentBehavior.delay > 0 Then
                CurrentBehavior.delay -= 1
                Paint()
                Exit Sub
            End If

            'if moving diagonally
            If diagonal <> 0 Then
                'Opposite = Hypotenuse * cosine of the angle
                movement.X = CSng(Math.Sqrt((speed ^ 2) * 2) * Math.Cos(diagonal))
                If movement.X < 0 Then
                    right = False
                Else
                    right = True
                End If
                'Adjacent = Hypotenuse * cosine of the angle
                '(negative because we are using pixel coordinates - down is positive)
                movement.Y = CSng(-Math.Sqrt((speed ^ 2) * 2) * Math.Sin(diagonal))
            Else
                'if not
                movement.X = CSng(speed * Math.Abs(CInt(horizontal)))
                movement.Y = CSng(speed * Math.Abs(CInt(vertical)))

                If Not right Then
                    movement.X = -movement.X
                End If

                If up = True Then
                    If movement.Y > 0 Then
                        movement.Y = -movement.Y
                    End If
                Else
                    If movement.Y < 0 Then
                        movement.Y = -movement.Y
                    End If
                End If

            End If

        End If

        Dim new_location = Point.Round(CType(Location, PointF) + movement)

        Dim NearCursor_Now = IsPonyNearMouseCursor(Location)
        ' Dim NearCursor_Now_all_Forms = IsPonyNearMouseCursor(current_location)
        Dim NearCursor_Future = IsPonyNearMouseCursor(new_location)
        '  Dim NearCursor_Future_All_Forms = IsPonyNearMouseCursor(new_location)

        Dim OnScreen_Now = IsPonyOnScreen(Location, Main.Instance.screens_to_use)
        Dim OnScreen_Future = IsPonyOnScreen(new_location, Main.Instance.screens_to_use)

        'Dim Playing_Game_And_OutofBounds = ((Playing_Game AndAlso Main.Instance.current_game.Status <> Game.GameStatus.Setup) AndAlso Not IsPonyInBox(new_location, Position.Allowed_Area))
        Dim playing_game_and_outofbounds = False

        Dim EnteringWindow_Now = False
        ' Dim EnteringWindow_Future = False

        If Options.WindowAvoidanceEnabled AndAlso _movingOnscreen = False Then
            EnteringWindow_Now = IsPonyEnteringWindow(Location, new_location, movement)
        End If

        Dim InAvoidanceZone_Now = IsPonyInAvoidanceArea(Location)
        Dim InAvoidanceZone_Future = IsPonyInAvoidanceArea(new_location)

        'if we ARE currently in the cursor's zone, then say that we should be halted (cursor_halt), save our current behavior so we 
        'can continue later, and set the current behavior to nothing so it will be changed.
        Cursor_Halt = NearCursor_Now
        If NearCursor_Now Then
            If _movingOnscreen Then
                StopMovingOnscreen() 'clear destination if moving_onscreen, otherwise we will get confused later.
            End If
            Paint() 'enable effects on mouseover.
            PonySpeak()
            Exit Sub
        ElseIf Is_cursor_halted Then
            'if we're not in the cursor's way, but still flagged that we are, exit mouseover mode.
            Cursor_Halt = False
            Cursor_Immunity = 30
            Exit Sub
        End If

        ' if we are heading into the cursor, change directions
        If NearCursor_Future Then
            Cursor_Halt = False

            Cursor_Immunity = 60

            'if we are moving to a destination, our path is blocked, and we need to abort the behavior
            'if we are just moving normally, just "bounce" off of the barrier.

            If Destination = Vector2.Zero Then
                Bounce(Me, Location, new_location, movement)
            Else
                CurrentBehavior = GetAppropriateBehavior(CurrentBehavior.Allowed_Movement, False)
            End If
            Exit Sub
        End If

        ''Check to see that we are moving off the screen, or into a zone we shouldn't (the zone set in the options or outside of our area when playing a game)
        If _movingOnscreen OrElse (OnScreen_Future AndAlso Not InAvoidanceZone_Future AndAlso Not playing_game_and_outofbounds) Then

            If EnteringWindow_Now Then
                If Destination = Vector2.Zero Then
                    Bounce(Me, Location, new_location, movement)
                Else
                    CurrentBehavior = Nothing
                End If
                Exit Sub
            End If

            'everything's cool.  Move and repaint.

            Location = new_location
            LastMovement = movement

            Paint(False)

            'check to see if we should interact at all

            If Options.PonyInteractionsEnabled AndAlso Is_Interacting = False AndAlso _movingOnscreen = False Then
                Dim interact As PonyBase.Interaction = GetReadiedInteraction()

                If Not IsNothing(interact) Then
                    StartInteraction(interact)
                End If
            End If

            'If we were trying to get out of a bad spot, and we find ourselves in a good area, continue on as normal...
            If _movingOnscreen AndAlso OnScreen_Now AndAlso Not InAvoidanceZone_Future AndAlso Not playing_game_and_outofbounds Then
                StopMovingOnscreen()
            Else

                'except if the user made changes to the avoidance area to include our current safe spot (we were already trying to avoid the area),
                'then get a new safe spot.

                If _movingOnscreen Then
                    If IsPonyInAvoidanceArea(Destination) OrElse Not IsPonyOnScreen(Destination, Main.Instance.screens_to_use) Then
                        Dim safespot = FindSafeDestination()
                        destination_coords = safespot
                    End If

                End If
            End If

            'if we were trying to get out of a bad area, but we are not moving, then continue on as normal.
            If _movingOnscreen AndAlso CurrentBehavior.Speed = 0 Then
                StopMovingOnscreen()
            End If

            'We are done.
            Exit Sub

        Else

            'The new move puts us off screen or into a bad area!
            'Sanity check time - are we even on screen now?
            If InAvoidanceZone_Now OrElse Not OnScreen_Now Then
                'we are no where! Find out where it is safe to be and run!

                If Main.Instance.Preview_Mode OrElse Options.PonyTeleportEnabled Then
                    Teleport()
                    Exit Sub
                End If

                Dim safespot = FindSafeDestination()

                _movingOnscreen = True

                If CurrentBehavior.Speed = 0 Then
                    CurrentBehavior = GetAppropriateBehavior(Allowed_Moves.All, True)
                End If

                follow_object = Nothing
                follow_object_name = ""
                destination_coords = safespot

                Paint(False)

                ' TODO: DO NOT update the start time, this prevents the time resetting when out-of-bounds. (Second) Need checking.
                'BehaviorStartTime = internalTime

                Exit Sub
            End If

        End If

        'Nothing to worry about, we are on screen, but our current behavior would take us 
        ' off-screen in the next move.  Just do something else.
        'if we are moving to a destination, our path is blocked: we'll wait for a bit.
        'if we are just moving normally, just "bounce" off of the barrier.

        If Destination = Vector2.Zero Then
            Bounce(Me, Location, new_location, movement)
            'we need to paint to reset the image centers
            Paint()
        Else
            If IsNothing(follow_object) Then
                'CurrentBehavior = Nothing
                speed = 0
            Else
                'do nothing but stare longingly in the direction of the object we want to follow...
                CurrentBehavior.blocked = True
                Paint()
            End If
        End If

    End Sub

    Sub StopMovingOnscreen()
        _movingOnscreen = False
        destination_coords = New Point(CurrentBehavior.original_destination_xcoord, CurrentBehavior.original_destination_ycoord)
        follow_object_name = CurrentBehavior.original_follow_object_name
        ' TODO: DO NOT update the start time, this prevents the behavior time resetting when returning from out-of-bounds. Needs checking
        ' for side effects, though.
        'BehaviorStartTime = internalTime
        Paint()
    End Sub

    'reverse directions as if we were bouncing off a boundary.
    Friend Sub Bounce(ByRef pony As Pony, ByRef current_location As Point, ByRef new_location As Point, movement As SizeF)

        If movement = SizeF.Empty Then
            Exit Sub
        End If

        'if we are moving in a simple direction (up/down, left/right) just reverse direction
        If movement.Width = 0 AndAlso movement.Height <> 0 Then
            up = Not up
            If diagonal <> 0 Then
                diagonal = 2 * Math.PI - diagonal
            End If
            Exit Sub
        End If
        If movement.Width <> 0 AndAlso movement.Height = 0 Then
            right = Not right
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

            If Not pony.IsPonyOnScreen(new_location_x, Main.Instance.screens_to_use) OrElse pony.IsPonyInAvoidanceArea(new_location_x) _
                OrElse pony.IsPonyEnteringWindow(current_location, new_location_x, New SizeF(movement.Width, 0)) Then
                x_bad = True
            End If

            If Not pony.IsPonyOnScreen(new_location_y, Main.Instance.screens_to_use) OrElse pony.IsPonyInAvoidanceArea(new_location_y) _
                OrElse pony.IsPonyEnteringWindow(current_location, new_location_y, New SizeF(0, movement.Height)) Then
                y_bad = True
            End If

        End If

        If Not x_bad AndAlso Not y_bad Then
            up = Not up
            right = Not right
            If diagonal <> 0 Then
                diagonal = Math.PI - diagonal
                diagonal = 2 * Math.PI - diagonal
            End If
            Exit Sub
        End If

        If x_bad AndAlso y_bad Then
            up = Not up
            right = Not right
            If diagonal <> 0 Then
                diagonal = Math.PI - diagonal
                diagonal = 2 * Math.PI - diagonal
            End If
            Exit Sub
        End If

        If x_bad Then
            right = Not right
            If diagonal <> 0 Then
                diagonal = Math.PI - diagonal
            End If
            Exit Sub
        End If
        If y_bad Then
            up = Not up
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
        Return Point.Round(CType(Location, PointF) + LastMovement * Number_Of_Interations)
    End Function

    Friend Sub Paint(Optional useOverrideBehavior As Boolean = True)
        visual_override_behavior = Nothing

        'If we are going to a particular point or following something, we need to pick the 
        'appropriate graphics to how we are moving instead of using what the behavior specifies.
        If Not Destination = Vector2.Zero AndAlso Not ManualControl_P1 AndAlso Not ManualControl_P2 Then ' AndAlso Not Playing_Game Then

            Dim horizontalDistance = Math.Abs(Destination.X - Center.X)
            Dim verticalDistance = Math.Abs(Destination.Y - Center.Y)
            Dim appropriate_behavior As PonyBase.Behavior = Nothing

            'We are supposed to be following, so say we can move any direction to do that.
            Dim allowed_movement = Allowed_Moves.All

            'if the distance to the destination is mostly horizontal, or mostly vertical, set the movement to either of those
            'This allows pegasi to fly up to reach their target instead of walking straight up.
            'This is weighted more on the vertical side for better effect
            If horizontalDistance * 0.75 > verticalDistance Then
                allowed_movement = allowed_movement And Allowed_Moves.Horizontal_Only
            Else
                allowed_movement = allowed_movement And Allowed_Moves.Vertical_Only
            End If

            If AtDestination OrElse CurrentBehavior.blocked OrElse CurrentBehavior.Speed = 0 OrElse CurrentBehavior.delay > 0 Then
                allowed_movement = Allowed_Moves.None
                Dim paint_stop_now = Paint_stop
                Paint_stop = True

                'If at our destination, we want to allow one final animation change.  
                'However after that, we want to stop painting as we may be stuck in a left-right loop
                'Detect here if the destination is between the right and left image centers, which would cause flickering between the two.
                If paint_stop_now Then

                    If Destination.X >= CurrentBehavior.LeftImageCenter.X + Location.X AndAlso
                        Destination.X <= CurrentBehavior.RightImageCenter.X + Location.X Then
                        '  Console.WriteLine(Me.Name & " paint stopped")
                        Exit Sub
                    End If

                End If

            Else
                Paint_stop = False
            End If

            If CurrentBehavior.Auto_Select_Images_On_Follow = True OrElse IsNothing(CurrentBehavior.FollowStoppedBehavior) OrElse IsNothing(CurrentBehavior.FollowMovingBehavior) Then
                appropriate_behavior = GetAppropriateBehavior(allowed_movement, True, Nothing)
            Else
                If allowed_movement = Allowed_Moves.None Then
                    appropriate_behavior = CurrentBehavior.FollowStoppedBehavior
                Else
                    appropriate_behavior = CurrentBehavior.FollowMovingBehavior
                End If
            End If

            If IsNothing(appropriate_behavior) Then Throw New Exception("Couldn't find appropriate behavior for Paint() method on follow.")

            If useOverrideBehavior Then visual_override_behavior = appropriate_behavior
        Else
            Paint_stop = False
        End If

        Dim new_center As New Point

        If right Then
            new_center = Point.Round(CurrentBehavior.RightImageCenter * CSng(GetScale()))
        Else
            new_center = Point.Round(CurrentBehavior.LeftImageCenter * CSng(GetScale()))
        End If

        If current_image_center = New Point Then
            current_image_center = new_center
        End If

        'reposition the form based on the new image center, if different:
        If current_image_center <> New Point AndAlso current_image_center <> new_center Then
            Location = New Point(Location.X - new_center.X + current_image_center.X, Location.Y - new_center.Y + current_image_center.Y)
            current_image_center = new_center
        End If

        Effects_to_Remove.Clear()

        For Each effect As Effect In Me.Active_Effects
            If effect.Close_On_New_Behavior Then
                If CurrentBehavior.Name <> effect.behavior_name Then
                    Effects_to_Remove.Add(effect)
                End If
            End If
        Next

        For Each effect In Effects_to_Remove
            Me.Active_Effects.Remove(effect)
            Main.Instance.Dead_Effects.Add(effect)
        Next

    End Sub

    Friend Sub Sleep()

        'Pick a sleep, mouseover, or 0 movement behavior, in that order.

        Dim sleep_behavior = GetAppropriateBehavior(Pony.Allowed_Moves.Sleep, False)

        If BeingDragged = False Then
            If sleep_behavior.Allowed_Movement <> Pony.Allowed_Moves.Sleep Then
                sleep_behavior = GetAppropriateBehavior(Pony.Allowed_Moves.MouseOver, False)
                If sleep_behavior.Allowed_Movement <> Pony.Allowed_Moves.MouseOver Then
                    sleep_behavior = GetAppropriateBehavior(Pony.Allowed_Moves.None, False)
                End If
            End If
        Else
            sleep_behavior = GetAppropriateBehavior(Pony.Allowed_Moves.Dragged, False)
            If sleep_behavior.Allowed_Movement <> Pony.Allowed_Moves.Dragged Then
                sleep_behavior = GetAppropriateBehavior(Pony.Allowed_Moves.Sleep, False)
                If sleep_behavior.Allowed_Movement <> Pony.Allowed_Moves.Sleep Then
                    sleep_behavior = GetAppropriateBehavior(Pony.Allowed_Moves.MouseOver, False)
                    If sleep_behavior.Allowed_Movement <> Pony.Allowed_Moves.MouseOver Then
                        sleep_behavior = GetAppropriateBehavior(Pony.Allowed_Moves.None, False)
                    End If
                End If
            End If
        End If

        SelectBehavior(sleep_behavior)
        BehaviorDesiredDuration = TimeSpan.FromHours(8)
        Paint()
        sleeping = True
    End Sub

    Friend Sub WakeUp()
        sleeping = False

        Cursor_Halt = False

        'Ponies added during sleep will not be initialized yet, so don't paint them.
        If Not IsNothing(CurrentBehavior) Then
            BehaviorDesiredDuration = TimeSpan.Zero
            Paint()
        End If

    End Sub

    'You can place effects at an offset to the pony, and also set them to the left or the right of themselves for big effects.
    Friend Function GetEffectLocation(ByRef EffectImageSize As Size, ByRef direction As Directions,
                                      ByRef ParentLocation As Point, ByRef ParentSize As Vector2, ByRef centering As Directions) As Point

        Dim point As Point

        With ParentSize * CSng(GetScale())
            Select Case direction
                Case Directions.bottom
                    point = New Point(CInt(ParentLocation.X + .X / 2), CInt(ParentLocation.Y + .Y))
                Case Directions.bottom_left
                    point = New Point(ParentLocation.X, CInt(ParentLocation.Y + .Y))
                Case Directions.bottom_right
                    point = New Point(CInt(ParentLocation.X + .X), CInt(ParentLocation.Y + .Y))
                Case Directions.center
                    point = New Point(CInt(ParentLocation.X + .X / 2), CInt(ParentLocation.Y + .Y / 2))
                Case Directions.left
                    point = New Point(ParentLocation.X, CInt(ParentLocation.Y + .Y / 2))
                Case Directions.right
                    point = New Point(CInt(ParentLocation.X + .X), CInt(ParentLocation.Y + .Y / 2))
                Case Directions.top
                    point = New Point(CInt(ParentLocation.X + .X / 2), ParentLocation.Y)
                Case Directions.top_left
                    point = New Point(ParentLocation.X, ParentLocation.Y)
                Case Directions.top_right
                    point = New Point(CInt(ParentLocation.X + .X), ParentLocation.Y)
            End Select

        End With

        Dim effectscaling = Options.ScaleFactor

        Select Case centering
            Case Directions.bottom
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width) / 2), CInt(point.Y - (effectscaling * EffectImageSize.Height)))
            Case Directions.bottom_left
                point = New Point(point.X, CInt(point.Y - (effectscaling * EffectImageSize.Height)))
            Case Directions.bottom_right
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width)), CInt(point.Y - (effectscaling * EffectImageSize.Height)))
            Case Directions.center
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width) / 2), CInt(point.Y - (effectscaling * EffectImageSize.Height) / 2))
            Case Directions.left
                point = New Point(point.X, CInt(point.Y - (effectscaling * EffectImageSize.Height) / 2))
            Case Directions.right
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width)), CInt(point.Y - (effectscaling * EffectImageSize.Height) / 2))
            Case Directions.top
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width) / 2), point.Y)
            Case Directions.top_left
                'no change
            Case Directions.top_right
                point = New Point(CInt(point.X - (effectscaling * EffectImageSize.Width)), point.Y)
        End Select

        Return point

    End Function

    'Pick a behavior that matches the speed (fast or slow) and direction we want to go in.
    'Use the specified behavior if it works.
    Friend Function GetAppropriateBehavior(ByRef movement As Allowed_Moves, ByRef speed As Boolean,
                                           Optional ByRef specified_behavior As PonyBase.Behavior = Nothing) As PonyBase.Behavior

        Dim selected_behavior_speed As Integer = 0
        Dim selected_behavior As PonyBase.Behavior = CurrentBehavior


        'does the current behavior work?
        If Not IsNothing(CurrentBehavior) Then
            If (CurrentBehavior.Allowed_Movement And movement) = movement OrElse movement = Allowed_Moves.All Then
                If CurrentBehavior.Speed = 0 AndAlso movement = Allowed_Moves.None Then
                    Return CurrentBehavior
                End If
                If CurrentBehavior.Speed <> 0 AndAlso movement = Allowed_Moves.All Then
                    Return CurrentBehavior
                End If
            End If
        Else
            selected_behavior = Behaviors(0)
        End If

        For Each behavior In Behaviors

            If behavior.Group <> Current_BehaviorGroup Then Continue For

            If Behavior.Allowed_Movement = Allowed_Moves.Sleep AndAlso movement <> Allowed_Moves.Sleep AndAlso movement <> Allowed_Moves.Dragged Then
                Continue For
            End If

            'skip behaviors that are parts of a chain and shouldn't be used individually
            'however, when being dragged or sleeping, we may still need to consider these.
            If behavior.Skip = True AndAlso movement <> Allowed_Moves.Dragged AndAlso movement <> Allowed_Moves.Sleep Then Continue For

            If (Behavior.Allowed_Movement And movement) = movement OrElse movement = Allowed_Moves.All Then

                If behavior.Speed = 0 AndAlso movement <> Allowed_Moves.All Then
                    Return behavior
                Else

                    'see if the specified behavior works.  If not, we'll find another.
                    If Not IsNothing(specified_behavior) Then
                        If (specified_behavior.Allowed_Movement And movement) = movement OrElse movement = Allowed_Moves.All Then

                            If Destination <> Vector2.Zero Then
                                right = (GetDestinationDirections(Destination)(0) = Directions.right)
                            End If

                            Return specified_behavior

                        End If
                    End If

                    'if this behavior has a destination or an object to follow, don't use it.
                    If (destination_coords.X <> 0 OrElse destination_coords.Y <> 0 OrElse _
                        follow_object_name <> "") AndAlso Not Playing_Game AndAlso Not _movingOnscreen Then
                        Continue For
                    End If

                    'If the user is pressing shift while "taking control"
                    If speed Then
                        If Math.Abs(behavior.Speed) > selected_behavior_speed Then
                            selected_behavior = behavior
                            selected_behavior_speed = CInt(Math.Abs(behavior.Speed))
                        End If
                    Else
                        If behavior.Speed <> 0 AndAlso (Math.Abs(behavior.Speed) < selected_behavior_speed OrElse (selected_behavior_speed = 0)) Then
                            selected_behavior_speed = CInt(Math.Abs(behavior.Speed))
                            selected_behavior = behavior
                        End If
                    End If
                End If


            End If
        Next

        Return selected_behavior

    End Function

    Shared Function GetScreenContainingPoint(point As Point) As Screen
        For Each screen In Main.Instance.screens_to_use
            If (screen.WorkingArea.Contains(point)) Then Return screen
        Next
        Return Main.Instance.screens_to_use(0)
    End Function

    'Test to see if we overlap with another application's window.
    Function IsPonyEnteringWindow(ByRef current_location As Point, ByRef new_location As Point, movement As SizeF) As Boolean
        If Not OperatingSystemInfo.IsWindows Then Return False

        Try
            If Main.Instance.Preview_Mode Then Return False
            If Not Options.WindowAvoidanceEnabled Then Return False

            If movement = SizeF.Empty Then Return False

            Dim scale = GetScale()

            Dim current_window_1 = WindowFromPoint(current_location)
            Dim current_window_2 = WindowFromPoint(New Point(CInt(current_location.X + (scale * CurrentImageSize.X)), CInt(current_location.Y + (scale * CurrentImageSize.Y))))
            Dim current_window_3 = WindowFromPoint(New Point(CInt(current_location.X + (scale * CurrentImageSize.X)), current_location.Y))
            Dim current_window_4 = WindowFromPoint(New Point(current_location.X, CInt(current_location.Y + (scale * CurrentImageSize.Y))))

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
                    new_window_2 = WindowFromPoint(New Point(CInt(new_location.X + (scale * CurrentImageSize.X)), CInt(new_location.Y + (scale * CurrentImageSize.Y))))
                    new_window_3 = WindowFromPoint(New Point(CInt(new_location.X + (scale * CurrentImageSize.X)), new_location.Y))
                Case Is < 0
                    new_window_1 = WindowFromPoint(new_location)
                    new_window_4 = WindowFromPoint(New Point(new_location.X, CInt(new_location.Y + (scale * CurrentImageSize.Y))))
            End Select

            Select Case movement.Height
                Case Is > 0
                    If (new_window_2) = IntPtr.Zero Then new_window_2 = WindowFromPoint(New Point(CInt(new_location.X + (scale * CurrentImageSize.X)), CInt(new_location.Y + (scale * CurrentImageSize.Y))))
                    If (new_window_4) = IntPtr.Zero Then new_window_4 = WindowFromPoint(New Point(new_location.X, CInt(new_location.Y + (scale * CurrentImageSize.Y))))
                Case Is < 0
                    If (new_window_1) = IntPtr.Zero Then new_window_1 = WindowFromPoint(new_location)
                    If (new_window_3) = IntPtr.Zero Then new_window_3 = WindowFromPoint(New Point(CInt(new_location.X + (scale * CurrentImageSize.X)), new_location.Y))
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

                    Dim process_id As Integer = 0
                    GetWindowThreadProcessId(collision, process_id)

                    'ignore collisions with other ponies or effects
                    If Options.PonyAvoidsPonies AndAlso process_id = Main.Instance.process_id Then
                        pony_collision_count += 1
                    Else

                        'we are colliding with another window boundary.
                        'are we already inside of it, and therefore should go through to the outside?
                        'or are we on the outside, and need to stay out?

                        If Options.PonyStaysInBox Then Continue For

                        Dim collisionArea As New RECT
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
        If Main.Instance.Preview_Mode Then Return True

        For Each screen In screenList
            If EveryLocationPointContainedInBounds(location, screen.WorkingArea) Then Return True
        Next

        Return False
    End Function

    Function EveryLocationPointContainedInBounds(location As Point, bounds As Rectangle) As Boolean
        Dim scale = GetScale()

        'test center (or upper right if no center is defined)
        If Not bounds.Contains(location.X + current_image_center.X, location.Y + current_image_center.Y) Then Return False

        If up AndAlso right Then
            'test upper right corner
            If Not bounds.Contains(CInt(location.X + (scale * CurrentImageSize.X)), location.Y) Then Return False
        End If

        If up AndAlso Not right Then
            'top left
            If Not bounds.Contains(location) Then Return False
        End If

        If Not up AndAlso right Then
            'bottom right
            If Not bounds.Contains(CInt(location.X + (scale * CurrentImageSize.X)),
                               CInt(location.Y + (scale * CurrentImageSize.Y))) Then Return False
        End If

        If Not up AndAlso Not right Then
            'bottom left
            If Not bounds.Contains(location.X, CInt(location.Y + (scale * CurrentImageSize.Y))) Then Return False
        End If

        Return True
    End Function

    Shared Function IsPonyInBox(ByRef location As Point, ByRef box As Rectangle) As Boolean
        Return box.IsEmpty OrElse box.Contains(location)
    End Function

    ''are we inside the user specified "Everfree Forest"?
    Function IsPonyInAvoidanceArea(ByRef new_location As System.Drawing.Point) As Boolean

        If Main.Instance.Preview_Mode Then
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

        Dim scale = GetScale()

        'add center (or upper right if no center is defined)
        Dim center As Point = New Point(
                              CInt(new_location.X + (scale * current_image_center.X)),
                              CInt(new_location.Y + (scale * current_image_center.Y)))

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
        If Main.Instance.screen_saver_mode Then Return False

        If Cursor_Immunity > 0 Then Return False

        'ignore this if we are interacting - we don't want to cancel it.
        If Me.Is_Interacting Then Return False

        If ManualControl_P1 OrElse ManualControl_P2 Then Return False

        With Main.Instance
            Dim scale = GetScale()

            For Each behavior In Behaviors
                If behavior.Allowed_Movement = Pony.Allowed_Moves.MouseOver Then
                    Dim rightCenter As Point
                    If right AndAlso behavior.RightImageCenter <> Vector2.Zero Then
                        rightCenter = New Point(CInt(location.X + (scale * (behavior.RightImageCenter.X))),
                                                CInt(location.Y + (scale * (behavior.RightImageCenter.Y))))
                    Else
                        rightCenter = New Point(CInt(location.X + (scale * (Behaviors(0).RightImageSize.X)) / 2),
                          CInt(location.Y + (scale * (Behaviors(0).RightImageSize.Y)) / 2))
                    End If
                    Dim leftCenter As Point
                    If Not right AndAlso behavior.LeftImageCenter <> Vector2.Zero Then
                        leftCenter = New Point(CInt(location.X + (scale * (behavior.LeftImageCenter.X))),
                                             CInt(location.Y + (scale * (behavior.LeftImageCenter.Y))))
                    Else
                        leftCenter = New Point(CInt(location.X + (scale * (Behaviors(0).LeftImageSize.X)) / 2),
                              CInt(location.Y + (scale * (Behaviors(0).LeftImageSize.Y)) / 2))
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

    Friend Sub Teleport()

        If Main.Instance.Preview_Mode Then
            Dim preview_center = Pony.PreviewWindowRectangle.Location
            Location = New Point(preview_center.X + 10, preview_center.Y + 10)
            Exit Sub
        End If

        Dim UsableScreens = Main.Instance.screens_to_use

        Dim dice = 0
        Dim random_screen As Screen = Nothing
        Dim teleport_location As Point


        For tries = 0 To 300
            dice = Rng.Next(UsableScreens.Count)

            random_screen = UsableScreens(dice)
            teleport_location = New Point(
                CInt(random_screen.WorkingArea.X + Math.Round(Rng.NextDouble() * random_screen.WorkingArea.Width, 0)),
                CInt(random_screen.WorkingArea.Y + Math.Round(Rng.NextDouble() * random_screen.WorkingArea.Height, 0)))

            If IsPonyInAvoidanceArea(teleport_location) = False Then
                Exit For
            End If

        Next

        Location = teleport_location
    End Sub

    'Find a spot on the screen that the pony is allowed to be (similar to teleport, but just reports the point found).
    Friend Function FindSafeDestination() As Point

        If Main.Instance.Preview_Mode Then
            Dim preview_center = Pony.PreviewWindowRectangle.Location
            Return New Point(preview_center.X + 10, preview_center.Y + 10)
        End If

        Dim UsableScreens = Main.Instance.screens_to_use

        Dim teleport_location As Point = Nothing

        If teleport_location = Nothing Then

            Dim dice = 0
            Dim random_screen As Screen = Nothing
            teleport_location = Point.Empty


            For tries = 0 To 300
                dice = Rng.Next(UsableScreens.Count)

                random_screen = UsableScreens(dice)
                teleport_location = New Point(
                    CInt(random_screen.WorkingArea.X + Math.Round(Rng.NextDouble() * random_screen.WorkingArea.Width, 0)),
                    CInt(random_screen.WorkingArea.Y + Math.Round(Rng.NextDouble() * random_screen.WorkingArea.Height, 0)))

                If IsPonyInAvoidanceArea(teleport_location) = False Then Exit For
            Next
        End If

        Return teleport_location

    End Function

    Friend Function GetDestinationDirections(ByRef destination As Point) As IList(Of Directions)

        Dim direction(2) As Directions

        Dim scale = GetScale()

        Dim right_image_center = New Point(
                                 CInt(Location.X + (scale * CurrentBehavior.RightImageCenter.X)),
                                 CInt(Location.Y + (scale * CurrentBehavior.RightImageCenter.Y)))
        Dim left_image_center = New Point(
                                CInt(Location.X + (scale * CurrentBehavior.LeftImageCenter.X)),
                                CInt(Location.Y + (scale * CurrentBehavior.LeftImageCenter.Y)))

        If right_image_center.X > destination.X AndAlso left_image_center.X < destination.X OrElse
            destination.X - Center.X <= 0 Then
            direction(0) = Directions.left
        Else
            direction(0) = Directions.right
        End If

        If (right_image_center.Y > destination.Y AndAlso left_image_center.Y < destination.Y) OrElse
           (right_image_center.Y < destination.Y AndAlso left_image_center.Y > destination.Y) OrElse
           destination.Y - Center.Y <= 0 Then
            direction(1) = Directions.top
        Else
            direction(1) = Directions.bottom
        End If
        Return direction

    End Function

    Friend ReadOnly Property CurrentImageSize As Vector2
        Get
            Dim behavior = If(visual_override_behavior, CurrentBehavior)
            Return If(right, behavior.RightImageSize, behavior.LeftImageSize)
        End Get
    End Property

    Friend Function Center() As Point

        Dim image_center As Point = GetImageCenter()

        If image_center = New Point AndAlso Not IsNothing(CurrentBehavior) Then
            Dim scale = GetScale()
            image_center = New Point(CInt((scale * (CurrentImageSize.X) / 2)), CInt((scale * (CurrentImageSize.Y) / 2)))
        End If

        Return New Point(Me.Location.X + image_center.X, Me.Location.Y + image_center.Y)

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

                        If already_added = True Then
                            Continue For
                        End If

                        Interaction.InteractsWith.Add(pony)
                    End If
                Next
            Next
        Next

    End Sub

    Sub CancelInteraction()

        Is_Interacting = False

        If IsNothing(_currentInteraction) Then Exit Sub

        If Me.Is_Interaction_Initiator Then
            For Each Pony In _currentInteraction.InteractsWith

                'Check to see if we are interacting with ourselves.  If so, avoid an infinite loop
                If ReferenceEquals(Me, Pony) Then
                    Continue For
                End If

                'check that the other pony is actually in an interaction
                If IsNothing(Pony._currentInteraction) Then
                    Continue For
                End If

                'check to make sure that I am the initiator of the interaction that this pony is running
                'Note:  Without this check, multiple sets of the same pony interacting with themselves will cause
                'an infinite recursion.
                If Not IsNothing(Pony._currentInteraction.Initiator) AndAlso ReferenceEquals(Pony._currentInteraction.Initiator, Me) Then
                    Pony.CancelInteraction()
                End If
            Next
        End If

        interactionDelayUntil = internalTime + TimeSpan.FromSeconds(_currentInteraction.Reactivation_Delay)

        _currentInteraction = Nothing
        Is_Interaction_Initiator = False

    End Sub

    Friend Sub StartInteraction(ByRef interaction As PonyBase.Interaction)

        Is_Interaction_Initiator = True
        _currentInteraction = interaction
        SelectBehavior(interaction.BehaviorList(Rng.Next(interaction.BehaviorList.Count)))

        'do we interact with ALL targets, including copies, or just the pony that we ran into?
        If interaction.Targets_Activated <> PonyBase.Interaction.TargetActivation.One Then
            For Each targetPony In interaction.InteractsWith
                targetPony.StartInteractionAsTarget(CurrentBehavior.Name, Me, interaction)
            Next
        Else
            interaction.Trigger.StartInteractionAsTarget(CurrentBehavior.Name, Me, interaction)
        End If


        Is_Interacting = True

    End Sub

    Friend Sub StartInteractionAsTarget(ByRef BehaviorName As String, ByRef initiator As Pony, interaction As PonyBase.Interaction)
        For Each behavior In Behaviors
            If BehaviorName = behavior.Name Then
                SelectBehavior(behavior)
                Exit For
            End If
        Next

        interaction.Initiator = initiator
        Is_Interaction_Initiator = False
        _currentInteraction = interaction
        Is_Interacting = True
    End Sub

    Function GetReadiedInteraction() As PonyBase.Interaction

        'If we recently ran an interaction, don't start a new one until the delay expires.
        If internalTime < interactionDelayUntil Then
            Return Nothing
        End If


        For Each interaction In Interactions

            For Each target As Pony In interaction.InteractsWith

                'don't start an interaction if we or the target haven't finished loading yet
                If IsNothing(target.CurrentBehavior) Then
                    Continue For
                End If

                'Don't start a new interaction for ponies already in one.
                If target.Is_Interacting Then
                    Continue For
                End If

                'Ponies shouldn't interact with themselves
                If ReferenceEquals(target, Me) Then
                    Continue For
                End If

                ' Make sure that all targets are present, if all are required.
                If interaction.Targets_Activated = PonyBase.Interaction.TargetActivation.All AndAlso
                    interaction.InteractsWith.Count <> interaction.InteractsWithByDirectory.Count Then
                    Continue For
                End If

                Dim distance = Vector.Distance(Location + New Size(CInt(CurrentImageSize.X / 2), CInt(CurrentImageSize.Y / 2)),
                                                 target.Location + New Size(CInt(target.CurrentImageSize.X / 2), CInt(target.CurrentImageSize.Y / 2)))

                If distance <= interaction.Proximity_Activation_Distance Then

                    Dim dice = Rng.NextDouble()

                    If dice <= interaction.Probability Then
                        interaction.Trigger = target
                        Return interaction
                    End If

                End If

            Next

        Next

        Return Nothing

    End Function

    Friend Function GetScale() As Double
        Return If(Base.Scale <> 0, Base.Scale, Options.ScaleFactor)
    End Function

    Friend Function GetImageCenter() As Point

        Dim scale = GetScale()

        If current_image_center = New Point Then
            If Not IsNothing(CurrentBehavior) Then
                Return New Point(CInt(scale * (CurrentImageSize.X / 2)), CInt(scale * (CurrentImageSize.Y / 2)))
            End If
        Else
            Return New Point(current_image_center.X, current_image_center.Y)
        End If
    End Function

    Sub Remove()
        'Removes a pony from memory.
        Me.CancelInteraction()

        Pony.CurrentAnimator.RemovePony(Me)

        ' TODO: Unload images from sprite interface.

    End Sub

    Private lastSpeakTime As TimeSpan = TimeSpan.FromDays(-1)
    Private lastSpeakLine As String
    Friend internalTime As TimeSpan
    Private lastUpdateTime As TimeSpan

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

    Public Sub Start(startTime As System.TimeSpan) Implements ISprite.Start
        Dim area = Rectangle.Empty
        For Each screen In Main.Instance.screens_to_use
            area = Rectangle.Union(area, screen.WorkingArea)
        Next
        CurrentBehavior = Behaviors(0)
        'Location.X = CInt(Rng.NextDouble() * (area.Width - CurrentImageSize.X) + area.X)
        'Location.Y = CInt(Rng.NextDouble() * (area.Height - CurrentImageSize.Y) + area.Y)
        internalTime = startTime
        lastUpdateTime = startTime
        Teleport()
        UpdateOnce()
    End Sub

    Public Sub Update(updateTime As System.TimeSpan) Implements ISprite.Update
        Dim difference = updateTime - lastUpdateTime
        While difference.TotalMilliseconds > 0
            UpdateOnce()
            difference -= TimeSpan.FromMilliseconds(1000.0 / 30.0 / Options.TimeFactor)
        End While
        lastUpdateTime = updateTime - difference
    End Sub

    Public ReadOnly Property CurrentTime As System.TimeSpan Implements ISprite.CurrentTime
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
            Dim behavior = If(visual_override_behavior, CurrentBehavior)
            Dim path = If(right, behavior.RightImagePath, behavior.LeftImagePath)
            If String.IsNullOrEmpty(path) Then
                Console.WriteLine("Encountered a null or empty path.")
                Console.WriteLine(New Diagnostics.StackTrace(True))
            End If
            Return path
        End Get
    End Property

    Public ReadOnly Property Region As System.Drawing.Rectangle Implements ISprite.Region
        Get
            If IsNothing(CurrentBehavior) Then
                Dim previous = "<Nothing>"
                If Not IsNothing(Me.previous_behavior) Then previous = Me.previous_behavior.Name
                Console.WriteLine("Null behavior. Name = " & Me.Directory & " Previous = " & previous)
                CurrentBehavior = Behaviors(0)
            End If
            Dim width = CInt(CurrentImageSize.X * Options.ScaleFactor)
            Dim height = CInt(CurrentImageSize.Y * Options.ScaleFactor)
            Return New Rectangle(Location, New Size(width, height))
        End Get
    End Property

    Private Function ManualControl(ponyAction As Boolean,
                              ponyUp As Boolean, ponyDown As Boolean, ponyLeft As Boolean, ponyRight As Boolean,
                              ponySpeed As Boolean) As Double
        diagonal = 0
        If Not Playing_Game AndAlso ponyAction Then
            Cursor_Halt = True
            Paint() 'enable effects on mouseover.
            Return CurrentBehavior.Speed * GetScale()
        Else
            'if we're not in the cursor's way, but still flagged that we are, exit mouseover mode.
            If Is_cursor_halted Then
                Cursor_Halt = False
                Return CurrentBehavior.Speed * GetScale()
            End If
        End If

        Dim appropriateMovement = Allowed_Moves.None
        vertical = False
        horizontal = False
        If ponyUp AndAlso Not ponyDown Then
            up = True
            vertical = True
            appropriateMovement = appropriateMovement Or Allowed_Moves.Vertical_Only
        End If
        If ponyDown AndAlso Not ponyUp Then
            up = False
            vertical = True
            appropriateMovement = appropriateMovement Or Allowed_Moves.Vertical_Only
        End If
        If ponyRight AndAlso Not ponyLeft Then
            right = True
            horizontal = True
            appropriateMovement = appropriateMovement Or Allowed_Moves.Horizontal_Only
        End If
        If ponyLeft AndAlso Not ponyRight Then
            right = False
            horizontal = True
            appropriateMovement = appropriateMovement Or Allowed_Moves.Horizontal_Only
        End If
        If appropriateMovement = (Allowed_Moves.Horizontal_Only Or Allowed_Moves.Vertical_Only) Then
            appropriateMovement = Allowed_Moves.Diagonal_Only
        End If
        CurrentBehavior = GetAppropriateBehavior(appropriateMovement, ponySpeed)
        Dim speedupFactor = If(ponySpeed, 2, 1)
        Return If(appropriateMovement = Allowed_Moves.None, 0, CurrentBehavior.Speed * GetScale() * speedupFactor)
    End Function

    Friend Function Get_Destination() As Point

        'if we are off-screen and trying to get back on, just return the pre-calculated coordinates.
        If MovingOnscreen Then
            Return destination_coords
        End If

        'If being recalled to a house
        If Going_Home Then
            Return Destination
        End If

        'If we should be following something, but we don't know what yet, select a pony/effect to follow
        If (follow_object_name <> "" AndAlso IsNothing(follow_object)) Then

            'If we are interacting, and the name of the pony we should be following matches that of the trigger, follow that one.
            'Otherwise, we may end up following the wrong copy if there are more than one.
            If Is_Interacting AndAlso
                String.Equals(Trim(follow_object_name), Trim(CurrentInteraction.Trigger.Directory), StringComparison.OrdinalIgnoreCase) Then
                follow_object = CurrentInteraction.Trigger
                Return New Point(CurrentInteraction.Trigger.Center.X + destination_coords.X,
                                 CurrentInteraction.Trigger.Center.Y + destination_coords.Y)
            End If
            'For the reverse case of a trigger pony trying to find out which initiator to follow when interacting.
            If Is_Interacting AndAlso Not IsNothing(CurrentInteraction.Initiator) AndAlso
                String.Equals(Trim(follow_object_name), Trim(CurrentInteraction.Initiator.Directory), StringComparison.OrdinalIgnoreCase) Then
                follow_object = CurrentInteraction.Initiator
                Return New Point(CurrentInteraction.Initiator.Location.X + destination_coords.X,
                                 CurrentInteraction.Initiator.Location.Y + destination_coords.Y)
            End If

            'If not interacting, or following a different pony, we need to figure out which one.

            Dim ponies_to_follow As New List(Of Pony)

            Dim found = False

            For Each ponyToFollow In CurrentAnimator.Ponies()
                If String.Equals(ponyToFollow.Directory, follow_object_name, StringComparison.OrdinalIgnoreCase) Then
                    ponies_to_follow.Add(ponyToFollow)
                    found = True
                End If
            Next

            If ponies_to_follow.Count <> 0 Then

                'pick a random copy if there is more than one.
                Dim dice = Rng.Next(ponies_to_follow.Count)
                follow_object = ponies_to_follow(dice)
                Return New Point(ponies_to_follow(dice).Location.X + destination_coords.X,
                                 ponies_to_follow(dice).Location.Y + destination_coords.Y)

            End If

            'Apparently we are not following a pony, but an effect...

            Dim effects_to_follow As New List(Of Effect)

            For Each effect In CurrentAnimator.Effects()
                If LCase(effect.Name) = follow_object_name Then
                    effects_to_follow.Add(effect)
                    found = True
                End If
            Next

            If found = False Then
                'We didn't find a match, so stop.
                Return New Point()
            End If

            If effects_to_follow.Count <> 0 Then
                Dim dice = Rng.Next(effects_to_follow.Count)
                follow_object = effects_to_follow(dice)
                Return New Point(effects_to_follow(dice).Location.X + destination_coords.X,
                                 effects_to_follow(dice).Location.Y + destination_coords.Y)
            End If
        End If

        If Not IsNothing(follow_object) Then
            'We've already selected an object to follow previously.
            If follow_object.GetType() Is GetType(Pony) Then
                Dim follow_pony As Pony = DirectCast(follow_object, Pony)
                If lead_target Then
                    Return follow_pony.FutureLocation()
                Else
                    Return New Point(CInt(follow_pony.Center.X + (follow_pony.GetScale() * destination_coords.X)), _
                                     CInt(follow_pony.Center.Y + (follow_pony.GetScale() * destination_coords.Y)))
                End If
            Else
                Dim follow_effect As Effect = DirectCast(follow_object, Effect)
                Return New Point(follow_effect.Center.X + destination_coords.X, follow_effect.Center.Y + destination_coords.Y)
            End If
        End If

        ''We are not following an object, but going to a point on the screen.
        'If Not IsNothing(screen) AndAlso destination_xcoord <> 0 AndAlso destination_ycoord <> 0 Then
        '    Return New Point(0.01 * destination_xcoord * screen.WorkingArea.Width + screen.WorkingArea.X, _
        '                     0.01 * destination_ycoord * screen.WorkingArea.Height + screen.WorkingArea.Y)
        'End If
        If destination_coords.X <> 0 AndAlso destination_coords.Y <> 0 Then
            Return New Point(CInt(0.01 * destination_coords.X), CInt(0.01 * destination_coords.Y))
        End If

        'no destination
        Return New Point()

    End Function
End Class

Class Effect
    Implements ISprite

    Friend Name As String = ""
    Friend Location As New Point()
    Friend translated_location As New Point()

    Friend beingDragged As Boolean = False

    Friend dont_repeat_image_animations As Boolean = False

    Friend behavior_name As String
    Friend Pony_Name As String
    Friend Owning_Pony As Pony

    Friend right_image_path As String
    Friend right_image_size As Size
    Friend left_image_path As String
    Friend left_image_size As Size
    Friend current_image_path As String
    Friend Duration As Double
    Friend DesiredDuration As Double

    Friend Repeat_Delay As Double

    Friend placement_direction_right As Directions
    Friend centering_right As Directions
    Friend placement_direction_left As Directions
    Friend centering_left As Directions

    Friend centering As Directions
    Friend direction As Directions

    Friend Facing_Left As Boolean = False

    Private start_time As TimeSpan
    Friend Close_On_New_Behavior As Boolean = False

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

        Dim UsableScreens = Main.Instance.screens_to_use

        Dim dice = 0
        Dim random_screen As Screen = Nothing
        Dim teleport_location As Point


        '  For tries = 0 To 300
        dice = Rng.Next(UsableScreens.Count)

        random_screen = UsableScreens(dice)
        teleport_location = New Point(
            CInt(random_screen.WorkingArea.X + Math.Round(Rng.NextDouble() * (random_screen.WorkingArea.Width - left_image_size.Width), 0)),
            CInt(random_screen.WorkingArea.Y + Math.Round(Rng.NextDouble() * (random_screen.WorkingArea.Height - left_image_size.Height), 0)))

        'If IsPonyOnScreen(teleport_location) = False Then
        '    Exit For
        'End If

        'Next

        Location = teleport_location

    End Sub

    Overridable Function duplicate() As Effect

        Dim new_effect As New Effect(right_image_path, left_image_path)

        new_effect.Name = Name
        new_effect.behavior_name = behavior_name

        new_effect.Pony_Name = Pony_Name

        new_effect.Duration = Duration
        new_effect.Repeat_Delay = Repeat_Delay
        new_effect.placement_direction_right = placement_direction_right
        new_effect.centering_right = centering_right
        new_effect.placement_direction_left = placement_direction_left
        new_effect.centering_left = centering_left

        new_effect.dont_repeat_image_animations = dont_repeat_image_animations

        new_effect.follow = follow

        new_effect.already_played_for_currentbehavior = already_played_for_currentbehavior

        Return new_effect

    End Function

    Friend Function Center() As Point
        Dim scale As Double

        If Not IsNothing(Owning_Pony) Then
            scale = Owning_Pony.GetScale()
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

    Public Sub Start(startTime As System.TimeSpan) Implements ISprite.Start
        current_image_path = If(Facing_Left, left_image_path, right_image_path)
        start_time = startTime
        internalTime = startTime
    End Sub

    Public Sub Update(updateTime As System.TimeSpan) Implements ISprite.Update
        internalTime = updateTime
        current_image_path = If(Facing_Left, left_image_path, right_image_path)
        If beingDragged Then
            Location = Pony.CursorLocation - New Size(CInt(CurrentImageSize.Width / 2), CInt(CurrentImageSize.Height / 2))
        End If
    End Sub

    Public ReadOnly Property CurrentTime As System.TimeSpan Implements ISprite.CurrentTime
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
                    RecallPony(Me, currentTime)
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

                deployed_pony.Location = instance.Location + New Size(base.DoorPosition) - New Size(deployed_pony.GetImageCenter())

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

    Private Sub RecallPony(instance As Effect, currentTime As TimeSpan)

        Dim choices As New List(Of String)

        Dim all As Boolean = False

        For Each entry In base.Visitors
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
                For Each otherpony In base.Visitors
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

                If pony.Is_Interacting Then Exit Sub
                If pony.BeingDragged Then Exit Sub

                If pony.sleeping Then pony.WakeUp()

                pony.Destination = instance.Location + New Size(base.DoorPosition)
                pony.Going_Home = True
                pony.CurrentBehavior = pony.GetAppropriateBehavior(pony.Allowed_Moves.All, False)
                pony.BehaviorDesiredDuration = TimeSpan.FromMinutes(5)

                deployedPonies.Remove(pony)

                Console.WriteLine(Me.Name & " - Recalled " & pony.Directory)

                Exit Sub
            End If
        Next

    End Sub

End Class

Friend Enum Directions

    top = 0
    bottom = 1
    left = 2
    right = 3
    bottom_right = 4
    bottom_left = 5
    top_right = 6
    top_left = 7
    center = 8
    random = 9
    random_not_center = 10

End Enum