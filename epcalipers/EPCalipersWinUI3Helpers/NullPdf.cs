//-----------------------------------------------------------------------
// <copyright file="IPdfHelper.cs" company="EP Studios">
// Copyright (c) EP Studios. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;

namespace PDFHandler
{
	/// <summary>
	/// Dummy PDF class for the ARM64 version which does not support PDFs.
	/// </summary>
	public class NullPdf : IPdfHelper
	{
		public bool SupportsPdfs => false;

        public int CurrentPageNumber => 1;

		public string FilePath { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public bool IsMultiPage => false;

        public int MaximumPageNumber => 1;

		public int NumberOfPdfPages => 1;

        public bool PdfIsLoaded => false;

		public void ClearPdfFile()
		{
            // Do nothing
		}

		public Task<SoftwareBitmapSource> GetNextPage()
		{
			throw new System.NotImplementedException();
		}

		public Task<SoftwareBitmapSource> GetPdfPageSourceAsync(int pageNumber)
		{
			throw new System.NotImplementedException();
		}

		public Task<SoftwareBitmapSource> GetPreviousPage()
		{
			throw new System.NotImplementedException();
		}

        public bool IsPdfFile(StorageFile file) =>
            file.FileType.Equals(".PDF", StringComparison.CurrentCultureIgnoreCase);

		public void LoadPdfFile(StorageFile file)
		{
            // Do nothing
		}
	}
}
