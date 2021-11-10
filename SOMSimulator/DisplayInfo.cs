using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Diagnostics.Contracts;

using SomLibrary;
using SomLibrary.Drawers;
using Util;

namespace SOMSimulator
{
    [Serializable]
    class DisplayInfo
    {        
        [NonSerialized]
        private IVisualiser _visualiser;
        [NonSerialized]
        private GifAnimator _gifAnimation;
        [NonSerialized]
        private WinDrawer _panelDrawer;
        [NonSerialized]
        private WinDrawer _animationDrawer;
        [NonSerialized]
        private PictureBox _pb; // picture box control associated with panel   
        [NonSerialized]
        private int fnCounter = 0;

        private bool _generatingAnimation;
        private string _animationFilename;
        private string _visualiserName;  // used for the deserializing of this data structure        
        private TableLayoutPanelCellPosition _displayCell;
         
        public DisplayInfo(PictureBox pictureBox, TableLayoutPanelCellPosition cell, IVisualiser vis) : 
            this(pictureBox, cell)
        {
            // Contract.Requires(pictureBox != null);
            // Contract.Requires(vis != null);

            this._visualiser = vis;
        }

        public DisplayInfo(PictureBox pictureBox, TableLayoutPanelCellPosition cell)
        {
            // Contract.Requires(pictureBox != null);

            this._pb = pictureBox;
            this._displayCell = cell;
            _panelDrawer = new WinDrawer(pictureBox.Size);
            this._visualiserName = pictureBox.Name;
            if (VisualiserName == null)
                VisualiserName = "Display" + fnCounter++;

            _gifAnimation = new GifAnimator();
            _animationDrawer = new WinDrawer(800, 600);
            _animationFilename = VisualiserName + ".gif";

            this._pb.SizeChanged += new EventHandler(_pb_SizeChanged);
        }

        public string VisualiserName
        {
            get { return _visualiserName; }
            set 
            {
                // Contract.Requires(value != null);

                _visualiserName = value; 
            }
        }

        public IVisualiser Visualiser
        {
            get { return _visualiser; }
            set { this._visualiser = value; }
        }

        public string AnimationFilename
        {
            get { return _animationFilename;}
            set { _animationFilename = value; }
        }

        public bool GeneratingAnimation
        {
            get { return _generatingAnimation; }
            set 
            {
                _generatingAnimation = value;
                _gifAnimation.Clear();
            }
        }

        public TableLayoutPanelCellPosition DisplayCell { get { return _displayCell; } }

        public Size AnimationSize 
        { 
            get { return _animationDrawer.Size; }
            set { _animationDrawer.Size = value; }
        }


        public Image Image { get { return _pb.Image; } }

        // method is threadsafe
        internal void DrawMapVisual(ISOM algorithm)
        {
            // Contract.Requires(algorithm != null);

            if (_visualiser != null)
            {
                lock (_visualiser)
                {
                    if (_visualiser.CanVisualiseMap(algorithm))
                    {
                        _visualiser.VisualiseMap(algorithm, _panelDrawer);

                        if (_generatingAnimation)
                        {
                            _visualiser.VisualiseMap(algorithm, _animationDrawer);
                            Image im = _animationDrawer.GetImage();
                            _gifAnimation.AddFrame((Image)im.Clone(), 25);
                        }
                    }
                }
            }
        }

        internal void PaintPictureBox()
        {
            Image im = _panelDrawer.GetImage();
            _pb.Image = (Image)im.Clone();

        }

        internal void WriteAnimation()
        {
            // Contract.Requires(AnimationFilename != null);

            if (_generatingAnimation)
            { // Save animation                       
                try
                {
                    _gifAnimation.SaveFile(Path.GetFileName(_animationFilename));
                }
                catch
                {
                    Trace.WriteLine("Error writing animation file: " +
                        Path.GetFileName(_animationFilename));
                }
                _gifAnimation.Clear();
            }
        }

        private void _pb_SizeChanged(object sender, EventArgs e)
        {
            _panelDrawer = new WinDrawer(_pb.Size);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(_pb != null);
            // Contract.Invariant(_panelDrawer != null);
            // Contract.Invariant(_visualiserName != null);
            // Contract.Invariant(_animationDrawer != null);
            // Contract.Invariant(_gifAnimation != null);
        }
    }
}
