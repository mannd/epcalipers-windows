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
    }
}
