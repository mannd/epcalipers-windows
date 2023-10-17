﻿using EPCalipersWinUI3.Contracts;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace EPCalipersWinUI3.Calipers
{
	public sealed class AmplitudeCaliper : Caliper
	{
        public Bar TopBar { get; set; }
        public Bar BottomBar { get; set; }
        public Bar CrossBar { get; set; }

		private bool _fakeBarLines;

		public AmplitudeCaliper(CaliperPosition position, ICaliperView caliperView,
			bool fakeBarLines = false) : base(caliperView)
		{
			_fakeBarLines = fakeBarLines;
			SetInitialPosition(position);
			Bars = new[] { TopBar, BottomBar, CrossBar };
			SetThickness(2);
			CaliperType = CaliperType.Amplitude;
		}

		private void SetInitialPosition(CaliperPosition position)
		{
			// NB Crossbar must be first to allow isNear to work properly.
			CrossBar = new Bar(Bar.Role.VerticalCrossBar,
				position.Center, position.First, position.Last, _fakeBarLines);
			TopBar = new Bar(Bar.Role.Horizontal, position.First, 0, Bounds.Width, _fakeBarLines);
			BottomBar = new Bar(Bar.Role.Horizontal, position.Last, 0, Bounds.Width, _fakeBarLines);
		}

		public override void Drag(Bar bar, Point delta)
		{
            if (bar == TopBar)
            {
				bar.Y1 += delta.Y;
				bar.Y2 += delta.Y;
				CrossBar.Y1 += delta.Y;
			}
			else if (bar == BottomBar)
            {
				bar.Y1 += delta.Y;
				bar.Y2 += delta.Y;
				CrossBar.Y2 += delta.Y;
			}
			else if (bar == CrossBar)
            {
                TopBar.Y1 += delta.Y;
                TopBar.Y2 += delta.Y;
                BottomBar.Y1 += delta.Y;
                BottomBar.Y2 += delta.Y;
                bar.X1 += delta.X;
                bar.X2 += delta.X;
                bar.Y1 += delta.Y;
                bar.Y2 += delta.Y;
			}
		}

		public override Bar IsNearBar(Point p)
		{
            foreach (var bar in Bars)
            {
                if (bar.IsNear(p))
                {
                    return bar;
                }
            }
            return null;
		}

		public override double Value()
		{
			return BottomBar.Position - TopBar.Position;
		}
	}
}