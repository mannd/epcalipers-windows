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
		private Size _size;

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
			_size = ShapeMeasure(TextBlock);
			_position = new CaliperLabelPosition();
			SetPosition(true);
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
			_size.Width = TextBlock.ActualWidth;
			// Angle caliper labels are always at the top
			_position.Left = (int)(Caliper.ApexBar.MidPoint.X - _size.Width / 2);
			_position.Top = (int)(Caliper.ApexBar.Position - _size.Height - _padding);
			// TODO: deal with brugada triangle.
		}

	}
}
