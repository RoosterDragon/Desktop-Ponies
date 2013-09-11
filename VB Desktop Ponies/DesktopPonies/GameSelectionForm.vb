Imports System.Globalization

Public Class GameSelectionForm
    Private games As IList(Of Game)
    Private game As Game
    Private team1 As Game.Team
    Private team2 As Game.Team
    Private ReadOnly ponies As PonyCollection

    Public Sub New(collection As PonyCollection)
        InitializeComponent()
        Icon = My.Resources.Twilight
        ponies = Argument.EnsureNotNull(collection, "collection")
    End Sub

    Private Sub GameSelectionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        Enabled = False
        Update()
        Application.DoEvents()

        Dim gameDirectories = IO.Directory.GetDirectories(IO.Path.Combine(Options.InstallLocation, game.RootDirectory))
        games = New List(Of Game)(gameDirectories.Length)
        For Each gameDirectory In gameDirectories
            Try
                games.Add(New Game(ponies, gameDirectory))
            Catch ex As Exception
                My.Application.NotifyUserOfNonFatalException(ex, "Error loading game: " & gameDirectory)
            End Try
        Next

        'add all possible ponies to the selection window.

        MonitorComboBox.Items.AddRange(Screen.AllScreens.Select(Function(screen) screen.DeviceName).ToArray())
        If MonitorComboBox.Items.Count > 0 Then
            MonitorComboBox.SelectedIndex = 0
        End If

        Dim ponyImageList = PonyEditor.GenerateImageList(ponies.Bases, 75, PonyList.BackColor, Function(b) b.RightImage.Path)
        PonyList.LargeImageList = ponyImageList
        PonyList.SmallImageList = ponyImageList

        Dim ponycount As Integer = 0
        PonyList.Items.Add(New ListViewItem(ponies.RandomBase.Directory, ponycount))
        ponycount += 1
        For Each ponyBase In ponies.Bases
            PonyList.Items.Add(New ListViewItem(ponyBase.Directory, ponycount))
            ponycount += 1
        Next

        PonyList.Columns.Add("Pony")

        'do the same for the game list
        GameList.Items.Clear()

        Dim gameImageList As New ImageList() With {.ImageSize = New Size(75, 75)}
        For Each game As Game In games
            gameImageList.Images.Add(Image.FromFile(game.Balls(0).Handler.Behaviors(0).RightImage.Path))
        Next

        GameList.LargeImageList = gameImageList
        GameList.SmallImageList = gameImageList

        Dim gamecount As Integer = 0
        For Each game As Game In games
            GameList.Items.Add(New ListViewItem(game.Name, gamecount))
            gamecount += 1
        Next

        GameList.Columns.Add("Game")

        SetStage(1)

        Enabled = True
    End Sub

    Private Sub GameList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GameList.SelectedIndexChanged

        If GameList.SelectedIndices.Count = 0 Then Exit Sub

        If Not IsNothing(game) AndAlso game.Name <> games(GameList.SelectedIndices(0)).Name Then
            GameTeam1.TeamPanel.Controls.Clear()
            GameTeam2.TeamPanel.Controls.Clear()
            For Each position In team1.Positions
                position.Player = Nothing
            Next
            For Each position In team2.Positions
                position.Player = Nothing
            Next
            team1 = Nothing
            team2 = Nothing
        End If

        game = games(GameList.SelectedIndices(0))

        GameDescriptionLabel.Text = game.Name & ": " & game.Description

        team1 = game.Teams(0)
        team2 = game.Teams(1)

        GameTeam1.TeamNameLabel.Text = team1.Name
        GameTeam2.TeamNameLabel.Text = team2.Name

        GetSpotCounts()

        Dim last_y_team1 As Integer = 0
        Dim last_y_team2 As Integer = 0

        For Each team As Game.Team In game.Teams
            For Each Position As Game.Position In team.Positions

                Position.Player = Nothing

                Dim picturebox As New PictureBox

                Position.SelectionMenuPictureBox = picturebox

                picturebox.SizeMode = PictureBoxSizeMode.StretchImage
                picturebox.Name = "NotSet"
                picturebox.Image = My.Resources.RandomPony
                picturebox.Size = picturebox.Image.Size

                AddHandler picturebox.Click, AddressOf PictureClick

                Dim new_label As New Label
                new_label.Text = Position.Name

                Select Case Position.TeamNumber
                    Case 1
                        GameTeam1.TeamPanel.Controls.Add(picturebox)
                        GameTeam1.TeamPanel.Controls.Add(new_label)
                        picturebox.Location = New Point(0, last_y_team1)
                        new_label.Location = New Point(10, picturebox.Location.Y + picturebox.Size.Height)
                        last_y_team1 += picturebox.Size.Height + new_label.Size.Height + 10
                    Case 2
                        GameTeam2.TeamPanel.Controls.Add(picturebox)
                        GameTeam2.TeamPanel.Controls.Add(new_label)
                        picturebox.Location = New Point(0, last_y_team2)
                        new_label.Location = New Point(10, picturebox.Location.Y + picturebox.Size.Height)
                        last_y_team2 += picturebox.Size.Height + new_label.Size.Height + 10
                    Case Else
                        Throw New NotImplementedException("Games with more than one team can not be used yet - Not implemented.  Unable to use game: " & game.Name)

                End Select

            Next
        Next

        SetStage(2)

    End Sub

    Private Sub Team1AddButton_Click(sender As Object, e As EventArgs) Handles Team1AddButton.Click
        AddToPanel(GameTeam1.TeamPanel, 1)
    End Sub

    Private Sub Team2AddButton_Click(sender As Object, e As EventArgs) Handles Team2AddButton.Click
        AddToPanel(GameTeam2.TeamPanel, 2)
    End Sub

    Private Sub AddToPanel(panel As Panel, team As Integer)

        If PonyList.SelectedIndices.Count = 0 Then
            MessageBox.Show(Me, "Select a pony by clicking on its picture first.",
                            "No Pony Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim selection As Integer = PonyList.SelectedIndices(0)
        Do While ponies.Bases(selection).Directory = "Random Pony"
            selection = Rng.Next(ponies.Bases.Count)
        Loop
        Dim pony = New Pony(ponies.Bases(selection))

        Dim empty_spot_found = False

        Dim position_picturebox As PictureBox = Nothing

        For Each Control In panel.Controls
            If Control.GetType Is GetType(PictureBox) Then
                Dim picturebox As PictureBox = CType(Control, Windows.Forms.PictureBox)
                If picturebox.Name = "NotSet" Then
                    picturebox.Name = pony.Directory
                    Select Case team
                        Case 1
                            picturebox.Image = Image.FromFile(pony.Behaviors(0).RightImage.Path)
                        Case 2
                            picturebox.Image = Image.FromFile(pony.Behaviors(0).LeftImage.Path)
                    End Select
                    empty_spot_found = True
                    position_picturebox = picturebox
                    Exit For
                End If
            End If
        Next

        If empty_spot_found = False Then
            If IsNothing(game) Then
                MessageBox.Show(Me, "Select a game first!", "No Game Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show(Me, "Team is full!", "Team Full", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            Select Case team
                Case 1
                    For Each position In team1.Positions
                        If ReferenceEquals(position.SelectionMenuPictureBox, position_picturebox) Then
                            position.Player = pony
                            position.Team = team1
                            Exit For
                        End If
                    Next
                Case 2
                    For Each position In team2.Positions
                        If ReferenceEquals(position.SelectionMenuPictureBox, position_picturebox) Then
                            position.Player = pony
                            position.Team = team2
                            Exit For
                        End If
                    Next
            End Select
            GetSpotCounts()
        End If

    End Sub

    Private Sub PictureClick(sender As Object, e As EventArgs)

        Dim pony_picturebox As PictureBox = CType(sender, PictureBox)

        pony_picturebox.Name = "NotSet"
        pony_picturebox.Image = My.Resources.RandomPony

        For Each position As Game.Position In team1.Positions
            If ReferenceEquals(position.SelectionMenuPictureBox, pony_picturebox) Then
                position.Player = Nothing
                Exit For
            End If
        Next
        For Each position As Game.Position In team2.Positions
            If ReferenceEquals(position.SelectionMenuPictureBox, pony_picturebox) Then
                position.Player = Nothing
                Exit For
            End If
        Next

        GetSpotCounts()

    End Sub

    Private Sub GetSpotCounts()

        Dim required_spots1 = 0
        Dim optional_spots1 = 0
        For Each position In team1.Positions
            If position.Required = True AndAlso IsNothing(position.Player) Then
                required_spots1 += 1
            ElseIf IsNothing(position.Player) Then
                optional_spots1 += 1
            End If
        Next

        GameTeam1.RequiredSpacesLeftCountLabel.Text = required_spots1.ToString(CultureInfo.CurrentCulture)
        GameTeam1.SpacesLeftCountLabel.Text = optional_spots1.ToString(CultureInfo.CurrentCulture)

        Dim required_spots2 = 0
        Dim optional_spots2 = 0
        For Each position In team2.Positions
            If position.Required = True AndAlso IsNothing(position.Player) Then
                required_spots2 += 1
            ElseIf IsNothing(position.Player) Then
                optional_spots2 += 1
            End If
        Next

        GameTeam2.RequiredSpacesLeftCountLabel.Text = required_spots2.ToString(CultureInfo.CurrentCulture)
        GameTeam2.SpacesLeftCountLabel.Text = optional_spots2.ToString(CultureInfo.CurrentCulture)

        If required_spots1 = 0 AndAlso required_spots2 = 0 Then
            SetStage(3)
        Else
            SetStage(2)
        End If

    End Sub

    Private Sub PlayButton_Click(sender As Object, e As EventArgs) Handles PlayButton.Click

        If IsNothing(game) Then
            MessageBox.Show(Me, "Select a game first!", "No Game Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If MonitorComboBox.SelectedIndex = -1 Then
            MessageBox.Show(Me, "You need to select a monitor to play the game on.",
                            "No Monitor Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If Integer.Parse(GameTeam1.RequiredSpacesLeftCountLabel.Text, CultureInfo.InvariantCulture) > 0 OrElse
            Integer.Parse(GameTeam2.RequiredSpacesLeftCountLabel.Text, CultureInfo.InvariantCulture) > 0 Then
            MessageBox.Show(Me, "You must fill each required position with a pony before you can start the game.",
                            "Insufficient Positions Filled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        game.GameScreen = Screen.AllScreens(MonitorComboBox.SelectedIndex)

        Me.DialogResult = DialogResult.OK
        Game.CurrentGame = game
    End Sub

    Private Sub SetStage(stage As Byte)
        Dim inactiveColor = Color.RoyalBlue
        Dim activeColor = Color.Orange
        Dim highlightColor = Color.Gold

        SelectGameLabel.ForeColor = inactiveColor
        Transition1Label.ForeColor = inactiveColor
        SelectPlayersLabel.ForeColor = inactiveColor
        Transition2Label.ForeColor = inactiveColor
        StartLabel.ForeColor = inactiveColor
        Transition3Label.ForeColor = inactiveColor

        If stage >= 1 Then
            SelectGameLabel.ForeColor = If(stage = 1, highlightColor, activeColor)
            Transition1Label.ForeColor = activeColor
        End If

        If stage >= 2 Then
            SelectPlayersLabel.ForeColor = If(stage = 2, highlightColor, activeColor)
            Transition2Label.ForeColor = activeColor
        End If

        If stage >= 3 Then
            StartLabel.ForeColor = If(stage = 3, highlightColor, activeColor)
            Transition3Label.ForeColor = activeColor
        End If

    End Sub
End Class