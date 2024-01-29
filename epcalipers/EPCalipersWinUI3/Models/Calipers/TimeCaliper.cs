using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using Windows.Foundation;

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

		public void AddMarchingCaliper()
		{
			MarchingCaliper = new MarchingCaliper(CaliperView, this, 10, LeftMostBarPosition, RightMostBarPosition, _fakeUI);
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
			return new List<Bar> { LeftBar, RightBar, CrossBar };
		}

		private void InitCaliperLabel()
		{
			var text = Text;
			var alignment = _settings.TimeCaliperLabelAlignment;
			var autoAlignLabel = _settings.AutoAlignLabel;
			CaliperLabel = new TimeCaliperLabel(this, CaliperView, text, alignment, autoAlignLabel, _fakeUI);
		}

		public override void ApplySettings(ISettings settings)
		{
			base.ApplySettings(settings);
			CaliperLabel.AutoAlignLabel = settings.AutoAlignLabel;
			CaliperLabel.Alignment = settings.TimeCaliperLabelAlignment;
			CaliperLabel.SetPosition();
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

		public override void Drag(Bar bar, Point delta, Point previousPoint)
		{
			if (bar == LeftBar)
			{
				bar.Position += delta.X;
				CrossBar.X1 += delta.X;
			}
			else if (bar == RightBar)
			{
				bar.Position += delta.X;
				CrossBar.X2 += delta.X;
			}
			else if (bar == CrossBar)
			{
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
			UpdateLabel();
		}
		#endregion
	}
}
