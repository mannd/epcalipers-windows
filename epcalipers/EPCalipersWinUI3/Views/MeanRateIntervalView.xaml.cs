using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.Foundation;
using Windows.System;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
	public sealed partial class MeanRateIntervalView : Page
	{
		private bool _forQtcMeasurement = false;
		public WindowEx Window { get; set; }
		public QtcParameters QtcParameters { get; set; }	

		public MeasureIntervalViewModel ViewModel { get; set; }

		public MeanRateIntervalView(Caliper caliper, CaliperCollection caliperCollection)
		{
			InitializeComponent();
			ViewModel = new MeasureIntervalViewModel(caliperCollection);
		}

		public MeanRateIntervalView()
		{
			InitializeComponent();
		}

		// Only navigated to in context of QTc measurement.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_forQtcMeasurement = true;
			base.OnNavigatedTo(e);
			QtcParameters = e.Parameter as QtcParameters;
			var caliperCollection = QtcParameters.CaliperCollection;
			var numberOfIntervals = QtcParameters.NumberOfIntervals;
			ViewModel = new MeasureIntervalViewModel(caliperCollection, numberOfIntervals);
			ViewModel.Title = "Measure RR Interval";
			ViewModel.RateVisibility = Visibility.Collapsed;
			QtcParameters.IntervalMeasured = Models.Calipers.IntervalMeasured.RR;
			ViewModel.QtcParameters = QtcParameters;
			ViewModel.GetResults();
		}

		private void MeanRateIntervalViewCancel_Click(object sender, RoutedEventArgs e)
		{
			CloseWindow();
		}

		private void CloseWindow()
		{
			if (_forQtcMeasurement)
			{
				_forQtcMeasurement = false;

				Frame frame = QtcParameters.Window?.Content as Frame;
				if (frame != null && frame.CanGoBack)
				{
					frame.GoBack();
				}
			} 
			else
			{
				Window?.Close();
			}
		}

		private void Page_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			switch (e.Key)
			{
				// Note if focus in on the number picker, it will suck up the keystrokes and 
				// this won't close the window.  If not, the window closes fine.
				case VirtualKey.Escape: CloseWindow(); break;
				default: break;
			}
		}
	}
}
