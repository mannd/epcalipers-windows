using EPCalipersWinUI3.Contracts;
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

		public AmplitudeCaliper(CaliperPosition position, ICaliperView caliperView,
			bool fakeUI = false) : base(caliperView)
		{
			_fakeUI = fakeUI;
			Bars = InitBars(position);
			InitCaliperLabel();
			SetThickness(2);
			CaliperType = CaliperType.Amplitude;
		}

		private Bar[] InitBars(CaliperPosition position)
		{
			// NB Crossbar must be first to allow isNear to work properly.
			CrossBar = new Bar(Bar.Role.VerticalCrossBar,
				position.Center, position.First, position.Last, _fakeUI);
			TopBar = new Bar(Bar.Role.Horizontal, position.First, 0, Bounds.Width, _fakeUI);
			BottomBar = new Bar(Bar.Role.Horizontal, position.Last, 0, Bounds.Width, _fakeUI);
			return new[] { TopBar, BottomBar, CrossBar };
		}
		private void InitCaliperLabel()
		{
			var text = $"{Value} points";
			CaliperLabel = new AmplitudeCaliperLabel(this, CaliperView, text,
				CaliperLabelAlignment.Top, false, base._fakeUI);
		}

		public override void ChangeBounds()
		{
			var bounds = CaliperView.Bounds;
			TopBar.X2 = bounds.Width;
			BottomBar.X2 = bounds.Width;
		}

		public override void Drag(Bar bar, Point delta, Point previousPoint)
		{
            if (bar == TopBar)
            {
				bar.Position += delta.Y;
				CrossBar.Y1 += delta.Y;
			}
			else if (bar == BottomBar)
            {
				bar.Position += delta.Y;
				CrossBar.Y2 += delta.Y;
			}
			else if (bar == CrossBar)
            {
                TopBar.Position += delta.Y;
                BottomBar.Position += delta.Y;
                bar.Position += delta.X;
                bar.Y1 += delta.Y;
                bar.Y2 += delta.Y;
			}
			CaliperLabel.Text = $"{Value} points";
			CaliperLabel.SetPosition();
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

		public override double Value =>  BottomBar.Position - TopBar.Position;
	}
}
