using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using Windows.Storage;

namespace EPCalipersWinUI3.Helpers
{
	public static class AppHelper
	{
		public static MainWindow AppMainWindow => (Application.Current as App)?.MainWindow;

		public static BitmapImage StartUpImage { get; set; }

		public static StorageFile StartupFile {  get; set; }

		public static string AppTitleBarText
		{
			get => AppMainWindow.AppTitleBarText;
			set => AppMainWindow.AppTitleBarText = value;
		}

		public static string AppDisplayName => MainWindow.GetAppTitleFromSystem();

		public static void Navigate(Type type)
		{
			AppMainWindow?.Navigate(type);
		}

		public static void NavigateBack()
		{
			AppMainWindow?.NavigateBack();
		}

		private static string CachedTitleBarText { get; set; }
		public static string MainPageTitleBarText { get; set; }
		public static string TransparentPageTitleBarText { get; set; }
		public static string SettingsPageTitleBarText { get; set; }
		public static string HelpPageTitleBarText { get; set; }

		public static void SaveTitleBarText()
		{
			CachedTitleBarText = AppTitleBarText;
		}

		public static void RestoreTitleBarText()
		{
			AppTitleBarText = CachedTitleBarText ?? string.Empty;
		}
	}
}
