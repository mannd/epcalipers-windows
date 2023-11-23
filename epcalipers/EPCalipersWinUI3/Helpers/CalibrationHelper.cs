using EPCalipersWinUI3.Calipers;
using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Helpers
{
	public struct CalibrationInput
	{
		double CalibrationValue { get; init; }
		CalibrationUnit Unit { get; init; }
		string CustomInput; {  get; init; }
	}

	public static class CalibrationHelper
	{
		public static Calibration ParseCalibrationInput(CalibrationInput input)
		{
			var calibration = new Calibration();

			return new Calibration();
		}
	}
}
