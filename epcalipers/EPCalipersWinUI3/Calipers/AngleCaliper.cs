using EPCalipersWinUI3.Contracts;
using Microsoft.UI;
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

		private bool _fakeBarLines;

		public AngleCaliper(AngleCaliperPosition position, 
			ICaliperView caliperView, bool fakeBarLines = false) : base(caliperView)
		{
			_fakeBarLines = fakeBarLines;
			InitBars(position);
			Bars = new[] { LeftAngleBar, RightAngleBar};
			SetThickness(2);
			CaliperType = CaliperType.Angle;

		}

		private void InitBars(AngleCaliperPosition position)
		{
			LeftAngleBar = new Bar(Bar.Role.LeftAngle, position.Apex, position.FirstAngle, Bounds);
			LeftAngleBar.Angle = position.FirstAngle;
			LeftAngleBar.Color = Colors.Red;
			RightAngleBar = new Bar(Bar.Role.RightAngle, position.Apex, position.LastAngle, Bounds); 
			RightAngleBar.Angle = position.LastAngle;
			RightAngleBar.Color = Colors.Green;
		}

		public override void Drag(Bar bar, Point delta)
		{
			throw new NotImplementedException();
		}

		public override Bar IsNearBar(Point p)
		{
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
