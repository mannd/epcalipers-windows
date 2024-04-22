//-----------------------------------------------------------------------
// <copyright file="IPdfHelper.cs" company="EP Studios">
// Copyright (c) EP Studios. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;

namespace EPCalipersWinUI3Helpers
{
    /// <summary>
    /// Interface for classes providing PDF functions.
    /// </summary>
    public interface IPdfHelper
    {
        /// <summary>
        /// Gets current PDF page number.
        /// Page numbering is 1 based.
        /// </summary>
        int CurrentPageNumber { get; }

        /// <summary>
        /// Gets or sets path to PDF file.
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// Gets a value indicating whether PDF has multiple pages.
        /// </summary>
        bool IsMultiPage { get; }

        /// <summary>
        /// Gets the highest page number of a PDF document.
        /// This is 1 based.
        /// </summary>
        int MaximumPageNumber { get; }

        /// <summary>
        /// Gets the number of pages of a PDF.
        /// </summary>
        int NumberOfPdfPages { get; }

        /// <summary>
        /// Gets a value indicating whether a PDF file is loaded.
        /// </summary>
        bool PdfIsLoaded { get; }

        /// <summary>
        /// Clears and nullifies the PDF document.
        /// </summary>
        void ClearPdfFile();

        /// <summary>
        /// Gets bitmap of next PDF page.
        /// </summary>
        /// <returns>Bitmap of next PDF page or null if no next page.</returns>
        Task<SoftwareBitmapSource> GetNextPage();

        /// <summary>
        /// Get a PDF page asynchronously.
        /// </summary>
        /// <param name="pageNumber">Page number, 1 based.</param>
        /// <returns>Page Bitmap.</returns>
        Task<SoftwareBitmapSource> GetPdfPageSourceAsync(int pageNumber);

        /// <summary>
        /// Gets bitmap of previous PDF page.
        /// </summary>
        /// <returns>Bitmap of previous PDF page or null if no previous page.</returns>
        Task<SoftwareBitmapSource> GetPreviousPage();

        /// <summary>
        /// Checks whether a file contains a PDF document.
        /// </summary>
        /// <param name="file">File to be checked.</param>
        /// <returns>True is file is a PDF document file.</returns>
        bool IsPdfFile(StorageFile file);

        /// <summary>
        /// Load a PDF document from a file.
        /// </summary>
        /// <param name="file">File containing the PDF document.</param>
        void LoadPdfFile(StorageFile file);
    }
}
