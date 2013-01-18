Imports System.Globalization
Imports System.IO
Imports System.Text

Public NotInheritable Class Options
    Public Const DefaultProfileName = "default"
    Private Shared installLocationValue As String = Path.GetDirectoryName(Application.ExecutablePath)
    Public Shared Property InstallLocation As String
        Get
            Return installLocationValue
        End Get
        Set(value As String)
            installLocationValue = value
        End Set
    End Property

    Private Const OptionsCount = 30
    Public Shared ProfileName As String

    Public Shared SuspendForFullscreenApplication As Boolean
    Public Shared ShowInTaskbar As Boolean
    Public Shared AlwaysOnTop As Boolean
    Public Shared AlphaBlendingEnabled As Boolean
    Public Shared WindowAvoidanceEnabled As Boolean
    Public Shared CursorAvoidanceEnabled As Boolean
    Public Shared CursorAvoidanceSize As Single
    Public Shared SoundEnabled As Boolean
    Public Shared SoundVolume As Single
    Public Shared SoundSingleChannelOnly As Boolean

    Public Shared PonyAvoidsPonies As Boolean
    Public Shared PonyStaysInBox As Boolean
    Public Shared PonyEffectsEnabled As Boolean
    Public Shared PonyDraggingEnabled As Boolean
    Public Shared PonyTeleportEnabled As Boolean
    Public Shared PonySpeechEnabled As Boolean
    Public Shared PonySpeechChance As Single
    Public Shared PonyInteractionsExist As Boolean
    Public Shared PonyInteractionsEnabled As Boolean
    Public Shared DisplayPonyInteractionsErrors As Boolean

    Public Shared ScreenSaverSoundEnabled As Boolean
    Public Shared ScreenSaverStyle As ScreenSaverBackgroundStyle
    Public Shared ScreenSaverBackgroundColor As Color
    Public Shared ScreenSaverBackgroundImagePath As String = ""

    Public Shared MaxPonyCount As Integer
    Public Shared TimeFactor As Single
    Public Shared ScaleFactor As Single
    Public Shared ExclusionZone As RectangleF

    Public Shared MonitorNames As New List(Of String)
    Public Shared PonyCounts As New Dictionary(Of String, Integer)
    Public Shared CustomTags As New List(Of String)

    Public Shared ReadOnly Property ProfileDirectory As String
        Get
            Return Path.Combine(Options.InstallLocation, "Profiles")
        End Get
    End Property

    Public Enum ScreenSaverBackgroundStyle
        Transparent
        SolidColor
        BackgroundImage
    End Enum

    Shared Sub New()
        LoadDefaultProfile()
    End Sub

    Private Shared Sub ValidateProfileName(profile As String)
        If String.IsNullOrEmpty(profile) Then Throw New ArgumentException("profile must not be null or empty.", "profile")
        If String.Equals(profile, DefaultProfileName, StringComparison.OrdinalIgnoreCase) Then
            Throw New ArgumentException("profile must not match the default profile name.", "profile")
        End If
    End Sub

    Public Shared Function GetKnownProfiles() As String()
        Try
            Dim files = Directory.GetFiles(ProfileDirectory, "*.ini", SearchOption.TopDirectoryOnly)
            For i = 0 To files.Length - 1
                files(i) = files(i).Replace(ProfileDirectory & Path.DirectorySeparatorChar, "").Replace(".ini", "")
            Next
            Return files
        Catch ex As DirectoryNotFoundException
            ' Screensaver mode set up a bad path, and we couldn't find what we needed.
            Return Nothing
        End Try
    End Function

    Public Shared Sub LoadProfile(profile As String)
        If String.IsNullOrEmpty(profile) Then Throw New ArgumentException("profile must not be null or empty.", "profile")

        If String.Equals(profile, DefaultProfileName, StringComparison.OrdinalIgnoreCase) Then
            LoadDefaultProfile()
        Else
            Using reader As New StreamReader(Path.Combine(ProfileDirectory, profile & ".ini"), Encoding.UTF8)
                ProfileName = profile
                MonitorNames.Clear()
                PonyCounts.Clear()
                CustomTags.Clear()
                While Not reader.EndOfStream
                    Dim columns = CommaSplitQuoteQualified(reader.ReadLine())
                    If columns.Length = 0 Then Continue While

                    Select Case columns(0)
                        Case "options"
                            If columns.Length - 1 < OptionsCount Then Throw New InvalidDataException(
                                String.Format(CultureInfo.CurrentCulture, "Expected at least {0} options on the options line.", OptionsCount))
                            PonySpeechEnabled = Boolean.Parse(columns(1))
                            PonySpeechChance = Single.Parse(columns(2), CultureInfo.InvariantCulture)
                            CursorAvoidanceEnabled = Boolean.Parse(columns(3))
                            CursorAvoidanceSize = Single.Parse(columns(4), CultureInfo.InvariantCulture)
                            PonyDraggingEnabled = Boolean.Parse(columns(5))
                            PonyInteractionsEnabled = Boolean.Parse(columns(6))
                            DisplayPonyInteractionsErrors = Boolean.Parse(columns(7))
                            ExclusionZone.X = Single.Parse(columns(8), CultureInfo.InvariantCulture)
                            ExclusionZone.Y = Single.Parse(columns(9), CultureInfo.InvariantCulture)
                            ExclusionZone.Width = Single.Parse(columns(10), CultureInfo.InvariantCulture)
                            ExclusionZone.Height = Single.Parse(columns(11), CultureInfo.InvariantCulture)
                            ScaleFactor = Single.Parse(columns(12), CultureInfo.InvariantCulture)
                            MaxPonyCount = Integer.Parse(columns(13), CultureInfo.InvariantCulture)
                            AlphaBlendingEnabled = Boolean.Parse(columns(14))
                            PonyEffectsEnabled = Boolean.Parse(columns(15))
                            WindowAvoidanceEnabled = Boolean.Parse(columns(16))
                            PonyAvoidsPonies = Boolean.Parse(columns(17))
                            PonyStaysInBox = Boolean.Parse(columns(18))
                            PonyTeleportEnabled = Boolean.Parse(columns(19))
                            TimeFactor = Single.Parse(columns(20), CultureInfo.InvariantCulture)
                            SoundEnabled = Boolean.Parse(columns(21))
                            SoundSingleChannelOnly = Boolean.Parse(columns(22))
                            SoundVolume = Single.Parse(columns(23), CultureInfo.InvariantCulture)
                            AlwaysOnTop = Boolean.Parse(columns(24))
                            SuspendForFullscreenApplication = Boolean.Parse(columns(25))
                            ScreenSaverSoundEnabled = Boolean.Parse(columns(26))
                            ScreenSaverStyle = CType([Enum].Parse(GetType(ScreenSaverBackgroundStyle), columns(27)), 
                                ScreenSaverBackgroundStyle)
                            ScreenSaverBackgroundColor = Color.FromArgb(Integer.Parse(columns(28), CultureInfo.InvariantCulture))
                            ScreenSaverBackgroundImagePath = columns(29)
                            Main.Instance.NoRandomDuplicates = Boolean.Parse(columns(30))
                        Case "monitor"
                            If columns.Length - 1 <> 1 Then Throw New InvalidDataException("Expected a monitor name on the monitor line.")
                            MonitorNames.Add(columns(1))
                        Case "count"
                            If columns.Length - 1 <> 2 Then Throw New InvalidDataException("Expected a count on the count line.")
                            PonyCounts.Add(columns(1), Integer.Parse(columns(2), CultureInfo.InvariantCulture))
                        Case "tag"
                            If columns.Length - 1 <> 1 Then Throw New InvalidDataException("Expected a tag name on the tag line.")
                            CustomTags.Add(columns(1))
                    End Select
                End While
            End Using
        End If

        LoadPonyCounts()
        LoadCustomTags()
    End Sub

    Public Shared Sub LoadDefaultProfile()
        ProfileName = DefaultProfileName
        MonitorNames.Clear()
        PonyCounts.Clear()
        CustomTags.Clear()

        SuspendForFullscreenApplication = True
        ShowInTaskbar = True
        AlwaysOnTop = True
        AlphaBlendingEnabled = True
        WindowAvoidanceEnabled = False
        CursorAvoidanceEnabled = True
        CursorAvoidanceSize = 100
        SoundEnabled = True
        SoundVolume = 0.65
        SoundSingleChannelOnly = False

        PonyAvoidsPonies = False
        PonyStaysInBox = False
        PonyEffectsEnabled = True
        PonyDraggingEnabled = True
        PonyTeleportEnabled = False
        PonySpeechEnabled = True
        PonySpeechChance = 0.01
        PonyInteractionsExist = False
        PonyInteractionsEnabled = True
        DisplayPonyInteractionsErrors = False

        ScreenSaverSoundEnabled = True
        ScreenSaverStyle = ScreenSaverBackgroundStyle.Transparent
        ScreenSaverBackgroundColor = Color.Black
        ScreenSaverBackgroundImagePath = ""

        MaxPonyCount = 300
        TimeFactor = 1
        ScaleFactor = 1
        ExclusionZone = RectangleF.Empty
    End Sub

    Public Shared Sub LoadPonyCounts()
        If Main.Instance.Ponies_Have_Launched Then Exit Sub

        For Each ponyPanel As PonySelectionControl In Main.Instance.PonySelectionPanel.Controls
            If PonyCounts.ContainsKey(ponyPanel.PonyName.Text) Then
                ponyPanel.PonyCount.Text = CStr(PonyCounts(ponyPanel.PonyBase.Directory))
            Else
                ponyPanel.PonyCount.Text = "0"
            End If
        Next
    End Sub

    Public Shared Sub LoadCustomTags()
        If Main.Instance.Ponies_Have_Launched Then Exit Sub

        Main.Instance.FilterOptionsBox.Items.Clear()

        Main.Instance.FilterOptionsBox.Items.Add("Main Ponies")
        Main.Instance.FilterOptionsBox.Items.Add("Supporting Ponies")
        Main.Instance.FilterOptionsBox.Items.Add("Alternate Art")
        Main.Instance.FilterOptionsBox.Items.Add("Fillies")
        Main.Instance.FilterOptionsBox.Items.Add("Colts")
        Main.Instance.FilterOptionsBox.Items.Add("Pets")
        Main.Instance.FilterOptionsBox.Items.Add("Stallions")
        Main.Instance.FilterOptionsBox.Items.Add("Mares")
        Main.Instance.FilterOptionsBox.Items.Add("Alicorns")
        Main.Instance.FilterOptionsBox.Items.Add("Unicorns")
        Main.Instance.FilterOptionsBox.Items.Add("Pegasi")
        Main.Instance.FilterOptionsBox.Items.Add("Earth Ponies")
        Main.Instance.FilterOptionsBox.Items.Add("Non-Ponies")
        Main.Instance.FilterOptionsBox.Items.Add("Not Tagged")

        Main.Instance.FilterOptionsBox.Items.AddRange(CustomTags.ToArray())
    End Sub

    Public Shared Sub SaveProfile(profile As String)
        ValidateProfileName(profile)

        Using file As New StreamWriter(Path.Combine(ProfileDirectory, profile & ".ini"), False, Encoding.UTF8)
            Dim optionsLine = String.Join(",", "options",
                                     PonySpeechEnabled,
                                     PonySpeechChance.ToString(CultureInfo.InvariantCulture),
                                     CursorAvoidanceEnabled,
                                     CursorAvoidanceSize.ToString(CultureInfo.InvariantCulture),
                                     PonyDraggingEnabled,
                                     PonyInteractionsEnabled,
                                     DisplayPonyInteractionsErrors,
                                     ExclusionZone.X.ToString(CultureInfo.InvariantCulture),
                                     ExclusionZone.Y.ToString(CultureInfo.InvariantCulture),
                                     ExclusionZone.Width.ToString(CultureInfo.InvariantCulture),
                                     ExclusionZone.Height.ToString(CultureInfo.InvariantCulture),
                                     ScaleFactor.ToString(CultureInfo.InvariantCulture),
                                     MaxPonyCount.ToString(CultureInfo.InvariantCulture),
                                     AlphaBlendingEnabled,
                                     PonyEffectsEnabled,
                                     WindowAvoidanceEnabled,
                                     PonyAvoidsPonies,
                                     PonyStaysInBox,
                                     PonyTeleportEnabled,
                                     TimeFactor.ToString(CultureInfo.InvariantCulture),
                                     SoundEnabled,
                                     SoundSingleChannelOnly,
                                     SoundVolume.ToString(CultureInfo.InvariantCulture),
                                     AlwaysOnTop,
                                     SuspendForFullscreenApplication,
                                     ScreenSaverSoundEnabled,
                                     ScreenSaverStyle,
                                     ScreenSaverBackgroundColor.ToArgb().ToString(CultureInfo.InvariantCulture),
                                     ScreenSaverBackgroundImagePath,
                                     Main.Instance.NoRandomDuplicates)
            file.WriteLine(optionsLine)

            GetMonitorNames()
            GetPonyCounts()

            For Each monitorName In MonitorNames
                file.WriteLine(String.Join(",", "monitor", ControlChars.Quote & monitorName & ControlChars.Quote))
            Next

            For Each entry In PonyCounts
                file.WriteLine(String.Join(",", "count", ControlChars.Quote & entry.Key & ControlChars.Quote,
                                           entry.Value.ToString(CultureInfo.InvariantCulture)))
            Next

            For Each tag In CustomTags
                file.WriteLine(String.Join(",", "tag", ControlChars.Quote & tag & ControlChars.Quote))
            Next
        End Using
    End Sub

    Public Shared Function DeleteProfile(profile As String) As Boolean
        ValidateProfileName(profile)
        Try
            File.Delete(Path.Combine(ProfileDirectory, profile & ".ini"))
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Shared Function ExclusionZoneForBounds(bounds As Rectangle) As Rectangle
        Return New Rectangle(CInt(ExclusionZone.X * bounds.Width + bounds.X),
                             CInt(ExclusionZone.Y * bounds.Height + bounds.Y),
                             CInt(ExclusionZone.Width * bounds.Width),
                             CInt(ExclusionZone.Height * bounds.Height))
    End Function

    Private Shared Sub GetMonitorNames()
        MonitorNames.Clear()
        For Each monitorName As String In OptionsForm.Instance.MonitorsSelection.SelectedItems
            MonitorNames.Add(monitorName)
        Next
    End Sub

    Private Shared Sub GetPonyCounts()
        PonyCounts.Clear()
        For Each ponyPanel As PonySelectionControl In Main.Instance.PonySelectionPanel.Controls
            Dim count As Integer
            If Integer.TryParse(ponyPanel.PonyCount.Text, count) AndAlso count > 0 Then
                PonyCounts.Add(ponyPanel.PonyBase.Directory, count)
            End If
        Next
    End Sub

End Class
