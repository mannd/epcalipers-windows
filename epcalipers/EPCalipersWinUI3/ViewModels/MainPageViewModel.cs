using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppUIBasics.Helper;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel;
using Windows.Storage.Streams;
using System.Drawing;
using EPCalipersWinUI3.Views;

namespace EPCalipersWinUI3
{
    public partial class MainPageViewModel : ObservableObject
    {
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

			// Open the picker for the user to pick a file
			var file = await openPicker.PickSingleFileAsync();
			if (file != null)
			{
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

		[RelayCommand]
		private static void Exit() => Application.Current.Exit();


		[ObservableProperty]
        private string testText = "Test";

        [ObservableProperty]
        private Image mainImage;

        [ObservableProperty]
        private ImageSource mainImageSource 
			= new BitmapImage { UriSource = new Uri("ms-appx:///Assets/Images/sampleECG.jpg") };
    }
}
