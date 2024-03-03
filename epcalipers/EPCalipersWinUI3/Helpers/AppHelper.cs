﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace EPCalipersWinUI3.Helpers
{
	public static class AppHelper
	{
		public static MainWindow AppMainWindow => (Application.Current as App)?.MainWindow;

		public static string AppTitleBarText
		{
			get => AppMainWindow.AppTitleBarText;
			set => AppMainWindow.AppTitleBarText = value;
		}

		public static void Navigate(Type type)
		{
			AppMainWindow?.Navigate(type);
		}

		public static void NavigateBack()
		{
			AppMainWindow?.NavigateBack();
		}

		private static string CachedTitleBarText { get; set; }

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
