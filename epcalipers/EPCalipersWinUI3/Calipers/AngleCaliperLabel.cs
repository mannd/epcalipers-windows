using EPCalipersWinUI3.Contracts;
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
	public class AngleCaliperLabel : CaliperLabel
	{
		private CaliperLabelPosition _position;

		new AngleCaliper Caliper { get; set; }

		public AngleCaliperLabel(
			AngleCaliper caliper, 
			ICaliperView caliperView, 
			string text, 
			CaliperLabelAlignment alignment, 
			bool autoPosition, 
			bool fakeUI = false) 
			: base(caliper, caliperView, text, alignment, autoPosition, fakeUI)
		{
			Caliper = caliper;
			_position = new CaliperLabelPosition();
			SetPosition(true);
		}

		public override void SetPosition(bool initialPosition = false)
		{
			if (TextBlock == null) return;
			GetPosition(initialPosition);
			TextBlock.Margin = new Thickness(_position.Left, _position.Top, 0, 0);
		}

		private void GetPosition(bool initialPosition = false)
		{
			if (TextBlock == null) return;
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
			// Angle caliper labels are always at the top
			_position.Left = (int)(Caliper.ApexBar.MidPoint.X - size.Width / 2);
			_position.Top = (int)(Caliper.ApexBar.Position - size.Height - _padding);
			// TODO: deal with brugada triangle.
		}

		public static Size ShapeMeasure(TextBlock textBlock)
		{
			if (textBlock == null) return new Size(0, 0);
			// Measured Size is bounded to be less than maxSize
			Size maxSize = new Size(
				 double.PositiveInfinity,
				 double.PositiveInfinity);
			textBlock.Measure(maxSize);
			return textBlock.DesiredSize;
		}

	}
}
