using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Foundation;
using EPCalipersWinUI3.Contracts;

namespace EPCalipersWinUI3.Models.Calipers
{
	public class AngleCaliperLabel : CaliperLabel
	{
		private CaliperLabelPosition _position;
        private Size _size;
		private ICaliperView _view;

		new AngleCaliper Caliper { get; set; }

		public TextBlock TriangleTextBlock { get; set; }

		public AngleCaliperLabel(
			AngleCaliper caliper,
			ICaliperView caliperView,
			string text,
			CaliperLabelAlignment alignment,
			bool autoPosition,
			int fontSize,
			bool scaleFont,
			double scaleFactor, bool fakeUI = false)
			: base(caliper, text, alignment, autoPosition, fontSize, scaleFont, scaleFactor, fakeUI: fakeUI)
		{
			Caliper = caliper;
			_view = caliperView;
			if (!fakeUI)
			{
				TextBlock.Text = text;
				_size = ShapeMeasure(TextBlock);  // Estimate TextBlock size.

			}
			else
			{
				_size = new Size();
			}
			_position = new CaliperLabelPosition();
			UpdateScaledFontSize();
		}

		public override void SetPosition()
		{
			if (TextBlock == null) return;
			GetPosition();
			TextBlock.Margin = new Thickness(_position.Left, _position.Top, 0, 0);
		}

		private void GetPosition()
		{
			if (TextBlock == null) return;
			_size = ShapeMeasure(TextBlock);
			_size.Width = TextBlock.ActualWidth;
			_size.Height = TextBlock.ActualHeight;
			// Angle caliper labels are always at the top,
			// but they need to adjust to avoid hitting the view bounds.
			var left = (int)(Caliper.ApexBar.MidPoint.X - _size.Width / 2);
			left = Math.Max(left, 10);
			var right = (int)(left + _size.Width);
			if (right > _view.Bounds.Width - 10)
			{
				left =  left - (right - ((int)_view.Bounds.Width - 10));
			}
			var top = (int)(Caliper.ApexBar.Position - _size.Height - _padding);
			top = Math.Max(top, 10);
			_position.Left = left;
			_position.Top = top;
		}
	}
}
