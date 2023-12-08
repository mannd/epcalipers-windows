using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Xaml;
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
					FirstField = "1000 msec".GetLocalized();
					SecondField = "1 sec".GetLocalized();
					break;
				case CaliperType.Amplitude:
					FirstField = "10 mm".GetLocalized();
					SecondField = "1 mV".GetLocalized();
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public async Task SetCalibration(XamlRoot xamlRoot)
		{
			if (_caliperCollection == null) return;
			// Program logic should prevent setting calibration on an angle caliper.
			Debug.Assert(_caliperType != CaliperType.Angle);
			switch (_caliperType)
			{
				case CaliperType.Time:
					await CalibrateTimeCaliper(xamlRoot);
					break;
				case CaliperType.Amplitude:
					await CalibrateAmplitudeCaliper(xamlRoot);
					break;
				case CaliperType.Angle:
					// Shouldn't ever get here
					break;
			}
		}

		private async Task CalibrateTimeCaliper(XamlRoot xamlRoot)
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
					input = new CalibrationInput(
						0,
						CalibrationUnit.Custom,
						CustomInterval);
					break;
			}
			try
			{
				_caliperCollection.TimeCalibration = new Calibration(_caliper.Value, input);
				_caliperCollection.SetCalibration(CaliperType.Time);
			}
			catch (Exception e)
			{
				var dialog = MessageHelper.CreateErrorDialog(e.Message);
				dialog.XamlRoot = xamlRoot;
				await dialog.ShowAsync();
			}
		}
		private async Task CalibrateAmplitudeCaliper(XamlRoot xamlRoot)
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
					input = new CalibrationInput(
						0,
						CalibrationUnit.Custom,
						CustomInterval);
					break;
			}
			try
			{
			_caliperCollection.AmplitudeCalibration = new Calibration(_caliper.Value, input);
			_caliperCollection.SetCalibration(CaliperType.Amplitude);
			}
			catch (Exception e)
			{
				var dialog = MessageHelper.CreateErrorDialog(e.Message);
				dialog.XamlRoot = xamlRoot;
				await dialog.ShowAsync();
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
