using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Calipers;
using EPCalipersWinUI3.Helpers;
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
