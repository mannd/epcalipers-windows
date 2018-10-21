using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersCore;
using EPCalipersCore.Properties;

namespace EPCalipersCore
{
//    public enum CaliperDirection { Horizontal, Vertical };

    public enum CaliperComponent { LeftBar, RightBar, CrossBar, LowerBar, UpperBar, Apex, NoComponent};

    public enum MovementDirection { Up, Down, Left, Right, Stationary };

    public class BaseCaliper
    {
        protected float DELTA = 20.0f;
        //private int tmpLineWidth;
        private static float differential = 0.0f;
        // constants for marching calipers
        protected static readonly float minDistanceForMarch = 20f;
        protected static readonly int maxMarchingCalipers = 20;

        protected string roundToIntString = "D";
        protected string roundToFourPlacesString = "G4";
        protected string roundToTenthsString = "F1";
        protected string roundToHundredthsString = "F2";
        protected string noRoundingString = "G";

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
        public Preferences.Rounding rounding { set; get; }

        protected bool caliperIsAngleCaliper = false;
        protected bool caliperRequiresCalibration = true;

        public bool isMarching = false;

        // added for AngleCaliper derived class
        public bool requiresCalibration
        {
            get { return caliperRequiresCalibration; }
        }

        public bool isAngleCaliper
        {
            get { return caliperIsAngleCaliper; }
        }

        public BaseCaliper()
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
            rounding = Preferences.Rounding.ToInt;
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

        //public virtual void Draw(Graphics g, RectangleF rect)
        //{
        //    Brush brush = new SolidBrush(CaliperColor);
        //    Pen pen = new Pen(brush, LineWidth);
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        //    if (Direction == CaliperDirection.Horizontal)
        //    {
        //        CrossbarPosition = Math.Min(CrossbarPosition, rect.Size.Height - DELTA);
        //        CrossbarPosition = Math.Max(CrossbarPosition, DELTA);
        //        Bar1Position = Math.Min(Bar1Position, rect.Size.Width - DELTA);
        //        Bar2Position = Math.Max(Bar2Position, DELTA);
        //        g.DrawLine(pen, Bar1Position, 0.0f, Bar1Position, rect.Size.Height);
        //        g.DrawLine(pen, Bar2Position, 0.0f, Bar2Position, rect.Size.Height);
        //        g.DrawLine(pen, Bar2Position, CrossbarPosition, Bar1Position, CrossbarPosition);
        //    }
        //    else
        //    {
        //        CrossbarPosition = Math.Min(CrossbarPosition, rect.Size.Width - DELTA);
        //        CrossbarPosition = Math.Max(CrossbarPosition, DELTA);
        //        Bar1Position = Math.Min(Bar1Position, rect.Size.Height - DELTA);
        //        Bar2Position = Math.Max(Bar2Position, DELTA);
        //        g.DrawLine(pen, 0.0f, Bar1Position, rect.Size.Width, Bar1Position);
        //        g.DrawLine(pen, 0.0f, Bar2Position, rect.Size.Width, Bar2Position);
        //        g.DrawLine(pen, CrossbarPosition, Bar2Position, CrossbarPosition, Bar1Position);
        //    }
        //    if (isMarching && isTimeCaliper())
        //    {
        //        drawMarchingCalipers(g, brush, rect);
        //    }
        //    CaliperText(g, brush);
        //    pen.Dispose();
        //    brush.Dispose();
        //}

        //protected void CaliperText(Graphics g, Brush brush)
        //{
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        //    string text = Measurement();
        //    SizeF sizeOfString = g.MeasureString(text, TextFont);
        //    float stringWidth = sizeOfString.Width;
        //    float stringHeight = sizeOfString.Height;
        //    float firstBarPosition = Bar2Position > Bar1Position ? Bar1Position : Bar2Position;
        //    float center = firstBarPosition + (Math.Abs(Bar2Position - Bar1Position) / 2);
        //    if (Direction == CaliperDirection.Horizontal)
        //    {
        //        g.DrawString(text, TextFont, brush, center - stringWidth / 2, CrossbarPosition - 30);
        //    }
        //    else
        //    {
        //        g.DrawString(text, TextFont, brush, CrossbarPosition + 5, center - stringHeight / 2);
        //    }
        //}

        public bool isTimeCaliper()
        {
            return Direction == CaliperDirection.Horizontal && !isAngleCaliper;
        }

