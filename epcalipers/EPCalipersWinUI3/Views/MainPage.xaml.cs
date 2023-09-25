using Microsoft.UI;
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
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
	public sealed partial class MainPage : Page
	{
        // Note zoom factors used in Mac OS X version
        // These are taken from the Apple IKImageView demo
        private readonly float _zoomInFactor = 1.414214f;
        private readonly float _zoomOutFactor = 0.7071068f;

        private readonly static float _maxZoom = 10;
        private readonly static float _minZoom = 0.1f;
        private readonly static TimeSpan _rotationDuration = TimeSpan.FromSeconds(0.4);

        double lineThickness = 5;
        private double _imageRotation = 0;

        public MainPageViewModel ViewModel { get; set; }
		public MainPage()
		{
			this.InitializeComponent();
            ViewModel = new MainPageViewModel();

			// demo
            DrawLine(500, 0, 500, 500);

			// Set up image rotation
			InitImageForRotation();
            scrollViewer.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, (s, e) =>
            {
                //lineThickness = Math.Max(1.0, lineThickness / scrollViewer.ZoomFactor);
                //lineThickness = Math.Min(lineThickness, 5.0);

                //canvas.Children.Remove(line);
                //DrawLine(500, 0, 500, 500);
                //Debug.Print(scrollViewer.ZoomFactor.ToString());
            });

            EcgImage.RegisterPropertyChangedCallback(Image.SourceProperty, (s, e) =>
            {
                SetZoom(1);
				InitImageForRotation();
            });
        }

		// demo
        private void DrawLine(int x1, int y1, int x2, int y2)
        {
            //canvas.Children.Clear();
            Line line = new();
            var brush = new SolidColorBrush(Microsoft.UI.Colors.Blue);
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            lineThickness = Math.Max(1.0, lineThickness / scrollViewer.ZoomFactor);
            lineThickness = Math.Min(lineThickness, 5.0);
            line.Stroke = brush;
            CaliperGrid.Children.Add(line);
        }

        private void scrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            // Just thin out lines as view zooms
        }

        private void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
        }

		private void scrollViewer_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
            //var position = e.GetCurrentPoint(this.canvas);
            //Debug.WriteLine($"Scrollview touched at {position.Position}");
            //DrawLine((int)position.Position.X, (int)position.Position.Y, 500, 500);
            //pointerDown = true;
		}

		private void scrollViewer_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
            //if (pointerDown) {
            //    var position = e.GetCurrentPoint(this.canvas);
            //    DrawLine((int)position.Position.X, (int)position.Position.Y, 500, 500);
            //}
		}

		private void scrollView_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
            //Debug.WriteLine("Pointer released.");
            //pointerDown = false;
		}

		#region zoom
		private void ZoomIn_Click(object _1, RoutedEventArgs _2)
		{
            Zoom(_zoomInFactor);
		}

		private void ZoomOut_Click(object sender, RoutedEventArgs e)
		{
            Zoom(_zoomOutFactor);
		}

		private void ResetZoom_Click(object sender, RoutedEventArgs e)
		{
            SetZoom(1);
		}

          private void Zoom(float multiple)
        {
            var zoomTarget = multiple * scrollViewer.ZoomFactor;
            if (zoomTarget < _minZoom || zoomTarget > _maxZoom) { return; }
            SetZoom(zoomTarget);
		}

        private void SetZoom(float zoom)
        {
            scrollViewer?.ChangeView(0, 0, zoom);
            // TODO: Remove this if not used.
            // Not sure if we need ViewModel to track this...
            ViewModel.ZoomFactor = zoom;
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
			// NB: This is a bit of a regression from V2, since rotated image is clipped.
			Storyboard storyboard = new();
			storyboard.Duration = new Duration(_rotationDuration);
			DoubleAnimation rotateAnimation = new()
			{
				From = startAngle,
				To = endAngle,
				Duration = storyboard.Duration
			};

			Storyboard.SetTarget(rotateAnimation, EcgImage);
			Storyboard.SetTargetProperty(rotateAnimation,
				"(UIElement.RenderTransform).(RotateTransform.Angle)");
			storyboard.Children.Add(rotateAnimation);
			storyboard.Begin();
		}

		private void InitImageForRotation()
		{
			if (EcgImage?.Source  == null) { return; }
			_imageRotation = 0;
            EcgImage.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotateTransform = new RotateTransform()
            {
                CenterX = EcgImage.Width / 2,
                CenterY = EcgImage.Height / 2,
				Angle = _imageRotation
            };
            EcgImage.RenderTransform = rotateTransform;
		}
		#endregion
	}
}
