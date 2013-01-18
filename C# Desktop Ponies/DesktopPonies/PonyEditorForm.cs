namespace CsDesktopPonies.DesktopPonies
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Displays the data about each pony, and allows editing to be done.
    /// </summary>
    public partial class PonyEditorForm : Form
    {
        /// <summary>
        /// The list of pony templates.
        /// </summary>
        private IList<PonyTemplate> templates;

        /// <summary>
        /// Gets or sets the list of templates that can be edited.
        /// </summary>
        public IList<PonyTemplate> Templates
        {
            get
            {
                return templates;
            }
            set
            {
                templates = value;
                PonyDirectorySelector.DataSource = templates;
                PonyDirectorySelector.DisplayMember = "Directory";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.PonyEditorForm"/> class.
        /// </summary>
        public PonyEditorForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.Twilight;

            foreach (Directions direction in Enum.GetValues(typeof(Directions)))
                BehaviorMovementColumn.Items.Add(direction);
            foreach (SpeechTriggers trigger in Enum.GetValues(typeof(SpeechTriggers)))
                SpeechTriggersColumn.Items.Add(trigger);
            foreach (ContentAlignment alignment in Enum.GetValues(typeof(ContentAlignment)))
            {
                EffectAlignmentParentLeftColumn.Items.Add(alignment);
                EffectAlignmentOffsetLeftColumn.Items.Add(alignment);
                EffectAlignmentParentRightColumn.Items.Add(alignment);
                EffectAlignmentOffsetRightColumn.Items.Add(alignment);
            }
            PonyRaceSelector.DataSource = Enum.GetValues(typeof(PonyRace));
            PonyRoleSelector.DataSource = Enum.GetValues(typeof(Role));
        }

        /// <summary>
        /// Raised when the form is loaded.
        /// Loads the first pony for editing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonyEditorForm_Load(object sender, EventArgs e)
        {
            PonyDirectorySelector.SelectedIndex = 0;
        }

        /// <summary>
        /// Displays the data about the template at the given index in the template list.
        /// </summary>
        /// <param name="index">The index in the list of templates whose template should be displayed.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        private void DisplayTemplate(int index)
        {
            BehaviorsTable.SuspendLayout();
            SpeechesTable.SuspendLayout();
            EffectsTable.SuspendLayout();
            BehaviorsTable.Rows.Clear();
            SpeechesTable.Rows.Clear();
            EffectsTable.Rows.Clear();

            BehaviorNextBehaviorColumn.Items.Clear();
            BehaviorSpeechStartColumn.Items.Clear();
            BehaviorSpeechEndColumn.Items.Clear();
            BehaviorLeftImageColumn.Items.Clear();
            BehaviorRightImageColumn.Items.Clear();
            EffectLeftImageColumn.Items.Clear();
            EffectRightImageColumn.Items.Clear();

            if (index != -1)
            {
                PonyTemplate template = templates[index];

                PonyNameField.Text = template.Name;
                PonyRaceSelector.SelectedItem = template.Race;
                PonyRoleSelector.SelectedItem = template.Role;

                BehaviorNextBehaviorColumn.Items.Add("");
                foreach (Behavior behavior in template.Behaviors)
                    BehaviorNextBehaviorColumn.Items.Add(behavior.Name);

                BehaviorSpeechStartColumn.Items.Add("");
                BehaviorSpeechEndColumn.Items.Add("");
                foreach (Speech speech in template.Speeches)
                {
                    BehaviorSpeechStartColumn.Items.Add(speech.Name);
                    BehaviorSpeechEndColumn.Items.Add(speech.Name);
                }
                foreach (string filename in
                    Directory.GetFiles(template.Directory, "*.gif").Union<string>(Directory.GetFiles(template.Directory, "*.png")))
                {
                    BehaviorLeftImageColumn.Items.Add(Path.GetFileName(filename));
                    BehaviorRightImageColumn.Items.Add(Path.GetFileName(filename));
                    EffectLeftImageColumn.Items.Add(Path.GetFileName(filename));
                    EffectRightImageColumn.Items.Add(Path.GetFileName(filename));
                }

                foreach (Behavior behavior in template.Behaviors)
                {
                    DataGridViewRow newRow = null;
                    try
                    {
                        newRow = new DataGridViewRow();
                        newRow.CreateCells(BehaviorsTable, "Run", behavior.Name, behavior.Chance,
                            behavior.MinDuration, behavior.MaxDuration,
                            behavior.Speed, behavior.MovementAllowed,
                            Path.GetFileName(behavior.LeftImageName).ToLowerInvariant(),
                            Path.GetFileName(behavior.RightImageName).ToLowerInvariant(),
                            behavior.StartSpeech == null ? "" : behavior.StartSpeech.Name,
                            behavior.EndSpeech == null ? "" : behavior.EndSpeech.Name,
                            behavior.NextBehavior == null ? "" : behavior.NextBehavior.Name,
                            ListEffects(behavior.Effects));
                        BehaviorsTable.Rows.Add(newRow);
                    }
                    catch (InvalidOperationException)
                    {
                        if (newRow != null)
                            newRow.Dispose();
                        throw;
                    }
                }

                foreach (Speech speech in template.Speeches)
                {
                    DataGridViewRow newRow = null;
                    try
                    {
                        newRow = new DataGridViewRow();
                        newRow.CreateCells(SpeechesTable, speech.Name, speech.Line, speech.Trigger);
                    }
                    catch (InvalidOperationException)
                    {
                        if (newRow != null)
                            newRow.Dispose();
                        throw;
                    }
                    SpeechesTable.Rows.Add(newRow);
                }

                foreach (EffectTemplate effect in template.Effects)
                {
                    DataGridViewRow newRow = null;
                    try
                    {
                        newRow = new DataGridViewRow();
                        newRow.CreateCells(EffectsTable, effect.Name, effect.Duration,
                            effect.MinRepeatDuration, effect.MaxRepeatDuration,
                            effect.FollowParent,
                            Path.GetFileName(effect.LeftImageName),
                            Path.GetFileName(effect.RightImageName),
                            effect.AlignmentToParentLeft, effect.AlignmentAtOffsetLeft,
                            effect.AlignmentToParentRight, effect.AlignmentAtOffsetRight);
                        EffectsTable.Rows.Add(newRow);
                    }
                    catch (InvalidOperationException)
                    {
                        if (newRow != null)
                            newRow.Dispose();
                        throw;
                    }
                }
            }

            BehaviorsTable.ResumeLayout();
            SpeechesTable.ResumeLayout();
            EffectsTable.ResumeLayout();
        }

        /// <summary>
        /// Creates a string listing all the effects in the given list.
        /// </summary>
        /// <param name="effects">The list of effects to be turned into a string.</param>
        /// <returns>A string listing each each in the given list.</returns>
        private static string ListEffects(ICollection<EffectTemplate> effects)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;
            foreach (EffectTemplate effect in effects)
            {
                result.Append(effect.Name);
                if (i++ < effects.Count - 1)
                    result.Append(", ");
            }
            return result.ToString();
        }

        /// <summary>
        /// Raised when the selected index of PonyDirectorySelector is changed.
        /// Displays the template for the given index and adjusts the sizing of the grids to maximize viewable information.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonyDirectorySelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayTemplate(PonyDirectorySelector.SelectedIndex);

            Size speechSize = SpeechesTable.GetPreferredSize(SpeechAndEffectsContainer.Size);
            Size effectSize = EffectsTable.GetPreferredSize(SpeechAndEffectsContainer.Size);
            if (SpeechesTable.Width > speechSize.Width &&
                EffectsTable.Width > effectSize.Width)
                SpeechAndEffectsContainer.SplitterDistance =
                    (int)
                    (((float)speechSize.Width / (float)(speechSize.Width + effectSize.Width)) * (float)SpeechAndEffectsContainer.Width);
            else if (SpeechesTable.Width > speechSize.Width)
                SpeechAndEffectsContainer.SplitterDistance = speechSize.Width;
            else if (EffectsTable.Width > effectSize.Width)
                SpeechAndEffectsContainer.SplitterDistance =
                    SpeechAndEffectsContainer.Width - effectSize.Width - SpeechAndEffectsContainer.SplitterWidth;
            else
                SpeechAndEffectsContainer.SplitterDistance =
                    (int)
                    (((float)speechSize.Width / (float)(speechSize.Width + effectSize.Width)) * (float)SpeechAndEffectsContainer.Width);
        }

        /// <summary>
        /// Raised when the form has been closed.
        /// Perform cleanup and a garbage collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonyEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Hide the form and let cleanup happen without the user having to watch.
            Hide();

            // Dispose of resources.
            Dispose();
            BehaviorsTable.Dispose();
            SpeechesTable.Dispose();
            EffectsTable.Dispose();

            // Cleanup the resources the now closed form can finally release.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
