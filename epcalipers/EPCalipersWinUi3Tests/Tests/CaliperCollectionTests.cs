using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EPCalipersWinUI3.Calipers;
using EPCalipersWinUI3.Views;

namespace EPCalipersWinUi3Tests.Tests
{
	public class CaliperCollectionTests
	{
		[Fact]
		public void TestFilteredCollection()
		{
			// TODO: Add more caliper types and calipers.
			var stubCaliperView = new FakeCaliperView();
			var caliperCollection = new CaliperCollection(stubCaliperView);
			var timeCaliper = new TimeCaliper(new CaliperPosition(100, 100, 200), stubCaliperView, true);
			caliperCollection.Add(timeCaliper);
			var amplitudeCaliper = new AmplitudeCaliper(new CaliperPosition(100, 100, 200), stubCaliperView, true);
			caliperCollection.Add(amplitudeCaliper);
			var timeCalipers = caliperCollection.FilteredCalipers(CaliperType.Time);
			Assert.Single(timeCalipers);
			var amplitudeCalipers = caliperCollection.FilteredCalipers(CaliperType.Amplitude);
			Assert.Single(amplitudeCalipers);
			var angleCalipers = caliperCollection.FilteredCalipers(CaliperType.Angle);
			Assert.Empty(angleCalipers);
		}
	}
}
