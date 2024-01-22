using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using WinUIEx;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.ViewModels;
using Windows.System;
using Windows.Foundation;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
	public sealed partial class MeanRateIntervalView : Page
	{
		private static Point _meanRateIntervalWindowPosition;
        public WindowEx Window { get; set; }

        public MeanRateIntervalViewModel ViewModel { get; set; }

        public MeanRateIntervalView(Caliper caliper)
        {
            InitializeComponent();
            ViewModel = new MeanRateIntervalViewModel();
        }

        private void MeanRateIntervalViewCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void MeanRateIntervalViewMeasure_Click(object sender, RoutedEventArgs e)
        {
            //await ViewModel.SetCalibration(XamlRoot);
            CloseWindow();
        }

        private void CloseWindow()
        {
            Window?.Close();
        }

        // TODO: Why aren't keys being detected here?
        private void Page_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Enter:
                    ViewModel.NumberOfIntervals = (int)NumberOfIntervalsBox.Value;
                    //await ViewModel.SetCalibration(XamlRoot);
                    CloseWindow();
                    break;
                case VirtualKey.Escape: CloseWindow(); break;
                default: break;
            }
        }
	}
}