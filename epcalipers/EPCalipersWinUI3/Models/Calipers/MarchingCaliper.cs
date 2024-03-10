﻿using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
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

		private void InitBars()
		{
			var value = RightPosition - LeftPosition;
			var leftOrigin = LeftPosition;
			var rightOrigin = RightPosition;
			var height = TimeCaliper.LeftBar.Y2;
			// TODO: Other means to deemphasize marching calipers?
			var thickness = Math.Max(Settings.Instance.BarThickness - 1, 1);
			for (int i = 0; i < NumberOfBars; i++)
			{
				Bar leftBar = new Bar(Bar.Role.Marching, leftOrigin - (value * (i + 1)), 0, height, _fakeUI);
				leftBar.SelectedColor = TimeCaliper.SelectedColor;
				leftBar.UnselectedColor = TimeCaliper.UnselectedColor;
				leftBar.IsSelected = TimeCaliper.IsSelected;
				leftBar.Thickness = thickness;
				leftBar.AddToView(CaliperView);
				LeftBars.Add(leftBar);
				Bar rightBar = new Bar(Bar.Role.Marching, rightOrigin + (value * (i + 1)), 0, height, _fakeUI);
				rightBar.SelectedColor = TimeCaliper.SelectedColor;
				rightBar.UnselectedColor = TimeCaliper.UnselectedColor;
				rightBar.IsSelected = TimeCaliper.IsSelected;
				rightBar.Thickness = thickness;
				rightBar.Visibility = rightOrigin + (value * (i + 1)) > Bounds.Width ? Microsoft.UI.Xaml.Visibility.Collapsed : Microsoft.UI.Xaml.Visibility.Visible;
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
				RightBars[i].Visibility = right + (value * (i + 1)) > Bounds.Width ? Microsoft.UI.Xaml.Visibility.Collapsed : Microsoft.UI.Xaml.Visibility.Visible;
				// TODO: leftbars need to be hidden when caliper is negative and left bars are on the right
				//LeftBars[i].Visibility = right + (value * (i + 1)) > Bounds.Width ? Microsoft.UI.Xaml.Visibility.Collapsed : Microsoft.UI.Xaml.Visibility.Visible;
			}
			// TODO: hide bars when cycle length less than minimum value ? necessary
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
