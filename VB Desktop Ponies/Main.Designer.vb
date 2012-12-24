<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.AnimationTimer = New System.Windows.Forms.Timer(Me.components)
        Me.PonySelectionPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.LoadingProgressBar = New System.Windows.Forms.ProgressBar()
        Me.TemplateLoader = New System.ComponentModel.BackgroundWorker()
        Me.PonyLoader = New System.ComponentModel.BackgroundWorker()
        Me.SelectionControlsPanel = New System.Windows.Forms.Panel()
        Me.PonyCountValueLabel = New System.Windows.Forms.Label()
        Me.DeleteProfileButton = New System.Windows.Forms.Button()
        Me.CopyProfileButton = New System.Windows.Forms.Button()
        Me.ProfileLabel = New System.Windows.Forms.Label()
        Me.ProfileComboBox = New System.Windows.Forms.ComboBox()
        Me.ZeroPoniesButton = New System.Windows.Forms.Button()
        Me.PonyCountLabel = New System.Windows.Forms.Label()
        Me.FilterGroupBox = New System.Windows.Forms.GroupBox()
        Me.FilterAllRadio = New System.Windows.Forms.RadioButton()
        Me.FilterOptionsBox = New System.Windows.Forms.CheckedListBox()
        Me.FilterAnyRadio = New System.Windows.Forms.RadioButton()
        Me.FilterExactlyRadio = New System.Windows.Forms.RadioButton()
        Me.GamesButton = New System.Windows.Forms.Button()
        Me.LoadProfileButton = New System.Windows.Forms.Button()
        Me.SaveProfileButton = New System.Windows.Forms.Button()
        Me.OnePoniesButton = New System.Windows.Forms.Button()
        Me.PonyEditorButton = New System.Windows.Forms.Button()
        Me.OptionsButton = New System.Windows.Forms.Button()
        Me.GoButton = New System.Windows.Forms.Button()
        Me.PonyPaginationPanel = New System.Windows.Forms.Panel()
        Me.PonyPaginationLabel = New System.Windows.Forms.Label()
        Me.LastPageButton = New System.Windows.Forms.Button()
        Me.NextPageButton = New System.Windows.Forms.Button()
        Me.NextPonyButton = New System.Windows.Forms.Button()
        Me.PreviousPonyButton = New System.Windows.Forms.Button()
        Me.PreviousPageButton = New System.Windows.Forms.Button()
        Me.FirstPageButton = New System.Windows.Forms.Button()
        Me.PoniesPerPageLabel = New System.Windows.Forms.Label()
        Me.PoniesPerPage = New System.Windows.Forms.NumericUpDown()
        Me.PaginationEnabled = New System.Windows.Forms.CheckBox()
        Me.SelectionControlsPanel.SuspendLayout()
        Me.FilterGroupBox.SuspendLayout()
        Me.PonyPaginationPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'AnimationTimer
        '
        Me.AnimationTimer.Interval = 33
        '
        'PonySelectionPanel
        '
        Me.PonySelectionPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonySelectionPanel.AutoScroll = True
        Me.PonySelectionPanel.Enabled = False
        Me.PonySelectionPanel.Location = New System.Drawing.Point(0, 30)
        Me.PonySelectionPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.PonySelectionPanel.Name = "PonySelectionPanel"
        Me.PonySelectionPanel.Size = New System.Drawing.Size(734, 458)
        Me.PonySelectionPanel.TabIndex = 2
        '
        'LoadingProgressBar
        '
        Me.LoadingProgressBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LoadingProgressBar.Location = New System.Drawing.Point(9, 465)
        Me.LoadingProgressBar.Margin = New System.Windows.Forms.Padding(0)
        Me.LoadingProgressBar.Name = "LoadingProgressBar"
        Me.LoadingProgressBar.Size = New System.Drawing.Size(716, 23)
        Me.LoadingProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.LoadingProgressBar.TabIndex = 3
        '
        'TemplateLoader
        '
        Me.TemplateLoader.WorkerReportsProgress = True
        '
        'PonyLoader
        '
        Me.PonyLoader.WorkerReportsProgress = True
        '
        'SelectionControlsPanel
        '
        Me.SelectionControlsPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SelectionControlsPanel.Controls.Add(Me.PonyCountValueLabel)
        Me.SelectionControlsPanel.Controls.Add(Me.DeleteProfileButton)
        Me.SelectionControlsPanel.Controls.Add(Me.CopyProfileButton)
        Me.SelectionControlsPanel.Controls.Add(Me.ProfileLabel)
        Me.SelectionControlsPanel.Controls.Add(Me.ProfileComboBox)
        Me.SelectionControlsPanel.Controls.Add(Me.ZeroPoniesButton)
        Me.SelectionControlsPanel.Controls.Add(Me.PonyCountLabel)
        Me.SelectionControlsPanel.Controls.Add(Me.FilterGroupBox)
        Me.SelectionControlsPanel.Controls.Add(Me.GamesButton)
        Me.SelectionControlsPanel.Controls.Add(Me.LoadProfileButton)
        Me.SelectionControlsPanel.Controls.Add(Me.SaveProfileButton)
        Me.SelectionControlsPanel.Controls.Add(Me.OnePoniesButton)
        Me.SelectionControlsPanel.Controls.Add(Me.PonyEditorButton)
        Me.SelectionControlsPanel.Controls.Add(Me.OptionsButton)
        Me.SelectionControlsPanel.Controls.Add(Me.GoButton)
        Me.SelectionControlsPanel.Enabled = False
        Me.SelectionControlsPanel.Location = New System.Drawing.Point(0, 491)
        Me.SelectionControlsPanel.Margin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.SelectionControlsPanel.Name = "SelectionControlsPanel"
        Me.SelectionControlsPanel.Size = New System.Drawing.Size(734, 121)
        Me.SelectionControlsPanel.TabIndex = 4
        '
        'PonyCountValueLabel
        '
        Me.PonyCountValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.PonyCountValueLabel.Location = New System.Drawing.Point(678, 66)
        Me.PonyCountValueLabel.Name = "PonyCountValueLabel"
        Me.PonyCountValueLabel.Size = New System.Drawing.Size(43, 13)
        Me.PonyCountValueLabel.TabIndex = 13
        Me.PonyCountValueLabel.Text = "0"
        Me.PonyCountValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'DeleteProfileButton
        '
        Me.DeleteProfileButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.DeleteProfileButton.Location = New System.Drawing.Point(236, 95)
        Me.DeleteProfileButton.Name = "DeleteProfileButton"
        Me.DeleteProfileButton.Size = New System.Drawing.Size(68, 23)
        Me.DeleteProfileButton.TabIndex = 8
        Me.DeleteProfileButton.Text = "Delete"
        Me.DeleteProfileButton.UseVisualStyleBackColor = True
        '
        'CopyProfileButton
        '
        Me.CopyProfileButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.CopyProfileButton.Location = New System.Drawing.Point(162, 95)
        Me.CopyProfileButton.Name = "CopyProfileButton"
        Me.CopyProfileButton.Size = New System.Drawing.Size(68, 23)
        Me.CopyProfileButton.TabIndex = 7
        Me.CopyProfileButton.Text = "Copy"
        Me.CopyProfileButton.UseVisualStyleBackColor = True
        '
        'ProfileLabel
        '
        Me.ProfileLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ProfileLabel.AutoSize = True
        Me.ProfileLabel.Location = New System.Drawing.Point(12, 68)
        Me.ProfileLabel.Name = "ProfileLabel"
        Me.ProfileLabel.Size = New System.Drawing.Size(84, 13)
        Me.ProfileLabel.TabIndex = 3
        Me.ProfileLabel.Text = "Selected Profile:"
        '
        'ProfileComboBox
        '
        Me.ProfileComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ProfileComboBox.Location = New System.Drawing.Point(102, 63)
        Me.ProfileComboBox.Name = "ProfileComboBox"
        Me.ProfileComboBox.Size = New System.Drawing.Size(202, 21)
        Me.ProfileComboBox.TabIndex = 4
        '
        'ZeroPoniesButton
        '
        Me.ZeroPoniesButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ZeroPoniesButton.Location = New System.Drawing.Point(622, 3)
        Me.ZeroPoniesButton.Name = "ZeroPoniesButton"
        Me.ZeroPoniesButton.Size = New System.Drawing.Size(100, 23)
        Me.ZeroPoniesButton.TabIndex = 10
        Me.ZeroPoniesButton.Text = "0 of All Ponies"
        Me.ZeroPoniesButton.UseVisualStyleBackColor = True
        '
        'PonyCountLabel
        '
        Me.PonyCountLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.PonyCountLabel.AutoSize = True
        Me.PonyCountLabel.Location = New System.Drawing.Point(604, 66)
        Me.PonyCountLabel.Name = "PonyCountLabel"
        Me.PonyCountLabel.Size = New System.Drawing.Size(69, 13)
        Me.PonyCountLabel.TabIndex = 12
        Me.PonyCountLabel.Text = "Total Ponies:"
        '
        'FilterGroupBox
        '
        Me.FilterGroupBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.FilterGroupBox.Controls.Add(Me.FilterAllRadio)
        Me.FilterGroupBox.Controls.Add(Me.FilterOptionsBox)
        Me.FilterGroupBox.Controls.Add(Me.FilterAnyRadio)
        Me.FilterGroupBox.Controls.Add(Me.FilterExactlyRadio)
        Me.FilterGroupBox.Location = New System.Drawing.Point(310, 3)
        Me.FilterGroupBox.Name = "FilterGroupBox"
        Me.FilterGroupBox.Size = New System.Drawing.Size(288, 115)
        Me.FilterGroupBox.TabIndex = 9
        Me.FilterGroupBox.TabStop = False
        Me.FilterGroupBox.Text = "Pony Filter"
        '
        'FilterAllRadio
        '
        Me.FilterAllRadio.AutoSize = True
        Me.FilterAllRadio.Checked = True
        Me.FilterAllRadio.Location = New System.Drawing.Point(6, 19)
        Me.FilterAllRadio.Name = "FilterAllRadio"
        Me.FilterAllRadio.Size = New System.Drawing.Size(66, 17)
        Me.FilterAllRadio.TabIndex = 0
        Me.FilterAllRadio.TabStop = True
        Me.FilterAllRadio.Text = "Show All"
        Me.FilterAllRadio.UseVisualStyleBackColor = True
        '
        'FilterOptionsBox
        '
        Me.FilterOptionsBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FilterOptionsBox.CheckOnClick = True
        Me.FilterOptionsBox.Enabled = False
        Me.FilterOptionsBox.FormattingEnabled = True
        Me.FilterOptionsBox.Items.AddRange(New Object() {"Ponies", "Supporting Ponies", "Alternate Art", "Fillies", "Pets", "Stallions", "Mares", "Alicorns", "Unicorns", "Pegasi", "Earth Ponies", "Non-Ponies", "Not Tagged"})
        Me.FilterOptionsBox.Location = New System.Drawing.Point(112, 14)
        Me.FilterOptionsBox.Name = "FilterOptionsBox"
        Me.FilterOptionsBox.Size = New System.Drawing.Size(170, 94)
        Me.FilterOptionsBox.TabIndex = 3
        '
        'FilterAnyRadio
        '
        Me.FilterAnyRadio.AutoSize = True
        Me.FilterAnyRadio.Location = New System.Drawing.Point(6, 42)
        Me.FilterAnyRadio.Name = "FilterAnyRadio"
        Me.FilterAnyRadio.Size = New System.Drawing.Size(105, 17)
        Me.FilterAnyRadio.TabIndex = 1
        Me.FilterAnyRadio.Text = "Any that match..."
        Me.FilterAnyRadio.UseVisualStyleBackColor = True
        '
        'FilterExactlyRadio
        '
        Me.FilterExactlyRadio.AutoSize = True
        Me.FilterExactlyRadio.Location = New System.Drawing.Point(6, 65)
        Me.FilterExactlyRadio.Name = "FilterExactlyRadio"
        Me.FilterExactlyRadio.Size = New System.Drawing.Size(68, 17)
        Me.FilterExactlyRadio.TabIndex = 2
        Me.FilterExactlyRadio.Text = "Exactly..."
        Me.FilterExactlyRadio.UseVisualStyleBackColor = True
        '
        'GamesButton
        '
        Me.GamesButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.GamesButton.Location = New System.Drawing.Point(208, 3)
        Me.GamesButton.Name = "GamesButton"
        Me.GamesButton.Size = New System.Drawing.Size(96, 23)
        Me.GamesButton.TabIndex = 2
        Me.GamesButton.Text = "MINI-GAMES"
        Me.GamesButton.UseVisualStyleBackColor = True
        '
        'LoadProfileButton
        '
        Me.LoadProfileButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LoadProfileButton.Location = New System.Drawing.Point(14, 95)
        Me.LoadProfileButton.Name = "LoadProfileButton"
        Me.LoadProfileButton.Size = New System.Drawing.Size(68, 23)
        Me.LoadProfileButton.TabIndex = 5
        Me.LoadProfileButton.Text = "Reload"
        Me.LoadProfileButton.UseVisualStyleBackColor = True
        '
        'SaveProfileButton
        '
        Me.SaveProfileButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.SaveProfileButton.Location = New System.Drawing.Point(88, 95)
        Me.SaveProfileButton.Name = "SaveProfileButton"
        Me.SaveProfileButton.Size = New System.Drawing.Size(68, 23)
        Me.SaveProfileButton.TabIndex = 6
        Me.SaveProfileButton.Text = "Save"
        Me.SaveProfileButton.UseVisualStyleBackColor = True
        '
        'OnePoniesButton
        '
        Me.OnePoniesButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.OnePoniesButton.Location = New System.Drawing.Point(622, 32)
        Me.OnePoniesButton.Name = "OnePoniesButton"
        Me.OnePoniesButton.Size = New System.Drawing.Size(100, 23)
        Me.OnePoniesButton.TabIndex = 11
        Me.OnePoniesButton.Text = "1 of All Ponies"
        Me.OnePoniesButton.UseVisualStyleBackColor = True
        '
        'PonyEditorButton
        '
        Me.PonyEditorButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.PonyEditorButton.Location = New System.Drawing.Point(110, 3)
        Me.PonyEditorButton.Name = "PonyEditorButton"
        Me.PonyEditorButton.Size = New System.Drawing.Size(92, 23)
        Me.PonyEditorButton.TabIndex = 1
        Me.PonyEditorButton.Text = "PONY EDITOR"
        Me.PonyEditorButton.UseVisualStyleBackColor = True
        '
        'OptionsButton
        '
        Me.OptionsButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.OptionsButton.Location = New System.Drawing.Point(12, 3)
        Me.OptionsButton.Name = "OptionsButton"
        Me.OptionsButton.Size = New System.Drawing.Size(92, 23)
        Me.OptionsButton.TabIndex = 0
        Me.OptionsButton.Text = "OPTIONS"
        Me.OptionsButton.UseVisualStyleBackColor = True
        '
        'GoButton
        '
        Me.GoButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.GoButton.Location = New System.Drawing.Point(604, 95)
        Me.GoButton.Name = "GoButton"
        Me.GoButton.Size = New System.Drawing.Size(118, 23)
        Me.GoButton.TabIndex = 14
        Me.GoButton.Text = "GIVE ME PONIES!"
        Me.GoButton.UseVisualStyleBackColor = True
        '
        'PonyPaginationPanel
        '
        Me.PonyPaginationPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonyPaginationPanel.Controls.Add(Me.PonyPaginationLabel)
        Me.PonyPaginationPanel.Controls.Add(Me.LastPageButton)
        Me.PonyPaginationPanel.Controls.Add(Me.NextPageButton)
        Me.PonyPaginationPanel.Controls.Add(Me.NextPonyButton)
        Me.PonyPaginationPanel.Controls.Add(Me.PreviousPonyButton)
        Me.PonyPaginationPanel.Controls.Add(Me.PreviousPageButton)
        Me.PonyPaginationPanel.Controls.Add(Me.FirstPageButton)
        Me.PonyPaginationPanel.Controls.Add(Me.PoniesPerPageLabel)
        Me.PonyPaginationPanel.Controls.Add(Me.PoniesPerPage)
        Me.PonyPaginationPanel.Enabled = False
        Me.PonyPaginationPanel.Location = New System.Drawing.Point(127, 0)
        Me.PonyPaginationPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.PonyPaginationPanel.Name = "PonyPaginationPanel"
        Me.PonyPaginationPanel.Size = New System.Drawing.Size(607, 30)
        Me.PonyPaginationPanel.TabIndex = 1
        '
        'PonyPaginationLabel
        '
        Me.PonyPaginationLabel.AutoSize = True
        Me.PonyPaginationLabel.Location = New System.Drawing.Point(358, 9)
        Me.PonyPaginationLabel.Name = "PonyPaginationLabel"
        Me.PonyPaginationLabel.Size = New System.Drawing.Size(88, 13)
        Me.PonyPaginationLabel.TabIndex = 8
        Me.PonyPaginationLabel.Text = "Loading ponies..."
        '
        'LastPageButton
        '
        Me.LastPageButton.Location = New System.Drawing.Point(324, 4)
        Me.LastPageButton.Name = "LastPageButton"
        Me.LastPageButton.Size = New System.Drawing.Size(28, 22)
        Me.LastPageButton.TabIndex = 7
        Me.LastPageButton.Text = ">|"
        Me.LastPageButton.UseVisualStyleBackColor = True
        '
        'NextPageButton
        '
        Me.NextPageButton.Location = New System.Drawing.Point(290, 4)
        Me.NextPageButton.Name = "NextPageButton"
        Me.NextPageButton.Size = New System.Drawing.Size(28, 22)
        Me.NextPageButton.TabIndex = 6
        Me.NextPageButton.Text = ">>"
        Me.NextPageButton.UseVisualStyleBackColor = True
        '
        'NextPonyButton
        '
        Me.NextPonyButton.Location = New System.Drawing.Point(256, 4)
        Me.NextPonyButton.Name = "NextPonyButton"
        Me.NextPonyButton.Size = New System.Drawing.Size(28, 22)
        Me.NextPonyButton.TabIndex = 5
        Me.NextPonyButton.Text = ">"
        Me.NextPonyButton.UseVisualStyleBackColor = True
        '
        'PreviousPonyButton
        '
        Me.PreviousPonyButton.Location = New System.Drawing.Point(222, 4)
        Me.PreviousPonyButton.Name = "PreviousPonyButton"
        Me.PreviousPonyButton.Size = New System.Drawing.Size(28, 22)
        Me.PreviousPonyButton.TabIndex = 4
        Me.PreviousPonyButton.Text = "<"
        Me.PreviousPonyButton.UseVisualStyleBackColor = True
        '
        'PreviousPageButton
        '
        Me.PreviousPageButton.Location = New System.Drawing.Point(188, 4)
        Me.PreviousPageButton.Name = "PreviousPageButton"
        Me.PreviousPageButton.Size = New System.Drawing.Size(28, 22)
        Me.PreviousPageButton.TabIndex = 3
        Me.PreviousPageButton.Text = "<<"
        Me.PreviousPageButton.UseVisualStyleBackColor = True
        '
        'FirstPageButton
        '
        Me.FirstPageButton.Location = New System.Drawing.Point(154, 4)
        Me.FirstPageButton.Name = "FirstPageButton"
        Me.FirstPageButton.Size = New System.Drawing.Size(28, 22)
        Me.FirstPageButton.TabIndex = 2
        Me.FirstPageButton.Text = "|<"
        Me.FirstPageButton.UseVisualStyleBackColor = True
        '
        'PoniesPerPageLabel
        '
        Me.PoniesPerPageLabel.AutoSize = True
        Me.PoniesPerPageLabel.Location = New System.Drawing.Point(3, 9)
        Me.PoniesPerPageLabel.Name = "PoniesPerPageLabel"
        Me.PoniesPerPageLabel.Size = New System.Drawing.Size(88, 13)
        Me.PoniesPerPageLabel.TabIndex = 0
        Me.PoniesPerPageLabel.Text = "Ponies per Page:"
        '
        'PoniesPerPage
        '
        Me.PoniesPerPage.Location = New System.Drawing.Point(98, 5)
        Me.PoniesPerPage.Name = "PoniesPerPage"
        Me.PoniesPerPage.Size = New System.Drawing.Size(50, 20)
        Me.PoniesPerPage.TabIndex = 1
        Me.PoniesPerPage.Value = New Decimal(New Integer() {7, 0, 0, 0})
        '
        'PaginationEnabled
        '
        Me.PaginationEnabled.AutoSize = True
        Me.PaginationEnabled.Location = New System.Drawing.Point(12, 8)
        Me.PaginationEnabled.Name = "PaginationEnabled"
        Me.PaginationEnabled.Size = New System.Drawing.Size(112, 17)
        Me.PaginationEnabled.TabIndex = 0
        Me.PaginationEnabled.Text = "Enable Pagination"
        Me.PaginationEnabled.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(734, 612)
        Me.Controls.Add(Me.PaginationEnabled)
        Me.Controls.Add(Me.PonyPaginationPanel)
        Me.Controls.Add(Me.LoadingProgressBar)
        Me.Controls.Add(Me.PonySelectionPanel)
        Me.Controls.Add(Me.SelectionControlsPanel)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(750, 350)
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Desktop Ponies"
        Me.SelectionControlsPanel.ResumeLayout(False)
        Me.SelectionControlsPanel.PerformLayout()
        Me.FilterGroupBox.ResumeLayout(False)
        Me.FilterGroupBox.PerformLayout()
        Me.PonyPaginationPanel.ResumeLayout(False)
        Me.PonyPaginationPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AnimationTimer As System.Windows.Forms.Timer
    Friend WithEvents LoadingProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents TemplateLoader As System.ComponentModel.BackgroundWorker
    Friend WithEvents PonyLoader As System.ComponentModel.BackgroundWorker
    Friend WithEvents PonySelectionPanel As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents SelectionControlsPanel As System.Windows.Forms.Panel
    Friend WithEvents PonyCountValueLabel As System.Windows.Forms.Label
    Friend WithEvents DeleteProfileButton As System.Windows.Forms.Button
    Friend WithEvents CopyProfileButton As System.Windows.Forms.Button
    Friend WithEvents ProfileLabel As System.Windows.Forms.Label
    Friend WithEvents ProfileComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents ZeroPoniesButton As System.Windows.Forms.Button
    Friend WithEvents PonyCountLabel As System.Windows.Forms.Label
    Friend WithEvents FilterGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents FilterAllRadio As System.Windows.Forms.RadioButton
    Friend WithEvents FilterOptionsBox As System.Windows.Forms.CheckedListBox
    Friend WithEvents FilterAnyRadio As System.Windows.Forms.RadioButton
    Friend WithEvents FilterExactlyRadio As System.Windows.Forms.RadioButton
    Friend WithEvents GamesButton As System.Windows.Forms.Button
    Friend WithEvents LoadProfileButton As System.Windows.Forms.Button
    Friend WithEvents SaveProfileButton As System.Windows.Forms.Button
    Friend WithEvents OnePoniesButton As System.Windows.Forms.Button
    Friend WithEvents PonyEditorButton As System.Windows.Forms.Button
    Friend WithEvents OptionsButton As System.Windows.Forms.Button
    Friend WithEvents GoButton As System.Windows.Forms.Button
    Friend WithEvents PonyPaginationPanel As System.Windows.Forms.Panel
    Friend WithEvents LastPageButton As System.Windows.Forms.Button
    Friend WithEvents NextPageButton As System.Windows.Forms.Button
    Friend WithEvents NextPonyButton As System.Windows.Forms.Button
    Friend WithEvents PreviousPonyButton As System.Windows.Forms.Button
    Friend WithEvents PreviousPageButton As System.Windows.Forms.Button
    Friend WithEvents FirstPageButton As System.Windows.Forms.Button
    Friend WithEvents PoniesPerPageLabel As System.Windows.Forms.Label
    Friend WithEvents PoniesPerPage As System.Windows.Forms.NumericUpDown
    Friend WithEvents PonyPaginationLabel As System.Windows.Forms.Label
    Friend WithEvents PaginationEnabled As System.Windows.Forms.CheckBox

End Class
