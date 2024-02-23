using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using System.Diagnostics;

namespace EPCalipersWinUi3Tests.Tests
{
	public class QTcTests
	{
		[Fact]
		public void TestQtcFormulas()
		{
			//var calculator = new MathHelper.QtcCalculator(MathHelper.QtcFormula.qtcBzt);
			//var calibration = new Calibration();
			//Assert.Equal("700 msec", calculator.Calculate(100, 200, false, "msec"));
			var result = MathHelper.QtcCalculator.QtcBazettMsec(345, 879);
			Assert.Equal(368, Math.Round(result));
			result = MathHelper.QtcCalculator.QtcBazettMsec(499, 999);
			Assert.Equal(499, Math.Round(result));
			result = MathHelper.QtcCalculator.QtcBazettMsec(459, 777);
			Assert.Equal(521, Math.Round(result));
			var calculator = new MathHelper.QtcCalculator(MathHelper.QtcFormula.qtcBzt);
			var calibration = new Calibration();
			var formattedResult = calculator.Calculate(0.345, 0.879, true, "msec");
			Assert.Equal(
				"RR interval = 879 msec\nQT interval = 345 msec\nQTc = 368 msec (Bazett formula)", 
				formattedResult);
			result = MathHelper.QtcCalculator.QtcBazettSec(0.345, 0.879);
			Assert.Equal(0.368, result, 0.001);
			result = MathHelper.QtcCalculator.QtcFrdSec(0.345, 0.879);
			Assert.Equal(0.3602, result, 0.001);
			result = MathHelper.QtcCalculator.QtcHdgSec(0.499, 0.631);
			Assert.Equal(0.5604, result, 0.001);
			result = MathHelper.QtcCalculator.QtcFrmSec(0.459, 0.777);
			Assert.Equal(0.4933, result, 0.001);
		}
	}
}
