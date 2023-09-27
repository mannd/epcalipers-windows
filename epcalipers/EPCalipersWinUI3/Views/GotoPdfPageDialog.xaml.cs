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

namespace EPCalipersWinUI3.Views
{
	public sealed partial class GotoPdfPageDialog : ContentDialog
	{
		public MainPageViewModel ViewModel { get; }

		public int PageNumber {  get; set; }

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
