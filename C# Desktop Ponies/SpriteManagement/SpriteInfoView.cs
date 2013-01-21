namespace CSDesktopPonies.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using CSDesktopPonies.Collections;

#pragma warning disable 1591
    /// <summary>
    /// Provides a wrapper around an existing <see cref="T:CSDesktopPonies.SpriteManagement.ISpriteCollectionView"/> that allows sprite
    /// information to be displayed.
    /// </summary>
    public class SpriteInfoView : Disposable, ISpriteCollectionView
    {
        private ISpriteCollectionView view;
        private SpriteInfoForm form;
        private IList<string> columns;
        private Func<ISprite, IList<string>> detailFactory;

        public SpriteInfoView(ISpriteCollectionView viewer, IList<string> columns, Func<ISprite, IList<string>> detailFactory)
        {
            Argument.EnsureNotNull(viewer, "viewer");
            view = viewer;
            this.columns = columns;
            this.detailFactory = detailFactory;
        }

        public void LoadImages(IEnumerable<string> imageFilePaths)
        {
            view.LoadImages(imageFilePaths);
        }

        public void LoadImages(IEnumerable<string> imageFilePaths, EventHandler imageLoadedHandler)
        {
            view.LoadImages(imageFilePaths, imageLoadedHandler);
        }

        public ISimpleContextMenu CreateContextMenu(IEnumerable<ISimpleContextMenuItem> menuItems)
        {
            return view.CreateContextMenu(menuItems);
        }

        public void Open()
        {
            view.Open();
            form = new SpriteInfoForm();
            form.Show();
        }

        public void Hide()
        {
            view.Hide();
            form.Hide();
        }

        public void Show()
        {
            view.Show();
            form.Show();
        }

        public void Pause()
        {
            view.Pause();
        }

        public void Unpause()
        {
            view.Unpause();
        }

        public void Draw(AsyncLinkedList<ISprite> sprites)
        {
            Argument.EnsureNotNull(sprites, "sprites");
            view.Draw(sprites);
            IList<IList<string>> details = new string[sprites.Count][];
            int i = 0;
            foreach (ISprite sprite in sprites)
                details[i++] = detailFactory(sprite);

            form.RefreshInfo(columns, details);
        }

        public void Close()
        {
            view.Close();
            form.Close();
        }

        public string WindowTitle
        {
            get
            {
                return view.WindowTitle;
            }
            set
            {
                view.WindowTitle = value;
            }
        }

        public string WindowIconFilePath
        {
            get
            {
                return view.WindowIconFilePath;
            }
            set
            {
                view.WindowIconFilePath = value;
            }
        }

        public bool Topmost
        {
            get
            {
                return view.Topmost;
            }
            set
            {
                view.Topmost = value;
            }
        }

        public bool ShowInTaskbar
        {
            get
            {
                return view.ShowInTaskbar;
            }
            set
            {
                view.ShowInTaskbar = value;
            }
        }

        public bool IsAlphaBlended
        {
            get { return view.IsAlphaBlended; }
        }

        public Point CursorPosition
        {
            get { return view.CursorPosition; }
        }

        public event EventHandler<SimpleKeyEventArgs> KeyPress
        {
            add { view.KeyPress += value; }
            remove { view.KeyPress -= value; }
        }

        public event EventHandler<SimpleMouseEventArgs> MouseDown
        {
            add { view.MouseDown += value; }
            remove { view.MouseDown -= value; }
        }

        public event EventHandler<SimpleMouseEventArgs> MouseClick
        {
            add { view.MouseClick += value; }
            remove { view.MouseClick -= value; }
        }

        public event EventHandler<SimpleMouseEventArgs> MouseUp
        {
            add { view.MouseUp += value; }
            remove { view.MouseUp -= value; }
        }

        public event EventHandler InterfaceClosed
        {
            add { view.InterfaceClosed += value; }
            remove { view.InterfaceClosed -= value; }
        }

        protected override void Dispose(bool disposing)
        {
            view.Dispose();
            if (form != null)
                form.Dispose();
        }
    }
#pragma warning restore 1591
}