namespace epcalipers
{
    partial class MeasureDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.meanIntervalRateButton = new System.Windows.Forms.RadioButton();
            this.qtcButton = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.qtcButton);
            this.groupBox1.Controls.Add(this.meanIntervalRateButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 69);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose Measurement";
            // 
            // meanIntervalRateButton
            // 
            this.meanIntervalRateButton.AutoSize = true;
            this.meanIntervalRateButton.Location = new System.Drawing.Point(6, 19);
            this.meanIntervalRateButton.Name = "meanIntervalRateButton";
            this.meanIntervalRateButton.Size = new System.Drawing.Size(112, 17);
            this.meanIntervalRateButton.TabIndex = 0;
            this.meanIntervalRateButton.TabStop = true;
            this.meanIntervalRateButton.Text = "Mean interval/rate";
            this.meanIntervalRateButton.UseVisualStyleBackColor = true;
            // 
            // qtcButton
            // 
            this.qtcButton.AutoSize = true;
            this.qtcButton.Location = new System.Drawing.Point(6, 42);
            this.qtcButton.Name = "qtcButton";
            this.qtcButton.Size = new System.Drawing.Size(122, 17);
            this.qtcButton.TabIndex = 1;
            this.qtcButton.TabStop = true;
            this.qtcButton.Text = "QTc (Bazett formula)";
            this.qtcButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(197, 226);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(116, 226);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // MeasureDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "MeasureDialog";
            this.Text = "Measure";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton qtcButton;
        public System.Windows.Forms.RadioButton meanIntervalRateButton;
        public System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}