using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersCore
{
    public interface ICalipers
    {
        int NumberOfCalipers();
        bool NoCaliperIsSelected();
        void SelectSoleCaliper();
        BaseCaliper GetActiveCaliper();

        
        Calibration HorizontalCalibration { get; set; }
        Calibration VerticalCalibration { get; set; }
    }
}
