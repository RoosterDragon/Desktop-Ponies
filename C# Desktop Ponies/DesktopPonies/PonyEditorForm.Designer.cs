namespace CSDesktopPonies.DesktopPonies
{
    partial class PonyEditorForm
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
            this.BehaviorsTable = new System.Windows.Forms.DataGridView();
            this.BehaviorRunColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.BehaviorNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BehaviorChanceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BehaviorMinDurationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BehaviorMaxDurationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BehaviorSpeedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BehaviorMovementColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.BehaviorLeftImageColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.BehaviorRightImageColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.BehaviorSpeechStartColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.BehaviorSpeechEndColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.BehaviorNextBehaviorColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.BehaviorEffectsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PonyDirectorySelector = new System.Windows.Forms.ComboBox();
            this.PonyDirectoryLabel = new System.Windows.Forms.Label();
            this.SpeechesTable = new System.Windows.Forms.DataGridView();
            this.SpeechNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpeechLineColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpeechTriggersColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.EffectsTable = new System.Windows.Forms.DataGridView();
            this.EffectNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EffectDurationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EffectMinRepeatColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EffectMaxRepeatDurationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EffectFollowColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.EffectLeftImageColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.EffectRightImageColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.EffectAlignmentParentLeftColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.EffectAlignmentOffsetLeftColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.EffectAlignmentParentRightColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.EffectAlignmentOffsetRightColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PonyRoleLabel = new System.Windows.Forms.Label();
            this.PonyRoleSelector = new System.Windows.Forms.ComboBox();
            this.PonyNameLabel = new System.Windows.Forms.Label();
            this.PonyNameField = new System.Windows.Forms.TextBox();
            this.PonyRaceLabel = new System.Windows.Forms.Label();
            this.PonyRaceSelector = new System.Windows.Forms.ComboBox();
            this.SpeechAndEffectsContainer = new System.Windows.Forms.SplitContainer();
            this.SpeechAndEffectsContainer.Panel1.SuspendLayout();
            this.SpeechAndEffectsContainer.Panel2.SuspendLayout();
            this.SpeechAndEffectsContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // BehaviorsTable
            // 
            this.BehaviorsTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BehaviorsTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.BehaviorsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BehaviorsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BehaviorRunColumn,
            this.BehaviorNameColumn,
            this.BehaviorChanceColumn,
            this.BehaviorMinDurationColumn,
            this.BehaviorMaxDurationColumn,
            this.BehaviorSpeedColumn,
            this.BehaviorMovementColumn,
            this.BehaviorLeftImageColumn,
            this.BehaviorRightImageColumn,
            this.BehaviorSpeechStartColumn,
            this.BehaviorSpeechEndColumn,
            this.BehaviorNextBehaviorColumn,
            this.BehaviorEffectsColumn});
            this.BehaviorsTable.Location = new System.Drawing.Point(15, 40);
            this.BehaviorsTable.Name = "BehaviorsTable";
            this.BehaviorsTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.BehaviorsTable.Size = new System.Drawing.Size(710, 330);
            this.BehaviorsTable.TabIndex = 8;
            // 
            // BehaviorRunColumn
            // 
            this.BehaviorRunColumn.Frozen = true;
            this.BehaviorRunColumn.HeaderText = "Run";
            this.BehaviorRunColumn.MinimumWidth = 35;
            this.BehaviorRunColumn.Name = "BehaviorRunColumn";
            this.BehaviorRunColumn.Text = "Run";
            this.BehaviorRunColumn.Width = 35;
            // 
            // BehaviorNameColumn
            // 
            this.BehaviorNameColumn.HeaderText = "Name";
            this.BehaviorNameColumn.MinimumWidth = 45;
            this.BehaviorNameColumn.Name = "BehaviorNameColumn";
            this.BehaviorNameColumn.Width = 45;
            // 
            // BehaviorChanceColumn
            // 
            this.BehaviorChanceColumn.HeaderText = "Chance";
            this.BehaviorChanceColumn.MinimumWidth = 50;
            this.BehaviorChanceColumn.Name = "BehaviorChanceColumn";
            this.BehaviorChanceColumn.Width = 50;
            // 
            // BehaviorMinDurationColumn
            // 
            this.BehaviorMinDurationColumn.HeaderText = "Min Duration";
            this.BehaviorMinDurationColumn.MinimumWidth = 55;
            this.BehaviorMinDurationColumn.Name = "BehaviorMinDurationColumn";
            this.BehaviorMinDurationColumn.Width = 55;
            // 
            // BehaviorMaxDurationColumn
            // 
            this.BehaviorMaxDurationColumn.HeaderText = "Max Duration";
            this.BehaviorMaxDurationColumn.MinimumWidth = 55;
            this.BehaviorMaxDurationColumn.Name = "BehaviorMaxDurationColumn";
            this.BehaviorMaxDurationColumn.Width = 55;
            // 
            // BehaviorSpeedColumn
            // 
            this.BehaviorSpeedColumn.HeaderText = "Speed";
            this.BehaviorSpeedColumn.MinimumWidth = 50;
            this.BehaviorSpeedColumn.Name = "BehaviorSpeedColumn";
            this.BehaviorSpeedColumn.Width = 50;
            // 
            // BehaviorMovementColumn
            // 
            this.BehaviorMovementColumn.HeaderText = "Movement Allowed";
            this.BehaviorMovementColumn.MinimumWidth = 65;
            this.BehaviorMovementColumn.Name = "BehaviorMovementColumn";
            this.BehaviorMovementColumn.Width = 65;
            // 
            // BehaviorLeftImageColumn
            // 
            this.BehaviorLeftImageColumn.HeaderText = "Left Image";
            this.BehaviorLeftImageColumn.MinimumWidth = 75;
            this.BehaviorLeftImageColumn.Name = "BehaviorLeftImageColumn";
            this.BehaviorLeftImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BehaviorLeftImageColumn.Width = 75;
            // 
            // BehaviorRightImageColumn
            // 
            this.BehaviorRightImageColumn.HeaderText = "Right Image";
            this.BehaviorRightImageColumn.MinimumWidth = 75;
            this.BehaviorRightImageColumn.Name = "BehaviorRightImageColumn";
            this.BehaviorRightImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BehaviorRightImageColumn.Width = 75;
            // 
            // BehaviorSpeechStartColumn
            // 
            this.BehaviorSpeechStartColumn.HeaderText = "Start Speech";
            this.BehaviorSpeechStartColumn.MinimumWidth = 50;
            this.BehaviorSpeechStartColumn.Name = "BehaviorSpeechStartColumn";
            this.BehaviorSpeechStartColumn.Width = 50;
            // 
            // BehaviorSpeechEndColumn
            // 
            this.BehaviorSpeechEndColumn.HeaderText = "End Speech";
            this.BehaviorSpeechEndColumn.MinimumWidth = 50;
            this.BehaviorSpeechEndColumn.Name = "BehaviorSpeechEndColumn";
            this.BehaviorSpeechEndColumn.Width = 50;
            // 
            // BehaviorNextBehaviorColumn
            // 
            this.BehaviorNextBehaviorColumn.HeaderText = "Next Behavior";
            this.BehaviorNextBehaviorColumn.MinimumWidth = 50;
            this.BehaviorNextBehaviorColumn.Name = "BehaviorNextBehaviorColumn";
            this.BehaviorNextBehaviorColumn.Width = 50;
            // 
            // BehaviorEffectsColumn
            // 
            this.BehaviorEffectsColumn.HeaderText = "Effects";
            this.BehaviorEffectsColumn.MinimumWidth = 50;
            this.BehaviorEffectsColumn.Name = "BehaviorEffectsColumn";
            this.BehaviorEffectsColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BehaviorEffectsColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BehaviorEffectsColumn.Width = 50;
            // 
            // PonyDirectorySelector
            // 
            this.PonyDirectorySelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PonyDirectorySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PonyDirectorySelector.FormattingEnabled = true;
            this.PonyDirectorySelector.Location = new System.Drawing.Point(67, 12);
            this.PonyDirectorySelector.Name = "PonyDirectorySelector";
            this.PonyDirectorySelector.Size = new System.Drawing.Size(242, 21);
            this.PonyDirectorySelector.TabIndex = 1;
            this.PonyDirectorySelector.SelectedIndexChanged += new System.EventHandler(this.PonyDirectorySelector_SelectedIndexChanged);
            // 
            // PonyDirectoryLabel
            // 
            this.PonyDirectoryLabel.AutoSize = true;
            this.PonyDirectoryLabel.Location = new System.Drawing.Point(12, 15);
            this.PonyDirectoryLabel.Name = "PonyDirectoryLabel";
            this.PonyDirectoryLabel.Size = new System.Drawing.Size(49, 13);
            this.PonyDirectoryLabel.TabIndex = 0;
            this.PonyDirectoryLabel.Text = "Directory";
            // 
            // SpeechesTable
            // 
            this.SpeechesTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.SpeechesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SpeechesTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SpeechNameColumn,
            this.SpeechLineColumn,
            this.SpeechTriggersColumn});
            this.SpeechesTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpeechesTable.Location = new System.Drawing.Point(0, 0);
            this.SpeechesTable.Name = "SpeechesTable";
            this.SpeechesTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.SpeechesTable.ShowCellErrors = false;
            this.SpeechesTable.ShowRowErrors = false;
            this.SpeechesTable.Size = new System.Drawing.Size(357, 124);
            this.SpeechesTable.TabIndex = 0;
            // 
            // SpeechNameColumn
            // 
            this.SpeechNameColumn.HeaderText = "Name";
            this.SpeechNameColumn.MinimumWidth = 45;
            this.SpeechNameColumn.Name = "SpeechNameColumn";
            this.SpeechNameColumn.Width = 45;
            // 
            // SpeechLineColumn
            // 
            this.SpeechLineColumn.HeaderText = "Line";
            this.SpeechLineColumn.MinimumWidth = 35;
            this.SpeechLineColumn.Name = "SpeechLineColumn";
            this.SpeechLineColumn.Width = 35;
            // 
            // SpeechTriggersColumn
            // 
            this.SpeechTriggersColumn.HeaderText = "Triggers";
            this.SpeechTriggersColumn.MinimumWidth = 55;
            this.SpeechTriggersColumn.Name = "SpeechTriggersColumn";
            this.SpeechTriggersColumn.Width = 55;
            // 
            // EffectsTable
            // 
            this.EffectsTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.EffectsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EffectsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EffectNameColumn,
            this.EffectDurationColumn,
            this.EffectMinRepeatColumn,
            this.EffectMaxRepeatDurationColumn,
            this.EffectFollowColumn,
            this.EffectLeftImageColumn,
            this.EffectRightImageColumn,
            this.EffectAlignmentParentLeftColumn,
            this.EffectAlignmentOffsetLeftColumn,
            this.EffectAlignmentParentRightColumn,
            this.EffectAlignmentOffsetRightColumn});
            this.EffectsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EffectsTable.Location = new System.Drawing.Point(0, 0);
            this.EffectsTable.Name = "EffectsTable";
            this.EffectsTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.EffectsTable.ShowCellErrors = false;
            this.EffectsTable.ShowRowErrors = false;
            this.EffectsTable.Size = new System.Drawing.Size(349, 124);
            this.EffectsTable.TabIndex = 0;
            // 
            // EffectNameColumn
            // 
            this.EffectNameColumn.HeaderText = "Name";
            this.EffectNameColumn.MinimumWidth = 45;
            this.EffectNameColumn.Name = "EffectNameColumn";
            this.EffectNameColumn.Width = 45;
            // 
            // EffectDurationColumn
            // 
            this.EffectDurationColumn.HeaderText = "Duration";
            this.EffectDurationColumn.MinimumWidth = 55;
            this.EffectDurationColumn.Name = "EffectDurationColumn";
            this.EffectDurationColumn.Width = 55;
            // 
            // EffectMinRepeatColumn
            // 
            this.EffectMinRepeatColumn.HeaderText = "Min Repeat Duration";
            this.EffectMinRepeatColumn.MinimumWidth = 75;
            this.EffectMinRepeatColumn.Name = "EffectMinRepeatColumn";
            this.EffectMinRepeatColumn.Width = 75;
            // 
            // EffectMaxRepeatDurationColumn
            // 
            this.EffectMaxRepeatDurationColumn.HeaderText = "Max Repeat Duration";
            this.EffectMaxRepeatDurationColumn.MinimumWidth = 75;
            this.EffectMaxRepeatDurationColumn.Name = "EffectMaxRepeatDurationColumn";
            this.EffectMaxRepeatDurationColumn.Width = 75;
            // 
            // EffectFollowColumn
            // 
            this.EffectFollowColumn.HeaderText = "Follow Parent";
            this.EffectFollowColumn.MinimumWidth = 45;
            this.EffectFollowColumn.Name = "EffectFollowColumn";
            this.EffectFollowColumn.Width = 45;
            // 
            // EffectLeftImageColumn
            // 
            this.EffectLeftImageColumn.HeaderText = "Left Image";
            this.EffectLeftImageColumn.MinimumWidth = 75;
            this.EffectLeftImageColumn.Name = "EffectLeftImageColumn";
            this.EffectLeftImageColumn.Width = 75;
            // 
            // EffectRightImageColumn
            // 
            this.EffectRightImageColumn.HeaderText = "Right Image";
            this.EffectRightImageColumn.MinimumWidth = 75;
            this.EffectRightImageColumn.Name = "EffectRightImageColumn";
            this.EffectRightImageColumn.Width = 75;
            // 
            // EffectAlignmentParentLeftColumn
            // 
            this.EffectAlignmentParentLeftColumn.HeaderText = "Alignment to Parent Left";
            this.EffectAlignmentParentLeftColumn.MinimumWidth = 75;
            this.EffectAlignmentParentLeftColumn.Name = "EffectAlignmentParentLeftColumn";
            this.EffectAlignmentParentLeftColumn.Width = 75;
            // 
            // EffectAlignmentOffsetLeftColumn
            // 
            this.EffectAlignmentOffsetLeftColumn.HeaderText = "Alignment at Offset Left";
            this.EffectAlignmentOffsetLeftColumn.MinimumWidth = 75;
            this.EffectAlignmentOffsetLeftColumn.Name = "EffectAlignmentOffsetLeftColumn";
            this.EffectAlignmentOffsetLeftColumn.Width = 75;
            // 
            // EffectAlignmentParentRightColumn
            // 
            this.EffectAlignmentParentRightColumn.HeaderText = "Alignment to Parent Right";
            this.EffectAlignmentParentRightColumn.MinimumWidth = 75;
            this.EffectAlignmentParentRightColumn.Name = "EffectAlignmentParentRightColumn";
            this.EffectAlignmentParentRightColumn.Width = 75;
            // 
            // EffectAlignmentOffsetRightColumn
            // 
            this.EffectAlignmentOffsetRightColumn.HeaderText = "Alignment at Offset Right";
            this.EffectAlignmentOffsetRightColumn.MinimumWidth = 75;
            this.EffectAlignmentOffsetRightColumn.Name = "EffectAlignmentOffsetRightColumn";
            this.EffectAlignmentOffsetRightColumn.Width = 75;
            // 
            // PonyRoleLabel
            // 
            this.PonyRoleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PonyRoleLabel.AutoSize = true;
            this.PonyRoleLabel.Location = new System.Drawing.Point(492, 15);
            this.PonyRoleLabel.Name = "PonyRoleLabel";
            this.PonyRoleLabel.Size = new System.Drawing.Size(29, 13);
            this.PonyRoleLabel.TabIndex = 4;
            this.PonyRoleLabel.Text = "Role";
            // 
            // PonyRoleSelector
            // 
            this.PonyRoleSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PonyRoleSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PonyRoleSelector.FormattingEnabled = true;
            this.PonyRoleSelector.Location = new System.Drawing.Point(527, 12);
            this.PonyRoleSelector.Name = "PonyRoleSelector";
            this.PonyRoleSelector.Size = new System.Drawing.Size(75, 21);
            this.PonyRoleSelector.TabIndex = 5;
            // 
            // PonyNameLabel
            // 
            this.PonyNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PonyNameLabel.AutoSize = true;
            this.PonyNameLabel.Location = new System.Drawing.Point(315, 15);
            this.PonyNameLabel.Name = "PonyNameLabel";
            this.PonyNameLabel.Size = new System.Drawing.Size(35, 13);
            this.PonyNameLabel.TabIndex = 2;
            this.PonyNameLabel.Text = "Name";
            // 
            // PonyNameField
            // 
            this.PonyNameField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PonyNameField.Location = new System.Drawing.Point(356, 12);
            this.PonyNameField.Name = "PonyNameField";
            this.PonyNameField.Size = new System.Drawing.Size(130, 20);
            this.PonyNameField.TabIndex = 3;
            // 
            // PonyRaceLabel
            // 
            this.PonyRaceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PonyRaceLabel.AutoSize = true;
            this.PonyRaceLabel.Location = new System.Drawing.Point(608, 15);
            this.PonyRaceLabel.Name = "PonyRaceLabel";
            this.PonyRaceLabel.Size = new System.Drawing.Size(33, 13);
            this.PonyRaceLabel.TabIndex = 6;
            this.PonyRaceLabel.Text = "Race";
            // 
            // PonyRaceSelector
            // 
            this.PonyRaceSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PonyRaceSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PonyRaceSelector.FormattingEnabled = true;
            this.PonyRaceSelector.Location = new System.Drawing.Point(647, 12);
            this.PonyRaceSelector.Name = "PonyRaceSelector";
            this.PonyRaceSelector.Size = new System.Drawing.Size(75, 21);
            this.PonyRaceSelector.TabIndex = 7;
            // 
            // SpeechAndEffectsContainer
            // 
            this.SpeechAndEffectsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpeechAndEffectsContainer.Location = new System.Drawing.Point(15, 376);
            this.SpeechAndEffectsContainer.Name = "SpeechAndEffectsContainer";
            // 
            // SpeechAndEffectsContainer.Panel1
            // 
            this.SpeechAndEffectsContainer.Panel1.Controls.Add(this.SpeechesTable);
            // 
            // SpeechAndEffectsContainer.Panel2
            // 
            this.SpeechAndEffectsContainer.Panel2.Controls.Add(this.EffectsTable);
            this.SpeechAndEffectsContainer.Size = new System.Drawing.Size(710, 124);
            this.SpeechAndEffectsContainer.SplitterDistance = 357;
            this.SpeechAndEffectsContainer.TabIndex = 9;
            // 
            // PonyEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 512);
            this.Controls.Add(this.SpeechAndEffectsContainer);
            this.Controls.Add(this.PonyRaceLabel);
            this.Controls.Add(this.PonyRaceSelector);
            this.Controls.Add(this.PonyNameField);
            this.Controls.Add(this.PonyNameLabel);
            this.Controls.Add(this.PonyRoleLabel);
            this.Controls.Add(this.PonyRoleSelector);
            this.Controls.Add(this.PonyDirectoryLabel);
            this.Controls.Add(this.PonyDirectorySelector);
            this.Controls.Add(this.BehaviorsTable);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "PonyEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Ponies - C# Desktop Ponies";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PonyEditorForm_FormClosed);
            this.Load += new System.EventHandler(this.PonyEditorForm_Load);
            this.SpeechAndEffectsContainer.Panel1.ResumeLayout(false);
            this.SpeechAndEffectsContainer.Panel2.ResumeLayout(false);
            this.SpeechAndEffectsContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView BehaviorsTable;
        private System.Windows.Forms.ComboBox PonyDirectorySelector;
        private System.Windows.Forms.Label PonyDirectoryLabel;
        private System.Windows.Forms.DataGridView SpeechesTable;
        private System.Windows.Forms.DataGridView EffectsTable;
        private System.Windows.Forms.Label PonyRoleLabel;
        private System.Windows.Forms.ComboBox PonyRoleSelector;
        private System.Windows.Forms.Label PonyNameLabel;
        private System.Windows.Forms.TextBox PonyNameField;
        private System.Windows.Forms.Label PonyRaceLabel;
        private System.Windows.Forms.ComboBox PonyRaceSelector;
        private System.Windows.Forms.SplitContainer SpeechAndEffectsContainer;
        private System.Windows.Forms.DataGridViewButtonColumn BehaviorRunColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BehaviorNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BehaviorChanceColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BehaviorMinDurationColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BehaviorMaxDurationColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BehaviorSpeedColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn BehaviorMovementColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn BehaviorLeftImageColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn BehaviorRightImageColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn BehaviorSpeechStartColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn BehaviorSpeechEndColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn BehaviorNextBehaviorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BehaviorEffectsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpeechNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpeechLineColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn SpeechTriggersColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EffectNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EffectDurationColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EffectMinRepeatColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EffectMaxRepeatDurationColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn EffectFollowColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn EffectLeftImageColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn EffectRightImageColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn EffectAlignmentParentLeftColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn EffectAlignmentOffsetLeftColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn EffectAlignmentParentRightColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn EffectAlignmentOffsetRightColumn;
    }
}