using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : WinUIEx.WindowEx
	{
		readonly UISettings _uiSettings;

		public MainWindow()
		{
			this.InitializeComponent();
			Debug.Print("MainWindow constructor");
			ExtendsContentIntoTitleBar = true;
			SetTitleBar(TitleBar);
			//AppTitleTextBlock.Text = "AppDisplayName".GetLocalized();
			AppTitleTextBlock.Text = GetAppTitleFromSystem();
			PersistenceId = "EPCalipersMainWindowID";
			var settings = Settings.Instance;
			Debug.Assert(settings != null);
			// Ignore starting with transparent window if using open with to open an image at startup
			if (AppHelper.StartupFile == null)
			{
				switch (settings.StartupPage)
				{
					case StartupPage.Transparent:
						SystemBackdrop = new WinUIEx.TransparentTintBackdrop();
						MainFrame.Navigate(typeof(Views.TransparentPage));
						break;
					case StartupPage.Main:
						MainFrame.Navigate(typeof(Views.MainPage));
						break;
				}
			}
			else
			{
				MainFrame.Navigate(typeof(Views.MainPage));
			}
			IsAlwaysOnTop = settings.IsAlwaysOnTop;
			MainFrame.NavigationFailed += OnNavigationFailed;
			Activated += MainWindow_Activated;
			Closed += MainWindow_Closed;
			_uiSettings = new();
			_uiSettings.ColorValuesChanged += UISettings_ColorValuesChanged;
		}

		private void MainWindow_Closed(object sender, WindowEventArgs args)
		{
			CommandHelper.ApplicationExit();
		}

		public TextBlock GetTitleBar()
		{
			return AppTitleTextBlock;
		}

		public string AppTitleBarText
		{
			get => AppTitleTextBlock.Text;
			set => AppTitleTextBlock.Text = value;
		}

		public static string GetAppTitleFromSystem()
		{
			return Windows.ApplicationModel.Package.Current.DisplayName;
		}

		public void Navigate(System.Type type)
		{
			if (type == null) { return; }
			MainFrame.Navigate(type);
		}

		public void NavigateBack()
		{
			if (!MainFrame.CanGoBack) { return; }
			MainFrame.GoBack();
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

		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		private void UISettings_ColorValuesChanged(UISettings sender, object args)
		{
			DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.High,
				() =>
				{
					// Workaround for failure of WinUI 3 to change titlebar button colors
					// with theme change.
					if (Application.Current.RequestedTheme == ApplicationTheme.Light)
					{
						Debug.Print("Light theme");
						AppWindow.TitleBar.ButtonForegroundColor = Colors.Black;
					}
					else
					{
						Debug.Print("Dark theme");
						AppWindow.TitleBar.ButtonForegroundColor = Colors.White;
					}
				});
		}
	}
}
