using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
		public WindowEx Window { get; set; }

		public QtcView()
		{
			this.InitializeComponent();
			ViewModel = new QtcViewModel();
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
			Window?.Close();
		}
	}
}
