using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Windows.Foundation.Point;

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


		// Algorithm from: https://stackoverflow.com/questions/15690103/intersection-between-two-lines-in-coordinates
		// Returns intersection point of two line segments, nil if no intersection.
		public static Point? Intersection(Point p1, Point p2, Point p3, Point p4)
		{
			var d = (p2.X - p1.X) * (p4.Y - p3.Y) - (p2.Y - p1.Y) * (p4.X - p3.X);
			if (d == 0) {
				return null; // parallel lines
			}
			var u = ((p3.X - p1.X) * (p4.Y - p3.Y) - (p3.Y - p1.Y) * (p4.X - p3.X)) / d;
			var v = ((p3.X - p1.X) * (p2.Y - p1.Y) - (p3.Y - p1.Y) * (p2.X - p1.X)) / d;
			if (u < 0.0 || u > 1.0)
			{
				return null; // intersection point not between p1 and p2
			}
			if (v < 0.0 || v > 1.0)
			{
				return null; // intersection point not between p3 and p4
			}
			var intersection = new Point();
			intersection.X = p1.X + u * (p2.X - p1.X);
			intersection.Y = p1.Y + u * (p2.Y - p1.Y);
			return intersection;
		}
	}
}
