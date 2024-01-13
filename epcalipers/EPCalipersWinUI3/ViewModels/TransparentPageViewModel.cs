
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using System.Threading.Tasks;
using Windows.Foundation;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class TransparentPageViewModel: CaliperPageViewModel
	{
		public TransparentPageViewModel(ICaliperView caliperView) : base(caliperView) { }

		[RelayCommand]
		private static void ToggleTransparentWindow()
		{
			var mainWindow = AppHelper.AppMainWindow;
			mainWindow.SystemBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop()
			{ Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt };
			AppHelper.SaveTitleBarText();
			mainWindow.Navigate(typeof(MainPage));
		}
	}
}
