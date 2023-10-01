using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Helpers;
using Xunit;



namespace EPCalipersWinUi3Tests.Tests
{
	public class MathTests
	{
		[Fact]
		public void TestScaleRectangle()
		{
			var scale0 = MathHelper.ScaleRectangleToFit(100, 50, 90);
			Assert.True(scale0 < 1.0);
			var scale1 = MathHelper.ScaleRectangleToFit(50, 100, 90);
			Assert.True(scale1 >= 1.0);
		}
	}
}
