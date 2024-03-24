using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Models.Calipers
{
	public class BarThickness
	{
		public double Thickness;
		public double ScaleFactor;
		public bool ScaleThickness;

		public BarThickness(double thickness, double scaleFactor, bool scaleThickness = false)
		{
			Debug.Assert(scaleFactor > 0);
			Thickness = thickness;
			ScaleFactor = scaleFactor;
			ScaleThickness = scaleThickness;
		}

		public double ScaledThickness() => ScaleThickness ? Thickness / ScaleFactor : Thickness;
	}
}
