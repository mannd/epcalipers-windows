using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;

namespace EPCalipersWinUI3.Models.Calipers
{
	/// <summary>
	/// Indicates whether a caliper is fully, partially, or not selected.
	/// A fully selected caliper has all bars selected.  A partially selected caliper has exactly one
	/// bar selected.
	/// </summary>
	public enum CaliperSelection
	{
		Full,
		Partial,
		None
	}

	/// <summary>
	/// Indicates whether a caliper measures time, amplitude, or angles.
	/// </summary>
	public enum CaliperType
	{
		Time,
		Amplitude,
		Angle,
		None
	}

	public readonly record struct Bounds(double Width, double Height);

	/// <summary>
	/// A caliper's position is defined by the positions of the 3 bars.
	/// The Center position is the crossbar, and First and Last are Left and Right
	/// for Time calipers, and Top and Bottom for Amplitude calipers.
	/// Angle calipers may need a different structure.
	/// </summary>
	/// <param name="Center"></param>
	/// <param name="First"></param>
	/// <param name="Last"></param>
	public readonly record struct CaliperPosition(double Center, double First, double Last);
	public readonly record struct AngleCaliperPosition(Point Apex, double FirstAngle, double LastAngle);
	public sealed class NumberOutOfRangeException : Exception
	{
		public NumberOutOfRangeException() : base("NumberOutOfRangeException") { }
	}

	public abstract class Caliper : INotifyPropertyChanged
	{
		private const double _defaultCaliperValue = 200;
		private static readonly int _maxNumberIntervals = 10;
		protected bool _fakeUI;  // Is true for testing.
		protected Bounds Bounds => CaliperView.Bounds;

		protected Caliper(ICaliperView caliperView, Calibration calibration = null)
		{
			CaliperView = caliperView;
			Calibration = calibration ?? Calibration.Uncalibrated;
		}
			
		public event PropertyChangedEventHandler PropertyChanged;

		public CaliperSelection Selection
		{
			get => _selection;
			set
			{
				if (_selection != value)
				{
					_selection = value;
				}
				OnPropertyChanged(nameof(Selection));
			}
		}
		private CaliperSelection _selection = CaliperSelection.None;

		public CaliperType CaliperType { get; init; }
		public CaliperLabel CaliperLabel { get; set; }
		public bool ShowRate { get; set; } = false;
		public virtual bool IsSelected => Selection != CaliperSelection.None;
		public Bar SelectedBar
		{
			get
			{
				switch (Selection)
				{
					case CaliperSelection.None:
						return null;
					case CaliperSelection.Full:
						return HandleBar;
					case CaliperSelection.Partial:
						return GetFirstSelectedBar();
					default:
						return null;
				}
			}
		}

		/// <summary>
		/// The HandleBar for a Caliper is the Bar that moves the caliper as a unit.
		/// </summary>
		public abstract Bar HandleBar { get; }

		/// <summary>
		/// The Position of a Caliper is the midpoint of the HandleBar.
		/// </summary>
		public Point Position => HandleBar.MidPoint;

		public Calibration Calibration { get; set; }

		/// <summary>
		/// The raw measurement of a caliper, in points, or in degrees for angle calipers.
		/// </summary>
		public abstract double Value { get; }

		public virtual string Text => Calibration.GetFormattedMeasurement(Value, ShowRate);

		public string LabelText
		{
			get
			{
				return _labelText;
			}
			set
			{
				_labelText = value;
				OnPropertyChanged(nameof(LabelText));
			}
		}
		private string _labelText;

		public double BarThickness
		{
			set
			{
				foreach (var bar in Bars)
				{
					bar.Thickness = value;
				}
			}
		}

		public virtual Color Color
		{
			get => _color;
			set
			{
				_color = value;
				foreach (var bar in Bars)
				{
					bar.Color = _color;
				}
				CaliperLabel.Color = _color;
			}
		}
		private Color _color;

		public virtual Color UnselectedColor
		{
			get => _unselectedColor;
			set
			{
				_unselectedColor = value;
				foreach (var bar in Bars)
				{
					bar.UnselectedColor = value;
				}
				CaliperLabel.UnselectedColor = value;
			}
		}
		private Color _unselectedColor;
		public virtual Color SelectedColor
		{
			get => _selectedColor;
			set
			{
				_selectedColor = value;
				foreach (var bar in Bars)
				{
					bar.SelectedColor = value;
				}
				CaliperLabel.SelectedColor = value;
			}
		}
		private Color _selectedColor;

		public ICaliperView CaliperView { get; init; }

		protected List<Bar> Bars { get; init; }

