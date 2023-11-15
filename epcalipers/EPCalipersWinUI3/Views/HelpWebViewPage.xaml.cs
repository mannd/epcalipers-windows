using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using EPCalipersWinUI3.ViewModels;
using EPCalipersWinUI3.Helpers;
using TemplateTest2.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EPCalipersWinUI3.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class HelpWebViewPage : Page
	{
		public HelpViewModel ViewModel {  get; set; }

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
