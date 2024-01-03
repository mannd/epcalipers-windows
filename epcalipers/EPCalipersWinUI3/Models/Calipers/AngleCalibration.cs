using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Models.Calipers
{
	public sealed class AngleCalibration: Calibration
	{
		public AngleCalibration() : base(1.0, new CalibrationParameters(1.0, CalibrationUnit.Degrees, "°")) { }

		public override string GetText(double interval, bool showBpm = false)
		{
			return string.Format("{0:0.#} {1}", interval, Parameters.UnitString);
		}

		public override string GetSecondaryText(double interval, string unit)
		{
			return string.Format("{0} {1}", interval, unit);
		}
	}
}
