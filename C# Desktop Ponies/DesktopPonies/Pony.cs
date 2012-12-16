namespace CsDesktopPonies.DesktopPonies
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using CsDesktopPonies.SpriteManagement;

    // TODO: Fix documentation.

    #region Enumerations
    /// <summary>
    /// Represents a set of directions for movement.
    /// </summary>
    [Flags]
    public enum Directions
    {
        /// <summary>
        /// No movement.
        /// </summary>
        None = 0,
        /// <summary>
        /// Horizontal movement only.
        /// </summary>
        Horizontal = 0x01,
        /// <summary>
        /// Vertical movement only.
        /// </summary>
        Vertical = 0x02,
        /// <summary>
        /// Diagonal movement only.
        /// </summary>
        Diagonal = 0x04,
        /// <summary>
        /// Horizontal or vertical movement.
        /// </summary>
        HorizontalVertical = Horizontal | Vertical,
        /// <summary>
        /// Horizontal or diagonal movement.
        /// </summary>
        HorizontalDiagonal = Horizontal | Diagonal,
        /// <summary>
        /// Vertical or diagonal movement.
        /// </summary>
        VerticalDiagonal = Vertical | Diagonal,
        /// <summary>
        /// Any or all of horizontal, vertical and diagonal movement.
        /// </summary>
        Any = Horizontal | Vertical | Diagonal
    }

    /// <summary>
    /// Represents the role of a character (how important they are in the show).
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// This character does not fit into any other defined roles.
        /// </summary>
        Other,
        /// <summary>
        /// This is a leading character.
        /// </summary>
        Main,
        /// <summary>
        /// This is a supporting character.
        /// </summary>
        Support,
        /// <summary>
        /// This is a character that has low important but reoccurs occasionally, or was of importance in only a few episodes.
        /// </summary>
        Incidental,
        /// <summary>
        /// This is a background character.
        /// </summary>
        Background,
    }

    /// <summary>
    /// Represents the gender of a character.
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// This character is female.
        /// </summary>
        Female,
        /// <summary>
        /// This character is male.
        /// </summary>
        Male,
    }

    /// <summary>
    /// Represents the race of a character.
    /// </summary>
    public enum PonyRace
    {
        /// <summary>
        /// This character is not a pony.
        /// </summary>
        NonPony,
        /// <summary>
        /// This character is an alicorn pony (possesses both wings and a horn).
        /// </summary>
        Alicorn,
        /// <summary>
        /// This character is a pegasus pony (possesses only wings).
        /// </summary>
        Pegasus,
        /// <summary>
        /// This character is a unicorn pony (possesses only a horn).
        /// </summary>
        Unicorn,
        /// <summary>
        /// This character is an earth pony (possesses neither wings nor a horn).
        /// </summary>
        Earth,
    }

    /// <summary>
    /// Represents possible triggers of speech.
    /// </summary>
    [Flags]
    public enum SpeechTriggers
    {
        /// <summary>
        /// Indicates a speech cannot occur. This is not a valid trigger in configuration files.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates a speech may only occur as a result of being scripted as part of an behavior.
        /// </summary>
        Script = 0x01,
        /// <summary>
        /// Indicates a speech may only occur when the pony has mouseover.
        /// </summary>
        Mouse = 0x02,
        /// <summary>
        /// Indicates a speech may only occur at random intervals.
        /// </summary>
        Random = 0x04,
        /// <summary>
        /// Indicates a speech may occur as a result of a script, or on mouseover.
        /// </summary>
        ScriptMouse = Script | Mouse,
        /// <summary>
        /// Indicates a speech may occur as a result of a script, or at random intervals.
        /// </summary>
        ScriptRandom = Script | Random,
        /// <summary>
        /// Indicates a speech may occur on mouseover, or at random intervals. 
        /// </summary>
        MouseRandom = Mouse | Random,
        /// <summary>
        /// Indicates a speech may occur as a result of an behavior, on mouseover, or at random intervals.
        /// </summary>
        Any = Script | Mouse | Random
    }

    /// <summary>
    /// Indicates how to use a left-facing and right-facing image when drawing an object facing left or right.
    /// </summary>
    public enum ImageFlip
    {
        /// <summary>
        /// Indicates to simply use the image facing in the direction required.
        /// </summary>
        UseOriginal,
        /// <summary>
        /// Indicates that the right-facing image should always be used, and be mirrored when left facing is required.
        /// </summary>
        LeftMirrorsRight,
        /// <summary>
        /// Indicates that the left-facing image should always be used, and be mirrored when right facing is required.
        /// </summary>
        RightMirrorsLeft,
    }
    #endregion

    /// <summary>
    /// Maintains a set of interactions, and activates them when the targets required are present.
    /// </summary>
    public class InteractionManager
    {
        /// <summary>
        /// Filename of the interactions file.
        /// </summary>
        private const string IniFilename = "interactions.ini";
        /// <summary>
        /// List of all possible interactions.
        /// </summary>
        private List<InteractionTemplate> interactions = new List<InteractionTemplate>();
        /// <summary>
        /// List of currently running interactions.
        /// </summary>
        private List<InteractionInstance> runningInteractions = new List<InteractionInstance>();
        /// <summary>
        /// Gets or sets the available targets, which will affect which interactions can be triggered.
        /// </summary>
        public IEnumerable<PonyInstance> AvailableTargets { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.InteractionManager"/> class. Interactions are
        /// loaded from the file expected in the given directory, and are then applied to the given templates.
        /// </summary>
        /// <param name="directory">The directory containing the interactions file.</param>
        /// <param name="templates">The set of <see cref="T:CsDesktopPonies.DesktopPonies.PonyTemplate"/> to which the interactions should
        /// be linked.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="directory"/> is null.-or-<paramref name="templates"/> is null.
        /// </exception>
        public InteractionManager(string directory, IEnumerable<PonyTemplate> templates)
        {
            LoadFromIni(directory, templates);
        }

        /// <summary>
        /// Loads the interactions from the ini file in the given directory, and attempts to link them to the given templates.
        /// </summary>
        /// <param name="directory">The directory containing the interactions file.</param>
        /// <param name="templates">The set of <see cref="T:CsDesktopPonies.DesktopPonies.PonyTemplate"/> to which the interactions should
        /// be linked.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="directory"/> is null.-or-<paramref name="templates"/> is null.
        /// </exception>
        private void LoadFromIni(string directory, IEnumerable<PonyTemplate> templates)
        {
            Argument.EnsureNotNull(directory, "directory");
            Argument.EnsureNotNull(templates, "templates");

            // Open the file for reading.
            string path = Path.Combine(directory, IniFilename);
            using (StreamReader stream = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                while (!stream.EndOfStream)
                {
                    string line = stream.ReadLine();

                    // Ignore blank lines.
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Skip over lines starting with a quote, these are comments.
                    if (line[0] == '\'')
                        continue;

                    // Get the chunks in this line, lines are comma delimited and qualified by a curly brace or double quote pairing.
                    char[] delimiters = new char[1] { ',' };
                    char[,] qualifiers = new char[2, 2] { { '{', '}' }, { '"', '"' } };
                    string[] chunks = line.SplitQualified(delimiters, qualifiers, StringSplitOptions.None);
                    for (int i = 0; i < chunks.Length; i++)
                        chunks[i] = chunks[i].Trim();

                    if (chunks.Length != 8)
                        throw new InvalidDataException("Interaction did not have the required number of elements.");

                    string name = chunks[0];
                    string initiator = chunks[1];
                    // Multiply by 100 to get an integer as most values are no more than 2dp.
                    // The chance given in ini values is not used in the same manner, and there will be a different probability
                    // distribution.
                    int chance = (int)(double.Parse(chunks[2], CultureInfo.InvariantCulture) * 100);
                    int proximity = int.Parse(chunks[3], CultureInfo.InvariantCulture);
                    string[] targets = chunks[4].SplitQualified(delimiters, qualifiers, StringSplitOptions.None);
                    for (int i = 0; i < targets.Length; i++)
                        targets[i] = targets[i].Trim();
                    string selector = chunks[5];
                    string[] behaviorNames = chunks[6].SplitQualified(delimiters, qualifiers, StringSplitOptions.None);
                    int repeatDelay = int.Parse(chunks[7], CultureInfo.InvariantCulture) * 1000;

                    string[] allTargets = new string[targets.Length + 1];
                    for (int i = 0; i < targets.Length; i++)
                        allTargets[i] = targets[i];
                    allTargets[targets.Length] = initiator;

                    PonyTemplate[] allTargetTemplates = new PonyTemplate[allTargets.Length];

                    for (int i = 0; i < allTargets.Length; i++)
                    {
                        bool found = false;
                        foreach (PonyTemplate template in templates)
                            if (template.TemplateName == allTargets[i])
                            {
                                allTargetTemplates[i] = template;
                                found = true;
                                break;
                            }
                        if (!found)
                            throw new InvalidDataException(
                                string.Format(
                                CultureInfo.CurrentCulture,
                                "Interaction \"{0}\" specified a target template named \"{1}\" which did not exist.",
                                name, allTargets[i]));
                    }
                    interactions.Add(
                        new InteractionTemplate(name, repeatDelay, repeatDelay, proximity, behaviorNames[0], allTargetTemplates));
                }
        }

        /// <summary>
        /// Triggers any available interactions for the given time, based on the currently available targets.
        /// </summary>
        /// <param name="currentTime">The current time, against which the ending and cool down of interactions should be checked.</param>
        /// <returns>An array of string which details the current state of all interactions.</returns>
        public string[] TriggerInteractions(TimeSpan currentTime)
        {
            string[] results = new string[5];
            results[0] = "Running:\n";
            results[1] = "Missing behaviors on targets:\n";
            results[2] = "Out of Range:\n";
            results[3] = "Missing targets:\n";
            results[4] = "All Interactions:\n";

            for (int i = 0; i < interactions.Count; i++)
            {
                string targets = "";
                foreach (PonyTemplate target in interactions[i].Targets)
                    targets += target.TemplateName + ", ";
                results[4] += interactions[i].Name + " activates " + interactions[i].BehaviorName + " on  {" + targets + "}\n";
            }

            for (int i = 0; i < runningInteractions.Count; i++)
                results[0] += runningInteractions[i].Template.Name + " ends in " +
                    (runningInteractions[i].InteractionEndTime - currentTime).TotalSeconds + "\n";

            for (int i = 0; i < runningInteractions.Count;)
                if (currentTime > runningInteractions[i].InteractionEndTime)
                    runningInteractions.RemoveAt(i);
                else
                    i++;

            for (int i = 0; i < runningInteractions.Count; i++)
                results[0] += runningInteractions[i].Template.Name + " ends in " +
                    (runningInteractions[i].InteractionEndTime - currentTime).TotalSeconds + "\n";

            foreach (InteractionTemplate interaction in interactions)
            {
                bool alreadyRunning = false;
                foreach (InteractionInstance runningInteraction in runningInteractions)
                    if (interaction.Name == runningInteraction.Template.Name)
                    {
                        alreadyRunning = true;
                        break;
                    }

                if (alreadyRunning)
                    continue;

                PonyInstance[] targets = new PonyInstance[interaction.Targets.Count];

                Point topLeft = new Point(int.MaxValue, int.MaxValue);
                Point bottomRight = new Point(int.MinValue, int.MinValue);
                bool foundAllTargets = true;
                for (int i = 0; i < interaction.Targets.Count; i++)
                {
                    bool foundTarget = false;
                    foreach (PonyInstance candidateTarget in AvailableTargets)
                        if (candidateTarget.Template == interaction.Targets[i])
                        {
                            targets[i] = candidateTarget;
                            if (candidateTarget.Position.X < topLeft.X)
                                topLeft.X = candidateTarget.Position.X;
                            if (candidateTarget.Position.Y < topLeft.Y)
                                topLeft.Y = candidateTarget.Position.Y;
                            if (candidateTarget.Position.X > bottomRight.X)
                                bottomRight.X = candidateTarget.Position.X;
                            if (candidateTarget.Position.Y > bottomRight.Y)
                                bottomRight.Y = candidateTarget.Position.Y;
                            foundTarget = true;
                            break;
                        }
                    if (!foundTarget)
                    {
                        foundAllTargets = false;
                        break;
                    }
                }

                if (foundAllTargets)
                {
                    int distance = (int)Vector.Distance(topLeft, bottomRight);
                    if (Vector.Distance(topLeft, bottomRight) < interaction.Proximity)
                    {
                        if (interaction.Run(targets))
                        {
                            runningInteractions.Add(new InteractionInstance(interaction, currentTime));
                        }
                        else
                        {
                            results[1] += interaction.Name + " has behaviors missing from its targets.\n";
                        }
                    }
                    else
                    {
                        results[2] += interaction.Name + " range of " + distance + " is above max of " + interaction.Proximity + "\n";
                    }
                }
                else
                {
                    results[3] += interaction.Name + " has missing targets.\n";
                }
            }

            return results;
        }
    }

    /// <summary>
    /// Runs a specific <see cref="T:CsDesktopPonies.DesktopPonies.InteractionTemplate"/> for its duration.
    /// </summary>
    public class InteractionInstance
    {
        /// <summary>
        /// Gets the template on which this instance is based.
        /// </summary>
        public InteractionTemplate Template { get; private set; }
        /// <summary>
        /// Gets the time at which the interaction started, as compared to the total elapsed time.
        /// </summary>
        public TimeSpan InteractionStartTime { get; private set; }
        /// <summary>
        /// Gets the time at which the interaction will end, as compared to the total elapsed time.
        /// </summary>
        public TimeSpan InteractionEndTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.InteractionInstance"/> class.
        /// </summary>
        /// <param name="template">The <see cref="T:CsDesktopPonies.DesktopPonies.InteractionTemplate"/> on which this instance is based.
        /// </param>
        /// <param name="currentTime">The current instant in time, used to measure the duration of this interaction.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="template"/> is null.</exception>
        public InteractionInstance(InteractionTemplate template, TimeSpan currentTime)
        {
            Argument.EnsureNotNull(template, "template");

            Template = template;
            InteractionStartTime = currentTime;
            InteractionEndTime = InteractionStartTime + TimeSpan.FromMilliseconds(Template.MinRepeatDuration);
        }
    }

    /// <summary>
    /// Defines an interaction between a set of ponies.
    /// </summary>
    public class InteractionTemplate
    {
        /// <summary>
        /// Gets the name of this interaction.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the minimum waiting time, in milliseconds, before repeating this interaction.
        /// </summary>
        public int MinRepeatDuration { get; private set; }
        /// <summary>
        /// Gets the maximum waiting time, in milliseconds, before repeating this interaction.
        /// </summary>
        public int MaxRepeatDuration { get; private set; }

        /// <summary>
        /// Gets the range within which all the targets must be in relation to each other in order for this interaction in order for it to
        /// be triggered.
        /// </summary>
        public int Proximity { get; private set; }
        /// <summary>
        /// Gets the name of the behavior initiated on all targets when this interaction triggers.
        /// </summary>
        public string BehaviorName { get; private set; }

        /// <summary>
        /// Gets the set of target templates that will be involved in the interaction.
        /// </summary>
        public IList<PonyTemplate> Targets { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.InteractionTemplate"/> class.
        /// </summary>
        /// <param name="name">The name of the interaction.</param>
        /// <param name="minRepeatDuration">The minimum time to wait, in milliseconds, before repeating this interaction.</param>
        /// <param name="maxRepeatDuration">The maximum time to wait, in milliseconds, before repeating this interaction.</param>
        /// <param name="proximity">The distance within which all targets must lie in order for the interaction to trigger.
        /// A value of 0 indicates this value should be ignored, that is, the interaction may be triggered at any distance.</param>
        /// <param name="behaviorName">The name of the behavior that will be initiated on all targets when this interaction triggers.
        /// </param>
        /// <param name="targets">The collection of <see cref="T:CsDesktopPonies.DesktopPonies.PonyTemplate"/> which are involved in this
        /// interaction.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="targets"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="targets"/> contains less than two targets.</exception>
        public InteractionTemplate(string name, int minRepeatDuration, int maxRepeatDuration, int proximity, string behaviorName,
            ICollection<PonyTemplate> targets)
        {
            Argument.EnsureNotNull(targets, "targets");

            if (targets.Count < 2)
                throw new ArgumentException("There must be at least two targets.", "targets");

            Name = name;
            MinRepeatDuration = minRepeatDuration;
            MaxRepeatDuration = maxRepeatDuration;
            Proximity = proximity;
            BehaviorName = behaviorName;
            Targets = new PonyTemplate[targets.Count];
            int i = 0;
            foreach (PonyTemplate target in targets)
                Targets[i++] = target;
        }

        /// <summary>
        /// Attempts to run the interaction on the given targets.
        /// </summary>
        /// <param name="targets">The targets on which to initiate this interaction.</param>
        /// <returns>Returns true if the interaction was initiated; false if any of the targets were missing behaviors required by the
        /// interaction.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="targets"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="targets"/> contains less than two targets.</exception>
        public bool Run(IList<PonyInstance> targets)
        {
            Argument.EnsureNotNull(targets, "targets");

            if (targets.Count < 2)
                throw new ArgumentException("There must be at least two targets.", "targets");

            Behavior[] behaviors = new Behavior[targets.Count];
            bool behaviorsExist = true;
            for (int i = 0; i < targets.Count; i++)
            {
                bool behaviorFound = false;
                foreach (Behavior behavior in targets[i].Template.Behaviors)
                    if (behavior.Name == BehaviorName)
                    {
                        behaviors[i] = behavior;
                        behaviorFound = true;
                        break;
                    }
                if (!behaviorFound)
                {
                    behaviorsExist = false;
                    break;
                }
            }
            if (behaviorsExist)
                for (int i = 0; i < targets.Count; i++)
                    targets[i].SelectBehavior(behaviors[i]);
            return behaviorsExist;
        }
    }

    /// <summary>
    /// Realizes a single instance of a <see cref="T:CsDesktopPonies.DesktopPonies.PonyTemplate"/>.
    /// </summary>
    public sealed class PonyInstance : ISpeakingSprite
    {
        /// <summary>
        /// Gets or sets the current bounds within which instances should remain when starting or updating.
        /// </summary>
        public static Rectangle Bounds { get; set; }
        /// <summary>
        /// Gets or sets the current cursor position, which will be reacted to.
        /// </summary>
        public static Point CursorPosition { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current image needs to be flipped.
        /// </summary>
        public bool FlipImage { get; private set; }
        /// <summary>
        /// Gets the path to the image required to display the pony as it performs its current behavior.
        /// </summary>
        public string ImagePath
        {
            get { return MovingLeft ? CurrentBehavior.LeftImageName : CurrentBehavior.RightImageName; }
        }
        /// <summary>
        /// Gets the region the the pony currently occupies.
        /// </summary>
        public Rectangle Region
        {
            get { return DrawRectangle; }
        }

        /// <summary>
        /// Gets a value indicating whether the pony is currently speaking.
        /// </summary>
        public bool IsSpeaking { get; private set; }
        /// <summary>
        /// Gets the current speech text that is being spoken by the pony.
        /// </summary>
        public string SpeechText
        {
            get { return currentSpeech.DisplayText; }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is over this pony.
        /// </summary>
        public bool HasMouseover { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether the pony is currently being dragged.
        /// </summary>
        public bool IsDragging { get; set; }
        /// <summary>
        /// Gets the template on which this instance is based.
        /// </summary>
        public PonyTemplate Template { get; private set; }
        /// <summary>
        /// Gets the current behavior being performed.
        /// </summary>
        public Behavior CurrentBehavior { get; private set; }
        /// <summary>
        /// Gets the instant in time that is the time index for the current behavior.
        /// </summary>
        public TimeSpan CurrentTime { get; private set; }
        /// <summary>
        /// Gets the time at which the behavior started, as compared to the total elapsed time.
        /// </summary>
        public TimeSpan BehaviorStartTime { get; private set; }
        /// <summary>
        /// Gets the time at which the behavior will end, as compared to the total elapsed time.
        /// </summary>
        public TimeSpan BehaviorEndTime { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the pony is moving left or right.
        /// If true, the pony is moving left. Otherwise it is moving right.
        /// </summary>
        public bool MovingLeft { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the pony is moving up or down.
        /// If true, the pony is moving up. Otherwise it is moving down.
        /// </summary>
        public bool MovingUp { get; private set; }

        /// <summary>
        /// The center position of this pony.
        /// </summary>
        private Point position;
        /// <summary>
        /// Gets or sets the center position of this pony.
        /// </summary>
        public Point Position
        {
            get { return position; }
            set { position = value; }
        }
        /// <summary>
        /// The rectangle occupied by drawing the pony.
        /// </summary>
        private Rectangle drawRectangle;
        /// <summary>
        /// Gets the rectangle the will be occupied by drawing the pony image. This excludes the area affected by speech and effects.
        /// </summary>
        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
        }

        /// <summary>
        /// Indicates which direction has been selected for the behavior from the allowable set of directions.
        /// </summary>
        private Directions chosenDirection;
        /// <summary>
        /// The line of text that is being spoken.
        /// </summary>
        private Speech currentSpeech;
        /// <summary>
        /// The time when the line of text being spoken should stop.
        /// </summary>
        private TimeSpan speechEndTime;
        /// <summary>
        /// Gets a collection of the effects currently running for this pony.
        /// </summary>
        public LinkedList<EffectInstance> ActiveEffects { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.PonyInstance"/> class based on a template.
        /// </summary>
        /// <param name="template">The <see cref="T:CsDesktopPonies.DesktopPonies.PonyTemplate"/> on which this pony is to be based.
        /// </param>
        public PonyInstance(PonyTemplate template)
        {
            Template = template;
            ActiveEffects = new LinkedList<EffectInstance>();

            SelectBehavior(null);
            MovingLeft = Rng.Next(2) == 0;

            if (MovingLeft)
                drawRectangle.Size = CurrentBehavior.LeftImageDrawSize;
            else
                drawRectangle.Size = CurrentBehavior.RightImageDrawSize;
        }

        /// <summary>
        /// Starts the pony using the given time as a zero point. The pony starts randomly within the given bounds.
        /// </summary>
        /// <param name="startTime">The time that will be used as a zero point against the time given in future updates.</param>
        public void Start(TimeSpan startTime)
        {
            CurrentTime = startTime;

            // Get the offset of the draw, we need this to adjust the resulting position.
            Point offset = OffsetDrawLocation();

            // Start at a random position within bounds, but offset inwards by the size of the draw.
            // This ensures the correct size of the range of values, but it will be off be a constant factor.
            Position = new Point(Rng.Next(drawRectangle.Width, Bounds.Width), Rng.Next(drawRectangle.Height, Bounds.Height));

            // Adjust for the offset of the boundary.
            position.X += Bounds.X;
            position.Y += Bounds.Y;

            // Remove the offset, so the minimum and maximum ranges of position are now correct.
            position.X += offset.X;
            position.Y += offset.Y;

            // Set draw location.
            drawRectangle.Location = OffsetDrawLocation();
        }

        /// <summary>
        /// Updates the pony to the given instant in time. The pony will stay within the given bounds, and use the cursor to determine
        /// mouseover and trigger speech.
        /// </summary>
        /// <param name="updateTime">The instant in time which the pony should update itself to.</param>
        public void Update(TimeSpan updateTime)
        {
            // Run the update the required number of times to catch up. This will cause temporal aliasing as different numbers of updates
            // may be run depending on how often the function is called. However sprite images are pixel aligned and do not have
            // particularly large frame rates, so the effect is unlikely to be noticeable in comparison.
            while (updateTime - CurrentTime > TimeSpan.FromMilliseconds(30))
                UpdateOnce(Bounds, CursorPosition);
        }

        /// <summary>
        /// Runs one update for a pony to advance its state. This method advances the state by 30ms with each call.
        /// </summary>
        /// <param name="drawBounds">The bounds in which the pony should remain.</param>
        /// <param name="cursorPosition">The current position of the cursor, used to determine mouseover and trigger speeches.</param>
        private void UpdateOnce(Rectangle drawBounds, Point cursorPosition)
        {
            // Step forward in time.
            CurrentTime += TimeSpan.FromMilliseconds(30);

            // Check for mouseover.
            // Currently this just checks the bounding box, and thus mouseover can occur on transparent parts of the image.
            HasMouseover =
                cursorPosition.X >= drawRectangle.Left &&
                cursorPosition.Y >= drawRectangle.Top &&
                cursorPosition.X < drawRectangle.Right &&
                cursorPosition.Y < drawRectangle.Bottom;

            UpdateEffects();

            // Check if the current behavior has expired, and select a new one if it has.
            if (CurrentTime > BehaviorEndTime)
                StartNewBehavior();

            // Move the pony.
            if (IsDragging)
                Position = cursorPosition;
            else
                Move(drawBounds);

            // Offset the draw position of the image.
            drawRectangle.Location = OffsetDrawLocation();

            UpdateSpeech();
        }

        /// <summary>
        /// Updates the active effects for the pony.
        /// </summary>
        private void UpdateEffects()
        {
            // Update effects.
            foreach (EffectInstance effect in ActiveEffects)
                effect.Update(CurrentTime);

            // Remove effects that have ended, and are not set to repeat later.
            LinkedListNode<EffectInstance> effectNode = ActiveEffects.First;
            while (effectNode != null)
            {
                if (effectNode.Value.HasEnded && !effectNode.Value.RepeatPending)
                    ActiveEffects.Remove(effectNode);
                effectNode = effectNode.Next;
            }

            // Check if repeating effects need to be added.
            effectNode = ActiveEffects.Last;
            while (effectNode != null)
            {
                if (effectNode.Value.RepeatPending && CurrentTime > effectNode.Value.EffectRepeatTime)
                {
                    effectNode.Value.RepeatPending = false;
                    // Only repeat an effect if the trigger behavior is still the current behavior.
                    if (CurrentBehavior == effectNode.Value.Trigger)
                        ActiveEffects.AddLast(new EffectInstance(effectNode.Value.Template, this, CurrentBehavior, CurrentTime));
                }
                effectNode = effectNode.Previous;
            }
        }

        /// <summary>
        /// Ends the current behavior, then begins a new behavior.
        /// </summary>
        private void StartNewBehavior()
        {
            // Display end speech for the expired behavior.
            if (CurrentBehavior.EndSpeech != null)
                DisplaySpeech(CurrentBehavior.EndSpeech, true, TimeSpan.FromSeconds(3));

            // Move to the next behavior in the sequence, or else select one at random.
            SelectBehavior(CurrentBehavior.NextBehavior);

            // Start any effects for the new behavior.
            foreach (EffectTemplate effect in CurrentBehavior.Effects)
                ActiveEffects.AddLast(new EffectInstance(effect, this, CurrentBehavior, CurrentTime));

            // Set draw size.
            if (MovingLeft)
                drawRectangle.Size = CurrentBehavior.LeftImageDrawSize;
            else
                drawRectangle.Size = CurrentBehavior.RightImageDrawSize;
        }

        /// <summary>
        /// Moves the pony according to the current behavior, staying within the given bounds.
        /// </summary>
        /// <param name="bounds">The area in which the pony should stay while moving.</param>
        private void Move(Rectangle bounds)
        {
            // Move according to the current behavior.
            const double NewDirectionChance = 0.014;
            const double FlipDirectionAxisChance = 0.007;

            // Randomly move along a new direction.
            if (Rng.NextDouble() < NewDirectionChance)
                SelectChosenDirection();

            // The speed is applied along each axis, so if we are moving diagonally we need to scale it down so the overall magnitude is
            // unaffected.
            int speed = CurrentBehavior.Speed;
            if (chosenDirection == Directions.Diagonal)
                speed = (int)Math.Round(speed / Math.Sqrt(2));

            // Update our horizontal position, staying within the bounds given.
            if (chosenDirection == Directions.Horizontal || chosenDirection == Directions.Diagonal)
            {
                // Select movement along given direction.
                if (drawRectangle.Left < bounds.Left)
                    MovingLeft = false;
                else if (drawRectangle.Right > bounds.Right)
                    MovingLeft = true;
                else if (Rng.NextDouble() < FlipDirectionAxisChance)
                    MovingLeft = !MovingLeft;

                // Apply movement.
                if (MovingLeft)
                    position.X -= speed;
                else
                    position.X += speed;
            }

            // Determine if the image will need to be flipped to be displayed.
            FlipImage =
                (CurrentBehavior.BehaviorImageFlip == ImageFlip.LeftMirrorsRight && MovingLeft) ||
                (CurrentBehavior.BehaviorImageFlip == ImageFlip.RightMirrorsLeft && !MovingLeft);

            // Update our vertical position, staying within the bounds given.
            if (chosenDirection == Directions.Vertical || chosenDirection == Directions.Diagonal)
            {
                // Select movement along given direction.
                if (drawRectangle.Top < bounds.Top)
                    MovingUp = false;
                else if (drawRectangle.Bottom > bounds.Bottom)
                    MovingUp = true;
                else if (Rng.NextDouble() < FlipDirectionAxisChance)
                    MovingUp = !MovingUp;

                // Apply movement.
                if (MovingUp)
                    position.Y -= speed;
                else
                    position.Y += speed;
            }
        }

        /// <summary>
        /// Calculates the top-left draw location of the image so that the position of the pony is in the defined center.
        /// </summary>
        /// <returns>The point the represents the location where the top-left corner should be drawn in order that the image center aligns
        /// with the current position.</returns>
        private Point OffsetDrawLocation()
        {
            // Adjust the position so the given point will be the center of the draw.
            // If no custom point is given for the image, use the actual center point.
            if (MovingLeft)
            {
                Point leftCenter = CurrentBehavior.LeftImageCenter.GetValueOrDefault(
                    new Point(CurrentBehavior.LeftImageDrawSize.Width / 2, CurrentBehavior.LeftImageDrawSize.Height / 2));
                return new Point(Position.X - leftCenter.X, Position.Y - leftCenter.Y);
            }
            else
            {
                Point rightCenter = CurrentBehavior.RightImageCenter.GetValueOrDefault(
                    new Point(CurrentBehavior.RightImageDrawSize.Width / 2, CurrentBehavior.RightImageDrawSize.Height / 2));
                return new Point(Position.X - rightCenter.X, Position.Y - rightCenter.Y);
            }
        }

        /// <summary>
        /// Activates mouseover and random speeches and sets up the speech bubble for drawing.
        /// </summary>
        private void UpdateSpeech()
        {
            // Display a speech if the mouse is over us.
            if (HasMouseover && Template.MouseoverSpeeches.Count != 0)
                DisplaySpeech(
                    Template.MouseoverSpeeches[Rng.Next(Template.MouseoverSpeeches.Count)], false, TimeSpan.FromSeconds(3));

            // Speak a line at random.
            const double RandomSpeechChance = 0.005;
            if (Template.RandomSpeeches.Count != 0 && Rng.NextDouble() < RandomSpeechChance)
                DisplaySpeech(Template.RandomSpeeches[Rng.Next(Template.RandomSpeeches.Count)], false, TimeSpan.FromSeconds(3));

            // Check if there is active speech.
            IsSpeaking = speechEndTime > CurrentTime;
            if (!IsSpeaking)
            {
                // Keep the end time in line with elapsed time if there is no speech.
                // This is so the next speech triggered can just extend the end time.
                speechEndTime = CurrentTime;
                currentSpeech = null;
            }
        }

        /// <summary>
        /// Selects the given behavior to perform immediately, or a random behavior if null is given.
        /// </summary>
        /// <param name="behavior">The behavior to perform, or null to randomly select an behavior.</param>
        public void SelectBehavior(Behavior behavior)
        {
            // Select an behavior at random.
            if (behavior == null)
            {
                int chanceToSeek = Rng.Next(Template.ChanceTotal);
                int behaviorIndex = 0;
                while (chanceToSeek > Template.Behaviors[behaviorIndex].Chance)
                    chanceToSeek -= Template.Behaviors[behaviorIndex++].Chance;
                behavior = Template.Behaviors[behaviorIndex];
            }

            // Use this behavior and choose a direction of movement.
            CurrentBehavior = behavior;
            SelectChosenDirection();

            // Define the behavior start/end times.
            BehaviorStartTime = CurrentTime;
            BehaviorEndTime = CurrentTime + TimeSpan.FromMilliseconds(Rng.Next(CurrentBehavior.MinDuration, CurrentBehavior.MaxDuration));

            // Display the starting speech, if any.
            if (CurrentBehavior.StartSpeech != null)
                DisplaySpeech(CurrentBehavior.StartSpeech, true, TimeSpan.FromSeconds(3));
        }

        /// <summary>
        /// Chooses one possible direction to move in from the allowable directions for the movement.
        /// This means the chosen direction will be one of horizontal, vertical or diagonal only.
        /// </summary>
        private void SelectChosenDirection()
        {
            if (CurrentBehavior.MovementAllowed == Directions.Any)
            {
                double rand = Rng.NextDouble();
                if (rand < 1d / 3d)
                    chosenDirection = Directions.Horizontal;
                else if (rand < 2d / 3d)
                    chosenDirection = Directions.Vertical;
                else
                    chosenDirection = Directions.Diagonal;
            }
            else if (CurrentBehavior.MovementAllowed == Directions.HorizontalVertical)
            {
                if (Rng.NextDouble() < 0.5)
                    chosenDirection = Directions.Horizontal;
                else
                    chosenDirection = Directions.Vertical;
            }
            else if (CurrentBehavior.MovementAllowed == Directions.HorizontalDiagonal)
            {
                if (Rng.NextDouble() < 0.5)
                    chosenDirection = Directions.Horizontal;
                else
                    chosenDirection = Directions.Diagonal;
            }
            else if (CurrentBehavior.MovementAllowed == Directions.VerticalDiagonal)
            {
                if (Rng.NextDouble() < 0.5)
                    chosenDirection = Directions.Vertical;
                else
                    chosenDirection = Directions.Diagonal;
            }
            else
            {
                chosenDirection = CurrentBehavior.MovementAllowed;
            }
        }

        /// <summary>
        /// Sets the current speech of the pony, to be displayed in a speech bubble when the instance is drawn.
        /// </summary>
        /// <param name="speech">The speech to display.</param>
        /// <param name="overrideExisting">If a speech is already being displayed, indicates if it should be overridden.</param>
        /// <param name="time">The length of time to display the line for.</param>
        private void DisplaySpeech(Speech speech, bool overrideExisting, TimeSpan time)
        {
            if (overrideExisting || currentSpeech == null)
            {
                currentSpeech = speech;
                speechEndTime += time;
            }
        }
    }

    /// <summary>
    /// Represents the a basis for a pony, which can be realized by a <see cref="T:CsDesktopPonies.DesktopPonies.PonyInstance"/>.
    /// </summary>
    public class PonyTemplate
    {
        /// <summary>
        /// The name of the xml file we expect in the directory.
        /// </summary>
        private const string XmlFilename = "pony.xml";
        /// <summary>
        /// The name of the ini file we expect in the directory.
        /// </summary>
        private const string IniFilename = "pony.ini";
        /// <summary>
        /// The version of the configuration file.
        /// If new features are added that don't break the format, the minor version is incremented.
        /// This indicates older files of the same major version can still be read by the program.
        /// If a breaking change is made to the format, the major version is incremented.
        /// This indicates the files must be updated before they can be read by the program.
        /// </summary>
        private static readonly Version Version = new Version(1, 0);
        /// <summary>
        /// Shared cache of reader settings that affect how xml files are read.
        /// </summary>
        private static XmlReaderSettings readerSettings;
        /// <summary>
        /// Shared cache of writer settings that affect how xml files are written.
        /// </summary>
        private static XmlWriterSettings writerSettings;

        /// <summary>
        /// Gets the directory that contains the files for this template.
        /// </summary>
        public string Directory { get; private set; }
        /// <summary>
        /// Gets the name of the character template, given in the Directory string.
        /// </summary>
        public string TemplateName { get; private set; }
        /// <summary>
        /// The comment in the configuration file, if any.
        /// </summary>
        private string fileComment;
        /// <summary>
        /// Gets the name of this character, for use in speech bubbles.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the role of this character.
        /// </summary>
        public Role Role { get; private set; }
        /// <summary>
        /// Gets the gender of this character.
        /// </summary>
        public Gender Gender { get; private set; }
        /// <summary>
        /// Gets the type of pony race of this character.
        /// </summary>
        public PonyRace Race { get; private set; }
        /// <summary>
        /// Gets a lookup table for the names of each behavior group. An entry for every index in use is not guaranteed to exist.
        /// </summary>
        public IDictionary<int, string> BehaviorGroupNames { get; private set; }
        /// <summary>
        /// Gets the possible behaviors this pony can undertake.
        /// </summary>
        public IList<Behavior> Behaviors { get; private set; }
        /// <summary>
        /// Gets the sum of the chance values for all behaviors.
        /// </summary>
        public int ChanceTotal { get; private set; }
        /// <summary>
        /// Gets the available lines of dialogue.
        /// </summary>
        public IList<Speech> Speeches { get; private set; }
        /// <summary>
        /// Gets the available lines of dialogue for use at random intervals.
        /// </summary>
        public IList<Speech> RandomSpeeches { get; private set; }
        /// <summary>
        /// Gets the available lines of dialogue for use on mouseover.
        /// </summary>
        public IList<Speech> MouseoverSpeeches { get; private set; }
        /// <summary>
        /// Gets the set of all effects this pony may use.
        /// </summary>
        public IList<EffectTemplate> Effects { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.PonyTemplate"/> class using configuration and
        /// images files in the given directory.
        /// </summary>
        /// <param name="directory">The directory in which the files are located. The directory must contain a configuration file and any
        /// images referred to in that file.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="directory"/> is null.</exception>
        /// <exception cref="System.IO.InvalidDataException">Thrown when the configuration file is badly formed. Check the inner exception
        /// for more information if this occurs.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Throw when the configuration file could not be found-or-when a filename
        /// given within the configuration file could not be found.</exception>
        /// <exception cref="System.IO.IOException">Thrown when an I/O error occurs when attempting to read the configuration file.
        /// </exception>
        public PonyTemplate(string directory)
        {
            LoadFromXml(directory);
        }

        /// <summary>
        /// Initializes a new instance of the PonyTemplate class using configuration and images files in the given directory.
        /// </summary>
        /// <param name="directory">The directory in which the files are located.
        /// The directory must contain a configuration file and any images referred to in that file.</param>
        /// <param name="vbFiles">If true, indicates these are VB version files instead of new files.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="directory"/> is null.</exception>
        /// <exception cref="System.IO.InvalidDataException">Thrown when the configuration file is badly formed. Check the inner exception
        /// for more information if this occurs.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Throw when the configuration file could not be found-or-when a filename
        /// given within the configuration file could not be found.</exception>
        /// <exception cref="System.IO.IOException">Thrown when an I/O error occurs when attempting to read the configuration file.
        /// </exception>
        public PonyTemplate(string directory, bool vbFiles)
        {
            try
            {
                if (vbFiles)
                    LoadFromIni(directory);
                else
                    LoadFromXml(directory);
            }
            catch (Exception ex)
            {
                throw new Exception("Error attempting to load pony template \"" +
                    Path.Combine(directory, vbFiles ? PonyTemplate.IniFilename : PonyTemplate.XmlFilename) + "\"", ex);
            }
        }

        /// <summary>
        /// Load the xml configuration file in the given directory to create the template.
        /// </summary>
        /// <param name="directory">The directory which contains the xml file.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="directory"/> is null.</exception>
        /// <exception cref="System.IO.InvalidDataException">Thrown when the configuration file is badly formed. Check the inner exception
        /// for more information if this occurs.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Throw when the configuration file could not be found-or-when a filename
        /// given within the configuration file could not be found.</exception>
        /// <exception cref="System.IO.IOException">Thrown when an I/O error occurs when attempting to read the configuration file.
        /// </exception>
        private void LoadFromXml(string directory)
        {
            Argument.EnsureNotNull(directory, "directory");

            Directory = directory;
            TemplateName = Directory.Substring(Directory.LastIndexOf(Path.DirectorySeparatorChar) + 1);

            // Set up the reader if we have not done so.
            if (readerSettings == null)
                readerSettings = new XmlReaderSettings()
                {
                    CloseInput = true,
                    IgnoreWhitespace = true,
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    DtdProcessing = DtdProcessing.Ignore,
                };
            
            // Open the file for reading.
            FileStream stream = null;
            string path = Path.Combine(Directory, XmlFilename);
            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                XmlReader reader = XmlReader.Create(stream, readerSettings);

                reader.Read(); // Read the XML declaration.
                reader.Read(); // Advance to the root node.

                // Check it is the pony node, and check the version.
                if (!(reader.NodeType == XmlNodeType.Element && reader.Name == "pony"))
                    throw new InvalidDataException("Could not find the expected pony tag. It must be the root tag.");
                if (!reader.MoveToAttribute("version"))
                    throw new InvalidDataException("Pony element does not contain the required \"version\" attribute");
                if (reader.Value != Version.ToString(2))
                    throw new InvalidDataException("Pony version attribute did not match expected value: " + Version.ToString(2));
                reader.MoveToElement();
                reader.ReadStartElement("pony");

                // If there is an info section, save the text.
                if (reader.Name == "info")
                {
                    reader.ReadStartElement("info");
                    fileComment = reader.ReadContentAsString();
                    reader.ReadEndElement();
                }

                // Read the pony's name.
                reader.ReadStartElement("name");
                Name = reader.ReadContentAsString();
                reader.ReadEndElement();

                // Read the pony's role.
                reader.ReadStartElement("role");
                Role role;
                ParseEnumFromValue<Role>(reader.ReadContentAsString(), out role);
                Role = role;
                reader.ReadEndElement();

                // Read the pony's gender.
                reader.ReadStartElement("gender");
                Gender gender;
                ParseEnumFromValue<Gender>(reader.ReadContentAsString(), out gender);
                Gender = gender;
                reader.ReadEndElement();

                // Read the pony's race.
                reader.ReadStartElement("race");
                PonyRace race;
                ParseEnumFromValue<PonyRace>(reader.ReadContentAsString(), out race);
                Race = race;
                reader.ReadEndElement();

                // Skip any future elements added after these elements.
                if (reader.Name != "dialogue")
                    if (!reader.ReadToFollowing("dialogue"))
                        throw new InvalidDataException("Unable to locate required tag \"dialogue\"");

                // Read in the dialogue.
                Speeches = new List<Speech>();
                ReadDialogueFromXml(reader);

                // Skip any future elements added after these elements.
                if (reader.Name != "behaviors")
                    if (!reader.ReadToFollowing("behaviors"))
                        throw new InvalidDataException("Unable to locate required tag \"behaviors\"");

                // Read in the behaviors.
                Behaviors = new List<Behavior>();
                ReadBehaviorsFromXml(reader);

                // Skip any future elements added after these elements.
                if (reader.Name != "effects")
                    if (!reader.ReadToFollowing("effects"))
                        throw new InvalidDataException("Unable to locate required tag \"effects\"");

                // Read in the effects.
                Effects = new List<EffectTemplate>();
                ReadEffectsFromXml(reader);

                // Skip any future elements that may be added after these, and before the closing pony tag.
                if (reader.NodeType != XmlNodeType.EndElement || reader.Name != "pony")
                    reader.ReadToNextSibling("pony");

                // Read the end of the file.
                reader.ReadEndElement();
                if (!reader.EOF)
                    throw new InvalidDataException("File did not end after closing pony tag. Nothing should come after this tag.");

                // Resolve names into objects, and calculate the chance total.
                foreach (Behavior behavior in Behaviors)
                {
                    behavior.ResolveNextBehavior(Behaviors);
                    behavior.ResolveEffects(Effects);
                    ChanceTotal += behavior.Chance;
                }

                // Generate lists of speech for use at random intervals and on mouseover.
                RandomSpeeches = new List<Speech>();
                MouseoverSpeeches = new List<Speech>();
                foreach (Speech speech in Speeches)
                {
                    if (speech.Trigger.HasFlag(SpeechTriggers.Random))
                        RandomSpeeches.Add(speech);
                    if (speech.Trigger.HasFlag(SpeechTriggers.Mouse))
                        MouseoverSpeeches.Add(speech);
                }

                // Initialize an empty dictionary for behavior group names, since they are not defined in .xml
                BehaviorGroupNames = new Dictionary<int, string>(0);
            }
            catch (InvalidDataException ex)
            {
                throw new InvalidDataException("File \"" + path + "\" did not conform to the expected format.", ex);
            }
            catch (XmlException ex)
            {
                throw new InvalidDataException("File \"" + path + "\" did not conform to the expected format.\n" +
                "Error on line " + ex.LineNumber + " at position " + ex.LinePosition + ".", ex);
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidDataException("File \"" + path + "\" has a value that could not be parsed.", ex);
            }
            catch (FormatException ex)
            {
                throw new InvalidDataException("File \"" + path + "\" has a value that could not be parsed.", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidDataException("File \"" + path + "\" contains a value that was not allowable.", ex);
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        /// Reads the dialogue section in the given XmlReader.
        /// </summary>
        /// <param name="reader">The XmlReader which is positioned to read the dialogue subtree.</param>
        private void ReadDialogueFromXml(XmlReader reader)
        {
            reader.ReadStartElement("dialogue");
            while (reader.Name != "dialogue")
            {
                reader.ReadStartElement("speech");

                string name;
                reader.ReadStartElement("name");
                name = reader.ReadContentAsString();
                reader.ReadEndElement();

                string line;
                reader.ReadStartElement("line");
                line = reader.ReadContentAsString();
                reader.ReadEndElement();

                SpeechTriggers trigger;
                reader.ReadStartElement("trigger");
                ParseEnumFromValue<SpeechTriggers>(reader.ReadContentAsString(), out trigger);
                reader.ReadEndElement();

                // Skip any future elements added to the speech list.
                if (reader.Name != "speech")
                    reader.ReadToNextSibling("speech");

                // Create a new Speech from the given values.
                Speeches.Add(new Speech(name, Name, line, trigger));
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// Reads the behaviors section in the given XmlReader.
        /// </summary>
        /// <param name="reader">The XmlReader which is positioned to read the behaviors subtree.</param>
        private void ReadBehaviorsFromXml(XmlReader reader)
        {
            reader.ReadStartElement("behaviors");
            do
            {
                reader.ReadStartElement("behavior");

                string name;
                reader.ReadStartElement("name");
                name = reader.ReadContentAsString();
                reader.ReadEndElement();

                int chance;
                reader.ReadStartElement("chance");
                chance = reader.ReadContentAsInt();
                reader.ReadEndElement();

                int minDuration;
                reader.ReadStartElement("minDuration");
                minDuration = reader.ReadContentAsInt();
                reader.ReadEndElement();

                int maxDuration;
                reader.ReadStartElement("maxDuration");
                maxDuration = reader.ReadContentAsInt();
                reader.ReadEndElement();

                if (maxDuration < minDuration)
                    throw new InvalidDataException("The minimum duration is less than the maximum duration.");

                int speed;
                reader.ReadStartElement("speed");
                speed = reader.ReadContentAsInt();
                reader.ReadEndElement();

                Directions movement;
                reader.ReadStartElement("movement");
                ParseEnumFromValue<Directions>(reader.ReadContentAsString(), out movement);
                reader.ReadEndElement();

                string leftImage;
                reader.ReadStartElement("leftImage");
                leftImage = reader.ReadContentAsString();
                reader.ReadEndElement();

                string rightImage;
                reader.ReadStartElement("rightImage");
                rightImage = reader.ReadContentAsString();
                reader.ReadEndElement();

                Speech startSpeech;
                reader.ReadStartElement("startSpeech");
                startSpeech = FindSpeechFromName(reader.ReadContentAsString(), SpeechTriggers.Script);
                reader.ReadEndElement();

                Speech endSpeech;
                reader.ReadStartElement("endSpeech");
                endSpeech = FindSpeechFromName(reader.ReadContentAsString(), SpeechTriggers.Script);
                reader.ReadEndElement();

                string nextBehaviorName;
                reader.ReadStartElement("nextBehavior");
                nextBehaviorName = reader.ReadContentAsString();
                reader.ReadEndElement();

                List<string> effectNames = new List<string>();
                reader.ReadStartElement("effects");
                while (reader.Name != "effects")
                {
                    reader.ReadStartElement("effect");
                    effectNames.Add(reader.ReadContentAsString());
                    reader.ReadEndElement();
                }
                reader.ReadEndElement();

                // Skip any future elements added to the behavior list.
                if (reader.Name != "behavior")
                    reader.ReadToNextSibling("behavior");

                // Get image filenames and resolve image mirroring.
                string leftImageName = leftImage == "mirror" ? null : Path.Combine(Directory, leftImage);
                string rightImageName = rightImage == "mirror" ? null : Path.Combine(Directory, rightImage);
                ImageFlip imageFlip = ImageFlip.UseOriginal;
                if (leftImageName == null && rightImageName == null)
                    throw new InvalidDataException(
                        "Behavior \"" + name + "\" defines both images as \"mirror\". At least one must be a filename.");
                else if (leftImageName == null)
                    imageFlip = ImageFlip.LeftMirrorsRight;
                else if (rightImageName == null)
                    imageFlip = ImageFlip.RightMirrorsLeft;

                Point? leftImageCenter = null;
                Point? rightImageCenter = null;

                // Create a new Behavior from given values.
                Behaviors.Add(new Behavior(0, name, chance, minDuration, maxDuration, speed, movement,
                    imageFlip, leftImageName, rightImageName, leftImageCenter, rightImageCenter,
                    startSpeech, endSpeech, nextBehaviorName));
                reader.ReadEndElement();
            }
            while (reader.Name != "behaviors");
            reader.ReadEndElement();
        }

        /// <summary>
        /// Returns the Speech in Speeches with the given name, provided it supports the given triggers.
        /// </summary>
        /// <param name="name">The name of the speech to search for.</param>
        /// <param name="requiredTriggers">The minimum set of triggers the speech must support.</param>
        /// <returns>Returns null is name is null or empty. An <see cref="T:System.ArgumentException"/> will be thrown if there is no
        /// speech with that name, or the speech with that name did not support the given triggers. Otherwise returns the desired speech.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">There is no speech with the given name, or the speech was this name did not
        /// support the required triggers.</exception>
        private Speech FindSpeechFromName(string name, SpeechTriggers requiredTriggers)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            foreach (Speech speech in Speeches)
                if (speech.Name == name)
                    if (speech.Trigger.HasFlag(requiredTriggers))
                        return speech;
                    else
                        throw new ArgumentException("Speech \"" + name + "\" was found, but does not supported the given triggers of \"" +
                            requiredTriggers + "\".", "requiredTriggers");

            throw new ArgumentException("Speech \"" + name + "\" not found.", "name");
        }

        /// <summary>
        /// Reads the effects section in the given <see cref="T:System.Xml.XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> which is positioned to read the effects subtree.</param>
        private void ReadEffectsFromXml(XmlReader reader)
        {
            reader.ReadStartElement("effects");
            while (reader.Name != "effects")
            {
                reader.ReadStartElement("effect");

                string name;
                reader.ReadStartElement("name");
                name = reader.ReadContentAsString();
                reader.ReadEndElement();

                int duration;
                reader.ReadStartElement("duration");
                duration = reader.ReadContentAsInt();
                reader.ReadEndElement();

                int minRepeatDuration;
                reader.ReadStartElement("minRepeatDuration");
                minRepeatDuration = reader.ReadContentAsInt();
                reader.ReadEndElement();

                int maxRepeatDuration;
                reader.ReadStartElement("maxRepeatDuration");
                maxRepeatDuration = reader.ReadContentAsInt();
                reader.ReadEndElement();

                if (maxRepeatDuration < minRepeatDuration)
                    throw new InvalidDataException("The minimum repeat duration is less than the maximum repeat duration.");

                bool followParent;
                reader.ReadStartElement("followParent");
                followParent = reader.ReadContentAsBoolean();
                reader.ReadEndElement();

                string leftImage;
                reader.ReadStartElement("leftImage");
                leftImage = reader.ReadContentAsString();
                reader.ReadEndElement();

                string rightImage;
                reader.ReadStartElement("rightImage");
                rightImage = reader.ReadContentAsString();
                reader.ReadEndElement();

                ContentAlignment alignmentToParentLeft;
                reader.ReadStartElement("leftImageAlignmentToParent");
                ParseEnumFromValue<ContentAlignment>(reader.ReadContentAsString(), out alignmentToParentLeft);
                reader.ReadEndElement();

                ContentAlignment alignmentAtOffsetLeft;
                reader.ReadStartElement("leftImageAlignmentAtOffset");
                ParseEnumFromValue<ContentAlignment>(reader.ReadContentAsString(), out alignmentAtOffsetLeft);
                reader.ReadEndElement();

                ContentAlignment alignmentToParentRight;
                reader.ReadStartElement("rightImageAlignmentToParent");
                ParseEnumFromValue<ContentAlignment>(reader.ReadContentAsString(), out alignmentToParentRight);
                reader.ReadEndElement();

                ContentAlignment alignmentAtOffsetRight;
                reader.ReadStartElement("rightImageAlignmentAtOffset");
                ParseEnumFromValue<ContentAlignment>(reader.ReadContentAsString(), out alignmentAtOffsetRight);
                reader.ReadEndElement();

                // Skip any future elements added to the effect list.
                if (reader.Name != "effect")
                    reader.ReadToNextSibling("effect");

                // Create a new EffectTemplate from given values.
                Effects.Add(new EffectTemplate(name, Path.Combine(Directory, leftImage), Path.Combine(Directory, rightImage),
                    alignmentToParentLeft, alignmentAtOffsetLeft, alignmentToParentRight, alignmentAtOffsetRight,
                    duration, minRepeatDuration, maxRepeatDuration, followParent));
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// Wraps <see cref="M:System.Enum.TryParse``1(System.String,``0@)"/> in order to throw a human readable
        /// <see cref="T:System.ArgumentException"/> if it fails.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type to which to convert <paramref name="value"/>.</typeparam>
        /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="result">When this method returns, contains an object of type <typeparamref name="TEnum"/> whose value is
        /// represented by <paramref name="value"/>. This parameter is passed uninitialized.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="value"/> was not a valid member of the enumeration. The exception
        /// contains a list of valid enumeration elements.</exception>
        private static void ParseEnumFromValue<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            if (!Enum.TryParse<TEnum>(value, out result))
            {
                string valueList = string.Join(", ", Enum.GetNames(typeof(TEnum)));
                throw new ArgumentException(value + " is not a valid " + typeof(TEnum).Name + "."
                    + Environment.NewLine + "Valid values are: " + valueList + ".");
            }
        }

        /// <summary>
        /// Load the ini configuration file in the given directory to create the template. This is intended to read the comma-delimited,
        /// quote-qualified lines from the original VB Desktop Ponies. Unsupported elements will be ignored. Where possible alternate
        /// implementations of similar features will be converted.
        /// </summary>
        /// <param name="directory">The directory which contains the ini file.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="directory"/> is null.</exception>
        /// <exception cref="System.IO.InvalidDataException">Thrown when the configuration file is badly formed. Check the inner exception
        /// for more information if this occurs.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Throw when the configuration file could not be found-or-when a filename
        /// given within the configuration file could not be found.</exception>
        /// <exception cref="System.IO.IOException">Thrown when an I/O error occurs when attempting to read the configuration file.
        /// </exception>
        private void LoadFromIni(string directory)
        {
            Argument.EnsureNotNull(directory, "directory");
            if (directory.Length == 0)
                throw new ArgumentException("directory must not be empty.", "directory");

            Directory = directory;
            if (Directory[Directory.Length - 1] == Path.DirectorySeparatorChar ||
                Directory[Directory.Length - 1] == Path.AltDirectorySeparatorChar)
                Directory = Directory.Remove(Directory.Length - 1);
            TemplateName = Directory.Substring(Directory.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            fileComment = null;

            // Scale specifies an override of the global scale factor. It is currently unused.
            double scale;
            // The list of categories that apply to this character, to be parsed later.
            List<string> tags = new List<string>();
            // Create our list of speeches, which will be built as the file is read.
            Speeches = new List<Speech>();
            // Create a lookup table of behavior group names, which will be built as the file is read.
            BehaviorGroupNames = new Dictionary<int, string>();
            // Behaviors are dependant on speeches, so must be processed after the whole file has been read.
            List<string[]> behaviorChunks = new List<string[]>();
            // Effects are dependant on behaviors, so must be processed after behaviors are created.
            List<string[]> effectChunks = new List<string[]>();

            // Open the file for reading.
            string path = Path.Combine(Directory, IniFilename);
            using (StreamReader stream = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                while (!stream.EndOfStream)
                {
                    string line = stream.ReadLine();

                    // Ignore blank lines.
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Skip over lines starting with a quote, these are comments.
                    if (line[0] == '\'')
                    {
                        fileComment += line + "\r\n";
                        continue;
                    }

                    // Get the chunks in this line, lines are comma delimited and qualified by a curly brace or double quote pairing.
                    char[] delimiters = new char[1] { ',' };
                    char[,] qualifiers = new char[2, 2] { { '{', '}' }, { '"', '"' } };
                    string[] chunks = line.SplitQualified(delimiters, qualifiers, StringSplitOptions.None);
                    for (int i = 0; i < chunks.Length; i++)
                        chunks[i] = chunks[i].Trim();

                    // The first chunk identifies the information in the line.
                    switch (chunks[0].ToLowerInvariant())
                    {
                        case "name":
                            Name = chunks[1];
                            break;
                        case "scale":
                            scale = double.Parse(chunks[1], CultureInfo.InvariantCulture);
                            break;
                        case "categories":
                            for (int i = 1; i < chunks.Length; i++)
                                tags.Add(chunks[i].ToLowerInvariant());
                            break;
                        case "speak":
                            ReadSpeechFromIni(chunks);
                            break;
                        case "behavior":
                            behaviorChunks.Add(chunks);
                            break;
                        case "effect":
                            effectChunks.Add(chunks);
                            break;
                        case "behaviorgroup":
                            BehaviorGroupNames.Add(int.Parse(chunks[1], CultureInfo.InvariantCulture), chunks[2]);
                            break;
                        default:
                            throw new Exception("Unexpected value for line identifier: " + chunks[0].ToLowerInvariant());
                    }
                }

            if (string.IsNullOrEmpty(Name))
                throw new InvalidDataException("Pony name not specified.");

            Behaviors = new List<Behavior>();
            foreach (string[] chunks in behaviorChunks)
                ReadBehaviorFromIni(chunks);

            Effects = new List<EffectTemplate>();
            foreach (string[] chunks in effectChunks)
                ReadEffectFromIni(chunks);

            // Generate lists of speech for use at random intervals and on mouseover.
            RandomSpeeches = new List<Speech>();
            MouseoverSpeeches = new List<Speech>();
            foreach (Speech speech in Speeches)
            {
                if (speech.Trigger.HasFlag(SpeechTriggers.Random))
                    RandomSpeeches.Add(speech);
                if (speech.Trigger.HasFlag(SpeechTriggers.Mouse))
                    MouseoverSpeeches.Add(speech);
            }

            // Define attributes.
            Role = Role.Other;
            Gender = Gender.Female;
            Race = PonyRace.NonPony;
            ReadAttributesFromIni(tags);

            // Resolve names into objects, and calculate the chance total.
            foreach (Behavior behavior in Behaviors)
            {
                behavior.ResolveNextBehavior(Behaviors);
                behavior.ResolveEffects(Effects);
                ChanceTotal += behavior.Chance;
            }
        }

        /// <summary>
        /// Reads the given array of strings as a speech.
        /// </summary>
        /// <param name="chunks">The chunks that define the speech.</param>
        private void ReadSpeechFromIni(string[] chunks)
        {
            // Possible forms of speaking options:
            // 1 entry - 1 line text
            // 4 or 5 entries -
            //     1 line name
            //     2 line text
            //     3 line sound filename -or- line sound filenames in a comma-delimited, curly-brace demarked list
            //     4 skip for normal use
            //     5 group number (optional) (has no effect)
            switch (chunks.Length - 1)
            {
                case 1:
                    Speeches.Add(new Speech("Unnamed", Name, chunks[1], SpeechTriggers.Any));
                    break;
                case 4:
                case 5:
                    // For now, ignoring the line sounds filename(s) and group number.
                    Speeches.Add(new Speech(chunks[1], Name, chunks[2],
                            bool.Parse(chunks[4].ToUpperInvariant()) ? SpeechTriggers.Script : SpeechTriggers.Any));
                    break;
                default:
                    throw new InvalidDataException("Encountered a speech definition that did not have the required number of variables.");
            }
        }

        /// <summary>
        /// Reads the given array of strings as an behavior.
        /// </summary>
        /// <param name="chunks">The chunks that define the behavior.</param>
        private void ReadBehaviorFromIni(string[] chunks)
        {
            Argument.EnsureNotNull(chunks, "chunks");
            if (chunks.Length < 9)
                throw new ArgumentException("There must be at least 9 chunks.", "chunks");

            string name = chunks[1];

            // Multiply by 100 to get an integer as most values are no more than 2dp.
            // The chance given in ini values is not used in the same manner, and there will be a different probability distribution.
            int chance = (int)(double.Parse(chunks[2], CultureInfo.InvariantCulture) * 100);

            // Multiply duration by 1000 to convert from seconds to milliseconds.
            int maxDuration = (int)(double.Parse(chunks[3], CultureInfo.InvariantCulture) * 1000);
            int minDuration = (int)(double.Parse(chunks[4], CultureInfo.InvariantCulture) * 1000);

            // Add 50% to the speed to get a roughly equivalent value.
            int speed = (int)(double.Parse(chunks[5], CultureInfo.InvariantCulture) * 1.5d);

            // Get image filenames and resolve image mirroring.
            string rightImageName = chunks[6] == "mirror" ? null : Path.Combine(Directory, chunks[6]);
            string leftImageName = chunks[7] == "mirror" ? null : Path.Combine(Directory, chunks[7]);
             //Force mirroring.
            rightImageName = null;
            ImageFlip imageFlip = ImageFlip.UseOriginal;
            if (leftImageName == null && rightImageName == null)
                throw new InvalidDataException(
                    "Behavior \"" + name + "\" defines both images as \"mirror\". At least one must be a filename.");
            else if (leftImageName == null)
                imageFlip = ImageFlip.LeftMirrorsRight;
            else if (rightImageName == null)
                imageFlip = ImageFlip.RightMirrorsLeft;

            Directions movement;
            #region Get movement.
            switch (chunks[8].ToLowerInvariant())
            {
                case "none":
                    movement = Directions.None;
                    break;
                case "horizontal_only":
                    movement = Directions.Horizontal;
                    break;
                case "vertical_only":
                    movement = Directions.Vertical;
                    break;
                case "horizontal_vertical":
                    movement = Directions.HorizontalVertical;
                    break;
                case "diagonal_only":
                    movement = Directions.Diagonal;
                    break;
                case "diagonal_horizontal":
                    movement = Directions.HorizontalDiagonal;
                    break;
                case "diagonal_vertical":
                    movement = Directions.VerticalDiagonal;
                    break;
                case "all":
                    movement = Directions.Any;
                    break;
                // Special cases which I do not handle as movement. For now, deny movement.
                case "mouseover":
                case "sleep":
                case "dragged":
                    movement = Directions.None;
                    break;
                default:
                    throw new InvalidDataException("Unexpected value for movement.");
            }
            #endregion

            Speech startSpeech = null;
            Speech endSpeech = null;
            string linkedBehavior = null;

            // thingToFollow is what the pony should follow.
            // If thingToFollow is not null, the moveTo are offsets relative to this thing where the pony should aim.
            // If thingToFollow is null, the moveTo are screen co-ordinates.
            // The followXBehavior defines manually selected behaviors to use whilst following.
            // autoSelectImageOnFollow overrides these manual selections, if set.
            // doNotRepeatAnimations prevents an animation from looping, if set.
            // group is the number of the behavior group to which the behavior belongs.
            int moveToX = 0;
            int moveToY = 0;
            string thingToFollow = null;
            string followStoppedBehavior = "";
            string followMovingBehavior = "";
            bool autoSelectImageOnFollow = true;
            bool doNotRepeatAnimations = false;
            int group = 0;

            // The center position of the images.
            Point? leftImageCenter = null;
            Point? rightImageCenter = null;

            // Expanded definition.
            if (chunks.Length > 9)
            {
                if (chunks.Length < 16)
                    throw new ArgumentException(
                        "In an expanded definition (10 or more chunks) there must be at least 16 chunks.", "chunks");

                linkedBehavior = chunks[9];
                if (linkedBehavior == "None")
                    linkedBehavior = null;

                string startSpeechName = chunks[10];
                string endSpeechName = chunks[11];
                #region Resolve speech names.
                if (!string.IsNullOrEmpty(startSpeechName))
                {
                    foreach (Speech speech in Speeches)
                        if (string.Equals(speech.Name, startSpeechName, StringComparison.OrdinalIgnoreCase))
                        {
                            startSpeech = speech;
                            break;
                        }
                    if (startSpeech == null)
                        throw new InvalidDataException(
                            "Unable to find a starting speech named in behavior \"" + name + "\". Wanted: " + startSpeechName);
                }
                if (!string.IsNullOrEmpty(endSpeechName))
                {
                    foreach (Speech speech in Speeches)
                        if (string.Equals(speech.Name, endSpeechName, StringComparison.OrdinalIgnoreCase))
                        {
                            endSpeech = speech;
                            break;
                        }
                    if (endSpeech == null)
                        throw new InvalidDataException(
                            "Unable to find a ending speech named in behavior \"" + name + "\". Wanted: " + endSpeechName);
                }
                #endregion

                bool skip = bool.Parse(chunks[12]);
                if (skip)
                    chance = 0;

                moveToX = int.Parse(chunks[13], CultureInfo.InvariantCulture);
                moveToY = int.Parse(chunks[14], CultureInfo.InvariantCulture);

                thingToFollow = chunks[15].ToLowerInvariant();

                // Optional extra attributes on the end of an expanded definition.
                if (chunks.Length > 16)
                    autoSelectImageOnFollow = bool.Parse(chunks[16]);
                if (chunks.Length > 17)
                    followStoppedBehavior = chunks[17];
                if (chunks.Length > 18)
                    followMovingBehavior = chunks[18];
                if (chunks.Length > 20)
                {
                    string[] coords = chunks[19].Split(',');
                    rightImageCenter = new Point(
                        int.Parse(coords[0], CultureInfo.InvariantCulture), int.Parse(coords[1], CultureInfo.InvariantCulture));
                    coords = chunks[20].Split(',');
                    leftImageCenter = new Point(
                        int.Parse(coords[0], CultureInfo.InvariantCulture), int.Parse(coords[1], CultureInfo.InvariantCulture));
                }
                if (chunks.Length > 21)
                    doNotRepeatAnimations = bool.Parse(chunks[21]);
                if (chunks.Length > 22)
                    group = int.Parse(chunks[22], CultureInfo.InvariantCulture);
            }

            if (Program.UseRelaxedParsing)
            {
                // Avoid durations of zero length.
                if (minDuration == 0)
                    minDuration = 1;
                if (maxDuration == 0)
                    maxDuration = 1;

                // Convert 0,0 to an actual null value.
                if (leftImageCenter == Point.Empty)
                    leftImageCenter = null;
                if (rightImageCenter == Point.Empty)
                    rightImageCenter = null;
            }

            Behaviors.Add(new Behavior(group, name, chance, minDuration, maxDuration, speed, movement,
                imageFlip, leftImageName, rightImageName, leftImageCenter, rightImageCenter,
                startSpeech, endSpeech, linkedBehavior));
            // Ignoring values: moveToX, moveToY, thingToFollow, autoSelectImageOnFollow, followStoppedBehavior, followMovingBehavior,
            // doNotRepeatAnimations.
        }

        /// <summary>
        /// Reads the given array of strings as an effect.
        /// </summary>
        /// <param name="chunks">The chunks that define the effect.</param>
        private void ReadEffectFromIni(string[] chunks)
        {
            string name = chunks[1];
            string behaviorName = chunks[2];

            // Link up effect to behavior.
            bool linked = false;
            foreach (Behavior behavior in Behaviors)
                if (behavior.Name == behaviorName)
                {
                    behavior.EffectNames.Add(name);
                    linked = true;
                    break;
                }
            if (!linked)
                throw new InvalidDataException("An effect tried to link to a non-existent behavior." +
                                Environment.NewLine + "The effect " + name + " tried to link to " + behaviorName + " which didn't exist.");

            string leftImage = Path.Combine(Directory, chunks[4]);
            string rightImage = Path.Combine(Directory, chunks[3]);
            
            // Multiply duration by 1000 to convert from seconds to milliseconds.
            int duration = (int)(double.Parse(chunks[5], CultureInfo.InvariantCulture) * 1000);
            int minRepeatDuration = (int)(double.Parse(chunks[6], CultureInfo.InvariantCulture) * 1000);
            int maxRepeatDuration = minRepeatDuration;

            ContentAlignment directionRight = ContentAlignment.MiddleCenter;
            ContentAlignment centeringRight = ContentAlignment.MiddleCenter;
            ContentAlignment directionLeft = ContentAlignment.MiddleCenter;
            ContentAlignment centeringLeft = ContentAlignment.MiddleCenter;
            #region Determine content alignment.
            for (int i = 7; i <= 10; i++)
            {
                ContentAlignment contentAlignment;
                switch (chunks[i].ToLowerInvariant())
                {
                    case "top":
                        contentAlignment = ContentAlignment.TopCenter;
                        break;
                    case "bottom":
                        contentAlignment = ContentAlignment.BottomCenter;
                        break;
                    case "left":
                        contentAlignment = ContentAlignment.MiddleLeft;
                        break;
                    case "right":
                        contentAlignment = ContentAlignment.MiddleRight;
                        break;
                    case "bottom_right":
                        contentAlignment = ContentAlignment.BottomRight;
                        break;
                    case "bottom_left":
                        contentAlignment = ContentAlignment.BottomLeft;
                        break;
                    case "top_right":
                        contentAlignment = ContentAlignment.TopRight;
                        break;
                    case "top_left":
                        contentAlignment = ContentAlignment.TopLeft;
                        break;
                    case "center":
                    // Just going to use the center for these values for now.
                    case "any":
                    case "any-not_center":
                        contentAlignment = ContentAlignment.MiddleCenter;
                        break;
                    default:
                        throw new InvalidDataException("Invalid placement direction or centering for effect.");
                }
                if (i == 7)
                    directionRight = contentAlignment;
                else if (i == 8)
                    centeringRight = contentAlignment;
                else if (i == 9)
                    directionLeft = contentAlignment;
                else if (i == 10)
                    centeringLeft = contentAlignment;
            }
            #endregion

            bool follow = bool.Parse(chunks[11]);

            Effects.Add(new EffectTemplate(name, leftImage, rightImage,
                directionLeft, centeringLeft, directionRight, centeringRight,
                duration, minRepeatDuration, maxRepeatDuration, follow));
        }

        /// <summary>
        /// Reads the given list of attributes and finds the matching enum values.
        /// </summary>
        /// <param name="attributes">The list of attributes to be converted.</param>
        private void ReadAttributesFromIni(IEnumerable<string> attributes)
        {
            int count = 0;
            foreach (string attribute in attributes)
            {
                count++;
                switch (attribute)
                {
                    case "main ponies":
                        Role = Role.Main;
                        break;
                    case "supporting ponies":
                        Role = Role.Support;
                        break;
                    case "stallions":
                    case "colts":
                        Gender = Gender.Male;
                        break;
                    case "mares":
                    case "fillies":
                        Gender = Gender.Female;
                        break;
                    case "alicorns":
                        Race = PonyRace.Alicorn;
                        break;
                    case "pegasi":
                        Race = PonyRace.Pegasus;
                        break;
                    case "unicorns":
                        Race = PonyRace.Unicorn;
                        break;
                    case "earth ponies":
                        Race = PonyRace.Earth;
                        break;
                    case "pets":
                    case "non-ponies":
                        Race = PonyRace.NonPony;
                        break;
                    // Valid tags that I'm ignoring.
                    case "alternate art":
                        break;
                    default:
                        throw new ArgumentException("Unknown category " + attribute, "attributes");
                }
            }
            //if (count != 3 && !Program.UseRelaxedParsing)
            //    throw new InvalidDataException("Expected 3 attributes, but found " + count);
        }

        /// <summary>
        /// Save the template to the same configuration file we loaded from.
        /// </summary>
        public void SaveToXml()
        {
            // Set up the writer if we have not done so.
            if (writerSettings == null)
                writerSettings = new XmlWriterSettings() { CloseOutput = true, Indent = true };

            XmlWriter writer = XmlWriter.Create(Path.Combine(Directory, XmlFilename), writerSettings);
            try
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("pony");
                writer.WriteAttributeString("version", Version.ToString(2));

                if (fileComment != null)
                {
                    writer.WriteStartElement("info");
                    writer.WriteString(fileComment);
                    writer.WriteEndElement();
                }

                writer.WriteStartElement("name");
                writer.WriteString(Name);
                writer.WriteEndElement();

                writer.WriteStartElement("role");
                writer.WriteString(Role.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("gender");
                writer.WriteString(Gender.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("race");
                writer.WriteString(Race.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("dialogue");
                foreach (Speech speech in Speeches)
                {
                    writer.WriteStartElement("speech");

                    writer.WriteStartElement("name");
                    writer.WriteString(speech.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("line");
                    writer.WriteString(speech.Line);
                    writer.WriteEndElement();

                    writer.WriteStartElement("trigger");
                    writer.WriteString(speech.Trigger.ToString());
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                if (Speeches.Count == 0)
                    writer.WriteString("");
                writer.WriteEndElement();

                writer.WriteStartElement("behaviors");
                foreach (Behavior behavior in Behaviors)
                {
                    writer.WriteStartElement("behavior");

                    writer.WriteStartElement("name");
                    writer.WriteString(behavior.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("chance");
                    writer.WriteValue(behavior.Chance);
                    writer.WriteEndElement();

                    writer.WriteStartElement("minDuration");
                    writer.WriteValue(behavior.MinDuration);
                    writer.WriteEndElement();

                    writer.WriteStartElement("maxDuration");
                    writer.WriteValue(behavior.MaxDuration);
                    writer.WriteEndElement();

                    writer.WriteStartElement("speed");
                    writer.WriteValue(behavior.Speed);
                    writer.WriteEndElement();

                    writer.WriteStartElement("movement");
                    writer.WriteString(Enum.GetName(typeof(Directions), behavior.MovementAllowed));
                    writer.WriteEndElement();

                    writer.WriteStartElement("leftImage");
                    writer.WriteString(
                        behavior.BehaviorImageFlip == ImageFlip.LeftMirrorsRight ? "mirror" : Path.GetFileName(behavior.LeftImageName));
                    writer.WriteEndElement();

                    writer.WriteStartElement("rightImage");
                    writer.WriteString(
                        behavior.BehaviorImageFlip == ImageFlip.RightMirrorsLeft ? "mirror" : Path.GetFileName(behavior.RightImageName));
                    writer.WriteEndElement();

                    writer.WriteStartElement("startSpeech");
                    writer.WriteString(behavior.StartSpeech == null ? "" : behavior.StartSpeech.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("endSpeech");
                    writer.WriteString(behavior.EndSpeech == null ? "" : behavior.EndSpeech.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("nextBehavior");
                    writer.WriteString(behavior.NextBehavior == null ? "" : behavior.NextBehavior.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("effects");
                    foreach (EffectTemplate effect in behavior.Effects)
                    {
                        writer.WriteStartElement("effect");
                        writer.WriteString(effect.Name);
                        writer.WriteEndElement();
                    }
                    if (behavior.Effects.Count == 0)
                        writer.WriteString("");
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteStartElement("effects");
                foreach (EffectTemplate effect in Effects)
                {
                    writer.WriteStartElement("effect");

                    writer.WriteStartElement("name");
                    writer.WriteString(effect.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("duration");
                    writer.WriteValue(effect.Duration);
                    writer.WriteEndElement();

                    writer.WriteStartElement("minRepeatDuration");
                    writer.WriteValue(effect.MinRepeatDuration);
                    writer.WriteEndElement();

                    writer.WriteStartElement("maxRepeatDuration");
                    writer.WriteValue(effect.MaxRepeatDuration);
                    writer.WriteEndElement();

                    writer.WriteStartElement("followParent");
                    writer.WriteValue(effect.FollowParent);
                    writer.WriteEndElement();

                    writer.WriteStartElement("leftImage");
                    writer.WriteString(Path.GetFileName(effect.LeftImageName));
                    writer.WriteEndElement();

                    writer.WriteStartElement("rightImage");
                    writer.WriteString(Path.GetFileName(effect.RightImageName));
                    writer.WriteEndElement();

                    writer.WriteStartElement("leftImageAlignmentToParent");
                    writer.WriteString(Enum.GetName(typeof(ContentAlignment), effect.AlignmentToParentLeft));
                    writer.WriteEndElement();

                    writer.WriteStartElement("leftImageAlignmentAtOffset");
                    writer.WriteString(Enum.GetName(typeof(ContentAlignment), effect.AlignmentAtOffsetLeft));
                    writer.WriteEndElement();

                    writer.WriteStartElement("rightImageAlignmentToParent");
                    writer.WriteString(Enum.GetName(typeof(ContentAlignment), effect.AlignmentToParentRight));
                    writer.WriteEndElement();

                    writer.WriteStartElement("rightImageAlignmentAtOffset");
                    writer.WriteString(Enum.GetName(typeof(ContentAlignment), effect.AlignmentAtOffsetRight));
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                if (Effects.Count == 0)
                    writer.WriteString("");
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            finally
            {
                writer.Close();
            }
        }
    }

    /// <summary>
    /// Represents lines a pony can speak, and the allowable triggers of the line.
    /// </summary>
    public class Speech
    {
        /// <summary>
        /// Gets the name of the speech.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the content of the line to be spoken.
        /// </summary>
        public string Line { get; private set; }
        /// <summary>
        /// Gets the full text that is intended to be displayed for this speech inside a speech bubble.
        /// </summary>
        public string DisplayText { get; private set; }
        /// <summary>
        /// Gets the set of possible triggers for the line.
        /// </summary>
        public SpeechTriggers Trigger { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.Speech"/> class.
        /// </summary>
        /// <param name="name">The name of the speech, used as an identifier.</param>
        /// <param name="characterName">The name of the character who says this speech.</param>
        /// <param name="line">The string containing the line to speak.</param>
        /// <param name="trigger">The allowable triggers that can be used to make this speech appear.</param>
        public Speech(string name, string characterName, string line, SpeechTriggers trigger)
        {
            Name = name;
            Line = line;
            DisplayText = characterName + ": " + Line;
            Trigger = trigger;
        }
    }

    /// <summary>
    /// Represents a behavior a pony can assume. Specifies the images to use and the movements to perform.
    /// </summary>
    public class Behavior
    {
        /// <summary>
        /// Gets the group number of the behavior. Only behaviors in this group or group 0 may follow on from this behavior unless
        /// overridden by <see cref="P:CsDesktopPonies.DesktopPonies.Behavior.NextBehavior"/>.
        /// </summary>
        public int Group { get; private set; }
        /// <summary>
        /// Gets the name of the behavior.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the chance the behavior occurs. This is used as a ratio against other chance values when determining the behavior to use
        /// at random.
        /// </summary>
        public int Chance { get; private set; }
        /// <summary>
        /// Gets the minimum duration, in milliseconds, that the behavior will take place for.
        /// </summary>
        public int MinDuration { get; private set; }
        /// <summary>
        /// Gets the maximum duration, in milliseconds, that the behavior will take place for.
        /// </summary>
        public int MaxDuration { get; private set; }
        /// <summary>
        /// Gets the speed the pony will travel, in pixels per tick, in the horizontal and vertical directions.
        /// </summary>
        public int Speed { get; private set; }
        /// <summary>
        /// Gets the set of directions the pony is allowed to move in.
        /// </summary>
        public Directions MovementAllowed { get; private set; }
        /// <summary>
        /// Gets a value indicating how to use the leftImage and rightImage to render for a pony facing left or right.
        /// </summary>
        public ImageFlip BehaviorImageFlip { get; private set; }
        /// <summary>
        /// Gets the filename for the image used when the pony is facing left.
        /// </summary>
        public string LeftImageName { get; private set; }
        /// <summary>
        /// Gets the desired relative center point when drawing the left facing image. If null, use the actual center.
        /// </summary>
        public Point? LeftImageCenter { get; private set; }
        /// <summary>
        /// Gets the size of the rectangle occupied by drawing the image when the pony is facing left.
        /// </summary>
        public Size LeftImageDrawSize { get; private set; }
        /// <summary>
        /// Gets the filename for the image used when the pony is facing right.
        /// </summary>
        public string RightImageName { get; private set; }
        /// <summary>
        /// Gets the desired relative center point when drawing the right facing image. If null, use the actual center.
        /// </summary>
        public Point? RightImageCenter { get; private set; }
        /// <summary>
        /// Gets the size of the rectangle occupied by drawing the image when the pony is facing right.
        /// </summary>
        public Size RightImageDrawSize { get; private set; }
        /// <summary>
        /// Gets the line to use at the start of the behavior.
        /// </summary>
        public Speech StartSpeech { get; private set; }
        /// <summary>
        /// Gets the line to use at the end of the behavior.
        /// </summary>
        public Speech EndSpeech { get; private set; }
        /// <summary>
        /// Gets the next behavior to use in a series of linked behaviors. If not set, a random behavior will be selected from behaviors in
        /// the same group, or group 0. This selection is determined by the chance value.
        /// </summary>
        public Behavior NextBehavior { get; private set; }
        /// <summary>
        /// The name of the next behavior to take, this needs to be resolved into an behavior object.
        /// </summary>
        private string nextBehaviorName;
        /// <summary>
        /// Gets the set of effects to be applied during this behavior.
        /// </summary>
        public IList<EffectTemplate> Effects { get; private set; }
        /// <summary>
        /// Gets the names of the effects to be applied, these need to be resolved into effect objects.
        /// </summary>
        public ICollection<string> EffectNames { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.Behavior"/> class.
        /// </summary>
        /// <param name="group">The group number to which this behavior belongs. Only behaviors the same group or group 0 may become the
        /// next behavior.</param>
        /// <param name="name">The name of the behavior.</param>
        /// <param name="chance">The chance the behavior occurs. This is used as a ratio against other chance values.</param>
        /// <param name="minDuration">The minimum duration the behavior will take place for, in milliseconds.
        /// Must be less than or equal to maxDuration.</param>
        /// <param name="maxDuration">The maximum duration the behavior will take place for, in milliseconds.
        /// Must be greater than or equal to minDuration.</param>
        /// <param name="speed">The speed the pony will travel in pixels per tick.
        /// This applies to each of the horizontal and vertical directions.</param>
        /// <param name="movementAllowed">The set of directions the pony is allowed to move in.</param>
        /// <param name="behaviorImageFlip">Indicates how the images for this behavior are to be drawn depending on facing.</param>
        /// <param name="leftImageName">The filename of the image to be used when the pony is facing left.</param>
        /// <param name="rightImageName">The filename of the image to be used when the pony is facing right.</param>
        /// <param name="leftImageCenter">The custom point that specifies the center of the left image.
        /// If null, indicates to use the actual center.</param>
        /// <param name="rightImageCenter">The custom point that specifies the center of the right image.
        /// If null, indicates to use the actual center.</param>
        /// <param name="startSpeech">The Speech to be said at the start of the behavior.</param>
        /// <param name="endSpeech">The Speech to be said at the end of the behavior.</param>
        /// <param name="nextBehaviorName">The name of the behavior to take immediately after this.
        /// Use ResolveNextBehavior(behaviors) to find the Behavior object in the given list.</param>
        public Behavior(int group, string name, int chance, int minDuration, int maxDuration, int speed, Directions movementAllowed,
            ImageFlip behaviorImageFlip, string leftImageName, string rightImageName, Point? leftImageCenter, Point? rightImageCenter,
            Speech startSpeech, Speech endSpeech, string nextBehaviorName)
        {
            if (maxDuration < minDuration)
                if (Program.UseRelaxedParsing)
                {
                    int temp = maxDuration;
                    maxDuration = minDuration;
                    minDuration = temp;
                }
                else
                {
                    throw new ArgumentException("The minimum duration is less than the maximum duration.");
                }
            if (minDuration <= 0)
                throw new ArgumentException("The minimum duration must be greater than zero.");

            if (behaviorImageFlip != ImageFlip.LeftMirrorsRight && !File.Exists(leftImageName))
                throw new FileNotFoundException(leftImageName, leftImageName);
            if (behaviorImageFlip != ImageFlip.RightMirrorsLeft && !File.Exists(rightImageName))
                throw new FileNotFoundException(rightImageName, rightImageName);

            Group = group;
            Name = name;
            MinDuration = minDuration;
            MaxDuration = maxDuration;
            Speed = speed;
            Chance = chance;
            MovementAllowed = movementAllowed;
            BehaviorImageFlip = behaviorImageFlip;

            if (BehaviorImageFlip != ImageFlip.LeftMirrorsRight)
                LeftImageName = leftImageName;
            else
                LeftImageName = rightImageName;

            if (BehaviorImageFlip != ImageFlip.RightMirrorsLeft)
                RightImageName = rightImageName;
            else
                RightImageName = leftImageName;

            LeftImageDrawSize = ImageSize.GetSize(LeftImageName);
            RightImageDrawSize = ImageSize.GetSize(RightImageName);
            LeftImageCenter = leftImageCenter;
            RightImageCenter = rightImageCenter;
            StartSpeech = startSpeech;
            EndSpeech = endSpeech;
            this.nextBehaviorName = nextBehaviorName;
            Effects = new List<EffectTemplate>();
            EffectNames = new List<string>();
        }

        /// <summary>
        /// If an behavior has a next behavior defined, this function will attempt to find the matching behavior from the given list.
        /// If successful, the behaviors will be linked. If not, an <see cref="T:System.ArgumentException"/> is thrown.
        /// </summary>
        /// <param name="behaviors">The list of behaviors to be searched for the linked behavior.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="behaviors"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">There was no behavior with the name requested as the next behavior.</exception>
        public void ResolveNextBehavior(IEnumerable<Behavior> behaviors)
        {
            Argument.EnsureNotNull(behaviors, "behaviors");

            if (!string.IsNullOrEmpty(nextBehaviorName))
            {
                foreach (Behavior behavior in behaviors)
                    if (behavior.Name == nextBehaviorName)
                    {
                        NextBehavior = behavior;
                        nextBehaviorName = null;
                        return;
                    }
                throw new ArgumentException("A behavior tried to link to a non-existent behavior." +
                Environment.NewLine + "The behavior " + Name + " tried to link to " + nextBehaviorName + " which didn't exist.");
            }
        }
 
        /// <summary>
        /// If an behavior has any effects defined, this function will attempt to find the matching effects from the given list.
        /// If successful, the effects will be linked. If not, an <see cref="T:System.ArgumentException"/> is thrown.
        /// </summary>
        /// <param name="effects">The list of effects to be searched for the linked behavior.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="effects"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">There was no effect with the name requested by a behavior.</exception>
        public void ResolveEffects(IEnumerable<EffectTemplate> effects)
        {
            Argument.EnsureNotNull(effects, "effects");

            if (EffectNames != null && EffectNames.Count != 0)
            {
                List<EffectTemplate> resolvedEffects = new List<EffectTemplate>();
                foreach (string effectName in EffectNames)
                {
                    bool effectFound = false;
                    foreach (EffectTemplate effect in effects)
                        if (effect.Name == effectName)
                        {
                            resolvedEffects.Add(effect);
                            effectFound = true;
                            break;
                        }
                    if (!effectFound)
                        throw new ArgumentException("An behavior tried to link to a non-existent effect." +
                            Environment.NewLine + "The behavior " + Name + " tried to link to " + effectName + " which didn't exist.");
                }
                resolvedEffects.TrimExcess();
                Effects = resolvedEffects;
            }
        }
    }

    /// <summary>
    /// Realizes a single instance of an <see cref="T:CsDesktopPonies.DesktopPonies.EffectTemplate"/>.
    /// </summary>
    public sealed class EffectInstance : ISprite
    {
        /// <summary>
        /// Gets or sets a value indicating whether addition to the sprite collection is pending.
        /// </summary>
        public bool AdditionPending { get; set; }
        /// <summary>
        /// Gets the template on which this instance is based.
        /// </summary>
        public EffectTemplate Template { get; private set; }
        /// <summary>
        /// Gets the PonyInstance to which this EffectInstance belongs.
        /// </summary>
        public PonyInstance Parent { get; private set; }
        /// <summary>
        /// Gets the behavior that triggered this effect. Effects should only be repeated during this behavior.
        /// </summary>
        public Behavior Trigger { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this effect has finished, and should stop being displayed.
        /// A repeating effect may need to be kept around to trigger a repeat at a later time however.
        /// </summary>
        public bool HasEnded { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether an effect should be repeated in the future.
        /// False if the effect does not repeat, or has already repeated.
        /// </summary>
        public bool RepeatPending { get; set; }
        /// <summary>
        /// Gets or sets time this effect was started.
        /// </summary>
        private TimeSpan StartTime { get; set; }
        /// <summary>
        /// Gets or sets the current time for this instance, from which time-based operations should be referenced.
        /// </summary>
        private TimeSpan CurrentTime { get; set; }
        /// <summary>
        /// Gets the time a repeating effect should reoccur.
        /// </summary>
        public TimeSpan EffectRepeatTime { get; private set; }
        /// <summary>
        /// The duration this effect should last for.
        /// </summary>
        private readonly int duration;
        /// <summary>
        /// Indicates if the image has it's X and Y position defined.
        /// </summary>
        private bool positionSet;
        /// <summary>
        /// The x co-ordinate of the initial draw position, used to draw the image again if FollowParent is false.
        /// </summary>
        private int x;
        /// <summary>
        /// The y co-ordinate of the initial draw position, used to draw the image again if FollowParent is false.
        /// </summary>
        private int y;
        /// <summary>
        /// If true indicates the image should be drawn facing to the left, otherwise to the right.
        /// </summary>
        private bool facingLeft;
        /// <summary>
        /// The size of the instance to be drawn.
        /// </summary>
        private Size drawSize;

        /// <summary>
        /// Initializes a new instance of the EffectInstance class belonging to the given parent, with the given time as the zero point.
        /// </summary>
        /// <param name="template">The template on which to model this effect.</param>
        /// <param name="parent">The parent pony to which this effect belongs and should be attached to.</param>
        /// <param name="trigger">The behavior that triggered this effect. Effects will only be repeated during this behavior.</param>
        /// <param name="elapsedTime">The current elapsed time to be used as the effective zero time for animation.</param>
        public EffectInstance(EffectTemplate template, PonyInstance parent, Behavior trigger, TimeSpan elapsedTime)
        {
            Argument.EnsureNotNull(parent, "parent");

            AdditionPending = true;
            Template = template;
            Parent = parent;
            Trigger = trigger;
            HasEnded = false;
            RepeatPending = Template.MaxRepeatDuration > 0;
            StartTime = elapsedTime;
            EffectRepeatTime = StartTime + TimeSpan.FromMilliseconds(Rng.Next(Template.MinRepeatDuration, Template.MaxRepeatDuration));

            // If specified, use the template's duration, else end the effect at the end of the behavior.
            if (Template.Duration != 0)
                duration = Template.Duration;
            else
                duration = (int)(parent.BehaviorEndTime - StartTime).TotalMilliseconds;
            positionSet = false;
            Update(StartTime);
        }

        /// <summary>
        /// Given a new time, updates the effect.
        /// </summary>
        /// <param name="updateTime">The current elapsed time of the window, to time effects on.</param>
        public void Update(TimeSpan updateTime)
        {
            CurrentTime = updateTime;

            // Determine if the instance should no longer be drawn.
            if ((CurrentTime - StartTime).TotalMilliseconds > duration)
            {
                HasEnded = true;
                return;
            }

            // Update position if we are following the parent, or if we are drawing for the first time.
            if (Template.FollowParent || !positionSet)
            {
                drawSize = Parent.MovingLeft ? Template.LeftDrawSize : Template.RightDrawSize;
                facingLeft = Parent.MovingLeft;
                positionSet = true;

                // Move to parent position.
                x = Parent.DrawRectangle.X;
                y = Parent.DrawRectangle.Y;

                // Adjust position so effect is offset correctly.
                AdjustRelativeToParent();
                AdjustRelativeToSelf();
            }
        }

        /// <summary>
        /// Moves the image relative to its parent, so that the given location on the parent image will act as the offset point.
        /// </summary>
        private void AdjustRelativeToParent()
        {
            // Move x position relative to the parent.
            switch (Parent.MovingLeft ? Template.AlignmentToParentLeft : Template.AlignmentToParentRight)
            {
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    x += Parent.DrawRectangle.Width / 2;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    x += Parent.DrawRectangle.Width;
                    break;
            }

            // Move y position relative to the parent.
            switch (Parent.MovingLeft ? Template.AlignmentToParentLeft : Template.AlignmentToParentRight)
            {
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    y += Parent.DrawRectangle.Height / 2;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    y += Parent.DrawRectangle.Height;
                    break;
            }
        }

        /// <summary>
        /// Moves the image relative to itself, so that the given location on the image coincides with the offset position.
        /// </summary>
        private void AdjustRelativeToSelf()
        {
            // Adjust x position relative to self.
            switch (Parent.MovingLeft ? Template.AlignmentAtOffsetLeft : Template.AlignmentAtOffsetRight)
            {
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    x -= drawSize.Width / 2;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    x -= drawSize.Width;
                    break;
            }

            // Adjust y position relative to self.
            switch (Parent.MovingLeft ? Template.AlignmentAtOffsetLeft : Template.AlignmentAtOffsetRight)
            {
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    y -= drawSize.Width / 2;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    y -= drawSize.Width;
                    break;
            }
        }

        /// <summary>
        /// Gets the path to the image file that should be used to display the effect.
        /// </summary>
        public string ImagePath
        {
            get { return facingLeft ? Template.LeftImageName : Template.RightImageName; }
        }
        /// <summary>
        /// Gets a value indicating whether the image should be flipped horizontally from its original orientation.
        /// </summary>
        public bool FlipImage
        {
            get { return false; }
        }
        /// <summary>
        /// Gets the region the effect currently occupies.
        /// </summary>
        public Rectangle Region
        {
            get { return new Rectangle(x, y, drawSize.Width, drawSize.Height); }
        }
        /// <summary>
        /// Gets the instant in time that represents the current state of the effect. This tracks the most recent update time.
        /// </summary>
        TimeSpan ISprite.CurrentTime
        {
            get { return CurrentTime - StartTime; }
        }
        /// <summary>
        /// Starts the effect using the given time as a zero point.
        /// </summary>
        /// <param name="startTime">The time that will be used as a zero point against the time given in future updates.</param>
        public void Start(TimeSpan startTime)
        {
        }
    }

    /// <summary>
    /// Represents an effect that behaviors can activate for a pony, which can be realized be an
    /// <see cref="T:CsDesktopPonies.DesktopPonies.EffectInstance"/>.
    /// </summary>
    public class EffectTemplate
    {
        /// <summary>
        /// Gets the name of the effect.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the filename for the image used when the parent is facing left.
        /// </summary>
        public string LeftImageName { get; private set; }
        /// <summary>
        /// Gets the size of the rectangle occupied by the left-facing image of this behavior.
        /// </summary>
        public Size LeftDrawSize { get; private set; }
        /// <summary>
        /// Gets the filename for the image used when the parent is facing right.
        /// </summary>
        public string RightImageName { get; private set; }
        /// <summary>
        /// Gets the size of the rectangle occupied by the right-facing image of this behavior.
        /// </summary>
        public Size RightDrawSize { get; private set; }
        /// <summary>
        /// Gets the relative position of the image to the parent when facing left.
        /// </summary>
        public ContentAlignment AlignmentToParentLeft { get; private set; }
        /// <summary>
        /// Gets a value specifying which part of the image is to be drawn from the offset given by
        /// <see cref="P:CsDesktopPonies.DesktopPonies.EffectTemplate.AlignmentToParentLeft"/>.
        /// </summary>
        public ContentAlignment AlignmentAtOffsetLeft { get; private set; }
        /// <summary>
        /// Gets the relative position of the image to the parent when facing right.
        /// </summary>
        public ContentAlignment AlignmentToParentRight { get; private set; }
        /// <summary>
        /// Gets a value specifying which part of the image is to be drawn from the offset given by
        /// <see cref="P:CsDesktopPonies.DesktopPonies.EffectTemplate.AlignmentToParentRight"/>.
        /// </summary>
        public ContentAlignment AlignmentAtOffsetRight { get; private set; }
        /// <summary>
        /// Gets the duration the effect is to remain visible, in milliseconds.
        /// </summary>
        public int Duration { get; private set; }
        /// <summary>
        /// Gets the minimum waiting time, in milliseconds, before repeating this effect.
        /// </summary>
        public int MinRepeatDuration { get; private set; }
        /// <summary>
        /// Gets the maximum waiting time, in milliseconds, before repeating this effect. Values less than or equal to zero indicate a
        /// non-repeating effect.
        /// </summary>
        public int MaxRepeatDuration { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the effects will continue to follow their parent after being deployed, or else remain
        /// stationary.
        /// </summary>
        public bool FollowParent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.EffectTemplate"/> class.
        /// </summary>
        /// <param name="name">The name of the effect.</param>
        /// <param name="leftImageName">The filename of the image to be used when the effect is facing left.</param>
        /// <param name="rightImageName">The filename of the image to be used when the effect is facing right.</param>
        /// <param name="alignmentToParentLeft">The position of the effect, relative to the parent image, when the effect is facing left.
        /// </param>
        /// <param name="alignmentAtOffsetLeft">The part of the image that is centered on the point given by the parental alignment, when
        /// the effect is facing left.</param>
        /// <param name="alignmentToParentRight">The position of the effect, relative to the parent image, when the effect is facing right.
        /// </param>
        /// <param name="alignmentAtOffsetRight">The part of the image that is centered on the point given by the parental alignment, when
        /// the effect is facing right.</param>
        /// <param name="duration">The duration the effect will take place for, in milliseconds. A value of zero indicates the effect will
        /// inherit the duration of the behavior calling it.</param>
        /// <param name="minRepeatDuration">The minimum time to wait, in milliseconds, before repeating this effect. A value of zero for
        /// <paramref name="maxRepeatDuration"/> indicates the effect should not repeat.</param>
        /// <param name="maxRepeatDuration">The maximum time to wait, in milliseconds, before repeating this effect. A value of zero
        /// indicates the effect should not repeat.</param>
        /// <param name="follow">If true the effect will follow the position of its parent, otherwise it will remain statically positioned.
        /// </param>
        public EffectTemplate(string name, string leftImageName, string rightImageName,
            ContentAlignment alignmentToParentLeft, ContentAlignment alignmentAtOffsetLeft,
            ContentAlignment alignmentToParentRight, ContentAlignment alignmentAtOffsetRight,
            int duration, int minRepeatDuration, int maxRepeatDuration, bool follow)
        {
            if (maxRepeatDuration < minRepeatDuration)
                throw new ArgumentException("The minimum repeat duration is less than the maximum repeat duration.");
            // A duration of zero indicates that the effect should last as long as the behavior it is linked to.

            Name = name;
            LeftImageName = leftImageName;
            RightImageName = rightImageName;
            if (!File.Exists(leftImageName))
                throw new FileNotFoundException(leftImageName, leftImageName);
            if (!File.Exists(rightImageName))
                throw new FileNotFoundException(rightImageName, rightImageName);

            LeftDrawSize = ImageSize.GetSize(LeftImageName);
            RightDrawSize = ImageSize.GetSize(RightImageName);
            AlignmentToParentLeft = alignmentToParentLeft;
            AlignmentAtOffsetLeft = alignmentAtOffsetLeft;
            AlignmentToParentRight = alignmentToParentRight;
            AlignmentAtOffsetRight = alignmentAtOffsetRight;
            Duration = duration;
            MinRepeatDuration = minRepeatDuration;
            MaxRepeatDuration = maxRepeatDuration;
            FollowParent = follow;
        }
    }
}