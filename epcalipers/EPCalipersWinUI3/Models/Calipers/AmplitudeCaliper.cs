using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using Windows.Foundation;

namespace EPCalipersWinUI3.Models.Calipers
{
	public sealed class AmplitudeCaliper : Caliper
	{
		public Bar TopBar { get; set; }
		public Bar BottomBar { get; set; }
		public Bar CrossBar { get; set; }

		public override Bar HandleBar => CrossBar;

		public override double Value => BottomBar.Position - TopBar.Position;

		public double TopMostBarPosition => Math.Min(TopBar.Position, BottomBar.Position);
		public double BottomMostBarPosition => Math.Max(TopBar.Position, BottomBar.Position);

		private readonly ISettings _settings;

		public AmplitudeCaliper(CaliperPosition position, ICaliperView caliperView, ISettings settings,
			bool fakeUI = false, Calibration calibration = null) : base(caliperView, calibration)
		{
			_fakeUI = fakeUI;
			_settings = settings;
			Bars = InitBars(position);
			InitCaliperLabel();
			CaliperType = CaliperType.Amplitude;
		}

		public override void UpdateSidebars()
		{
			SetSidebarLength();
		}

		private void SetSidebarLength()
		{
			if (_settings.AdjustableSidebarLength)
			{
				var sidebarHalfLength = _settings.SidebarLength / 2;
				TopBar.X1 = Math.Min(CrossBar.Position - sidebarHalfLength, Bounds.Width);
				TopBar.X2 = Math.Max(CrossBar.Position + sidebarHalfLength, 0);
				BottomBar.X1 = Math.Min(CrossBar.Position - sidebarHalfLength, Bounds.Width);
				BottomBar.X2 = Math.Max(CrossBar.Position + sidebarHalfLength, 0);
			}
			else
			{
				TopBar.X1 = 0;
				TopBar.X2 = Bounds.Width;
				BottomBar.X1 = 0;
				BottomBar.X2 = Bounds.Width;
			}
		}

		private List<Bar> InitBars(CaliperPosition position)
		{
			// NB Crossbar must be first to allow isNear to work properly.
			CrossBar = new Bar(Bar.Role.VerticalCrossBar,
				position.Center, position.First, position.Last, _fakeUI);
			if (_settings.AdjustableSidebarLength)
			{
				var sidebarHalfLength = _settings.SidebarLength / 2;
				TopBar = new Bar(Bar.Role.Horizontal, position.First, position.Center - sidebarHalfLength, position.Center + sidebarHalfLength, _fakeUI);
				BottomBar = new Bar(Bar.Role.Horizontal, position.Last, position.Center - sidebarHalfLength, position.Center + sidebarHalfLength, _fakeUI);
			}
			else
			{
				TopBar = new Bar(Bar.Role.Horizontal, position.First, 0, Bounds.Width, _fakeUI);
				BottomBar = new Bar(Bar.Role.Horizontal, position.Last, 0, Bounds.Width, _fakeUI);
			}
			return new List<Bar> { TopBar, BottomBar, CrossBar };
		}
		private void InitCaliperLabel()
		{
			var text = Text;
			var alignment = _settings.AmplitudeCaliperLabelAlignment;
			var autoAlignLabel = _settings.AutoAlignLabel;
			var fontSize = _settings.FontSize;
			CaliperLabel = new AmplitudeCaliperLabel(this, CaliperView, text,
				alignment, autoAlignLabel, fontSize, _settings.AdjustCaliperLabelSizeWithZoom, ScaleFactor, _fakeUI);
		}

		public override void ChangeBounds()
		{
			SetSidebarLength();
		}

		public override void Drag(Bar bar, Point delta, Point previousPoint)
		{
			var width = Bounds.Width - _margin;
			var height = Bounds.Height - _margin;
			if (bar == TopBar)
			{
				var topBarPosition = TopBar.Position + delta.Y;
				if (topBarPosition > height || topBarPosition < _margin) return;
				bar.Position += delta.Y;
				CrossBar.Y1 += delta.Y;
			}
			else if (bar == BottomBar)
			{
				var bottomBarPosition = BottomBar.Position + delta.Y;
				if (bottomBarPosition > height || bottomBarPosition < _margin) return;
				bar.Position += delta.Y;
				CrossBar.Y2 += delta.Y;
			}
			else if (bar == CrossBar)
			{
				var topBarPosition = TopBar.Position + delta.Y;
				var bottomBarPosition = BottomBar.Position + delta.Y;
				var crossBarPosition = CrossBar.Position + delta.X;
				if (topBarPosition > height || topBarPosition < _margin) return;
				if (bottomBarPosition > height || bottomBarPosition < _margin) return;
				if (crossBarPosition > width || crossBarPosition < _margin) return;
				TopBar.Position += delta.Y;
				BottomBar.Position += delta.Y;
				bar.Position += delta.X;
				bar.Y1 += delta.Y;
				bar.Y2 += delta.Y;
				if (_settings.AdjustableSidebarLength)
				{
					var sidebarHalfLength = _settings.SidebarLength / 2;
					TopBar.X1 = Math.Min(CrossBar.Position - sidebarHalfLength, Bounds.Width);
					TopBar.X2 = Math.Max(CrossBar.Position + sidebarHalfLength, 0);
					BottomBar.X1 = Math.Min(CrossBar.Position - sidebarHalfLength, Bounds.Width);
					BottomBar.X2 = Math.Max(CrossBar.Position + sidebarHalfLength, 0);
				}
			}
			UpdateLabel();
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

		public override void ApplySettings(ISettings settings)
		{
			base.ApplySettings(settings);
			CaliperLabel.Alignment = settings.AmplitudeCaliperLabelAlignment;
			CaliperLabel.SetPosition();
		}


	}
}
