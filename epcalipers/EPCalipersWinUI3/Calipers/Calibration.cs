using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Contracts;

namespace EPCalipersWinUI3.Calipers
{
	public class Calibration : ICalibration
	{
		public CalibrationUnits Units { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public double Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public double Multiplier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
}
