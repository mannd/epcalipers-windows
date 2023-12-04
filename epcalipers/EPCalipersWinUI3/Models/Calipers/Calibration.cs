using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;

namespace EPCalipersWinUI3.Models.Calipers
{
	public enum CalibrationUnit
	{
		Msec,
		Sec,
		Mv,
		Mm,
		Bpm,
		Custom,
		Uncalibrated,
        Unknown
	}

    /// <summary>
    /// Provides parameters need to calibrate the calipers.
    /// For example, 1000 msec translates to a CalibrationValue of 1000,
    /// a CalibrationUnit of .Msec, and either a CustomInput of "" if 
    /// the parameters were derived from radiobutton selections, or of
    /// "1000 msec" if entered in the custom input textbox.
    /// </summary>
    public struct CalibrationInput
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

    public struct CalibrationParameters
    {
        public double Value { get; init; }
        public CalibrationUnit Unit { get; init; }
        public string UnitString { get; init; }
    }

    public struct Calibration 
    {
        public CalibrationParameters Parameters { get; init; }
        public readonly double Multiplier { get; init; }
        public string Text
        {
            get
            {
                Debug.Print(string.Format("{0:0#} {1}", Parameters.Value, Parameters.UnitString));
                return string.Format("{0:0#} {1}", Parameters.Value, Parameters.UnitString);

            }
        }

        /// <summary>
        /// Provides a calibration factor that calculates the actual interval.
        /// </summary>
        /// <param name="value">The value of the caliper in points at the time of calibration.</param>
        /// <param name="input">The desired calibration interval parameters, e.g. "1000 msec"</param>
        public Calibration(double value, CalibrationInput input)
        {
            // TODO: need to throw exception if can't get good calibration from custom input.
            // When exception thrown, calibration is not replaced, and maybe error msg shown.
            Parameters = ParseInput(input);
            Multiplier = Parameters.Value / value;
        }

        public Calibration()
        {
			Parameters = UncalibratedParameters();
			Multiplier = 1.0;
		}

        public string GetText(double value)
        {
            var calibratedValue = Multiplier * value;
			return string.Format("{0:0.00#} {1}", calibratedValue, Parameters.UnitString);
		}

		public static CalibrationParameters UncalibratedParameters()
        {
            return new CalibrationParameters
            {
                Unit = CalibrationUnit.Uncalibrated,
                UnitString = CalibrationUnitToString(CalibrationUnit.Uncalibrated),
                Value = 0.0,
            };
        }

        public static CalibrationParameters ParseInput(CalibrationInput input)
        {
            if (input.Unit == CalibrationUnit.Custom)
            {
                var (value, unitString) = ParseCustomString(input.CustomInput);
                var unit = StringToCalibrationUnit(unitString);

                return new CalibrationParameters
                {
                    Value = value,
                    UnitString = unitString,
                    Unit = unit
                };
            }
            return new CalibrationParameters {
                Value = input.CalibrationValue, 
                Unit = input.Unit, 
                UnitString = CalibrationUnitToString(input.Unit)
            };
        }

        public static (double, string) ParseCustomString(string s)
        {
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
                throw new Exception("Calibration can't be zero.");
            }
            return (value, units);
        }

        // TODO: callers will need to localize the results
        public static string CalibrationUnitToString(CalibrationUnit unit)
        {
            switch (unit)
            {
                case CalibrationUnit.Msec:
                    return "msec";
                case CalibrationUnit.Sec:
                    return "sec";
                case CalibrationUnit.Mm:
                    return "mm";
                case CalibrationUnit.Mv:
                    return "mV";
                case CalibrationUnit.Bpm:
                    return "bpm";
                case CalibrationUnit.Uncalibrated:
                    return "points";
                default:
                    return string.Empty;
            }
        }

        // TODO: Consider improving these regexs, e.g. mVolt doesn't match, millimeters doesn't match.
		public static bool IsMillisecondsUnit(string input)
		{
			string pattern = @"^(msec|millis|мсек|миллис|ms|мс)$";
			return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
		}

		public static bool IsSecondsUnit(string input)
		{
			string pattern = @"^(sec|сек|s|с)$";
			return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
		}
		public static bool IsMillimetersUnit(string input)
		{
			string pattern = @"^(millim|миллим|mm|мм)$";
			return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
		}

		public static bool IsMillivoltsUnit(string input)
		{
			string pattern = @"^(milliv|mv|миллив|мв)$";
			return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
		}

        public static CalibrationUnit StringToCalibrationUnit(string input)
        {
            if (string.IsNullOrEmpty(input)) return CalibrationUnit.Unknown;
            if (IsMillimetersUnit(input)) return CalibrationUnit.Mm;
            if (IsMillisecondsUnit(input)) return CalibrationUnit.Msec;
            if (IsSecondsUnit(input)) return CalibrationUnit.Sec;
            if (IsMillivoltsUnit(input)) return CalibrationUnit.Mv;
            return CalibrationUnit.Unknown;
        }
	}
}
