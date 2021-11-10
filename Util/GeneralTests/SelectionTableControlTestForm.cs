using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics.Contracts;

using Util.CustomControls;

namespace Util.GeneralTests
{
    public partial class SelectionTableControlTestForm : Form
    {
        public SelectionTableControlTestForm()
        {
            InitializeComponent();
        }

        private void SelectionTableControlTest_Load(object sender, EventArgs e)
        {
            Contract.Assume(selectionTableControl1 != null);
            Contract.Assume(label1 != null);

            TableLayoutPanelCellPosition cell = selectionTableControl1.SelectedCell;
            label1.Text = "Currently selected cell: row = " + cell.Row + ", column = " + cell.Column;
        }

        private void selectionTableControl1_SelectedPanelChanged(object sender, PanelChangedEventArgs e)
        {
            Contract.Assume(label1 != null);
            label1.Text = "Currently selected cell: row = " + e.Cell.Row + ", column = " + e.Cell.Column;            
        }
    }
}
