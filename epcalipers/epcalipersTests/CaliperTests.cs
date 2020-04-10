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
	}
}