using EPCalipersWinUI3.Contracts;
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

        public string Text { get; set; }  // TODO: a class for the Text of a caliper

		private bool _fakeComponentLines;

        public TimeCaliper(CaliperPosition position, 
			ICaliperView caliperView, bool fakeComponentLines = false) : base(caliperView)
        {
            _fakeComponentLines = fakeComponentLines;
            SetInitialPosition(position);
            CaliperComponents =  new[] { LeftBar, RightBar, CrossBar };
			SetThickness(2);
			CaliperType = CaliperType.Time;
        }

		private void SetInitialPosition(CaliperPosition position)
        {
			LeftBar = new Bar(Bar.Role.Vertical, position.First, 0, Bounds.Height, _fakeComponentLines);
			RightBar = new Bar(Bar.Role.Vertical, position.Last, 0, Bounds.Height, _fakeComponentLines);
			CrossBar = new Bar(Bar.Role.HorizontalCrossBar, position.Center, position.First, position.Last, _fakeComponentLines);
		}
		private void SetInitialPositionNearCorner()
		{
			//Point offset = ecgPictureBox.Location;
			//c.initialOffset = new Point(-offset.X, -offset.Y);

			//// init with Horizontal bar offsets
			//int barOffset = _initialOffset.X;
			//int crossbarOffset = _initialOffset.Y;

			//if (Direction == CaliperDirection.Vertical)
			//{
			//	barOffset = _initialOffset.Y;
			//	crossbarOffset = _initialOffset.X;
			//}

			//Bar1Position = 50 + differential + barOffset;
			//Bar2Position = 100 + differential + barOffset;
			//CrossbarPosition = 100 + differential + crossbarOffset;
			//differential += 15.0f;
			//if (differential > 80.0f)
			//{
			//	differential = 0.0f;
			//}
		}

		public override double Value()
		{
			return RightBar.Position - LeftBar.Position;
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

		public override void Drag(Bar component, Point delta)
		{
            if (component == LeftBar)
            {
				component.X1 += delta.X;
				component.X2 += delta.X;
				CrossBar.X1 += delta.X;
			}
			else if (component == RightBar)
            {
				component.X1 += delta.X;
				component.X2 += delta.X;
				CrossBar.X2 += delta.X;
			}
			else if (component == CrossBar)
            {
                LeftBar.X1 += delta.X;
                LeftBar.X2 += delta.X;
                RightBar.X1 += delta.X;
                RightBar.X2 += delta.X;
                CrossBar.X1 += delta.X;
                CrossBar.X2 += delta.X;
                CrossBar.Y1 += delta.Y;
                CrossBar.Y2 += delta.Y;
			}
		}
	}
}
