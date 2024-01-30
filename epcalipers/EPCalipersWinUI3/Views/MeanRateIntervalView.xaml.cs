using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;
using Windows.System;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
	public sealed partial class MeanRateIntervalView : Page
	{
		public WindowEx Window { get; set; }

		public MeanRateIntervalViewModel ViewModel { get; set; }

		public MeanRateIntervalView(Caliper caliper, CaliperCollection caliperCollection)
		{
			InitializeComponent();
			ViewModel = new MeanRateIntervalViewModel(caliper, caliperCollection);
		}

		private void MeanRateIntervalViewCancel_Click(object sender, RoutedEventArgs e)
		{
			CloseWindow();
		}

		private void CloseWindow()
		{
			Window?.Close();
		}

		// TODO: Why aren't keys being detected here?
		private async void Page_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			switch (e.Key)
			{
				case VirtualKey.Escape: CloseWindow(); break;
				default: break;
			}
		}
	}
}
