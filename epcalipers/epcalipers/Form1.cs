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

        Bitmap theBitmap;
        Calipers theCalipers;
        Button imageButton;
        Button addCalipersButton;
        Button calibrateButton;
        Button intervalRateButton;
        Button measureButton;

        Point firstPoint;

        public Form1()
        {
            InitializeComponent();
            theCalipers = new Calipers();
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            pictureBox1.MouseDoubleClick += pictureBox1_MouseDoubleClick;
            SetupButtons();
            ShowMainMenu();
        }

        private void SetupButtons()
        {
            imageButton = new Button();
            imageButton.Text = "Image";
            imageButton.Click += imageButton_Click;
            addCalipersButton = new Button();
            addCalipersButton.Text = "Add Calipers";
            addCalipersButton.Click += addCaliper_Click;
            calibrateButton = new Button();
            calibrateButton.Text = "Calibrate";
            calibrateButton.Click += calibrateButton_Click;
        
            // other buttons here
        }

        private void ShowMainMenu()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.AddRange(new Control[]{ imageButton, addCalipersButton, calibrateButton });
            if (theBitmap == null)
            {
                addCalipersButton.Enabled = false;
                calibrateButton.Enabled = false;
            }
        }

        private void imageButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("image button pushed");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                theBitmap = new Bitmap(pictureBox1.Image);
                trackBar1.Value = 1;
                addCalipersButton.Enabled = true;
                calibrateButton.Enabled = true;
            }
        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("calibrate button pushed");

        }

        private void addCaliper_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null )
            {
                return;
            }
            NewCaliperDialog dialog = new NewCaliperDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                CaliperDirection direction;
                if (dialog.horizontalCaliperRadioButton.Checked)
                {
                    direction = CaliperDirection.Horizontal;
                }
                else
                {
                    direction = CaliperDirection.Vertical;
                }
                AddCaliper(direction);
            }
        }

         private void AddCaliper(CaliperDirection direction)
        {
            Caliper c = new Caliper();
            c.Direction = direction;
            c.SetInitialPositionInRect(pictureBox1.DisplayRectangle);
            theCalipers.addCaliper(c);
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("double click");
            Point mouseClickLocation = new Point(e.X, e.Y);
            if (theCalipers.DeleteCaliperIfClicked(mouseClickLocation))
            {
                pictureBox1.Refresh();
            }
        }

         private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Update the mouse path with the mouse information
            Point mouseDownLocation = new Point(e.X, e.Y);
            Debug.WriteLine("mouse down {0}, {1}", e.X, e.Y);
            Point mouseClickLocation = new Point(e.X, e.Y);
            firstPoint = mouseClickLocation;
            theCalipers.GrabCaliperIfClicked(mouseClickLocation);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point newPoint = new Point(e.X, e.Y);
            int deltaX = newPoint.X - firstPoint.X;
            int deltaY = newPoint.Y - firstPoint.Y;
            if (theCalipers.DragGrabbedCaliper(deltaX, deltaY))
            {
                firstPoint = newPoint;
                pictureBox1.Refresh();
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (theCalipers.ReleaseGrabbedCaliper(e.Clicks))
            {
                pictureBox1.Refresh();

            }
        }

        private Bitmap Zoom(Bitmap originalBitmap, Double zoomFactor)
        {
            Size newSize = new Size((int)(originalBitmap.Width * zoomFactor), (int)(originalBitmap.Height * zoomFactor));
            Bitmap bmp = new Bitmap(originalBitmap, newSize);
            return bmp;
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;
            theCalipers.Draw(g, pictureBox1.DisplayRectangle);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (theBitmap == null)
            {
                return;
            }
            if (trackBar1.Value == 1)
            {
                pictureBox1.Image = theBitmap;
                return;
            }
            Bitmap zoomedBitmap = Zoom(theBitmap, trackBar1.Value);
            pictureBox1.Image = zoomedBitmap;
        }

 
    }
}
