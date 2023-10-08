﻿using Microsoft.UI.Input;
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
    public abstract class Caliper
    {
		public static Caliper Create(CaliperType caliperType, Bounds bounds)
        {
            switch (caliperType)
			{
				case CaliperType.Time:
					return new TimeCaliper(bounds);
				case CaliperType.Amplitude:
					return null;
				case CaliperType.Angle:
					return null;
			}
			return null;
		}

		protected abstract CaliperComponent[] GetCaliperComponents(); 

        public void SetColor(Color color)
        {
            foreach (var component in GetCaliperComponents())
            {
                component.Color = color;
            }
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

        public abstract void Drag(CaliperComponent component, Point position);

        public abstract CaliperComponent IsNearComponent(Point p);
    }
}