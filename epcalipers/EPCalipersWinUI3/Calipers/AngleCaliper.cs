using EPCalipersWinUI3.Contracts;
using Microsoft.UI;
using Microsoft.UI.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace EPCalipersWinUI3.Calipers
{
	internal class AngleCaliper : Caliper
	{
		public Bar LeftAngleBar { get; set; }
		public Bar RightAngleBar { get; set; }
		public Bar ApexBar { get; set; } // a pseudobar

		private bool _fakeBarLines;

		public AngleCaliper(AngleCaliperPosition position, 
			ICaliperView caliperView, bool fakeBarLines = false) : base(caliperView)
		{
			_fakeBarLines = fakeBarLines;
			InitBars(position);
			Bars = new[] { LeftAngleBar, RightAngleBar, ApexBar};
			SetThickness(2);
			CaliperType = CaliperType.Angle;
		}

		private void InitBars(AngleCaliperPosition position)
		{
			LeftAngleBar = new Bar(Bar.Role.LeftAngle, position.Apex, position.FirstAngle, Bounds, _fakeBarLines);
			LeftAngleBar.Angle = position.FirstAngle;
			LeftAngleBar.Color = Colors.Red;
			RightAngleBar = new Bar(Bar.Role.RightAngle, position.Apex, position.LastAngle, Bounds, _fakeBarLines); 
			RightAngleBar.Angle = position.LastAngle;
			RightAngleBar.Color = Colors.Green;
			ApexBar = new(Bar.Role.Apex, position.Apex, 0, Bounds, _fakeBarLines);
			// TODO: make apex bar invisible even when tapped.
			ApexBar.Color = Colors.Transparent;  // Change to a different color to debug.
		}

		public override void ChangeBounds()
		{
			var bounds = CaliperView.Bounds;
			LeftAngleBar.Bounds = bounds;
			RightAngleBar.Bounds = bounds;
			LeftAngleBar.SetupAngleBar(new Point(LeftAngleBar.X1, LeftAngleBar.Y1), LeftAngleBar.Angle);
			RightAngleBar.SetupAngleBar(new Point(RightAngleBar.X1, RightAngleBar.Y1), RightAngleBar.Angle);
		}

		public override void Drag(Bar bar, Point delta, Point location)
		{
			switch (bar.BarRole)
			{
				case Bar.Role.Apex:
					bar.X1 += delta.X;
					bar.X2 += delta.X;
					bar.Y1 += delta.Y;
					bar.Y2 += delta.Y;
					var apex = bar.MidPoint;
					LeftAngleBar.SetupAngleBar(apex, LeftAngleBar.Angle);
					RightAngleBar.SetupAngleBar(apex, RightAngleBar.Angle);
					break;
				case Bar.Role.LeftAngle:
					LeftAngleBar.Angle = RelativeTheta(location);
					LeftAngleBar.SetupAngleBar(new Point(LeftAngleBar.X1, LeftAngleBar.Y1), LeftAngleBar.Angle);
					break;
				case Bar.Role.RightAngle:
					RightAngleBar.Angle = RelativeTheta(location);
					RightAngleBar.SetupAngleBar(new Point(RightAngleBar.X1, RightAngleBar.Y1), RightAngleBar.Angle);
					break;
				default: break;
			}
		}

		private double MoveBarAngle(Point delta, Point location)
		{
			Point newPosition = new Point(location.X + delta.X, location.Y + delta.Y);
			return RelativeTheta(newPosition);
		}

		public override Bar IsNearBar(Point p)
		{
			if (ApexBar.IsNear(p))
			{
				Debug.Print("near apex");
				return ApexBar;
			}
			if (PointNearBar(p, LeftAngleBar.Angle))
			{
				Debug.Print("near left angle bar");
				return LeftAngleBar;
			} 
			if (PointNearBar(p, RightAngleBar.Angle))
			{
				Debug.Print("near right angle bar");
				return RightAngleBar;
			}
			Debug.Print("not near any bar");
			return null;
		}

		private readonly double angleDelta = 0.1;

		public bool PointNearBar(Point p, double barAngle)
		{
			double theta = RelativeTheta(p);
			return theta < (double)barAngle + angleDelta &&
				theta > (double)barAngle - angleDelta;
		}

		private double RelativeTheta(Point p)
		{
			var x = p.X - LeftAngleBar.X1;
			var y = p.Y - LeftAngleBar.Y1;
			return Math.Atan2(y, x);
		}

		public override double Value()
		{
			throw new NotImplementedException();
		}
	}
}
