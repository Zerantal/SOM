using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace Util.CustomControls
{
    public class SelectionTableControl : TableLayoutPanel
    {
        private TableLayoutPanelCellPosition _selectedCell;
        private int _selectionWidth;
        private Color _selectionColour = Color.Black;

        public event EventHandler<PanelChangedEventArgs> SelectedPanelChanged;

        public SelectionTableControl()
            : base()
        {
            _selectedCell = new TableLayoutPanelCellPosition(-1, -1);

            _selectionWidth = 3;
        }

        [Browsable(true)]
        public TableLayoutPanelCellPosition SelectedCell
        {
            get
            {
                return _selectedCell;
            }

            set
            {
                if (_selectedCell == value)
                    return;

                _selectedCell = value;
                this.Refresh();
            }
        }

        public int SelectorLineWidth
        {
            get { return _selectionWidth; }
            set
            {
                // Contract.Requires(value >= 0 && value < 256);
                _selectionWidth = value;

                this.Refresh();
            }
        }

        public Color SelectionColor
        {
            get { return _selectionColour; }
            set { _selectionColour = value; }
        }

        protected override void OnCellPaint(TableLayoutCellPaintEventArgs e)
        {
            // Contract.Assume(e != null);

            base.OnCellPaint(e);

            Graphics g = CreateGraphics();
            if (_selectedCell == new TableLayoutPanelCellPosition(e.Column, e.Row))
            {
                using (Pen p = new Pen(_selectionColour, _selectionWidth))
                {
                    g.Clear(BackColor);
                    Rectangle cellRect = e.CellBounds;
                    cellRect.X += _selectionWidth / 2;
                    cellRect.Y += _selectionWidth / 2;
                    cellRect.Width -= _selectionWidth;
                    cellRect.Height -= _selectionWidth;
                    g.DrawRectangle(p, cellRect);
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            // Contract.Assume(e != null);

            base.OnControlAdded(e);

            // Contract.Assume(e.Control != null);
            e.Control.Margin = new Padding(_selectionWidth);
            e.Control.MouseClick += new MouseEventHandler(Control_MouseClick);                     
        }



        protected override void OnControlRemoved(ControlEventArgs e)
        {
            // Contract.Assume(e != null);

            base.OnControlRemoved(e);

            // Contract.Assume(e.Control != null);
            e.Control.MouseClick -= Control_MouseClick;                    
        }

        private void Control_MouseClick(object sender, MouseEventArgs e)
        {
            Control ctrl = sender as Control;
            TableLayoutPanelCellPosition newCell;

            if (ctrl != null)
            {
                newCell = this.GetCellPosition(ctrl);

                if (newCell != _selectedCell)
                {
                    _selectedCell = newCell;

                    OnSelectedPanelChanged(new PanelChangedEventArgs(_selectedCell));
                }
            }
        }

        protected virtual void OnSelectedPanelChanged(PanelChangedEventArgs e)
        {
            this.Refresh();

            if (SelectedPanelChanged != null)
                SelectedPanelChanged(this, e);
        }

    }

    public class PanelChangedEventArgs : EventArgs
    {
        TableLayoutPanelCellPosition _cell;

        public PanelChangedEventArgs(TableLayoutPanelCellPosition cell)
        {
            this._cell = cell;
        }

        public TableLayoutPanelCellPosition Cell { get { return _cell; } }
    }
}