using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using EPCalipersCore.Properties;
using TextPosition = EPCalipersCore.Properties.Preferences.TextPosition;

namespace EPCalipersCore
{
    using MBrush = System.Windows.Media.Brush;
    using DBrush = System.Drawing.Brush;

    public class AngleCaliper : Caliper
    {
        Line bar1Line = new Line();
        Line bar2Line = new Line();
        Line crossbarLine = new Line();
        TextBlock textBlock = new TextBlock();
        TextBlock baseTextBlock = new TextBlock();

        // private static float differential = 0.0f;

        float angleBar1 = (float)(0.5 * Math.PI);
        float angleBar2 = (float)(0.25 * Math.PI);
        public Calibration VerticalCalibration { get; set; }
        TextPosition triangleBaseTextPosition = TextPosition.CenterAbove;

        override public TextPosition CaliperTextPosition {
            //get { return BaseCaliper.CaliperTextPosition; }
            set { triangleBaseTextPosition = value; }
}

       
        const double angleDelta = 0.15;

        public AngleCaliper() : base()
        {
            caliperIsAngleCaliper = true;
            caliperRequiresCalibration = false;
            triangleBaseTextPosition = CaliperTextPosition;
        }

        public override void SetInitialPosition()
        {
            base.SetInitialPosition();
            Bar2Position = Bar1Position;
        }

        public override void Draw(Graphics g, RectangleF rect)
        {
            DBrush brush = new SolidBrush(CaliperColor);
            var pen = new System.Drawing.Pen(brush, LineWidth);
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

            CaliperText(g, brush, rect, TextPosition.CenterAbove, false);

            if (VerticalCalibration.Calibrated && VerticalCalibration.UnitsAreMM)
            {
                // show Brugada triangle
                if (angleInSouthernHemisphere(angleBar1) && angleInSouthernHemisphere(angleBar2))
                {
                    double pointsPerMM = 1.0 / VerticalCalibration.Multiplier;
                    DrawTriangleBase(g, pen, brush, 5 * pointsPerMM, rect);
                }
            }

            pen.Dispose();
            brush.Dispose();
        }

