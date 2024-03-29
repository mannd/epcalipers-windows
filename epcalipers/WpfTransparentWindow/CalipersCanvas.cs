﻿using EPCalipersCore;
using EPCalipersCore.Properties;
using System.Windows.Controls;

namespace WpfTransparentWindow
{
	class CalipersCanvas : Canvas, ICalipers
	{
		readonly BaseCalipers calipers = new BaseCalipers();
		public Calibration HorizontalCalibration
		{
			get
			{
				return calipers.HorizontalCalibration;
			}
			set
			{
				calipers.HorizontalCalibration = value;
			}
		}
		public Calibration VerticalCalibration
		{
			get
			{
				return calipers.VerticalCalibration;
			}
			set
			{
				calipers.VerticalCalibration = value;
			}
		}

		public bool TweakingComponent
		{
			get
			{
				return calipers.TweakingComponent;
			}
			set
			{
				calipers.TweakingComponent = value;
			}
		}

		public CaliperComponent ChosenComponent
		{
			get
			{
				return calipers.ChosenComponent;
			}
			set
			{
				calipers.ChosenComponent = value;
			}
		}

		public CalipersCanvas() : base()
		{

		}

		public void AddCaliper(BaseCaliper c)
		{
			calipers.AddCaliper(c);
		}

		public void DrawCalipers()
		{
			// This ensures deleted caliper disappears
			Children.Clear();
			foreach (BaseCaliper c in calipers.GetCalipers())
			{
				DrawCaliper(c);
			}
		}

		public void UpdatePreferences()
		{
			// TODO: this isn't working
			Preferences p = new Preferences();
			p.Load();
			calipers.UpdatePreferences(p);
		}

		private void DrawCaliper(BaseCaliper c)
		{
			c.Draw(this);
		}

		public void GrabCaliperIfClicked(System.Windows.Point point)
		{
			calipers.GrabCaliperIfClicked(ConvertPoint(point));
		}

		private System.Drawing.Point ConvertPoint(System.Windows.Point point)
		{
			return new System.Drawing.Point((int)point.X, (int)point.Y);
		}

		public bool DragGrabbedCaliper(float deltaX, float deltaY, System.Windows.Point location)
		{
			return calipers.DragGrabbedCaliper(deltaX, deltaY, new System.Drawing.PointF((float)location.X, (float)location.Y));
		}

		public bool ReleaseGrabbedCaliper(int clickCount)
		{
			return calipers.ReleaseGrabbedCaliper(clickCount);
		}

		public bool DeleteCaliperIfClicked(System.Windows.Point point)
		{
			return calipers.DeleteCaliperIfClicked(ConvertPoint(point));
		}

		public int NumberOfCalipers()
		{
			return calipers.NumberOfCalipers();
		}

		public bool NoCaliperIsSelected()
		{
			return calipers.NoCaliperIsSelected();
		}

		public void SelectSoleCaliper()
		{
			calipers.SelectSoleCaliper();
		}

		public BaseCaliper GetActiveCaliper()
		{
			return calipers.GetActiveCaliper();
		}

		public BaseCaliper GetLoneTimeCaliper()
		{
			return calipers.GetLoneTimeCaliper();
		}

		public void SelectCaliper(BaseCaliper c)
		{
			calipers.SelectCaliper(c);
		}

		public void UnselectCalipersExcept(BaseCaliper c)
		{
			calipers.UnselectCalipersExcept(c);
		}

		public bool NoTimeCaliperSelected()
		{
			return calipers.NoTimeCaliperSelected();
		}

		public void SetChosenCaliper(System.Windows.Point point)
		{
			calipers.SetChosenCaliper(ConvertPoint(point));
		}

		public void SetChosenCaliperComponent(System.Windows.Point point)
		{
			calipers.SetChosenCaliperComponent(ConvertPoint(point));
		}

		public bool NoChosenCaliper()
		{
			return calipers.NoChosenCaliper();
		}

		public BaseCaliper GetGrabbedCaliper(System.Windows.Point point)
		{
			return calipers.GetGrabbedCaliper(ConvertPoint(point));
		}

		public bool PointIsNearCaliper(System.Windows.Point point)
		{
			return calipers.PointIsNearCaliper(ConvertPoint(point));
		}

		public void DeleteAllCalipers()
		{
			calipers.DeleteAllCalipers();
		}

		public void DeleteSelectedCaliper()
		{
            BaseCaliper c = calipers.GetActiveCaliper();
            if (c != null)
            {
                calipers.DeleteCaliper(c);
            }

		}

		public void UnselectChosenCaliper()
		{
			calipers.UnselectChosenCaliper();
		}
		public System.Drawing.Color GetChosenCaliperColor()
		{
			return calipers.GetChosenCaliperColor();
		}

		public void SetChosenCaliperColor(System.Drawing.Color color)
		{
			calipers.SetChosenCaliperColor(color);
		}

		public bool MarchCaliper()
		{
			return calipers.MarchCaliper();
		}

		public void CancelTweaking()
		{
			calipers.ClearAllChosenComponents();
			calipers.CancelTweaking();
		}

		public string GetChosenComponentName()
		{
			return calipers.GetChosenComponentName();
		}

		public void Move(MovementDirection movementDirection)
		{
			calipers.Move(movementDirection);
		}

		public void MicroMove(MovementDirection movementDirection)
		{
			calipers.MicroMove(movementDirection);
		}

		public void ClearAllChosenComponentsExceptForChosenCaliper()
		{
			calipers.ClearAllChosenComponentsExceptForChosenCaliper();
		}

		public void ClearAllChosenComponents()
		{
			calipers.ClearAllChosenComponents();
		}

		public BaseCaliper GetChosenCaliper()
		{
			return calipers.chosenCaliper;
		}
	}
}
