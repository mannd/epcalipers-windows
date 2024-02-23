using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using Windows.System;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MeasureIntervalView : Page
	{
		MeasureIntervalViewModel ViewModel { get; set; }

		private bool _forQtcMeasurement = false;
		public WindowEx Window { get; set; }
		public QtcParameters QtcParameters { get; set; }	

		public MeasureIntervalView()
		{
			this.InitializeComponent();
			ViewModel = new MeasureIntervalViewModel();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_forQtcMeasurement = true;
			base.OnNavigatedTo(e);
			QtcParameters = e.Parameter as QtcParameters;
			QtcParameters.IntervalMeasured = Models.Calipers.IntervalMeasured.QT;
			var caliperCollection = QtcParameters.CaliperCollection;
			ViewModel = new MeasureIntervalViewModel(caliperCollection, QtcParameters);
			ViewModel.GetResults();
		}

		private void Done_Click(object sender, RoutedEventArgs e)
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
