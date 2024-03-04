using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models;
using Xunit;

namespace EPCalipersWinUi3Tests.Tests
{
	public class TitleBarTests
	{
		[Fact]
		public void TestTitleBar()
		{
			const string appName = "EP Calipers 3";
			const string transparentWindowName = "transparent window";
			const string screenshotName = "screenshot";
			var titleBar = new TitleBar(appName, transparentWindowName, screenshotName);
			Assert.Equal(appName, titleBar.AppName);
			Assert.Equal("", titleBar.FileName);
			Assert.Equal(appName, titleBar.FullName);
			titleBar.FileName = "TEST.jpg";
			Assert.Equal("EP Calipers 3", titleBar.FullName);
			titleBar.Role = TitleBarRole.SinglePageFile;
			Assert.Equal("EP Calipers 3 - TEST.jpg", titleBar.FullName);
			titleBar.Role = TitleBarRole.TransparentWindow;
			Assert.Equal("EP Calipers 3 - transparent window", titleBar.FullName);
			titleBar.Role = TitleBarRole.Screenshot;
			Assert.Equal("EP Calipers 3 - screenshot", titleBar.FullName);
		}
	}
}
