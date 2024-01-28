using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ColorView : Page
	{
		public ColorViewModel ViewModel { get; set; }
		public WindowEx Window { get; set; }
		public Caliper caliper { get; set; }

		public ColorView(Caliper caliper)
		{
			this.InitializeComponent();
			ViewModel = new ColorViewModel(caliper);
		}

		private void ColorViewCancel_Click(object sender, RoutedEventArgs e)
		{
			CloseWindow();
		}

		private void ColorViewOk_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.CaliperColor = CaliperColorPicker.Color;
			CloseWindow();
		}

		private void CloseWindow() => Window.Close();

		private void Page_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			switch (e.Key)
			{
				case VirtualKey.Enter:
					ViewModel.CaliperColor = CaliperColorPicker.Color;
					CloseWindow();
					break;
				case VirtualKey.Escape: CloseWindow(); break;
				default: break;
			}
		}
	}
}
