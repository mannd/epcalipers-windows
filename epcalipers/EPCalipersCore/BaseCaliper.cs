﻿using EPCalipersCore.Properties;
using System;
using System.Drawing;
using System.Windows.Controls;

namespace EPCalipersCore
{
	public enum CaliperComponent { LeftBar, RightBar, CrossBar, LowerBar, UpperBar, Apex, NoComponent };

	public enum MovementDirection { Up, Down, Left, Right, Stationary };

	public class BaseCaliper
	{
		protected float DELTA = 20.0f;
		//private int tmpLineWidth;
		private static float differential = 0.0f;
		// constants for marching calipers
		protected static readonly float minDistanceForMarch = 20f;
		protected static readonly int maxMarchingCalipers = 20;

		protected string roundToIntString = "D";
		protected string roundToFourPlacesString = "G4";
		protected string roundToTenthsString = "F1";
		protected string roundToHundredthsString = "F2";
		protected string noRoundingString = "G";

		private float _bar1Position;
		private float _bar2Position;
		private float _crossbarPosition;

		public float Bar1Position { 
			get {
				System.Diagnostics.Debug.Print("zoom = {0}", CurrentCalibration.CurrentZoom);
				return _bar1Position * (float)CurrentCalibration.CurrentZoom;
			} 
			set { 
				_bar1Position = value / (float)CurrentCalibration.CurrentZoom; 
			}
		}
		public float Bar2Position { 
			get {
				return _bar2Position * (float)CurrentCalibration.CurrentZoom;
			} 
			set { 
				_bar2Position = value / (float)CurrentCalibration.CurrentZoom; 
			}
		}
		public float CrossbarPosition { 
			get {
				return _crossbarPosition * (float)CurrentCalibration.CurrentZoom;
			} 
			set { 
				_crossbarPosition = value / (float)CurrentCalibration.CurrentZoom; 
			}
		}

		public CaliperDirection Direction { set; get; }
		public Color CaliperColor { set; get; }
		public Color UnselectedColor { set; get; }
		public Color SelectedColor { set; get; }
		public int LineWidth { set; get; }
		public float ValueInPoints
		{
			get { return Bar2Position - Bar1Position; }
		}
		public bool IsSelected { set; get; }
		public Calibration CurrentCalibration { set; get; }
		public Font TextFont { set; get; }
		public int TextFontSize { set; get; }
		public bool RoundMsecRate { set; get; }
		public Preferences.Rounding Rounding { set; get; }
		public bool AutoPositionText { set; get; }
		private Preferences.TextPosition _caliperTextPosition;
		virtual public Preferences.TextPosition CaliperTextPosition
		{
			set
			{
				_caliperTextPosition = value;
			}
			get
			{
				return _caliperTextPosition;
			}
		}

		protected bool caliperIsAngleCaliper = false;
		protected bool caliperRequiresCalibration = true;
		protected readonly int defaultFontSize = 12;
		protected readonly int defaultCanvasFontSize = 16;

		public bool isTweaking = false;
		public CaliperComponent ChosenComponent { set; get; } = CaliperComponent.NoComponent;


		public bool isMarching = false;

		// added for AngleCaliper derived class
		public bool RequiresCalibration
		{
			get { return caliperRequiresCalibration; }
		}

		public bool IsAngleCaliper
		{
			get { return caliperIsAngleCaliper; }
		}

		// Shouldn't directly init BaseCaliper
		protected BaseCaliper()
		{
			InitWithDirection(CaliperDirection.Horizontal, 0.0f, 0.0f, 100.0f);
		}

		public void InitWithDirection(
			CaliperDirection direction,
			float bar1Position,
			float bar2Position,
			float crossbarPosition)
		{
			Direction = direction;
			CurrentCalibration = new Calibration();
			Bar1Position = bar1Position;
			Bar2Position = bar2Position;
			CrossbarPosition = crossbarPosition;
			UnselectedColor = Color.Blue;
			SelectedColor = Color.Red;
			CaliperColor = Color.Blue;
			LineWidth = 2;
			IsSelected = false;
			TextFontSize = defaultFontSize;
			TextFont = new Font("Segoe UI", TextFontSize);
			RoundMsecRate = true;
			Rounding = Preferences.Rounding.ToInt;
			AutoPositionText = true;
			_caliperTextPosition = Preferences.TextPosition.Right;
		}

