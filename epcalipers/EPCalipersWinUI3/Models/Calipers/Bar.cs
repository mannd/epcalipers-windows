﻿using EPCalipersWinUI3.Contracts;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Devices;
using Windows.UI;
using EPCalipersWinUI3.Helpers;
using Color = Windows.UI.Color;
using Point = Windows.Foundation.Point;
using ABI.Microsoft.UI.Xaml;
using Microsoft.UI.Xaml;
using Xunit;

namespace EPCalipersWinUI3.Models.Calipers
{

    /// <summary>
    /// One of the bars of a caliper.
    /// </summary>
    public sealed class Bar
    {
        /// <summary>
        /// The direction and function of the Bar in a Caliper.
        /// </summary>
		public enum Role
        {
            Horizontal,
            Vertical,
            HorizontalCrossBar,
            VerticalCrossBar,
            LeftAngle,
            RightAngle,
            Apex,  // Invisible bar at apex of angle caliper
            TriangleBase // For BrugadaMeter.
        }
        /// <summary>
        /// This is a graphics object that is drawable, just a Line but can substitute
        /// an ILine stub for testing.
        /// </summary>
        public Role BarRole { get; set; }
        public Bounds Bounds { get; set; }

        public Color SelectedColor { get; set; }
        public Color UnselectedColor { get; set; }

        private Line _line;

        // Angle is only used for angle calipers.
        public double Angle { get; set; }
        public double Position
        {
            get
            {
                switch (BarRole)
                {
                    case Role.Horizontal:
                    case Role.HorizontalCrossBar:
                    case Role.Apex:
                        return Y1;
                    case Role.Vertical:
                    case Role.VerticalCrossBar:
                        return X1;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (BarRole)
                {
                    case Role.Horizontal:
                    case Role.HorizontalCrossBar:
                    case Role.Apex:
                        Y1 = value; Y2 = value;
                        break;
                    case Role.Vertical:
                    case Role.VerticalCrossBar:
                        X1 = value; X2 = value;
                        break;
                    default:
                        break;
                }
            }
        }
        public double X1
        {
            get => _x1;
            set
            {
                _x1 = value;
                if (_line != null) _line.X1 = value;
            }
        }
        private double _x1;
        public double Y1
        {
            get => _y1;
            set
            {
                _y1 = value;
                if (_line != null) _line.Y1 = value;
            }
        }
        private double _y1;
        public double X2
        {
            get => _x2;
            set
            {
                _x2 = value;
                if (_line != null) _line.X2 = value;
            }
        }
        private double _x2;
        public double Y2
        {
            get => _y2;
            set
            {
                _y2 = value;
                if (_line != null) _line.Y2 = value;
            }
        }
        private double _y2;

        private readonly double _precision = 10; // Used to determine if touches are nearby.

        public Bar(Role role,
            double position, double start, double end, bool fakeUI = false)
        {
            Debug.Assert(role != Role.LeftAngle && role != Role.RightAngle,
                "Angle bar passed to non-angle bar constructor.");
            BarRole = role;
            _line = fakeUI ? null : new Line();
            SetBarPosition(position, start, end);
        }

        public Bar(Role role, Point apex, double angle, Bounds bounds, bool fakeUI = false)
        {

            Debug.Assert(role == Role.LeftAngle || role == Role.RightAngle || role == Role.Apex,
                "Non-angle bar passed to angle bar constructor.");
            BarRole = role;
            _line = fakeUI ? null : new Line();
            Bounds = bounds;
            if (role == Role.Apex)
            {
                SetBarPosition(apex.Y, apex.X - 10, apex.X + 10);
                // Comment out this line to show hidden ApexBar for debugging.
                if (_line != null) _line.Visibility = Visibility.Collapsed;
            }
            else
            {
                SetAngleBarPosition(apex, angle);
            }
        }

        private void SetBarPosition(double position, double start, double end)
        {
            switch (BarRole)
            {
                case Role.Horizontal:
                    X1 = 0;
                    Y1 = position;
                    X2 = end;
                    Y2 = position;
                    break;
                case Role.Vertical:
                    X1 = position;
                    Y1 = 0;
                    X2 = position;
                    Y2 = end;
                    break;
                case Role.HorizontalCrossBar:
                    X1 = start;
                    Y1 = position;
                    X2 = end;
                    Y2 = position;
                    break;
                case Role.VerticalCrossBar:
                    X1 = position;
                    Y1 = start;
                    X2 = position;
                    Y2 = end;
                    break;
                case Role.Apex:
                    X1 = start;
                    Y1 = position;
                    X2 = end;
                    Y2 = position;
                    break;
                case Role.LeftAngle:
                case Role.RightAngle:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public void SetAngleBarPosition(Point apex, double angle)
        {
            var length = 2 * Math.Max(Bounds.Height, Bounds.Width);
            var adjustedEndPoint = ClippedEndPoint(apex, angle, length, new Point(0, Bounds.Height), new Point(Bounds.Width, Bounds.Height));
            X1 = apex.X;
            Y1 = apex.Y;
            X2 = adjustedEndPoint.X;
            Y2 = adjustedEndPoint.Y;
        }

        private static Point? AdjustEndPoint(Point apex, Point endPoint, Point border1, Point border2)
        {
            var intersection = MathHelper.Intersection(apex, endPoint, border1, border2);
            return intersection;
        }

        private Point ClippedEndPoint(Point apex, double angle, double length, Point lowerBorder, Point rightBorder)
        {
            var endPoint = EndPointForPosition(apex, angle, length);
            var adjustedEndPoint = AdjustEndPoint(apex, endPoint, lowerBorder, rightBorder) ?? endPoint;
            adjustedEndPoint = AdjustEndPoint(apex, adjustedEndPoint, new Point(0, 0), lowerBorder) ?? adjustedEndPoint;
            adjustedEndPoint = AdjustEndPoint(apex, adjustedEndPoint, new Point(0, 0), new Point(rightBorder.X, 0)) ?? adjustedEndPoint;
            adjustedEndPoint = AdjustEndPoint(apex, adjustedEndPoint, new Point(rightBorder.X, 0), rightBorder) ?? adjustedEndPoint;
            return adjustedEndPoint;
        }

        public Point MidPoint => new Point((X2 - X1) / 2.0 + X1, (Y2 - Y1) / 2.0 + Y1);

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                if (_line != null)
                {
                    Color = _isSelected ? SelectedColor : UnselectedColor;
                }
            }


        }
        private bool _isSelected;
        public Color Color
        {
            set
            {
                if (_line != null)
                {
                    var brush = new SolidColorBrush(value);
                    _line.Stroke = brush;
                }
            }
        }
        public double Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                if (_line != null) _line.StrokeThickness = value;
            }
        }
        private double _thickness;

