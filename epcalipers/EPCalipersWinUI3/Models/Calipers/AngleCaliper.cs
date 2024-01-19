using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;

namespace EPCalipersWinUI3.Models.Calipers
{
	public class AngleCaliper : Caliper
    {
        public Bar LeftAngleBar { get; set; }
        public Bar RightAngleBar { get; set; }
        public Bar ApexBar { get; set; } // Not visible, but present so it can be grabbed.
        public Bar TriangleBaseBar { get; set; } // Only visible under certain conditions.

        public override Bar HandleBar => ApexBar;

        public override string Text => AngleCalibration.GetText(Value);

		public TriangleBaseLabel TriangleBaseLabel { get; set; } // Visible when Brugada triangle visible.

        public AngleCalibration AngleCalibration { get; set; }

        public double LeftMostBarPosition => Math.Min(TriangleBaseBar.X1, TriangleBaseBar.X2);
        public double RightMostBarPosition => Math.Max(TriangleBaseBar.X1, TriangleBaseBar.X2);
        public double TriangleBaseValue
        {
            get
            {
                if (ShowBrugadaTriangle(CaliperPosition))
                {
                    return TriangleBaseBar.X2 - TriangleBaseBar.X1;

                }
                else { return 0; }
            }
        }
        // Current position of the angle caliper.
        public AngleCaliperPosition CaliperPosition => 
            new(ApexBar.MidPoint, LeftAngleBar.Angle, RightAngleBar.Angle);

        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                TriangleBaseLabel.IsSelected = value;
            }
        }
        public override Color UnselectedColor
        {
            get => base.UnselectedColor;
            set
            {
                base.UnselectedColor = value;
                TriangleBaseLabel.UnselectedColor = value;
            }
        }
        public override Color SelectedColor
        {
            get => base.SelectedColor;
            set
            {
                base.SelectedColor = value;
                TriangleBaseLabel.SelectedColor = value;
            }
        }
        public override Color Color
        {
            get => Color;
            set
            {
                base.Color = value;
                TriangleBaseLabel.Color = value;
            }
        }

		private readonly ISettings _settings;

        public AngleCaliper(AngleCaliperPosition position,
            ICaliperView caliperView,
            ISettings settings,
            AngleCalibration angleCalibration = null,
            bool fakeUI = false) : base(caliperView)
        {
            _fakeUI = fakeUI;
            _settings = settings;
            CaliperType = CaliperType.Angle;
            AngleCalibration = angleCalibration ?? AngleCalibration.Uncalibrated;
            Bars = InitBars(position);
            InitCaliperLabel();
            InitTriangleLabel(position);
        }

		private List<Bar> InitBars(AngleCaliperPosition position)
        {
			LeftAngleBar = new Bar(Bar.Role.LeftAngle, position.Apex, position.FirstAngle, Bounds, _fakeUI)
			{
				Angle = position.FirstAngle
			};
			RightAngleBar = new Bar(Bar.Role.RightAngle, position.Apex, position.LastAngle, Bounds, _fakeUI)
			{
				Angle = position.LastAngle
			};
			ApexBar = new Bar(Bar.Role.Apex, position.Apex, 0, Bounds, _fakeUI); // ApexBar never drawn
			InitTriangleBase(TriangleHeight());
			TriangleBaseBar.Visibility = ShowBrugadaTriangle(position) ?
				Visibility.Visible :
				Visibility.Collapsed;
			return new List<Bar> { LeftAngleBar, RightAngleBar, ApexBar, TriangleBaseBar };
		}

		private double TriangleHeight()
        {
            if (AngleCalibration.AmplitudeCalibration != null && AngleCalibration.AmplitudeCalibration.Multiplier != 0)
            {
                double pointsPerMM = 1.0 / AngleCalibration.AmplitudeCalibration.Multiplier;
                return 5.0 * pointsPerMM;
            }
            return 0;
            // TODO: is it ok to return 0?
		}

		private bool ShowBrugadaTriangle(AngleCaliperPosition position)
        {
            return (_settings.ShowBrugadaTriangle &&
                AngleCalibration.TimeCalibration.Parameters.Unit == CalibrationUnit.Msec &&
                AngleCalibration.AmplitudeCalibration.Parameters.Unit == CalibrationUnit.Mm &&
				AngleInSouthernHemisphere(position.FirstAngle) &&
				AngleInSouthernHemisphere(position.LastAngle));
        }

		private static bool AngleInSouthernHemisphere(double angle)
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
			Point point = new(pointX, pointY);
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
			Point point = new(pointX, pointY);
			return point;
		}

        private void InitTriangleBase(double height)
        {
            Point point1 = GetBasePoint1ForHeight(height);
            Point point2 = GetBasePoint2ForHeight(height);
            double position = point1.Y;
            TriangleBaseBar = new Bar(Bar.Role.TriangleBase, position, point1.X, point2.X, _fakeUI);
        }

        private void InitCaliperLabel()
        {
            string text = AngleCalibration.GetText(Value);
            CaliperLabel = new AngleCaliperLabel(this, text,
                CaliperLabelAlignment.Top, false, _fakeUI);
        }

        private void InitTriangleLabel(AngleCaliperPosition position)
        {
            var text = AngleCalibration.GetSecondaryText(TriangleBaseValue);
            var alignment = _settings.TimeCaliperLabelAlignment;
            var autoAlignLabel = _settings.AutoAlignLabel;
            Visibility visibility = ShowBrugadaTriangle(position) ?
                Visibility.Visible : Visibility.Collapsed;
            TriangleBaseLabel = new TriangleBaseLabel(this, CaliperView, text,
                alignment, autoAlignLabel, _fakeUI, visibility);
        }

		public override void AddToView(ICaliperView caliperView)
		{
			base.AddToView(caliperView);
            TriangleBaseLabel?.AddToView(caliperView);
		}

		public override void Remove(ICaliperView caliperView)
		{
			base.Remove(caliperView);
            TriangleBaseLabel?.RemoveFromView(caliperView);
		}

		public override void ChangeBounds()
        {
            var bounds = CaliperView.Bounds;
            LeftAngleBar.Bounds = bounds;
            RightAngleBar.Bounds = bounds;
            LeftAngleBar.SetAngleBarPosition(new Point(LeftAngleBar.X1, LeftAngleBar.Y1), LeftAngleBar.Angle);
            RightAngleBar.SetAngleBarPosition(new Point(RightAngleBar.X1, RightAngleBar.Y1), RightAngleBar.Angle);
        }

        public override void Drag(Bar bar, Point delta, Point position)
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
					LeftAngleBar.Angle = RelativeTheta(position);
					LeftAngleBar.SetAngleBarPosition(new Point(LeftAngleBar.X1, LeftAngleBar.Y1), LeftAngleBar.Angle);
					break;
				case Bar.Role.RightAngle:
					RightAngleBar.Angle = RelativeTheta(position);
					RightAngleBar.SetAngleBarPosition(new Point(RightAngleBar.X1, RightAngleBar.Y1), RightAngleBar.Angle);
					break;
				default: break;
			}
			DrawTriangleBase();
			CaliperLabel.Text = AngleCalibration.GetText(Value);
			CaliperLabel.SetPosition();
		}

		public void DrawTriangleBase()
		{
			if (ShowBrugadaTriangle(CaliperPosition))
			{
				// Triangle base needs redrawing no matter how angle caliper moves.
				TriangleBaseBar.Visibility = Visibility.Visible;
				double height = TriangleHeight();
				Point point1 = GetBasePoint1ForHeight(height);
				Point point2 = GetBasePoint2ForHeight(height);
				double position = point1.Y;
				TriangleBaseBar.X1 = point1.X;
				TriangleBaseBar.Y1 = position;
				TriangleBaseBar.X2 = point2.X;
				TriangleBaseBar.Y2 = position;
				double baseValue = point2.X - point1.X;
                TriangleBaseLabel.Text = AngleCalibration.GetSecondaryText(baseValue);
                TriangleBaseLabel.SetPosition();
                TriangleBaseLabel.Visibility = Visibility.Visible; 
			}
			else
			{
				TriangleBaseBar.Visibility = Visibility.Collapsed;
                TriangleBaseLabel.Visibility = Visibility.Collapsed; 
			}
		}

        public override void ApplySettings(ISettings settings)
        {
            base.ApplySettings(settings);
            TriangleBaseLabel.AutoAlignLabel = settings.AutoAlignLabel;
            TriangleBaseLabel.Alignment = settings.TimeCaliperLabelAlignment;
            TriangleBaseLabel.SetPosition();
            DrawTriangleBase();
        }

		public override Bar IsNearBar(Point p)
        {
            if (ApexBar.IsNear(p))
            {
                return ApexBar;
            }
            if (PointNearBar(p, LeftAngleBar.Angle))
            {
                return LeftAngleBar;
            }
            if (PointNearBar(p, RightAngleBar.Angle))
            {
                return RightAngleBar;
            }
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
