using System;
using System.Diagnostics;

namespace EPCalipersWinUI3.Models.Calipers
{
    public class ScaledBarThickness
	{
		private const double _minThickness = 1;
		private const double _maxThickness = 10;
		public double Thickness { get; set; }
		public double ScaleFactor { get; set; }
		public bool DoScaling { get; set; }

		public ScaledBarThickness(double thickness, double scaleFactor, bool doScaling = false)
		{
			Debug.Assert(scaleFactor > 0);
			Thickness = thickness;
			ScaleFactor = scaleFactor;
			DoScaling = doScaling;
		}

		public double ScaledThickness() => DoScaling ? Math.Clamp(Thickness / ScaleFactor, _minThickness, _maxThickness) 
			: Thickness;
	}
}
