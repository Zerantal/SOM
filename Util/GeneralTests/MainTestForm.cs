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
    public partial class MainTestForm : Form
    {
        public MainTestForm()
        {
            InitializeComponent();
        }

        private void selectionTableControlTestBtn_Click(object sender, EventArgs e)
        {
            Form test = new SelectionTableControlTestForm();

            test.ShowDialog(this);
        }
    }
}
