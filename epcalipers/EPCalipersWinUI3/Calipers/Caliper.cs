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
    /// A caliper's position is defined by the positions of the 3 components.
    /// The Center position is the crossbar, and First and Last are Left and Right
    /// for Time calipers, and Top and Bottom for Amplitude calipers.
    /// Angle calipers may need a different structure.
    /// </summary>
    /// <param name="Center"></param>
    /// <param name="First"></param>
    /// <param name="Last"></param>
    public readonly record struct CaliperPosition(double Center, double First, double Last);
    public abstract class Caliper
    {
        protected Bounds Bounds { get; init; }
        protected ICaliperView CaliperView { get; init; }

        protected Caliper(ICaliperView caliperView) 
        {
            CaliperView = caliperView;
            Bounds = caliperView.Bounds;
        }

		public static Caliper Create(CaliperType caliperType, CaliperPosition position, ICaliperView caliperView)
        {
            switch (caliperType)
			{
				case CaliperType.Time:
					return new TimeCaliper(position, caliperView);
				case CaliperType.Amplitude:
					return new AmplitudeCaliper(position, caliperView);
				case CaliperType.Angle:
					return null;
			}
			return null;
		}

        public CaliperType CaliperType { get; init; }

        protected Bar[] CaliperComponents { get; init; }

        public void SetColor(Color color)
        {
            foreach (var component in CaliperComponents)
            {
                component.Color = color;
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
                foreach (var component in CaliperComponents)
                {
                    component.IsSelected = value;
                    component.Color = component.IsSelected ? SelectedColor : UnselectedColor;
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
            foreach (var component in CaliperComponents)
            {
                component.Width = thickness;
            }
        }

        public abstract double Value();

        public void Add(ICaliperView caliperView)
        {
            if (caliperView == null) return;
			foreach (var bar in CaliperComponents) caliperView.Add(bar.GetBarLine());
        }

        public void Remove(ICaliperView caliperView)
        {
            if (caliperView == null) return;
			foreach (var bar in CaliperComponents) caliperView.Remove(bar.GetBarLine());
        }

        public abstract void Drag(Bar component, Point delta);

        public abstract Bar IsNearComponent(Point p);
    }
}
