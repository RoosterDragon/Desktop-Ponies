Imports System.Globalization

Public Class GameSelectionForm

    Dim team1 As Game.Team = Nothing
    Dim team2 As Game.Team = Nothing
    Dim game As Game = Nothing

    Public Sub New()
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub GameSelectionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'add all possible ponies to the selection window.

        ScreenSelection_Box.Items.Clear()

        For Each monitor In Screen.AllScreens
            ScreenSelection_Box.Items.Add(monitor.DeviceName)
        Next

        If ScreenSelection_Box.Items.Count <> 0 Then
            ScreenSelection_Box.SelectedIndex = 0
        End If

        Pony_Selection_View.Items.Clear()

        Team1_Panel.Controls.Clear()
        Team2_Panel.Controls.Clear()

        Dim pony_image_list As New ImageList()
        pony_image_list.ImageSize = New Size(75, 75)

        For Each ponyPanel As PonySelectionControl In Main.Instance.PonySelectionPanel.Controls
            pony_image_list.Images.Add(CType(ponyPanel.GetPonyImage(0).Image.Clone(), Bitmap))
        Next

        Pony_Selection_View.LargeImageList = pony_image_list
        Pony_Selection_View.SmallImageList = pony_image_list

        Dim ponycount As Integer = 0
        For Each Pony In Main.Instance.SelectablePonies
            Pony_Selection_View.Items.Add(New ListViewItem(Pony.Directory, ponycount))
            ponycount += 1
        Next

        Pony_Selection_View.Columns.Add("Pony")


        'do the same for the game list
        Game_Selection_View.Items.Clear()

        Dim game_image_list As New ImageList()
        game_image_list.ImageSize = New Size(75, 75)

        For Each game As Game In Main.Instance.games
            game_image_list.Images.Add(Image.FromFile(game.Balls(0).Handler.Behaviors(0).RightImagePath))
        Next

        Game_Selection_View.LargeImageList = game_image_list
        Game_Selection_View.SmallImageList = game_image_list

        Dim gamecount As Integer = 0
        For Each game As Game In Main.Instance.games
            Game_Selection_View.Items.Add(New ListViewItem(game.Name, gamecount))
            gamecount += 1
        Next

        Game_Selection_View.Columns.Add("Game")

    End Sub

    Private Sub Game_Selection_View_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Game_Selection_View.SelectedIndexChanged

        If Game_Selection_View.SelectedIndices.Count = 0 Then Exit Sub

        If Not IsNothing(game) AndAlso game.Name <> Main.Instance.games(Game_Selection_View.SelectedIndices(0)).Name Then
            Team1_Panel.Controls.Clear()
            Team2_Panel.Controls.Clear()
            For Each position In team1.Positions
                position.Player = Nothing
            Next
            For Each position In team2.Positions
                position.Player = Nothing
            Next
            team1 = Nothing
            team2 = Nothing
        End If

        game = Main.Instance.games(Game_Selection_View.SelectedIndices(0))

        team1 = game.Teams(0)
        team2 = game.Teams(1)

        Team1_Label.Text = team1.Name
        Team2_Label.Text = team2.Name

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
                picturebox.Image = My.Resources.MysteryThumb
                picturebox.Size = picturebox.Image.Size

                AddHandler picturebox.Click, AddressOf PictureClick

                Dim new_label As New Label
                new_label.Text = Position.Name

                Select Case Position.TeamNumber
                    Case 1
                        Team1_Panel.Controls.Add(picturebox)
                        Team1_Panel.Controls.Add(new_label)
                        picturebox.Location = New Point(0, last_y_team1)
                        new_label.Location = New Point(10, picturebox.Location.Y + picturebox.Size.Height)
                        last_y_team1 += picturebox.Size.Height + new_label.Size.Height + 10
                    Case 2
                        Team2_Panel.Controls.Add(picturebox)
                        Team2_Panel.Controls.Add(new_label)
                        picturebox.Location = New Point(0, last_y_team2)
                        new_label.Location = New Point(10, picturebox.Location.Y + picturebox.Size.Height)
                        last_y_team2 += picturebox.Size.Height + new_label.Size.Height + 10
                    Case Else
                        Throw New NotImplementedException("Games with more than one team can not be used yet - Not implemented.  Unable to use game: " & game.Name)

                End Select

            Next
        Next

        Prompt_Label.Text = "Add ponies until all required slots are filled..."

    End Sub

    Private Sub Add_To_Team1_Button_Click(sender As Object, e As EventArgs) Handles Add_Team1_Button.Click
        AddToPanel(Team1_Panel, 1)
    End Sub

    Private Sub Add_To_Team2_Button_Click(sender As Object, e As EventArgs) Handles Add_Team2_Button.Click
        AddToPanel(Team2_Panel, 2)
    End Sub

    Private Sub AddToPanel(panel As Panel, team As Integer)

        If Pony_Selection_View.SelectedIndices.Count = 0 Then
            MessageBox.Show(Me, "Select a pony by clicking on its picture first.",
                            "No Pony Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim selection As Integer = Pony_Selection_View.SelectedIndices(0)

        If Main.Instance.SelectablePonies(Pony_Selection_View.SelectedIndices(0)).Directory = "Random Pony" Then

            Do Until Main.Instance.SelectablePonies(selection).Directory <> "Random Pony"
                selection = Rng.Next(Main.Instance.SelectablePonies.Count)
            Loop

        End If
        Dim pony = New Pony(Main.Instance.SelectablePonies(selection))

        Dim empty_spot_found = False

        Dim position_picturebox As PictureBox = Nothing

        For Each Control In panel.Controls
            If Control.GetType Is GetType(PictureBox) Then
                Dim picturebox As PictureBox = CType(Control, Windows.Forms.PictureBox)
                If picturebox.Name = "NotSet" Then
                    picturebox.Name = pony.Directory
                    Select Case team
                        Case 1
                            picturebox.Image = Image.FromFile(pony.Behaviors(0).RightImagePath)
                        Case 2
                            picturebox.Image = Image.FromFile(pony.Behaviors(0).LeftImagePath)
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
        pony_picturebox.Image = My.Resources.MysteryThumb

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

    Sub GetSpotCounts()

        Dim required_spots = 0
        Dim optional_spots = 0
        For Each position In team1.Positions
            If position.Required = True AndAlso IsNothing(position.Player) Then
                required_spots += 1
            ElseIf IsNothing(position.Player) Then
                optional_spots += 1
            End If
        Next

        team1_requiredleft_label.Text = CStr(required_spots)
        team1_spotsleft_label.Text = CStr(optional_spots)

        required_spots = 0
        optional_spots = 0
        For Each position In team2.Positions
            If position.Required = True AndAlso IsNothing(position.Player) Then
                required_spots += 1
            ElseIf IsNothing(position.Player) Then
                optional_spots += 1
            End If
        Next

        team2_requiredleft_label.Text = CStr(required_spots)
        team2_spotsleft_label.Text = CStr(optional_spots)

        If Double.Parse(team1_requiredleft_label.Text, CultureInfo.InvariantCulture) = 0 AndAlso
            Double.Parse(team2_requiredleft_label.Text, CultureInfo.InvariantCulture) = 0 Then
            Prompt_Label.Text = "You can play the game now!"
            Prompt_Label.ForeColor = Color.ForestGreen
        Else
            Prompt_Label.Text = "Add ponies until all required slots are filled..."
            Prompt_Label.ForeColor = Color.RoyalBlue
        End If

    End Sub

    Private Sub Play_Button_Click(sender As Object, e As EventArgs) Handles Play_Button.Click

        If IsNothing(game) Then
            MessageBox.Show(Me, "Select a game first!", "No Game Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If ScreenSelection_Box.SelectedItems.Count = 0 Then
            MessageBox.Show(Me, "You need to select a monitor to play the game on.",
                            "No Monitor Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If Double.Parse(team1_requiredleft_label.Text, CultureInfo.InvariantCulture) > 0 OrElse
            Double.Parse(team2_requiredleft_label.Text, CultureInfo.InvariantCulture) > 0 Then
            MessageBox.Show(Me, "You must fill each required position with a pony before you can start the game.",
                            "Insufficient Positions Filled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        game.GameScreen = Screen.AllScreens(ScreenSelection_Box.SelectedIndex)

        Me.DialogResult = DialogResult.OK
        Main.Instance.CurrentGame = game
    End Sub

    Private Sub Info_Click(sender As Object, e As EventArgs) Handles Info_Button.Click
        If IsNothing(game) Then
            MessageBox.Show(Me, "Select a game first!", "No Game Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show(Me, game.Description, game.Name, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class