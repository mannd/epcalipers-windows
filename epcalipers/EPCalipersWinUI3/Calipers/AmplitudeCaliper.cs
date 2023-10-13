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

		private bool _fakeComponentLines;

		public AmplitudeCaliper(CaliperPosition position, ICaliperView caliperView,
			bool fakeComponentLines = false) : base(caliperView)
		{
			_fakeComponentLines = fakeComponentLines;
			SetInitialPosition(position);
			CaliperComponents = new[] { TopBar, BottomBar, CrossBar };
			SetThickness(2);
			CaliperType = CaliperType.Amplitude;
		}

		private void SetInitialPosition(CaliperPosition position)
		{
			TopBar = new Bar(Bar.Role.Horizontal, position.First, 0, Bounds.Width, _fakeComponentLines);
			BottomBar = new Bar(Bar.Role.Horizontal, position.Last, 0, Bounds.Width, _fakeComponentLines);
			CrossBar = new Bar(Bar.Role.VerticalCrossBar,
				position.Center, position.First, position.Last, _fakeComponentLines);
		}

		public override void Drag(Bar component, Point delta)
		{
            if (component == TopBar)
            {
				component.Y1 += delta.Y;
				component.Y2 += delta.Y;
				CrossBar.Y1 += delta.Y;
			}
			else if (component == BottomBar)
            {
				component.Y1 += delta.Y;
				component.Y2 += delta.Y;
				CrossBar.Y2 += delta.Y;
			}
			else if (component == CrossBar)
            {
                TopBar.Y1 += delta.Y;
                TopBar.Y2 += delta.Y;
                BottomBar.Y1 += delta.Y;
                BottomBar.Y2 += delta.Y;
                CrossBar.X1 += delta.X;
                CrossBar.X2 += delta.X;
                CrossBar.Y1 += delta.Y;
                CrossBar.Y2 += delta.Y;
			}
		}

		public override Bar IsNearComponent(Point p)
		{
            foreach (var component in CaliperComponents)
            {
                if (component.IsNear(p))
                {
                    return component;
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
