using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using EPCalipersPdf;
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
using System.Diagnostics;

namespace EPCalipersPdfCore
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
        public string FilePath { get; set; } = null; // not used in this implementation, but required by interface

		/// <inheritdoc/>
		public bool IsMultiPage => _pdfDocument?.Pages.Count > 1;

        /// <inheritdoc/>
        public int MaximumPageNumber => NumberOfPdfPages;

		/// <inheritdoc/>
		public int NumberOfPdfPages => _pdfDocument?.Pages.Count ?? 0;

		/// <inheritdoc/>
		public bool PdfIsLoaded => _pdfDocument != null;

		/// <inheritdoc/>
		public PdfResolution Resolution { get; set; } = EPCalipersPdf.PdfResolution.High;

		/// <inheritdoc/>
		public void ClearPdfFile()
		{
			try
			{
				_pdfDocument?.Close();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"ClearPdfFile: unexpected error closing PDF: {ex}");
			}
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
			// guard: valid zero-based index
			if (pageNumber < 0 || pageNumber >= _pdfDocument.Pages.Count) return null;

			_pageNumber = pageNumber;

			try
			{
				using var pdfPage = _pdfDocument.Pages[pageNumber];
				var dpiX = DotsPerInch();
				var dpiY = DotsPerInch();

				// ensure non-zero, positive dimensions
				var pageWidth = Math.Max(1, (int)(dpiX * pdfPage.Size.Width / 72));
				var pageHeight = Math.Max(1, (int)(dpiY * pdfPage.Size.Height / 72));

				using var bitmap = new PdfiumBitmap(pageWidth, pageHeight, true);
				pdfPage.Render(bitmap, PageOrientations.Normal, RenderingFlags.None);

				using var stream = bitmap.AsBmpStream(dpiX, dpiY);

				// create a managed Bitmap from the stream and convert
				using var gdiBitmap = new Bitmap(stream);
				var source = await GetWinUI3BitmapSourceFromGdiBitmap(gdiBitmap);
				return source;
			}
			catch (Exception ex)
			{
				// log and return null so callers can handle failure without app crash
				Debug.WriteLine($"GetPdfPageSourceAsync failed (page {pageNumber}): {ex}");
				return null;
			}
		}

        // From https://stackoverflow.com/questions/76640972/convert-system-drawing-icon-to-microsoft-ui-xaml-imagesource
        private static async Task<SoftwareBitmapSource> GetWinUI3BitmapSourceFromGdiBitmap(Bitmap bmp)
        {
            if (bmp == null)
            {
                return null;
            }

			BitmapData data = null;
            try
            {
                // get pixels as an array of bytes
                data = bmp.LockBits(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly,
                    bmp.PixelFormat);

                var bytes = new byte[Math.Abs(data.Stride) * data.Height];
                Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

                // get WinRT SoftwareBitmap
                var softwareBitmap = new SoftwareBitmap(
                    BitmapPixelFormat.Bgra8,
                    bmp.Width,
                    bmp.Height,
                    BitmapAlphaMode.Premultiplied);

                // If pixel formats don't match you may need to convert bytes appropriately.
                softwareBitmap.CopyFromBuffer(bytes.AsBuffer());

                // build WinUI3 SoftwareBitmapSource
                var source = new SoftwareBitmapSource();
                await source.SetBitmapAsync(softwareBitmap);
                return source;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetWinUI3BitmapSourceFromGdiBitmap failed: {ex}");
                return null;
            }
            finally
            {
                // ensure unlock even on exception
                if (data != null)
                {
                    try { bmp.UnlockBits(data); }
                    catch (Exception ex) { Debug.WriteLine($"UnlockBits failed: {ex}"); }
                }
            }
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
            file != null && file.FileType.Equals(".PDF", StringComparison.CurrentCultureIgnoreCase);

		/// <inheritdoc/>
		public void LoadPdfFile(StorageFile file)
		{
			if (file == null) return;

			// Let callers handle critical load errors (they usually wrap this call),
			// but guard to log and keep helper in consistent state if construction fails.
			try
			{
				_pdfDocument = new PdfDocument(file.Path);
				_pageNumber = 0;
				FilePath = file.Path;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"LoadPdfFile failed for '{file?.Path}': {ex}");
				ClearPdfFile();
				throw;
			}
		}
	}
}
