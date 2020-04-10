﻿using epcalipers.Properties;
using ImageMagick;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Runtime.InteropServices;
using EPCalipersCore;
using EPCalipersCore.Properties;
using System.Globalization;

namespace epcalipers
{
    public partial class Form1 : Form
    {
        #region Fields
        Bitmap theBitmap;
        BaseCalipers theCalipers;
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
        MeasureRRDialog measureRRDialog = new MeasureRRDialog();
        CalibrationDialog calibrationDialog = new CalibrationDialog();
        GotoPDFPageForm gotoPdfPageForm = new GotoPDFPageForm();

        float rotationAngle = 0.0f;
        Color BACKGROUND_COLOR = Color.Transparent;
        Point firstPoint;

        string openFileTypeFilter = "Image or PDF files | *.jpg; *.bmp; *.png; *.pdf";
        string saveFileTypeFilter = "Image files (*.jpg, *.bmp *.png) | *.jpg; *.bmp; *.png";

        // Note zoom factors used in Mac OS X version
        // These are taken from the Apple IKImageView demo
        double zoomInFactor = 1.414214;
        double zoomOutFactor = 0.7071068;
        double currentActualZoom = 1.0;
        double maximumZoom = 5.0;

        // Drag and drop variables
        private Thread getImageThread;
        private bool validData;
        // private DragDropEffects effects;
        private string lastFilename = "";
        private Image image;
        private Image nextImage;
        private int lastX = 0;
        private int lastY = 0;

