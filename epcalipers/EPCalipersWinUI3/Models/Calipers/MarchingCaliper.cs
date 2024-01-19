using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Foundation;

namespace EPCalipersWinUI3.Models.Calipers
{
	public class MarchingCaliper : Caliper
	{
		private const int _maxBars = 10;
		private const int _minBars = 1;
		private const int _minimumValue = 20;

		public int NumberOfBars { get; set; }
		public double LeftPosition { get; set; }
		public double RightPosition { get; set; }

		public List<Bar> LeftBars { get; set; } = new List<Bar>();
		public List<Bar> RightBars { get; set; } = new List<Bar>();

		public TimeCaliper TimeCaliper { get; set; }


		public MarchingCaliper(ICaliperView caliperView, TimeCaliper caliper, int numberOfBars, double left, double right, bool fakeUI = false) : base(caliperView, Calibration.None)
		{
			NumberOfBars = Math.Clamp(numberOfBars, _minBars, _maxBars);
			LeftPosition = left;
			RightPosition = right;
			TimeCaliper = caliper;
			InitBars();
		}

		private void InitBars()
		{
			var value = RightPosition - LeftPosition;
			var leftOrigin = LeftPosition;
			var rightOrigin = RightPosition;
			for (int i = 1; i <= NumberOfBars; i++)
			{
				Bar leftBar = new Bar(Bar.Role.Marching, leftOrigin - (value * i), 0, Bounds.Height, _fakeUI);
				leftBar.SelectedColor = TimeCaliper.SelectedColor;
				leftBar.UnselectedColor = TimeCaliper.UnselectedColor;
				leftBar.IsSelected = TimeCaliper.IsSelected;
				leftBar.AddToView(CaliperView);
				LeftBars.Add(leftBar);
				Bar rightBar = new Bar(Bar.Role.Marching, rightOrigin + (value * i), 0, Bounds.Height, _fakeUI); 
				rightBar.SelectedColor = TimeCaliper.SelectedColor;
				rightBar.UnselectedColor = TimeCaliper.UnselectedColor;
				rightBar.IsSelected = TimeCaliper.IsSelected;
				rightBar.AddToView(CaliperView);
				RightBars.Add(rightBar);
			}
		}

		public override void Remove(ICaliperView caliperView)
		{
			if (caliperView == null) return;
			foreach (var bar in LeftBars) bar?.RemoveFromView(caliperView);
			foreach (var bar in RightBars) bar?.RemoveFromView(caliperView);
		}

		public override Bar HandleBar => throw new NotImplementedException();

		public override double Value => throw new NotImplementedException();

		public override void ChangeBounds()
		{
			throw new NotImplementedException();
		}

		public override void Drag(Bar bar, Point delta, Point previousPoint)
		{
			throw new NotImplementedException();
		}

		public override Bar IsNearBar(Point p)
		{
			throw new NotImplementedException();
		}
	}
}
