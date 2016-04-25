using Microsoft.VisualStudio.TestTools.UnitTesting;
using epcalipers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace epcalipers.Tests
{
    [TestClass()]
    public class CaliperTests
    {
        [TestMethod()]
        public void barCoordTest()
        {
            Caliper c = new Caliper();
            Assert.IsTrue(c.bar1Position == 0);
            Assert.IsTrue(c.bar2Position == 0);
            Assert.IsTrue(c.crossbarPosition == 100);
            PointF p = new PointF(100, 50);
            Assert.IsTrue(c.barCoord(p) == 100);
            c.direction = CaliperDirection.Vertical;
            Assert.IsTrue(c.barCoord(p) == 50);
        }
    }
}