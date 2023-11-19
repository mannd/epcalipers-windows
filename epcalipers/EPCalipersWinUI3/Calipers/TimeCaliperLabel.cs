using EPCalipersWinUI3.Contracts;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace EPCalipersWinUI3.Calipers
{
	public class TimeCaliperLabel : CaliperLabel
	{
		private CaliperLabelPosition _position;
		private Size _size;
		private ICaliperView _view;

		new TimeCaliper Caliper { get; set; }

		public Size Size
		{
			get => _size;
			set => _size = value;
		}

		public TimeCaliperLabel(TimeCaliper caliper, 
			ICaliperView caliperView, 
			string text, 
			CaliperLabelAlignment alignment, 
			bool autoPosition, 
			bool fakeUI = false) : base(caliper, caliperView, text, alignment, autoPosition, fakeUI)
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
			_position = new CaliperLabelPosition();
			SetPosition();
		}

		public override void SetPosition()
		{
			if (TextBlock == null) return;
			var alignment = AutoAlign(Alignment, true);
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
					distance = (int)(Math.Abs(Caliper.Value) - _size.Width + _padding);
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
					distance = (int)(Math.Abs(Caliper.Value) - _size.Width + _padding);
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


		//private TextPosition OptimizedTextPosition(float left, double right, double center,
		//											double windowWidth, double windowHeight,
		//											TextPosition textPosition,
		//											double textWidth, double textHeight,
		//											bool optimizeTextPosition)
		//{
		//	if (!AutoPositionText || !optimizeTextPosition)
		//	{
		//		return textPosition;
		//	}
		//	const double offset = 4;
		//	var optimizedPosition = textPosition;
		//	if (Direction == CaliperDirection.Horizontal)
		//	{
		//		switch (optimizedPosition)
		//		{
		//			case TextPosition.CenterAbove:
		//			case TextPosition.CenterBelow:
		//				// Avoid squeezing label.
		//				if (textWidth + offset > right - left)
		//				{
		//					optimizedPosition = (textWidth + right + offset > windowWidth)
		//						? TextPosition.Left : TextPosition.Right;
		//				}
		//				break;
		//			case TextPosition.Left:
		//				if (textWidth + offset > left)
		//				{
		//					optimizedPosition = (textWidth + right + offset > windowWidth)
		//						? TextPosition.CenterAbove : TextPosition.Right;
		//				}
		//				break;
		//			case TextPosition.Right:
		//				if (textWidth + right + offset > windowWidth)
		//				{
		//					optimizedPosition = (textWidth + offset > left)
		//						? TextPosition.CenterAbove : TextPosition.Left;
		//				}
		//				break;
		//			default:
		//				optimizedPosition = textPosition;
		//				break;
		//		}
		//	}
		//	else if (Direction == CaliperDirection.Vertical)
		//	{
		//		// watch for squeeze
		//		if ((optimizedPosition == TextPosition.Left || optimizedPosition == TextPosition.Right)
		//			&& textHeight + offset > right - left)
		//		{
		//			optimizedPosition = (left - textHeight - offset < 0)
		//				? TextPosition.Bottom : TextPosition.Top;
		//		}
		//		else
		//		{
		//			switch (optimizedPosition)
		//			{
		//				case TextPosition.Left:
		//					if (textWidth + offset > center)
		//					{
		//						optimizedPosition = TextPosition.Right;
		//					}
		//					break;
		//				case TextPosition.Right:
		//					if (textWidth + center + offset > windowWidth)
		//					{
		//						optimizedPosition = TextPosition.Left;
		//					}
		//					break;
		//				case TextPosition.Top:
		//					if (left - textHeight - offset < 0)
		//					{
		//						optimizedPosition = (right + textHeight - offset < 0)
		//							? TextPosition.Right : TextPosition.Bottom;
		//					}
		//					break;
		//				case TextPosition.Bottom:
		//					if (right + textHeight + offset > windowHeight)
		//					{
		//						optimizedPosition = (left - textHeight - offset < 0)
		//							? TextPosition.Right : TextPosition.Top;
		//					}
		//					break;
		//				default:
		//					optimizedPosition = textPosition;
		//					break;
		//			}
		//		}
		//	}
		//	return optimizedPosition;
		//}
	}
}
