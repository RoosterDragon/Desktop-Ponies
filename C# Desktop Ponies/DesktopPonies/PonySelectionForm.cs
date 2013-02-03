namespace CSDesktopPonies.DesktopPonies
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using CSDesktopPonies.Collections;
    using CSDesktopPonies.SpriteManagement;

    /// <summary>
    /// Form that allows the user to select the ponies they want to run.
    /// </summary>
    public partial class PonySelectionForm : Form
    {
        #region Fields and Properties
        /// <summary>
        /// The collection of ponies to be displayed for selection.
        /// </summary>
        private List<PonyDisplay> ponyDisplays;

        /// <summary>
        /// The total number of pony instances that have been selected to be run.
        /// </summary>
        private int totalPonies;
        /// <summary>
        /// The total number of pony templates that have been selected to be run.
        /// </summary>
        private int totalTemplates;

        /// <summary>
        /// The <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> that handles the set of pony images.
        /// </summary>
        private LazyDictionary<string, AnimatedImage<BitmapFrame>> imageManager =
            new LazyDictionary<string, AnimatedImage<BitmapFrame>>(fileName => new AnimatedImage<BitmapFrame>(
                fileName, file => new BitmapFrame(file), BitmapFrame.FromBuffer, BitmapFrame.AllowableBitDepths));

        /// <summary>
        /// The <see cref="T:CSDesktopPonies.SpriteManagement.ISpriteCollectionView"/> handling the display of sprites.
        /// </summary>
        private ISpriteCollectionView spriteInterface;
        /// <summary>
        /// The pony instances to be run by the interface.
        /// </summary>
        private LinkedList<PonyInstance> ponyInstances = new LinkedList<PonyInstance>();
        /// <summary>
        /// The <see cref="T:CSDesktopPonies.DesktopPonies.InteractionManager"/> the handles interaction between ponies.
        /// </summary>
        private InteractionManager interactionManager;

        /// <summary>
        /// The time loading of templates started (after the form has itself loaded).
        /// </summary>
        private DateTime templateLoadStartTime;
        /// <summary>
        /// The time loading of pony instances started (when the user has clicked run).
        /// </summary>
        private DateTime instanceLoadStartTime;
        /// <summary>
        /// Used to display loading times.
        /// </summary>
        private Timer loadTimer = new Timer();

        /// <summary>
        /// Tracks the previous window state of the form when the form changes location.
        /// </summary>
        private FormWindowState oldWindowState;
        /// <summary>
        /// Indicates if the form was just restored from being minimized, and a forced layout is required.
        /// </summary>
        private bool layoutPendingFromRestore = false;
        #endregion

        #region PonyDisplay class
        /// <summary>
        /// Holds the template and selection control one template.
        /// </summary>
        private class PonyDisplay : IDisposable
        {
            /// <summary>
            /// Gets or sets the template for this pony.
            /// </summary>
            public PonyTemplate Template { get; set; }
            /// <summary>
            /// Gets the <see cref="T:CSDesktopPonies.DesktopPonies.PonySelectionControl"/> used to display this pony.
            /// </summary>
            public PonySelectionControl SelectionControl { get; private set; }
            
            /// <summary>
            /// The <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> handling display images on the SelectionControl.
            /// </summary>
            private LazyDictionary<string, AnimatedImage<BitmapFrame>> manager;
            /// <summary>
            /// The image displayed on the SelectionControl.
            /// </summary>
            private AnimatedImage<BitmapFrame> selectionImage;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.DesktopPonies.PonySelectionForm.PonyDisplay"/> class to hold
            /// details for the given template and provide a selection control to display it.
            /// </summary>
            /// <param name="template">The <see cref="T:CSDesktopPonies.DesktopPonies.PonyTemplate"/> that represents this pony.</param>
            /// <param name="countChangedHandler">The method handling the
            /// <see cref="E:CSDesktopPonies.DesktopPonies.PonySelectionControl.CountChanged"/> event.</param>
            /// <param name="displayImageManager">Images displayed on the selection control will be managed by this collection.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="template"/> is null.-or-
            /// <paramref name="displayImageManager"/> is null.</exception>
            public PonyDisplay(PonyTemplate template, EventHandler<CountChangedEventArgs> countChangedHandler,
                LazyDictionary<string, AnimatedImage<BitmapFrame>> displayImageManager)
            {
                Argument.EnsureNotNull(template, "template");
                Argument.EnsureNotNull(displayImageManager, "displayImageManager");

                Template = template;
                manager = displayImageManager;

                string imageName = Template.Behaviors[0].RightImageName;
                manager.Add(imageName);
                selectionImage = manager[imageName];

                SelectionControl = new PonySelectionControl(Template.TemplateName, selectionImage,
                    Template.Behaviors[0].BehaviorImageFlip == ImageFlip.RightMirrorsLeft);
                SelectionControl.CountChanged += countChangedHandler;
            }

            /// <summary>
            /// Releases all resources used by the <see cref="T:CSDesktopPonies.DesktopPonies.PonySelectionForm.PonyDisplay"/> object.
            /// </summary>
            public void Dispose()
            {
                manager.Remove(selectionImage.FilePath);
                selectionImage.Dispose();
                SelectionControl.Dispose();
            }
        }
        #endregion

        #region Creation and Loading
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.DesktopPonies.PonySelectionForm"/> class that loads and presents
        /// available pony templates to the user. Any number of each template can be selected to be run.
        /// </summary>
        public PonySelectionForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.Twilight;

            // Link the checkboxes in our filters to the enumeration they represent.
            RoleMainOption.Tag = Role.Main;
            RoleSupportOption.Tag = Role.Support;
            RoleIncidentalOption.Tag = Role.Incidental;
            RoleBackgroundOption.Tag = Role.Background;
            RoleOtherOption.Tag = Role.Other;

            GenderFemaleOption.Tag = Gender.Female;
            GenderMaleOption.Tag = Gender.Male;

            RaceAlicornOption.Tag = PonyRace.Alicorn;
            RacePegasusOption.Tag = PonyRace.Pegasus;
            RaceUnicornOption.Tag = PonyRace.Unicorn;
            RaceEarthOption.Tag = PonyRace.Earth;
            RaceNonPonyOption.Tag = PonyRace.NonPony;

            // Show options for available interfaces.
            InterfaceWinFormOption.Enabled = WinFormSpriteInterface.IsRunable;
            InterfaceGtkOption.Enabled = GtkSpriteInterface.IsRunable;
            if (InterfaceWinFormOption.Enabled)
                InterfaceWinFormOption.Checked = true;
            else if (InterfaceGtkOption.Enabled)
                InterfaceGtkOption.Checked = true;

            // Hook up the mouse wheel scrolling.
            PonyDisplayPanel.MouseWheel += PonyDisplayPanel_MouseWheel;
        }

        /// <summary>
        /// Raised when the form has loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonySelectionForm_Load(object sender, EventArgs e)
        {
            // Start loading the ponies on a new thread.
            UseWaitCursor = true;
            templateLoadStartTime = DateTime.UtcNow;
            LoadTemplatesWorker.RunWorkerAsync();
            LoadTemplatesWorker.Dispose();
            loadTimer.Tick += (tickSender, tickE) =>
                    LoadTimeInfo.Text = (DateTime.UtcNow - templateLoadStartTime).ToString(@"mm\:ss", CultureInfo.CurrentCulture);
            loadTimer.Interval = 1000;
            loadTimer.Enabled = true;
        }

        /// <summary>
        /// Raised when LoadTemplatesWorker runs.
        /// This will load all the files for pony templates.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void LoadTemplatesWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadTemplatesWorker.ReportProgress(0, "Loading templates.");

            // Get the list of directories, each of which contains a template.
            string[] ponyDirectories = Directory.GetDirectories(Program.PonyDirectory);

            // Create the set of displays for selecting templates.
            ponyDisplays = new List<PonyDisplay>(ponyDirectories.Length);
            for (int i = 0; i < ponyDirectories.Length; i++)
            {
                // Try to load the template, and create a control for it.
                try
                {
                    PonyTemplate newTemplate = new PonyTemplate(ponyDirectories[i], !Program.UseXmlFiles);
                    //newTemplate.SaveToXml();
                    ponyDisplays.Add(new PonyDisplay(newTemplate, PonyDisplay_CountChanged, imageManager));
                    Invoke(new MethodInvoker(() => PonyDisplayPanel.Controls.Add(ponyDisplays[ponyDisplays.Count - 1].SelectionControl)));
                }
                catch (Exception ex)
                {
                    ProcessingException(ex, "This file will be skipped.");
                }

                string progress = string.Format(CultureInfo.CurrentCulture,
                    "Loading templates. {0} loaded. {1} remaining.", i, ponyDirectories.Length - i);
                LoadTemplatesWorker.ReportProgress(i * 100 / ponyDirectories.Length, progress);
            }

            LoadTemplatesWorker.ReportProgress(100, "Loading interactions...");

            try
            {
                interactionManager = new InteractionManager(Program.PonyDirectory, ponyDisplays.ConvertAll(display => display.Template));
            }
            catch (Exception ex)
            {
                ProcessingException(ex, "Interactions will not be loaded.");
            }

            LoadTemplatesWorker.ReportProgress(100, "Displaying...");
        }

        /// <summary>
        /// Displays a message about an exception that occurred during processing of files.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="skipMessage">The text that informs the user that the file will be skipped.</param>
        private static void ProcessingException(Exception ex, string skipMessage)
        {
            string message = ex.Message;
            Exception innerException = ex.InnerException;
            while (innerException != null)
            {
                message += Environment.NewLine + Environment.NewLine + innerException.Message;
                innerException = innerException.InnerException;
            }
            message += Environment.NewLine + Environment.NewLine + skipMessage;
            MessageBox.Show(message, "Error Processing Configuration File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Raised when the progress for LoadTemplatesWorker has changed.
        /// Used to report the status to the user.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void LoadTemplatesWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar1.Value = e.ProgressPercentage;
            LoadInfo.Text = (string)e.UserState;
        }

        /// <summary>
        /// Raised when LoadTemplatesWorker finishes running.
        /// This will activate our form so it can be used.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void LoadTemplatesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Display the ponies that match the default filters.
            DetermineSelectedTemplates();

            // Allow bulk selection of templates.
            EnableControls(true);

            // Reset load timer.
            loadTimer.Dispose();
            loadTimer = new Timer();
            LoadTimeInfo.Text = "";

            // Start animation.
            AnimationTimer.Start();

            // Do a garbage collection so we can free garbage caused by startup and loading the templates.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Report status to the user.
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "Loaded {1} templates in {0} seconds. {2} images in cache.",
                (DateTime.UtcNow - templateLoadStartTime).TotalSeconds.ToString(@"0.00", CultureInfo.CurrentCulture),
                ponyDisplays.Count,
                imageManager.Count));
            LoadInfo.Text =
                (DateTime.UtcNow - templateLoadStartTime).TotalSeconds.ToString("Loaded in 0.00 seconds", CultureInfo.CurrentCulture);
            UseWaitCursor = false;
        }
        #endregion

        #region Pony Selection Handling
        /// <summary>
        /// Raised when a filter option is changed.
        /// Re-determines which templates are visible with the new set of filters.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void FilterOption_CheckedChanged(object sender, EventArgs e)
        {
            if (!LoadTemplatesWorker.IsBusy)
                DetermineSelectedTemplates();
        }

        /// <summary>
        /// Updates our display of ponies. Shows ponies for whom all attributes are selected.
        /// </summary>
        private void DetermineSelectedTemplates()
        {
            if (ponyDisplays == null)
                return;

            PonyDisplayPanel.SuspendLayout();
            foreach (PonyDisplay display in ponyDisplays)
            {
                int filtersMet = 0;

                foreach (CheckBox filter in RoleFiltersGroup.Controls)
                    if (filter.CheckState == CheckState.Checked && display.Template.Role == (Role)filter.Tag)
                        filtersMet++;

                foreach (CheckBox filter in GenderFiltersGroup.Controls)
                    if (filter.CheckState == CheckState.Checked && display.Template.Gender == (Gender)filter.Tag)
                        filtersMet++;

                foreach (CheckBox filter in RaceFiltersGroup.Controls)
                    if (filter.CheckState == CheckState.Checked && display.Template.Race == (PonyRace)filter.Tag)
                        filtersMet++;

                // Display the control if all the filters are met.
                display.SelectionControl.Visible = filtersMet == 3;
            }

            PonyDisplayPanel.ResumeLayout();
            PonyDisplayPanel.Update();
        }

        /// <summary>
        /// Raised when a bulk selection command is clicked.
        /// Sets the counts for every template.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void SetCounts_Click(object sender, EventArgs e)
        {
            if (sender == SetAll0Command)
                SetAllCounts(0, false);
            else if (sender == SetAll1Command)
                SetAllCounts(1, false);
            else if (sender == SetVisible0Command)
                SetAllCounts(0, true);
            else if (sender == SetVisible1Command)
                SetAllCounts(1, true);
        }

        /// <summary>
        /// Sets the count for all templates to the given value, optionally only affects templates that are visible.
        /// </summary>
        /// <param name="count">The count to set the templates to.</param>
        /// <param name="visibleOnly">If true, will only affect currently visible templates, otherwise will affect all templates.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is negative.</exception>
        private void SetAllCounts(int count, bool visibleOnly)
        {
            foreach (PonyDisplay display in ponyDisplays)
                if (!visibleOnly || display.SelectionControl.Visible)
                    display.SelectionControl.PonyCount = count;
        }

        /// <summary>
        /// Raised when a selection control for a display has its count changed.
        /// Updates the running total.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonyDisplay_CountChanged(object sender, CountChangedEventArgs e)
        {
            if (e.Count == 0 && e.Change < 0)
                totalTemplates--;
            else if (e.Count > 0 && e.Change == e.Count)
                totalTemplates++;

            totalPonies += e.Change;
            PonyCountInfo.Text = string.Format(CultureInfo.CurrentCulture,
                "Pony Templates: {0} Pony Count: {1}", totalTemplates, totalPonies);
            if (totalPonies > 0)
                RunCommand.Enabled = true;
            else
                RunCommand.Enabled = false;
        }

        /// <summary>
        /// Raised when PonyDisplayPanel is resized.
        /// Recalculates the distance that is scrolled when the user scrolls through the selection.
        /// Sets the distances to be in multiples of row height.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonyDisplayPanel_Resize(object sender, EventArgs e)
        {
            if (ponyDisplays != null)
            {
                int rowHeight = ponyDisplays[0].SelectionControl.Height + ponyDisplays[0].SelectionControl.Margin.Vertical;
                int visibleRowCount = PonyDisplayPanel.Height / rowHeight;
                PonyDisplayPanel.VerticalScroll.SmallChange = rowHeight;
                PonyDisplayPanel.VerticalScroll.LargeChange = rowHeight * visibleRowCount;
            }
        }

        /// <summary>
        /// Raised when PonyDisplayPanel has focus and is scrolled by the mouse wheel.
        /// Scrolls control by the SmallChange value instead of the mouse wheel default distance.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonyDisplayPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int numberOfPixelsToMove = -e.Delta / 120 * PonyDisplayPanel.VerticalScroll.SmallChange;
            // Take the current value and add on our custom scroll distance.
            // The event will also be handled by the control, so undo the scroll distance it will add later.
            int newValue = PonyDisplayPanel.VerticalScroll.Value + numberOfPixelsToMove + e.Delta;
            if (newValue > PonyDisplayPanel.VerticalScroll.Minimum)
            {
                if (newValue < PonyDisplayPanel.VerticalScroll.Maximum)
                    PonyDisplayPanel.VerticalScroll.Value = newValue;
                else
                    PonyDisplayPanel.VerticalScroll.Value = PonyDisplayPanel.VerticalScroll.Maximum;
            }
            else
            {
                PonyDisplayPanel.VerticalScroll.Value = PonyDisplayPanel.VerticalScroll.Minimum;
            }
        }

        #region Fix flow panel scrollbar after restoring from minimized state.
        /// <summary>
        /// Raised when the location of the form is changed.
        /// Used to force the layout of the flow panel to fix a dodgy scrollbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonySelectionForm_LocationChanged(object sender, EventArgs e)
        {
            // If we have just returned from the minimized state, the flow panel will have an incorrect scrollbar.
            // Force a layout to get the bar re-evaluated and fixed.
            if (oldWindowState == FormWindowState.Minimized && WindowState != FormWindowState.Minimized)
                layoutPendingFromRestore = true;
            oldWindowState = WindowState;
        }

        /// <summary>
        /// Raised when PonyDisplayPanel paints itself.
        /// Used to force the layout of the flow panel to fix a dodgy scrollbar after restoring from a minimized state.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonyDisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            if (layoutPendingFromRestore)
            {
                PonyDisplayPanel.PerformLayout();
                layoutPendingFromRestore = false;
            }
        }
        #endregion
        #endregion

        #region Start Ponies
        /// <summary>
        /// Raised when RunCommand is clicked.
        /// Creates the desired pony instances and starts animation.
        /// Can also cancel a load in progress.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void RunCommand_Click(object sender, EventArgs e)
        {
            if (!LoadInstancesWorker.IsBusy)
            {
                const bool AlphaBlending = true;

                System.Drawing.Rectangle area = System.Drawing.Rectangle.Empty;
                foreach (Screen screen in Screen.AllScreens)
                    if (area == System.Drawing.Rectangle.Empty)
                        area = screen.WorkingArea;
                    else
                        area = System.Drawing.Rectangle.Union(area, screen.WorkingArea);

                EnableControls(false);
                if (InterfaceWinFormOption.Checked)
                    spriteInterface = new WinFormSpriteInterface(area, AlphaBlending);
                else if (InterfaceGtkOption.Checked)
                    spriteInterface = new GtkSpriteInterface(AlphaBlending);
                else
                    throw new InvalidOperationException("No interface has been selected.");
                
                LoadInstancesWorker.RunWorkerAsync();
                RunCommand.Text = "CANCEL";
                instanceLoadStartTime = DateTime.UtcNow;
                loadTimer.Tick += (tickSender, tickE) =>
                        LoadTimeInfo.Text = (DateTime.UtcNow - instanceLoadStartTime).ToString(@"mm\:ss", CultureInfo.CurrentCulture);
                loadTimer.Interval = 1000;
                loadTimer.Enabled = true;
            }
            else
            {
                LoadInstancesWorker.CancelAsync();
                RunCommand.Enabled = false;
            }
        }

        /// <summary>
        /// Raised when LoadInstancesWorker runs.
        /// Creates the desired pony instances.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void LoadInstancesWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadInstancesWorker.ReportProgress(0, "Loading instances.");
            int templatesLoaded = 0;

            var imageSequence = GetImageFileNames(
                ponyDisplays.Where(display => display.SelectionControl.PonyCount > 0).Select(display => display.Template))
                .Distinct(StringComparer.Ordinal);
            int totalImages = imageSequence.Count() + totalTemplates;

            Invoke(new MethodInvoker(() =>
            {
                ProgressBar1.Value = 0;
                ProgressBar2.Value = 0;
                ProgressBar1.Maximum = totalTemplates;
                ProgressBar2.Maximum = imageSequence.Count();
                float initialAmount = (float)totalTemplates / totalImages;
                ProgressBar1.Width = (int)(initialAmount * ProgressBarPanel.Width);
                ProgressBar2.Left = ProgressBar1.Right;
                ProgressBar2.Width = (int)((1 - initialAmount) * ProgressBarPanel.Width);
            }));

            // Loads the images for templates we'll be using, and create the desired number of instances of each template.
            for (int i = 0; i < ponyDisplays.Count; i++)
            {
                // Cancel loading.
                if (LoadInstancesWorker.CancellationPending)
                {
                    spriteInterface.Close();
                    spriteInterface = null;
                    ponyInstances.Clear();
                    GC.Collect();
                    e.Cancel = true;
                    break;
                }

                // Skip loading this template if it was not selected.
                if (ponyDisplays[i].SelectionControl.PonyCount == 0)
                    continue;

                // Load template images and create instances.
                try
                {
                    LinkedListNode<PonyInstance> newInstancesStart = null;
                    // Create the desired number of instances.
                    for (int count = 0; count < ponyDisplays[i].SelectionControl.PonyCount; count++)
                    {
                        ponyInstances.AddLast(new PonyInstance(ponyDisplays[i].Template));
                        if (count == 0)
                            newInstancesStart = ponyInstances.Last;
                    }

                    EventHandler imageLoadedHandler = (ilSender, ilE) =>
                    {
                        string progress = string.Format(CultureInfo.CurrentCulture,
                             "Loading templates. {0} loaded. {1} remaining. Loading {2}.",
                             templatesLoaded, totalTemplates - templatesLoaded, ponyDisplays[i].Template.Name);
                        LoadInstancesWorker.ReportProgress(templatesLoaded, progress);
                    };

                    try
                    {
                        // Load the images for the initial behavior of all the instances.
                        spriteInterface.LoadImages(
                            GetCurrentBehaviorImageNamesForNodeSequence(newInstancesStart).Distinct(StringComparer.Ordinal),
                            imageLoadedHandler);
                    }
                    catch (AggregateException ex)
                    {
                        #region Parse exception data and rethrow.
                        string message = "Error loading images for " + ponyDisplays[i].Template.Name + "." +
                            Environment.NewLine + ex.Message;
                        message += Environment.NewLine + "Errors for up to five images shown.";
                        for (int j = 0; j < Math.Min(5, ex.InnerExceptions.Count); j++)
                        {
                            message += Environment.NewLine;
                            Exception innerException = ex.InnerExceptions[j];
                            while (innerException != null)
                            {
                                message += Environment.NewLine + innerException.Message;
                                innerException = innerException.InnerException;
                            }
                        }
                        throw new Exception(message);
                        #endregion
                    }

                    templatesLoaded++;
                }
                catch (Exception ex)
                {
                    #region Remove template and show an error message.
                    // Dispose and remove the template that does not work.
                    Invoke(new MethodInvoker(() =>
                        {
                            ponyDisplays[i].SelectionControl.PonyCount = 0;
                            ponyDisplays[i].Dispose();
                        }));
                    ponyDisplays.RemoveAt(i--);

                    // Display an error message.
                    string message = ex.Message;
                    Exception innerException = ex.InnerException;
                    while (innerException != null)
                    {
                        message += Environment.NewLine + Environment.NewLine + innerException.Message;
                        innerException = innerException.InnerException;
                    }
                    message += Environment.NewLine + Environment.NewLine + "This pony will be skipped.";
                    MessageBox.Show(message, "Error Loading Pony Images", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    #endregion
                }
            }

            // Load all images in the background. This prevents a long delay in starting since loading all the images will take
            // some time, but over time helps reduce pauses caused be having to load an unloaded image at the instant it is required
            // for drawing.
            //if (spriteInterface is WinFormSpriteInterface)
                //System.Threading.ThreadPool.QueueUserWorkItem(o =>
                //{
                    EventHandler imageLoadedHandler2 = (ilSender, ilE) => Invoke(new MethodInvoker(() =>
                        {
                            ProgressBar2.Value++;
                            LoadInfo.Text = "Images Loaded: " + ProgressBar2.Value + " of " + ProgressBar2.Maximum;
                            //if (ProgressBar2.Value > .4f * ProgressBar2.Maximum && Visible)
                            //    StartAnimator();
                        }));
                    spriteInterface.LoadImages(imageSequence, imageLoadedHandler2);
                    GC.Collect();
                //});

            // Trim excess due to templates that could not load.
            ponyDisplays.TrimExcess();

            LoadInstancesWorker.ReportProgress(templatesLoaded, "Done.");
        }

        /// <summary>
        /// Enumerates through the image file paths for the current behavior of each pony instance in the given node, and any subsequent
        /// nodes in the list.
        /// </summary>
        /// <param name="node">The node whose image names should be returned, along with the names for subsequent nodes in the linked list
        /// to which it belongs.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that returns all the image file paths for the current
        /// behavior of each <see cref="T:CSDesktopPonies.DesktopPonies.PonyInstance"/> contained in the given node and subsequent nodes in
        /// the list.</returns>
        private IEnumerable<string> GetCurrentBehaviorImageNamesForNodeSequence(LinkedListNode<PonyInstance> node)
        {
            while (node != null)
            {
                if (node.Value.MovingLeft)
                    yield return node.Value.CurrentBehavior.LeftImageName;
                else
                    yield return node.Value.CurrentBehavior.RightImageName;
                node = node.Next;
            }
        }

        /// <summary>
        /// Gets the file names used by the given collection of templates, sorted with the most likely behaviors first.
        /// </summary>
        /// <param name="templates">The set of templates whose file names should be enumerated.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1"/> of the file names for the images referenced by the given
        /// templates, sorted in descending order of likelihood.</returns>
        private IEnumerable<string> GetImageFileNames(IEnumerable<PonyTemplate> templates)
        {
            var behaviors = templates
                .SelectMany(template => template.Behaviors, (template, behavior) => new Tuple<PonyTemplate, Behavior>(template, behavior))
                .OrderByDescending(tuple => (float)tuple.Item2.Chance / tuple.Item1.ChanceTotal)
                .Select(tuple => tuple.Item2);

            foreach (Behavior behavior in behaviors)
            {
                yield return behavior.LeftImageName;
                yield return behavior.RightImageName;
                foreach (EffectTemplate effect in behavior.Effects)
                {
                    yield return effect.LeftImageName;
                    yield return effect.RightImageName;
                }
            }
        }

        /// <summary>
        /// Raised when the progress for LoadInstancesWorker has changed.
        /// Used to report the status to the user.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void LoadInstancesWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ((string)e.UserState == "Loading images.")
                RunCommand.Enabled = false;
            ProgressBar1.Value = e.ProgressPercentage;
            LoadInfo.Text = (string)e.UserState;
        }

        /// <summary>
        /// Raised when LoadInstancesWorker finishes running.
        /// Starts the graphics form with our pony instances.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void LoadInstancesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "Loaded {1} instances in {0}.",
                (DateTime.UtcNow - instanceLoadStartTime).ToString(@"mm\:ss", CultureInfo.CurrentCulture), totalPonies));
            loadTimer.Dispose();
            loadTimer = new Timer();
            LoadTimeInfo.Text = "";
            LoadInfo.Text = "Ready.";
            RunCommand.Text = "START";
            EnableControls(true);
            RunCommand.Enabled = true;

            if (ponyInstances.Count > 0)
            {
                if (!e.Cancelled)
                    StartAnimator();

                // We no longer need to keep track of these references.
                ponyInstances.Clear();
            }
        }

        /// <summary>
        /// Creates and starts the animation system.
        /// </summary>
        private void StartAnimator()
        {
            System.Drawing.Rectangle area = System.Drawing.Rectangle.Empty;
            foreach (Screen screen in Screen.AllScreens)
                area = System.Drawing.Rectangle.Union(area, screen.WorkingArea);
            PonyInstance.Bounds = area;
            DesktopPonyAnimator animator = new DesktopPonyAnimator(spriteInterface, ponyInstances);
            animator.InstanceCountChanged += (iccSender, iccE) =>
            {
                PonySelectionControl templateControl =
                    ponyDisplays.Find(display => display.Template == iccE.Template).SelectionControl;
                templateControl.Invoke(new MethodInvoker(() => templateControl.PonyCount += iccE.Change));
            };
            animator.AnimatorClosed += (acSender, acE) => Invoke(new MethodInvoker(Show));
            animator.ProgramExitRequested += (perSender, perE) => Invoke(new MethodInvoker(Close));
            animator.Start();

            Hide();
        }
        #endregion

        /// <summary>
        /// Enables or disables controls in bulk.
        /// Affects the FiltersGroup, PoniesGroup, InterfacesGroup, GifAlphaCommand, GifViewerCommand and PonyEditorCommand controls.
        /// </summary>
        /// <param name="enable">Pass true to enable controls, false to disable controls.</param>
        private void EnableControls(bool enable)
        {
            FiltersGroup.Enabled = enable;
            PoniesGroup.Enabled = enable;
            InterfacesGroup.Enabled = enable;
            GifAlphaCommand.Visible = enable;
            GifViewerCommand.Visible = enable;
            PonyEditorCommand.Visible = enable;
        }

        /// <summary>
        /// Raised when GifAlphaCommand is clicked.
        /// Opens the GIF alpha mapping form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GifAlphaCommand_Click(object sender, EventArgs e)
        {
            SwitchToForm<GifAlphaForm>();
        }

        /// <summary>
        /// Raised when GifViewerCommand is clicked.
        /// Opens the GIF viewer form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GifViewerCommand_Click(object sender, EventArgs e)
        {
            SwitchToForm<GifForm>();
        }

        /// <summary>
        /// Raised when PonyEditorCommand is clicked.
        /// Opens the editor form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonyEditorCommand_Click(object sender, EventArgs e)
        {
            List<PonyTemplate> ponyTemplates = new List<PonyTemplate>(ponyDisplays.Count);
            foreach (PonyDisplay display in ponyDisplays)
                ponyTemplates.Add(display.Template);

            var editor = new PonyEditorForm(ponyTemplates);
            editor.Disposed += (dSender, dE) => Show();
            editor.Show();
            Hide();
        }

        /// <summary>
        /// Shows a new form of the specified type and hides the selection menu until the given form is closed, after which it reappears.
        /// </summary>
        /// <typeparam name="TForm">The type of the form to show.</typeparam>
        private void SwitchToForm<TForm>() where TForm : Form, new()
        {
            Form form = new TForm();
            form.Disposed += (sender, e) => Show();
            form.Show();
            Hide();
        }

        /// <summary>
        /// Raised when the animation timer ticks.
        /// Advances the time index of ponies being displayed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (Visible)
                foreach (PonyDisplay display in ponyDisplays)
                    display.SelectionControl.AdvanceTimeIndex(TimeSpan.FromMilliseconds(AnimationTimer.Interval));
        }

        /// <summary>
        /// Raised when the form has been closed.
        /// Releases outstanding resources and performs a garbage collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonySelectionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Prevent closing while a background load is in progress.
            if (LoadTemplatesWorker.IsBusy)
                return;

            if (spriteInterface != null)
                spriteInterface.Dispose();

            Invoke(new MethodInvoker(() =>
            {
                // Hide the form and let cleanup happen without the user having to watch.
                Hide();

                // Dispose of resources.
                Dispose(true);
                if (ponyDisplays != null)
                    foreach (PonyDisplay display in ponyDisplays)
                        display.Dispose();
                foreach (AnimatedImage<BitmapFrame> image in imageManager.InitializedValues)
                    image.Dispose();
                loadTimer.Dispose();
                LoadInstancesWorker.Dispose();
            }));

            // Cleanup the resources the now closed form can finally release.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}