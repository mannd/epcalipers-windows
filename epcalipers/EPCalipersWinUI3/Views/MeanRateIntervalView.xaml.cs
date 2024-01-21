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
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MeanRateIntervalView : Page
	{
		private static Point _meanRateIntervalWindowPosition;
        public WindowEx Window
        {
            get
            {
                return _window;
            }
            set
            {
                _window = value;
                if (_window != null && _meanRateIntervalWindowPosition != new Point(0, 0))
                {
                    _window.Move((int)_meanRateIntervalWindowPosition.X, (int)_meanRateIntervalWindowPosition.Y);
                }
            }
        }
        private WindowEx _window;

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
			// Save coordinates to open window the same place next time...
			_meanRateIntervalWindowPosition = new Point(Window.AppWindow.Position.X,
				Window.AppWindow.Position.Y);
			Debug.Print("X = " + Window.AppWindow.Position.X.ToString() + "Y = " +
				Window.AppWindow.Position.Y.ToString());
            Window?.Close();
            Window = null;
        }

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
