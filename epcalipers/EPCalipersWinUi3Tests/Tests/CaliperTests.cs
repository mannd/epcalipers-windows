using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Calipers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI;
using Windows.UI;
using Xunit;
using EPCalipersWinUI3;
using EPCalipersWinUI3.Views;

namespace EPCalipersWinUi3Tests.Tests
{
    public class CaliperTests
	{
		private TimeCaliper GetTimeCaliper()
		{
			var stubCaliperView = new FakeCaliperView();
			return new TimeCaliper(new CaliperPosition(50, 50, 200), stubCaliperView, true);
		}
		private AmplitudeCaliper GetAmplitudeCaliper()
		{
			var stubCaliperView = new FakeCaliperView();
			return new AmplitudeCaliper(new CaliperPosition(50, 50, 200), stubCaliperView, true);
		}
		private AngleCaliper GetAngleCaliper()
		{
			var stubCaliperView = new FakeCaliperView();
			double firstAngle = 0.5 * Math.PI;
			double secondAngle = 0.25 * Math.PI;
			return new AngleCaliper(new AngleCaliperPosition(new Windows.Foundation.Point(50, 50),
				firstAngle, secondAngle), stubCaliperView, true);
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
	}
}
