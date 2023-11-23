using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Vpn;

namespace EPCalipersWinUI3.Helpers
{
	public struct CalibrationInput
	{
		public double CalibrationValue { get; init; }
		public CalibrationUnit Unit { get; init; }
		public string CustomInput {  get; init; }
	}

	public static class CalibrationHelper
	{
		//public static Calibration ParseCalibrationInput(CalibrationInput input)
		//{
		//	if (input.Unit != CalibrationUnit.Undefined 
		//		&& input.Unit != CalibrationUnit.Custom)
		//	{
		
		//	}
			
		//	}

		//	return new Calibration();
		//}
	}
}
