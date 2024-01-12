﻿
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
		private readonly CaliperHelper _caliperHelper;

		public TransparentPageViewModel(ICaliperView caliperView)
		{
			var caliperCollection = new CaliperCollection(caliperView);
			_caliperHelper = new CaliperHelper(caliperCollection);
		}

		public void RefreshCalipers()
		{
			_caliperHelper.RefreshCalipers();
		}

		[RelayCommand]
		public void AddTimeCaliper()
		{
			_caliperHelper.AddTimeCaliper();
		}

		[RelayCommand]
		public void AddAmplitudeCaliper()
		{
			_caliperHelper.AddAmplitudeCaliper();
		}

		[RelayCommand]
		public void AddAngleCaliper()
		{
			_caliperHelper.AddAngleCaliper();
		}

		[RelayCommand]
		public void DeleteAllCalipers()
		{
			_caliperHelper.DeleteAllCalipers();
		}

		[RelayCommand]
		public void UnselectAllCalipers()
		{
			_caliperHelper?.UnselectAllCalipers();
		}

		[RelayCommand]
		public void DeleteSelectedCaliper()
		{
			_caliperHelper.DeleteSelectedCaliper();
		}

		public void ToggleCaliperSelection(Point point)
		{
			_caliperHelper.ToggleCaliperSelection(point);
		}
		public void RemoveAtPoint(Point point)
		{
			_caliperHelper.RemoveAtPoint(point);
		}

		public void GrabCaliper(Point point)
		{
			_caliperHelper.GrabCaliper(point);
		}

		public void DragCaliperComponent(Point point)
		{
			_caliperHelper.DragCaliperComponent(point);
		}

		public void ReleaseGrabbedCaliper()
		{
			_caliperHelper.ReleaseGrabbedCaliper();
		}

		public void ChangeBounds()
		{
			_caliperHelper.ChangeBounds();
		}

		[RelayCommand]
		private async Task ToggleRateInterval()
		{
			await _caliperHelper.ToggleRateInterval();
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
			await _caliperHelper.SetCalibrationAsync();
		}

		[RelayCommand]
		private void ClearCalibration()
		{
			_caliperHelper.ClearCalibration();
		}


		#region movement
		[RelayCommand]
		private void MoveLeft()
		{
			_caliperHelper.MoveLeft();
		}

		[RelayCommand]
		private void MoveRight()
		{
			_caliperHelper.MoveRight();
		}

		[RelayCommand]
		private void MoveUp()
		{
			_caliperHelper.MoveUp();
		}

		[RelayCommand]
		private void MoveDown()
		{
			_caliperHelper.MoveDown();
		}

		[RelayCommand]
		private void MicroMoveLeft()
		{
			_caliperHelper.MicroMoveLeft();
		}

		[RelayCommand]
		private void MicroMoveRight()
		{
			_caliperHelper.MicroMoveRight();
		}

		[RelayCommand]
		private void MicroMoveUp()
		{
			_caliperHelper.MicroMoveUp();
		}

		[RelayCommand]
		private void MicroMoveDown()
		{
			_caliperHelper.MicroMoveDown();
		}

		#endregion movement
	}
}
