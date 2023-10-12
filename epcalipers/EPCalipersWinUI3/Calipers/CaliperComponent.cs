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
    public sealed class CaliperComponent : ILine
    {
        /// <summary>
        /// The direction and function of the CaliperComponent in a Caliper.
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
        public ILine ComponentLine { get;  set; }
        public Role ComponentRole { get; set; }
		public double Position
        {
            get
            {
				switch (ComponentRole)
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
		public double X1 { get => ComponentLine.X1; set => ComponentLine.X1 = value; }
        public double X2 { get => ComponentLine.X2; set => ComponentLine.X2 = value; }
        public double Y1 { get => ComponentLine.Y1; set => ComponentLine.Y1 = value; }
        public double Y2 { get => ComponentLine.Y2; set => ComponentLine.Y2 = value; }

        public bool IsGrabbed { get; set; } = false;

        private readonly double _precision = 10; // Used to determine if touches are nearby.

        // TODO: Should (position, start, end) be a struct?
		public CaliperComponent(Role role,
			double position, double start, double end, ILine componentLine = null)
        {
            ComponentRole = role;
            ComponentLine = componentLine ?? new ComponentLine();
            SetupComponentLine(start, end, position);
        }

        private void SetupComponentLine(double start, double end, double position)
        {
            switch (ComponentRole)
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
            set => ComponentLine.Color = value; }
		public double Width
        {
            get => ComponentLine.Width;
            set => ComponentLine.Width = value;
        }

		public bool IsNear(Point p)
        {
            switch (ComponentRole)
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

        public Line GetComponent() => ComponentLine.GetComponent();
    }
}
