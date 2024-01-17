using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Models.Calipers
{
	public sealed class AngleCalibration : Calibration
	{
		public Calibration TimeCalibration { get; set; } = Calibration.Uncalibrated;
		public Calibration AmplitudeCalibration { get; set; } = Calibration.Uncalibrated;
		public AngleCalibration() : base(1.0, new CalibrationParameters(1.0, CalibrationUnit.Degrees, "°")) { }

		public static new AngleCalibration Uncalibrated => new AngleCalibration();

		public override string GetText(double interval, bool showBpm = false)
		{
			return base.GetText(interval, showBpm);
		}

		// TODO: refactor to match Time Caliper rounding, force msec, etc.
		public string GetSecondaryText(double interval)
		{
			return TimeCalibration.GetText(interval);
			//var value = GetRoundedValue(interval, showBpm: false);
			//return string.Format("{0} {1}", value, unit);
		}

	}
}
