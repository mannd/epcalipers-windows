using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Xaml.Documents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class CalibrationViewModel: ObservableObject
	{
		private CaliperType _caliperType;
		private Caliper _caliper;
		private CaliperCollection _caliperCollection;
		public CalibrationViewModel(Caliper caliper, CaliperCollection caliperCollection)
		{
			_caliper = caliper;
			_caliperType = caliper.CaliperType;
			_caliperCollection = caliperCollection;
			switch (_caliperType)
			{
				case CaliperType.Time:
					FirstField = "1000 msec";
					SecondField = "1.0 sec";
					break;
				case CaliperType.Amplitude:
					FirstField = "10 mm";
					SecondField = "1.0 mV";
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public void SetCalibration()
		{
			// TODO: Need to add amplitude and angle calibration.
			// TODO: Need to clean this up, break it up.  Need exception handling when
			// input doesn't make sense.
			if (_caliperCollection == null) return;
			switch (_caliperType)
			{
				case CaliperType.Time:
					CalibrateTimeCaliper();
					break;
				case CaliperType.Amplitude:
					CalibrateAmplitudeCaliper();
					break;
				case CaliperType.Angle:
					break;
			}
		}

		private void CalibrateTimeCaliper()
		{
			CalibrationInput input = new();
			switch (IntervalSelection)
			{
				case 0:
					input = new CalibrationInput(
						1000,
						CalibrationUnit.Msec);
					break;
				case 1:
					input = new CalibrationInput(
						1.0,
						CalibrationUnit.Sec);
					break;
				case 2:
					input = new CalibrationInput(0,
						CalibrationUnit.Custom,
						CustomInterval);
					break;
			}
			try
			{
				var calibration = new Calibration(_caliper.Value, input);
				_caliperCollection.TimeCalibration = calibration;
				_caliperCollection.SetCalibration(CaliperType.Time);
			}
			catch (Exception)
			{
				Debug.Print("Could not calibrate.");
			}
		}
		private void CalibrateAmplitudeCaliper()
		{
			CalibrationInput input = new();
			switch (IntervalSelection)
			{
				case 0:
					input = new CalibrationInput(
						10,
						CalibrationUnit.Mm);
					break;
				case 1:
					input = new CalibrationInput(
						1.0,
						CalibrationUnit.Mv);
					break;
				case 2:
					input = new CalibrationInput(0,
						CalibrationUnit.Custom,
						CustomInterval);
					break;
			}
			try
			{
			var calibration = new Calibration(_caliper.Value, input);
			_caliperCollection.AmplitudeCalibration = calibration;
			_caliperCollection.SetCalibration(CaliperType.Amplitude);
			}
			catch (Exception)
			{
				Debug.Print("bad calibration");
			}
		}

		[ObservableProperty]
		private int intervalSelection;

		[ObservableProperty]
		private string firstField;

		[ObservableProperty]
		private string secondField;

		[ObservableProperty]
		private string customInterval;
	}
}
