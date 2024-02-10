using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Models.Calipers
{
	public readonly struct Measurement
	{
		public Measurement(double value, Unit unit, string unitString)
		{
			Value = value;
			Unit = unit;
			UnitString = unitString;
		}

		public Measurement()
		{
			Value = 0;
			Unit = Unit.None;
			UnitString = "";
		}

		public double Value {  get; init; }
		public Unit Unit { get; init; }
		public string UnitString { get; init; }
	}
}
