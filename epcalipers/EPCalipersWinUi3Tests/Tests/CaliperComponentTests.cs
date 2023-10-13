using EPCalipersWinUI3.Calipers;
using Microsoft.UI;
using Windows.Foundation;
using Xunit;

namespace EPCalipersWinUi3Tests.Tests
{
    public class CaliperComponentTests
	{

		private Bar GetFakeComponent(Bar.Role role)
		{
			return new Bar(role, 100, 0, 200, true);
		}

		[Fact]
		public void TestIsNear()
		{
			var verticalComponent = GetFakeComponent(Bar.Role.Vertical);
			verticalComponent.X1 = 100;
			verticalComponent.X2 = 100;
			Point p = new Point(101, 50);
			Assert.True(verticalComponent.IsNear(p));
			Point p0 = new Point(95, 50);
			Assert.True(verticalComponent.IsNear(p0));
			Point p1 = new Point(150, 50);
			Assert.False(verticalComponent.IsNear(p1));

			var horizontalComponent = GetFakeComponent(Bar.Role.Horizontal);
			horizontalComponent.Y1 = 100;
			horizontalComponent.Y2 = 100;
			Point p2 = new Point(50, 101);
			Assert.True(horizontalComponent.IsNear(p2));
			Point p3 = new Point(50, 95);
			Assert.True(horizontalComponent.IsNear(p3));
			Point p4 = new Point(50, 150);
			Assert.False(horizontalComponent.IsNear(p4));

			var horizontalCrossbarComponent = GetFakeComponent(Bar.Role.HorizontalCrossBar);
			horizontalCrossbarComponent.X1 = 100;
			horizontalCrossbarComponent.X2 = 200;
			horizontalCrossbarComponent.Y1 = 50;
			Point p5 = new Point(101, 52);
			Assert.True(horizontalCrossbarComponent.IsNear(p5));
			Point p6 = new Point(198, 48);
			Assert.True(horizontalCrossbarComponent.IsNear(p6));
			Point p7 = new Point(150, 70);
			Assert.False(horizontalCrossbarComponent.IsNear(p7));
		}
	}
}
