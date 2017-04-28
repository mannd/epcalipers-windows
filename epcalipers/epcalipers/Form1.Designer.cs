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

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (theBitmap != null) theBitmap.Dispose();
                if (pdfImages != null) pdfImages.Dispose();
                if (nextImage != null) nextImage.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

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
            this.calipersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeCaliperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.amplitudeCaliperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.angleCaliperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCaliperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.calibrateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCalibrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleRateintervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.meanRateIntervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qTcMeasurementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.showHandlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transparentWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutEPCalipersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.caliperColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(688, 458);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(81, 424);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(604, 31);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.thumbnail);
            this.panel1.Controls.Add(this.ecgPictureBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(682, 415);
            this.panel1.TabIndex = 3;
            // 
            // thumbnail
            // 
            this.thumbnail.Location = new System.Drawing.Point(0, 0);
            this.thumbnail.Name = "thumbnail";
            this.thumbnail.Size = new System.Drawing.Size(100, 50);
            this.thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.thumbnail.TabIndex = 1;
            this.thumbnail.TabStop = false;
            this.thumbnail.Visible = false;
            // 
            // ecgPictureBox
            // 
            this.ecgPictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.ecgPictureBox.Location = new System.Drawing.Point(0, 0);
            this.ecgPictureBox.Name = "ecgPictureBox";
            this.ecgPictureBox.Size = new System.Drawing.Size(682, 415);
            this.ecgPictureBox.TabIndex = 0;
            this.ecgPictureBox.TabStop = false;
            this.ecgPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ecgPictureBox_MouseMove);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel2.Controls.Add(this.zoomInButton);
            this.flowLayoutPanel2.Controls.Add(this.zoomOutButton);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 424);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(72, 31);
            this.flowLayoutPanel2.TabIndex = 4;
            // 
            // zoomInButton
            // 
            this.zoomInButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.zoomInButton.Location = new System.Drawing.Point(3, 3);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Size = new System.Drawing.Size(23, 23);
            this.zoomInButton.TabIndex = 0;
            this.zoomInButton.Text = "+";
            this.toolTip1.SetToolTip(this.zoomInButton, "Zoom in");
            this.zoomInButton.UseVisualStyleBackColor = true;
            this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.zoomOutButton.Location = new System.Drawing.Point(32, 3);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Size = new System.Drawing.Size(23, 23);
            this.zoomOutButton.TabIndex = 1;
            this.zoomOutButton.Text = "-";
            this.toolTip1.SetToolTip(this.zoomOutButton, "Zoom out");
            this.zoomOutButton.UseVisualStyleBackColor = true;
            this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.zoomToolStripMenuItem,
            this.calipersToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(688, 24);
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.imageButton_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(143, 6);
            // 
            // quitToolStripMenuItem1
            // 
            this.quitToolStripMenuItem1.Name = "quitToolStripMenuItem1";
            this.quitToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.quitToolStripMenuItem1.Text = "Quit";
            this.quitToolStripMenuItem1.Click += new System.EventHandler(this.quitToolStripMenuItem1_Click);
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
            this.previousPageToolStripMenuItem1});
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)));
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.zoomToolStripMenuItem.Text = "Image";
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
            this.zoomInToolStripMenuItem.ShowShortcutKeys = false;
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.zoomInToolStripMenuItem.Text = "Zoom in";
            this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
            this.zoomOutToolStripMenuItem.ShowShortcutKeys = false;
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.zoomOutToolStripMenuItem.Text = "Zoom out";
            this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // resetZoomToolStripMenuItem
            // 
            this.resetZoomToolStripMenuItem.Name = "resetZoomToolStripMenuItem";
            this.resetZoomToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.resetZoomToolStripMenuItem.Text = "Reset zoom";
            this.resetZoomToolStripMenuItem.Click += new System.EventHandler(this.resetZoomToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(208, 6);
            // 
            // rotate90RToolStripMenuItem
            // 
            this.rotate90RToolStripMenuItem.Name = "rotate90RToolStripMenuItem";
            this.rotate90RToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.rotate90RToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.rotate90RToolStripMenuItem.Text = "Rotate 90° R";
            this.rotate90RToolStripMenuItem.Click += new System.EventHandler(this.rotate90RToolStripMenuItem_Click_1);
            // 
            // rotate90LToolStripMenuItem
            // 
            this.rotate90LToolStripMenuItem.Name = "rotate90LToolStripMenuItem";
            this.rotate90LToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.L)));
            this.rotate90LToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.rotate90LToolStripMenuItem.Text = "Rotate 90° L";
            this.rotate90LToolStripMenuItem.Click += new System.EventHandler(this.rotate90LToolStripMenuItem_Click);
            // 
            // rotate1RToolStripMenuItem
            // 
            this.rotate1RToolStripMenuItem.Name = "rotate1RToolStripMenuItem";
            this.rotate1RToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.rotate1RToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.rotate1RToolStripMenuItem.Text = "Rotate 1°R";
            this.rotate1RToolStripMenuItem.Click += new System.EventHandler(this.rotate1RToolStripMenuItem_Click);
            // 
            // rotate1LToolStripMenuItem
            // 
            this.rotate1LToolStripMenuItem.Name = "rotate1LToolStripMenuItem";
            this.rotate1LToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.rotate1LToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.rotate1LToolStripMenuItem.Text = "Rotate 1°L";
            this.rotate1LToolStripMenuItem.Click += new System.EventHandler(this.rotate1LToolStripMenuItem_Click);
            // 
            // rotateTinyRToolStripMenuItem
            // 
            this.rotateTinyRToolStripMenuItem.Name = "rotateTinyRToolStripMenuItem";
            this.rotateTinyRToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.R)));
            this.rotateTinyRToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.rotateTinyRToolStripMenuItem.Text = "Rotate 0.1°R";
            this.rotateTinyRToolStripMenuItem.Click += new System.EventHandler(this.rotateTinyRToolStripMenuItem_Click);
            // 
            // rotateTinyLToolStripMenuItem
            // 
            this.rotateTinyLToolStripMenuItem.Name = "rotateTinyLToolStripMenuItem";
            this.rotateTinyLToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.L)));
            this.rotateTinyLToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.rotateTinyLToolStripMenuItem.Text = "Rotate 0.1°L";
            this.rotateTinyLToolStripMenuItem.Click += new System.EventHandler(this.rotateTinyLToolStripMenuItem_Click);
            // 
            // resetImageToolStripMenuItem1
            // 
            this.resetImageToolStripMenuItem1.Name = "resetImageToolStripMenuItem1";
            this.resetImageToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0)));
            this.resetImageToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.resetImageToolStripMenuItem1.Text = "Reset image";
            this.resetImageToolStripMenuItem1.Click += new System.EventHandler(this.resetImageToolStripMenuItem1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(208, 6);
            // 
            // nextPageToolStripMenuItem1
            // 
            this.nextPageToolStripMenuItem1.Name = "nextPageToolStripMenuItem1";
            this.nextPageToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)));
            this.nextPageToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.nextPageToolStripMenuItem1.Text = "Next page";
            this.nextPageToolStripMenuItem1.Click += new System.EventHandler(this.nextPageToolStripMenuItem1_Click);
            // 
            // previousPageToolStripMenuItem1
            // 
            this.previousPageToolStripMenuItem1.Name = "previousPageToolStripMenuItem1";
            this.previousPageToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)));
            this.previousPageToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.previousPageToolStripMenuItem1.Text = "Previous page";
            this.previousPageToolStripMenuItem1.Click += new System.EventHandler(this.previousPageToolStripMenuItem1_Click);
            // 
            // calipersToolStripMenuItem
            // 
            this.calipersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeCaliperToolStripMenuItem,
            this.amplitudeCaliperToolStripMenuItem,
            this.angleCaliperToolStripMenuItem,
            this.deleteCaliperToolStripMenuItem,
            this.toolStripSeparator4,
            this.calibrateToolStripMenuItem,
            this.clearCalibrationToolStripMenuItem,
            this.toolStripSeparator5,
            this.toggleRateintervalToolStripMenuItem,
            this.meanRateIntervalToolStripMenuItem,
            this.qTcMeasurementToolStripMenuItem,
            this.toolStripSeparator6,
            this.showHandlesToolStripMenuItem});
            this.calipersToolStripMenuItem.Name = "calipersToolStripMenuItem";
            this.calipersToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.calipersToolStripMenuItem.Text = "Calipers";
            // 
            // timeCaliperToolStripMenuItem
            // 
            this.timeCaliperToolStripMenuItem.Name = "timeCaliperToolStripMenuItem";
            this.timeCaliperToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.timeCaliperToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.timeCaliperToolStripMenuItem.Text = "Time caliper";
            this.timeCaliperToolStripMenuItem.Click += new System.EventHandler(this.timeCaliperToolStripMenuItem_Click);
            // 
            // amplitudeCaliperToolStripMenuItem
            // 
            this.amplitudeCaliperToolStripMenuItem.Name = "amplitudeCaliperToolStripMenuItem";
            this.amplitudeCaliperToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.amplitudeCaliperToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.amplitudeCaliperToolStripMenuItem.Text = "Amplitude caliper";
            this.amplitudeCaliperToolStripMenuItem.Click += new System.EventHandler(this.amplitudeCaliperToolStripMenuItem_Click);
            // 
            // angleCaliperToolStripMenuItem
            // 
            this.angleCaliperToolStripMenuItem.Name = "angleCaliperToolStripMenuItem";
            this.angleCaliperToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.angleCaliperToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.angleCaliperToolStripMenuItem.Text = "Angle caliper";
            this.angleCaliperToolStripMenuItem.Click += new System.EventHandler(this.angleCaliperToolStripMenuItem_Click);
            // 
            // deleteCaliperToolStripMenuItem
            // 
            this.deleteCaliperToolStripMenuItem.Name = "deleteCaliperToolStripMenuItem";
            this.deleteCaliperToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteCaliperToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.deleteCaliperToolStripMenuItem.Text = "Delete caliper";
            this.deleteCaliperToolStripMenuItem.Click += new System.EventHandler(this.deleteCaliperToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(211, 6);
            // 
            // calibrateToolStripMenuItem
            // 
            this.calibrateToolStripMenuItem.Name = "calibrateToolStripMenuItem";
            this.calibrateToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.calibrateToolStripMenuItem.Text = "Calibrate";
            this.calibrateToolStripMenuItem.Click += new System.EventHandler(this.calibrateToolStripMenuItem_Click);
            // 
            // clearCalibrationToolStripMenuItem
            // 
            this.clearCalibrationToolStripMenuItem.Name = "clearCalibrationToolStripMenuItem";
            this.clearCalibrationToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.clearCalibrationToolStripMenuItem.Text = "Clear calibration";
            this.clearCalibrationToolStripMenuItem.Click += new System.EventHandler(this.clearCalibrationToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(211, 6);
            // 
            // toggleRateintervalToolStripMenuItem
            // 
            this.toggleRateintervalToolStripMenuItem.Name = "toggleRateintervalToolStripMenuItem";
            this.toggleRateintervalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.toggleRateintervalToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.toggleRateintervalToolStripMenuItem.Text = "Toggle rate/interval";
            this.toggleRateintervalToolStripMenuItem.Click += new System.EventHandler(this.toggleRateintervalToolStripMenuItem_Click);
            // 
            // meanRateIntervalToolStripMenuItem
            // 
            this.meanRateIntervalToolStripMenuItem.Name = "meanRateIntervalToolStripMenuItem";
            this.meanRateIntervalToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.meanRateIntervalToolStripMenuItem.Text = "Mean rate interval";
            this.meanRateIntervalToolStripMenuItem.Click += new System.EventHandler(this.meanRateIntervalToolStripMenuItem_Click);
            // 
            // qTcMeasurementToolStripMenuItem
            // 
            this.qTcMeasurementToolStripMenuItem.Name = "qTcMeasurementToolStripMenuItem";
            this.qTcMeasurementToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.qTcMeasurementToolStripMenuItem.Text = "QTc measurement";
            this.qTcMeasurementToolStripMenuItem.Click += new System.EventHandler(this.qTcMeasurementToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(211, 6);
            // 
            // showHandlesToolStripMenuItem
            // 
            this.showHandlesToolStripMenuItem.CheckOnClick = true;
            this.showHandlesToolStripMenuItem.Name = "showHandlesToolStripMenuItem";
            this.showHandlesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.showHandlesToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.showHandlesToolStripMenuItem.Text = "Show handles";
            this.showHandlesToolStripMenuItem.Click += new System.EventHandler(this.showHandlesToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.ToolTipText = "Change options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.transparentWindowToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // transparentWindowToolStripMenuItem
            // 
            this.transparentWindowToolStripMenuItem.CheckOnClick = true;
            this.transparentWindowToolStripMenuItem.Name = "transparentWindowToolStripMenuItem";
            this.transparentWindowToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.transparentWindowToolStripMenuItem.Text = "Transparent window";
            this.transparentWindowToolStripMenuItem.Click += new System.EventHandler(this.transparentWindowToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewHelpToolStripMenuItem,
            this.toolStripSeparator1,
            this.aboutEPCalipersToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // viewHelpToolStripMenuItem
            // 
            this.viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
            this.viewHelpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.viewHelpToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.viewHelpToolStripMenuItem.Text = "EP Calipers help";
            this.viewHelpToolStripMenuItem.Click += new System.EventHandler(this.viewHelpToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(174, 6);
            // 
            // aboutEPCalipersToolStripMenuItem
            // 
            this.aboutEPCalipersToolStripMenuItem.Name = "aboutEPCalipersToolStripMenuItem";
            this.aboutEPCalipersToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.aboutEPCalipersToolStripMenuItem.Text = "About EP Calipers";
            this.aboutEPCalipersToolStripMenuItem.Click += new System.EventHandler(this.aboutEPCalipersToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.caliperColorToolStripMenuItem,
            this.tToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(194, 48);
            // 
            // caliperColorToolStripMenuItem
            // 
            this.caliperColorToolStripMenuItem.Name = "caliperColorToolStripMenuItem";
            this.caliperColorToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.caliperColorToolStripMenuItem.Text = "Caliper Color";
            // 
            // tToolStripMenuItem
            // 
            this.tToolStripMenuItem.Name = "tToolStripMenuItem";
            this.tToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.tToolStripMenuItem.Text = "Tweak Caliper Position";
            this.tToolStripMenuItem.Click += new System.EventHandler(this.tToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 482);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "EP Calipers";
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
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem transparentWindowToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem showHandlesToolStripMenuItem;
        private ToolStripMenuItem angleCaliperToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem caliperColorToolStripMenuItem;
        private ToolStripMenuItem tToolStripMenuItem;
    }
}

