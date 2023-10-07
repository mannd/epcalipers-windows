using EPCalipersWinUI3.Contracts;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Calipers
{
	public class ComponentLine : ILine
	{
		private Line _line = new();
		public double X1 { get => _line.X1; set => _line.X1 = value; }
		public double X2 { get => _line.X2; set => _line.X2 = value; }
		public double Y1 { get => _line.Y1; set => _line.Y1 = value; }
		public double Y2 { get => _line.Y2; set => _line.Y2 = value; }
		public Windows.UI.Color Color
		{
			set
			{
				var brush = new SolidColorBrush(value);
				_line.Stroke = brush;
			}
		} 
		public double Width
		{
			get => _line.StrokeThickness;
			set => _line.StrokeThickness = value;
		}

		public Line GetComponent() => _line;
	}

	public class FakeComponentLine : ILine
	{
		public double X1 { get; set; }
		public double X2 { get; set; }
		public double Y1 { get; set; }
		public double Y2 { get; set; }
		public Windows.UI.Color Color { set { } }
		public double Width { get; set; }
		public Line GetComponent() => null;
	}
}
