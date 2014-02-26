Imports System.Globalization
Imports System.IO
Imports System.Text

Public NotInheritable Class Options
    Public Shared ReadOnly Property DefaultProfileName As CaseInsensitiveString
        Get
            Return "default"
        End Get
    End Property

    Public Shared ProfileName As String

    Public Shared SuspendForFullscreenApplication As Boolean
    Public Shared ShowInTaskbar As Boolean
    Public Shared AlwaysOnTop As Boolean
    Private Shared alphaBlendingEnabled As Boolean
    Public Shared WindowAvoidanceEnabled As Boolean
    Public Shared CursorAvoidanceEnabled As Boolean
    Public Shared CursorAvoidanceSize As Single
    Public Shared SoundEnabled As Boolean
    Public Shared SoundVolume As Single
    Public Shared SoundSingleChannelOnly As Boolean

    Public Shared PonyAvoidsPonies As Boolean
    Public Shared WindowContainment As Boolean
    Public Shared PonyEffectsEnabled As Boolean
    Public Shared PonyDraggingEnabled As Boolean
    Public Shared PonyTeleportEnabled As Boolean
    Public Shared PonySpeechEnabled As Boolean
    Public Shared PonySpeechChance As Single
    Public Shared PonyInteractionsEnabled As Boolean
    Private Shared displayPonyInteractionsErrors As Boolean

    Public Shared ScreensaverSoundEnabled As Boolean
    Public Shared ScreensaverStyle As ScreensaverBackgroundStyle
    Public Shared ScreensaverBackgroundColor As Color
    Public Shared ScreensaverBackgroundImagePath As String = ""

    Public Shared NoRandomDuplicates As Boolean

    Public Shared MaxPonyCount As Integer
    Public Shared TimeFactor As Single
    Public Shared ScaleFactor As Single
    Public Shared ExclusionZone As RectangleF

    Private Shared _screens As ImmutableArray(Of Screen)
    Public Shared Property Screens As ImmutableArray(Of Screen)
        Get
            Return _screens
        End Get
        Set(value As ImmutableArray(Of Screen))
            Argument.EnsureNotNullOrEmpty(value, "value")
            _screens = value
        End Set
    End Property
    Public Shared AllowedRegion As Rectangle?
    Public Shared PonyCounts As ReadOnlyDictionary(Of String, Integer)
    Public Shared CustomTags As ImmutableArray(Of CaseInsensitiveString)

    Public Shared EnablePonyLogs As Boolean
    Public Shared ShowPerformanceGraph As Boolean

    Public Shared ReadOnly Property ProfileDirectory As String
        Get
            Return Path.Combine(EvilGlobals.InstallLocation, "Profiles")
        End Get
    End Property

    Public Enum ScreensaverBackgroundStyle
        Transparent
        SolidColor
        BackgroundImage
    End Enum

    Shared Sub New()
        LoadDefaultProfile()
    End Sub

    Private Sub New()
    End Sub

    Private Shared Sub ValidateProfileName(profile As String)
        If String.IsNullOrEmpty(profile) Then Throw New ArgumentException("profile must not be null or empty.", "profile")
        If profile = DefaultProfileName Then
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

    Public Shared Sub LoadProfile(profile As String, setAsCurrent As Boolean)
        Argument.EnsureNotNullOrEmpty(profile, "profile")

        If profile = DefaultProfileName Then
            LoadDefaultProfile()
        Else
            Using reader As New StreamReader(Path.Combine(ProfileDirectory, profile & ".ini"), Encoding.UTF8)
                ProfileName = profile
                Dim newScreens As New List(Of Screen)()
                Dim newCounts As New Dictionary(Of String, Integer)()
                Dim newTags As New List(Of CaseInsensitiveString)()
                While Not reader.EndOfStream
                    Dim columns = CommaSplitQuoteQualified(reader.ReadLine())
                    If columns.Length = 0 Then Continue While

                    Select Case columns(0)
                        Case "options"
                            Dim p As New StringCollectionParser(
                                columns,
                                {"Identifier", "Speech Enabled", "Speech Chance", "Cursor Awareness Enabled", "Cursor Avoidance Radius",
                                 "Pony Dragging Enabled", "Pony Interactions Enabled", "Display Interactions Errors",
                                 "Exclusion Zone X", "Exclusion Zone Y", "Exclusion Zone Width", "Exclusion Zone Height",
                                 "Scale Factor", "Max Pony Count", "Alpha Blending Enabled", "Pony Effects Enabled",
                                 "Window Avoidance Enabled", "Ponies Avoid Ponies", "Pony Containment Enabled", "Pony Teleport Enabled",
                                 "Time Factor", "Sound Enabled", "Sound Single Channel Only", "Sound Volume",
                                 "Always On Top", "Suspend For Fullscreen Application",
                                 "Screensaver Sound Enabled", "Screensaver Style",
                                 "Screensaver Background Color", "Screensaver Background Image Path",
                                 "No Random Duplicates", "Show In Taskbar",
                                 "Allowed Area X", "Allowed Area Y", "Allowed Area Width", "Allowed Area Height"})
                            p.NoParse()
                            PonySpeechEnabled = p.ParseBoolean(True)
                            PonySpeechChance = p.ParseSingle(0.01)
                            CursorAvoidanceEnabled = p.ParseBoolean(True)
                            CursorAvoidanceSize = p.ParseSingle(100)
                            PonyDraggingEnabled = p.ParseBoolean(True)
                            PonyInteractionsEnabled = p.ParseBoolean(True)
                            displayPonyInteractionsErrors = p.ParseBoolean(False)
                            ExclusionZone.X = p.ParseSingle(0)
                            ExclusionZone.Y = p.ParseSingle(0)
                            ExclusionZone.Width = p.ParseSingle(0)
                            ExclusionZone.Height = p.ParseSingle(0)
                            ScaleFactor = p.ParseSingle(1)
                            MaxPonyCount = p.ParseInt32(300)
                            alphaBlendingEnabled = p.ParseBoolean(True)
                            PonyEffectsEnabled = p.ParseBoolean(True)
                            WindowAvoidanceEnabled = p.ParseBoolean(False)
                            PonyAvoidsPonies = p.ParseBoolean(False)
                            WindowContainment = p.ParseBoolean(False)
                            PonyTeleportEnabled = p.ParseBoolean(False)
                            TimeFactor = p.ParseSingle(1)
                            SoundEnabled = p.ParseBoolean(True)
                            SoundSingleChannelOnly = p.ParseBoolean(False)
                            SoundVolume = p.ParseSingle(0.75)
                            AlwaysOnTop = p.ParseBoolean(True)
                            SuspendForFullscreenApplication = p.ParseBoolean(True) ' TODO: Respect or remove this option.
                            ScreensaverSoundEnabled = p.ParseBoolean(True)
                            ScreensaverStyle = p.ParseEnum(ScreensaverBackgroundStyle.Transparent)
                            ScreensaverBackgroundColor = Color.FromArgb(p.ParseInt32(0))
                            ScreensaverBackgroundImagePath = p.NotNull("")
                            NoRandomDuplicates = p.ParseBoolean(True)
                            ShowInTaskbar = p.ParseBoolean(OperatingSystemInfo.IsWindows)
                            Dim region = New Rectangle()
                            region.X = p.ParseInt32(0)
                            region.Y = p.ParseInt32(0)
                            region.Width = p.ParseInt32(0)
                            region.Height = p.ParseInt32(0)
                            If region.Width > 0 AndAlso region.Height > 0 Then AllowedRegion = region
                        Case "monitor"
                            If columns.Length <> 2 Then Exit Select
                            Dim monitor = Screen.AllScreens.FirstOrDefault(Function(s) s.DeviceName = columns(1))
                            If monitor IsNot Nothing Then newScreens.Add(monitor)
                        Case "count"
                            If columns.Length <> 3 Then Exit Select
                            Dim count As Integer
                            If Integer.TryParse(columns(2), NumberStyles.Integer, CultureInfo.InvariantCulture, count) Then
                                newCounts.Add(columns(1), count)
                            End If
                        Case "tag"
                            If columns.Length <> 2 Then Exit Select
                            newTags.Add(columns(1))
                    End Select
                End While
                If newScreens.Count = 0 Then newScreens.Add(Screen.PrimaryScreen)
                Screens = newScreens.ToImmutableArray()
                PonyCounts = newCounts.AsReadOnly()
                CustomTags = newTags.ToImmutableArray()
            End Using
        End If

        LoadPonyCounts()

        If setAsCurrent Then
            Try
                IO.File.WriteAllText(IO.Path.Combine(Options.ProfileDirectory, "current.txt"), profile, System.Text.Encoding.UTF8)
            Catch ex As IO.IOException
                ' If we cannot write out the file that remembers the last used profile, that is unfortunate but not a fatal problem.
                Console.WriteLine("Warning: Failed to save current.txt file.")
            End Try
        End If
    End Sub

    Public Shared Sub LoadDefaultProfile()
        ProfileName = DefaultProfileName
        Screens = {Screen.PrimaryScreen}.ToImmutableArray()
        AllowedRegion = Nothing
        PonyCounts = New Dictionary(Of String, Integer)().AsReadOnly()
        CustomTags = New CaseInsensitiveString() {}.ToImmutableArray()

        SuspendForFullscreenApplication = True
        ShowInTaskbar = OperatingSystemInfo.IsWindows
        AlwaysOnTop = True
        alphaBlendingEnabled = True
        WindowAvoidanceEnabled = False
        CursorAvoidanceEnabled = True
        CursorAvoidanceSize = 100
        SoundEnabled = True
        SoundVolume = 0.75
        SoundSingleChannelOnly = False

        PonyAvoidsPonies = False
        WindowContainment = False
        PonyEffectsEnabled = True
        PonyDraggingEnabled = True
        PonyTeleportEnabled = False
        PonySpeechEnabled = True
        PonySpeechChance = 0.01
        PonyInteractionsEnabled = True
        displayPonyInteractionsErrors = False

        ScreensaverSoundEnabled = True
        ScreensaverStyle = ScreensaverBackgroundStyle.Transparent
        ScreensaverBackgroundColor = Color.Empty
        ScreensaverBackgroundImagePath = ""

        NoRandomDuplicates = True

        MaxPonyCount = 300
        TimeFactor = 1
        ScaleFactor = 1
        ExclusionZone = RectangleF.Empty

        EnablePonyLogs = False
        ShowPerformanceGraph = False
    End Sub

    Public Shared Sub LoadPonyCounts()
        If EvilGlobals.PoniesHaveLaunched Then Exit Sub

        For Each ponyPanel As PonySelectionControl In EvilGlobals.Main.PonySelectionPanel.Controls
            If PonyCounts.ContainsKey(ponyPanel.PonyName.Text) Then
                ponyPanel.Count = PonyCounts(ponyPanel.PonyBase.Directory)
            Else
                ponyPanel.Count = 0
            End If
        Next
    End Sub

    Public Shared Sub SaveProfile(profile As String)
        ValidateProfileName(profile)

        Using file As New StreamWriter(Path.Combine(ProfileDirectory, profile & ".ini"), False, Encoding.UTF8)
            Dim region = If(AllowedRegion, Rectangle.Empty)
            Dim optionsLine = String.Join(",", "options",
                                     PonySpeechEnabled,
                                     PonySpeechChance.ToString(CultureInfo.InvariantCulture),
                                     CursorAvoidanceEnabled,
                                     CursorAvoidanceSize.ToString(CultureInfo.InvariantCulture),
                                     PonyDraggingEnabled,
                                     PonyInteractionsEnabled,
                                     displayPonyInteractionsErrors,
                                     ExclusionZone.X.ToString(CultureInfo.InvariantCulture),
                                     ExclusionZone.Y.ToString(CultureInfo.InvariantCulture),
                                     ExclusionZone.Width.ToString(CultureInfo.InvariantCulture),
                                     ExclusionZone.Height.ToString(CultureInfo.InvariantCulture),
                                     ScaleFactor.ToString(CultureInfo.InvariantCulture),
                                     MaxPonyCount.ToString(CultureInfo.InvariantCulture),
                                     alphaBlendingEnabled,
                                     PonyEffectsEnabled,
                                     WindowAvoidanceEnabled,
                                     PonyAvoidsPonies,
                                     WindowContainment,
                                     PonyTeleportEnabled,
                                     TimeFactor.ToString(CultureInfo.InvariantCulture),
                                     SoundEnabled,
                                     SoundSingleChannelOnly,
                                     SoundVolume.ToString(CultureInfo.InvariantCulture),
                                     AlwaysOnTop,
                                     SuspendForFullscreenApplication,
                                     ScreensaverSoundEnabled,
                                     ScreensaverStyle,
                                     ScreensaverBackgroundColor.ToArgb().ToString(CultureInfo.InvariantCulture),
                                     ScreensaverBackgroundImagePath,
                                     NoRandomDuplicates,
                                     ShowInTaskbar,
                                     region.X.ToString(CultureInfo.InvariantCulture),
                                     region.Y.ToString(CultureInfo.InvariantCulture),
                                     region.Width.ToString(CultureInfo.InvariantCulture),
                                     region.Height.ToString(CultureInfo.InvariantCulture))
            file.WriteLine(optionsLine)

            GetPonyCounts()

            For Each screen In Screens
                file.WriteLine(String.Join(",", "monitor", Quoted(screen.DeviceName)))
            Next

            For Each entry In PonyCounts
                file.WriteLine(String.Join(",", "count", Quoted(entry.Key), entry.Value.ToString(CultureInfo.InvariantCulture)))
            Next

            For Each tag In CustomTags
                file.WriteLine(String.Join(",", "tag", Quoted(tag)))
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

    Private Shared Sub GetPonyCounts()
        Dim newCounts = New Dictionary(Of String, Integer)()
        For Each ponyPanel As PonySelectionControl In EvilGlobals.Main.PonySelectionPanel.Controls
            newCounts.Add(ponyPanel.PonyBase.Directory, ponyPanel.Count)
        Next
        PonyCounts = newCounts.AsReadOnly()
    End Sub

    Public Shared Function GetAllowedArea() As Rectangle
        If AllowedRegion Is Nothing Then
            Dim area As Rectangle = Rectangle.Empty
            For Each screen In Screens
                If area = Rectangle.Empty Then
                    area = screen.WorkingArea
                Else
                    area = Rectangle.Union(area, screen.WorkingArea)
                End If
            Next
            Return area
        Else
            Return AllowedRegion.Value
        End If
    End Function

    Public Shared Function GetCombinedScreenBounds() As Rectangle
        Dim area As Rectangle = Rectangle.Empty
        For Each s In Screen.AllScreens
            If area = Rectangle.Empty Then
                area = s.Bounds
            Else
                area = Rectangle.Union(area, s.Bounds)
            End If
        Next
        Return area
    End Function

    Public Shared Function GetExclusionArea(allowedArea As Rectangle, exclusionZone As RectangleF) As Rectangle
        Dim x = allowedArea.X + allowedArea.Width * exclusionZone.X
        Dim y = allowedArea.Y + allowedArea.Height * exclusionZone.Y
        Dim width = allowedArea.Width * exclusionZone.Width
        Dim height = allowedArea.Height * exclusionZone.Height
        Dim area = Rectangle.Round(New RectangleF(x, y, width, height))
        If area.Right > allowedArea.Right Then area.Width -= area.Right - allowedArea.Right
        If area.Bottom > allowedArea.Bottom Then area.Height -= area.Bottom - allowedArea.Bottom
        Return area
    End Function

    Public Shared Function GetInterface() As DesktopSprites.SpriteManagement.ISpriteCollectionView
        Dim viewer As DesktopSprites.SpriteManagement.ISpriteCollectionView
        If GetInterfaceType() = GetType(DesktopSprites.SpriteManagement.WinFormSpriteInterface) Then
            viewer = New DesktopSprites.SpriteManagement.WinFormSpriteInterface(GetAllowedArea())
            viewer.BufferPreprocess = AddressOf GifProcessing.LosslessDownscale
        Else
            viewer = New DesktopSprites.SpriteManagement.GtkSpriteInterface()
        End If
        viewer.ShowInTaskbar = ShowInTaskbar
        Return viewer
    End Function

    Public Shared Function GetInterfaceType() As Type
        If OperatingSystemInfo.IsWindows AndAlso Not Runtime.IsMono Then
            Return GetType(DesktopSprites.SpriteManagement.WinFormSpriteInterface)
        Else
            Return GetType(DesktopSprites.SpriteManagement.GtkSpriteInterface)
        End If
    End Function

End Class
