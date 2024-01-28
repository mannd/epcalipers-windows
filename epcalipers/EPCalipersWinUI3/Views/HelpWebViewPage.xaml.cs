using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EPCalipersWinUI3.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class HelpWebViewPage : Page
	{
		public HelpViewModel ViewModel { get; set; }

		public HelpWebViewPage()
		{
			this.InitializeComponent();
			ViewModel = new HelpViewModel();
			AppHelper.SaveTitleBarText();
			AppHelper.AppTitleBarText = "AppSimpleTitle".GetLocalized();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			AppHelper.RestoreTitleBarText();
			AppHelper.NavigateBack();
		}
	}
}
