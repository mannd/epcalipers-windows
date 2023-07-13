using System;
using System.Windows.Forms;

namespace epcalipers
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.thumbnail = new System.Windows.Forms.PictureBox();
			this.ecgPictureBox = new System.Windows.Forms.PictureBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.zoomInButton = new System.Windows.Forms.Button();
			this.zoomOutButton = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
			this.quitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.rotate90RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rotate90LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rotate1RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rotate1LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rotateTinyRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rotateTinyLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetImageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.nextPageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.previousPageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.gotoPDFPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.calipersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timeCaliperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.amplitudeCaliperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.angleCaliperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteCaliperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteAllCalipersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.calibrateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearCalibrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toggleRateintervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.meanRateIntervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.qTcMeasurementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.transparentWindowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutEPCalipersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.caliperColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tweakToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.marchingCaliperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ecgPictureBox)).BeginInit();
			this.flowLayoutPanel2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.34752F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.65248F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 44);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1376, 883);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(162, 818);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(1208, 59);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
			this.panel1.Controls.Add(this.thumbnail);
			this.panel1.Controls.Add(this.ecgPictureBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(6, 6);
			this.panel1.Margin = new System.Windows.Forms.Padding(6);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1364, 800);
			this.panel1.TabIndex = 3;
			// 
			// thumbnail
			// 
			this.thumbnail.Location = new System.Drawing.Point(0, 0);
			this.thumbnail.Margin = new System.Windows.Forms.Padding(6);
			this.thumbnail.Name = "thumbnail";
			this.thumbnail.Size = new System.Drawing.Size(200, 96);
			this.thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.thumbnail.TabIndex = 1;
			this.thumbnail.TabStop = false;
			this.thumbnail.Visible = false;
			// 
			// ecgPictureBox
			// 
			this.ecgPictureBox.BackColor = System.Drawing.SystemColors.Window;
			this.ecgPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ecgPictureBox.Location = new System.Drawing.Point(0, 0);
			this.ecgPictureBox.Margin = new System.Windows.Forms.Padding(6);
			this.ecgPictureBox.Name = "ecgPictureBox";
			this.ecgPictureBox.Size = new System.Drawing.Size(0, 0);
			this.ecgPictureBox.TabIndex = 0;
			this.ecgPictureBox.TabStop = false;
			this.ecgPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EcgPictureBox_MouseMove);
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
			this.flowLayoutPanel2.Controls.Add(this.zoomInButton);
			this.flowLayoutPanel2.Controls.Add(this.zoomOutButton);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(6, 818);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(6);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(144, 59);
			this.flowLayoutPanel2.TabIndex = 4;
			// 
			// zoomInButton
			// 
			this.zoomInButton.AutoSize = true;
			this.zoomInButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.zoomInButton.Location = new System.Drawing.Point(6, 6);
			this.zoomInButton.Margin = new System.Windows.Forms.Padding(6);
			this.zoomInButton.Name = "zoomInButton";
			this.zoomInButton.Size = new System.Drawing.Size(34, 35);
			this.zoomInButton.TabIndex = 0;
			this.zoomInButton.Text = "+";
			this.toolTip1.SetToolTip(this.zoomInButton, "Zoom in");
			this.zoomInButton.UseVisualStyleBackColor = true;
			this.zoomInButton.Click += new System.EventHandler(this.ZoomInButton_Click);
			// 
			// zoomOutButton
			// 
			this.zoomOutButton.AutoSize = true;
			this.zoomOutButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.zoomOutButton.Location = new System.Drawing.Point(52, 6);
			this.zoomOutButton.Margin = new System.Windows.Forms.Padding(6);
			this.zoomOutButton.Name = "zoomOutButton";
			this.zoomOutButton.Size = new System.Drawing.Size(29, 35);
			this.zoomOutButton.TabIndex = 1;
			this.zoomOutButton.Text = "-";
			this.toolTip1.SetToolTip(this.zoomOutButton, "Zoom out");
			this.zoomOutButton.UseVisualStyleBackColor = true;
			this.zoomOutButton.Click += new System.EventHandler(this.ZoomOutButton_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// menuStrip1
			// 
			this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.zoomToolStripMenuItem,
            this.calipersToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1376, 44);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.printToolStripMenuItem,
            this.quitToolStripMenuItem,
            this.quitToolStripMenuItem1});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(74, 40);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(306, 44);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.ImageButton_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(306, 44);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
			// 
			// printToolStripMenuItem
			// 
			this.printToolStripMenuItem.Name = "printToolStripMenuItem";
			this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.printToolStripMenuItem.Size = new System.Drawing.Size(306, 44);
			this.printToolStripMenuItem.Text = "Print";
			this.printToolStripMenuItem.Click += new System.EventHandler(this.PrintToolStripMenuItem_Click);
			// 
			// quitToolStripMenuItem
			// 
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.Size = new System.Drawing.Size(303, 6);
			// 
			// quitToolStripMenuItem1
			// 
			this.quitToolStripMenuItem1.Name = "quitToolStripMenuItem1";
			this.quitToolStripMenuItem1.Size = new System.Drawing.Size(306, 44);
			this.quitToolStripMenuItem1.Text = "Quit";
			this.quitToolStripMenuItem1.Click += new System.EventHandler(this.QuitToolStripMenuItem1_Click);
			// 
			// zoomToolStripMenuItem
			// 
			this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem,
            this.resetZoomToolStripMenuItem,
            this.toolStripSeparator2,
            this.rotate90RToolStripMenuItem,
            this.rotate90LToolStripMenuItem,
            this.rotate1RToolStripMenuItem,
            this.rotate1LToolStripMenuItem,
            this.rotateTinyRToolStripMenuItem,
            this.rotateTinyLToolStripMenuItem,
            this.resetImageToolStripMenuItem1,
            this.toolStripSeparator3,
            this.nextPageToolStripMenuItem1,
            this.previousPageToolStripMenuItem1,
            this.gotoPDFPageToolStripMenuItem});
			this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
			this.zoomToolStripMenuItem.Size = new System.Drawing.Size(106, 40);
			this.zoomToolStripMenuItem.Text = "Image";
			// 
			// zoomInToolStripMenuItem
			// 
			this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
			this.zoomInToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl++";
			this.zoomInToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
			this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(471, 44);
			this.zoomInToolStripMenuItem.Text = "Zoom in";
			this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.ZoomInToolStripMenuItem_Click);
			// 
			// zoomOutToolStripMenuItem
			// 
			this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
			this.zoomOutToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+-";
			this.zoomOutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
			this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(471, 44);
			this.zoomOutToolStripMenuItem.Text = "Zoom out";
			this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.ZoomOutToolStripMenuItem_Click);
			// 
			// resetZoomToolStripMenuItem
			// 
			this.resetZoomToolStripMenuItem.Name = "resetZoomToolStripMenuItem";
			this.resetZoomToolStripMenuItem.Size = new System.Drawing.Size(471, 44);
			this.resetZoomToolStripMenuItem.Text = "Reset zoom";
			this.resetZoomToolStripMenuItem.Click += new System.EventHandler(this.ResetZoomToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(468, 6);
			// 
			// rotate90RToolStripMenuItem
			// 
			this.rotate90RToolStripMenuItem.Name = "rotate90RToolStripMenuItem";
			this.rotate90RToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
			this.rotate90RToolStripMenuItem.Size = new System.Drawing.Size(471, 44);
			this.rotate90RToolStripMenuItem.Text = "Rotate 90° R";
			this.rotate90RToolStripMenuItem.Click += new System.EventHandler(this.Rotate90RToolStripMenuItem_Click_1);
			// 
			// rotate90LToolStripMenuItem
			// 
			this.rotate90LToolStripMenuItem.Name = "rotate90LToolStripMenuItem";
			this.rotate90LToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.L)));
			this.rotate90LToolStripMenuItem.Size = new System.Drawing.Size(471, 44);
			this.rotate90LToolStripMenuItem.Text = "Rotate 90° L";
			this.rotate90LToolStripMenuItem.Click += new System.EventHandler(this.Rotate90LToolStripMenuItem_Click);
			// 
			// rotate1RToolStripMenuItem
			// 
			this.rotate1RToolStripMenuItem.Name = "rotate1RToolStripMenuItem";
			this.rotate1RToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.rotate1RToolStripMenuItem.Size = new System.Drawing.Size(471, 44);
			this.rotate1RToolStripMenuItem.Text = "Rotate 1°R";
			this.rotate1RToolStripMenuItem.Click += new System.EventHandler(this.Rotate1RToolStripMenuItem_Click);
			// 
			// rotate1LToolStripMenuItem
			// 
			this.rotate1LToolStripMenuItem.Name = "rotate1LToolStripMenuItem";
			this.rotate1LToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.rotate1LToolStripMenuItem.Size = new System.Drawing.Size(471, 44);
			this.rotate1LToolStripMenuItem.Text = "Rotate 1°L";
			this.rotate1LToolStripMenuItem.Click += new System.EventHandler(this.Rotate1LToolStripMenuItem_Click);
			// 
			// rotateTinyRToolStripMenuItem
			// 
			this.rotateTinyRToolStripMenuItem.Name = "rotateTinyRToolStripMenuItem";
			this.rotateTinyRToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.R)));
			this.rotateTinyRToolStripMenuItem.Size = new System.Drawing.Size(471, 44);
			this.rotateTinyRToolStripMenuItem.Text = "Rotate 0.1°R";
			this.rotateTinyRToolStripMenuItem.Click += new System.EventHandler(this.RotateTinyRToolStripMenuItem_Click);
			// 
			// rotateTinyLToolStripMenuItem
			// 
			this.rotateTinyLToolStripMenuItem.Name = "rotateTinyLToolStripMenuItem";
			this.rotateTinyLToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.L)));
			this.rotateTinyLToolStripMenuItem.Size = new System.Drawing.Size(471, 44);
			this.rotateTinyLToolStripMenuItem.Text = "Rotate 0.1°L";
			this.rotateTinyLToolStripMenuItem.Click += new System.EventHandler(this.RotateTinyLToolStripMenuItem_Click);
			// 
			// resetImageToolStripMenuItem1
			// 
			this.resetImageToolStripMenuItem1.Name = "resetImageToolStripMenuItem1";
			this.resetImageToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0)));
			this.resetImageToolStripMenuItem1.Size = new System.Drawing.Size(471, 44);
			this.resetImageToolStripMenuItem1.Text = "Reset image";
			this.resetImageToolStripMenuItem1.Click += new System.EventHandler(this.ResetImageToolStripMenuItem1_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(468, 6);
			// 
			// nextPageToolStripMenuItem1
			// 
			this.nextPageToolStripMenuItem1.Name = "nextPageToolStripMenuItem1";
			this.nextPageToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Right)));
			this.nextPageToolStripMenuItem1.Size = new System.Drawing.Size(471, 44);
			this.nextPageToolStripMenuItem1.Text = "Next PDF page";
			this.nextPageToolStripMenuItem1.Click += new System.EventHandler(this.NextPageToolStripMenuItem1_Click);
			// 
			// previousPageToolStripMenuItem1
			// 
			this.previousPageToolStripMenuItem1.Name = "previousPageToolStripMenuItem1";
			this.previousPageToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Left)));
			this.previousPageToolStripMenuItem1.Size = new System.Drawing.Size(471, 44);
			this.previousPageToolStripMenuItem1.Text = "Previous PDF page";
			this.previousPageToolStripMenuItem1.Click += new System.EventHandler(this.PreviousPageToolStripMenuItem1_Click);
			// 
			// gotoPDFPageToolStripMenuItem
			// 
			this.gotoPDFPageToolStripMenuItem.Name = "gotoPDFPageToolStripMenuItem";
			this.gotoPDFPageToolStripMenuItem.Size = new System.Drawing.Size(471, 44);
			this.gotoPDFPageToolStripMenuItem.Text = "Go to PDF page";
			this.gotoPDFPageToolStripMenuItem.Click += new System.EventHandler(this.GotoPDFPageToolStripMenuItem_Click);
			// 
			// calipersToolStripMenuItem
			// 
			this.calipersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeCaliperToolStripMenuItem,
            this.amplitudeCaliperToolStripMenuItem,
            this.angleCaliperToolStripMenuItem,
            this.deleteCaliperToolStripMenuItem,
            this.deleteAllCalipersToolStripMenuItem,
            this.toolStripSeparator4,
            this.calibrateToolStripMenuItem,
            this.clearCalibrationToolStripMenuItem,
            this.toolStripSeparator5,
            this.toggleRateintervalToolStripMenuItem,
            this.meanRateIntervalToolStripMenuItem,
            this.qTcMeasurementToolStripMenuItem});
			this.calipersToolStripMenuItem.Name = "calipersToolStripMenuItem";
			this.calipersToolStripMenuItem.Size = new System.Drawing.Size(125, 40);
			this.calipersToolStripMenuItem.Text = "Calipers";
			// 
			// timeCaliperToolStripMenuItem
			// 
			this.timeCaliperToolStripMenuItem.Name = "timeCaliperToolStripMenuItem";
			this.timeCaliperToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
			this.timeCaliperToolStripMenuItem.Size = new System.Drawing.Size(472, 44);
			this.timeCaliperToolStripMenuItem.Text = "Time caliper";
			this.timeCaliperToolStripMenuItem.Click += new System.EventHandler(this.TimeCaliperToolStripMenuItem_Click);
			// 
			// amplitudeCaliperToolStripMenuItem
			// 
			this.amplitudeCaliperToolStripMenuItem.Name = "amplitudeCaliperToolStripMenuItem";
			this.amplitudeCaliperToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.amplitudeCaliperToolStripMenuItem.Size = new System.Drawing.Size(472, 44);
			this.amplitudeCaliperToolStripMenuItem.Text = "Amplitude caliper";
			this.amplitudeCaliperToolStripMenuItem.Click += new System.EventHandler(this.AmplitudeCaliperToolStripMenuItem_Click);
			// 
			// angleCaliperToolStripMenuItem
			// 
			this.angleCaliperToolStripMenuItem.Name = "angleCaliperToolStripMenuItem";
			this.angleCaliperToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.angleCaliperToolStripMenuItem.Size = new System.Drawing.Size(472, 44);
			this.angleCaliperToolStripMenuItem.Text = "Angle caliper";
			this.angleCaliperToolStripMenuItem.Click += new System.EventHandler(this.AngleCaliperToolStripMenuItem_Click);
			// 
			// deleteCaliperToolStripMenuItem
			// 
			this.deleteCaliperToolStripMenuItem.Name = "deleteCaliperToolStripMenuItem";
			this.deleteCaliperToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.deleteCaliperToolStripMenuItem.Size = new System.Drawing.Size(472, 44);
			this.deleteCaliperToolStripMenuItem.Text = "Delete caliper";
			this.deleteCaliperToolStripMenuItem.Click += new System.EventHandler(this.DeleteCaliperToolStripMenuItem_Click);
			// 
			// deleteAllCalipersToolStripMenuItem
			// 
			this.deleteAllCalipersToolStripMenuItem.Name = "deleteAllCalipersToolStripMenuItem";
			this.deleteAllCalipersToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
			this.deleteAllCalipersToolStripMenuItem.Size = new System.Drawing.Size(472, 44);
			this.deleteAllCalipersToolStripMenuItem.Text = "Delete all calipers";
			this.deleteAllCalipersToolStripMenuItem.Click += new System.EventHandler(this.DeleteAllCalipersToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(469, 6);
			// 
			// calibrateToolStripMenuItem
			// 
			this.calibrateToolStripMenuItem.Name = "calibrateToolStripMenuItem";
			this.calibrateToolStripMenuItem.Size = new System.Drawing.Size(472, 44);
			this.calibrateToolStripMenuItem.Text = "Set calibration";
			this.calibrateToolStripMenuItem.Click += new System.EventHandler(this.CalibrateToolStripMenuItem_Click);
			// 
			// clearCalibrationToolStripMenuItem
			// 
			this.clearCalibrationToolStripMenuItem.Name = "clearCalibrationToolStripMenuItem";
			this.clearCalibrationToolStripMenuItem.Size = new System.Drawing.Size(472, 44);
			this.clearCalibrationToolStripMenuItem.Text = "Clear calibration";
			this.clearCalibrationToolStripMenuItem.Click += new System.EventHandler(this.ClearCalibrationToolStripMenuItem_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(469, 6);
			// 
			// toggleRateintervalToolStripMenuItem
			// 
			this.toggleRateintervalToolStripMenuItem.Name = "toggleRateintervalToolStripMenuItem";
			this.toggleRateintervalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
			this.toggleRateintervalToolStripMenuItem.Size = new System.Drawing.Size(472, 44);
			this.toggleRateintervalToolStripMenuItem.Text = "Toggle rate/interval";
			this.toggleRateintervalToolStripMenuItem.Click += new System.EventHandler(this.ToggleRateintervalToolStripMenuItem_Click);
			// 
			// meanRateIntervalToolStripMenuItem
			// 
			this.meanRateIntervalToolStripMenuItem.Name = "meanRateIntervalToolStripMenuItem";
			this.meanRateIntervalToolStripMenuItem.Size = new System.Drawing.Size(472, 44);
			this.meanRateIntervalToolStripMenuItem.Text = "Mean rate interval";
			this.meanRateIntervalToolStripMenuItem.Click += new System.EventHandler(this.MeanRateIntervalToolStripMenuItem_Click);
			// 
			// qTcMeasurementToolStripMenuItem
			// 
			this.qTcMeasurementToolStripMenuItem.Name = "qTcMeasurementToolStripMenuItem";
			this.qTcMeasurementToolStripMenuItem.Size = new System.Drawing.Size(472, 44);
			this.qTcMeasurementToolStripMenuItem.Text = "QTc measurement";
			this.qTcMeasurementToolStripMenuItem.Click += new System.EventHandler(this.QTcMeasurementToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.transparentWindowToolStripMenuItem1,
            this.toolStripSeparator7,
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(93, 40);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// transparentWindowToolStripMenuItem1
			// 
			this.transparentWindowToolStripMenuItem1.Name = "transparentWindowToolStripMenuItem1";
			this.transparentWindowToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
			this.transparentWindowToolStripMenuItem1.Size = new System.Drawing.Size(464, 44);
			this.transparentWindowToolStripMenuItem1.Text = "Transparent Window";
			this.transparentWindowToolStripMenuItem1.Click += new System.EventHandler(this.TransparentWindowToolStripMenuItem1_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(461, 6);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(464, 44);
			this.optionsToolStripMenuItem.Text = "Options...";
			this.optionsToolStripMenuItem.ToolTipText = "Change options";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewHelpToolStripMenuItem,
            this.toolStripSeparator1,
            this.aboutEPCalipersToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(88, 40);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// viewHelpToolStripMenuItem
			// 
			this.viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
			this.viewHelpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.viewHelpToolStripMenuItem.Size = new System.Drawing.Size(373, 44);
			this.viewHelpToolStripMenuItem.Text = "EP Calipers help";
			this.viewHelpToolStripMenuItem.Click += new System.EventHandler(this.ViewHelpToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(370, 6);
			// 
			// aboutEPCalipersToolStripMenuItem
			// 
			this.aboutEPCalipersToolStripMenuItem.Name = "aboutEPCalipersToolStripMenuItem";
			this.aboutEPCalipersToolStripMenuItem.Size = new System.Drawing.Size(373, 44);
			this.aboutEPCalipersToolStripMenuItem.Text = "About EP Calipers";
			this.aboutEPCalipersToolStripMenuItem.Click += new System.EventHandler(this.AboutEPCalipersToolStripMenuItem_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(218, 6);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.caliperColorToolStripMenuItem,
            this.tweakToolStripMenuItem,
            this.marchingCaliperToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(347, 174);
			this.contextMenuStrip1.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.contextMenuStrip1_Closed);
			// 
			// caliperColorToolStripMenuItem
			// 
			this.caliperColorToolStripMenuItem.Name = "caliperColorToolStripMenuItem";
			this.caliperColorToolStripMenuItem.Size = new System.Drawing.Size(346, 42);
			this.caliperColorToolStripMenuItem.Text = "Caliper Color";
			this.caliperColorToolStripMenuItem.Click += new System.EventHandler(this.CaliperColorToolStripMenuItem_Click);
			// 
			// tweakToolStripMenuItem
			// 
			this.tweakToolStripMenuItem.Name = "tweakToolStripMenuItem";
			this.tweakToolStripMenuItem.Size = new System.Drawing.Size(346, 42);
			this.tweakToolStripMenuItem.Text = "Tweak Caliper Position";
			this.tweakToolStripMenuItem.Click += new System.EventHandler(this.TweakToolStripMenuItem_Click);
			// 
			// marchingCaliperToolStripMenuItem
			// 
			this.marchingCaliperToolStripMenuItem.Name = "marchingCaliperToolStripMenuItem";
			this.marchingCaliperToolStripMenuItem.Size = new System.Drawing.Size(346, 42);
			this.marchingCaliperToolStripMenuItem.Text = "Marching Caliper";
			this.marchingCaliperToolStripMenuItem.Click += new System.EventHandler(this.MarchingCaliperToolStripMenuItem_Click);
			// 
			// Form1
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(1376, 927);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(6);
			this.Name = "Form1";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "EP Calipers";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
			this.DragLeave += new System.EventHandler(this.OnDragLeave);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ecgPictureBox)).EndInit();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox ecgPictureBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripSeparator quitToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem1;
        private SaveFileDialog saveFileDialog1;
        private PageSetupDialog pageSetupDialog1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button zoomInButton;
        private Button zoomOutButton;
        private ToolTip toolTip1;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        protected PictureBox thumbnail;
        private ToolStripMenuItem calipersToolStripMenuItem;
        private ToolStripMenuItem zoomToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutEPCalipersToolStripMenuItem;
        private ToolStripMenuItem viewHelpToolStripMenuItem;
        private ToolStripMenuItem zoomInToolStripMenuItem;
        private ToolStripMenuItem zoomOutToolStripMenuItem;
        private ToolStripMenuItem resetZoomToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem rotate90RToolStripMenuItem;
        private ToolStripMenuItem rotate90LToolStripMenuItem;
        private ToolStripMenuItem rotate1RToolStripMenuItem;
        private ToolStripMenuItem rotate1LToolStripMenuItem;
        private ToolStripMenuItem resetImageToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem nextPageToolStripMenuItem1;
        private ToolStripMenuItem previousPageToolStripMenuItem1;
        private ToolStripMenuItem timeCaliperToolStripMenuItem;
        private ToolStripMenuItem amplitudeCaliperToolStripMenuItem;
        private ToolStripMenuItem deleteCaliperToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem calibrateToolStripMenuItem;
        private ToolStripMenuItem clearCalibrationToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem toggleRateintervalToolStripMenuItem;
        private ToolStripMenuItem meanRateIntervalToolStripMenuItem;
        private ToolStripMenuItem qTcMeasurementToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem rotateTinyRToolStripMenuItem;
        private ToolStripMenuItem rotateTinyLToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem angleCaliperToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem caliperColorToolStripMenuItem;
        private ToolStripMenuItem tweakToolStripMenuItem;
        private ToolStripMenuItem deleteAllCalipersToolStripMenuItem;
        private ToolStripMenuItem marchingCaliperToolStripMenuItem;
        private ToolStripMenuItem transparentWindowToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem gotoPDFPageToolStripMenuItem;
    }
}

