using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Models.Calipers
{
	public class ScaledBarThickness
	{
		public double Thickness;
		public double ScaleFactor;
		public bool DoScaling;

		public ScaledBarThickness(double thickness, double scaleFactor, bool doScaling = false)
		{
			Debug.Assert(scaleFactor > 0);
			Thickness = thickness;
			ScaleFactor = scaleFactor;
			DoScaling = doScaling;
		}

		public double ScaledThickness() => DoScaling ? Thickness / ScaleFactor : Thickness;
	}
}
