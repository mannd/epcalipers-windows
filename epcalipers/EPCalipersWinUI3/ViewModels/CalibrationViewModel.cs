using CommunityToolkit.Mvvm.ComponentModel;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	// TODO: add more default calibrations, e.g. 200 msec, 0.2 sec for time
	public partial class CalibrationViewModel : ObservableObject
	{
		public static readonly IDictionary<Unit, string> CalibrationStrings =
			new Dictionary<Unit, string>()
			{
				{Unit.Msec, "msec".GetLocalized() },
				{Unit.Sec, "sec".GetLocalized() },
				{Unit.Mm, "mm".GetLocalized() },
				{Unit.Mv, "mV".GetLocalized() },
				{Unit.Uncalibrated, "points".GetLocalized() },
			};

		private readonly CaliperType _caliperType;
		private readonly Caliper _caliper;
		private readonly CaliperCollection _caliperCollection;
		private readonly struct CalibrationInput
		{
			public double CalibrationValue { get; init; }
			public Unit Unit { get; init; }
			public string CustomInput { get; init; }

			public CalibrationInput(double value, Unit unit, string customInput = "")
			{
				CalibrationValue = value;
				Unit = unit;
				CustomInput = customInput;
			}
		}
		public CalibrationViewModel(Caliper caliper, CaliperCollection caliperCollection)
		{
			ErrorRaised = false;
			_caliper = caliper;
			_caliperType = caliper.CaliperType;
			_caliperCollection = caliperCollection;
			switch (_caliperType)
			{
				case CaliperType.Time:
					FirstField = "1000 msec".GetLocalized();
					SecondField = "200 msec";
					ThirdField = "1 sec".GetLocalized();
					FourthField = "0.2 sec";
					ExtraFieldVisibility = Visibility.Visible;
					break;
				case CaliperType.Amplitude:
					FirstField = "10 mm".GetLocalized();
					SecondField = "1 mV".GetLocalized();
					ExtraFieldVisibility = Visibility.Collapsed;
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
					Debug.Assert(false, "Calibration dialog opened for Angle caliper!");
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
						0,
						Unit.Custom,
						CustomInterval);
					break;
				case 1:
					input = new CalibrationInput(
						1000,
						Unit.Msec);
					break;
				case 2:
					input = new CalibrationInput(
						200,
						Unit.Msec);
					break;
				case 3:
					input = new CalibrationInput(
						1.0,
						Unit.Sec);
					break;
				case 4:
					input = new CalibrationInput(
						0.2,
						Unit.Sec);
					break;
			}
			try
			{
				CalibrationMeasurement parameters = ParseInput(input);
				_caliperCollection.TimeCalibration = new Calibration(_caliper.Value, parameters);
				_caliperCollection.SetCalibration();
			}
			catch (Exception e)
			{
				ErrorRaised = true;
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
						0,
						Unit.Custom,
						CustomInterval);
					break;
				case 1:
					input = new CalibrationInput(
						10,
						Unit.Mm);
					break;
				case 2:
					input = new CalibrationInput(
						1.0,
						Unit.Mv);
					break;
			}
			try
			{
				CalibrationMeasurement parameters = ParseInput(input);
				_caliperCollection.AmplitudeCalibration = new Calibration(_caliper.Value, parameters);
				_caliperCollection.SetCalibration();
			}
			catch (Exception e)
			{
				ErrorRaised = true;
				var dialog = MessageHelper.CreateErrorDialog(e.Message);
				dialog.XamlRoot = xamlRoot;
				await dialog.ShowAsync();
			}
		}
		private static CalibrationMeasurement ParseInput(CalibrationInput input)
		{
			if (input.Unit == Unit.Custom)
			{
				var (value, unitString) = ParseCustomString(input.CustomInput);
				var unit = Calibration.StringToCalibrationUnit(unitString);

				return new CalibrationMeasurement(value, unit, unitString);
			}
			return new CalibrationMeasurement(input.CalibrationValue,
				input.Unit,
				CalibrationStrings[input.Unit]);
		}
		public static (double, string) ParseCustomString(string s)
		{
			if (s == null || s.Length == 0)
			{
				throw new EmptyCustomStringException();
			}
			double value;
			string units = string.Empty;
			char[] delimiters = { ' ' };
			string[] parts = s.Split(delimiters);
			value = float.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
			value = Math.Abs(value);
			if (parts.Length > 1)
			{
				// assume second substring is units
				units = parts[1];
			}
			if (value == 0)
			{
				throw new ZeroValueException();
			}
			return (value, units);
		}

		[ObservableProperty]
		private int intervalSelection;

		[ObservableProperty]
		private string firstField;

		[ObservableProperty]
		private string secondField;

		[ObservableProperty]
		private string customInterval;

		[ObservableProperty]
		private bool errorRaised;

		[ObservableProperty]
		private string thirdField;

		[ObservableProperty]
		private string fourthField;

		[ObservableProperty]
		private Visibility extraFieldVisibility;
	}
}
