using System;
using System.Collections.Generic;
using System.Linq;
Using System.Text;
using System.Threading.Tasks;

namespace epcalipers
{
    public class QtcCalculator
    {

	public enum QTcFormula
	    {
		qtcBzt,
		qtcFrm,
		qtcHdg,
		qtcFrd,
		qtcAll  // calculate all the above QTcs
	    }

	public Dictionary<QTcFormula, string> formulaNames;
	private QtcFormula formula;
	
	private QtcFormula[] allFormulas = {
            QtcFormula.qtcBzt,
            QtcFormula.qtcFrm,
            QtcFormula.qtcHdg,
            QtcFormula.qtcFrd
	};

	QtcCalculator(QtcFormula formula) {
	    this.formula = formula;
	    // format here
	    formulaNames = new Dictionary<QtcFormula, string>;
	    formulaNames.Add(QtcFormula.qtcBzt, "Bazett");
	    formulaNames.Add(QtcFormula.qtcFrm, "Framingham");
	    formulaNames.Add(QtcFormula.qtcHdg, "Hodges");
	    formulaNames.Add(QtcFormula.qtcFrd, "Fridericia"); 
	}

	public string Calculate(double qtInSec, double rrInSec,
				bool convertToMsec, string units)
	{

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
            return  qtInSec / (double)Math.Sqrt(rrInSec);
        }

        public static double QtcBazettMsec(double qt, double rrInMsec)
        {
            return SecToMsec(QtcBazettSec(MsecToSec(qt), MsecToSec(rrInMsec)));
        }

	public static double QtcFrmSec(double qtInSec, double rrInSec) {
	    return qtInSec + 0.154 * (1 - rrInSec);
	}

	public static double QtcHdgSec(double qtInSec, double rrInSec) {
	    return qtInSec + 0.00175 * (60.0 / rrInSec - 60);
	}

	public static double QtcFrdSec(double qtInSec, double rrInSec) {
	    return qtInSec / Math.Pow(rrInSec, 1 / 3.0);
	}
}
