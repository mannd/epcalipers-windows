using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFepcalipers
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		// Menu methods
		// Help
		private void About_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("About");
			var aboutBox = new AboutBox();
			if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
			{
				aboutBox.AdditionalOptions = true;
			}
			aboutBox.ShowDialog();
		}

		private void Help_Click(object sender, RoutedEventArgs e)
		{
			// https://warrenpclark.blogspot.com/2011/09/how-to-build-help-into-wpf-program.html
			// Seems like we need to use Windows Forms for this.
			Debug.WriteLine("Help");
			Debug.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory + "epcalipers-help.chm");
			System.Windows.Forms.Help.ShowHelp(null, System.AppDomain.CurrentDomain.BaseDirectory + "epcalipers-help.chm");

		}
	}
}
