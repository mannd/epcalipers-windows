using EPCalipersWinUI3.Models;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace EPCalipersWinUi3Tests
{
	public class UnitTest1
	{

		[Fact]
		public void TestApp()
		{
			AppInfo appInfo = new AppInfo();
			Assert.Equal("3.0.0.0-alpha", appInfo.AssemblyVersion);
		}


		[Fact]
		public void Test1()
		{
			Assert.Equal(4, Add(2, 2));
		}

		[Fact]
		public void Test2()
		{
			Assert.Equal(5, Add(2, 2));
		}

		[Theory]
		[InlineData(3)]
		[InlineData(5)]
		[InlineData(6)]
		public void MyFirstTheory(int value)
		{
			Assert.True(IsOdd(value));
		}

		bool IsOdd(int value)
		{
			return value % 2 == 1;
		}


		int Add(int x, int y)
		{
			return x + y;
		}
	}
}