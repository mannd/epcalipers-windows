using EPCalipersWinUI3.Calipers;
using Microsoft.UI;
using Windows.Foundation;
using Xunit;

namespace EPCalipersWinUi3Tests.Tests
{
    public class CaliperComponentTests
	{
		CaliperComponent component;

		public CaliperComponentTests()
		{
			var stubComponentLine = new FakeComponentLine();
			component = new CaliperComponent(CaliperComponent.Role.Vertical, 100, 0, 200, stubComponentLine);
		}

		[Fact]
		public void TestPosition()
		{
			var position = component.Position;
			Assert.Equal(100, position);
			component.X1 = 200;
			Assert.Equal(200, component.Position);
		}

		[Fact]
		public void TestIsNear()
		{
			Point p = new Point(101, 50);
			Assert.True(component.IsNear(p));
			Point p0 = new Point(95, 50);
			Assert.True(component.IsNear(p0));
			Point p1 = new Point(150, 50);
			Assert.False(component.IsNear(p1));
		}
	}
}
