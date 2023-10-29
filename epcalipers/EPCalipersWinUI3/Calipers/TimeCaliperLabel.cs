using EPCalipersWinUI3.Contracts;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace EPCalipersWinUI3.Calipers
{
	public class TimeCaliperLabel : CaliperLabel
	{
		readonly int padding = 10;

		private CaliperLabelPosition _position;
		private Size _size;

		new TimeCaliper Caliper { get; set; }

		public TimeCaliperLabel(TimeCaliper caliper, 
			ICaliperView caliperView, 
			string text, 
			CaliperLabelAlignment alignment, 
			bool autoPosition, 
			bool fakeUI = false) : base(caliper, caliperView, text, alignment, autoPosition, fakeUI)
		{
			Caliper = caliper;
			TextBlock.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Colors.Blue);
			_position = new CaliperLabelPosition();
			_size = ShapeMeasure(TextBlock);
			SetPosition();
		}

		public void SetPosition()
		{
			GetPosition();
			TextBlock.Margin = new Thickness(_position.Left, _position.Top, 0, 0);
		}

		private void GetPosition()
		{
			_position.Left = (int)(Caliper.CrossBar.MidPoint.X - _size.Width / 2);
			_position.Top = (int)(Caliper.CrossBar.Position - _size.Height - padding);
		}

		public static Size ShapeMeasure(TextBlock tb)
		{
			// Measured Size is bounded to be less than maxSize
			Size maxSize = new Size(
				 double.PositiveInfinity,
				 double.PositiveInfinity);
			tb.Measure(maxSize);
			return tb.DesiredSize;
		}
	}
}
