using EPCalipersCore.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPCalipersCore
{
    // Any common static methods I can extract will go here
    public class CommonCaliper
    {
        public delegate void RefreshCaliperScreen();
        public delegate void ShowMainMenu();
        public delegate void ToggleMeasurementItems(bool value);
        public delegate void ShowQTcStep1Menu();
        public delegate void ShowQTcStep2Menu();
        public delegate void SetupCaliperMethod(Caliper c);

        public static DialogResult GetDialogResult(Form dialog) {
            return dialog.ShowDialog();
        }
        private static double rrIntervalForQtc;

        private static int[] customColors;

        public static void PickAndAddCaliper(ICalipers calipers, SetupCaliperMethod setupCaliperMethod)
        {
            var dialog = new NewCaliperDialog();
            if (GetDialogResult(dialog) == DialogResult.OK)
            {
                CaliperDirection direction;
                if (dialog.horizontalCaliperRadioButton.Checked)
                {
                    direction = CaliperDirection.Horizontal;
                    AddCaliper(calipers, direction, setupCaliperMethod);
                }
                else if (dialog.VerticalCaliperRadioButton.Checked)
                {
                    direction = CaliperDirection.Vertical;
                    AddCaliper(calipers, direction, setupCaliperMethod);
                }
                else    
                {
                    AddAngleCaliper(calipers, setupCaliperMethod);
                }
            }

        }
            

        public static void AddCaliper(ICalipers calipers, CaliperDirection direction, SetupCaliperMethod setupCaliperMethod)
        {
            Caliper c = new Caliper
            {
                Direction = direction
            };
            if (direction == CaliperDirection.Horizontal)
            {
                c.CurrentCalibration = calipers.HorizontalCalibration;
            }
            else
            {
                c.CurrentCalibration = calipers.VerticalCalibration;
            }
            setupCaliperMethod(c);
        }

        public static void AddAngleCaliper(ICalipers calipers, SetupCaliperMethod setupCaliperMethod)
        {
            AngleCaliper c = new AngleCaliper
            {
                Direction = CaliperDirection.Horizontal,
                CurrentCalibration = calipers.HorizontalCalibration,
                VerticalCalibration = calipers.VerticalCalibration
            };
            setupCaliperMethod(c);
        }

        public static bool NoCalipersError(int numberOfCalipers)
        {
            bool noCalipers = false;
            if (numberOfCalipers < 1)
            {
                ShowNoCalipersDialog();
                noCalipers = true; ;
            }
            return noCalipers;
        }

        private static void ShowNoCalipersDialog()
        {
            MessageBox.Show("Add one or more calipers first before proceeding.",
                "No Calipers To Use");
        }

        public static void SetCalibration(ICalipers calipers, Preferences preferences, 
            CalibrationDialog calibrationDialog, double currentActualZoom, RefreshCaliperScreen refresh, 
            ShowMainMenu showMainMenu)
        {
            Debug.Assert(calibrationDialog != null);
            try
            {
                if (NoCalipersError(calipers.NumberOfCalipers()))
                {
                    return;
                }
                if (calipers.NoCaliperIsSelected())
                {
                    if (calipers.NumberOfCalipers() == 1)
                    {
                        // assume user wants to calibrate sole caliper so select it
                        calipers.SelectSoleCaliper();
                        refresh();
                    }
                    else
                    {
                        MessageBox.Show("Select (by single-clicking it) the caliper that you want to calibrate, and then set it to a known interval.",
                            "No Caliper Selected");
                        return;
                    }
                }
                BaseCaliper c = calipers.GetActiveCaliper();
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
                    if (calipers.HorizontalCalibration.CalibrationString == null)
                    {
                        calipers.HorizontalCalibration.CalibrationString = preferences.HorizontalCalibration;
                    }
                    calibrationDialog.calibrationMeasurementTextBox.Text = calipers.HorizontalCalibration.CalibrationString;
                }
                else
                {
                    if (calipers.VerticalCalibration.CalibrationString == null)
                    {
                        calipers.VerticalCalibration.CalibrationString = preferences.VerticalCalibration;
                    }
                    calibrationDialog.calibrationMeasurementTextBox.Text = calipers.VerticalCalibration.CalibrationString;
                }
                if (GetDialogResult(calibrationDialog) == DialogResult.OK)
                {
                    Calibrate(calibrationDialog.calibrationMeasurementTextBox.Text, calipers, currentActualZoom, 
                        refresh, showMainMenu);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Calibration Error");
            }
        }

        private static void Calibrate(string rawCalibration, ICalipers calipers,
            double currentActualZoom, RefreshCaliperScreen refresh, ShowMainMenu showMainMenu)
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
                BaseCaliper c = calipers.GetActiveCaliper();
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
                    calibration = calipers.HorizontalCalibration;
                }
                else
                {
                    calibration = calipers.VerticalCalibration;
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
                refresh();
                // return to main menu after successful calibration
                // = behavior in mobile versions
                showMainMenu();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Calibration Error");
            }
        }

        public static bool MeasurementsAllowed(ICalipers calipers)
        {
            return calipers.HorizontalCalibration.CanDisplayRate;
        }

        public static void ResetCalibration(ICalipers calipers, ToggleMeasurementItems toggleMeasurementItems)
        {
            if (calipers.HorizontalCalibration.Calibrated ||
                calipers.VerticalCalibration.Calibrated)
            {
                calipers.HorizontalCalibration.Reset();
                calipers.VerticalCalibration.Reset();
                toggleMeasurementItems(false);
            }
        }

        public static void ClearCalibration(ICalipers calipers, RefreshCaliperScreen refresh, 
            ToggleMeasurementItems toggle)
        {
            ResetCalibration(calipers, toggle);
            refresh();
        }

        public static void NoTimeCaliperError()
        {
            MessageBox.Show("Select a time caliper.", "Measurement Error");
        }

        public static void MeasureMeanIntervalRate(ICalipers calipers, RefreshCaliperScreen refreshCaliperScreen,
            MeasureRRDialog measureRRDialog, Preferences preferences)
        {
            Debug.Assert(measureRRDialog != null);
            if (NoCalipersError(calipers.NumberOfCalipers()))
            {
                return;
            }
            BaseCaliper singleHorizontalCaliper = calipers.GetLoneTimeCaliper();
            if (singleHorizontalCaliper != null)
            {
                calipers.SelectCaliper(singleHorizontalCaliper);
                calipers.UnselectCalipersExcept(singleHorizontalCaliper);
                refreshCaliperScreen();
            }
            if (calipers.NoCaliperIsSelected())
            {
                NoTimeCaliperError();
                return;
            }
            BaseCaliper c = calipers.GetActiveCaliper();
            if (c.Direction == CaliperDirection.Vertical || c.isAngleCaliper)
            {
                NoTimeCaliperError();
                return;
            }
            measureRRDialog.numericUpDown.Value = preferences.NumberOfIntervalsMeanRR;
            if (GetDialogResult(measureRRDialog) == DialogResult.OK)
            {
                int value = (int)measureRRDialog.numericUpDown.Value;
                try
                {
                    Tuple<double, double> tuple = getMeanRRMeanRate(value, c);
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

        public static Tuple<double, double> getMeanRRMeanRate(int value, BaseCaliper c)
        {
            int divisor = value;
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

        public static void SelectCaliperColor(ICalipers calipers, RefreshCaliperScreen refreshCaliperScreen)
        {
            calipers.UnselectChosenCaliper();
            refreshCaliperScreen();
            ColorDialog colorDialog = new ColorDialog
            {
                Color = calipers.GetChosenCaliperColor(),
                AllowFullOpen = true,
                CustomColors = customColors
            };
            // color dialogs always float, even if Form is TopMost
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                calipers.SetChosenCaliperColor(colorDialog.Color);
                customColors = colorDialog.CustomColors;
                refreshCaliperScreen();
            }
        }

        #region QTc

        public static void QTcInterval(ICalipers calipers, RefreshCaliperScreen refreshCaliperScreen, 
            ShowQTcStep1Menu showQTcStep1Menu, ShowMainMenu showMainMenu)
        {
            if (NoCalipersError(calipers.NumberOfCalipers()))
            {
                return;
            }
            calipers.HorizontalCalibration.DisplayRate = false;
            refreshCaliperScreen();
            BaseCaliper singleHorizontalCaliper = calipers.GetLoneTimeCaliper();
            if (singleHorizontalCaliper != null)
            {
                calipers.SelectCaliper(singleHorizontalCaliper);
                calipers.UnselectCalipersExcept(singleHorizontalCaliper);
                refreshCaliperScreen();
            }
            if (calipers.NoTimeCaliperSelected())
            {
                NoTimeCaliperError();
                showMainMenu();
            }
            else
            {
                showQTcStep1Menu();
            }
        }

        public static void MeasureRRForQTc(ICalipers calipers, MeasureRRDialog measureRRDialog, ShowMainMenu showMainMenu,
            ShowQTcStep2Menu showQTcStep2Menu, Preferences preferences)
        {
            Debug.Assert(measureRRDialog != null);
            if (calipers.NoTimeCaliperSelected())
            {
                NoTimeCaliperError();
                return;
            }
            measureRRDialog.numericUpDown.Value = preferences.NumberOfIntervalsQtc;
            if (GetDialogResult(measureRRDialog) == DialogResult.OK)
            {
                try
                {
                    int value = (int)measureRRDialog.numericUpDown.Value;
                    Tuple<double, double> tuple = getMeanRRMeanRate(value, calipers.GetActiveCaliper());
                    rrIntervalForQtc = tuple.Item1;
                    showQTcStep2Menu();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Measurement Error");
                    showMainMenu();
                }
            }
        }

        public static void MeasureQTc(ICalipers calipers, ShowMainMenu showMainMenu,
            ShowQTcStep2Menu showQTcStep2Menu, Preferences preferences)
        {
            if (calipers.NoTimeCaliperSelected())
            {
                NoTimeCaliperError();
                return;
            }
            BaseCaliper c = calipers.GetActiveCaliper();
            if (c == null)
            {
                return;
            }
            double qt = Math.Abs(c.IntervalInSecs(c.IntervalResult()));
            double meanRR = Math.Abs(c.IntervalInSecs(rrIntervalForQtc));
            string result = "Invalid Result";
            if (meanRR > 0)
            {
                QtcCalculator calc = new QtcCalculator(preferences.ActiveQtcFormula());
                result = calc.Calculate(qt, meanRR, c.CurrentCalibration.UnitsAreMsecs, c.CurrentCalibration.Units);
            }
            if (MessageBox.Show(result, "Calculated QTc", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
            {
                showQTcStep2Menu();
            }
            else
            {
                showMainMenu();
            }
        }
        #endregion
    }
}
