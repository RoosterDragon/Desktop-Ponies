Imports System.Globalization
Imports System.IO
Imports DesktopSprites.SpriteManagement

Public Class Game
    Public Const RootDirectory = "Games"
    Public Const ConfigFilename = "game.ini"

    Public Enum BallType
        ''' <summary>
        ''' Is pushed around and slows to a stop.  No Gravity
        ''' </summary>
        Soccer
        ''' <summary>
        ''' Bounces around and doesn't stop/slow
        ''' </summary>
        PingPong
    End Enum

    Public Enum GameStatus
        Setup
        SetupWaitForPositions
        SetupAddBalls
        SetupReadyBalls
        InProgress
        Completed
    End Enum

    Private Enum ScoreStyle
        BallAtGoal
        BallHitsOtherTeam
        BallDestroyed
        BallAtSides
    End Enum

    Public Enum PlayerActionType
        NotSet = 0
        ReturnToStart = 1
        ChaseBall = 2
        AvoidBall = 3
        ThrowBallToGoal = 4
        ThrowBallToTeammate = 5
        ThrowBallAtTarget = 6
        ThrowBallReflect = 7
        ApproachOwnGoal = 8
        ApproachTargetGoal = 9
        LeadBall = 10
        ApproachTarget = 12
        Idle = 13
    End Enum

    Public Property Name As String = ""
    Public Property Description As String = ""

    Private ReadOnly _speedOverride As Double
    Public ReadOnly Property SpeedOverride As Double
        Get
            Return _speedOverride
        End Get
    End Property

    Private Const MinTeams As Integer = 2
    Private ReadOnly _teams As New List(Of Team)()
    Public ReadOnly Property Teams As List(Of Team)
        Get
            Return _teams
        End Get
    End Property
    Private ReadOnly allPlayers As New List(Of Position)()

    Private ReadOnly _balls As New List(Of Ball)()
    Public ReadOnly Property Balls As List(Of Ball)
        Get
            Return _balls
        End Get
    End Property
    Private ReadOnly activeBalls As New List(Of Ball)()

    Private ReadOnly goals As New List(Of GoalArea)()

    Private minBalls As Integer
    Private maxBalls As Integer

    Public Property Status As GameStatus
    Private maxScore As Integer

    Private scoreboard As GameScoreboard
    Private ReadOnly scoringStyles As New List(Of ScoreStyle)()

    Public Property GameScreen As Screen

    Private manualControlPlayerOne As Pony
    Private manualControlPlayerTwo As Pony

    Public Sub New(ponyCollection As PonyCollection, ponyContext As PonyContext, directory As String)
        Dim gameData As String = Nothing
        Dim descriptionData As String = Nothing
        Dim positionData As New List(Of String)()
        Dim ballData As New List(Of String)()
        Dim goalData As New List(Of String)()

        Using configFile As New StreamReader(Path.Combine(directory, Game.ConfigFilename))
            Do Until configFile.EndOfStream
                Dim line = configFile.ReadLine

                ' Ignore blank lines, or lines commented out with the single quote character.
                If line = "" OrElse line(0) = "'" Then Continue Do

                Dim columns = CommaSplitQuoteQualified(line)

                If UBound(columns) < 1 Then Continue Do

                Select Case columns(0).ToLowerInvariant()
                    Case "game"
                        gameData = line
                    Case "description"
                        descriptionData = line
                    Case "position"
                        positionData.Add(line)
                    Case "ball"
                        ballData.Add(line)
                    Case "goal"
                        goalData.Add(line)
                    Case "scoreboard"
                        scoreboard = New GameScoreboard(columns(1), columns(2),
                                                        Path.Combine(directory, Trim(columns(3).Replace(ControlChars.Quote, ""))),
                                                        ponyContext)
                    Case Else
                        Throw New InvalidDataException("Invalid line in config file: " & line)
                End Select
            Loop
        End Using

        If gameData Is Nothing OrElse positionData.Count = 0 OrElse ballData.Count = 0 Then
            Throw New InvalidDataException("Game ini file does not define Game, Position, or Ball data. It must contain all 3.")
        End If

        Dim gameColumns = CommaSplitBraceQualified(gameData)
        Name = gameColumns(1).Replace(ControlChars.Quote, "")
        _speedOverride = If(Name = "Ping Pong Pony", 267, 167)

        Dim descriptionColumns = CommaSplitQuoteQualified(descriptionData)
        If descriptionColumns.Length >= 2 Then
            Description = descriptionColumns(1)
        Else
            MessageBox.Show("Invalid description line for game: " & Name & ". Are you missing quotes around the text?",
                            "Invalid Description", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        minBalls = Integer.Parse(gameColumns(3), CultureInfo.InvariantCulture)
        maxBalls = Integer.Parse(gameColumns(4), CultureInfo.InvariantCulture)
        maxScore = Integer.Parse(gameColumns(5), CultureInfo.InvariantCulture)
        If maxScore < 1 Then
            Throw New InvalidDataException("The maximum score must be at least 1 - error loading name " & Name)
        End If
        If maxBalls < minBalls Then Throw New InvalidDataException(
            "Minimum number of balls in play is larger than the Maximum setting defined for game: " & Name)

        If MinTeams < 2 Then Throw New InvalidDataException(
            "You must have at least two teams for a game.  The minimum setting is too low for game: " & Name)
        ' Maybe later we can have them play tag or a zombie game... but for now:
        If minBalls < 1 Then Throw New InvalidDataException(
            "You must have at least one ball for the ponies to play with.  The minimum setting is too low for game: " & Name)

        Dim scoringStylesList = Split(gameColumns(6), ",")
        For Each style In scoringStylesList
            Select Case style.ToLowerInvariant().Trim()
                Case "ball_at_goal"
                    scoringStyles.Add(ScoreStyle.BallAtGoal)
                Case "ball_hits_other_team"
                    scoringStyles.Add(ScoreStyle.BallHitsOtherTeam)
                Case "ball_destroyed"
                    scoringStyles.Add(ScoreStyle.BallDestroyed)
                Case "ball_at_sides"
                    scoringStyles.Add(ScoreStyle.BallAtSides)
                Case Else
                    Throw New InvalidDataException("Invalid scoring style: " & style)
            End Select
        Next

        Dim teamNames = CommaSplitQuoteQualified(gameColumns(2))
        Dim teamNumber = 1
        For Each teamName In teamNames
            Teams.Add(New Team(teamName, teamNumber))
            teamNumber += 1
        Next

        For Each line In goalData
            Dim columns = CommaSplitBraceQualified(line)
            Dim newGoal As New GoalArea(Integer.Parse(columns(1), CultureInfo.InvariantCulture),
                                        Path.Combine(directory, Trim(columns(2).Replace(ControlChars.Quote, ""))),
                                        columns(3), ponyContext)
            goals.Add(newGoal)
        Next

        For Each goal In goals
            If goal.TeamNumber <> 0 Then
                Teams(goal.TeamNumber - 1).Goal = goal
            End If
        Next

        For Each line In positionData
            Dim columns = CommaSplitBraceQualified(line)
            Dim newPosition As New Position(columns(1), Integer.Parse(columns(2), CultureInfo.InvariantCulture), columns(3),
                                             columns(4), columns(5), columns(6), columns(7), columns(8), columns(9), columns(10),
                                             columns(11), Me)
            Teams(Integer.Parse(columns(2), CultureInfo.InvariantCulture) - 1).Positions.Add(newPosition)
        Next

        For Each line In ballData
            Dim columns = CommaSplitQuoteQualified(line)
            Dim newBall As New Ball(columns(1),
                                     Trim(columns(2)), Trim(columns(3)), Trim(columns(4)), Trim(columns(5)), Trim(columns(6)),
                                     Integer.Parse(columns(7), CultureInfo.InvariantCulture),
                                     Integer.Parse(columns(8), CultureInfo.InvariantCulture), directory & Path.DirectorySeparatorChar,
                                     ponyCollection, ponyContext)
            Balls.Add(newBall)
        Next

        Status = GameStatus.Setup
    End Sub

    Public Sub Setup()
        If Options.WindowAvoidanceEnabled OrElse Options.CursorAvoidanceEnabled Then
            Options.WindowAvoidanceEnabled = False
            Options.CursorAvoidanceEnabled = False
        End If

        For Each goal In goals
            goal.Initialize(GameScreen)
            EvilGlobals.CurrentAnimator.AddEffect(goal.HostEffect)
        Next

        scoreboard.Initialize(GameScreen)
        scoreboard.SetScores(Teams(0), Teams(1))
        EvilGlobals.CurrentAnimator.AddEffect(scoreboard.HostEffect)
        EvilGlobals.CurrentAnimator.AddSprites(scoreboard.ScoreDisplays)

        For Each team In Teams
            Dim positionsToRemove As New List(Of Position)
            For Each position In team.Positions
                If position.Player IsNot Nothing Then
                    position.Initialize(GameScreen)
                    EvilGlobals.CurrentAnimator.AddPony(position.Player)
                    allPlayers.Add(position)
                Else
                    positionsToRemove.Add(position)
                End If
            Next
            For Each entry In positionsToRemove
                team.Positions.Remove(entry)
            Next
        Next

        For Each ball In Balls
            ball.Initialize(GameScreen)
        Next

        If Options.ScaleFactor <> 1 Then
            MessageBox.Show(String.Format(CultureInfo.CurrentCulture,
                                          "Note: Games may not work properly if you use a scale factor other than 1." &
                                          " You are currently using a scale factor of {0:0.00}x.", Options.ScaleFactor))
        End If
    End Sub

    Public Sub Update(manualControlPlayerOne As Pony, manualControlPlayerTwo As Pony)
        Me.manualControlPlayerOne = manualControlPlayerOne
        Me.manualControlPlayerTwo = manualControlPlayerTwo
        Select Case Status
            Case GameStatus.Setup
                For Each ball In Balls
                    ball.LastHandledBy = Nothing
                Next
                For Each team In Teams
                    For Each position In team.Positions
                        EvilGlobals.CurrentAnimator.AllowManualControl = False
                        position.SetDestinationToStartLocation()
                        position.CurrentAction = PlayerActionType.ReturnToStart
                        position.CurrentActionGroup = Nothing
                    Next
                Next
                Status = GameStatus.SetupWaitForPositions
            Case GameStatus.SetupWaitForPositions
                For Each team In Teams
                    For Each position In team.Positions
                        If Not position.Player.AtDestination Then Exit Select
                    Next
                Next
                Status = GameStatus.SetupAddBalls
            Case GameStatus.SetupAddBalls
                For Each position In allPlayers
                    position.CurrentAction = PlayerActionType.NotSet
                Next
                For Each ball In Balls
                    activeBalls.Add(ball)
                    EvilGlobals.CurrentAnimator.AddPony(ball.Handler)
                Next
                Status = GameStatus.SetupReadyBalls
            Case GameStatus.SetupReadyBalls
                EvilGlobals.CurrentAnimator.AllowManualControl = True
                For Each ball In Balls
                    ball.Handler.SpeedOverride = Nothing
                    ball.Handler.SetBehavior(ball.Handler.Base.Behaviors(0))
                    ball.Handler.Location = ball.StartPosition
                    ball.UpdateSpeed()
                    If ball.BallType = BallType.PingPong Then
                        ball.Kick(5, Rng.NextDouble() * (2 * Math.PI), Nothing)
                    End If
                    If activeBalls.Count >= minBalls Then Exit For
                Next
                Status = GameStatus.InProgress
            Case GameStatus.InProgress
                For Each ball In activeBalls
                    ball.UpdateSpeed()
                Next
                For Each team In Teams
                    For Each position In team.Positions
                        position.DecideOnAction(Me)
                        position.PushBackOverlappingPonies(allPlayers)
                    Next
                Next
                If CheckForScore() Then
                    For Each ball In Balls
                        activeBalls.Remove(ball)
                        EvilGlobals.CurrentAnimator.RemoveSprite(ball.Handler)
                    Next
                    For Each team In Teams
                        If team.Score >= maxScore Then
                            Status = GameStatus.Completed
                            EvilGlobals.CurrentAnimator.Pause(False)
                            MessageBox.Show(team.Name & " won!", "Winner", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            EvilGlobals.CurrentAnimator.Finish(ExitRequest.ReturnToMenu)
                            Exit Sub
                        End If
                    Next
                    Status = GameStatus.Setup
                End If
            Case Else
                Throw New NotImplementedException("State not implemented: " & Status)
        End Select
    End Sub

    Private Function IsUnderManualControl(pony As Pony) As Boolean
        Return Object.ReferenceEquals(pony, manualControlPlayerOne) OrElse Object.ReferenceEquals(pony, manualControlPlayerTwo)
    End Function

    Private Function IsUnderManualControlAndActionWanted(pony As Pony) As Boolean
        Return (Object.ReferenceEquals(pony, manualControlPlayerOne) AndAlso KeyboardState.IsKeyPressed(Keys.RControlKey)) OrElse
            (Object.ReferenceEquals(pony, manualControlPlayerTwo) AndAlso KeyboardState.IsKeyPressed(Keys.LControlKey))
    End Function

    Private Function CheckForScore() As Boolean
        For Each ball In Balls
            If scoringStyles.Contains(ScoreStyle.BallAtSides) Then
                If ball.Handler.Region.Left <
                    GameScreen.WorkingArea.Left + (GameScreen.WorkingArea.Width * 0.02) Then
                    Teams(1).Score += 1
                    scoreboard.SetScores(Teams(0), Teams(1))
                    Return True
                Else
                    If ball.Handler.Region.Right >
                        (GameScreen.WorkingArea.Right) - (GameScreen.WorkingArea.Width * 0.02) Then
                        Teams(0).Score += 1
                        scoreboard.SetScores(Teams(0), Teams(1))
                        Return True
                    End If
                End If
            End If
            If scoringStyles.Contains(ScoreStyle.BallAtGoal) Then
                For Each goal In goals
                    Dim goalArea As New Rectangle(goal.HostEffect.TopLeftLocation, goal.HostEffect.CurrentImageSize)
                    If ball.Handler.IsPonyContainedInRect(Vector2.Round(ball.Handler.Location), goalArea) Then
                        For Each team In Teams
                            If ReferenceEquals(team.Goal, goal) AndAlso
                                ball.LastHandledBy IsNot Nothing AndAlso
                                Not ReferenceEquals(team, ball.LastHandledBy.Team) Then
                                For Each otherTeam In Teams
                                    If Not ReferenceEquals(otherTeam, team) Then
                                        otherTeam.Score += 1
                                        scoreboard.SetScores(Teams(0), Teams(1))
                                        Return True
                                    End If
                                Next
                            End If
                        Next
                    End If
                Next
            End If
        Next
        Return False
    End Function

    Public Sub CleanUp()
        For Each Ball In Balls
            activeBalls.Remove(Ball)
        Next

        EvilGlobals.CurrentAnimator.Clear()

        Teams(0).Score = 0
        Teams(1).Score = 0

        allPlayers.Clear()
    End Sub

    Public Class Ball
        Private ReadOnly _ballType As BallType
        Public ReadOnly Property BallType As BallType
            Get
                Return _ballType
            End Get
        End Property
        Private ReadOnly context As PonyContext
        Private ReadOnly handlerBase As PonyBase
        Private _handler As GamePony
        Public ReadOnly Property Handler As GamePony
            Get
                If _handler Is Nothing Then
                    _handler = New GamePony(context, handlerBase)
                    AddHandler _handler.Expired, Sub() _handler = Nothing
                End If
                Return _handler
            End Get
        End Property
        Private _startPosition As Vector2
        Public ReadOnly Property StartPosition As Vector2
            Get
                Return _startPosition
            End Get
        End Property

        Public Property LastHandledBy As Position

        Public Sub New(_type As String, idle_image_filename As String, slow_right_image_filename As String, slow_left_image_filename As String,
                fast_right_image_filename As String, fast_left_image_filename As String, x_location As Integer, y_location As Integer,
                files_path As String, ponyCollection As PonyCollection, ponyContext As PonyContext)
            context = Argument.EnsureNotNull(ponyContext, "ponyContext")

            ' We need to duplicate the new pony as only "duplicates" are fully loaded. A new pony by itself is considered a template.
            handlerBase = PonyBase.CreateInMemory(ponyCollection)
            handlerBase.DisplayName = "Ball"

            Dim idleBehavior = New Behavior(handlerBase) With {
                .Name = "idle",
                .MinDuration = 600,
                .MaxDuration = 600,
                .Speed = 0,
                .AllowedMovement = AllowedMoves.None}
            Dim idleImagePath = files_path & Replace(idle_image_filename, ControlChars.Quote, "")
            idleBehavior.LeftImage.Path = idleImagePath
            idleBehavior.RightImage.Path = idleImagePath
            handlerBase.Behaviors.Add(idleBehavior)

            Dim slowBehavior = New Behavior(handlerBase) With {
                .Name = "slow",
                .MinDuration = 600,
                .MaxDuration = 600,
                .Speed = 3,
                .AllowedMovement = AllowedMoves.All}
            slowBehavior.LeftImage.Path = files_path & Replace(slow_left_image_filename, ControlChars.Quote, "")
            slowBehavior.RightImage.Path = files_path & Replace(slow_right_image_filename, ControlChars.Quote, "")
            handlerBase.Behaviors.Add(slowBehavior)

            Dim fastBehavior = New Behavior(handlerBase) With {
                .Name = "fast",
                .MinDuration = 600,
                .MaxDuration = 600,
                .Speed = 5,
                .AllowedMovement = AllowedMoves.All}
            Dim fastImagePath = files_path & Replace(idle_image_filename, ControlChars.Quote, "")
            fastBehavior.LeftImage.Path = files_path & Replace(fast_left_image_filename, ControlChars.Quote, "")
            fastBehavior.RightImage.Path = files_path & Replace(fast_right_image_filename, ControlChars.Quote, "")
            handlerBase.Behaviors.Add(fastBehavior)

            _startPosition = New Vector2(x_location, y_location)

            Select Case LCase(Trim(_type))
                Case "soccer"
                    _ballType = BallType.Soccer
                Case "pingpong"
                    _ballType = BallType.PingPong
                Case Else
                    Throw New ArgumentException("Invalid ball type: " & _type, _type)
            End Select
        End Sub

        Public Sub Initialize(gameScreen As Screen)
            Argument.EnsureNotNull(gameScreen, "gameScreen")

            _startPosition = New Vector2(
                CInt(StartPosition.X * 0.01 * gameScreen.WorkingArea.Width + gameScreen.WorkingArea.X),
                CInt(StartPosition.Y * 0.01 * gameScreen.WorkingArea.Height + gameScreen.WorkingArea.Y))

            Handler.Location = StartPosition
        End Sub

        Public Sub UpdateSpeed()
            Dim currentHandlerSpeed As Double = If(Handler.SpeedOverride, Handler.CurrentBehavior.SpeedInPixelsPerSecond)
            If BallType = BallType.Soccer Then
                currentHandlerSpeed *= 0.98
                Handler.SpeedOverride = currentHandlerSpeed
            End If
            Dim behaviorIndex As Integer
            Select Case currentHandlerSpeed
                Case Is < 1
                    behaviorIndex = 0
                Case Is < 100
                    behaviorIndex = 1
                Case Else
                    behaviorIndex = 2
            End Select
            Dim newBehavior = Handler.Base.Behaviors(behaviorIndex)
            If Not Object.ReferenceEquals(newBehavior, Handler.CurrentBehavior) Then
                Handler.SetBehavior(newBehavior)
                Handler.MovementOverride = Handler.Movement
            End If
        End Sub

        Public Sub Kick(_speed As Double, _angle As Double, kicker As Position)
            LastHandledBy = kicker
            Handler.SpeedOverride = _speed * (1000.0 / 30.0)
            Handler.MovementOverride = New Vector2F(CSng(Math.Cos(_angle)), CSng(Math.Sin(_angle)))
        End Sub
    End Class

    Public Class Team
        Public Property Name As String
        Public Property Number As Integer
        Private ReadOnly _positions As New List(Of Position)()
        Public ReadOnly Property Positions As List(Of Position)
            Get
                Return _positions
            End Get
        End Property
        Public Property Score As Integer
        Public Property Goal As GoalArea

        Public Sub New(_name As String, _number As Integer)
            Name = _name
            Number = _number
        End Sub

    End Class

    Public Class GoalArea
        Public Property HostEffect As Effect
        Public Property TeamNumber As Integer ' 0 = a score any team
        Private startPoint As Point

        Public Sub New(_teamNumber As Integer, imageFilename As String, location As String, context As PonyContext)
            If Not File.Exists(imageFilename) Then
                Throw New FileNotFoundException("File does not exist: " & imageFilename)
            End If

            TeamNumber = _teamNumber
            Dim base As New EffectBase("Team " & TeamNumber & "'s Goal", imageFilename, imageFilename)
            HostEffect = New Effect(base, context)

            Dim locationParts = Split(location, ",")
            startPoint = New Point(
                Integer.Parse(locationParts(0), CultureInfo.InvariantCulture),
                Integer.Parse(locationParts(1), CultureInfo.InvariantCulture))
        End Sub

        Public Sub Initialize(gameScreen As Screen)
            Argument.EnsureNotNull(gameScreen, "gameScreen")

            HostEffect.TopLeftLocation = New Point(
                CInt(startPoint.X * 0.01 * gameScreen.WorkingArea.Width + gameScreen.WorkingArea.X),
                CInt(startPoint.Y * 0.01 * gameScreen.WorkingArea.Height + gameScreen.WorkingArea.Y))
        End Sub

        Public Function Center() As PointF
            Return HostEffect.Region.Center()
        End Function
    End Class

    Public Class GameScoreboard
        Public Property HostEffect As Effect
        Private startPoint As Point

        Private teamOneNameDisplay As New ScoreDisplay(Me) With {.LocalPosition = New Vector2F(66, 107)}
        Private teamTwoNameDisplay As New ScoreDisplay(Me) With {.LocalPosition = New Vector2F(66, 150)}
        Private teamOneScoreDisplay As New ScoreDisplay(Me) With {.LocalPosition = New Vector2F(130, 113)}
        Private teamTwoScoreDisplay As New ScoreDisplay(Me) With {.LocalPosition = New Vector2F(135, 156)}

        Public ReadOnly Iterator Property ScoreDisplays As IEnumerable(Of ScoreDisplay)
            Get
                Yield teamOneNameDisplay
                Yield teamTwoNameDisplay
                Yield teamOneScoreDisplay
                Yield teamTwoScoreDisplay
            End Get
        End Property

        Public Sub New(x As String, y As String, imageFilename As String, context As PonyContext)
            If Not File.Exists(imageFilename) Then
                Throw New FileNotFoundException("File does not exist: " & imageFilename)
            End If

            Dim base As New EffectBase("Scoreboard", imageFilename, imageFilename)
            HostEffect = New Effect(base, context)

            startPoint = New Point(
                Integer.Parse(x, CultureInfo.InvariantCulture),
                Integer.Parse(y, CultureInfo.InvariantCulture))
        End Sub

        Public Sub Initialize(gameScreen As Screen)
            Argument.EnsureNotNull(gameScreen, "gameScreen")

            HostEffect.TopLeftLocation = New Point(
                CInt(startPoint.X * 0.01 * gameScreen.WorkingArea.Width + gameScreen.WorkingArea.X),
                CInt(startPoint.Y * 0.01 * gameScreen.WorkingArea.Height + gameScreen.WorkingArea.Y))
        End Sub

        Public Sub SetScores(teamOne As Team, teamTwo As Team)
            Argument.EnsureNotNull(teamOne, "teamOne")
            Argument.EnsureNotNull(teamTwo, "teamTwo")
            teamOneNameDisplay.Text = teamOne.Name
            teamOneScoreDisplay.Text = teamOne.Score.ToString(CultureInfo.CurrentCulture)
            teamTwoNameDisplay.Text = teamTwo.Name
            teamTwoScoreDisplay.Text = teamTwo.Score.ToString(CultureInfo.CurrentCulture)
        End Sub

        Public Class ScoreDisplay
            Implements ISpeakingSprite

            Public Property Text As String
            Private parent As GameScoreboard
            Public Property LocalPosition As Vector2F

            Public Sub New(parentBoard As GameScoreboard)
                parent = parentBoard
            End Sub

            Public ReadOnly Property SpeechText As String Implements ISpeakingSprite.SpeechText
                Get
                    Return Text
                End Get
            End Property

            Public ReadOnly Property ImageTimeIndex As TimeSpan Implements ISprite.ImageTimeIndex
                Get
                    Return TimeSpan.Zero
                End Get
            End Property

            Public ReadOnly Property Region As Rectangle Implements ISprite.Region
                Get
                    Return New Rectangle(Vector2.Round(New Vector2(parent.HostEffect.TopLeftLocation) +
                                                       LocalPosition * parent.HostEffect.Context.ScaleFactor), Size.Empty)
                End Get
            End Property

            Public Sub Start(startTime As TimeSpan) Implements ISprite.Start

            End Sub

            Public Sub Update(updateTime As TimeSpan) Implements ISprite.Update

            End Sub

            Public ReadOnly Property FacingRight As Boolean Implements ISprite.FacingRight
                Get
                    Return True
                End Get
            End Property

            Public ReadOnly Property ImagePaths As SpriteImagePaths Implements ISprite.ImagePaths
                Get
                    Return parent.HostEffect.ImagePaths
                End Get
            End Property
        End Class
    End Class

    Public Class Position
        Public Property Name As String
        Public Property TeamNumber As Integer
        Public Property Team As Team
        Private _player As GamePony
        Public Property Player As GamePony
            Get
                Return _player
            End Get
            Set(value As GamePony)
                If _player IsNot Nothing Then _player.CurrentPosition = Nothing
                _player = value
                If _player IsNot Nothing Then _player.CurrentPosition = Me
            End Set
        End Property
        Private allowedArea As Rectangle
        Private startLocation As Point
        Public Property CurrentAction As PlayerActionType
        Public Property CurrentActionGroup As List(Of PlayerActionType)
        Private ReadOnly game As Game

        Public Property Required As Boolean

        Private areaPoints As String()

        Private lastKickTime As DateTime = DateTime.MinValue

        Private nearestBallDistance As Double

        Public Property SelectionMenuPictureBox As PictureBox

        Private ReadOnly haveBallActions As New List(Of PlayerActionType)()
        Private ReadOnly hostileBallActions As New List(Of PlayerActionType)()
        Private ReadOnly friendlyBallActions As New List(Of PlayerActionType)()
        Private ReadOnly neutralBallActions As New List(Of PlayerActionType)()
        Private ReadOnly distantBallActions As New List(Of PlayerActionType)()
        Private ReadOnly noBallActions As New List(Of PlayerActionType)()

        Public Sub New(_Name As String, _team_number As Integer, _start_location As String, _Allowed_area As String,
                _Have_Ball_Actions As String, _Hostile_Ball_Actions As String, _Friendly_Ball_Actions As String, _
                _Neutral_Ball_Actions As String, _Distance_Ball_Actions As String, _No_Ball_Actions As String, _required As String,
                game As Game)
            Argument.EnsureNotNull(_Name, "_Name")
            Me.game = Argument.EnsureNotNull(game, "game")

            Name = Trim(_Name.Replace(ControlChars.Quote, ""))
            TeamNumber = _team_number

            Select Case LCase(Trim(_required))
                Case "required"
                    Required = True
                Case "optional"
                    Required = False
                Case Else
                    Throw New ArgumentException("Invalid entry for required/optional setting of position " & Name & ". ")
            End Select

            Dim start_points = Split(_start_location, ",")
            startLocation = New Point(
                Integer.Parse(start_points(0), CultureInfo.InvariantCulture),
                Integer.Parse(start_points(1), CultureInfo.InvariantCulture))

            If LCase(Trim(_Allowed_area)) <> "any" Then
                areaPoints = Split(_Allowed_area, ",")
            Else
                allowedArea = Nothing
            End If

            Dim Action_Lists As New List(Of List(Of PlayerActionType))
            Action_Lists.Add(haveBallActions)
            Action_Lists.Add(hostileBallActions)
            Action_Lists.Add(friendlyBallActions)
            Action_Lists.Add(neutralBallActions)
            Action_Lists.Add(distantBallActions)
            Action_Lists.Add(noBallActions)

            Dim Actions_strings As New List(Of String)
            Actions_strings.Add(_Have_Ball_Actions)
            Actions_strings.Add(_Hostile_Ball_Actions)
            Actions_strings.Add(_Friendly_Ball_Actions)
            Actions_strings.Add(_Neutral_Ball_Actions)
            Actions_strings.Add(_Distance_Ball_Actions)
            Actions_strings.Add(_No_Ball_Actions)

            For i = 0 To Action_Lists.Count - 1
                Dim list As List(Of PlayerActionType) = Action_Lists(i)
                Dim action_string As String = Actions_strings(i)

                Dim actions = Split(action_string, ",")
                For Each action In actions
                    list.Add(CType([Enum].Parse(GetType(PlayerActionType), action), PlayerActionType))
                Next
            Next
        End Sub

        Public Sub Initialize(gameScreen As Screen)
            Argument.EnsureNotNull(gameScreen, "gameScreen")
            If Not IsNothing(areaPoints) Then
                allowedArea = New Rectangle(
                    CInt(Double.Parse(areaPoints(0), CultureInfo.InvariantCulture) * 0.01 * gameScreen.WorkingArea.Width + gameScreen.WorkingArea.X),
                    CInt(Double.Parse(areaPoints(1), CultureInfo.InvariantCulture) * 0.01 * gameScreen.WorkingArea.Height + gameScreen.WorkingArea.Y),
                    CInt(Double.Parse(areaPoints(2), CultureInfo.InvariantCulture) * 0.01 * gameScreen.WorkingArea.Width),
                    CInt(Double.Parse(areaPoints(3), CultureInfo.InvariantCulture) * 0.01 * gameScreen.WorkingArea.Height))
            Else
                allowedArea = gameScreen.WorkingArea
            End If
            startLocation = New Point(
                CInt(startLocation.X * 0.01 * allowedArea.Width + allowedArea.X),
                CInt(startLocation.Y * 0.01 * allowedArea.Height + allowedArea.Y))
            Player.SpeedOverride = If(game.Name = "Ping Pong Pony", 267, 167)
        End Sub

        Public Sub DecideOnAction(game As Game)
            Argument.EnsureNotNull(game, "game")
            Dim nearestBall = GetNearestBall(game.activeBalls)
            Dim screenDiagonal = Math.Sqrt((game.GameScreen.WorkingArea.Height) ^ 2 + (game.GameScreen.WorkingArea.Width) ^ 2)
            If nearestBallDistance > screenDiagonal / 2 Then
                PerformAction(distantBallActions, nearestBall)
                Exit Sub
            End If
            If nearestBallDistance <= (Player.Region.Width / 2) + 50 Then
                PerformAction(haveBallActions, nearestBall)
                Exit Sub
            End If

            Dim ballLastHandledByTeam = If(nearestBall.LastHandledBy IsNot Nothing, nearestBall.LastHandledBy.Team, Nothing)
            If ballLastHandledByTeam Is Nothing Then
                PerformAction(neutralBallActions, nearestBall)
            ElseIf ballLastHandledByTeam.Number = TeamNumber Then
                PerformAction(friendlyBallActions, nearestBall)
            Else
                PerformAction(hostileBallActions, nearestBall)
            End If
        End Sub

        Private Sub PerformAction(actionList As List(Of PlayerActionType), ball As Ball)
            If CurrentActionGroup IsNot Nothing Then
                If ReferenceEquals(actionList, CurrentActionGroup) Then
                    'we are already doing an action from this list

                    'if we recently kicked the ball, don't do it for 2 seconds.
                    If ReferenceEquals(CurrentActionGroup, haveBallActions) Then
                        If DateDiff(DateInterval.Second, lastKickTime, DateTime.UtcNow) <= 2 Then
                            If DateDiff(DateInterval.Second, lastKickTime, DateTime.UtcNow) > 1 AndAlso
                                game.IsUnderManualControl(Player) Then
                                Speak("Can't kick again so soon!")
                            End If

                            Exit Sub
                        End If
                    Else
                        'if it was any other action, don't reset it.
                        Exit Sub
                    End If
                Else
                    CurrentActionGroup = Nothing
                End If
            End If

            Dim selectedAction = actionList.RandomElement()
            Select Case selectedAction
                Case PlayerActionType.NotSet
                    Throw New Exception("Can't do this action (reserved): " & selectedAction)
                Case PlayerActionType.ReturnToStart
                    SetDestinationToStartLocation()
                Case PlayerActionType.ChaseBall
                    SetFollowBehavior(ball.Handler)
                Case PlayerActionType.LeadBall
                    SetFollowBehavior(ball.Handler)
                Case PlayerActionType.AvoidBall
                    Throw New NotImplementedException("Not implemented yet: action type " & selectedAction)
                Case PlayerActionType.ThrowBallToGoal
                    If DateDiff(DateInterval.Second, lastKickTime, DateTime.UtcNow) <= 2 Then
                        'can't kick again so soon
                        Exit Sub
                    End If
                    'If ponies are being controlled, don't kick unless the action key (control - left or right) is being pushed
                    If game.IsUnderManualControl(Player) AndAlso
                        Not game.IsUnderManualControlAndActionWanted(Player) Then Exit Sub
                    KickBall(ball, 10, GetOtherTeamGoal(), Nothing, Me, "*Kick*!")
                    lastKickTime = DateTime.UtcNow
                Case PlayerActionType.ThrowBallToTeammate
                    If DateDiff(DateInterval.Second, lastKickTime, DateTime.UtcNow) <= 2 Then
                        'can't kick again so soon
                        Exit Sub
                    End If

                    'don't pass if we are under control - kick instead.
                    If game.IsUnderManualControl(Player) Then Exit Sub

                    lastKickTime = DateTime.UtcNow
                    Dim openTeammate = GetOpenTeammate()
                    If openTeammate Is Nothing Then
                        'no teammates to pass to, kick to goal instead, unless a player controlled pony.
                        If game.IsUnderManualControl(Player) Then Exit Sub
                        KickBall(ball, 10, GetOtherTeamGoal(), Nothing, Me, "*Kick*!")
                        Exit Sub
                    End If

                    KickBall(ball, 10, Nothing, openTeammate, Me, "*Pass*!")
                Case PlayerActionType.ThrowBallReflect
                    If DateDiff(DateInterval.Second, lastKickTime, DateTime.UtcNow) <= 2 Then
                        'can't kick again so soon
                        Exit Sub
                    End If

                    If game.IsUnderManualControl(Player) AndAlso
                        Not game.IsUnderManualControlAndActionWanted(Player) Then Exit Sub

                    BounceBall(ball, 7, Me, "*Ping*!")
                    lastKickTime = DateTime.UtcNow
                Case PlayerActionType.ApproachOwnGoal
                    SetDestination(GetTeamGoal().HostEffect.Center())
                Case PlayerActionType.ApproachTargetGoal
                    SetDestination(GetOtherTeamGoal().HostEffect.Center())
                Case PlayerActionType.Idle
                    If CurrentAction = PlayerActionType.Idle Then Exit Sub
                    Player.FollowTargetOverride = Nothing
                    If game.IsUnderManualControl(Player) Then Exit Sub
                    Console.WriteLine(Player.Base.Directory & " Calling SetBehavior from Idle action " & DateTime.UtcNow)
                    Player.SetBehavior(Nothing, False)
                Case Else
                    EvilGlobals.CurrentAnimator.Pause(False)
                    Throw New System.ComponentModel.InvalidEnumArgumentException("Invalid action type: " & selectedAction)
            End Select

            CurrentAction = selectedAction
            CurrentActionGroup = actionList
        End Sub

        Private Function GetNearestBall(balls As List(Of Ball)) As Ball
            Argument.EnsureNotNullOrEmpty(balls, "balls")
            Dim nearestBallDistanceSquared = Single.MaxValue
            Dim nearestBall As Ball = Nothing
            For Each ball In balls
                Dim distanceSquared = Vector2F.DistanceSquared(Player.Location, ball.Handler.Location)
                If distanceSquared < nearestBallDistanceSquared Then
                    nearestBallDistanceSquared = distanceSquared
                    nearestBall = ball
                End If
            Next
            nearestBallDistance = Math.Sqrt(nearestBallDistanceSquared)
            Return nearestBall
        End Function

        Private Function GetOtherTeamGoal() As GoalArea

            For Each goal In game.goals
                If goal.TeamNumber <> TeamNumber Then
                    Return goal
                End If
            Next

            Throw New Exception("Couldn't find a goal for another team.")

        End Function

        Private Function GetTeamGoal() As GoalArea

            For Each goal In game.goals
                If goal.TeamNumber = TeamNumber Then
                    Return goal
                End If
            Next

            Throw New Exception("Couldn't find a goal for pony's team.")

        End Function

        Public Sub SetDestinationToStartLocation()
            SetDestination(startLocation)
        End Sub

        Public Sub SetDestination(destination As Point)
            Player.SpeedOverride = game.SpeedOverride
            Player.DestinationOverride = New Vector2(destination)
            Player.FollowTargetOverride = Nothing
        End Sub

        Public Sub SetFollowBehavior(target As GamePony)
            Player.DestinationOverride = Nothing
            Player.FollowTargetOverride = target
        End Sub

        Private Sub KickBall(ball As Ball, speed As Double, targetGoal As GoalArea, targetPony As Pony,
                              kicker As Position, line As String)
            If Rng.NextDouble() < 0.05 Then
                Speak("Missed!")
                Exit Sub
            End If

            Speak(line)

            Dim angle As Double = Nothing

            If IsNothing(targetGoal) Then
                angle = GetAngleToObject(targetPony.Location)
            Else
                angle = GetAngleToObject(targetGoal.Center())
            End If

            'add a bit of inaccuracy
            If Rng.NextDouble() < 0.5 Then
                angle += Rng.NextDouble() * (Math.PI / 8)
            Else
                angle -= Rng.NextDouble() * (Math.PI / 8)
            End If

            ball.Kick(speed, angle, kicker)
        End Sub

        Private Sub BounceBall(ball As Ball, speed As Double, kicker As Position, line As String)
            If game.Name = "Ping Pong Pony" Then
                'avoid bouncing the ball back into our own goal.
                If Not IsNothing(ball.LastHandledBy) AndAlso ReferenceEquals(ball.LastHandledBy, Me) Then
                    Exit Sub
                End If
            End If

            Speak(line)

            Dim angle As Double
            Dim gamescreen = game.GameScreen

            If ball.Handler.Movement.X >= 0 Then
                'ball is going to the right, it will 'bounce' to the left.
                angle = Math.PI
            Else
                'ball is going to the left, and will 'bounce' right.
                angle = 0
            End If

            Dim ball_center As Point = Point.Round(ball.Handler.Location)
            Dim kicker_center As Point = Point.Round(kicker.Player.Location)

            Dim y_difference = kicker_center.Y - ball_center.Y

            Dim kicker_height = kicker.Player.Region.Height / 1.5

            If kicker_center.X < (gamescreen.WorkingArea.Width * 0.5) Then
                If y_difference > 0 Then
                    angle += (Math.PI / 4) * (Math.Abs(y_difference) / kicker_height)
                Else
                    angle -= (Math.PI / 4) * (Math.Abs(y_difference) / kicker_height)
                End If

            Else
                If y_difference > 0 Then
                    angle -= (Math.PI / 4) * (Math.Abs(y_difference) / kicker_height)
                Else
                    angle += (Math.PI / 4) * (Math.Abs(y_difference) / kicker_height)
                End If
            End If

            'add a bit of inaccuracy
            If Rng.NextDouble() < 0.5 Then
                angle += Rng.NextDouble() * (Math.PI / 8)
            Else
                angle -= Rng.NextDouble() * (Math.PI / 8)
            End If

            'avoid high angels
            If angle >= Math.PI / 2 AndAlso angle < Math.PI * (2 / 3) Then
                angle = Math.PI * (2 / 3)
            End If
            If angle <= Math.PI / 2 AndAlso angle > Math.PI * (1 / 3) Then
                angle = Math.PI / 3
            End If
            If angle <= Math.PI * (3 / 2) AndAlso angle > Math.PI * (4 / 3) Then
                angle = Math.PI * (4 / 3)
            End If
            If angle >= Math.PI * (3 / 2) AndAlso angle < Math.PI * (5 / 3) Then
                angle = Math.PI * (5 / 3)
            End If

            ball.Kick(speed, angle, kicker)

        End Sub

        ''' <summary>
        ''' Calculates the angle to the target in radians.
        ''' </summary>
        ''' <param name="target">The target location.</param>
        ''' <returns>The angle to the target in radians.</returns>
        Private Function GetAngleToObject(target As Vector2F) As Double
            Dim a = Player.Location
            Dim b = target
            Return Math.Atan2(b.Y - a.Y, b.X - a.X)
        End Function

        Private Sub Speak(line As String)
            Player.Speak(New Speech() With {.Name = line, .Text = line})
        End Sub

        Public Sub PushBackOverlappingPonies(allPositions As List(Of Position))
            Argument.EnsureNotNull(allPositions, "allPositions")

            For Each otherposition As Position In allPositions
                Dim otherpony = otherposition.Player
                If ReferenceEquals(Me.Player, otherpony) Then Continue For

                ' Push overlapping ponies a bit apart.
                PonyPush(Me.Player, otherpony)
            Next
        End Sub

        Private Sub PonyPush(pony1 As GamePony, pony2 As GamePony)
            Dim region1 = pony1.Region
            Dim region2 = pony2.Region

            If Not region1.IntersectsWith(region2) Then Return
            
            Dim leftDistance = region1.Right - region2.Left
            Dim rightDistance = region2.Right - region1.Left
            Dim topDistance = region1.Bottom - region2.Top
            Dim bottomDistance = region2.Bottom - region1.Top

            Dim minDistance = leftDistance
            If rightDistance < minDistance Then minDistance = rightDistance
            If topDistance < minDistance Then minDistance = topDistance
            If bottomDistance < minDistance Then minDistance = bottomDistance

            Dim change As New Vector2F
            Dim pushDistance = Math.Min(2.0F, minDistance)
            If leftDistance = minDistance Then
                change.X -= pushDistance
            ElseIf rightDistance = minDistance Then
                change.X += pushDistance
            ElseIf topDistance = minDistance Then
                change.Y -= pushDistance
            ElseIf bottomDistance = minDistance Then
                change.Y += pushDistance
            End If

            Dim newLocation = pony1.Location + change
            If pony1.IsPonyContainedInRect(newLocation, allowedArea) Then pony1.Location = newLocation
        End Sub

        ''' <summary>
        ''' Gets a teammate that is not near any enemy players and is closer to the goal than we are.
        ''' </summary>
        ''' <returns>A teammate that is not near any enemy players and is closer to the goal than we are.</returns>
        Private Function GetOpenTeammate() As Pony
            Dim goal = GetOtherTeamGoal()
            Dim distanceToGoalSquared = Vector2F.DistanceSquared(Player.Location, goal.Center())
            Dim openTeammates As List(Of Pony) = Nothing
            For Each friendlyPosition In Team.Positions
                If ReferenceEquals(friendlyPosition, Me) Then Continue For

                Dim open = True
                For Each enemyPosition In game.allPlayers
                    If enemyPosition.Team.Name = Team.Name Then Continue For
                    Dim distanceSquared = Vector2F.DistanceSquared(friendlyPosition.Player.Location, enemyPosition.Player.Location)
                    If distanceSquared <= 200 ^ 2 Then
                        open = False
                        Exit For
                    End If
                Next

                If open Then
                    Dim friendlyDistanceToGoalSquared = Vector2F.DistanceSquared(friendlyPosition.Player.Location, goal.Center())
                    If friendlyDistanceToGoalSquared <= distanceToGoalSquared Then
                        If openTeammates Is Nothing Then openTeammates = New List(Of Pony)()
                        openTeammates.Add(friendlyPosition.Player)
                    End If
                End If
            Next
            If openTeammates Is Nothing OrElse openTeammates.Count = 0 Then Return Nothing
            Return openTeammates.RandomElement()
        End Function
    End Class

    Public Class GamePony
        Inherits Pony
        Public Property CurrentPosition As Position
        Public Sub New(context As PonyContext, base As PonyBase)
            MyBase.New(context, base)
        End Sub
        Public Function IsPonyContainedInRect(centerLocation As Vector2F, rect As RectangleF) As Boolean
            Dim sz = Region.Size
            Return rect.Contains(New RectangleF(centerLocation - (New Vector2F(sz.Width, sz.Height) / 2), sz))
        End Function
    End Class

    Public Class GameAnimator
        Inherits DesktopPonyAnimator
        Private ReadOnly game As Game
        Public Sub New(spriteViewer As ISpriteCollectionView, spriteCollection As IEnumerable(Of ISprite),
                       ponyCollection As PonyCollection, ponyContext As PonyContext, game As Game)
            MyBase.New(spriteViewer, spriteCollection, ponyCollection, ponyContext)
            Me.game = Argument.EnsureNotNull(game, "game")
        End Sub
        Private ReadOnly _zOrderer As Comparison(Of ISprite) = Function(a, b)
                                                                   Dim aIsDisplay = TypeOf a Is GameScoreboard.ScoreDisplay
                                                                   Dim bIsDisplay = TypeOf b Is GameScoreboard.ScoreDisplay
                                                                   If aIsDisplay Xor bIsDisplay Then Return If(aIsDisplay, 1, -1)
                                                                   Return MyBase.ZOrderer(a, b)
                                                               End Function
        Protected Overrides ReadOnly Property ZOrderer As Comparison(Of ISprite)
            Get
                Return _zOrderer
            End Get
        End Property
        Protected Overrides Sub SynchronizeContext()
            PonyContext.SynchronizeWithGlobalOptionsWithAvoidanceOverrides()
            PonyContext.Region = game.GameScreen.WorkingArea
            game.Update(ManualControlPlayerOne, ManualControlPlayerTwo)
        End Sub
    End Class
End Class
