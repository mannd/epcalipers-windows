using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using WinRT.Interop;
using WinUIEx;

namespace EPCalipersWinUI3.Views
{
	public sealed partial class MainPage : Page
	{
		public MainPageViewModel ViewModel { get; set; }
		private Point _rightClickPosition;

		private Windows.Win32.Graphics.Direct3D11.ID3D11Device _d3dDevice;
		private IDirect3DDevice _device;

		public MainPage()
		{
			InitializeComponent();
			ViewModel = new MainPageViewModel(SetZoom, CaliperView);

			_d3dDevice = Direct3D11Helper.CreateD3DDevice();
			_device = Direct3D11Helper.CreateDirect3DDeviceFromD3D11Device(_d3dDevice);

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
				if (ViewModel.ResetRotationWithNewImage)
				{
					RotateImageWithoutAnimation(0);
				}
				else
				{
					var originalRotation = _imageRotation;
					RotateImageWithoutAnimation(originalRotation);
				}
			});
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel.RefreshCalipers();
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

		private void ScrollView_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			var position = e.GetPosition(CaliperView);
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
		private void MarchingCaliper_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ToggleMarchingCaliper(_rightClickPosition);
		}
		private void ColorCaliper_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ShowColorDialog(_rightClickPosition);
		}

		private void ScrollView_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var position = e.GetCurrentPoint(CaliperView);
			pointerPosition = position.Position;
			pointerDown = true;
			ViewModel.GrabCaliper(pointerPosition);
		}

		private void ScrollViewer_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (pointerDown) // && dragging caliper...
			{
				var position = e.GetCurrentPoint(CaliperView);
				if (position.Position.X < EcgImage.ActualWidth
					&& position.Position.Y < EcgImage.ActualHeight
					&& position.Position.Y > 0
					&& position.Position.X > 0)
				{
					ViewModel.DragCaliperComponent(position.Position);
				}
			}
		}

		private void ScrollView_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
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
			if (EcgImage?.Source == null) { return; }
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
		//private async void StartCaptureFromItem(GraphicsCaptureItem item)
		//{
		//	var surface = await CaptureSnapshot.CaptureAsync(_device, item);
		//	var softwareBitmap = await SoftwareBitmap.CreateCopyFromSurfaceAsync(surface, BitmapAlphaMode.Premultiplied);

		//	var source = new SoftwareBitmapSource();
		//	await source.SetBitmapAsync(softwareBitmap);

		//	//ScreenshotImage.Source = source;
		//}
		private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
		{
			if (GraphicsCaptureSession.IsSupported())
			{
				IntPtr hwnd = WindowNative.GetWindowHandle(AppHelper.AppMainWindow);
				var picker = new GraphicsCapturePicker();
				InitializeWithWindow.Initialize(picker, hwnd);
				// Use picker to load any screen window into app
				//var capturedItem = await picker.PickSingleItemAsync();
				// This just gets the EP Calipers window
				var capturedItem = CaptureSnapshot.CreateItemForWindow((Windows.Win32.Foundation.HWND)hwnd);
				if (capturedItem != null)
				{
					var surface =	await CaptureSnapshot.CaptureAsync(_device, capturedItem);
					var softwareBitmap = await SoftwareBitmap.CreateCopyFromSurfaceAsync(surface, BitmapAlphaMode.Premultiplied);

					var source = new SoftwareBitmapSource();
					await source.SetBitmapAsync(softwareBitmap);

					// TODO: change this to save image to a file!!!
					ViewModel.MainImageSource = source;

				}
			} // else error message
		
			return;

			// Clear previous returned file name, if it exists, between iterations of this scenario
			//SaveFileOutputTextBlock.Text = "";

			// Create a file picker
			FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();

			// See the sample code below for how to make the window accessible from the App class.
			var window = AppHelper.AppMainWindow;

			// Retrieve the window handle (HWND) of the current WinUI 3 window.
			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

			// Initialize the file picker with the window handle (HWND).
			WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

			// Set options for your file picker
			savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
			// Dropdown of file types the user can save the file as
			savePicker.FileTypeChoices.Add("JPG image", new List<string>() { ".jpg" });
			// Default file name if the user does not type one in or select a file to replace
			//var enteredFileName = ((sender as Button).Parent as StackPanel)
			//.FindName("FileNameTextBox") as TextBox;
			savePicker.SuggestedFileName = "EPCsavedimage.jpg";

			// Open the picker for the user to pick a file
			StorageFile file = await savePicker.PickSaveFileAsync();
			if (file != null)
			{
				// Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
				CachedFileManager.DeferUpdates(file);


				// write to file
				var imageStream = GraphicsHelper.CaptureScreenshot(AppHelper.AppMainWindow, ImageFormat.Jpeg);

				//await FileIO.WriteBufferAsync(file, (Windows.Storage.Streams.IBuffer)imageStream);

				//var textBox = ((sender as Button).Parent as StackPanel)
				//.FindName("FileContentTextBox") as TextBox;
				//using (var stream = await file.OpenStreamForWriteAsync())
				//{
				//	using (var tw = new StreamWriter(stream))
				//	{
				//		tw.WriteLine(textBox?.Text);
				//	}
				//}
				// Another way to write a string to the file is to use this instead:
				// await FileIO.WriteTextAsync(file, "Example file contents.");

				// Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
				// Completing updates may require Windows to ask for user input.
				FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
				//if (status == FileUpdateStatus.Complete)
				//{
				//	SaveFileOutputTextBlock.Text = "File " + file.Name + " was saved.";
				//}
				//else if (status == FileUpdateStatus.CompleteAndRenamed)
				//{
				//	SaveFileOutputTextBlock.Text = "File " + file.Name + " was renamed and saved.";
				//}
				//else
				//{
				//	SaveFileOutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
				//}
				//}
				//else
				//{
				//	SaveFileOutputTextBlock.Text = "Operation cancelled.";
				//}
			}
		}
	}
}
