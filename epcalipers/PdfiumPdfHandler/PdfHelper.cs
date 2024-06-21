//-----------------------------------------------------------------------
// <copyright file="PdfHelper.cs" company="EP Studios">
// Copyright (c) EP Studios. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using PdfiumViewer;

namespace PdfiumPdfHandler
{
    /// <summary>
    /// Encapsulate and isolate nasty PDF code.
    /// This uses the Pdfium NuGet package.
    /// </summary>
    public class PdfHelper : PdfHandler.IPdfHelper
    {
        private PdfDocument _pdfDocument = null;
        private int _pageNumber = 0;

        /// <inheritdoc/>
        public bool SupportsPdfs => true;

        /// <inheritdoc/>
        public string FilePath { get; set; }

        /// <inheritdoc/>
        public bool PdfIsLoaded => _pdfDocument != null;

        /// <inheritdoc/>
        public int NumberOfPdfPages => _pdfDocument?.PageCount ?? 0;

        /// <inheritdoc/>
        public int CurrentPageNumber => _pageNumber + 1;

        /// <inheritdoc/>
        public int MaximumPageNumber => NumberOfPdfPages;

        /// <inheritdoc/>
        public bool IsMultiPage
        {
            get
            {
                return _pdfDocument?.PageCount > 0;
            }
        }

		public int DotsPerInch { get; set; } = 300;  // default to high res

		/// <inheritdoc/>
		public bool IsPdfFile(string fileName)
        {
            try
            {
                return Path.GetExtension(fileName).ToUpperInvariant() == ".PDF";
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void LoadPdfFile(string fileName)
        {
            if (fileName == null)
            {
                return;
            }

            try
            {
                _pdfDocument = PdfDocument.Load(Path.GetFullPath(fileName));
                _pageNumber = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <inheritdoc/>
        public Image GetPdfPageSource(int pageNumber)
        {
            if (_pdfDocument == null)
            {
                return null;
            }

            if (pageNumber < 0 || pageNumber > _pdfDocument.PageCount - 1)
            {
                return null;
            }

            _pageNumber = pageNumber;
            var img = _pdfDocument.Render(pageNumber, DotsPerInch, DotsPerInch, PdfRenderFlags.CorrectFromDpi);
            return img;
        }

        /// <inheritdoc/>
        public void ClearPdfFile()
        {
            _pdfDocument?.Dispose();
            _pdfDocument = null;
            _pageNumber = 0;
        }

        /// <inheritdoc/>
        public Image GetNextPage()
        {
            if (_pdfDocument == null)
            {
                return null;
            }

            int nextPage = _pageNumber + 1;
            if (nextPage > _pdfDocument.PageCount - 1)
            {
                return null;
            }

            _pageNumber = nextPage;
            return GetPdfPageSource(nextPage);
        }

        /// <inheritdoc/>
        public Image GetPreviousPage()
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
            return GetPdfPageSource(previousPage);
        }
    }
}
