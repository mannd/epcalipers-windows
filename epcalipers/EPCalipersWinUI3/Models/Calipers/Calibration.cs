using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;

namespace EPCalipersWinUI3.Models.Calipers
{
    public struct RawCalibrationInput
    {
        public double CalibrationValue { get; init; }
        public CalibrationUnit Unit { get; init; }
        public string CustomInput { get; init; }

        public RawCalibrationInput(double value, CalibrationUnit unit, string customInput)
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
                return string.Format("{0:0#} {1}", Parameters.Value, Parameters.UnitString);

            }
        }


        public Calibration(double value, RawCalibrationInput input)
        {
            Parameters = ParseInput(input);
            Multiplier = Parameters.Value / value;
        }

        public static CalibrationParameters ParseInput(RawCalibrationInput input)
        {
            // ignore custom for now
            if (input.Unit == CalibrationUnit.Custom)
            {
                var (value, unitString) = ParseCustomString(input.CustomInput);

                return new CalibrationParameters
                {
                    Value = value,
                    UnitString = unitString,
                    Unit = StringToCalibrationUnit(unitString)
                };
            }
            return new CalibrationParameters {
                Value = input.CalibrationValue, 
                Unit = input.Unit, 
                UnitString = CalibrationUnitToString(input.Unit)
            };
        }

        public static CalibrationParameters ParseCustomInput(string customInput)
        {
            (double value, string units) = ParseCustomString(customInput);
            return new CalibrationParameters
            {
                Value = value,
                Unit = CalibrationUnit.Undefined,
                UnitString = units
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
            if (string.IsNullOrEmpty(input)) return CalibrationUnit.Undefined;
            if (IsMillimetersUnit(input)) return CalibrationUnit.Mm;
            if (IsMillisecondsUnit(input)) return CalibrationUnit.Msec;
            if (IsSecondsUnit(input)) return CalibrationUnit.Sec;
            if (IsMillivoltsUnit(input)) return CalibrationUnit.Mv;
            return CalibrationUnit.Undefined;
        }
	}
}
