using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using WinRT.Interop;

namespace EPCalipersWinUI3.Views
{
	public sealed partial class TransparentPage : Page
	{
		TransparentPageViewModel ViewModel { get; set; }
		private Point _rightClickPosition;


		public TransparentPage()
		{
			this.InitializeComponent();
			ViewModel = new TransparentPageViewModel(TransparentCaliperView);
			SizeChanged += TransparentPage_SizeChanged;
			ViewModel.SetTitleBarName("TransparentWindow".GetLocalized());
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel.RefreshCalipers();
			ViewModel.RestoreTitleBarName();
		}

		private void TransparentPage_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			Debug.WriteLine("size changed");
			ViewModel.ChangeBounds();
		}

		private bool pointerDown = false;
		private Point pointerPosition;

		private void CaliperGrider_Tapped(object sender, TappedRoutedEventArgs e)
		{
			var position = e.GetPosition(TransparentCaliperView);
			ViewModel.ToggleCaliperSelection(position);
		}

		private void CaliperGrider_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
		{
			var position = e.GetPosition(TransparentCaliperView);
			ViewModel.RemoveAtPoint(position);
		}

		private void CaliperGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var position = e.GetCurrentPoint(this.TransparentCaliperView);
			pointerPosition = position.Position;
			pointerDown = true;
			ViewModel.GrabCaliper(pointerPosition);
		}
		private void CaliperGrider_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (pointerDown) // && dragging caliper...
			{
				var position = e.GetCurrentPoint(this.TransparentCaliperView);
				if (position.Position.X < TransparentCaliperView.ActualWidth
					&& position.Position.Y < TransparentCaliperView.ActualHeight
					&& position.Position.Y > 0
					&& position.Position.X > 0)
				{
					ViewModel.DragCaliperComponent(position.Position);
				}
			}
		}
		private void CaliperGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			var position = e.GetPosition(TransparentCaliperView);
			_rightClickPosition = position;
			var caliper = ViewModel.GetCaliperAt(position);
			ViewModel.IsNearCaliper = caliper != null;
			if (caliper != null && caliper is TimeCaliper timeCaliper)
			{
				ViewModel.CaliperIsMarching = timeCaliper.IsMarching;
			}
			else
			{
				ViewModel.CaliperIsMarching = false;
			}
		}

		private void SelectComponent_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ToggleComponentSelection(_rightClickPosition);
		}
		private void SelectCaliper_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ToggleCaliperSelection(_rightClickPosition);
		}
		private void DeleteCaliper_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.DeleteCaliperAt(_rightClickPosition);
		}
		private void MarchingCaliper_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ToggleMarchingCaliper(_rightClickPosition);
		}
		private void ColorCaliper_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ShowColorDialog(_rightClickPosition);
		}
		private void CaliperGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			ViewModel.ReleaseGrabbedCaliper();
			pointerDown = false;
		}

		private async void About_Click(object sender, RoutedEventArgs e) => await CommandHelper.About(XamlRoot);
	}
}
