using EPCalipersWinUI3.Contracts;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Devices;
using Windows.UI;
using EPCalipersWinUI3.Helpers;
using Color = Windows.UI.Color;
using Point = Windows.Foundation.Point;
using ABI.Microsoft.UI.Xaml;
using Microsoft.UI.Xaml;

namespace EPCalipersWinUI3.Calipers
{
    /// <summary>
    /// One of the bars of a caliper.
    /// </summary>
    public sealed class Bar
    {
        /// <summary>
        /// The direction and function of the Bar in a Caliper.
        /// </summary>
		public enum Role
		{
			Horizontal,
			Vertical,
			HorizontalCrossBar,
			VerticalCrossBar,
			LeftAngle,
            RightAngle,
            Apex,  // Invisible bar at apex of angle caliper
			TriangleBase // For BrugadaMeter.
		}
        /// <summary>
        /// This is a graphics object that is drawable, just a Line but can substitute
        /// an ILine stub for testing.
        /// </summary>
        public IBarLine BarLine { get;  set; }
        public Role BarRole { get; set; }

        public Bounds Bounds { get; set; }

        // Angle is only used for angle calipers.
        public double Angle { get; set; }
		public double Position
        {
            get
            {
				switch (BarRole)
				{
					case Role.Horizontal:
						return Y1;
					case Role.Vertical:
						return X1;
					case Role.HorizontalCrossBar:
						return Y1;
					case Role.VerticalCrossBar:
						return X1;
					default:
						return 0;
				}
			}
            set
            {
                switch (BarRole)
                {
					case Role.Horizontal:
                    case Role.HorizontalCrossBar:
						Y1 = value; Y2 = value;
                        break;
					case Role.Vertical:
                    case Role.VerticalCrossBar:
                        X1 = value; X2 = value;
                        break;
					default:
                        break;
                }
            }
		}
		public double X1 { get => BarLine.X1; set => BarLine.X1 = value; }
        public double X2 { get => BarLine.X2; set => BarLine.X2 = value; }
        public double Y1 { get => BarLine.Y1; set => BarLine.Y1 = value; }
        public double Y2 { get => BarLine.Y2; set => BarLine.Y2 = value; }

        private readonly double _precision = 10; // Used to determine if touches are nearby.

		public Bar(Role role,
			double position, double start, double end, bool fakeBarLine = false)
        {
            BarRole = role;
            BarLine = fakeBarLine ? new FakeBarLine() : new BarLine();
            SetBarPosition(position, start, end);
        }

        public Bar(Role role, Point apex, double angle, Bounds bounds, bool fakeBarLine = false)
        {
            // TODO: exception if role is not an Angle role.
            BarRole = role;
            BarLine = fakeBarLine ? new FakeBarLine() : new BarLine();
            Bounds = bounds;
            if (role == Role.Apex)
            {
				SetBarPosition(apex.Y, apex.X - 10, apex.X + 10);
                // Comment out this line to show hidden ApexBar for debugging.
                BarLine.Line.Visibility = Visibility.Collapsed; 
            }
            else
            {
                SetAngleBarPosition(apex, angle);
            }
        }

        private void SetBarPosition(double position, double start, double end)
		{
            switch (BarRole)
            {
                case Role.Horizontal:
                    X1 = 0;
                    Y1 = position;
                    X2 = end;
                    Y2 = position;
                    break;
                case Role.Vertical:
                    X1 = position;
                    Y1 = 0;
                    X2 = position;
                    Y2 = end;
                    break;
                case Role.HorizontalCrossBar:
                    X1 = start;
                    Y1 = position;
                    X2 = end;
                    Y2 = position;
                    break;
                case Role.VerticalCrossBar:
                    X1 = position;
                    Y1 = start;
                    X2 = position;
                    Y2 = end;
                    break;
                case Role.Apex:
                    X1 = start;
                    Y1 = position;
                    X2 = end;
                    Y2 = position;
                    break;
                case Role.LeftAngle:
                case Role.RightAngle:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public void SetAngleBarPosition(Point apex, double angle)
        {
            var adjustedEndPoint = ClippedEndPoint(apex, angle, 1000, new Point(0, Bounds.Height), new Point(Bounds.Width, Bounds.Height));
			X1 = apex.X;
			Y1 = apex.Y;
			X2 = adjustedEndPoint.X;
			Y2 = adjustedEndPoint.Y;
		}

		private Point? AdjustEndPoint(Point apex, Point endPoint, Point border1, Point border2)
        {
            var intersection = MathHelper.Intersection(apex, endPoint, border1, border2);
            return intersection;
        }

        private Point ClippedEndPoint(Point apex, double angle, double length, Point lowerBorder, Point rightBorder)
        {
			var endPoint = EndPointForPosition(apex, angle, length);
			var adjustedEndPoint = AdjustEndPoint(apex, endPoint, lowerBorder, rightBorder) ?? endPoint;
			adjustedEndPoint = AdjustEndPoint(apex, adjustedEndPoint, new Point(0, 0), lowerBorder) ?? adjustedEndPoint;
			adjustedEndPoint = AdjustEndPoint(apex, adjustedEndPoint, new Point(0, 0), new Point(rightBorder.X, 0)) ?? adjustedEndPoint;
			adjustedEndPoint = AdjustEndPoint(apex, adjustedEndPoint, new Point(rightBorder.X, 0), rightBorder) ?? adjustedEndPoint;
            return adjustedEndPoint;
        }

        public Point MidPoint => new Point(((X2 - X1) / 2.0) + X1, ((Y2 - Y1) / 2.0) + Y1);

        public bool IsSelected { get; set; }
		public Color Color {
            set => BarLine.Color = value; }
		public double Width
        {
            get => BarLine.Width;
            set => BarLine.Width = value;
        }

		public bool IsNear(Point p)
        {
            switch (BarRole)
            {
                case Role.Horizontal:
					return p.Y > Y1 - _precision && p.Y < Y1 + _precision;
                case Role.Vertical:
					return p.X > X1 - _precision && p.X < X1 + _precision;
				case Role.HorizontalCrossBar:
                case Role.Apex:
					return p.X > Math.Min(X1, X2)
                            && p.X < Math.Max(X1, X2)
                            && p.Y > Y1 - _precision
                            && p.Y < Y1 + _precision;
				case Role.VerticalCrossBar:
					return p.Y > Math.Min(Y1, Y2)
                            && p.Y < Math.Max(Y1, Y2)
                            && p.X > X1 - _precision
                            && p.X < X1 + _precision;
                default: return false;
            }
        }

		private Point EndPointForPosition(Point p, double angle, double length)
		{
			// Note Windows coordinates origin is top left of screen
			double endX = Math.Cos(angle) * length + p.X;
			double endY = Math.Sin(angle) * length + p.Y;
			Point endPoint = new Point(endX, endY);
			return endPoint;
		}

		public Line Line() => BarLine.Line;
    }
}
