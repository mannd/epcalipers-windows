namespace epcalipers
{
    partial class NewCaliperDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewCaliperDialog));
            this.horizontalCaliperRadioButton = new System.Windows.Forms.RadioButton();
            this.VerticalCaliperRadioButton = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // horizontalCaliperRadioButton
            // 
            this.horizontalCaliperRadioButton.AutoSize = true;
            this.horizontalCaliperRadioButton.Checked = true;
            this.horizontalCaliperRadioButton.Location = new System.Drawing.Point(17, 19);
            this.horizontalCaliperRadioButton.Name = "horizontalCaliperRadioButton";
            this.horizontalCaliperRadioButton.Size = new System.Drawing.Size(139, 17);
            this.horizontalCaliperRadioButton.TabIndex = 0;
            this.horizontalCaliperRadioButton.TabStop = true;
            this.horizontalCaliperRadioButton.Text = "Time Caliper (Horizontal)";
            this.horizontalCaliperRadioButton.UseVisualStyleBackColor = true;
            // 
            // VerticalCaliperRadioButton
            // 
            this.VerticalCaliperRadioButton.AutoSize = true;
            this.VerticalCaliperRadioButton.Location = new System.Drawing.Point(17, 44);
            this.VerticalCaliperRadioButton.Name = "VerticalCaliperRadioButton";
            this.VerticalCaliperRadioButton.Size = new System.Drawing.Size(150, 17);
            this.VerticalCaliperRadioButton.TabIndex = 1;
            this.VerticalCaliperRadioButton.Text = "Amplitude Caliper (Vertical)";
            this.VerticalCaliperRadioButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(197, 226);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(116, 226);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.horizontalCaliperRadioButton);
            this.groupBox1.Controls.Add(this.VerticalCaliperRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(38, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 102);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Caliper Direction";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(17, 69);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(87, 17);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Angle Caliper";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // NewCaliperDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewCaliperDialog";
            this.Text = "Add Caliper";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton horizontalCaliperRadioButton;
        public System.Windows.Forms.RadioButton VerticalCaliperRadioButton;
        public System.Windows.Forms.RadioButton radioButton1;
    }
}