        public bool IsNear(Point p)
        {
            switch (BarRole)
            {
                case Role.Horizontal:
                    return p.Y > Y1 - _precision && p.Y < Y1 + _precision;
                case Role.Vertical:
                    return p.X > X1 - _precision && p.X < X1 + _precision;
                case Role.HorizontalCrossBar:
                case Role.Apex:
                    return p.X > Math.Min(X1, X2)
                            && p.X < Math.Max(X1, X2)
                            && p.Y > Y1 - 3 * _precision // give more space at top of apex (where label is)
                            && p.Y < Y1 + _precision;
                case Role.VerticalCrossBar:
                    return p.Y > Math.Min(Y1, Y2)
                            && p.Y < Math.Max(Y1, Y2)
                            && p.X > X1 - _precision
                            && p.X < X1 + _precision;
                default: return false;
            }
        }

        public void AddToView(ICaliperView view)
        {
            if (view == null) return;
            view.Add(_line);
        }

        public void RemoveFromView(ICaliperView view)
        {
            if (view == null) return;
            view.Remove(_line);
        }

        private Point EndPointForPosition(Point p, double angle, double length)
        {
            // Note Windows coordinates origin is top left of screen
            double endX = Math.Cos(angle) * length + p.X;
            double endY = Math.Sin(angle) * length + p.Y;
            Point endPoint = new Point(endX, endY);
            return endPoint;
        }

    }
}