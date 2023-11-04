using EPCalipersWinUI3.Helpers;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using EPCalipersWinUI3.Calipers;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using WinUIEx;
using Microsoft.UI.Windowing;
using TemplateTest2.Helpers;

namespace EPCalipersWinUI3.Views
{
    public sealed partial class MainPage : Page
	{
        public MainPageViewModel ViewModel { get; set; }

		public MainPage()
		{
			this.InitializeComponent();
            ViewModel = new MainPageViewModel(SetZoom, CaliperView);

			// TODO: make this a setting?  Note that left/top alignment avoids image shifting, and
			// so should be the default.
			CaliperView.HorizontalAlignment = HorizontalAlignment.Left;
			CaliperView.VerticalAlignment = VerticalAlignment.Top;

            ScrollView.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, (s, e) =>
            {
				ViewModel.ZoomFactor = ScrollView.ZoomFactor;
            });

            EcgImage.RegisterPropertyChangedCallback(Image.SourceProperty, (s, e) =>
            {
				ViewModel.Bounds = CaliperView.Bounds;
				ViewModel.DeleteAllCalipersCommand.Execute(null);


				// TODO: Change to settings.  Settings.ResetZoomWithNewImage, etc.
				if (ViewModel.ResetZoomWithNewImage)
				{
					ViewModel.ResetZoomCommand.Execute(null);
				}
				if (ViewModel.ResetRotationWithNewImage) {
					RotateImageWithoutAnimation(0);
				} 
				else
				{
					var originalRotation = _imageRotation;
					RotateImageWithoutAnimation(originalRotation);
				}
            });
        }

		#region touches
		private bool pointerDown = false;
		private Point pointerPosition;

		private void ScrollViewer_Tapped(object sender, TappedRoutedEventArgs e)
		{
			var position = e.GetPosition(CaliperView);
			ViewModel.ToggleCaliperSelection(position);
		}

		private void ScrollViewer_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
		{
			var position = e.GetPosition(CaliperView);
			ViewModel.RemoveAtPoint(position);
		}

