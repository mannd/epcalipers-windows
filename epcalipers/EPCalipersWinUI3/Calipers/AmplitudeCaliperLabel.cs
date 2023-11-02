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
	public class AmplitudeCaliperLabel: CaliperLabel
	{
		private CaliperLabelPosition _position;
		private Size _size;
		new AmplitudeCaliper Caliper { get; set; }

		public AmplitudeCaliperLabel(AmplitudeCaliper caliper, 
			ICaliperView caliperView, 
			string text, 
			CaliperLabelAlignment alignment, 
			bool autoPosition, 
			bool fakeUI = false) : base(caliper, caliperView, text, alignment, autoPosition, fakeUI)
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
			switch (Alignment)
			{
				case CaliperLabelAlignment.Top:
					_position.Left = (int)(Caliper.CrossBar.Position - _size.Width / 2);
					_position.Top = (int)(Caliper.TopMostBarPosition - _size.Height - _padding);
					break;
				case CaliperLabelAlignment.Bottom:
					_position.Left = (int)(Caliper.CrossBar.Position - _size.Width / 2);
					_position.Top = (int)(Caliper.BottomMostBarPosition + _padding);
					break;
				case CaliperLabelAlignment.Left:
					_position.Left = (int)(Caliper.CrossBar.Position - _size.Width - _padding);
					_position.Top = (int)(Caliper.CrossBar.MidPoint.Y - _size.Height / 2);
					break;
				case CaliperLabelAlignment.Right:
					_position.Left = (int)(Caliper.CrossBar.Position + _padding);
					_position.Top = (int)(Caliper.CrossBar.MidPoint.Y - _size.Height / 2);
					break;
			}
		}
	}
}
