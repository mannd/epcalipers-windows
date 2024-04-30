using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;

namespace EPCalipersWinUI3.Models.Calipers
{
	public class MarchingCaliper : Caliper
	{
		public int NumberOfBars { get; set; }
		public double LeftPosition { get; set; }
		public double RightPosition { get; set; }

		public List<Bar> LeftBars { get; set; } = [];
		public List<Bar> RightBars { get; set; } = [];

		public TimeCaliper TimeCaliper { get; set; }

		public override void SetFullSelectionTo(bool value)
		{
			foreach (var bar in LeftBars)
			{
				bar.IsSelected = value;
			}
			foreach (var bar in RightBars)
			{
				bar.IsSelected = value;
			}
		}

		public MarchingCaliper(ICaliperView caliperView, 
			TimeCaliper caliper, double left, double right, bool fakeUI = false) : base(caliperView, Calibration.None)
		{
			NumberOfBars = Settings.Instance.NumberOfMarchingCalipers;
			LeftPosition = left;
			RightPosition = right;
			TimeCaliper = caliper;
			InitBars();
		}

		public override Color UnselectedColor
		{
			get => base.UnselectedColor;
			set
			{
				foreach (var bar in LeftBars)
				{
					bar.UnselectedColor = value;
				}
				foreach (var bar in RightBars)
				{
					bar.UnselectedColor = value;
				}
			}
		}

		private void InitBars()
		{
			var value = RightPosition - LeftPosition;
			var leftOrigin = LeftPosition;
			var rightOrigin = RightPosition;
			var height = TimeCaliper.LeftBar.Y2;
			// DEFER: Other means to deemphasize marching calipers?
			var thickness = TimeCaliper.ScaledBarThickness.Thickness - 1;
			for (int i = 0; i < NumberOfBars; i++)
			{
				Bar leftBar = new Bar(Bar.Role.Marching, leftOrigin - (value * (i + 1)), 0, height, _fakeUI)
				{
					SelectedColor = TimeCaliper.SelectedColor,
					UnselectedColor = TimeCaliper.UnselectedColor,
					IsSelected = TimeCaliper.IsSelected,
					Thickness = thickness,
					Visibility = leftOrigin - (value * (i + 1)) < 0 ? Microsoft.UI.Xaml.Visibility.Collapsed : Microsoft.UI.Xaml.Visibility.Visible
				};
				leftBar.AddToView(CaliperView);
				LeftBars.Add(leftBar);
				Bar rightBar = new Bar(Bar.Role.Marching, rightOrigin + (value * (i + 1)), 0, height, _fakeUI)
				{
					SelectedColor = TimeCaliper.SelectedColor,
					UnselectedColor = TimeCaliper.UnselectedColor,
					IsSelected = TimeCaliper.IsSelected,
					Thickness = thickness,
					Visibility = rightOrigin + (value * (i + 1)) > Bounds.Width ? Microsoft.UI.Xaml.Visibility.Collapsed : Microsoft.UI.Xaml.Visibility.Visible
				};
				rightBar.AddToView(CaliperView);
				RightBars.Add(rightBar);
			}
		}

		public void UpdateMarchingScaledBarThickness()
		{
			var thickness = Math.Max(1.0, TimeCaliper.ScaledBarThickness.ScaledThickness() - 1);
			foreach (var bar in LeftBars)
			{
				bar.Thickness = thickness;
			}
			foreach (var bar in RightBars)
			{
				bar.Thickness = thickness;
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
			var bounds = CaliperView.Bounds;
			foreach (var bar in LeftBars)
			{
				bar.Y2 = bounds.Height;
			}
			foreach (var bar in RightBars)
			{
				bar.Y2 = bounds.Height;
			}
		}

		public void Move()
		{
			var left = TimeCaliper.LeftMostBarPosition;
			var right = TimeCaliper.RightMostBarPosition;
			var value = TimeCaliper.Value;
			for (var i = 0; i < NumberOfBars; i++)
			{
				LeftBars[i].X1 = left - (value * (i + 1));
				LeftBars[i].X2 = LeftBars[i].X1;
				RightBars[i].X1 = right + (value * (i + 1));
				RightBars[i].X2 = RightBars[i].X1;
				RightBars[i].Visibility = right + (value * (i + 1)) > Bounds.Width || right + (value * (i + 1)) < 0 
					? Microsoft.UI.Xaml.Visibility.Collapsed : Microsoft.UI.Xaml.Visibility.Visible;
				LeftBars[i].Visibility = left - (value * (i + 1)) < 0 || left - (value * (i + 1)) > Bounds.Width 
					? Microsoft.UI.Xaml.Visibility.Collapsed : Microsoft.UI.Xaml.Visibility.Visible;
			}
			// DEFER: hide bars when cycle length less than minimum value ? necessary
			// Can do this by adding a abs(value) < minimumValue to the Visibility statements above.
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
