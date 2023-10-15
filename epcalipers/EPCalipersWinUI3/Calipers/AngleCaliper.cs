using EPCalipersWinUI3.Contracts;
using Microsoft.UI;
using System;
using System.Collections.Generic;
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

		public AngleCaliper(CaliperPosition position,
			ICaliperView caliperView, bool fakeBarLines = false) : base(caliperView)
		{
			_fakeBarLines = fakeBarLines;
			InitBars(position);
			Bars = new[] { LeftAngleBar, RightAngleBar};
			SetThickness(2);
			CaliperType = CaliperType.Angle;
		}

		private void InitBars(CaliperPosition position)
		{
			LeftAngleBar = new Bar(Bar.Role.LeftAngle, new Point(100, 100), 0.5 * Math.PI); 
			LeftAngleBar.Color = Colors.Red;
			RightAngleBar = new Bar(Bar.Role.RightAngle, new Point(100, 100), 0.25 * Math.PI); 
			RightAngleBar.Color = Colors.Green;

		}

		public override void Drag(Bar bar, Point delta)
		{
			throw new NotImplementedException();
		}

		public override Bar IsNearBar(Point p)
		{
			throw new NotImplementedException();
		}

		public override double Value()
		{
			throw new NotImplementedException();
		}
	}
}
