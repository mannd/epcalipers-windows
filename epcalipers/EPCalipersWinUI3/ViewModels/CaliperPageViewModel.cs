using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Capture;
using Windows.UI;
using WinUIEx;

namespace EPCalipersWinUI3.ViewModels
{

    public partial class CaliperPageViewModel : BasePageViewModel
	{
		protected readonly CaliperCollection _caliperCollection;
		private ScrollViewer _scrollViewer;
		private ICaliperView _caliperView;

		public CaliperPageViewModel(ICaliperView caliperView, ScrollViewer scrollViewer)
		{
			_scrollViewer = scrollViewer;
			_caliperView = caliperView;
			_caliperCollection = new CaliperCollection(caliperView, defaultUnit: "points".GetLocalized(),
				defaultBpm: "bpm".GetLocalized());
			_caliperCollection.PropertyChanged += OnMyPropertyChanged;
			_caliperCollection.CaliperCollectionChangeHandler = CaliperCollectionChanged;
			IsTimeCalibrated = _caliperCollection.TimeCalibration?.IsCalibrated ?? false;
			IsCalibrated = _caliperCollection.IsCalibrated;
			IsNotCalibrating = !_caliperCollection.IsCalibrating;
			ShowCaliperMenuItems = CanAddCalipers();
			ShowSelectedCaliperMenuItems = CanActOnSelectedCaliper();
			ShowActOnCalipersMenuItems = CanActOnCalipers();
		}

		public void CaliperCollectionChanged(int numberOfCalipers)
		{
			HasCalipers = numberOfCalipers > 0;
			ShowActOnCalipersMenuItems = CanActOnCalipers();
		}

		private void OnMyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(CaliperCollection.SelectedCaliper))
			{ 
				ACaliperIsSelected = _caliperCollection.SelectedCaliper?.IsSelected ?? false;
				ShowSelectedCaliperMenuItems = CanActOnSelectedCaliper();
			}
			if (e.PropertyName == nameof(CaliperCollection.TimeCalibration))
			{
				IsTimeCalibrated = _caliperCollection.TimeCalibration?.IsCalibrated ?? false;
				IsCalibrated = _caliperCollection.IsCalibrated;
			}
			if (e.PropertyName == nameof(CaliperCollection.AmplitudeCalibration))
			{
				IsCalibrated = _caliperCollection.IsCalibrated;
			}
			if (e.PropertyName == nameof(CaliperCollection.IsCalibrating))
			{
				IsNotCalibrating = !_caliperCollection.IsCalibrating;
				ShowCaliperMenuItems = CanAddCalipers();
				ShowSelectedCaliperMenuItems = CanActOnSelectedCaliper();
				ShowActOnCalipersMenuItems = CanActOnCalipers();
			}
		}

		// TODO: Need to close measurement windows when switching between views.

		// DEFER: Add check marks to right click menu (like we have with Marching Calipers)?

		public virtual bool CanAddCalipers()
		{
			return IsNotCalibrating;
		}

		public virtual bool CanActOnSelectedCaliper()
		{
			return ACaliperIsSelected && IsNotCalibrating;
		}

		public virtual bool CanActOnCalipers()
		{
			return HasCalipers && IsNotCalibrating;
		}

		public bool IsCalibrating => _caliperCollection.IsCalibrating;

		public async Task<bool> WarnIfLocked(XamlRoot xamlRoot)
		{
			if (IsCalibrating)
			{
				var title = "OperationBlockedTitle".GetLocalized();
				var message = "OperationBlockedMessage".GetLocalized();
				var dialog = MessageHelper.CreateMessageDialog(title, message);
				dialog.XamlRoot = xamlRoot;
				await dialog.ShowAsync();
				return true;
			}
			else
			{
				return false;
			}
		}

		private Bounds ViewportBounds
		{
			get
			{
				if (_scrollViewer == null)
				{
					return _caliperView.Bounds;
				}
				var scrollW = _scrollViewer.ViewportWidth  / _scrollViewer.ZoomFactor;
				var scrollH = _scrollViewer.ViewportHeight / _scrollViewer.ZoomFactor;
				var caliperViewW = _caliperView.Bounds.Width;
				var caliperViewH = _caliperView.Bounds.Height;
				Debug.Print(new Bounds(Math.Min(scrollW, caliperViewW), Math.Min(scrollH, caliperViewH)).ToString());
				return new Bounds(Math.Min(scrollW, caliperViewW), Math.Min(scrollH, caliperViewH));
			}
		}

		private Point ViewportOffset
		{
			get
			{
				if (_scrollViewer == null)
				{
					return new Point();
				}
				return new Point(_scrollViewer.HorizontalOffset / _scrollViewer.ZoomFactor, _scrollViewer.VerticalOffset / _scrollViewer.ZoomFactor);
			}
		}

		private ViewportBoundsOffset _viewportBoundsOffset => new ViewportBoundsOffset(ViewportBounds, ViewportOffset);


		#region calipers
		public virtual void RefreshCalipers()
		{
			_caliperCollection.RefreshCalipers();
		}

		public Caliper GetCaliperAt(Point point)
		{
			return _caliperCollection.GetCaliperAt(point);
		}

		[RelayCommand]
		public virtual void AddTimeCaliper()
		{
			_caliperCollection.AddCaliper(CaliperType.Time, _viewportBoundsOffset);
		}

		[RelayCommand]
		public virtual void AddAmplitudeCaliper()
		{
			_caliperCollection.AddCaliper(CaliperType.Amplitude, _viewportBoundsOffset);
		}

		[RelayCommand]
		public virtual void AddAngleCaliper()
		{
			_caliperCollection.AddCaliper(CaliperType.Angle, _viewportBoundsOffset);
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

		public void ToggleMarchingCaliper(Point point)
		{
			CaliperIsMarching = _caliperCollection.ToggleMarchingCaliper(point);
		}

		public Color CurrentCaliperColorAt(Point point)
		{
			return _caliperCollection.CurrentCaliperColorAt(point);
		}

		public void SetCurrentCaliperColor(Point point, Color color)
		{
			_caliperCollection.SetCurrentCaliperColor(point, color);

		}

		public void ShowColorDialog(Point point)
		{
			_caliperCollection.ShowColorDialog(point);
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
			CaliperIsSelected = _caliperCollection.ToggleCaliperSelection(point);
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
			_caliperCollection.DragCaliperBar(point);
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
		private async Task MeanRateInterval()
		{
			await _caliperCollection.MeanRateInterval();
		}

		[RelayCommand]
		private async Task MeasureQtc()
		{
			await _caliperCollection.MeasureQtc();
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
			Debug.Print("moving left");
			_caliperCollection.MoveLeft();
		}

		[RelayCommand]
		private void MoveRight()
		{
			Debug.Print("moving right");
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

		[ObservableProperty]
		private bool isNearCaliperAllowDuringCalibration;

		[ObservableProperty]
		private bool caliperIsMarching;

		[ObservableProperty]
		private bool aCaliperIsSelected;

		[ObservableProperty]
		private bool hasCalipers;

		[ObservableProperty]
		private bool isTimeCalibrated;

		[ObservableProperty]
		private bool isCalibrated;

		[ObservableProperty]
		private bool isNotCalibrating;

		[ObservableProperty]
		private bool showCaliperMenuItems;

		[ObservableProperty]
		private bool showSelectedCaliperMenuItems;

		[ObservableProperty]
		private bool showActOnCalipersMenuItems;

		[ObservableProperty]
		private bool caliperIsSelected;

		#endregion
	}
}
