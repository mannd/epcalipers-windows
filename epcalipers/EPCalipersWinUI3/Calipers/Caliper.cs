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

        public void SetColor(Color color)
        {
            foreach (var bar in Bars)
            {
                bar.Color = color;
            }
        }

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

        public abstract double Value();

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

    }
}