		public virtual void SetInitialPosition()
		{
			// This is better than setting in middle, because caliper can become lost.
			SetInitialPositionNearCorner();
		}

		private void SetInitialPositionNearCorner()
		{
			Bar1Position = 50 + differential;
			Bar2Position = 100 + differential;
			CrossbarPosition = 100 + differential;
			differential += 15.0f;
			if (differential > 80.0f)
			{
				differential = 0.0f;
			}
		}

		public virtual void Draw(Graphics g, RectangleF rect) { }

		public virtual void Draw(Canvas canvas) { }

		public bool IsTimeCaliper()
		{
			return Direction == CaliperDirection.Horizontal && !IsAngleCaliper;
		}

		// returns significant bar coordinate depending on direction of caliper
		public float BarCoord(PointF p)
		{
			return Direction == CaliperDirection.Horizontal ? p.X : p.Y;
		}

		protected virtual string Measurement()
		{
			string s;
			if (CurrentCalibration.unitsAreMsecOrRate())
			{
				string format;
				switch (Rounding)
				{
					case Preferences.Rounding.ToInt:
						format = roundToIntString;
						break;
					case Preferences.Rounding.ToFourPlaces:
						format = roundToFourPlacesString;
						break;
					case Preferences.Rounding.ToTenths:
						format = roundToTenthsString;
						break;
					case Preferences.Rounding.ToHundredths:
						format = roundToHundredthsString;
						break;
					case Preferences.Rounding.None:
						format = noRoundingString;
						break;
					default:
						format = roundToIntString;
						break;
				}
				if (Rounding == Preferences.Rounding.ToInt)
				{
					s = string.Format("{0} {1}", Math.Round(CalibratedResult()),
					CurrentCalibration.Units);
				}
				else
				{
					s = string.Format("{0} {1}", CalibratedResult().ToString(format), CurrentCalibration.Units);
				}
			}
			else
			{
				s = string.Format("{0} {1}", CalibratedResult().ToString("G4"), CurrentCalibration.Units);
			}
			return s;
		}

		private double CalibratedResult()
		{
			double result = IntervalResult();
			if (result != 0.0f && CurrentCalibration.DisplayRate &&
				CurrentCalibration.CanDisplayRate)
			{
				result = RateResult(result);
			}
			return result;
		}

		public virtual double IntervalResult()
		{
			return ValueInPoints * CurrentCalibration.Multiplier;
		}

		private double RateResult(double interval)
		{
			if (interval != 0.0)
			{

				if (CurrentCalibration.UnitsAreMsecs)
				{
					interval = EPCalculator.MsecToBpm(interval);
				}
				else if (CurrentCalibration.UnitsAreSeconds)
				{
					interval = EPCalculator.SecToBpm(interval);
				}
			}
			return interval;
		}

		public double IntervalInSecs(double interval)
		{
			if (CurrentCalibration.UnitsAreSeconds)
			{
				return interval;
			}
			else
			{
				return EPCalculator.MsecToSec(interval);
			}
		}

		private bool PointNearBar(PointF p, float barPosition)
		{
			return BarCoord(p) > barPosition - DELTA && BarCoord(p) < barPosition + DELTA;
		}

		public virtual bool PointNearBar1(PointF p)
		{
			return PointNearBar(p, Bar1Position);
		}

		public virtual bool PointNearBar2(PointF p)
		{
			return PointNearBar(p, Bar2Position);
		}

		public virtual bool PointNearCrossbar(PointF p)
		{
			bool nearBar;
			float delta = DELTA + 5.0f;
			if (Direction == CaliperDirection.Horizontal)
			{
				nearBar = (p.X > Math.Min(Bar1Position, Bar2Position) && p.X < Math.Max(Bar2Position, Bar1Position) && p.Y > CrossbarPosition - delta && p.Y < CrossbarPosition + delta);

			}
			else
			{
				nearBar = (p.Y > Math.Min(Bar1Position, Bar2Position) &&
					p.Y < Math.Max(Bar2Position, Bar1Position) && p.X > CrossbarPosition - delta &&
					p.X < CrossbarPosition + delta);
			}
			return nearBar;
		}

