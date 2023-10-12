using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EPCalipersWinUI3.Calipers;

namespace EPCalipersWinUi3Tests.Tests
{
	public class CaliperCollectionTests
	{
		[Fact]
		public void TestFilteredCollection()
		{
			// TODO: Add more caliper types and calipers.
			var caliperCollection = new CaliperCollection(null);
			var timeCaliper = new TimeCaliper(new Bounds(0, 100), new CaliperPosition(100, 100, 200), true);
			caliperCollection.Add(timeCaliper);
			var timeCalipers = caliperCollection.FilteredCalipers(CaliperType.Time);
			Assert.Single(timeCalipers);
			Assert.True(timeCalipers[0] == timeCaliper);
			var amplitudeCalipers = caliperCollection.FilteredCalipers(CaliperType.Amplitude);
			Assert.Empty(amplitudeCalipers);
		}
	}
}
