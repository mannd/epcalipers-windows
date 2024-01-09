using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EPCalipersWinUI3.Views;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;

namespace EPCalipersWinUi3Tests.Tests
{
    public class CaliperCollectionTests
	{
		[Fact]
		public void TestFilteredCollection()
		{
			// TODO: Add more caliper types and calipers.
			// TODO: CaliperCollection.Add only used in testing, not in app?
			var stubCaliperView = new FakeCaliperView();
			var stubSettings = new FakeSettings();
			var caliperCollection = new CaliperCollection(stubCaliperView, stubSettings);
			var timeCaliper = new TimeCaliper(new CaliperPosition(100, 100, 200), stubCaliperView, stubSettings, true);
			caliperCollection.Add(timeCaliper);
			var amplitudeCaliper = new AmplitudeCaliper(new CaliperPosition(100, 100, 200), stubCaliperView, stubSettings, true);
			caliperCollection.Add(amplitudeCaliper);
			var timeCalipers = caliperCollection.FilteredCalipers(CaliperType.Time);
			Assert.Single(timeCalipers);
			var amplitudeCalipers = caliperCollection.FilteredCalipers(CaliperType.Amplitude);
			Assert.Single(amplitudeCalipers);
			var angleCalipers = caliperCollection.FilteredCalipers(CaliperType.Angle);
			Assert.Empty(angleCalipers);
		}

		[Fact]
		public void TestSelectedCaliper()
		{
			var stubCaliperView = new FakeCaliperView();
			var stubSettings = new FakeSettings();
			var caliperCollection = new CaliperCollection(stubCaliperView, stubSettings);
			var timeCaliper = new TimeCaliper(new CaliperPosition(100, 100, 200), stubCaliperView, stubSettings, true);
			caliperCollection.Add(timeCaliper);
			var amplitudeCaliper = new AmplitudeCaliper(new CaliperPosition(100, 100, 200), stubCaliperView, stubSettings, true);
			caliperCollection.Add(amplitudeCaliper);
			var selectedCaliper = caliperCollection.SelectedCaliper;
			Assert.Null(selectedCaliper);
			timeCaliper.IsSelected = true;
			selectedCaliper = caliperCollection.SelectedCaliper;
			Assert.Equal(timeCaliper, selectedCaliper);
			var selectedCaliperType = caliperCollection.SelectedCaliperType;
			Assert.Equal(CaliperType.Time, selectedCaliperType);
			timeCaliper.IsSelected = false;
			selectedCaliper = caliperCollection.SelectedCaliper;
			Assert.Null(selectedCaliper);
			selectedCaliperType = caliperCollection.SelectedCaliperType;
			Assert.Equal(CaliperType.None, selectedCaliperType);
		}
	}
}
