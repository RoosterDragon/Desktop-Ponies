Imports System.Globalization
Imports System.IO
Imports DesktopSprites.SpriteManagement

Public Class Game
    Public Const RootDirectory = "Games"
    Public Const ConfigFilename = "game.ini"

    Public Shared Property CurrentGame As Game

    Public Enum BallType
        Soccer = 0 'Is pushed around and slows to a stop.  No Gravity
        Baseball = 1 'Is thrown and arcs, then slows if not caught.
        PingPong = 2 'Bounces around and doesn't stop/slow
        Dodge = 3 'Travels in a straight line and is destroyed when impacting barriers
    End Enum

    Public Enum GameStatus
        Setup = 0
        Ready = 1
        InProgress = 2
        'RoundFinishing = 3
        'NextRoundSetup = 4
        'Finishing = 5
        Completed = 6
    End Enum

    Private Enum ScoreStyle
        BallAtGoal = 0
        BallHitsOtherTeam = 1
        BallDestroyed = 2
        BallAtSides = 3
    End Enum

    Public Enum PlayerActionType
        NotSet = 0
        ReturnToStart = 1
        ChaseBall = 2
        AvoidBall = 3
        ThrowBallToGoal = 4
        ThrowBallToTeammate = 5
        ThrowBallAtTarget = 6
        ThrowBallReflect = 7 'ball bounces off
        ApproachOwnGoal = 8
        ApproachTargetGoal = 9
        LeadBall = 10
        ApproachTarget = 12
        Idle = 13
    End Enum

    Public Property Name As String = ""
    Public Property Description As String = ""

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

    Public Sub New(collection As PonyCollection, directory As String)
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
                                                        Path.Combine(directory, Trim(columns(3).Replace(ControlChars.Quote, ""))))
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
                                        columns(3))
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
                                             columns(11))
            Teams(Integer.Parse(columns(2), CultureInfo.InvariantCulture) - 1).Positions.Add(newPosition)
        Next

        For Each line In ballData
            Dim columns = CommaSplitQuoteQualified(line)
            Dim newBall As New Ball(columns(1),
                                     Trim(columns(2)), Trim(columns(3)), Trim(columns(4)), Trim(columns(5)), Trim(columns(6)),
                                     Integer.Parse(columns(7), CultureInfo.InvariantCulture),
                                     Integer.Parse(columns(8), CultureInfo.InvariantCulture), directory & Path.DirectorySeparatorChar,
                                     collection)
            Balls.Add(newBall)
        Next

        Status = GameStatus.Setup
    End Sub

    Public Sub CleanUp()
        For Each Ball In Balls
            'Ball.Handler.Close()
            activeBalls.Remove(Ball)
        Next

        Pony.CurrentAnimator.Clear()

        'For Each goal In Goals
        '    goal.Visible = False
        'Next

        'ScoreBoard.Visible = False

        Teams(0).Score = 0
        Teams(1).Score = 0

        'For Each position In AllPlayers
        '    position.Player.Close()
        'Next

        allPlayers.Clear()
    End Sub

    Public Sub Setup()
        If Options.WindowAvoidanceEnabled OrElse Options.CursorAvoidanceEnabled Then
            Options.WindowAvoidanceEnabled = False
            Options.CursorAvoidanceEnabled = False
        End If

        For Each goal In goals
            goal.Initialize(GameScreen)
            goal.HostEffect.DesiredDuration = 60 * 60 * 24 * 365
            Pony.CurrentAnimator.AddEffect(goal.HostEffect)
        Next

        scoreboard.Initialize(GameScreen)
        scoreboard.SetScores(Teams(0), Teams(1))
        scoreboard.HostEffect.DesiredDuration = 60 * 60 * 24 * 365
        Pony.CurrentAnimator.AddEffect(scoreboard.HostEffect)
        Pony.CurrentAnimator.AddSprites(scoreboard.ScoreDisplays)

        For Each team In Teams
            Dim positionsToRemove As New List(Of Position)
            For Each position In team.Positions
                position.Initialize(GameScreen)
                If position.Player IsNot Nothing Then
                    position.Player.PlayingGame = True
                    Pony.CurrentAnimator.AddPony(position.Player)
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

        Options.Screens.Clear()
        Options.Screens.Add(GameScreen)

        If Options.ScaleFactor <> 1 Then
            MessageBox.Show(String.Format(CultureInfo.CurrentCulture,
                                          "Note: Games may not work properly if you use a scale factor other than 1." &
                                          " You are currently using a scale factor of {0:0.00}x.", Options.ScaleFactor))
        End If
    End Sub

    Public Sub Update()
        Select Case Status
            Case GameStatus.Setup
                Dim allInPosition = True
                For Each team In Teams
                    For Each position In team.Positions
                        If position.CurrentAction <> PlayerActionType.ReturnToStart OrElse
                            position.Player.CurrentBehavior.AllowedMovement = AllowedMoves.None Then
                            ' Go to starting position.
                            position.SetFollowBehavior(Nothing, Nothing, True)
                        End If
                        If Not position.Player.AtDestination Then allInPosition = False
                    Next
                Next
                If allInPosition Then Status = GameStatus.Ready
            Case GameStatus.Ready
                For Each position In allPlayers
                    position.CurrentAction = PlayerActionType.NotSet
                    position.CurrentActionGroup = Nothing
                Next

                For Each ball In Balls
                    activeBalls.Add(ball)
                    Pony.CurrentAnimator.AddPony(ball.Handler)
                    ball.Handler.CurrentBehavior = ball.Handler.Behaviors(1)
                    ball.Handler.TopLeftLocation = ball.StartPosition
                    ball.Update()
                    If ball.Type = BallType.PingPong Then
                        ball.Kick(5, Rng.NextDouble() * (2 * Math.PI), Nothing)
                    End If
                    If activeBalls.Count >= minBalls Then Exit For
                Next

                Status = GameStatus.InProgress
            Case GameStatus.InProgress
                For Each team In Teams
                    For Each position In team.Positions
                        position.DecideOnAction(Game.CurrentGame)
                        position.PushBackOverlappingPonies(allPlayers)
                        'Position.Player.Update(Pony.CurrentAnimator.ElapsedTime)
                    Next
                Next

                For Each ball In activeBalls
                    ball.Update()
                Next

                If CheckForScore() Then
                    For Each ball In Balls
                        activeBalls.Remove(ball)
                        Pony.CurrentAnimator.RemovePonyAndReinitializeInteractions(ball.Handler)
                    Next

                    For Each team In Teams
                        If team.Score >= maxScore Then
                            Status = GameStatus.Completed
                            Pony.CurrentAnimator.Pause(False)
                            MessageBox.Show(team.Name & " won!", "Winner", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Main.Instance.PonyShutdown()
                            Main.Instance.SmartInvoke(Sub() Main.Instance.Visible = True)
                            Exit Sub
                        End If
                    Next
                    Status = GameStatus.Setup
                End If
            Case Else
                Throw New NotImplementedException("State not implemented: " & Status)
        End Select
    End Sub

    Private Function CheckForScore() As Boolean
        For Each ball In Balls
            If scoringStyles.Contains(ScoreStyle.BallAtSides) Then
                If ball.Handler.TopLeftLocation.X <
                    GameScreen.WorkingArea.X + (GameScreen.WorkingArea.Width * 0.02) Then
                    Teams(1).Score += 1
                    scoreboard.SetScores(Teams(0), Teams(1))
                    Return True
                Else
                    If ball.Handler.TopLeftLocation.X + ball.Handler.CurrentImageSize.X >
                        (GameScreen.WorkingArea.X + GameScreen.WorkingArea.Width) - (GameScreen.WorkingArea.Width * 0.02) Then
                        Teams(0).Score += 1
                        scoreboard.SetScores(Teams(0), Teams(1))
                        Return True
                    End If
                End If
            End If
            If scoringStyles.Contains(ScoreStyle.BallAtGoal) Then
                For Each goal In goals
                    Dim goalArea As New Rectangle(goal.HostEffect.Location, goal.HostEffect.CurrentImageSize)
                    If Pony.IsPonyInBox(ball.Handler.CenterLocation, goalArea) Then
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

    Private Shared Function GetBallLastHandlerTeam(ball As Ball) As Team

        If IsNothing(ball.LastHandledBy) Then Return Nothing

        Return ball.LastHandledBy.Team

    End Function

    Private Function GetTeamByPlayer(Player As Pony) As Team

        For Each team In Teams
            For Each Position In team.Positions
                If ReferenceEquals(Position.Player, Player) Then
                    Return team
                End If
            Next
        Next

        Return Nothing

    End Function

    Public Class Ball
        Public Property Type As BallType
        Public Property StartPosition As Point
        Private initialPosition As Point
        Private friction As Double = 0.992
        Public Property LastHandledBy As Position
        Private speed As Double

        Public Property Handler As Pony 'the ball is a pony type that move like a pony

        Public Sub New(_type As String, idle_image_filename As String, slow_right_image_filename As String, slow_left_image_filename As String,
                fast_right_image_filename As String, fast_left_image_filename As String, x_location As Integer, y_location As Integer,
                files_path As String, collection As PonyCollection)

            ' We need to duplicate the new pony as only "duplicates" are fully loaded. A new pony by itself is considered a template.
            Dim handlerBase = PonyBase.CreateInMemory(collection)
            handlerBase.DisplayName = "Ball"
            Handler = New Pony(handlerBase)

            handlerBase.AddBehavior("idle", 100, 99, 99, 0,
                                    files_path & Replace(idle_image_filename, ControlChars.Quote, ""),
                                    files_path & Replace(idle_image_filename, ControlChars.Quote, ""),
                                    AllowedMoves.None, "", "", "", "", "")
            handlerBase.AddBehavior("slow", 100, 99, 99, 3,
                                    files_path & Replace(slow_right_image_filename, ControlChars.Quote, ""),
                                    files_path & Replace(slow_left_image_filename, ControlChars.Quote, ""),
                                    AllowedMoves.All, "", "", "", "", "")
            handlerBase.AddBehavior("fast", 100, 99, 99, 5,
                                    files_path & Replace(fast_right_image_filename, ControlChars.Quote, ""),
                                    files_path & Replace(fast_left_image_filename, ControlChars.Quote, ""),
                                    AllowedMoves.All, "", "", "", "", "")

            initialPosition = New Point(x_location, y_location)

            Select Case LCase(Trim(_type))

                Case "soccer"
                    Type = BallType.Soccer
                Case "baseball"
                    Type = BallType.Baseball
                Case "pingpong"
                    Type = BallType.PingPong
                Case "dodge"
                    Type = BallType.Dodge
                Case Else
                    Throw New ArgumentException("Invalid ball type: " & _type, _type)
            End Select
        End Sub

        Public Sub Initialize(gameScreen As Screen)
            Argument.EnsureNotNull(gameScreen, "gameScreen")

            StartPosition = New Point(
                CInt(initialPosition.X * 0.01 * gameScreen.WorkingArea.Width + gameScreen.WorkingArea.X),
                CInt(initialPosition.Y * 0.01 * gameScreen.WorkingArea.Height + gameScreen.WorkingArea.Y))

            Handler.TopLeftLocation = StartPosition
        End Sub

        Public Function Center() As Point
            Return New Point(CInt(Me.Handler.TopLeftLocation.X + Me.Handler.CurrentImageSize.X / 2),
                             CInt(Me.Handler.TopLeftLocation.Y + Me.Handler.CurrentImageSize.Y / 2))
        End Function

        Public Sub Update()
            Dim up = True
            Dim right = True
            Dim angle As Double = 0

            Select Case Type
                Case BallType.Soccer
                    speed = Handler.CurrentBehavior.Speed * friction
                Case BallType.PingPong
                    speed = Handler.CurrentBehavior.Speed
            End Select

            up = Handler.facingUp
            right = Handler.facingRight
            angle = Handler.Diagonal

            Select Case Handler.CurrentBehavior.Speed
                Case Is < 0.1
                    speed = 0
                    Handler.CurrentBehavior = Handler.Behaviors(0)
                Case Is < 3
                    Handler.CurrentBehavior = Handler.Behaviors(1)
                Case Is > 3
                    Handler.CurrentBehavior = Handler.Behaviors(2)
            End Select

            speed = Handler.CurrentBehavior.Speed
            Handler.facingUp = up
            Handler.facingRight = right
            Handler.Diagonal = angle
            Handler.Move()
        End Sub

        Public Sub Kick(_speed As Double, _angle As Double, kicker As Position)
            LastHandledBy = kicker

            Handler.CurrentBehavior = Handler.GetAppropriateBehaviorOrCurrent(AllowedMoves.All, True)
            speed = _speed
            Handler.Diagonal = _angle
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

        Public Sub New(_teamNumber As Integer, imageFilename As String, location As String)
            If Not File.Exists(imageFilename) Then
                Throw New FileNotFoundException("File does not exist: " & imageFilename)
            End If

            TeamNumber = _teamNumber
            Dim base As New EffectBase("Team " & TeamNumber & "'s Goal", imageFilename, imageFilename)
            HostEffect = New Effect(base, True)

            Dim locationParts = Split(location, ",")
            startPoint = New Point(
                Integer.Parse(locationParts(0), CultureInfo.InvariantCulture),
                Integer.Parse(locationParts(1), CultureInfo.InvariantCulture))
        End Sub

        Public Sub Initialize(gameScreen As Screen)
            Argument.EnsureNotNull(gameScreen, "gameScreen")

            HostEffect.Location = New Point(
                CInt(startPoint.X * 0.01 * gameScreen.WorkingArea.Width + gameScreen.WorkingArea.X),
                CInt(startPoint.Y * 0.01 * gameScreen.WorkingArea.Height + gameScreen.WorkingArea.Y))
        End Sub

        Public Function Center() As Point
            Return New Point(CInt(HostEffect.Location.X + (HostEffect.CurrentImageSize.Width / 2)),
                             CInt(HostEffect.Location.Y + (HostEffect.CurrentImageSize.Height) / 2))
        End Function
    End Class

    Public Class GameScoreboard
        Public Property HostEffect As Effect
        Private startPoint As Point

        Private teamOneNameDisplay As New ScoreDisplay(Me) With {.LocalPosition = New Size(66, 107)}
        Private teamTwoNameDisplay As New ScoreDisplay(Me) With {.LocalPosition = New Size(66, 150)}
        Private teamOneScoreDisplay As New ScoreDisplay(Me) With {.LocalPosition = New Size(130, 113)}
        Private teamTwoScoreDisplay As New ScoreDisplay(Me) With {.LocalPosition = New Size(135, 156)}

        Public ReadOnly Iterator Property ScoreDisplays As IEnumerable(Of ScoreDisplay)
            Get
                Yield teamOneNameDisplay
                Yield teamTwoNameDisplay
                Yield teamOneScoreDisplay
                Yield teamTwoScoreDisplay
            End Get
        End Property

        Public Sub New(x As String, y As String, imageFilename As String)
            If Not File.Exists(imageFilename) Then
                Throw New FileNotFoundException("File does not exist: " & imageFilename)
            End If

            Dim base As New EffectBase("Scoreboard", imageFilename, imageFilename)
            HostEffect = New Effect(base, True)

            startPoint = New Point(
                Integer.Parse(x, CultureInfo.InvariantCulture),
                Integer.Parse(y, CultureInfo.InvariantCulture))
        End Sub

        Public Sub Initialize(gameScreen As Screen)
            Argument.EnsureNotNull(gameScreen, "gameScreen")

            HostEffect.Location = New Point(
                CInt(startPoint.X * 0.01 * gameScreen.WorkingArea.Width + gameScreen.WorkingArea.X),
                CInt(startPoint.Y * 0.01 * gameScreen.WorkingArea.Height + gameScreen.WorkingArea.Y))
        End Sub

        Public Function Center() As Point
            Return New Point(CInt(HostEffect.Location.X + (HostEffect.CurrentImageSize.Width / 2)),
                             CInt(HostEffect.Location.Y + (HostEffect.CurrentImageSize.Height) / 2))
        End Function

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
            Public Property LocalPosition As Size

            Public Sub New(parentBoard As GameScoreboard)
                parent = parentBoard
            End Sub

            Public ReadOnly Property IsSpeaking As Boolean Implements ISpeakingSprite.IsSpeaking
                Get
                    Return True
                End Get
            End Property

            Public ReadOnly Property SpeechText As String Implements ISpeakingSprite.SpeechText
                Get
                    Return Text
                End Get
            End Property

            Public ReadOnly Property CurrentTime As TimeSpan Implements ISprite.CurrentTime
                Get
                    Return TimeSpan.Zero
                End Get
            End Property

            Public ReadOnly Property FlipImage As Boolean Implements ISprite.FlipImage
                Get
                    Return False
                End Get
            End Property

            Public ReadOnly Property ImagePath As String Implements ISprite.ImagePath
                Get
                    Return parent.HostEffect.CurrentImagePath
                End Get
            End Property

            Public ReadOnly Property Region As Rectangle Implements ISprite.Region
                Get
                    Return New Rectangle(parent.HostEffect.Location + LocalPosition, Size.Empty)
                End Get
            End Property

            Public Sub Start(startTime As TimeSpan) Implements ISprite.Start

            End Sub

            Public Sub Update(updateTime As TimeSpan) Implements ISprite.Update

            End Sub
        End Class
    End Class

    Public Class Position
        Public Property Name As String
        Public Property TeamNumber As Integer
        Public Property Team As Team
        Public Property Player As Pony
        Private allowedArea As Rectangle? 'nothing means allowed anywhere
        Private startLocation As Point
        Public Property CurrentAction As PlayerActionType
        Public Property CurrentActionGroup As List(Of PlayerActionType)

        Public Property Required As Boolean

        Private areaPoints As String()

        Private lastKickTime As DateTime = DateTime.MinValue

        Private nearestBallDistance As Integer

        Public Property SelectionMenuPictureBox As PictureBox

        Private ReadOnly haveBallActions As New List(Of PlayerActionType)()
        Private ReadOnly hostileBallActions As New List(Of PlayerActionType)()
        Private ReadOnly friendlyBallActions As New List(Of PlayerActionType)()
        Private ReadOnly neutralBallActions As New List(Of PlayerActionType)()
        Private ReadOnly distantBallActions As New List(Of PlayerActionType)()
        Private ReadOnly noBallActions As New List(Of PlayerActionType)()

        Public Sub New(_Name As String, _team_number As Integer, _start_location As String, _Allowed_area As String,
                _Have_Ball_Actions As String, _Hostile_Ball_Actions As String, _Friendly_Ball_Actions As String, _
                _Neutral_Ball_Actions As String, _Distance_Ball_Actions As String, _No_Ball_Actions As String, _required As String)
            Argument.EnsureNotNull(_Name, "_Name")

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
            End If
        End Sub

        Public Sub DecideOnAction(game As Game)
            Argument.EnsureNotNull(game, "game")
            Dim nearest_ball = GetNearestBall(game.activeBalls)

            If IsNothing(nearest_ball) Then
                PerformAction(noBallActions, Nothing)
                Exit Sub
            End If

            Dim screen_diagonal = Math.Sqrt((game.GameScreen.WorkingArea.Height) ^ 2 + (game.GameScreen.WorkingArea.Width) ^ 2)
            If nearestBallDistance > screen_diagonal * (1 / 2) Then 'AndAlso _
                PerformAction(distantBallActions, nearest_ball)
                Exit Sub
            End If

            If nearestBallDistance <= (Player.CurrentImageSize.X / 2) + 50 Then ' / 2 Then
                PerformAction(haveBallActions, nearest_ball)
                Exit Sub
            End If

            Dim BallOwner_Team = game.GetBallLastHandlerTeam(nearest_ball)

            If IsNothing(BallOwner_Team) Then
                PerformAction(neutralBallActions, nearest_ball)
                Exit Sub
            End If

            If BallOwner_Team.Number = TeamNumber Then
                PerformAction(friendlyBallActions, nearest_ball)
                Exit Sub
            Else
                PerformAction(hostileBallActions, nearest_ball)
                Exit Sub
            End If

        End Sub

        Private Sub PerformAction(actionList As List(Of PlayerActionType), ball As Ball)
            If Not IsNothing(CurrentActionGroup) Then ' AndAlso Not ReferenceEquals(Current_Action_Group, Have_Ball_Actions) Then
                If ReferenceEquals(actionList, CurrentActionGroup) Then
                    'we are already doing an action from this list

                    'if we recently kicked the ball, don't do it for 2 seconds.
                    If ReferenceEquals(CurrentActionGroup, haveBallActions) Then
                        If DateDiff(DateInterval.Second, lastKickTime, DateTime.UtcNow) <= 2 Then
                            If DateDiff(DateInterval.Second, lastKickTime, DateTime.UtcNow) > 1 AndAlso (Player.ManualControlPlayerOne OrElse Player.ManualControlPlayerTwo) Then
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

            Dim selection As Integer = Rng.Next(actionList.Count)

            Dim selected_action As PlayerActionType = actionList(selection)

            Select Case selected_action

                Case PlayerActionType.NotSet
                    Throw New Exception("Can't do this action (reserved): " & selected_action)
                Case PlayerActionType.ReturnToStart
                    SetFollowBehavior(Nothing, Nothing, True)
                Case PlayerActionType.ChaseBall
                    SetFollowBehavior(ball.Handler.Directory, ball.Handler)
                Case PlayerActionType.LeadBall
                    SetFollowBehavior(ball.Handler.Directory, ball.Handler, False, True)
                Case PlayerActionType.AvoidBall
                    Throw New NotImplementedException("Not implemented yet: action type " & selected_action)
                Case PlayerActionType.ThrowBallToGoal
                    If DateDiff(DateInterval.Second, lastKickTime, DateTime.UtcNow) <= 2 Then
                        'can't kick again so soon
                        Exit Sub
                    End If
                    'If ponies are being controlled, don't kick unless the action key (control - left or right) is being pushed
                    If Player.ManualControlPlayerOne AndAlso Not Player.ManualControlAction Then Exit Sub
                    If Player.ManualControlPlayerTwo AndAlso Not Player.ManualControlAction Then Exit Sub
                    KickBall(ball, 10, GetOtherTeamGoal(), Nothing, Me, "*Kick*!")
                    lastKickTime = DateTime.UtcNow
                Case PlayerActionType.ThrowBallToTeammate
                    If DateDiff(DateInterval.Second, lastKickTime, DateTime.UtcNow) <= 2 Then
                        'can't kick again so soon
                        Exit Sub
                    End If

                    'don't pass if we are under control - kick instead.
                    If Player.ManualControlPlayerOne Then Exit Sub
                    If Player.ManualControlPlayerTwo Then Exit Sub

                    Dim open_teammate = GetOpenTeammate(Me.Team, GetOtherTeamGoal())
                    If IsNothing(open_teammate) Then
                        'no teammates to pass to, kick to goal instead, unless a player controlled pony.
                        If Player.ManualControlPlayerOne OrElse Player.ManualControlPlayerTwo Then Exit Sub
                        KickBall(ball, 10, GetOtherTeamGoal(), Nothing, Me, "*Kick*!")
                        lastKickTime = DateTime.UtcNow
                        Exit Sub
                    End If

                    KickBall(ball, 10, Nothing, open_teammate, Me, "*Pass*!")
                    lastKickTime = DateTime.UtcNow

                Case PlayerActionType.ThrowBallReflect
                    If DateDiff(DateInterval.Second, lastKickTime, DateTime.UtcNow) <= 2 Then
                        'can't kick again so soon
                        Exit Sub
                    End If

                    If Player.ManualControlPlayerOne AndAlso Not Player.ManualControlAction Then Exit Sub
                    If Player.ManualControlPlayerTwo AndAlso Not Player.ManualControlAction Then Exit Sub

                    BounceBall(ball, 7, Me, "*Ping*!")
                    lastKickTime = DateTime.UtcNow

                Case PlayerActionType.ApproachOwnGoal
                    Dim goal = GetTeamGoal()
                    SetFollowBehavior(goal.HostEffect.Base.Name, goal.HostEffect)

                Case PlayerActionType.ApproachTargetGoal
                    Dim goal = GetOtherTeamGoal()
                    SetFollowBehavior(goal.HostEffect.Base.Name, goal.HostEffect)

                Case PlayerActionType.Idle
                    If CurrentAction = PlayerActionType.Idle Then Exit Sub
                    Player.followTarget = Nothing
                    Player.followTargetName = ""

                    If Player.ManualControlPlayerOne Then Exit Sub
                    If Player.ManualControlPlayerTwo Then Exit Sub

                    Player.SelectBehavior()
                    SpeedOverride(True)

                Case Else
                    Pony.CurrentAnimator.Pause(False)
                    Throw New System.ComponentModel.InvalidEnumArgumentException("Invalid action type: " & selected_action)
            End Select

            CurrentAction = selected_action
            CurrentActionGroup = actionList

        End Sub

        Private Function GetNearestBall(balls As List(Of Ball)) As Ball
            Dim nearest_ball As Ball = Nothing
            Dim nearest_ball_distance As Double = Double.MaxValue

            For Each ball In balls
                Dim distance = Vector2.Distance(Player.CenterLocation, ball.Center)
                If distance < nearest_ball_distance Then
                    nearest_ball_distance = distance
                    nearest_ball = ball
                End If
            Next

            If IsNothing(nearest_ball) Then Throw New Exception("No available balls found when checking distance!")

            Me.nearestBallDistance = CInt(nearest_ball_distance)
            Return nearest_ball

        End Function

        Private Function GetOtherTeamGoal() As GoalArea

            For Each goal In Game.CurrentGame.goals
                If goal.TeamNumber <> TeamNumber Then
                    Return goal
                End If
            Next

            Throw New Exception("Couldn't find a goal for another team.")

        End Function

        Private Function GetTeamGoal() As GoalArea

            For Each goal In Game.CurrentGame.goals
                If goal.TeamNumber = TeamNumber Then
                    Return goal
                End If
            Next

            Throw New Exception("Couldn't find a goal for pony's team.")

        End Function

        Public Sub SetFollowBehavior(targetName As String, target As ISprite,
                                      Optional returnToStart As Boolean = False, Optional leadTarget As Boolean = False)

            SpeedOverride(True)

            Player.CurrentBehavior = Player.GetAppropriateBehaviorOrCurrent(AllowedMoves.All, True)
            'Player.CurrentBehavior = Player.GetAppropriateBehaviorForSpeed()
            Player.followTarget = Nothing
            Player.followTargetName = ""

            If returnToStart Then
                Player.destinationCoords = startLocation
                Exit Sub
            Else
                Player.followTargetName = If(targetName, "")
                Player.followTarget = target
                Player.destinationCoords = Point.Empty
                If leadTarget Then
                    Player.leadTarget = True
                End If

                'If Main.Instance.current_game.Name = "Ping Pong Pony" Then
                '    Player.current_behavior.speed = Player.current_behavior.original_speed * 1.5
                'End If
            End If
        End Sub

        Private Sub KickBall(ball As Ball, speed As Double, targetGoal As GoalArea, targetPony As Pony,
                              kicker As Position, line As String)
            If Rng.NextDouble() < 0.05 Then
                Speak("Missed!")
                Exit Sub
            End If

            speed = kicker.Player.Scale * speed

            Speak(line)

            Dim angle As Double = Nothing

            If IsNothing(targetGoal) Then
                angle = GetAngleToObject(targetPony.CenterLocation)
            Else
                angle = GetAngleToObject(targetGoal.Center)
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
            If Game.CurrentGame.Name = "Ping Pong Pony" Then
                'avoid boucing the ball back into our own goal.
                If Not IsNothing(ball.LastHandledBy) AndAlso ReferenceEquals(ball.LastHandledBy, Me) Then
                    Exit Sub
                End If
            End If

            Speak(line)

            Dim angle As Double
            Dim gamescreen = Game.CurrentGame.GameScreen

            If ball.Handler.Diagonal < (Math.PI / 2) OrElse ball.Handler.Diagonal > (3 / 2) * Math.PI Then
                'ball is going to the right, it will 'bounce' to the left.
                angle = Math.PI
            Else
                'ball is going to the left, and will 'bounce' right.
                angle = 0
            End If

            Dim ball_center As Point = ball.Handler.CenterLocation
            Dim kicker_center As Point = kicker.Player.CenterLocation

            Dim y_difference = kicker_center.Y - ball_center.Y

            Dim kicker_height = kicker.Player.CurrentImageSize.Y / 1.5

            If kicker.Player.CenterLocation.X < (gamescreen.WorkingArea.Width * 0.5) Then

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

            ball.Kick(speed * kicker.Player.Scale, angle, kicker)

        End Sub

        'returns radians
        Private Function GetAngleToObject(target As Point) As Double

            'opposite = y_distance
            Dim opposite = Player.CenterLocation.Y - target.Y

            'adjacent = x_distance
            Dim adjacent = Player.CenterLocation.X - target.X

            Dim hypotenuse As Double = Math.Sqrt(opposite ^ 2 + adjacent ^ 2)

            'sin(angle) = opposite / h
            'angle = asin(opposite / h)

            Dim angle = Math.Asin(Math.Abs(opposite) / hypotenuse)

            ' if the target is below, flip the angle to the 4th quadrant
            If target.Y > Player.CenterLocation.Y Then
                angle = (2 * Math.PI) - angle
                'if the target is to the left, flip the angle to 3rd quadrant
                If target.X < Player.CenterLocation.X Then
                    angle = Math.PI - angle
                End If
            Else
                ' If the tartget is above and to the left, flip the angle to the 2nd quadrant.
                If target.X < Player.CenterLocation.X Then
                    angle = Math.PI - angle
                End If
            End If

            Return angle

        End Function

        Private Sub Speak(line As String)
            Dim new_line As New Speech() With {.Name = "Kick", .Text = line, .Skip = True, .Group = 0}
            Player.PonySpeak(new_line)
        End Sub

        Public Sub PushBackOverlappingPonies(allPositions As List(Of Position))
            Argument.EnsureNotNull(allPositions, "allPositions")

            For Each otherposition As Position In allPositions
                Dim otherpony = otherposition.Player
                If ReferenceEquals(Me.Player, otherpony) Then Continue For


                If DoesPonyOverlap(Me.Player, otherpony) Then
                    'Push overlapping ponies a bit apart
                    PonyPush(Me.Player, otherpony)
                    Exit Sub
                End If


            Next

        End Sub

        Private Shared Function DoesPonyOverlap(pony As Pony, otherPony As Pony) As Boolean

            Dim otherpony_area As New Rectangle(otherPony.TopLeftLocation.X, _
                                             otherPony.TopLeftLocation.Y, _
                                             CInt(otherPony.CurrentImageSize.X * otherPony.Scale),
                                             CInt(otherPony.CurrentImageSize.Y * otherPony.Scale))


            If otherpony_area.IntersectsWith(New Rectangle(pony.TopLeftLocation, New Size(CInt(pony.Scale * pony.CurrentImageSize.X), _
                                                                                    CInt(pony.Scale * pony.CurrentImageSize.Y)))) Then
                Return True
            Else
                Return False
            End If

        End Function

        Private Sub PonyPush(pony1 As Pony, pony2 As Pony)

            Dim xchange = 1
            Dim ychange = 2
            If pony1.GetDestinationDirectionHorizontal(pony2.CenterLocation) = Direction.MiddleLeft Then
                xchange = -1
            End If
            If pony1.GetDestinationDirectionVertical(pony2.CenterLocation) = Direction.TopCenter Then
                ychange = -2
            End If

            Dim change = New Size(xchange, ychange)
            Dim new_location = pony1.TopLeftLocation + change

            If pony1.IsPonyOnScreen(new_location, Game.CurrentGame.GameScreen) AndAlso
                (Not allowedArea.HasValue OrElse Pony.IsPonyInBox(new_location, allowedArea.Value)) Then
                pony1.TopLeftLocation = new_location
            End If

        End Sub

        Private Shared Function FriendlyPoniesAroundBall(ball As Ball, team As Team, minDistance As Integer) As List(Of Pony)

            Dim ponies As New List(Of Pony)

            For Each Position In team.Positions
                Dim distance = Vector2.Distance(Position.Player.CenterLocation, ball.Center)
                If distance <= minDistance Then
                    ponies.Add(Position.Player)
                End If
            Next

            Return ponies
        End Function

        'get a teammate that is not near any enemy players and is closer to the goal than we are.
        Private Function GetOpenTeammate(team As Team, goal As GoalArea) As Pony

            Dim open_teammates As New List(Of Pony)

            For Each Position In team.Positions

                If ReferenceEquals(Position, Me) Then
                    Continue For
                End If

                Dim open = True
                For Each other_position As Position In Game.CurrentGame.allPlayers
                    If other_position.Team.Name = Me.Team.Name Then
                        Continue For
                    End If

                    Dim distance = Vector2.Distance(Position.Player.CenterLocation, other_position.Player.CenterLocation)
                    If distance <= 200 Then
                        open = False
                    End If
                Next

                Dim me_distance_to_goal = Vector2.Distance(Player.CenterLocation, goal.Center)
                Dim teammate_distance_to_goal = Vector2.Distance(Position.Player.CenterLocation, goal.Center)

                If open = True AndAlso teammate_distance_to_goal <= me_distance_to_goal Then
                    open_teammates.Add(Position.Player)
                End If
            Next

            If open_teammates.Count = 0 Then Return Nothing

            Return open_teammates(Rng.Next(open_teammates.Count))

        End Function

        Private Sub SpeedOverride(enable As Boolean)
            If enable Then
                Player.SpeedOverride = If(Game.CurrentGame.Name = "Ping Pong Pony", 8 * Player.Scale, 5 * Player.Scale)
            Else
                Player.SpeedOverride = Nothing
            End If
        End Sub

    End Class
End Class
