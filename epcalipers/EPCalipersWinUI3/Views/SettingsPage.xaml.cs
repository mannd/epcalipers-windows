﻿using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EPCalipersWinUI3.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SettingsPage : Page
	{
		public SettingsViewModel ViewModel { get; set; }
		public SettingsPage()
		{
			this.InitializeComponent();
			ViewModel = new SettingsViewModel();
			ViewModel.SetTitleBarName("Settings".GetLocalized());
		}
		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			AppHelper.RestoreTitleBarText();
			AppHelper.NavigateBack();
		}

		private void TimeLabelAlignment_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is RadioButtons rb)
			{
				int selection = rb.SelectedIndex;
				ViewModel.TimeCaliperLabelAlignment = selection;
			}
		}
		private void AmplitudeLabelAlignment_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is RadioButtons rb)
			{
				int selection = rb.SelectedIndex;
				ViewModel.AmplitudeCaliperLabelAlignment = selection;
			}
		}
		private void UnselectedColorButton_Click(object sender, RoutedEventArgs e)
		{
			unselectedColorPickerButton.Flyout.Hide();
		}
		private void SelectedColorButton_Click(object sender, RoutedEventArgs e)
		{
			selectedColorPickerButton.Flyout.Hide();
		}

		// NOTE: Due to a bug(?) in WinUI 3, the selection changed event is fired twice, and
		// the first time returns a SelectedIndex of -1!  thus need to ignore those events.
		private void Rounding_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is RadioButtons rb)
			{
				int selection = rb.SelectedIndex;
				if (selection >= 0) ViewModel.Rounding = selection;
			}

		}

		private void CaliperLabelSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is RadioButtons rb)
			{
				int selection = rb.SelectedIndex;
				//ViewModel.CaliperLabelSize = selection;
				if (selection >= 0) ViewModel.FontSize = selection;
			}
		}
	}
}
