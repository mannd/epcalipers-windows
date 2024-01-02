﻿using System;
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
        Custom,
        Uncalibrated,
        Unknown
    }
    public readonly struct CalibrationParameters
    {
        public double CalibrationInterval { get; init; }
        public CalibrationUnit Unit { get; init; }
        public string UnitString { get; init; }
        public CalibrationParameters(double interval, CalibrationUnit unit, string unitString)
        {
            CalibrationInterval = interval;
            Unit = unit;
            UnitString = unitString;
        }
    }
    public sealed class ZeroValueException : Exception
    {
        public ZeroValueException() : base("ZeroValueException") { }
    }

    public sealed class EmptyCustomStringException : Exception
    {
        public EmptyCustomStringException() : base("EmptyCustomStringException") { }
    }
    public class Calibration
    {
        // Rounding format strings
		private const string _roundToIntString = "D";
		private const string _roundToFourPlacesString = "G4";
		private const string _roundToTenthsString = "F1";
		private const string _roundToHundredthsString = "F2";
		private const string _noRoundingString = "G8";  // This is too precise for clinical use.

        private IDictionary<Rounding, string> _roundingFormat = new Dictionary<Rounding, string>()
        {
            {Rounding.ToInt, _roundToIntString },
            {Rounding.ToFourPlaces, _roundToFourPlacesString },
            {Rounding.ToTenths, _roundToTenthsString },
            {Rounding.ToHundredths, _roundToHundredthsString },
            {Rounding.None, _noRoundingString },
        };

        public Rounding Rounding { get; set; } = Rounding.ToInt;

        public CalibrationParameters Parameters { get; init; }
        public double Multiplier { get; init; }

        // These two strings are localized by CaliperHelper.
        public static string DefaultUnit { get; set; } = "points";
        public static string DefaultBpm { get; set; } = "bpm";

        private readonly CalibrationUnit _originalUnit;

        /// <summary>
        /// Provides a calibration factor that calculates the actual interval.
        /// </summary>
        /// <param name="value">The value of the caliper in points at the time of calibration.</param>
        /// <param name="input">The desired calibration interval parameters, e.g. "1000 msec"</param>
        public Calibration(double value, CalibrationParameters parameters)
        {
            Parameters = parameters;
            Multiplier = Parameters.CalibrationInterval / value;
            _originalUnit = Parameters.Unit;
        }

        public Calibration()
        {
            Parameters = new CalibrationParameters
            {
                Unit = CalibrationUnit.Uncalibrated,
                UnitString = DefaultUnit,
                CalibrationInterval = 1
            };
            Multiplier = 1.0;
            _originalUnit = Parameters.Unit;
        }

        public static Calibration Uncalibrated => new Calibration(); // Default Calibration.Unit is Uncalibrated.

        public string GetText(double interval, bool showBpm = false)
        {
            var valueUnit = CalibratedInterval(interval, showBpm);
            double value = valueUnit.Item1;
            string unitString = valueUnit.Item2;
            string format = _roundingFormat[Rounding];
            if (Rounding == Rounding.ToInt)
            {
                int intValue = (int)Math.Round(value);
				return string.Format("{0} {1}", intValue.ToString(format), unitString);

            }
            else
            {
				return string.Format("{0} {1}", valueUnit.Item1.ToString(format), valueUnit.Item2);
			}
		}

        private  (double, string) CalibratedInterval(double interval, bool showBpm = false)
        {
            if (showBpm)
            {
                if (Parameters.Unit == CalibrationUnit.Msec)
                {
                    return (MathHelper.AbsMsecToBpm(Multiplier * interval), DefaultBpm);
                }
                if (Parameters.Unit == CalibrationUnit.Sec)
                {
                    return (MathHelper.AbsSecToBpm(Multiplier * interval), DefaultBpm);
                }
            }
			return (Multiplier * interval, Parameters.UnitString);
		}

		public static bool IsMillisecondsUnit(string input)
        {
            string pattern = @"^(msec|мсек|ms|мс)$|^(millis|миллис)";
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsSecondsUnit(string input)
        {
            string pattern = @"^(sec|сек|s|с)$";
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
        public static bool IsMillimetersUnit(string input)
        {
            string pattern = @"^(mm|мм)$|^(millim|миллим)";
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsMillivoltsUnit(string input)
        {
            string pattern = @"^(mv|мв)$|^(milliv|миллив)";
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
