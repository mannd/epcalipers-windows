using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers
{
    class AngleCaliper : Caliper
    {
       // private static float differential = 0.0f;

        float angleBar1 = (float)(0.5 * Math.PI);
        float angleBar2 = (float)(0.25 * Math.PI);
        Calibration verticalCalibration;

        const double angleDelta = 0.15;

        public AngleCaliper() : base()
        {
            isAngleCaliper = true;
            requiresCalibration = false;   
        }

        public override void SetInitialPositionInRect(RectangleF rect)
        {
            base.SetInitialPositionInRect(rect);
            Bar2Position = Bar1Position;
        }
    }
}
