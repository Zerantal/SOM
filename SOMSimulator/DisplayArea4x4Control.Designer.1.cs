namespace SOMSimulator
{
    partial class DisplayArea4x4Control
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.displayTable = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // displayTable
            // 
            this.displayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.displayTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayTable.Location = new System.Drawing.Point(0, 0);
            this.displayTable.Name = "displayTable";
            this.displayTable.Size = new System.Drawing.Size(150, 150);
            this.displayTable.TabIndex = 0;
            // 
            // DisplayArea4x4Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.displayTable);
            this.Name = "DisplayArea4x4Control";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel displayTable;
    }
}
