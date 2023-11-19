using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EPCalipersWinUI3.Calipers;
using EPCalipersWinUI3.Views;

namespace EPCalipersWinUi3Tests.Tests
{
	public class CaliperLabelTests
	{
		[Fact]
		public void TestTimeCaliperAutoAlign()
		{
			var position = new CaliperPosition();
			var stubCaliperView = new FakeCaliperView(); // Bounds(800, 400)
			var caliper = new TimeCaliper(position, stubCaliperView, true);
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
			var caliper = new AmplitudeCaliper(position, stubCaliperView, true);
			var alignment = CaliperLabelAlignment.Top;
			var label = new AmplitudeCaliperLabel(caliper, stubCaliperView, "100 points",
				alignment, false, true);
			//label.Size = new Windows.Foundation.Size(100, 50);
			//var autoAlignment = label.AutoAlign(alignment, false);
			//Assert.Equal(CaliperLabelAlignment.Top, autoAlignment);
			//caliper.CrossBar.Position = 100;
			//caliper.TopBar.Position = 100;
			//caliper.BottomBar.Position = 300;  // Give enough room to fit the label in between
			//autoAlignment = label.AutoAlign(alignment, true);
			//Assert.Equal(CaliperLabelAlignment.Top, autoAlignment);
			//caliper.TopBar.Position = 200;
			//caliper.BottomBar.Position = 210;  // make them too close
			//autoAlignment = label.AutoAlign(alignment, true);
			//Assert.Equal(CaliperLabelAlignment.Left, autoAlignment);
			//caliper.TopBar.Position = 50;  // too close to left edge of view
			//caliper.BottomBar.Position = 100;  // make them too close
			//autoAlignment = label.AutoAlign(alignment, true);
			//Assert.Equal(CaliperLabelAlignment.Right, autoAlignment);
			//alignment = CaliperLabelAlignment.Right;
			//caliper.TopBar.Position = 500;  
			//caliper.BottomBar.Position = 600;  // plenty of room all around
			//autoAlignment = label.AutoAlign(alignment, true);
			//Assert.Equal(CaliperLabelAlignment.Right, autoAlignment);
			//caliper.TopBar.Position = 500;  
			//caliper.BottomBar.Position = 710;  // too close to right side of view
			//autoAlignment = label.AutoAlign(alignment, true);
			//Assert.Equal(CaliperLabelAlignment.Left, autoAlignment);
		}
	}
}
