using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class MeanRateIntervalViewModel: ObservableObject
	{
		public int NumberOfIntervals { get; set; }
		public int Interval { get; set; }
		private Caliper _caliper;

		public MeanRateIntervalViewModel(Caliper caliper)
		{
			_caliper = caliper;

		}

		public static double Calculate(double interval, int numberOfIntervals)
		{
			return MathHelper.MeanInterval(interval, numberOfIntervals);
		}

		[RelayCommand]
		public async Task ShowResult(XamlRoot xamlRoot)
		{
			var result = Calculate(_caliper.Value, NumberOfIntervals);
			var formattedValues = Calibration.GetCalibratedRateInterval(result, _caliper.Calibration);
			var message = $"Mean rate = {formattedValues.Item1}\nMean interval = {formattedValues.Item2}";
			var dialog = MessageHelper.CreateMessageDialog("Results", message);
			dialog.XamlRoot = xamlRoot;
			await dialog.ShowAsync();
		}
	}
}
