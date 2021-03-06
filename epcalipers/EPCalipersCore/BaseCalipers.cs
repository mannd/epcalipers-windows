﻿using EPCalipersCore.Properties;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace EPCalipersCore
{
	// This class manages the set of calipers on the screen
	public class BaseCalipers : ICalipers
	{
		readonly List<BaseCaliper> calipers = new List<BaseCaliper>();
		public Calibration HorizontalCalibration { get; set; }
		public Calibration VerticalCalibration { get; set; }
		public bool IsFullyTransparent { get; set; }

		// for caliper movement
		private BaseCaliper grabbedCaliper;
		private bool crossbarGrabbed = false;
		private bool bar1Grabbed = false;
		private bool bar2Grabbed = false;
		private bool caliperWasDragged = false;

		// for color and tweaking
		private BaseCaliper chosenCaliper;
		public CaliperComponent ChosenComponent { get; set; }
		public bool TweakingComponent { get; set; }
		private readonly float tweakDistance = 0.4f;
		private readonly float hiresTweakDistance = 0.01f;


		public BaseCalipers()
		{
			grabbedCaliper = null;
			HorizontalCalibration = new Calibration(CaliperDirection.Horizontal);
			VerticalCalibration = new Calibration(CaliperDirection.Vertical);
			chosenCaliper = null;
			ChosenComponent = CaliperComponent.NoComponent;
			TweakingComponent = false;
			IsFullyTransparent = false;
		}

		public List<BaseCaliper> GetCalipers()
		{
			return calipers;
		}

		public void Draw(Graphics g, RectangleF rect)
		{
			foreach (var c in calipers)
			{
				c.Draw(g, rect);
			}
		}

		public void AddCaliper(BaseCaliper c)
		{
			calipers.Add(c);
		}

		public void DeleteCaliper(BaseCaliper c)
		{
			calipers.Remove(c);
		}

		public void DeleteAllCalipers()
		{
			calipers.Clear();
		}

		// this is the highlighted/selected caliper used for calibration or measurements
		public BaseCaliper GetActiveCaliper()
		{
			BaseCaliper c = null;
			for (int i = calipers.Count - 1; i >= 0 && c == null; i--)
			{
				if (calipers[i].IsSelected)
				{
					c = calipers[i];
				}
			}
			return c;
		}

		public bool NoCaliperIsSelected()
		{
			return GetActiveCaliper() == null;
		}

		// returns true if caliper selected changes
		public bool SelectCaliperIfNoneSelected()
		{
			bool selectionMade = false;
			if (calipers.Count > 0 && NoCaliperIsSelected())
			{
				SelectCaliper(calipers[calipers.Count - 1]);
				selectionMade = true;
			}
			return selectionMade;
		}

		public bool NoTimeCaliperSelected()
		{
			return (calipers.Count < 1 ||
				NoCaliperIsSelected() ||
				GetActiveCaliper().Direction == CaliperDirection.Vertical) ||
				GetActiveCaliper().IsAngleCaliper;
		}

		private Color AdjustColor(Color color)
		{
			// We have to avoid the color red if transparency is operational.
			// A real red color will disappear!
			if (IsFullyTransparent && color == Color.Red)
			{
				return Color.Firebrick;
			}
			else
			{
				return color;
			}
		}

		public void SelectCaliper(BaseCaliper c)
		{
			c.CaliperColor = AdjustColor(c.SelectedColor);
			c.IsSelected = true;
		}

		public void UnselectCaliper(BaseCaliper c)
		{
			c.CaliperColor = AdjustColor(c.UnselectedColor);
			c.IsSelected = false;
		}

		public void SelectSoleCaliper()
		{
			if (calipers.Count != 1)
			{
				return;
			}
			SelectCaliper(calipers[0]);
		}

		public bool ToggleCaliperIfClicked(Point point)
		{
			bool caliperToggled = false;
			for (int i = calipers.Count - 1; i >= 0; i--)
			{
				if (calipers[i].PointNearCaliper(point) && !caliperToggled)
				{
					caliperToggled = true;
					if (calipers[i].IsSelected)
					{
						UnselectCaliper(calipers[i]);
					}
					else
					{
						SelectCaliper(calipers[i]);
					}
				}
				else
				{
					UnselectCaliper(calipers[i]);
				}
			}
			return caliperToggled;
		}

		public bool DeleteCaliperIfClicked(Point point)
		{
			bool deleted = false;
			for (int i = calipers.Count - 1; i >= 0; i--)
			{
				if (calipers[i].PointNearCaliper(point) && !deleted)
				{
					DeleteCaliper(calipers[i]);
					deleted = true;
				}
			}
			return deleted;
		}

		// This shortens the caliper grabbing process c/w the other versions of EP Calipers
		public void GrabCaliperIfClicked(Point point)
		{
			BaseCaliper caliper = null;
			foreach (var c in calipers)
			{
				if (c.PointNearCrossbar(point) && caliper == null)
				{
					crossbarGrabbed = true;
					caliper = c;
				}
				else if (c.PointNearBar1(point) && caliper == null)
				{
					bar1Grabbed = true;
					caliper = c;
				}
				else if (c.PointNearBar2(point) && caliper == null)
				{
					bar2Grabbed = true;
					caliper = c;
				}
			}
			grabbedCaliper = caliper;
		}

		public BaseCaliper GetGrabbedCaliper(Point point)
		{
			BaseCaliper caliper = null;
			foreach (var c in calipers)
			{
				if (c.PointNearCaliper(point) && caliper == null)
				{
					caliper = c;
				}
			}
			return caliper;
		}

		public void SetChosenCaliper(Point point)
		{
			chosenCaliper = GetGrabbedCaliper(point);
		}

		public bool NoChosenCaliper()
		{
			return chosenCaliper == null;
		}

		// assumes chosenCaliper is not selected (e.g. highlighted)
		public void SetChosenCaliperColor(Color color)
		{
			if (chosenCaliper == null)
			{
				return;
			}
			color = AdjustColor(color);
			chosenCaliper.CaliperColor = color;
			chosenCaliper.UnselectedColor = color;
		}

		public void UnselectChosenCaliper()
		{
			if (chosenCaliper == null)
			{
				return;
			}
			UnselectCaliper(chosenCaliper);
		}

		public Color GetChosenCaliperColor()
		{
			if (chosenCaliper == null)
			{
				// shouldn't happen, but just in case
				return Color.Blue;
			}
			return chosenCaliper.UnselectedColor;
		}

		public string GetChosenComponentName()
		{
			return BaseCaliper.ComponentName(ChosenComponent);
		}

		public void SetChosenCaliperComponent(Point point)
		{
			if (chosenCaliper == null)
			{
				ChosenComponent = CaliperComponent.NoComponent;
			}
			else
			{
				ChosenComponent = GetCaliperComponent(chosenCaliper, point);
				Debug.WriteLine("set chosen component");
				chosenCaliper.ChosenComponent = ChosenComponent;
			}
		}

		private CaliperComponent GetCaliperComponent(BaseCaliper caliper, Point point)
		{
			if (caliper == null)
			{
				return CaliperComponent.NoComponent;
			}
			if (caliper.PointNearBar1(point))
			{
				return caliper.Direction == CaliperDirection.Horizontal ? CaliperComponent.LeftBar : CaliperComponent.UpperBar;
			}
			else if (caliper.PointNearBar2(point))
			{
				return caliper.Direction == CaliperDirection.Horizontal ? CaliperComponent.RightBar : CaliperComponent.LowerBar;
			}
			else if (caliper.PointNearCrossbar(point))
			{
				return caliper.IsAngleCaliper ? CaliperComponent.Apex : CaliperComponent.CrossBar;
			}
			else
			{
				return CaliperComponent.NoComponent;
			}

		}

		public void CancelTweaking()
		{
			ClearAllChosenComponents();
			TweakingComponent = false;
		}

		private void ClearAllChosenComponentsExceptFor(BaseCaliper caliper)
		{
			if (caliper == null)
			{
				ClearAllChosenComponents();
			}
			else
			{
				foreach (Caliper c in calipers)
				{
					if (c != caliper)
					{
						c.ChosenComponent = CaliperComponent.NoComponent;
					}
				}
			}
		}

		public void ClearAllChosenComponentsExceptForChosenCaliper()
		{
			ClearAllChosenComponentsExceptFor(chosenCaliper);
		}

		public void ClearAllChosenComponents()
		{
			ChosenComponent = CaliperComponent.NoComponent;
			foreach (BaseCaliper c in calipers)
			{
				c.ChosenComponent = CaliperComponent.NoComponent;
			}
		}

		public virtual void MicroMove(MovementDirection movementDirection)
		{
			MoveChosenComponent(movementDirection, hiresTweakDistance);
		}

		public virtual void Move(MovementDirection movementDirection)
		{
			MoveChosenComponent(movementDirection, tweakDistance);
		}

		private void MoveChosenComponent(MovementDirection movementDirection, float distance)
		{
			if (chosenCaliper != null)
			{
				chosenCaliper.MoveBarInDirection(movementDirection, distance, ChosenComponent);
			}
		}

		public bool DragGrabbedCaliper(float deltaX, float deltaY, PointF location)
		{
			bool needsRefresh = false;
			if (grabbedCaliper != null)
			{
				PointF delta = new PointF(deltaX, deltaY);
				if (grabbedCaliper.Direction == CaliperDirection.Vertical)
				{
					float tmp = delta.X;
					delta.X = delta.Y;
					delta.Y = tmp;
				}
				if (crossbarGrabbed)
				{
					grabbedCaliper.MoveCrossbar(delta);
				}
				else if (bar1Grabbed)
				{
					grabbedCaliper.MoveBar1(delta, location);
				}
				else if (bar2Grabbed)
				{
					grabbedCaliper.MoveBar2(delta, location);
				}
				needsRefresh = true;
				caliperWasDragged = true;
			}
			return needsRefresh;
		}

		public bool ReleaseGrabbedCaliper(int clickCount)
		{
			bool needsRefresh = false;
			if (grabbedCaliper != null)
			{
				if (!caliperWasDragged)
				{
					if (clickCount == 1)
					{
						ToggleCaliperState(grabbedCaliper);
						needsRefresh = true;
					}
				}
				grabbedCaliper = null;
				bar1Grabbed = false;
				bar2Grabbed = false;
				crossbarGrabbed = false;
				caliperWasDragged = false;
			}
			return needsRefresh;
		}

		private void ToggleCaliperState(BaseCaliper c)
		{
			if (c == null)
			{
				return;
			}
			if (c.IsSelected)
			{
				UnselectCaliper(c);
			}
			else
			{
				SelectCaliper(c);
			}
			UnselectCalipersExcept(c);
		}

		public void UnselectCalipersExcept(BaseCaliper c)
		{
			foreach (var caliper in calipers)
			{
				if (caliper != c)
				{
					UnselectCaliper(caliper);
				}
			}
		}

		public int NumberOfCalipers()
		{
			return calipers.Count;
		}

		public bool PointIsNearCaliper(Point p)
		{
			bool pointNearCaliper = false;
			foreach (var c in calipers)
			{
				if (c.PointNearCaliper(p))
				{
					pointNearCaliper = true;
					break;
				}
			}
			return pointNearCaliper;
		}

		public bool UpdateCalibration(double zoomFactor)
		{
			// FIXME: remove if
			//if (HorizontalCalibration.Calibrated ||
			//	VerticalCalibration.Calibrated)
			//{
				HorizontalCalibration.CurrentZoom = zoomFactor;
				VerticalCalibration.CurrentZoom = zoomFactor;
			//}
			return NumberOfCalipers() > 0;
		}

		public BaseCaliper GetLoneTimeCaliper()
		{
			BaseCaliper c = null;
			int n = 0;
			if (calipers.Count > 0)
			{
				foreach (BaseCaliper caliper in calipers)
				{
					if (caliper.Direction == CaliperDirection.Horizontal
						&& !caliper.IsAngleCaliper)
					{
						c = caliper;
						n++;
					}
				}
			}
			if (n == 1)
			{
				return c;

			}
			else
			{
				return null;
			}
		}

		public void UpdatePreferences(Preferences p)
		{
			foreach (BaseCaliper caliper in calipers)
			{
				caliper.LineWidth = p.LineWidth;
				//caliper.UnselectedColor = p.CaliperColor;
				caliper.SelectedColor = AdjustColor(p.HighlightColor);
				if (caliper.IsSelected)
				{
					caliper.CaliperColor = caliper.SelectedColor;
				}
				else
				{
					//caliper.CaliperColor = caliper.UnselectedColor;
				}
				caliper.Rounding = p.RoundingParameter();
				caliper.AutoPositionText = p.AutoPositionText;
				if (caliper.Direction == CaliperDirection.Horizontal)
				{
					caliper.CaliperTextPosition = p.TimeCaliperTextPositionParameter();
				}
				else if (caliper.Direction == CaliperDirection.Vertical)
				{
					caliper.CaliperTextPosition = p.AmplitudeCaliperTextPositionParameter();
				}
			}
		}

		public bool MarchCaliper()
		{
			if (chosenCaliper == null)
			{
				return false;
			}
			chosenCaliper.isMarching = !chosenCaliper.isMarching;
			return true;
		}
	}
}
