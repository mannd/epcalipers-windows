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

namespace EPCalipersWinUi3Tests.Tests
{
    public class CaliperTests
	{
		private TimeCaliper GetTimeCaliper()
		{
			var bounds = new Bounds(500, 1000);
			return new TimeCaliper(bounds, new CaliperPosition(50, 100, 200), true);
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
		}

	}
}
