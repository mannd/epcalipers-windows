﻿namespace EPCalipersCore
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
			this.angleCaliperRadioButton = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// horizontalCaliperRadioButton
			// 
			this.horizontalCaliperRadioButton.AutoSize = true;
			this.horizontalCaliperRadioButton.Checked = true;
			this.horizontalCaliperRadioButton.Location = new System.Drawing.Point(34, 37);
			this.horizontalCaliperRadioButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.horizontalCaliperRadioButton.Name = "horizontalCaliperRadioButton";
			this.horizontalCaliperRadioButton.Size = new System.Drawing.Size(281, 29);
			this.horizontalCaliperRadioButton.TabIndex = 0;
			this.horizontalCaliperRadioButton.TabStop = true;
			this.horizontalCaliperRadioButton.Text = "Time Caliper (Horizontal)";
			this.horizontalCaliperRadioButton.UseVisualStyleBackColor = true;
			// 
			// VerticalCaliperRadioButton
			// 
			this.VerticalCaliperRadioButton.AutoSize = true;
			this.VerticalCaliperRadioButton.Location = new System.Drawing.Point(34, 85);
			this.VerticalCaliperRadioButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.VerticalCaliperRadioButton.Name = "VerticalCaliperRadioButton";
			this.VerticalCaliperRadioButton.Size = new System.Drawing.Size(304, 29);
			this.VerticalCaliperRadioButton.TabIndex = 1;
			this.VerticalCaliperRadioButton.Text = "Amplitude Caliper (Vertical)";
			this.VerticalCaliperRadioButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(364, 369);
			this.okButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(150, 44);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(202, 369);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(150, 44);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.angleCaliperRadioButton);
			this.groupBox1.Controls.Add(this.horizontalCaliperRadioButton);
			this.groupBox1.Controls.Add(this.VerticalCaliperRadioButton);
			this.groupBox1.Location = new System.Drawing.Point(54, 87);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.groupBox1.Size = new System.Drawing.Size(400, 196);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Caliper Direction";
			// 
			// angleCaliperRadioButton
			// 
			this.angleCaliperRadioButton.AutoSize = true;
			this.angleCaliperRadioButton.Location = new System.Drawing.Point(34, 133);
			this.angleCaliperRadioButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.angleCaliperRadioButton.Name = "angleCaliperRadioButton";
			this.angleCaliperRadioButton.Size = new System.Drawing.Size(172, 29);
			this.angleCaliperRadioButton.TabIndex = 2;
			this.angleCaliperRadioButton.TabStop = true;
			this.angleCaliperRadioButton.Text = "Angle Caliper";
			this.angleCaliperRadioButton.UseVisualStyleBackColor = true;
			// 
			// NewCaliperDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(568, 502);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
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
        public System.Windows.Forms.RadioButton angleCaliperRadioButton;
    }
}