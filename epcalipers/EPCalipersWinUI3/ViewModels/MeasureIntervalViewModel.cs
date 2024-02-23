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
	public partial class MeasureIntervalViewModel : ObservableObject
	{
		private static readonly string _invalidCaliperText = "InvalidCaliperText".GetLocalized();
		private static readonly string _dialogTitle = "MeanRateIntervalTitle".GetLocalized();

		public QtcParameters QtcParameters {  get; set; }
		public Caliper Caliper { get; set; }
		public CaliperCollection CaliperCollection { get; set; }

		// TODO: We are having trouble initializing NumberOfIntervals properly between the use of
		// this ViewModel for Mean RR interval measurement and RR interval measurement for QTc.
		// The two settings are not being initialized or updated properly.....
		public MeasureIntervalViewModel(CaliperCollection caliperCollection,
			QtcParameters qtcParameters = null)
		{
			QtcParameters = qtcParameters;
			if (qtcParameters == null) // qtcParmaters only non-null for QTc dialog.
			{
				NumberOfIntervals = Settings.Instance.NumberOfMeanIntervals;
			}
			else switch (qtcParameters.IntervalMeasured)
				{
					case IntervalMeasured.RR:
						NumberOfIntervals = Settings.Instance.NumberOfRRIntervals;
						break;
					case IntervalMeasured.MeanRR:  // Shouldn't happen though.
						Debug.Assert(false, "qtcParameters contains IntervalMeasured.MeanRR!");
						NumberOfIntervals = Settings.Instance.NumberOfMeanIntervals;
						break;
					case IntervalMeasured.QT:
						NumberOfIntervals = 1;
						break;
				}
			Caliper = caliperCollection.SelectedCaliper;
			CaliperCollection = caliperCollection;
			CaliperCollection.PropertyChanged += OnMyPropertyChanged;
			if (Caliper != null) Caliper.PropertyChanged += OnMyPropertyChanged;
			PropertyChanged += OnMyPropertyChanged;
			Title = _dialogTitle;
			GetResults();
		}

		public MeasureIntervalViewModel() { }

		private void OnMyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Caliper.LabelText) 
				|| e.PropertyName == nameof(NumberOfIntervals))
			{
				GetResults();
				if (QtcParameters != null)
				{
					switch (QtcParameters.IntervalMeasured)
					{
						case IntervalMeasured.RR:
							Settings.Instance.NumberOfRRIntervals = NumberOfIntervals;
							break;
						default:
							break;
					}
				} else
				{
					Settings.Instance.NumberOfMeanIntervals = NumberOfIntervals;
				}
			}
			else if (e.PropertyName == nameof(CaliperCollection.SelectedCaliper))
			{
				Caliper = CaliperCollection.SelectedCaliper;
				if (Caliper != null) Caliper.PropertyChanged += OnMyPropertyChanged;
				GetResults();
			}
		}

		public void GetResults()
		{
			TotalInterval = GetFormattedTotalInterval();
			MeanInterval = GetFormattedMeanInterval();
			MeanRate = GetFormattedMeanRate();
			if (QtcParameters == null) return;
			switch (QtcParameters.IntervalMeasured)
			{
				case IntervalMeasured.MeanRR:
					return;
				case IntervalMeasured.RR:
					if (IsValidCaliper())
					{
						QtcParameters.RRMeasurement = Caliper.Calibration
							.MeanCalibratedInterval(Caliper.Value, NumberOfIntervals);
					}
					else
					{
						QtcParameters.RRMeasurement = new Measurement();
					}
					break;
				case IntervalMeasured.QT:
					if (IsValidCaliper())
					{
						QtcParameters.QTMeasurement = Caliper.Calibration
							.MeanCalibratedInterval(Caliper.Value, 1);
					}
					else
					{
						QtcParameters.QTMeasurement = new Measurement();
					}
					break;
			}
		}

		private bool IsValidCaliper()
		{
			return Caliper != null && Caliper.CaliperType == CaliperType.Time && Caliper.Calibration.IsCalibrated;
		}

		private string GetFormattedTotalInterval()
		{
			// Number of intervals = 1 forces total interval, and showBpm false forces interval, not bpm.
			var interval = Caliper?.Calibration.GetMeanCalibratedInterval(Caliper.Value, 1, false);
			return IsValidCaliper() ? $"Total interval = {interval?.Item1} {interval?.Item2}" : _invalidCaliperText;
		}
		private string GetFormattedMeanInterval()
		{
			var interval = Caliper?.Calibration.GetMeanCalibratedInterval(Caliper.Value, NumberOfIntervals, false);
			return IsValidCaliper() ? $"Mean interval = {interval?.Item1} {interval?.Item2}" : _invalidCaliperText;
		}
		private string GetFormattedMeanRate()
		{
			var interval = Caliper?.Calibration.GetMeanCalibratedInterval(Caliper.Value, NumberOfIntervals, true);
			return IsValidCaliper() ? $"Mean rate = {interval?.Item1} {interval?.Item2}" : _invalidCaliperText;
		}

		private Measurement MeanIntervalMeasurement()
		{
			if (Caliper != null)
			{
				return Caliper.Calibration.MeanCalibratedInterval(Caliper.Value, NumberOfIntervals);
			}
			return new Measurement();
		}

		[ObservableProperty]
		private string totalInterval;

		[ObservableProperty]
		private string meanInterval;

		[ObservableProperty]
		private string meanRate;

		[ObservableProperty]
		private int numberOfIntervals;

		[ObservableProperty]
		private Visibility rateVisibility;

		[ObservableProperty]
		private string title;

	}
}
