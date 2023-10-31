using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using EPCalipersWinUI3.Contracts;

namespace EPCalipersWinUI3.Calipers
{
    public enum CaliperType
    {
        Time,
        Amplitude,
        Angle
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
        protected Bounds Bounds { get; init; }
        protected ICaliperView CaliperView { get; init; }

        protected Caliper(ICaliperView caliperView) 
        {
            CaliperView = caliperView;
            Bounds = caliperView.Bounds;
        }

        public CaliperType CaliperType { get; init; }

        protected Bar[] Bars { get; init; }
		public CaliperLabel CaliperLabel { get; set; }

        public Color Color
        {
            get => _color;
			set
			{
				_color = value;
				foreach (var bar in Bars)
				{
					bar.Color = _color;
				}
			}
		}
		private Color _color;

        public Color UnselectedColor {  get; set; }
        public Color SelectedColor { get; set; }
		public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                foreach (var bar in Bars)
                {
                    if (bar.BarRole == Bar.Role.Apex) break;
                    bar.IsSelected = value;
                    bar.Color = bar.IsSelected ? SelectedColor : UnselectedColor;
				}
                if (CaliperLabel != null)
				{
					CaliperLabel.TextBlock.Foreground = new SolidColorBrush(_isSelected ? SelectedColor : UnselectedColor);
				}
			}
		}
		private bool _isSelected = false;

        public void ToggleIsSelected()
        {
            IsSelected = !IsSelected;
        }

        protected void SetThickness(double thickness)
        {
            foreach (var bar in Bars)
            {
                bar.Width = thickness;
            }
        }

        /// <summary>
        /// The raw measurement of a caliper, in points, or in degrees or angle calipers.
        /// </summary>
        public abstract double Value { get; }

        public void Add(ICaliperView caliperView)
        {
            if (caliperView == null) return;
			foreach (var bar in Bars) caliperView.Add(bar.Line());
        }

        public void Remove(ICaliperView caliperView)
        {
            if (caliperView == null) return;
			foreach (var bar in Bars) caliperView.Remove(bar.Line());
        }

        public abstract void ChangeBounds();

        public abstract void Drag(Bar bar, Point delta, Point previousPoint);

        public abstract Bar IsNearBar(Point p);

        //Standard placement of new calipers

        public static Caliper InitCaliper(CaliperType type, ICaliperView caliperView) {
            CaliperPosition initialPosition;
            AngleCaliperPosition initialAnglePosition;
            Caliper caliper = null;
            switch (type)
            {
                case CaliperType.Time:
                    initialPosition = SetInitialCaliperPosition(type, 200, caliperView);
                    caliper = new TimeCaliper(initialPosition, caliperView);
                    break;
                case CaliperType.Amplitude:
                    initialPosition = SetInitialCaliperPosition(type, 200, caliperView);
                    caliper = new AmplitudeCaliper(initialPosition, caliperView);
                    break;
                case CaliperType.Angle:
                    initialAnglePosition = SetInitialAngleCaliperPosition(caliperView);
                    caliper = new AngleCaliper(initialAnglePosition, caliperView);
                    break;
            }
            return caliper;
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
