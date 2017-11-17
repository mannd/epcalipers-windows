using epcalipers.Properties;
using ImageMagick;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace epcalipers
{
    public partial class Form1 : Form
    {
        #region Fields
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
        Button cancelTweakButton;
        Label measureQtcMessageLabel;
        Label measureRRForQtcMessageLabel;
        Label tweakLabel;
        Control[] mainMenu;
        Control[] calibrationMenu;
        Control[] qtcStep1Menu;
        Control[] qtcStep2Menu;
        Control[] tweakMenu;
        static int maxControlNumber = 10;
        Control[] oldControls = new Control[maxControlNumber];


        Preferences preferences;
        PreferencesDialog preferencesDialog;
        MeasureRRDialog measureRRDialog;
        CalibrationDialog calibrationDialog;

        float rotationAngle = 0.0f;
        Color BACKGROUND_COLOR = Color.Transparent;

        // allow users to retain custom colors per session
        int[] customColors;

        Point firstPoint;

        string openFileTypeFilter = "Image or PDF files | *.jpg; *.bmp; *.png; *.pdf";
        string saveFileTypeFilter = "Image files (*.jpg, *.bmp *.png) | *.jpg; *.bmp; *.png";

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
        protected string lastFilename = "";
        protected Image image;
        protected Image nextImage;
        protected int lastX = 0;
        protected int lastY = 0;

        // PDF stuff
        private MagickImageCollection pdfImages = null;
        int numberOfPdfPages = 0;
        int currentPdfPage = 0;

        // transparency stuff
        private Color oldFormBackgroundColor;
        private Color oldTransparencyKey;
        private bool isTransparent;


        #endregion
        #region Initialization
        public Form1()
        {
            InitializeComponent();
            preferences = new Preferences();
            theCalipers = new Calipers();

            oldFormBackgroundColor = BackColor;
            oldTransparencyKey = TransparencyKey;
            ecgPictureBox.BackColor = Color.White;
            isTransparent = false;
            FormBorderStyle = FormBorderStyle.Sizable;

            ecgPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            ecgPictureBox.Paint += ecgPictureBox_Paint;
            ecgPictureBox.MouseDown += ecgPictureBox_MouseDown;
            ecgPictureBox.MouseDoubleClick += ecgPictureBox_MouseDoubleClick;
            ecgPictureBox.MouseUp += ecgPictureBox_MouseUp;

            //KeyPreview = true;
            //this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            SetupButtons();
            ShowMainMenu();
            // form starts with no image loaded, so no pages either
            EnablePages(false);

            if (preferences.ShowHandlesPictureMode)
            {
                showHandles(true);
                showHandlesToolStripMenuItem.Checked = true;
            }

            try
            {
                if (Environment.GetCommandLineArgs().Length > 1)
                {
                    string arg1 = Environment.GetCommandLineArgs()[1];
                    string ext = System.IO.Path.GetExtension(arg1);
                    if (IsValidFileType(ext, true))
                    {
                        lastFilename = arg1;
                        if (ext.ToLower() == ".pdf")
                        {
                            OpenPdf(lastFilename);
                        }
                        else
                        {
                            Image argImage = new Bitmap(lastFilename);
                            ecgPictureBox.Image = argImage;
                        }
                        ResetBitmap(ecgPictureBox.Image);
                        ShowMainMenu();
                    }
                }
            }
            catch (Exception)
            {
                lastFilename = "";
                
            }
        }

        private void makeTransparent(bool value)
        {
            KillBitmap();
            theCalipers.deleteAllCalipers();
            ecgPictureBox.Image = null;
            CancelTweaking();
            //imageButton.Enabled = !value;
            //openToolStripMenuItem.Enabled = !value;
            isTransparent = value;
            // when transparent, can't allow maximize, since bug in Windows makes it impossible
            // to grab the window after being maximized and restored
            MaximizeBox = !value;
            WindowState = FormWindowState.Normal;
            if (value)
            {
                ecgPictureBox.Dock = DockStyle.Fill;
                if (preferences.ShowHandlesTransparentMode)
                {
                    setShowHandles(true);
                }
                ecgPictureBox.BackColor = Color.Transparent;
                BackColor = Color.Gray;
                TransparencyKey = Color.Gray;
                
            }
            else
            {
                ecgPictureBox.Dock = DockStyle.None;
                ecgPictureBox.Size = this.Size;
                if (preferences.ShowHandlesPictureMode)
                {
                    setShowHandles(true);
                }
                ecgPictureBox.BackColor = Color.White;
                BackColor = oldFormBackgroundColor;
                TransparencyKey = oldTransparencyKey;
            }
            ResetCalibration();
            ecgPictureBox.Refresh();
            // TODO: below sometimes called twice
            ShowMainMenu();
        }

        private void setShowHandles(bool value)
        {
            showHandles(value);
            showHandlesToolStripMenuItem.Checked = value;
        }

        private void SetupButtons()
        {
            imageButton = new Button();
           // imageButton.BackColor = Control.DefaultBackColor;
            imageButton.Text = "Open";
            toolTip1.SetToolTip(imageButton, "Open ECG image file or PDF");
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
            intervalRateButton.Text = "Rate/Int";
            intervalRateButton.Click += intervalRateButton_Click;
            toolTip1.SetToolTip(intervalRateButton, "Toggle between rate and interval");

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
            toolTip1.SetToolTip(meanRRButton, "Measure mean rate and interval");

            qtcButton = new Button();
            qtcButton.Text = "QTc";
            qtcButton.Click += QtcButton_Click;
            toolTip1.SetToolTip(qtcButton, "Measure corrected QT (QTc)");

            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Click += CancelButton_Click;
            toolTip1.SetToolTip(cancelButton, "Cancel measurement");

            cancelTweakButton = new Button();
            cancelTweakButton.Text = "Cancel";
            cancelTweakButton.Click += CancelTweakButton_Click;
            toolTip1.SetToolTip(cancelTweakButton, "Cancel tweaking");
  
            measureRRForQtcMessageLabel = new Label();
            measureRRForQtcMessageLabel.Text = "Measure one or more RR intervals";
            AdjustLabel(measureRRForQtcMessageLabel);
            measureQtcMessageLabel = new Label();
            measureQtcMessageLabel.Text = "Measure QT";
            AdjustLabel(measureQtcMessageLabel);
            tweakLabel = new Label();
            // tweakLabel text is changed on the fly
            AdjustLabel(tweakLabel);
        }

 
        private void AdjustLabel(Label label)
        {
            // properties below ensure label is aligned with Buttons
            label.AutoSize = true;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
        }
      
        #endregion
        #region Buttons
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
                result = string.Format("Mean RR = {0} msec\nQT = {1} msec\nQTc = {2} msec (Bazett's formula)", meanRR.ToString("G4"),
                    qt.ToString("G4"), qtc.ToString("G4"));
            }
            MessageBox.Show(result, "Calculated QTc");
            ShowMainMenu();
        }

        // TODO: cancel button needs to return to QT measurement if we are doing QTc,
        // and, must keep calipers locked if doing QTc.
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
            if (measureRRDialog == null)
            {
                measureRRDialog = new MeasureRRDialog();
            }
            measureRRDialog.numberOfIntervalsTextBox.Text = preferences.NumberOfIntervalsQtc.ToString();
            DialogResult result = measureRRDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    string rawValue = measureRRDialog.numberOfIntervalsTextBox.Text;
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
            // opening a file removes transparency automatically
            if (isTransparent) { 
              makeTransparent(false);
            }
            transparentWindowToolStripMenuItem.Checked = false;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = openFileTypeFilter;
            bool isPDF = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                try
                {
                    {
                        if (Path.GetExtension(openFileDialog1.FileName).ToLower() == ".pdf")
                        {
                            isPDF = true;
                            ClearPdf();
                            OpenPdf(openFileDialog1.FileName);
                        }
                        else
                        {
                            ecgPictureBox.Load(openFileDialog1.FileName);
                            ClearPdf();
                        }
                        ResetBitmap(ecgPictureBox.Image);
                        ShowMainMenu();
                    }
                }
                catch (Exception exception)
                {
                    String pdfWarning = "";
                    if (isPDF)
                    {
                        pdfWarning = "A reminder: to open PDF files, the Ghostscript library must be installed.  See Help for more information";

                    }
                    MessageBox.Show(String.Format("Could not open {0} from disk. {1}", openFileDialog1.FileName, pdfWarning) + "\n\nDetailed error: " +
                        exception.Message, "Error");
                }

        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {
            DoCalibration();
        }

        private void setCalibrationButton_Click(object sender, EventArgs e)
        {
            try
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
                if (calibrationDialog == null)
                {
                    calibrationDialog = new CalibrationDialog();
                }
                Caliper c = theCalipers.GetActiveCaliper();
                if (c == null)
                {
                    throw new Exception("No caliper for calibration");
                }
                if (c.isAngleCaliper)
                {
                    throw new Exception("Angle calipers don't require calibration.  " +
                        "Only time or amplitude calipers need to be calibrated.\n\n" +
                        "If you want to use an angle caliper as a Brugadometer, " +
                        "you must first calibrate time and amplitude calipers.");
                }
                if (c.Direction == CaliperDirection.Horizontal)
                {
                    if (theCalipers.HorizontalCalibration.CalibrationString == null)
                    {
                        theCalipers.HorizontalCalibration.CalibrationString = preferences.HorizontalCalibration;
                    }
                    calibrationDialog.calibrationMeasurementTextBox.Text = theCalipers.HorizontalCalibration.CalibrationString;
                }
                else
                {
                    if (theCalipers.VerticalCalibration.CalibrationString == null)
                    {
                        theCalipers.VerticalCalibration.CalibrationString = preferences.VerticalCalibration;
                    }
                    calibrationDialog.calibrationMeasurementTextBox.Text = theCalipers.VerticalCalibration.CalibrationString;
                }
                DialogResult result = calibrationDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Calibrate(calibrationDialog.calibrationMeasurementTextBox.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Calibration Error");
            }
        }

        private void addCaliper_Click(object sender, EventArgs e)
        {
            if (ecgPictureBox.Image == null && !isTransparent)
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
                    AddCaliper(direction);
                }
                else if (dialog.VerticalCaliperRadioButton.Checked)
                {
                    direction = CaliperDirection.Vertical;
                    AddCaliper(direction);
                }
                else    //if (dialog.angleCaliperRadioButton.Checked)
                {
                    // angle caliper checked
                    AddAngleCaliper();
                }
            }
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void intervalRateButton_Click(object sender, EventArgs e)
        {
            ToggleIntervalRate();
        }

        private void CancelTweakButton_Click(object sender, EventArgs e)
        {
            CancelTweaking();
        }

        private void CancelTweaking()
        {
            // always returns to previous toolbar
            flowLayoutPanel1.Controls.Clear();
            foreach (Control c in oldControls)
            {
                flowLayoutPanel1.Controls.Add(c);
            }
            theCalipers.CancelTweaking();
        }


        #endregion
        #region Mouse
        private void ecgPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point mouseClickLocation = new Point(e.X, e.Y);
            if (theCalipers.DeleteCaliperIfClicked(mouseClickLocation))
            {
                ecgPictureBox.Refresh();
            }
        }

        private void ecgPictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point clickPoint = new Point(e.X, e.Y);
            if  (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Hide();
                theCalipers.SetChosenCaliper(clickPoint);
                theCalipers.SetChosenCaliperComponent(clickPoint);
                if (theCalipers.NoChosenCaliper() && theCalipers.tweakingComponent)
                {
                    CancelTweaking();
                }
                if (!theCalipers.tweakingComponent)
                {
                    contextMenuStrip1.Enabled = theCalipers.PointIsNearCaliper(clickPoint);
                    contextMenuStrip1.Show(this, clickPoint);
                }
                else
                {
                    TweakCaliper();
                }
                
                return;
            }
            // Update the mouse path with the mouse information
            firstPoint = clickPoint;
            theCalipers.GrabCaliperIfClicked(clickPoint);
        }

        private void ecgPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point newPoint = new Point(e.X, e.Y);
            int deltaX = newPoint.X - firstPoint.X;
            int deltaY = newPoint.Y - firstPoint.Y;
            if (theCalipers.DragGrabbedCaliper(deltaX, deltaY, firstPoint))
            {
                firstPoint = newPoint;
                ecgPictureBox.Refresh();
            }

        }

        private void ecgPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (theCalipers.ReleaseGrabbedCaliper(e.Clicks))
            {
                ecgPictureBox.Refresh();
            }
        }
        #endregion
        #region Drag and Drop
        /* Drag and drop image onto form
         * based on http://www.codeproject.com/Articles/9017/A-Simple-Drag-And-Drop-How-To-Example
         * Note re license: "This article has no explicit license attached to it but may contain
         *  usage terms in the article text or the download files themselves. 
         *  If in doubt please contact the author via the discussion board below."
         */
        private void OnDragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("OnDragDrop");
            if (isTransparent)
            {
                makeTransparent(false);
            }
            try
            {
                if (validData)
                {
                    if (getImageThread != null)
                    {
                        while (getImageThread.IsAlive)
                        {
                            Application.DoEvents();
                            Thread.Sleep(0);
                        }
                    }
                    thumbnail.Visible = false;
                    if (nextImage != null)
                    {
                        image = nextImage;
                    }
                    if ((ecgPictureBox.Image != null) && (ecgPictureBox.Image != nextImage))
                    {
                        ecgPictureBox.Image.Dispose();
                    }
                    if (FileIsPdf(lastFilename))
                    {
                        ClearPdf();
                        OpenPdf(lastFilename);
                        ResetBitmap(ecgPictureBox.Image);
                    }
                    else
                    {
                        ecgPictureBox.Image = image;
                        ResetBitmap(image);
                        ClearPdf();
                    }
                    ShowMainMenu();
                }
            }
            catch (Exception exception)
            {

                MessageBox.Show("Exception: " + exception.Message, "Error");
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
                    if (!FileIsPdf(filename))
                    {
                        getImageThread = new Thread(new ThreadStart(LoadImage));
                        getImageThread.Start();
                    }
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
                        string ext = System.IO.Path.GetExtension(filename);
                        result = IsValidFileType(ext, true);
                    }
                }
            }
            return result;
        }

        private bool IsValidFileType(string fileExtension, bool allowPDF)
        {
            string ext = fileExtension.ToLower();
            return ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp") || (allowPDF && ext == ".pdf"));
        }

        private bool FileIsPdf(string fileName)
        {
            try
            {
                return System.IO.Path.GetExtension(fileName).ToLower() == ".pdf";
            }
            catch (Exception)
            {
                return false;
            }
        
        }

        public delegate void AssignImageDlgt();

        protected void LoadImage()
        {
            try
            {
                nextImage = new Bitmap(lastFilename);
                this.Invoke(new AssignImageDlgt(AssignImage));
            }
            catch (Exception exception)
            {

                MessageBox.Show("Exception in LoadImage: " + exception.Message, "Error");
            }
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
        #endregion
        #region Calipers
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
            if (c.Direction == CaliperDirection.Vertical || c.isAngleCaliper)
            {
                NoTimeCaliperError();
                return;
            }
            if (measureRRDialog == null)
            {
                measureRRDialog = new MeasureRRDialog();
            }
            measureRRDialog.numberOfIntervalsTextBox.Text = preferences.NumberOfIntervalsMeanRR.ToString();
            DialogResult result = measureRRDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string rawValue = measureRRDialog.numberOfIntervalsTextBox.Text;
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
            CancelButton = cancelButton;
        }

        private void showQTcStep2Menu()
        {
            flowLayoutPanel1.Controls.Clear();
            if (qtcStep2Menu == null)
            {
                qtcStep2Menu = new Control[] { cancelButton, measureQTcButton, measureQtcMessageLabel };
            }
            flowLayoutPanel1.Controls.AddRange(qtcStep2Menu);
            CancelButton = cancelButton;
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
            EnableImageMenuItems(ImageIsLoaded());
            EnableCaliperMenuItems(CalipersAllowed());
            EnableMeasurementMenuItems(MeasurementsAllowed());

            theCalipers.Locked = false;
        }

        private void EnableCaliperMenuItems(bool enable)
        {
            addCalipersButton.Enabled = enable;
            calibrateButton.Enabled = enable;
            timeCaliperToolStripMenuItem.Enabled = enable;
            amplitudeCaliperToolStripMenuItem.Enabled = enable;
            angleCaliperToolStripMenuItem.Enabled = enable;
            deleteCaliperToolStripMenuItem.Enabled = enable;
            showHandlesToolStripMenuItem.Enabled = enable;
            deleteCaliperToolStripMenuItem.Enabled = enable;
            deleteAllCalipersToolStripMenuItem.Enabled = enable;
            calibrateToolStripMenuItem.Enabled = enable;
            clearCalibrationToolStripMenuItem.Enabled = enable;
        }

        private void EnableMeasurementMenuItems(bool enable)
        {
            intervalRateButton.Enabled = enable;
            meanRRButton.Enabled = enable;
            qtcButton.Enabled = enable;
            toggleRateintervalToolStripMenuItem.Enabled = enable;
            meanRateIntervalToolStripMenuItem.Enabled = enable;
            qTcMeasurementToolStripMenuItem.Enabled = enable;
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

        private void DoCalibration()
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
                EnableMeasurementMenuItems(false);
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
            SetupCaliper(c);
        }

        private void AddAngleCaliper()
        {
            AngleCaliper c = new AngleCaliper();
            // TODO: refactor common code
            c.Direction = CaliperDirection.Horizontal;
            c.CurrentCalibration = theCalipers.HorizontalCalibration;
            c.VerticalCalibration = theCalipers.VerticalCalibration;
            SetupCaliper(c);
        }

        private void SetupCaliper(Caliper c)
        {
            c.LineWidth = preferences.LineWidth;
            c.UnselectedColor = preferences.CaliperColor;
            c.SelectedColor = preferences.HighlightColor;
            c.CaliperColor = c.UnselectedColor;
            c.RoundMsecRate = preferences.RoundMsecRate;
            c.hasHandles = showHandlesToolStripMenuItem.Checked;
            c.SetInitialPositionInRect(ecgPictureBox.DisplayRectangle);
            theCalipers.addCaliper(c);
            ecgPictureBox.Refresh();
        }

        private void ToggleIntervalRate()
        {
            theCalipers.HorizontalCalibration.DisplayRate = !theCalipers.HorizontalCalibration.DisplayRate;
            ecgPictureBox.Refresh();
        }

        #endregion
        #region Image

        private bool ImageIsLoaded()
        {
            return ecgPictureBox.Image != null;
        }

        private bool CalipersAllowed()
        {
            return ImageIsLoaded() || isTransparent;
        }

        private bool MeasurementsAllowed()
        {
            return theCalipers.HorizontalCalibration.CanDisplayRate;
        }

        private void ZoomIn()
        {
            ZoomBy(zoomInFactor);
        }

        private void ZoomOut()
        {
            ZoomBy(zoomOutFactor);
        }

        private void ResetZoom()
        {
            if (ecgPictureBox.Image == null)
            {
                return;
            }
            currentActualZoom = 1.0;
            Bitmap rotatedBitmap = RotateImage(theBitmap, rotationAngle, BACKGROUND_COLOR);
            Bitmap zoomedBitmap = Zoom(rotatedBitmap);
            rotatedBitmap.Dispose();
            if (ecgPictureBox.Image != null && ecgPictureBox.Image != theBitmap)
            {
                ecgPictureBox.Image.Dispose();
            }
            ecgPictureBox.Image = zoomedBitmap;
        }

        private void ZoomBy(double zoomFactor)
        {
            if (ecgPictureBox.Image == null)
            {
                return;
            }
            currentActualZoom *= zoomFactor;
            Bitmap rotatedBitmap = RotateImage(theBitmap, rotationAngle, BACKGROUND_COLOR);
            Bitmap zoomedBitmap = Zoom(rotatedBitmap);
            rotatedBitmap.Dispose();
            if (ecgPictureBox.Image != null && ecgPictureBox.Image != theBitmap)
            {
                ecgPictureBox.Image.Dispose();
            }
            ecgPictureBox.Image = zoomedBitmap;
        }

        private Bitmap Zoom(Bitmap bitmap)
        {

            theCalipers.updateCalibration(currentActualZoom);
            Size newSize = new Size((int)(bitmap.Width * currentActualZoom), (int)(bitmap.Height * currentActualZoom));
            Bitmap bmp = new Bitmap(bitmap, newSize);
            return bmp;
        }

        private void ecgPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            theCalipers.Draw(g, ecgPictureBox.DisplayRectangle);
        }

        private void KillBitmap()
        {
            if (theBitmap != null)
            {
                theBitmap.Dispose();
            }
        }

        private void ResetBitmap(Image image)
        {
            KillBitmap();
            theBitmap = new Bitmap(image);
            currentActualZoom = 1.0;
            rotationAngle = 0.0f;
            ClearCalibration();
        }

        // rotation
        private void RotateEcgImage(float angle)
        {
            if (ecgPictureBox.Image == null)
            {
                return;
            }
            rotationAngle += angle;
            Bitmap rotatedBitmap = RotateImage(theBitmap, rotationAngle, BACKGROUND_COLOR);
            // apply zoom to rotated bitmap
            Bitmap zoomedBitmap = Zoom(rotatedBitmap);
            // don't need the unzoomed rotated bitmap anymore
            rotatedBitmap.Dispose();
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
            if (theBitmap != null)
            {
                if (ecgPictureBox.Image != null && ecgPictureBox.Image != theBitmap)
                {
                    ecgPictureBox.Image.Dispose();
                }
                ecgPictureBox.Image = theBitmap;
                ResetCalibration();
                currentActualZoom = 1.0;
                rotationAngle = 0.0f;
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
        #endregion
        #region PDF
        // PDF stuff
        private void OpenPdf(string filename)
        {
            MagickReadSettings settings = new MagickReadSettings();
            settings.Density = new Density(300, 300);
            PdfReadDefines defines = new PdfReadDefines();

            using (pdfImages)
            {
                if (pdfImages == null)
                {
                    pdfImages = new MagickImageCollection();
                }
                Cursor.Current = Cursors.WaitCursor;
                // Consider using background worker here
                //Application.DoEvents();
                pdfImages.Read(filename, settings);
                Cursor.Current = Cursors.Default;
                numberOfPdfPages = pdfImages.Count;
                EnablePages(numberOfPdfPages > 1);
                currentPdfPage = 1;
                ecgPictureBox.Image = pdfImages[currentPdfPage - 1].ToBitmap();
            }
        }

        private void EnablePages(bool enable)
        {
            nextPageToolStripMenuItem1.Enabled = enable;
            previousPageToolStripMenuItem1.Enabled = enable;
        }

        private void ClearPdf()
        {
            if (pdfImages != null)
            {
                pdfImages.Dispose();
                pdfImages = null;
                numberOfPdfPages = 0;
                currentPdfPage = 0;
                EnablePages(false);
            }
        }

        private void NextPdfPage()
        {
            if (pdfImages == null || ecgPictureBox.Image == null)
            {
                return;
            }
            if (currentPdfPage < numberOfPdfPages)
            {
                currentPdfPage++;
                ecgPictureBox.Image.Dispose();
                ecgPictureBox.Image = pdfImages[currentPdfPage - 1].ToBitmap();
                ResetBitmap(ecgPictureBox.Image);
            }
        }

        private void PreviousPdfPage()
        {
            if (pdfImages == null || ecgPictureBox.Image == null)
            {
                return;
            }
            if (currentPdfPage > 1)
            {
                currentPdfPage--;
                ecgPictureBox.Image.Dispose();
                ecgPictureBox.Image = pdfImages[currentPdfPage - 1].ToBitmap();
                ResetBitmap(ecgPictureBox.Image);
            }
        }
        #endregion
        #region Menu

        #region MainMenu

        private void EnableImageMenuItems(bool value)
        {
            zoomInButton.Enabled = value;
            zoomOutButton.Enabled = value;
            zoomInToolStripMenuItem.Enabled = value;
            zoomOutToolStripMenuItem.Enabled = value;
            resetZoomToolStripMenuItem.Enabled = value;
            rotate1LToolStripMenuItem.Enabled = value;
            rotate1RToolStripMenuItem.Enabled = value;
            rotate90LToolStripMenuItem.Enabled = value;
            rotate90RToolStripMenuItem.Enabled = value;
            rotateTinyLToolStripMenuItem.Enabled = value;
            rotateTinyRToolStripMenuItem.Enabled = value;
            resetImageToolStripMenuItem1.Enabled = value;
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
            string filename = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".jpg";
            Image image = (Image)ecgPictureBox.Image.Clone();
            Graphics g = Graphics.FromImage(image);
            theCalipers.Draw(g, ecgPictureBox.DisplayRectangle);
            image.Save(filename);
            var p = new Process();
            p.StartInfo.FileName = filename;
            p.StartInfo.Verb = "Print";
            p.Start();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ecgPictureBox.Image == null)
            {
                MessageBox.Show("No image is open so how can you save?", "No Image Open");
                return;
            }
            saveFileDialog1.Filter = saveFileTypeFilter;
            saveFileDialog1.DefaultExt = "jpg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image image = (Image)ecgPictureBox.Image.Clone();
                Graphics g = Graphics.FromImage(image);
                theCalipers.Draw(g, ecgPictureBox.DisplayRectangle);
                image.Save(saveFileDialog1.FileName);
                image.Dispose();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (preferencesDialog == null)
            {
                preferencesDialog = new PreferencesDialog();
            }
            if (preferencesDialog.ShowDialog() == DialogResult.OK)
            {
                preferencesDialog.Save();
                UpdatePreferences();
            }

        }

        private void UpdatePreferences()
        {
            preferences.Load();
            // update all the calipers
            theCalipers.UpdatePreferences(preferences);
            if ((isTransparent && preferences.ShowHandlesTransparentMode) || preferences.ShowHandlesPictureMode)
            {
                // this method incorporates ecgPictureBox.Refresh()
                setShowHandles(true);
            }
            else
            {
                ecgPictureBox.Refresh();
            }
        }

        private void aboutEPCalipersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 box = new AboutBox1();
            box.ShowDialog();
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetZoom();
        }

        private void rotate90RToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            RotateEcgImage(90.0f);
        }

        private void rotate90LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateEcgImage(-90.0f);
        }

        private void rotate1RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateEcgImage(1.0f);
        }

        private void rotate1LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateEcgImage(-1.0f);
        }

        private void resetImageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResetEcgImage();
        }

        private void timeCaliperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CalipersAllowed())
            {
                return;
            }
            AddCaliper(CaliperDirection.Horizontal);
        }

        private void amplitudeCaliperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CalipersAllowed())
            {
                return;
            }
            AddCaliper(CaliperDirection.Vertical);
        }

        private void angleCaliperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CalipersAllowed())
            {
                return;
            }
            AddAngleCaliper();
        }

        private void deleteCaliperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CalipersAllowed())
            {
                return;
            }
            Caliper c = theCalipers.GetActiveCaliper();
            if (c != null)
            {
                theCalipers.deleteCaliper(c);
                ecgPictureBox.Refresh();
            }
        }

        private void deleteAllCalipersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteAllCalipers();
        }

        private void deleteAllCalipers()
        {
            if (!CalipersAllowed())
            {
                return;
            }
            theCalipers.deleteAllCalipers();
            ecgPictureBox.Refresh();
        }

        private void calibrateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoCalibration();
        }

        private void clearCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearCalibration();
        }

        private void nextPageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NextPdfPage();
        }

        private void previousPageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PreviousPdfPage();
        }

        private void toggleRateintervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            theCalipers.HorizontalCalibration.DisplayRate = !theCalipers.HorizontalCalibration.DisplayRate;
            ecgPictureBox.Refresh();

        }

        private void meanRateIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MeasureMeanIntervalRate();
        }

        private void qTcMeasurementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QTcInterval();
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(Application.StartupPath);
            Debug.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory);
            Help.ShowHelp(this, "epcalipers-help.chm");
        }

        private void rotateTinyRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateEcgImage(0.1f);
        }

        private void rotateTinyLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateEcgImage(-0.1f);
        }

        private void transparentWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            makeTransparent(transparentWindowToolStripMenuItem.Checked);
        }

        private void showHandlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showHandles(showHandlesToolStripMenuItem.Checked);
        }

        private void showHandles(bool value)
        {
            theCalipers.showHandles(value);
            ecgPictureBox.Refresh();
        }

        #endregion

        #region right-click menu
        private void caliperColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            theCalipers.UnselectChosenCaliper();
            ecgPictureBox.Refresh();
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = theCalipers.GetChosenCaliperColor();
            colorDialog.AllowFullOpen = true;
            colorDialog.CustomColors = customColors;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                theCalipers.SetChosenCaliperColor(Preferences.SneakilyAdjustColor(colorDialog.Color));
                customColors = colorDialog.CustomColors;
                ecgPictureBox.Refresh();
            }
        }

        private void tweakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(flowLayoutPanel1.Controls.Count < maxControlNumber);
            flowLayoutPanel1.Controls.CopyTo(oldControls, 0);
            flowLayoutPanel1.Controls.Clear();
            if (tweakMenu == null)
            {
                tweakMenu = new Control[] { cancelTweakButton, tweakLabel };
            }
            flowLayoutPanel1.Controls.AddRange(tweakMenu);
            CancelButton = cancelTweakButton;
            TweakCaliper();
        }

        private void TweakCaliper()
        {
            if (theCalipers.chosenComponent != CaliperComponent.NoComponent)
            {
                string componentName = theCalipers.GetChosenComponentName();
                string message = string.Format("Tweak {0} with arrow keys.  Right-click to tweak a different component.", componentName);
                tweakLabel.Text = message;
                if (!theCalipers.tweakingComponent)
                {
                    theCalipers.tweakingComponent = true;
                }
            }
            else
            {
                CancelTweaking();
            }
        }



        #endregion

        #endregion

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!theCalipers.tweakingComponent)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            switch(keyData)
            {
                case Keys.Left:
                    theCalipers.Move(MovementDirection.Left);
                    break; 
                case Keys.Right:
                    theCalipers.Move(MovementDirection.Right);
                    break;
                case Keys.Up:
                    theCalipers.Move(MovementDirection.Up);
                    break;
                case Keys.Down:
                    theCalipers.Move(MovementDirection.Down);
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            ecgPictureBox.Refresh();
            return true;
        }

      
    }
}
