using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;

namespace EPCalipersWinUI3.Models.Calipers
{

	public sealed class TimeCaliper : Caliper
	{
		public bool IsMarching { get; set; } = false;
		public MarchingCaliper MarchingCaliper { get; set; }

		#region properties
		public Bar LeftBar { get; set; }
		public Bar RightBar { get; set; }
		public Bar CrossBar { get; set; }

		public override Bar HandleBar => CrossBar;

		public override double Value => RightBar.Position - LeftBar.Position;
		public double LeftMostBarPosition => Math.Min(LeftBar.Position, RightBar.Position);
		public double RightMostBarPosition => Math.Max(RightBar.Position, LeftBar.Position);
		#endregion

		#region fields
		private readonly ISettings _settings;
		#endregion

		#region init
		public TimeCaliper(CaliperPosition position,
			ICaliperView caliperView, ISettings settings, bool fakeUI = false,
			Calibration calibration = null) : base(caliperView, calibration)
		{
			_fakeUI = fakeUI;
			_settings = settings;
			Bars = InitBars(position);
			CaliperType = CaliperType.Time;
			InitCaliperLabel();
		}

		public override Color UnselectedColor
		{
			get => base.UnselectedColor;
			set
			{
				base.UnselectedColor = value;
				if (MarchingCaliper != null)
				{
					MarchingCaliper.UnselectedColor = value;
				}
			}
		}

		public void AddMarchingCaliper()
		{
			MarchingCaliper = new MarchingCaliper(CaliperView, this, LeftMostBarPosition, RightMostBarPosition, _fakeUI);
		}
		public override void UpdateScaledBarThickness()
		{
			base.UpdateScaledBarThickness();
			MarchingCaliper?.UpdateMarchingScaledBarThickness();
		}

		public void RemoveMarchingCaliper()
		{
			MarchingCaliper?.Remove(CaliperView);
			MarchingCaliper = null;
		}

		public override void SelectPartialCaliper(Bar bar)
		{
			MarchingCaliper?.UnselectFullCaliper();
			base.SelectPartialCaliper(bar);
		}

		public override void SetFullSelectionTo(bool value)
		{
			base.SetFullSelectionTo(value);
			MarchingCaliper?.SetFullSelectionTo(value);
		}

		public override void Remove(ICaliperView caliperView)
		{
			RemoveMarchingCaliper();
			base.Remove(caliperView);
		}

		private List<Bar> InitBars(CaliperPosition position)
		{
			// NB Crossbar must be first, to allow IsNear to work correctly.
			CrossBar = new Bar(Bar.Role.HorizontalCrossBar, position.Center, position.First, position.Last, _fakeUI);
			LeftBar = new Bar(Bar.Role.Vertical, position.First, 0, Bounds.Height, _fakeUI);
			RightBar = new Bar(Bar.Role.Vertical, position.Last, 0, Bounds.Height, _fakeUI);
			return [LeftBar, RightBar, CrossBar];
		}

		private void InitCaliperLabel()
		{
			var text = Text;
			var alignment = _settings.TimeCaliperLabelAlignment;
			var autoAlignLabel = _settings.AutoAlignLabel;
			var fontSize = _settings.FontSize;
			CaliperLabel = new TimeCaliperLabel(this, CaliperView, text, alignment, autoAlignLabel, fontSize, 
				_settings.AdjustCaliperLabelSizeWithZoom, ScaleFactor, _fakeUI);
		}

		public override void ApplySettings(ISettings settings)
		{
			base.ApplySettings(settings);
			CaliperLabel.Alignment = settings.TimeCaliperLabelAlignment;
			CaliperLabel.SetPosition();
			if (MarchingCaliper != null)
			{
				// Need to force update layout, or bounds are 0,0.  
				// Bounds are based on ActualHeight, ActualWidth, which may default to 0,0 when
				// view is first shown, like when returning from Settings.
				CaliperView.UpdateLayout();
				RemoveMarchingCaliper();
				AddMarchingCaliper();
				MarchingCaliper.UpdateMarchingScaledBarThickness();
			}
		}
		#endregion
		#region movement
		public override void ChangeBounds()
		{
			var bounds = CaliperView.Bounds;
			LeftBar.Y2 = bounds.Height;
			RightBar.Y2 = bounds.Height;
		}

		public override Bar IsNearBar(Point p)
		{
			foreach (var bar in Bars)
			{
				if (bar.IsNear(p))
				{
					return bar;
				}
			}
			return null;
		}

		// TODO: Consider hiding out of bounds caliper components, to avoid image shifting when it is centered.
		public override void Drag(Bar bar, Point delta, Point previousPoint)
		{
			var width = Bounds.Width - _margin;
			var height = Bounds.Height - _margin;
			if (bar == LeftBar)
			{
				var leftBarPosition = LeftBar.Position + delta.X;
				if (leftBarPosition > width || leftBarPosition < _margin) return;
				bar.Position += delta.X;
				CrossBar.X1 += delta.X;
			}
			else if (bar == RightBar)
			{
				var rightBarPosition = RightBar.Position + delta.X;
				if (rightBarPosition > width || rightBarPosition < _margin) return;
				bar.Position += delta.X;
				CrossBar.X2 += delta.X;
			}
			else if (bar == CrossBar)
			{
				var leftBarPosition = LeftBar.Position + delta.X;
				var rightBarPosition = RightBar.Position + delta.X;
				var crossBarPosition = CrossBar.Position + delta.Y;
				if (leftBarPosition > width || leftBarPosition < _margin) return;
				if (rightBarPosition > width || rightBarPosition < _margin) return;
				if (crossBarPosition > height || crossBarPosition < _margin) return;
				LeftBar.Position += delta.X;
				RightBar.Position += delta.X;
				bar.X1 += delta.X;
				bar.X2 += delta.X;
				bar.Position += delta.Y;
			}
			if (IsMarching)
			{
				MarchingCaliper?.Move();
			}
			// TODO: Don't allow label to go out of bounds!
			UpdateLabel();
		}
		#endregion
	}
}
