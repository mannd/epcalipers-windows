using EPCalipersWinUI3.Contracts;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
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
			_position = new CaliperLabelPosition();
			SetPosition(true);
		}

		public override void SetPosition(bool initialPosition = false)
		{
			GetPosition(initialPosition);
			TextBlock.Margin = new Thickness(_position.Left, _position.Top, 0, 0);
		}

		private void GetPosition(bool initialPosition = false)
		{
			Size size = new Size();
			// First positioning of label needs to estimate size of label.
			if (initialPosition)
			{
				size = ShapeMeasure(TextBlock);
			} else
			{
				size.Width = TextBlock.ActualWidth;
				size.Height = TextBlock.ActualHeight;
			}
			switch (Alignment)
			{
				case CaliperLabelAlignment.Top:
					_position.Left = (int)(Caliper.CrossBar.MidPoint.X - size.Width / 2);
					_position.Top = (int)(Caliper.CrossBar.Position - size.Height - padding);
					break;
				case CaliperLabelAlignment.Bottom:
					_position.Left = (int)(Caliper.CrossBar.MidPoint.X - size.Width / 2);
					_position.Top = (int)(Caliper.CrossBar.Position + padding);
					break;
				default:
					break;
			}
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
