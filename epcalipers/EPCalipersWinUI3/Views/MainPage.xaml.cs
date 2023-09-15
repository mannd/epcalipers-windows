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
	public sealed partial class MainPage : Page
	{
        double lineThickness = 5;
        Microsoft.UI.Xaml.Shapes.Line line = new();
        public MainPageViewModel ViewModel { get; set; }
		public MainPage()
		{
			this.InitializeComponent();
            ViewModel = new MainPageViewModel();

            DrawLine(500, 0, 500, 500);

            scrollViewer.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, (s, e) =>
            {
                lineThickness = Math.Max(1.0, lineThickness / scrollViewer.ZoomFactor);
                lineThickness = Math.Min(lineThickness, 5.0);

                canvas.Children.Remove(line);
                DrawLine(500, 0, 500, 500);
                Debug.Print(scrollViewer.ZoomFactor.ToString());
            });
        }

        private void DrawLine(int x1, int y1, int x2, int y2)
        {
            //	canvas.Children.Clear();
            var brush = new SolidColorBrush(Microsoft.UI.Colors.Blue);
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            lineThickness = Math.Max(1.0, lineThickness / scrollViewer.ZoomFactor);
            lineThickness = Math.Min(lineThickness, 5.0);
            line.Stroke = brush;
            canvas.Children.Add(line);
        }

        private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Need to set canvas size to match image size when image is loaded
        }

        private void scrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            // Just thin out lines as view zooms
        }

        private void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
        }

        private void PrintMessage()
        {
            Debug.Print("print message");
        }


		private void ZoomIn_Click(object sender, RoutedEventArgs e)
		{
            Zoom(2);
		}

		private void ZoomOut_Click(object sender, RoutedEventArgs e)
		{
            Zoom(0.5f);
		}
          private void Zoom(float multiple)
        {
            if (scrollViewer != null)
            {
                scrollViewer.ChangeView(0, 0, multiple * scrollViewer.ZoomFactor);
            }
        }

		private async void About_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("About");
            var aboutDialog = new AboutDialog();
            aboutDialog.XamlRoot = this.XamlRoot;
            var result = await aboutDialog.ShowAsync();
		}
	}
}
