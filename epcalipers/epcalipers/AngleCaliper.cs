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
        public Calibration VerticalCalibration { get; set; }
       
        const double angleDelta = 0.15;

        public AngleCaliper() : base()
        {
            caliperIsAngleCaliper = true;
            caliperRequiresCalibration = false;
        }

        public override void SetInitialPositionInRect(RectangleF rect)
        {
            base.SetInitialPositionInRect(rect);
            Bar2Position = Bar1Position;
        }

        public override void Draw(Graphics g, RectangleF rect)
        {
            Brush brush = new SolidBrush(CaliperColor);
            Pen pen = new Pen(brush, LineWidth);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // ensure caliper always extends past the screen edges
            float length = Math.Max(rect.Size.Height, rect.Size.Width) * 2.0f;

            CrossbarPosition = Math.Min(CrossbarPosition, rect.Size.Height - DELTA);
            CrossbarPosition = Math.Max(CrossbarPosition, DELTA);
            Bar1Position = Math.Min(Bar1Position, rect.Size.Width - DELTA);
            Bar2Position = Bar1Position;

            PointF endPointBar1 = EndPointForPosition(new PointF(Bar1Position, CrossbarPosition),
                angleBar1, length);
            g.DrawLine(pen, Bar1Position, CrossbarPosition, endPointBar1.X, endPointBar1.Y);

            PointF endPointBar2 = EndPointForPosition(new PointF(Bar2Position, CrossbarPosition),
                angleBar2, length);
            g.DrawLine(pen, Bar2Position, CrossbarPosition, endPointBar2.X, endPointBar2.Y);

            CaliperText(g, brush);

            if (VerticalCalibration.Calibrated && VerticalCalibration.UnitsAreMM)
            {
                // show Brugada triangle
                if (angleInSouthernHemisphere(angleBar1) && angleInSouthernHemisphere(angleBar2))
                {
                    double pointsPerMM = 1.0 / VerticalCalibration.Multiplier;
                    DrawTriangleBase(g, pen, brush, 5 * pointsPerMM);
                }
            }

            pen.Dispose();
            brush.Dispose();
        }

        private PointF EndPointForPosition(PointF p, float angle, float length)
        {
            // Note Windows coordinates origin is top left of screen
            double endX = Math.Cos(angle) * length + p.X;
            double endY = Math.Sin(angle) * length + p.Y;
            PointF endPoint = new PointF((float)endX, (float)endY);
            return endPoint;
        }

        public bool PointNearBar(PointF p, float barAngle)
        {
            double theta = relativeTheta(p);
            return theta < (double)barAngle + angleDelta &&
                theta > (double)barAngle - angleDelta;
        }

        private double relativeTheta(PointF p)
        {
            float x = p.X - Bar1Position;
            float y = p.Y - CrossbarPosition;
            return Math.Atan2((double)y, (double)x);
        }

        public override bool PointNearBar1(PointF p)
        {
            return PointNearBar(p, angleBar1);
        }

        public override bool PointNearBar2(PointF p)
        {
            return PointNearBar(p, angleBar2);
        }

        public override bool PointNearCrossbar(PointF p)
        {
            float delta = 40.0f;
            return (p.X > Bar1Position - delta && p.X < Bar1Position + delta &&
                p.Y > CrossbarPosition - delta && p.Y < CrossbarPosition + delta);
        }

        protected override string Measurement()
        {
            float angle = angleBar1 - angleBar2;
            double degrees = RadiansToDegrees(angle);
            string text = string.Format("{0:0.#}°", degrees);
            return text;
        }

        private double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        private double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180.0;
        }

        public override double IntervalResult()
        {
            return angleBar1 - angleBar2;
        }

        #region Brugada
        private bool angleInSouthernHemisphere(float angle)
        {
            // Note can't be <= because we get divide by zero error with Sin(angle) == 0
            return (0 < (double)angle && angle < Math.PI);
        }

        private void DrawTriangleBase(Graphics g, Pen pen, Brush brush, double height)
        {
            PointF point1 = GetBasePoint1ForHeight(height);
            PointF point2 = GetBasePoint2ForHeight(height);
            double lengthInPoints = point2.X - point1.X;
            g.DrawLine(pen, point1.X, point1.Y, point2.X, point2.Y);

            string text = BaseMeasurement(lengthInPoints);
            // we put the label below the base
            RectangleF rect = new RectangleF((point2.X > point1.X ? point1.X - 25 : point2.X - 20),
                point1.Y + 5, 
               (float)Math.Max(120.0, Math.Abs(point2.X - point1.X) + 50), 
               20.0f);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            g.DrawString(text, TextFont, brush, rect, format);
        }

        private PointF GetBasePoint1ForHeight(double height)
        {
            // Dangerous possible divide by zero here
            double pointY = CrossbarPosition + height;
            double pointX = height * (Math.Sin(angleBar1 - Math.PI / 2) 
                / Math.Sin(Math.PI - angleBar1));
            pointX = Bar1Position - pointX;
            PointF point = new PointF((float)pointX, (float)pointY);
            return point;
        }

        private PointF GetBasePoint2ForHeight(double height)
        {
            // Dangerous possible divide by zero here
            double pointY = CrossbarPosition + height;
            double pointX = height * (Math.Sin(Math.PI / 2 - angleBar2)
                / Math.Sin(angleBar2));
            pointX += Bar1Position;
            PointF point = new PointF((float)pointX, (float)pointY);
            return point;
        }

        private double CalibratedBaseResult(double lengthInPoints)
        {
            lengthInPoints = lengthInPoints * CurrentCalibration.Multiplier;
            if (RoundMsecRate && CurrentCalibration.UnitsAreMsecs)
            {
                lengthInPoints = Math.Round(lengthInPoints);
            }
            return lengthInPoints;
        }

        private string BaseMeasurement(double lengthInPoints)
        {
            string s = string.Format("{0} {1}", 
                CalibratedBaseResult(lengthInPoints).ToString("G4"), CurrentCalibration.RawUnits);
            return s;
        }

        #endregion

        #region Movement

        private double MoveBarAngle(PointF delta, PointF location)
        {
            PointF newPosition = new PointF(location.X + delta.X, location.Y + delta.Y);
            return relativeTheta(newPosition);
        }

        public override void MoveBar1(PointF delta, PointF location)
        {
            angleBar1 = (float)MoveBarAngle(delta, location);
        }

        public override void MoveBar2(PointF delta, PointF location)
        {
            angleBar2 = (float)MoveBarAngle(delta, location);
        }

        #endregion

        #region Tweak

        public override void MoveBarInDirection(MovementDirection movementDirection, float distance, CaliperComponent component)
        {
            CaliperComponent adjustedComponent = MoveCrossbarInsteadOfSideBar(movementDirection, component) ? CaliperComponent.Apex : component;
            if (adjustedComponent == CaliperComponent.Apex)
            {
                base.MoveBarInDirection(movementDirection, distance, CaliperComponent.CrossBar);
            }
            // TODO: test this
            // We use smaller increments for angle calipers, otherwise movement is too large.
            distance = distance / 2.0f;
            if (movementDirection == MovementDirection.Left)
            {
                distance = -distance;
            }
            switch (adjustedComponent)
            {
                case CaliperComponent.LeftBar:
                    angleBar1 -= (float)(DegreesToRadians(distance));
                    break;
                case CaliperComponent.RightBar:
                    angleBar2 -= (float)(DegreesToRadians(distance));
                    break;
                default:
                    break;
            }
        }

        #endregion


    }
}
