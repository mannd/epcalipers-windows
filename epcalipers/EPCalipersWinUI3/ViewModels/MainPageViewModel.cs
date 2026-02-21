using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.ViewModels;
using EPCalipersWinUI3.Views;
using EPCalipersPdfCore;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.UI.Xaml.Controls;
using EPCalipersPdf;
using Windows.Graphics.Capture;
using Microsoft.UI.Xaml;

namespace EPCalipersWinUI3
{
    public partial class MainPageViewModel : CaliperPageViewModel
	{
		private readonly IPdfHelper _pdfHelper;
		private bool _isStartup = true;
		private readonly ISettings _settings = Settings.Instance;

		public delegate void SetZoomDelegate(float zoomFactor);
		public SetZoomDelegate SetZoom { get; set; }
		public MainPageViewModel(SetZoomDelegate setZoomDelegate, ICaliperView caliperView, ScrollViewer scrollViewer)
			: base(caliperView, scrollViewer)
		{
			Debug.Print("MainPageViewModel constructor");
			SetZoom = setZoomDelegate;
			_pdfHelper = new PdfHelper();
			HasMainImage = (MainImageSource != null);
			HasNoMainImage = !HasMainImage;
			SupportsPdfs = _pdfHelper.SupportsPdfs;
		}

		public override bool CanAddCalipers()
		{
			return HasMainImage && base.CanAddCalipers();
		}