        // PDF stuff
        private MagickImageCollection pdfImages = null;
        int numberOfPdfPages = 0;
        int currentPdfPage = 0;
        #endregion
        #region Initialization
        public Form1()
        {
            InitializeComponent();
            preferences = new Preferences();
            theCalipers = new BaseCalipers();

            ecgPictureBox.BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.Sizable;

            ecgPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            ecgPictureBox.Paint += ecgPictureBox_Paint;
            ecgPictureBox.MouseDown += ecgPictureBox_MouseDown;
            ecgPictureBox.MouseDoubleClick += ecgPictureBox_MouseDoubleClick;
            ecgPictureBox.MouseUp += ecgPictureBox_MouseUp;

            // Ghostscript location
            string ghostscriptDir = AppDomain.CurrentDomain.BaseDirectory;
            MagickNET.SetGhostscriptDirectory(ghostscriptDir);

            SetupButtons();
            // form starts with no image loaded, so no pages either
            EnablePages(false);

            try
            {
                if (Environment.GetCommandLineArgs().Length > 1)
                {
                    string arg1 = Environment.GetCommandLineArgs()[1];
                    string ext = Path.GetExtension(arg1);
                    if (IsValidFileType(ext, true))
                    {
                        lastFilename = arg1;
                        if (ext.ToUpperInvariant() == ".PDF")
                        {
                            OpenPdf(lastFilename);
                        }
                        else
                        {
                            Image argImage = new Bitmap(lastFilename);
                            ecgPictureBox.Image = argImage;
                        }
                        ResetBitmap(ecgPictureBox.Image);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is MagickException)
                {
                    lastFilename = "";
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                ShowMainMenu();
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (theBitmap != null) theBitmap.Dispose();
                if (pdfImages != null) pdfImages.Dispose();
                if (nextImage != null) nextImage.Dispose();
                if (imageButton != null) imageButton.Dispose();
                if (addCalipersButton != null) addCalipersButton.Dispose();
                if (calibrateButton != null) calibrateButton.Dispose();
                if (intervalRateButton != null) intervalRateButton.Dispose();
                if (measureRRForQtcButton != null) measureRRForQtcButton.Dispose();
                if (setCalibrationButton != null) setCalibrationButton.Dispose();
                if (clearCalibrationButton != null) clearCalibrationButton.Dispose();
                if (backCalibrationButton != null) backCalibrationButton.Dispose();
                if (meanRRButton != null) meanRRButton.Dispose();
                if (qtcButton != null) qtcButton.Dispose();
                if (cancelButton != null) cancelButton.Dispose();
                if (measureQTcButton != null) measureQTcButton.Dispose();
                if (cancelTweakButton != null) cancelTweakButton.Dispose();
                if (measureQtcMessageLabel != null) measureQtcMessageLabel.Dispose();
                if (measureRRForQtcMessageLabel != null) measureRRForQtcMessageLabel.Dispose();
                if (tweakLabel != null) tweakLabel.Dispose();
                if (preferencesDialog != null) preferencesDialog.Dispose();
                if (measureRRDialog != null) measureRRDialog.Dispose();
                if (calibrationDialog != null) calibrationDialog.Dispose();
                if (gotoPdfPageForm != null) gotoPdfPageForm.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void SetupButtons()
        {
            imageButton = new Button();
            InitButton(imageButton, "Open", "Open ECG image file or PDF", imageButton_Click);

            addCalipersButton = new Button();
            InitButton(addCalipersButton, "Add Caliper", "Add new caliper", addCaliper_Click);

            calibrateButton = new Button();
            InitButton(calibrateButton, "Calibration", "Calibrate, recalibrate or clear calibration", calibrateButton_Click);

            setCalibrationButton = new Button();
            InitButton(setCalibrationButton, "Set Calibration", "Set calibration interval", setCalibrationButton_Click);

            clearCalibrationButton = new Button();
            InitButton(clearCalibrationButton, "Clear Calibration", "Clear all calibration", clearCalibrationButton_Click);

            backCalibrationButton = new Button();
            InitButton(backCalibrationButton, "Done", "Done with calibration", backCalibrationButton_Click);

            intervalRateButton = new Button();
            InitButton(intervalRateButton, "Rate/Int", "Toggle between rate and interval", intervalRateButton_Click);

            measureRRForQtcButton = new Button();
            InitButton(measureRRForQtcButton, "Measure", "Measure 1 or more RR intervals for QTc", 
                MeasureRRForQtcButton_Click);

            measureQTcButton = new Button();
            InitButton(measureQTcButton, "Measure", "Measure QT interval", MeasureQTcButton_Click);

            meanRRButton = new Button();
            InitButton(meanRRButton, "Mean Rate", "Measure mean rate and interval", MeanRRButton_Click);

            qtcButton = new Button();
            InitButton(qtcButton, "QTc", "Measure corrected QT (QTc)", QtcButton_Click);

            cancelButton = new Button();
            InitButton(cancelButton, "Cancel", "Cancel measurement", CancelButton_Click);

            cancelTweakButton = new Button();
            InitButton(cancelTweakButton, "Done", "Cancel tweaking", CancelTweakButton_Click);

            measureRRForQtcMessageLabel = new Label
            {
                Text = Resources.measureRRForQtcMessageText
            };
            AdjustLabel(measureRRForQtcMessageLabel);
            measureQtcMessageLabel = new Label
            {
                Text = Resources.measureQTText
            };
            AdjustLabel(measureQtcMessageLabel);
            tweakLabel = new Label();
            // tweakLabel text is changed on the fly
            AdjustLabel(tweakLabel);
        }

        private void InitButton(Button button, String label, String toolTip, EventHandler onClickFunc)
        {
            button.Text = label;
            toolTip1.SetToolTip(button, toolTip);
            button.Click += onClickFunc;
            button.AutoSize = true;
        }
 
        private static void AdjustLabel(Label label)
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
            CommonCaliper.MeasureQTc(theCalipers, ShowMainMenu, ShowQTcStep2Menu, preferences);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            ShowMainMenu();
        }

        private void QtcButton_Click(object sender, EventArgs e)
        {
            DisableCalibrationAndMeasurements();
            CommonCaliper.QTcInterval(theCalipers, ImageRefresh, ShowQTcStep1Menu, ShowMainMenu);
        }

        private void MeanRRButton_Click(object sender, EventArgs e)
        {
            CommonCaliper.MeasureMeanIntervalRate(theCalipers, ImageRefresh, measureRRDialog, preferences);
        }

        private void MeasureRRForQtcButton_Click(object sender, EventArgs e)
        {
            CommonCaliper.MeasureRRForQTc(theCalipers, measureRRDialog, ShowMainMenu, ShowQTcStep2Menu, preferences);
        }

        private void clearCalibrationButton_Click(object sender, EventArgs e)
        {
            CommonCaliper.ClearCalibration(theCalipers, ImageRefresh, EnableMeasurementMenuItems);
        }

        private void backCalibrationButton_Click(object sender, EventArgs e)
        {
            ShowMainMenu();
        }

        private void imageButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = openFileTypeFilter;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                try
                {
                    {
                        if (Path.GetExtension(openFileDialog1.FileName).ToUpperInvariant() == ".PDF")
                        {
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
                    if (exception is FileNotFoundException || exception is MagickException)
                    {
                        MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resources.couldNotOpenFileErrorText, openFileDialog1.FileName) + "\n\nDetailed error: " +
                            exception.Message, Resources.errorTitleText);
                        return;
                    }
                    throw;
                }
        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {
            DoCalibration();
        }

        private void setCalibrationButton_Click(object sender, EventArgs e)
        {
            SetCalibration();
        }

        private void SetCalibration()
        {
            CommonCaliper.SetCalibration(theCalipers, preferences, calibrationDialog, currentActualZoom,
                ImageRefresh, ShowMainMenu);
        }

        private void addCaliper_Click(object sender, EventArgs e)
        {
            if (ecgPictureBox.Image == null)
            {
               return;
            }
            CommonCaliper.PickAndAddCaliper(theCalipers, SetupCaliper);
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
            flowLayoutPanel1.Refresh();
            foreach (Control c in oldControls)
            {
                flowLayoutPanel1.Controls.Add(c);
            }
            theCalipers.CancelTweaking();
            ecgPictureBox.Refresh();
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
                    bool pointNearCaliper = theCalipers.PointIsNearCaliper(clickPoint);
                    contextMenuStrip1.Enabled = pointNearCaliper;
                    if (pointNearCaliper)
                    {
                        BaseCaliper c = theCalipers.getGrabbedCaliper(clickPoint);
                        if (c != null)
                        {
                            marchingCaliperToolStripMenuItem.Checked = c.isMarching && c.isTimeCaliper();
                            marchingCaliperToolStripMenuItem.Enabled = c.isTimeCaliper();
                        }
                    }
                    else
                    {
                        marchingCaliperToolStripMenuItem.Checked = false;
                    }
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
                if (exception is FileNotFoundException || exception is MagickException)
                {
                    MessageBox.Show(Resources.errorOpeningFileText + exception.Message, Resources.errorTitleText);
                    return;
                }
                throw;
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("OnDragEnter");
            validData = GetFilename(out string filename, e);
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

        protected static bool GetFilename(out string filename, DragEventArgs e)
        {
            bool result = false;
            filename = String.Empty;

            if (e == null)
            {
                return false;
            }

            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                if (((IDataObject)e.Data).GetData("FileDrop") is Array data)
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

        private static bool IsValidFileType(string fileExtension, bool allowPDF)
        {
            string ext = fileExtension.ToUpperInvariant();
            return ((ext == ".JPG") || (ext == ".JPEG") || (ext == ".TIFF") || (ext == ".TIF") || (ext == ".PNG") || (ext == ".BMP") || (allowPDF && ext == ".PDF"));
        }

        private bool FileIsPdf(string fileName)
        {
            try
            {
                return Path.GetExtension(fileName).ToUpperInvariant() == ".PDF";
            }
            catch (ArgumentException)
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
            catch (FileNotFoundException exception)
            {
                MessageBox.Show("Exception in LoadImage: " + exception.Message, Resources.errorTitleText);
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

        private void DisableCalibrationAndMeasurements()
        {
            calibrateToolStripMenuItem.Enabled = false;
            clearCalibrationToolStripMenuItem.Enabled = false;
            toggleRateintervalToolStripMenuItem.Enabled = false;
            meanRateIntervalToolStripMenuItem.Enabled = false;
            qTcMeasurementToolStripMenuItem.Enabled = false;
        }

        private void ShowQTcStep1Menu()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Refresh();
            if (qtcStep1Menu == null)
            {
                qtcStep1Menu = new Control[] { cancelButton, measureRRForQtcButton, measureRRForQtcMessageLabel };
            }
            flowLayoutPanel1.Controls.AddRange(qtcStep1Menu);
            CancelButton = cancelButton;
        }

        private void ShowQTcStep2Menu()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Refresh();
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
            flowLayoutPanel1.Refresh();
            if (mainMenu == null)
            {
                mainMenu = new Control[] { qtcButton, meanRRButton,
                    intervalRateButton, calibrateButton,
                    addCalipersButton, imageButton  };

            }
            flowLayoutPanel1.Controls.AddRange(mainMenu);
            EnableImageMenuItems(ImageIsLoaded());
            EnableCaliperMenuItems(CalipersAllowed());
            EnableMeasurementMenuItems(CommonCaliper.MeasurementsAllowed(theCalipers));
            imageButton.Select();
        }

        private void EnableCaliperMenuItems(bool enable)
        {
            addCalipersButton.Enabled = enable;
            calibrateButton.Enabled = enable;
            timeCaliperToolStripMenuItem.Enabled = enable;
            amplitudeCaliperToolStripMenuItem.Enabled = enable;
            angleCaliperToolStripMenuItem.Enabled = enable;
            deleteCaliperToolStripMenuItem.Enabled = enable;
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
            flowLayoutPanel1.Refresh();
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
            if (CommonCaliper.NoCalipersError(theCalipers.NumberOfCalipers()))
            {
                return;
            }
            ShowCalibrationMenu();
            if (theCalipers.SelectCaliperIfNoneSelected())
            {
                ecgPictureBox.Refresh();
            }
        }

        private void ImageRefresh()
        {
            ecgPictureBox.Refresh();
        }

        private void SetupCaliper(Caliper c)
        {
            c.Setup(preferences);
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
            return ImageIsLoaded();
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
            if (currentActualZoom > maximumZoom)
            {
                currentActualZoom = maximumZoom;
                // no further processing, or can get memory overflow
                return;
            }
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
            try
            {
                theCalipers.updateCalibration(currentActualZoom);
                Size newSize = new Size((int)(bitmap.Width * currentActualZoom), (int)(bitmap.Height * currentActualZoom));
                Bitmap bmp = new Bitmap(bitmap, newSize);
                return bmp;
            }
            // Note that new Bitmap only throws a general Exception, so impossible to fulfill warning below.
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                MessageBox.Show(Resources.couldNotResizeText);
                return null;
            }
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
            CommonCaliper.ClearCalibration(theCalipers, ImageRefresh, EnableMeasurementMenuItems);
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
            CommonCaliper.ResetCalibration(theCalipers, EnableMeasurementMenuItems);

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
                CommonCaliper.ResetCalibration(theCalipers, EnableMeasurementMenuItems);
                currentActualZoom = 1.0;
                rotationAngle = 0.0f;
                // not necessary to refresh image when image updated
                //ecgPictureBox.Refresh();
            }
        }

        // code based on http://stackoverflow.com/questions/14184700/how-to-rotate-image-x-degrees-in-c
        private static Bitmap RotateImage(Bitmap bmp, float angle, Color bkColor)
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
            MagickReadSettings settings = new MagickReadSettings
            {
                Density = new Density(300, 300)
            };
            //PdfReadDefines defines = new PdfReadDefines();

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
            gotoPDFPageToolStripMenuItem.Enabled = enable;
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
            if (NoPdfIsLoaded())
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
            if (NoPdfIsLoaded())
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

        private void GotoPdfPage()
        {
            if (NoPdfIsLoaded())
            {
                return;
            }
            gotoPdfPageForm.pdfPageUpDown.Value = currentPdfPage;
            DialogResult result = gotoPdfPageForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                int value = (int)gotoPdfPageForm.pdfPageUpDown.Value;
                int pageNumber = value;
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }
                if (pageNumber > numberOfPdfPages)
                {
                    pageNumber = numberOfPdfPages;
                }
                currentPdfPage = pageNumber;
                ecgPictureBox.Image.Dispose();
                ecgPictureBox.Image = pdfImages[currentPdfPage - 1].ToBitmap();
                ResetBitmap(ecgPictureBox.Image);
            }
        }

        private bool NoPdfIsLoaded()
        {
            return pdfImages == null || ecgPictureBox.Image == null;
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
                MessageBox.Show(Resources.noImageToPrintText, Resources.noImageOpenTitleText);
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
            p.Dispose();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ecgPictureBox.Image == null)
            {
                MessageBox.Show(Resources.noImageToSaveText, Resources.noImageOpenTitleText);
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
            if (CommonCaliper.GetDialogResult(preferencesDialog) == DialogResult.OK)
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
            ecgPictureBox.Refresh();
        }

        private void aboutEPCalipersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 box = new AboutBox1();
            CommonCaliper.GetDialogResult(box);
            box.Dispose();
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
            CommonCaliper.AddCaliper(theCalipers, CaliperDirection.Horizontal, SetupCaliper);
        }

