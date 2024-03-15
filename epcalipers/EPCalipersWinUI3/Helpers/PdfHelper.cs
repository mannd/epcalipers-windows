using Microsoft.UI.Xaml.Media.Imaging;
using PdfiumViewer;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Graphics.Imaging;
using EPCalipersWinUI3.Contracts;

namespace EPCalipersWinUI3.Helpers
{
    /// <summary>
    /// Encapsulate and isolate nasty PDF code.
    /// </summary>
    public class PdfHelper : IPdfHelper
	{
		private PdfDocument _pdfDocument = null;
		private int _pageNumber = 0;

		public string FilePath { get; set; }
		public bool PdfIsLoaded => _pdfDocument != null;

		public int NumberOfPdfPages => _pdfDocument.PageCount;

		// Two properties below use 1 based pagination.
		public int CurrentPageNumber => _pageNumber + 1;

		public int MaximumPageNumber => NumberOfPdfPages;

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
				_pageNumber = 0;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public async Task<SoftwareBitmapSource> GetPdfPageSourceAsync(int pageNumber)
		{
			if (_pdfDocument == null) { return null; }
			if (pageNumber < 0 || pageNumber > _pdfDocument.PageCount - 1) { return null; }
			_pageNumber = pageNumber;
			var img = _pdfDocument.Render(pageNumber, 300, 300, PdfRenderFlags.CorrectFromDpi);
			var source = await GetWinUI3BitmapSourceFromGdiBitmap(new Bitmap(img));
			img.Dispose();
			return source;
		}

		public void ClearPdfFile()
		{
			_pdfDocument?.Dispose();
			_pdfDocument = null;
			_pageNumber = 0;
		}

		public bool IsMultiPage
		{
			get
			{
				return _pdfDocument?.PageCount > 0;
			}
		}

		public async Task<SoftwareBitmapSource> GetNextPage()
		{
			if (_pdfDocument == null) return null;
			int nextPage = _pageNumber + 1;
			if (nextPage > _pdfDocument.PageCount - 1) return null;
			_pageNumber = nextPage;
			return await GetPdfPageSourceAsync(nextPage);
		}

		public async Task<SoftwareBitmapSource> GetPreviousPage()
		{
			if (_pdfDocument == null) return null;
			int previousPage = _pageNumber - 1;
			if (previousPage < 0) return null;
			_pageNumber = previousPage;
			return await GetPdfPageSourceAsync(previousPage);
		}

		// From https://stackoverflow.com/questions/76640972/convert-system-drawing-icon-to-microsoft-ui-xaml-imagesource
		private static async Task<SoftwareBitmapSource> GetWinUI3BitmapSourceFromGdiBitmap(Bitmap bmp)
		{
			if (bmp == null)
				return null;
			// get pixels as an array of bytes
			var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
				ImageLockMode.ReadOnly, bmp.PixelFormat);
			var bytes = new byte[data.Stride * data.Height];
			Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
			bmp.UnlockBits(data);

			// get WinRT SoftwareBitmap
			var softwareBitmap = new SoftwareBitmap(
				BitmapPixelFormat.Bgra8,
				bmp.Width,
				bmp.Height,
				BitmapAlphaMode.Premultiplied);
			softwareBitmap.CopyFromBuffer(bytes.AsBuffer());

			// build WinUI3 SoftwareBitmapSource
			var source = new SoftwareBitmapSource();
			await source.SetBitmapAsync(softwareBitmap);
			return source;
		}
	}
}
