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
using EPCalipersWinUI3.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalibrationView : Page
    {
        public WindowEx Window { get; set; }
        public CaliperType CaliperType { get; set; }
        public CalibrationViewModel ViewModel { get; set; }

        public CalibrationView(Caliper caliper, CaliperCollection caliperCollection)
        {
            this.InitializeComponent();
            EnableCustomText();
            CaliperType = caliper.CaliperType;
            ViewModel = new CalibrationViewModel(caliper, caliperCollection);
        }

        public void EnableCustomText()
        {
            CustomIntervalTextBox.IsEnabled = CustomRadioButton.IsChecked ?? false;
        }

		private void CalibrationRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            EnableCustomText();
			if (sender is RadioButtons rb)
			{
				int selection = rb.SelectedIndex;
				ViewModel.IntervalSelection = selection;
			}
		}

		private void CalibrationDialogCancel_Click(object sender, RoutedEventArgs e)
		{
            if (Window != null)
            {
                Window.Close();
                Window = null;
            }
		}

		private async void CalibrationDialogCalibrate_Click(object sender, RoutedEventArgs e)
		{
            await ViewModel.SetCalibration(XamlRoot);
            if (Window != null)
            {
                Window.Close();
                Window = null;
            }

		}
	}
}
