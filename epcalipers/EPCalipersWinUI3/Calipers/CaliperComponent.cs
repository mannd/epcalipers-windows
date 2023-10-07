using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public double Position { get; set; }
        public Line Line { get; set; }
        public Role ComponentRole { get; set; }

		public CaliperComponent(Role role,
			double position, double start, double end)
        {
            ComponentRole = role;
            Position = position;
            InitLine(start, end);
        }

        private void InitLine(double start, double end)
        {
            if (Line == null)
            {
                Line = new Line();
            }
            switch (ComponentRole)
            {
                case Role.Vertical:
                    Line.X1 = Position;
                    Line.Y1 = 0;
                    Line.X2 = Position;
                    Line.Y2 = end;
                    break;
                case Role.HorizontalCrossBar:
                    Line.X1 = start;
                    Line.Y1 = Position;
                    Line.X2 = end;
                    Line.Y2 = Position;
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


    }
}
