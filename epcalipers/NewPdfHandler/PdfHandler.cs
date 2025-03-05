using Microsoft.UI.Xaml.Media.Imaging;
using PDFHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NewPdfHandler
{
	public class PdfHandler : PDFHandler.IPdfHelper
	{
		bool IPdfHelper.SupportsPdfs => throw new NotImplementedException();

		int IPdfHelper.CurrentPageNumber => throw new NotImplementedException();

		string IPdfHelper.FilePath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		bool IPdfHelper.IsMultiPage => throw new NotImplementedException();

		int IPdfHelper.MaximumPageNumber => throw new NotImplementedException();

		int IPdfHelper.NumberOfPdfPages => throw new NotImplementedException();

		bool IPdfHelper.PdfIsLoaded => throw new NotImplementedException();

		PdfResolution IPdfHelper.Resolution { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		void IPdfHelper.ClearPdfFile()
		{
			throw new NotImplementedException();
		}

		Task<SoftwareBitmapSource> IPdfHelper.GetNextPage()
		{
			throw new NotImplementedException();
		}

		Task<SoftwareBitmapSource> IPdfHelper.GetPdfPageSourceAsync(int pageNumber)
		{
			throw new NotImplementedException();
		}

		Task<SoftwareBitmapSource> IPdfHelper.GetPreviousPage()
		{
			throw new NotImplementedException();
		}

		bool IPdfHelper.IsPdfFile(StorageFile file)
		{
			throw new NotImplementedException();
		}

		void IPdfHelper.LoadPdfFile(StorageFile file)
		{
			throw new NotImplementedException();
		}
	}
}
