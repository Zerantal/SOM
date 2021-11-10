using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics.Contracts;

using SomLibrary;
using Util;

namespace SOMSimulator
{
    [TODO("this needs generalising to any number of display areas. Picturebox should" + 
        " be generalised to any type of control.")]
    public partial class MultiPanelSelectorControl : UserControl
    {
        private int _selectedPanel;
        private SelectorControl[] _selectorCells;
        private int _rows;
        private int _columns;
        private int _totalCells;

        public event EventHandler SelectedPanelChanged;

        public MultiPanelSelectorControl() : this(2, 2) {}

        public MultiPanelSelectorControl(int Rows, int Columns)
        {
            InitializeComponent();

            _rows = Rows;
            _columns = Columns;
            _totalCells = _rows * _columns;

            _selectorCells = new SelectorControl[_totalCells];
            SelectorControl s;
          
            for (int i = 0; i < _totalCells; i++)
            {
                s = new SelectorControl();
                s.Dock = DockStyle.Fill;
                s.Name = "Cell" + i;
            }

            SelectedPanel = -1;  // no panel selected  
            ConstructDisplayArea();
        }

        public void OnSelectedPanelChanged(EventArgs e)
        {
            this.Refresh();

            if (SelectedPanelChanged != null)
                SelectedPanelChanged(this, e);            
        }


        public int SelectedPanel
        {
            get { return _selectedPanel; }
            set
            {
                if (value < -1 || value >= 4)
                    throw new ArgumentOutOfRangeException("SelectedPanel",
                        "Panel index is not found");

                ((SelectorControl)_selectorCells[_selectedPanel]).Selected = false;

                if (value == -1) return;
                ((SelectorControl)_selectorCells[value]).Selected = true;
                _selectedPanel = value;
            }
        }

        public void SetPanelControl(int panelID, Control ctrl)
        {
            Contract.Requires<ArgumentOutOfRangeException>(panelID >= 0 && panelID < PanelCount);
            Contract.Requires<ArgumentNullException>(ctrl != null);

            _selectorCells[panelID].Controls.Clear();
            _selectorCells[panelID].Controls.Add(ctrl);
            ctrl.Size = _selectorCells[panelID].ClientSize;
            ctrl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.displayTable_MouseClick);
        }

        public Control GetPanelControl(int panelID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(panelID >= 0 && panelID < PanelCount);
            
            return _selectorCells[panelID].Controls[0];
        }

        private void ConstructDisplayArea()
        {
            displayTable.ColumnCount = 2;
            displayTable.ColumnStyles.Clear();
            displayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            displayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            displayTable.RowCount = 2;
            displayTable.RowStyles.Clear();
            displayTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            displayTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));            
            for (int i = 0; i < _totalCells; i++)            
                displayTable.Controls.Add(_selectorCells[i]);                            
        }

        private void displayTable_MouseClick(object sender, MouseEventArgs e)
        {                       
            string cellNum = ((Control)sender).Name.Remove(0,4);
            int i = Convert.ToInt32(cellNum);

            SelectedPanel = i;

            OnSelectedPanelChanged(EventArgs.Empty);
        }

        public int PanelCount
        {
            get { return _totalCells; }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_totalCells == _rows * _columns);
            Contract.Invariant(_selectorCells.Length == _totalCells);
            Contract.Invariant(_selectedPanel >= -1 && _selectedPanel < _totalCells);
            Contract.Invariant(_rows >= 1 && _columns >= 1);
        }
    }   
}
