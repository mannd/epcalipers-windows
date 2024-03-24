using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EPCalipersWinUI3;
using EPCalipersWinUI3.Models.Calipers;

namespace EPCalipersWinUi3Tests.Tests
{
	public class BarThicknessTests
	{
		[Fact]
		public void TestBarThickness()
		{
			var barThickness = new BarThickness(100, 2, false);
			Assert.Equal(100, barThickness.ScaledThickness());
			barThickness.ScaleThickness = true;
			Assert.Equal(50, barThickness.ScaledThickness());
		}
	}
}
