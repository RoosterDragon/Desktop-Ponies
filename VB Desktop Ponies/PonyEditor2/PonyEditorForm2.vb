Imports System.IO

Public Class PonyEditorForm2
    Private ReadOnly idleWorker As IdleWorker = idleWorker.CurrentThreadWorker

    Private ReadOnly bases As New Dictionary(Of String, PonyBase)

    Private Class PageRef
        Private ReadOnly _ponyBase As MutablePonyBase
        Public ReadOnly Property PonyBase As MutablePonyBase
            Get
                Return _ponyBase
            End Get
        End Property
        Private ReadOnly _pageContent As PageContent
        Public ReadOnly Property PageContent As PageContent
            Get
                Return _pageContent
            End Get
        End Property
        Public Sub New(ponyBase As MutablePonyBase, pageContent As PageContent)
            _ponyBase = ponyBase
            _pageContent = pageContent
        End Sub
    End Class

    Private Enum PageContent
        Ponies
        Pony
        Behaviors
        Behavior
        Effects
        Effect
        Speeches
        Speech
    End Enum

    Public Sub New()
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub PonyEditorForm2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Enabled = False
        Dim screenArea = Screen.FromHandle(Handle).WorkingArea.Size
        'Size = New Size(CInt(screenArea.Width * 0.8), screenArea.Height)
        Size = New Size(1024 - SystemInformation.Border3DSize.Width, screenArea.Height)
        CenterToScreen()
        Threading.ThreadPool.QueueUserWorkItem(Sub() LoadBases())
    End Sub

    Private Sub LoadBases()
        Dim ponyBaseDirectories = Directory.GetDirectories(Path.Combine(Options.InstallLocation, PonyBase.RootDirectory))
        Array.Sort(ponyBaseDirectories, StringComparer.CurrentCultureIgnoreCase)

        IdleWorker.QueueTask(Sub() EditorProgressBar.Maximum = ponyBaseDirectories.Length)
        Dim poniesNode As TreeNode = Nothing
        IdleWorker.QueueTask(Sub()
                                 poniesNode = New TreeNode("Ponies") With
                                              {.Tag = New PageRef(Nothing, PageContent.Ponies)}
                                 DocumentsView.Nodes.Add(poniesNode)
                                 poniesNode.Expand()
                             End Sub)
        For Each directory In ponyBaseDirectories
            Dim ponyBase As New MutablePonyBase(directory)
            bases.Add(ponyBase.Directory, ponyBase)
            IdleWorker.QueueTask(Sub()
                                     Dim ponyBaseNode = New TreeNode(ponyBase.Directory) With
                                                        {.Tag = New PageRef(ponyBase, PageContent.Pony)}
                                     poniesNode.Nodes.Add(ponyBaseNode)

                                     Dim behaviorsNode = New TreeNode("Behaviors") With
                                                        {.Tag = New PageRef(ponyBase, PageContent.Behaviors)}
                                     ponyBaseNode.Nodes.Add(behaviorsNode)
                                     Dim effectsNode = New TreeNode("Effects") With
                                                           {.Tag = New PageRef(ponyBase, PageContent.Effects)}
                                     ponyBaseNode.Nodes.Add(effectsNode)
                                     Dim speechesNode = New TreeNode("Speeches") With
                                                        {.Tag = New PageRef(ponyBase, PageContent.Speeches)}
                                     ponyBaseNode.Nodes.Add(speechesNode)

                                     For Each behavior In ponyBase.Behaviors
                                         behaviorsNode.Nodes.Add(
                                             New TreeNode(behavior.Name) With {.Tag = New PageRef(ponyBase, PageContent.Behavior)})
                                     Next

                                     For Each effect In ponyBase.Effects
                                         effectsNode.Nodes.Add(
                                             New TreeNode(effect.Name) With {.Tag = New PageRef(ponyBase, PageContent.Effect)})
                                     Next

                                     For Each speech In ponyBase.SpeakingLines
                                         speechesNode.Nodes.Add(
                                             New TreeNode(speech.Name) With {.Tag = New PageRef(ponyBase, PageContent.Speech)})
                                     Next

                                     EditorProgressBar.Value += 1
                                 End Sub)
        Next
        idleWorker.QueueTask(Sub()
                                 EditorStatus.Text = "Ready"
                                 EditorProgressBar.Value = 0
                                 EditorProgressBar.Maximum = 0
                                 DocumentsView.TopNode.Expand()
                                 UseWaitCursor = False
                                 Enabled = True
                                 DocumentsView.Focus()
                             End Sub)
    End Sub

    Private Sub DocumentsView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles DocumentsView.AfterSelect
        
    End Sub

    Private Function GetTabText(pageRef As PageRef, itemName As String) As String
        Select Case pageRef.PageContent
            Case PageContent.Ponies
                Return itemName
            Case PageContent.Pony
                Return pageRef.PonyBase.Directory
            Case PageContent.Behaviors, PageContent.Effects, PageContent.Speeches
                Return pageRef.PonyBase.Directory & " - " & itemName
            Case PageContent.Behavior, PageContent.Effect, PageContent.Speech
                Return pageRef.PonyBase.Directory & ": " & itemName
            Case Else
                Throw New System.ComponentModel.InvalidEnumArgumentException("Unknown Content in pageRef")
        End Select
    End Function

    Private Sub DocumentsView_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles DocumentsView.NodeMouseDoubleClick
        OpenTab(e.Node)
    End Sub

    Private Sub DocumentsView_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DocumentsView.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            OpenTab(DocumentsView.SelectedNode)
            e.Handled = True
        End If
    End Sub

    Private Sub OpenTab(node As TreeNode)
        If node Is Nothing Then Return

        Dim page = Documents.TabPages.Item(node.FullPath)
        Dim itemName = node.Text

        If page Is Nothing Then
            Dim pageRef = DirectCast(node.Tag, PageRef)
            Dim pageControl As Control = Nothing
            Select Case pageRef.PageContent
                Case PageContent.Ponies
                    pageControl = New Control()
                Case PageContent.Pony
                    pageControl = New Control()
                Case PageContent.Behaviors
                    pageControl = New Control()
                Case PageContent.Behavior
                    Dim editor = New BehaviorEditor()
                    editor.LoadItem(pageRef.PonyBase, itemName)
                    pageControl = editor
                Case PageContent.Effects
                    pageControl = New Control()
                Case PageContent.Effect
                    Dim editor = New EffectEditor()
                    editor.LoadItem(pageRef.PonyBase, itemName)
                    pageControl = editor
                Case PageContent.Speeches
                    pageControl = New Control()
                Case PageContent.Speech
                    Dim editor = New SpeechEditor()
                    editor.LoadItem(pageRef.PonyBase, itemName)
                    pageControl = editor
            End Select
            pageControl.Dock = DockStyle.Fill
            page = New TabPage(GetTabText(pageRef, itemName)) With {.Name = node.FullPath}
            page.Controls.Add(pageControl)
            Documents.TabPages.Add(page)
        End If

        Documents.SelectedTab = page
        DocumentsView.Select()
        DocumentsView.SelectedNode = node
    End Sub
End Class