using System;
using System.Collections.Generic;

namespace EPCalipersCore
{

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
				qtc = EPCalculator.Calculate(qtcFormula, qtInSec, rrInSec);
				if (double.IsInfinity(qtc) || double.IsNaN(qtc))
				{
					return errorResult;
				}
				// TODO: is this needed?
				//                qtc = (Math.Round(qtc * 100000.0) / 100000.0);
				if (convertToMsec)
				{
					qtc *= 1000.0;
				}
				result += string.Format("\nQTc = {0} {1} ({2} formula)", qtc.ToString("G4"), units, formulaNames[qtcFormula]);
			}
			return result;
		}


	}

	public static class EPCalculator
	{
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
