using epcalipers.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epcalipers
{
    public partial class Form1 : Form
    {

        Bitmap theBitmap;
        Bitmap originalBitmapCopy;
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

        // Note zoom factors used in Mac OS X version
        // These are taken from the Apple IKImageView demo
        double zoomInFactor = 1.414214;
        double zoomOutFactor = 0.7071068;
        double currentActualZoom = 1.0;

        double rrIntervalForQtc = 0.0;

        // Drag and drop variables
        protected Thread getImageThread;
        protected bool validData;
        protected DragDropEffects effects;
        protected string lastFilename;
        protected Image image;
        protected Image nextImage;
        protected int lastX = 0;
        protected int lastY = 0;

        public Form1()
        {
            InitializeComponent();
            preferences = new Preferences();
            theCalipers = new Calipers();
            ecgPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            ecgPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            ecgPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            ecgPictureBox.MouseDoubleClick += pictureBox1_MouseDoubleClick;
            ecgPictureBox.MouseUp += pictureBox1_MouseUp;
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
                ecgPictureBox.Refresh();
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
                ecgPictureBox.Refresh();
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
                ecgPictureBox.Load(openFileDialog1.FileName);
                ResetBitmap(ecgPictureBox.Image);
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
                ecgPictureBox.Refresh();
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
                    ecgPictureBox.Refresh();
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
                ecgPictureBox.Refresh();
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
            ecgPictureBox.Refresh();

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
            if (ecgPictureBox.Image == null )
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
            c.LineWidth = preferences.LineWidth;
            c.UnselectedColor = preferences.CaliperColor;
            c.SelectedColor = preferences.HighlightColor;
            c.CaliperColor = c.UnselectedColor;
            c.RoundMsecRate = preferences.RoundMsecRate;
            c.Direction = direction;
            if (direction == CaliperDirection.Horizontal)
            {
                c.CurrentCalibration = theCalipers.HorizontalCalibration;
            }
            else
            {
                c.CurrentCalibration = theCalipers.VerticalCalibration;
            }
            c.SetInitialPositionInRect(ecgPictureBox.DisplayRectangle);
            theCalipers.addCaliper(c);
            ecgPictureBox.Refresh();
        }

        private void intervalRateButton_Click(object sender, EventArgs e)
        {
            theCalipers.HorizontalCalibration.DisplayRate = !theCalipers.HorizontalCalibration.DisplayRate;
            ecgPictureBox.Refresh();
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            if (ecgPictureBox.Image == null)
            {
                return;
            }
            currentActualZoom *= zoomInFactor;
            Bitmap zoomedBitmap = Zoom(theBitmap);
            if (ecgPictureBox.Image != null && ecgPictureBox.Image != theBitmap)
            {
                ecgPictureBox.Image.Dispose();
            }
            ecgPictureBox.Image = zoomedBitmap;
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            if (ecgPictureBox.Image == null)
            {
                return;
            }
            currentActualZoom *= zoomOutFactor;
            Bitmap zoomedBitmap = Zoom(theBitmap);
            if (ecgPictureBox.Image != null && ecgPictureBox.Image != theBitmap)
            {
                ecgPictureBox.Image.Dispose();
            }
            ecgPictureBox.Image = zoomedBitmap;
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point mouseClickLocation = new Point(e.X, e.Y);
            if (theCalipers.DeleteCaliperIfClicked(mouseClickLocation))
            {
                ecgPictureBox.Refresh();
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
                ecgPictureBox.Refresh();
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (theCalipers.ReleaseGrabbedCaliper(e.Clicks))
            {
                ecgPictureBox.Refresh();
            }
        }

        private Bitmap Zoom(Bitmap originalBitmap)
        {
           
            theCalipers.updateCalibration(currentActualZoom);
            Size newSize = new Size((int)(originalBitmap.Width * currentActualZoom), (int)(originalBitmap.Height * currentActualZoom));
            Bitmap bmp = new Bitmap(originalBitmap, newSize);
            return bmp;
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            theCalipers.Draw(g, ecgPictureBox.DisplayRectangle);
        }

        private void quitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ecgPictureBox.Image == null)
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
            Image image = (Image)ecgPictureBox.Image.Clone();
            Graphics g = Graphics.FromImage(image);
            /// TODO: Need theCalipers.DrawPrint method to make font smaller for printing
            /// and saving images.
            theCalipers.Draw(g, ecgPictureBox.DisplayRectangle);
            e.Graphics.DrawImage(image, 0, 0);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ecgPictureBox.Image == null)
            {
                MessageBox.Show("No image is open so how can you save?", "No Image Open");
                return;
            }
            saveFileDialog1.Filter = fileTypeFilter;
            saveFileDialog1.DefaultExt = "jpg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image image = (Image)ecgPictureBox.Image.Clone();
                Graphics g = Graphics.FromImage(image);
                theCalipers.Draw(g, ecgPictureBox.DisplayRectangle);
                image.Save(saveFileDialog1.FileName);               
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesDialog dialog = new PreferencesDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                dialog.Save();
                UpdatePreferences();
            }

        }

        private void UpdatePreferences()
        {
            preferences.Load();
            // update all the calipers
            theCalipers.UpdatePreferences(preferences);
            ecgPictureBox.Refresh();
        }

        /* Drag and drop image onto form
         * based on http://www.codeproject.com/Articles/9017/A-Simple-Drag-And-Drop-How-To-Example
         * Note re license: "This article has no explicit license attached to it but may contain
         *  usage terms in the article text or the download files themselves. 
         *  If in doubt please contact the author via the discussion board below."
         */
        private void OnDragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("OnDragDrop");
            if (validData)
            {
                while (getImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                thumbnail.Visible = false;
                image = nextImage;
                if ((ecgPictureBox.Image != null) && (ecgPictureBox.Image != nextImage))
                {
                    ecgPictureBox.Image.Dispose();
                }
                ecgPictureBox.Image = image;
                ResetBitmap(image);
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("OnDragEnter");
            string filename;
            validData = GetFilename(out filename, e);
            if (validData)
            {
                if (lastFilename != filename)
                {
                    thumbnail.Image = null;
                    thumbnail.Visible = false;
                    lastFilename = filename;
                    getImageThread = new Thread(new ThreadStart(LoadImage));
                    getImageThread.Start();
                }
                else
                {
                    thumbnail.Visible = true;
                }
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OnDragLeave(object sender, EventArgs e)
        {
            Debug.WriteLine("OnDragLeave");
            thumbnail.Visible = false;
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            Debug.WriteLine("OnDragOver");
            if (validData)
            {
                // only bother if mouse position changes
                if ((e.X != lastX) || (e.Y != lastY))
                {
                    setThumbnailLocation(this.PointToClient(new Point(e.X, e.Y)));
                    Debug.WriteLine("lastX and lastY = {0} & {1}", lastX, lastY);
                    lastX = e.X;
                    lastY = e.Y;
                }
            }
        }

        protected bool GetFilename(out string filename, DragEventArgs e)
        {
            bool result = false;
            filename = String.Empty;

            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                        string ext = System.IO.Path.GetExtension(filename).ToLower();
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp"))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public delegate void AssignImageDlgt();

        protected void LoadImage()
        {
            nextImage = new Bitmap(lastFilename);
            this.Invoke(new AssignImageDlgt(AssignImage));
        }

        protected void AssignImage()
        {
            thumbnail.Width = 100;
            thumbnail.Height = nextImage.Height * 100 / nextImage.Width;
            setThumbnailLocation(PointToClient(new Point(lastX, lastY)));
            thumbnail.Image = nextImage;
        }

        protected void setThumbnailLocation(Point point)
        {
            if (thumbnail.Image == null)
            {
                thumbnail.Visible = false;
            }
            else
            {
                point.X -= thumbnail.Width / 2;
                point.Y -= thumbnail.Height / 2;
                thumbnail.Location = point;
                thumbnail.Visible = true;
            }
        }

        private void ResetBitmap(Image image)
        {
            if (theBitmap != null)
            {
                theBitmap.Dispose();
            }
            theBitmap = new Bitmap(image);
            // reset originalBitmapCopy so it can be used for rotation of new image
            if (originalBitmapCopy != null)
            {
                originalBitmapCopy.Dispose();
                originalBitmapCopy = null;
            }
            currentActualZoom = 1.0;
            ClearCalibration();
            addCalipersButton.Enabled = true;
            calibrateButton.Enabled = true;
        }

        

        // rotation
        private void RotateEcgImage(float angle)
        {
            if (ecgPictureBox.Image == null)
            {
                return;
            }
            if (originalBitmapCopy == null)
            {
                originalBitmapCopy = new Bitmap(theBitmap);
            }
            Bitmap rotatedBitmap = RotateImage(theBitmap, angle, Color.Black);
            theBitmap.Dispose();
            theBitmap = new Bitmap(rotatedBitmap);
            Bitmap zoomedBitmap = Zoom(theBitmap);
            if (ecgPictureBox.Image != null && ecgPictureBox.Image != theBitmap)
            {
                ecgPictureBox.Image.Dispose();
            }
            ecgPictureBox.Image = zoomedBitmap;
            // calibration can't be maintained with rotation
            ResetCalibration();

        }

        private void ResetEcgImage()
        {
            if (theBitmap != null && originalBitmapCopy != null)
            {
                theBitmap.Dispose();
                theBitmap = new Bitmap(originalBitmapCopy);
                if (ecgPictureBox.Image != null && ecgPictureBox.Image != theBitmap)
                {
                    ecgPictureBox.Image.Dispose();
                }
                ecgPictureBox.Image = theBitmap;
                ResetCalibration();
                currentActualZoom = 1.0;
                // not necessary to refresh image when image updated
                //ecgPictureBox.Refresh();
            }
        }
        

        // code based on http://stackoverflow.com/questions/14184700/how-to-rotate-image-x-degrees-in-c
        private Bitmap RotateImage(Bitmap bmp, float angle, Color bkColor)
        {
            angle = angle % 360;
            if (angle > 180)
                angle -= 360;

            PixelFormat pf = default(PixelFormat);
            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
            float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
            float newImgWidth = sin * bmp.Height + cos * bmp.Width;
            float newImgHeight = sin * bmp.Width + cos * bmp.Height;
            float originX = 0f;
            float originY = 0f;

            if (angle > 0)
            {
                if (angle <= 90)
                    originX = sin * bmp.Height;
                else
                {
                    originX = newImgWidth;
                    originY = newImgHeight - sin * bmp.Width;
                }
            }
            else
            {
                if (angle >= -90)
                    originY = sin * bmp.Width;
                else
                {
                    originX = newImgWidth - sin * bmp.Height;
                    originY = newImgHeight;
                }
            }

            Bitmap newImg = new Bitmap((int)newImgWidth, (int)newImgHeight, pf);
            Graphics g = Graphics.FromImage(newImg);
            g.Clear(bkColor);
            g.TranslateTransform(originX, originY); // offset the origin to our calculated values
            g.RotateTransform(angle); // set up rotate
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(bmp, 0, 0); // draw the image at 0, 0
            g.Dispose();

            return newImg;
        }

        private void resetImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetEcgImage();
        }

        private void rotate90RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateEcgImage(90.0f);
        }

        private void rotate1LToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            RotateEcgImage(-1.0f);
        }

        private void rotate90LToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RotateEcgImage(-90.0f);
        }

        private void rotate1RToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            RotateEcgImage(1.0f);
        }
    }
}
