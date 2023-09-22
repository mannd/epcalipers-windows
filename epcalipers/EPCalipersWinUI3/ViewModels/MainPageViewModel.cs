﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace EPCalipersWinUI3
{
	public partial class MainPageViewModel : ObservableObject
	{
		private PdfHelper _pdfHelper;

		public MainPageViewModel()
		{
			_pdfHelper = new PdfHelper();
		}
		
		[RelayCommand]
		void Test()
		{
			Debug.Print("test command");
		}

		[RelayCommand]
		private async Task About(UIElement element)
		{
			Debug.WriteLine("About");
			var aboutDialog = new AboutDialog();
			aboutDialog.XamlRoot = (App.Current as App)?.Window.Content.XamlRoot;
			await aboutDialog.ShowAsync();
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
			if (file != null)
			{
				if (PdfHelper.IsPdfFile(file))
				{
					using (var doc = PdfiumViewer.PdfDocument.Load(file.Path)) {
						var img = doc.Render(0, 300, 300, PdfiumViewer.PdfRenderFlags.CorrectFromDpi);
						img.Save("\\Users\\mannd\\Downloads\\tempfile1.bmp");

						var source = await GetWinUI3BitmapSourceFromGdiBitmap(new Bitmap(img));
						MainImageSource = source;
						return;
					}
				};
				BitmapImage bitmapImage = new()
				{
					UriSource = new Uri(file.Path)
				};
				MainImageSource = bitmapImage;
				// Alternate method using fileStream ->
				//using (IRandomAccessStream fileStream =
				//	await file.OpenAsync(Windows.Storage.FileAccessMode.Read)) {
				//	BitmapImage bitmapImage = new BitmapImage();
				//	bitmapImage.DecodePixelHeight = 500;
				//	await bitmapImage.SetSourceAsync(fileStream);
				//	MainImageSource = bitmapImage;
				//}
			}
			else
			{
				Debug.Print("Operation cancelled.");
			}
		}

		private void renderPdfToFile(string pdfFilename, string outputImageFilename, int dpi)
		{
			var tempPath = Path.GetTempPath();
			Debug.WriteLine($"{tempPath}");
			using (var doc = PdfiumViewer.PdfDocument.Load(pdfFilename))
			{ // Load PDF Document from file
				for (int page = 0; page < doc.PageCount; page++)
				{ // Loop through pages
					using (var img = doc.Render(page, dpi, dpi, false))
					{ // Render with dpi and with forPrinting false
						var filePath = $"{tempPath}page_{page}_{outputImageFilename}.bmp";
						Console.WriteLine(filePath);
						if (File.Exists(filePath))
						{
							File.Delete(filePath);
						}
						img.Save(filePath); // Save rendered image to disc
					}
				}
			}
		}

		// from https://stackoverflow.com/questions/76640972/convert-system-drawing-icon-to-microsoft-ui-xaml-imagesource
		public static async Task<Microsoft.UI.Xaml.Media.Imaging.SoftwareBitmapSource> GetWinUI3BitmapSourceFromBitmap(System.Drawing.Bitmap bitmap)
		{
			if (bitmap == null)
				return null;

			// convert to bitmap
			return await GetWinUI3BitmapSourceFromGdiBitmap(bitmap);
		}

		public static async Task<Microsoft.UI.Xaml.Media.Imaging.SoftwareBitmapSource> GetWinUI3BitmapSourceFromGdiBitmap(System.Drawing.Bitmap bmp)
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


		[ObservableProperty]
		private string testText = "Test";

		[ObservableProperty]
		private Microsoft.UI.Xaml.Controls.Image mainImage;

		[ObservableProperty]
		private ImageSource mainImageSource
			= new BitmapImage { UriSource = new Uri("ms-appx:///Assets/Images/sampleECG.jpg") };
	}
}
