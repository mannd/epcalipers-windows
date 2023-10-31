﻿using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Graphics.DirectX;
using EPCalipersWinUI3.Contracts;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace EPCalipersWinUI3.Calipers
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
	public abstract class CaliperLabel
    {
		private string _text;
		public Caliper Caliper { get; set; }
		public ICaliperView CaliperView { get; set; }
		public string Text { get
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

			}
		}
		protected CaliperLabelPosition Position { get; set; }
		public CaliperLabelAlignment Alignment { get; set; }
		public bool AutoPosition { get; set;}
        public TextBlock TextBlock { get; set; }

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

		public CaliperLabel(
			Caliper caliper,
			ICaliperView caliperView,
			string text,
			CaliperLabelAlignment alignment,
			bool autoPosition,
			bool fakeUI = false)
		{
			Caliper = caliper;
			Text = text;
			Alignment = alignment;
			AutoPosition = autoPosition;
			if (fakeUI)
			{
				TextBlock = null;
			}
			else
			{
				TextBlock = new TextBlock()
				{
					Text = text,
				};
			}
		}

		public abstract void SetPosition(bool initialPosition = false);
	}
}
