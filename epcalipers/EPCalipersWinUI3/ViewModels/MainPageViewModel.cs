using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Calipers;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Views;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace EPCalipersWinUI3
{
	public partial class MainPageViewModel : ObservableObject
	{
		private readonly PdfHelper _pdfHelper;
		private readonly CaliperCollection _caliperCollection;
		private readonly ICaliperView _caliperView;
		private Caliper _grabbedCaliper = null;
		private Bar _grabbedComponent = null;
		private Point _startingDragPoint;

        // Note zoom factors used in Mac OS X version
        // These are taken from the Apple IKImageView demo
        private readonly float _zoomInFactor = 1.414214f;
        private readonly float _zoomOutFactor = 0.7071068f;
        private readonly static float _maxZoom = 10;
        private readonly static float _minZoom = 0.1f;

		//
		private static double _newCaliperDisplacement = 0;
		private static readonly double _displacementAmount = 10;
		private static readonly double _maximumDisplacement = 100;

		public delegate void SetZoomDelegate(float zoomFactor);
		public SetZoomDelegate SetZoom {  get; set; }

		// TODO: Setting should allow reset zoom with each opened image or new PDF page to be false.
		// It should only allow reset rotation to be false if image is multipage PDF.
		public bool ResetZoomWithNewImage { get; private set; } = true;
		public bool ResetRotationWithNewImage { get; private set; } = true;
		public float ZoomFactor { get; set; } = 1;

		public MainPageViewModel(SetZoomDelegate setZoomDelegate, ICaliperView caliperView)
		{
			SetZoom = setZoomDelegate;
			_pdfHelper = new PdfHelper();
			_caliperCollection = new CaliperCollection(caliperView);
			_caliperView = caliperView;
		}
		#region commands
		[RelayCommand]
		public void AddTimeCaliper()
		{
			var caliper = Caliper.InitCaliper(CaliperType.Time, _caliperView);
			FinalizeCaliper(caliper);
		}

		[RelayCommand]
		public void AddAmplitudeCaliper()
		{
			var caliper = Caliper.InitCaliper(CaliperType.Amplitude, _caliperView);
			FinalizeCaliper(caliper);
		}

		[RelayCommand]
		public void AddAngleCaliper()
		{
			var caliper = Caliper.InitCaliper(CaliperType.Angle, _caliperView);
			FinalizeCaliper(caliper);
		}

		private void FinalizeCaliper(Caliper c)
		{
			if (c == null) return;
			c.UnselectedColor = Colors.Blue;
			c.SelectedColor = Colors.Red;
			c.IsSelected = false;
			_caliperCollection.Add(c);

		}

		[RelayCommand]
		public void DeleteAllCalipers()
		{
			_caliperCollection.Clear();
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
			// Detect if this is near a caliper component, and if so, load it up for movement.
			(_grabbedCaliper, _grabbedComponent) = _caliperCollection.GetGrabbedCaliperAndBar(point);
			_startingDragPoint = point;
		}

		public void DragCaliperComponent(Point point)
		{
			if (_grabbedCaliper == null || _grabbedComponent == null) return;
			var delta = new Point(point.X - _startingDragPoint.X, point.Y - _startingDragPoint.Y);
			_startingDragPoint.X += delta.X;
			_startingDragPoint.Y += delta.Y;
			_grabbedCaliper.Drag(_grabbedComponent, delta, _startingDragPoint);
		}

		public void ReleaseGrabbedCaliper()
		{
			_grabbedCaliper = null;
			_grabbedComponent = null;
		}

		public async Task OpenImageFile(StorageFile file)
		{
			if (file != null)
			{
				_pdfHelper.ClearPdfFile();
				// NB can get OOM errors with large PDF files when running on x86 system.
				if (PdfHelper.IsPdfFile(file))
				{
					_pdfHelper.LoadPdfFile(file);
					SoftwareBitmapSource pdfImagePage = await _pdfHelper.GetPdfPageSourceAsync(0);
					MainImageSource = pdfImagePage;
					MaximumPdfPage = _pdfHelper.MaximumPageNumber;
					IsMultipagePdf = _pdfHelper.IsMultiPage;
					UpdatePageNumber();
				}
				else
				{
					var bitmapImage = new BitmapImage();
					bitmapImage.SetSource(await file.OpenAsync(FileAccessMode.Read));
					// Set the image on the main page to the dropped image
					MainImageSource = bitmapImage;
					IsMultipagePdf = false;
				}
			}
			else
			{
				Debug.Print("Operation cancelled.");
				//ContentDialog dialog = MessageDialog.Create(title: "Test", message: "Message");
				//dialog.XamlRoot = (App.Current as App)?.Window.Content.XamlRoot;
				//await dialog.ShowAsync();
			}

		}

		public static async Task<SoftwareBitmapSource> GetWinUI3BitmapSourceFromGdiBitmap(System.Drawing.Bitmap bmp)
		{
			if (bmp == null)
				return null;

			// get pixels as an array of bytes
			var data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
			var bytes = new byte[data.Stride * data.Height];
			Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
			bmp.UnlockBits(data);

			// get WinRT SoftwareBitmap
			var softwareBitmap = new Windows.Graphics.Imaging.SoftwareBitmap(
				Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
				bmp.Width,
				bmp.Height,
				Windows.Graphics.Imaging.BitmapAlphaMode.Premultiplied);
			softwareBitmap.CopyFromBuffer(bytes.AsBuffer());

			// build WinUI3 SoftwareBitmapSource
			var source = new SoftwareBitmapSource();
			await source.SetBitmapAsync(softwareBitmap);
			return source;
		}

		private void ZoomView(float multiple)
		{
			var zoomTarget = multiple * ZoomFactor;
			if (zoomTarget < _minZoom || zoomTarget > _maxZoom) { return; }
			ZoomFactor = zoomTarget;
			SetZoom(ZoomFactor);
		}

		[RelayCommand]
		private void ZoomIn()
		{
			ZoomView(_zoomInFactor);
		}

		[RelayCommand]
		private void ZoomOut()
		{
			ZoomView(_zoomOutFactor);
		}

		[RelayCommand]
		private void ResetZoom()
		{
			ZoomFactor = 1;
			SetZoom(ZoomFactor);
		}

		[RelayCommand]
		private static void Settings()
		{
			var mainWindow = (Application.Current as App)?.Window as MainWindow;
            mainWindow.Navigate(typeof(SettingsPage));
		}

		[RelayCommand]
		private static void TransparenWindow()
		{
			var mainWindow = (Application.Current as App)?.Window as MainWindow;
            mainWindow.Navigate(typeof(TransparentPage));
		}

		[RelayCommand]
		private static void Help()
		{
			var mainWindow = (Application.Current as App)?.Window as MainWindow;
            mainWindow.Navigate(typeof(HelpWebViewPage));
		}

		[RelayCommand]
		private static void Exit() => CommandHelper.ApplicationExit();

		[RelayCommand]
		private async Task NextPdfPage() 
		{
			var nextPage = await _pdfHelper.GetNextPage();
			if (nextPage != null)
			{
				MainImageSource = nextPage;
				UpdatePageNumber();
					
			}
			Debug.WriteLine($"Current page number = {CurrentPdfPageNumber}");
		}

		[RelayCommand]
		private async Task PreviousPdfPage() 
		{ 
			var previousPage = await _pdfHelper.GetPreviousPage();
			if (previousPage != null)
			{
				MainImageSource = previousPage;
				UpdatePageNumber();
			}
		}

		public async Task GotoPdfPage(int pageNumber) 
		{
			// User's input 1 based page numbers.
			var page = await _pdfHelper.GetPdfPageSourceAsync(pageNumber - 1);
			if (page != null)
			{
				MainImageSource = page;
				UpdatePageNumber();
			}
		}
		private void UpdatePageNumber()
		{
			CurrentPdfPageNumber = _pdfHelper.CurrentPageNumber;
			IsNotFirstPageOfPdf = IsMultipagePdf && _pdfHelper.CurrentPageNumber > 1;
			IsNotLastPageOfPdf = IsMultipagePdf && _pdfHelper.CurrentPageNumber < _pdfHelper.NumberOfPdfPages;
		}
		#endregion

		#region observable properties
		[ObservableProperty]
		private string testText = "Test";

		[ObservableProperty]
		private Microsoft.UI.Xaml.Controls.Image mainImage;

		[ObservableProperty]
		private ImageSource mainImageSource
			= new BitmapImage { UriSource = new Uri("ms-appx:///Assets/Images/sampleECG.jpg") };

		[ObservableProperty]
		private int maximumPdfPage;

		[ObservableProperty]
		private int currentPdfPageNumber;

		[ObservableProperty]
		private bool isMultipagePdf;

		[ObservableProperty]
		private bool isNotFirstPageOfPdf;

		[ObservableProperty]
		private bool isNotLastPageOfPdf;

		[ObservableProperty]
		private Bounds bounds;


		#endregion
	}
}
