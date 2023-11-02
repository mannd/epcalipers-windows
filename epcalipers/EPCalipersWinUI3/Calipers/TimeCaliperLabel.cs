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
			_size = ShapeMeasure(TextBlock);  // Estimate TextBlock size.
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
			_size.Width = TextBlock.ActualWidth; // Only width changes as TextBlock.Text changes.
			switch (Alignment)
			{
				case CaliperLabelAlignment.Top:
					_position.Left = (int)(Caliper.CrossBar.MidPoint.X - _size.Width / 2);
					_position.Top = (int)(Caliper.CrossBar.Position - _size.Height - _padding);
					break;
				case CaliperLabelAlignment.Bottom:
					_position.Left = (int)(Caliper.CrossBar.MidPoint.X - _size.Width / 2);
					_position.Top = (int)(Caliper.CrossBar.Position + _padding);
					break;
				case CaliperLabelAlignment.Left:
					_position.Left = (int)(Caliper.LeftMostBarPosition - _size.Width - _padding);
					_position.Top = (int)(Caliper.CrossBar.Position - _size.Height / 2);
					break;
				case CaliperLabelAlignment.Right:
					_position.Left = (int)(Caliper.RightMostBarPosition + _padding);
					_position.Top = (int)(Caliper.CrossBar.Position - _size.Height / 2);
					break;
			}
		}
	}
}
