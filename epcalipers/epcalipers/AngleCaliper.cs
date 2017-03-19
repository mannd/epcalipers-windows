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
            // TODO: handle handles!
            //if (hasHandles)
            //{
            //    DrawHorizontalHandles(g, brush);
            //}

            CaliperText(g, brush);
            pen.Dispose();
            brush.Dispose();
        }

        //private void CaliperText(Graphics g, Brush brush)
        //{

        //}

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

        public override double IntervalResult()
        {
            return angleBar1 - angleBar2;
        }

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


    }
}
