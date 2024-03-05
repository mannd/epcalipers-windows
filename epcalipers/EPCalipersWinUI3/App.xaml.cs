using EPCalipersWinUI3.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public partial class App : Application
	{
		private MainWindow _window;

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeComponent();
		}

		public MainWindow MainWindow => _window;

		/// <summary>
		/// Invoked when the application is launched.
		/// </summary>
		/// <param name="args">Details about the launch request and process.</param>
		protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
		{
			// see https://stackoverflow.com/questions/76650127/how-to-handle-activation-through-files-in-winui-3-packaged
			AppActivationArguments appActivationArguments = AppInstance.GetCurrent().GetActivatedEventArgs();

			if (appActivationArguments.Kind is ExtendedActivationKind.File &&
				appActivationArguments.Data is IFileActivatedEventArgs fileActivatedEventArgs &&
				fileActivatedEventArgs.Files.FirstOrDefault() is StorageFile storageFile)
			{
				// TODO: open file
				var bitmapImage = new BitmapImage();
				bitmapImage.SetSource(await storageFile.OpenAsync(FileAccessMode.Read));
				AppHelper.StartUpImage = bitmapImage;
			}
			_window = new MainWindow();
			_window.Activate();

#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				// Setting to true shows fast track rendered text in green.
				DebugSettings.IsTextPerformanceVisualizationEnabled = false;
			}
#endif
		}
	}
}
