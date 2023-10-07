using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Calipers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Xunit;

namespace EPCalipersWinUi3Tests.Tests
{
    public class CaliperTests
	{
		[Fact]
		public void TestCaliperIsSelected()
		{
			var stubComponentLine = new FakeComponentLine();
			var component = new CaliperComponent(CaliperComponent.Role.Vertical, 100, 0, 100, stubComponentLine);
			Assert.False(false);

			//TimeCaliper timeCaliper = new TimeCaliper(grid);
			//Assert.False(timeCaliper.LeftBar.IsSelected);
			//Assert.False(timeCaliper.RightBar.IsSelected);
			//Assert.False(timeCaliper.CrossBar.IsSelected);
			//timeCaliper.IsSelected = true;
			//Assert.True(timeCaliper.LeftBar.IsSelected);
			//Assert.True(timeCaliper.RightBar.IsSelected);
			//Assert.True(timeCaliper.CrossBar.IsSelected);
			//timeCaliper.IsSelected = false;
			//Assert.False(timeCaliper.LeftBar.IsSelected);
			//Assert.False(timeCaliper.RightBar.IsSelected);
			//Assert.False(timeCaliper.CrossBar.IsSelected);
		}
	}
}
