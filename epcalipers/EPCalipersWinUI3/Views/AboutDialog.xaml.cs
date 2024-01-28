using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace EPCalipersWinUI3.Views
{
	public sealed partial class AboutDialog : ContentDialog
	{
		public AboutViewModel ViewModel { get; set; }

		public AboutDialog()
		{
			ViewModel = new AboutViewModel();
			this.InitializeComponent();
			ViewModel.DebugPrintAssemblyInfo();
		}
	}
}
