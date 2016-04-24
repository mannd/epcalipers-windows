using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers
{
    public enum CaliperDirection { Horizontal, Vertical };

    public class Caliper
    {
        public float bar1Position { set; get; }
        public float bar2Position { set; get; }
        public float crossbarPosition { set; get; }
        public CaliperDirection direction { set; get; }
        public Color color { set; get; }
        public Color unselectedColor { set; get; }
        public Color selectedColor { set; get; }
        public int lineWidh { set; get; }
        public float valueInPoints { set; get; }
        public Boolean isSelected { set; get; }
        //public Calibration calibration { set; get; }
        public Font textFont { set; get; }


    }
}
