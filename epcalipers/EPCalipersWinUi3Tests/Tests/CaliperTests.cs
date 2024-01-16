using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using Windows.Foundation;
using Xunit;

namespace EPCalipersWinUi3Tests.Tests
{
	public class CaliperTests
	{
		private static TimeCaliper GetTimeCaliper()
		{
			var stubCaliperView = new FakeCaliperView();
			var stubSettings = new FakeSettings();
			var calibration = new Calibration();
			calibration.Rounding = Rounding.ToHundredths;
			var timeCaliper = new TimeCaliper(new CaliperPosition(50, 50, 200), 
				stubCaliperView, stubSettings, true, calibration);
			return timeCaliper;
		}
		private static AmplitudeCaliper GetAmplitudeCaliper()
		{
			var stubCaliperView = new FakeCaliperView();
			var stubSettings = new FakeSettings();
			var calibration = new Calibration();
			calibration.Rounding = Rounding.ToHundredths;
			var amplitudeCaliper = new AmplitudeCaliper(new CaliperPosition(50, 50, 200), 
				stubCaliperView, stubSettings, true, calibration);
			return amplitudeCaliper;
		}
		private static AngleCaliper GetAngleCaliper()
		{
			var stubCaliperView = new FakeCaliperView();
			var stubSettings = new FakeSettings();
			double firstAngle = 0.5 * Math.PI;
			double secondAngle = 0.25 * Math.PI;
			return new AngleCaliper(new AngleCaliperPosition(new Windows.Foundation.Point(50, 50),
				firstAngle, secondAngle), stubCaliperView, stubSettings, fakeUI: true);
		}

		[Fact]
		public void TestCaliperIsSelected()
		{
			var timeCaliper = GetTimeCaliper();
			Assert.False(timeCaliper.LeftBar.IsSelected);
			Assert.False(timeCaliper.RightBar.IsSelected);
			Assert.False(timeCaliper.CrossBar.IsSelected);
			timeCaliper.IsSelected = true;
			Assert.True(timeCaliper.LeftBar.IsSelected);
			Assert.True(timeCaliper.RightBar.IsSelected);
			Assert.True(timeCaliper.CrossBar.IsSelected);
			timeCaliper.IsSelected = false;
			Assert.False(timeCaliper.LeftBar.IsSelected);
			Assert.False(timeCaliper.RightBar.IsSelected);
			Assert.False(timeCaliper.CrossBar.IsSelected);
			var amplitudeCaliper = GetAmplitudeCaliper();
			Assert.False(amplitudeCaliper.TopBar.IsSelected);
			Assert.False(amplitudeCaliper.BottomBar.IsSelected);
			Assert.False(amplitudeCaliper.CrossBar.IsSelected);
			amplitudeCaliper.IsSelected = true;
			Assert.True(amplitudeCaliper.TopBar.IsSelected);
			Assert.True(amplitudeCaliper.BottomBar.IsSelected);
			Assert.True(amplitudeCaliper.CrossBar.IsSelected);
			amplitudeCaliper.IsSelected = false;
			Assert.False(amplitudeCaliper.TopBar.IsSelected);
			Assert.False(amplitudeCaliper.BottomBar.IsSelected);
			Assert.False(amplitudeCaliper.CrossBar.IsSelected);
			var angleCaliper = GetAngleCaliper();
			angleCaliper.IsSelected = true;
			Assert.True(angleCaliper.LeftAngleBar.IsSelected);
			Assert.True(angleCaliper.RightAngleBar.IsSelected);
			angleCaliper.IsSelected = false;
			Assert.False(angleCaliper.LeftAngleBar.IsSelected);
			Assert.False(angleCaliper.RightAngleBar.IsSelected);
		}

		[Fact]
		public void TestCaliperStaysInBounds()
		{
			var timeCaliper = GetTimeCaliper();
			var bounds = new FakeCaliperView().Bounds;
			Assert.True(timeCaliper.LeftBar.Y2 <= bounds.Height);
			Assert.True(timeCaliper.RightBar.Y2 <= bounds.Height);
			var amplitudeCaliper = GetAmplitudeCaliper();
			Assert.True(amplitudeCaliper.TopBar.Y2 <= bounds.Height);
			Assert.True(amplitudeCaliper.BottomBar.Y2 <= bounds.Height);
			var angleCaliper = GetAngleCaliper();
			Assert.True(angleCaliper.LeftAngleBar.Y2 <= bounds.Height);
			Assert.True(angleCaliper.RightAngleBar.Y2 <= bounds.Height);
		}

		[Fact]
		public void TestCaliperLabel()
		{
			// NB: This test will need revision once Rounding is implemented.
			var timeCaliper = GetTimeCaliper(); // calibration on init is toHundredths
			var labelText = timeCaliper.CaliperLabel.Text;
			Assert.Equal("150.00 points", labelText);
			var amplitudeCaliper = GetAmplitudeCaliper();
			Assert.Equal("150.00 points", amplitudeCaliper.CaliperLabel.Text);
		}

		[Fact]
		public void TestHandleBar()
		{
			var timeCaliper = GetTimeCaliper();
			Assert.Equal(Bar.Role.HorizontalCrossBar, timeCaliper.HandleBar.BarRole);
			var amplitudeCaliper = GetAmplitudeCaliper();
			Assert.Equal(Bar.Role.VerticalCrossBar, amplitudeCaliper.HandleBar.BarRole);
			var angleCaliper = GetAngleCaliper();
			Assert.Equal(Bar.Role.Apex, angleCaliper.HandleBar.BarRole);
		}
	}
}
