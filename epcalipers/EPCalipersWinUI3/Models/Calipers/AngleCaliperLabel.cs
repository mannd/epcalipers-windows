using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace EPCalipersWinUI3.Models.Calipers
{
	public class AngleCaliperLabel : CaliperLabel
	{
		private CaliperLabelPosition _position;
        private Size _size;

		new AngleCaliper Caliper { get; set; }

		public TextBlock TriangleTextBlock { get; set; }

		public AngleCaliperLabel(
			AngleCaliper caliper,
			string text,
			CaliperLabelAlignment alignment,
			bool autoPosition,
			int fontSize,
			bool fakeUI = false)
			: base(caliper, text, alignment, autoPosition, fontSize, fakeUI)
		{
			Caliper = caliper;
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
			SetPosition();
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
			// Angle caliper labels are always at the top
			_position.Left = (int)(Caliper.ApexBar.MidPoint.X - _size.Width / 2);
			_position.Top = (int)(Caliper.ApexBar.Position - _size.Height - _padding);
		}
	}
}
