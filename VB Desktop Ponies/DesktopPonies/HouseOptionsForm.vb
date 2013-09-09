Imports System.Globalization
Imports System.IO

Public Class HouseOptionsForm

    Dim house As House
    Dim houseImage As Image
    Dim doorLocation As Point

    Friend Sub New(_house As House, ponyBases As IEnumerable(Of PonyBase))
        InitializeComponent()
        Icon = My.Resources.Twilight

        house = _house
        Dim base = house.HouseBase

        Text = base.Name + " - House Options - Desktop Ponies"

        doorLocation = base.DoorPosition
        Cycle_Counter.Value = CDec(base.CycleInterval.TotalSeconds)
        MinSpawn_Counter.Value = base.MinimumPonies
        MaxSpawn_Counter.Value = base.MaximumPonies
        Bias_TrackBar.Value = CInt(base.Bias * 10)

        Try
            houseImage = Image.FromFile(base.LeftImage.Path)
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Couldn't open the image file for the house.")
            Exit Sub
        End Try
        DoorLocation_Label.Text = doorLocation.ToString()

        Visitors_CheckedListBox.Items.Clear()
        For Each selectablePony In ponyBases
            Visitors_CheckedListBox.Items.Add(selectablePony.Directory)
        Next

        Dim ponies_to_check As New List(Of Integer)

        SyncLock base.Visitors
            For Each visitor In base.Visitors
                Dim index = 0

                If String.Equals("all", visitor, StringComparison.OrdinalIgnoreCase) Then
                    For i = 0 To Visitors_CheckedListBox.Items.Count - 1
                        Visitors_CheckedListBox.SetItemChecked(i, True)
                    Next
                    Exit For
                End If

                For Each item As String In Visitors_CheckedListBox.Items
                    If String.Equals(item, visitor, StringComparison.OrdinalIgnoreCase) Then
                        ponies_to_check.Add(index)
                    End If
                    index += 1
                Next
            Next
        End SyncLock

        For Each index In ponies_to_check
            Visitors_CheckedListBox.SetItemChecked(index, True)
        Next
    End Sub

    Private Sub MinSpawn_Counter_ValueChanged(sender As Object, e As EventArgs) Handles MinSpawn_Counter.ValueChanged
        MaxSpawn_Counter.Minimum = MinSpawn_Counter.Value
        If MaxSpawn_Counter.Value < MinSpawn_Counter.Value Then
            MaxSpawn_Counter.Value = MinSpawn_Counter.Value
        End If
    End Sub

    Private Sub MaxSpawn_Counter_ValueChanged(sender As Object, e As EventArgs) Handles MaxSpawn_Counter.ValueChanged
        MinSpawn_Counter.Maximum = MaxSpawn_Counter.Value
        If MinSpawn_Counter.Value > MaxSpawn_Counter.Value Then
            MinSpawn_Counter.Value = MaxSpawn_Counter.Value
        End If
    End Sub

    Private Sub ClearVisitors_Button_Click(sender As Object, e As EventArgs) Handles ClearVisitors_Button.Click
        If Visitors_CheckedListBox.Items.Count = 0 Then Exit Sub

        Dim status = Visitors_CheckedListBox.CheckedItems.Count <> 0
        For i = 0 To Visitors_CheckedListBox.Items.Count - 1
            Visitors_CheckedListBox.SetItemChecked(i, Not status)
        Next
    End Sub

    Private Function GetHouseImageDrawLocation() As Point
        Dim sizeDifference = House_ImageBox.Size - houseImage.Size
        Return New Point(CInt(sizeDifference.Width / 2), CInt(sizeDifference.Height / 2))
    End Function

    Private Sub House_ImageBox_MouseClick(sender As Object, e As MouseEventArgs) Handles House_ImageBox.MouseClick
        doorLocation = Point.Subtract(e.Location, CType(GetHouseImageDrawLocation(), Size))
        DoorLocation_Label.Text = doorLocation.ToString()
        House_ImageBox.Invalidate()
    End Sub

    Private Sub House_ImageBox_SizeChanged(sender As Object, e As EventArgs) Handles House_ImageBox.SizeChanged
        House_ImageBox.Invalidate()
    End Sub

    Private Sub House_ImageBox_Paint(sender As Object, e As PaintEventArgs) Handles House_ImageBox.Paint
        Dim g = e.Graphics

        g.Clear(Color.White)

        Dim drawLocation = GetHouseImageDrawLocation()
        g.FillRectangle(Brushes.Black, New Rectangle(drawLocation, houseImage.Size))
        g.DrawImage(houseImage, drawLocation)

        Dim localDoorLocation = Point.Add(drawLocation, CType(doorLocation, Drawing.Size))
        Using pen As New Pen(Color.Red, 2)
            g.DrawLine(pen,
                       localDoorLocation.X - 5, localDoorLocation.Y - 5,
                       localDoorLocation.X + 5, localDoorLocation.Y + 5)
            g.DrawLine(pen,
                       localDoorLocation.X + 5, localDoorLocation.Y - 5,
                       localDoorLocation.X - 5, localDoorLocation.Y + 5)
        End Using
    End Sub

    Private Function SaveSettings() As Boolean
        Dim base = house.HouseBase

        base.CycleInterval = TimeSpan.FromSeconds(Cycle_Counter.Value)
        base.DoorPosition = doorLocation
        base.MinimumPonies = CInt(MinSpawn_Counter.Value)
        base.MaximumPonies = CInt(MaxSpawn_Counter.Value)
        base.Bias = Bias_TrackBar.Value / 10D

        If Visitors_CheckedListBox.CheckedItems.Count = 0 Then
            MessageBox.Show(Me, "You must select at least one visitor!",
                            "No Visitors Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        SyncLock base.Visitors
            base.Visitors.Clear()
            For Each entry As String In Visitors_CheckedListBox.CheckedItems
                base.Visitors.Add(entry)
            Next
        End SyncLock

        house.InitializeVisitorList()

        Return True
    End Function

    Private Sub Save_Button_Click(sender As Object, e As EventArgs) Handles Save_Button.Click
        If SaveSettings() = False Then Exit Sub

        Try
            Dim base = house.HouseBase

            Dim comments As New List(Of String)
            Dim ini_file_path = Path.Combine(Options.InstallLocation, HouseBase.RootDirectory, base.Directory, HouseBase.ConfigFilename)
            If File.Exists(ini_file_path) Then
                Using existing_ini As New StreamReader(ini_file_path)
                    Do Until existing_ini.EndOfStream
                        Dim line = existing_ini.ReadLine()
                        If Mid(line, 1, 1) = "'" Then
                            comments.Add(line)
                        End If
                        line = Nothing
                    Loop
                End Using
            End If

            Using new_ini As New StreamWriter(ini_file_path, False, System.Text.Encoding.UTF8)

                For Each line In comments
                    new_ini.WriteLine(line)
                Next

                new_ini.WriteLine("name," & house.HouseBase.Name)

                Dim cma = ","

                new_ini.WriteLine("image," & base.LeftImage.Path.Remove(0, base.LeftImage.Path.LastIndexOf(Path.DirectorySeparatorChar) + 1))
                new_ini.WriteLine("door," &
                                  base.DoorPosition.X.ToString(CultureInfo.InvariantCulture) & cma &
                                  base.DoorPosition.Y.ToString(CultureInfo.InvariantCulture))
                new_ini.WriteLine("cycletime," & CInt(base.CycleInterval.TotalSeconds).ToString(CultureInfo.InvariantCulture))
                new_ini.WriteLine("minspawn," & base.MinimumPonies.ToString(CultureInfo.InvariantCulture))
                new_ini.WriteLine("maxspawn," & base.MaximumPonies.ToString(CultureInfo.InvariantCulture))
                new_ini.WriteLine("bias," & base.Bias.ToString(CultureInfo.InvariantCulture))

                If Visitors_CheckedListBox.Items.Count = Visitors_CheckedListBox.CheckedItems.Count Then
                    new_ini.WriteLine("ALL")
                Else
                    SyncLock base.Visitors
                        For Each entry In base.Visitors
                            new_ini.WriteLine(entry)
                        Next
                    End SyncLock
                End If
            End Using

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Unable to save house file")
            Exit Sub
        End Try

        MessageBox.Show(Me, "Save Completed!", "Save Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Close_Button_Click(sender As Object, e As EventArgs) Handles Close_Button.Click
        Me.Close()
    End Sub

    Private Sub HouseOptionsForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        house.HouseBase.OptionsForm = Nothing
    End Sub
End Class