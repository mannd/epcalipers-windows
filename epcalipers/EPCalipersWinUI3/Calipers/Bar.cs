using EPCalipersWinUI3.Contracts;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Devices;
using Windows.UI;

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
			Angle,
			TriangleBase // For BrugadaMeter.
		}
        /// <summary>
        /// This is a graphics object that is drawable, just a Line but can substitute
        /// an ILine stub for testing.
        /// </summary>
        public IBarLine BarLine { get;  set; }
        public Role BarRole { get; set; }
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
					default:
						return 0;
				}
			}
		}
		public double X1 { get => BarLine.X1; set => BarLine.X1 = value; }
        public double X2 { get => BarLine.X2; set => BarLine.X2 = value; }
        public double Y1 { get => BarLine.Y1; set => BarLine.Y1 = value; }
        public double Y2 { get => BarLine.Y2; set => BarLine.Y2 = value; }

        private readonly double _precision = 10; // Used to determine if touches are nearby.

        // TODO: Should (position, start, end) be a struct or record?
		public Bar(Role role,
			double position, double start, double end, bool fakeComponentLine = false)
        {
            BarRole = role;
            BarLine = fakeComponentLine ? new FakeBarLine() : new BarLine();
            SetupBar(position, start, end);
        }

        private void SetupBar(double position, double start, double end)
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
                default:
                    break;
            }
        }
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

        public Line Line() => BarLine.Line;
    }
}
