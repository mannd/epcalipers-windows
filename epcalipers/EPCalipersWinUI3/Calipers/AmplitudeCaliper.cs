using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace EPCalipersWinUI3.Calipers
{
	public sealed class AmplitudeCaliper : Caliper
	{
        public Bar TopBar { get; set; }
        public Bar BottomBar { get; set; }
        public Bar CrossBar { get; set; }

		public override double Value =>  BottomBar.Position - TopBar.Position;

		public double TopMostBarPosition => Math.Min(TopBar.Position, BottomBar.Position);
		public double BottomMostBarPosition => Math.Max(TopBar.Position, BottomBar.Position);

		private ISettings _settings;

		public AmplitudeCaliper(CaliperPosition position, ICaliperView caliperView,
			bool fakeUI = false) : base(caliperView)
		{
			_fakeUI = fakeUI;
			if (_fakeUI)
			{
				_settings = new FakeSettings();
			}
			else
			{
				_settings = new Settings();
			}
			Bars = InitBars(position);
			InitCaliperLabel();
			CaliperType = CaliperType.Amplitude;
		}

		private Bar[] InitBars(CaliperPosition position)
		{
			// NB Crossbar must be first to allow isNear to work properly.
			CrossBar = new Bar(Bar.Role.VerticalCrossBar,
				position.Center, position.First, position.Last, _fakeUI);
			TopBar = new Bar(Bar.Role.Horizontal, position.First, 0, Bounds.Width, _fakeUI);
			BottomBar = new Bar(Bar.Role.Horizontal, position.Last, 0, Bounds.Width, _fakeUI);
			return new[] { TopBar, BottomBar, CrossBar };
		}
		private void InitCaliperLabel()
		{
			var text = $"{Value} points";
			var alignment = _settings.AmplitudeCaliperLabelAlignment;
			CaliperLabel = new AmplitudeCaliperLabel(this, CaliperView, text,
				alignment, false, base._fakeUI);
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
			string text = string.Format("{0:0.#} points", Value);
			CaliperLabel.Text = text;
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
			CaliperLabel.Alignment = settings.AmplitudeCaliperLabelAlignment;
			CaliperLabel.SetPosition();
		}


	}
}
