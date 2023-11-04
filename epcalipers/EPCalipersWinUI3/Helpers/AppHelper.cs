using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            AppMainWindow.Navigate(type);
        }

        public static string CachedTitleBarText { get; set; }
    }
}
