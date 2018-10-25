using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersCore
{
    public interface ICalipers
    {
        Calibration HorizontalCalibration { get; set; }
        Calibration VerticalCalibration { get; set; }
        bool Locked { get; set; }

        int NumberOfCalipers();
        bool NoCaliperIsSelected();
        void SelectSoleCaliper();
        BaseCaliper GetActiveCaliper();
        BaseCaliper GetLoneTimeCaliper();
        void SelectCaliper(BaseCaliper c);
        void UnselectCalipersExcept(BaseCaliper c);
        bool NoTimeCaliperSelected();
    }
}
