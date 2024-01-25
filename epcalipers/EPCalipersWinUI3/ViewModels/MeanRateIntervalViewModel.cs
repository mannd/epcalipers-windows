using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class MeanRateIntervalViewModel : ObservableObject
	{
		public int Interval { get; set; }
		private Caliper _caliper;

		public MeanRateIntervalViewModel(Caliper caliper)
		{
			_caliper = caliper;
			_caliper.PropertyChanged += OnMyPropertyChanged;
			this.PropertyChanged += OnMyPropertyChanged;
			NumberOfIntervals = 3;
			TotalInterval = _caliper.Calibration.GetMeanCalibratedInterval(_caliper.Value, NumberOfIntervals, false).Item1;
		}

		private void OnMyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_caliper.LabelText) || e.PropertyName == nameof(NumberOfIntervals)) {
				TotalInterval = _caliper.Calibration.GetMeanCalibratedInterval(_caliper.Value, NumberOfIntervals, false).Item1;
			}
		}

		[RelayCommand]
		public async Task ShowResult(XamlRoot xamlRoot)
		{
			var intervalResult = _caliper.Calibration.GetMeanCalibratedInterval(_caliper.Value, NumberOfIntervals, false);
			var rateResult = _caliper.Calibration.GetMeanCalibratedInterval(_caliper.Value, NumberOfIntervals, true);
			var message = $"Mean rate = {rateResult.Item1} {rateResult.Item2}\nMean interval = {intervalResult.Item1} {intervalResult.Item2}";
			var dialog = MessageHelper.CreateMessageDialog("Results", message);
			dialog.XamlRoot = xamlRoot;
			await dialog.ShowAsync();
		}

		[ObservableProperty]
		private string totalInterval;

		[ObservableProperty]
		private int numberOfIntervals;
	}
}
