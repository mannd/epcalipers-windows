using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Contracts;
using TemplateTest2.Helpers;

namespace EPCalipersWinUI3.Calipers
{
	public struct Calibration : ICalibration
	{
		public CalibrationUnit Unit { get; set; }
		public double Value { get; set; }
		public double CalibrationValue { get; set; }
		public string UnitString { get; set; }

		public Calibration(double value, double calibrationValue, CalibrationUnit unit)
		{
			Unit = unit;
			Value = value;
			CalibrationValue = calibrationValue;
			UnitString = unit.ToString();
		}

		public void Clear()
		{
		}

		public static string CalibrationUnitToString(CalibrationUnit unit)
		{
			switch (unit)
			{
				case CalibrationUnit.Msec:
					return "msec".GetLocalized();
				case CalibrationUnit.Sec:
					return "sec".GetLocalized();
				case CalibrationUnit.Mm:
					return "mm".GetLocalized();
				case CalibrationUnit.Mv:
					return "mV".GetLocalized();
				default:
					return string.Empty;
			}
		}


	}
}
