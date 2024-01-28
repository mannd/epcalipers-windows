using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using Xunit;



namespace EPCalipersWinUi3Tests.Tests
{
	public class MathTests
	{
		[Fact]
		public void TestScaleRectangle()
		{
			int width = 100;
			int height = 50;
			var scale0 = MathHelper.ScaleToFit(width, height, 90);
			Assert.True(scale0 < 1.0);
		}

		[Fact]
		public void TestCenter()
		{
			Bounds bounds = new Bounds(100, 50);
			Assert.Equal(50, MathHelper.Center(bounds).X);
			Assert.Equal(25, MathHelper.Center(bounds).Y);
		}
	}
}
