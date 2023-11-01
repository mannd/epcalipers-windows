﻿using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Input;
using Windows.Foundation;
using Windows.Gaming.XboxLive.Storage;

namespace EPCalipersWinUI3.Calipers
{

    public sealed class TimeCaliper : Caliper
    {
        public Bar LeftBar { get; set; }
        public Bar RightBar { get; set; }
        public Bar CrossBar { get; set; }

        public TimeCaliper(CaliperPosition position, 
			ICaliperView caliperView, bool fakeUI = false) : base(caliperView)
        {
            _fakeUI = fakeUI;
            Bars = InitBars(position);
			SetThickness(2);
			CaliperType = CaliperType.Time;
			InitCaliperLabel();
        }

		private Bar[] InitBars(CaliperPosition position)
        {
			// NB Crossbar must be first, to allow IsNear to work correctly.
			CrossBar = new Bar(Bar.Role.HorizontalCrossBar, position.Center, position.First, position.Last, _fakeUI);
			LeftBar = new Bar(Bar.Role.Vertical, position.First, 0, Bounds.Height, _fakeUI);
			RightBar = new Bar(Bar.Role.Vertical, position.Last, 0, Bounds.Height, _fakeUI);
            return  new[] { LeftBar, RightBar, CrossBar };
		}

		private void InitCaliperLabel()
		{
			var text = $"{Value} points";
			CaliperLabel = new TimeCaliperLabel(this, CaliperView, text,
				CaliperLabelAlignment.Right, false, _fakeUI);
		}

		public override void ChangeBounds()
		{
			var bounds = CaliperView.Bounds;
			LeftBar.Y2 = bounds.Height;
			RightBar.Y2 = bounds.Height;
		}

		public override double Value => RightBar.Position - LeftBar.Position; 


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

		public override void Drag(Bar bar, Point delta, Point previousPoint)
		{
            if (bar == LeftBar)
            {
				bar.Position += delta.X;
				CrossBar.X1 += delta.X;
			}
			else if (bar == RightBar)
            {
				bar.Position += delta.X;
				CrossBar.X2 += delta.X;
			}
			else if (bar == CrossBar)
            {
                LeftBar.Position += delta.X;
                RightBar.Position += delta.X;
                bar.X1 += delta.X;
                bar.X2 += delta.X;
                bar.Position += delta.Y;
			}
			CaliperLabel.Text = $"{Value} points";
			CaliperLabel.SetPosition();
		}
	}
}
