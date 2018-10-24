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

        public static DialogResult GetDialogResult(Form dialog) {
            return dialog.ShowDialog();
        }
            
        public delegate void SetupCaliperMethod(Caliper c);

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
            Caliper c = new Caliper();
            c.Direction = direction;
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
            AngleCaliper c = new AngleCaliper();
            c.Direction = CaliperDirection.Horizontal;
            c.CurrentCalibration = calipers.HorizontalCalibration;
            c.VerticalCalibration = calipers.VerticalCalibration;
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
    }
}
