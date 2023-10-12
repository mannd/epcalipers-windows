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
        public CaliperComponent TopBar { get; set; }
        public CaliperComponent BottomBar { get; set; }
        public CaliperComponent CrossBar { get; set; }

		private bool _fakeComponentLines;

		public AmplitudeCaliper(Bounds bounds, CaliperPosition position,
			bool fakeComponentLines = false) : base(bounds)
		{
			_fakeComponentLines = fakeComponentLines;
			SetInitialPosition(position);
			CaliperComponents = new[] { TopBar, BottomBar, CrossBar };
			SetThickness(2);
			CaliperType = CaliperType.Amplitude;
		}

		private void SetInitialPosition(CaliperPosition position)
		{
            if (_fakeComponentLines)
            {
                var fakeComponentLine = new FakeComponentLine();
                TopBar = new CaliperComponent(CaliperComponent.Role.Horizontal, position.First, 0, Bounds.Width, fakeComponentLine);
                BottomBar = new CaliperComponent(CaliperComponent.Role.Horizontal, position.Last, 0, Bounds.Width, fakeComponentLine);
                CrossBar = new CaliperComponent(CaliperComponent.Role.VerticalCrossBar, 
                    position.Center, position.First, position.Last, fakeComponentLine);
            }
            else
            {
                TopBar = new CaliperComponent(CaliperComponent.Role.Horizontal, position.First, 0, Bounds.Width);
                BottomBar = new CaliperComponent(CaliperComponent.Role.Horizontal, position.Last, 0, Bounds.Width);
                CrossBar = new CaliperComponent(CaliperComponent.Role.VerticalCrossBar, 
                    position.Center, position.First, position.Last);
            }

		}

		public override void Add(Grid grid)
		{
            if (grid == null) return; // grid can be null for testing.
            grid.Children.Add(TopBar.GetComponent());
            grid.Children.Add(BottomBar.GetComponent());
            grid.Children.Add(CrossBar.GetComponent());
            //grid.Children.Add(CaliperLabel);
		}

		public override void Delete(Grid grid)
		{
            if (grid == null) return;
            grid.Children.Remove(TopBar.GetComponent());
            grid.Children.Remove(BottomBar.GetComponent());
            grid.Children.Remove(CrossBar.GetComponent());
            //grid.Children.Remove(CaliperLabel);
		}

		public override void Drag(CaliperComponent component, Point delta)
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

		public override CaliperComponent IsNearComponent(Point p)
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
