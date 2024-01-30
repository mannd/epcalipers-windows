using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
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
		private Caliper _caliper;
		private CaliperCollection _caliperCollection;


		public MeanRateIntervalViewModel(Caliper caliper, CaliperCollection caliperCollection)
		{
			_caliper = caliper;
			_caliperCollection = caliperCollection;
			_caliperCollection.PropertyChanged += OnMyPropertyChanged;
			if (_caliper != null) _caliper.PropertyChanged += OnMyPropertyChanged;
			PropertyChanged += OnMyPropertyChanged;
			NumberOfIntervals = 3; // TODO: either set default in settings, or remember last number
			TotalInterval = GetTotalInterval();
			MeanInterval = GetMeanInterval();
			MeanRate = GetMeanRate();
		}

		private void OnMyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_caliper.LabelText) 
				|| e.PropertyName == nameof(NumberOfIntervals))
			{
				GetResults();
			}
			else if (e.PropertyName == nameof(_caliperCollection.SelectedCaliper))
			{
				_caliper = _caliperCollection.SelectedCaliper;
				if (_caliper != null) _caliper.PropertyChanged += OnMyPropertyChanged;
				GetResults();
			}
		}

		private void GetResults()
		{
			TotalInterval = GetTotalInterval();
			MeanInterval = GetMeanInterval();
			MeanRate = GetMeanRate();
		}

		private bool IsValidCaliper()
		{
			return _caliper != null && _caliper.CaliperType == CaliperType.Time && _caliper.Calibration.IsCalibrated;
		}

		private string GetTotalInterval()
		{
			// Number of intervals = 1 forces total interval, and showBpm false forces interval, not bpm.
			var interval = _caliper?.Calibration.GetMeanCalibratedInterval(_caliper.Value, 1, false);
			return IsValidCaliper() ? $"Total interval = {interval?.Item1} {interval?.Item2}" : _invalidCaliperText;
		}
		private string GetMeanInterval()
		{
			var interval = _caliper?.Calibration.GetMeanCalibratedInterval(_caliper.Value, NumberOfIntervals, false);
			return IsValidCaliper() ? $"Mean interval = {interval?.Item1} {interval?.Item2}" : _invalidCaliperText;
		}
		private string GetMeanRate()
		{
			var interval = _caliper?.Calibration.GetMeanCalibratedInterval(_caliper.Value, NumberOfIntervals, true);
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
