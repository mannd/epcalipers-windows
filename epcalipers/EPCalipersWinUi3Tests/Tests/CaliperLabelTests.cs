using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EPCalipersWinUI3.Views;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;

namespace EPCalipersWinUi3Tests.Tests
{
    public class CaliperLabelTests
	{
		[Fact]
		public void TestTimeCaliperAutoAlign()
		{
			var position = new CaliperPosition();
			var stubCaliperView = new FakeCaliperView(); // Bounds(800, 400)
			var stubSettings = new FakeSettings();
			var caliper = new TimeCaliper(position, stubCaliperView, stubSettings, true);
			var alignment = CaliperLabelAlignment.Top;
			var label = new TimeCaliperLabel(caliper, stubCaliperView, "100 points",
				alignment, false, true);
			label.Size = new Windows.Foundation.Size(100, 50);
			var autoAlignment = label.AutoAlign(alignment, false);
			Assert.Equal(CaliperLabelAlignment.Top, autoAlignment);
			caliper.CrossBar.Position = 100;
			caliper.LeftBar.Position = 100;
			caliper.RightBar.Position = 300;  // Give enough room to fit the label in between
			autoAlignment = label.AutoAlign(alignment, true);
			Assert.Equal(CaliperLabelAlignment.Top, autoAlignment);
			caliper.LeftBar.Position = 200;
			caliper.RightBar.Position = 210;  // make them too close
			autoAlignment = label.AutoAlign(alignment, true);
			Assert.Equal(CaliperLabelAlignment.Left, autoAlignment);
			caliper.LeftBar.Position = 50;  // too close to left edge of view
			caliper.RightBar.Position = 100;  // make them too close
			autoAlignment = label.AutoAlign(alignment, true);
			Assert.Equal(CaliperLabelAlignment.Right, autoAlignment);
			alignment = CaliperLabelAlignment.Right;
			caliper.LeftBar.Position = 500;  
			caliper.RightBar.Position = 600;  // plenty of room all around
			autoAlignment = label.AutoAlign(alignment, true);
			Assert.Equal(CaliperLabelAlignment.Right, autoAlignment);
			caliper.LeftBar.Position = 500;  
			caliper.RightBar.Position = 710;  // too close to right side of view
			autoAlignment = label.AutoAlign(alignment, true);
			Assert.Equal(CaliperLabelAlignment.Left, autoAlignment);
		}

		[Fact]
		public void TestAmplitudeCaliperAutoAlign()
		{
			var position = new CaliperPosition();
			var stubCaliperView = new FakeCaliperView(); // Bounds(800, 400)
			var stubSettings = new FakeSettings();
			var caliper = new AmplitudeCaliper(position, stubCaliperView, stubSettings, true);
			var alignment = CaliperLabelAlignment.Left;
			var label = new AmplitudeCaliperLabel(caliper, stubCaliperView, "100 points",
				alignment, false, true);
			label.Size = new Windows.Foundation.Size(100, 50);
			var autoAlignment = label.AutoAlign(alignment, false);
			Assert.Equal(CaliperLabelAlignment.Left, autoAlignment);
			caliper.CrossBar.Position = 200;
			caliper.TopBar.Position = 100;
			caliper.BottomBar.Position = 300;  // Give enough room to fit the label in between
			autoAlignment = label.AutoAlign(alignment, true);
			Assert.Equal(CaliperLabelAlignment.Left, autoAlignment);
			caliper.TopBar.Position = 200;
			caliper.BottomBar.Position = 210;  // make them too close
			autoAlignment = label.AutoAlign(alignment, true);
			Assert.Equal(CaliperLabelAlignment.Top, autoAlignment);
			caliper.TopBar.Position = 50;  
			caliper.BottomBar.Position = 200;
			caliper.CrossBar.Position = 50;  // too close to left edge of view
			autoAlignment = label.AutoAlign(alignment, true);
			Assert.Equal(CaliperLabelAlignment.Right, autoAlignment);
			alignment = CaliperLabelAlignment.Right;
			caliper.TopBar.Position = 500;
			caliper.BottomBar.Position = 600;  // plenty of room all around
			autoAlignment = label.AutoAlign(alignment, true);
			Assert.Equal(CaliperLabelAlignment.Right, autoAlignment);
			caliper.TopBar.Position = 200;
			caliper.BottomBar.Position = 300;
			caliper.CrossBar.Position = 780; // Too close to right margin
			autoAlignment = label.AutoAlign(alignment, true);
			Assert.Equal(CaliperLabelAlignment.Left, autoAlignment);
		}
	}
}
