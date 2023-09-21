using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Models;
using Xunit;

namespace EPCalipersWinUi3Tests.Tests
{
	public class CaliperTests
	{
		[Fact]
		public void TestCaliperIsSelected()
		{
			TimeCaliper timeCaliper = new TimeCaliper();
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
