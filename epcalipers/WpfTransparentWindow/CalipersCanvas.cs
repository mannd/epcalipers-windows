using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EPCalipersCore;

namespace WpfTransparentWindow
{
    class CalipersCanvas: Canvas
    {
        List<BaseCaliper> calipers = new List<BaseCaliper>();
        public bool Locked { get; set; }
        public Calibration HorizontalCalibration { get; set; }
        public Calibration VerticalCalibration { get; set; }

        // for caliper movement
        private BaseCaliper grabbedCaliper;
        private bool crossbarGrabbed = false;
        private bool bar1Grabbed = false;
        private bool bar2Grabbed = false;
        private bool caliperWasDragged = false;


        public CalipersCanvas(): base()
        {

        }
    }
}
