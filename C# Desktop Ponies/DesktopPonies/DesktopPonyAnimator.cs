namespace CSDesktopPonies.DesktopPonies
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using CSDesktopPonies.Collections;
    using CSDesktopPonies.SpriteManagement;

    /// <summary>
    /// Animates a collection of ponies on the desktop.
    /// </summary>
    public class DesktopPonyAnimator : AnimationLoopBase
    {
        #region InstanceCountChangedEventArgs class
        /// <summary>
        /// Provides data for the <see cref="E:CSDesktopPonies.DesktopPonies.DesktopPonyAnimator.InstanceCountChanged"/> event.
        /// </summary>
        public class InstanceCountChangedEventArgs : EventArgs
        {
            /// <summary>
            /// Gets the template of the affected ponies.
            /// </summary>
            public PonyTemplate Template { get; private set; }
            /// <summary>
            /// Gets the signed change in the number of ponies.
            /// </summary>
            public int Change { get; private set; }

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="T:CSDesktopPonies.DesktopPonies.DesktopPonyAnimator.InstanceCountChangedEventArgs"/> class.
            /// </summary>
            /// <param name="template">The template of the affected ponies.</param>
            /// <param name="change">The signed number of ponies that were added or removed.</param>
            public InstanceCountChangedEventArgs(PonyTemplate template, int change)
            {
                Template = template;
                Change = change;
            }
        }
        #endregion

        /// <summary>
        /// Occurs when the number of pony instances being animated changes.
        /// </summary>
        public event EventHandler<InstanceCountChangedEventArgs> InstanceCountChanged;
        /// <summary>
        /// Occurs when the animator has been closed.
        /// </summary>
        public event EventHandler AnimatorClosed;
        /// <summary>
        /// Occurs when the program has been requested to exit by user input to the animator.
        /// </summary>
        public event EventHandler ProgramExitRequested;

        /// <summary>
        /// Defines a comparison for Z-order sorting. This is done based on the location of the bottom part of the image.
        /// </summary>
        private Comparison<ISprite> zOrder = new Comparison<ISprite>((a, b) => a.Region.Bottom - b.Region.Bottom);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.DesktopPonies.DesktopPonyAnimator"/> class.
        /// </summary>
        /// <param name="spriteViewer">The <see cref="T:CSDesktopPonies.SpriteManagement.ISpriteCollectionView"/> that will display the
        /// ponies.</param>
        /// <param name="spriteCollection">The collection of ponies to animate.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="spriteViewer"/> is null.</exception>
        public DesktopPonyAnimator(ISpriteCollectionView spriteViewer, IEnumerable<ISprite> spriteCollection)
            : base(spriteViewer, spriteCollection)
        {
            MaximumFramesPerSecond = 30;
            Viewer.WindowTitle = "C# Desktop Ponies";
            Viewer.WindowIconFilePath = "Twilight.ico";
            Viewer.InterfaceClosed += (sender, e) => AnimatorClosed.Raise(this);

            CreateMenus();
            CreateDraggingHooks();

            Sprites.ItemAdded += Sprites_ItemAdded;
            Sprites.ItemRemoved += Sprites_ItemRemoved;
            Sprites.ItemsAdded += Sprites_ItemsAdded;
            Sprites.ItemsRemoved += Sprites_ItemsRemoved;
        }

        /// <summary>
        /// Updates the ponies.
        /// </summary>
        protected override void Update()
        {
            // Add new effects to the collection, and remove expired effects.
            LinkedListNode<ISprite> spriteNode = Sprites.First;
            while (spriteNode != null)
            {
                PonyInstance pony = spriteNode.Value as PonyInstance;
                if (pony != null)
                {
                    foreach (EffectInstance effect in pony.ActiveEffects)
                        if (effect.AdditionPending)
                        {
                            Sprites.AddLast(effect);
                            effect.AdditionPending = false;
                        }
                }
                else
                {
                    EffectInstance effect = spriteNode.Value as EffectInstance;
                    if (effect != null)
                        if (effect.HasEnded)
                            Sprites.Remove(effect);
                }
                spriteNode = spriteNode.Next;
            }

            PonyInstance.CursorPosition = Viewer.CursorPosition;
            base.Update();
            Sprites.Sort(zOrder);
        }

        /// <summary>
        /// Creates the context menus and hooks them up to the right-click event.
        /// </summary>
        private void CreateMenus()
        {
            // The sprite that was selected when the context menu was opened.
            PonyInstance selectedInstance = null;

            // Items in the pony menu.
            LinkedList<SimpleContextMenuItem> ponyMenuItems = new LinkedList<SimpleContextMenuItem>();
            #region ponyMenuItems
            // Add a new pony based on this template.
            ponyMenuItems.AddLast(new SimpleContextMenuItem(null, (sender, e) => AddNewPony(selectedInstance.Template)));

            // Remove this pony.
            ponyMenuItems.AddLast(new SimpleContextMenuItem(null, (sender, e) => RemovePony(selectedInstance)));

            // Remove all ponies based on this template.
            ponyMenuItems.AddLast(new SimpleContextMenuItem(null, (sender, e) => RemovePonies(selectedInstance.Template)));

            // Separator.
            ponyMenuItems.AddLast(new SimpleContextMenuItem());
            #endregion

            // Items common to all menus.
            LinkedList<SimpleContextMenuItem> commonMenuItems = new LinkedList<SimpleContextMenuItem>();
            #region commonMenuItems
            // Toggle timings graph.
            WinFormSpriteInterface winFormViewer = Viewer as WinFormSpriteInterface;
            if (winFormViewer != null && winFormViewer.Collector != null)
                commonMenuItems.AddLast(new SimpleContextMenuItem(
                    null, (sender, e) => winFormViewer.ShowPerformanceGraph = !winFormViewer.ShowPerformanceGraph));

            // Pause/resume animation.
            commonMenuItems.AddLast(new SimpleContextMenuItem(null, (sender, e) => Paused = !Paused));

            // Toggle topmost level of windows.
            commonMenuItems.AddLast(new SimpleContextMenuItem(null, (sender, e) =>
            {
                Viewer.Topmost = !Viewer.Topmost;
                Viewer.ShowInTaskbar = !Viewer.Topmost;
            }));

            // Return to menu.
            commonMenuItems.AddLast(new SimpleContextMenuItem("Return To Menu", (sender, e) => ReturnToMenu()));

            // Close program.
            commonMenuItems.AddLast(new SimpleContextMenuItem("Exit", (sender, e) => ExitProgram()));
            #endregion

            // Context menus for display on right click.
            ISimpleContextMenu ponyMenu = Viewer.CreateContextMenu(ponyMenuItems.Concat(commonMenuItems));
            ISimpleContextMenu menu = Viewer.CreateContextMenu(commonMenuItems);

            #region Menu Display
            // Display menu on right click.
            Viewer.MouseClick += (sender, e) =>
            {
                if (e.Buttons.HasFlag(SimpleMouseButtons.Right))
                {
                    // Get selected pony.
                    selectedInstance = GetSelectedInstance(e.Location);

                    // Show menu.
                    int i = 0;
                    if (selectedInstance == null)
                    {
                        if (winFormViewer != null && winFormViewer.Collector != null)
                            menu.Items[i++].Text =
                                winFormViewer.ShowPerformanceGraph ? "Hide Performance Graph" : "Show Performance Graph";
                        menu.Items[i++].Text = Paused ? "Resume" : "Pause";
                        menu.Items[i++].Text = Viewer.Topmost ? "Allow Other Windows On Top" : "Keep On Top";
                        menu.Show(e.X, e.Y);
                    }
                    else
                    {
                        ponyMenu.Items[i++].Text = "Add New " + selectedInstance.Template.TemplateName;
                        ponyMenu.Items[i++].Text = "Remove " + selectedInstance.Template.TemplateName;
                        ponyMenu.Items[i++].Text = "Remove Every " + selectedInstance.Template.TemplateName;
                        i++;
                        if (winFormViewer != null && winFormViewer.Collector != null)
                            ponyMenu.Items[i++].Text =
                                winFormViewer.ShowPerformanceGraph ? "Hide Performance Graph" : "Show Performance Graph";
                        ponyMenu.Items[i++].Text = Paused ? "Resume" : "Pause";
                        ponyMenu.Items[i++].Text = Viewer.Topmost ? "Allow Other Windows On Top" : "Keep On Top";
                        ponyMenu.Show(e.X, e.Y);
                    }
                }
            };
            #endregion
        }

        /// <summary>
        /// Make the ponies draggable by hooking up to mouse events.
        /// </summary>
        private void CreateDraggingHooks()
        {
            // The pony instance currently being dragged.
            PonyInstance draggedInstance = null;

            // Pick up a pony.
            Viewer.MouseDown += (sender, e) =>
            {
                if (e.Buttons.HasFlag(SimpleMouseButtons.Left))
                {
                    PonyInstance selectedInstance = GetSelectedInstance(e.Location);
                    if (selectedInstance != null)
                    {
                        selectedInstance.IsDragging = true;
                        draggedInstance = selectedInstance;
                    }
                }
            };

            // Drop the pony.
            Viewer.MouseUp += (sender, e) =>
            {
                if (e.Buttons.HasFlag(SimpleMouseButtons.Left))
                {
                    if (draggedInstance != null)
                    {
                        draggedInstance.IsDragging = false;
                        draggedInstance = null;
                    }
                }
            };
        }

        /// <summary>
        /// Stops animation and raises the <see cref="E:CSDesktopPonies.DesktopPonies.DesktopPonyAnimator.AnimatorClosed"/> event.
        /// </summary>
        private void ReturnToMenu()
        {
            Finish();
            AnimatorClosed.Raise(this);
        }

        /// <summary>
        /// Stops animation and raises the <see cref="E:CSDesktopPonies.DesktopPonies.DesktopPonyAnimator.ProgramExitRequested"/> event.
        /// </summary>
        private void ExitProgram()
        {
            Finish();
            ProgramExitRequested.Raise(this);
        }

        /// <summary>
        /// Creates a new <see cref="T:CSDesktopPonies.DesktopPonies.PonyInstance"/> from the given template and adds it to the collection.
        /// </summary>
        /// <param name="template">The <see cref="T:CSDesktopPonies.DesktopPonies.PonyTemplate"/> from which to create a new instance.
        /// </param>
        private void AddNewPony(PonyTemplate template)
        {
            Sprites.AddLast(new PonyInstance(template));
        }

        /// <summary>
        /// Removes the given <see cref="T:CSDesktopPonies.DesktopPonies.PonyInstance"/> from the collection, along with any effects it
        /// owns.
        /// </summary>
        /// <param name="instance">The existing <see cref="T:CSDesktopPonies.DesktopPonies.PonyInstance"/> to remove.</param>
        private void RemovePony(PonyInstance instance)
        {
            Sprites.RemoveAll(sprite =>
            {
                if (sprite == instance)
                    return true;
                EffectInstance effect = sprite as EffectInstance;
                return effect != null && effect.Parent == instance;
            });
        }

        /// <summary>
        /// Removes every <see cref="T:CSDesktopPonies.DesktopPonies.PonyInstance"/> in the collection which is based on the given
        /// <see cref="T:CSDesktopPonies.DesktopPonies.PonyTemplate"/>, along with any effects they own.
        /// </summary>
        /// <param name="template">The <see cref="T:CSDesktopPonies.DesktopPonies.PonyTemplate"/> whose instances should be removed.
        /// </param>
        private void RemovePonies(PonyTemplate template)
        {
            Sprites.RemoveAll(sprite =>
            {
                PonyInstance pony = sprite as PonyInstance;
                EffectInstance effect = sprite as EffectInstance;
                return
                    (pony != null && pony.Template == template) ||
                    (effect != null && effect.Parent.Template == template);
            });
        }

        /// <summary>
        /// Gets the <see cref="T:CSDesktopPonies.DesktopPonies.PonyInstance"/> closest to the given point, which also has mouseover, from
        /// the collection of sprites.
        /// </summary>
        /// <param name="location">The <see cref="T:System.Drawing.Point"/> to which the closest pony instance is to be found.</param>
        /// <returns>The <see cref="T:CSDesktopPonies.DesktopPonies.PonyInstance"/> that is closest to the given point which also has
        /// mouseover, or null if no instance has mouseover.</returns>
        private PonyInstance GetSelectedInstance(Point location)
        {
            PonyInstance selectedInstance = null;
            int smallestDistance = int.MaxValue;

            foreach (ISprite sprite in Sprites)
            {
                PonyInstance instance = sprite as PonyInstance;
                if (instance != null && instance.HasMouseover)
                {
                    int currentDistance = Vector.DistanceSquared(instance.Position, location);
                    if (currentDistance < smallestDistance)
                    {
                        smallestDistance = currentDistance;
                        selectedInstance = instance;
                    }
                }
            }

            return selectedInstance;
        }

        /// <summary>
        /// Raised when an item is added to Sprites.
        /// Raises an instance count change event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Sprites_ItemAdded(object sender, CollectionItemChangedEventArgs<ISprite> e)
        {
            PonyInstance pony = e.Item as PonyInstance;
            if (pony != null)
                OnInstanceCountChange(new InstanceCountChangedEventArgs(pony.Template, 1));
        }

        /// <summary>
        /// Raised when an item is removed from Sprites.
        /// Raises an instance count change event, and returns to the menu if no instances remain.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Sprites_ItemRemoved(object sender, CollectionItemChangedEventArgs<ISprite> e)
        {
            PonyInstance pony = e.Item as PonyInstance;
            if (pony != null)
                OnInstanceCountChange(new InstanceCountChangedEventArgs(pony.Template, -1));
            
            if (Sprites.Count == 0)
                ReturnToMenu();
        }

        /// <summary>
        /// Raised when multiple items are added to Sprites.
        /// Raises an instance count change event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Sprites_ItemsAdded(object sender, CollectionItemsChangedEventArgs<ISprite> e)
        {
            var groups = e.Items.OfType<PonyInstance>().GroupBy(pony => pony.Template);
            foreach (var group in groups)
                OnInstanceCountChange(new InstanceCountChangedEventArgs(group.Key, group.Count()));
        }

        /// <summary>
        /// Raised when multiple items are removed from Sprites.
        /// Raises an instance count change event, and returns to the menu if no instances remain.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Sprites_ItemsRemoved(object sender, CollectionItemsChangedEventArgs<ISprite> e)
        {
            var groups = e.Items.OfType<PonyInstance>().GroupBy(pony => pony.Template);
            foreach (var group in groups)
                OnInstanceCountChange(new InstanceCountChangedEventArgs(group.Key, -group.Count()));

            if (Sprites.Count == 0)
                ReturnToMenu();
        }

        /// <summary>
        /// Raises the <see cref="E:CSDesktopPonies.DesktopPonies.DesktopPonyAnimator.InstanceCountChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:CSDesktopPonies.DesktopPonies.DesktopPonyAnimator.InstanceCountChangedEventArgs"/> that
        /// contains the event data.</param>
        protected virtual void OnInstanceCountChange(InstanceCountChangedEventArgs e)
        {
            InstanceCountChanged.Raise(this, e);
        }
    }
}