        public override void Draw(Canvas canvas)
        {
            var brush = new SolidColorBrush(ConvertColor(CaliperColor));
            canvas.Children.Remove(bar1Line);
            canvas.Children.Remove(bar2Line);
            canvas.Children.Remove(crossbarLine);
            // ensure caliper always extends past the screen edges
            float length = (float)Math.Max(canvas.ActualHeight, canvas.ActualWidth) * 2.0f;
            CrossbarPosition = (float)Math.Min(CrossbarPosition, canvas.ActualHeight - DELTA);
            CrossbarPosition = Math.Max(CrossbarPosition, DELTA);
            Bar1Position = (float)Math.Min(Bar1Position, canvas.ActualWidth - DELTA);
            Bar2Position = Bar1Position;

            PointF endPointBar1 = EndPointForPosition(new PointF(Bar1Position, CrossbarPosition),
                angleBar1, length);
            MakeLine(ref bar1Line, Bar1Position, endPointBar1.X, CrossbarPosition, endPointBar1.Y);
            bar1Line.StrokeThickness = LineWidth;
            bar1Line.Stroke = brush;
            canvas.Children.Add(bar1Line);
            PointF endPointBar2 = EndPointForPosition(new PointF(Bar2Position, CrossbarPosition),
                angleBar2, length);
            MakeLine(ref bar2Line, Bar2Position, endPointBar2.X, CrossbarPosition, endPointBar2.Y);
            bar2Line.StrokeThickness = LineWidth;
            bar2Line.Stroke = brush;
            canvas.Children.Add(bar2Line);

            CaliperText(canvas, brush, TextPosition.CenterAbove, false);

            if (VerticalCalibration.Calibrated && VerticalCalibration.UnitsAreMM)
            {
                // show Brugada triangle
                if (angleInSouthernHemisphere(angleBar1) && angleInSouthernHemisphere(angleBar2))
                {
                    double pointsPerMM = 1.0 / VerticalCalibration.Multiplier;
                    DrawTriangleBase(canvas, brush, 5 * pointsPerMM);
                }
            }
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
        
        private void DrawTriangleBase(Canvas canvas, MBrush brush, double height)
        {
            canvas.Children.Remove(baseTextBlock);
            PointF point1 = GetBasePoint1ForHeight(height);
            PointF point2 = GetBasePoint2ForHeight(height);
            double lengthInPoints = point2.X - point1.X;
            MakeLine(ref crossbarLine, point1.X, point2.X, point1.Y, point2.Y);
            crossbarLine.StrokeThickness = LineWidth;
            crossbarLine.Stroke = brush;
            canvas.Children.Add(crossbarLine);

            string text = BaseMeasurement(lengthInPoints);
            // we put the label below the base
            RectangleF rect = new RectangleF((point2.X > point1.X ? point1.X - 25 : point2.X - 25),
                point1.Y + 5, 
               (float)Math.Max(120.0, Math.Abs(point2.X - point1.X) + 50), 
               20.0f);
            baseTextBlock.FontFamily = new System.Windows.Media.FontFamily("Helvetica");
            baseTextBlock.FontSize = defaultCanvasFontSize;
            baseTextBlock.Text = text;
            baseTextBlock.TextAlignment = System.Windows.TextAlignment.Center;
            baseTextBlock.Padding = new System.Windows.Thickness(3);
            baseTextBlock.Foreground = brush;
            Font font = new Font("Helvetica", defaultCanvasFontSize);
            Size size = System.Windows.Forms.TextRenderer.MeasureText(text, font);
            baseTextBlock.MinWidth = size.Width;
            baseTextBlock.MinHeight = size.Height;
            //baseTextBlock.Background = new SolidColorBrush(ConvertColor(System.Drawing.Color.Gray));

            RectangleF textRect = GetCaliperTextPosition(triangleBaseTextPosition, Math.Min(point1.X, point2.X),
                      Math.Max(point1.X, point2.X), point1.Y, size,
                      new RectangleF(0, 0, (float)canvas.ActualWidth, (float)canvas.ActualHeight),
                      true);

            Canvas.SetLeft(baseTextBlock, textRect.X);
            Canvas.SetTop(baseTextBlock, textRect.Y);
            canvas.Children.Add(baseTextBlock);
        }


        private void DrawTriangleBase(Graphics g, System.Drawing.Pen pen, DBrush brush, double height, RectangleF rect)
        {
            PointF point1 = GetBasePoint1ForHeight(height);
            PointF point2 = GetBasePoint2ForHeight(height);
            double lengthInPoints = point2.X - point1.X;
            g.DrawLine(pen, point1.X, point1.Y, point2.X, point2.Y);

            string text = BaseMeasurement(lengthInPoints);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            var size = g.MeasureString(text, TextFont);
            var textRect = GetCaliperTextPosition(triangleBaseTextPosition,
                        Math.Min(point1.X, point2.X), Math.Max(point1.X, point2.X),
                        point1.Y, size, rect, true);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawString(text, TextFont, brush, textRect, format);
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
            if (rounding == Preferences.Rounding.ToInt && CurrentCalibration.UnitsAreMsecs)
            {
                lengthInPoints = Math.Round(lengthInPoints);
            }
            return lengthInPoints;
        }

        private string BaseMeasurement(double lengthInPoints)
        {
            string s;
            string format;
            switch (rounding) {
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
                s = string.Format("{0} {1}", (int)(CalibratedBaseResult(lengthInPoints)),
                    CurrentCalibration.RawUnits);
            }
            else
            {
                s = string.Format("{0} {1}", CalibratedBaseResult(lengthInPoints).ToString(format), CurrentCalibration.RawUnits);
            }
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
