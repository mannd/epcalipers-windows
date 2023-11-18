﻿using EPCalipersWinUI3.Calipers;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.ViewModels;
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
using TemplateTest2.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
			AppHelper.SaveTitleBarText();
			AppHelper.AppTitleBarText = "AppSimpleTitle".GetLocalized();
		
			
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
    }
}
