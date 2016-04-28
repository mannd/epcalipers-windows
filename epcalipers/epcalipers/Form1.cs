using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epcalipers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);

        }

        private void imageButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("image button pushed");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
            }
        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("calibrate button pushed");

        }

        private void addCaliper_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("addCaliper button pushed");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("click");

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Debug.WriteLine("double click");
        }

        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Update the mouse path with the mouse information
            Point mouseDownLocation = new Point(e.X, e.Y);
            Debug.WriteLine("mouse down {0}, {1}", e.X, e.Y);

        }
    }
}
