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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
    public sealed partial class MainPage : Page
	{
        private readonly static TimeSpan _rotationDuration = TimeSpan.FromSeconds(0.4);

        private double _imageRotation = 0;
		private double _rotatedImageScale = 1.0;

		private Caliper testCaliper;
		private List<Caliper> _calipers = new List<Caliper>();
		private CaliperComponent _grabbedComponent = null;

		private bool pointerDown = false;
		private Point startingPoint;

        public MainPageViewModel ViewModel { get; set; }

		public MainPage()
		{
			this.InitializeComponent();
            ViewModel = new MainPageViewModel(SetZoom);

			// demo

            ScrollView.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, (s, e) =>
            {
				ViewModel.ZoomFactor = ScrollView.ZoomFactor;
            });

            EcgImage.RegisterPropertyChangedCallback(Image.SourceProperty, (s, e) =>
            {
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
			

				// Below happens automatically...
				//CaliperGrid.Height = EcgImage.Height;
				//CaliperGrid.Width = EcgImage.Width;
            });
        }

		#region calipers
		private void AddTimeCaliper_Click(object sender, RoutedEventArgs e)
		{
			if (testCaliper == null)
			{
				var bounds = new Bounds(CaliperGrid.ActualWidth, CaliperGrid.ActualHeight);
				testCaliper = Caliper.Create(CaliperType.Time, bounds);
				testCaliper.SetColor(Colors.Blue);
				_calipers.Add(testCaliper);
				testCaliper.Add(CaliperGrid);
			}
		}

        private void ScrollView_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            // ? thin out lines as view zooms
        }

        private void ScrollView_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
        }

		private void ScrollView_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var position = e.GetCurrentPoint(this.CaliperGrid);
			startingPoint = position.Position;
			// Detect if this is near a caliper component, and if so, load it up for movement.
			foreach (var caliper in _calipers)
			{
				var component = caliper.IsNearComponent(startingPoint);
				if (component != null)
				{
					pointerDown = true;
					_grabbedComponent = component;
					break;
				}

			}
		}

		private void ScrollViewer_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (pointerDown) // && dragging caliper...
			{
				var position = e.GetCurrentPoint(this.CaliperGrid);
				// NB: only allow moving pointer withing bounds of EcgImage.
				// Going outside of bounds increases size of CalipersGrid, and that distors calipers.
				// By only registering in-bounds movements, the calipers can't go out of bounds or
				// get stuck at the edges of the image.
				//
				// Also note, if we decide to center the image in the Grid, will need to check that
				// Position.X and Position.Y are > 0 too.
				if (position.Position.X < EcgImage.ActualWidth && position.Position.Y < EcgImage.ActualHeight)
				{
					var delta = new Point(position.Position.X - startingPoint.X, position.Position.Y - startingPoint.Y);
					startingPoint.X += delta.X;
					startingPoint.Y += delta.Y;
					// temporary.  In reality, would detect caliper and caliper component with initial
					// touch, and only drag that.
					_calipers[0].Drag(_grabbedComponent, delta);
				}
			}
		}

		private void ScrollView_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("Pointer released.");
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
			Debug.WriteLine("Dropped file");
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

		private void ResetImage_Click(object sender, RoutedEventArgs e)
		{
			//ResetRotation();
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

			// Bug, reversing rotation by 1 degree and reversing direction causes scale to alternate
			// between legitimate scale and scale of 1.0;
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

		private void ResetRotation()
		{
			_rotatedImageScale = 1.0;
			RotateImage(_imageRotation, 0);
			_imageRotation = 0;
			//EcgImage.RenderTransform = null;
			//RotateImageWithoutAnimation(0);
		}

		// TODO: Calculate the scaling factor based on height and width and rotation angle...
		// https://stackoverflow.com/questions/65554301/how-to-calculate-the-sizes-of-a-rectangle-that-contains-rotated-image-potential#:~:text=and%20the%20width%20and%20height%20of%20the%20rotated,%28theta%20%2B%20theta0%29%29%2C%20fabs%20%28sin%20%28theta%20-%20theta0%29%29

		#endregion
		#region event handlers

		private async void About_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("About");
			var aboutDialog = new AboutDialog
			{
				XamlRoot = XamlRoot
			};
			await aboutDialog.ShowAsync();
		}

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
			var mainWindow = (Application.Current as App)?.Window as MainWindow;
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
			CaliperGrid.InputCursor = InputSystemCursor.Create(InputSystemCursorShape.Wait);
			await ViewModel.OpenImageFile(file);
			CaliperGrid.InputCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
		}
		#endregion

		private void Options_Click(object sender, RoutedEventArgs e)
		{
			var mainWindow = (Application.Current as App)?.Window as MainWindow;
            mainWindow.Navigate(typeof(SettingsPage));
		}

		private void Help_Click(object sender, RoutedEventArgs e)
		{
			var mainWindow = (Application.Current as App)?.Window as MainWindow;
            mainWindow.Navigate(typeof(HelpWebViewPage));

		}
	}
}
