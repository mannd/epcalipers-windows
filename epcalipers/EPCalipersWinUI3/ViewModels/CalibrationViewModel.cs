using CommunityToolkit.Mvvm.ComponentModel;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Models;
using Microsoft.UI.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class CalibrationViewModel : ObservableObject
	{
		public static readonly IDictionary<CalibrationUnit, string> CalibrationStrings = 
			new Dictionary<CalibrationUnit, string>()
			{
				{CalibrationUnit.Msec, "msec".GetLocalized() },
				{CalibrationUnit.Sec, "sec".GetLocalized() },
				{CalibrationUnit.Mm, "mm".GetLocalized() },
				{CalibrationUnit.Mv, "mV".GetLocalized() },
				{CalibrationUnit.Uncalibrated, "points".GetLocalized() },
			};

		private readonly CaliperType _caliperType;
		private readonly Caliper _caliper;
		private CaliperCollection _caliperCollection;
		private struct CalibrationInput
		{
			public double CalibrationValue { get; init; }
			public CalibrationUnit Unit { get; init; }
			public string CustomInput { get; init; }

			public CalibrationInput(double value, CalibrationUnit unit, string customInput = "")
			{
				CalibrationValue = value;
				Unit = unit;
				CustomInput = customInput;
			}
		}
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
				CalibrationParameters parameters = ParseInput(input);
				_caliperCollection.TimeCalibration = new Calibration(_caliper.Value, parameters);
				_caliperCollection.SetCalibration(CaliperType.Time, Settings.Instance);
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
				CalibrationParameters parameters = ParseInput(input);
				_caliperCollection.AmplitudeCalibration = new Calibration(_caliper.Value, parameters);
				_caliperCollection.SetCalibration(CaliperType.Amplitude, Settings.Instance);
			}
			catch (Exception e)
			{
				var dialog = MessageHelper.CreateErrorDialog(e.Message);
				dialog.XamlRoot = xamlRoot;
				await dialog.ShowAsync();
			}
		}
		private static CalibrationParameters ParseInput(CalibrationInput input)
		{
			if (input.Unit == CalibrationUnit.Custom)
			{
				var (value, unitString) = ParseCustomString(input.CustomInput);
				var unit = Calibration.StringToCalibrationUnit(unitString);

				return new CalibrationParameters
				{
					CalibrationInterval = value,
					UnitString = unitString,
					Unit = unit
				};
			}
			return new CalibrationParameters {
				CalibrationInterval = input.CalibrationValue,
				Unit = input.Unit,
				UnitString = CalibrationStrings[input.Unit]
			};
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
	}
}
