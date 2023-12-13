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
    public readonly struct Calibration
    {
        public CalibrationParameters Parameters { get; init; }
        public readonly double Multiplier { get; init; }
        public string Text
        {
            get
            {
                return string.Format("{0:0#} {1}", 
                    Parameters.CalibrationInterval, 
                    Parameters.UnitString);
            }
        }

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

        public readonly string GetText(double interval)
        {
            var valueUnit = CalibratedInterval(interval);
            return string.Format("{0:0.00#} {1}", valueUnit.Item1, valueUnit.Item2);
        }
        private readonly (double, string) CalibratedInterval(double interval)
        {
            if (Parameters.Unit != CalibrationUnit.Bpm)
            {
                return (Multiplier * interval, Parameters.UnitString);
            }
            else
            {
                throw new NotImplementedException();
            }
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
