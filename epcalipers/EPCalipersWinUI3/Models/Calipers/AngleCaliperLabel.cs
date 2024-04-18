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
			if (GetPosition()) {
				TextBlock.Margin = new Thickness(_position.Left, _position.Top, 0, 0);
				TextBlock.Visibility = Visibility.Visible;
			}
			else
			{
				TextBlock.Visibility = Visibility.Collapsed;
			}
		}

		private bool GetPosition()
		{
			if (TextBlock == null) return false;
			_size = ShapeMeasure(TextBlock);
			_size.Width = TextBlock.ActualWidth;
			_size.Height = TextBlock.ActualHeight;
			var left = (int)(Caliper.ApexBar.MidPoint.X - _size.Width / 2);
			var top = (int)(Caliper.ApexBar.Position - _size.Height - _padding);
			_position.Left = left;
			_position.Top = top;
			return IsInBounds(_position, _size, _view.Bounds);
		}
	}
}
