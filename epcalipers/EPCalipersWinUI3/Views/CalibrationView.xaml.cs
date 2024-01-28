using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CalibrationView : Page
	{
		public WindowEx Window { get; set; }
		public CaliperType CaliperType { get; set; }
		public CalibrationViewModel ViewModel { get; set; }

		public CalibrationView(Caliper caliper, CaliperCollection caliperCollection)
		{
			InitializeComponent();
			EnableCustomText();
			CaliperType = caliper.CaliperType;
			ViewModel = new CalibrationViewModel(caliper, caliperCollection);
		}

		public void EnableCustomText()
		{
			CustomIntervalTextBox.IsEnabled = CustomRadioButton.IsChecked ?? false;
		}

		private void CalibrationRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			EnableCustomText();
			if (sender is RadioButtons rb)
			{
				int selection = rb.SelectedIndex;
				ViewModel.IntervalSelection = selection;
			}
		}

		private void CalibrationDialogCancel_Click(object sender, RoutedEventArgs e)
		{
			CloseWindow();
		}

		private async void CalibrationDialogCalibrate_Click(object sender, RoutedEventArgs e)
		{
			await ViewModel.SetCalibration(XamlRoot);
			if (!ViewModel.ErrorRaised) CloseWindow();
		}

		private void CloseWindow()
		{
			Window?.Close();
		}

		private async void Page_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			switch (e.Key)
			{
				case VirtualKey.Enter:
					ViewModel.CustomInterval = CustomIntervalTextBox.Text;
					await ViewModel.SetCalibration(XamlRoot);
					CloseWindow();
					break;
				case VirtualKey.Escape: CloseWindow(); break;
				default: break;
			}
		}
	}
}
