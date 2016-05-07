using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
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
        Button setCalibrationButton;
        Button clearCalibrationButton;
        Button backCalibrationButton;
        Control[] mainMenu;
        Control[] calibrationMenu;

        Point firstPoint;

        string fileTypeFilter = "Image Files (*.jpg, *.bmp) | *.jpg; *.bmp";
        double zoomInFactor = 1.414214;
        double zoomOutFactor = 0.7071068;
        double currentActualZoom = 1.0;

        public Form1()
        {
            InitializeComponent();
            theCalipers = new Calipers();
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            pictureBox1.MouseDoubleClick += pictureBox1_MouseDoubleClick;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            SetupButtons();
            ShowMainMenu();
        }

        private void SetupButtons()
        {
            imageButton = new Button();
            imageButton.Text = "Open";
            toolTip1.SetToolTip(imageButton, "Open ECG image");
            imageButton.Click += imageButton_Click;
            addCalipersButton = new Button();
            addCalipersButton.Text = "Add Caliper";
            toolTip1.SetToolTip(addCalipersButton, "Add new caliper");
            addCalipersButton.Click += addCaliper_Click;
            calibrateButton = new Button();
            calibrateButton.Text = "Calibrate";
            toolTip1.SetToolTip(calibrateButton, "Calibrate caliper");
            calibrateButton.Click += calibrateButton_Click;
            setCalibrationButton = new Button();
            setCalibrationButton.Text = "Set";
            toolTip1.SetToolTip(setCalibrationButton, "Set calibration interval");
            setCalibrationButton.Click += setCalibrationButton_Click;
            clearCalibrationButton = new Button();
            clearCalibrationButton.Text = "Clear";
            toolTip1.SetToolTip(clearCalibrationButton, "Clear all calibration");
            clearCalibrationButton.AutoSize = true;
            clearCalibrationButton.Click += clearCalibrationButton_Click;
            backCalibrationButton = new Button();
            toolTip1.SetToolTip(backCalibrationButton, "Done with calibration");
            backCalibrationButton.Text = "Done";
            backCalibrationButton.Click += backCalibrationButton_Click;
            intervalRateButton = new Button();
            intervalRateButton.Text = "Int/Rate";
            intervalRateButton.Click += intervalRateButton_Click;
            toolTip1.SetToolTip(intervalRateButton, "Toggle between interval and rate");
            measureButton = new Button();
            measureButton.Text = "Measure";
            measureButton.Click += MeasureButton_Click;
            toolTip1.SetToolTip(measureButton, "Make measurements");
            //
        }

        private void MeasureButton_Click(object sender, EventArgs e)
        {
            MeasureDialog dialog = new MeasureDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
               
            }
        }

        private void ShowMainMenu()
        {
            flowLayoutPanel1.Controls.Clear();
            if (mainMenu == null)
            {
                mainMenu = new Control[] { measureButton,
                    intervalRateButton, calibrateButton,
                    addCalipersButton, imageButton  };

            }
            flowLayoutPanel1.Controls.AddRange(mainMenu);
            if (theBitmap == null)
            {
                addCalipersButton.Enabled = false;
                calibrateButton.Enabled = false;
            }
            intervalRateButton.Enabled = theCalipers.HorizontalCalibration.CanDisplayRate;
            measureButton.Enabled = theCalipers.HorizontalCalibration.CanDisplayRate;
            theCalipers.Locked = false;
        }

        private void ShowCalibrationMenu()
        {
            flowLayoutPanel1.Controls.Clear();
            if (calibrationMenu == null)
            {
                calibrationMenu = new Control[] { backCalibrationButton,
                    clearCalibrationButton,
                    setCalibrationButton };
            }
            flowLayoutPanel1.Controls.AddRange(calibrationMenu);
        }

        private void clearCalibrationButton_Click(object sender, EventArgs e)
        {
            ClearCalibration();
        }

        private void backCalibrationButton_Click(object sender, EventArgs e)
        {
            ShowMainMenu();
        }

        private void imageButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("image button pushed");
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = fileTypeFilter;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                theBitmap = new Bitmap(pictureBox1.Image);
                currentActualZoom = 1.0;
                ClearCalibration();
                addCalipersButton.Enabled = true;
                calibrateButton.Enabled = true;
            }
        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {
            if (NoCalipersError())
            {
                return;
            }
            ShowCalibrationMenu();
            if (theCalipers.SelectCaliperIfNoneSelected())
            {
                pictureBox1.Refresh();
            }
        }

        private void setCalibrationButton_Click(object sender, EventArgs e)
        {
            if (NoCalipersError())
            {
                return;
            }
            if (theCalipers.NoCaliperIsSelected())
            {
                if (theCalipers.NumberOfCalipers() == 1)
                {
                    // assume user wants to calibrate sole caliper so select it
                    theCalipers.SelectSoleCaliper();
                    pictureBox1.Refresh();
                }
                else
                {
                    MessageBox.Show("Select (by single-clicking it) the caliper that you want to calibrate, and then set it to a known interval");
                    return;
                }
            }
            CalibrationDialog dialog = new CalibrationDialog();
            Caliper c = theCalipers.GetActiveCaliper();
            if (c.Direction == CaliperDirection.Horizontal)
            {
                dialog.calibrationMeasurementTextBox.Text = theCalipers.HorizontalCalibration.CalibrationString;
            }
            else
            {
                dialog.calibrationMeasurementTextBox.Text = theCalipers.VerticalCalibration.CalibrationString;
            }
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Calibrate(dialog.calibrationMeasurementTextBox.Text);
            }
        }

        private bool NoCalipersError()
        {
            bool noCalipers = false;
            if (theCalipers.NumberOfCalipers() < 1)
            {
                ShowNoCalipersDialog();
                noCalipers = true; ;
            }
            return noCalipers;
        }

        private void ShowNoCalipersDialog()
        {
            MessageBox.Show("Add one or more calipers first before proceeding.",
                "No Calipers To Use");
        }

        private void Calibrate(string rawCalibration)
        {
            try
            {
                if (rawCalibration.Length < 1)
                {
                    throw new Exception("No calibration measurement entered.");
                }
                float value = 0.0f;
                string units = "";
                char[] delimiters = { ' ' };
                string[] parts = rawCalibration.Split(delimiters);
                value = float.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                value = Math.Abs(value);
                if (parts.Length > 1)
                {
                    // assume second substring is units
                    units = parts[1];
                }
                if (value == 0)
                {
                    throw new Exception("Calibration can't be zero.");
                }
                Caliper c = theCalipers.GetActiveCaliper();
                if (c == null)
                {
                    // this really shouldn't happen
                    throw new Exception("No caliper for calibration.");
                }
                if (c.ValueInPoints <= 0)
                {
                    // this could happen
                    throw new Exception("Caliper must be positive to calibrate.");
                }
                Calibration calibration;
                if (c.Direction == CaliperDirection.Horizontal)
                {
                    calibration = theCalipers.HorizontalCalibration;
                }
                else
                {
                    calibration = theCalipers.VerticalCalibration;
                }
                calibration.CalibrationString = rawCalibration;
                calibration.Units = units;
                if (!calibration.CanDisplayRate)
                {
                    calibration.DisplayRate = false;
                }
                calibration.OriginalZoom = currentActualZoom;
                calibration.OriginalCalFactor = value / c.ValueInPoints;
                calibration.CurrentZoom = calibration.OriginalZoom;
                calibration.Calibrated = true;
                pictureBox1.Refresh();
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Calibration Error");
            }
        }

        private void ClearCalibration()
        {
            ResetCalibration();
            pictureBox1.Refresh();

        }

        private void ResetCalibration()
        {
            if (theCalipers.HorizontalCalibration.Calibrated ||
                theCalipers.VerticalCalibration.Calibrated)
            {
                theCalipers.HorizontalCalibration.Reset();
                theCalipers.VerticalCalibration.Reset();
            }
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
            if (direction == CaliperDirection.Horizontal)
            {
                c.CurrentCalibration = theCalipers.HorizontalCalibration;
            }
            else
            {
                c.CurrentCalibration = theCalipers.VerticalCalibration;
            }
            c.SetInitialPositionInRect(pictureBox1.DisplayRectangle);
            theCalipers.addCaliper(c);
            pictureBox1.Refresh();
        }

        private void intervalRateButton_Click(object sender, EventArgs e)
        {
            theCalipers.HorizontalCalibration.DisplayRate = !theCalipers.HorizontalCalibration.DisplayRate;
            pictureBox1.Refresh();
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }
            currentActualZoom *= zoomInFactor;
            Bitmap zoomedBitmap = Zoom(theBitmap);
            pictureBox1.Image = zoomedBitmap;
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }
            currentActualZoom *= zoomOutFactor;
            Bitmap zoomedBitmap = Zoom(theBitmap);
            pictureBox1.Image = zoomedBitmap;
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
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

        private Bitmap Zoom(Bitmap originalBitmap)
        {
            // Note zoom factors used in Mac OS X version
            // // These are taken from the Apple IKImageView demo
            //let zoomInFactor: CGFloat = 1.414214
            //let zoomOutFactor: CGFloat = 0.7071068
            
            theCalipers.updateCalibration(currentActualZoom);
            Size newSize = new Size((int)(originalBitmap.Width * currentActualZoom), (int)(originalBitmap.Height * currentActualZoom));
            Bitmap bmp = new Bitmap(originalBitmap, newSize);
            return bmp;
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            theCalipers.Draw(g, pictureBox1.DisplayRectangle);
        }

        private void quitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintPictureBox);
            pd.Print();
        }

        private void PrintPictureBox(object sender, PrintPageEventArgs e)
        {
            //Graphics g = e.Graphics;
            Image image = (Image)pictureBox1.Image.Clone();
            Graphics g = Graphics.FromImage(image);
            /// TODO: Need theCalipers.DrawPrint method to make font smaller for printing
            /// and saving images.
            theCalipers.Draw(g, pictureBox1.DisplayRectangle);
            e.Graphics.DrawImage(image, 0, 0);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }
            saveFileDialog1.Filter = fileTypeFilter;
            saveFileDialog1.DefaultExt = "jpg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image image = (Image)pictureBox1.Image.Clone();
                Graphics g = Graphics.FromImage(image);
                theCalipers.Draw(g, pictureBox1.DisplayRectangle);
                image.Save(saveFileDialog1.FileName);               
            }
        }
    }
}
