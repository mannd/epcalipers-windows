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

		private List<Bar> InitBars(CaliperPosition position)
		{
			// NB Crossbar must be first to allow isNear to work properly.
			CrossBar = new Bar(Bar.Role.VerticalCrossBar,
				position.Center, position.First, position.Last, _fakeUI);
			TopBar = new Bar(Bar.Role.Horizontal, position.First, 0, Bounds.Width, _fakeUI);
			BottomBar = new Bar(Bar.Role.Horizontal, position.Last, 0, Bounds.Width, _fakeUI);
			return new List<Bar> { TopBar, BottomBar, CrossBar };
		}
		private void InitCaliperLabel()
		{
			var text = Text;
			var alignment = _settings.AmplitudeCaliperLabelAlignment;
			var autoAlignLabel = _settings.AutoAlignLabel;
			CaliperLabel = new AmplitudeCaliperLabel(this, CaliperView, text,
				alignment, autoAlignLabel, _fakeUI);
		}

		public override void ChangeBounds()
		{
			var bounds = CaliperView.Bounds;
			TopBar.X2 = bounds.Width;
			BottomBar.X2 = bounds.Width;
		}

		public override void Drag(Bar bar, Point delta, Point previousPoint)
		{
			if (bar == TopBar)
			{
				bar.Position += delta.Y;
				CrossBar.Y1 += delta.Y;
			}
			else if (bar == BottomBar)
			{
				bar.Position += delta.Y;
				CrossBar.Y2 += delta.Y;
			}
			else if (bar == CrossBar)
			{
				TopBar.Position += delta.Y;
				BottomBar.Position += delta.Y;
				bar.Position += delta.X;
				bar.Y1 += delta.Y;
				bar.Y2 += delta.Y;
			}
			CaliperLabel.Text = Text;
			CaliperLabel.SetPosition();
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
			CaliperLabel.AutoAlignLabel = settings.AutoAlignLabel;
			CaliperLabel.Alignment = settings.AmplitudeCaliperLabelAlignment;
			CaliperLabel.SetPosition();
		}


	}
}
