using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleBar);
            MainFrame.Navigate(typeof(Views.MainPage));
			Activated += MainWindow_Activated;
		}

		private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
		{
			if (args.WindowActivationState == WindowActivationState.Deactivated)
			{
				AppTitleTextBlock.Foreground =
					(SolidColorBrush)App.Current.Resources["WindowCaptionForegroundDisabled"];
			}
			else
			{
				AppTitleTextBlock.Foreground =
					(SolidColorBrush)App.Current.Resources["WindowCaptionForeground"];
			}
		}
	}
}
