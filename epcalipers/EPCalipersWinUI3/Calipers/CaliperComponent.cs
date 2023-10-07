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

namespace EPCalipersWinUI3.Calipers
{
    public enum ComponentDirection
    {
        Horizontal,
        Vertical,
        Angle
    }

    /// <summary>
    /// One of the bars of a caliper.
    /// </summary>
    public class CaliperComponent
    {
		public enum Role
		{
			Horizontal,
			Vertical,
			HorizontalCrossBar,
			VerticalCrossBar,
			Angle,
			TriangleBase // For BrugadaMeter.
		}

        // TODO: Implement Position as get only, based on role of component.

        public Line Line { get; set; }
        public Role ComponentRole { get; set; }

        private readonly double _precision = 10;

		public CaliperComponent(Role role,
			double position, double start, double end)
        {
            ComponentRole = role;
            InitLine(start, end, position);
        }

        private void InitLine(double start, double end, double position)
        {
            if (Line == null)
            {
                Line = new Line();
            }
            switch (ComponentRole)
            {
                case Role.Vertical:
                    Line.X1 = position;
                    Line.Y1 = 0;
                    Line.X2 = position;
                    Line.Y2 = end;
                    break;
                case Role.HorizontalCrossBar:
                    Line.X1 = start;
                    Line.Y1 = position;
                    Line.X2 = end;
                    Line.Y2 = position;
                    break;
                default:
                    break;
            }
        }

		public SolidColorBrush Brush
        {
            set
            {
                Line.Stroke = value;
            }
        }

        public double Thickness
        {
            set
            {
                Line.StrokeThickness = value;
            }
        }

        public bool IsSelected { get; set; }



        public bool IsNear(Point p)
        {
            if (Line == null)
            {
                return false;
            }
            switch (ComponentRole)
            {
                case Role.Vertical:
					return p.X > Line.X1 - _precision && p.X < Line.X1 + _precision;
                    case Role.HorizontalCrossBar:
                    return p.X > Math.Min(Line.X1, Line.X2)
                            && p.X < Math.Max(Line.X1, Line.X2)
                            && p.Y > Line.Y1 - _precision
                            && p.Y < Line.Y1 + _precision;
                default: return false;
            }
        }


    }
}
