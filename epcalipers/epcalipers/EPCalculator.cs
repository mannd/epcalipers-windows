using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers
{
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
    }
}
