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
        protected Caliper(Bounds bounds) 
        {
            Bounds = bounds;
        }

		public static Caliper Create(CaliperType caliperType, Bounds bounds, CaliperPosition position)
        {
            switch (caliperType)
			{
				case CaliperType.Time:
					return new TimeCaliper(bounds, position);
				case CaliperType.Amplitude:
					return new AmplitudeCaliper(bounds, position);
				case CaliperType.Angle:
					return null;
			}
			return null;
		}

        public CaliperType CaliperType { get; init; }

        protected CaliperComponent[] CaliperComponents { get; init; }

		protected  CaliperComponent[] GetCaliperComponents()
        {
            return CaliperComponents;
        }

        public void SetColor(Color color)
        {
            foreach (var component in GetCaliperComponents())
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
                foreach (var component in GetCaliperComponents())
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
            foreach (var component in GetCaliperComponents())
            {
                component.Width = thickness;
            }
        }

        public abstract double Value();

        public abstract void Add(Grid grid);

        public abstract void Delete(Grid grid);

        public abstract void Drag(CaliperComponent component, Point delta);

        public abstract CaliperComponent IsNearComponent(Point p);
    }
}
