using Microsoft.VisualStudio.TestTools.UnitTesting;
using epcalipers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers.Tests
{
    [TestClass()]
    public class CalibrationTests
    {
        [TestMethod()]
        public void CalibrationTest()
        {
            Calibration cal = new Calibration();
            cal.calibrated = true;
            cal.units = "msec";
            Assert.IsTrue(cal.canDisplayRate);
            cal.units = "milliseconds";
            Assert.IsTrue(cal.canDisplayRate);
            cal.units = "sec";
            Assert.IsTrue(cal.canDisplayRate);
            cal.units = "secs";
            Assert.IsTrue(cal.canDisplayRate);
            cal.units = "Msec";
            Assert.IsTrue(cal.canDisplayRate);
            cal.units = "ms";
            Assert.IsTrue(cal.canDisplayRate);
            cal.units = "mm";
            Assert.IsFalse(cal.canDisplayRate);
            cal.units = "mSecs";
            Assert.IsTrue(cal.canDisplayRate);
            cal.direction = CaliperDirection.Vertical;
            Assert.IsFalse(cal.canDisplayRate);
        }



        [TestMethod()]
        public void initWithDirectionTest()
        {
            Calibration cal = new Calibration();
            Assert.IsTrue(cal.direction == CaliperDirection.Horizontal);
            Assert.IsTrue(cal.units == "points");
            Assert.IsFalse(cal.displayRate);
            Assert.IsFalse(cal.canDisplayRate);
        }

        [TestMethod()]
        public void currentHorizontalCalFactorTest()
        {
            Calibration cal = new Calibration();
            cal.originalZoom = 1.0f;
            cal.originalCalFactor = 0.5f;
            cal.currentZoom = 1.0f;
            Assert.IsTrue(cal.currentCalFactor == 0.5f);
            cal.currentZoom = 2.0f;
            Assert.IsTrue(cal.currentCalFactor == 0.25f);
        }

        [TestMethod()]
        public void complexCalibrationTest()
        {
            Calibration cal = new Calibration();
            cal.units = "Seconds";
            cal.displayRate = true;
            Assert.IsTrue(cal.units == "points");
            cal.calibrated = true;
            Assert.IsTrue(cal.units == "bpm");
            cal.calibrated = false;
            cal.displayRate = false;
            Assert.IsTrue(cal.units == "points");
            cal.calibrated = true;
            Assert.IsTrue(cal.units == "Seconds");
        }
    }
}