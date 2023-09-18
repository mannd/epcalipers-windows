using EPCalipersWinUI3.Models;
using Xunit;

namespace EPCalipersWinUi3Tests
{
	public class UnitTest1
	{

		[Fact]
		public void TestApp()
		{
			AppInfo appInfo = new AppInfo();
			Assert.Equal("3.0.0.0-alpha", appInfo.ProductVersion);
			Assert.Equal("3.0.0.0", appInfo.FileVersion);
		}

		[Theory]
		[InlineData(3)]
		[InlineData(5)]
	//	[InlineData(6)]
		public void MyFirstTheory(int value)
		{
			Assert.True(IsOdd(value));
		}

		private bool IsOdd(int value)
		{
			return value % 2 == 1;
		}
	}
}