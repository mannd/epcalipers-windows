using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Models;
using Xunit;

namespace EPCalipersWinUi3Tests.Tests
{
	public class AppInfoModelTests
	{
		private AppInfo appInfo;

		public AppInfoModelTests() 
		{
			appInfo = new AppInfo();
		}

		[Fact]
		public void TestAppInfo()
		{
			//Assert.Equal("3.0.0.0-alpha", appInfo.ProductVersion);
			Assert.Equal("3.0.0.0", appInfo.FileVersion);
			Assert.Equal("EPCalipersWinUI3", appInfo.Title);
			Assert.Equal("EP Calipers 3", appInfo.ProductName);
			Assert.Equal("Copyright (c) 2023", appInfo.Copyright);
			Assert.Equal("EP Studios", appInfo.Company);
		}
	}
}
