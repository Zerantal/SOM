namespace SOMSimulator
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanAssemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeMapVectorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.infoTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.algorithmPanel = new System.Windows.Forms.Panel();
            this.randomInput = new System.Windows.Forms.CheckBox();
            this.updateIntervalUpDown = new System.Windows.Forms.NumericUpDown();
            this.neuronMapPropGB = new System.Windows.Forms.GroupBox();
            this.neuronMapPropPanel = new System.Windows.Forms.Panel();
            this.neuronMapTypeCB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.trainBtn = new System.Windows.Forms.Button();
            this.algorithmLB = new System.Windows.Forms.ListBox();
            this.algPropGB = new System.Windows.Forms.GroupBox();
            this.algPropPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.detachVisualiserBtn = new System.Windows.Forms.Button();
            this.attachVisBtn = new System.Windows.Forms.Button();
            this.visualiserLB = new System.Windows.Forms.ListBox();
            this.visPropGB = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.visualiserPropPanel = new System.Windows.Forms.Panel();
            this.visualiserLabel = new System.Windows.Forms.Label();
            this.saveImageBtn = new System.Windows.Forms.Button();
            this.animationControls = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.animFilenameTB = new System.Windows.Forms.TextBox();
            this.recordAnimCheckBox = new System.Windows.Forms.CheckBox();
            this.SelectAnimFileBtn = new System.Windows.Forms.Button();
            this.displayTable = new Util.CustomControls.SelectionTableControl();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.algorithmPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateIntervalUpDown)).BeginInit();
            this.neuronMapPropGB.SuspendLayout();
            this.algPropGB.SuspendLayout();
            this.panel2.SuspendLayout();
            this.visPropGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.animationControls.SuspendLayout();
            this.displayTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.inputSourceToolStripMenuItem,
            this.simulateToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanAssemblyToolStripMenuItem,
            this.saveMapMenuItem,
            this.loadMapMenuItem,
            this.writeMapVectorsToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // scanAssemblyToolStripMenuItem
            // 
            this.scanAssemblyToolStripMenuItem.Name = "scanAssemblyToolStripMenuItem";
            this.scanAssemblyToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.scanAssemblyToolStripMenuItem.Text = "Scan &Assembly...";
            this.scanAssemblyToolStripMenuItem.Click += new System.EventHandler(this.OnScanAssemblyToolStripMenuItemClick);
            // 
            // saveMapMenuItem
            // 
            this.saveMapMenuItem.Name = "saveMapMenuItem";
            this.saveMapMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveMapMenuItem.Text = "Save Al&gorithm State...";
            this.saveMapMenuItem.Click += new System.EventHandler(this.saveAlgStateMenuItem_Click);
            // 
            // loadMapMenuItem
            // 
            this.loadMapMenuItem.Name = "loadMapMenuItem";
            this.loadMapMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadMapMenuItem.Text = "L&oad Algorithm State...";
            this.loadMapMenuItem.Click += new System.EventHandler(this.loadAlgStateMenuItem_Click);
            // 
            // writeMapVectorsToolStripMenuItem
            // 
            this.writeMapVectorsToolStripMenuItem.Name = "writeMapVectorsToolStripMenuItem";
            this.writeMapVectorsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.writeMapVectorsToolStripMenuItem.Text = "&Write Out Map Vectors";
            this.writeMapVectorsToolStripMenuItem.Click += new System.EventHandler(this.writeMapVectorsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.OnQuitToolStripMenuItemClick);
            // 
            // inputSourceToolStripMenuItem
            // 
            this.inputSourceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem});
            this.inputSourceToolStripMenuItem.Name = "inputSourceToolStripMenuItem";
            this.inputSourceToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.inputSourceToolStripMenuItem.Text = "Input Source";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.CheckOnClick = true;
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.openFileToolStripMenuItem.Text = "&Training File...";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.SelectInputSource);
            // 
            // simulateToolStripMenuItem
            // 
            this.simulateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trainToolStripMenuItem});
            this.simulateToolStripMenuItem.Name = "simulateToolStripMenuItem";
            this.simulateToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.simulateToolStripMenuItem.Text = "&Simulate";
            // 
            // trainToolStripMenuItem
            // 
            this.trainToolStripMenuItem.Enabled = false;
            this.trainToolStripMenuItem.Name = "trainToolStripMenuItem";
            this.trainToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.trainToolStripMenuItem.Text = "&Train";
            this.trainToolStripMenuItem.Click += new System.EventHandler(this.TrainToolStripMenuItemClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Algorithm";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Available Visualisers";
            // 
            // infoTB
            // 
            this.infoTB.BackColor = System.Drawing.Color.AliceBlue;
            this.infoTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoTB.Location = new System.Drawing.Point(3, 500);
            this.infoTB.Multiline = true;
            this.infoTB.Name = "infoTB";
            this.infoTB.ReadOnly = true;
            this.infoTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoTB.Size = new System.Drawing.Size(489, 205);
            this.infoTB.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 355);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Information";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 495F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.algorithmPanel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.infoTB, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.displayTable, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.33898F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.66102F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1008, 708);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // algorithmPanel
            // 
            this.algorithmPanel.AutoScroll = true;
            this.algorithmPanel.Controls.Add(this.randomInput);
            this.algorithmPanel.Controls.Add(this.updateIntervalUpDown);
            this.algorithmPanel.Controls.Add(this.neuronMapPropGB);
            this.algorithmPanel.Controls.Add(this.label4);
            this.algorithmPanel.Controls.Add(this.cancelBtn);
            this.algorithmPanel.Controls.Add(this.trainBtn);
            this.algorithmPanel.Controls.Add(this.algorithmLB);
            this.algorithmPanel.Controls.Add(this.label1);
            this.algorithmPanel.Controls.Add(this.algPropGB);
            this.algorithmPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.algorithmPanel.Location = new System.Drawing.Point(3, 3);
            this.algorithmPanel.Name = "algorithmPanel";
            this.algorithmPanel.Size = new System.Drawing.Size(489, 491);
            this.algorithmPanel.TabIndex = 0;
            // 
            // randomInput
            // 
            this.randomInput.AutoSize = true;
            this.randomInput.Checked = true;
            this.randomInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.randomInput.Location = new System.Drawing.Point(3, 390);
            this.randomInput.Name = "randomInput";
            this.randomInput.Size = new System.Drawing.Size(106, 17);
            this.randomInput.TabIndex = 22;
            this.randomInput.Text = "Randomize Input";
            this.randomInput.UseVisualStyleBackColor = true;
            this.randomInput.CheckedChanged += new System.EventHandler(this.randomInput_CheckedChanged);
            // 
            // updateIntervalUpDown
            // 
            this.updateIntervalUpDown.Location = new System.Drawing.Point(84, 338);
            this.updateIntervalUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.updateIntervalUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updateIntervalUpDown.Name = "updateIntervalUpDown";
            this.updateIntervalUpDown.Size = new System.Drawing.Size(63, 20);
            this.updateIntervalUpDown.TabIndex = 20;
            this.updateIntervalUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // neuronMapPropGB
            // 
            this.neuronMapPropGB.Controls.Add(this.neuronMapPropPanel);
            this.neuronMapPropGB.Controls.Add(this.neuronMapTypeCB);
            this.neuronMapPropGB.Controls.Add(this.label5);
            this.neuronMapPropGB.Location = new System.Drawing.Point(164, 281);
            this.neuronMapPropGB.Name = "neuronMapPropGB";
            this.neuronMapPropGB.Size = new System.Drawing.Size(319, 207);
            this.neuronMapPropGB.TabIndex = 16;
            this.neuronMapPropGB.TabStop = false;
            this.neuronMapPropGB.Text = "Neuron Map Properties";
            // 
            // neuronMapPropPanel
            // 
            this.neuronMapPropPanel.AutoScroll = true;
            this.neuronMapPropPanel.AutoSize = true;
            this.neuronMapPropPanel.Location = new System.Drawing.Point(6, 45);
            this.neuronMapPropPanel.Name = "neuronMapPropPanel";
            this.neuronMapPropPanel.Size = new System.Drawing.Size(307, 156);
            this.neuronMapPropPanel.TabIndex = 4;
            // 
            // neuronMapTypeCB
            // 
            this.neuronMapTypeCB.FormattingEnabled = true;
            this.neuronMapTypeCB.Location = new System.Drawing.Point(77, 18);
            this.neuronMapTypeCB.Name = "neuronMapTypeCB";
            this.neuronMapTypeCB.Size = new System.Drawing.Size(175, 21);
            this.neuronMapTypeCB.TabIndex = 3;
            this.neuronMapTypeCB.SelectedIndexChanged += new System.EventHandler(this.neuronMapTypeCB_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Map Type: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 340);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Update Interval:";
            // 
            // cancelBtn
            // 
            this.cancelBtn.Enabled = false;
            this.cancelBtn.Image = global::SOMSimulator.Properties.Resources.process_stop;
            this.cancelBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelBtn.Location = new System.Drawing.Point(70, 304);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(66, 23);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.OnCancelBtnClick);
            // 
            // trainBtn
            // 
            this.trainBtn.Enabled = false;
            this.trainBtn.Location = new System.Drawing.Point(3, 304);
            this.trainBtn.Name = "trainBtn";
            this.trainBtn.Size = new System.Drawing.Size(61, 23);
            this.trainBtn.TabIndex = 0;
            this.trainBtn.Text = "Train";
            this.trainBtn.UseVisualStyleBackColor = true;
            this.trainBtn.Click += new System.EventHandler(this.TrainToolStripMenuItemClick);
            // 
            // algorithmLB
            // 
            this.algorithmLB.FormattingEnabled = true;
            this.algorithmLB.Location = new System.Drawing.Point(6, 35);
            this.algorithmLB.Name = "algorithmLB";
            this.algorithmLB.Size = new System.Drawing.Size(141, 95);
            this.algorithmLB.TabIndex = 14;
            this.algorithmLB.SelectedIndexChanged += new System.EventHandler(this.OnAlgorithmLbSelectedIndexChanged);
            this.algorithmLB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.algorithmLB_MouseMove);
            // 
            // algPropGB
            // 
            this.algPropGB.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algPropGB.Controls.Add(this.algPropPanel);
            this.algPropGB.Location = new System.Drawing.Point(164, 3);
            this.algPropGB.Name = "algPropGB";
            this.algPropGB.Size = new System.Drawing.Size(322, 272);
            this.algPropGB.TabIndex = 7;
            this.algPropGB.TabStop = false;
            this.algPropGB.Text = "Algorithm Properties";
            // 
            // algPropPanel
            // 
            this.algPropPanel.AutoScroll = true;
            this.algPropPanel.AutoSize = true;
            this.algPropPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algPropPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.algPropPanel.Location = new System.Drawing.Point(3, 16);
            this.algPropPanel.Name = "algPropPanel";
            this.algPropPanel.Size = new System.Drawing.Size(316, 253);
            this.algPropPanel.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.detachVisualiserBtn);
            this.panel2.Controls.Add(this.attachVisBtn);
            this.panel2.Controls.Add(this.visualiserLB);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.visPropGB);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(498, 500);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(507, 205);
            this.panel2.TabIndex = 7;
            // 
            // detachVisualiserBtn
            // 
            this.detachVisualiserBtn.Location = new System.Drawing.Point(6, 163);
            this.detachVisualiserBtn.Name = "detachVisualiserBtn";
            this.detachVisualiserBtn.Size = new System.Drawing.Size(99, 23);
            this.detachVisualiserBtn.TabIndex = 15;
            this.detachVisualiserBtn.Text = "Detach Visualiser";
            this.toolTip1.SetToolTip(this.detachVisualiserBtn, "Remove the visualiser from currently selected display panel");
            this.detachVisualiserBtn.UseVisualStyleBackColor = true;
            this.detachVisualiserBtn.Click += new System.EventHandler(this.detachVisualiserBtn_Click);
            // 
            // attachVisBtn
            // 
            this.attachVisBtn.Location = new System.Drawing.Point(6, 133);
            this.attachVisBtn.Name = "attachVisBtn";
            this.attachVisBtn.Size = new System.Drawing.Size(99, 24);
            this.attachVisBtn.TabIndex = 14;
            this.attachVisBtn.Text = "Attach Visualiser";
            this.toolTip1.SetToolTip(this.attachVisBtn, "Attach selected visualiser to currently selected display panel");
            this.attachVisBtn.UseVisualStyleBackColor = true;
            this.attachVisBtn.Click += new System.EventHandler(this.attachVisBtn_Click);
            // 
            // visualiserLB
            // 
            this.visualiserLB.FormattingEnabled = true;
            this.visualiserLB.Location = new System.Drawing.Point(6, 19);
            this.visualiserLB.Name = "visualiserLB";
            this.visualiserLB.Size = new System.Drawing.Size(141, 108);
            this.visualiserLB.TabIndex = 13;
            // 
            // visPropGB
            // 
            this.visPropGB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.visPropGB.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.visPropGB.Controls.Add(this.splitContainer1);
            this.visPropGB.Location = new System.Drawing.Point(177, 3);
            this.visPropGB.Name = "visPropGB";
            this.visPropGB.Size = new System.Drawing.Size(325, 192);
            this.visPropGB.TabIndex = 8;
            this.visPropGB.TabStop = false;
            this.visPropGB.Text = "Properties of Selected Display";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(3, 16);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.visualiserPropPanel);
            this.splitContainer1.Panel1.Controls.Add(this.visualiserLabel);
            this.splitContainer1.Panel1.Controls.Add(this.saveImageBtn);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.animationControls);
            this.splitContainer1.Panel2MinSize = 71;
            this.splitContainer1.Size = new System.Drawing.Size(319, 173);
            this.splitContainer1.SplitterDistance = 101;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // visualiserPropPanel
            // 
            this.visualiserPropPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.visualiserPropPanel.Location = new System.Drawing.Point(0, 26);
            this.visualiserPropPanel.Name = "visualiserPropPanel";
            this.visualiserPropPanel.Size = new System.Drawing.Size(319, 75);
            this.visualiserPropPanel.TabIndex = 1;
            // 
            // visualiserLabel
            // 
            this.visualiserLabel.AutoSize = true;
            this.visualiserLabel.Location = new System.Drawing.Point(6, 4);
            this.visualiserLabel.Name = "visualiserLabel";
            this.visualiserLabel.Size = new System.Drawing.Size(0, 13);
            this.visualiserLabel.TabIndex = 0;
            // 
            // saveImageBtn
            // 
            this.saveImageBtn.Enabled = false;
            this.saveImageBtn.Location = new System.Drawing.Point(232, -1);
            this.saveImageBtn.Name = "saveImageBtn";
            this.saveImageBtn.Size = new System.Drawing.Size(85, 23);
            this.saveImageBtn.TabIndex = 2;
            this.saveImageBtn.Text = "Save Image...";
            this.saveImageBtn.UseVisualStyleBackColor = true;
            this.saveImageBtn.Click += new System.EventHandler(this.saveImageBtn_Click);
            // 
            // animationControls
            // 
            this.animationControls.Controls.Add(this.label10);
            this.animationControls.Controls.Add(this.animFilenameTB);
            this.animationControls.Controls.Add(this.recordAnimCheckBox);
            this.animationControls.Controls.Add(this.SelectAnimFileBtn);
            this.animationControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.animationControls.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.animationControls.Location = new System.Drawing.Point(0, 0);
            this.animationControls.Name = "animationControls";
            this.animationControls.Size = new System.Drawing.Size(319, 71);
            this.animationControls.TabIndex = 19;
            this.animationControls.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 39);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Filename:";
            // 
            // animFilenameTB
            // 
            this.animFilenameTB.Enabled = false;
            this.animFilenameTB.Location = new System.Drawing.Point(61, 36);
            this.animFilenameTB.Name = "animFilenameTB";
            this.animFilenameTB.Size = new System.Drawing.Size(146, 20);
            this.animFilenameTB.TabIndex = 18;
            this.animFilenameTB.TextChanged += new System.EventHandler(this.animFilenameTB_TextChanged);
            // 
            // recordAnimCheckBox
            // 
            this.recordAnimCheckBox.AutoSize = true;
            this.recordAnimCheckBox.Location = new System.Drawing.Point(3, 19);
            this.recordAnimCheckBox.Name = "recordAnimCheckBox";
            this.recordAnimCheckBox.Size = new System.Drawing.Size(110, 17);
            this.recordAnimCheckBox.TabIndex = 6;
            this.recordAnimCheckBox.Text = "Record Animation";
            this.recordAnimCheckBox.UseVisualStyleBackColor = true;
            this.recordAnimCheckBox.CheckedChanged += new System.EventHandler(this.OnRecordAnimCheckBoxCheckedChanged);
            // 
            // SelectAnimFileBtn
            // 
            this.SelectAnimFileBtn.Enabled = false;
            this.SelectAnimFileBtn.Location = new System.Drawing.Point(213, 34);
            this.SelectAnimFileBtn.Name = "SelectAnimFileBtn";
            this.SelectAnimFileBtn.Size = new System.Drawing.Size(25, 23);
            this.SelectAnimFileBtn.TabIndex = 17;
            this.SelectAnimFileBtn.Text = "...";
            this.SelectAnimFileBtn.UseVisualStyleBackColor = true;
            this.SelectAnimFileBtn.Click += new System.EventHandler(this.OnSelectAnimFileBtnClick);
            // 
            // displayTable
            // 
            this.displayTable.ColumnCount = 2;
            this.displayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.displayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.displayTable.Controls.Add(this.pictureBox4, 1, 1);
            this.displayTable.Controls.Add(this.pictureBox3, 0, 1);
            this.displayTable.Controls.Add(this.pictureBox2, 1, 0);
            this.displayTable.Controls.Add(this.pictureBox1, 0, 0);
            this.displayTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayTable.Location = new System.Drawing.Point(498, 3);
            this.displayTable.Name = "displayTable";
            this.displayTable.RowCount = 2;
            this.displayTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.displayTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.displayTable.SelectedCell = new System.Windows.Forms.TableLayoutPanelCellPosition(0, 0);
            this.displayTable.SelectionColor = System.Drawing.Color.Red;
            this.displayTable.SelectorLineWidth = 3;
            this.displayTable.Size = new System.Drawing.Size(507, 491);
            this.displayTable.TabIndex = 8;
            this.displayTable.SelectedPanelChanged += new System.EventHandler<Util.CustomControls.PanelChangedEventArgs>(this.displayTable_SelectedPanelChanged);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox4.Location = new System.Drawing.Point(256, 248);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(248, 240);
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Location = new System.Drawing.Point(3, 248);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(247, 240);
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(256, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(248, 239);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(247, 239);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tableLayoutPanel2);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.label3);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1008, 708);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(1008, 732);
            this.toolStripContainer1.TabIndex = 6;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1008, 732);
            this.Controls.Add(this.toolStripContainer1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(790, 590);
            this.Name = "MainForm";
            this.Text = "SOM simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.algorithmPanel.ResumeLayout(false);
            this.algorithmPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateIntervalUpDown)).EndInit();
            this.neuronMapPropGB.ResumeLayout(false);
            this.neuronMapPropGB.PerformLayout();
            this.algPropGB.ResumeLayout(false);
            this.algPropGB.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.visPropGB.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.animationControls.ResumeLayout(false);
            this.animationControls.PerformLayout();
            this.displayTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox infoTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel algorithmPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button trainBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scanAssemblyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simulateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trainToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox recordAnimCheckBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox animFilenameTB;
        private System.Windows.Forms.Button SelectAnimFileBtn;
        private System.Windows.Forms.ToolStripMenuItem inputSourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.GroupBox animationControls;
        private System.Windows.Forms.NumericUpDown updateIntervalUpDown;
        private System.Windows.Forms.ListBox algorithmLB;
        private System.Windows.Forms.ListBox visualiserLB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox visPropGB;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox neuronMapPropGB;
        private System.Windows.Forms.ComboBox neuronMapTypeCB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel neuronMapPropPanel;
        private System.Windows.Forms.Button attachVisBtn;
        private System.Windows.Forms.Panel visualiserPropPanel;
        private System.Windows.Forms.Label visualiserLabel;
        private System.Windows.Forms.Button saveImageBtn;
        private System.Windows.Forms.ToolStripMenuItem writeMapVectorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMapMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadMapMenuItem;
        private System.Windows.Forms.CheckBox randomInput;
        private System.Windows.Forms.Button detachVisualiserBtn;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox algPropGB;
        private System.Windows.Forms.Panel algPropPanel;
        private Util.CustomControls.SelectionTableControl displayTable;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

