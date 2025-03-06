using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using PDFHandler;
using PdfLibCore;
using PdfLibCore.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NewPdfHandler
{
	public class PdfHelper : IPdfHelper
	{
		private PdfDocument _pdfDocument = null;
		private int _pageNumber = 0;  // zero-based page numbers

		/// <inheritdoc/>
		public bool SupportsPdfs => true;

		/// <inheritdoc/>
		public int CurrentPageNumber => _pageNumber + 1;

		/// <inheritdoc/>
		public string FilePath { get; set; }

		/// <inheritdoc/>
		public bool IsMultiPage => _pdfDocument?.Pages.Count > 1;

        /// <inheritdoc/>
        public int MaximumPageNumber => NumberOfPdfPages;

		/// <inheritdoc/>
		public int NumberOfPdfPages => _pdfDocument?.Pages.Count ?? 0;

		/// <inheritdoc/>
		public bool PdfIsLoaded => _pdfDocument != null;

		/// <inheritdoc/>
		public PdfResolution Resolution { get; set; } = PDFHandler.PdfResolution.High;

		/// <inheritdoc/>
		public void ClearPdfFile()
		{
			_pdfDocument?.Close();
			_pdfDocument = null;
			_pageNumber = 0;
		}

		/// <inheritdoc/>
		public async Task<SoftwareBitmapSource> GetNextPage()
		{
            if (_pdfDocument == null)
            {
                return null;
            }

            int nextPage = _pageNumber + 1;
            if (nextPage > _pdfDocument?.Pages.Count - 1)
            {
                return null;
            }

            _pageNumber = nextPage;
            return await GetPdfPageSourceAsync(nextPage);
		}

		/// <inheritdoc/>
		public async Task<SoftwareBitmapSource> GetPdfPageSourceAsync(int pageNumber)
		{
			if (_pdfDocument == null) return null;
			if (pageNumber < 0 || pageNumber > _pdfDocument.Pages.Count) return null;
			_pageNumber = pageNumber;
			using var pdfPage = _pdfDocument.Pages[pageNumber];
            var dpiX = DotsPerInch();
			var dpiY = DotsPerInch();
			var pageWidth = (int)(dpiX * pdfPage.Size.Width / 72);
			var pageHeight = (int)(dpiY * pdfPage.Size.Height / 72);

			using var bitmap = new PdfiumBitmap(pageWidth, pageHeight, true);
			pdfPage.Render(bitmap, PageOrientations.Normal, RenderingFlags.None);
			using var stream = bitmap.AsBmpStream(dpiX, dpiY);
            var source = await GetWinUI3BitmapSourceFromGdiBitmap(new Bitmap(stream));
			return source;
		}

        // From https://stackoverflow.com/questions/76640972/convert-system-drawing-icon-to-microsoft-ui-xaml-imagesource
        private static async Task<SoftwareBitmapSource> GetWinUI3BitmapSourceFromGdiBitmap(Bitmap bmp)
        {
            if (bmp == null)
            {
                return null;
            }

            // get pixels as an array of bytes
            var data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                bmp.PixelFormat);
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

        /// <summary>
        /// Converts PDF resolution to dots per inch.
        /// </summary>
        /// <returns>resolution in dots per inch.</returns>
        private int DotsPerInch()
        {
            return Resolution switch
            {
                PdfResolution.High => 300,
                PdfResolution.Low => 150,
                _ => 300,
            };
        }
		/// <inheritdoc/>
		public async Task<SoftwareBitmapSource> GetPreviousPage()
		{
            if (_pdfDocument == null)
            {
                return null;
            }

            int previousPage = _pageNumber - 1;
            if (previousPage < 0)
            {
                return null;
            }

            _pageNumber = previousPage;
            return await GetPdfPageSourceAsync(previousPage);
		}

		/// <inheritdoc/>
        public bool IsPdfFile(StorageFile file) =>
            file.FileType.Equals(".PDF", StringComparison.CurrentCultureIgnoreCase);

		/// <inheritdoc/>
		public void LoadPdfFile(StorageFile file)
		{
			if (file == null) return;
			_pdfDocument = new PdfDocument(file.Path);
			_pageNumber = 0;
		}
	}
}
