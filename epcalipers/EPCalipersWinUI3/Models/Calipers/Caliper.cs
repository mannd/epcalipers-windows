using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;

namespace EPCalipersWinUI3.Models.Calipers
{
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
    public abstract class Caliper
    {
        private const double _defaultCaliperValue = 200;
        protected bool _fakeUI;  // Is true for testing.

        protected Bounds Bounds => CaliperView.Bounds;

        protected ICaliperView CaliperView { get; init; }

        protected Caliper(ICaliperView caliperView, Calibration calibration = null)
        {
            CaliperView = caliperView;
			Calibration = calibration ?? Calibration.Uncalibrated;
		}

		public CaliperType CaliperType { get; init; }

        public bool ShowRate { get; set; } = false;

        protected List<Bar> Bars { get; init; }
        public CaliperLabel CaliperLabel { get; set; }

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
        public virtual bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                foreach (var bar in Bars)
                {
                    bar.IsSelected = value;
                }
                CaliperLabel.IsSelected = value;
            }
        }
        private bool _isSelected = false;

        public Calibration Calibration { get; set; }
        //public Calibration SecondaryCalibration { get; set; } = Calibration.Uncalibrated;

        public string Text => Calibration.GetText(Value, ShowRate);

        public void ToggleIsSelected()
        {
            IsSelected = !IsSelected;
        }

        public void RefreshColor()
        {
            foreach (var bar in Bars)
            {
                //bar.RefreshColor(); 
            }
        }

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

        public virtual void ApplySettings(ISettings settings)
        {
            BarThickness = settings.BarThickness;
            SelectedColor = settings.SelectedCaliperColor;
            
            if (IsSelected)
            {
                Color = SelectedColor;
                // TODO: setting to change all unselected colors too?
                // If selected, all unselected colors are changed.
            }
        }

        /// <summary>
        /// The raw measurement of a caliper, in points, or in degrees for angle calipers.
        /// </summary>
        public abstract double Value { get; }

        public virtual void Add(ICaliperView caliperView)
        {
            if (caliperView == null) return;
            foreach (var bar in Bars) bar?.AddToView(caliperView);
            CaliperLabel.AddToView(caliperView);
        }

        public virtual void Remove(ICaliperView caliperView)
        {
            if (caliperView == null) return;
            foreach (var bar in Bars) bar?.RemoveFromView(caliperView);
            CaliperLabel.RemoveFromView(caliperView);
        }

        public abstract void ChangeBounds();

        public abstract void Drag(Bar bar, Point delta, Point previousPoint);

        public abstract Bar IsNearBar(Point p);

        //Standard placement of new calipers

        public static Caliper InitCaliper(CaliperType type, 
            ICaliperView caliperView, 
            ISettings settings,
            Calibration timeCalibration = null,
            Calibration amplitudeCalibration = null)
        {
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
                    caliper = new AngleCaliper(initialAnglePosition, caliperView, settings, timeCalibration, amplitudeCalibration);
                    break;
            }
            ApplySettings(caliper, settings);
            return caliper;
        }

        private static void ApplySettings(Caliper c, ISettings settings)
        {
            if (c == null) return;
            c.UnselectedColor = settings.UnselectedCaliperColor;
            c.SelectedColor = settings.SelectedCaliperColor;
            c.BarThickness = settings.BarThickness;
            c.IsSelected = false;
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

        public virtual void ClearCalibration()
        {
            Debug.Print("clearing calibration");
        }

        public void UpdateLabel()
        {
            CaliperLabel.Text = Text;
            CaliperLabel.SetPosition();
        }
    }
}
