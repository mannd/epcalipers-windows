﻿using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Models
{
    public enum ComponentRole
    {
        Left,
        Right,
        Top,
        Bottom,
        Cross,
        Apex
    }
	public class TimeCaliper  
	{
		public CaliperComponent LeftBar { get; set; }
		public CaliperComponent RightBar { get; set; }
		public CaliperComponent CrossBar { get; set; }

		// Used to set the bounds of the caliper components.
		public double Height { get; set; }
		public double Width { get; set; }	

		public Dictionary<ComponentRole, CaliperComponent> CaliperComponents { get; set; }

		public delegate void DrawDelegate();

		public DrawDelegate Draw { get; set; } = null;

		public TimeCaliper(double boundsWidth, double boundsHeight)
		{
			LeftBar = new CaliperComponent(ComponentDirection.Vertical);
			RightBar = new CaliperComponent(ComponentDirection.Vertical);
			CrossBar = new CaliperComponent(ComponentDirection.Horizontal);
			CaliperComponents = new()
			{
				{ComponentRole.Left, LeftBar },
				{ComponentRole.Right, RightBar },
				{ComponentRole.Cross, CrossBar }
			};
			// temp initial position of caliper
			LeftBar.Position = 100;
			RightBar.Position = 200;
			CrossBar.Position = 200;

			LeftBar.Line = new Microsoft.UI.Xaml.Shapes.Line
			{
				X1 = LeftBar.Position,
				X2 = LeftBar.Position,
				Y1 = 0,
				Y2 = boundsHeight
			};
			RightBar.Line = new Microsoft.UI.Xaml.Shapes.Line
			{
				X1 = RightBar.Position,
				X2 = RightBar.Position,
				Y1 = 0,
				Y2 = boundsHeight
			};
			CrossBar.Line = new Microsoft.UI.Xaml.Shapes.Line
			{
				X1 = LeftBar.Position,
				X2 = RightBar.Position,
				Y1 = CrossBar.Position,
				Y2 = CrossBar.Position
			};
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

		public double Value
		{
			get
			{
				return (double)RightBar.Position - LeftBar.Position;
			}
		}

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				foreach (var component in CaliperComponents)
				{
					component.Value.IsSelected = value;
				}
			}
		}
		private bool _isSelected;

		public void Move(CaliperComponent component, int distance)
		{
			component?.Move(distance);
		}

		public void Move(int distance)
		{
			Move(LeftBar, distance);
			Move(RightBar, distance);
		}

		public void ToggleIsSelected()
		{
			throw new NotImplementedException();
		}

		public void DrawCaliper()
		{
			if (Draw != null)
			{
				Draw();
			}
		}
	}
}
