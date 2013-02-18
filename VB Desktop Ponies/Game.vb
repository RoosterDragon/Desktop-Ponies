Imports System.Globalization
Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Module Games
    Class Game
        Public Const RootDirectory = "Games"
        Public Const ConfigFilename = "game.ini"

        Enum BallType

            Soccer = 0 'Is pushed around and slows to a stop.  No Gravity
            Baseball = 1 'Is thrown and arcs, then slows if not caught.
            PingPong = 2 'Bounces around and doesn't stop/slow
            Dodge = 3 'Travels in a straight line and is destroyed when impacting barriers

        End Enum

        Enum GameStatus

            Setup = 0
            Ready = 1
            InProgress = 2
            '   Round_Finishing = 3
            'next_Round_Setup = 4
            'Finishing = 5
            Completed = 6

        End Enum

        Enum ScoreStyle

            Ball_at_Goal = 0
            Ball_Hits_Other_Team = 1
            Ball_Destroyed = 2
            Ball_At_Sides = 3

        End Enum

        Enum PlayerActionType

            Not_Set = 0
            Return_To_Start = 1
            Chase_Ball = 2
            Avoid_Ball = 3
            Throw_Ball_ToGoal = 4
            Throw_Ball_ToTeammate = 5
            Throw_Ball_AtTarget = 6
            Throw_Ball_Reflect = 7 'ball bounces off
            Approach_Own_Goal = 8
            Approach_Target_Goal = 9
            Lead_Ball = 10
            Approach_Target = 12
            Idle = 13

        End Enum

        Public Name As String = ""
        Friend Description As String = ""

        Const MinTeams As Integer = 2
        Friend Teams As New List(Of Team)
        Dim AllPlayers As New List(Of Position)

        Friend Balls As New List(Of Ball)

        Friend Active_Balls As New List(Of Ball)

        Dim MinBalls As Integer
        Dim MaxBalls As Integer

        Friend Status As GameStatus
        Friend MaxScore As Integer

        Dim WinningTeam As Team

        Friend ScoreBoard As Game_ScoreBoard
        Dim ScoreBoard_Location As Point
        Dim ScoringStyles As New List(Of ScoreStyle)

        Friend GameScreen As Screen = Nothing

        Friend Goals As New List(Of Goal_Area)

        Sub New(config_file_path As String, files_path As String)

            Dim position_data As New List(Of String)
            Dim game_data As String = Nothing
            Dim description_data As String = Nothing
            Dim ball_data As New List(Of String)
            Dim goal_data As New List(Of String)

            Using config_file As New System.IO.StreamReader(config_file_path)
                Do Until config_file.EndOfStream
                    Dim line = config_file.ReadLine

                    'ignore blank or 'commented out' lines.
                    If line = "" OrElse line(0) = "'" Then
                        Continue Do
                    End If

                    Dim columns = CommaSplitQuoteQualified(line)

                    If UBound(columns) < 1 Then
                        Continue Do
                    End If

                    Select Case LCase(columns(0))

                        Case "game"
                            game_data = line
                        Case "description"
                            description_data = line
                        Case "position"
                            position_data.Add(line)
                        Case "ball"
                            ball_data.Add(line)
                        Case "goal"
                            goal_data.Add(line)
                        Case "scoreboard"
                            ScoreBoard = New Game_ScoreBoard(columns(1) & "," & columns(2),
                                                             files_path & Trim(columns(3).Replace(ControlChars.Quote, "")))
                        Case Else
                            Throw New InvalidDataException("Invalid line in config file: " & line)
                    End Select

                Loop
            End Using

            If IsNothing(game_data) OrElse position_data.Count = 0 OrElse ball_data.Count = 0 Then
                Throw New InvalidDataException("Game ini file does not define Game, Position, or Ball data.  It must contain all 3.")
            End If

            'process game data

            Dim game_columns = CommaSplitBraceQualified(game_data)

            Name = game_columns(1).Replace(ControlChars.Quote, "")

            Dim description_columns = CommaSplitQuoteQualified(game_data)

            Try
                Description = description_columns(1)
            Catch ex As Exception
                MsgBox("Invalid description line for game: " & Name & " . Are you missing quotes around the text?")
            End Try

            MinBalls = Integer.Parse(game_columns(3), CultureInfo.InvariantCulture)
            MaxBalls = Integer.Parse(game_columns(4), CultureInfo.InvariantCulture)
            MaxScore = Integer.Parse(game_columns(5), CultureInfo.InvariantCulture)
            If MaxScore < 1 Then
                Throw New InvalidDataException("The maximum score must be at least 1 - error loading name " & Name)
            End If
            If MaxBalls < MinBalls Then Throw New InvalidDataException(
                "Minimum number of balls in play is larger than the Maximum setting defined for game: " & Name)

            If MinTeams < 2 Then Throw New InvalidDataException(
                "You must have at least two teams for a game.  The minimum setting is too low for game: " & Name)
            'maybe later we can have them play tag or a zombie game... but for now:
            If MinBalls < 1 Then Throw New InvalidDataException(
                "You must have at least one ball for the ponies to play with.  The minimum setting is too low for game: " & Name)

            'do scoring styles
            Dim ScoringStyles_list = Split(game_columns(6), ",")
            For Each style In ScoringStyles_list
                Select Case Trim(LCase(style))
                    Case "ball_at_goal"
                        ScoringStyles.Add(ScoreStyle.Ball_at_Goal)
                    Case "ball_hits_other_team"
                        ScoringStyles.Add(ScoreStyle.Ball_Hits_Other_Team)
                    Case "ball_destroyed"
                        ScoringStyles.Add(ScoreStyle.Ball_Destroyed)
                    Case "ball_at_sides"
                        ScoringStyles.Add(ScoreStyle.Ball_At_Sides)
                    Case Else
                        Throw New InvalidDataException("Invalid scoring style: " & style)
                End Select
            Next

            'do teamnames
            Dim TeamNames = CommaSplitQuoteQualified(game_columns(2))

            Dim number = 1
            For Each line In TeamNames
                Dim new_team As New Team(line, number)
                Teams.Add(new_team)
                number += 1
            Next

            'do goals
            For Each line In goal_data
                Dim columns = CommaSplitBraceQualified(line)
                Dim new_goal As New Goal_Area(Integer.Parse(columns(1), CultureInfo.InvariantCulture),
                                              files_path & Trim(columns(2).Replace(ControlChars.Quote, "")), columns(3))
                Goals.Add(new_goal)
            Next

            For Each goal As Goal_Area In Goals
                If goal.team_number <> 0 Then
                    Teams(goal.team_number - 1).Goal = goal
                End If
            Next

            'do positions
            For Each line In position_data
                Dim columns = CommaSplitBraceQualified(line)

                Dim new_position As New Position(columns(1), Integer.Parse(columns(2), CultureInfo.InvariantCulture), columns(3),
                                                 columns(4), columns(5), columns(6), columns(7), columns(8), columns(9), columns(10),
                                                 columns(11))

                Teams(Integer.Parse(columns(2), CultureInfo.InvariantCulture) - 1).Positions.Add(new_position)
            Next

            For Each line In ball_data
                Dim columns = CommaSplitQuoteQualified(line)

                Dim new_ball As New Ball(columns(1),
                                         Trim(columns(2)), Trim(columns(3)), Trim(columns(4)), Trim(columns(5)), Trim(columns(6)),
                                         Integer.Parse(columns(7), CultureInfo.InvariantCulture),
                                         Integer.Parse(columns(8), CultureInfo.InvariantCulture), files_path)
                Balls.Add(new_ball)
            Next

            Status = GameStatus.Setup

        End Sub

        Friend Sub CleanUp()

            For Each Ball In Balls
                '  Ball.Handler.Close()
                Active_Balls.Remove(Ball)
            Next

            Pony.CurrentAnimator.Clear()

            'For Each goal In Goals
            '    goal.Visible = False
            'Next

            'ScoreBoard.Visible = False

            Teams(0).Score = 0
            Teams(1).Score = 0

            'For Each position As Position In AllPlayers
            '    '  position.Player.Close()
            'Next

            AllPlayers.Clear()

        End Sub

        Friend Sub Setup()


            If Options.WindowAvoidanceEnabled OrElse Options.CursorAvoidanceEnabled Then
                Options.WindowAvoidanceEnabled = False
                Options.CursorAvoidanceEnabled = False
                '  MsgBox("Note:  Window avoidance and cursor avoidance have been disabled as they may interfere with the game.")
            End If


            For Each Goal As Goal_Area In Goals
                Goal.Initialize(GameScreen)
                Goal.form.DesiredDuration = 60 * 60 * 24 * 365
                Pony.CurrentAnimator.AddEffect(Goal.form)
            Next

            ScoreBoard.Initialize(GameScreen)
            ScoreBoard.SetScore(Teams(0).Name, Teams(0).Score, Teams(1).Name, Teams(1).Score)
            ScoreBoard.form.DesiredDuration = 60 * 60 * 24 * 365
            Pony.CurrentAnimator.AddEffect(ScoreBoard.form)

            For Each Team As Team In Teams
                Dim positions_to_remove As New List(Of Position)
                For Each Position As Position In Team.Positions
                    Position.Initialize(GameScreen)
                    If Not IsNothing(Position.Player) Then

                        Position.Player.PlayingGame = True
                        Pony.CurrentAnimator.AddPony(Position.Player)
                        AllPlayers.Add(Position)
                    Else
                        positions_to_remove.Add(Position)
                    End If
                Next
                For Each entry In positions_to_remove
                    Team.Positions.Remove(entry)
                Next
            Next

            For Each Ball In Balls
                Ball.Initialize(GameScreen)
                Pony.CurrentAnimator.AddPony(Ball.Handler)
            Next

            Dim monitor = GameScreen
            Main.Instance.ScreensToUse.Clear()
            Main.Instance.ScreensToUse.Add(monitor)

            If Options.ScaleFactor <> 1 Then
                MsgBox("Note:  Games may not work properly with the scale option set to values other than 1...  You are currently playing with scale " & Options.ScaleFactor & "x.")
            End If

        End Sub

        Friend Sub Update()

            Select Case Status

                Case GameStatus.Setup

                    Dim all_in_position = True

                    For Each Team As Team In Teams
                        For Each Position As Position In Team.Positions

                            If IsNothing(Position.Current_Action) OrElse Position.Current_Action <> PlayerActionType.Return_To_Start _
                                OrElse Position.Player.CurrentBehavior.AllowedMovement = Pony.AllowedMoves.None Then
                                Position.SetFollowBehavior(Nothing, Nothing, True) 'go to starting position
                            End If

                            If Position.Player.AtDestination = False Then
                                all_in_position = False
                            End If

                            'Position.Player.Start(Pony.CurrentAnimator.ElapsedTime)

                        Next
                    Next

                    If all_in_position Then
                        Status = GameStatus.Ready
                    End If

                Case GameStatus.Ready

                    'For Each Ball In Balls
                    '    Ball.Handler.Visible = False
                    'Next

                    For Each position As Position In AllPlayers
                        position.Current_Action = Nothing
                        position.Current_Action_Group = Nothing
                    Next

                    For Each Ball In Balls
                        Active_Balls.Add(Ball)
                        Ball.Handler.CurrentBehavior = Ball.Handler.GetAppropriateBehaviorOrCurrent(Pony.AllowedMoves.None, False)
                        Ball.Handler.TopLeftLocation = Ball.StartPosition
                        'Ball.Handler.Visible = True
                        Pony.CurrentAnimator.AddPony(Ball.Handler)
                        Ball.Update()
                        If Ball.Type = BallType.PingPong Then
                            Ball.Kick(5, Rng.NextDouble() * (2 * Math.PI), Nothing)
                        End If
                        If Active_Balls.Count >= MinBalls Then Exit For
                    Next

                    Status = GameStatus.InProgress

                Case GameStatus.InProgress

                    For Each Team As Team In Teams
                        For Each Position As Position In Team.Positions
                            Position.Decide_On_Action(Main.Instance.CurrentGame)
                            Position.PushBackOverlappingPonies(AllPlayers)
                            'Position.Player.Update(Pony.CurrentAnimator.ElapsedTime)
                        Next
                    Next

                    For Each Ball In Active_Balls
                        Ball.Update()
                    Next

                    If CheckForScore() Then

                        For Each Ball In Balls
                            Active_Balls.Remove(Ball)
                            Pony.CurrentAnimator.RemovePony(Ball.Handler)
                        Next

                        For Each Team In Teams
                            If Team.Score >= MaxScore Then
                                Status = GameStatus.Completed
                                Pony.CurrentAnimator.Pause(False)
                                MsgBox(Team.Name & " won!")
                                Main.Instance.Pony_Shutdown()
                                Main.Instance.Visible = True
                                Exit Sub
                            End If
                        Next

                        Status = GameStatus.Setup
                    End If


                Case Else
                    Throw New NotImplementedException("State not implemented: " & Status)

            End Select

        End Sub

        Shared Function Get_Ball_LastHandler_Team(ball As Ball) As Team

            If IsNothing(ball.Last_Handled_By) Then Return Nothing

            Return ball.Last_Handled_By.Team

        End Function

        Function Get_Team_By_Player(Player As Pony) As Team

            For Each Team In Teams
                For Each Position In Team.Positions
                    If ReferenceEquals(Position.Player, Player) Then
                        Return Team
                    End If
                Next
            Next

            Return Nothing

        End Function

        Function CheckForScore() As Boolean

            For Each ScoreStyle As ScoreStyle In Main.Instance.CurrentGame.ScoringStyles

                For Each Ball In Balls

                    Select Case ScoreStyle
                        Case ScoreStyle.Ball_At_Sides
                            If Ball.Handler.TopLeftLocation.X < GameScreen.WorkingArea.X + (GameScreen.WorkingArea.Width * 0.02) Then
                                Teams(1).Score += 1
                                ScoreBoard.SetScore(Teams(0).Name, Teams(0).Score, Teams(1).Name, Teams(1).Score)
                                Return True
                            Else
                                If Ball.Handler.TopLeftLocation.X + Ball.Handler.CurrentImageSize.X > (GameScreen.WorkingArea.X + GameScreen.WorkingArea.Width) - (GameScreen.WorkingArea.Width * 0.02) Then
                                    Teams(0).Score += 1
                                    ScoreBoard.SetScore(Teams(0).Name, Teams(0).Score, Teams(1).Name, Teams(1).Score)
                                    Return True
                                End If
                            End If

                        Case ScoreStyle.Ball_at_Goal

                            For Each goal In Goals
                                Dim goalArea As New Rectangle(goal.form.Location, goal.form.CurrentImageSize())
                                If Pony.IsPonyInBox(Ball.Handler.CenterLocation, goalArea) Then
                                    For Each Team In Teams
                                        If ReferenceEquals(Team.Goal, goal) AndAlso Not ReferenceEquals(Team, Ball.Last_Handled_By.Team) Then
                                            For Each OtherTeam In Teams
                                                If Not ReferenceEquals(OtherTeam, Team) Then
                                                    OtherTeam.Score += 1
                                                    ScoreBoard.SetScore(Teams(0).Name, Teams(0).Score, Teams(1).Name, Teams(1).Score)
                                                    Return True
                                                End If
                                            Next
                                        End If
                                    Next
                                End If
                            Next

                    End Select
                Next
            Next

            Return False


        End Function

        Class Ball

            Friend Type As BallType
            Friend StartPosition As Point
            Dim Initial_Position As Point
            Friend friction As Double = 0.992
            Friend Last_Handled_By As Position = Nothing
            Private m_speed As Double

            Friend Handler As Pony 'the ball is a pony type that move like a pony

            Sub New(_type As String, idle_image_filename As String, slow_right_image_filename As String, slow_left_image_filename As String,
                    fast_right_image_filename As String, fast_left_image_filename As String, x_location As Integer, y_location As Integer, files_path As String)

                ' We need to duplicate the new pony as only "duplicates" are fully loaded. A new pony by itself is considered a template.
                Dim handlerBase = New PonyBase()
                handlerBase.Name = "Ball"
                Handler = New Pony(handlerBase)

                handlerBase.AddBehavior("idle", 100, 99, 99, 0,
                                        files_path & Replace(idle_image_filename, ControlChars.Quote, ""),
                                        files_path & Replace(idle_image_filename, ControlChars.Quote, ""),
                                        Pony.AllowedMoves.None, "", "", "")
                handlerBase.AddBehavior("slow", 100, 99, 99, 3,
                                        files_path & Replace(slow_right_image_filename, ControlChars.Quote, ""),
                                        files_path & Replace(slow_left_image_filename, ControlChars.Quote, ""),
                                        Pony.AllowedMoves.All, "", "", "")
                handlerBase.AddBehavior("fast", 100, 99, 99, 5,
                                        files_path & Replace(fast_right_image_filename, ControlChars.Quote, ""),
                                        files_path & Replace(fast_left_image_filename, ControlChars.Quote, ""),
                                        Pony.AllowedMoves.All, "", "", "")

                Initial_Position = New Point(x_location, y_location)

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

            Sub Initialize(gamescreen As Screen)

                StartPosition = New Point(CInt(Initial_Position.X * 0.01 * gamescreen.WorkingArea.Width + gamescreen.WorkingArea.X), _
                                   CInt(Initial_Position.Y * 0.01 * gamescreen.WorkingArea.Height + gamescreen.WorkingArea.Y))

                Handler.TopLeftLocation = StartPosition
            End Sub

            Friend Function Center() As Point
                Return New Point(CInt(Me.Handler.TopLeftLocation.X + Me.Handler.CurrentImageSize.X / 2),
                                 CInt(Me.Handler.TopLeftLocation.Y + Me.Handler.CurrentImageSize.Y / 2))
            End Function

            Sub Update()
                Dim up = True
                Dim right = True
                Dim angle As Double = 0

                Select Case Type
                    Case BallType.Soccer
                        m_speed = Handler.CurrentBehavior.Speed * friction
                    Case BallType.PingPong
                        m_speed = Handler.CurrentBehavior.Speed
                End Select

                up = Handler.facingUp
                right = Handler.facingRight
                angle = Handler.diagonal

                Select Case Handler.CurrentBehavior.Speed
                    Case Is < 0.1
                        m_speed = 0
                        Handler.CurrentBehavior = Handler.Behaviors(0)
                    Case Is < 3
                        Handler.CurrentBehavior = Handler.Behaviors(1)
                    Case Is > 3
                        Handler.CurrentBehavior = Handler.Behaviors(2)
                End Select

                m_speed = Handler.CurrentBehavior.Speed
                Handler.facingUp = up
                Handler.facingRight = right
                Handler.diagonal = angle
                Handler.Move()
            End Sub

            Friend Sub Kick(_speed As Double, _angle As Double, kicker As Position)
                Last_Handled_By = kicker

                Handler.CurrentBehavior = Handler.GetAppropriateBehaviorOrCurrent(Pony.AllowedMoves.All, True)
                m_speed = _speed
                Handler.diagonal = _angle
            End Sub
        End Class

        Class Team

            Friend Name As String
            Friend Number As Integer
            Friend Positions As New List(Of Position)
            Friend Score As Integer = 0
            Friend Goal As Goal_Area = Nothing

            Sub New(_name As String, _number As Integer)
                Name = _name
                Number = _number
            End Sub

        End Class

        Class Goal_Area

            Friend form As Effect
            Friend team_number As Integer ' 0 = a score any team
            Dim start_point As Point = New Point
            Dim image As Image

            Sub New(_team_number As Integer, image_filename As String, location As String)

                team_number = _team_number
                form = New Effect(image_filename, image_filename)
                form.Name = "Team " & team_number & "'s Goal"
                If Not My.Computer.FileSystem.FileExists(image_filename) Then Throw New FileNotFoundException("File does not exist: " & image_filename)

                Dim location_parts = Split(location, ",")
                start_point = New Point(Integer.Parse(location_parts(0), CultureInfo.InvariantCulture), Integer.Parse(location_parts(1), CultureInfo.InvariantCulture))

                image = image.FromFile(image_filename)

                'form.Size = form.Effect_Image.Image.Size
                'form.Effect_Image.Size = form.Effect_Image.Image.Size

            End Sub

            Sub Initialize(gamescreen As Screen)
                form.Location = New Point(CInt(start_point.X * 0.01 * gamescreen.WorkingArea.Width + gamescreen.WorkingArea.X), _
                                   CInt(start_point.Y * 0.01 * gamescreen.WorkingArea.Height + gamescreen.WorkingArea.Y))
                form.current_image_path = form.right_image_path
            End Sub

            Function Center() As Point
                Return New Point(CInt(form.Location.X + (form.CurrentImageSize().Width / 2)), CInt(Me.form.Location.Y + (Me.form.CurrentImageSize().Height) / 2))
            End Function

        End Class

        Class Game_ScoreBoard
            Friend form As Effect
            Dim graphics As Graphics
            Dim start_point As Point = New Point
            Dim original_image As Image
            Friend Image As Image

            Dim team1 As String = ""
            Dim team2 As String = ""
            Dim team1_score As Integer = 0
            Dim team2_score As Integer = 0

            Sub New(location As String, image_filename As String)
                If Not My.Computer.FileSystem.FileExists(image_filename) Then Throw New FileNotFoundException("File does not exist: " & image_filename)

                form = New Effect(image_filename, image_filename)
                form.Name = "Scoreboard"

                Dim location_parts = Split(location, ",")
                start_point = New Point(Integer.Parse(location_parts(0), CultureInfo.InvariantCulture), Integer.Parse(location_parts(1), CultureInfo.InvariantCulture))

                original_image = Image.FromFile(image_filename)

            End Sub

            Sub Initialize(gamescreen As Screen)
                form.Location = New Point(CInt(start_point.X * 0.01 * gamescreen.WorkingArea.Width + gamescreen.WorkingArea.X), _
                                   CInt(start_point.Y * 0.01 * gamescreen.WorkingArea.Height + gamescreen.WorkingArea.Y))
            End Sub

            Function Center() As Point
                Return New Point(CInt(Me.form.Location.X + (form.CurrentImageSize().Width / 2)), CInt(Me.form.Location.Y + (form.CurrentImageSize().Height) / 2))
            End Function

            Sub SetScore(_team1 As String, _team1_score As Integer, _team2 As String, _team2_score As Integer)
                team1 = _team1
                team1_score = _team1_score
                team2 = _team2
                team2_score = _team2_score
            End Sub

            Sub Paint(screengraphics As Graphics)

                Image = CType(original_image.Clone(), Image)
                graphics = graphics.FromImage(Image)

                Using font As New Font("Arial", 8)
                    TextRenderer.DrawText(graphics, team1, font, New Rectangle(New Point(30, 82), New Size(75, 35)), Color.White)
                    TextRenderer.DrawText(graphics, team2, font, New Rectangle(New Point(30, 126), New Size(75, 35)), Color.White)
                End Using
                Using font As New Font("Arial", 8, FontStyle.Bold)
                    TextRenderer.DrawText(graphics, CStr(team1_score), font, New Rectangle(New Point(95, 85), New Size(75, 35)), Color.White)
                    TextRenderer.DrawText(graphics, CStr(team2_score), font, New Rectangle(New Point(95, 130), New Size(75, 35)), Color.White)
                End Using

                Dim translated_location = Point.Empty

                screengraphics.DrawImageUnscaled(Image, translated_location.X, translated_location.Y)

            End Sub
        End Class

        Class Position
            Friend Name As String
            Friend Team_Number As Integer
            Friend Team As Team
            Friend Player As Pony = Nothing
            Friend Allowed_Area As Rectangle? = Nothing  'nothing means allowed anywhere
            Friend Start_Location As Point
            Friend Current_Action As PlayerActionType = Nothing
            Friend Current_Action_Group As List(Of PlayerActionType) = Nothing
            Friend Required As Boolean

            Dim area_points As String() = Nothing

            Friend Last_Kick_Time As DateTime = DateTime.MinValue

            Friend Hasball As Ball = Nothing

            Friend nearest_ball_distance As Integer = 0

            Friend Selection_Menu_Picturebox As PictureBox = Nothing

            Friend Have_Ball_Actions As New List(Of PlayerActionType)
            Friend Hostile_Ball_Actions As New List(Of PlayerActionType)
            Friend Friendly_Ball_Actions As New List(Of PlayerActionType)
            Friend Neutral_Ball_Actions As New List(Of PlayerActionType)
            Friend Distant_Ball_Actions As New List(Of PlayerActionType)
            Friend No_Ball_Actions As New List(Of PlayerActionType)

            Sub New(_Name As String, _team_number As Integer, _start_location As String, _Allowed_area As String,
                    _Have_Ball_Actions As String, _Hostile_Ball_Actions As String, _Friendly_Ball_Actions As String, _
                    _Neutral_Ball_Actions As String, _Distance_Ball_Actions As String, _No_Ball_Actions As String, _required As String)

                Name = Trim(_Name.Replace(ControlChars.Quote, ""))
                Team_Number = _team_number

                Select Case LCase(Trim(_required))
                    Case "required"
                        Required = True
                    Case "optional"
                        Required = False
                    Case Else
                        Throw New ArgumentException("Invalid entry for required/optional setting of position " & Name & ". ")
                End Select

                Dim start_points = Split(_start_location, ",")
                Start_Location = New Point(
                    Integer.Parse(start_points(0), CultureInfo.InvariantCulture),
                    Integer.Parse(start_points(1), CultureInfo.InvariantCulture))

                If LCase(Trim(_Allowed_area)) <> "any" Then
                    area_points = Split(_Allowed_area, ",")
                Else
                    Allowed_Area = Nothing
                End If

                Dim Action_Lists As New List(Of List(Of PlayerActionType))
                Action_Lists.Add(Have_Ball_Actions)
                Action_Lists.Add(Hostile_Ball_Actions)
                Action_Lists.Add(Friendly_Ball_Actions)
                Action_Lists.Add(Neutral_Ball_Actions)
                Action_Lists.Add(Distant_Ball_Actions)
                Action_Lists.Add(No_Ball_Actions)

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

            Sub Initialize(gamescreen As Screen)
                If Not IsNothing(area_points) Then
                    Allowed_Area = New Rectangle(
                        CInt(Double.Parse(area_points(0), CultureInfo.InvariantCulture) * 0.01 * gamescreen.WorkingArea.Width + gamescreen.WorkingArea.X),
                        CInt(Double.Parse(area_points(1), CultureInfo.InvariantCulture) * 0.01 * gamescreen.WorkingArea.Height + gamescreen.WorkingArea.Y),
                        CInt(Double.Parse(area_points(2), CultureInfo.InvariantCulture) * 0.01 * gamescreen.WorkingArea.Width),
                        CInt(Double.Parse(area_points(3), CultureInfo.InvariantCulture) * 0.01 * gamescreen.WorkingArea.Height))
                End If
            End Sub

            Sub Decide_On_Action(game As Game)

                Dim nearest_ball = get_nearest_ball(game.Active_Balls)

                If IsNothing(nearest_ball) Then
                    PerformAction(No_Ball_Actions, Nothing)
                    Exit Sub
                End If

                Dim screen_diagonal = Math.Sqrt((game.GameScreen.WorkingArea.Height) ^ 2 + (game.GameScreen.WorkingArea.Width) ^ 2)
                If nearest_ball_distance > screen_diagonal * (1 / 2) Then 'AndAlso _
                    PerformAction(Distant_Ball_Actions, nearest_ball)
                    Exit Sub
                End If

                If nearest_ball_distance <= (Player.CurrentImageSize.X / 2) + 50 Then ' / 2 Then
                    PerformAction(Have_Ball_Actions, nearest_ball)
                    Exit Sub
                End If

                Dim BallOwner_Team = game.Get_Ball_LastHandler_Team(nearest_ball)

                If IsNothing(BallOwner_Team) Then
                    PerformAction(Neutral_Ball_Actions, nearest_ball)
                    Exit Sub
                End If

                If BallOwner_Team.Number = Me.Team_Number Then
                    PerformAction(Friendly_Ball_Actions, nearest_ball)
                    Exit Sub
                Else
                    PerformAction(Hostile_Ball_Actions, nearest_ball)
                    Exit Sub
                End If

            End Sub

            Sub PerformAction(action_list As List(Of PlayerActionType), ball As Ball)

                If Not IsNothing(Current_Action_Group) Then ' AndAlso Not ReferenceEquals(Current_Action_Group, Have_Ball_Actions) Then
                    If ReferenceEquals(action_list, Current_Action_Group) Then
                        'we are already doing an action from this list

                        'if we recently kicked the ball, don't do it for 2 seconds.
                        If ReferenceEquals(Current_Action_Group, Have_Ball_Actions) Then
                            If DateDiff(DateInterval.Second, Last_Kick_Time, DateTime.UtcNow) <= 2 Then
                                If DateDiff(DateInterval.Second, Last_Kick_Time, DateTime.UtcNow) > 1 AndAlso (Player.ManualControlPlayerOne OrElse Player.ManualControlPlayerTwo) Then
                                    Speak("Can't kick again so soon!")
                                End If

                                Exit Sub
                            End If
                        Else
                            'if it was any other action, don't reset it.
                            Exit Sub
                        End If
                    Else
                        Current_Action_Group = Nothing
                    End If
                End If

                Dim selection As Integer = Rng.Next(action_list.Count)

                Dim selected_action As PlayerActionType = action_list(selection)

                Select Case selected_action

                    Case PlayerActionType.Not_Set
                        Throw New Exception("Can't do this action (reserved): " & selected_action)
                    Case PlayerActionType.Return_To_Start
                        SetFollowBehavior(Nothing, Nothing, True)
                    Case PlayerActionType.Chase_Ball
                        SetFollowBehavior(ball.Handler.Directory, ball.Handler)
                    Case PlayerActionType.Lead_Ball
                        SetFollowBehavior(ball.Handler.Directory, ball.Handler, False, True)
                    Case PlayerActionType.Avoid_Ball
                        Throw New NotImplementedException("Not implemented yet: action type " & selected_action)
                    Case PlayerActionType.Throw_Ball_ToGoal
                        If DateDiff(DateInterval.Second, Last_Kick_Time, DateTime.UtcNow) <= 2 Then
                            'can't kick again so soon
                            Exit Sub
                        End If
                        'If ponies are being controlled, don't kick unless the action key (control - left or right) is being pushed
                        If Player.ManualControlPlayerOne AndAlso Not Main.Instance.PonyAction Then Exit Sub
                        If Player.ManualControlPlayerTwo AndAlso Not Main.Instance.PonyAction_2 Then Exit Sub
                        Kick_Ball(ball, 10, Get_OtherTeam_Goal(), Nothing, Me, "*Kick*!")
                        Last_Kick_Time = DateTime.UtcNow
                    Case PlayerActionType.Throw_Ball_ToTeammate
                        If DateDiff(DateInterval.Second, Last_Kick_Time, DateTime.UtcNow) <= 2 Then
                            'can't kick again so soon
                            Exit Sub
                        End If

                        'don't pass if we are under control - kick instead.
                        If Player.ManualControlPlayerOne Then Exit Sub
                        If Player.ManualControlPlayerTwo Then Exit Sub

                        Dim open_teammate = Get_Open_Teammate(Me.Team, Get_OtherTeam_Goal())
                        If IsNothing(open_teammate) Then
                            'no teammates to pass to, kick to goal instead, unless a player controlled pony.
                            If Player.ManualControlPlayerOne OrElse Player.ManualControlPlayerTwo Then Exit Sub
                            Kick_Ball(ball, 10, Get_OtherTeam_Goal(), Nothing, Me, "*Kick*!")
                            Last_Kick_Time = DateTime.UtcNow
                            Exit Sub
                        End If

                        Kick_Ball(ball, 10, Nothing, open_teammate, Me, "*Pass*!")
                        Last_Kick_Time = DateTime.UtcNow

                    Case PlayerActionType.Throw_Ball_Reflect
                        If DateDiff(DateInterval.Second, Last_Kick_Time, DateTime.UtcNow) <= 2 Then
                            'can't kick again so soon
                            Exit Sub
                        End If

                        If Player.ManualControlPlayerOne AndAlso Not Main.Instance.PonyAction Then Exit Sub
                        If Player.ManualControlPlayerTwo AndAlso Not Main.Instance.PonyAction_2 Then Exit Sub

                        Bounce_Ball(ball, 7, Me, "*Ping*!")
                        Last_Kick_Time = DateTime.UtcNow

                    Case PlayerActionType.Approach_Own_Goal
                        Dim goal = Get_Team_Goal()
                        SetFollowBehavior(goal.form.Name, goal.form)

                    Case PlayerActionType.Approach_Target_Goal
                        Dim goal = Get_OtherTeam_Goal()
                        SetFollowBehavior(goal.form.Name, goal.form)

                    Case PlayerActionType.Idle
                        If Current_Action = PlayerActionType.Idle Then Exit Sub
                        Player.followObject = Nothing
                        Player.followObjectName = ""

                        If Player.ManualControlPlayerOne Then Exit Sub
                        If Player.ManualControlPlayerTwo Then Exit Sub

                        Player.SelectBehavior()
                        SetSpeed()

                    Case Else
                        Pony.CurrentAnimator.Pause(False)
                        MsgBox("Invalid action type: " & selected_action)
                        Throw New Exception("Invalid action type: " & selected_action)
                End Select

                Current_Action = selected_action
                Current_Action_Group = action_list

            End Sub

            Function get_nearest_ball(balls As List(Of Ball)) As Ball
                Dim nearest_ball As Ball = Nothing
                Dim nearest_ball_distance As Double = Double.MaxValue

                For Each Ball In balls
                    Dim distance = Vector.Distance(Player.CenterLocation, Ball.Center)
                    If distance < nearest_ball_distance Then
                        nearest_ball_distance = distance
                        nearest_ball = Ball
                    End If
                Next

                If IsNothing(nearest_ball) Then Throw New Exception("No available balls found when checking distance!")

                Me.nearest_ball_distance = CInt(nearest_ball_distance)
                Return nearest_ball

            End Function

            Function Get_OtherTeam_Goal() As Goal_Area

                For Each goal In Main.Instance.CurrentGame.Goals
                    If goal.team_number <> Me.Team_Number Then
                        Return goal
                    End If
                Next

                Throw New Exception("Couldn't find a goal for another team.")

            End Function

            Function Get_Team_Goal() As Goal_Area

                For Each goal In Main.Instance.CurrentGame.Goals
                    If goal.team_number = Me.Team_Number Then
                        Return goal
                    End If
                Next

                Throw New Exception("Couldn't find a goal for pony's team.")

            End Function

            Sub SetFollowBehavior(target_name As String, target As ISprite, Optional return_to_start As Boolean = False, Optional lead_target As Boolean = False)

                SetSpeed()

                Player.CurrentBehavior = Player.GetAppropriateBehaviorOrCurrent(Pony.AllowedMoves.All, True)
                'Player.CurrentBehavior = Player.GetAppropriateBehaviorForSpeed()
                Player.followObject = Nothing
                Player.followObjectName = ""

                If return_to_start Then
                    Player.destinationCoords = Start_Location
                    Exit Sub
                Else
                    Player.followObjectName = target_name
                    Player.followObject = target
                    Player.destinationCoords = Point.Empty
                    If lead_target Then
                        Player.leadTarget = True
                    End If

                    'If Main.Instance.current_game.Name = "Ping Pong Pony" Then
                    '    Player.current_behavior.speed = Player.current_behavior.original_speed * 1.5
                    'End If
                End If
            End Sub

            Sub Kick_Ball(ball As Ball, speed As Double, target_goal As Goal_Area, target_pony As Pony, kicker As Position, line As String)

                If Rng.NextDouble() < 0.05 Then
                    Speak("Missed!")
                    Exit Sub
                End If

                speed = kicker.Player.Scale * speed

                Speak(line)

                Dim angle As Double = Nothing

                If IsNothing(target_goal) Then
                    angle = Get_Angle_To_Object(target_pony.CenterLocation)
                Else
                    angle = Get_Angle_To_Object(target_goal.Center)
                End If


                'add a bit of inaccuracy
                If Rng.NextDouble() < 0.5 Then
                    angle += Rng.NextDouble() * (Math.PI / 8)
                Else
                    angle -= Rng.NextDouble() * (Math.PI / 8)
                End If

                ball.Kick(speed, angle, kicker)

            End Sub

            Sub Bounce_Ball(ball As Ball, speed As Double, kicker As Position, line As String)

                If Main.Instance.CurrentGame.Name = "Ping Pong Pony" Then
                    'avoid boucing the ball back into our own goal.
                    If Not IsNothing(ball.Last_Handled_By) AndAlso ReferenceEquals(ball.Last_Handled_By, Me) Then
                        Exit Sub
                    End If
                End If

                Speak(line)

                Dim angle As Double
                Dim gamescreen = Main.Instance.CurrentGame.GameScreen

                If ball.Handler.diagonal < (Math.PI / 2) OrElse ball.Handler.diagonal > (3 / 2) * Math.PI Then
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
            Function Get_Angle_To_Object(target As Point) As Double

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

            Sub Speak(line As String)
                Dim new_line As New PonyBase.Behavior.SpeakingLine(Player.Name, "Kick", line, "", "", True, 0)
                Player.PonySpeak(new_line)
            End Sub

            Sub PushBackOverlappingPonies(all_positions As List(Of Position))

                For Each otherposition As Position In all_positions
                    Dim otherpony = otherposition.Player
                    If ReferenceEquals(Me.Player, otherpony) Then Continue For


                    If DoesPonyOverlap(Me.Player, otherpony) Then
                        'Push overlapping ponies a bit apart
                        PonyPush(Me.Player, otherpony, Allowed_Area)
                        Exit Sub
                    End If


                Next

            End Sub

            Shared Function DoesPonyOverlap(pony As Pony, otherpony As Pony) As Boolean

                Dim otherpony_area As New Rectangle(otherpony.TopLeftLocation.X, _
                                                 otherpony.TopLeftLocation.Y, _
                                                 CInt(otherpony.CurrentImageSize.X * otherpony.Scale),
                                                 CInt(otherpony.CurrentImageSize.Y * otherpony.Scale))


                If otherpony_area.IntersectsWith(New Rectangle(pony.TopLeftLocation, New Size(CInt(pony.Scale * pony.CurrentImageSize.X), _
                                                                                        CInt(pony.Scale * pony.CurrentImageSize.Y)))) Then
                    Return True
                Else
                    Return False
                End If

            End Function

            Shared Sub PonyPush(pony1 As Pony, pony2 As Pony, allowed_area As Rectangle?)

                Dim xchange = 1
                Dim ychange = 2

                Dim direction = pony1.GetDestinationDirections(pony2.CenterLocation)

                If direction(0) = Directions.left Then
                    xchange = -1
                End If
                If direction(1) = Directions.top Then
                    ychange = -2
                End If

                Dim change = New Size(xchange, ychange)
                Dim new_location = pony1.TopLeftLocation + change

                Dim screenlist As New List(Of Screen)
                screenlist.Add(Main.Instance.CurrentGame.GameScreen)

                If pony1.IsPonyOnScreen(new_location, screenlist) AndAlso
                    (Not allowed_area.HasValue OrElse Pony.IsPonyInBox(new_location, allowed_area.Value)) Then
                    pony1.TopLeftLocation = new_location
                End If

            End Sub

            Shared Function Friendly_Ponies_Around_Ball(ball As Ball, team As Team, min_distance As Integer) As List(Of Pony)

                Dim ponies As New List(Of Pony)

                For Each Position In team.Positions
                    Dim distance = Vector.Distance(Position.Player.CenterLocation, ball.Center)
                    If distance <= min_distance Then
                        ponies.Add(Position.Player)
                    End If
                Next

                Return ponies
            End Function

            'get a teammate that is not near any enemy players and is closer to the goal than we are.
            Function Get_Open_Teammate(team As Team, goal As Goal_Area) As Pony

                Dim open_teammates As New List(Of Pony)

                For Each Position In team.Positions

                    If ReferenceEquals(Position, Me) Then
                        Continue For
                    End If

                    Dim open = True
                    For Each other_position As Position In Main.Instance.CurrentGame.AllPlayers
                        If other_position.Team.Name = Me.Team.Name Then
                            Continue For
                        End If

                        Dim distance = Vector.Distance(Position.Player.CenterLocation, other_position.Player.CenterLocation)
                        If distance <= 200 Then
                            open = False
                        End If
                    Next

                    Dim me_distance_to_goal = Vector.Distance(Player.CenterLocation, goal.Center)
                    Dim teammate_distance_to_goal = Vector.Distance(Position.Player.CenterLocation, goal.Center)

                    If open = True AndAlso teammate_distance_to_goal <= me_distance_to_goal Then
                        open_teammates.Add(Position.Player)
                    End If
                Next

                If open_teammates.Count = 0 Then Return Nothing

                Return open_teammates(Rng.Next(open_teammates.Count))

            End Function

            Sub SetSpeed()
                Dim speed = If(Main.Instance.CurrentGame.Name = "Ping Pong Pony", 8 * Player.Scale, 5 * Player.Scale)
                If Player.AtDestination Then speed = 0
                Player.CurrentBehavior.SetSpeed(speed)
            End Sub

        End Class
    End Class
End Module