        //private void drawMarchingCalipers(Graphics g, Brush brush, RectangleF rect)
        //{
        //    // note that pen width < 1 (e.g. 0) will always just draw as width of 1
        //    Pen pen = new Pen(brush, LineWidth - 1.0f);
        //    float difference = Math.Abs(Bar1Position - Bar2Position);
        //    if (difference < minDistanceForMarch)
        //    {
        //        return;
        //    }
        //    float greaterBar = Math.Max(Bar1Position, Bar2Position);
        //    float lesserBar = Math.Min(Bar1Position, Bar2Position);
        //    float[] biggerBars = new float[maxMarchingCalipers];
        //    float[] smallerBars = new float[maxMarchingCalipers];
        //    float point = greaterBar + difference;
        //    int index = 0;
        //    while (point < rect.Size.Width && index < maxMarchingCalipers)
        //    {
        //        biggerBars[index] = point;
        //        point += difference;
        //        index++;
        //    }
        //    int maxBiggerBars = index;
        //    index = 0;
        //    point = lesserBar - difference;
        //    while (point > 0 && index < maxMarchingCalipers)
        //    {
        //        smallerBars[index] = point;
        //        point -= difference;
        //        index++;
        //    }
        //    int maxSmallerBars = index;
        //    // draw them
        //    int i = 0;
        //    while (i < maxBiggerBars)
        //    {
        //        g.DrawLine(pen, biggerBars[i], 0, biggerBars[i], rect.Size.Height);
        //        i++;
        //    }
        //    i = 0;
        //    while (i < maxSmallerBars)
        //    {
        //        g.DrawLine(pen, smallerBars[i], 0, smallerBars[i], rect.Size.Height);
        //        i++;
        //    }
        //    pen.Dispose();
        //}


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
            if (CurrentCalibration.unitsAreMsecOrRate()) {
                string format;
                switch (rounding)
                {
                    case Preferences.Rounding.ToInt:
                        format = roundToIntString;
                        break;
                    case Preferences.Rounding.ToFourPlaces:
                        format = roundToFourPlacesString;
                        break;
                    case Preferences.Rounding.ToTenths:
                        format = roundToTenthsString;
                        break;
                    case Preferences.Rounding.ToHundredths:
                        format = roundToHundredthsString;
                        break;
                    case Preferences.Rounding.None:
                        format = noRoundingString;
                        break;
                    default:
                        format = roundToIntString;
                        break;
                }
                if (rounding == Preferences.Rounding.ToInt)
                {
                    s = string.Format("{0} {1}", Math.Round(CalibratedResult()),
                    CurrentCalibration.Units);
                }
                else
                {
                    s = string.Format("{0} {1}", CalibratedResult().ToString(format), CurrentCalibration.Units);
                }
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

        public double IntervalInSecs(double interval)
        {
            if (CurrentCalibration.UnitsAreSeconds)
            {
                return interval;
            }
            else
            {
                return EPCalculator.MsecToSec(interval);
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

        #region Tweak

        public static String ComponentName(CaliperComponent component)
        {
            switch (component)
            {
                case CaliperComponent.LeftBar:
                    return "left bar";
                case CaliperComponent.RightBar:
                    return "right bar";
                case CaliperComponent.CrossBar:
                    return "crossbar";
                case CaliperComponent.UpperBar:
                    return "upper bar";
                case CaliperComponent.LowerBar:
                    return "lower bar";
                case CaliperComponent.Apex:
                    return "apex";
                default:
                    return "unknown component";
            }
        }

        // NB: Window coordinates origin are upper left corner, like iOS, not like macOS
        public virtual void MoveBarInDirection(MovementDirection movementDirection, float distance, CaliperComponent component)
        {
            if (component == CaliperComponent.NoComponent)
            {
                return;
            }
            CaliperComponent adjustedComponent = MoveCrossbarInsteadOfSideBar(movementDirection, component) ? CaliperComponent.CrossBar : component;
            if (adjustedComponent == CaliperComponent.CrossBar)
            {
                MoveCrossbarInDirection(movementDirection, distance);
                return;
            }

            if (movementDirection == MovementDirection.Up || movementDirection == MovementDirection.Left)
            {
                distance = -distance;
            }
            // TODO: make sure directions are correct, c.f. iOS version
            switch (adjustedComponent)
            {
                case CaliperComponent.LeftBar:
                case CaliperComponent.UpperBar:
                    Bar1Position += distance;
                    break;
                case CaliperComponent.RightBar:
                case CaliperComponent.LowerBar:
                    Bar2Position += distance;
                    break;
                default:
                    break;
            }
        }

        protected bool MoveCrossbarInsteadOfSideBar(MovementDirection movementDirection, CaliperComponent component)
        {
            if (component == CaliperComponent.CrossBar || component == CaliperComponent.Apex)
            {
                return false;
            }
            return (Direction == CaliperDirection.Horizontal && 
                (movementDirection == MovementDirection.Up || movementDirection == MovementDirection.Down)) 
                ||
                (Direction == CaliperDirection.Vertical && 
                (movementDirection == MovementDirection.Left || movementDirection == MovementDirection.Right));
        }

        private void MoveCrossbarInDirection(MovementDirection movementDirection, float distance)
        {
            if (Direction == CaliperDirection.Vertical)
            {
                movementDirection = SwapDirection(movementDirection);
            }
            switch (movementDirection)
            {
                case MovementDirection.Up:
                    CrossbarPosition -= distance;
                    break;
                case MovementDirection.Down:
                    CrossbarPosition += distance;
                    break;
                case MovementDirection.Left:
                    Bar1Position -= distance;
                    Bar2Position -= distance;
                    break;
                case MovementDirection.Right:
                    Bar1Position += distance;
                    Bar2Position += distance;
                    break;
                default:
                    break;
            }
        }

        private MovementDirection SwapDirection(MovementDirection movementDirection)
        {
            // TODO: check these directions
            switch (movementDirection)
            {
                case MovementDirection.Left:
                    return MovementDirection.Up;
                case MovementDirection.Right:
                    return MovementDirection.Down;
                case MovementDirection.Up:
                    return MovementDirection.Left;
                case MovementDirection.Down:
                    return MovementDirection.Right;
                default:
                    return MovementDirection.Stationary;
            }
        }

        #endregion

    }
}
