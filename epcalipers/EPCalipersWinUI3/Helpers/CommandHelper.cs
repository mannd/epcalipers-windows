using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Helpers
{
	class CommandHelper
	{
		public static void ApplicationExit() => Application.Current.Exit();


		public static async Task About(XamlRoot xamlRoot)
		{
			var aboutDialog = new AboutDialog
			{
				XamlRoot = xamlRoot
			};
			await aboutDialog.ShowAsync();
		}

	}
}
