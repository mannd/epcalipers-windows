using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Models;
using Microsoft.UI.Xaml;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class MeanRateIntervalViewModel : ObservableObject
	{
		// TODO: localize
		private static string _invalidCaliperText = "Invalid caliper";

		public QtcParameters QtcParameters { get; set; }

		public Caliper Caliper { get; set; }
		public CaliperCollection CaliperCollection { get; set; }

		// TODO: Refactor to pass QtcParameters (maybe renamed to MeasurementParameters)
		// so that we use QtcParameters.Caliper, QtcParameters.CaliperCollection and
		// QtcParameters.NumberOfIntervals directly...
		public MeanRateIntervalViewModel(CaliperCollection caliperCollection,
			int numberOfIntervals = 3)
		{
			Caliper = caliperCollection.SelectedCaliper;
			CaliperCollection = caliperCollection;
			CaliperCollection.PropertyChanged += OnMyPropertyChanged;
			if (Caliper != null) Caliper.PropertyChanged += OnMyPropertyChanged;
			PropertyChanged += OnMyPropertyChanged;
			NumberOfIntervals = numberOfIntervals;
			TotalInterval = GetTotalInterval();
			MeanInterval = GetMeanInterval();
			MeanRate = GetMeanRate();
		}

		public MeanRateIntervalViewModel() { }


		private void OnMyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Caliper.LabelText) 
				|| e.PropertyName == nameof(NumberOfIntervals))
			{
				GetResults();
			}
			else if (e.PropertyName == nameof(CaliperCollection.SelectedCaliper))
			{
				Caliper = CaliperCollection.SelectedCaliper;
				if (Caliper != null) Caliper.PropertyChanged += OnMyPropertyChanged;
				GetResults();
			}
		}

		private void GetResults()
		{
			TotalInterval = GetTotalInterval();
			MeanInterval = GetMeanInterval();
			MeanRate = GetMeanRate();
			if (QtcParameters != null)
			{
				QtcParameters.RawRRInterval = Caliper.Value;
			}
		}

		private bool IsValidCaliper()
		{
			return Caliper != null && Caliper.CaliperType == CaliperType.Time && Caliper.Calibration.IsCalibrated;
		}

		private string GetTotalInterval()
		{
			// Number of intervals = 1 forces total interval, and showBpm false forces interval, not bpm.
			var interval = Caliper?.Calibration.GetMeanCalibratedInterval(Caliper.Value, 1, false);
			return IsValidCaliper() ? $"Total interval = {interval?.Item1} {interval?.Item2}" : _invalidCaliperText;
		}
		private string GetMeanInterval()
		{
			var interval = Caliper?.Calibration.GetMeanCalibratedInterval(Caliper.Value, NumberOfIntervals, false);
			return IsValidCaliper() ? $"Mean interval = {interval?.Item1} {interval?.Item2}" : _invalidCaliperText;
		}
		private string GetMeanRate()
		{
			var interval = Caliper?.Calibration.GetMeanCalibratedInterval(Caliper.Value, NumberOfIntervals, true);
			return IsValidCaliper() ? $"Mean rate = {interval?.Item1} {interval?.Item2}" : _invalidCaliperText;

		}

		[ObservableProperty]
		private string totalInterval;

		[ObservableProperty]
		private string meanInterval;

		[ObservableProperty]
		private string meanRate;

		[ObservableProperty]
		private int numberOfIntervals;

	}
}
