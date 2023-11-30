using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using WinUIEx;
using EPCalipersWinUI3.Models.Calipers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalibrationDialog : Page
    {
        public WindowEx Window { get; set; }
        public CaliperCollection CaliperCollection { get; set; }
        public CaliperType CaliperType { get; set; }

        public CalibrationDialog()
        {
            this.InitializeComponent();
            EnableCustomText();
        }

        public void EnableCustomText()
        {
            CustomIntervalTextBox.IsEnabled = CustomRadioButton.IsChecked ?? false;
        }

		private void CalibrationRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            EnableCustomText();
		}

		private void CalibrationDialogCancel_Click(object sender, RoutedEventArgs e)
		{
            if (Window != null)
            {
                Window.Close();
            }
		}

		private void CalibrationDialogCalibrate_Click(object sender, RoutedEventArgs e)
		{
            if (CaliperCollection != null)
            {
                // TODO: Get Calibration from dialog, set time or amplitude calibration of 
                // caliber collection.
            }
            if (Window != null)
            {
                Window.Close();
            }

		}
	}
}
