using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Contracts
{
	public enum CalibrationUnit
	{
		Msec,
		Sec,
		Mv,
		Mm,
		Bpm,
		Custom,
		Uncalibrated
	}

	public interface ICalibration
	{
		public CalibrationUnit Unit { get; set; }
		public double Value { get; set; }
	}
}
