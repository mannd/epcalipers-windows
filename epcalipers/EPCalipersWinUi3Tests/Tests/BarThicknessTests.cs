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
			var barThickness = new ScaledBarThickness(10, 2, false);
			Assert.Equal(10, barThickness.ScaledThickness());
			barThickness.DoScaling = true;
			Assert.Equal(5, barThickness.ScaledThickness());
		}
	}
}
