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
        public double Value { get; init; }
        public CalibrationUnit Unit { get; init; }
        public string UnitString { get; init; }

        public CalibrationParameters(double value, CalibrationUnit unit, string unitString)
        {
            Value = value;
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
                return string.Format("{0:0#} {1}", Parameters.Value, Parameters.UnitString);
            }
        }

        /// <summary>
        /// Provides a calibration factor that calculates the actual interval.
        /// </summary>
        /// <param name="value">The value of the caliper in points at the time of calibration.</param>
        /// <param name="input">The desired calibration interval parameters, e.g. "1000 msec"</param>
        public Calibration(double value, CalibrationParameters parameters)
        {
            Parameters = parameters;
            Multiplier = Parameters.Value / value;
        }

        public Calibration()
        {
            Parameters = new CalibrationParameters
            {
                Unit = CalibrationUnit.Uncalibrated,
                UnitString = "points", // Note this is not localized
                Value = 1
            };
            Multiplier = 1.0;
        }

        public readonly string GetText(double value)
        {
            var calibratedValue = Multiplier * value;
            return string.Format("{0:0.00#} {1}", calibratedValue, Parameters.UnitString);
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
