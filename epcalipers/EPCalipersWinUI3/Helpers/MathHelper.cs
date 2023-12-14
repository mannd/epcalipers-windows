using EPCalipersWinUI3.Models.Calipers;
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
			var theta = DegreesToRadians(angle);

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
			if (d == 0)
			{
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

		public static Point Center(Bounds bounds)
		{
			return new Point(bounds.Width / 2.0, bounds.Height / 2.0);
		}

		public static Point OffsetPoint(Point p, double offset)
		{
			return new Point(p.X + offset, p.Y + offset);
		}

		public static double RadiansToDegrees(double radians)
		{
			return radians * 180.0 / Math.PI;
		}

		public static double DegreesToRadians(double degrees)
		{
			return (degrees * Math.PI) / 180.0;
		}

		public static double MsecToBpm(double interval)
		{
			return 60000.0 / interval;
		}

		public static double BpmToMsec(double rate)
		{
			return 60000.0 / rate;
		}

		public static double SecToBpm(double interval)
		{
			return 60.0 / interval;
		}

		public static double BpmToSec(double rate)
		{
			return 60.0 / rate;
		}

		public static double MsecToSec(double interval)
		{
			return interval / 1000.0;
		}

		public static double SecToMsec(double interval)
		{
			return interval * 1000.0;
		}

		public static double MeanInterval(double interval, int numberOfIntervals)
		{
			return interval / numberOfIntervals;
		}

		public enum QtcFormula
		{
			qtcBzt,
			qtcFrm,
			qtcHdg,
			qtcFrd,
			qtcAll  // calculate all the above QTcs
		}

		public class QtcCalculator
		{
			private QtcFormula[] allFormulas = {
			QtcFormula.qtcBzt,
			QtcFormula.qtcFrm,
			QtcFormula.qtcHdg,
			QtcFormula.qtcFrd
	};

			private QtcFormula formula;
			private Dictionary<QtcFormula, string> formulaNames;

			public QtcCalculator(QtcFormula formula)
			{
				this.formula = formula;
				// format here
				formulaNames = new Dictionary<QtcFormula, string>();
				formulaNames.Add(QtcFormula.qtcBzt, "Bazett");
				formulaNames.Add(QtcFormula.qtcFrm, "Framingham");
				formulaNames.Add(QtcFormula.qtcHdg, "Hodges");
				formulaNames.Add(QtcFormula.qtcFrd, "Fridericia");
			}

			public string Calculate(double qtInSec, double rrInSec,
						bool convertToMsec, string units)
			{
				string errorResult = "Invalid Result";
				if (rrInSec <= 0)
				{
					return errorResult;
				}
				QtcFormula[] qtcFormulas;
				double qtc;
				switch (formula)
				{
					case QtcFormula.qtcBzt:
						qtcFormulas = new QtcFormula[] { QtcFormula.qtcBzt };
						break;
					case QtcFormula.qtcFrd:
						qtcFormulas = new QtcFormula[] { QtcFormula.qtcFrd };
						break;
					case QtcFormula.qtcFrm:
						qtcFormulas = new QtcFormula[] { QtcFormula.qtcFrm };
						break;
					case QtcFormula.qtcHdg:
						qtcFormulas = new QtcFormula[] { QtcFormula.qtcHdg };
						break;
					case QtcFormula.qtcAll:
						qtcFormulas = new QtcFormula[] { QtcFormula.qtcBzt, QtcFormula.qtcFrm, QtcFormula.qtcFrd, QtcFormula.qtcHdg };
						break;
					default:
						return errorResult;
				}
				double meanRR = rrInSec;
				double qt = qtInSec;
				if (convertToMsec)
				{
					qt *= 1000.0;
					meanRR *= 1000.0;
				}
				string result = string.Format("Mean RR = {0} {2}\nQT = {1} {2}", meanRR.ToString("G4"),
						qt.ToString("G4"), units);
				foreach (QtcFormula qtcFormula in qtcFormulas)
				{
					qtc = Calculate(qtcFormula, qtInSec, rrInSec);
					if (double.IsInfinity(qtc) || double.IsNaN(qtc))
					{
						return errorResult;
					}
					if (convertToMsec)
					{
						qtc *= 1000.0;
					}
					result += string.Format("\nQTc = {0} {1} ({2} formula)", qtc.ToString("G4"), units, formulaNames[qtcFormula]);
				}
				return result;
			}

			public static double QtcBazettSec(double qtInSec, double rrInSec)
			{
				return qtInSec / (double)Math.Sqrt(rrInSec);
			}

			public static double QtcBazettMsec(double qt, double rrInMsec)
			{
				return SecToMsec(QtcBazettSec(MsecToSec(qt), MsecToSec(rrInMsec)));
			}

			public static double QtcFrmSec(double qtInSec, double rrInSec)
			{
				return qtInSec + 0.154 * (1 - rrInSec);
			}

			public static double QtcHdgSec(double qtInSec, double rrInSec)
			{
				return qtInSec + 0.00175 * (60.0 / rrInSec - 60);
			}

			public static double QtcFrdSec(double qtInSec, double rrInSec)
			{
				return qtInSec / Math.Pow(rrInSec, 1 / 3.0);
			}

			public static double Calculate(QtcFormula formula, double qtInSec, double rrInSec)
			{
				switch (formula)
				{
					case QtcFormula.qtcBzt:
						return QtcBazettSec(qtInSec, rrInSec);
					case QtcFormula.qtcFrd:
						return QtcFrdSec(qtInSec, rrInSec);
					case QtcFormula.qtcFrm:
						return QtcFrmSec(qtInSec, rrInSec);
					case QtcFormula.qtcHdg:
						return QtcHdgSec(qtInSec, rrInSec);
					default:
						return 0.0;
				}
			}
		}
	}
}
