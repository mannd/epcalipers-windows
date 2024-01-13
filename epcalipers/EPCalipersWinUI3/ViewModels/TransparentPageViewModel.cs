
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using EPCalipersWinUI3.Views;
using EPCalipersWinUI3.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class TransparentPageViewModel: ObservableObject
	{
		private readonly CaliperCollection _caliperCollection;

		public TransparentPageViewModel(ICaliperView caliperView)
		{
			_caliperCollection = new CaliperCollection(caliperView, defaultUnit: "points".GetLocalized(),
				defaultBpm: "bpm".GetLocalized());
		}

		public void RefreshCalipers()
		{
			_caliperCollection.RefreshCalipers();
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

		[RelayCommand]
		private async Task ToggleRateInterval()
		{
			await _caliperCollection.ToggleRateInterval();
		}

		[RelayCommand]
		private static void ToggleTransparentWindow()
		{
			var mainWindow = AppHelper.AppMainWindow;
			mainWindow.SystemBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop()
				{ Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt };
			AppHelper.SaveTitleBarText();
			mainWindow.Navigate(typeof(MainPage));
		}

		[RelayCommand]
		private static void ShowSettings() => AppHelper.Navigate(typeof(SettingsPage));

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

		#endregion movement
	}
}
