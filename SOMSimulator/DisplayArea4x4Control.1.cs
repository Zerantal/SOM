using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SOMSimulator
{
    public partial class DisplayArea4x4Control : UserControl
    {
        private int m_SelectedPanel;
        private SelectorControl[] SelectorCells;

        public DisplayArea4x4Control()
        {
            InitializeComponent();

            SelectorCells = new SelectorControl[4];
            SelectorControl s;
            DisplayPanelControl p;
            for (int i = 0; i < 4; i++)
            {
                s = new SelectorControl();
                p = new DisplayPanelControl();
                s.Controls.Add(p);
                SelectorCells[i] = s;
                s.Dock = DockStyle.Fill;
                p.BackColor = System.Drawing.SystemColors.Desktop;
                p.Dock = DockStyle.Fill;
                p.Location = new Point(0, 0);
                p.Name = "Display" + i;
            }

            SelectedPanel = -1;  // no panel selected  
            ConstructDisplayArea();
        }


        public int SelectedPanel
        {
            get { return m_SelectedPanel; }
            set
            {
                if (value < -1 || value >= 4)
                    throw new ArgumentOutOfRangeException("SelectedPanel",
                        "Panel index is not found");

                ((SelectorControl)SelectorCells[m_SelectedPanel]).Selected = false;

                if (value == -1) return;
                ((SelectorControl)SelectorCells[value]).Selected = true;
                m_SelectedPanel = value;
            }
        }

        private void ConstructDisplayArea()
        {
            DisplayPanelControl p;

            displayTable.ColumnCount = 2;
            displayTable.ColumnStyles.Clear();
            displayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            displayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            displayTable.RowCount = 2;
            displayTable.RowStyles.Clear();
            displayTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            displayTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));

            for (int i = 0; i < 4; i++)
            {
                displayTable.Controls.Add(SelectorCells[i]);
                p = (DisplayPanelControl)((SelectorControl)SelectorCells[i]).Controls["Display" + i];
                p.Size = displayTable.Controls[i].ClientSize;
                p.MouseClick += new System.Windows.Forms.MouseEventHandler(this.displayTable_MouseClick);
            }         
        }

        private void displayTable_MouseClick(object sender, MouseEventArgs e)
        {
            DisplayPanelControl panel = (DisplayPanelControl)sender;
            string num = panel.Name.Remove(0, 7);
            int i = Convert.ToInt32(num);

            SelectedPanel = i;
        }
    }   
}
