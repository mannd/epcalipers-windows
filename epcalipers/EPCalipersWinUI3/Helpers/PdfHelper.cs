using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using PdfiumViewer;
using Windows.Storage;

namespace EPCalipersWinUI3.Helpers
{
	/// <summary>
	/// Encapsulate and isolate nasty PDF code.
	/// </summary>
	public class PdfHelper
	{
		private PdfDocument _pdfDocument = null;

		public string FilePath { get; set; }
		public bool PdfIsLoaded => _pdfDocument != null;

		public static bool IsPdfFile(StorageFile file) =>
			file.FileType.ToUpper() == ".PDF";

		public void LoadPdfFile(StorageFile file)
		{
			if (file == null)
			{
				return;
			}
			try
			{
				_pdfDocument = PdfDocument.Load(file.Path);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public async Task<SoftwareBitmapSource> GetPdfPageAsync(int pageNumber)
		{
			if (_pdfDocument == null) { return null; }
			var img = _pdfDocument.Render(0, 300, 300, PdfRenderFlags.CorrectFromDpi);
			var source = await GetWinUI3BitmapSourceFromGdiBitmap(new Bitmap(img));
			return source;
		}

		public void ClearPdfFile()
		{
			_pdfDocument?.Dispose();
			_pdfDocument = null;
		}

		// from https://stackoverflow.com/questions/76640972/convert-system-drawing-icon-to-microsoft-ui-xaml-imagesource
		private static async Task<Microsoft.UI.Xaml.Media.Imaging.SoftwareBitmapSource> GetWinUI3BitmapSourceFromGdiBitmap(System.Drawing.Bitmap bmp)
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

		// Currently not used.
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
	}
}
