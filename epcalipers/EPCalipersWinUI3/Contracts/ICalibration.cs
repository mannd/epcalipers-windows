using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Contracts
{
	public enum CalibrationUnits
	{
		Msec,
		Sec,
		Mv,
		Mm,
		Custom
	}

	public interface ICalibration
	{
		public CalibrationUnits Units { get; set; }
		public double Value { get; set; }
		public double Multiplier { get; set; }
	}
}