		public static Caliper InitCaliper(CaliperType type,
			ICaliperView caliperView,
			ISettings settings,
			Calibration timeCalibration = null,
			Calibration amplitudeCalibration = null,
			AngleCalibration angleCalibration = null)
		{
			Debug.Assert(type != CaliperType.None);
			CaliperPosition initialPosition;
			AngleCaliperPosition initialAnglePosition;
			Caliper caliper = null;
			switch (type)
			{
				case CaliperType.Time:
					initialPosition = SetInitialCaliperPosition(type, _defaultCaliperValue, caliperView);
					caliper = new TimeCaliper(initialPosition, caliperView, settings, calibration: timeCalibration);
					break;
				case CaliperType.Amplitude:
					initialPosition = SetInitialCaliperPosition(type, _defaultCaliperValue, caliperView);
					caliper = new AmplitudeCaliper(initialPosition, caliperView, settings, calibration: amplitudeCalibration);
					break;
				case CaliperType.Angle:
					initialAnglePosition = SetInitialAngleCaliperPosition(caliperView);
					caliper = new AngleCaliper(initialAnglePosition, caliperView, settings, angleCalibration);
					break;
			}
			ApplySettings(caliper, settings);
			return caliper;
		}

		public static double MeanInterval(double interval, int number)
		{
			if (number < 1 || number > _maxNumberIntervals)
			{
				throw new NumberOutOfRangeException();
			}
			return MathHelper.MeanInterval(interval, number);
		}

		public virtual void SelectFullCaliper()
		{
			SetFullSelectionTo(true);
		}
		public virtual void UnselectFullCaliper()
		{
			SetFullSelectionTo(false);
		}

		public virtual void SetFullSelectionTo(bool value)
		{
			Bars.ForEach(bar => bar.IsSelected = value);
			CaliperLabel.IsSelected = value;
			Selection = value ? CaliperSelection.Full : CaliperSelection.None;
		}

		public virtual void SelectPartialCaliper(Bar bar)
		{
			Bars.ForEach(bar => bar.IsSelected = false);
			bar.IsSelected = true;
			CaliperLabel.IsSelected = true; // ? keep label selected with partial selections?
			Selection = CaliperSelection.Partial;
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private Bar GetFirstSelectedBar()
		{
			foreach (var bar in Bars)
			{
				if (bar.IsSelected) return bar;
			}
			return null;
		}

		public virtual void ApplySettings(ISettings settings)
		{
			BarThickness = settings.BarThickness;
			SelectedColor = settings.SelectedCaliperColor;

			if (Selection == CaliperSelection.Full)
			{
				Color = SelectedColor;
				// TODO: setting to change all unselected colors too?
				// If selected, all unselected colors are changed.
			}
			else if (Selection == CaliperSelection.Partial)
			{
				foreach(var bar in Bars)
				{
					if (bar.IsSelected) 
					{
						bar.Color = SelectedColor;
					}
				}
			}
			UpdateLabel();
		}

		public virtual void AddToView(ICaliperView caliperView)
		{
			if (caliperView == null) return;
			foreach (var bar in Bars) bar?.AddToView(caliperView);
			CaliperLabel.AddToView(caliperView);
		}

		public virtual void Remove(ICaliperView caliperView)
		{
			Selection = CaliperSelection.None;
			if (caliperView == null) return;
			foreach (var bar in Bars) bar?.RemoveFromView(caliperView);
			CaliperLabel?.RemoveFromView(caliperView);
		}

		public abstract void ChangeBounds();

		public abstract void Drag(Bar bar, Point delta, Point previousPoint);

		public abstract Bar IsNearBar(Point p);

		public void UpdateLabel()
		{
			CaliperLabel.Text = Text;
			LabelText = Text;
			CaliperLabel.SetPosition();
		}

		private static void ApplySettings(Caliper c, ISettings settings)
		{
			if (c == null) return;
			c.UnselectedColor = settings.UnselectedCaliperColor;
			c.SelectedColor = settings.SelectedCaliperColor;
			c.BarThickness = settings.BarThickness;
			c.UnselectFullCaliper();
		}

		private static CaliperPosition SetInitialCaliperPosition(CaliperType type, double spacing, ICaliperView caliperView)
		{
			Point p = caliperView.GetOffsettedCenter();
			double halfSpacing = spacing / 2.0;
			switch (type)
			{
				case CaliperType.Time:
					return new CaliperPosition(p.Y, p.X - halfSpacing, p.X + halfSpacing);
				case CaliperType.Amplitude:
					return new CaliperPosition(p.X, p.Y - halfSpacing, p.Y + halfSpacing);
				default:
					return new CaliperPosition(0, 0, 0);
			}
		}

		private static AngleCaliperPosition SetInitialAngleCaliperPosition(ICaliperView caliperView)
		{
			var apex = caliperView.GetOffsettedCenter();
			double firstAngle = 0.5 * Math.PI;
			double secondAngle = 0.25 * Math.PI;
			return new AngleCaliperPosition(apex, firstAngle, secondAngle);
		}
	}
}
