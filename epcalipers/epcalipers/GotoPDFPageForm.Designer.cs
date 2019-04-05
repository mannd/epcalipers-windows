namespace epcalipers
{
    partial class GotoPDFPageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GotoPDFPageForm));
            this.label1 = new System.Windows.Forms.Label();
            this.cancelGotoPageButton = new System.Windows.Forms.Button();
            this.okGotoPageButton = new System.Windows.Forms.Button();
            this.pdfPageUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pdfPageUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Go to PDF page...";
            // 
            // cancelGotoPageButton
            // 
            this.cancelGotoPageButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelGotoPageButton.Location = new System.Drawing.Point(100, 148);
            this.cancelGotoPageButton.Name = "cancelGotoPageButton";
            this.cancelGotoPageButton.Size = new System.Drawing.Size(75, 23);
            this.cancelGotoPageButton.TabIndex = 1;
            this.cancelGotoPageButton.Text = "Cancel";
            this.cancelGotoPageButton.UseVisualStyleBackColor = true;
            // 
            // okGotoPageButton
            // 
            this.okGotoPageButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okGotoPageButton.Location = new System.Drawing.Point(181, 148);
            this.okGotoPageButton.Name = "okGotoPageButton";
            this.okGotoPageButton.Size = new System.Drawing.Size(75, 23);
            this.okGotoPageButton.TabIndex = 2;
            this.okGotoPageButton.Text = "OK";
            this.okGotoPageButton.UseVisualStyleBackColor = true;
            // 
            // pdfPageUpDown
            // 
            this.pdfPageUpDown.Location = new System.Drawing.Point(100, 90);
            this.pdfPageUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pdfPageUpDown.Name = "pdfPageUpDown";
            this.pdfPageUpDown.Size = new System.Drawing.Size(120, 20);
            this.pdfPageUpDown.TabIndex = 3;
            this.pdfPageUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // GotoPDFPageForm
            // 
            this.AcceptButton = this.okGotoPageButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelGotoPageButton;
            this.ClientSize = new System.Drawing.Size(353, 223);
            this.Controls.Add(this.pdfPageUpDown);
            this.Controls.Add(this.okGotoPageButton);
            this.Controls.Add(this.cancelGotoPageButton);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GotoPDFPageForm";
            this.Text = "Go to PDF page";
            this.Load += new System.EventHandler(this.GotoPDFPageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pdfPageUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelGotoPageButton;
        private System.Windows.Forms.Button okGotoPageButton;
        public System.Windows.Forms.NumericUpDown pdfPageUpDown;
    }
}