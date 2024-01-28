namespace EPCalipersWinUI3.Models.Calipers
{
	public sealed class AngleCalibration : Calibration
	{
		public Calibration TimeCalibration { get; set; } = Calibration.Uncalibrated;
		public Calibration AmplitudeCalibration { get; set; } = Calibration.Uncalibrated;
		public AngleCalibration() : base(1.0, new CalibrationParameters(1.0, CalibrationUnit.Degrees, "°")) { }

		public static new AngleCalibration Uncalibrated => new();

		public override string GetText(double interval, bool showBpm = false)
		{
			return base.GetText(interval, showBpm);
		}

		public string GetSecondaryText(double interval)
		{
			return TimeCalibration.GetText(interval);
		}

	}
}
