using EPCalipersWinUI3.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("EPCalipersWinUi3Tests")]

namespace EPCalipersWinUI3.Models.Calipers
{
	public enum Unit
	{
		Msec,
		Sec,
		Mv,
		Mm,
		Degrees,
		Custom,
		Uncalibrated,
		Unknown,
		None
	}
	// TODO: If possible, refactor this to just be a Measurement, provided this struct
	// remains as just a wrapper of a Measurement.
	public readonly struct CalibrationMeasurement
	{
		public Measurement Measurement { get; init; }
		public double Value => Measurement.Value;
		public Unit Unit => Measurement.Unit;
		public string UnitString => Measurement.UnitString;
		public CalibrationMeasurement(double interval, Unit unit, string unitString)
		{
			Measurement = new Measurement(interval, unit, unitString);
		}
	}

	// TODO: Localize and improve exception error messages.
	public sealed class ZeroValueException : Exception
	{
		public ZeroValueException() : base("Divide by zero error.") { }
	}

	public sealed class EmptyCustomStringException : Exception
	{
		public EmptyCustomStringException() : base("Custom calibration can't be empty.") { }
	}

	public sealed class CantShowBpmException: Exception
	{
		public CantShowBpmException() : base("Can't show BPM with this type of calibration") { }
	}

	public class Calibration
	{
		// Rounding format strings
		private const string _roundToIntString = "D";
		private const string _roundToFourPlacesString = "G4"; // Useful?
		private const string _roundToTenthsString = "F1";
		private const string _roundToHundredthsString = "F2"; // This is needed for units in seconds.
		private const string _noRoundingString = "G8";  // This is too precise for clinical use.

		private readonly IDictionary<Rounding, string> _roundingFormat = new Dictionary<Rounding, string>()
		{
			{Rounding.ToInt, _roundToIntString },
			{Rounding.ToFourPlaces, _roundToFourPlacesString },
			{Rounding.ToTenths, _roundToTenthsString },
			{Rounding.ToHundredths, _roundToHundredthsString },
			{Rounding.None, _noRoundingString },
		};

		/// <summary>
		/// Provides a calibration factor that calculates the actual interval.
		/// </summary>
		/// <param name="uncalibratedValue">The value of the caliper in points at the time of calibration.</param>
		/// <param name="input">The desired calibration interval parameters, e.g. "1000 msec"</param>
		public Calibration(double uncalibratedValue, CalibrationMeasurement calibrationMeasurement)
		{
			if (uncalibratedValue == 0) throw new ZeroValueException();
			CalibrationMeasurment = calibrationMeasurement;
			Multiplier = CalibrationMeasurment.Value / uncalibratedValue;
		}
		public Calibration()
		{
			CalibrationMeasurment = new CalibrationMeasurement(1, Unit.Uncalibrated, DefaultUnit);
			Multiplier = 1.0;
		}

		public static string DefaultUnit { get; set; } = "points";
		public static string DefaultBpm { get; set; } = "bpm";
		public static Calibration Uncalibrated => new(); // Default Calibration.Unit is Uncalibrated.
		public static Calibration None => new(1.0, new CalibrationMeasurement(1.0, Unit.None, ""));

		public Rounding Rounding { get; set; } = Rounding.ToInt;
		public CalibrationMeasurement CalibrationMeasurment { get; init; }
		public double Multiplier { get; init; }
		public bool IsUncalibrated => CalibrationMeasurment.Unit == Unit.Uncalibrated;
		public bool IsCalibrated => !IsUncalibrated;

		public static Unit StringToCalibrationUnit(string input)
		{
			if (string.IsNullOrEmpty(input)) return Unit.Unknown;
			if (IsMillimetersUnit(input)) return Unit.Mm;
			if (IsMillisecondsUnit(input)) return Unit.Msec;
			if (IsSecondsUnit(input)) return Unit.Sec;
			if (IsMillivoltsUnit(input)) return Unit.Mv;
			return Unit.Unknown;
		}

