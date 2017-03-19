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
        protected float DELTA = 20.0f;
        //private int tmpLineWidth;
        private static float differential = 0.0f;

        public float Bar1Position { set; get; }
        public float Bar2Position { set; get; }
        public float CrossbarPosition { set; get; }
        public CaliperDirection Direction { set; get; }
        public Color CaliperColor { set; get; }
        public Color UnselectedColor { set; get; }
        public Color SelectedColor { set; get; }
        public int LineWidth { set; get; }
        public float ValueInPoints {
            get { return Bar2Position - Bar1Position; }
        }
        public bool IsSelected { set; get; }
        public Calibration CurrentCalibration { set; get; }
        public Font TextFont { set; get; }
        public bool RoundMsecRate { set; get; }
        public bool hasHandles { set; get; }

        protected bool caliperIsAngleCaliper = false;
        protected bool caliperRequiresCalibration = true;

        // added for AngleCaliper derived class
        public bool requiresCalibration
        {
            get { return caliperRequiresCalibration; }
        }

        public bool isAngleCaliper
        {
            get { return caliperIsAngleCaliper; }
        }

        public Caliper()
        {
            InitWithDirection(CaliperDirection.Horizontal, 0.0f, 0.0f, 100.0f);
        }

        public void InitWithDirection(
            CaliperDirection direction, 
            float bar1Position,
            float bar2Position,
            float crossbarPosition)
        {
            Direction = direction;
            Bar1Position = bar1Position;
            Bar2Position = bar2Position;
            CrossbarPosition = crossbarPosition;
            UnselectedColor = Color.Blue;
            SelectedColor = Color.Red;
            CaliperColor = Color.Blue;
            LineWidth = 2;
            IsSelected = false;
            TextFont = new Font("Helvetica", 14);
            CurrentCalibration = new Calibration();
            RoundMsecRate = true;
            hasHandles = false;
        }

        public virtual void SetInitialPositionInRect(RectangleF rect)
        {
            // This is better than setting in middle, because caliper can become lost.
            SetInitialPositionNearCorner(rect);
            return;
        }

        private void setInitialPositionNearMiddle(RectangleF rect)
        {
            if (Direction == CaliperDirection.Horizontal)
            {
                Bar1Position = (rect.Size.Width / 3.0f) + differential;
                Bar2Position = ((1.5f * rect.Size.Width) / 3.0f) + differential;
                CrossbarPosition = (rect.Size.Height / 2.0f) + differential;
            }
            else
            {
                Bar1Position = (rect.Size.Height / 3.0f) + differential;
                Bar2Position = ((1.5f * rect.Size.Height) / 3.0f) + differential;
                CrossbarPosition = (rect.Size.Width / 2.0f) + differential;
            }
            differential += 15.0f;
            if (differential > 80.0f)
            {
                differential = 0.0f;
            }
        }

        private void SetInitialPositionNearCorner(RectangleF rect)
        {
            Bar1Position = 50 + differential;
            Bar2Position = 100 + differential;
            CrossbarPosition = 100 + differential;
            differential += 15.0f;
            if (differential > 80.0f)
            {
                differential = 0.0f;
            }
        }

        public virtual void Draw(Graphics g, RectangleF rect)
        {
            Brush brush = new SolidBrush(CaliperColor);
            Pen pen = new Pen(brush, LineWidth);
            if (Direction == CaliperDirection.Horizontal)
            {
                CrossbarPosition = Math.Min(CrossbarPosition, rect.Size.Height - DELTA);
                CrossbarPosition = Math.Max(CrossbarPosition, DELTA);
                Bar1Position = Math.Min(Bar1Position, rect.Size.Width - DELTA);
                Bar2Position = Math.Max(Bar2Position, DELTA);
                g.DrawLine(pen, Bar1Position, 0.0f, Bar1Position, rect.Size.Height);
                g.DrawLine(pen, Bar2Position, 0.0f, Bar2Position, rect.Size.Height);
                g.DrawLine(pen, Bar2Position, CrossbarPosition, Bar1Position, CrossbarPosition);
                if (hasHandles)
                {
                    DrawHorizontalHandles(g, brush);
                }
            }
            else
            {
                CrossbarPosition = Math.Min(CrossbarPosition, rect.Size.Width - DELTA);
                CrossbarPosition = Math.Max(CrossbarPosition, DELTA);
                Bar1Position = Math.Min(Bar1Position, rect.Size.Height - DELTA);
                Bar2Position = Math.Max(Bar2Position, DELTA);
                g.DrawLine(pen, 0.0f, Bar1Position, rect.Size.Width, Bar1Position);
                g.DrawLine(pen, 0.0f, Bar2Position, rect.Size.Width, Bar2Position);
                g.DrawLine(pen, CrossbarPosition, Bar2Position, CrossbarPosition, Bar1Position);
                if (hasHandles)
                {
                    DrawVerticalHandles(g, brush);
                }
            }
            CaliperText(g, brush);
            pen.Dispose();
            brush.Dispose();
        }

        private void DrawHorizontalHandles(Graphics g, Brush brush)
        {
            int x1 = (int)(Bar2Position >= Bar1Position ? Bar1Position : Bar2Position);
            int x2 = (int)(Bar2Position >= Bar1Position ? Bar2Position : Bar1Position);
            g.FillRectangle(brush, new Rectangle(x1 - 20, (int)CrossbarPosition - 5, 20, 10));
            g.FillRectangle(brush, new Rectangle(x2, (int)CrossbarPosition - 5, 20, 10));
            g.FillRectangle(brush, new Rectangle((int)((Bar2Position - Bar1Position) / 2.0 + Bar1Position - 10), (int)CrossbarPosition, 20, 10));
        }

        private void DrawVerticalHandles(Graphics g, Brush brush)
        {
            int x1 = (int)(Bar2Position >= Bar1Position ? Bar1Position : Bar2Position);
            int x2 = (int)(Bar2Position >= Bar1Position ? Bar2Position : Bar1Position);
            g.FillRectangle(brush, new Rectangle((int)CrossbarPosition - 5, x1 - 20, 10, 20));
            g.FillRectangle(brush, new Rectangle((int)CrossbarPosition - 5, x2, 10, 20));
            g.FillRectangle(brush, new Rectangle((int)CrossbarPosition - 10, (int)(Bar2Position - ((Bar2Position - Bar1Position) / 2.0) - 10),
                10, 20));
        }

        protected void CaliperText(Graphics g, Brush brush)
        {
            string text = Measurement();
            SizeF sizeOfString = g.MeasureString(text, TextFont);
            float stringWidth = sizeOfString.Width;
            float stringHeight = sizeOfString.Height;
            float firstBarPosition = Bar2Position > Bar1Position ? Bar1Position : Bar2Position;
            float center = firstBarPosition + (Math.Abs(Bar2Position - Bar1Position) / 2);
            if (Direction == CaliperDirection.Horizontal)
            {
                g.DrawString(text, TextFont, brush, center - stringWidth / 2, CrossbarPosition - 30);
            }
            else
            {
                g.DrawString(text, TextFont, brush, CrossbarPosition + 5, center - stringHeight / 2);
            }
        }


        // returns significant bar coordinate depending on direction of caliper
        public float BarCoord(PointF p)
        {
            return Direction == CaliperDirection.Horizontal ? p.X : p.Y;
        }

        protected virtual string Measurement()
        {
            // "%.4g %s"
            /// TODO: Change below mimics behavior of the other versions of this app.
            // Original code rounds to nearest number, which is ok for msec but not for 
            // mV or sec.  Consider preference to allow rounding only if UnitsAreMsec() or displaying
            // rate and apply it here.  Below shows how to do either method, depending on UnitsAreMsec()
            string s;
            if (RoundMsecRate && (CurrentCalibration.UnitsAreMsecs || CurrentCalibration.DisplayRate))
            {
                s = string.Format("{0} {1}", Math.Round(CalibratedResult()),
                    CurrentCalibration.Units);
            }
            else
            { 
                s = string.Format("{0} {1}", CalibratedResult().ToString("G4"), CurrentCalibration.Units);
            }
            return s;
        }

        private double CalibratedResult()
        {
            double result = IntervalResult();
            if (result != 0.0f && CurrentCalibration.DisplayRate &&
                CurrentCalibration.CanDisplayRate)
            {
                result = RateResult(result);
            }
            return result;
        }

        public virtual double IntervalResult()
        {
            return ValueInPoints * CurrentCalibration.Multiplier;
        }

        private double RateResult(double interval)
        {
            if (interval != 0.0)
            {
                if (CurrentCalibration.UnitsAreMsecs)
                {
                    interval = EPCalculator.MsecToBpm(interval);
                }
                else if (CurrentCalibration.UnitsAreSeconds)
                {
                    interval = EPCalculator.SecToBpm(interval);
                }
            }
            return interval;
        }

        private double IntervalInSecs(double interval)
        {
            if (CurrentCalibration.UnitsAreSeconds)
            {
                return interval;
            }
            else
            {
                return EPCalculator.SecToMsec(interval);
            }
        }

        private double IntervalInMsec(double interval)
        {
            if (CurrentCalibration.UnitsAreMsecs)
            {
                return interval;
            }
            else
            {
                return EPCalculator.SecToMsec(interval);
            }
        }

        private bool PointNearBar(PointF p, float barPosition)
        {
            return BarCoord(p) > barPosition - DELTA && BarCoord(p) < barPosition + DELTA;
        }

        public virtual bool PointNearBar1(PointF p)
        {
            return PointNearBar(p, Bar1Position);
        }

        public virtual bool PointNearBar2(PointF p)
        {
            return PointNearBar(p, Bar2Position);
        }

        public virtual bool PointNearCrossbar(PointF p)
        {
            bool nearBar;
            float delta = DELTA + 5.0f;
            if (Direction == CaliperDirection.Horizontal)
            {
                nearBar = (p.X > Math.Min(Bar1Position, Bar2Position) && p.X < Math.Max(Bar2Position, Bar1Position) && p.Y > CrossbarPosition - delta && p.Y < CrossbarPosition + delta);

            }
            else
            {
                nearBar = (p.Y > Math.Min(Bar1Position , Bar2Position) &&
                    p.Y < Math.Max(Bar2Position, Bar1Position) && p.X > CrossbarPosition - delta &&
                    p.X < CrossbarPosition + delta);
            }
            return nearBar;
        }

        public bool PointNearCaliper(PointF p)
        {
            return PointNearCrossbar(p) || PointNearBar1(p) ||
                PointNearBar2(p);
        }

        #region Movement

        public virtual void MoveCrossbar(PointF delta)
        {
            Bar1Position += delta.X;
            Bar2Position += delta.X;
            CrossbarPosition += delta.Y;
        }

        public virtual void MoveBar1(PointF delta, PointF location)
        {
            Bar1Position += delta.X;
        }

        public virtual void MoveBar2(PointF delta, PointF location)
        {
            Bar2Position += delta.X;
        }

        #endregion
    }
}