		public bool PointNearCaliper(PointF p)
		{
			return PointNearCrossbar(p) || PointNearBar1(p) ||
				PointNearBar2(p);
		}

		#region Movement

		public virtual void MoveCrossbar(PointF delta)
		{
			Bar1Position += delta.X;
			Bar2Position += delta.X;
			CrossbarPosition += delta.Y;
		}

		public virtual void MoveBar1(PointF delta, PointF location)
		{
			Bar1Position += delta.X;
		}

		public virtual void MoveBar2(PointF delta, PointF location)
		{
			Bar2Position += delta.X;
		}

		#endregion

		#region Tweak

		public static String ComponentName(CaliperComponent component)
		{
			switch (component)
			{
				case CaliperComponent.LeftBar:
					return "left bar";
				case CaliperComponent.RightBar:
					return "right bar";
				case CaliperComponent.CrossBar:
					return "crossbar";
				case CaliperComponent.UpperBar:
					return "upper bar";
				case CaliperComponent.LowerBar:
					return "lower bar";
				case CaliperComponent.Apex:
					return "apex";
				default:
					return "unknown component";
			}
		}

		// NB: Window coordinates origin are upper left corner, like iOS, not like macOS
		public virtual void MoveBarInDirection(MovementDirection movementDirection, float distance, CaliperComponent component)
		{
			if (component == CaliperComponent.NoComponent)
			{
				return;
			}
			CaliperComponent adjustedComponent = MoveCrossbarInsteadOfSideBar(movementDirection, component) ? CaliperComponent.CrossBar : component;
			if (adjustedComponent == CaliperComponent.CrossBar)
			{
				MoveCrossbarInDirection(movementDirection, distance);
				return;
			}

			if (movementDirection == MovementDirection.Up || movementDirection == MovementDirection.Left)
			{
				distance = -distance;
			}
			switch (adjustedComponent)
			{
				case CaliperComponent.LeftBar:
				case CaliperComponent.UpperBar:
					Bar1Position += distance;
					break;
				case CaliperComponent.RightBar:
				case CaliperComponent.LowerBar:
					Bar2Position += distance;
					break;
				default:
					break;
			}
		}

		protected bool MoveCrossbarInsteadOfSideBar(MovementDirection movementDirection, CaliperComponent component)
		{
			if (component == CaliperComponent.CrossBar || component == CaliperComponent.Apex)
			{
				return false;
			}
			return (Direction == CaliperDirection.Horizontal &&
				(movementDirection == MovementDirection.Up || movementDirection == MovementDirection.Down))
				||
				(Direction == CaliperDirection.Vertical &&
				(movementDirection == MovementDirection.Left || movementDirection == MovementDirection.Right));
		}

		private void MoveCrossbarInDirection(MovementDirection movementDirection, float distance)
		{
			if (Direction == CaliperDirection.Vertical)
			{
				movementDirection = SwapDirection(movementDirection);
			}
			switch (movementDirection)
			{
				case MovementDirection.Up:
					CrossbarPosition -= distance;
					break;
				case MovementDirection.Down:
					CrossbarPosition += distance;
					break;
				case MovementDirection.Left:
					Bar1Position -= distance;
					Bar2Position -= distance;
					break;
				case MovementDirection.Right:
					Bar1Position += distance;
					Bar2Position += distance;
					break;
				default:
					break;
			}
		}

		private MovementDirection SwapDirection(MovementDirection movementDirection)
		{
			switch (movementDirection)
			{
				case MovementDirection.Left:
					return MovementDirection.Up;
				case MovementDirection.Right:
					return MovementDirection.Down;
				case MovementDirection.Up:
					return MovementDirection.Left;
				case MovementDirection.Down:
					return MovementDirection.Right;
				default:
					return MovementDirection.Stationary;
			}
		}

		#endregion

	}
}
