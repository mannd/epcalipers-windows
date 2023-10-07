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

namespace EPCalipersWinUI3.Calipers
{
    public enum CaliperType
    {
        Time,
        Amplitude,
        Angle
    }

    public abstract class Caliper
    {
        public static Caliper Create(CaliperType caliperType, Grid grid)
        {
            switch (caliperType)
			{
				case CaliperType.Time:
					return new TimeCaliper(grid);
				case CaliperType.Amplitude:
					return null;
				case CaliperType.Angle:
					return null;
			}
			return null;
		}

		protected abstract CaliperComponent[] GetCaliperComponents(); 

        protected void SetColor(Color color)
        {
            var brush = new SolidColorBrush(color);
            foreach (var component in GetCaliperComponents())
            {
                component.Brush = brush;
            }
        }

        protected void SetBrush(SolidColorBrush brush)
        {
            foreach (var component in GetCaliperComponents())
            {
                component.Brush = brush;
            }
        }

        protected void SetThickness(double thickness)
        {
            foreach (var component in GetCaliperComponents())
            {
                component.Thickness = thickness;
            }
        }

        public abstract double Value();

        public abstract void Draw();

        public abstract void Drag(CaliperComponent component, Point position);

        public abstract CaliperComponent IsNearComponent(Point p);
    }
}
