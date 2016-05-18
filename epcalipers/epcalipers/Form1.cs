using epcalipers.Properties;
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
        Button measureRRForQtcButton;
        Button setCalibrationButton;
        Button clearCalibrationButton;
        Button backCalibrationButton;
        Button meanRRButton;
        Button qtcButton;
        Button cancelButton;
        Button measureQTcButton;
        Label measureQtcMessageLabel;
        Label measureRRForQtcMessageLabel;
        Control[] mainMenu;
        Control[] calibrationMenu;
        Control[] qtcStep1Menu;
        Control[] qtcStep2Menu;
        Preferences preferences;

        Point firstPoint;

        string fileTypeFilter = "Image Files (*.jpg, *.bmp) | *.jpg; *.bmp";
        double zoomInFactor = 1.414214;
        double zoomOutFactor = 0.7071068;
        double currentActualZoom = 1.0;
        double rrIntervalForQtc = 0.0;

        public Form1()
        {
            InitializeComponent();
            preferences = new Preferences();
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
            toolTip1.SetToolTip(calibrateButton, "Calibrate calipers");
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
            backCalibrationButton.Text = "Back";
            backCalibrationButton.Click += backCalibrationButton_Click;
            intervalRateButton = new Button();
            intervalRateButton.Text = "Int/Rate";
            intervalRateButton.Click += intervalRateButton_Click;
            toolTip1.SetToolTip(intervalRateButton, "Toggle between interval and rate");
            measureRRForQtcButton = new Button();
            measureRRForQtcButton.Text = "Measure";
            measureRRForQtcButton.Click += MeasureRRForQtcButton_Click;
            toolTip1.SetToolTip(measureRRForQtcButton, "Measure 1 or more RR intervals for QTc");
            measureQTcButton = new Button();
            measureQTcButton.Text = "Measure";
            measureQTcButton.Click += MeasureQTcButton_Click;
            toolTip1.SetToolTip(measureQTcButton, "Measure QT interval");
            meanRRButton = new Button();
            meanRRButton.Text = "Mean Rate";
            meanRRButton.Click += MeanRRButton_Click;
            toolTip1.SetToolTip(meanRRButton, "Measure mean interval and rate");
            qtcButton = new Button();
            qtcButton.Text = "QTc";
            qtcButton.Click += QtcButton_Click;
            toolTip1.SetToolTip(qtcButton, "Measure corrected QT (QTc)");
            //
            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Click += CancelButton_Click;
            toolTip1.SetToolTip(cancelButton, "Cancel measurement");
            measureRRForQtcMessageLabel = new Label();
            measureRRForQtcMessageLabel.Text = "Measure one or more RR intervals";
            // properties below ensure label is aligned with Buttons
            measureRRForQtcMessageLabel.AutoSize = true;
            measureRRForQtcMessageLabel.Dock = DockStyle.Fill;
            measureRRForQtcMessageLabel.TextAlign = ContentAlignment.MiddleCenter;
            measureQtcMessageLabel = new Label();
            measureQtcMessageLabel.Text = "Measure QT";
            measureQtcMessageLabel.AutoSize = true;
            measureQtcMessageLabel.Dock = DockStyle.Fill;
            measureQtcMessageLabel.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void MeasureQTcButton_Click(object sender, EventArgs e)
        {
            if (theCalipers.NoTimeCaliperSelected())
            {
                NoTimeCaliperError();
                return;
            }
            Caliper c = theCalipers.GetActiveCaliper();
            if (c == null)
            {
                return;
            }
            double qt = Math.Abs(EPCalculator.MsecToSec(c.IntervalResult()));
            double meanRR = Math.Abs(EPCalculator.MsecToSec(rrIntervalForQtc));
            string result = "Invalid Result";
            if (meanRR > 0)
            {
                double qtc = EPCalculator.QtcBazettSec(qt, meanRR);
                if (c.CurrentCalibration.UnitsAreMsecs)
                {
                    meanRR *= 1000.0;
                    qt *= 1000.0;
                    qtc *= 1000.0;
                }
                //s = string.Format("{0} {1}", CalibratedResult().ToString("G4"), CurrentCalibration.Units);

                result = string.Format("Mean RR = {0}\nQT = {1}\nQTc = {2}\n(Bazett's formula)", meanRR.ToString("G4"),
                    qt.ToString("G4"), qtc.ToString("G4"));
            }
            MessageBox.Show(result, "Calculated QTc");
            ShowMainMenu();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            theCalipers.Locked = false;
            ShowMainMenu();
        }

        private void QtcButton_Click(object sender, EventArgs e)
        {
            QTcInterval();
        }

        private void MeanRRButton_Click(object sender, EventArgs e)
        {
            MeasureMeanIntervalRate();
        }

        private void MeasureRRForQtcButton_Click(object sender, EventArgs e)
        {
            MeasureRRDialog dialog = new MeasureRRDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    string rawValue = dialog.numberOfIntervalsTextBox.Text;
                    Tuple<double, double> tuple = getMeanRRMeanRate(rawValue, theCalipers.GetActiveCaliper());
                    rrIntervalForQtc = tuple.Item1;
                    showQTcStep2Menu();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Measurement Error");
                    ShowMainMenu();
                }
            }
        }

        private void MeasureMeanIntervalRate()
        {
            if (NoCalipersError())
            {
                return;
            }
            Caliper singleHorizontalCaliper = theCalipers.getLoneTimeCaliper();
            if (singleHorizontalCaliper != null)
            {
                theCalipers.SelectCaliper(singleHorizontalCaliper);
                theCalipers.UnselectCalipersExcept(singleHorizontalCaliper);
                pictureBox1.Refresh();
            }
            if (theCalipers.NoCaliperIsSelected())
            {
                NoTimeCaliperError();
                return;
            }
            Caliper c = theCalipers.GetActiveCaliper();
            if (c.Direction == CaliperDirection.Vertical)
            {
                NoTimeCaliperError();
                return;
            }
            MeasureRRDialog dialog = new MeasureRRDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string rawValue = dialog.numberOfIntervalsTextBox.Text;
                try
                {
                    Tuple<double, double> tuple = getMeanRRMeanRate(rawValue, c);
                    double meanRR = tuple.Item1;
                    double meanRate = tuple.Item2;
                    string message = string.Format("Mean interval = {0} {1}", meanRR.ToString("G4"), c.CurrentCalibration.Units);
                    message += Environment.NewLine;
                    message += string.Format("Mean rate = {0} bpm", meanRate.ToString("G4"));
                    MessageBox.Show(message, "Mean Interval and Rate");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Measurement Error");
                }
            }
        }

        private Tuple<double, double> getMeanRRMeanRate(string rawValue, Caliper c)
        {
            
            if (rawValue.Length < 1)
            {
                throw new Exception("Number of intervals not entered.");
            }
            int divisor = int.Parse(rawValue);
            divisor = Math.Abs(divisor);
            if (divisor == 0)
            {
                throw new Exception("Number of intervals can't be zero.");
            }
            if (c == null)
            {
                throw new Exception("Can't find a selected caliper.");
            }
            double intervalResult = Math.Abs(c.IntervalResult());
            double meanRR = intervalResult / divisor;
            double meanRate;
            if (c.CurrentCalibration.UnitsAreMsecs)
            {
                meanRate = EPCalculator.MsecToBpm(meanRR);
            }
            else
            {
                meanRate = EPCalculator.SecToBpm(meanRR);
            }
            return Tuple.Create(meanRR, meanRate);
        }

        private void NoTimeCaliperError()
        {
            MessageBox.Show("Select a time caliper.", "Measurement Error");
        }

        private void QTcInterval()
        {
            theCalipers.HorizontalCalibration.DisplayRate = false;
            Caliper singleHorizontalCaliper = theCalipers.getLoneTimeCaliper();
            if (singleHorizontalCaliper != null)
            {
                theCalipers.SelectCaliper(singleHorizontalCaliper);
                theCalipers.UnselectCalipersExcept(singleHorizontalCaliper);
                pictureBox1.Refresh();
            }
            if (theCalipers.NoTimeCaliperSelected())
            {
                NoTimeCaliperError();
            }
            else
            {
                showQTcStep1Menu();
                theCalipers.Locked = true;
            }
        }

        private void showQTcStep1Menu()
        {
            flowLayoutPanel1.Controls.Clear();
            if (qtcStep1Menu == null)
            {
                qtcStep1Menu = new Control[] { cancelButton, measureRRForQtcButton, measureRRForQtcMessageLabel };
            }
            flowLayoutPanel1.Controls.AddRange(qtcStep1Menu);
        }

        private void showQTcStep2Menu()
        {
            flowLayoutPanel1.Controls.Clear();
            if (qtcStep2Menu == null)
            {
                qtcStep2Menu = new Control[] { cancelButton, measureQTcButton, measureQtcMessageLabel };
            }
            flowLayoutPanel1.Controls.AddRange(qtcStep2Menu);
        }

        private void ShowMainMenu()
        {
            flowLayoutPanel1.Controls.Clear();
            if (mainMenu == null)
            {
                mainMenu = new Control[] { qtcButton, meanRRButton,
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
            meanRRButton.Enabled = theCalipers.HorizontalCalibration.CanDisplayRate;
            qtcButton.Enabled = theCalipers.HorizontalCalibration.CanDisplayRate;
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
                    MessageBox.Show("Select (by single-clicking it) the caliper that you want to calibrate, and then set it to a known interval.",
                        "No Caliper Selected");
                    return;
                }
            }
            CalibrationDialog dialog = new CalibrationDialog();
            Caliper c = theCalipers.GetActiveCaliper();
            if (c.Direction == CaliperDirection.Horizontal)
            {
                if (theCalipers.HorizontalCalibration.CalibrationString == null)
                {
                    theCalipers.HorizontalCalibration.CalibrationString = preferences.HorizontalCalibration;
                }
                dialog.calibrationMeasurementTextBox.Text = theCalipers.HorizontalCalibration.CalibrationString;
            }
            else
            {
                if (theCalipers.VerticalCalibration.CalibrationString == null)
                {
                    theCalipers.VerticalCalibration.CalibrationString = preferences.VerticalCalibration;
                }
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
                // return to main menu after successful calibration
                // = behavior in mobile versions
                ShowMainMenu();
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
            if (pictureBox1.Image != null && pictureBox1.Image != theBitmap)
            {
                pictureBox1.Image.Dispose();
            }
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
            if (pictureBox1.Image != null && pictureBox1.Image != theBitmap)
            {
                pictureBox1.Image.Dispose();
            }
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
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("No image is open so how can you print?", "No Image Open");
                return;
            }
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
                MessageBox.Show("No image is open so how can you save?", "No Image Open");
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

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesDialog dialog = new PreferencesDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                dialog.Save();
            }

        }

        // Drag and drop image onto form

    }
}
