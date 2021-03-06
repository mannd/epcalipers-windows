﻿using EPCalipersCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace epcalipers.Tests
{
	[TestClass()]
	public class CalibrationTests
	{
		[TestMethod()]
		public void CalibrationTest()
		{
			Calibration cal = new Calibration
			{
				Calibrated = true,
				Units = "msec"
			};
			Assert.IsTrue(cal.CanDisplayRate);
			cal.Units = "milliseconds";
			Assert.IsTrue(cal.CanDisplayRate);
			cal.Units = "sec";
			Assert.IsTrue(cal.CanDisplayRate);
			cal.Units = "secs";
			Assert.IsTrue(cal.CanDisplayRate);
			cal.Units = "Msec";
			Assert.IsTrue(cal.CanDisplayRate);
			cal.Units = "ms";
			Assert.IsTrue(cal.CanDisplayRate);
			cal.Units = "mm";
			Assert.IsFalse(cal.CanDisplayRate);
			cal.Units = "mSecs";
			Assert.IsTrue(cal.CanDisplayRate);
			cal.Direction = CaliperDirection.Vertical;
			Assert.IsFalse(cal.CanDisplayRate);
		}



		[TestMethod()]
		public void initWithDirectionTest()
		{
			Calibration cal = new Calibration();
			Assert.IsTrue(cal.Direction == CaliperDirection.Horizontal);
			Assert.IsTrue(cal.Units == "points");
			Assert.IsFalse(cal.DisplayRate);
			Assert.IsFalse(cal.CanDisplayRate);
		}

		[TestMethod()]
		public void currentHorizontalCalFactorTest()
		{
			Calibration cal = new Calibration
			{
				OriginalZoom = 1.0f,
				OriginalCalFactor = 0.5f,
				CurrentZoom = 1.0f
			};
			Assert.IsTrue(cal.CurrentCalFactor == 0.5f);
			cal.CurrentZoom = 2.0f;
			Assert.IsTrue(cal.CurrentCalFactor == 0.25f);
		}

		[TestMethod()]
		public void complexCalibrationTest()
		{
			Calibration cal = new Calibration
			{
				Units = "Seconds",
				DisplayRate = true
			};
			Assert.IsTrue(cal.Units == "points");
			cal.Calibrated = true;
			Assert.IsTrue(cal.Units == "bpm");
			cal.Calibrated = false;
			cal.DisplayRate = false;
			Assert.IsTrue(cal.Units == "points");
			cal.Calibrated = true;
			Assert.IsTrue(cal.Units == "Seconds");
		}

		[TestMethod()]
		public void roundingTest()
		{
			double n = 455.5;
			Assert.IsTrue(System.Math.Round(n) == 456);
			n = 455.49;
			Assert.IsTrue(System.Math.Round(n) == 455);
		}
	}
}