		public virtual string GetFormattedMeasurement(double interval, bool showBpm = false)
		{
			var measurement = CalibratedInterval(interval, showBpm);
			double value = measurement.Value;
			string unitString = measurement.UnitString;
			string formattedValue = GetFormattedRoundedValue(value, showBpm);
			return string.Format("{0} {1}", formattedValue, unitString);
		}

		public string GetFormattedRoundedValue(double value, bool showBpm = false)
		{
			// 
			string format = ForceRoundingToHundredths(showBpm) ?
				_roundingFormat[Rounding.ToHundredths] : _roundingFormat[Rounding];
			if (Rounding == Rounding.ToInt)
			{
				int intValue = (int)Math.Round(value);
				return intValue.ToString(format);
			}
			return value.ToString(format);
		}

		public (string, string) GetMeanCalibratedInterval(double interval, int numberOfIntervals, bool showBpm = false)
		{
			if (numberOfIntervals < 1) throw new ZeroValueException();
			interval = Math.Abs(interval);  // no negative mean intervals
			var meanInterval = MathHelper.MeanInterval(interval, numberOfIntervals);
			var intervalPlusUnit = CalibratedInterval(meanInterval, showBpm);
			var calibratedMeanInterval = intervalPlusUnit.Value;
			var roundedInterval = GetFormattedRoundedValue(calibratedMeanInterval, showBpm: showBpm);
			return (roundedInterval, intervalPlusUnit.UnitString);
		}

		public (string, string) GetNewMeanCalibratedInterval(double interval, int numberOfIntervals, bool showBpm = false)
		{
			var meanInterval = GetMeanInterval(interval, numberOfIntervals);
			var meanMeasurement = CalibratedInterval(meanInterval, showBpm);
			var calibratedMeanInterval = meanMeasurement.Value;
			var roundedInterval = GetFormattedRoundedValue(calibratedMeanInterval, showBpm: showBpm);
			return (roundedInterval, meanMeasurement.UnitString);
		}

		public double GetMeanInterval(double interval, int numberOfIntervals)
		{
			if (numberOfIntervals < 1) throw new ZeroValueException();
			interval = Math.Abs(interval);
			return MathHelper.MeanInterval(interval, numberOfIntervals);
		}

		public Measurement MeanCalibratedInterval(double interval, int n)
		{
			var meanInterval = GetMeanInterval(interval, n);
			return new Measurement(meanInterval, CalibrationMeasurment.Unit,
				CalibrationMeasurment.UnitString);
		}

		public Measurement CalibratedInterval(double interval, bool showBpm = false)
		{
			double value;
			string unitString;

			if (showBpm)
			{
				unitString = DefaultBpm;
				value = CalibrationMeasurment.Unit switch
				{
					Unit.Msec => MathHelper.AbsMsecToBpm(Multiplier * interval),
					Unit.Sec => MathHelper.AbsSecToBpm(Multiplier * interval),
					_ => Multiplier * interval
				};
			}
			else
			{
				if (CalibrationMeasurment.Unit == Unit.Mm)
				{
					value = Math.Abs(Multiplier * interval);
				}
				else
				{
					value = Multiplier * interval;
				}
				unitString = CalibrationMeasurment.UnitString;
			}

			return new Measurement(value, CalibrationMeasurment.Unit, unitString);
		}

		internal static bool IsMillisecondsUnit(string input)
		{
			string pattern = @"^(msec|мсек|ms|мс)$|^(millis|миллис)";
			return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
		}

		internal static bool IsSecondsUnit(string input)
		{
			string pattern = @"^(sec|сек|s|с)$";
			return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
		}
		internal static bool IsMillimetersUnit(string input)
		{
			string pattern = @"^(mm|мм)$|^(millim|миллим)";
			return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
		}
		internal static bool IsMillivoltsUnit(string input)
		{
			string pattern = @"^(mv|мв)$|^(milliv|миллив)";
			return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
		}

		// Rationale here is that sec and mV are useless without two decimal places.
		// User can get around this by defining custom units instead of using predefined units.
		private bool ForceRoundingToHundredths(bool showBpm)
		{
			return (CalibrationMeasurment.Unit == Unit.Sec && !showBpm)
				|| CalibrationMeasurment.Unit == Unit.Mv;
		}

	}
}
