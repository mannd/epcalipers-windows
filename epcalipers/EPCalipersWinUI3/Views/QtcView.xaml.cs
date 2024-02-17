using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.System;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class QtcView : Page
	{
		QtcViewModel ViewModel { get; set; }
		WindowEx Window { get; set; }
		QtcParameters QtcParameters { get; set; }

		public QtcView()
		{
			this.InitializeComponent();
			ViewModel = new QtcViewModel();
		}
		// TODO: Need cancel button on Measure and QT dialogs, so that interval is NOT updated if user doesn't want to update it.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			QtcParameters = e.Parameter as QtcParameters;
			if (QtcParameters != null)
			{
				ViewModel.QtcParameters = QtcParameters;
			}
			ViewModel.UpdateIntervals();
		}

		private void QtcView_Cancel(object sender, RoutedEventArgs e)
		{
			CloseWindow();
		}

		private void CloseWindow()
		{
			QtcParameters.Window?.Close();
		}

		private void Page_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			switch (e.Key)
			{
				case VirtualKey.Escape: CloseWindow(); break;
				default: break;
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			ViewModel.XamlRoot = XamlRoot;
		}
	}
}
