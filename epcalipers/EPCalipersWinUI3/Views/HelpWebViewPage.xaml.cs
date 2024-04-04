using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;

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
			ViewModel.SetTitleBarName("Help".GetLocalized());
			WebView.NavigationStarting += NavigationStarting;
			WebView.NavigationCompleted += NavigationCompleted;
			Init();
		}

		private void NavigationStarting(WebView2 sender, CoreWebView2NavigationStartingEventArgs args)
		{
			ViewModel.IsLoading = true;
		}

		private void NavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
		{
			ViewModel.IsLoading = false;
		}

		private async void Init()
		{
			await WebView.EnsureCoreWebView2Async();

			WebView.CoreWebView2.SetVirtualHostNameToFolderMapping(
				"appassets", "Assets", CoreWebView2HostResourceAccessKind.Allow);

			WebView.Source = new Uri("http://appassets/Help/beta-help.html");
			//WebView.CoreWebView2.OpenDevToolsWindow();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			AppHelper.NavigateBack();
		}
	}
}
