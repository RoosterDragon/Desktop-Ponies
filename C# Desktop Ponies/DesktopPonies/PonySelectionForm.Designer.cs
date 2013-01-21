namespace CSDesktopPonies.DesktopPonies
{
    partial class PonySelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.CommandsGroup = new System.Windows.Forms.GroupBox();
            this.ProgressBarPanel = new System.Windows.Forms.Panel();
            this.ProgressBar2 = new System.Windows.Forms.ProgressBar();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.GifAlphaCommand = new System.Windows.Forms.Button();
            this.GifViewerCommand = new System.Windows.Forms.Button();
            this.LoadTimeInfo = new System.Windows.Forms.Label();
            this.PonyEditorCommand = new System.Windows.Forms.Button();
            this.LoadInfo = new System.Windows.Forms.Label();
            this.RunCommand = new System.Windows.Forms.Button();
            this.SetAllLabel = new System.Windows.Forms.Label();
            this.PonyCountInfo = new System.Windows.Forms.Label();
            this.SetAll0Command = new System.Windows.Forms.Button();
            this.SetAll1Command = new System.Windows.Forms.Button();
            this.PoniesGroup = new System.Windows.Forms.GroupBox();
            this.SetVisibleLabel = new System.Windows.Forms.Label();
            this.SetVisible1Command = new System.Windows.Forms.Button();
            this.SetVisible0Command = new System.Windows.Forms.Button();
            this.PonyDisplayPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.FiltersGroup = new System.Windows.Forms.GroupBox();
            this.GenderFiltersGroup = new System.Windows.Forms.GroupBox();
            this.GenderMaleOption = new System.Windows.Forms.CheckBox();
            this.GenderFemaleOption = new System.Windows.Forms.CheckBox();
            this.RaceFiltersGroup = new System.Windows.Forms.GroupBox();
            this.RaceNonPonyOption = new System.Windows.Forms.CheckBox();
            this.RaceEarthOption = new System.Windows.Forms.CheckBox();
            this.RacePegasusOption = new System.Windows.Forms.CheckBox();
            this.RaceUnicornOption = new System.Windows.Forms.CheckBox();
            this.RaceAlicornOption = new System.Windows.Forms.CheckBox();
            this.RoleFiltersGroup = new System.Windows.Forms.GroupBox();
            this.RoleOtherOption = new System.Windows.Forms.CheckBox();
            this.RoleBackgroundOption = new System.Windows.Forms.CheckBox();
            this.RoleIncidentalOption = new System.Windows.Forms.CheckBox();
            this.RoleSupportOption = new System.Windows.Forms.CheckBox();
            this.RoleMainOption = new System.Windows.Forms.CheckBox();
            this.InterfacesGroup = new System.Windows.Forms.GroupBox();
            this.InterfaceGtkOption = new System.Windows.Forms.RadioButton();
            this.InterfaceWinFormOption = new System.Windows.Forms.RadioButton();
            this.LoadTemplatesWorker = new System.ComponentModel.BackgroundWorker();
            this.LoadInstancesWorker = new System.ComponentModel.BackgroundWorker();
            this.FiltersAndInterfacePanel = new System.Windows.Forms.Panel();
            this.AnimationTimer = new System.Windows.Forms.Timer(this.components);
            this.CommandsGroup.SuspendLayout();
            this.ProgressBarPanel.SuspendLayout();
            this.PoniesGroup.SuspendLayout();
            this.FiltersGroup.SuspendLayout();
            this.GenderFiltersGroup.SuspendLayout();
            this.RaceFiltersGroup.SuspendLayout();
            this.RoleFiltersGroup.SuspendLayout();
            this.InterfacesGroup.SuspendLayout();
            this.FiltersAndInterfacePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CommandsGroup
            // 
            this.CommandsGroup.AutoSize = true;
            this.CommandsGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CommandsGroup.Controls.Add(this.ProgressBarPanel);
            this.CommandsGroup.Controls.Add(this.GifAlphaCommand);
            this.CommandsGroup.Controls.Add(this.GifViewerCommand);
            this.CommandsGroup.Controls.Add(this.LoadTimeInfo);
            this.CommandsGroup.Controls.Add(this.PonyEditorCommand);
            this.CommandsGroup.Controls.Add(this.LoadInfo);
            this.CommandsGroup.Controls.Add(this.RunCommand);
            this.CommandsGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.CommandsGroup.Location = new System.Drawing.Point(10, 6);
            this.CommandsGroup.Name = "CommandsGroup";
            this.CommandsGroup.Size = new System.Drawing.Size(613, 87);
            this.CommandsGroup.TabIndex = 0;
            this.CommandsGroup.TabStop = false;
            this.CommandsGroup.Text = "Commands";
            // 
            // ProgressBarPanel
            // 
            this.ProgressBarPanel.Controls.Add(this.ProgressBar2);
            this.ProgressBarPanel.Controls.Add(this.ProgressBar1);
            this.ProgressBarPanel.Location = new System.Drawing.Point(207, 45);
            this.ProgressBarPanel.Name = "ProgressBarPanel";
            this.ProgressBarPanel.Size = new System.Drawing.Size(400, 23);
            this.ProgressBarPanel.TabIndex = 6;
            // 
            // ProgressBar2
            // 
            this.ProgressBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar2.Location = new System.Drawing.Point(400, 0);
            this.ProgressBar2.Margin = new System.Windows.Forms.Padding(0);
            this.ProgressBar2.Name = "ProgressBar2";
            this.ProgressBar2.Size = new System.Drawing.Size(0, 23);
            this.ProgressBar2.TabIndex = 1;
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ProgressBar1.Location = new System.Drawing.Point(0, 0);
            this.ProgressBar1.Margin = new System.Windows.Forms.Padding(0);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(400, 23);
            this.ProgressBar1.TabIndex = 0;
            // 
            // GifAlphaCommand
            // 
            this.GifAlphaCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GifAlphaCommand.Location = new System.Drawing.Point(189, 19);
            this.GifAlphaCommand.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.GifAlphaCommand.Name = "GifAlphaCommand";
            this.GifAlphaCommand.Size = new System.Drawing.Size(100, 23);
            this.GifAlphaCommand.TabIndex = 2;
            this.GifAlphaCommand.Text = "Gif Alpha";
            this.GifAlphaCommand.UseVisualStyleBackColor = true;
            this.GifAlphaCommand.Visible = false;
            this.GifAlphaCommand.Click += new System.EventHandler(this.GifAlphaCommand_Click);
            // 
            // GifViewerCommand
            // 
            this.GifViewerCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GifViewerCommand.Location = new System.Drawing.Point(295, 19);
            this.GifViewerCommand.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.GifViewerCommand.Name = "GifViewerCommand";
            this.GifViewerCommand.Size = new System.Drawing.Size(100, 23);
            this.GifViewerCommand.TabIndex = 3;
            this.GifViewerCommand.Text = "Gif Viewer";
            this.GifViewerCommand.UseVisualStyleBackColor = true;
            this.GifViewerCommand.Visible = false;
            this.GifViewerCommand.Click += new System.EventHandler(this.GifViewerCommand_Click);
            // 
            // LoadTimeInfo
            // 
            this.LoadTimeInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LoadTimeInfo.AutoSize = true;
            this.LoadTimeInfo.Location = new System.Drawing.Point(6, 40);
            this.LoadTimeInfo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.LoadTimeInfo.Name = "LoadTimeInfo";
            this.LoadTimeInfo.Size = new System.Drawing.Size(28, 13);
            this.LoadTimeInfo.TabIndex = 1;
            this.LoadTimeInfo.Text = "0:00";
            // 
            // PonyEditorCommand
            // 
            this.PonyEditorCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PonyEditorCommand.Location = new System.Drawing.Point(401, 19);
            this.PonyEditorCommand.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.PonyEditorCommand.Name = "PonyEditorCommand";
            this.PonyEditorCommand.Size = new System.Drawing.Size(100, 23);
            this.PonyEditorCommand.TabIndex = 4;
            this.PonyEditorCommand.Text = "Pony Viewer";
            this.PonyEditorCommand.UseVisualStyleBackColor = true;
            this.PonyEditorCommand.Visible = false;
            this.PonyEditorCommand.Click += new System.EventHandler(this.PonyEditorCommand_Click);
            // 
            // LoadInfo
            // 
            this.LoadInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LoadInfo.AutoSize = true;
            this.LoadInfo.Location = new System.Drawing.Point(6, 24);
            this.LoadInfo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.LoadInfo.Name = "LoadInfo";
            this.LoadInfo.Size = new System.Drawing.Size(54, 13);
            this.LoadInfo.TabIndex = 0;
            this.LoadInfo.Text = "Loading...";
            // 
            // RunCommand
            // 
            this.RunCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RunCommand.Enabled = false;
            this.RunCommand.Location = new System.Drawing.Point(507, 19);
            this.RunCommand.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.RunCommand.Name = "RunCommand";
            this.RunCommand.Size = new System.Drawing.Size(100, 23);
            this.RunCommand.TabIndex = 5;
            this.RunCommand.Text = "START";
            this.RunCommand.UseVisualStyleBackColor = true;
            this.RunCommand.Click += new System.EventHandler(this.RunCommand_Click);
            // 
            // SetAllLabel
            // 
            this.SetAllLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetAllLabel.AutoSize = true;
            this.SetAllLabel.Location = new System.Drawing.Point(435, 24);
            this.SetAllLabel.Name = "SetAllLabel";
            this.SetAllLabel.Size = new System.Drawing.Size(74, 13);
            this.SetAllLabel.TabIndex = 4;
            this.SetAllLabel.Text = "Set all counts:";
            // 
            // PonyCountInfo
            // 
            this.PonyCountInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PonyCountInfo.AutoSize = true;
            this.PonyCountInfo.Location = new System.Drawing.Point(6, 24);
            this.PonyCountInfo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.PonyCountInfo.Name = "PonyCountInfo";
            this.PonyCountInfo.Size = new System.Drawing.Size(165, 13);
            this.PonyCountInfo.TabIndex = 0;
            this.PonyCountInfo.Text = "Pony Templates: 0 Pony Count: 0";
            // 
            // SetAll0Command
            // 
            this.SetAll0Command.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetAll0Command.AutoSize = true;
            this.SetAll0Command.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SetAll0Command.Location = new System.Drawing.Point(515, 19);
            this.SetAll0Command.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.SetAll0Command.Name = "SetAll0Command";
            this.SetAll0Command.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.SetAll0Command.Size = new System.Drawing.Size(43, 23);
            this.SetAll0Command.TabIndex = 5;
            this.SetAll0Command.Text = "0";
            this.SetAll0Command.UseVisualStyleBackColor = true;
            this.SetAll0Command.Click += new System.EventHandler(this.SetCounts_Click);
            // 
            // SetAll1Command
            // 
            this.SetAll1Command.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetAll1Command.AutoSize = true;
            this.SetAll1Command.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SetAll1Command.Location = new System.Drawing.Point(564, 19);
            this.SetAll1Command.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.SetAll1Command.Name = "SetAll1Command";
            this.SetAll1Command.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.SetAll1Command.Size = new System.Drawing.Size(43, 23);
            this.SetAll1Command.TabIndex = 6;
            this.SetAll1Command.Text = "1";
            this.SetAll1Command.UseVisualStyleBackColor = true;
            this.SetAll1Command.Click += new System.EventHandler(this.SetCounts_Click);
            // 
            // PoniesGroup
            // 
            this.PoniesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PoniesGroup.Controls.Add(this.PonyCountInfo);
            this.PoniesGroup.Controls.Add(this.SetVisibleLabel);
            this.PoniesGroup.Controls.Add(this.SetVisible1Command);
            this.PoniesGroup.Controls.Add(this.SetVisible0Command);
            this.PoniesGroup.Controls.Add(this.SetAllLabel);
            this.PoniesGroup.Controls.Add(this.PonyDisplayPanel);
            this.PoniesGroup.Controls.Add(this.SetAll1Command);
            this.PoniesGroup.Controls.Add(this.SetAll0Command);
            this.PoniesGroup.Enabled = false;
            this.PoniesGroup.Location = new System.Drawing.Point(10, 219);
            this.PoniesGroup.Name = "PoniesGroup";
            this.PoniesGroup.Size = new System.Drawing.Size(613, 349);
            this.PoniesGroup.TabIndex = 2;
            this.PoniesGroup.TabStop = false;
            this.PoniesGroup.Text = "Ponies";
            // 
            // SetVisibleLabel
            // 
            this.SetVisibleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetVisibleLabel.AutoSize = true;
            this.SetVisibleLabel.Location = new System.Drawing.Point(238, 24);
            this.SetVisibleLabel.Name = "SetVisibleLabel";
            this.SetVisibleLabel.Size = new System.Drawing.Size(93, 13);
            this.SetVisibleLabel.TabIndex = 1;
            this.SetVisibleLabel.Text = "Set visible counts:";
            // 
            // SetVisible1Command
            // 
            this.SetVisible1Command.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetVisible1Command.AutoSize = true;
            this.SetVisible1Command.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SetVisible1Command.Location = new System.Drawing.Point(386, 19);
            this.SetVisible1Command.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.SetVisible1Command.Name = "SetVisible1Command";
            this.SetVisible1Command.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.SetVisible1Command.Size = new System.Drawing.Size(43, 23);
            this.SetVisible1Command.TabIndex = 3;
            this.SetVisible1Command.Text = "1";
            this.SetVisible1Command.UseVisualStyleBackColor = true;
            this.SetVisible1Command.Click += new System.EventHandler(this.SetCounts_Click);
            // 
            // SetVisible0Command
            // 
            this.SetVisible0Command.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetVisible0Command.AutoSize = true;
            this.SetVisible0Command.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SetVisible0Command.Location = new System.Drawing.Point(337, 19);
            this.SetVisible0Command.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.SetVisible0Command.Name = "SetVisible0Command";
            this.SetVisible0Command.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.SetVisible0Command.Size = new System.Drawing.Size(43, 23);
            this.SetVisible0Command.TabIndex = 2;
            this.SetVisible0Command.Text = "0";
            this.SetVisible0Command.UseVisualStyleBackColor = true;
            this.SetVisible0Command.Click += new System.EventHandler(this.SetCounts_Click);
            // 
            // PonyDisplayPanel
            // 
            this.PonyDisplayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PonyDisplayPanel.AutoScroll = true;
            this.PonyDisplayPanel.Location = new System.Drawing.Point(3, 45);
            this.PonyDisplayPanel.Name = "PonyDisplayPanel";
            this.PonyDisplayPanel.Size = new System.Drawing.Size(607, 301);
            this.PonyDisplayPanel.TabIndex = 7;
            this.PonyDisplayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.PonyDisplayPanel_Paint);
            this.PonyDisplayPanel.Resize += new System.EventHandler(this.PonyDisplayPanel_Resize);
            // 
            // FiltersGroup
            // 
            this.FiltersGroup.Controls.Add(this.GenderFiltersGroup);
            this.FiltersGroup.Controls.Add(this.RaceFiltersGroup);
            this.FiltersGroup.Controls.Add(this.RoleFiltersGroup);
            this.FiltersGroup.Location = new System.Drawing.Point(0, 0);
            this.FiltersGroup.Name = "FiltersGroup";
            this.FiltersGroup.Size = new System.Drawing.Size(501, 117);
            this.FiltersGroup.TabIndex = 0;
            this.FiltersGroup.TabStop = false;
            this.FiltersGroup.Text = "Filters";
            // 
            // GenderFiltersGroup
            // 
            this.GenderFiltersGroup.Controls.Add(this.GenderMaleOption);
            this.GenderFiltersGroup.Controls.Add(this.GenderFemaleOption);
            this.GenderFiltersGroup.Location = new System.Drawing.Point(368, 19);
            this.GenderFiltersGroup.Name = "GenderFiltersGroup";
            this.GenderFiltersGroup.Size = new System.Drawing.Size(127, 42);
            this.GenderFiltersGroup.TabIndex = 1;
            this.GenderFiltersGroup.TabStop = false;
            this.GenderFiltersGroup.Text = "Gender";
            // 
            // GenderMaleOption
            // 
            this.GenderMaleOption.AutoSize = true;
            this.GenderMaleOption.Checked = true;
            this.GenderMaleOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GenderMaleOption.Location = new System.Drawing.Point(72, 19);
            this.GenderMaleOption.Name = "GenderMaleOption";
            this.GenderMaleOption.Size = new System.Drawing.Size(49, 17);
            this.GenderMaleOption.TabIndex = 1;
            this.GenderMaleOption.Text = "Male";
            this.GenderMaleOption.UseVisualStyleBackColor = true;
            this.GenderMaleOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // GenderFemaleOption
            // 
            this.GenderFemaleOption.AutoSize = true;
            this.GenderFemaleOption.Checked = true;
            this.GenderFemaleOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GenderFemaleOption.Location = new System.Drawing.Point(6, 19);
            this.GenderFemaleOption.Name = "GenderFemaleOption";
            this.GenderFemaleOption.Size = new System.Drawing.Size(60, 17);
            this.GenderFemaleOption.TabIndex = 0;
            this.GenderFemaleOption.Text = "Female";
            this.GenderFemaleOption.UseVisualStyleBackColor = true;
            this.GenderFemaleOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // RaceFiltersGroup
            // 
            this.RaceFiltersGroup.Controls.Add(this.RaceNonPonyOption);
            this.RaceFiltersGroup.Controls.Add(this.RaceEarthOption);
            this.RaceFiltersGroup.Controls.Add(this.RacePegasusOption);
            this.RaceFiltersGroup.Controls.Add(this.RaceUnicornOption);
            this.RaceFiltersGroup.Controls.Add(this.RaceAlicornOption);
            this.RaceFiltersGroup.Location = new System.Drawing.Point(6, 67);
            this.RaceFiltersGroup.Name = "RaceFiltersGroup";
            this.RaceFiltersGroup.Size = new System.Drawing.Size(392, 42);
            this.RaceFiltersGroup.TabIndex = 2;
            this.RaceFiltersGroup.TabStop = false;
            this.RaceFiltersGroup.Text = "Race";
            // 
            // RaceNonPonyOption
            // 
            this.RaceNonPonyOption.AutoSize = true;
            this.RaceNonPonyOption.Checked = true;
            this.RaceNonPonyOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RaceNonPonyOption.Location = new System.Drawing.Point(305, 19);
            this.RaceNonPonyOption.Name = "RaceNonPonyOption";
            this.RaceNonPonyOption.Size = new System.Drawing.Size(81, 17);
            this.RaceNonPonyOption.TabIndex = 4;
            this.RaceNonPonyOption.Text = "Non Ponies";
            this.RaceNonPonyOption.UseVisualStyleBackColor = true;
            this.RaceNonPonyOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // RaceEarthOption
            // 
            this.RaceEarthOption.AutoSize = true;
            this.RaceEarthOption.Checked = true;
            this.RaceEarthOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RaceEarthOption.Location = new System.Drawing.Point(213, 19);
            this.RaceEarthOption.Name = "RaceEarthOption";
            this.RaceEarthOption.Size = new System.Drawing.Size(86, 17);
            this.RaceEarthOption.TabIndex = 3;
            this.RaceEarthOption.Text = "Earth Ponies";
            this.RaceEarthOption.UseVisualStyleBackColor = true;
            this.RaceEarthOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // RacePegasusOption
            // 
            this.RacePegasusOption.AutoSize = true;
            this.RacePegasusOption.Checked = true;
            this.RacePegasusOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RacePegasusOption.Location = new System.Drawing.Point(149, 19);
            this.RacePegasusOption.Name = "RacePegasusOption";
            this.RacePegasusOption.Size = new System.Drawing.Size(58, 17);
            this.RacePegasusOption.TabIndex = 2;
            this.RacePegasusOption.Text = "Pegasi";
            this.RacePegasusOption.UseVisualStyleBackColor = true;
            this.RacePegasusOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // RaceUnicornOption
            // 
            this.RaceUnicornOption.AutoSize = true;
            this.RaceUnicornOption.Checked = true;
            this.RaceUnicornOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RaceUnicornOption.Location = new System.Drawing.Point(75, 19);
            this.RaceUnicornOption.Name = "RaceUnicornOption";
            this.RaceUnicornOption.Size = new System.Drawing.Size(68, 17);
            this.RaceUnicornOption.TabIndex = 1;
            this.RaceUnicornOption.Text = "Unicorns";
            this.RaceUnicornOption.UseVisualStyleBackColor = true;
            this.RaceUnicornOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // RaceAlicornOption
            // 
            this.RaceAlicornOption.AutoSize = true;
            this.RaceAlicornOption.Checked = true;
            this.RaceAlicornOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RaceAlicornOption.Location = new System.Drawing.Point(6, 19);
            this.RaceAlicornOption.Name = "RaceAlicornOption";
            this.RaceAlicornOption.Size = new System.Drawing.Size(63, 17);
            this.RaceAlicornOption.TabIndex = 0;
            this.RaceAlicornOption.Text = "Alicorns";
            this.RaceAlicornOption.UseVisualStyleBackColor = true;
            this.RaceAlicornOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // RoleFiltersGroup
            // 
            this.RoleFiltersGroup.Controls.Add(this.RoleOtherOption);
            this.RoleFiltersGroup.Controls.Add(this.RoleBackgroundOption);
            this.RoleFiltersGroup.Controls.Add(this.RoleIncidentalOption);
            this.RoleFiltersGroup.Controls.Add(this.RoleSupportOption);
            this.RoleFiltersGroup.Controls.Add(this.RoleMainOption);
            this.RoleFiltersGroup.Location = new System.Drawing.Point(6, 19);
            this.RoleFiltersGroup.Name = "RoleFiltersGroup";
            this.RoleFiltersGroup.Size = new System.Drawing.Size(356, 42);
            this.RoleFiltersGroup.TabIndex = 0;
            this.RoleFiltersGroup.TabStop = false;
            this.RoleFiltersGroup.Text = "Role";
            // 
            // RoleOtherOption
            // 
            this.RoleOtherOption.AutoSize = true;
            this.RoleOtherOption.Checked = true;
            this.RoleOtherOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RoleOtherOption.Location = new System.Drawing.Point(298, 19);
            this.RoleOtherOption.Name = "RoleOtherOption";
            this.RoleOtherOption.Size = new System.Drawing.Size(52, 17);
            this.RoleOtherOption.TabIndex = 4;
            this.RoleOtherOption.Text = "Other";
            this.RoleOtherOption.UseVisualStyleBackColor = true;
            this.RoleOtherOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // RoleBackgroundOption
            // 
            this.RoleBackgroundOption.AutoSize = true;
            this.RoleBackgroundOption.Checked = true;
            this.RoleBackgroundOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RoleBackgroundOption.Location = new System.Drawing.Point(208, 19);
            this.RoleBackgroundOption.Name = "RoleBackgroundOption";
            this.RoleBackgroundOption.Size = new System.Drawing.Size(84, 17);
            this.RoleBackgroundOption.TabIndex = 3;
            this.RoleBackgroundOption.Text = "Background";
            this.RoleBackgroundOption.UseVisualStyleBackColor = true;
            this.RoleBackgroundOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // RoleIncidentalOption
            // 
            this.RoleIncidentalOption.AutoSize = true;
            this.RoleIncidentalOption.Checked = true;
            this.RoleIncidentalOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RoleIncidentalOption.Location = new System.Drawing.Point(130, 19);
            this.RoleIncidentalOption.Name = "RoleIncidentalOption";
            this.RoleIncidentalOption.Size = new System.Drawing.Size(72, 17);
            this.RoleIncidentalOption.TabIndex = 2;
            this.RoleIncidentalOption.Text = "Incidental";
            this.RoleIncidentalOption.UseVisualStyleBackColor = true;
            this.RoleIncidentalOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // RoleSupportOption
            // 
            this.RoleSupportOption.AutoSize = true;
            this.RoleSupportOption.Checked = true;
            this.RoleSupportOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RoleSupportOption.Location = new System.Drawing.Point(61, 19);
            this.RoleSupportOption.Name = "RoleSupportOption";
            this.RoleSupportOption.Size = new System.Drawing.Size(63, 17);
            this.RoleSupportOption.TabIndex = 1;
            this.RoleSupportOption.Text = "Support";
            this.RoleSupportOption.UseVisualStyleBackColor = true;
            this.RoleSupportOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // RoleMainOption
            // 
            this.RoleMainOption.AutoSize = true;
            this.RoleMainOption.Checked = true;
            this.RoleMainOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RoleMainOption.Location = new System.Drawing.Point(6, 19);
            this.RoleMainOption.Name = "RoleMainOption";
            this.RoleMainOption.Size = new System.Drawing.Size(49, 17);
            this.RoleMainOption.TabIndex = 0;
            this.RoleMainOption.Text = "Main";
            this.RoleMainOption.UseVisualStyleBackColor = true;
            this.RoleMainOption.CheckedChanged += new System.EventHandler(this.FilterOption_CheckedChanged);
            // 
            // InterfacesGroup
            // 
            this.InterfacesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InterfacesGroup.Controls.Add(this.InterfaceGtkOption);
            this.InterfacesGroup.Controls.Add(this.InterfaceWinFormOption);
            this.InterfacesGroup.Location = new System.Drawing.Point(507, 0);
            this.InterfacesGroup.Name = "InterfacesGroup";
            this.InterfacesGroup.Size = new System.Drawing.Size(106, 117);
            this.InterfacesGroup.TabIndex = 1;
            this.InterfacesGroup.TabStop = false;
            this.InterfacesGroup.Text = "Interface";
            // 
            // InterfaceGtkOption
            // 
            this.InterfaceGtkOption.AutoSize = true;
            this.InterfaceGtkOption.Location = new System.Drawing.Point(6, 42);
            this.InterfaceGtkOption.Name = "InterfaceGtkOption";
            this.InterfaceGtkOption.Size = new System.Drawing.Size(49, 17);
            this.InterfaceGtkOption.TabIndex = 2;
            this.InterfaceGtkOption.Text = "Gtk#";
            this.InterfaceGtkOption.UseVisualStyleBackColor = true;
            // 
            // InterfaceWinFormOption
            // 
            this.InterfaceWinFormOption.AutoSize = true;
            this.InterfaceWinFormOption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.InterfaceWinFormOption.Location = new System.Drawing.Point(6, 19);
            this.InterfaceWinFormOption.Name = "InterfaceWinFormOption";
            this.InterfaceWinFormOption.Size = new System.Drawing.Size(95, 17);
            this.InterfaceWinFormOption.TabIndex = 0;
            this.InterfaceWinFormOption.Text = "Windows Form";
            this.InterfaceWinFormOption.UseVisualStyleBackColor = true;
            // 
            // LoadTemplatesWorker
            // 
            this.LoadTemplatesWorker.WorkerReportsProgress = true;
            this.LoadTemplatesWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoadTemplatesWorker_DoWork);
            this.LoadTemplatesWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.LoadTemplatesWorker_ProgressChanged);
            this.LoadTemplatesWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.LoadTemplatesWorker_RunWorkerCompleted);
            // 
            // LoadInstancesWorker
            // 
            this.LoadInstancesWorker.WorkerReportsProgress = true;
            this.LoadInstancesWorker.WorkerSupportsCancellation = true;
            this.LoadInstancesWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoadInstancesWorker_DoWork);
            this.LoadInstancesWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.LoadInstancesWorker_ProgressChanged);
            this.LoadInstancesWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.LoadInstancesWorker_RunWorkerCompleted);
            // 
            // FiltersAndInterfacePanel
            // 
            this.FiltersAndInterfacePanel.Controls.Add(this.FiltersGroup);
            this.FiltersAndInterfacePanel.Controls.Add(this.InterfacesGroup);
            this.FiltersAndInterfacePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FiltersAndInterfacePanel.Location = new System.Drawing.Point(10, 93);
            this.FiltersAndInterfacePanel.Name = "FiltersAndInterfacePanel";
            this.FiltersAndInterfacePanel.Size = new System.Drawing.Size(613, 117);
            this.FiltersAndInterfacePanel.TabIndex = 1;
            // 
            // AnimationTimer
            // 
            this.AnimationTimer.Interval = 30;
            this.AnimationTimer.Tick += new System.EventHandler(this.AnimationTimer_Tick);
            // 
            // PonySelectionForm
            // 
            this.AcceptButton = this.RunCommand;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 578);
            this.Controls.Add(this.FiltersAndInterfacePanel);
            this.Controls.Add(this.PoniesGroup);
            this.Controls.Add(this.CommandsGroup);
            this.MinimumSize = new System.Drawing.Size(649, 465);
            this.Name = "PonySelectionForm";
            this.Padding = new System.Windows.Forms.Padding(10, 6, 10, 10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Ponies - C# Desktop Ponies";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PonySelectionForm_FormClosed);
            this.Load += new System.EventHandler(this.PonySelectionForm_Load);
            this.LocationChanged += new System.EventHandler(this.PonySelectionForm_LocationChanged);
            this.CommandsGroup.ResumeLayout(false);
            this.CommandsGroup.PerformLayout();
            this.ProgressBarPanel.ResumeLayout(false);
            this.PoniesGroup.ResumeLayout(false);
            this.PoniesGroup.PerformLayout();
            this.FiltersGroup.ResumeLayout(false);
            this.GenderFiltersGroup.ResumeLayout(false);
            this.GenderFiltersGroup.PerformLayout();
            this.RaceFiltersGroup.ResumeLayout(false);
            this.RaceFiltersGroup.PerformLayout();
            this.RoleFiltersGroup.ResumeLayout(false);
            this.RoleFiltersGroup.PerformLayout();
            this.InterfacesGroup.ResumeLayout(false);
            this.InterfacesGroup.PerformLayout();
            this.FiltersAndInterfacePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox CommandsGroup;
        private System.Windows.Forms.Button RunCommand;
        private System.Windows.Forms.GroupBox PoniesGroup;
        private System.Windows.Forms.Label LoadInfo;
        private System.Windows.Forms.FlowLayoutPanel PonyDisplayPanel;
        private System.Windows.Forms.GroupBox FiltersGroup;
        private System.Windows.Forms.Label PonyCountInfo;
        private System.Windows.Forms.Button SetAll0Command;
        private System.Windows.Forms.Button SetAll1Command;
        private System.Windows.Forms.Label SetAllLabel;
        private System.Windows.Forms.Label SetVisibleLabel;
        private System.Windows.Forms.Button SetVisible1Command;
        private System.Windows.Forms.Button SetVisible0Command;
        private System.ComponentModel.BackgroundWorker LoadTemplatesWorker;
        private System.ComponentModel.BackgroundWorker LoadInstancesWorker;
        private System.Windows.Forms.Button PonyEditorCommand;
        private System.Windows.Forms.Label LoadTimeInfo;
        private System.Windows.Forms.Button GifViewerCommand;
        private System.Windows.Forms.GroupBox InterfacesGroup;
        private System.Windows.Forms.RadioButton InterfaceGtkOption;
        private System.Windows.Forms.RadioButton InterfaceWinFormOption;
        private System.Windows.Forms.Panel FiltersAndInterfacePanel;
        private System.Windows.Forms.CheckBox RoleMainOption;
        private System.Windows.Forms.GroupBox RoleFiltersGroup;
        private System.Windows.Forms.CheckBox RoleBackgroundOption;
        private System.Windows.Forms.CheckBox RoleIncidentalOption;
        private System.Windows.Forms.CheckBox RoleSupportOption;
        private System.Windows.Forms.GroupBox GenderFiltersGroup;
        private System.Windows.Forms.CheckBox GenderMaleOption;
        private System.Windows.Forms.CheckBox GenderFemaleOption;
        private System.Windows.Forms.GroupBox RaceFiltersGroup;
        private System.Windows.Forms.CheckBox RaceNonPonyOption;
        private System.Windows.Forms.CheckBox RaceEarthOption;
        private System.Windows.Forms.CheckBox RacePegasusOption;
        private System.Windows.Forms.CheckBox RaceUnicornOption;
        private System.Windows.Forms.CheckBox RaceAlicornOption;
        private System.Windows.Forms.CheckBox RoleOtherOption;
        private System.Windows.Forms.Button GifAlphaCommand;
        private System.Windows.Forms.Timer AnimationTimer;
        private System.Windows.Forms.Panel ProgressBarPanel;
        private System.Windows.Forms.ProgressBar ProgressBar2;
        private System.Windows.Forms.ProgressBar ProgressBar1;
    }
}