using EPCalipersCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace epcalipers.Tests
{
	[TestClass()]
	public class CaliperTests
	{
		[TestMethod()]
		public void BarCoordTest()
		{
			Caliper c = new Caliper();
			Assert.IsTrue(c.Bar1Position == 0);
			Assert.IsTrue(c.Bar2Position == 0);
			Assert.IsTrue(c.CrossbarPosition == 100);
			PointF p = new PointF(100, 50);
			Assert.IsTrue(c.BarCoord(p) == 100);
			c.Direction = CaliperDirection.Vertical;
			Assert.IsTrue(c.BarCoord(p) == 50);
		}

		[TestMethod()]
		public void NonNegativeBPMTest()
		{
			Caliper c = new Caliper();
			Calibration cal = new Calibration();
			cal.OriginalCalFactor = 1.0;
			cal.CurrentZoom = 1.0;
			cal.OriginalZoom = 1.0;
			cal.CalibrationString = "1000 msec";
			cal.Units = "msec";
			cal.Calibrated = true;
			c.CurrentCalibration = cal;
			c.Bar1Position = 1000.0F;
			c.Bar2Position = 2000.0F;
			Assert.AreEqual("1000 msec", c.TestMeasurement());
			c.Bar2Position = 3000.0f;
			Assert.AreEqual("2000 msec", c.TestMeasurement());
			c.Bar2Position = 0.0f;
			Assert.AreEqual("-1000 msec", c.TestMeasurement());
			c.Bar2Position = 1000.0F;
			Assert.AreEqual("0 msec", c.TestMeasurement());
			// Test BPM
			c.Bar2Position = 2000.0F;
			Assert.AreEqual("1000 msec", c.TestMeasurement());
			cal.DisplayRate = true;
			Assert.AreEqual("60 bpm", c.TestMeasurement());
			c.Bar2Position = 0.0F;
			cal.DisplayRate = false;
			Assert.AreEqual("-1000 msec", c.TestMeasurement());
			cal.DisplayRate = true;
			Assert.AreEqual("60 bpm", c.TestMeasurement());


		}

	}
}