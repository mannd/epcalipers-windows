using EPCalipersWinUI3.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : WinUIEx.WindowEx
	{
		public MainWindow()
		{
			this.InitializeComponent();
			ExtendsContentIntoTitleBar = true;
			SetTitleBar(TitleBar);
			//AppTitleTextBlock.Text = "AppDisplayName".GetLocalized();
			AppTitleTextBlock.Text = GetAppTitleFromSystem();
			PersistenceId = "EPCalipersMainWindowID";
			MainFrame.Navigate(typeof(Views.MainPage));
			MainFrame.NavigationFailed += OnNavigationFailed;
			Activated += MainWindow_Activated;
			Closed += MainWindow_Closed;
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

		public string GetAppTitleFromSystem()
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
	}
}
