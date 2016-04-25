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
        public float valueInPoints {
            get { return bar2Position - bar1Position; }
        }
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

        public void draw(Graphics g, RectangleF rect)
        {
            Brush brush = new SolidBrush(color);
            Pen pen = new Pen(brush, lineWidth);
            if (direction == CaliperDirection.Horizontal)
            {
                crossbarPosition = Math.Min(crossbarPosition, rect.Size.Height - DELTA);
                crossbarPosition = Math.Max(crossbarPosition, DELTA);
                bar1Position = Math.Min(bar1Position, rect.Size.Width - DELTA);
                bar2Position = Math.Max(bar2Position, DELTA);
                g.DrawLine(pen, bar1Position, 0.0f, bar1Position, rect.Size.Height);
                g.DrawLine(pen, bar2Position, 0.0f, bar2Position, rect.Size.Height);
                g.DrawLine(pen, bar2Position, crossbarPosition, bar1Position, crossbarPosition);
            }
            else
            {
                crossbarPosition = Math.Min(crossbarPosition, rect.Size.Width - DELTA);
                crossbarPosition = Math.Max(crossbarPosition, DELTA);
                bar1Position = Math.Min(bar1Position, rect.Size.Height - DELTA);
                bar2Position = Math.Max(bar2Position, DELTA);
                g.DrawLine(pen, 0.0f, bar1Position, rect.Size.Width, bar1Position);
                g.DrawLine(pen, 0.0f, bar2Position, rect.Size.Width, bar2Position);
                g.DrawLine(pen, crossbarPosition, bar2Position, crossbarPosition, bar1Position);              
            }
            String text = measurement();
            SizeF sizeOfString = g.MeasureString(text, textFont);
            float stringWidth = sizeOfString.Width / 2;
            float firstBarPosition = bar2Position > bar1Position ? bar1Position : bar2Position;
            float center = firstBarPosition + Math.Abs(bar2Position - bar1Position);
            if (direction == CaliperDirection.Horizontal)
            {
                g.DrawString(text, textFont, brush, center - stringWidth, crossbarPosition - 20);
            }
            else
            {
                g.DrawString(text, textFont, brush, crossbarPosition + 5, bar1Position
                    + (bar2Position - bar1Position) / 2);
            }
        }


        // returns significant bar coordinate depending on direction of caliper
        public float barCoord(PointF p)
        {
            return direction == CaliperDirection.Horizontal ? p.X : p.Y;
        }

        private String measurement()
        {
            String s = String.Format("%.4g %s", calibratedResult(),
                calibration.units);
            return s;
        }

        private float calibratedResult()
        {
            float result = intervalResult();
            if (result != 0.0f && calibration.displayRate &&
                calibration.canDisplayRate)
            {
                result = rateResult(result);
            }
            return result;
        }

        private float intervalResult()
        {
            return valueInPoints * calibration.multiplier;
        }

        private float rateResult(float interval)
        {
            if (interval != 0.0f)
            {
                if (calibration.unitsAreMsecs)
                {
                    interval = 60000.0f / interval;
                }
                else if (calibration.unitsAreSeconds)
                {
                    interval = 60.0f / interval;
                }
            }
            return interval;
        }

        private float intervalInSecs(float interval)
        {
            if (calibration.unitsAreSeconds)
            {
                return interval;
            }
            else
            {
                return interval / 1000.0f;
            }
        }

        private float intervalInMsec(float interval)
        {
            if (calibration.unitsAreMsecs)
            {
                return interval;
            }
            else
            {
                return 1000.0f * interval;
            }
        }

        public Boolean pointNearBar(PointF p, float barPosition)
        {
            return barCoord(p) > barPosition - DELTA && barCoord(p) < barPosition + DELTA;
        }

        public Boolean pointNearCrossbar(PointF p)
        {
            Boolean nearBar;
            float delta = DELTA + 5.0f;
            if (direction == CaliperDirection.Horizontal)
            {
                nearBar = (p.X > Math.Min(bar1Position, bar2Position) && p.X < Math.Max(bar2Position, bar1Position) && p.Y > crossbarPosition - delta && p.Y < crossbarPosition + delta);

            }
            else
            {
                nearBar = (p.Y > Math.Min(bar1Position , bar2Position) &&
                    p.Y < Math.Max(bar2Position, bar1Position) && p.X > crossbarPosition - delta &&
                    p.X < crossbarPosition + delta);
            }
            return nearBar;
        }

        private Boolean pointNearCaliper(PointF p)
        {
            return pointNearCrossbar(p) || pointNearBar(p, bar1Position) ||
                pointNearBar(p, bar2Position);
        }

    }
}
