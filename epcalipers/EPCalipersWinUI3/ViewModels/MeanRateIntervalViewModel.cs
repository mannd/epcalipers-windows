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
		public int Interval { get; set; }
		private Caliper _caliper;
		private CaliperCollection _caliperCollection;

		public MeanRateIntervalViewModel(Caliper caliper, CaliperCollection caliperCollection)
		{
			_caliper = caliper;
			_caliperCollection = caliperCollection;
			_caliperCollection.PropertyChanged += OnMyPropertyChanged;
			if (_caliper != null) _caliper.PropertyChanged += OnMyPropertyChanged;
			PropertyChanged += OnMyPropertyChanged;
			NumberOfIntervals = 3;
			TotalInterval = GetTotalInterval();
			MeanInterval = GetMeanInterval();
			MeanRate = GetMeanRate();
		}

		// TODO: 
		// 1) Show real time total interval, mean interval, mean rate in View
		// 2) Either prohibit changing selected caliper (IsLocked = true) or deal with IsSelected changes.
		// 3) If we are dealing with IsSelected changes, we need to pass whole CaliperCollection to these floating windows
		// 4) E.g., if no time caliper selected, show No Time caliper selected message
		// If we implement the above, no need for Calculate button.  Only restriction before entering measurement is to assure
		// time calipers are calibrated.  Need to deal with calibration being cleared too.  
		private void OnMyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// TODO: Needs to work for partially selected calipers too!
			if (e.PropertyName == nameof(_caliper.LabelText) || e.PropertyName == nameof(NumberOfIntervals))
			{
				TotalInterval = GetTotalInterval();
				MeanInterval = GetMeanInterval();
				MeanRate = GetMeanRate();
			}
			// TODO: stops working when there are zero calipers, and then one is added
			// If the original selected caliper is deleted, the new caliper is not the in selected, and it doesn't send 
			// notifications.
			if (e.PropertyName == nameof(_caliperCollection.CaliperSelectionChanged)
				|| e.PropertyName == nameof(_caliperCollection.PartiallyOrFullySelectedCaliper.IsSelected))
			{
				Debug.Print("ISSELECTED change");
				_caliper = _caliperCollection.NewSelectedCaliper;
				if (_caliper != null) _caliper.PropertyChanged += OnMyPropertyChanged;
				TotalInterval = GetTotalInterval();
				MeanInterval = GetMeanInterval();
				MeanRate = GetMeanRate();
			}
		}

		private bool IsValidCaliper()
		{
			return _caliper != null && _caliper.CaliperType == CaliperType.Time && _caliper.Calibration != Calibration.Uncalibrated;
			// or caliper is partially selected...
		}

		private string GetTotalInterval()
		{
			var interval = _caliper?.LabelText;
			return IsValidCaliper() ? $"Total interval = {interval} " : "Invalid caliper";
		}
		private string GetMeanInterval()
		{
			var interval = _caliper?.Calibration.GetMeanCalibratedInterval(_caliper.Value, NumberOfIntervals, false);
			return IsValidCaliper() ? $"Mean interval = {interval?.Item1} {interval?.Item2} " : "Invalid caliper";
		}
		private string GetMeanRate()
		{
			var interval = _caliper?.Calibration.GetMeanCalibratedInterval(_caliper.Value, NumberOfIntervals, true);
			return IsValidCaliper() ? $"Mean rate = {interval?.Item1} {interval?.Item2} " : "Invalid caliper";

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
		private string meanInterval;

		[ObservableProperty]
		private string meanRate;

		[ObservableProperty]
		private int numberOfIntervals;
	}
}
