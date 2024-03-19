using EPCalipersWinUI3.Contracts;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using Windows.Foundation;

namespace EPCalipersWinUI3.Models.Calipers
{
	public class TriangleBaseLabel : CaliperLabel
	{
		new AngleCaliper Caliper { get; set; }

		public Size Size
		{
			get => _size;
			set => _size = value;  // Set only used for testing.
		}
		private Size _size;

		public Visibility Visibility
		{
			get
			{
				return _visibility;
			}
			set
			{
				_visibility = value;
				if (TextBlock != null)
				{
					TextBlock.Visibility = value;
				}
			}
		}
		private Visibility _visibility = Visibility.Collapsed;

		private CaliperLabelPosition _position;
		private readonly ICaliperView _view;

		public TriangleBaseLabel(AngleCaliper caliper,
			ICaliperView caliperView,
			string text,
			CaliperLabelAlignment alignment,
			bool autoAlignLabel,
			CaliperLabelSize caliperLabelSize,
			int fontSize,
			bool fakeUI = false, Visibility visibility = Visibility.Collapsed)
			: base(caliper, text, alignment, autoAlignLabel, caliperLabelSize, fontSize, fakeUI)
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
			AutoAlignLabel = autoAlignLabel;
			_position = new CaliperLabelPosition();
			SetPosition();
			Visibility = visibility;
		}

		public override void SetPosition()
		{
			Debug.Print("TriangleBaseLabel.SetPosition()");
			if (TextBlock == null) return;
			var alignment = AutoAlign(Alignment, AutoAlignLabel);
			GetPosition(alignment);
			TextBlock.Margin = new Thickness(_position.Left, _position.Top, 0, 0);
			Debug.Print(TextBlock.Text);
			Debug.Print(TextBlock.Margin.ToString());
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
					_position.Left = (int)(Caliper.TriangleBaseBar.MidPoint.X - _size.Width / 2);
					_position.Top = (int)(Caliper.TriangleBaseBar.Y1 - _size.Height - _padding);
					break;
				case CaliperLabelAlignment.Bottom:
					_position.Left = (int)(Caliper.TriangleBaseBar.MidPoint.X - _size.Width / 2);
					_position.Top = (int)(Caliper.TriangleBaseBar.Y1 + _padding);
					break;
				case CaliperLabelAlignment.Left:
					_position.Left = (int)(Caliper.LeftMostBarPosition - _size.Width - _padding);
					_position.Top = (int)(Caliper.TriangleBaseBar.Y1 - _size.Height / 2);
					break;
				case CaliperLabelAlignment.Right:
					_position.Left = (int)(Caliper.RightMostBarPosition + _padding);
					_position.Top = (int)(Caliper.TriangleBaseBar.Y1 - _size.Height / 2);
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
						distance = (int)(Caliper.TriangleBaseBar.Y1 - _size.Height - _padding);
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
						distance = (int)(Caliper.TriangleBaseBar.Y1 + _size.Height + _padding);
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
