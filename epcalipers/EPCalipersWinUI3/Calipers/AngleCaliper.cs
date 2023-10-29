using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace EPCalipersWinUI3.Calipers
{
	public class AngleCaliper : Caliper
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
			TextBlock testTextBlock = new TextBlock();
			testTextBlock.Text = "Testing";
			CaliperView.Add(testTextBlock);

		}

		private void InitBars(AngleCaliperPosition position)
		{
			LeftAngleBar = new Bar(Bar.Role.LeftAngle, position.Apex, position.FirstAngle, Bounds, _fakeBarLines);
			LeftAngleBar.Angle = position.FirstAngle;
			RightAngleBar = new Bar(Bar.Role.RightAngle, position.Apex, position.LastAngle, Bounds, _fakeBarLines); 
			RightAngleBar.Angle = position.LastAngle;
			ApexBar = new(Bar.Role.Apex, position.Apex, 0, Bounds, _fakeBarLines); // ApexBar never drawn
		}

		public override void ChangeBounds()
		{
			var bounds = CaliperView.Bounds;
			LeftAngleBar.Bounds = bounds;
			RightAngleBar.Bounds = bounds;
			LeftAngleBar.SetAngleBarPosition(new Point(LeftAngleBar.X1, LeftAngleBar.Y1), LeftAngleBar.Angle);
			RightAngleBar.SetAngleBarPosition(new Point(RightAngleBar.X1, RightAngleBar.Y1), RightAngleBar.Angle);
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
					LeftAngleBar.SetAngleBarPosition(apex, LeftAngleBar.Angle);
					RightAngleBar.SetAngleBarPosition(apex, RightAngleBar.Angle);
					break;
				case Bar.Role.LeftAngle:
					LeftAngleBar.Angle = RelativeTheta(location);
					LeftAngleBar.SetAngleBarPosition(new Point(LeftAngleBar.X1, LeftAngleBar.Y1), LeftAngleBar.Angle);
					break;
				case Bar.Role.RightAngle:
					RightAngleBar.Angle = RelativeTheta(location);
					RightAngleBar.SetAngleBarPosition(new Point(RightAngleBar.X1, RightAngleBar.Y1), RightAngleBar.Angle);
					break;
				default: break;
			}
			string text = string.Format("{0:0.#}°", Value);
			Debug.WriteLine(text);
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

		public override double Value =>  MathHelper.RadiansToDegrees(LeftAngleBar.Angle - RightAngleBar.Angle);
	}
}
