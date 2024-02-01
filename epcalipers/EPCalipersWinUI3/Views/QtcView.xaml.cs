using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
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
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			QtcParameters = e.Parameter as QtcParameters;
			if (QtcParameters != null)
			{
				ViewModel.QtcParameters = QtcParameters;
			}

		}
		private void QtcView_Cancel(object sender, RoutedEventArgs e)
		{
			CloseWindow();
		}

		private void QtcView_Calculate(object sender, RoutedEventArgs e)
		{
			// calculate QTc
			CloseWindow();
		}

		private void CloseWindow()
		{
			QtcParameters.Window?.Close();
		}
	}
}
