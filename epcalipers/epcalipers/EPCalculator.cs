using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers
{
    public static class EPCalculator
    {
        public static float MsecToBpm(float interval)
        {
            return 60000.0f / interval;
        }

        public static float BpmToMsec(float rate)
        {
            return 60000.0f / rate;
        }

        public static float SecToBpm(float interval)
        {
            return 60.0f / interval;
        }

        public static float BpmToSec(float rate)
        {
            return 60.0f / rate;
        }

        public static float MsecToSec(float interval)
        {
            return interval / 1000.0f;
        }

        public static float SecToMsec(float interval)
        {
            return interval * 1000.0f;
        }

        public static float MeanInterval(float interval, int numberOfIntervals)
        {
            return interval / numberOfIntervals;
        }
    }
}
