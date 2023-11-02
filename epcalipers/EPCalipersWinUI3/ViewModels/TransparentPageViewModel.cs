﻿
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Calipers;
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

namespace EPCalipersWinUI3.ViewModels
{
	public partial class TransparentPageViewModel
	{
		private readonly CaliperHelper _caliperHelper;

		public TransparentPageViewModel(ICaliperView caliperView)
		{
			var caliperCollection = new CaliperCollection(caliperView);
			_caliperHelper = new CaliperHelper(caliperView, caliperCollection);
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
		private static void ToggleTransparentWindow()
		{
			var mainWindow = (Application.Current as App)?.Window as MainWindow;
			mainWindow.SystemBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop()
				{ Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt };
			mainWindow.Navigate(typeof(MainPage));
		}

		[RelayCommand]
		private static void Exit() => CommandHelper.ApplicationExit();
	}
}