		private void ScrollView_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var position = e.GetCurrentPoint(this.CaliperView);
			pointerPosition = position.Position;
			pointerDown = true;
			ViewModel.GrabCaliper(pointerPosition);
		}

		private void ScrollViewer_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (pointerDown) // && dragging caliper...
			{
				var position = e.GetCurrentPoint(this.CaliperView);
				if (position.Position.X < EcgImage.ActualWidth 
					&& position.Position.Y < EcgImage.ActualHeight
					&& position.Position.Y > 0
					&& position.Position.X > 0) {
					ViewModel.DragCaliperComponent(position.Position);
				}
			}
		}

		private void ScrollView_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("Pointer released.");
			ViewModel.ReleaseGrabbedCaliper();
			pointerDown = false;
		}
		#endregion
		#region zoom
		/// <summary>
		/// Delegate method that view model uses to set zoom on scroll view
		/// </summary>
		/// <param name="zoom"></param>
        private void SetZoom(float zoom)
        {
            ScrollView?.ChangeView(0, 0, zoom);
        }
		#endregion
		#region drag and drop
		private async void EcgImage_Drop(object sender, DragEventArgs e)
		{
			if (e.DataView.Contains(StandardDataFormats.StorageItems))
			{
				var items = await e.DataView.GetStorageItemsAsync();
				if (items.Count > 0)
				{
					var storageFile = items[0] as StorageFile;
                    // check file types first???
                    await ViewModel.OpenImageFile(storageFile);
                }
			}
		}

		private void EcgImage_DragOver(object sender, DragEventArgs e)
		{
            e.AcceptedOperation = DataPackageOperation.Link; 
            e.Handled = true;
		}
		#endregion
		#region rotation
		// Note rotation is handled in the code behind file because it is purely a
		// manipulation of the view, with no affect on the view model.

        private readonly static TimeSpan _rotationDuration = TimeSpan.FromSeconds(0.4);
        private double _imageRotation = 0;
		private double _rotatedImageScale = 1.0;

		private void Rotate90R_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(90);
		}

		private void Rotate90L_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(-90);
		}

		private void Rotate1R_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(1);
		}

		private void Rotate1L_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(-1);
		}

		private void Rotate01R_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(0.1);
		}

		private void Rotate01L_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(-0.1);
		}

		private void ResetRotation_Click(object sender, RoutedEventArgs e)
		{
            RotateImageToAngle(0);
		}

		private void RotateImageByAngle(double angle)
		{
			var originalRotation = _imageRotation;
			_imageRotation += angle; 
			RotateImage(originalRotation, _imageRotation);
		}

        private void RotateImageToAngle(double angle)
        {
			var originalRotation = _imageRotation;
			_imageRotation = angle; 
			RotateImage(originalRotation, _imageRotation);
        }

		private void RotateImage(double startAngle, double endAngle)
		{
            EcgImage.RenderTransformOrigin = new Point(0.5, 0.5);
			Storyboard storyboard = new()
			{
				Duration = new Duration(_rotationDuration)
			};
			DoubleAnimation rotateAnimation = new()
			{
				From = startAngle,
				To = endAngle,
				Duration = storyboard.Duration
			};

			var scaledWidth = _rotatedImageScale * EcgImage.ActualWidth;
			var scaledHeight = _rotatedImageScale * EcgImage.ActualHeight; ;
			_rotatedImageScale = MathHelper.ScaleToFit(scaledWidth, scaledHeight, _imageRotation);

			DoubleAnimation scaleAnimation = new()
			{
				Duration = storyboard.Duration,
				To = _rotatedImageScale
			};
			DoubleAnimation scaleYAnimation = new()
			{
				Duration = storyboard.Duration,
				To = _rotatedImageScale
			};
			Storyboard.SetTarget(scaleAnimation, EcgImage);
			Storyboard.SetTarget(scaleYAnimation, EcgImage);
			Storyboard.SetTarget(rotateAnimation, EcgImage);
			Storyboard.SetTargetProperty(scaleAnimation,
				"(UIElement.RenderTransform).(CompositeTransform.ScaleX)");
			Storyboard.SetTargetProperty(scaleYAnimation,
				"(UIElement.RenderTransform).(CompositeTransform.ScaleY)");
			Storyboard.SetTargetProperty(rotateAnimation,
				"(UIElement.RenderTransform).(CompositeTransform.Rotation)");
			storyboard.Children.Add(scaleAnimation);
			storyboard.Children.Add(scaleYAnimation);
			storyboard.Children.Add(rotateAnimation);
			storyboard.Begin();
		}

		private void RotateImageWithoutAnimation(double angle)
		{
			if (EcgImage?.Source  == null) { return; }
			_imageRotation = angle;
            EcgImage.RenderTransformOrigin = new Point(0.5, 0.5);
            CompositeTransform rotateTransform = new()
            {
                CenterX = EcgImage.Width / 2,
                CenterY = EcgImage.Height / 2,
				Rotation = _imageRotation,
            };
			EcgImage.RenderTransform = rotateTransform;
		}
		#endregion
		#region event handlers
		// Events that open dialogs are handled in the code behind file.

		private async void About_Click(object sender, RoutedEventArgs e) => await CommandHelper.About(XamlRoot);

		private async void GotoPdfPage_Click(object sender, RoutedEventArgs e)
		{
			var gotoPdfPageDialog = new GotoPdfPageDialog(ViewModel)
			{
				XamlRoot = XamlRoot
			};
			var result = await gotoPdfPageDialog.ShowAsync();
			Debug.WriteLine(result);
			if (result == ContentDialogResult.Primary)
			{
				var page = gotoPdfPageDialog.PageNumber;
				await ViewModel.GotoPdfPage(page);
			}
		}

		private async void OpenFile_Click(object sender, RoutedEventArgs e)
		{
			// Create a file picker
			var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

			// Retrieve the window handle (HWND) of the current WinUI 3 window.
			var mainWindow = AppHelper.AppMainWindow;
			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(mainWindow);

			// Initialize the file picker with the window handle (HWND).
			WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

			// Set options for your file picker
			openPicker.ViewMode = PickerViewMode.Thumbnail;
			openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
			openPicker.FileTypeFilter.Add(".jpg");
			openPicker.FileTypeFilter.Add(".jpeg");
			openPicker.FileTypeFilter.Add(".png");
			openPicker.FileTypeFilter.Add(".pdf");

			// Open the picker for the user to pick a file
			var file = await openPicker.PickSingleFileAsync();
			// Change the cursor to a wait icon
			CaliperView.InputCursor = InputSystemCursor.Create(InputSystemCursorShape.Wait);
			await ViewModel.OpenImageFile(file);
			AppHelper.AppTitleBarText = ViewModel.TitleBarName;
			CaliperView.InputCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
		}
		#endregion
	}
}
