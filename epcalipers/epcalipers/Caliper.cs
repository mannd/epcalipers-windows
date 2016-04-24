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
        private float DELTA = 20.0f;
        private int tmpLineWidth;
        private static float differential = 0.0f;

        public float bar1Position { set; get; }
        public float bar2Position { set; get; }
        public float crossbarPosition { set; get; }
        public CaliperDirection direction { set; get; }
        public Color color { set; get; }
        public Color unselectedColor { set; get; }
        public Color selectedColor { set; get; }
        public int lineWidth { set; get; }
        public float valueInPoints { set; get; }
        public Boolean isSelected { set; get; }
        public Calibration calibration { set; get; }
        public Font textFont { set; get; }

        public Caliper()
        {
            initWithDirection(CaliperDirection.Horizontal, 0.0f, 0.0f, 100.0f);
        }

        public void initWithDirection(
            CaliperDirection direction, 
            float bar1Position,
            float bar2Position,
            float crossbarPosition)
        {
            this.direction = direction;
            this.bar1Position = bar1Position;
            this.bar2Position = bar2Position;
            this.crossbarPosition = crossbarPosition;
            unselectedColor = Color.Blue;
            selectedColor = Color.Red;
            color = Color.Blue;
            lineWidth = 2;
            isSelected = false;
            textFont = new Font("Helvetica", 18);
            // paragraph style ?
        }

        public void setInitialPositionInRect(RectangleF rect)
        {
           if (direction == CaliperDirection.Horizontal)
            {
                bar1Position = (rect.Size.Width / 3.0f) + differential;
                bar2Position = ((2.0f * rect.Size.Width) / 3.0f) + differential;
                crossbarPosition = (rect.Size.Height / 2.0f) + differential;
            }
           else
            {
                bar1Position = (rect.Size.Height / 3.0f) + differential;
                bar2Position = ((2.0f * rect.Size.Height) / 3.0f) + differential;
                crossbarPosition = (rect.Size.Width / 2.0f) + differential;            
            }
            differential += 15.0f;
            if (differential > 80.0f)
            {
                differential = 0.0f;
            }
        }

    }
}
