using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.ViewModels;
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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EPCalipersWinUI3.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class TransparentPage : Page
	{
		TransparentPageViewModel ViewModel { get; set; }
		public TransparentPage()
		{
			this.InitializeComponent();
			ViewModel = new TransparentPageViewModel(TransparentCaliperView);
			SizeChanged += TransparentPage_SizeChanged;
		}

		private void TransparentPage_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			Debug.WriteLine("size changed");
			ViewModel.ChangeBounds();
		}

		private void CaliperGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("pressed");
		}

		private void CaliperGrider_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
		}

		private void CaliperGrider_Tapped(object sender, TappedRoutedEventArgs e)
		{
			Debug.WriteLine("tapped");
		}

		private void CaliperGrider_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
		{
			Debug.WriteLine("double tapped");
		}

		private void CaliperGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			Debug.WriteLine("released");
		}

		private async void About_Click(object sender, RoutedEventArgs e) => await CommandHelper.About(XamlRoot);
	}
}
