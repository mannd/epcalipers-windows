using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using Windows.Foundation;
using Xunit;

namespace EPCalipersWinUi3Tests.Tests
{
	public class CaliperCollectionTests
	{
		[Fact]
		public void TestFilteredCollection()
		{
			// TODO: CaliperCollection.Add only used in testing, not in app?
			var stubCaliperView = new FakeCaliperView();
			var stubSettings = new FakeSettings();
			var caliperCollection = new CaliperCollection(stubCaliperView, stubSettings);
			caliperCollection.AddCaliper(CaliperType.Time, true);
			caliperCollection.AddCaliper(CaliperType.Amplitude, true);
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
			// TODO: redo this
			//var stubCaliperView = new FakeCaliperView();
			//var stubSettings = new FakeSettings();
			//var caliperCollection = new CaliperCollection(stubCaliperView, stubSettings);
			//var timeCaliper = new TimeCaliper(new CaliperPosition(100, 100, 200), stubCaliperView, stubSettings, true);
			//caliperCollection.Add(timeCaliper);
			//var amplitudeCaliper = new AmplitudeCaliper(new CaliperPosition(100, 100, 200), stubCaliperView, stubSettings, true);
			//caliperCollection.Add(amplitudeCaliper);
			//var selectedCaliper = caliperCollection.SelectedCaliper;
			//Assert.Null(selectedCaliper);
			//timeCaliper.IsSelected = true;
			//selectedCaliper = caliperCollection.SelectedCaliper;
			//Assert.Equal(timeCaliper, selectedCaliper);
			//var selectedCaliperType = caliperCollection.SelectedCaliperType;
			//Assert.Equal(CaliperType.Time, selectedCaliperType);
			//timeCaliper.IsSelected = false;
			//selectedCaliper = caliperCollection.SelectedCaliper;
			//Assert.Null(selectedCaliper);
			//selectedCaliperType = caliperCollection.SelectedCaliperType;
			//Assert.Equal(CaliperType.None, selectedCaliperType);
		}

		[Fact]
		public void TestSelectBar()
		{
			var stubCaliperView = new FakeCaliperView();
			var stubSettings = new FakeSettings();
			var caliperCollection = new CaliperCollection(stubCaliperView, stubSettings);
			var timeCaliper = (TimeCaliper)caliperCollection.AddCaliper(CaliperType.Time, true);
			var amplitudeCaliper = (AmplitudeCaliper)caliperCollection.AddCaliper(CaliperType.Amplitude, true);
			timeCaliper.SelectFullCaliper();
			Assert.True(timeCaliper.IsSelected);  // toggle bar unselects caliper
			caliperCollection.ToggleCaliperSelection(new Point(timeCaliper.LeftBar.Position, 0));
			Assert.False(timeCaliper.IsSelected);  // toggle bar unselects caliper
			// TODO: finish this
			//Assert.True(timeCaliper.LeftBar.IsSelected);
			//caliperCollection.ToggleComponentSelection(new Point(timeCaliper.RightBar.Position, 0));
			//Assert.False(timeCaliper.LeftBar.IsSelected);  // toggle other bar unselects first bar
			//Assert.True(timeCaliper.RightBar.IsSelected);
			//caliperCollection.ToggleComponentSelection(new Point(timeCaliper.RightBar.Position, 0));
			//Assert.False(timeCaliper.LeftBar.IsSelected);  // toggling bar again unselects it
			//Assert.False(timeCaliper.RightBar.IsSelected);
		}
	}
}
