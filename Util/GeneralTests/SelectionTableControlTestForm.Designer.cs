using Util.CustomControls;

namespace Util.GeneralTests
{
    partial class SelectionTableControlTestForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.Contracts.ContractVerification(false)]
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.selectionTableControl1 = new Util.CustomControls.SelectionTableControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.selectionTableControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 384);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 1;
            // 
            // selectionTableControl1
            // 
            this.selectionTableControl1.ColumnCount = 2;
            this.selectionTableControl1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.selectionTableControl1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.selectionTableControl1.Controls.Add(this.pictureBox1, 0, 0);
            this.selectionTableControl1.Controls.Add(this.pictureBox2, 1, 0);
            this.selectionTableControl1.Controls.Add(this.pictureBox4, 0, 1);
            this.selectionTableControl1.Controls.Add(this.panel1, 1, 1);
            this.selectionTableControl1.Location = new System.Drawing.Point(12, 12);
            this.selectionTableControl1.Name = "selectionTableControl1";
            this.selectionTableControl1.RowCount = 2;
            this.selectionTableControl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.selectionTableControl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.selectionTableControl1.SelectedCell = new System.Windows.Forms.TableLayoutPanelCellPosition(1, 1);
            this.selectionTableControl1.SelectionColor = System.Drawing.Color.Maroon;
            this.selectionTableControl1.SelectorLineWidth = 3;
            this.selectionTableControl1.Size = new System.Drawing.Size(589, 322);
            this.selectionTableControl1.TabIndex = 0;
            this.selectionTableControl1.SelectedPanelChanged += new System.EventHandler<Util.CustomControls.PanelChangedEventArgs>(this.selectionTableControl1_SelectedPanelChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Info;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(288, 155);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(297, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(289, 155);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.SystemColors.Desktop;
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox4.Location = new System.Drawing.Point(3, 164);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(288, 155);
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(297, 164);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(289, 155);
            this.panel1.TabIndex = 4;
            // 
            // SelectionTableControlTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 517);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.selectionTableControl1);
            this.Name = "SelectionTableControlTestForm";
            this.Text = "SelectionTableControlTest";
            this.Load += new System.EventHandler(this.SelectionTableControlTest_Load);
            this.selectionTableControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SelectionTableControl selectionTableControl1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;


    }
}