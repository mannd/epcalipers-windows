using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.ViewModels;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using EPCalipersWinUI3PDFHandler;

namespace EPCalipersWinUI3
{
    public partial class MainPageViewModel : CaliperPageViewModel
	{
		private readonly IPdfHelper _pdfHelper;
		private bool _isStartup = true;

		public delegate void SetZoomDelegate(float zoomFactor);
		public SetZoomDelegate SetZoom { get; set; }

		// TODO: Setting should allow reset zoom with each opened image or new PDF page to be false.
		// It should only allow reset rotation to be false if image is multipage PDF.
		public bool ResetZoomWithNewImage { get; private set; } = true;
		public bool ResetRotationWithNewImage { get; private set; } = true;
		public float ZoomFactor
		{
			get => _zoomFactor;
			set
			{
				if (_zoomFactor != value)
				{
					_zoomFactor = value;
					_caliperCollection.ScaleFactor = value;
				}
			}
		}
		private float _zoomFactor;

		public MainPageViewModel(SetZoomDelegate setZoomDelegate, ICaliperView caliperView)
			: base(caliperView)
		{
			// TODO: setting to start with blank screen, inactivate most menu items.
			Debug.Print("MainPageViewModel constructor");

			SetZoom = setZoomDelegate;
			_pdfHelper = new PdfHelper();
		}

		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.PropertyName == nameof(MainImageSource))
			{
				if (MainImageSource != null)
				{
					HasMainImage = true;
				}
			}
		}

		public override void RefreshCalipers()
		{
			base.RefreshCalipers();
			//_caliperCollection.ZoomBarThickness(ZoomFactor);

		}

		public override void AddTimeCaliper()
		{
			base.AddTimeCaliper();
			//_caliperCollection.ZoomBarThickness(ZoomFactor);
		}

		public override void AddAmplitudeCaliper()
		{
			base.AddAmplitudeCaliper();
			//_caliperCollection.ZoomBarThickness(ZoomFactor);
		}

		public override void AddAngleCaliper()
		{
			base.AddAngleCaliper();
			//_caliperCollection.ZoomBarThickness(ZoomFactor);
		}


		public void LoadSampleImage()
		{
			if (!_isStartup) { return; }
			var settings = Settings.Instance;
			if (AppHelper.StartUpImage == null && settings.ShowSampleEcgAtStartUp) 
			{
				MainImageSource = new BitmapImage { UriSource = new Uri("ms-appx:///Assets/Images/sampleECG.jpg") };
				SetTitleBarName("SampleECG".GetLocalized());
				HasMainImage = true;
			}
			else
			{
				HasMainImage = false;
			}
			_isStartup = false;
		}

		#region menu
		public async Task OpenImageFile(StorageFile file)
		{
			if (file != null)
			{
				FileName = file.DisplayName;
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
					await bitmapImage.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
					// Set the image on the main page to the dropped image
					MainImageSource = bitmapImage;
					IsMultipagePdf = false;
					SetTitleBarName(FileName);
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

		[RelayCommand]
		private static void TransparenWindow()
		{
			var mainWindow = AppHelper.AppMainWindow;
			// TODO: Save Main Page title (different from save title bar)
			mainWindow.SystemBackdrop = new WinUIEx.TransparentTintBackdrop();
			mainWindow.Navigate(typeof(TransparentPage));
		}
		#endregion

		#region zoom
		// Zoom methods
		// Note zoom factors used in Mac OS X version
		// These are taken from the Apple IKImageView demo
		private readonly float _zoomInFactor = 1.414214f;
		private readonly float _zoomOutFactor = 0.7071068f;
		private readonly static float _maxZoom = 10;
		private readonly static float _minZoom = 0.1f;
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
		#endregion


		#region pdf
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
			// Users input 1 based page numbers.
			var page = await _pdfHelper.GetPdfPageSourceAsync(pageNumber - 1);
			if (page != null)
			{
				MainImageSource = page;
				UpdatePageNumber();
			}
		}
		private void UpdatePageNumber()
		{
			IsNotFirstPageOfPdf = IsMultipagePdf && _pdfHelper.CurrentPageNumber > 1;
			IsNotLastPageOfPdf = IsMultipagePdf && _pdfHelper.CurrentPageNumber < _pdfHelper.NumberOfPdfPages;
			var extension = string.Format("AppMultipagePDFTitle".GetLocalized(),
				FileName, _pdfHelper.CurrentPageNumber, _pdfHelper.NumberOfPdfPages);
			SetTitleBarName(extension);
		}

		#endregion

		#region observable properties
		[ObservableProperty]
		private Microsoft.UI.Xaml.Controls.Image mainImage;

		[ObservableProperty]
		private ImageSource mainImageSource;

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

		[ObservableProperty]
		private string fileName;

		[ObservableProperty]
		private string titleBarName;

		[ObservableProperty]
		private bool hasMainImage;

		#endregion
	}
}