		public bool ClearCalipersBetweenPdfPages
		{
			get
			{
				if (IsMultipagePdf && !_settings.ClearCalipersBetweenPdfPages) return false;
				return true;
			}
		}
		public bool ResetZoomWithNewPdfPage
		{
			get 
			{
				if (IsMultipagePdf && !_settings.ResetZoomBetweenPdfPages) return false;
				return true;
			}
		}
		public bool ResetRotationWithNewPdfPage
		{
			get
			{
				if (IsMultipagePdf && !_settings.ResetRotationBetweenPdfPages) return false;
				return true;
			}
		}
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

		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.PropertyName == nameof(MainImageSource))
			{
				Debug.Print("changing main image source");
				HasMainImage = (MainImageSource != null);
				HasNoMainImage = !HasMainImage;
				ShowCaliperMenuItems = CanAddCalipers();
			}
		}

		/// <summary>
		/// If the PDF resolution has been changed in Settings, will reload the PDF page.
		/// Note that caliber calibration can't handle changing PDF resolution, so 
		/// calipers and calibration are both cleared.
		/// </summary>
		/// <returns>Task</returns>
		public async Task RefreshImageIfPdfResolutionChanged()
		{
			if (_pdfHelper.Resolution == _settings.PdfResolution) return;
			_pdfHelper.Resolution = _settings.PdfResolution;
			// specifically reload PDF page with new resolution if it has changed.
			if (!_pdfHelper.PdfIsLoaded) return;
			// GetPdfPageSourceAsync uses zero based page number
			MainImageSource = await _pdfHelper.GetPdfPageSourceAsync(_pdfHelper.CurrentPageNumber - 1);
			_caliperCollection.ClearCalibration();
			DeleteAllCalipers();
		}

		public override void RefreshCalipers()
		{
			base.RefreshCalipers();
		}

		public override void AddTimeCaliper()
		{
			base.AddTimeCaliper();
		}

		public override void AddAmplitudeCaliper()
		{
			base.AddAmplitudeCaliper();
		}

		public override void AddAngleCaliper()
		{
			base.AddAngleCaliper();
		}


		public void LoadSampleImage()
		{
			if (!_isStartup) { return; }
			if (AppHelper.StartUpImage == null && _settings.ShowSampleEcgAtStartUp) 
			{
				MainImageSource = new BitmapImage { UriSource = new Uri("ms-appx:///Assets/Images/sampleECG.jpg") };
				SetTitleBarName("SampleECG".GetLocalized());
			}
			_isStartup = false;
		}

		#region menu
		public async Task OpenImageFile(StorageFile file)
		{
			if (file == null)
			{
				Debug.Print("Operation cancelled.");
				return;
			}

			FileName = file.DisplayName;
			_pdfHelper.ClearPdfFile();

			try
			{
				if (_pdfHelper.IsPdfFile(file))
				{
					try
					{
						_pdfHelper.LoadPdfFile(file);
						_pdfHelper.Resolution = _settings.PdfResolution;

						// GetPdfPageSourceAsync uses zero based page number
						var pdfImagePage = await _pdfHelper.GetPdfPageSourceAsync(0);
						if (pdfImagePage == null)
						{
							Debug.WriteLine($"Failed to render PDF page for '{file.Path}'.");
							_pdfHelper.ClearPdfFile();
							await ShowExceptionDialog(new Exception("Failed to render PDF page."), file.Path);
						}
						else
						{
							MainImageSource = pdfImagePage;
							MaximumPdfPage = _pdfHelper.MaximumPageNumber;
							IsMultipagePdf = _pdfHelper.IsMultiPage;
							UpdatePageNumber();
							SetTitleBarName(FileName);
						}
					}
					catch (OutOfMemoryException oom)
					{
						Debug.WriteLine($"OutOfMemory while opening PDF '{file.Path}': {oom}");
						_pdfHelper.ClearPdfFile();
						await ShowExceptionDialog(oom, file.Path);
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"Exception while opening PDF '{file.Path}': {ex}");
						_pdfHelper.ClearPdfFile();
						await ShowExceptionDialog(ex, file.Path);
					}
				}
				else
				{
					var bitmapImage = new BitmapImage();
					try
					{
						using (var stream = await file.OpenAsync(FileAccessMode.Read))
						{
							await bitmapImage.SetSourceAsync(stream);
						}
						MainImageSource = bitmapImage;
						IsMultipagePdf = false;
						SetTitleBarName(FileName);
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"Failed to load image '{file.Path}': {ex}");
						await ShowExceptionDialog(ex, file.Path);
					}
				}

	_caliperCollection.ClearCalibration();
			}
			catch (Exception ex)
			{
				// Last-resort catch to prevent an unhandled exception from closing the app
				Debug.WriteLine($"Unhandled exception while opening file '{file?.Path}': {ex}");
				_pdfHelper.ClearPdfFile();
				await ShowExceptionDialog(ex, file?.Path);
			}
		}

		private async Task ShowExceptionDialog(Exception ex, string path)
		{
			if (ex == null) return;

			string details = $"File: {path ?? "(unknown)"}\n\nException: {ex.GetType().FullName}\nMessage: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}";

			// Use a read-only TextBlock for display so newlines and wrapping render correctly.
	var detailsBlock = new TextBlock
	{
		Text = details,
		TextWrapping = Microsoft.UI.Xaml.TextWrapping.Wrap,
		Height = 300,
		HorizontalAlignment = HorizontalAlignment.Stretch
	};

	// Use a ScrollViewer to enable vertical scrolling and disable horizontal scrolling.
	var scroll = new ScrollViewer
	{
		Content = detailsBlock,
		VerticalScrollMode = ScrollMode.Enabled,
		VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
		HorizontalScrollMode = ScrollMode.Disabled,
		HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
		Height = 300
	};

	var dialog = new ContentDialog
	{
		Title = "Error opening file",
		Content = scroll,
		PrimaryButtonText = "Copy Details",
		CloseButtonText = "OK",
	};

	// Ensure we have a XamlRoot to show the dialog.
	var mainWindow = AppHelper.AppMainWindow;
	if (mainWindow?.Content != null)
	{
		dialog.XamlRoot = mainWindow.Content.XamlRoot;
	}

	var result = await dialog.ShowAsync();

	if (result == ContentDialogResult.Primary)
	{
		var dp = new Windows.ApplicationModel.DataTransfer.DataPackage();
		dp.SetText(details);
		Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
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
				HandleBetweenPagePdfCalibration();

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
				HandleBetweenPagePdfCalibration();
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
				HandleBetweenPagePdfCalibration();
			}
		}

		private void HandleBetweenPagePdfCalibration()
		{
			if (_settings.RecalibrateBetweenPdfPages)
			{
				_caliperCollection.ClearCalibration();
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

		[ObservableProperty]
		private bool hasNoMainImage;

		[ObservableProperty]
		private bool isOpenFromScreenshotSupported;

		[ObservableProperty]
		private bool supportsPdfs;

		#endregion
	}
}
