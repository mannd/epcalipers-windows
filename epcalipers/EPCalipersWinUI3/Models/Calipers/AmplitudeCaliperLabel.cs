using EPCalipersWinUI3.Contracts;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Foundation;

namespace EPCalipersWinUI3.Models.Calipers
{
	public class AmplitudeCaliperLabel : CaliperLabel
	{
		private CaliperLabelPosition _position;
		private readonly ICaliperView _view;
		private readonly Bounds _bounds;

		public Size Size
		{
			get => _size;
			set => _size = value;  // Set only used for testing.
		}
		private Size _size;

		new AmplitudeCaliper Caliper { get; set; }

		public AmplitudeCaliperLabel(AmplitudeCaliper caliper,
			ICaliperView caliperView,
			string text,
			CaliperLabelAlignment alignment,
			CaliperLabelSize caliperLabelSize,
			bool autoAlignLabel,
			bool fakeUI = false) : base(caliper, text, alignment, autoAlignLabel, caliperLabelSize, fakeUI)
		{
			Caliper = caliper;
			_view = caliperView;
			_bounds = _view.Bounds;
			if (!fakeUI)
			{
				TextBlock.Text = text;
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
			_size = ShapeMeasure(TextBlock);
			_size.Width = TextBlock.ActualWidth;
			_size.Height = TextBlock.ActualHeight;
			switch (alignment)
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

		public CaliperLabelAlignment AutoAlign(CaliperLabelAlignment alignment, bool autoAlign)
		{
			if (!autoAlign) { return alignment; }
			if (TextBlock != null)
			{
				_size.Width = TextBlock.ActualWidth;
				_size.Height = TextBlock.ActualHeight;
			}
			int distance;
			switch (alignment)
			{
				case CaliperLabelAlignment.Top:
					distance = (int)(Caliper.TopMostBarPosition - _size.Height - _padding);
					if (distance < 0)
					{
						return CaliperLabelAlignment.Bottom;
					}
					break;
				case CaliperLabelAlignment.Bottom:
					distance = (int)(Caliper.BottomMostBarPosition + _size.Height + _padding);
					if (distance > _bounds.Height)
					{
						return CaliperLabelAlignment.Top;
					}
					break;
				case CaliperLabelAlignment.Left:
					distance = (int)(Math.Abs(Caliper.Value) - _size.Height - _padding);
					if (distance > 0)
					{
						distance = (int)(Caliper.CrossBar.Position - _size.Width - _padding);
						if (distance < 0)
						{
							return CaliperLabelAlignment.Right;
						}
					}
					else
					{
						return AutoAlign(CaliperLabelAlignment.Top, autoAlign);
					}
					break;
				case CaliperLabelAlignment.Right:
					distance = (int)(Math.Abs(Caliper.Value) - _size.Height - _padding);
					if (distance > 0)
					{
						distance = (int)(Caliper.CrossBar.Position + _size.Width + _padding);
						if (distance > _bounds.Height)
						{
							return CaliperLabelAlignment.Left;
						}
					}
					else
					{
						return AutoAlign(CaliperLabelAlignment.Top, autoAlign);
					}
					break;
				default:
					return alignment;
			}
			return alignment;
		}
	}
}
