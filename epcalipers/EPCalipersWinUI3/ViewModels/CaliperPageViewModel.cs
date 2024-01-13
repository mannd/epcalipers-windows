using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using System.Threading.Tasks;
using Windows.Foundation;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class CaliperPageViewModel : ObservableObject
	{
		private readonly CaliperCollection _caliperCollection;

		public CaliperPageViewModel(ICaliperView caliperView)
		{
			_caliperCollection = new CaliperCollection(caliperView, defaultUnit: "points".GetLocalized(),
				defaultBpm: "bpm".GetLocalized());
		}

		#region calipers
		public void RefreshCalipers()
		{
			_caliperCollection.RefreshCalipers();
		}

		public bool PointIsNearCaliper(Point point)
		{
			return _caliperCollection.PointIsNearCaliper(point);
		}

		[RelayCommand]
		public void AddTimeCaliper()
		{
			_caliperCollection.AddCaliper(CaliperType.Time);
		}

		[RelayCommand]
		public void AddAmplitudeCaliper()
		{
			_caliperCollection.AddCaliper(CaliperType.Amplitude);
		}

		[RelayCommand]
		public void AddAngleCaliper()
		{
			_caliperCollection.AddCaliper(CaliperType.Angle);
		}

		[RelayCommand]
		public void DeleteAllCalipers()
		{
			_caliperCollection.Clear();
		}
		public void DeleteCaliperAt(Point point)
		{
			_caliperCollection.DeleteCaliperAt(point);
		}

		[RelayCommand]
		public void UnselectAllCalipers()
		{
			_caliperCollection?.UnselectAllCalipers();
		}

		[RelayCommand]
		public void DeleteSelectedCaliper()
		{
			_caliperCollection.RemoveActiveCaliper();
		}

		public void ToggleCaliperSelection(Point point)
		{
			_caliperCollection.ToggleCaliperSelection(point);
		}

		[RelayCommand]
		public void ToggleComponentSelection(Point point)
		{
			_caliperCollection.ToggleComponentSelection(point);
		}

		public void RemoveAtPoint(Point point)
		{
			_caliperCollection.RemoveAtPoint(point);
		}

		public void GrabCaliper(Point point)
		{
			_caliperCollection.GrabCaliper(point);
		}

		public void DragCaliperComponent(Point point)
		{
			_caliperCollection.DragCaliperComponent(point);
		}

		public void ReleaseGrabbedCaliper()
		{
			_caliperCollection.ReleaseGrabbedCaliper();
		}
		public void ChangeBounds()
		{
			_caliperCollection.ChangeBounds();
		}
		#endregion 

		#region menu comands
		[RelayCommand]
		private async Task ToggleRateInterval()
		{
			await _caliperCollection.ToggleRateInterval();
		}

		[RelayCommand]
		private static void ShowSettings() => AppHelper.Navigate(typeof(SettingsPage));

		[RelayCommand]
		private static void Help() => AppHelper.Navigate(typeof(HelpWebViewPage));

		[RelayCommand]
		private static void Exit() => CommandHelper.ApplicationExit();

		[RelayCommand]
		public async Task SetCalibrationAsync()
		{
			await _caliperCollection.SetCalibrationAsync();
		}

		[RelayCommand]
		private void ClearCalibration()
		{
			_caliperCollection.ClearCalibration();
		}
		#endregion

		#region movement
		[RelayCommand]
		private void MoveLeft()
		{
			_caliperCollection.MoveLeft();
		}

		[RelayCommand]
		private void MoveRight()
		{
			_caliperCollection.MoveRight();
		}

		[RelayCommand]
		private void MoveUp()
		{
			_caliperCollection.MoveUp();
		}

		[RelayCommand]
		private void MoveDown()
		{
			_caliperCollection.MoveDown();
		}

		[RelayCommand]
		private void MicroMoveLeft()
		{
			_caliperCollection.MicroMoveLeft();
		}

		[RelayCommand]
		private void MicroMoveRight()
		{
			_caliperCollection.MicroMoveRight();
		}

		[RelayCommand]
		private void MicroMoveUp()
		{
			_caliperCollection.MicroMoveUp();
		}

		[RelayCommand]
		private void MicroMoveDown()
		{
			_caliperCollection.MicroMoveDown();
		}
		#endregion

		#region observable properties
		[ObservableProperty]
		private bool isNearCaliper;
		#endregion
	}
}
