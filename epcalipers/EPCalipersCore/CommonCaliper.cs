using EPCalipersCore.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPCalipersCore
{
    // Any common static methods I can extract will go here
    public class CommonCaliper
    {
        public static DialogResult GetDialogResult(Form dialog) {
            return dialog.ShowDialog();
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

        public delegate void Refresh();

        public static void SetCalibration(ICalipers calipers, Preferences preferences, 
            CalibrationDialog calibrationDialog, Refresh refresh)
        {
            try
            {
                if (CommonCaliper.NoCalipersError(calipers.NumberOfCalipers()))
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
                if (calibrationDialog == null)
                {
                    calibrationDialog = new CalibrationDialog();
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
                if (CommonCaliper.GetDialogResult(calibrationDialog) == System.Windows.Forms.DialogResult.OK)
                {
                    Calibrate(calibrationDialog.calibrationMeasurementTextBox.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Calibration Error");
            }
        }

        private static void Calibrate(string rawCalibration) { }

    }
}
