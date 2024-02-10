namespace EPCalipersWinUI3.Models.Calipers
{
	public sealed class AngleCalibration : Calibration
	{
		public Calibration TimeCalibration { get; set; } = Calibration.Uncalibrated;
		public Calibration AmplitudeCalibration { get; set; } = Calibration.Uncalibrated;
		public AngleCalibration() : base(1.0, new CalibrationMeasurement(1.0, Unit.Degrees, "°")) { }

		public static new AngleCalibration Uncalibrated => new();

		public override string GetFormattedMeasurement(double interval, bool showBpm = false)
		{
			return base.GetFormattedMeasurement(interval, showBpm);
		}

		public string GetSecondaryText(double interval)
		{
			return TimeCalibration.GetFormattedMeasurement(interval);
		}

	}
}
