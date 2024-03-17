using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage;

namespace EPCalipersWinUI3PDFHandler
{
	public interface IPdfHelper
    {
        int CurrentPageNumber { get; }
        string FilePath { get; set; }
        bool IsMultiPage { get; }
        int MaximumPageNumber { get; }
        int NumberOfPdfPages { get; }
        bool PdfIsLoaded { get; }

        void ClearPdfFile();
        Task<SoftwareBitmapSource> GetNextPage();
        Task<SoftwareBitmapSource> GetPdfPageSourceAsync(int pageNumber);
        Task<SoftwareBitmapSource> GetPreviousPage();
        void LoadPdfFile(StorageFile file);
    }
}
