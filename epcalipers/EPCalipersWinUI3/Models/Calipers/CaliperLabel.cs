using EPCalipersWinUI3.Contracts;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;

namespace EPCalipersWinUI3.Models.Calipers
{
	public enum CaliperLabelAlignment
	{
		Top,
		Bottom,
		Left,
		Right,
	}

	public struct CaliperLabelPosition
	{
		public int Left { get; set; }
		public int Top { get; set; }

		public CaliperLabelPosition(int left, int top)
		{
			Left = left;
			Top = top;
		}

		public CaliperLabelPosition(double left, double top)
		{
			Left = (int)left;
			Top = (int)top;
		}
	}
	public abstract class CaliperLabel: INotifyPropertyChanged
	{
		public const int ExtraSmallFont = 12;
		public const int SmallFont = 16;
		public const int MediumFont = 24;
		public const int LargeFont = 32;
		public const int ExtraLargeFont = 46;

		protected readonly int _padding = 10;

		public CaliperLabel(
			Caliper caliper,
			string text,
			CaliperLabelAlignment alignment,
			bool autoPosition,
			int fontSize,
			bool scaleFont, 
			double scaleFactor, 
			bool fakeUI = false)
		{
			Caliper = caliper;
			Text = text;
			Alignment = alignment;
			AutoAlignLabel = autoPosition;
			TextBlock = fakeUI ? null : new TextBlock()
			{
				Text = text
			};
			FontSize = fontSize;
			ScaleFactor = scaleFactor;
			DoScaleFontSize = scaleFont;
		}

		public Caliper Caliper { get; set; }

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				if (TextBlock != null)
				{
					TextBlock.Text = Text;
				}
				OnPropertyChanged(nameof(Text));
			}
		}
		private string _text;
		protected CaliperLabelPosition Position { get; set; }
		public CaliperLabelAlignment Alignment { get; set; }
		public bool AutoAlignLabel { get; set; }
		public TextBlock TextBlock { get; set; }
		public double ScaleFactor {  get; set; }
		public bool DoScaleFontSize { get; set; }
		public int FontSize { get; set; }

		public int ScaledFontSize
		{
			set
			{
				if (_scaledFontSize != value)
				{
					_scaledFontSize = value;
					if (TextBlock != null)
					{
						TextBlock.FontSize = value;
					}
				}
			}

		}
		private int _scaledFontSize;

		public bool IsSelected
		{
			set
			{
				_isSelected = value;
				if (TextBlock != null)
				{
					Color = _isSelected ? SelectedColor : UnselectedColor;
				}
			}
		}
		private bool _isSelected;
		public Color SelectedColor { get; set; }
		public Color UnselectedColor { get; set; }

		public Color Color
		{
			get => _color;
			set
			{
				_color = value;
				if (TextBlock != null)
				{
					TextBlock.Foreground = new SolidColorBrush(_color);
				}
			}
		}
		private Color _color;

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void AddToView(ICaliperView view)
		{
			if (TextBlock != null) view.Add(TextBlock);
		}

		public void RemoveFromView(ICaliperView view)
		{
			if (TextBlock != null) view.Remove(TextBlock);
		}

		public abstract void SetPosition();

		public static Size ShapeMeasure(TextBlock textBlock)
		{
			if (textBlock == null) return new Size(0, 0);
			// Measured Size is bounded to be less than maxSize
			Size maxSize = new(
				 double.PositiveInfinity,
				 double.PositiveInfinity);
			textBlock.Measure(maxSize);
			return textBlock.DesiredSize;
		}

		public void UpdateScaledFontSize()
		{
			Debug.Print("Update font scale factor");
			if (DoScaleFontSize)
			{
				var adjustedSize = FontSize / ScaleFactor;
				adjustedSize = Math.Max(CaliperLabel.ExtraSmallFont, adjustedSize);
				adjustedSize = Math.Min(CaliperLabel.ExtraLargeFont, adjustedSize);
				int adjustedCaliperLabelSize = (int)adjustedSize;
				ScaledFontSize = adjustedCaliperLabelSize;
			}
			else
			{
				ScaledFontSize = FontSize;
			}
			SetPosition();
		}

		private int AdjustCaliperLabelSize(int caliperLabelSize, double zoomFactor)
		{
			var fontSize = caliperLabelSize;
			var adjustedSize = fontSize / zoomFactor;
			adjustedSize = Math.Max(CaliperLabel.ExtraSmallFont, adjustedSize);
			adjustedSize = Math.Min(CaliperLabel.ExtraLargeFont, adjustedSize);
			int adjustedCaliperLabelSize = (int)adjustedSize;
			return adjustedCaliperLabelSize;
		}
	}
}
