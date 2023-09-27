using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace EPCalipersWinUI3
{
	public partial class MainPageViewModel : ObservableObject
	{
		private readonly PdfHelper _pdfHelper = new PdfHelper();

		public bool ResetZoom { get; private set; } = true;
		public bool ResetRotation { get; private set; } = false;

		public float ZoomFactor
		{
			get
			{
				return _zoomFactor;
			}
			set
			{
				_zoomFactor = value;
				Debug.WriteLine(_zoomFactor.ToString());
			}
		}

		private float _zoomFactor;

		public MainPageViewModel()
		{
			_pdfHelper = new PdfHelper();
		}

		#region commands
		[RelayCommand]
		void Test()
		{
			Debug.Print("test command");
		}

		[RelayCommand]
		private async Task Open()
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
			await OpenImageFile(file);
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
					CurrentPdfPageNumber = _pdfHelper.CurrentPageNumber;
					IsMultipagePdf = _pdfHelper.IsMultiPage;
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
			var source = new Microsoft.UI.Xaml.Media.Imaging.SoftwareBitmapSource();
			await source.SetBitmapAsync(softwareBitmap);
			return source;
		}


		[RelayCommand]
		private static void Exit() => Application.Current.Exit();

		[RelayCommand]
		private async Task NextPdfPage() 
		{
			var nextPage = await _pdfHelper.GetNextPage();
			if (nextPage != null)
			{
				MainImageSource = nextPage;
				CurrentPdfPageNumber = _pdfHelper.CurrentPageNumber;
					
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
				CurrentPdfPageNumber = _pdfHelper.CurrentPageNumber;
			}
		}


		[RelayCommand]
		private async Task GotoPdfPage(int pageNumber) 
		{
			// User's input 1 based page numbers.
			var page = await _pdfHelper.GetPdfPageSourceAsync(pageNumber - 1);
			if (page != null)
			{
				MainImageSource = page;
				CurrentPdfPageNumber = _pdfHelper.CurrentPageNumber;
			}
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
		public int maximumPdfPage;

		[ObservableProperty]
		public int currentPdfPageNumber;

		[ObservableProperty]
		public bool isMultipagePdf;


		#endregion
	}
}
