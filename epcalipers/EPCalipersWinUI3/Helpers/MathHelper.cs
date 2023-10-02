using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Helpers
{
	public class MathHelper
	{
		// From https://stackoverflow.com/questions/33866535/how-to-scale-a-rotated-rectangle-to-always-fit-another-rectangle
		public static double ScaleToFit(double width, double height, double angle)
		{
			// Convert angle to radians.
			var theta = angle * Math.PI / 180.0;

			// Compute W and H of bounds of rotated rect.
			var W = width * Math.Abs(Math.Cos(theta)) + height * Math.Abs(Math.Sin(theta));
			var H = width * Math.Abs(Math.Sin(theta)) + height * Math.Abs(Math.Cos(theta));

			var scalingFactor = Math.Min(width / W, height / H);
			return scalingFactor;
		}
	}
}
