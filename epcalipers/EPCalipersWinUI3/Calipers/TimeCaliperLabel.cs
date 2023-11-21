using EPCalipersWinUI3.Contracts;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Foundation;

namespace EPCalipersWinUI3.Calipers
{
	public class TimeCaliperLabel : CaliperLabel
	{
		private CaliperLabelPosition _position;
		private ICaliperView _view;

		new TimeCaliper Caliper { get; set; }

		public Size Size
		{
			get => _size;
			set => _size = value;  // Set only used for testing.
		}
		private Size _size;

		public TimeCaliperLabel(TimeCaliper caliper, 
			ICaliperView caliperView, 
			string text, 
			CaliperLabelAlignment alignment, 
			bool autoAlignLabel, 
			bool fakeUI = false) : base(caliper, caliperView, text, alignment, autoAlignLabel, fakeUI)
		{
			Caliper = caliper;
			_view = caliperView;
			if (!fakeUI)
			{
				_size = ShapeMeasure(TextBlock);  // Estimate TextBlock size.
			}
			else
			{
				_size = new Size();
			}
			AutoAlignLabel = autoAlignLabel;
			_position = new CaliperLabelPosition();
			SetPosition();
		}

		public override void SetPosition()
		{
			if (TextBlock == null) return;
			var alignment = AutoAlign(Alignment, AutoAlignLabel);
			GetPosition(alignment);
			TextBlock.Margin = new Thickness(_position.Left, _position.Top, 0, 0);
		}

		private void GetPosition(CaliperLabelAlignment alignment)
		{
			if (TextBlock == null) return;
			_size.Width = TextBlock.ActualWidth; // Only width changes as TextBlock.Text changes.
			switch (alignment)
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

		public CaliperLabelAlignment AutoAlign(CaliperLabelAlignment alignment, bool autoAlign)
		{
			if (!autoAlign)  { return alignment; }
			if (TextBlock != null)
			{
				_size.Width = TextBlock.ActualWidth;
				_size.Height = TextBlock.ActualHeight;
			}
			int distance;
			switch (alignment)
			{
				case CaliperLabelAlignment.Left:
					distance = (int)(Caliper.LeftMostBarPosition - _size.Width - _padding);
					if (distance < 0)
					{
						return CaliperLabelAlignment.Right;
					}
					break;
				case CaliperLabelAlignment.Right:
					distance = (int)(Caliper.RightMostBarPosition + _size.Width + _padding);
					if (distance > _view.Bounds.Width)
					{
						return CaliperLabelAlignment.Left;
					}
					break;
				case CaliperLabelAlignment.Top:
					distance = (int)(Math.Abs(Caliper.Value) - _size.Width - _padding);
					if (distance > 0)
					{
						distance = (int)(Caliper.CrossBar.Position - _size.Height - _padding);
						if (distance < 0)
						{
							return CaliperLabelAlignment.Bottom;
						}
					}
					else
					{
						return AutoAlign(CaliperLabelAlignment.Left, autoAlign);
					}
					break;
				case CaliperLabelAlignment.Bottom:
					distance = (int)(Math.Abs(Caliper.Value) - _size.Width - _padding);
					if (distance > 0)
					{
						distance = (int)(Caliper.CrossBar.Position + _size.Height + _padding);
						if (distance > _view.Bounds.Height)
						{
							return CaliperLabelAlignment.Top;
						}
					}
					else
					{
						return AutoAlign(CaliperLabelAlignment.Left, autoAlign);
					}
					break;
				default:
					return alignment;
			}
			return alignment;
		}
	}
}
