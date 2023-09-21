using EPCalipersWinUI3.Models;
using Xunit;

namespace EPCalipersWinUi3Tests.Tests
{
	public class CaliperComponentTests
	{
		CaliperComponent component;

		public CaliperComponentTests()
		{
			component = new CaliperComponent();
		}

		[Fact]
		public void TestMoveComponent()
		{
			Assert.Equal(0, component.Position);
			component.Move(0);
			Assert.Equal(0, component.Position);
			component.Move(1);
			Assert.Equal(1, component.Position);
			component.Move(2);
			Assert.Equal(3, component.Position);
			component.Move(-5);
			Assert.Equal(-2, component.Position);
		}
	}
}
