﻿using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace EPCalipersWinUI3.Models.Calipers
{
    public class AngleCaliper : Caliper
    {
        public Bar LeftAngleBar { get; set; }
        public Bar RightAngleBar { get; set; }
        public Bar ApexBar { get; set; } // a pseudobar
        public Bar TriangleBaseBar { get; set; }

        public Calibration TimeCalibration { get; set; }
        public Calibration AmplitudeCalibration { get; set; }

        private ISettings _settings;

        public AngleCaliper(AngleCaliperPosition position,
            ICaliperView caliperView,
            ISettings settings,
            Calibration timeCalibration = null,
            Calibration amplitudeCalibration = null,
            bool fakeUI = false) : base(caliperView)
        {
            _fakeUI = fakeUI;
            _settings = settings;
            Calibration = new AngleCalibration();
            TimeCalibration = timeCalibration ?? Calibration.Uncalibrated;
            AmplitudeCalibration = amplitudeCalibration ?? Calibration.Uncalibrated;
            Bars = InitBars(position);
            InitCaliperLabel();
            CaliperType = CaliperType.Angle;
        }

        private List<Bar> InitBars(AngleCaliperPosition position)
        {
            LeftAngleBar = new Bar(Bar.Role.LeftAngle, position.Apex, position.FirstAngle, Bounds, _fakeUI);
            LeftAngleBar.Angle = position.FirstAngle;
            RightAngleBar = new Bar(Bar.Role.RightAngle, position.Apex, position.LastAngle, Bounds, _fakeUI);
            RightAngleBar.Angle = position.LastAngle;
			ApexBar = new Bar(Bar.Role.Apex, position.Apex, 0, Bounds, _fakeUI); // ApexBar never drawn
			InitTriangleBase(TriangleHeight());
			TriangleBaseBar.Visibility = ShowBrugadaTriangle(position) ?
				Microsoft.UI.Xaml.Visibility.Visible :
				Microsoft.UI.Xaml.Visibility.Collapsed;
			return new List<Bar> { LeftAngleBar, RightAngleBar, ApexBar, TriangleBaseBar };
		}

		private double TriangleHeight()
        {
            if (AmplitudeCalibration != null && AmplitudeCalibration.Multiplier != 0)
            {
                double pointsPerMM = 1.0 / AmplitudeCalibration.Multiplier;
                return 5.0 * pointsPerMM;
            }
            return 0;
            // TODO: is it ok to return 0?
		}

		private bool ShowBrugadaTriangle(AngleCaliperPosition position)
        {
            return (_settings.ShowBrugadaTriangle &&
                TimeCalibration.Parameters.Unit == CalibrationUnit.Msec &&
                AmplitudeCalibration.Parameters.Unit == CalibrationUnit.Mm &&
                AngleInSouthernHemisphere(position.FirstAngle) &&
                AngleInSouthernHemisphere(position.LastAngle));
        }

		private bool AngleInSouthernHemisphere(double angle)
		{
			// Note can't be <= because we get divide by zero error with Sin(angle) == 0
			return (0 < angle && angle < Math.PI);
		}

		private Point GetBasePoint1ForHeight(double height)
		{
			// Dangerous possible divide by zero here
			double pointY = ApexBar.Position + height;
			double pointX = height * (Math.Sin(LeftAngleBar.Angle - Math.PI / 2)
				/ Math.Sin(Math.PI - LeftAngleBar.Angle));
            double apexX = ApexBar.MidPoint.X;
			pointX =  apexX - pointX;
			Point point = new Point(pointX, pointY);
			return point;
		}

		private Point GetBasePoint2ForHeight(double height)
		{
			// Dangerous possible divide by zero here
			double pointY = ApexBar.Position + height;
			double pointX = height * (Math.Sin(Math.PI / 2 - RightAngleBar.Angle)
				/ Math.Sin(RightAngleBar.Angle));
            double apexX = ApexBar.MidPoint.X;
            pointX += apexX;
			Point point = new Point(pointX, pointY);
			return point;
		}

        private void InitTriangleBase(double height)
        {
            Point point1 = GetBasePoint1ForHeight(height);
            Point point2 = GetBasePoint2ForHeight(height);
            double position = point1.Y;
            TriangleBaseBar = new Bar(Bar.Role.TriangleBase, position, point1.X, point2.X, _fakeUI);
            double baseValue = point2.X - point1.X;
            Debug.Print(Calibration.GetSecondaryText(baseValue, TimeCalibration.Parameters.UnitString));
        }

        private void InitCaliperLabel()
        {
            string text = Calibration.GetText(Value);
            CaliperLabel = new AngleCaliperLabel(this, CaliperView, text,
                CaliperLabelAlignment.Right, false, _fakeUI);
        }

        public override void ChangeBounds()
        {
            var bounds = CaliperView.Bounds;
            LeftAngleBar.Bounds = bounds;
            RightAngleBar.Bounds = bounds;
            LeftAngleBar.SetAngleBarPosition(new Point(LeftAngleBar.X1, LeftAngleBar.Y1), LeftAngleBar.Angle);
            RightAngleBar.SetAngleBarPosition(new Point(RightAngleBar.X1, RightAngleBar.Y1), RightAngleBar.Angle);
        }

        // TODO: control drag from keyboard too
        public override void Drag(Bar bar, Point delta, Point location)
		{
			switch (bar.BarRole)
			{
				case Bar.Role.Apex:
					bar.X1 += delta.X;
					bar.X2 += delta.X;
					bar.Y1 += delta.Y;
					bar.Y2 += delta.Y;
					var apex = bar.MidPoint;
					LeftAngleBar.SetAngleBarPosition(apex, LeftAngleBar.Angle);
					RightAngleBar.SetAngleBarPosition(apex, RightAngleBar.Angle);
					break;
				case Bar.Role.LeftAngle:
					LeftAngleBar.Angle = RelativeTheta(location);
					LeftAngleBar.SetAngleBarPosition(new Point(LeftAngleBar.X1, LeftAngleBar.Y1), LeftAngleBar.Angle);
					break;
				case Bar.Role.RightAngle:
					RightAngleBar.Angle = RelativeTheta(location);
					RightAngleBar.SetAngleBarPosition(new Point(RightAngleBar.X1, RightAngleBar.Y1), RightAngleBar.Angle);
					break;
				default: break;
			}
			DrawTriangleBase();
			CaliperLabel.Text = Calibration.GetText(Value);
			CaliperLabel.SetPosition();
		}

		public void DrawTriangleBase()
		{
			if (ShowBrugadaTriangle(new AngleCaliperPosition(ApexBar.MidPoint, LeftAngleBar.Angle, RightAngleBar.Angle)))
			{
				// Triangle base needs redrawing no matter how angle caliper moves.
				TriangleBaseBar.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
				double height = TriangleHeight();
				Point point1 = GetBasePoint1ForHeight(height);
				Point point2 = GetBasePoint2ForHeight(height);
				double position = point1.Y;
				TriangleBaseBar.X1 = point1.X;
				TriangleBaseBar.Y1 = position;
				TriangleBaseBar.X2 = point2.X;
				TriangleBaseBar.Y2 = position;
				double baseValue = point2.X - point1.X;
				Debug.Print(Calibration.GetSecondaryText(baseValue, TimeCalibration.Parameters.UnitString));
			}
			else
			{
				TriangleBaseBar.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
			}
		}

		public override Bar IsNearBar(Point p)
        {
            if (ApexBar.IsNear(p))
            {
                Debug.Print("near apex");
                return ApexBar;
            }
            if (PointNearBar(p, LeftAngleBar.Angle))
            {
                Debug.Print("near left angle bar");
                return LeftAngleBar;
            }
            if (PointNearBar(p, RightAngleBar.Angle))
            {
                Debug.Print("near right angle bar");
                return RightAngleBar;
            }
            Debug.Print("not near any bar");
            return null;
        }

        private readonly double angleDelta = 0.1;

        public bool PointNearBar(Point p, double barAngle)
        {
            double theta = RelativeTheta(p);
            return theta < (double)barAngle + angleDelta &&
                theta > (double)barAngle - angleDelta;
        }

        private double RelativeTheta(Point p)
        {
            var x = p.X - LeftAngleBar.X1;
            var y = p.Y - LeftAngleBar.Y1;
            return Math.Atan2(y, x);
        }

        public override double Value => MathHelper.RadiansToDegrees(LeftAngleBar.Angle - RightAngleBar.Angle);
    }
}
