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
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.addButton = new System.Windows.Forms.Button();
            this.calibrateButton = new System.Windows.Forms.Button();
            this.intervalRateButton = new System.Windows.Forms.Button();
            this.meanRRButton = new System.Windows.Forms.Button();
            this.qtcButton = new System.Windows.Forms.Button();
            this.imageButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(688, 482);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(682, 427);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.addButton);
            this.flowLayoutPanel1.Controls.Add(this.calibrateButton);
            this.flowLayoutPanel1.Controls.Add(this.intervalRateButton);
            this.flowLayoutPanel1.Controls.Add(this.meanRRButton);
            this.flowLayoutPanel1.Controls.Add(this.qtcButton);
            this.flowLayoutPanel1.Controls.Add(this.imageButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 436);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(682, 43);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // addButton
            // 
            this.addButton.AutoSize = true;
            this.addButton.Location = new System.Drawing.Point(3, 3);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Add Caliper";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // calibrateButton
            // 
            this.calibrateButton.AutoSize = true;
            this.calibrateButton.Location = new System.Drawing.Point(84, 3);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(75, 23);
            this.calibrateButton.TabIndex = 1;
            this.calibrateButton.Text = "Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = true;
            // 
            // intervalRateButton
            // 
            this.intervalRateButton.AutoSize = true;
            this.intervalRateButton.Location = new System.Drawing.Point(165, 3);
            this.intervalRateButton.Name = "intervalRateButton";
            this.intervalRateButton.Size = new System.Drawing.Size(80, 23);
            this.intervalRateButton.TabIndex = 2;
            this.intervalRateButton.Text = "Interval/Rate";
            this.intervalRateButton.UseVisualStyleBackColor = true;
            // 
            // meanRRButton
            // 
            this.meanRRButton.AutoSize = true;
            this.meanRRButton.Location = new System.Drawing.Point(251, 3);
            this.meanRRButton.Name = "meanRRButton";
            this.meanRRButton.Size = new System.Drawing.Size(75, 23);
            this.meanRRButton.TabIndex = 3;
            this.meanRRButton.Text = "Mean RR";
            this.meanRRButton.UseVisualStyleBackColor = true;
            // 
            // qtcButton
            // 
            this.qtcButton.AutoSize = true;
            this.qtcButton.Location = new System.Drawing.Point(332, 3);
            this.qtcButton.Name = "qtcButton";
            this.qtcButton.Size = new System.Drawing.Size(75, 23);
            this.qtcButton.TabIndex = 4;
            this.qtcButton.Text = "QTc";
            this.qtcButton.UseVisualStyleBackColor = true;
            // 
            // imageButton
            // 
            this.imageButton.Location = new System.Drawing.Point(413, 3);
            this.imageButton.Name = "imageButton";
            this.imageButton.Size = new System.Drawing.Size(75, 23);
            this.imageButton.TabIndex = 5;
            this.imageButton.Text = "Image";
            this.imageButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 482);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "EP Calipers";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.Button intervalRateButton;
        private System.Windows.Forms.Button meanRRButton;
        private System.Windows.Forms.Button qtcButton;
        private System.Windows.Forms.Button imageButton;
    }
}

