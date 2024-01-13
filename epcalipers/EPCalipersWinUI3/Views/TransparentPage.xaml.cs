using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.Foundation;

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
			AppHelper.SaveTitleBarText();
			AppHelper.AppTitleBarText = "AppTransparentWindowTitle".GetLocalized();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel.RefreshCalipers();
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
					&& position.Position.X > 0) {
					ViewModel.DragCaliperComponent(position.Position);
				}
			}
		}
		private void CaliperGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			var position = e.GetPosition(TransparentCaliperView);
			_rightClickPosition = position;
			ViewModel.IsNearCaliper = ViewModel.PointIsNearCaliper(position);

		}
		private void Right_Click(object sender, RoutedEventArgs e)
		{
			Debug.Print("right click menu item");
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
		private void CaliperGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("Pointer released.");
			ViewModel.ReleaseGrabbedCaliper();
			pointerDown = false;
		}

		private async void About_Click(object sender, RoutedEventArgs e) => await CommandHelper.About(XamlRoot);

	}
}
