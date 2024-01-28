using Microsoft.UI.Xaml.Controls;

namespace EPCalipersWinUI3.Views
{
	public sealed partial class GotoPdfPageDialog : ContentDialog
	{
		public MainPageViewModel ViewModel { get; }

		public int PageNumber { get; set; }

		public GotoPdfPageDialog(MainPageViewModel viewModel)
		{
			ViewModel = viewModel;
			this.InitializeComponent();
		}

		public GotoPdfPageDialog()
		{
			this.InitializeComponent();
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			PageNumber = (int)GotoPdfPageNumberBox.Value;
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}
	}
}
