Public Class CommunityDialog

    Public Structure LinkInfo
        Public ReadOnly Name As String
        Public ReadOnly Url As Uri
        Public ReadOnly Description As String
        Public Sub New(name As String, url As Uri, description As String)
            Me.Name = name
            Me.Url = url
            Me.Description = description
        End Sub
    End Structure

    Public Class CommunityInfo
        Public Shared Function Retrieve() As CommunityInfo
            Dim latestVersion As Version
            Dim latestVersionUrl As Uri
            Dim patchFromVersion As Version = Nothing
            Dim latestVersionPatchUrl As Uri = Nothing
            Dim links As New List(Of LinkInfo)()
            Try
                Dim latest As String = Nothing
                Using client As New Net.WebClient()
                    latest = client.DownloadString("https://dl.dropboxusercontent.com/s/tjeymr4wsopqgib/community.xml")
                End Using
                Dim doc = System.Xml.Linq.XDocument.Parse(latest)
                latestVersion = Version.Parse(doc.Root.<LatestVersion>.Single().Value)
                latestVersionUrl = New Uri(doc.Root.<LatestVersionUrl>.Single().Value)
                Dim patchFromValue = doc.Root.<LatestVersionPatchFrom>.Single().Value
                If Not String.IsNullOrEmpty(patchFromValue) Then patchFromVersion = Version.Parse(patchFromValue)
                Dim patchUrlValue = doc.Root.<LatestVersionPatchUrl>.Single().Value
                If Not String.IsNullOrEmpty(patchUrlValue) Then latestVersionPatchUrl = New Uri(patchUrlValue)
                For Each element In doc.Root.<Links>.Single().Elements("Link")
                    Dim name = element.<Name>.Single().Value
                    Dim url = New Uri(element.<Url>.Single().Value)
                    Dim description = element.<Description>.Single().Value
                    links.Add(New LinkInfo(name, url, description))
                Next
                Return New CommunityInfo(latestVersion, latestVersionUrl,
                                         patchFromVersion, latestVersionPatchUrl, links.ToImmutableArray())
            Catch ex As Net.WebException When ex.InnerException IsNot Nothing AndAlso
                TypeOf ex.InnerException Is IO.IOException AndAlso
                ex.InnerException.InnerException IsNot Nothing AndAlso
                ex.InnerException.InnerException.GetType().ToString() = "Mono.Security.Protocol.Tls.TlsException"
                ' Default mono installations do not have any certificates installed and thus do not trust any https connections.
                ' This is a fairly user unfriendly out of the box default that we'll have to let the user know about and hope they care
                ' enough to fix it.
                Return New CommunityInfo()
            Catch ex As Exception
                ' There may be a whole variety of reasons we cannot get up-to-date community information. We won't bother the user if any
                ' problems occur - it's not important.
                Return Nothing
            End Try
        End Function
        Public ReadOnly CertificateError As Boolean
        Public ReadOnly LatestVersion As Version
        Public ReadOnly LatestVersionUrl As Uri
        Public ReadOnly NewerVersionAvailable As Boolean
        Public ReadOnly PatchFromVersion As Version
        Public ReadOnly LatestVersionPatchUrl As Uri
        Public ReadOnly CanPatch As Boolean
        Public ReadOnly Links As ImmutableArray(Of LinkInfo)
        Private Sub New()
            CertificateError = True
        End Sub
        Private Sub New(latestVersion As Version, latestVersionUrl As Uri,
                        patchFromVersion As Version, latestVersionPatchUrl As Uri,
                        links As ImmutableArray(Of LinkInfo))
            Me.LatestVersion = latestVersion
            Me.LatestVersionUrl = latestVersionUrl
            NewerVersionAvailable = latestVersion > My.MyApplication.GetAssemblyVersion()
            Me.PatchFromVersion = patchFromVersion
            Me.LatestVersionPatchUrl = latestVersionPatchUrl
            CanPatch = patchFromVersion IsNot Nothing AndAlso My.MyApplication.GetAssemblyVersion() >= patchFromVersion
            Me.Links = links
        End Sub
    End Class

    Private link As LinkLabel.Link

    Public Sub New(info As CommunityInfo)
        Argument.EnsureNotNull(info, "info")
        InitializeComponent()
        Icon = My.Resources.Twilight

        ' Note: Mono seems fine with hiding initially visible controls on tables, but attempting to converse does not appear to work. Don't
        ' attempt to streamline the visible/invisible logic by swapping things round or the UI on mono might become useless.
        If info.CertificateError Then
            LinksTable.Visible = False
            DownloadLink.Visible = False
            PatchLink.Visible = False
            PatchTextBox.Visible = False
            Return
        Else
            CertificateTextBox.Visible = False
        End If

        If info.NewerVersionAvailable Then
            DownloadLink.Links.Add(New LinkLabel.Link() With {.LinkData = info.LatestVersionUrl})
            DownloadLink.Text = "Download v" & info.LatestVersion.ToDisplayString() & " [New!]"
            If info.CanPatch Then
                PatchLink.Links.Add(New LinkLabel.Link() With {.LinkData = info.LatestVersionPatchUrl})
                PatchLink.Text = "Download patch to v" & info.LatestVersion.ToDisplayString() & " [New!]"
            Else
                PatchLink.Visible = False
                PatchTextBox.Visible = False
            End If
        Else
            DownloadLink.Visible = False
            PatchLink.Visible = False
            PatchTextBox.Visible = False
        End If
        For Each linkInfo In info.Links
            Dim linkLabel As New LinkLabel() With {.Text = linkInfo.Name, .AutoSize = True, .Padding = New Padding(3, 2, 3, 2)}
            linkLabel.Links.Add(New LinkLabel.Link() With {.LinkData = linkInfo.Url})
            LinkToolTip.SetToolTip(linkLabel, linkInfo.Description)
            LinksTable.Controls.Add(linkLabel)
            AddHandler linkLabel.MouseClick, AddressOf Link_MouseClick
            AddHandler linkLabel.MouseDown, AddressOf Link_MouseDown
        Next
    End Sub

    Private Sub Link_MouseClick(sender As Object, e As MouseEventArgs) Handles DownloadLink.MouseClick, PatchLink.MouseClick
        link = DirectCast(sender, LinkLabel).Links(0)
        If e.Button = MouseButtons.Left Then
            OpenLink()
        End If
    End Sub

    Private Sub Link_MouseDown(sender As Object, e As MouseEventArgs) Handles DownloadLink.MouseDown, PatchLink.MouseDown
        link = DirectCast(sender, LinkLabel).Links(0)
        If e.Button = Windows.Forms.MouseButtons.Right Then
            LinkContextMenu.Show(Control.MousePosition)
        End If
    End Sub

    Private Sub OpenLinkMenuItem_Click(sender As Object, e As EventArgs) Handles OpenLinkMenuItem.Click
        OpenLink()
    End Sub

    Private Sub CopyLinkMenuItem_Click(sender As Object, e As EventArgs) Handles CopyLinkMenuItem.Click
        Try
            Clipboard.SetText(link.LinkData.ToString())
        Catch ex As System.Runtime.InteropServices.ExternalException
            MessageBox.Show(Me, "Failed to copy text to clipboard. Another process may be using the clipboard at this time.",
                            "Copy Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub OpenLink()
        Try
            Diagnostics.Process.Start(link.LinkData.ToString())
            link.Visited = True
            Refresh()
        Catch ex As Exception
            MessageBox.Show(Me, "Failed to open URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class