        private void amplitudeCaliperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CalipersAllowed())
            {
                return;
            }
            CommonCaliper.AddCaliper(theCalipers, CaliperDirection.Vertical, SetupCaliper);
        }

        private void angleCaliperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CalipersAllowed())
            {
                return;
            }
            CommonCaliper.AddAngleCaliper(theCalipers, SetupCaliper);
        }

        private void deleteCaliperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CalipersAllowed())
            {
                return;
            }
            BaseCaliper c = theCalipers.GetActiveCaliper();
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
            SetCalibration();
        }

        private void clearCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonCaliper.ClearCalibration(theCalipers, ImageRefresh, EnableMeasurementMenuItems);
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
            CommonCaliper.MeasureMeanIntervalRate(theCalipers, ImageRefresh, measureRRDialog, preferences);
        }

        private void qTcMeasurementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableCalibrationAndMeasurements();
            CommonCaliper.QTcInterval(theCalipers, ImageRefresh, ShowQTcStep1Menu, ShowMainMenu);
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(Application.StartupPath);
            Debug.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory);
            Help.ShowHelp(this, System.AppDomain.CurrentDomain.BaseDirectory + "epcalipers-help.chm");
        }

        private void rotateTinyRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateEcgImage(0.1f);
        }

        private void rotateTinyLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateEcgImage(-0.1f);
        }

        private void transparentWindowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // see https://stackoverflow.com/questions/16511382/open-a-wpf-window-from-winforms-link-form-app-with-wpf-app
            var transWindow = new WpfTransparentWindow.Window1();
            ElementHost.EnableModelessKeyboardInterop(transWindow);
            transWindow.Show();
            transWindow.Dispose();
        }

        private void gotoPDFPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GotoPdfPage();
        }

        #endregion
        #region right-click menu
        private void caliperColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            theCalipers.ClearAllChosenComponents();
            CommonCaliper.SelectCaliperColor(theCalipers, ImageRefresh);
        }

        private void tweakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(flowLayoutPanel1.Controls.Count < maxControlNumber);
            flowLayoutPanel1.Controls.CopyTo(oldControls, 0);
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Refresh();
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
            theCalipers.ClearAllChosenComponentsExceptForChosenCaliper();
            if (theCalipers.chosenComponent != CaliperComponent.NoComponent)
            {
                string componentName = theCalipers.GetChosenComponentName();
                string message = string.Format(CultureInfo.CurrentCulture, Resources.tweakText, componentName);
                tweakLabel.Text = message;
                if (!theCalipers.tweakingComponent)
                {
                    theCalipers.tweakingComponent = true;
                }
                ecgPictureBox.Refresh();
            }
            else
            {
                CancelTweaking();
            }
        }

        private void marchingCaliperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            theCalipers.ClearAllChosenComponents();
            if (theCalipers.MarchCaliper())
            {
                ecgPictureBox.Refresh();
                marchingCaliperToolStripMenuItem.Checked = true;
            }
            else
            {
                marchingCaliperToolStripMenuItem.Checked = false;
            }
        }

        #endregion

        #endregion
        #region Keys
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
                case Keys.Left | Keys.Control:
                    theCalipers.MicroMove(MovementDirection.Left);
                    break; 
                case Keys.Right | Keys.Control:
                    theCalipers.MicroMove(MovementDirection.Right);
                    break;
                case Keys.Up | Keys.Control:
                    theCalipers.MicroMove(MovementDirection.Up);
                    break;
                case Keys.Down | Keys.Control:
                    theCalipers.MicroMove(MovementDirection.Down);
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            ecgPictureBox.Refresh();
            return true;
        }


        #endregion

   
    }
}
