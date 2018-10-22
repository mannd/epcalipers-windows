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
        BaseCalipers calipers = new BaseCalipers();
        public Calibration HorizontalCalibration {
            get
            {
                return calipers.HorizontalCalibration;
            }
            set
            {
                calipers.HorizontalCalibration = value;
            }
        }
        public Calibration VerticalCalibration {
            get
            {
                return calipers.VerticalCalibration;
            }
            set
            {
                calipers.VerticalCalibration = value;
            }
        }

        public CalipersCanvas(): base()
        {

        }

        public void AddCaliper(BaseCaliper c)
        {
            calipers.addCaliper(c);
        }

        public void DrawCalipers()
        {
            foreach(BaseCaliper c in calipers.GetCalipers())
            {
                DrawCaliper(c);
            }
        }

        private void DrawCaliper(BaseCaliper c)
        {
            c.Draw(this);
        }
    }
}
